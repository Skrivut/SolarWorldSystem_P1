-- SolarWorldERP: run this whole script in SSMS (connected to your SQL Server instance). Safe to re-run.
IF DB_ID('SolarWorldERP') IS NULL CREATE DATABASE SolarWorldERP;
GO
USE SolarWorldERP;
GO

-- ===== Core tables =====
IF OBJECT_ID('dbo.Users') IS NULL
CREATE TABLE dbo.Users (
    UserId       INT IDENTITY PRIMARY KEY,
    UserName     VARCHAR(50)  NOT NULL UNIQUE,
    FullName     VARCHAR(100) NOT NULL,
    PasswordHash VARCHAR(64)  NOT NULL,            -- SHA-256 hex
    Role         VARCHAR(20)  NOT NULL CHECK (Role IN ('Staff','Owner','Driver')),
    IsActive     BIT NOT NULL DEFAULT 1
);

IF OBJECT_ID('dbo.Items') IS NULL
CREATE TABLE dbo.Items (
    ItemId        INT IDENTITY PRIMARY KEY,
    ItemCode      VARCHAR(30)  NOT NULL UNIQUE,
    ItemName      VARCHAR(150) NOT NULL,
    Unit          VARCHAR(20)  NOT NULL,
    StockTag      VARCHAR(10)  NOT NULL CHECK (StockTag IN ('Project','ForSale')),
    QtyOnHand     INT NOT NULL DEFAULT 0 CHECK (QtyOnHand >= 0),
    QtyReserved   INT NOT NULL DEFAULT 0 CHECK (QtyReserved >= 0),
    IsScarce      BIT NOT NULL DEFAULT 0,
    ReorderPoint  INT NOT NULL DEFAULT 0,
    MaxStockLevel INT NOT NULL DEFAULT 0
);

IF OBJECT_ID('dbo.Requisitions') IS NULL
CREATE TABLE dbo.Requisitions (
    RfId         INT IDENTITY PRIMARY KEY,
    RfNumber     VARCHAR(20)  NOT NULL UNIQUE,     -- e.g. RF-2026-0001
    RequestedBy  VARCHAR(100) NOT NULL,
    ProjectName  VARCHAR(150) NOT NULL,
    StockTag     VARCHAR(10)  NOT NULL,
    Status       VARCHAR(30)  NOT NULL,            -- Submitted, Approved, PendingFulfillment, Fulfilled, ReturnedForRevision, Cancelled
    DateCreated  DATETIME     NOT NULL DEFAULT GETDATE(),
    CreatedBy    VARCHAR(100) NULL,
    ApprovedBy   VARCHAR(100) NULL,
    DateApproved DATETIME     NULL,
    Remarks      VARCHAR(255) NULL
);

IF OBJECT_ID('dbo.RequisitionItems') IS NULL
CREATE TABLE dbo.RequisitionItems (
    RfItemId     INT IDENTITY PRIMARY KEY,
    RfId         INT NOT NULL REFERENCES dbo.Requisitions(RfId),
    ItemId       INT NOT NULL REFERENCES dbo.Items(ItemId),
    QtyRequested INT NOT NULL CHECK (QtyRequested > 0),
    QtyReserved  INT NOT NULL DEFAULT 0,
    QtyShortage  INT NOT NULL DEFAULT 0
);
GO

-- Sample users (passwords: staff123 / owner123 / driver123). Change before deployment.
IF NOT EXISTS (SELECT 1 FROM dbo.Users)
INSERT INTO dbo.Users (UserName, FullName, PasswordHash, Role) VALUES
 ('staff1',  'Office Staff', CONVERT(VARCHAR(64), HASHBYTES('SHA2_256','staff123'), 2),  'Staff'),
 ('owner1',  'Owner',        CONVERT(VARCHAR(64), HASHBYTES('SHA2_256','owner123'), 2),  'Owner'),
 ('driver1', 'Driver',       CONVERT(VARCHAR(64), HASHBYTES('SHA2_256','driver123'), 2), 'Driver');

