CREATE PROCEDURE spProductoInsertar(
    IN  pProductName        VARCHAR(40),
    IN  pSupplierID         INT,
    IN  pCategoryID         INT,
    IN  pQuantityPerUnit    VARCHAR(20),
    IN  pUnitPrice          DECIMAL(10,2),
    IN  pUnitsInStock       SMALLINT,
    IN  pUnitsOnOrder       SMALLINT,
    IN  pReorderLevel       SMALLINT,
    IN  pDiscontinued       TINYINT,
    IN  pRowVersionIn       INT,
    OUT pProductID          INT
)
BEGIN
    INSERT INTO Products
        (ProductName, SupplierID, CategoryID, QuantityPerUnit, UnitPrice,
         UnitsInStock, UnitsOnOrder, ReorderLevel, Discontinued, RowVersion)
    VALUES
        (NULLIF(pProductName, ''), 
         NULLIF(pSupplierID, 0), 
         NULLIF(pCategoryID, 0), 
         NULLIF(TRIM(pQuantityPerUnit), ''), 
         pUnitPrice,
         pUnitsInStock,
         pUnitsOnOrder,
         pReorderLevel,
         pDiscontinued,
         pRowVersionIn
         );

    SET pProductID = LAST_INSERT_ID();
END