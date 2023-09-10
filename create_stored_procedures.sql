USE [Northwind]
GO

CREATE PROCEDURE [dbo].[InsertProduct]
	@productName nvarchar(40),
	@supplierId int,
	@categoryId int,
	@quantityPerUnit nvarchar(20),
	@unitPrice money,
	@unitsInStock smallint,
	@unitsOnOrder smallint,
	@reorderLevel smallint,
	@discontinued bit
AS
INSERT INTO dbo.Products
		(ProductName, SupplierID, CategoryID, QuantityPerUnit, UnitPrice, UnitsInStock, UnitsOnOrder, ReorderLevel, Discontinued) 
		OUTPUT Inserted.ProductID
VALUES (@productName, @supplierId, @categoryId, @quantityPerUnit, @unitPrice, @unitsInStock, @unitsOnOrder, @reorderLevel, @discontinued)
GO

CREATE PROCEDURE [dbo].[DeleteProduct]
	@productID int
AS
DELETE FROM dbo.Products 
    WHERE ProductID = @productID
SELECT @@ROWCOUNT
GO

CREATE PROCEDURE [dbo].[UpdateProduct]
	@productId int,
	@productName nvarchar(40),
	@supplierId int,
	@categoryId int,
	@quantityPerUnit nvarchar(20),
	@unitPrice money,
	@unitsInStock smallint,
	@unitsOnOrder smallint,
	@reorderLevel smallint,
	@discontinued bit
AS
UPDATE dbo.Products
SET ProductName = @productName,
	SupplierID = @supplierId, 
	CategoryID = @categoryId, 
	QuantityPerUnit = @quantityPerUnit, 
	UnitPrice = @unitPrice, 
	UnitsInStock = @unitsInStock, 
	UnitsOnOrder = @unitsOnOrder, 
	ReorderLevel = @reorderLevel, 
	Discontinued = @discontinued
WHERE ProductID = @productId
SELECT @@ROWCOUNT
GO

CREATE PROCEDURE [dbo].[FindProduct]
	@productId int
AS
SELECT p.ProductID, p.ProductName, p.SupplierID, p.CategoryID, p.QuantityPerUnit, p.UnitPrice, p.UnitsInStock, p.UnitsOnOrder, p.ReorderLevel, p.Discontinued 
FROM dbo.Products AS p
WHERE p.ProductID = @productId
GO

CREATE PROCEDURE [dbo].[SelectProducts]
	@offsetProducts int = 0,
	@limitProducts int = NULL
AS
SELECT p.ProductID, p.ProductName, p.SupplierID, p.CategoryID, p.QuantityPerUnit, p.UnitPrice, p.UnitsInStock, p.UnitsOnOrder, p.ReorderLevel, p.Discontinued 
FROM dbo.Products AS p
ORDER BY p.ProductID
OFFSET @offsetProducts ROWS
FETCH FIRST ISNULL(@limitProducts, (SELECT COUNT(*) FROM dbo.Products))  ROWS ONLY
GO

CREATE PROCEDURE [dbo].[SelectProductsByName]
	@productName nvarchar(40)
AS
SELECT p.ProductID, p.ProductName, p.SupplierID, p.CategoryID, p.QuantityPerUnit, p.UnitPrice,
        p.UnitsInStock, p.UnitsOnOrder, p.ReorderLevel, p.Discontinued 
FROM dbo.Products AS p
WHERE p.ProductName = @productName
ORDER BY p.ProductID
GO

CREATE PROCEDURE [dbo].[SelectProductsByCategory]
	@categoryId int
AS
SELECT p.ProductID, p.ProductName, p.SupplierID, p.CategoryID, p.QuantityPerUnit, p.UnitPrice,
        p.UnitsInStock, p.UnitsOnOrder, p.ReorderLevel, p.Discontinued
FROM dbo.Products AS p
WHERE p.CategoryID = @categoryId
GO

CREATE PROCEDURE [dbo].[SelectProductsByNames]
	@productNames xml