-- Sample items
IF NOT EXISTS (SELECT 1 FROM dbo.Items)
INSERT INTO dbo.Items (ItemCode, ItemName, Unit, StockTag, QtyOnHand, IsScarce, ReorderPoint, MaxStockLevel) VALUES
 ('SP-550',   'Solar Panel 550W',          'pcs',   'Project', 40,  1, 20, 100),
 ('INV-5K',   'Hybrid Inverter 5kW',       'pcs',   'Project', 6,   1, 3,  15),
 ('MC4-CON',  'MC4 Connector Pair',        'pairs', 'Project', 200, 0, 50, 0),
 ('PV-CBL-6', 'PV Cable 6mm (per meter)',  'm',     'Project', 500, 0, 100, 0),
 ('MNT-RAIL', 'Mounting Rail 4.2m',        'pcs',   'Project', 30,  0, 10, 0),
 ('BRK-DC',   'DC Breaker 2P 32A',         'pcs',   'Project', 15,  0, 5,  0),
 ('SP-550-S', 'Solar Panel 550W (for sale)','pcs',  'ForSale', 10,  1, 5,  30);
GO

-- ===== Schema version tracking: future changes are appended as new numbered blocks =====
IF OBJECT_ID('dbo.SchemaVersion') IS NULL
CREATE TABLE dbo.SchemaVersion (
    Version     INT PRIMARY KEY,
    Description VARCHAR(200) NOT NULL,
    AppliedOn   DATETIME NOT NULL DEFAULT GETDATE()
);
GO

-- ===== Version 1: purchasing and delivery tables =====
IF OBJECT_ID('dbo.Suppliers') IS NULL
CREATE TABLE dbo.Suppliers (
    SupplierId   INT IDENTITY PRIMARY KEY,
    SupplierName VARCHAR(150) NOT NULL UNIQUE,
    ContactInfo  VARCHAR(255) NULL,
    IsActive     BIT NOT NULL DEFAULT 1
);

IF OBJECT_ID('dbo.PurchaseOrders') IS NULL
CREATE TABLE dbo.PurchaseOrders (
    PoId         INT IDENTITY PRIMARY KEY,
    PoNumber     VARCHAR(20) NOT NULL UNIQUE,      -- e.g. PO-2026-0001
    SupplierId   INT NOT NULL REFERENCES dbo.Suppliers(SupplierId),
    RfId         INT NULL REFERENCES dbo.Requisitions(RfId),  -- set when ordering to cover an RF shortage
    Status       VARCHAR(30) NOT NULL DEFAULT 'Draft',        -- no CHECK, so new statuses need no schema change
    DateCreated  DATETIME NOT NULL DEFAULT GETDATE(),
    CreatedBy    VARCHAR(100) NULL,
    ExpectedDate DATE NULL,
    Remarks      VARCHAR(255) NULL
);

IF OBJECT_ID('dbo.PurchaseOrderItems') IS NULL AND OBJECT_ID('dbo.PurchaseOrderLines') IS NULL
CREATE TABLE dbo.PurchaseOrderItems (
    PoItemId    INT IDENTITY PRIMARY KEY,
    PoId        INT NOT NULL REFERENCES dbo.PurchaseOrders(PoId),
    ItemId      INT NOT NULL REFERENCES dbo.Items(ItemId),
    QtyOrdered  INT NOT NULL CHECK (QtyOrdered > 0),
    QtyReceived INT NOT NULL DEFAULT 0 CHECK (QtyReceived >= 0),
    UnitCost    DECIMAL(12,2) NULL
);

