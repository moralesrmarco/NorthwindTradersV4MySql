CREATE PROCEDURE spProductoActualizar(
    IN  pProductID           INT,
    IN  pProductName         VARCHAR(40),
    IN  pSupplierID          INT,
    IN  pCategoryID          INT,
    IN  pQuantityPerUnit     VARCHAR(20),
    IN  pUnitPrice           DECIMAL(10,2),
    IN  pUnitsInStock        SMALLINT,
    IN  pUnitsOnOrder        SMALLINT,
    IN  pReorderLevel        SMALLINT,
    IN  pDiscontinued        TINYINT,
    IN  pRowVersion        INT
)
BEGIN
    UPDATE Products
    SET
        ProductName       = NULLIF(TRIM(pProductName), ''),
        SupplierID        = NULLIF(pSupplierID, 0),
        CategoryID        = NULLIF(pCategoryID, 0),
        QuantityPerUnit   = NULLIF(TRIM(pQuantityPerUnit), ''),
        UnitPrice         = pUnitPrice,
        UnitsInStock      = pUnitsInStock,
        UnitsOnOrder      = pUnitsOnOrder,
        ReorderLevel      = pReorderLevel,
        Discontinued      = pDiscontinued,
        RowVersion        = RowVersion + 1
    WHERE ProductID = pProductID
      AND RowVersion = pRowVersion;
END
