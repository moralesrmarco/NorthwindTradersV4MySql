using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NorthwindTradersV4MySql
{
    internal class CategoriaRepository
    {
        private readonly string _connectionString;

        public CategoriaRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public DataTable ObtenerCategorias(object sender, DtoCategoriasBuscar dtoCategoriasBuscar)
        {
            var dt = new DataTable();
            string query;
            if (sender == null)
            {
                query = "SELECT CategoryID, CategoryName, Description, Picture FROM Categories ORDER BY CategoryId Desc LIMIT 20;";
            }
            else
            {
                query = @"SELECT CategoryID, CategoryName, Description, Picture FROM Categories 
                        WHERE 
                        (@IdIni = 0 OR CategoryID BETWEEN @IdIni AND @IdFin) AND 
                        (@CategoryName = '' OR CategoryName LIKE CONCAT('%',@CategoryName, '%'))
                        ORDER BY CategoryId Desc;
                        ";
            }
            using (var cn = new MySqlConnection(_connectionString))
            using (var cmd = new MySqlCommand(query, cn))
            using (var da = new MySqlDataAdapter(cmd))
            {
                if (sender != null)
                {
                    cmd.Parameters.AddWithValue("@IdIni", dtoCategoriasBuscar.IdIni);
                    cmd.Parameters.AddWithValue("@IdFin", dtoCategoriasBuscar.IdFin);
                    cmd.Parameters.AddWithValue("@CategoryName", dtoCategoriasBuscar.CategoryName ?? string.Empty);
                }
                da.Fill(dt);
            }
            return dt;
        }

        public int Insertar(Categoria categoria)
        {
            string query = @"INSERT INTO Categories (CategoryName, Description, Picture) 
                             VALUES (@CategoryName, @Description, @Picture);
                            ";
            using (var cn = new MySqlConnection(_connectionString))
            using (var cmd = new MySqlCommand(query, cn))
            {
                cmd.Parameters.AddWithValue("@CategoryName", categoria.CategoryName);
                cmd.Parameters.AddWithValue("@Description", categoria.Description);
                cmd.Parameters.AddWithValue("@Picture", categoria.Picture);
                cn.Open();
                int numRegs = cmd.ExecuteNonQuery();
                categoria.CategoryID = (int)cmd.LastInsertedId;
                return numRegs;
            }
        }

        public int Actualizar(Categoria categoria)
        {
            string query = @"UPDATE Categories 
                             SET CategoryName = @CategoryName, Description = @Description";
            if (categoria.CategoryID > 8)
                query += ", Picture = @Picture ";
            query += @"
                             WHERE CategoryID = @CategoryID;
                            ";
            using (var cn = new MySqlConnection(_connectionString))
            using (var cmd = new MySqlCommand(query, cn))
            {
                cmd.Parameters.AddWithValue("@CategoryID", categoria.CategoryID);
                cmd.Parameters.AddWithValue("@CategoryName", categoria.CategoryName);
                cmd.Parameters.AddWithValue("@Description", categoria.Description);
                if (categoria.CategoryID > 8)
                    cmd.Parameters.AddWithValue("@Picture", categoria.Picture);
                cn.Open();
                return cmd.ExecuteNonQuery();
            }
        }

        public int Eliminar(Categoria categoria)
        {
            if (categoria.CategoryID <= 8)
                throw new InvalidOperationException("No se puede eliminar una categoría del sistema.");
            string query = @"DELETE FROM Categories WHERE CategoryID = @CategoryId;";
            using (var cn = new MySqlConnection(_connectionString))
            using (var cmd = new MySqlCommand(query, cn))
            {
                cmd.Parameters.AddWithValue("@CategoryId", categoria.CategoryID);
                cn.Open();
                return cmd.ExecuteNonQuery();
            }
        }

        public DataSet ObtenerCategoriasProductosDataSet()
        {
            DataSet ds = new DataSet();
            ds.Locale = System.Globalization.CultureInfo.InvariantCulture;
            using (var cn = new MySqlConnection(_connectionString))
            {
                cn.Open();
                string queryCategorias = "Select * from Categories Order by CategoryId Desc;";
                using (var dapCategorias = new MySqlDataAdapter(queryCategorias, cn)) 
                    dapCategorias.Fill(ds, "Categorias");
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
                using (var dapProductos = new MySqlDataAdapter(queryProductos, cn))
                    dapProductos.Fill(ds, "Productos");
            }
            // en la siguiente instrucción se deben de proporcionar los nombres de los campos (alias) que devuelve el store procedure
            DataRelation dataRelation = new DataRelation("CategoriasProductos", ds.Tables["Categorias"].Columns["CategoryID"], ds.Tables["Productos"].Columns["CategoryID"]);
            ds.Relations.Add(dataRelation);
            return ds;
        }

        public DataTable ObtenerProductosPorCategoriaListado()
        {
            DataTable dt = new DataTable();
            string query = "Select * from vw_productosporcategorialistado order by CategoryName, ProductName;";
            using (var cn = new MySqlConnection(_connectionString))
            using (var cmd = new MySqlCommand(query, cn))
            using (var da = new MySqlDataAdapter(cmd))
                da.Fill(dt);
            return dt;
        }

        public List<Categoria> ObtenerCategoriasList()
        {
            var categorias = new List<Categoria>();
            string query = "SELECT CategoryID, CategoryName, Description, Picture FROM Categories;";
            using (var cn = new MySqlConnection(_connectionString))
            using (var cmd = new MySqlCommand(query, cn))
            {
                cn.Open();
                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        var categoria = new Categoria
                        {
                            CategoryID = dr.GetInt32(dr.GetOrdinal("CategoryID")),
                            CategoryName = dr.GetString(dr.GetOrdinal("CategoryName")),
                            Description = dr.IsDBNull(dr.GetOrdinal("Description")) ? null : dr.GetString(dr.GetOrdinal("Description")),
                        };
                        if (!dr.IsDBNull(3))
                        {
                            byte[] photoData;
                            if (categoria.CategoryID <= 8)
                            {
                                // Las primeras 8 categorías tienen un encabezado de 78 bytes que debe ser removido
                                byte[] fullData = (byte[])dr["Picture"];
                                photoData = new byte[fullData.Length - 78];
                                Array.Copy(fullData, 78, photoData, 0, photoData.Length);
                            }
                            else
                            {
                                photoData = (byte[])dr["Picture"];
                            }
                            categoria.Picture = photoData;
                        }
                        else
                        {
                            categoria.Picture = null;
                        }
                        categorias.Add(categoria);
                    }
                }
            }
            return categorias;
        }
    }
}
