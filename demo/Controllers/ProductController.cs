using Demo.Data;
using Demo.Data.Entities;
using Demo.Data.ViewModel;
using Demo.Service.Core;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public IActionResult GetAllProduct()
        {
            var products = _productService.Get();
            return Ok(products);

        }

        [HttpPost]
        [Route("pagination")]
        
        public IActionResult GetAllProduct(PagingModel pagingModel)
        {
            var products = _productService.Get(pagingModel);
            return Ok(products);

        }

        [HttpPut("id")]
        public IActionResult UpdateProduct(Guid id, ProductRequest request)
        {
            var result = _productService.UpdateProductById(id, request);
            if (!result)
                return BadRequest($"Khong ton tai product voi id: {id}");
            return Ok(_productService.UpdateProductById(id, request));
        }
    }
}
