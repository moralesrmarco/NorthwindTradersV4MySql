using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace NorthwindTradersV4MySql
{
    internal class ClientesyProveedoresRepository
    {
        private readonly string _connectionString;

        public ClientesyProveedoresRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<ClientesyProveedores> ObtenerClientesyProveedores(bool checkBoxClientes, bool checkBoxProveedores)
        {
            string query = string.Empty;
            if (checkBoxClientes & checkBoxProveedores)
            {
                query = "Select * from Vw_ClientesProveedores Order by Relation, CompanyName";
            } else if (checkBoxClientes & !checkBoxProveedores)
            {
                query = "Select * from Vw_ClientesProveedores Where Relation = 'Cliente' Order by CompanyName";
            }
            else if (!checkBoxClientes & checkBoxProveedores)
            {
                query = "Select * from Vw_ClientesProveedores Where Relation = 'Proveedor' Order by CompanyName";
            }
            var lista = new List<ClientesyProveedores>();
            using (var cn = new MySql.Data.MySqlClient.MySqlConnection(_connectionString))
            using (var cmd = new MySql.Data.MySqlClient.MySqlCommand(query, cn))
            {
                if (cn.State != ConnectionState.Open) cn.Open();
                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        var item = new ClientesyProveedores
                        {
                            CompanyName = dr["CompanyName"] == DBNull.Value ? string.Empty : dr["CompanyName"].ToString(),
                            Contact = dr["Contact"] == DBNull.Value ? string.Empty : dr["Contact"].ToString(),
                            Relation = dr["Relation"] == DBNull.Value ? string.Empty : dr["Relation"].ToString(),
                            Address = dr["Address"] == DBNull.Value ? string.Empty : dr["Address"].ToString(),
                            City = dr["City"] == DBNull.Value ? string.Empty : dr["City"].ToString(),
                            Region = dr["Region"] == DBNull.Value ? string.Empty : dr["Region"].ToString(),
                            PostalCode = dr["PostalCode"] == DBNull.Value ? string.Empty : dr["PostalCode"].ToString(),
                            Country = dr["Country"] == DBNull.Value ? string.Empty : dr["Country"].ToString(),
                            Phone = dr["Phone"] == DBNull.Value ? string.Empty : dr["Phone"].ToString(),
                            Fax = dr["Fax"] == DBNull.Value ? string.Empty : dr["Fax"].ToString()
                        };
                        lista.Add(item);
                    }
                }
            }
            return lista;
        }
    }
}
