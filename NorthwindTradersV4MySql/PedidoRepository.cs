using MySql.Data.MySqlClient;
using NorthwindTradersV4MySql.ScriptsSql;
using Org.BouncyCastle.Asn1.X500;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NorthwindTradersV4MySql
{
    internal class PedidoRepository
    {
        private readonly string _connectionString;

        public PedidoRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public DataTable ObtenerPedidos(DtoPedidosBuscar dtoPedidosBuscar)
        {
            var dt = new DataTable();
            string query;
            if (dtoPedidosBuscar == null)
                query = "spPedidosListarLast20";
            else
                query = "spPedidosBuscar";
            try
            {
                using (var cn = new MySqlConnection(_connectionString))
                using (var cmd = new MySqlCommand(query, cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (dtoPedidosBuscar != null)
                    {
                        cmd.Parameters.AddWithValue("p_IdInicial", dtoPedidosBuscar.IdIni);
                        cmd.Parameters.AddWithValue("p_IdFinal", dtoPedidosBuscar.IdFin);
                        cmd.Parameters.AddWithValue("p_Cliente", dtoPedidosBuscar.Cliente);
                        cmd.Parameters.AddWithValue("p_FPedido", dtoPedidosBuscar.FPedido);
                        cmd.Parameters.AddWithValue("p_FPedidoNull", dtoPedidosBuscar.FPedidoNull);
                        cmd.Parameters.AddWithValue("p_FPedidoIni", dtoPedidosBuscar.FPedidoIni);
                        cmd.Parameters.AddWithValue("p_FPedidoFin", dtoPedidosBuscar.FPedidoFin);
                        cmd.Parameters.AddWithValue("p_FRequerido", dtoPedidosBuscar.FRequerido);
                        cmd.Parameters.AddWithValue("p_FRequeridoNull", dtoPedidosBuscar.FRequeridoNull);
                        cmd.Parameters.AddWithValue("p_FRequeridoIni", dtoPedidosBuscar.FRequeridoIni);
                        cmd.Parameters.AddWithValue("p_FRequeridoFin", dtoPedidosBuscar.FRequeridoFin);
                        cmd.Parameters.AddWithValue("p_FEnvio", dtoPedidosBuscar.FEnvio);
                        cmd.Parameters.AddWithValue("p_FEnvioNull", dtoPedidosBuscar.FEnvioNull);
                        cmd.Parameters.AddWithValue("p_FEnvioIni", dtoPedidosBuscar.FEnvioIni);
                        cmd.Parameters.AddWithValue("p_FEnvioFin", dtoPedidosBuscar.FEnvioFin);
                        cmd.Parameters.AddWithValue("p_Empleado", dtoPedidosBuscar.Empleado);
                        cmd.Parameters.AddWithValue("p_CompañiaT", dtoPedidosBuscar.CompañiaT);
                        cmd.Parameters.AddWithValue("p_DirigidoA", dtoPedidosBuscar.DirigidoA);
                    }
                    using (var da = new MySqlDataAdapter(cmd))
                        da.Fill(dt);
                }
            }
            catch (MySqlException ex)
            {
                throw new Exception("Error al obtener los pedidos: " + ex.Message);
            }
            return dt;
        }

        public DataTable ObtenerProductosPorCategorias(int categoria)
        {
            DataTable dt = new DataTable();
            try
            {
                using (var cn = new MySqlConnection(_connectionString))
                using (var cmd = new MySqlCommand("spProductosSeleccionarPorCategorias", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("pCategoria", categoria);
                    using (var da = new MySqlDataAdapter(cmd))
                        da.Fill(dt);
                }
            }
            catch (MySqlException ex)
            {
                throw new Exception("Error al obtener los productos por categoría: " + ex.Message);
            }
            return dt;
        }

        public DtoEnvioInformacion ObtenerInformacionEnvio(string customerId)
        {
            if (string.IsNullOrWhiteSpace(customerId))
                return null;
            DtoEnvioInformacion dtoEnvioInformacion = null;
            const string query = @"
                                    SELECT
                                        ShipName, ShipAddress, ShipCity, ShipRegion, ShipPostalCode, ShipCountry
                                    FROM Orders
                                    WHERE CustomerId = @CustomerId
                                    ORDER BY OrderId DESC
                                    LIMIT 1;
                                ";
            try
            {
                using (var cn = new MySqlConnection(_connectionString))
                using (var cmd = new MySqlCommand(query, cn))
                {
                    cmd.Parameters.AddWithValue("@CustomerId", customerId);
                    cn.Open();
                    using (var rdr = cmd.ExecuteReader(CommandBehavior.SingleRow))
                    {
                        if (rdr.Read())
                        {
                            dtoEnvioInformacion = new DtoEnvioInformacion
                            {
                                ShipName = rdr["ShipName"] == DBNull.Value ? string.Empty : rdr.GetString(rdr.GetOrdinal("ShipName")),
                                ShipAddress = rdr["ShipAddress"] == DBNull.Value ? string.Empty : rdr.GetString(rdr.GetOrdinal("ShipAddress")),
                                ShipCity = rdr["ShipCity"] == DBNull.Value ? string.Empty : rdr.GetString(rdr.GetOrdinal("ShipCity")),
                                ShipRegion = rdr["ShipRegion"] == DBNull.Value ? string.Empty : rdr.GetString(rdr.GetOrdinal("ShipRegion")),
                                ShipPostalCode = rdr["ShipPostalCode"] == DBNull.Value ? string.Empty : rdr.GetString(rdr.GetOrdinal("ShipPostalCode")),
                                ShipCountry = rdr["ShipCountry"] == DBNull.Value ? string.Empty : rdr.GetString(rdr.GetOrdinal("ShipCountry"))
                            };
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                throw new Exception("Error al obtener la información de envío: " + ex.Message);
            }
            return dtoEnvioInformacion;
        }

        public DtoProductoCostoInventario ObtenerProductoCostoInventario(int productId)
        {
            if (productId <= 0)
                return null;
            DtoProductoCostoInventario dtoProductoCostoInventario = null;
            const string query = @"
                                    SELECT
                                        UnitPrice, UnitsInStock
                                    FROM Products
                                    WHERE ProductId = @ProductId;
                                ";
            try
            {
                using (var cn = new MySqlConnection(_connectionString))
                using (var cmd = new MySqlCommand(query, cn))
                {
                    cmd.Parameters.AddWithValue("@ProductId", productId);
                    cn.Open();
                    using (var rdr = cmd.ExecuteReader(CommandBehavior.SingleRow))
                    {
                        if (rdr.Read())
                        {
                            dtoProductoCostoInventario = new DtoProductoCostoInventario
                            {
                                UnitPrice = rdr["UnitPrice"] == DBNull.Value ? 0m : rdr.GetDecimal(rdr.GetOrdinal("UnitPrice")),
                                UnitsInStock = (short)(rdr["UnitsInStock"] == DBNull.Value ? 0 : rdr.GetInt16(rdr.GetOrdinal("UnitsInStock")))
                            };
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                throw new Exception("Error al obtener el costo e inventario del producto: " + ex.Message);
            }
            return dtoProductoCostoInventario;
        }

        public Pedido ObtenerPedidoPorId(int orderId)
        {
            if (orderId <= 0)
                return null;
            Pedido pedido = null;
            try
            {
                using (var cn = new MySqlConnection(_connectionString))
                using (var cmd = new MySqlCommand("spPedidoPorId", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("pPedidoId", orderId);
                    cn.Open();
                    using (var rdr = cmd.ExecuteReader(CommandBehavior.SingleRow))
                    {
                        if (rdr.Read())
                        {
                            pedido = new Pedido
                            {
                                OrderID = rdr.GetInt32(rdr.GetOrdinal("OrderID")),
                                CustomerID = rdr.IsDBNull(rdr.GetOrdinal("CustomerID")) ? null : rdr.GetString(rdr.GetOrdinal("CustomerID")),
                                EmployeeID = rdr.IsDBNull(rdr.GetOrdinal("EmployeeID")) ? (int?)null : rdr.GetInt32(rdr.GetOrdinal("EmployeeID")),
                                OrderDate = rdr.IsDBNull(rdr.GetOrdinal("OrderDate")) ? (DateTime?)null : rdr.GetDateTime(rdr.GetOrdinal("OrderDate")),
                                RequiredDate = rdr.IsDBNull(rdr.GetOrdinal("RequiredDate")) ? (DateTime?)null : rdr.GetDateTime(rdr.GetOrdinal("RequiredDate")),
                                ShippedDate = rdr.IsDBNull(rdr.GetOrdinal("ShippedDate")) ? (DateTime?)null : rdr.GetDateTime(rdr.GetOrdinal("ShippedDate")),
                                ShipVia = rdr.IsDBNull(rdr.GetOrdinal("ShipVia")) ? (int?)null : rdr.GetInt32(rdr.GetOrdinal("ShipVia")),
                                Freight = rdr.IsDBNull(rdr.GetOrdinal("Freight")) ? (decimal?)null : rdr.GetDecimal(rdr.GetOrdinal("Freight")),
                                ShipName = rdr.IsDBNull(rdr.GetOrdinal("ShipName")) ? null : rdr.GetString(rdr.GetOrdinal("ShipName")),
                                ShipAddress = rdr.IsDBNull(rdr.GetOrdinal("ShipAddress")) ? null : rdr.GetString(rdr.GetOrdinal("ShipAddress")),
                                ShipCity = rdr.IsDBNull(rdr.GetOrdinal("ShipCity")) ? null : rdr.GetString(rdr.GetOrdinal("ShipCity")),
                                ShipRegion = rdr.IsDBNull(rdr.GetOrdinal("ShipRegion")) ? null : rdr.GetString(rdr.GetOrdinal("ShipRegion")),
                                ShipPostalCode = rdr.IsDBNull(rdr.GetOrdinal("ShipPostalCode")) ? null : rdr.GetString(rdr.GetOrdinal("ShipPostalCode")),
                                ShipCountry = rdr.IsDBNull(rdr.GetOrdinal("ShipCountry")) ? null : rdr.GetString(rdr.GetOrdinal("ShipCountry")),
                                RowVersion = rdr.IsDBNull(rdr.GetOrdinal("RowVersion")) ? (int?)null : Convert.ToInt32(rdr["RowVersion"])
                            };
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                throw new Exception("Error al obtener el pedido por ID: " + ex.Message);
            }
            return pedido;
        }
    }
}
