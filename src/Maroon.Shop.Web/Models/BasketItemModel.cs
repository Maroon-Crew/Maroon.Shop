namespace Maroon.Shop.Web.Models
{
    public class BasketItemModel
    {
        public required long BasketItemId { get; set; }
        public required long BasketId { get; set; }
        public required long ProductId { get; set; }
        public required string ProductName { get; set; }
        public required int Quantity { get; set; }
        public required decimal UnitPrice { get; set; }
        public required decimal TotalPrice { get; set; }
    }
}