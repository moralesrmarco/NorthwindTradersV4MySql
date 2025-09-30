CREATE PROCEDURE `spProductoPorId` (IN pProductId INT)
BEGIN
	Select * From Products Where ProductId = pProductId;
END
