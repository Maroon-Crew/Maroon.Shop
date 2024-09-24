namespace Maroon.Shop.Web.Models
{
    /// <summary>
    /// Represents a Basket Item.
    /// </summary>
    public class BasketItemModel
    {
        /// <summary>
        /// Gets or Sets a <see cref="long"/> representing the 'Id' of the Basket Item.
        /// </summary>
        public required long BasketItemId { get; set; }

        /// <summary>
        /// Gets or Sets a <see cref="long"/> representing the 'Id' of the Basket.
        /// </summary>
        public required long BasketId { get; set; }

        /// <summary>
        /// Gets or Sets a <see cref="long"/> representing the 'Id' of the Product.
        /// </summary>
        public required long ProductId { get; set; }

        /// <summary>
        /// Gets or Sets a <see cref="string"/> representing the 'Name' of the Product.
        /// </summary>
        public required string ProductName { get; set; }

        /// <summary>
        /// Gets or Sets an <see cref="int"/> representing the 'Quantity' of Products.
        /// </summary>
        public required int Quantity { get; set; }

        /// <summary>
        /// Gets or Sets a <see cref="decimal"/> representing the 'Unit Price' of the Product.
        /// </summary>
        public required decimal UnitPrice { get; set; }

        /// <summary>
        /// Gets or Sets a <see cref="decimal"/> representing the 'Total Price' of the Basket Item.
        /// </summary>
        public required decimal TotalPrice { get; set; }
    }
}