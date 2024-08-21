CREATE TABLE [dbo].[Customers]
(
	[CustomerId] BIGINT NOT NULL PRIMARY KEY IDENTITY, 
    [FirstName] NVARCHAR(50) NOT NULL, 
    [LastName] NVARCHAR(50) NOT NULL, 
    [EmailAddress] NVARCHAR(50) NOT NULL, 
    [BillingAddressId] BIGINT NOT NULL, 
    [DefaultShippingAddressId] BIGINT NOT NULL, 
    CONSTRAINT [FK_Customer_BillingAddress] FOREIGN KEY ([BillingAddressId]) REFERENCES [Addresses]([AddressId]),
    CONSTRAINT [FK_Customer_DefaultShippingAddress] FOREIGN KEY (DefaultShippingAddressId) REFERENCES [Addresses]([AddressId])
)
