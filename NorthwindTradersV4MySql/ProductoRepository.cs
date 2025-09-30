using MySql.Data.MySqlClient;
using System;
using System.Data;

namespace NorthwindTradersV4MySql
{
    internal class ProductoRepository
    {
        private readonly string _connectionString;

        public ProductoRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public DataTable ObtenerComboCategorias()
        {
            var dt = new DataTable();
            try
            {
                using (var cn = new MySql.Data.MySqlClient.MySqlConnection(_connectionString))
                using (var cmd = new MySql.Data.MySqlClient.MySqlCommand("spCategoriasSeleccionar", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (var da = new MySql.Data.MySqlClient.MySqlDataAdapter(cmd))
                        da.Fill(dt);
                }
            }
            catch (MySqlException ex)
            {
                throw new Exception("Error al obtener las categorías: " + ex.Message);
            }
            return dt;
        }

        public DataTable ObtenerComboProveedores()
        {
            var dt = new DataTable();
            try
            {
                using (var cn = new MySql.Data.MySqlClient.MySqlConnection(_connectionString))
                using (var cmd = new MySql.Data.MySqlClient.MySqlCommand("spProveedoresSeleccionar", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (var da = new MySql.Data.MySqlClient.MySqlDataAdapter(cmd))
                        da.Fill(dt);
                }
            }
            catch (MySqlException ex)
            {
                throw new Exception("Error al obtener los proveedores: " + ex.Message);
            }
            return dt;
        }

        public DataTable ObtenerProductos(DtoProductosBuscar dtoProductosBuscar)
        {
            var dt = new DataTable();
            string query;
            if (dtoProductosBuscar == null)
                query = "spProductosListarLast20";
            else
                query = "spProductosBuscar";
            try
            {
                using (var cn = new MySqlConnection(_connectionString))
                using (var cmd = new MySqlCommand(query, cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (dtoProductosBuscar != null)
                    {
                        cmd.Parameters.AddWithValue("@IdIni", dtoProductosBuscar.IdIni);
                        cmd.Parameters.AddWithValue("@IdFin", dtoProductosBuscar.IdFin);
                        cmd.Parameters.AddWithValue("@Producto", dtoProductosBuscar.Producto ?? string.Empty);
                        cmd.Parameters.AddWithValue("@Categoria", dtoProductosBuscar.Categoria);
                        cmd.Parameters.AddWithValue("@Proveedor", dtoProductosBuscar.Proveedor);
                    }
                    using (var da = new MySqlDataAdapter(cmd))
                        da.Fill(dt);
                }
            }
            catch (MySqlException ex)
            {
                throw new Exception("Error al obtener los productos: " + ex.Message);
            }
            return dt;
        }

        public Producto ObtenerProducto(Producto producto)
        {
            try
            {
                using (var cn = new MySqlConnection(_connectionString))
                using (var cmd = new MySqlCommand("spProductoPorId", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("pProductID", producto.ProductID);
                    cn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            producto = new Producto
                            {
                                RowVersion = Convert.ToInt32(reader["RowVersion"]),
                                ProductName = reader.IsDBNull(reader.GetOrdinal("ProductName")) ? null : reader["ProductName"].ToString(),
                                SupplierID = Convert.ToInt32(reader["SupplierID"]),
                                CategoryID = Convert.ToInt32(reader["CategoryID"]),
                                QuantityPerUnit = reader["QuantityPerUnit"].ToString(),
                                UnitPrice = Convert.ToDecimal(reader["UnitPrice"]),
                                UnitsInStock = Convert.ToInt16(reader["UnitsInStock"]),
                                UnitsOnOrder = Convert.ToInt16(reader["UnitsOnOrder"]),
                                ReorderLevel = Convert.ToInt16(reader["ReorderLevel"]),
                                Discontinued = Convert.ToBoolean(reader["Discontinued"])
                            };
                        }
                        else
                            producto = null;
                    }
                }
            }
            catch (MySqlException ex)
            {
                throw new Exception("Error al obtener el producto: " + ex.Message);
            }
            return producto;
        }

        public int Insertar(Producto producto)
        {
            int numRegs = 0;
            try
            {
                using (var cn = new MySqlConnection(_connectionString))
                using (var cmd = new MySqlCommand("spProductoInsertar", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("pProductName", producto.ProductName ?? string.Empty);
                    cmd.Parameters.AddWithValue("pSupplierID", producto.SupplierID);
                    cmd.Parameters.AddWithValue("pCategoryID", producto.CategoryID);
                    cmd.Parameters.AddWithValue("pQuantityPerUnit", string.IsNullOrWhiteSpace(producto.QuantityPerUnit) ? (object)DBNull.Value : producto.QuantityPerUnit.Trim());
                    cmd.Parameters.AddWithValue("pUnitPrice", producto.UnitPrice);
                    cmd.Parameters.AddWithValue("pUnitsInStock", producto.UnitsInStock);
                    cmd.Parameters.AddWithValue("pUnitsOnOrder", producto.UnitsOnOrder);
                    cmd.Parameters.AddWithValue("pReorderLevel", producto.ReorderLevel);
                    cmd.Parameters.AddWithValue("pDiscontinued", producto.Discontinued);
                    cmd.Parameters.AddWithValue("pRowVersionIn", 1);
                    //cmd.Parameters.AddWithValue("pProductID", producto.ProductID).Direction = ParameterDirection.Output;
                    var outParam = new MySqlParameter("pProductID", MySqlDbType.Int32)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(outParam);

                    cn.Open();
                    numRegs = cmd.ExecuteNonQuery();
                    // Leer id generado y asignarlo al objeto
                    if (outParam.Value != DBNull.Value && outParam.Value != null)
                    {
                        producto.ProductID = Convert.ToInt32(outParam.Value);
                    }
                }
            }
            catch (MySqlException ex)
            {
                throw new Exception("Error al guardar el producto: " + ex.Message);
            }
            return numRegs;
        }

        public int Actualizar(Producto producto)
        {
            int numRegs = 0;
            try
            {
                using (var cn = new MySqlConnection(_connectionString))
                using (var cmd = new MySqlCommand("spProductoActualizar", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("pProductID", producto.ProductID);
                    cmd.Parameters.AddWithValue("pProductName", producto.ProductName ?? string.Empty);
                    cmd.Parameters.AddWithValue("pSupplierID", producto.SupplierID);
                    cmd.Parameters.AddWithValue("pCategoryID", producto.CategoryID);
                    cmd.Parameters.AddWithValue("pQuantityPerUnit", string.IsNullOrWhiteSpace(producto.QuantityPerUnit) ? (object)DBNull.Value : producto.QuantityPerUnit.Trim());
                    cmd.Parameters.AddWithValue("pUnitPrice", producto.UnitPrice);
                    cmd.Parameters.AddWithValue("pUnitsInStock", producto.UnitsInStock);
                    cmd.Parameters.AddWithValue("pUnitsOnOrder", producto.UnitsOnOrder);
                    cmd.Parameters.AddWithValue("pReorderLevel", producto.ReorderLevel);
                    cmd.Parameters.AddWithValue("pDiscontinued", producto.Discontinued);
                    cmd.Parameters.AddWithValue("pRowVersion", producto.RowVersion);
                    cn.Open();
                    numRegs = cmd.ExecuteNonQuery();
                }
            }
            catch (MySqlException ex)
            {
                throw new Exception("Error al actualizar el producto: " + ex.Message);
            }
            return numRegs;
        }

        public int Eliminar(Producto producto)
        {
            int numRegs = 0;
            try
            {
                using (var cn = new MySqlConnection(_connectionString))
                using (var cmd = new MySqlCommand("spProductoEliminar", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("pProductID", producto.ProductID);
                    cmd.Parameters.AddWithValue("pRowVersion", producto.RowVersion);
                    cn.Open();
                    numRegs = cmd.ExecuteNonQuery();
                }
            }
            catch (MySqlException ex)
            {
                throw new Exception("Error al eliminar el producto: " + ex.Message);
            }
            return numRegs;
        }
    }
}
