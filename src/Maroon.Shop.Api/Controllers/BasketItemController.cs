using Maroon.Shop.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Maroon.Shop.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketItemController : ControllerBase
    {
        private readonly ShopContext _context;
        public BasketItemController(ShopContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("{basketItemId}")]
        public ActionResult<BasketItem> GetById(long basketItemId)
        {
            var query = _context.BasketItems.Where(a => a.BasketItemId == basketItemId);

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
        public ActionResult<IEnumerable<BasketItem>> GetBasketItems()
        {
            return _context.BasketItems;
        }
    }
}
