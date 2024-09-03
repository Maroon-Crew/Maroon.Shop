using System.ComponentModel.DataAnnotations;

namespace Maroon.Shop.Web.Models
{
    public enum ShirtSize
    {
        Small,
        Medium,
        Large,
        [Display(Name = "X-Large")]
        XLarge,
        [Display(Name = "XX-Large")]
        XXLarge,
    }

    public class ProductModel
    {
        public long ProductId { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public string? PleaseNote { get; set; }
        public required string UrlFriendlyName { get; set; }
        public required string ImageUrl { get; set; }
        public required decimal Price { get; set; }
        [Required]
        public ShirtSize? SelectedShirtSize { get; set; }
        public IEnumerable<ShirtSize> ShirtSizes { get; } = Enum.GetValues(typeof(ShirtSize)).Cast<ShirtSize>();
        [Range(1, 99)]
        [Required]
        public int? SelectedQuantity { get; set; }
    }
}
