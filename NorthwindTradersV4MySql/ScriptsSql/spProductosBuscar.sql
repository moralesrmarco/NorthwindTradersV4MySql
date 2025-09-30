CREATE DEFINER=`root`@`localhost` PROCEDURE `spProductosBuscar`(
  IN IdIni INT,
  IN IdFin INT,
  IN Producto VARCHAR(40),
  IN Categoria INT,
  IN Proveedor INT
)
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
  WHERE
    ( IdIni = 0 OR Products.ProductID BETWEEN IdIni AND IdFin )
    AND ( Producto = '' OR Products.ProductName LIKE CONCAT('%', Producto, '%') )
    AND ( Categoria = 0 OR Products.CategoryID = Categoria )
    AND ( Proveedor = 0 OR Products.SupplierID = Proveedor )
  ORDER BY Products.ProductID DESC;
END