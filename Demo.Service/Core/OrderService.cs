using Demo.Data;
using Demo.Data.Entities;
using Demo.Data.ViewModel;
using Mapster;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Demo.Service.Core
{
    public interface IOrderService
    {
        bool Create(List<OrderRequest> request);
        List<OrderResponse> GetOrderByDateTime(DateTime from, DateTime to);
    }
    public class OrderService : IOrderService
    {
        private readonly AppDbContext appDbContext;

        public OrderService(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }
        public bool Create(List<OrderRequest> request)
        {
            if (request == null)
                return false;
            if (request.Any(tmp => tmp.Quantity < 1 || tmp.Quantity >= 100))
                return false;

            // Check connection to database
            if (!appDbContext.Database.CanConnect())
                throw new Exception("Can't connect to the database");

            using var transaction = new TransactionScope(TransactionScopeOption.Required);
            try
            {
                var order = new Order
                {
                    Code = DateTime.Now.Ticks.ToString(),
                    TotalPrice = 0
                };

                // List tam
                List<OrderDetail> orderDetails = new();

                request.ForEach(o =>
                {
                    var p = appDbContext.Products.FirstOrDefault(tmp => tmp.Id.Equals(o.ProductId));
                    if (p == null)
                        throw new Exception("Product not existed");
                    if (p.Quantity < o.Quantity)
                        throw new Exception("Out of stock");

                    order.TotalPrice += p.Price * o.Quantity;

                    p.Quantity -= o.Quantity;

                    appDbContext.Update(p);

                    // Map request to Order Detail
                    OrderDetail detail = new();
                    o.Adapt(detail);

                    // Them vao List tam
                    orderDetails.Add(detail);
                });

                order.Date = DateTime.Now;

                appDbContext.Orders.Add(order);

                orderDetails.ForEach(od =>
                {
                    od.OrderId = order.Id;
                    appDbContext.OrderDetails.Add(od);
                });

                appDbContext.SaveChanges();

                transaction.Complete();
                return true;
            }
            catch (Exception e)
            {
                transaction.Dispose();

                Console.WriteLine(e.Message);
                return false;
            }
        }

        public List<OrderResponse> GetOrderByDateTime(DateTime from, DateTime to)
        {

            var orderDetails = appDbContext.Orders
                .Join(appDbContext.OrderDetails, o => o.Id, od => od.OrderId, (o, od) => od)
                .Where(od => od.Order.Date >= from && od.Order.Date <= to)
                .ToList();

            if (orderDetails == null)
                throw new Exception("Order Detail is null");

            // Map danh sach order details to Order Response List
            var orderResponses = orderDetails
                .Join(appDbContext.Products, od => od.ProductId, p => p.Id, (od, p) => new OrderResponse
                {
                    ProductName = p.Name, Quantity = od.Quantity
                }).ToList();

            return orderResponses;
        }

    }
}
