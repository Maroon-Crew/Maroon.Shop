using System.ComponentModel.DataAnnotations;

namespace Maroon.Shop.Api.Requests
{
    public class CreateProductRequest
    {
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
        [Required(ErrorMessage = "Description is required.")]
        public required string Description { get; set; }
        [Required(ErrorMessage = "ImageUrl is required.")]
        public required string ImageUrl { get; set; }
    }
}