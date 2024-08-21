CREATE TABLE [dbo].[Baskets]
(
	[BasketId] BIGINT NOT NULL PRIMARY KEY IDENTITY, 
    [CustomerId] BIGINT NOT NULL, 
    [TotalPrice] MONEY NOT NULL DEFAULT 0, 
    CONSTRAINT [FK_Basket_Customer] FOREIGN KEY ([CustomerId]) REFERENCES [Customers]([CustomerId]) ON DELETE CASCADE
)
