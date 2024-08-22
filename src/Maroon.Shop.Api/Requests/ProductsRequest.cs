namespace Maroon.Shop.Api.Requests
{
    public class ProductsRequest
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}