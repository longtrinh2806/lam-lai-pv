using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Data.Entities
{
    public class OrderDetail
    {
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public int Price { get; set; }
        public Product Product { get; set; }
        public Order Order { get; set; }

        public override string ToString()
        {
            return $"OrderDetail: {OrderId}, ProductId: {ProductId}, Quantity: {Quantity}, Price: {Price}, Product: {Product?.Name}, Order: {Order?.Id}";
        }
    }
}
