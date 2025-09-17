using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.IO;

namespace NorthwindTradersV4MySql
{
    internal class EmpleadoRepository
    {
        private readonly string _connectionString;
        
        public EmpleadoRepository(string _connectionString) 
        {
            this._connectionString = _connectionString;
        }

        public DataTable ObtenerPaisesEmpleados()
        {
            DataTable dt = new DataTable();
            using (var cn = new MySqlConnection(_connectionString))
            using (var cmd = new MySqlCommand("SELECT '' As Id, '»--- Seleccione ---«' As Pais UNION ALL SELECT DISTINCT Country As Id, Country As Pais FROM Employees ORDER BY Pais;", cn))
            using (var da = new MySqlDataAdapter(cmd))
                da.Fill(dt);
            return dt;
        }

        public DataTable ObtenerReportaAEmpleados()
        {
            DataTable dt = new DataTable();
            using (var cn = new MySqlConnection(_connectionString))
            using (var cmd = new MySqlCommand("SELECT -1 As Id, '»--- Seleccione ---«' As Nombre, '000' As Orden UNION ALL SELECT 0 As Id, '' As Nombre, '111' As Orden UNION ALL SELECT EmployeeID As Id, CONCAT(LastName, ', ', FirstName) As Nombre, Concat(LastName, ', ', FirstName) As Orden FROM Employees Order by Orden;", cn))
            using (var da = new MySqlDataAdapter(cmd))
                da.Fill(dt);
            return dt;
        }

        public Empleado ObtenerEmpleado(Empleado empleado)
        {
            string query = @"
                            SELECT
                                e.EmployeeID                  AS Id,
                                e.FirstName                   AS Nombres,
                                e.LastName                    AS Apellidos,
                                e.Title                       AS Título,
                                e.TitleOfCourtesy             AS `Título de cortesía`,
                                e.BirthDate                   AS `Fecha de nacimiento`,
                                e.HireDate                    AS `Fecha de contratación`,
                                e.Address                     AS Domicilio,
                                e.City                        AS Ciudad,
                                e.Region                      AS Región,
                                e.PostalCode                  AS `Código postal`,
                                e.Country                     AS País,
                                e.HomePhone                   AS Teléfono,
                                e.Extension                   AS Extensión,
                                e.Photo                       AS Foto,
                                e.Notes                       AS Notas,
                                e.ReportsTo                   AS Reportaa,
                                CONCAT(e1.LastName, ', ', e1.FirstName) AS `Reporta a`,
                                e.RowVersion
                            FROM Employees AS e
                            LEFT JOIN Employees AS e1
                                ON e.ReportsTo = e1.EmployeeID
                            WHERE e.EmployeeID = @Id;
                            ";
            using (var cn = new MySqlConnection(_connectionString))
            using (var cmd = new MySqlCommand(query, cn))
            {
                cmd.Parameters.AddWithValue("Id", empleado.EmployeeID);
                if (cn.State != ConnectionState.Open) cn.Open();
                using (MySqlDataReader rdr = cmd.ExecuteReader())
                {
                    if (rdr.Read())
                    {
                        empleado.RowVersion = (int)rdr["RowVersion"];
                        empleado.FirstName = rdr["Nombres"].ToString();
                        empleado.LastName = rdr["Apellidos"].ToString();
                        empleado.Title = rdr.IsDBNull(rdr.GetOrdinal("Título")) ? null : rdr.GetString(rdr.GetOrdinal("Título"));
                        empleado.TitleOfCourtesy = rdr.IsDBNull(rdr.GetOrdinal("Título de cortesía")) ? null : rdr.GetString(rdr.GetOrdinal("Título de cortesía"));
                        empleado.BirthDate = rdr.IsDBNull(rdr.GetOrdinal("Fecha de nacimiento")) ? (DateTime?)null : rdr.GetDateTime(rdr.GetOrdinal("Fecha de nacimiento"));
                        empleado.HireDate = rdr.IsDBNull(rdr.GetOrdinal("Fecha de contratación")) ? (DateTime?)null : rdr.GetDateTime(rdr.GetOrdinal("Fecha de contratación"));
                        empleado.Address = rdr.IsDBNull(rdr.GetOrdinal("Domicilio")) ? null : rdr.GetString(rdr.GetOrdinal("Domicilio"));
                        empleado.City = rdr.IsDBNull(rdr.GetOrdinal("Ciudad")) ? null : rdr.GetString(rdr.GetOrdinal("Ciudad"));
                        empleado.Region = rdr.IsDBNull(rdr.GetOrdinal("Región")) ? null : rdr.GetString(rdr.GetOrdinal("Región"));
                        empleado.PostalCode = rdr.IsDBNull(rdr.GetOrdinal("Código postal")) ? null : rdr.GetString(rdr.GetOrdinal("Código postal"));
                        empleado.Country = rdr.IsDBNull(rdr.GetOrdinal("País")) ? null : rdr.GetString(rdr.GetOrdinal("País"));
                        empleado.HomePhone = rdr.IsDBNull(rdr.GetOrdinal("Teléfono")) ? null : rdr.GetString(rdr.GetOrdinal("Teléfono"));
                        empleado.Extension = rdr.IsDBNull(rdr.GetOrdinal("Extensión")) ? null : rdr.GetString(rdr.GetOrdinal("Extensión"));
                        empleado.Notes = rdr.IsDBNull(rdr.GetOrdinal("Notas")) ? null : rdr.GetString(rdr.GetOrdinal("Notas"));
                        empleado.ReportsTo = rdr.IsDBNull(rdr.GetOrdinal("Reportaa")) ? (int?)null : rdr.GetInt32(rdr.GetOrdinal("Reportaa"));
                        empleado.Photo = rdr.IsDBNull(rdr.GetOrdinal("Foto"))
                                    ? null
                                    : Utils.StripOleHeader(
                                        (byte[])rdr["Foto"],                            // cast directo a byte[]
                                        rdr.GetInt32(rdr.GetOrdinal("Id")));
                    }
                    else
                        empleado = null;
                }
            }
            return empleado;
        }

