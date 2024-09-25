namespace Maroon.Shop.Api.Data.Responses
{
    /// <summary>
    /// Represents a response model for an Order Item.
    /// </summary>
    public class OrderItemResponse
    {
        /// <summary>
        /// Gets or Sets the unique identifier of the Order to be updated.
        /// </summary>
        public long OrderItemId { get; set; }

        /// <summary>
        /// Gets or Sets the Id of the Order that the Item is for.
        /// </summary>
        public long OrderId { get; set; }

        /// <summary>
        /// Gets or Sets the Id of the Product that the Item is for.
        /// </summary>
        public long ProductId { get; set; }

        /// <summary>
        /// Gets or Sets the Quantity.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Gets or Sets the Unit Price.
        /// </summary>
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// Gets or Sets the Total Price.
        /// </summary>
        public decimal TotalPrice { get; set; }
    }
}