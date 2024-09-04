using System.ComponentModel.DataAnnotations;

namespace Maroon.Shop.Api.Requests
{
    /// <summary>
    /// Represents a Request model for retrieving a Basket Item by it's identifier.
    /// </summary>
    public class GetBasketItemRequest
    {
        /// <summary>
        /// Gets or Sets the unique identifier of the Basket Item to be retrieved. This field is required.
        /// </summary>
        [Required(ErrorMessage = "Basket Item Id is required.")]
        public long BasketItemId { get; set; }
    }
}