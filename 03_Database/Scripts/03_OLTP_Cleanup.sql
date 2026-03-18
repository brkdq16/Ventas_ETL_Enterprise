USE Ventas_OLTP;
GO

-- 1. Borrar registros en orden inverso a la jerarquía
DELETE FROM OrderDetails;
DELETE FROM Orders;
DELETE FROM Products;
DELETE FROM Categories;
DELETE FROM Customers;
DELETE FROM DataSources;

-- 2. Reiniciar los contadores IDENTITY a 0
DBCC CHECKIDENT ('OrderDetails', RESEED, 0);
DBCC CHECKIDENT ('Orders', RESEED, 0);
DBCC CHECKIDENT ('Products', RESEED, 0);
DBCC CHECKIDENT ('Categories', RESEED, 0);
DBCC CHECKIDENT ('Customers', RESEED, 0);
DBCC CHECKIDENT ('DataSources', RESEED, 0);

PRINT 'Base de datos Ventas_OLTP limpia y lista para carga real.';
GO
