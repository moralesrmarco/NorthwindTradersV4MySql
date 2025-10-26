CREATE PROCEDURE `spProductosSeleccionar` (
in p_Categoria int
)
BEGIN
	SELECT 0 AS Id, '«--- Seleccione ---»' AS Producto
	UNION ALL
	Select ProductId As Id,  ProductName As Producto 
	From Products
	Where CategoryId = p_Categoria And Discontinued = FALSE
	Order by Producto;
END
