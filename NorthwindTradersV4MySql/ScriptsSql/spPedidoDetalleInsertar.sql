CREATE PROCEDURE `spPedidoDetalleInsertar` (
	in pOrderId int,
	in pProductId int,
	in pUnitPrice decimal(19, 4),
	in pQuantity smallint,
	in pDiscount float(24, 2),
	in pRowVersion int
)
BEGIN
-- Validaciones básicas
  IF pOrderId IS NULL OR pOrderId <= 0 THEN
    SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'OrderId inválido';
  END IF;

  IF pProductId IS NULL OR pProductId <= 0 THEN
    SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'ProductId inválido';
  END IF;

  IF pQuantity IS NULL OR pQuantity <= 0 THEN
    SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Quantity debe ser mayor a 0';
  END IF;

  IF pUnitPrice IS NULL OR pUnitPrice < 0 THEN
    SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'UnitPrice inválido';
  END IF;

  -- Inserción
  INSERT INTO `order details`
    (`OrderID`, `ProductID`, `UnitPrice`, `Quantity`, `Discount`, `RowVersion`)
  VALUES
    (pOrderId, pProductId, pUnitPrice, pQuantity, pDiscount, pRowVersion);
END
