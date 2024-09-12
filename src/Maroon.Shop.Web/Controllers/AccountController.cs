using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Maroon.Shop.Web.Models;
using Microsoft.AspNetCore.Authorization;

namespace Maroon.Shop.Web.Controllers
{
    public class AccountController : Controller
    {
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
    }
}
