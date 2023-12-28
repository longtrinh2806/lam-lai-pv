using Demo.Data.Entities;
using Demo.Data.ViewModel;
using Demo.Service.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        // POST: OrderController/Create
        [HttpPost]
        public IActionResult CreateOrder(List<OrderRequest> request)
        {
            var result = _orderService.Create(request);

            if (!result.IsSuccess)
                return BadRequest(result);
            return Ok(result);
        }
        [HttpGet]
        public IActionResult GetOrder(DateTime from, DateTime to)
        {
            var result = _orderService.GetOrderByDateTime(from, to);

            if (!result.IsSuccess)
                return BadRequest(result);
            return Ok(result);

        }
    }
}
