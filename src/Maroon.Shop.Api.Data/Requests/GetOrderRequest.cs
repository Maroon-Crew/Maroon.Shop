using System.ComponentModel.DataAnnotations;

namespace Maroon.Shop.Api.Data.Requests
{
    /// <summary>
    /// Represents a Request model for retrieving an Order by it's identifier.
    /// </summary>
    public class GetOrderRequest
    {
        /// <summary>
        /// Gets or Sets the unique identifier of the Order to be retrieved. This field is required.
        /// </summary>
        [Required(ErrorMessage = "Order Id is required.")]
        public long OrderId { get; set; }
    }
}