AS
SELECT p.ProductID, p.ProductName, p.SupplierID, p.CategoryID, p.QuantityPerUnit, p.UnitPrice,
			p.UnitsInStock, p.UnitsOnOrder, p.ReorderLevel, p.Discontinued 
FROM dbo.Products AS p
INNER JOIN 
		(SELECT record.value('.','nvarchar(40)') AS nameProduct
			FROM @productNames.nodes('/Names/name') AS x(record)) AS t
	ON p.ProductName = t.nameProduct
ORDER BY p.ProductID
GO

CREATE PROCEDURE [dbo].[SelectProductsByCategoryIds]
	@categoryIds xml
AS
SELECT p.ProductID, p.ProductName, p.SupplierID, p.CategoryID, p.QuantityPerUnit, p.UnitPrice,
        p.UnitsInStock, p.UnitsOnOrder, p.ReorderLevel, p.Discontinued 
FROM dbo.Products AS p
INNER JOIN 
		(SELECT record.value('.','int') AS id_category
			FROM @categoryIds.nodes('/CategoryIds/idCategory') AS x(record)) AS t
	ON p.CategoryID = t.id_category
GO

CREATE PROCEDURE [dbo].[InsertCategory]
	@categoryName nvarchar(15),
	@description ntext,
	@picture image
AS
INSERT INTO dbo.Categories 
		(CategoryName, Description, Picture) 
		OUTPUT Inserted.CategoryID
VALUES (@categoryName, @description, @picture)
GO

CREATE PROCEDURE [dbo].[DeleteCategory]
	@categoryID int
AS
DELETE FROM dbo.Categories 
	WHERE CategoryID = @categoryID
SELECT @@ROWCOUNT
GO

CREATE PROCEDURE [dbo].[UpdateCategory]
	@categoryId int,
	@categoryName nvarchar(15),
	@description ntext,
	@picture image
AS
UPDATE dbo.Categories 
SET CategoryName = @categoryName, 
	Description = @description, 
	Picture = @picture
WHERE CategoryID = @categoryId
SELECT @@ROWCOUNT
GO

CREATE PROCEDURE [dbo].[FindCategory]
	@categoryId int
AS
SELECT c.CategoryID, c.CategoryName, c.Description, c.Picture 
FROM dbo.Categories AS c
WHERE c.CategoryID = @categoryId
GO

CREATE PROCEDURE [dbo].[SelectCategories]
	@offsetCategories int = 0,
	@limitCategories int = NULL
AS
SELECT c.CategoryID, c.CategoryName, c.Description, c.Picture 
FROM dbo.Categories AS c
ORDER BY c.CategoryID
OFFSET @offsetCategories ROWS
FETCH FIRST ISNULL(@limitCategories, (SELECT COUNT(*) FROM dbo.Categories)) ROWS ONLY
GO

CREATE PROCEDURE [dbo].[SelectCategoriesByName]
	@categoryName nvarchar(15)
AS
SELECT c.CategoryID, c.CategoryName, c.Description, c.Picture 
FROM dbo.Categories AS c
WHERE c.CategoryName = @categoryName
ORDER BY c.CategoryID
GO

CREATE PROCEDURE [dbo].[SelectCategoriesByNames]
	@categoryNames xml
AS
SELECT c.CategoryID, c.CategoryName, c.Description, c.Picture 
FROM dbo.Categories AS c
INNER JOIN 
		(SELECT record.value('.','nvarchar(15)') AS nameCategory
			FROM @categoryNames.nodes('/Names/name') AS x(record)) AS t
	ON c.CategoryName = t.nameCategory
ORDER BY c.CategoryID
GO

CREATE PROCEDURE [dbo].[InsertEmployee]
	@lastName nvarchar(20),
	@firstName nvarchar(10),
	@title nvarchar(30),
	@titleOfCourtesy nvarchar(25),
	@birthDate datetime,
	@hireDate datetime,
	@address nvarchar(60),
	@city nvarchar(15),
	@region nvarchar(15),
	@postalCode nvarchar(10),
	@country nvarchar(15),
	@homePhone nvarchar(24),
	@extension nvarchar(4),
	@photo image,
	@notes ntext,
	@reportsTo int,
	@photoPath nvarchar(255)
