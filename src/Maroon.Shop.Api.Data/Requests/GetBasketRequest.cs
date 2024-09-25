using System.ComponentModel.DataAnnotations;

namespace Maroon.Shop.Api.Data.Requests
{
    /// <summary>
    /// Represents a Request model for retrieving a Basket by it's identifier.
    /// </summary>
    public class GetBasketRequest
    {
        /// <summary>
        /// Gets or Sets the unique identifier of the Basket to be retrieved. This field is required.
        /// </summary>
        [Required(ErrorMessage = "Basket Id is required.")]
        public long BasketId { get; set; }
    }
}