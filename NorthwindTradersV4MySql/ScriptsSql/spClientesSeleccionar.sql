CREATE PROCEDURE `spClientesSeleccionar` ()
BEGIN
	Select '0' As CustomerId, '«--- Seleccione ---»' AS CompanyName
    Union all
    Select CustomerId, CompanyName From Customers order by CompanyName;
END
