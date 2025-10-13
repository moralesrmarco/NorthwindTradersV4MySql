CREATE PROCEDURE `spDetallePedidosChkRowVersion` (
	in pPedidoId int,
    in pProductId int
)
BEGIN
	Select RowVersion From `order details` 
    Where OrderID = pPedidoId And ProductId = pProductId
    Limit 1;
END
