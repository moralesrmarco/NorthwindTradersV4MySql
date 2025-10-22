CREATE PROCEDURE spPedidosDetalleEliminar(
    IN p_OrderId INT,
    IN p_ProductId INT,
    OUT p_RowsDeleted INT
)
BEGIN
    DECLARE v_Quantity INT;
    DECLARE v_DeletedCount INT DEFAULT 0;

    DECLARE exit handler FOR SQLEXCEPTION
    BEGIN
        -- Manejo de error: rollback y salida
        ROLLBACK;
        SET p_RowsDeleted = 0;
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'Error inesperado al eliminar el detalle del pedido.';
    END;

    START TRANSACTION;

    -- Verificar si el detalle del pedido existe
    IF EXISTS (
        SELECT 1 FROM `Order Details`
        WHERE OrderID = p_OrderId AND ProductID = p_ProductId
    ) THEN

        -- Obtener la cantidad
        SELECT Quantity INTO v_Quantity
        FROM `Order Details`
        WHERE OrderID = p_OrderId AND ProductID = p_ProductId;

        -- Verificar si es nulo
        IF v_Quantity IS NULL THEN
            ROLLBACK;
            SET p_RowsDeleted = 0;
            SIGNAL SQLSTATE '45000'
                SET MESSAGE_TEXT = 'La cantidad del producto es nula.';
        ELSE
            -- Actualizar inventario
            UPDATE Products
            SET UnitsInStock = COALESCE(UnitsInStock, 0) + v_Quantity
            WHERE ProductID = p_ProductId;

            -- Eliminar el detalle del pedido
            DELETE FROM `Order Details`
            WHERE OrderID = p_OrderId AND ProductID = p_ProductId;

            -- Obtener número de registros eliminados
            SET v_DeletedCount = ROW_COUNT();

            COMMIT;
            SET p_RowsDeleted = v_DeletedCount;
        END IF;

    ELSE
        ROLLBACK;
        SET p_RowsDeleted = 0;
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'El detalle del pedido no existe. El registro fue eliminado previamente por otro usuario de la red.';
    END IF;
END