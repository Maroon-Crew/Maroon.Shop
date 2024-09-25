using Maroon.Shop.Api.Data.Requests;
using Maroon.Shop.Api.Data.Responses;
using Maroon.Shop.Data;
using Microsoft.EntityFrameworkCore;

namespace Maroon.Shop.Api.Data.Repositories
{
    public class CustomerRepository
    {
        private readonly ShopContext _context;

        public CustomerRepository(ShopContext context)
        {
            _context = context;
        }

        public CustomerResponse? GetById(GetCustomerRequest getCustomerRequest)
        {
            // Query for a Customer with the given customerId.
            var query = _context.Customers.Include(c => c.BillingAddress).Include(c => c.DefaultShippingAddress).Where(customer => customer.CustomerId == getCustomerRequest.CustomerId)
                .Select(c => new CustomerResponse
                {
                    BillingAddressId = c.BillingAddressId,
                    EmailAddress = c.EmailAddress,
                    CustomerId = c.CustomerId,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    DefaultShippingAddressId = c.DefaultShippingAddressId,
                });

            if (!query.Any())
            {
                // The Customer could not be found, return a 404 Not Found response.
                return null;
            }
            else
            {
                // Return the first matching Customer.
                return query.First();
            }
        }

        public IEnumerable<CustomerResponse> GetCustomers()
        {
            // Return all Customers.
            return _context.Customers.Include(c => c.BillingAddress).Include(c => c.DefaultShippingAddress).Select(c => new CustomerResponse
            {
                BillingAddressId = c.BillingAddressId,
                EmailAddress = c.EmailAddress,
                CustomerId = c.CustomerId,
                FirstName = c.FirstName,
                LastName = c.LastName,
                DefaultShippingAddressId = c.DefaultShippingAddressId,
            });
        }
    }
}