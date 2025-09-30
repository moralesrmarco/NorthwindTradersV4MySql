CREATE PROCEDURE `spProveedoresSeleccionar` ()
BEGIN
	Select 0 As SupplierId, '«--- Seleccione ---»' as CompanyName
    Union All
    Select SupplierId, CompanyName From Suppliers Order by CompanyName;
END