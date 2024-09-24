using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Maroon.Shop.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Maroon.Shop.Data;
using Microsoft.EntityFrameworkCore;

namespace Maroon.Shop.Web.Controllers
{
    public class AccountController : Controller
    {
        // Private backing fields.
        private readonly ShopContext _context;

        /// <summary>
        /// Constructor. Initialises an instance of <see cref="AccountController"/> and sets up the Data Context.
        /// </summary>
        /// <param name="context">A <see cref="ShopContext"/> representing the Maroon Shop Data Context.</param>
        public AccountController(ShopContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Login([FromQuery] string returnUrl)
        {
            var model = new LoginModel
            {
                CustomerId = 0,
                ReturnUrl = returnUrl,
            };

            return View(model);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            if (!ModelState.IsValid)
            {
                return View(loginModel);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, loginModel.CustomerId.ToString(), ClaimValueTypes.Integer),
                new Claim("CustomerId", loginModel.CustomerId.ToString(), ClaimValueTypes.Integer),
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                AllowRefresh = true,
                // Refreshing the authentication session should be allowed.

                //ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                // The time at which the authentication ticket expires. A 
                // value set here overrides the ExpireTimeSpan option of 
                // CookieAuthenticationOptions set with AddCookie.

                IsPersistent = true,
                // Whether the authentication session is persisted across 
                // multiple requests. When used with cookies, controls
                // whether the cookie's lifetime is absolute (matching the
                // lifetime of the authentication ticket) or session-based.

                //IssuedUtc = <DateTimeOffset>,
                // The time at which the authentication ticket was issued.

                RedirectUri = loginModel.ReturnUrl,
                // The full path or absolute URI to be used as an http 
                // redirect response value.
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

            if (!string.IsNullOrWhiteSpace(loginModel.ReturnUrl))
            {
                return LocalRedirect(loginModel.ReturnUrl);
            }
            else
            {
                return LocalRedirect("/");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction(nameof(Login));
        }

        /// <summary>
        /// Handles GET to '../Account/'.
        /// Redirects to the 'SimonAccount' action, which shows the Account details.
        /// </summary>
        /// <returns>An <see cref="IActionResult"/> redirecting to the SimonAccount Action.</returns>
        public IActionResult Index()
        {
            // Redirect to SimonAccount action.
            return RedirectToAction("SimonAccount");
        }

        /// <summary>
        /// Handles GET to '../Account/SimonAccount'.
        /// Displays the user's Account details. 
        /// If the user is not authenticated, returns the Log-in View.
        /// </summary>
        /// <returns> A <see cref="Task{IActionResult}"/> representing the Account View with Account Model data.</returns>
        public async Task<IActionResult> SimonAccount()
        {
            // Check if the user is authenticated.
            if (User.Identity?.IsAuthenticated != true)
            {
                // The user is not authenticated. Therefore return the Log-in View.
                return View(0);
            }

            // Get the logged-in user's account name (Customer Id).
            string userAccountId = User.Identity?.Name ?? "0";
            var customerId = int.Parse(userAccountId);

            // Build the query to fetch the Customer and it's Addresses for the logged-in user.
            var customerQuery = from customerAccount in _context.Customers
                              where customerAccount.CustomerId == customerId
                              select new CustomerModel
                              {
                                  CustomerId = customerAccount.CustomerId,
                                  FirstName = customerAccount.FirstName, 
                                  LastName = customerAccount.LastName,
                                  EmailAddress = customerAccount.EmailAddress,
                                  BillingAddressId = customerAccount.BillingAddressId,
                                  DefaultShippingAddressId = customerAccount.DefaultShippingAddressId,
                                  BillingAddress = (new AddressModel
                                  {
                                      AddressId = customerAccount.BillingAddressId,
                                      NameOfRecipient = customerAccount.BillingAddress.NameOfRecipient,
                                      Line1 = customerAccount.BillingAddress.Line1,
                                      Line2 = customerAccount.BillingAddress.Line2,
                                      Town = customerAccount.BillingAddress.Town,
                                      County = customerAccount.BillingAddress.County,
                                      PostCode = customerAccount.BillingAddress.PostCode,
                                      Country = customerAccount.BillingAddress.Country
                                  }),
                                  // Populate ShippingAddress only if DefaultShippingAddressId != BillingAddressId
                                  ShippingAddress = customerAccount.DefaultShippingAddressId != customerAccount.BillingAddressId
                                    ? new AddressModel
                                    {
                                        AddressId = customerAccount.DefaultShippingAddressId,
                                        NameOfRecipient = customerAccount.DefaultShippingAddress.NameOfRecipient,
                                        Line1 = customerAccount.DefaultShippingAddress.Line1,
                                        Line2 = customerAccount.DefaultShippingAddress.Line2,
                                        Town = customerAccount.DefaultShippingAddress.Town,
                                        County = customerAccount.DefaultShippingAddress.County,
                                        PostCode = customerAccount.DefaultShippingAddress.PostCode,
                                        Country = customerAccount.DefaultShippingAddress.Country
                                    }
                                    : null, // ShippingAddress remains null if the IDs are the same
                                    IsShippingAddressTheSameAsBillingAddress = customerAccount.DefaultShippingAddressId == customerAccount.BillingAddressId
                              };

            // Execute the query and fetch the Customer data.
            var customer = await customerQuery.AsNoTracking().FirstOrDefaultAsync();

            // Return the SimonAccount view with the Customer model.
            return View("SimonAccount", customer);
        }

        /// <summary>
        /// Handles GET to '../Account/UpdateCustomer'.
        /// Attempts to update the user's Account details. 
        /// </summary>
        /// <returns> A <see cref="Task{IActionResult}"/> representing the Acction/View to return to.</returns>
        [HttpPost]
        public IActionResult UpdateCustomer(CustomerModel customerModel)
        {
            if (ModelState.IsValid)
            {
                // Get the existing Customer with related Billing and Shipping Addresses.
                var customer = _context.Customers
                    .Include(c => c.BillingAddress)
                    .Include(c => c.DefaultShippingAddress)
                    .FirstOrDefault(c => c.CustomerId == customerModel.CustomerId);

                if (customer == null)
                {
                    // The Customer could not be found.
                    return View("Error");
                }

                // Update Customer details.
                customer.FirstName = customerModel.FirstName;
                customer.LastName = customerModel.LastName;
                customer.EmailAddress = customerModel.EmailAddress;

                // Update Billing Address if it exists.
                if (customerModel.BillingAddress != null)
                {
                    UpdateAddress(customer.BillingAddress, customerModel.BillingAddress);
                }

                // Handle the Shipping Address.
                if (customerModel.IsShippingAddressTheSameAsBillingAddress)
                {
                    // Set Shipping Address to Billing Address.
                    customer.DefaultShippingAddressId = customer.BillingAddressId;
                    customer.DefaultShippingAddress = customer.BillingAddress;
                }
                else
                {
                    if (customerModel.ShippingAddress?.AddressId != 0)
                    {
                        // Update existing Shipping Address.
                        UpdateAddress(customer.DefaultShippingAddress, customerModel.ShippingAddress);
                    }
                    else if (customerModel.ShippingAddress != null)
                    {
                        // Create a new Shipping Address.
                        var newShippingAddress = new Address
                        {
                            NameOfRecipient = customerModel.ShippingAddress.NameOfRecipient,
                            Line1 = customerModel.ShippingAddress.Line1,
                            Line2 = customerModel.ShippingAddress.Line2,
                            Town = customerModel.ShippingAddress.Town,
                            County = customerModel.ShippingAddress.County,
                            Country = customerModel.ShippingAddress.Country,
                            PostCode = customerModel.ShippingAddress.PostCode
                        };

                        customer.DefaultShippingAddress = newShippingAddress;

                        //  Add the new Address to the Context.
                        _context.Addresses.Add(newShippingAddress);
                    }
                }

                // Save any changes to the Database.
                _context.SaveChanges();

                return RedirectToAction("Index", "Product");
            }

            // If model is invalid, re-render the form with validation errors.
            return View("SimonAccount", customerModel);
        }

        /// <summary>
        /// Attempts to update an existing Address with an Address Model.
        /// </summary>
        /// <param name="existingAddress">An <see cref="Address"/> representing the existing Address.</param>
        /// <param name="newAddressModel">An <see cref="AddressModel"/> representing the Updated Address values.</param>
        private void UpdateAddress(Address existingAddress, AddressModel newAddressModel)
        {
            if (existingAddress != null && newAddressModel != null)
            {
                existingAddress.NameOfRecipient = newAddressModel.NameOfRecipient;
                existingAddress.Line1 = newAddressModel.Line1;
                existingAddress.Line2 = newAddressModel.Line2;
                existingAddress.Town = newAddressModel.Town;
                existingAddress.County = newAddressModel.County;
                existingAddress.Country = newAddressModel.Country;
                existingAddress.PostCode = newAddressModel.PostCode;
            }
        }
    }
}