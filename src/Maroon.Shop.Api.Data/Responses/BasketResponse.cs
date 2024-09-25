namespace Maroon.Shop.Api.Data.Responses
{
    /// <summary>
    /// Represents a response model for a Basket.
    /// </summary>
    public class BasketResponse
    {
        /// <summary>
        /// Gets or Sets the unique identifier of the Basket.
        /// </summary>
        public long BasketId { get; set; }

        /// <summary>
        /// Gets or Sets the Id of the Customer that the Basket is for.
        /// </summary>
        public long CustomerId { get; set; }

        /// <summary>
        /// Gets or Sets the Total Price of the Basket.
        /// </summary>
        public decimal TotalPrice { get; set; }
    }
}