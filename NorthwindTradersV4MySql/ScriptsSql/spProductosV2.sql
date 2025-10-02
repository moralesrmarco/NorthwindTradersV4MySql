CREATE PROCEDURE `spProductosV2`(
  IN OrdenadoPor varchar(255),
  IN AscDesc varchar(4)
)
BEGIN
SELECT
  Products.ProductID,
  Products.ProductName,
  Products.QuantityPerUnit,
  Products.UnitPrice,
  Products.UnitsInStock,
  Products.UnitsOnOrder,
  Products.ReorderLevel,
  Products.Discontinued,
  Categories.CategoryName,
  Categories.Description,
  Suppliers.CompanyName,
  Categories.CategoryID,
  Suppliers.SupplierID
FROM Products
LEFT JOIN Categories ON Products.CategoryID = Categories.CategoryID
LEFT JOIN Suppliers ON Products.SupplierID = Suppliers.SupplierID

ORDER BY
    /* ProductID */
    CASE WHEN UPPER(TRIM(OrdenadoPor)) = 'PRODUCTID' AND UPPER(TRIM(AscDesc)) = 'ASC' THEN Products.ProductID END ASC,
    CASE WHEN UPPER(TRIM(OrdenadoPor)) = 'PRODUCTID' AND UPPER(TRIM(AscDesc)) = 'DESC' THEN Products.ProductID END DESC,

    /* ProductName */
    CASE WHEN UPPER(TRIM(OrdenadoPor)) = 'PRODUCTNAME' AND UPPER(TRIM(AscDesc)) = 'ASC' THEN Products.ProductName END ASC,
    CASE WHEN UPPER(TRIM(OrdenadoPor)) = 'PRODUCTNAME' AND UPPER(TRIM(AscDesc)) = 'DESC' THEN Products.ProductName END DESC,

    /* QuantityPerUnit */
    CASE WHEN UPPER(TRIM(OrdenadoPor)) = 'QUANTITYPERUNIT' AND UPPER(TRIM(AscDesc)) = 'ASC' THEN Products.QuantityPerUnit END ASC,
    CASE WHEN UPPER(TRIM(OrdenadoPor)) = 'QUANTITYPERUNIT' AND UPPER(TRIM(AscDesc)) = 'DESC' THEN Products.QuantityPerUnit END DESC,

    /* UnitPrice */
    CASE WHEN UPPER(TRIM(OrdenadoPor)) = 'UNITPRICE' AND UPPER(TRIM(AscDesc)) = 'ASC' THEN Products.UnitPrice END ASC,
    CASE WHEN UPPER(TRIM(OrdenadoPor)) = 'UNITPRICE' AND UPPER(TRIM(AscDesc)) = 'DESC' THEN Products.UnitPrice END DESC,

    /* UnitsInStock */
    CASE WHEN UPPER(TRIM(OrdenadoPor)) = 'UNITSINSTOCK' AND UPPER(TRIM(AscDesc)) = 'ASC' THEN Products.UnitsInStock END ASC,
    CASE WHEN UPPER(TRIM(OrdenadoPor)) = 'UNITSINSTOCK' AND UPPER(TRIM(AscDesc)) = 'DESC' THEN Products.UnitsInStock END DESC,

    /* UnitsOnOrder */
    CASE WHEN UPPER(TRIM(OrdenadoPor)) = 'UNITSONORDER' AND UPPER(TRIM(AscDesc)) = 'ASC' THEN Products.UnitsOnOrder END ASC,
    CASE WHEN UPPER(TRIM(OrdenadoPor)) = 'UNITSONORDER' AND UPPER(TRIM(AscDesc)) = 'DESC' THEN Products.UnitsOnOrder END DESC,

    /* ReorderLevel */
    CASE WHEN UPPER(TRIM(OrdenadoPor)) = 'REORDERLEVEL' AND UPPER(TRIM(AscDesc)) = 'ASC' THEN Products.ReorderLevel END ASC,
    CASE WHEN UPPER(TRIM(OrdenadoPor)) = 'REORDERLEVEL' AND UPPER(TRIM(AscDesc)) = 'DESC' THEN Products.ReorderLevel END DESC,

    /* Discontinued */
    CASE WHEN UPPER(TRIM(OrdenadoPor)) = 'DISCONTINUED' AND UPPER(TRIM(AscDesc)) = 'ASC' THEN Products.Discontinued END ASC,
    CASE WHEN UPPER(TRIM(OrdenadoPor)) = 'DISCONTINUED' AND UPPER(TRIM(AscDesc)) = 'DESC' THEN Products.Discontinued END DESC,

    /* CategoryName */
    CASE WHEN UPPER(TRIM(OrdenadoPor)) = 'CATEGORYNAME' AND UPPER(TRIM(AscDesc)) = 'ASC' THEN Categories.CategoryName END ASC,
    CASE WHEN UPPER(TRIM(OrdenadoPor)) = 'CATEGORYNAME' AND UPPER(TRIM(AscDesc)) = 'DESC' THEN Categories.CategoryName END DESC,

    /* CategoryID */
    CASE WHEN UPPER(TRIM(OrdenadoPor)) = 'CATEGORYID' AND UPPER(TRIM(AscDesc)) = 'ASC' THEN Categories.CategoryID END ASC,
    CASE WHEN UPPER(TRIM(OrdenadoPor)) = 'CATEGORYID' AND UPPER(TRIM(AscDesc)) = 'DESC' THEN Categories.CategoryID END DESC,

    /* CompanyName (Supplier) */
    CASE WHEN UPPER(TRIM(OrdenadoPor)) = 'COMPANYNAME' AND UPPER(TRIM(AscDesc)) = 'ASC' THEN Suppliers.CompanyName END ASC,
    CASE WHEN UPPER(TRIM(OrdenadoPor)) = 'COMPANYNAME' AND UPPER(TRIM(AscDesc)) = 'DESC' THEN Suppliers.CompanyName END DESC,

    /* SupplierID */
    CASE WHEN UPPER(TRIM(OrdenadoPor)) = 'SUPPLIERID' AND UPPER(TRIM(AscDesc)) = 'ASC' THEN Suppliers.SupplierID END ASC,
    CASE WHEN UPPER(TRIM(OrdenadoPor)) = 'SUPPLIERID' AND UPPER(TRIM(AscDesc)) = 'DESC' THEN Suppliers.SupplierID END DESC,

    /* Fallback order */
    Products.ProductID DESC;
END