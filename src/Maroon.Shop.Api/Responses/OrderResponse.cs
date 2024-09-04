namespace Maroon.Shop.Api.Responses
{
    /// <summary>
    /// Represents a response model for an Order.
    /// </summary>
    public class OrderResponse
    {
        /// <summary>
        /// Gets or Sets the unique identifier of the Order.
        /// </summary>
        public long OrderId { get; set; }

        /// <summary>
        /// Gets or Sets the Id of the Customer that the Order is for.
        /// </summary>
        public long CustomerId { get; set; }

        /// <summary>
        /// Gets or Sets the Total Price of the Order.
        /// </summary>
        public decimal TotalPrice { get; set; }

        /// <summary>
        /// Gets or Sets the Creation Date of the Order.
        /// </summary>
        public DateTime DateCreated { get; set; }

        /// <summary>
        /// Gets or Sets the Id of the Billing Address.
        /// </summary>
        public long BillingAddressId { get; set; }

        /// <summary>
        /// Gets or Sets the Id of the Billing Address.
        /// </summary>
        public long ShippingAddressId { get; set; }
    }
}