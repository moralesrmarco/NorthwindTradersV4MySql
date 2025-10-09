CREATE PROCEDURE `spPedidosActualizar`( 
    IN pOrderId INT, 
    IN pCustomerId CHAR(5), 
    IN pEmployeeId INT, 
    IN pOrderDate DATETIME, 
    IN pRequiredDate DATETIME, 
    IN pShippedDate DATETIME, 
    IN pShipVia INT, 
    IN pFreight DECIMAL(19,4), 
    IN pShipName VARCHAR(40), 
    IN pShipAddress VARCHAR(60), 
    IN pShipCity VARCHAR(15), 
    IN pShipRegion VARCHAR(15), 
    IN pShipPostalCode VARCHAR(10), 
    IN pShipCountry VARCHAR(15), 
    OUT pRowVersion INT,
    OUT pFilasAfectadas INT
)
BEGIN 

DECLARE v_rows INT DEFAULT 0;

START TRANSACTION;

UPDATE orders
SET
    CustomerID    = pCustomerId,
    EmployeeID    = pEmployeeId,
    OrderDate     = pOrderDate,
    RequiredDate  = pRequiredDate,
    ShippedDate   = pShippedDate,
    ShipVia       = pShipVia,
    Freight       = pFreight,
    ShipName      = pShipName,
    ShipAddress   = pShipAddress,
    ShipCity      = pShipCity,
    ShipRegion    = pShipRegion,
    ShipPostalCode= pShipPostalCode,
    ShipCountry   = pShipCountry,
    RowVersion    = IFNULL(RowVersion, 0) + 1
WHERE OrderID = pOrderId;

SET v_rows = ROW_COUNT();
SET pFilasAfectadas = v_rows;

IF v_rows = 1 THEN
    SELECT RowVersion INTO pRowVersion FROM orders WHERE OrderID = pOrderId;
    COMMIT;
ELSE
    ROLLBACK;
    SET pRowVersion = 0;
END IF;

END