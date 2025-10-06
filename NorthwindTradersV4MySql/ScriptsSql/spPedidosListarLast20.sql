CREATE PROCEDURE spPedidosListarLast20()
BEGIN
  SELECT
    Orders.OrderID,
    Customers.CompanyName As Customer,
    Customers.ContactName,
    Orders.OrderDate,
    Orders.RequiredDate,
    Orders.ShippedDate,
    CONCAT(Employees.LastName, ', ', Employees.FirstName) As Employee,
    Shippers.CompanyName As Shipper,
    Orders.ShipName
  FROM Orders
    INNER JOIN Customers ON Orders.CustomerID = Customers.CustomerID
    INNER JOIN Employees ON Orders.EmployeeID = Employees.EmployeeID
    INNER JOIN Shippers ON Orders.ShipVia = Shippers.ShipperID
  ORDER BY Orders.OrderID DESC
  LIMIT 20;
END