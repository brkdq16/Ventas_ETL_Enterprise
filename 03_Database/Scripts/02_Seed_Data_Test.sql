USE Ventas_OLTP;
GO

-- 1. Insertar Categoría y Fuente de Datos (Nivel 1)
INSERT INTO Categories (CategoryName) VALUES ('Laptops');
INSERT INTO DataSources (SourceType) VALUES ('CSV_Source_Test');

-- 2. Insertar Cliente (Nivel 1)
INSERT INTO Customers (FirstName, LastName, Email, City, Country) 
VALUES ('Juan', 'Perez', 'juan.perez@test.com', 'Santo Domingo', 'RD');

-- 3. Insertar Producto (Nivel 2 - Depende de CategoryID 1)
INSERT INTO Products (ProductName, CategoryID, Price, Stock) 
VALUES ('Legion 5 Pro', 1, 1200.00, 50);

-- 4. Insertar Orden (Nivel 2 - Depende de CustomerID 1 y SourceID 1)
INSERT INTO Orders (OrderDate, Status, CustomerID, SourceID) 
VALUES (GETDATE(), 'Completado', 1, 1);

-- 5. Insertar Detalle (Nivel 3 - Depende de OrderID 1 y ProductID 1)
-- Aquí SQL calculará automáticamente el LineTotal (2 * 1200 = 2400)
INSERT INTO OrderDetails (OrderID, ProductID, Quantity, UnitPrice) 
VALUES (1, 1, 2, 1200.00);

-- 6. Verificación Final
SELECT * FROM OrderDetails;
GO
