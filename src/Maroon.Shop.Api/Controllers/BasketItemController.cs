using Maroon.Shop.Data;
using Microsoft.AspNetCore.Mvc;

namespace Maroon.Shop.Api.Controllers
{
    /// <summary>
    /// Basket Item Controller Class, inherits from <see cref="Controller"/>.
    /// Handles requests routed to "api/[controller]", where [controller] is replaced by the name of the controller, in this case, "BasketItem".
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class BasketItemController : Controller
    {
        // Private backing fields.
        private readonly ShopContext _context;

        /// <summary>
        /// Constructor. Initialises the Basket Item Controller.
        /// </summary>
        /// <param name="context">A <see cref="ShopContext"/> representing the Data Context.</param>
        public BasketItemController(ShopContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Handles GET requests to "api/BasketItem/{basketItemId}".
        /// Attempts to retrieve a Basket Item for the given Basket Item Id.
        /// </summary>
        /// <param name="basketItemId">A <see cref="long"/> representing the Id of the Basket Item.</param>
        /// <returns>An <see cref="ActionResult{BasketItem}"/> representing the Basket Item found.</returns>
        [HttpGet]
        [Route("{basketItemId}")]
        public ActionResult<BasketItem> GetById(long basketItemId)
        {
            // Query for a Basket Item with the given basketItemId.
            var query = _context.BasketItems.Where(basketItem => basketItem.BasketItemId == basketItemId);

            if (!query.Any())
            {
                // The Basket Item could not be found, return a 404 Not Found response.
                return NotFound();
            }
            else
            {
                // Return the first matching Basket Item.
                return query.First();
            }
        }

        /// <summary>
        /// Handles GET requests to "api/BasketItem/All".
        /// Attempts to retrieve all Basket Items.
        /// </summary>
        /// <returns>An <see cref="ActionResult{IEnumerable{BasketItem}}"/> representing the Basket Items found.</returns>
        [HttpGet("All")]
        public ActionResult<IEnumerable<BasketItem>> GetBasketItems()
        {
            // Return all Basket Items.
            return _context.BasketItems;
        }

        /// <summary>
        /// Handles GET requests to "api/BasketItem/ByBasketId".
        /// Attempts to retrieve all Basket Items for the given basketId.
        /// </summary>
        /// <param name="basketId">A <see cref="long"/> representing the Id of the Basket.</param>
        /// <returns>An <see cref="ActionResult{IEnumerable{BasketItem}}"/> representing the Basket Items found.</returns>
        [HttpGet("ByBasketId")]
        public ActionResult<IEnumerable<BasketItem>> GetBasketItemsByBasket(long basketId)
        {
            // Query for all Basket Items with the given basketId.
            var query = _context.BasketItems.Where(basketItem => basketItem.Basket.BasketId == (basketId));

            return query.ToList();
        }

        /// <summary>
        /// Handles GET requests to "api/BasketItem/ByProductId".
        /// Attempts to retrieve all Basket Items for the given productId.
        /// </summary>
        /// <param name="productId">A <see cref="long"/> representing the Id of the Product.</param>
        /// <returns>An <see cref="ActionResult{IEnumerable{BasketItem}}"/> representing the Basket Items found.</returns>
        [HttpGet("ByProductId")]
        public ActionResult<IEnumerable<BasketItem>> GetBasketItemsByProduct(long productId)
        {
            // Query for all Basket Items with the given productId.
            var query = _context.BasketItems.Where(basketItem => basketItem.Product.ProductId == (productId));

            return query.ToList();
        }
    }
}