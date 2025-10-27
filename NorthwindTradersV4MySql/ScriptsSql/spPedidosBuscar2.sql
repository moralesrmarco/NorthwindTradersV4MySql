CREATE PROCEDURE spPedidosBuscar2 (
	IN IdInicial INT,
	IN IdFinal INT,
	IN Cliente VARCHAR(40),
	IN FPedido BOOLEAN,
	IN FPedidoNull BOOLEAN,
	IN FPedidoIni DATETIME,
	IN FPedidoFin DATETIME,
	IN FRequerido BOOLEAN,
	IN FRequeridoNull BOOLEAN,
	IN FRequeridoIni DATETIME,
	IN FRequeridoFin DATETIME,
	IN FEnvio BOOLEAN,
	IN FEnvioNull BOOLEAN,
	IN FEnvioIni DATETIME,
	IN FEnvioFin DATETIME,
	IN Empleado VARCHAR(31),
	IN CompañiaT VARCHAR(40),
	IN Dirigidoa VARCHAR(40)
)
BEGIN
	SELECT DISTINCT
		Orders.OrderID AS Id,
		Customers.CompanyName AS Cliente,
		CONCAT(Employees.LastName, ', ', Employees.FirstName) AS Vendedor,
		Orders.OrderDate AS FechaDePedido,
		Orders.RequiredDate AS FechaRequerido,
		Orders.ShippedDate AS FechaDeEnvio,
		Shippers.CompanyName AS CompaniaTransportista,
		Orders.ShipName AS DirigidoA,
		Orders.ShipAddress AS Domicilio,
		Orders.ShipCity AS Ciudad,
		Orders.ShipRegion AS Region,
		Orders.ShipPostalCode AS CodigoPostal,
		Orders.ShipCountry AS Pais,
		Orders.Freight AS Flete
	FROM `Order Details`
		RIGHT JOIN Orders ON `Order Details`.OrderID = Orders.OrderID
		LEFT JOIN Employees ON Orders.EmployeeID = Employees.EmployeeID
		LEFT JOIN Shippers ON Orders.ShipVia = Shippers.ShipperID
		LEFT JOIN Customers ON Orders.CustomerID = Customers.CustomerID
	WHERE
		(IdInicial = 0 OR Orders.OrderID BETWEEN IdInicial AND IdFinal)
		AND (Cliente = '' OR Customers.CompanyName LIKE CONCAT('%', Cliente, '%'))
		AND (FPedido = 0 OR Orders.OrderDate BETWEEN FPedidoIni AND FPedidoFin)
		AND (FPedidoNull = 0 OR Orders.OrderDate IS NULL)
		AND (FRequerido = 0 OR Orders.RequiredDate BETWEEN FRequeridoIni AND FRequeridoFin)
		AND (FRequeridoNull = 0 OR Orders.RequiredDate IS NULL)
		AND (FEnvio = 0 OR Orders.ShippedDate BETWEEN FEnvioIni AND FEnvioFin)
		AND (FEnvioNull = 0 OR Orders.ShippedDate IS NULL)
		AND (Empleado = '' OR CONCAT(Employees.LastName, ' ', Employees.FirstName) LIKE CONCAT('%', Empleado, '%'))
		AND (CompañiaT = '' OR Shippers.CompanyName LIKE CONCAT('%', CompañiaT, '%'))
		AND (Dirigidoa = '' OR Orders.ShipName LIKE CONCAT('%', Dirigidoa, '%'))
	ORDER BY Orders.OrderID DESC;
END
