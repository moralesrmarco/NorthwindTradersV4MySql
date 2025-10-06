CREATE PROCEDURE `spProductosSeleccionarPorCategorias` (
in pCategoria int
)
BEGIN
	select 0 as Id, '«--- Seleccione ---»' As Producto
    union all
    select ProductId as Id, ProductName as Producto
    from products
    where CategoryId = pCategoria And Discontinued = false
    order by Producto;
END
