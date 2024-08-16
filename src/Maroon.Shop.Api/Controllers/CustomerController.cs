using Maroon.Shop.Data;
using Microsoft.AspNetCore.Mvc;

namespace Maroon.Shop.Api.Controllers
{
    /// <summary>
    /// Customer Controller Class, inherits from <see cref="Controller"/>.
    /// Handles requests routed to "api/[controller]", where [controller] is replaced by the name of the controller, in this case, "Customer".
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : Controller
    {
        // Private backing fields.
        private readonly ShopContext _context;

        /// <summary>
        /// Constructor. Initialises the Customer Controller.
        /// </summary>
        /// <param name="context">A <see cref="ShopContext"/> representing the Data Context.</param>
        public CustomerController(ShopContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Handles GET requests to "api/Customer/{customerId}".
        /// Attempts to retrieve a Customer for the given Customer Id.
        /// </summary>
        /// <param name="customerId">A <see cref="long"/> representing the Id of the Customer.</param>
        /// <returns>An <see cref="ActionResult{Customer}"/> representing the Customer found.</returns>
        [HttpGet]
        [Route("{customerId}")]
        public ActionResult<Customer> GetById(long customerId)
        {
            // Query for a Customer with the given customerId.
            var query = _context.Customers.Where(customer => customer.CustomerId == customerId);

            if (!query.Any())
            {
                // The Customer could not be found, return a 404 Not Found response.
                return NotFound();
            }
            else
            {
                // Return the first matching Customer.
                return query.First();
            }
        }

        /// <summary>
        /// Handles GET requests to "api/Customer/All".
        /// Attempts to retrieve all Customers.
        /// </summary>
        /// <returns>An <see cref="ActionResult{IEnumerable{Customer}}"/> representing the Customers found.</returns>
        [HttpGet("All")]
        public ActionResult<IEnumerable<Customer>> GetCustomers()
        {
            // Return all Customers.
            return _context.Customers;
        }
    }
}