CREATE PROCEDURE `spPedidosBuscar`(
  IN p_IdInicial INT,
  IN p_IdFinal INT,
  IN p_Cliente VARCHAR(40),
  IN p_FPedido TINYINT(1),
  IN p_FPedidoNull TINYINT(1),
  IN p_FPedidoIni DATETIME,
  IN p_FPedidoFin DATETIME,
  IN p_FRequerido TINYINT(1),
  IN p_FRequeridoNull TINYINT(1),
  IN p_FRequeridoIni DATETIME,
  IN p_FRequeridoFin DATETIME,
  IN p_FEnvio TINYINT(1),
  IN p_FEnvioNull TINYINT(1),
  IN p_FEnvioIni DATETIME,
  IN p_FEnvioFin DATETIME,
  IN p_Empleado VARCHAR(31),
  IN p_CompañiaT VARCHAR(40),
  IN p_Dirigidoa VARCHAR(40)
)
BEGIN
  SELECT DISTINCT
    o.OrderID,
    c.CompanyName As Customer,
    c.ContactName,
    o.OrderDate,
    o.RequiredDate,
    o.ShippedDate,
    CONCAT(e.LastName, ', ', e.FirstName) As Employee,
    s.CompanyName As Shipper,
    o.ShipName
  FROM `Order Details` od
    RIGHT JOIN Orders o ON od.OrderID = o.OrderID
    LEFT JOIN Employees e ON o.EmployeeID = e.EmployeeID
    LEFT JOIN Shippers s ON o.ShipVia = s.ShipperID
    LEFT JOIN Customers c ON o.CustomerID = c.CustomerID
  WHERE
    (p_IdInicial = 0 OR o.OrderID BETWEEN p_IdInicial AND p_IdFinal)
    AND (p_Cliente = '' OR c.CompanyName LIKE CONCAT('%', p_Cliente, '%'))
    AND (p_FPedido = 0 OR (o.OrderDate BETWEEN p_FPedidoIni AND p_FPedidoFin))
    AND (p_FPedidoNull = 0 OR o.OrderDate IS NULL)
    AND (p_FRequerido = 0 OR (o.RequiredDate BETWEEN p_FRequeridoIni AND p_FRequeridoFin))
    AND (p_FRequeridoNull = 0 OR o.RequiredDate IS NULL)
    AND (p_FEnvio = 0 OR (o.ShippedDate BETWEEN p_FEnvioIni AND p_FEnvioFin))
    AND (p_FEnvioNull = 0 OR o.ShippedDate IS NULL)
    AND (p_Empleado = '' OR CONCAT(e.LastName, ' ', e.FirstName) LIKE CONCAT('%', p_Empleado, '%'))
    AND (p_CompañiaT = '' OR s.CompanyName LIKE CONCAT('%', p_CompañiaT, '%'))
    AND (p_Dirigidoa = '' OR o.ShipName LIKE CONCAT('%', p_Dirigidoa, '%'))
  ORDER BY o.OrderID DESC;
  -- AND (p_FPedido = '' OR o.OrderDate BETWEEN p_FPedidoIni AND DATE_ADD(p_FPedidoFin, INTERVAL 1 MICROSECOND));
END