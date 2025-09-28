using System;
using System.Collections.Generic;
using System.Data;

namespace NorthwindTradersV4MySql
{
    internal class ProductosxProveedoresRepository
    {
        private readonly string _connectionString;

        public ProductosxProveedoresRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<ProductosxProveedores> ObtenerProductosxProveedoresList()
        {
            var lista = new List<ProductosxProveedores>();
            using (var connection = new MySql.Data.MySqlClient.MySqlConnection(_connectionString))
            {
                connection.Open();
                string query = @"
                                SELECT
                                  s.CompanyName,
                                  p.ProductID,
                                  p.ProductName,
                                  p.QuantityPerUnit,
                                  p.UnitPrice,
                                  p.UnitsInStock,
                                  p.UnitsOnOrder,
                                  p.ReorderLevel,
                                  p.Discontinued,
                                  c.CategoryName
                                FROM Suppliers AS s
                                LEFT JOIN Products AS p
                                  ON p.SupplierID = s.SupplierID
                                LEFT JOIN Categories AS c
                                  ON c.CategoryID = p.CategoryID;
                                ";
                using (var command = new MySql.Data.MySqlClient.MySqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var item = new ProductosxProveedores
                            {
                                CompanyName = reader["CompanyName"].ToString(),
                                ProductID = reader["ProductID"] == DBNull.Value
                                            ? (int?)null
                                            : (int?)reader["ProductID"],
                                ProductName = (reader["ProductName"] as string) ?? "Sin producto",
                                QuantityPerUnit = reader["QuantityPerUnit"].ToString(),
                                UnitPrice = reader["UnitPrice"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(reader["UnitPrice"]),
                                UnitsInStock = reader["UnitsInStock"] == DBNull.Value ? (short?)null : Convert.ToInt16(reader["UnitsInStock"]),
                                UnitsOnOrder = reader["UnitsOnOrder"] == DBNull.Value ? (short?)null : Convert.ToInt16(reader["UnitsOnOrder"]),
                                ReorderLevel = reader["ReorderLevel"] == DBNull.Value ? (short?)null : Convert.ToInt16(reader["ReorderLevel"]),
                                CategoryName = (reader["CategoryName"] as string) ?? "Sin categoría"
                            };
                            object val = reader["Discontinued"];
                            bool discontinued = val == DBNull.Value ? false : Convert.ToBoolean(val);
                            item.Discontinued = discontinued;
                            lista.Add(item);
                        }
                    }
                }
            }
            return lista;
        }

        public List<ProductosxProveedoresConDetProv> ObtenerProductosxProveedoresConDetProvList()
        {
            var lista = new List<ProductosxProveedoresConDetProv>();
            using (var connection = new MySql.Data.MySqlClient.MySqlConnection(_connectionString))
            {
                connection.Open();
                string query = @"
                                SELECT
                                  s.SupplierID,
                                  s.CompanyName,
                                  s.ContactName,
                                  s.ContactTitle,
                                  s.Address,
                                  s.City,
                                  s.Region,
                                  s.PostalCode,
                                  s.Country,
                                  s.Phone,
                                  s.Fax,
                                  p.ProductID,
                                  p.ProductName,
                                  p.QuantityPerUnit,
                                  p.UnitPrice,
                                  p.UnitsInStock,
                                  p.UnitsOnOrder,
                                  p.ReorderLevel,
                                  p.Discontinued,
                                  c.CategoryName
                                FROM Suppliers AS s
                                LEFT JOIN Products AS p
                                  ON p.SupplierID = s.SupplierID
                                LEFT JOIN Categories AS c
                                  ON c.CategoryID = p.CategoryID
                                Order by s.CompanyName, p.ProductName;
                                ";
                using (var command = new MySql.Data.MySqlClient.MySqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var item = new ProductosxProveedoresConDetProv
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
                                Fax = reader["Fax"].ToString(),
                                ProductID = reader["ProductID"] == DBNull.Value
                                            ? (int?)null
                                            : (int?)reader["ProductID"],
                                ProductName = (reader["ProductName"] as string) ?? "Sin producto",
                                QuantityPerUnit = reader["QuantityPerUnit"].ToString(),
                                UnitPrice = reader["UnitPrice"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(reader["UnitPrice"]),
                                UnitsInStock = reader["UnitsInStock"] == DBNull.Value ? (short?)null : Convert.ToInt16(reader["UnitsInStock"]),
                                UnitsOnOrder = reader["UnitsOnOrder"] == DBNull.Value ? (short?)null : Convert.ToInt16(reader["UnitsOnOrder"]),
                                ReorderLevel = reader["ReorderLevel"] == DBNull.Value ? (short?)null : Convert.ToInt16(reader["ReorderLevel"]),
                                CategoryName = (reader["CategoryName"] as string) ?? "Sin categoría"
                            };
                            object val = reader["Discontinued"];
                            bool discontinued = val == DBNull.Value ? false : Convert.ToBoolean(val);
                            item.Discontinued = discontinued;
                            lista.Add(item);
                        }
                    }
                }
            }
            return lista;
        }

        public DataSet ObtenerProveedoresProductosDataSet()
        {
            var ds = new DataSet();
            ds.Locale = System.Globalization.CultureInfo.InvariantCulture;
            using (var connection = new MySql.Data.MySqlClient.MySqlConnection(_connectionString))
            {
                connection.Open();
                string queryProveedores = "Select * from Suppliers Order by SupplierId Desc";
                using (var dapProveedores = new MySql.Data.MySqlClient.MySqlDataAdapter(queryProveedores, connection))
                    dapProveedores.Fill(ds, "Proveedores");
                string queryProductos = @"
                                SELECT
                                  Products.ProductID,
                                  Products.ProductName,
                                  Products.QuantityPerUnit,
                                  Products.UnitPrice,
                                  Products.UnitsInStock,
                                  Products.UnitsOnOrder,
                                  Products.ReorderLevel,
                                  Products.Discontinued,
                                  Categories.CategoryName,
                                  Categories.Description,
                                  Suppliers.CompanyName,
                                  Categories.CategoryID,
                                  Suppliers.SupplierID
                                FROM Products
                                LEFT OUTER JOIN Categories
                                  ON Products.CategoryID = Categories.CategoryID
                                LEFT OUTER JOIN Suppliers
                                  ON Products.SupplierID = Suppliers.SupplierID
                                ORDER BY Products.ProductName;
                                ";
                using (var dapProductos = new MySql.Data.MySqlClient.MySqlDataAdapter(queryProductos, connection))
                    dapProductos.Fill(ds, "Productos");
            }
            // en la siguiente instrucción se deben de proporcionar los nombres de los campos (alias) que devuelve el store procedure
            DataRelation dataRelation = new DataRelation("ProveedoresProductos", ds.Tables["Proveedores"].Columns["SupplierID"], ds.Tables["Productos"].Columns["SupplierID"]);
            ds.Relations.Add(dataRelation);
            return ds;
        }
    }
}
