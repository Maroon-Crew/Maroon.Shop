namespace Maroon.Shop.Web.Models
{
    /// <summary>
    /// Represents a Basket.
    /// </summary>
    public class BasketModel
    {
        /// <summary>
        /// Gets or Sets a <see cref="long"/> representing the 'Id' of the Basket.
        /// </summary>
        public required long BasketId { get; set; }

        /// <summary>
        /// Gets or Sets a <see cref="long"/> representing the 'Id' of the Customer.
        /// </summary>
        public required long CustomerId { get; set; }

        /// <summary>
        /// Gets or Sets a <see cref="decimal"/> representing the 'Total Price' of the Basket.
        /// </summary>
        public required decimal TotalPrice { get; set; } = 0;

        /// <summary>
        /// Gets or Sets an <see cref="IEnumerable{BasketItemModel}"/> representing the Basket Items.
        /// </summary>
        public IEnumerable<BasketItemModel> BasketItems { get; set; } = [];
    }
}