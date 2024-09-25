using System.ComponentModel.DataAnnotations;

namespace Maroon.Shop.Api.Data.Requests
{
    /// <summary>
    /// Represents a Request model for retrieving a paginated list of Addresses by Post Code.
    /// </summary>
    public class GetAddressesByPostCodeRequest
    {
        /// <summary>
        /// Gets or Sets the Post Code of the Addresses to be retrieved. This field is required.
        /// </summary>
        [Required(ErrorMessage = "Post Code is required.")]
        public required string PostCode { get; set; }

        /// <summary>
        /// Gets or Sets the Page Number for pagination. Defaults to 1.
        /// </summary>
        /// <remarks>
        /// This property determines which page of Addresses to retrieve. The first page is 1.
        /// </remarks>
        public int PageNumber { get; set; } = 1;

        /// <summary>
        /// Gets or Sets the number of Addresses to retrieve per page. Defaults to 10.
        /// </summary>
        /// <remarks>
        /// This property determines how many Addresses are included in each page of results.
        /// </remarks>
        public int PageSize { get; set; } = 10;
    }
}