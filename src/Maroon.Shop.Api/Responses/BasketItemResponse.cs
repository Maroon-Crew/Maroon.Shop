namespace Maroon.Shop.Api.Responses
{
    /// <summary>
    /// Represents a response model for a Basket Item.
    /// </summary>
    public class BasketItemResponse
    {
        /// <summary>
        /// Gets or Sets the unique identifier of the Basket Item.
        /// </summary>
        public long BasketItemId { get; set; }

        // <summary>
        /// Gets or Sets the Id of the Basket that the Basket Item is for.
        /// </summary>
        public long BasketId { get; set; }

        /// <summary>
        /// Gets or Sets the Id of the Product that the Basket Item is for.
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