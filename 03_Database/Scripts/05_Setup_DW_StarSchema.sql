
GO
USE Ventas_DW;

-- 1. Dimensión Cliente
CREATE TABLE DimCustomer (
    CustomerKey INT IDENTITY(1,1) PRIMARY KEY, -- Clave subrogada (Surrogate Key)
    CustomerID INT, -- ID original de la OLTP
    FullName NVARCHAR(200),
    City NVARCHAR(100),
    Country NVARCHAR(100)
);

-- 2. Dimensión Producto
CREATE TABLE DimProduct (
    ProductKey INT IDENTITY(1,1) PRIMARY KEY,
    ProductID INT,
    ProductName NVARCHAR(200),
    CategoryName NVARCHAR(100)
);

-- 3. Dimensión Tiempo (Vital para KPIs de Mes/Año)
CREATE TABLE DimDate (
    DateKey INT PRIMARY KEY, -- Formato YYYYMMDD
    FullDate DATE,
    Day INT,
    Month INT,
    MonthName NVARCHAR(20),
    Quarter INT,
    Year INT
);

-- 4. TABLA DE HECHOS (FactSales)
CREATE TABLE FactSales (
    SalesKey INT IDENTITY(1,1) PRIMARY KEY,
    DateKey INT NOT NULL,
    CustomerKey INT NOT NULL,
    ProductKey INT NOT NULL,
    OrderID INT, -- Para trazabilidad
    Quantity INT,
    UnitPrice DECIMAL(18,2),
    TotalLine DECIMAL(18,2), -- El KPI base
    
    -- Relaciones (FK)
    FOREIGN KEY (DateKey) REFERENCES DimDate(DateKey),
    FOREIGN KEY (CustomerKey) REFERENCES DimCustomer(CustomerKey),
    FOREIGN KEY (ProductKey) REFERENCES DimProduct(ProductKey)
);
