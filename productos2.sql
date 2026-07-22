/*
    Recreación completa de la base Productos2.
    ATENCIÓN: este script elimina la base y todos sus datos existentes.
*/
USE [master];
GO

IF DB_ID(N'Productos2') IS NOT NULL
BEGIN
    ALTER DATABASE [Productos2] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE [Productos2];
END
GO

CREATE DATABASE [Productos2];
GO

USE [Productos2];
GO

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

CREATE TABLE dbo.Proveedores
(
    Id              INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Proveedores PRIMARY KEY,
    RazonSocial     NVARCHAR(150) NOT NULL,
    Cuit            NVARCHAR(20) NULL,
    Telefono        NVARCHAR(30) NULL,
    Email           NVARCHAR(150) NULL,
    Direccion       NVARCHAR(200) NULL,
    FechaAlta       DATETIME2 NOT NULL CONSTRAINT DF_Proveedores_FechaAlta DEFAULT SYSUTCDATETIME(),
    Activo          BIT NOT NULL CONSTRAINT DF_Proveedores_Activo DEFAULT 1
);
GO

CREATE TABLE dbo.Clientes
(
    IdCliente       INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Clientes PRIMARY KEY,
    RazonSocial     NVARCHAR(150) NOT NULL,
    Celular         NVARCHAR(30) NULL,
    DniCuit         NVARCHAR(20) NULL,
    Email           NVARCHAR(150) NULL,
    Direccion       NVARCHAR(200) NULL,
    Localidad       NVARCHAR(120) NULL,
    Observaciones   NVARCHAR(200) NULL,
    CondicionIva    INT NOT NULL,
    DescuentoPorc   DECIMAL(5,2) NOT NULL CONSTRAINT DF_Clientes_Descuento DEFAULT 0,
    Saldo           DECIMAL(18,2) NOT NULL CONSTRAINT DF_Clientes_Saldo DEFAULT 0,
    CreditoLimite   DECIMAL(18,2) NOT NULL CONSTRAINT DF_Clientes_CreditoLimite DEFAULT 0,
    Activo          BIT NOT NULL CONSTRAINT DF_Clientes_Activo DEFAULT 1,
    CreatedAt       DATETIME2 NOT NULL CONSTRAINT DF_Clientes_CreatedAt DEFAULT SYSUTCDATETIME(),
    UpdatedAt       DATETIME2 NOT NULL CONSTRAINT DF_Clientes_UpdatedAt DEFAULT SYSUTCDATETIME()
);
GO

CREATE UNIQUE INDEX UX_Clientes_DniCuit
    ON dbo.Clientes(DniCuit)
    WHERE DniCuit IS NOT NULL;
GO

CREATE TABLE dbo.Productos
(
    Id                  INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Productos PRIMARY KEY,
    Nombre              NVARCHAR(150) NOT NULL,
    Familia             NVARCHAR(MAX) NULL,
    Tipo                INT NOT NULL,
    PrecioCompra        DECIMAL(18,2) NULL,
    PrecioVenta         DECIMAL(18,2) NOT NULL,
    Stock               DECIMAL(18,2) NOT NULL CONSTRAINT DF_Productos_Stock DEFAULT 0,
    StockMinimo         DECIMAL(18,2) NOT NULL CONSTRAINT DF_Productos_StockMinimo DEFAULT 0,
    Activo              BIT NOT NULL CONSTRAINT DF_Productos_Activo DEFAULT 1,
    FechaActualizacion  DATETIME2 NOT NULL CONSTRAINT DF_Productos_FechaActualizacion DEFAULT SYSUTCDATETIME(),
    IdProveedor         INT NULL,
    CONSTRAINT FK_Productos_Proveedores
        FOREIGN KEY (IdProveedor) REFERENCES dbo.Proveedores(Id)
);
GO

CREATE INDEX IX_Productos_IdProveedor ON dbo.Productos(IdProveedor);
GO

CREATE TABLE dbo.Ventas
(
    Id                  INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Ventas PRIMARY KEY,
    Fecha               DATETIME2 NOT NULL CONSTRAINT DF_Ventas_Fecha DEFAULT SYSUTCDATETIME(),
    TipoComprobante     INT NOT NULL,
    Total               DECIMAL(18,2) NOT NULL,
    Activo              BIT NOT NULL CONSTRAINT DF_Ventas_Activo DEFAULT 1,
    IdCliente           INT NOT NULL,
    CONSTRAINT FK_Ventas_Clientes
        FOREIGN KEY (IdCliente) REFERENCES dbo.Clientes(IdCliente)
);
GO

CREATE INDEX IX_Ventas_IdCliente ON dbo.Ventas(IdCliente);
GO

CREATE TABLE dbo.VentaDetalles
(
    Id                  INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_VentaDetalles PRIMARY KEY,
    IdVenta             INT NOT NULL,
    IdProducto          INT NOT NULL,
    Cantidad            DECIMAL(18,2) NOT NULL,
    PrecioUnitario      DECIMAL(18,2) NOT NULL,
    Subtotal            DECIMAL(18,2) NOT NULL,
    CONSTRAINT FK_VentaDetalles_Ventas
        FOREIGN KEY (IdVenta) REFERENCES dbo.Ventas(Id),
    CONSTRAINT FK_VentaDetalles_Productos
        FOREIGN KEY (IdProducto) REFERENCES dbo.Productos(Id)
);
GO

CREATE INDEX IX_VentaDetalles_IdVenta ON dbo.VentaDetalles(IdVenta);
CREATE INDEX IX_VentaDetalles_IdProducto ON dbo.VentaDetalles(IdProducto);
GO

CREATE TABLE dbo.VentaPagos
(
    Id                  INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_VentaPagos PRIMARY KEY,
    VentaId             INT NOT NULL,
    MedioPago           INT NOT NULL,
    Importe             DECIMAL(18,2) NOT NULL,
    CONSTRAINT FK_VentaPagos_Ventas
        FOREIGN KEY (VentaId) REFERENCES dbo.Ventas(Id)
);
GO

CREATE INDEX IX_VentaPagos_VentaId ON dbo.VentaPagos(VentaId);
GO
