CREATE PROCEDURE `spUsuarioEliminar`(
	in pId int, 
	out pRowsAfectados int
)
BEGIN
	DECLARE v_rows INT DEFAULT 0;

	DECLARE EXIT HANDLER FOR SQLEXCEPTION
	BEGIN
		ROLLBACK;
		SET pRowsAfectados = -1;
		RESIGNAL;
	END;

	START TRANSACTION;

		DELETE FROM permisos WHERE UsuarioId = pId;
		SET v_rows = v_rows + ROW_COUNT();

		DELETE FROM usuarios WHERE Id = pId;
		SET v_rows = v_rows + ROW_COUNT();

	COMMIT;

	SET pRowsAfectados = v_rows;
END