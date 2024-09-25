using Maroon.Shop.Api.Data.Repositories;
using Maroon.Shop.Api.Data.Requests;
using Maroon.Shop.Api.Data.Responses;
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
        private readonly CustomerRepository _customerRepository;

        public CustomerController(CustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        /// <summary>
        /// Handles GET requests to "api/Customer/{customerId}".
        /// Attempts to retrieve a Customer for the given Customer Id.
        /// </summary>
        /// <param name="customerId">A <see cref="long"/> representing the Id of the Customer.</param>
        /// <returns>An <see cref="ActionResult{CustomerResponse}"/> representing the Customer found.</returns>
        [HttpGet]
        [Route("{CustomerId}")]
        public ActionResult<CustomerResponse> GetById([FromRoute] GetCustomerRequest getCustomerRequest)
        {
            // Query for a Customer with the given customerId.
            var customer = _customerRepository.GetById(getCustomerRequest);

            if (customer == null)
            {
                // The Customer could not be found, return a 404 Not Found response.
                return NotFound();
            }
            else
            {
                // Return the first matching Customer.
                return customer;
            }
        }

        [HttpGet]
        [Route("{CustomerId}/Orders")]
        public IActionResult GetOrdersByCustomerId([FromRoute] GetCustomerRequest getCustomerRequest)
        {
            GetOrdersByCustomerRequest getOrderByCustomerRequest = new()
            {
                CustomerId = getCustomerRequest.CustomerId,
            };

            var url = Url.ActionLink("GetOrdersByCustomer", "Order", getOrderByCustomerRequest);
            var url1 = Url.ActionLink<OrderController>("GetOrdersByCustomer", getOrderByCustomerRequest);
            var url2 = Url.ActionLink<OrderController>(nameof(OrderController.GetOrdersByCustomer), getOrderByCustomerRequest);
            // does not work, would be nice to have
            var url3 = Url.ActionLink((OrderController o) => o.GetOrdersByCustomer(getOrderByCustomerRequest));

            //return RedirectToRoute("OrdersByCustomerId", new { customerId });
            //return RedirectToAction("GetOrdersByCustomer", "Order", new { customerId });
            return this.RedirectToAction((OrderController o) => o.GetOrdersByCustomer(getOrderByCustomerRequest));
        }

        /// <summary>
        /// Handles GET requests to "api/Customer/All".
        /// Attempts to retrieve all Customers.
        /// </summary>
        /// <returns>An <see cref="ActionResult{IEnumerable{CustomerResponse}}"/> representing the Customers found.</returns>
        [HttpGet()]
        public ActionResult<IEnumerable<CustomerResponse>> GetCustomers()
        {
            // Return all Customers.
            return Ok(_customerRepository.GetCustomers());
        }
    }
}