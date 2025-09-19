DROP VIEW IF EXISTS `VW_ClientesProveedores`;

CREATE VIEW `VW_ClientesProveedores` AS
SELECT
  CompanyName,
  CONCAT(ContactTitle, ', ', ContactName) AS Contact,
  'Cliente' AS Relation,
  Address,
  City,
  Region,
  PostalCode,
  Country,
  Phone,
  Fax
FROM Customers

UNION ALL

SELECT
  CompanyName,
  CONCAT(ContactTitle, ', ', ContactName) AS Contact,
  'Proveedor' AS Relation,
  Address,
  City,
  Region,
  PostalCode,
  Country,
  Phone,
  Fax
FROM Suppliers;