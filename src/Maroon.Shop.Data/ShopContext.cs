using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Maroon.Shop.Data
{
    public class ShopContext : DbContext
    {
        public ShopContext() { }

        public ShopContext(DbContextOptions<ShopContext> options) : base(options) { }

        public DbSet<Address> Addresses { get; set; }
        public DbSet<Basket> Baskets { get; set; }
        public DbSet<BasketItem> BasketItems { get; set; }        
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer(@"Data Source=(LocalDB)\MSSQLLocalDB;Database=Maroon.Shop.Database;Integrated Security=True");
        }
    }

    public class Address
    {
        public long AddressId { get; set; }
        public required string NameOfRecipient { get; set; }
        public required string Line1 { get; set; }
        public string? Line2 { get; set; }
        public string? Town { get; set; }
        public string? County { get; set; }
        public required string PostCode { get; set; }
        public string? Country { get; set; }
    }

    public class Basket
    {
        public long BasketId { get; set; }
        //public long CustomerId { get; set; }
        public required Customer Customer { get; set; }
        public required decimal TotalPrice { get; set; }
        public ICollection<BasketItem> Items { get; set; }
    }

    public class BasketItem
    {
        public long BasketItemId { get; set; }
        //public long BasketId { get; set; }
        public required Basket Basket { get; set; }
        //public long ProductId { get; set; }
        public required Product Product { get; set; }
        public required int Quantity { get; set; }
        public required decimal UnitPrice { get; set; }
        public required decimal TotalPrice { get; set; }
    }

    public class Customer
    {
        public long CustomerId { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string EmailAddress { get; set; }
        public long BillingAddressId { get; set; }
        public required Address BillingAddress { get; set; }
        public long DefaultShippingAddressId { get; set; }
        public required Address DefaultShippingAddress { get; set; }
        public ICollection<Basket> Baskets { get; set; }
        public ICollection<Order> Orders { get; set; }
    }

    public class Order
    {
        public long OrderId { get; set; }
        //public long CustomerId { get; set; }
        public required Customer Customer { get; set; }
        public required decimal TotalPrice { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime DateCreated { get; set; }
        public long BillingAddressId { get; set; }
        public required Address BillingAddress { get; set; }
        public long ShippingAddressId { get; set; }
        public required Address ShippingAddress { get; set; }
        public ICollection<OrderItem> Items { get; set; }
    }

    public class OrderItem
    {
        public long OrderItemId { get; set; }
        //public long OrderId { get; set; }
        public required Order Order { get; set; }
        //public long ProductId { get; set; }
        public required Product Product { get; set; }
        public required int Quantity { get; set; }
        public required decimal UnitPrice { get; set; }
        public required decimal TotalPrice { get; set; }
    }

    public class Product
    {
        public long ProductId { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public string? PleaseNote { get; set; }
        public required string UrlFriendlyName { get; set; }
        public required string ImageUrl { get; set; }
        public required decimal Price { get; set; }
    }
}
