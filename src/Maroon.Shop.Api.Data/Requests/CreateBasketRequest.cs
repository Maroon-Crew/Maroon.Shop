using System.ComponentModel.DataAnnotations;

namespace Maroon.Shop.Api.Data.Requests
{
    /// <summary>
    /// Represents a Request model for creating a new Basket.
    /// </summary>
    public class CreateBasketRequest
    {
        /// <summary>
        /// Gets or Sets the Id of the Customer that the Basket is for. This field is required.
        /// </summary>
        [Required(ErrorMessage = "Customer Id is required.")]
        public long CustomerId { get; set; }

        /// <summary>
        /// Gets or Sets the Total Price of the Basket. This field is required.
        /// </summary>
        /// <remarks>
        /// The Total Price must be a positive value greater than zero.
        /// </remarks>
        [Required(ErrorMessage = "Total Price is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Total Price must be greater than zero.")]
        public decimal TotalPrice { get; set; }
    }
}