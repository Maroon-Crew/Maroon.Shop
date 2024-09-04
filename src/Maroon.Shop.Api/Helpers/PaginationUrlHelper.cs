using Microsoft.AspNetCore.Mvc;

namespace Maroon.Shop.Api.Helpers
{
    /// <summary>
    /// A Static Class providing helper methods for generating pagination URLs.
    /// </summary>
    public static class PaginationUrlHelper
    {
        /// <summary>
        /// Generates a URL for a specific page number and page size.
        /// </summary>
        /// <param name="urlHelper">An <see cref="IUrlHelper"/> used for generating the next and previous page URLs.</param>
        /// <param name="routeName">A <see cref="string"/> representing the Route used for generating the Next and Previous page URLs.</param>
        /// <param name="pageNumber">An <see cref="int"/> representing a page number for the URL.</param>
        /// <param name="pageSize">An <see cref="int"/> representing the page size for the URL.</param>
        /// <returns>A nullable <see cref="string"/> representing the URL for the specified page, or null if the urlHelper is null.</returns>
        public static string? GeneratePageUrl(IUrlHelper urlHelper, string routeName, int pageNumber, int pageSize, object? routeValues = null)
        {
            if (urlHelper == null)
            {
                // The provided URL Helper is null, therefore return null.
                return null;
            }

            // Combine the page number, page size, and any additional route values.
            var values = new RouteValueDictionary(routeValues)
            {
                { "pageNumber", pageNumber },
                { "pageSize", pageSize }
            };

            return urlHelper.Link(routeName, values);
        }
    }
}