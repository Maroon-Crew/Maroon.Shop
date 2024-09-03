CREATE TABLE [dbo].[Products]
(
	[ProductId] BIGINT NOT NULL PRIMARY KEY IDENTITY, 
    [Name] NVARCHAR(50) NOT NULL, 
    [UrlFriendlyName] NVARCHAR(50) NOT NULL,
    [Price] MONEY NOT NULL, 
    [Description] NVARCHAR(MAX) NOT NULL, 
    [PleaseNote] NVARCHAR(MAX) NULL, 
    [ImageUrl] NVARCHAR(512) NOT NULL
)
