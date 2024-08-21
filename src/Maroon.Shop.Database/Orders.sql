CREATE TABLE [dbo].[Orders]
(
	[OrderId] BIGINT NOT NULL PRIMARY KEY IDENTITY, 
    [CustomerId] BIGINT NOT NULL, 
    [TotalPrice] MONEY NOT NULL DEFAULT 0, 
    [DateCreated] DATETIME NOT NULL DEFAULT GETDATE(), 
    [BillingAddressId] BIGINT NOT NULL, 
    [ShippingAddressId] BIGINT NOT NULL, 
    CONSTRAINT [FK_Order_Customer] FOREIGN KEY ([CustomerId]) REFERENCES [Customers]([CustomerId]) ON DELETE CASCADE,
    CONSTRAINT [FK_Order_BillingAddress] FOREIGN KEY ([BillingAddressId]) REFERENCES [Addresses]([AddressId]), 
    CONSTRAINT [FK_Order_DefaultShippingAddress] FOREIGN KEY (ShippingAddressId) REFERENCES [Addresses]([AddressId])
)
