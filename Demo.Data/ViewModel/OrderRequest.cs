using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Data.ViewModel
{
    public class OrderRequest
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
