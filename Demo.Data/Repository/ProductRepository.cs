using Demo.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Data.Repository
{

    public interface IProductRepository
    {
        List<Product> FindAll();
        List<Product> FindAll(int startIndex, int endIndex);
    }
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _appDbContext;

        public ProductRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public List<Product> FindAll()
        {
            return _appDbContext.Products.ToList();
        }

        public List<Product> FindAll(int startIndex, int endIndex)
        {
            return _appDbContext.Products.Skip(startIndex).Take(endIndex - startIndex).ToList();
        }
    }
}
