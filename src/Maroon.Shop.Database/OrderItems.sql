CREATE TABLE [dbo].[OrderItems]
(
	[OrderItemId] BIGINT NOT NULL PRIMARY KEY IDENTITY,
    [OrderId] BIGINT NOT NULL, 
    [ProductId] BIGINT NOT NULL, 
    [Quantity] INT NOT NULL, 
    [UnitPrice] MONEY NOT NULL, 
    [TotalPrice] MONEY NOT NULL, 
    CONSTRAINT [FK_OrderItem_Order] FOREIGN KEY ([OrderId]) REFERENCES [Orders]([OrderId]) ON DELETE CASCADE, 
    CONSTRAINT [FK_OrderItem_Product] FOREIGN KEY ([ProductId]) REFERENCES [Products]([ProductId])
)
