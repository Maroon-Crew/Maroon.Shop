using System.ComponentModel.DataAnnotations;

namespace Maroon.Shop.Api.Requests
{
    /// <summary>
    /// Represents a Request model for retrieving a paginated list of Basket Items by Basket.
    /// </summary>
    public class GetBasketItemsByBasketRequest
    {
        /// <summary>
        /// Gets or Sets the Basket Id of the Basket Items to be retrieved. This field is required.
        /// </summary>
        [Required(ErrorMessage = "Basket Id is required.")]
        public required long BasketId { get; set; }

        /// <summary>
        /// Gets or Sets the Page Number for pagination. Defaults to 1.
        /// </summary>
        /// <remarks>
        /// This property determines which page of Basket Items to retrieve. The first page is 1.
        /// </remarks>
        public int PageNumber { get; set; } = 1;

        /// <summary>
        /// Gets or Sets the number of Addresses to retrieve per page. Defaults to 10.
        /// </summary>
        /// <remarks>
        /// This property determines how many Basket Items are included in each page of results.
        /// </remarks>
        public int PageSize { get; set; } = 10;
    }
}