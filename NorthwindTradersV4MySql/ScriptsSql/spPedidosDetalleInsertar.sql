CREATE PROCEDURE `spPedidosDetalleInsertar`(
  IN p_OrderId INT,
  IN p_ProductId INT,
  IN p_UnitPrice DECIMAL(19,4),
  IN p_Quantity SMALLINT,
  IN p_Discount DOUBLE, 
  OUT p_RowsInserted int
)
BEGIN
  DECLARE v_UnitsInStock INT;
  DECLARE EXIT HANDLER FOR SQLEXCEPTION
  BEGIN
    ROLLBACK;
    SET p_RowsInserted = 0;
    RESIGNAL;
  END;

  START TRANSACTION;

  -- Bloquear la fila del producto para evitar condiciones de carrera
  SELECT UnitsInStock
  INTO v_UnitsInStock
  FROM Products
  WHERE ProductID = p_ProductId
  FOR UPDATE;

  -- Verificación de cantidad de inventario
  IF v_UnitsInStock IS NULL THEN
    ROLLBACK;
    SET p_RowsInserted = 0;
    SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Producto no encontrado';
  ELSEIF v_UnitsInStock < p_Quantity THEN
    ROLLBACK;
    SET p_RowsInserted = 0;
    SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'La cantidad del producto en el pedido excedio el inventario disponible';
  END IF;

  -- Insertar en Order Details
  INSERT INTO `order details` (OrderID, ProductID, UnitPrice, Quantity, Discount)
  VALUES (p_OrderId, p_ProductId, p_UnitPrice, p_Quantity, p_Discount);
  
-- Capturar número de filas insertadas por la operación anterior
  SET p_RowsInserted = ROW_COUNT();

  -- Actualizar UnitsInStock en Products solo si se insertó al menos 1 fila
  IF p_RowsInserted > 0 THEN
    UPDATE Products
    SET UnitsInStock = UnitsInStock - p_Quantity
    WHERE ProductID = p_ProductId;
  END IF;

  COMMIT;
END