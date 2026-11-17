-- SolarWorldERP: run this whole script in SSMS (connected to your SQL Server instance).
IF DB_ID('SolarWorldERP') IS NULL CREATE DATABASE SolarWorldERP;
GO
USE SolarWorldERP;
GO

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
