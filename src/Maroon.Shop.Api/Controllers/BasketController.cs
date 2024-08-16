using Maroon.Shop.Data;
using Microsoft.AspNetCore.Mvc;

namespace Maroon.Shop.Api.Controllers
{
    /// <summary>
    /// Basket Controller Class, inherits from <see cref="Controller"/>.
    /// Handles requests routed to "api/[controller]", where [controller] is replaced by the name of the controller, in this case, "Basket".
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : Controller
    {
        // Private backing fields.
        private readonly ShopContext _context;

        /// <summary>
        /// Constructor. Initialises the Basket Controller.
        /// </summary>
        /// <param name="context">A <see cref="ShopContext"/> representing the Data Context.</param>
        public BasketController(ShopContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Handles GET requests to "api/Basket/{basketId}".
        /// Attempts to retrieve a Basket for the given Basket Id.
        /// </summary>
        /// <param name="basketId">A <see cref="long"/> representing the Id of the Basket.</param>
        /// <returns>An <see cref="ActionResult{Basket}"/> representing the Basket found.</returns>
        [HttpGet]
        [Route("{basketId}")]
        public ActionResult<Basket> GetById(long basketId)
        {
            // Query for a Basket with the given basketId.
            var query = _context.Baskets.Where(basket => basket.BasketId == basketId);

            if (!query.Any())
            {
                // The Basket could not be found, return a 404 Not Found response.
                return NotFound();
            }
            else
            {
                // Return the first matching Basket.
                return query.First();
            }
        }

        /// <summary>
        /// Handles GET requests to "api/Basket/All".
        /// Attempts to retrieve all Baskets.
        /// </summary>
        /// <returns>An <see cref="ActionResult{IEnumerable{Basket}}"/> representing the Baskets found.</returns>
        [HttpGet("All")]
        public ActionResult<IEnumerable<Basket>> GetBaskets()
        {
            // Return all Baskets.
            return _context.Baskets;
        }

        /// <summary>
        /// Handles GET requests to "api/Basket/ByCustomerId".
        /// Attempts to retrieve all Baskets for the given customerId.
        /// </summary>
        /// <param name="customerId">A <see cref="long"/> representing the Id of the Customer.</param>
        /// <returns>An <see cref="ActionResult{IEnumerable{Basket}}"/> representing the Baskets found.</returns>
        [HttpGet("ByCustomerId")]
        public ActionResult<IEnumerable<Basket>> GetBasketsByCustomer(long customerId)
        {
            // Query for all Baskets with the given customerId.
            var query = _context.Baskets.Where(basket => basket.Customer.CustomerId == (customerId));

            return query.ToList();
        }
    }
}