        public DataTable ObtenerEmpleados(object sender, EmpleadosBuscar empleadosBuscar)
        {
            DataTable dt = new DataTable();
            string query;
            if (sender == null)
            {
                query = @"
                            SELECT
                                e.EmployeeID   AS Id,
                                e.FirstName    AS Nombres,
                                e.LastName     AS Apellidos,
                                e.Title        AS `Título`,
                                e.BirthDate    AS `Fecha de nacimiento`,
                                e.City         AS Ciudad,
                                e.Country      AS País,
                                e.Photo        AS Foto,
                                CONCAT(e2.LastName, ', ', e2.FirstName) AS `Reporta a`
                            FROM Employees AS e
                            LEFT JOIN Employees AS e2
                                ON e.ReportsTo = e2.EmployeeID
                            ORDER BY Id DESC
                            LIMIT 20;
                            ";
            }
            else
            {
                query = @"
                            SELECT
                              e.EmployeeID AS Id,
                              e.FirstName AS Nombres,
                              e.LastName AS Apellidos,
                              e.Title AS `Título`,
                              e.BirthDate AS `Fecha de nacimiento`,
                              e.City AS Ciudad,
                              e.Country AS País,
                              e.Photo AS Foto,
                              CONCAT(e2.LastName, ', ', e2.FirstName) AS `Reporta a`
                            FROM Employees AS e
                            LEFT JOIN Employees AS e2
                              ON e.ReportsTo = e2.EmployeeID
                            WHERE
                              (@IdIni = 0 OR e.EmployeeID BETWEEN @IdIni AND @IdFin)
                              AND(@Nombres = '' OR e.FirstName  LIKE CONCAT('%', @Nombres, '%'))
                              AND(@Apellidos = '' OR e.LastName   LIKE CONCAT('%', @Apellidos, '%'))
                              AND(@Titulo = '' OR e.Title      LIKE CONCAT('%', @Titulo, '%'))
                              AND(@Domicilio = '' OR e.Address    LIKE CONCAT('%', @Domicilio, '%'))
                              AND(@Ciudad = '' OR e.City       LIKE CONCAT('%', @Ciudad, '%'))
                              AND(@Region = '' OR e.Region     LIKE CONCAT('%', @Region, '%'))
                              AND(@CodigoP = '' OR e.PostalCode LIKE CONCAT('%', @CodigoP, '%'))
                              AND(@Pais = '' OR e.Country    LIKE CONCAT('%', @Pais, '%'))
                              AND(@Telefono = '' OR e.HomePhone  LIKE CONCAT('%', @Telefono, '%'))
                            ORDER BY Id DESC;
                            ";
            }
            using (var cn = new MySqlConnection(_connectionString))
            using (var cmd = new MySqlCommand(query, cn))
            {
                if (sender != null)
                {
                    cmd.Parameters.AddWithValue("@IdIni", empleadosBuscar.IdIni);
                    cmd.Parameters.AddWithValue("@IdFin", empleadosBuscar.IdFin);
                    cmd.Parameters.AddWithValue("@Nombres", empleadosBuscar.Nombres);
                    cmd.Parameters.AddWithValue("@Apellidos", empleadosBuscar.Apellidos);
                    cmd.Parameters.AddWithValue("@Titulo", empleadosBuscar.Titulo);
                    cmd.Parameters.AddWithValue("@Domicilio", empleadosBuscar.Domicilio);
                    cmd.Parameters.AddWithValue("@Ciudad", empleadosBuscar.Ciudad);
                    cmd.Parameters.AddWithValue("@Region", empleadosBuscar.Region);
                    cmd.Parameters.AddWithValue("@CodigoP", empleadosBuscar.CodigoP);
                    cmd.Parameters.AddWithValue("@Pais", empleadosBuscar.Pais);
                    cmd.Parameters.AddWithValue("@Telefono", empleadosBuscar.Telefono);
                }
                using (var da = new MySqlDataAdapter(cmd))
                    da.Fill(dt);
            }
            return dt;
        }

