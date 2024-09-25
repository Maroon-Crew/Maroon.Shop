using System.ComponentModel.DataAnnotations;

namespace Maroon.Shop.Api.Data.Requests
{
    /// <summary>
    /// Represents a Request model for retrieving an Order Item by it's identifier.
    /// </summary>
    public class GetOrderItemRequest
    {
        /// <summary>
        /// Gets or Sets the unique identifier of the Order Item to be retrieved. This field is required.
        /// </summary>
        [Required(ErrorMessage = "Order Item Id is required.")]
        public long OrderItemId { get; set; }
    }
}