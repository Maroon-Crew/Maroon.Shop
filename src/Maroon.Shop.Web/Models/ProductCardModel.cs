namespace Maroon.Shop.Web.Models
{
    public class ProductCardModel
    {
        public long ProductId { get; set; }
        public required string Name { get; set; }
        public required string UrlFriendlyName { get; set; }
        public required string ImageUrl { get; set; }
        public required decimal Price { get; set; }
    }
}
