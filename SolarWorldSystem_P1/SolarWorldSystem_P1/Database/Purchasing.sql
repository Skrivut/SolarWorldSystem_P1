USE SolarWorldERP;
GO

IF OBJECT_ID('dbo.PurchaseOrders', 'U') IS NOT NULL AND OBJECT_ID('dbo.PurchaseOrder', 'U') IS NULL
	EXEC sp_rename 'dbo.PurchaseOrders', 'PurchaseOrder';
GO

IF OBJECT_ID('dbo.PurchaseOrder', 'U') IS NULL
BEGIN
	CREATE TABLE dbo.PurchaseOrder
	(
		PurchaseOrderId INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
		PONumber VARCHAR(30) NOT NULL UNIQUE,
		SupplierName VARCHAR(150) NOT NULL,
		RequestedBy VARCHAR(100) NOT NULL,
		Status VARCHAR(30) NOT NULL CONSTRAINT DF_PurchaseOrders_Status DEFAULT 'Draft',
		DateCreated DATETIME NOT NULL CONSTRAINT DF_PurchaseOrders_DateCreated DEFAULT GETDATE(),
		DateApproved DATETIME NULL,
		DateOrdered DATETIME NULL,
		TotalAmount DECIMAL(18,2) NOT NULL CONSTRAINT DF_PurchaseOrders_TotalAmount DEFAULT 0,
		CONSTRAINT CK_PurchaseOrder_Status CHECK (Status IN ('Draft','Approved','Ordered','PartiallyReceived','Completed','Cancelled')),
		CONSTRAINT CK_PurchaseOrder_TotalAmount CHECK (TotalAmount >= 0)
	);
END;
GO

IF OBJECT_ID('dbo.PurchaseOrderLines', 'U') IS NULL
BEGIN
	CREATE TABLE dbo.PurchaseOrderLines
	(
		PurchaseOrderLineId INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
		PurchaseOrderId INT NOT NULL,
		ItemId INT NOT NULL,
		QtyOrdered INT NOT NULL,
		QtyReceived INT NOT NULL CONSTRAINT DF_PurchaseOrderLines_QtyReceived DEFAULT 0,
		UnitCost DECIMAL(18,2) NOT NULL,
		CONSTRAINT FK_PurchaseOrderLines_Order FOREIGN KEY (PurchaseOrderId)
			REFERENCES dbo.PurchaseOrder(PurchaseOrderId),
		CONSTRAINT FK_PurchaseOrderLines_Item FOREIGN KEY (ItemId)
			REFERENCES dbo.Items(ItemId),
		CONSTRAINT CK_PurchaseOrderLines_QtyOrdered CHECK (QtyOrdered > 0),
		CONSTRAINT CK_PurchaseOrderLines_QtyReceived CHECK (QtyReceived >= 0 AND QtyReceived <= QtyOrdered),
		CONSTRAINT CK_PurchaseOrderLines_UnitCost CHECK (UnitCost >= 0)
	);
END;
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_PurchaseOrder_Status' AND object_id = OBJECT_ID('dbo.PurchaseOrder'))
	CREATE INDEX IX_PurchaseOrder_Status ON dbo.PurchaseOrder(Status);
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_PurchaseOrderLines_Order' AND object_id = OBJECT_ID('dbo.PurchaseOrderLines'))
	CREATE INDEX IX_PurchaseOrderLines_Order ON dbo.PurchaseOrderLines(PurchaseOrderId);
GO