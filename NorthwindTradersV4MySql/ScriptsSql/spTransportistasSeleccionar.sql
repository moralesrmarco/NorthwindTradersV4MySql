CREATE PROCEDURE `spTransportistasSeleccionar` ()
BEGIN
	Select 0 As ShipperId, '«--- Seleccione ---»' AS CompanyName
    Union all
    Select ShipperId, CompanyName From Shippers order by CompanyName;
END
