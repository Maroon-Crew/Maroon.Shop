using Maroon.Shop.Data;

namespace Maroon.Shop.Web.Models
{
    public class BasketModel
    {
        public required long BasketId { get; set; }
        public required long CustomerId { get; set; }
        public required decimal TotalPrice { get; set; } = 0;
        public IEnumerable<BasketItemModel> BasketItems { get; set; } = [];
    }
}