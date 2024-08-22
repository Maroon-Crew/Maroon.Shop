using System.ComponentModel.DataAnnotations;

namespace Maroon.Shop.Api.Requests
{
    public class UpdateProductRequest
    {
        public long ProductId { get; set; }

        [Required(ErrorMessage = "Product Name is required.")]
        [StringLength(50, ErrorMessage = "Product Name cannot exceed 50 characters.")]
        public required string Name { get; set; }

        [Required(ErrorMessage = "Url Friendly Name is required.")]
        [StringLength(50, ErrorMessage = "Url Friendly Name cannot exceed 50 characters.")]
        [RegularExpression("^[a-zA-Z0-9_-]+$", ErrorMessage = "Url Friendly Name can only contain letters, numbers, hyphens, and underscores.")]
        public required string UrlFriendlyName { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
        public decimal Price { get; set; }
    }
}