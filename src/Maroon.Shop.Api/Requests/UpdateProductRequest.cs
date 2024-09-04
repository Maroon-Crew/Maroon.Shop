using System.ComponentModel.DataAnnotations;

namespace Maroon.Shop.Api.Requests
{
    /// <summary>
    /// Represents a Request model for updating an existing Product.
    /// </summary>
    public class UpdateProductRequest
    {
        /// <summary>
        /// Gets or Sets the unique identifier of the Product to be updated.
        /// </summary>
        public long ProductId { get; set; }

        /// <summary>
        /// Gets or Sets the Name of the Product. This field is required.
        /// </summary>
        /// <remarks>
        /// The Product name cannot exceed 50 characters.
        /// </remarks>
        [Required(ErrorMessage = "Product Name is required.")]
        [StringLength(50, ErrorMessage = "Product Name cannot exceed 50 characters.")]
        public required string Name { get; set; }

        /// <summary>
        /// Gets or Sets the URL Friendly Name of the Product. This field is required.
        /// </summary>
        /// <remarks>
        /// The URL Friendly Name cannot exceed 50 characters and can only contain letters, numbers, hyphens, and underscores.
        /// </remarks>
        [Required(ErrorMessage = "Url Friendly Name is required.")]
        [StringLength(50, ErrorMessage = "Url Friendly Name cannot exceed 50 characters.")]
        [RegularExpression("^[a-zA-Z0-9_-]+$", ErrorMessage = "Url Friendly Name can only contain letters, numbers, hyphens, and underscores.")]
        public required string UrlFriendlyName { get; set; }

        /// <summary>
        /// Gets or Sets the Price of the Product. This field is required.
        /// </summary>
        /// <remarks>
        /// The Price must be a positive value greater than zero.
        /// </remarks>
        [Required(ErrorMessage = "Price is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
        public decimal Price { get; set; }
    }
}