
using Demo.Data;
using Demo.Data.Entities;
using Demo.Data.Repository;
using Demo.Data.ViewModel;
using Mapster;
using Microsoft.EntityFrameworkCore;
using System.Data.Entity.Core;

namespace Demo.Service.Core
{
    public interface IProductService
    {
        List<Product> Get();
        List<Product> Get(PagingModel pagingModel);
        bool UpdateProductById(Guid id, ProductRequest request);
    }

    public class ProductService : IProductService
    {

        private readonly IProductRepository _productRepository;
        private readonly AppDbContext _appDbContext;

        public ProductService(IProductRepository productRepository, AppDbContext appDbContext)
        {
            _productRepository = productRepository;
            _appDbContext = appDbContext;
        }
        public List<Product> Get()
        {
            List<Product> products = _productRepository.FindAll();

            if (products == null)
                throw new Exception("Khong ton tai ban ghi");
            return products;
        }

        public List<Product> Get(PagingModel pagingModel)
        {
            int startIndex = (pagingModel.PageIndex - 1) * pagingModel.PageSize;
            int endIndex = startIndex + pagingModel.PageSize;

            List<Product> products = _productRepository.FindAll(startIndex, endIndex);

            if (products == null)
                throw new Exception("Khong ton tai ban ghi");
            return products;
        }

        public bool UpdateProductById(Guid id, ProductRequest request)
        {
            try
            {
                var product = _appDbContext.Products.FirstOrDefault(p => p.Id == id);

                if (product == null)
                    return false;
                
                request.Adapt(product);

                // cap nhat row version de theo doi cac thay doi tiep theo
                product.RowVersion = Guid.NewGuid().ToByteArray();

                _appDbContext.Update(product);

                _appDbContext.SaveChanges();


                return true;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new DbUpdateConcurrencyException();
            }
            
        }
    }
}
