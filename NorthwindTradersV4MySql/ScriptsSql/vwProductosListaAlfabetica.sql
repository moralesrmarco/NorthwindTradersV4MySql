CREATE VIEW `vwProductosListaAlfabetica` AS
Select products.*, categories.CategoryName, suppliers.CompanyName
FROM Categories RIGHT JOIN Products
  ON Categories.CategoryID = Products.CategoryID
LEFT JOIN Suppliers
  ON Products.SupplierID = Suppliers.SupplierID;