IF OBJECT_ID('dbo.Deliveries') IS NULL
CREATE TABLE dbo.Deliveries (
    DeliveryId      INT IDENTITY PRIMARY KEY,
    DeliveryNumber  VARCHAR(20) NOT NULL UNIQUE,
    RfId            INT NOT NULL REFERENCES dbo.Requisitions(RfId),
    DriverUserId    INT NULL REFERENCES dbo.Users(UserId),
    Status          VARCHAR(30) NOT NULL DEFAULT 'Scheduled',
    DateScheduled   DATETIME NULL,
    DateDelivered   DATETIME NULL,
    ReceivedBy      VARCHAR(100) NULL,
    Remarks         VARCHAR(255) NULL
);
GO

IF NOT EXISTS (SELECT 1 FROM dbo.SchemaVersion WHERE Version = 1)
INSERT INTO dbo.SchemaVersion (Version, Description) VALUES (1, 'Purchasing and delivery tables');
GO

-- ===== Version 2: align purchasing/delivery tables with DeliveryService =====
IF OBJECT_ID('dbo.PurchaseOrderItems') IS NOT NULL AND OBJECT_ID('dbo.PurchaseOrderLines') IS NULL
    EXEC sp_rename 'dbo.PurchaseOrderItems', 'PurchaseOrderLines';
IF COL_LENGTH('dbo.PurchaseOrders', 'PoId') IS NOT NULL
    EXEC sp_rename 'dbo.PurchaseOrders.PoId', 'PurchaseOrderId', 'COLUMN';
IF COL_LENGTH('dbo.PurchaseOrderLines', 'PoItemId') IS NOT NULL
    EXEC sp_rename 'dbo.PurchaseOrderLines.PoItemId', 'PurchaseOrderLineId', 'COLUMN';
IF COL_LENGTH('dbo.PurchaseOrderLines', 'PoId') IS NOT NULL
    EXEC sp_rename 'dbo.PurchaseOrderLines.PoId', 'PurchaseOrderId', 'COLUMN';
GO

-- Deliveries now hang off a purchase order. Rebuilt only if it is still empty.
IF COL_LENGTH('dbo.Deliveries', 'RfId') IS NOT NULL
BEGIN
    IF EXISTS (SELECT 1 FROM dbo.Deliveries)
        THROW 50001, 'Deliveries has rows in the old layout. Migrate them manually first.', 1;
    DROP TABLE dbo.Deliveries;
END

IF OBJECT_ID('dbo.Deliveries') IS NULL
CREATE TABLE dbo.Deliveries (
    DeliveryId      INT IDENTITY PRIMARY KEY,
    DeliveryNumber  VARCHAR(20)  NOT NULL UNIQUE,   -- DEL-2026-0001
    PurchaseOrderId INT          NOT NULL REFERENCES dbo.PurchaseOrders(PurchaseOrderId),
    DeliveryAddress VARCHAR(255) NOT NULL,
    DriverName      VARCHAR(100) NULL,
    Status          VARCHAR(30)  NOT NULL DEFAULT 'Scheduled',  -- Scheduled, InTransit, Delivered
    ExpectedDate    DATETIME     NULL,
    DeliveredDate   DATETIME     NULL,
    Notes           VARCHAR(255) NULL
);
GO

IF OBJECT_ID('dbo.DeliveryLines') IS NULL
CREATE TABLE dbo.DeliveryLines (
    DeliveryLineId      INT IDENTITY PRIMARY KEY,
    DeliveryId          INT NOT NULL REFERENCES dbo.Deliveries(DeliveryId),
    PurchaseOrderLineId INT NOT NULL REFERENCES dbo.PurchaseOrderLines(PurchaseOrderLineId),
    QtyDelivered        INT NOT NULL CHECK (QtyDelivered > 0)
);
GO

-- PurchaseOrders.Status values used by the service: Ordered, PartiallyReceived, Completed.
IF NOT EXISTS (SELECT 1 FROM dbo.SchemaVersion WHERE Version = 2)
INSERT INTO dbo.SchemaVersion (Version, Description) VALUES (2, 'Align purchasing and delivery tables with DeliveryService');
GO

