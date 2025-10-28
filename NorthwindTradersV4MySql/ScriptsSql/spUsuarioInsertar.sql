CREATE PROCEDURE spUsuarioInsertar (
	out pId int,
	in pPaterno varchar(50),
	in pMaterno varchar(50),
	in pNombres varchar(80), 
	in pUsuario varchar(20), 
	in pPassword varchar(64),
	in pFechaCaptura datetime,
	in pFechaModificacion datetime,
	in pEstatus tinyint
)
BEGIN
	INSERT INTO Usuarios
		(Paterno, Materno, Nombres, Usuario, Password, FechaCaptura, FechaModificacion, Estatus)
	VALUES (
		NULLIF(pPaterno, ''),
		NULLIF(pMaterno, ''),
		pNombres, 
		pUsuario,
		pPassword,
		pFechaCaptura,
		pFechaModificacion,
		pEstatus
	);
	SET pId = LAST_INSERT_ID();
END
