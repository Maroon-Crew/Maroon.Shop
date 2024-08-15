using Maroon.Shop.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Maroon.Shop.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly ShopContext _context;

        public AddressController(ShopContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("{addressId}")]
        public ActionResult<Address> GetById(long addressId)
        {
            var query = _context.Addresses.Where(a => a.AddressId == addressId);

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
        public ActionResult<IEnumerable<Address>> GetAddresses()
        {
            return _context.Addresses;
        }

        [HttpGet("ByPostCode")]
        public ActionResult<IEnumerable<Address>> GetAddressesByPostCode(string postcode)
        {
            var query = _context.Addresses.Where(a => a.PostCode.StartsWith(postcode));

            return query.ToList();
        }
    }
}
