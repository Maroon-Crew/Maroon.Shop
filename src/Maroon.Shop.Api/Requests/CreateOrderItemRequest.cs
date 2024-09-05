using System.ComponentModel.DataAnnotations;

namespace Maroon.Shop.Api.Requests
{
    /// <summary>
    /// Represents a Request model for creating a new Order Item.
    /// </summary>
    public class CreateOrderItemRequest
    {
        /// <summary>
        /// Gets or Sets the Id of the Order that the Item is for. This field is required.
        /// </summary>
        [Required(ErrorMessage = "Order Id is required.")]
        public long OrderId { get; set; }

        /// <summary>
        /// Gets or Sets the Id of the Product that the Item is for. This field is required.
        /// </summary>
        [Required(ErrorMessage = "Product Id is required.")]
        public long ProductId { get; set; }

        /// <summary>
        /// Gets or Sets the Quantity. This field is required.
        /// </summary>
        /// <remarks>
        /// The Quantity must be a positive value greater than zero.
        /// </remarks>
        [Required(ErrorMessage = "Quantity is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than zero.")]
        public int Quantity { get; set; }

        /// <summary>
        /// Gets or Sets the Unit Price. This field is required.
        /// </summary>
        /// <remarks>
        /// The Unit Price must be a positive value greater than zero.
        /// </remarks>
        [Required(ErrorMessage = "Unit Price is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Unit Price must be greater than zero.")]
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// Gets or Sets the Total Price. This field is required.
        /// </summary>
        /// <remarks>
        /// The Total Price must be a positive value greater than zero.
        /// </remarks>
        [Required(ErrorMessage = "Total Price is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Total Price must be greater than zero.")]
        public decimal TotalPrice { get; set; }
    }
}