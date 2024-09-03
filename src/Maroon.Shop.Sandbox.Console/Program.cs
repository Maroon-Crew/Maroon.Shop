using Maroon.Shop.Data;
using Microsoft.EntityFrameworkCore;

using (var context = new ShopContext())
{    
    await WipeDatabase(context);

    var product1 = new Product
    {
        Name = "Maroon t-shirt, men",
        UrlFriendlyName = "maroon-t-shirt-men",
        ImageUrl = "https://m.media-amazon.com/images/I/51c+T7XynwL._AC_SX569_.jpg",
        Price = 12.99m,
        Description = @"This is a men's t-shirt, to wear on those very special Wednesdays.

Specially designed and formulated by our team for optimum performance in the gym, on the toilet, or just for casual wear.

Made from 100% cotton, this t-shirt is soft, comfortable, and durable. It is machine washable and can be tumble dried.

Available in sizes small, medium, large, x-large, and xx-large.
",
        PleaseNote = "Please note that this t-shirt is not suitable for children under the age of 3 years.",
    };

    var product2 = new Product
    {
        Name = "Maroon t-shirt, women",
        UrlFriendlyName = "maroon-t-shirt-women",
        ImageUrl = "https://ljworkwear.co.uk/wp-content/uploads/2021/02/BA863F_BURGUNDY_FRONT.jpg",
        Price = 11.99m,
        Description = @"This is a women's t-shirt, to wear on those very special Femmesdays.

Specially designed and formulated by our team for optimum performance in the gym, in the salon, or just for casual wear.

Made from 100% cotton, this t-shirt is soft, comfortable, and durable. It is machine washable and can be tumble dried.

Available in sizes small, medium, large, x-large, and xx-large.
",
    };

    var product3 = new Product
    {
        Name = "Maroon shirt, button down collar",
        UrlFriendlyName = "maroon-shirt-button-down-collar",
        ImageUrl = "https://www.charlestyrwhitt.com/dw/image/v2/AAWJ_PRD/on/demandware.static/-/Sites-ctshirts-master/default/dw903880ff/hi-res/CSR2125CLK_MODEL_UNTUCKED.jpg?sw=960&sh=1200",
        Price = 20.99m,
        Description = @"Introducing our Maroon Button-Down Collar Shirt – a perfect blend of style and comfort. Crafted from premium, breathable cotton, this shirt is designed to keep you looking sharp and feeling comfortable all day long. The rich, deep maroon color adds a touch of sophistication to any outfit.

The button-down collar provides a polished look, making it suitable for both casual and formal occasions. Made from high-quality cotton, it ensures durability and a soft, comfortable feel. Its versatile design makes it perfect for pairing with jeans for a casual look or dress trousers for a more formal ensemble.

Plus, it’s machine washable and designed to maintain its color and shape after every wash. Whether you’re heading to the office, a dinner date, or a casual outing, this maroon button-down collar shirt is your go-to choice for effortless style and comfort. Upgrade your wardrobe with this timeless piece today!
",
    };

    var prodcut4 = new Product
    {
        Name = "Maroon shirt, tailored fit",
        UrlFriendlyName = "maroon-shirt-tailored-fit",
        ImageUrl = "https://img.ltwebstatic.com/images3_pi/2024/07/01/cb/1719820119df28a586ea313bb1b1a3bd9f9185ac0e_thumbnail_900x.webp",
        Price = 25.99m,
        Description = @"Introducing the Maroon Shirt, Tailored Fit – because who doesn’t want to look like a sophisticated cranberry? This shirt is the epitome of fashion-forward thinking, designed for those who believe that blending in is for amateurs.

Key Features:
Tailored Fit: Because nothing says “I mean business” like a shirt that hugs you tighter than your last relationship.
Maroon Color: A bold choice that screams, “I’m here to make a statement, and that statement is maroon.”
Premium Fabric: Made from the finest materials that money can buy, assuming you don’t have much money.
Versatile Design: Perfect for those who want to transition seamlessly from a board meeting to a poetry slam.
Easy Care: Machine washable, because who has time for dry cleaning when you’re busy being this stylish?

Wear it to your next important meeting, casual Friday, or even your cousin’s wedding – because nothing says “I care” like showing up in a maroon shirt. Upgrade your wardrobe today and embrace the undeniable charm of looking like a well-dressed berry.",
    };

    var prodcut5 = new Product
    {
        Name = "Maroon shirt, men",
        UrlFriendlyName = "maroon-shirt-men",
        ImageUrl = "https://m.media-amazon.com/images/I/61KMBZJEDFL._AC_SY606_.jpg",
        Price = 19.99m,
        Description = @"Introducing the Maroon Shirt for Men – because who needs subtlety when you can scream sophistication in a color that’s one shade away from a traffic light?

The color is a bold maroon that says, ""I’m here to make a statement, even if that statement is just 'I own a maroon shirt.'"" The fabric is made from 100% ""I-hope-this-doesn’t-shrink-in-the-wash"" cotton. Perfect for those who enjoy living on the edge. The fit is tailored to fit like a glove... on someone else’s hand. But hey, who needs comfort when you have style? This shirt is versatile, ideal for any occasion where you want to be remembered as ""the guy in the maroon shirt."" Weddings, funerals, job interviews – you name it!

Why choose our Maroon Shirt? It’s attention-grabbing, perfect for those who believe that blending in is for chameleons and introverts. It’s a conversation starter, be prepared for comments like, ""Wow, that’s a bold choice!"" and ""Is that maroon or just a really dark red?"" It’s a confidence booster, wearing this shirt will make you feel like you can conquer the world – or at least the nearest coffee shop.

Care instructions: Hand wash with the tears of your fashion critics. Air dry while contemplating your life choices. Iron on low heat, or just embrace the wrinkles as a fashion statement.

Disclaimer: Side effects may include excessive compliments, unsolicited fashion advice, and the occasional double-take from strangers. We are not responsible for any sudden urges to buy matching maroon accessories or the realization that maroon might not be your color after all.

Embrace the bold, the daring, the maroon. Because life’s too short to wear boring colors.",
    };

    var prodcut6 = new Product
    {
        Name = "Maroon blazer",
        UrlFriendlyName = "maroon-blazer",
        ImageUrl = "https://m.media-amazon.com/images/I/61poQznp0PL._AC_SX466_.jpg",
        Price = 29.99m,
        Description = @"Introducing the Maroon Blazer – because why blend in when you can stand out in a color that’s one step away from a fire engine?

This blazer boasts a maroon hue that practically shouts, ""I’m here to make an impression, even if that impression is 'I own a maroon blazer.'"" The fabric is a luxurious blend of ""I-hope-this-doesn’t-wrinkle-too-much"" polyester and ""I-can’t-believe-it’s-not-silk"" rayon. Perfect for those who enjoy the thrill of high-maintenance fashion.

The fit is designed to hug your body in all the wrong places, ensuring you’ll never forget you’re wearing it. But who needs comfort when you can have style? This blazer is versatile enough for any event where you want to be remembered as ""the person in the maroon blazer."" Think weddings, board meetings, or even a casual stroll through the park.

Why choose our Maroon Blazer? It’s an attention magnet, ideal for those who believe that subtlety is for the faint-hearted. It’s a conversation piece, guaranteed to elicit remarks like, ""Wow, that’s a bold choice!"" and ""Is that maroon or just a really intense red?"" It’s a confidence amplifier, making you feel like you can conquer the world – or at least the nearest cocktail party.

Care instructions: Dry clean only, preferably by someone who understands the delicate balance of fashion and fabric. Store in a cool, dark place to avoid spontaneous combustion of style. Iron on low heat, or just embrace the creases as a testament to your adventurous spirit.

Disclaimer: Side effects may include excessive admiration, unsolicited fashion critiques, and the occasional double-take from passersby. We are not responsible for any sudden urges to buy matching maroon accessories or the realization that maroon might not be your color after all.

Embrace the bold, the daring, the maroon. Because life’s too short to wear boring blazers.",
    };

    var prodcut7 = new Product
    {
        Name = "Maroon gothic jacket",
        UrlFriendlyName = "maroon-gothic-jacket",
        ImageUrl = "https://litb-cgis.rightinthebox.com/images/1500x1500/202303/bps/product/inc/ufprqb1678875625713.jpg",
        Price = 35.99m,
        Description = @"Introducing the Maroon Gothic Jacket – because why settle for ordinary when you can embrace the dark, dramatic flair of maroon? This jacket is perfect for those who want to make a statement that says, ""I’m mysterious, stylish, and possibly a vampire.""

The color is a deep maroon that whispers secrets of the night and hints at a life lived on the edge. The fabric is a luxurious blend of ""I-hope-this-doesn’t-fade"" velvet and ""I-can’t-believe-it’s-not-leather"" faux leather. Ideal for those who enjoy the thrill of high-maintenance fashion with a touch of rebellion.

The fit is tailored to accentuate your brooding silhouette, ensuring you’ll look like you just stepped out of a gothic novel. Comfort is secondary to style, of course. This jacket is versatile enough for any event where you want to be remembered as ""the enigmatic figure in the maroon gothic jacket."" Think midnight gatherings, underground concerts, or even a casual stroll through a foggy cemetery.

Why choose our Maroon Gothic Jacket? It’s an attention magnet, perfect for those who believe that blending in is for the mundane. It’s a conversation piece, guaranteed to elicit remarks like, ""Wow, that’s a bold choice!"" and ""Is that maroon or just a really dark red?"" It’s a confidence amplifier, making you feel like you can conquer the world – or at least the nearest gothic club.

Care instructions: Dry clean only, preferably by someone who understands the delicate balance of fashion and fabric. Store in a cool, dark place to avoid spontaneous combustion of style. Iron on low heat, or just embrace the creases as a testament to your adventurous spirit.

Disclaimer: Side effects may include excessive admiration, unsolicited fashion critiques, and the occasional double-take from passersby. We are not responsible for any sudden urges to buy matching maroon accessories or the realization that maroon might not be your color after all.

Embrace the bold, the daring, the maroon. Because life’s too short to wear boring jackets.",
    };

    var prodcut8 = new Product
    {
        Name = "Maroon tie",
        UrlFriendlyName = "maroon-tie",
        ImageUrl = "https://www.jamesalexander.co.uk/images/twill_mens_clip_on_tie_burgundy_2-510x510.jpg",
        Price = 25.99m,
        Description = @"Introducing the maroon safety tie, the only tie that promises to keep you looking sharp while ensuring you survive your next DIY disaster. This isn't just any tie; it's a marvel of modern engineering, designed by the man who thought a jet-powered bicycle was a good idea. Perfect for those who want to add a touch of danger to their wardrobe, the maroon safety tie is your go-to accessory for both boardroom meetings and backyard explosions.

Crafted from a blend of high-tech materials and sheer audacity, this tie is built to withstand the most extreme conditions. Whether you're welding in your garage or just trying to survive another Monday at the office, the maroon safety tie has got you covered.

But wait, there's more! This tie isn't just about safety; it's also about style. With its sleek maroon finish, you'll be the envy of all your colleagues. And let's not forget the hidden compartment for storing small tools or snacks, because who doesn't need a mid-meeting pick-me-up?

In conclusion, the maroon safety tie is not just a fashion statement; it's a survival tool. So why settle for a boring, ordinary tie when you can have one that's unpredictable and exciting? Get yours today and be prepared for anything life throws at you, whether it's a surprise presentation or a spontaneous combustion.",
    };

    var prodcut9 = new Product
    {
        Name = "Maroon shirt, kids",
        UrlFriendlyName = "maroon-shirt-kids",
        ImageUrl = "https://m.media-amazon.com/images/I/51c+T7XynwL._AC_SX569_.jpg",
        Price = 9.99m,
        Description = @"It's like the men's shirt, but smaller.

It's never too early to get started in the gym.  Buy your kids one today, and they will never want to forget their kit again.

Kids are small, so it's even more imortant that they go down the gym and get big.",
        PleaseNote = "Please note that this t-shirt is not suitable for children under the age of 3 years.",
    };

    context.Products.AddRange(product1, product2, product3, prodcut4, prodcut5, prodcut6, prodcut7, prodcut8, prodcut9);

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
    await context.Database.ExecuteSqlAsync($"DBCC CHECKIDENT ('Addresses', RESEED, 1)");
    await context.Database.ExecuteSqlAsync($"DBCC CHECKIDENT ('BasketItems', RESEED, 1)");
    await context.Database.ExecuteSqlAsync($"DBCC CHECKIDENT ('Baskets', RESEED, 1)");
    await context.Database.ExecuteSqlAsync($"DBCC CHECKIDENT ('Customers', RESEED, 1)");
    await context.Database.ExecuteSqlAsync($"DBCC CHECKIDENT ('OrderItems', RESEED, 1)");
    await context.Database.ExecuteSqlAsync($"DBCC CHECKIDENT ('Orders', RESEED, 1)");
    await context.Database.ExecuteSqlAsync($"DBCC CHECKIDENT ('Products', RESEED, 1)");
}