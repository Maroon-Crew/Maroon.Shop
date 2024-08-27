using Maroon.Shop.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection;
using static Maroon.Shop.Api.Controllers.CustomerController;

namespace Maroon.Shop.Api.Controllers
{
    public static class UrlHelperExtensions
    {
        public static string? ActionLink<TController>(this IUrlHelper urlHelper, string action, object? values = null) where TController : Controller
        {
            return urlHelper.ActionLink(action, typeof(TController).Name.Replace("Controller", ""), values);
        }

        // does not work
        public static string? ActionLink<TController, TResult>(this IUrlHelper urlHelper, Expression<Func<TController, ActionResult<TResult>>> actionSelector) where TController : Controller
        {
            var methodInfo = GetMethodInfo(actionSelector);
            var values = GetMethodValues(actionSelector);
            return urlHelper.ActionLink(methodInfo.Name, typeof(TController).Name.Replace("Controller", ""), values);
        }

        private static MemberInfo GetMethodInfo<TController, TResult>(Expression<Func<TController, ActionResult<TResult>>> expression)
        {
            var callExpression = (MethodCallExpression)expression.Body;

            return callExpression.Method;
        }

        private static object? GetMethodValues<TController, TResult>(Expression<Func<TController, ActionResult<TResult>>> expression)
        {
            var callExpression = (MethodCallExpression)expression.Body;

            var query = from memberExpression in callExpression.Arguments.OfType<MemberExpression>()
                        where memberExpression.Expression.NodeType == ExpressionType.Constant
                        let constantExpression = (ConstantExpression)memberExpression.Expression
                        let parameterName = memberExpression.Member.Name
                        let constantValue = constantExpression.Value
                        select memberExpression.Member.ReflectedType.GetField(parameterName).GetValue(constantValue);

            return query.FirstOrDefault();
        }
    }

    public static class ControllerExtensions
    {
        public static IActionResult RedirectToAction<TController, TResult>(this ControllerBase controller, Expression<Func<TController, ActionResult<TResult>>> actionSelector) where TController : Controller
        {
            var methodInfo = GetMethodInfo(actionSelector);
            var values = GetMethodValues(actionSelector);
            return controller.RedirectToAction(methodInfo.Name, typeof(TController).Name.Replace("Controller", ""), values);
        }

        private static MemberInfo GetMethodInfo<TController, TResult>(Expression<Func<TController, ActionResult<TResult>>> expression)
        {
            var callExpression = (MethodCallExpression)expression.Body;

            return callExpression.Method;
        }

        private static object? GetMethodValues<TController, TResult>(Expression<Func<TController, ActionResult<TResult>>> expression)
        {
            var callExpression = (MethodCallExpression)expression.Body;

            var query = from memberExpression in callExpression.Arguments.OfType<MemberExpression>()
                        where memberExpression.Expression.NodeType == ExpressionType.Constant
                        let constantExpression = (ConstantExpression)memberExpression.Expression
                        let parameterName = memberExpression.Member.Name
                        let constantValue = constantExpression.Value
                        select memberExpression.Member.ReflectedType.GetField(parameterName).GetValue(constantValue);

            return query.FirstOrDefault();
        }

        public static IActionResult RedirectToAction<TController>(this ControllerBase controller, string action, object? values = null) where TController : Controller
        {
            return controller.RedirectToAction(action, typeof(TController).Name.Replace("Controller", ""), values);
        }
    }

    public class GetCustomerRequest
    {
        public long CustomerId { get; set; }
    }

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
        [Route("{CustomerId}")]
        public ActionResult<Customer> GetById([FromRoute] GetCustomerRequest getCustomerRequest)
        {
            // Query for a Customer with the given customerId.
            var query = _context.Customers.Include(c => c.BillingAddress).Include(c => c.DefaultShippingAddress).Where(customer => customer.CustomerId == getCustomerRequest.CustomerId);

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

        [HttpGet]
        [Route("{CustomerId}/Orders")]
        public IActionResult GetOrdersByCustomerId([FromRoute] GetCustomerRequest getCustomerRequest)
        {
            GetOrderByCustomerRequest getOrderByCustomerRequest = new()
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
        /// <returns>An <see cref="ActionResult{IEnumerable{Customer}}"/> representing the Customers found.</returns>
        [HttpGet()]
        public ActionResult<IEnumerable<Customer>> GetCustomers()
        {
            // Return all Customers.
            return Ok(_context.Customers.Include(c => c.BillingAddress).Include(c => c.DefaultShippingAddress));
        }
    }
}