CREATE PROCEDURE spProductoEliminar(
    IN  pProductID      INT,
    IN  pRowVersion     INT
)
BEGIN
    DELETE FROM Products
    WHERE ProductID = pProductID
      AND RowVersion = pRowVersion;
END
