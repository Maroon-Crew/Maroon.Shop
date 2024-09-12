using System.ComponentModel.DataAnnotations;

namespace Maroon.Shop.Web.Models
{
    public class LoginModel
    {
        [Range(1, int.MaxValue, ErrorMessage = "Please specify a customer id")]
        public required int CustomerId { get; set; }
        public string? ReturnUrl { get; set; }
    }
}
