using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Data.Entities
{
    public class Order
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public int TotalPrice { get; set; }
        public DateTime Date { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }

    }
}
