using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace NorthwindTradersV4MySql
{
    internal class ClientesyProveedoresRepository
    {
        private readonly string _connectionString;

        public ClientesyProveedoresRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<ClientesyProveedores> ObtenerClientesyProveedores(string nombreDeFormulario, string comboBoxSelectedValue, bool checkBoxClientes, bool checkBoxProveedores)
        {
            string query = string.Empty;
            if (nombreDeFormulario == "FrmRptClientesyProveedoresDirectorio")
            {
                if (checkBoxClientes & checkBoxProveedores)
                    query = "Select * from Vw_ClientesProveedores Order by Relation, CompanyName";
                else if (checkBoxClientes & !checkBoxProveedores)
                    query = "Select * from Vw_ClientesProveedores Where Relation = 'Cliente' Order by CompanyName";
                else if (!checkBoxClientes & checkBoxProveedores)
                    query = "Select * from Vw_ClientesProveedores Where Relation = 'Proveedor' Order by CompanyName";
            }
            else if (nombreDeFormulario == "FrmRptClientesyProveedoresDirectorioxCiudad")
            {
                if (comboBoxSelectedValue == "aaaaa" & checkBoxClientes & checkBoxProveedores)
                    query = "Select * from Vw_ClientesProveedores Order by City, Country, CompanyName";
                else if (comboBoxSelectedValue != "aaaaa" & checkBoxClientes & checkBoxProveedores)
                    query = $"Select * from Vw_ClientesProveedores Where City = '{comboBoxSelectedValue}' Order by Country, CompanyName";
                else if (comboBoxSelectedValue == "aaaaa" & checkBoxClientes & !checkBoxProveedores)
                    query = "Select * from Vw_ClientesProveedores Where Relation = 'Cliente' Order by City, Country, CompanyName";
                else if (comboBoxSelectedValue == "aaaaa" & !checkBoxClientes & checkBoxProveedores)
                    query = "Select * from Vw_ClientesProveedores Where Relation = 'Proveedor' Order by City, Country, CompanyName";
                else if (comboBoxSelectedValue != "aaaaa" & checkBoxClientes & !checkBoxProveedores)
                    query = $"Select * from Vw_ClientesProveedores Where City = '{comboBoxSelectedValue}' And Relation = 'Cliente' Order by Country, CompanyName";
                else if (comboBoxSelectedValue != "aaaaa" & !checkBoxClientes & checkBoxProveedores)
                    query = $"Select * from Vw_ClientesProveedores Where City = '{comboBoxSelectedValue}' And Relation = 'Proveedor' Order by Country, CompanyName";
            }
            else if (nombreDeFormulario == "FrmRptClientesyProveedoresDirectorioxPais")
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
            var lista = new List<ClientesyProveedores>();
            using (var cn = new MySqlConnection(_connectionString))
            using (var cmd = new MySqlCommand(query, cn))
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
