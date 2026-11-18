USE SolarWorldERP;
GO

IF OBJECT_ID('dbo.Deliveries', 'U') IS NOT NULL AND OBJECT_ID('dbo.Delivery', 'U') IS NULL
	EXEC sp_rename 'dbo.Deliveries', 'Delivery';
GO

IF OBJECT_ID('dbo.Delivery', 'U') IS NULL
BEGIN
	CREATE TABLE dbo.Delivery
	(
		DeliveryId INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
		DeliveryNumber VARCHAR(30) NOT NULL UNIQUE,
		PurchaseOrderId INT NOT NULL,
		DeliveryAddress VARCHAR(255) NOT NULL,
		DriverName VARCHAR(100) NULL,
		Status VARCHAR(20) NOT NULL CONSTRAINT DF_Deliveries_Status DEFAULT 'Scheduled',
		ExpectedDate DATETIME NULL,
		DeliveredDate DATETIME NULL,
		Notes VARCHAR(500) NULL,
		CONSTRAINT FK_Delivery_PurchaseOrder FOREIGN KEY (PurchaseOrderId)
			REFERENCES dbo.PurchaseOrder(PurchaseOrderId),
		CONSTRAINT CK_Delivery_Status CHECK (Status IN ('Scheduled','InTransit','Delivered','Cancelled'))
	);
END;
GO

IF OBJECT_ID('dbo.DeliveryLines', 'U') IS NULL
BEGIN
	CREATE TABLE dbo.DeliveryLines
	(
		DeliveryLineId INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
		DeliveryId INT NOT NULL,
		PurchaseOrderLineId INT NOT NULL,
		QtyDelivered INT NOT NULL,
		CONSTRAINT FK_DeliveryLines_Delivery FOREIGN KEY (DeliveryId)
			REFERENCES dbo.Deliveries(DeliveryId),
		CONSTRAINT FK_DeliveryLines_PurchaseLine FOREIGN KEY (PurchaseOrderLineId)
			REFERENCES dbo.PurchaseOrderLines(PurchaseOrderLineId),
		CONSTRAINT CK_DeliveryLines_QtyDelivered CHECK (QtyDelivered > 0)
	);
END;
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Delivery_Status' AND object_id = OBJECT_ID('dbo.Delivery'))
	CREATE INDEX IX_Delivery_Status ON dbo.Delivery(Status);
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Delivery_PurchaseOrder' AND object_id = OBJECT_ID('dbo.Delivery'))
	CREATE INDEX IX_Delivery_PurchaseOrder ON dbo.Delivery(PurchaseOrderId);
GO