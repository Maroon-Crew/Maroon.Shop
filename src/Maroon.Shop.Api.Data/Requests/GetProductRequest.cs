using System.ComponentModel.DataAnnotations;

namespace Maroon.Shop.Api.Data.Requests
{
    /// <summary>
    /// Represents a Request model for retrieving a Product by it's identifier.
    /// </summary>
    public class GetProductRequest
    {
        /// <summary>
        /// Gets or Sets the unique identifier of the Product to be retrieved. This field is required.
        /// </summary>
        [Required(ErrorMessage = "Product Id is required.")]
        public long ProductId { get; set; }
    }
}