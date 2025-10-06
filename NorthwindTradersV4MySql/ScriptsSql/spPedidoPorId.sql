CREATE PROCEDURE `spPedidoPorId`(IN pPedidoId INT)
BEGIN
	SELECT
		OrderID,
        CustomerID,
        EmployeeID,
        OrderDate,
        RequiredDate,
        ShippedDate,
        ShipVia,
        Freight,
        ShipName,
        ShipAddress,
        ShipCity,
        ShipRegion,
        ShipPostalCode,
        ShipCountry,
        RowVersion
    FROM Orders
    WHERE OrderID = pPedidoId;
END