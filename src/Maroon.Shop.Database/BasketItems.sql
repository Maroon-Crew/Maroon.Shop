CREATE TABLE [dbo].[BasketItems]
(
	[BasketItemId] BIGINT NOT NULL PRIMARY KEY IDENTITY, 
    [BasketId] BIGINT NOT NULL, 
    [ProductId] BIGINT NOT NULL, 
    [Quantity] INT NOT NULL, 
    [UnitPrice] MONEY NOT NULL, 
    [TotalPrice] MONEY NOT NULL, 
    CONSTRAINT [FK_BasketItem_Basket] FOREIGN KEY ([BasketId]) REFERENCES [Baskets]([BasketId]) ON DELETE CASCADE, 
    CONSTRAINT [FK_BasketItem_Product] FOREIGN KEY ([ProductId]) REFERENCES [Products]([ProductId])
)