        public int Insertar(Empleado e)
        {
            using (var cn = new MySqlConnection(_connectionString))
            using (var cmd = new MySqlCommand("INSERT INTO Employees (FirstName, LastName, Title, TitleOfCourtesy, BirthDate, HireDate, Address, City, Region, PostalCode, Country, HomePhone, Extension, Notes, ReportsTo, Photo) VALUES (@FirstName, @LastName, @Title, @TitleOfCourtesy, @BirthDate, @HireDate, @Address, @City, @Region, @PostalCode, @Country, @HomePhone, @Extension, @Notes, @ReportsTo, @Photo);", cn))
            {
                cmd.Parameters.AddWithValue("@FirstName", e.FirstName);
                cmd.Parameters.AddWithValue("@LastName", e.LastName);
                cmd.Parameters.AddWithValue("@Title", e.Title);
                cmd.Parameters.AddWithValue("@TitleOfCourtesy", e.TitleOfCourtesy);
                cmd.Parameters.AddWithValue("@BirthDate", e.BirthDate);
                cmd.Parameters.AddWithValue("@HireDate", e.HireDate);
                cmd.Parameters.AddWithValue("@Address", e.Address);
                cmd.Parameters.AddWithValue("@City", e.City);
                cmd.Parameters.AddWithValue("@Region", e.Region);
                cmd.Parameters.AddWithValue("@PostalCode", e.PostalCode);
                cmd.Parameters.AddWithValue("@Country", e.Country);
                cmd.Parameters.AddWithValue("@HomePhone", e.HomePhone);
                cmd.Parameters.AddWithValue("@Extension", e.Extension);
                cmd.Parameters.AddWithValue("@Notes", e.Notes);
                if (e.ReportsTo.HasValue)
                    cmd.Parameters.AddWithValue("@ReportsTo", e.ReportsTo.Value);
                else
                    cmd.Parameters.AddWithValue("@ReportsTo", DBNull.Value);
                if (e.Photo != null)
                    cmd.Parameters.AddWithValue("@Photo", e.Photo);
                else
                    cmd.Parameters.AddWithValue("@Photo", DBNull.Value);
                if (cn.State != ConnectionState.Open) cn.Open();
                int numRegs = cmd.ExecuteNonQuery();
                e.EmployeeID = (int)cmd.LastInsertedId;
                if (cn.State != ConnectionState.Closed) cn.Close();
                return numRegs;
            }
        }

