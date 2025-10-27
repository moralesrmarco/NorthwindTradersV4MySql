using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace NorthwindTradersV4MySql
{
    internal class PedidoRepository : IDisposable
    {
        private readonly string _connectionString;
        private bool _disposed;

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

        public List<PedidoDetalle> ObtenerDetallePedidoPorPedidoId(int orderId)
        {
            var detalles = new List<PedidoDetalle>();
            try
            {
                using (var cn = new MySqlConnection(_connectionString))
                using (var cmd = new MySqlCommand("spPedidoDetallePorPedidoId", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("pPedidoId", orderId);
                    cn.Open();
                    using (var rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            var detalle = new PedidoDetalle
                            {
                                OrderID = rdr.GetInt32(rdr.GetOrdinal("OrderID")),
                                ProductID = rdr.GetInt32(rdr.GetOrdinal("ProductID")),
                                ProductName = rdr.IsDBNull(rdr.GetOrdinal("ProductName")) ? null : rdr.GetString(rdr.GetOrdinal("ProductName")),
                                UnitPrice = rdr.GetDecimal(rdr.GetOrdinal("UnitPrice")),
                                Quantity = rdr.GetInt16(rdr.GetOrdinal("Quantity")),
                                Discount = rdr.GetDecimal(rdr.GetOrdinal("Discount")),
                                RowVersion = rdr.IsDBNull(rdr.GetOrdinal("RowVersion")) ? 0 : Convert.ToInt32(rdr["RowVersion"])
                            };
                            detalles.Add(detalle);
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                throw new Exception("Error al obtener los detalles del pedido: " + ex.Message);
            }
            return detalles;
        }

        public int Insertar(Pedido pedido, List<PedidoDetalle> detalles, out int orderId)
        {
            orderId = 0;
            int filasAfectadas = 0;
            using (var cn = new MySqlConnection(_connectionString))
            {
                cn.Open();
                using (var tx = cn.BeginTransaction())
                {
                    try
                    {
                        int tempOrderId;
                        // 1) Insertar registro padre (SP)
                        using (var cmd = new MySqlCommand("spPedidosInsertar", cn, tx))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("pCustomerId", string.IsNullOrWhiteSpace(pedido.CustomerID) ? (object)DBNull.Value : pedido.CustomerID);
                            cmd.Parameters.AddWithValue("pEmployeeId", pedido.EmployeeID);
                            cmd.Parameters.AddWithValue("pOrderDate", pedido.OrderDate.HasValue ? (object)pedido.OrderDate.Value : DBNull.Value);
                            cmd.Parameters.AddWithValue("pRequiredDate", pedido.RequiredDate.HasValue ? (object)pedido.RequiredDate.Value : DBNull.Value);
                            cmd.Parameters.AddWithValue("pShippedDate", pedido.ShippedDate.HasValue ? (object)pedido.ShippedDate.Value : DBNull.Value);
                            cmd.Parameters.AddWithValue("pShipVia", pedido.ShipVia);
                            cmd.Parameters.AddWithValue("pShipName", string.IsNullOrWhiteSpace(pedido.ShipName) ? (object)DBNull.Value : pedido.ShipName);
                            cmd.Parameters.AddWithValue("pShipAddress", string.IsNullOrWhiteSpace(pedido.ShipAddress) ? (object)DBNull.Value : pedido.ShipAddress);
                            cmd.Parameters.AddWithValue("pShipCity", string.IsNullOrWhiteSpace(pedido.ShipCity) ? (object)DBNull.Value : pedido.ShipCity); 
                            cmd.Parameters.AddWithValue("pShipRegion", string.IsNullOrWhiteSpace(pedido.ShipRegion) ? (object)DBNull.Value : pedido.ShipRegion);
                            cmd.Parameters.AddWithValue("pShipPostalCode", string.IsNullOrWhiteSpace(pedido.ShipPostalCode) ? (object)DBNull.Value : pedido.ShipPostalCode);
                            cmd.Parameters.AddWithValue("pShipCountry", string.IsNullOrWhiteSpace(pedido.ShipCountry) ? (object)DBNull.Value : pedido.ShipCountry);
                            cmd.Parameters.AddWithValue("pFreight", pedido.Freight);
                            cmd.Parameters.AddWithValue("pRowVersion", 1);
                            filasAfectadas += cmd.ExecuteNonQuery();
                        }
                        // Obtener OrderID generado
                        using (var cmd = new MySqlCommand("SELECT LAST_INSERT_ID();", cn, tx))
                        {
                            var idObj = cmd.ExecuteScalar();
                            tempOrderId = idObj == DBNull.Value ? 0 : Convert.ToInt32(idObj);
                        }
                        // 2) Preparar comandos reutilizables para cada detalle:
                        // 2.1) Preparar SELECT UnitsInStock FOR UPDATE
                        using (var cmdCheckStock = new MySqlCommand("SELECT UnitsInStock FROM products WHERE ProductID = @pid FOR UPDATE;", cn, tx))
                        {
                            cmdCheckStock.Parameters.Add(new MySqlParameter("@pid", MySqlDbType.Int32));

                            // 2.2) Preparar UPDATE products
                            using (var cmdUpdateStock = new MySqlCommand("UPDATE products SET UnitsInStock = UnitsInStock - @qty WHERE ProductID = @pid;", cn, tx))
                            {
                                cmdUpdateStock.Parameters.Add(new MySqlParameter("@qty", MySqlDbType.Int32));
                                cmdUpdateStock.Parameters.Add(new MySqlParameter("@pid", MySqlDbType.Int32));

                                // 2.3) Preparar inserción de detalle (SP)
                                using (var cmdInsertDetail = new MySqlCommand("spPedidoDetalleInsertar", cn, tx))
                                {
                                    cmdInsertDetail.CommandType = CommandType.StoredProcedure;
                                    cmdInsertDetail.Parameters.Add(new MySqlParameter("pOrderId", MySqlDbType.Int32));
                                    cmdInsertDetail.Parameters.Add(new MySqlParameter("pProductId", MySqlDbType.Int32));
                                    cmdInsertDetail.Parameters.Add(new MySqlParameter("pUnitPrice", MySqlDbType.Decimal));
                                    cmdInsertDetail.Parameters.Add(new MySqlParameter("pQuantity", MySqlDbType.Int16));
                                    cmdInsertDetail.Parameters.Add(new MySqlParameter("pDiscount", MySqlDbType.Float));
                                    cmdInsertDetail.Parameters.Add(new MySqlParameter("pRowVersion", MySqlDbType.Int32));

                                    // 3) Procesar cada detalle
                                    foreach (var d in detalles)
                                    {
                                        // 3.1) Validar existencia y bloquear fila del producto
                                        cmdCheckStock.Parameters["@pid"].Value = d.ProductID;
                                        var stockObj = cmdCheckStock.ExecuteScalar();
                                        if (stockObj == null || stockObj == DBNull.Value)
                                        {
                                            throw new InvalidOperationException($"Producto {d.ProductID} no existe.");
                                        }

                                        int currentStock = Convert.ToInt32(stockObj);

                                        // 3.2) Validar stock suficiente
                                        if (currentStock < d.Quantity)
                                        {
                                            throw new InvalidOperationException($"Inventario insuficiente para el producto {d.ProductID} {d.ProductName}. Disponible: {currentStock}, solicitado: {d.Quantity}.");
                                        }

                                        // 3.3) Actualizar stock
                                        cmdUpdateStock.Parameters["@qty"].Value = d.Quantity;
                                        cmdUpdateStock.Parameters["@pid"].Value = d.ProductID;
                                        var rowsUpdated = cmdUpdateStock.ExecuteNonQuery();
                                        if (rowsUpdated == 0)
                                        {
                                            throw new InvalidOperationException($"No se pudo actualizar el inventario para el producto {d.ProductID}.");
                                        }

                                        // 3.4) Insertar detalle (SP)
                                        cmdInsertDetail.Parameters["pOrderId"].Value = tempOrderId;
                                        cmdInsertDetail.Parameters["pProductId"].Value = d.ProductID;
                                        cmdInsertDetail.Parameters["pUnitPrice"].Value = d.UnitPrice;
                                        cmdInsertDetail.Parameters["pQuantity"].Value = d.Quantity;
                                        cmdInsertDetail.Parameters["pDiscount"].Value = d.Discount;
                                        cmdInsertDetail.Parameters["pRowVersion"].Value = 1;

                                        filasAfectadas += cmdInsertDetail.ExecuteNonQuery();
                                    } // foreach detalles
                                } // cmdInsertDetail
                            } // cmdUpdateStock
                        } // cmdCheckStock
                        tx.Commit();
                        orderId = tempOrderId;
                        return filasAfectadas;
                    }
                    catch (MySqlException ex) when (ex.Number == 1451)
                    {
                        try { tx.Rollback(); } catch (Exception) { }
                        orderId = 0;
                        throw new Exception("Algún producto en el pedido fue previamente eliminado o existe una restricción de integridad referencial.\n" + ex.Message);
                    }
                    catch (MySqlException ex) when (ex.Number == 1452)
                    {
                        try { tx.Rollback(); } catch (Exception) { }
                        orderId = 0;
                        throw new Exception("Operación inválida por restricción de clave foránea (no existe el registro padre).\n" + ex.Message);
                    }
                    catch (MySqlException ex) when (ex.Number == 1062)
                    {
                        try { tx.Rollback(); } catch (Exception) { }
                        orderId = 0;
                        throw new Exception("Error, existe un producto duplicado en el pedido, elimine el producto duplicado y modifique la cantidad del producto");
                    }
                    catch (Exception)
                    {
                        try { tx.Rollback(); } catch (Exception) { }
                        orderId = 0;
                        throw;
                    }
                }
            }
        }

        public int Actualizar(Pedido pedido, out int rowVersion)
        {
            int filasAfectadas = 0;
            try
            {
                using (var cn = new MySqlConnection(_connectionString))
                using (var cmd = new MySqlCommand("spPedidosActualizar", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("pOrderId", pedido.OrderID);
                    cmd.Parameters.AddWithValue("pCustomerId", string.IsNullOrWhiteSpace(pedido.CustomerID) ? (object)DBNull.Value : pedido.CustomerID);
                    cmd.Parameters.AddWithValue("pEmployeeId", ((object)pedido.EmployeeID == null || pedido.EmployeeID.Equals(0)) ? DBNull.Value : (object)pedido.EmployeeID);
                    cmd.Parameters.AddWithValue("pOrderDate", pedido.OrderDate.HasValue ? (object)pedido.OrderDate.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("pRequiredDate", pedido.RequiredDate.HasValue ? (object)pedido.RequiredDate.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("pShippedDate", pedido.ShippedDate.HasValue ? (object)pedido.ShippedDate.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("pShipVia", ((object)pedido.ShipVia == null || pedido.ShipVia.Equals(0)) ? DBNull.Value : (object)pedido.ShipVia);
                    cmd.Parameters.AddWithValue("pFreight", (object)pedido.Freight ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("pShipName", string.IsNullOrWhiteSpace(pedido.ShipName) ? (object)DBNull.Value : pedido.ShipName);
                    cmd.Parameters.AddWithValue("pShipAddress", string.IsNullOrWhiteSpace(pedido.ShipAddress) ? (object)DBNull.Value : pedido.ShipAddress);
                    cmd.Parameters.AddWithValue("pShipCity", string.IsNullOrWhiteSpace(pedido.ShipCity) ? (object)DBNull.Value : pedido.ShipCity);
                    cmd.Parameters.AddWithValue("pShipRegion", string.IsNullOrWhiteSpace(pedido.ShipRegion) ? (object)DBNull.Value : pedido.ShipRegion);
                    cmd.Parameters.AddWithValue("pShipPostalCode", string.IsNullOrWhiteSpace(pedido.ShipPostalCode) ? (object)DBNull.Value : pedido.ShipPostalCode);
                    cmd.Parameters.AddWithValue("pShipCountry", string.IsNullOrWhiteSpace(pedido.ShipCountry) ? (object)DBNull.Value : pedido.ShipCountry);
                    cmd.Parameters.AddWithValue("pRowVersion", 0);
                    cmd.Parameters["pRowVersion"].Direction = ParameterDirection.Output;
                    cmd.Parameters.AddWithValue("pFilasAfectadas", 0);
                    cmd.Parameters["pFilasAfectadas"].Direction = ParameterDirection.Output;
                    cn.Open();
                    cmd.ExecuteNonQuery();
                    rowVersion = Convert.ToInt32(cmd.Parameters["pRowVersion"].Value);
                    filasAfectadas = Convert.ToInt32(cmd.Parameters["pFilasAfectadas"].Value);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar el pedido: " + ex.Message);
            }
            return filasAfectadas;
        }

        public int Eliminar(Pedido pedido)
        {
            int filasAfectadas = 0;
            try
            {
                using (var cn = new MySqlConnection(_connectionString))
                using (var cmd = new MySqlCommand("spPedidosEliminar", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("pOrderId", pedido.OrderID);
                    cmd.Parameters.AddWithValue("pAffectedRows", 0);
                    cmd.Parameters["pAffectedRows"].Direction= ParameterDirection.Output;
                    cn.Open();
                    cmd.ExecuteNonQuery();
                    filasAfectadas = Convert.ToInt32(cmd.Parameters["pAffectedRows"].Value);
                }
            }
            catch (MySqlException ex)
            {
                throw new Exception("Error al eliminar el pedido: " + ex.Message);
            }
            return filasAfectadas;
        }

        public int? DetallePedidosChkRowVersion(int numPedido, int numProducto)
        {
            if (numPedido <= 0 || numProducto <= 0)
                return null;
            try
            {
                using (var cn = new MySqlConnection(_connectionString))
                using (var cmd = new MySqlCommand("spDetallePedidosChkRowVersion", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("pPedidoId", numPedido);
                    cmd.Parameters.AddWithValue("pProductId", numProducto);
                    cn.Open();
                    return (int?)cmd.ExecuteScalar();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener el RowVersion del detalle del pedido: " + ex.Message);
            }
        }

        public DataTable ObtenerPedido(int id)
        {
            DataTable dt = new DataTable();
            try
            {
                using (var cn = new MySqlConnection(_connectionString))
                using (var cmd = new MySqlCommand("spPedidosNotaRemision", cn))
                {
                    cmd.CommandType= CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("PedidoId", id);
                    using (var dap = new MySqlDataAdapter(cmd))
                        dap.Fill(dt);
                }
            }
            catch (MySqlException ex)
            {
                throw new Exception("Error al obtener los datos del pedido: " + ex.Message);
            }
            return dt;
        }

        public DataTable ObtenerDetallePedidoPorOrderID(int orderID)
        {
            DataTable dt = new DataTable();
            try
            {
                using (MySqlConnection cn = new MySqlConnection(_connectionString))
                {
                    string query = @"
                                    SELECT 
                                        p.ProductName,
                                        od.UnitPrice,
                                        od.Quantity,
                                        od.Discount,
                                        (od.Quantity * od.UnitPrice) * (1 - od.Discount) AS Total
                                    FROM 
                                        `Order Details` od
                                    JOIN 
                                        Products p ON p.ProductID = od.ProductID
                                    where OrderID = " + orderID + ";";
                    MySqlCommand cmd = new MySqlCommand(query, cn);
                    MySqlDataAdapter dap = new MySqlDataAdapter(cmd);
                    dap.Fill(dt);
                }
            }
            catch (MySqlException ex) 
            { 
                throw new Exception("Error al obtener los datos de detalle del pedido: " + ex.Message); 
            }
            return dt;
        }

        public int Insertar(PedidoDetalle pedidoDetalle)
        {
            int filasAfectadas = 0;
            try
            {
                using (var cn = new MySqlConnection(_connectionString))
                using (var cmd = new MySqlCommand("spPedidosDetalleInsertar", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("p_OrderId", pedidoDetalle.OrderID);
                    cmd.Parameters.AddWithValue("p_ProductId", pedidoDetalle.ProductID);
                    cmd.Parameters.AddWithValue("p_UnitPrice", pedidoDetalle.UnitPrice);
                    cmd.Parameters.AddWithValue("p_Quantity", pedidoDetalle.Quantity);
                    cmd.Parameters.AddWithValue("p_Discount", pedidoDetalle.Discount);
                    cmd.Parameters.AddWithValue("p_RowsInserted", 0);
                    cmd.Parameters["p_RowsInserted"].Direction = ParameterDirection.Output;
                    cn.Open();
                    cmd.ExecuteNonQuery();
                    filasAfectadas = Convert.ToInt32(cmd.Parameters["p_RowsInserted"].Value);
                }
            }
            catch (MySqlException ex) when (ex.Number == 1062) // 1062 = Duplicate entry
            {
                throw new Exception("Error al insertar el detalle del pedido: Ya existe un detalle con el mismo ProductID para este OrderID.");
            }
            catch (MySqlException ex)
            {
                throw new Exception("Error al insertar el detalle del pedido: " + ex.Message);
            }
            return filasAfectadas;
        }

        public int Eliminar(PedidoDetalle pedidoDetalle)
        {
            int filasAfectadas = 0;
            try
            {
                using (var cn = new MySqlConnection(_connectionString))
                using (var cmd = new MySqlCommand("spPedidosDetalleEliminar", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("p_OrderId", pedidoDetalle.OrderID);
                    cmd.Parameters.AddWithValue("p_ProductId", pedidoDetalle.ProductID);
                    cmd.Parameters.AddWithValue("p_RowsDeleted", 0);
                    cmd.Parameters["p_RowsDeleted"].Direction = ParameterDirection.Output;
                    cn.Open();
                    cmd.ExecuteNonQuery();
                    filasAfectadas = Convert.ToInt32(cmd.Parameters["p_RowsDeleted"].Value);
                }
            }
            catch (MySqlException ex) when (ex.Number == 1264)
            {
                throw new Exception("Al tratar de devolver las unidades vendidas al inventario, la cantidad de unidades excede las 32,767 unidades, rango máximo para un campo de tipo SMALLINT");
            }
            catch (MySqlException ex)
            {
                throw new Exception("Error al eliminar el detalle del pedido: " + ex.Message);
            }
            return filasAfectadas;
        }

        public short? ObtenerUInventario(int productoId)
        {
            short? uInventario = null;
            try
            {
                using (var cn = new MySqlConnection(_connectionString))
                using (var cmd = new MySqlCommand($"Select UnitsInStock From Products Where ProductId = @pProductoId Limit 1;", cn))
                {
                    cmd.Parameters.AddWithValue("@pProductoId", productoId);
                    cn.Open();
                    var result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        uInventario = Convert.ToInt16(result);
                    }
                }
            }
            catch (MySqlException ex)
            {
                throw new Exception("Error al obtener el inventario del producto: " + ex.Message);
            }
            return uInventario;
        }

        public int Actualizar(PedidoDetalle pedidoDetalle, short cantidadOld, decimal descuentoOld)
        {
            int filasAfectadas = 0;
            try
            {
                using (var cn = new MySqlConnection(_connectionString))
                using (var cmd = new MySqlCommand("spPedidosDetalleActualizar", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("p_OrderId", pedidoDetalle.OrderID);
                    cmd.Parameters.AddWithValue("p_ProductId", pedidoDetalle.ProductID);
                    cmd.Parameters.AddWithValue("p_Quantity", pedidoDetalle.Quantity);
                    cmd.Parameters.AddWithValue("p_Discount", pedidoDetalle.Discount);
                    cmd.Parameters.AddWithValue("p_RowVersion", pedidoDetalle.RowVersion);
                    cmd.Parameters["p_RowVersion"].Direction = ParameterDirection.InputOutput;
                    cmd.Parameters.AddWithValue("p_QuantityOld", cantidadOld);
                    cmd.Parameters.AddWithValue("p_DiscountOld", descuentoOld);
                    cmd.Parameters.AddWithValue("p_RegistrosModificados", 0);
                    cmd.Parameters["p_RegistrosModificados"].Direction = ParameterDirection.Output;
                    cn.Open();
                    cmd.ExecuteNonQuery();
                    filasAfectadas = Convert.ToInt32(cmd.Parameters["p_RegistrosModificados"].Value);
                }
            }
            catch (MySqlException ex)
            {
                throw new Exception("Error al actualizar el detalle del pedido: " + ex.Message);
            }
            return filasAfectadas;
        }

        public DataTable ObtenerPedidosPorFechaPedido(DateTime? from, DateTime? to)
        {
            DataTable dt = new DataTable();
            try
            {
                using (var cn = new MySqlConnection(_connectionString))
                using (var cmd = new MySqlCommand("spPedidosPorRangoFechaPedido", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("pFrom", from);
                    cmd.Parameters.AddWithValue("pTo", to);
                    using (var da = new MySqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }
            }
            catch (MySqlException ex)
            {
                throw new Exception("Error al obtener los pedidos por rango de fecha de pedido: " + ex.Message);
            }
            return dt;
        }



        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;
        }
    }
}
