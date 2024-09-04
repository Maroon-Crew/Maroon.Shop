using Maroon.Shop.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Maroon.Shop.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderItemController : ControllerBase
    {
        private readonly ShopContext _context;

        public OrderItemController(ShopContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("{addressId}")]
        public ActionResult<OrderItem> GetById(long addressId)
        {
            var query = _context.OrderItems.Where(a => a.OrderItemId == addressId);

            if (!query.Any())
            {
                return NotFound();
            }
            else
            {
                return query.First();
            }
        }

        [HttpGet("All")]
        public ActionResult<IEnumerable<OrderItem>> GetAddresses()
        {
            return _context.OrderItems;
        }

    }
}
