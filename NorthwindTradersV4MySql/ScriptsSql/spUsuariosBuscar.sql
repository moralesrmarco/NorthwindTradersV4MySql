CREATE PROCEDURE `spUsuariosBuscar`(
	IN pIdIni INT,
	IN pIdFin INT,
	IN pPaterno VARCHAR(50),
	IN pMaterno VARCHAR(50),
	IN pNombres VARCHAR(80),
	IN pUsuario VARCHAR(20)
)
BEGIN
	SELECT 
		Id, 
		Paterno, 
		Materno, 
		Nombres, 
		Usuario, 
		Password, 
		FechaCaptura, 
		FechaModificacion, 
		Estatus
	FROM Usuarios
	WHERE
		(pIdIni = 0 OR Id BETWEEN pIdIni AND pIdFin) AND
		(pPaterno = '' OR Paterno LIKE CONCAT('%', pPaterno, '%')) AND
		(pMaterno = '' OR Materno LIKE CONCAT('%', pMaterno, '%')) AND
		(pNombres = '' OR Nombres LIKE CONCAT('%', pNombres, '%')) AND
		(pUsuario = '' OR Usuario LIKE CONCAT('%', pUsuario, '%'))
	ORDER BY Paterno, Materno, Nombres, Usuario;
END