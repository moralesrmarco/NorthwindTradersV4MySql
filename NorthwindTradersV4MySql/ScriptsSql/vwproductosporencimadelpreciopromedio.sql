CREATE OR REPLACE VIEW vwproductosporencimadelpreciopromedio AS
SELECT
  ROW_NUMBER() OVER (ORDER BY ProductID ASC) AS Fila,
  ProductName AS Producto,
  UnitPrice AS Precio
FROM Products
WHERE UnitPrice > (SELECT AVG(UnitPrice) FROM Products);