AS
INSERT INTO dbo.Employees
		(LastName, FirstName, Title, TitleOfCourtesy, BirthDate, HireDate, Address, City, Region, PostalCode, Country, HomePhone, Extension, Photo, Notes, ReportsTo, PhotoPath)
		OUTPUT Inserted.EmployeeID
VALUES (@lastName, @firstName, @title, @titleOfCourtesy, @birthDate, @hireDate, @address, @city, @region, @postalCode, @country, @homePhone, @extension, @photo, @notes, @reportsTo, @photoPath)
GO

CREATE PROCEDURE [dbo].[DeleteEmployee]
	@employeeID int
AS
BEGIN
    BEGIN
        UPDATE dbo.Employees
        SET ReportsTo = NULL
        WHERE ReportsTo = @employeeId

        UPDATE dbo.Orders
        SET EmployeeID = NULL
        WHERE EmployeeID = @employeeId
        
        DELETE FROM dbo.EmployeeTerritories 
            WHERE EmployeeID = @employeeId
    END
    BEGIN
        DELETE FROM dbo.Employees 
            WHERE EmployeeID = @employeeId
        SELECT @@ROWCOUNT
    END
END
GO

CREATE PROCEDURE [dbo].[UpdateEmployee]
	@employeeId int,
	@lastName nvarchar(20),
	@firstName nvarchar(10),
	@title nvarchar(30),
	@titleOfCourtesy nvarchar(25),
	@birthDate datetime,
	@hireDate datetime,
	@address nvarchar(60),
	@city nvarchar(15),
	@region nvarchar(15),
	@postalCode nvarchar(10),
	@country nvarchar(15),
	@homePhone nvarchar(24),
	@extension nvarchar(4),
	@photo image,
	@notes ntext,
	@reportsTo int,
	@photoPath nvarchar(255)
AS
UPDATE dbo.Employees
SET LastName = @lastName,
	FirstName = @firstName, 
	Title = @title, 
	TitleOfCourtesy = @titleOfCourtesy, 
	BirthDate = @birthDate, 
	HireDate = @hireDate, 
	Address = @address, 
	City = @city, 
	Region = @region,
	PostalCode = @postalCode,
	Country = @country,
	HomePhone = @homePhone,
	Extension = @extension,
	Photo = @photo,
	Notes = @notes,
	ReportsTo = @reportsTo,
	PhotoPath = @photoPath
WHERE EmployeeID = @employeeId
SELECT @@ROWCOUNT
GO

CREATE PROCEDURE [dbo].[FindEmployee]
	@employeeId int
AS
SELECT e.EmployeeID, e.LastName, e.FirstName, e.Title, e.TitleOfCourtesy, e.BirthDate, e.HireDate, e.Address, e.City, e.Region, e.PostalCode, e.Country, e.HomePhone, e.Extension, e.Photo, e.Notes, e.ReportsTo, e.PhotoPath
FROM dbo.Employees AS e
WHERE e.EmployeeID = @employeeId
GO

CREATE PROCEDURE [dbo].[SelectEmployees]
	@offsetEmployees int = 0,
	@limitEmployees int = NULL
AS
SELECT e.EmployeeID, e.LastName, e.FirstName, e.Title, e.TitleOfCourtesy, e.BirthDate, e.HireDate, e.Address, e.City, e.Region, e.PostalCode, e.Country, e.HomePhone, e.Extension, e.Photo, e.Notes, e.ReportsTo, e.PhotoPath
FROM dbo.Employees AS e
ORDER BY e.EmployeeID
OFFSET @offsetEmployees ROWS
FETCH FIRST ISNULL(@limitEmployees, (SELECT COUNT(*) FROM dbo.Employees)) ROWS ONLY
GO