        public int Actualizar(Empleado e)
        {
            string query = @"
                            UPDATE Employees
                            SET 
                                FirstName       = @FirstName,
                                LastName        = @LastName,
                                Title           = @Title,
                                TitleOfCourtesy = @TitleOfCourtesy,
                                BirthDate       = @BirthDate,
                                HireDate        = @HireDate,
                                Address         = @Address,
                                City            = @City,
                                Region          = @Region,
                                PostalCode      = @PostalCode,
                                Country         = @Country,
                                HomePhone       = @HomePhone,
                                Extension       = @Extension,
                                Notes           = @Notes,
                                ReportsTo       = @ReportsTo,";
            if (e.Photo != null)
                query += "Photo           = @Photo,";
            query += @"
                    RowVersion      = RowVersion + 1
                    WHERE 
                        EmployeeID = @EmployeeID
                        AND RowVersion = @RowVersion;
                    ";
            using (var cn = new MySqlConnection(_connectionString))
            using (var cmd = new MySqlCommand(query, cn))
            {
                cmd.Parameters.AddWithValue("@EmployeeID", e.EmployeeID);
                cmd.Parameters.AddWithValue("@FirstName", e.FirstName);
                cmd.Parameters.AddWithValue("@LastName", e.LastName);
                cmd.Parameters.AddWithValue("@Title", e.Title);
                cmd.Parameters.AddWithValue("@TitleOfCourtesy", e.TitleOfCourtesy);
                cmd.Parameters.AddWithValue("@BirthDate", e.BirthDate);
                cmd.Parameters.AddWithValue("@HireDate", e.HireDate);
                cmd.Parameters.AddWithValue("@Address", e.Address);
                cmd.Parameters.AddWithValue("@City", e.City);
                cmd.Parameters.AddWithValue("@Region", e.Region);
                cmd.Parameters.AddWithValue("@PostalCode", e.PostalCode);
                cmd.Parameters.AddWithValue("@Country", e.Country);
                cmd.Parameters.AddWithValue("@HomePhone", e.HomePhone);
                cmd.Parameters.AddWithValue("@Extension", e.Extension);
                cmd.Parameters.AddWithValue("@Notes", e.Notes);
                if (e.ReportsTo.HasValue)
                    cmd.Parameters.AddWithValue("@ReportsTo", e.ReportsTo.Value);
                else
                    cmd.Parameters.AddWithValue("@ReportsTo", DBNull.Value);
                // si e.Photo es null no se actualiza la foto dado que es una foto con encabezado OLE y esas fotos no se permiten actualizar
                if (e.Photo != null) 
                    cmd.Parameters.AddWithValue("@Photo", e.Photo);
                cmd.Parameters.AddWithValue("@RowVersion", e.RowVersion);
                if (cn.State != ConnectionState.Open) cn.Open();
                int numRegs = cmd.ExecuteNonQuery();
                if (cn.State != ConnectionState.Closed) cn.Close();
                return numRegs;
            }
        }

        public int Eliminar(Empleado e)
        {
            using (var cn = new MySqlConnection(_connectionString))
            using (var cmd = new MySqlCommand("DELETE FROM Employees WHERE EmployeeID = @EmployeeID AND RowVersion = @RowVersion;", cn))
            {
                cmd.Parameters.AddWithValue("@EmployeeID", e.EmployeeID);
                cmd.Parameters.AddWithValue("@RowVersion", e.RowVersion);
                if (cn.State != ConnectionState.Open) cn.Open();
                int numRegs = cmd.ExecuteNonQuery();
                if (cn.State != ConnectionState.Closed) cn.Close();
                return numRegs;
            }
        }

