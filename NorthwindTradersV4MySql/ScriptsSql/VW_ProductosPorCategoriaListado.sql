alter View `northwind`.`vw_productosporcategorialistado` AS
SELECT
    Categories.CategoryName,
    Products.ProductName,
    Products.ProductID,
    Products.QuantityPerUnit,
    Products.UnitPrice,
    Products.UnitsInStock,
    Products.UnitsOnOrder,
    Products.ReorderLevel,
    Products.Discontinued,
    Suppliers.CompanyName
FROM
    Products
INNER JOIN Categories ON Products.CategoryID = Categories.CategoryID
INNER JOIN Suppliers  ON Products.SupplierID  = Suppliers.SupplierID;