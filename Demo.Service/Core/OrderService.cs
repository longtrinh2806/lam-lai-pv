using Demo.Data;
using Demo.Data.Entities;
using Demo.Data.ViewModel;
using Mapster;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Demo.Service.Core
{
    public interface IOrderService
    {
        ResponseModel Create(List<OrderRequest> request);
        ResponseModel GetOrderByDateTime(DateTime from, DateTime to);
    }
    public class OrderService : IOrderService
    {
        private readonly AppDbContext appDbContext;

        public OrderService(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }
        public ResponseModel Create(List<OrderRequest> request)
        {
            ResponseModel result = new();

            if (request == null)
            {
                result.IsSuccess = false;
                result.Message = "Invalid request!";

                return result;
            }
                
            if (request.Any(tmp => tmp.Quantity < 1 || tmp.Quantity >= 100))
            {
                result.IsSuccess = false;
                result.Message = "Invalid quantity!";

                return result;
            }

            using (var transaction = appDbContext.Database.BeginTransaction())
            {
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

                    result.IsSuccess = true;
                    result.Message = "Create Order Successfully";

                    transaction.Commit();
                    return result;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();

                    result.IsSuccess = false;
                    result.Message = ex.Message;

                    return result;
                }
            }
        }

        public ResponseModel GetOrderByDateTime(DateTime from, DateTime to)
        {
            ResponseModel result = new();

            //var orderResponses = appDbContext.Orders
            //    .Join(appDbContext.OrderDetails, o => o.Id, od => od.OrderId, (o, od) => od)
            //    .Where(od => od.Order.Date >= from && od.Order.Date <= to)
            //    .GroupBy(od => od.ProductId)
            //    .Select(group => new OrderResponse
            //    {
            //        ProductName = group.First().Product.Name,
            //        Quantity = group.Sum(od => od.Quantity)
            //    })
            //    .ToList();

            //if (orderResponses == null)
            //{
            //    result.IsSuccess = false;
            //    result.Message = "Order Detail is null";

            //    return result;
            //}


            var orderResponses = appDbContext.Orders
                .Where(order => order.Date >= from && order.Date <= to)
                .Include(order => order.OrderDetails)
                    .ThenInclude(orderDetail => orderDetail.Product)
                .SelectMany(order => order.OrderDetails, (order, orderDetail) => new OrderResponse
                {
                    ProductName = orderDetail.Product.Name,
                    Quantity = orderDetail.Quantity
                })
                .GroupBy(ordeResponse => ordeResponse.ProductName)
                .Select(group => new OrderResponse
                {
                    ProductName = group.Key,
                    Quantity = group.Sum(od => od.Quantity)
                })
                .ToList();

            if (orderResponses == null)
            {
                result.IsSuccess = false;
                result.Message = "Order Detail is null";

                return result;
            }

            result.Result = orderResponses;
            result.IsSuccess = true;
            result.Message = "Get Successfully";

            return result;
        }

    }
}