        public EmpleadoConReportsTo ObtenerEmpleadoConReportsTo(int id)
        {
            EmpleadoConReportsTo empleadoConReportsTo = new EmpleadoConReportsTo();
            using (var cn = new MySqlConnection(_connectionString))
            using (var cmd = new MySqlCommand(@"
                                                SELECT
                                                  e.EmployeeID,
                                                  e.FirstName,
                                                  e.LastName,
                                                  e.Title,
                                                  e.TitleOfCourtesy,
                                                  e.BirthDate,
                                                  e.HireDate,
                                                  e.Address,
                                                  e.City,
                                                  e.Region,
                                                  e.PostalCode,
                                                  e.Country,
                                                  e.HomePhone,
                                                  e.Extension,
                                                  e.Photo,
                                                  e.Notes,
                                                  e.ReportsTo,
                                                  CONCAT(r.LastName, ', ', r.FirstName) AS ReportsToName
                                                FROM Employees AS e
                                                LEFT JOIN Employees AS r
                                                  ON e.ReportsTo = r.EmployeeID
                                                WHERE e.EmployeeID = @EmployeeId;
                                               ", cn))
            {
                cmd.Parameters.AddWithValue("@EmployeeID", id);
                if (cn.State != ConnectionState.Open) cn.Open();
                using (var dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        empleadoConReportsTo = new EmpleadoConReportsTo
                        {
                            EmployeeID = dr.GetInt32(dr.GetOrdinal("EmployeeID")),
                            FirstName = dr.GetString(dr.GetOrdinal("FirstName")),
                            LastName = dr.GetString(dr.GetOrdinal("LastName")),
                            Title = dr.IsDBNull(dr.GetOrdinal("Title")) ? null : dr.GetString(dr.GetOrdinal("Title")),
                            TitleOfCourtesy = dr.IsDBNull(dr.GetOrdinal("TitleOfCourtesy")) ? null : dr.GetString(dr.GetOrdinal("TitleOfCourtesy")),
                            BirthDate = dr.IsDBNull(dr.GetOrdinal("BirthDate")) ? (DateTime?)null : dr.GetDateTime(dr.GetOrdinal("BirthDate")),
                            HireDate = dr.IsDBNull(dr.GetOrdinal("HireDate")) ? (DateTime?)null : dr.GetDateTime(dr.GetOrdinal("HireDate")),
                            Address = dr.IsDBNull(dr.GetOrdinal("Address")) ? null : dr.GetString(dr.GetOrdinal("Address")),
                            City = dr.IsDBNull(dr.GetOrdinal("City")) ? null : dr.GetString(dr.GetOrdinal("City")),
                            Region = dr.IsDBNull(dr.GetOrdinal("Region")) ? null : dr.GetString(dr.GetOrdinal("Region")),
                            PostalCode = dr.IsDBNull(dr.GetOrdinal("PostalCode")) ? null : dr.GetString(dr.GetOrdinal("PostalCode")),
                            Country = dr.IsDBNull(dr.GetOrdinal("Country")) ? null : dr.GetString(dr.GetOrdinal("Country")),
                            HomePhone = dr.IsDBNull(dr.GetOrdinal("HomePhone")) ? null : dr.GetString(dr.GetOrdinal("HomePhone")),
                            Extension = dr.IsDBNull(dr.GetOrdinal("Extension")) ? null : dr.GetString(dr.GetOrdinal("Extension")),
                            Notes = dr.IsDBNull(dr.GetOrdinal("Notes")) ? null : dr.GetString(dr.GetOrdinal("Notes")),
                            ReportsTo = dr.IsDBNull(dr.GetOrdinal("ReportsTo")) ? (int?)null : dr.GetInt32(dr.GetOrdinal("ReportsTo")),
                            ReportsToName = dr.IsDBNull(dr.GetOrdinal("ReportsToName")) ? "N/A" : dr.GetString(dr.GetOrdinal("ReportsToName")),
                            Photo = dr.IsDBNull(dr.GetOrdinal("Photo")) 
                                    ? null 
                                    : Utils.StripOleHeader(
                                        (byte[])dr["Photo"],                            // cast directo a byte[]
                                        dr.GetInt32(dr.GetOrdinal("EmployeeID")))
                        };
                    }
                }
            }
            return empleadoConReportsTo;
        }


    }
}
