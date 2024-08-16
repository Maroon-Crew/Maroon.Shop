using Maroon.Shop.Data;
using Microsoft.AspNetCore.Mvc;

namespace Maroon.Shop.Api.Controllers
{
    /// <summary>
    /// Product Controller Class, inherits from <see cref="Controller"/>.
    /// Handles requests routed to "api/[controller]", where [controller] is replaced by the name of the controller, in this case, "Product".
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : Controller
    {
        // Private backing fields.
        private readonly ShopContext _context;

        /// <summary>
        /// Constructor. Initialises the Product Controller.
        /// </summary>
        /// <param name="context">A <see cref="ShopContext"/> representing the Data Context.</param>
        public ProductController(ShopContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Handles GET requests to "api/Product/{productId}".
        /// Attempts to retrieve an Product for the given Product Id.
        /// </summary>
        /// <param name="productId">A <see cref="long"/> representing the Id of the Product.</param>
        /// <returns>An <see cref="ActionResult{Product}"/> representing the Product found.</returns>
        [HttpGet]
        [Route("{productId}")]
        public ActionResult<Product> GetById(long productId)
        {
            // Query for a Product with the given productId.
            var query = _context.Products.Where(product => product.ProductId == productId);

            if (!query.Any())
            {
                // The Product could not be found, return a 404 Not Found response.
                return NotFound();
            }
            else
            {
                // Return the first matching Product.
                return query.First();
            }
        }

        /// <summary>
        /// Handles GET requests to "api/Product/All".
        /// Attempts to retrieve all Products.
        /// </summary>
        /// <returns>An <see cref="ActionResult{IEnumerable{Product}}"/> representing the Products found.</returns>
        [HttpGet("All")]
        public ActionResult<IEnumerable<Product>> GetProducts()
        {
            // Return all Products.
            return _context.Products;
        }
    }
}