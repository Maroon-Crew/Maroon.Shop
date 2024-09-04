using Maroon.Shop.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace Maroon.Shop.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ShopContext _context;
        public CustomerController(ShopContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("{customerId}")]
        public ActionResult<Customer> GetById(long customerId)
        {
            var query = _context.Customers.Where(a => a.CustomerId == customerId);

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
        public ActionResult<IEnumerable<Customer>> GetCustomers()
        {
            return _context.Customers;
        }
    }
}
