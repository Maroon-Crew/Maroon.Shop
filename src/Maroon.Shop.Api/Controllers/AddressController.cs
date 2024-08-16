using Maroon.Shop.Data;
using Microsoft.AspNetCore.Mvc;

namespace Maroon.Shop.Api.Controllers
{
    /// <summary>
    /// Address Controller Class, inherits from <see cref="Controller"/>.
    /// Handles requests routed to "api/[controller]", where [controller] is replaced by the name of the controller, in this case, "Address".
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        // Private backing fields.
        private readonly ShopContext _context;

        /// <summary>
        /// Constructor. Initialises the Address Controller.
        /// </summary>
        /// <param name="context">A <see cref="ShopContext"/> representing the Data Context.</param>
        public AddressController(ShopContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Handles GET requests to "api/Address/{addressId}".
        /// Attempts to retrieve an Address for the given Address Id.
        /// </summary>
        /// <param name="addressId">A <see cref="long"/> representing the Id of the Address.</param>
        /// <returns>An <see cref="ActionResult{Address}"/> representing the Address found.</returns>
        [HttpGet]
        [Route("{addressId}")]
        public ActionResult<Address> GetById(long addressId)
        {
            // Query for an Address with the given addressId.
            var query = _context.Addresses.Where(address => address.AddressId == addressId);

            if (!query.Any())
            {
                // The Address could not be found, return a 404 Not Found response.
                return NotFound();
            }
            else
            {
                // Return the first matching Address.
                return query.First();
            }
        }

        /// <summary>
        /// Handles GET requests to "api/Address/All".
        /// Attempts to retrieve all Addresses.
        /// </summary>
        /// <returns>An <see cref="ActionResult{IEnumerable{Address}}"/> representing the Addresses found.</returns>
        [HttpGet("All")]
        public ActionResult<IEnumerable<Address>> GetAddresses()
        {
            // Return all Addresses.
            return _context.Addresses;
        }

        /// <summary>
        /// Handles GET requests to "api/Address/ByPostCode".
        /// Attempts to retrieve all Addresses that start with the given postCode.
        /// </summary>
        /// <param name="postCode">A <see cref="string"/> representing the PostCode.</param>
        /// <returns>An <see cref="ActionResult{IEnumerable{Address}}"/> representing the Addresses found.</returns>
        [HttpGet("ByPostCode")]
        public ActionResult<IEnumerable<Address>> GetAddressesByPostCode(string postCode)
        {
            var query = _context.Addresses.Where(address => address.PostCode.StartsWith(postCode));

            return query.ToList();
        }
    }
}