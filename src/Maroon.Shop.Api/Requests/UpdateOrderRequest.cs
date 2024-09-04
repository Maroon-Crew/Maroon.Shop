using System.ComponentModel.DataAnnotations;

namespace Maroon.Shop.Api.Requests
{
    /// <summary>
    /// Represents a Request model for updating an existing Order.
    /// </summary>
    public class UpdateOrderRequest
    {
        /// <summary>
        /// Gets or Sets the unique identifier of the Order to be updated.
        /// </summary>
        public long OrderId { get; set; }

        /// <summary>
        /// Gets or Sets the Id of the Customer that the Order is for. This field is required.
        /// </summary>
        [Required(ErrorMessage = "Customer Id is required.")]
        public long CustomerId { get; set; }

        /// <summary>
        /// Gets or Sets the Total Price of the Order. This field is required.
        /// </summary>
        /// <remarks>
        /// The Total Price must be a positive value greater than zero.
        /// </remarks>
        [Required(ErrorMessage = "Total Price is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Total Price must be greater than zero.")]
        public decimal TotalPrice { get; set; }

        /// <summary>
        /// Gets or Sets the Creation Date of the Order. This field is required.
        /// </summary>
        [Required(ErrorMessage = "Date Created is required.")]
        public DateTime DateCreated { get; set; }

        /// <summary>
        /// Gets or Sets the Id of the Billing Address. This field is required.
        /// </summary>
        [Required(ErrorMessage = "Billing Address Id is required.")]
        public long BillingAddressId { get; set; }

        /// <summary>
        /// Gets or Sets the Id of the Billing Address. This field is required.
        /// </summary>
        [Required(ErrorMessage = "Shipping Address Id is required.")]
        public long ShippingAddressId { get; set; }
    }
}