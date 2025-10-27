CREATE PROCEDURE `spPedidosPorRangoFechaPedido`(
in pFrom datetime,
in pTo datetime
)
BEGIN
	Select OrderDate, RequiredDate, ShippedDate, c.CompanyName, o.OrderID, Freight 
    From Orders o join Customers c on c.CustomerId = o.CustomerId
    WHERE
    (
      pFrom IS NULL AND pTo IS NULL AND OrderDate IS NULL
    )
    OR
    (
      pFrom IS NOT NULL AND pTo IS NOT NULL AND OrderDate >= pFrom AND OrderDate < pTo
    )
    order by OrderDate Desc, c.CompanyName;
END