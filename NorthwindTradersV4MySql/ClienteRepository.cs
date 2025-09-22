using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace NorthwindTradersV4MySql
{
    internal class ClienteRepository
    {
        private readonly string _connectionString;

        public ClienteRepository(string _connectionString)
        {
            this._connectionString = _connectionString;
        }

        public DataTable ObtenerPaisesClientes()
        {
            DataTable dt = new DataTable();
            using (var cn = new MySqlConnection(_connectionString))
            using (var cmd = new MySqlCommand("SELECT '' As Id, '»--- Seleccione ---«' As País UNION ALL SELECT DISTINCT Country As Id, Country As País FROM Customers ORDER BY País;", cn))
            using (var da = new MySqlDataAdapter(cmd))
                da.Fill(dt);
            return dt;
        }

        public DataTable ObtenerClientes(object sender, Cliente clienteBuscar)
        {
            DataTable dt = new DataTable();
            string query;
            if (sender == null)
            {
                query = @"
                            SELECT 
                              CustomerID AS Id,
                              CompanyName AS `Nombre de compañía`,
                              ContactName AS `Nombre de contacto`,
                              ContactTitle AS `Título de contacto`,
                              Address AS Domicilio,
                              City AS Ciudad,
                              Region AS Región,
                              PostalCode AS `Código postal`,
                              Country AS País,
                              Phone AS Teléfono,
                              Fax
                            FROM Customers
                            LIMIT 20;
                            ";
            }
            else
            {
                query = @"
                            SELECT 
                              CustomerID AS Id,
                              CompanyName AS `Nombre de compañía`,
                              ContactName AS `Nombre de contacto`,
                              ContactTitle AS `Título de contacto`,
                              Address AS Domicilio,
                              City AS Ciudad,
                              Region AS Región,
                              PostalCode AS `Código postal`,
                              Country AS País,
                              Phone AS Teléfono,
                              Fax
                            FROM Customers
                            WHERE
                              (@Id = '' OR CustomerID LIKE CONCAT('%', @Id, '%')) AND
                              (@Compañia = '' OR CompanyName LIKE CONCAT('%', @Compañia, '%')) AND
                              (@Contacto = '' OR ContactName LIKE CONCAT('%', @Contacto, '%')) AND
                              (@Domicilio = '' OR Address LIKE CONCAT('%', @Domicilio, '%')) AND
                              (@Ciudad = '' OR City LIKE CONCAT('%', @Ciudad, '%')) AND
                              (@Region = '' OR Region LIKE CONCAT('%', @Region, '%')) AND
                              (@CodigoP = '' OR PostalCode LIKE CONCAT('%', @CodigoP, '%')) AND
                              (@Pais = '' OR Country LIKE CONCAT('%', @Pais, '%')) AND
                              (@Telefono = '' OR Phone LIKE CONCAT('%', @Telefono, '%')) AND
                              (@Fax = '' OR Fax LIKE CONCAT('%', @Fax, '%'))
                            ORDER BY CustomerID;
                            ";
            }
            using (var cn = new MySqlConnection(_connectionString))
            using (var cmd = new MySqlCommand(query, cn))
            {
                if (sender != null)
                {
                    cmd.Parameters.AddWithValue("@Id", clienteBuscar.CustomerID);
                    cmd.Parameters.AddWithValue("@Compañia", clienteBuscar.CompanyName);
                    cmd.Parameters.AddWithValue("@Contacto", clienteBuscar.ContactName);
                    cmd.Parameters.AddWithValue("@Domicilio", clienteBuscar.Address);
                    cmd.Parameters.AddWithValue("@Ciudad", clienteBuscar.City);
                    cmd.Parameters.AddWithValue("@Region", clienteBuscar.Region);
                    cmd.Parameters.AddWithValue("@CodigoP", clienteBuscar.PostalCode);
                    cmd.Parameters.AddWithValue("@Pais", clienteBuscar.Country);
                    cmd.Parameters.AddWithValue("@Telefono", clienteBuscar.Phone);
                    cmd.Parameters.AddWithValue("@Fax", clienteBuscar.Fax);
                }
                using (var da = new MySqlDataAdapter(cmd))
                    da.Fill(dt);
            }
            return dt;
        }

        public Cliente ObtenerCliente(Cliente cliente)
        {
            string query = @"
                            Select * 
                            From Customers
                            Where CustomerID = @Id;
                            ";
            using (var cn = new MySqlConnection(_connectionString))
            using (var cmd = new MySqlCommand(query, cn))
            {
                cmd.Parameters.AddWithValue("@Id", cliente.CustomerID);
                if (cn.State != ConnectionState.Open) cn.Open();
                using (MySqlDataReader rdr = cmd.ExecuteReader())
                {
                    if (rdr.Read())
                    {
                        cliente.RowVersion = (int)rdr["RowVersion"];
                        cliente.CompanyName = rdr["CompanyName"].ToString();
                        cliente.ContactName = rdr["ContactName"].ToString();
                        cliente.ContactTitle = rdr["ContactTitle"].ToString();
                        cliente.Address = rdr["Address"].ToString();
                        cliente.City = rdr["City"].ToString();
                        cliente.Region = rdr["Region"].ToString();
                        cliente.PostalCode = rdr["PostalCode"].ToString();
                        cliente.Country = rdr["Country"].ToString();
                        cliente.Phone = rdr["Phone"].ToString();
                        cliente.Fax = rdr["Fax"].ToString();
                    }
                    else
                        cliente = null;
                }
            }
            return cliente;
        }

        public int Insertar(Cliente cliente)
        {
            string query = @"
                            INSERT INTO `Customers`
                            (
                              `CustomerID`,
                              `CompanyName`,
                              `ContactName`,
                              `ContactTitle`,
                              `Address`,
                              `City`,
                              `Region`,
                              `PostalCode`,
                              `Country`,
                              `Phone`,
                              `Fax`,
                              RowVersion
                            )
                            VALUES
                            (
                              @CustomerID,
                              @CompanyName,
                              @ContactName,
                              @ContactTitle,
                              @Address,
                              @City,
                              @Region,
                              @PostalCode,
                              @Country,
                              @Phone,
                              @Fax,
                              1
                            );
                            ";
            using (var cn = new MySqlConnection(_connectionString))
            using (var cmd = new MySqlCommand(query, cn))
            {
                cmd.Parameters.Add("CustomerID", MySqlDbType.VarChar, 5).Value = cliente.CustomerID;
                cmd.Parameters.Add("CompanyName", MySqlDbType.VarChar, 40)
                   .Value = cliente.CompanyName;
                cmd.Parameters.Add("ContactName", MySqlDbType.VarChar, 30)
                   .Value = cliente.ContactName;
                cmd.Parameters.Add("ContactTitle", MySqlDbType.VarChar, 30)
                   .Value = cliente.ContactTitle;
                cmd.Parameters.Add("Address", MySqlDbType.VarChar, 60)
                   .Value = cliente.Address;
                cmd.Parameters.Add("City", MySqlDbType.VarChar, 15)
                   .Value = cliente.City;
                cmd.Parameters.Add("Country", MySqlDbType.VarChar, 15)
                   .Value = cliente.Country;
                cmd.Parameters.Add("Phone", MySqlDbType.VarChar, 24)
                   .Value = cliente.Phone;

                // Parámetros que pueden ser null
                cmd.Parameters.Add("Region", MySqlDbType.VarChar, 15)
                   .Value = string.IsNullOrWhiteSpace(cliente.Region)
                               ? (object)DBNull.Value
                               : cliente.Region;

                cmd.Parameters.Add("PostalCode", MySqlDbType.VarChar, 10)
                   .Value = string.IsNullOrWhiteSpace(cliente.PostalCode)
                               ? (object)DBNull.Value
                               : cliente.PostalCode;

                cmd.Parameters.Add("Fax", MySqlDbType.VarChar, 24)
                   .Value = string.IsNullOrWhiteSpace(cliente.Fax)
                               ? (object)DBNull.Value
                               : cliente.Fax;
                if (cn.State != ConnectionState.Open) cn.Open();
                int numRegs = cmd.ExecuteNonQuery();
                return numRegs;
            }
        }

        public int Actualizar(Cliente cliente)
        {
            string query = @"
                            UPDATE `Customers`
                            SET
                              `CompanyName`  = @CompanyName,
                              `ContactName`  = @ContactName,
                              `ContactTitle` = @ContactTitle,
                              `Address`      = @Address,
                              `City`         = @City,
                              `Region`       = @Region,
                              `PostalCode`   = @PostalCode,
                              `Country`      = @Country,
                              `Phone`        = @Phone,
                              `Fax`          = @Fax,
                              `RowVersion`   = `RowVersion` + 1
                            WHERE
                              `CustomerID`   = @CustomerID
                               AND RowVersion = @RowVersion;
                            ";
            using (var cn = new MySqlConnection(_connectionString))
            using (var cmd = new MySqlCommand(query, cn))
            {
                // Parámetros de clave y concurrencia
                cmd.Parameters.Add("@CustomerID", MySqlDbType.VarChar, 5)
                    .Value = cliente.CustomerID.Trim();
                cmd.Parameters.Add("@RowVersion", MySqlDbType.Int32)
                   .Value = cliente.RowVersion;

                // Parámetros obligatorios de actualización
                cmd.Parameters.Add("@CompanyName", MySqlDbType.VarChar, 40)
                   .Value = cliente.CompanyName.Trim();
                cmd.Parameters.Add("@ContactName", MySqlDbType.VarChar, 30)
                   .Value = cliente.ContactName.Trim();
                cmd.Parameters.Add("@ContactTitle", MySqlDbType.VarChar, 30)
                   .Value = cliente.ContactTitle.Trim();
                cmd.Parameters.Add("@Address", MySqlDbType.VarChar, 60)
                   .Value = cliente.Address.Trim();
                cmd.Parameters.Add("@City", MySqlDbType.VarChar, 15)
                   .Value = cliente.City.Trim();
                cmd.Parameters.Add("@Country", MySqlDbType.VarChar, 15)
                   .Value = cliente.Country.Trim();
                cmd.Parameters.Add("@Phone", MySqlDbType.VarChar, 24)
                   .Value = cliente.Phone.Trim();

                // Parámetros que pueden ser NULL
                cmd.Parameters.Add("@Region", MySqlDbType.VarChar, 15)
                   .Value = string.IsNullOrWhiteSpace(cliente.Region)
                               ? (object)DBNull.Value
                               : cliente.Region.Trim();
                cmd.Parameters.Add("@PostalCode", MySqlDbType.VarChar, 10)
                   .Value = string.IsNullOrWhiteSpace(cliente.PostalCode)
                               ? (object)DBNull.Value
                               : cliente.PostalCode.Trim();
                cmd.Parameters.Add("@Fax", MySqlDbType.VarChar, 24)
                   .Value = string.IsNullOrWhiteSpace(cliente.Fax)
                               ? (object)DBNull.Value
                               : cliente.Fax.Trim();
                if (cn.State != ConnectionState.Open) cn.Open();
                int numRegs = cmd.ExecuteNonQuery();
                return numRegs;
            }
        }

        public int Eliminar(Cliente cliente)
        {
            using (var cn = new MySqlConnection(_connectionString))
            using (var cmd = new MySqlCommand("DELETE FROM Customers WHERE CustomerID = @CustomerID AND RowVersion = @RowVersion;", cn))
            {
                cmd.Parameters.AddWithValue("@CustomerID", cliente.CustomerID);
                cmd.Parameters.AddWithValue("@RowVersion", cliente.RowVersion);
                if (cn.State != ConnectionState.Open) cn.Open();
                int numRegs = cmd.ExecuteNonQuery();
                return numRegs;
            }
        }

        public DataTable ObtenerDirectorioClientesProveedores(string nombreDeFormulario, string  comboBoxSelectedValue, bool checkBoxClientes, bool checkBoxProveedores)
        {
            string query = string.Empty;
            if (nombreDeFormulario == "FrmClientesyProveedoresDirectorio")
            {
                if (checkBoxClientes & checkBoxProveedores)
                    query = "Select * from VW_ClientesProveedores Order by Relation, CompanyName;";
                else if (checkBoxClientes & !checkBoxProveedores)
                    query = "Select * from VW_ClientesProveedores Where Relation = 'Cliente' Order by CompanyName;";
                else if (!checkBoxClientes & checkBoxProveedores)
                    query = "Select * from VW_ClientesProveedores Where Relation = 'Proveedor' Order by CompanyName;";
            }
            else if (nombreDeFormulario == "FrmClientesyProveedoresDirectorioxCiudad")
            {
                if (comboBoxSelectedValue == "aaaaa" & checkBoxClientes & checkBoxProveedores)
                {
                    query = "Select * from Vw_ClientesProveedores Order by City, Country, CompanyName";
                }
                else if (comboBoxSelectedValue != "aaaaa" & checkBoxClientes & checkBoxProveedores)
                {
                    query = $"Select * from Vw_ClientesProveedores Where City = '{comboBoxSelectedValue}' Order by Country, CompanyName";
                }
                else if (comboBoxSelectedValue == "aaaaa" & checkBoxClientes & !checkBoxProveedores)
                {
                    query = "Select * from Vw_ClientesProveedores Where Relation = 'Cliente' Order by City, Country, CompanyName";
                }
                else if (comboBoxSelectedValue == "aaaaa" & !checkBoxClientes & checkBoxProveedores)
                {
                    query = "Select * from Vw_ClientesProveedores Where Relation = 'Proveedor' Order by City, Country, CompanyName";
                }
                else if (comboBoxSelectedValue != "aaaaa" & checkBoxClientes & !checkBoxProveedores)
                {
                    query = $"Select * from Vw_ClientesProveedores Where City = '{comboBoxSelectedValue}' And Relation = 'Cliente' Order by Country, CompanyName";
                }
                else if (comboBoxSelectedValue != "aaaaa" & !checkBoxClientes & checkBoxProveedores)
                {
                    query = $"Select * from Vw_ClientesProveedores Where City = '{comboBoxSelectedValue}' And Relation = 'Proveedor' Order by Country, CompanyName";
                }
            }
            else if (nombreDeFormulario == "FrmClientesyProveedoresDirectorioxPais")
            {
                if (comboBoxSelectedValue == "aaaaa" & checkBoxClientes & checkBoxProveedores)
                    query = "Select * from Vw_ClientesProveedores Order by Country, City, CompanyName";
                else if (comboBoxSelectedValue != "aaaaa" & checkBoxClientes & checkBoxProveedores)
                    query = $"Select * from Vw_ClientesProveedores Where Country = '{comboBoxSelectedValue}' Order by City, CompanyName";
                else if (comboBoxSelectedValue == "aaaaa" & checkBoxClientes & !checkBoxProveedores)
                    query = "Select * from Vw_ClientesProveedores Where Relation = 'Cliente' Order by Country, City, CompanyName";
                else if (comboBoxSelectedValue == "aaaaa" & !checkBoxClientes & checkBoxProveedores)
                    query = "Select * from Vw_ClientesProveedores Where Relation = 'Proveedor' Order by Country, City, CompanyName";
                else if (comboBoxSelectedValue != "aaaaa" & checkBoxClientes & !checkBoxProveedores)
                    query = $"Select * from Vw_ClientesProveedores Where Country = '{comboBoxSelectedValue}' And Relation = 'Cliente' Order by City, CompanyName";
                else if (comboBoxSelectedValue != "aaaaa" & !checkBoxClientes & checkBoxProveedores)
                    query = $"Select * from Vw_ClientesProveedores Where Country = '{comboBoxSelectedValue}' And Relation = 'Proveedor' Order by City, CompanyName";
            }
            var dt = new DataTable();
            using (var cn = new MySqlConnection(_connectionString))
            using (var cmd = new MySqlCommand(query, cn))
            using (var da = new MySqlDataAdapter(cmd))
                da.Fill(dt);
            return dt;
        }

        public DataTable ObtenerComboClientesProveedoresCiudad()
        {
            string query = @"
                            SELECT ''          AS Ciudad,
                                   '»--- Seleccione ---«' AS CiudadPaís
                            UNION
                            SELECT 'aaaaa'     AS Ciudad,
                                   '»--- Todas las ciudades ---«' AS CiudadPaís
                            UNION
                            SELECT City        AS Ciudad,
                                   CONCAT(City, ', ', Country) AS CiudadPaís
                              FROM Customers
                            UNION
                            SELECT City        AS Ciudad,
                                   CONCAT(City, ', ', Country) AS CiudadPaís
                              FROM Suppliers
                            ORDER BY Ciudad;
                            ";
            var dt = new DataTable();
            using (var cn = new MySqlConnection(_connectionString))
            using (var cmd = new MySqlCommand(query, cn))
            using (var da = new MySqlDataAdapter(cmd))
                da.Fill(dt);
            return dt;
        }

        public DataTable ObtenerComboClientesProveedoresPais()
        {
            DataTable dt = new DataTable();
            using (var cn = new MySqlConnection(_connectionString))
            using (var cmd = new MySqlCommand(@" 
                            SELECT '' AS IdPaís, '»--- Seleccione ---«' AS País
                            UNION ALL
                            SELECT 'aaaaa' AS IdPaís, '»--- Todos los países ---«' AS País
                            UNION ALL
                            SELECT Country AS IdPaís, Country AS País FROM Customers
                            UNION
                            SELECT Country AS IdPaís, Country AS País 
                            FROM Suppliers
                            ORDER BY País;", cn))
            using (var da = new MySqlDataAdapter(cmd))
                da.Fill(dt);
            return dt;
        }

        public List<Cliente> ObtenerClientesList()
        {
            const string query = @"
                            SELECT 
                              CustomerID,
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
                            FROM Customers
                            ORDER BY CustomerID;
                            ";
            var clientes = new List<Cliente>();
            using (var cn = new MySqlConnection(_connectionString))
            using (var cmd = new MySqlCommand(query, cn))
            {
                if (cn.State != ConnectionState.Open) cn.Open();
                using (MySqlDataReader rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        var cliente = new Cliente
                        {
                            CustomerID = rdr["CustomerId"].ToString(),
                            CompanyName = rdr["CompanyName"].ToString(),
                            ContactName = rdr["ContactName"].ToString(),
                            ContactTitle = rdr["ContactTitle"].ToString(),
                            Address = rdr["Address"].ToString(),
                            City = rdr["City"].ToString(),
                            Region = rdr["Region"].ToString(),
                            PostalCode = rdr["PostalCode"].ToString(),
                            Country = rdr["Country"].ToString(),
                            Phone = rdr["Phone"].ToString(),
                            Fax = rdr["Fax"].ToString()
                        };
                        clientes.Add(cliente);
                    }
                }
            }
            return clientes;
        }
    }
}
