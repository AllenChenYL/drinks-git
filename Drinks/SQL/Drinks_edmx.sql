USE Drinks;
GO
CREATE TABLE [dbo].[OrderDetail] (
    [ID] int IDENTITY(1,1) NOT NULL,
	[ORDER_ID] int NOT NULL,
    [CREATE_ID] nvarchar(128)  NOT NULL,
	[NAME] nvarchar(50)  NOT NULL,
    [SIZE] nvarchar(10)  NOT NULL,
	[SUGAR_LEVEL] nvarchar(10) NOT NULL,
    [ICE_LEVEL] nvarchar(10)  NOT NULL,
    [QUANTITY] int  NOT NULL,
    [MEMO] nvarchar(100) NULL
);
GO

CREATE TABLE [dbo].[Order](
	[ID] int IDENTITY(1,1) NOT NULL,
    [CREATE_ID] nvarchar(128)  NOT NULL,
    [STORE_NAME] nvarchar(50)  NOT NULL,
	[STORE_PHONE] nvarchar(20)  NOT NULL,
	[STORE_ADDRESS] nvarchar(100)  NOT NULL,
	[DEFAULT_IMAGE_ID] nvarchar(max)  NOT NULL,
	[NOTE] nvarchar(100) NULL,
	[CREATE_DATE] datetime  NOT NULL,
    [END_DATE] datetime  NOT NULL
);

CREATE TABLE [dbo].[Store](
	[ID] int IDENTITY(1,1) NOT NULL,
    [NAME] nvarchar(50)  NOT NULL,
	[PHONE] nvarchar(20)  NOT NULL,
	[ADDRESS] nvarchar(100)  NOT NULL,
	[DEFAULT_IMAGE_ID] nvarchar(max)  NOT NULL,
	[NOTE] nvarchar(100) NULL
);

ALTER TABLE [dbo].[Order]
ADD CONSTRAINT [PK_Order]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

ALTER TABLE [dbo].[OrderDetail]
ADD CONSTRAINT [PK_OrderDetail]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

ALTER TABLE [dbo].[Store]
ADD CONSTRAINT [PK_Store]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO
-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [OrderId] in table 'OrderDetails'
ALTER TABLE [dbo].[OrderDetail]
ADD CONSTRAINT [FK_OrderOrderDetail]
    FOREIGN KEY ([ORDER_ID])
    REFERENCES [dbo].[Order]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_OrderOrderDetail'
CREATE INDEX [IX_FK_OrderOrderDetail]
ON [dbo].[OrderDetail]
    ([ORDER_ID]);
GO

/* Delete FK Key
select * From INFORMATION_SCHEMA.KEY_COLUMN_USAGE

ALTER TABLE dbo.OrderDetails
DROP CONSTRAINT FK_OrderOrderDetail;

*/

/* Find table index

select * from sysindexes
select * from sysindexkeys

*/