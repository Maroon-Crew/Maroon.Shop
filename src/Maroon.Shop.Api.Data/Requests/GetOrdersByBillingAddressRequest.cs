﻿using System.ComponentModel.DataAnnotations;

namespace Maroon.Shop.Api.Data.Requests
{
    /// <summary>
    /// Represents a Request model for retrieving a paginated list of Orders by Billing Address Id.
    /// </summary>
    public class GetOrdersByBillingAddressRequest
    {
        /// <summary>
        /// Gets or Sets the Shipping Address Id of the Orders to be retrieved. This field is required.
        /// </summary>
        [Required(ErrorMessage = "Billing Address Id is required.")]
        public long BillingAddressId { get; set; }

        /// <summary>
        /// Gets or Sets the Page Number for pagination. Defaults to 1.
        /// </summary>
        /// <remarks>
        /// This property determines which page of Orders to retrieve. The first page is 1.
        /// </remarks>
        public int PageNumber { get; set; } = 1;

        /// <summary>
        /// Gets or Sets the number of Orders to retrieve per page. Defaults to 10.
        /// </summary>
        /// <remarks>
        /// This property determines how many Orders are included in each page of results.
        /// </remarks>
        public int PageSize { get; set; } = 10;
    }
}