﻿CREATE TABLE [dbo].[Products]
(
	[ProductId] BIGINT NOT NULL PRIMARY KEY IDENTITY, 
    [Name] NVARCHAR(50) NOT NULL, 
    [UrlFriendlyName] NVARCHAR(50) NOT NULL,
    [Price] DECIMAL NOT NULL
)
