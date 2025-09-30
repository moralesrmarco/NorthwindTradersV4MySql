CREATE DEFINER=`root`@`localhost` PROCEDURE `spCategoriasSeleccionar`()
BEGIN
	Select 0 As CategoryId, '«--- Seleccione ---»' AS CategoryName
    Union all
    Select CategoryId, CategoryName From Categories order by CategoryName;
END