using Maroon.Shop.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Maroon.Shop.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly ShopContext _context;

        public BasketController(ShopContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("{basketId}")]
        public ActionResult<Basket> GetById(long basketId)
        {
            var query = _context.Baskets.Where(a => a.BasketId == basketId);

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
        public ActionResult<IEnumerable<Basket>> GetBaskets()
        {
            return _context.Baskets;
        }
    }
}
