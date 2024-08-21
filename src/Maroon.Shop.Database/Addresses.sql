CREATE TABLE [dbo].[Addresses]
(
	[AddressId] BIGINT NOT NULL PRIMARY KEY IDENTITY, 
    [NameOfRecipient] NVARCHAR(50) NOT NULL, 
    [Line1] NVARCHAR(50) NOT NULL, 
    [Line2] NVARCHAR(50) NULL, 
    [Town] NVARCHAR(50) NULL, 
    [County] NVARCHAR(50) NULL, 
    [PostCode] NVARCHAR(50) NOT NULL, 
    [Country] NVARCHAR(50) NULL
)
