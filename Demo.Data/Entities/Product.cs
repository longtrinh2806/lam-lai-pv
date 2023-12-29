using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Demo.Data.Entities
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public int Quantity  { get; set; }

        // Thuộc tính này được sử dụng để đánh dấu một cột trong mô hình dữ liệu Entity Framework
        // Và cho biết rằng cột đó sẽ được sử dụng để theo dõi các thay đổi trong hàng dữ liệu.
        [Timestamp]
        public byte[] RowVersion { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }

        public Product(Guid id, string code, string name, int price, int quantity)
        {
            Id = id;
            Code = code;
            Name = name;
            Price = price;
            Quantity = quantity;
        }
    }
}