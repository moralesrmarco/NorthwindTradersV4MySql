CREATE PROCEDURE spPedidosEliminar(
    IN pOrderId INT, 
    OUT pAffectedRows INT
)
BEGIN
    DECLARE v_rows INT DEFAULT 0;

    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        ROLLBACK;
        SET pAffectedRows = -1;
        RESIGNAL;
    END;

    START TRANSACTION;

    -- Devolver los productos al inventario
    UPDATE Products p
    JOIN `Order Details` od ON p.ProductID = od.ProductID
    SET p.UnitsInStock = p.UnitsInStock + od.Quantity
    WHERE od.OrderID = pOrderId;
    SET v_rows = v_rows + ROW_COUNT();

    -- Eliminar detalles del pedido
    DELETE FROM `Order Details`
    WHERE OrderID = pOrderId;
    SET v_rows = v_rows + ROW_COUNT();

    -- Eliminar el pedido
    DELETE FROM Orders
    WHERE OrderID = pOrderId;
    SET v_rows = v_rows + ROW_COUNT();

    COMMIT;

    SET pAffectedRows = v_rows;
END;