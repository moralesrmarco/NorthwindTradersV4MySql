using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace NorthwindTradersV4MySql
{
    internal class ProveedorRepository
    {
        private readonly string _connectionString;
        
        public ProveedorRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Proveedor> ObtenerProveedoresList()
        {
            var lista = new List<Proveedor>();
            using (var connection = new MySql.Data.MySqlClient.MySqlConnection(_connectionString))
            {
                connection.Open();
                string query = "Select * from Suppliers Order by CompanyName";
                using (var command = new MySql.Data.MySqlClient.MySqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var proveedor = new Proveedor
                            {
                                SupplierID = (int)reader["SupplierID"],
                                CompanyName = reader["CompanyName"].ToString(),
                                ContactName = reader["ContactName"].ToString(),
                                ContactTitle = reader["ContactTitle"].ToString(),
                                Address = reader["Address"].ToString(),
                                City = reader["City"].ToString(),
                                Region = reader["Region"].ToString(),
                                PostalCode = reader["PostalCode"].ToString(),
                                Country = reader["Country"].ToString(),
                                Phone = reader["Phone"].ToString(),
                                Fax = reader["Fax"].ToString()
                            };
                            lista.Add(proveedor);
                        }
                    }
                }
            }
            return lista;
        }

        public DataTable ObtenerPaisesProveedores()
        {
            var dt = new DataTable();
            using (var cn = new MySqlConnection(_connectionString))
            using (var cmd = new MySqlCommand("SELECT '' As Id, '»--- Seleccione ---«' As Pais UNION ALL SELECT DISTINCT Country As Id, Country As Pais FROM Suppliers ORDER BY Pais;", cn))
            using (var da = new MySqlDataAdapter(cmd))
                da.Fill(dt);
            return dt;
        }

        public DataTable ObtenerProveedores(object sender, DtoProveedoresBuscar dtoProveedoresBuscar)
        {
            var dt = new DataTable();
            string query;
            if (sender == null)
            {
                query = @"
                        SELECT SupplierID, CompanyName, ContactName, ContactTitle, Address, City, Region, PostalCode, Country, Phone, Fax
                        FROM Suppliers
                        ORDER BY SupplierID DESC
                        LIMIT 20;
                        ";
            }
            else
            {
                query = @"
                        SELECT
                          SupplierID,
                          CompanyName,
                          ContactName,
                          ContactTitle,
                          Address,
                          City,
                          Region,
                          PostalCode,
                          Country,
                          Phone,
                          Fax
                        FROM Suppliers
                        WHERE
                          (@IdIni = 0 OR SupplierID BETWEEN @IdIni AND @IdFin) AND
                          (@CompanyName = '' OR CompanyName LIKE CONCAT('%', @CompanyName, '%')) AND
                          (@ContactName = '' OR ContactName LIKE CONCAT('%', @ContactName, '%')) AND
                          (@Address = '' OR Address LIKE CONCAT('%', @Address, '%')) AND
                          (@City = '' OR City LIKE CONCAT('%', @City, '%')) AND
                          (@Region = '' OR Region LIKE CONCAT('%', @Region, '%')) AND
                          (@PostalCode = '' OR PostalCode LIKE CONCAT('%', @PostalCode, '%')) AND
                          (@Country = '' OR Country LIKE CONCAT('%', @Country, '%')) AND
                          (@Phone = '' OR Phone LIKE CONCAT('%', @Phone, '%')) AND
                          (@Fax = '' OR Fax LIKE CONCAT('%', @Fax, '%'))
                        ORDER BY SupplierID DESC;
                        ";
            }
            using (var cn = new MySqlConnection(_connectionString))
            using (var cmd = new MySqlCommand(query, cn))
            using (var da = new MySqlDataAdapter(cmd))
            {
                if (sender != null)
                {
                    cmd.Parameters.AddWithValue("@IdIni", dtoProveedoresBuscar.IdIni);
                    cmd.Parameters.AddWithValue("@IdFin", dtoProveedoresBuscar.IdFin);
                    cmd.Parameters.AddWithValue("@CompanyName", dtoProveedoresBuscar.CompanyName);
                    cmd.Parameters.AddWithValue("@ContactName", dtoProveedoresBuscar.ContactName);
                    cmd.Parameters.AddWithValue("@Address", dtoProveedoresBuscar.Address);
                    cmd.Parameters.AddWithValue("@City", dtoProveedoresBuscar.City);
                    cmd.Parameters.AddWithValue("@Region", dtoProveedoresBuscar.Region);
                    cmd.Parameters.AddWithValue("@PostalCode", dtoProveedoresBuscar.PostalCode);
                    cmd.Parameters.AddWithValue("@Country", dtoProveedoresBuscar.Country);
                    cmd.Parameters.AddWithValue("@Phone", dtoProveedoresBuscar.Phone);
                    cmd.Parameters.AddWithValue("@Fax", dtoProveedoresBuscar.Fax);
                }
                da.Fill(dt);
            }
            return dt;
        }

        public Proveedor ObtenerProveedor(Proveedor proveedor)
        {
            string query = @"
                        SELECT *
                        FROM Suppliers
                        WHERE SupplierID = @SupplierID;
                        ";
            using (var cn = new MySqlConnection(_connectionString))
            using (var cmd = new MySqlCommand(query, cn))
            {
                cmd.Parameters.AddWithValue("@SupplierID", proveedor.SupplierID);
                cn.Open();
                using (var rdr = cmd.ExecuteReader())
                {
                    if (rdr.Read())
                    {
                        proveedor.RowVersion = rdr.IsDBNull(rdr.GetOrdinal("RowVersion")) ? 0 : rdr.GetInt32(rdr.GetOrdinal("RowVersion"));
                        proveedor.CompanyName = rdr.GetString(rdr.GetOrdinal("CompanyName"));
                        proveedor.ContactName = rdr.IsDBNull(rdr.GetOrdinal("ContactName")) ? null : rdr.GetString(rdr.GetOrdinal("ContactName"));
                        proveedor.ContactTitle = rdr.IsDBNull(rdr.GetOrdinal("ContactTitle")) ? null : rdr.GetString(rdr.GetOrdinal("ContactTitle"));
                        proveedor.Address = rdr.IsDBNull(rdr.GetOrdinal("Address")) ? null : rdr.GetString(rdr.GetOrdinal("Address"));
                        proveedor.City = rdr.IsDBNull(rdr.GetOrdinal("City")) ? null : rdr.GetString(rdr.GetOrdinal("City"));
                        proveedor.Region = rdr.IsDBNull(rdr.GetOrdinal("Region")) ? null : rdr.GetString(rdr.GetOrdinal("Region"));
                        proveedor.PostalCode = rdr.IsDBNull(rdr.GetOrdinal("PostalCode")) ? null : rdr.GetString(rdr.GetOrdinal("PostalCode"));
                        proveedor.Country = rdr.IsDBNull(rdr.GetOrdinal("Country")) ? null : rdr.GetString(rdr.GetOrdinal("Country"));
                        proveedor.Phone = rdr.IsDBNull(rdr.GetOrdinal("Phone")) ? null : rdr.GetString(rdr.GetOrdinal("Phone"));
                        proveedor.Fax = rdr.IsDBNull(rdr.GetOrdinal("Fax")) ? null : rdr.GetString(rdr.GetOrdinal("Fax"));
                    }
                    else
                        proveedor = null;
                }
            }
            return proveedor;
        }

        public int Insertar(Proveedor proveedor)
        {
            using (var cn = new MySqlConnection(_connectionString))
            using (var cmd = new MySqlCommand(@"
                                                INSERT INTO `suppliers`
                                                (
                                                `CompanyName`,
                                                `ContactName`,
                                                `ContactTitle`,
                                                `Address`,
                                                `City`,
                                                `Region`,
                                                `PostalCode`,
                                                `Country`,
                                                `Phone`,
                                                `Fax`
                                                )
                                                VALUES
                                                (
                                                @CompanyName,
                                                @ContactName,
                                                @ContactTitle,
                                                @Address,
                                                @City,
                                                @Region,
                                                @PostalCode,
                                                @Country,
                                                @Phone,
                                                @Fax
                                                );
                                                ", cn))
            {
                cmd.Parameters.AddWithValue("@CompanyName", proveedor.CompanyName);
                cmd.Parameters.AddWithValue("@ContactName", string.IsNullOrEmpty(proveedor.ContactName) ? (object)DBNull.Value : proveedor.ContactName);
                cmd.Parameters.AddWithValue("@ContactTitle", string.IsNullOrEmpty(proveedor.ContactTitle) ? (object)DBNull.Value : proveedor.ContactTitle);
                cmd.Parameters.AddWithValue("@Address", string.IsNullOrEmpty(proveedor.Address) ? (object)DBNull.Value : proveedor.Address);
                cmd.Parameters.AddWithValue("@City", string.IsNullOrEmpty(proveedor.City) ? (object)DBNull.Value : proveedor.City);
                cmd.Parameters.AddWithValue("@Region", string.IsNullOrEmpty(proveedor.Region) ? (object)DBNull.Value : proveedor.Region);
                cmd.Parameters.AddWithValue("@PostalCode", string.IsNullOrEmpty(proveedor.PostalCode) ? (object)DBNull.Value : proveedor.PostalCode);
                cmd.Parameters.AddWithValue("@Country", string.IsNullOrEmpty(proveedor.Country) ? (object)DBNull.Value : proveedor.Country);
                cmd.Parameters.AddWithValue("@Phone", string.IsNullOrEmpty(proveedor.Phone) ? (object)DBNull.Value : proveedor.Phone);
                cmd.Parameters.AddWithValue("@Fax", string.IsNullOrEmpty(proveedor.Fax) ? (object)DBNull.Value : proveedor.Fax);
                cn.Open();
                int numRegs = cmd.ExecuteNonQuery();
                proveedor.SupplierID = (int)cmd.LastInsertedId;
                return numRegs;
            }
        }

        public int Actualizar(Proveedor proveedor)
        {
            string query = @"
                        UPDATE `suppliers`
                        SET
                          `CompanyName` = @CompanyName,
                          `ContactName` = @ContactName,
                          `ContactTitle` = @ContactTitle,
                          `Address` = @Address,
                          `City` = @City,
                          `Region` = @Region,
                          `PostalCode` = @PostalCode,
                          `Country` = @Country,
                          `Phone` = @Phone,
                          `Fax` = @Fax,
                          `RowVersion` = `RowVersion` + 1
                        WHERE
                          SupplierID = @SupplierID AND
                          RowVersion = @RowVersion;
                        ";
            using (var cn = new MySqlConnection(_connectionString))
            using (var cmd = new MySqlCommand(query, cn))
            {
                cmd.Parameters.AddWithValue("@SupplierID", proveedor.SupplierID);
                cmd.Parameters.AddWithValue("@CompanyName", proveedor.CompanyName);
                cmd.Parameters.AddWithValue("@ContactName", string.IsNullOrEmpty(proveedor.ContactName) ? (object)DBNull.Value : proveedor.ContactName);
                cmd.Parameters.AddWithValue("@ContactTitle", string.IsNullOrEmpty(proveedor.ContactTitle) ? (object)DBNull.Value : proveedor.ContactTitle);
                cmd.Parameters.AddWithValue("@Address", string.IsNullOrEmpty(proveedor.Address) ? (object)DBNull.Value : proveedor.Address);
                cmd.Parameters.AddWithValue("@City", string.IsNullOrEmpty(proveedor.City) ? (object)DBNull.Value : proveedor.City);
                cmd.Parameters.AddWithValue("@Region", string.IsNullOrEmpty(proveedor.Region) ? (object)DBNull.Value : proveedor.Region);
                cmd.Parameters.AddWithValue("@PostalCode", string.IsNullOrEmpty(proveedor.PostalCode) ? (object)DBNull.Value : proveedor.PostalCode);
                cmd.Parameters.AddWithValue("@Country", string.IsNullOrEmpty(proveedor.Country) ? (object)DBNull.Value : proveedor.Country);
                cmd.Parameters.AddWithValue("@Phone", string.IsNullOrEmpty(proveedor.Phone) ? (object)DBNull.Value : proveedor.Phone);
                cmd.Parameters.AddWithValue("@Fax", string.IsNullOrEmpty(proveedor.Fax) ? (object)DBNull.Value : proveedor.Fax);
                cmd.Parameters.AddWithValue("@RowVersion", proveedor.RowVersion);
                cn.Open();
                int numRegs = cmd.ExecuteNonQuery();
                return numRegs;
            }
        }

        public int Eliminar(Proveedor proveedor)
        {
            string query = @"
                        DELETE FROM `suppliers`
                        WHERE
                          SupplierID = @SupplierID AND
                          RowVersion = @RowVersion;
                        ";
            using (var cn = new MySqlConnection(_connectionString))
            using (var cmd = new MySqlCommand(query, cn))
            {
                cmd.Parameters.AddWithValue("@SupplierID", proveedor.SupplierID);
                cmd.Parameters.AddWithValue("@RowVersion", proveedor.RowVersion);
                cn.Open();
                int numRegs = cmd.ExecuteNonQuery();
                return numRegs;
            }
        }
    }
}
