CREATE PROCEDURE `spPedidosInsertar` (
	in pCustomerId char(5),
	in pEmployeeId int,
	in pOrderDate datetime,
	in pRequiredDate datetime,
	in pShippedDate datetime,
	in pShipVia int,
	in pFreight decimal(19,4), 
	in pShipName varchar(40),
	in pShipAddress varchar(60),
	in pShipCity varchar(15),
	in pShipRegion varchar(15),
	in pShipPostalCode varchar(10),
	in pShipCountry varchar(15),
	in pRowVersion int
)
BEGIN
INSERT INTO `orders` (
    `CustomerID`,
    `EmployeeID`,
    `OrderDate`,
    `RequiredDate`,
    `ShippedDate`,
    `ShipVia`,
    `Freight`,
    `ShipName`,
    `ShipAddress`,
    `ShipCity`,
    `ShipRegion`,
    `ShipPostalCode`,
    `ShipCountry`,
    `RowVersion`
  ) VALUES (
    NULLIF(pCustomerId, ''),
    pEmployeeId,
    pOrderDate,
    pRequiredDate,
    pShippedDate,
    pShipVia,
    pFreight,
    NULLIF(pShipName, ''),
    NULLIF(pShipAddress, ''),
    NULLIF(pShipCity, ''),
    NULLIF(pShipRegion, ''),
    NULLIF(pShipPostalCode, ''),
    NULLIF(pShipCountry, ''),
    pRowVersion
  );
END
