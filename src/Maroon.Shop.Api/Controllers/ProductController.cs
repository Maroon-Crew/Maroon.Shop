using Maroon.Shop.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Maroon.Shop.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ShopContext _context;

        public ProductController(ShopContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("{addressId}")]
        public ActionResult<Product> GetById(long productId)
        {
            var query = _context.Products.Where(a => a.ProductId == productId);

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
        public ActionResult<IEnumerable<Product>> GetProducts()
        {
            return _context.Products;
        }

    }
}
