CREATE PROCEDURE spUsuarioActualizar (
	in pId int,
	in pPaterno varchar(50),
	in pMaterno varchar(50),
	in pNombres varchar(80), 
	in pUsuario varchar(20), 
	in pPassword varchar(64),
	in pFechaModificacion datetime,
	in pEstatus tinyint
)
BEGIN
	Update Usuarios
	SET
		Paterno = NULLIF(pPaterno, ''),
		Materno = NULLIF(pMaterno, ''),
		Nombres = pNombres, 
		Usuario = pUsuario, 
		Password = pPassword, 
		FechaModificacion = pFechaModificacion, 
		Estatus = pEstatus
	WHERE 
		Id = pId;
END