CREATE PROCEDURE `spPedidosDetalleActualizar`(
    IN p_OrderId INT,
    IN p_ProductId INT,
    IN p_Quantity SMALLINT,
    IN p_Discount DOUBLE,
    IN p_QuantityOld SMALLINT,
    IN p_DiscountOld DOUBLE,
    OUT p_RegistrosModificados INT
)
BEGIN
    DECLARE v_Difference SMALLINT;
    DECLARE v_AffectedOrderDetails INT DEFAULT 0;

    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        ROLLBACK;
        SET p_RegistrosModificados = 0;
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Error inesperado al actualizar el detalle del pedido.';
    END;

    SET v_Difference = p_Quantity - p_QuantityOld;
    START TRANSACTION;
	-- Verificar si el detalle del pedido existe
    IF EXISTS (
        SELECT 1 FROM `Order Details`
        WHERE OrderID = p_OrderId AND ProductID = p_ProductId
        LIMIT 1
    ) THEN
		-- Si @Difference es mayor que cero, significa que la nueva cantidad (@Quantity) es mayor que la 
		-- cantidad anterior (@QuantityOld), entonces debemos restar la diferencia a UnitsInStock porque 
		-- más productos se han vendido.
        IF v_Difference > 0 THEN
			-- Reduciendo el inventario
            UPDATE `Products`
            SET UnitsInStock = COALESCE(UnitsInStock,0) - v_Difference
            WHERE ProductID = p_ProductId;
		-- Si @Difference es menor que cero, significa que la nueva cantidad es menor que la cantidad anterior, 
		-- entonces debemos sumar la diferencia (en términos absolutos) a UnitsInStock porque menos productos 
		-- se han vendido o se han devuelto productos.
        ELSEIF v_Difference < 0 THEN
			-- Incrementando el inventario
            UPDATE `Products`
            SET UnitsInStock = COALESCE(UnitsInStock,0) + ABS(v_Difference)
            WHERE ProductID = p_ProductId;
        END IF;
		-- Actualizar el detalle del pedido
        UPDATE `Order Details`
        SET Quantity = p_Quantity,
            Discount = p_Discount
        WHERE OrderID = p_OrderId AND ProductID = p_ProductId;

        SET v_AffectedOrderDetails = ROW_COUNT();
		-- Commit de la transacción si todo es exitoso
        COMMIT;
        SET p_RegistrosModificados = v_AffectedOrderDetails;
    ELSE
        ROLLBACK;
        SET p_RegistrosModificados = 0;
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'El detalle del pedido no existe. El registro fue eliminado previamente por otro usuario de la red.';
    END IF;
END