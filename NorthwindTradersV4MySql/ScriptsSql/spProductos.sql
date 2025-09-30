CREATE PROCEDURE `spProductos` ()
BEGIN
SELECT
  Products.ProductID,
  Products.ProductName,
  Products.QuantityPerUnit,
  Products.UnitPrice,
  Products.UnitsInStock,
  Products.UnitsOnOrder,
  Products.ReorderLevel,
  Products.Discontinued,
  Categories.CategoryName,
  Categories.Description,
  Suppliers.CompanyName,
  Categories.CategoryID,
  Suppliers.SupplierID
FROM Products
LEFT JOIN Categories ON Products.CategoryID = Categories.CategoryID
LEFT JOIN Suppliers ON Products.SupplierID = Suppliers.SupplierID
ORDER BY Products.ProductID DESC;
END
