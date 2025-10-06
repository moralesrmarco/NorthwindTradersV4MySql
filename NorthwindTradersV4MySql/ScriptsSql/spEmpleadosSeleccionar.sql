CREATE PROCEDURE `spEmpleadosSeleccionar` ()
BEGIN
	Select 0 As EmployeeId, '«--- Seleccione ---»' AS EmployeeName
    Union all
    Select EmployeeId, Concat(LastName, ', ', FirstName) As EmployeeName From Employees order by EmployeeName;
END
