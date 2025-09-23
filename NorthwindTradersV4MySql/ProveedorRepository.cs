using System.Collections.Generic;

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


    }
}