-- ===== Version 4: purchasing flow (PO approval, supplier pickup tasks, receiving report) =====
-- (Version 3 was the one-line "UnitCost optional" fix; it is folded into the guarded step below.)
IF COL_LENGTH('dbo.Suppliers', 'Address') IS NULL
    EXEC('ALTER TABLE dbo.Suppliers ADD Address VARCHAR(255) NULL');   -- where the driver collects the goods

IF COL_LENGTH('dbo.PurchaseOrders', 'ApprovedBy') IS NULL
    EXEC('ALTER TABLE dbo.PurchaseOrders ADD ApprovedBy VARCHAR(100) NULL, DateApproved DATETIME NULL, DateClosed DATETIME NULL');

IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.PurchaseOrderLines') AND name = 'UnitCost' AND is_nullable = 0)
    EXEC('ALTER TABLE dbo.PurchaseOrderLines ALTER COLUMN UnitCost DECIMAL(12,2) NULL');

-- A Deliveries row is now the driver's pickup task: Pending, PickedUp, Delivered (to the office), Received (RR done), Cancelled.
IF COL_LENGTH('dbo.Deliveries', 'PickupAddress') IS NULL
    EXEC('ALTER TABLE dbo.Deliveries ADD PickupAddress VARCHAR(255) NULL, SupplierDrNumber VARCHAR(50) NULL, PickedUpDate DATETIME NULL, ReportNumber VARCHAR(20) NULL, ReceivedBy VARCHAR(100) NULL, ReceivedDate DATETIME NULL');

-- DeliveryLines now hold the receiving report (RR): accepted and rejected quantity per item.
IF COL_LENGTH('dbo.DeliveryLines', 'QtyAccepted') IS NULL
    EXEC('ALTER TABLE dbo.DeliveryLines ADD QtyAccepted INT NOT NULL CONSTRAINT DF_DeliveryLines_Acc DEFAULT 0, QtyRejected INT NOT NULL CONSTRAINT DF_DeliveryLines_Rej DEFAULT 0, RejectReason VARCHAR(255) NULL, DiscrepancyAction VARCHAR(20) NULL');
GO

IF NOT EXISTS (SELECT 1 FROM dbo.SchemaVersion WHERE Version = 4)
BEGIN
    -- Carry over rows made before this version (a delivery used to be a scheduled drop-off).
    UPDATE dbo.DeliveryLines SET QtyAccepted = QtyDelivered;
    UPDATE dbo.Deliveries SET PickupAddress = DeliveryAddress WHERE PickupAddress IS NULL;
    UPDATE dbo.Deliveries SET Status = CASE Status WHEN 'Scheduled' THEN 'Pending' WHEN 'InTransit' THEN 'PickedUp' WHEN 'Delivered' THEN 'Received' ELSE Status END;
    INSERT INTO dbo.SchemaVersion (Version, Description) VALUES (4, 'Purchasing flow: PO approval, pickup tasks, receiving report');
END
GO

-- Quantity on order per item, for the Incoming column of the inventory screen.
CREATE OR ALTER VIEW dbo.vwIncomingStock AS
SELECT l.ItemId, SUM(l.QtyOrdered - l.QtyReceived) AS QtyIncoming
FROM dbo.PurchaseOrderLines l
JOIN dbo.PurchaseOrders o ON o.PurchaseOrderId = l.PurchaseOrderId
WHERE o.Status IN ('Ordered', 'PartiallyReceived')
GROUP BY l.ItemId;
GO

/* ---- TEMPLATE for future changes: append a new block like this, never edit the blocks above ----
IF NOT EXISTS (SELECT 1 FROM dbo.SchemaVersion WHERE Version = 5)
BEGIN
    IF COL_LENGTH('dbo.Deliveries', 'VehiclePlate') IS NULL
        EXEC('ALTER TABLE dbo.Deliveries ADD VehiclePlate VARCHAR(20) NULL');
    INSERT INTO dbo.SchemaVersion (Version, Description) VALUES (5, 'Add vehicle plate to deliveries');
END
GO
-------------------------------------------------------------------------------------------------- */