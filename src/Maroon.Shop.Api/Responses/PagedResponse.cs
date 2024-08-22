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
    }
}