using MySql.Data.MySqlClient;
using System;
using System.Data;

namespace NorthwindTradersV4MySql
{
    internal class EmpleadoRepository
    {
        private readonly string _connectionString;
        
        public EmpleadoRepository(string _connectionString) 
        {
            this._connectionString = _connectionString;
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
