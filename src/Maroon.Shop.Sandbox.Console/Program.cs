using Maroon.Shop.Data;
using Microsoft.EntityFrameworkCore;

using (var context = new ShopContext())
{    
    await WipeDatabase(context);
    
    var product1 = new Product
    {
        Name = "Product 1",
        Price = 10,
        UrlFriendlyName = "product-1",
    };

    var product2 = new Product
    {
        Name = "Product 2",
        Price = 20,
        UrlFriendlyName = "product-2",
    };

    var product3 = new Product
    {
        Name = "Product 3",
        Price = 30,
        UrlFriendlyName = "product-3",
    };

    var customer1Address = new Address
    {
        NameOfRecipient = "John Doe",
        Line1 = "123 Main Street",
        Town = "Anytown",
        County = "Anyshire",
        PostCode = "AB1 2CD",
        Country = "United Kingdom",
    };

    var customer1 = new Customer
    {
        FirstName = "John",
        LastName = "Doe",
        EmailAddress = "john@test.com",
        DefaultShippingAddress = customer1Address,
        BillingAddress = customer1Address,
    };

    await context.Customers.AddAsync(customer1);

    var customer2ShippingAddress = new Address
    {
        NameOfRecipient = "Jane Doe",
        Line1 = "456 High Street",
        Town = "Anytown",
        County = "Anyshire",
        PostCode = "EF3 4GH",
        Country = "United Kingdom",
    };

    var customer2BillingAddress = new Address
    {
        NameOfRecipient = "Jane Doe",
        Line1 = "789 Low Street",
        Town = "Anytown",
        County = "Anyshire",
        PostCode = "IJ5 6KL",
        Country = "United Kingdom",
    };

    var customer2 = new Customer
    {
        FirstName = "Jane",
        LastName = "Doe",
        EmailAddress = "jane@test.com",
        DefaultShippingAddress = customer2ShippingAddress,
        BillingAddress = customer2BillingAddress,
    };

    await context.Customers.AddAsync(customer2);

    var basket1 = new Basket
    {
        Customer = customer1,
        TotalPrice = 0,
    };

    var basket1Items = new BasketItem[]
    {
        new BasketItem
        {
            Basket = basket1,
            Product = product1,
            Quantity = 1,
            UnitPrice = product1.Price,
            TotalPrice = product1.Price,
        },
        new BasketItem
        {
            Basket = basket1,
            Product = product2,
            Quantity = 2,
            UnitPrice = product2.Price,
            TotalPrice = product2.Price * 2,
        },
    };

    basket1.TotalPrice = basket1Items.Sum(x => x.TotalPrice);
    basket1.Items = basket1Items;

    await context.Baskets.AddAsync(basket1);

    var basket2 = new Basket
    {
        Customer = customer2,
        TotalPrice = 0,
    };

    var basket2Items = new BasketItem[]
    {
        new BasketItem
        {
            Basket = basket2,
            Product = product2,
            Quantity = 1,
            UnitPrice = product2.Price,
            TotalPrice = product2.Price,
        },
        new BasketItem
        {
            Basket = basket2,
            Product = product3,
            Quantity = 2,
            UnitPrice = product3.Price,
            TotalPrice = product3.Price * 2,
        },
    };

    basket2.TotalPrice = basket2Items.Sum(x => x.TotalPrice);
    basket2.Items = basket2Items;

    await context.Baskets.AddAsync(basket2);

    var order1 = ConvertBasketToOrder(basket1);
    await context.Orders.AddAsync(order1);
    context.Baskets.Remove(basket1);
    
    await context.SaveChangesAsync();
}

Order ConvertBasketToOrder(Basket basket)
{
    var order = new Order
    {
        BillingAddress = basket.Customer.BillingAddress,
        Customer = basket.Customer,
        ShippingAddress = basket.Customer.DefaultShippingAddress,
        TotalPrice = basket.TotalPrice,
    };

    var orderItems = basket.Items.Select(basketItem => new OrderItem
    {
        Product = basketItem.Product,
        Quantity = basketItem.Quantity,
        UnitPrice = basketItem.UnitPrice,
        TotalPrice = basketItem.TotalPrice,
        Order = order,
    }).ToList();

    order.Items = orderItems;

    return order;
}

async Task WipeDatabase(ShopContext context)
{
    await context.Customers.ExecuteDeleteAsync();
    await context.Addresses.ExecuteDeleteAsync();
    await context.BasketItems.ExecuteDeleteAsync();
    await context.Baskets.ExecuteDeleteAsync();
    await context.OrderItems.ExecuteDeleteAsync();
    await context.Orders.ExecuteDeleteAsync();
    await context.Products.ExecuteDeleteAsync();
    await context.SaveChangesAsync();
    await context.Database.ExecuteSqlAsync($"DBCC CHECKIDENT ('Addresses', RESEED, 0)");
    await context.Database.ExecuteSqlAsync($"DBCC CHECKIDENT ('BasketItems', RESEED, 0)");
    await context.Database.ExecuteSqlAsync($"DBCC CHECKIDENT ('Baskets', RESEED, 0)");
    await context.Database.ExecuteSqlAsync($"DBCC CHECKIDENT ('Customers', RESEED, 0)");
    await context.Database.ExecuteSqlAsync($"DBCC CHECKIDENT ('OrderItems', RESEED, 0)");
    await context.Database.ExecuteSqlAsync($"DBCC CHECKIDENT ('Orders', RESEED, 0)");
    await context.Database.ExecuteSqlAsync($"DBCC CHECKIDENT ('Products', RESEED, 0)");
}