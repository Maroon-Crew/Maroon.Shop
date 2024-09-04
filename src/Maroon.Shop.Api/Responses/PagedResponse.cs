using Maroon.Shop.Api.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace Maroon.Shop.Api.Responses
{
    /// <summary>
    /// Represents a paginated response for a collection of items.
    /// </summary>
    /// <typeparam name="T">The Type of items in the Data collection.</typeparam>
    public class PagedResponse<T>
    {
        /// <summary>
        /// Gets or Sets the collection of data items for the current page.
        /// </summary>
        public IEnumerable<T> Data { get; set; } = [];

        /// <summary>
        /// Gets or Sets the current page number.
        /// </summary>
        public int PageNumber { get; set; }

        /// <summary>
        /// Gets or Sets the number of items per page.
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Gets or Sets the total number of records in the collection.
        /// </summary>
        public int TotalRecords { get; set; }

        /// <summary>
        /// Gets the total number of pages, calculated based on the total records and page size.
        /// </summary>
        public int TotalPages => (int)Math.Ceiling((double)TotalRecords / PageSize);

        /// <summary>
        /// Gets the URL for the 'Next' Page of results. If null, there are no more pages of results to navigate to.
        /// </summary>
        public string? NextPageUrl { get; set; }

        /// <summary>
        /// Gets the URL for the 'Previous' Page of results. If null, there are no previous pages of results to navigate to.
        /// </summary>
        public string? PreviousPageUrl { get; set; }

        /// <summary>
        /// Constructor, creates a new instance of a Paged Response.
        /// </summary>
        /// <param name="data">An <see cref="IEnumerable{T}"/> representing the Data being returned.</param>
        /// <param name="pageNumber">An <see cref="int"/> representing the current page number.</param>
        /// <param name="pageSize">An <see cref="int"/> representing number of items per page.</param>
        /// <param name="totalRecords">An <see cref="int"/> representing the total number of records in the collection.</param>
        /// <param name="urlHelper">An <see cref="IUrlHelper"/> used for generating the next and previous page URLs.</param>
        /// <param name="routeName">A <see cref="string"/> representing the Route used for generating the Next and Previous page URLs.</param>
        /// <param name="routeValues">A Nullable <see cref="object"/> representing any Values from the Route Query.</param>
        public PagedResponse(IEnumerable<T> data, int pageNumber, int pageSize, int totalRecords, IUrlHelper urlHelper, string routeName, object? routeValues = null)
        {
            Data = data;
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalRecords = totalRecords;

            NextPageUrl = pageNumber < TotalPages
                ? PaginationUrlHelper.GeneratePageUrl(urlHelper, routeName, pageNumber + 1, pageSize, routeValues)
                : null;

            PreviousPageUrl = pageNumber > 1
                ? PaginationUrlHelper.GeneratePageUrl(urlHelper, routeName, pageNumber - 1, pageSize, routeValues)
                : null;
        }
    }
}