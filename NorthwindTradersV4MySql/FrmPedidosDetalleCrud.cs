using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Windows.Forms;

namespace NorthwindTradersV4MySql
{
    public partial class FrmPedidosDetalleCrud : Form
    {
        string cnStr = ConfigurationManager.ConnectionStrings["NorthwindMySql"].ConnectionString;
        int numDetalle = 1;

        public FrmPedidosDetalleCrud()
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
        }

        private void GrbPaint(object sender, PaintEventArgs e) => Utils.GrbPaint(this, sender, e);

        private void FrmPedidosDetalleCrud_FormClosed(object sender, FormClosedEventArgs e) => MDIPrincipal.ActualizarBarraDeEstado();

        private void FrmPedidosDetalleCrud_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (cboCategoria.SelectedIndex > 0 || cboProducto.SelectedIndex > 0)
                if (Utils.MensajeCerrarForm() == DialogResult.No)
                    e.Cancel = true;
        }

        private void FrmPedidosDetalleCrud_Load(object sender, EventArgs e)
        {
            DeshabilitarControles();
            Utils.LlenarCbo(cboCategoria, "spCategoriasSeleccionar", "CategoryName", "CategoryId");
            LlenarDgvPedidos(null);
            Utils.ConfDgv(DgvPedidos);
            Utils.ConfDgv(DgvDetalle);
            ConfDgvPedidos();
            ConfDgvDetalle();
            txtPrecio.Text = "$0.00";
            txtDescuento.Text = "0.00";
            txtTotal.Text = "$0.00";
            txtUInventario.Text = txtCantidad.Text = "0";
        }

        private void DeshabilitarControles()
        {
            cboCategoria.Enabled = cboProducto.Enabled = false;
            btnAgregar.Enabled = false;
        }

        private void HabilitarControles()
        {
            cboCategoria.Enabled = cboProducto.Enabled = true;
            btnAgregar.Enabled = true;
        }

        private void DeshabilitarControlesProducto()
        {
            txtCantidad.Enabled = txtDescuento.Enabled = false;
        }

        private void HabilitarControlesProducto()
        {
            txtCantidad.Enabled = txtDescuento.Enabled = true;
        }

        private void LlenarDgvPedidos(object sender)
        {
            try
            {
                MDIPrincipal.ActualizarBarraDeEstado(Utils.clbdd);
                DtoPedidosBuscar dtoPedidosBuscar;
                if (sender != null)
                {
                    dtoPedidosBuscar = new DtoPedidosBuscar
                    {
                        IdIni = string.IsNullOrEmpty(txtBIdInicial.Text) ? 0 : int.Parse(txtBIdInicial.Text),
                        IdFin = string.IsNullOrEmpty(txtBIdFinal.Text) ? 0 : int.Parse(txtBIdFinal.Text),
                        Cliente = txtBCliente.Text.Trim(),

                        FPedido = dtpBFPedidoIni.Checked && dtpBFPedidoFin.Checked,
                        FPedidoIni = (dtpBFPedidoIni.Checked && dtpBFPedidoFin.Checked) ? Convert.ToDateTime(dtpBFPedidoIni.Value.ToShortDateString() + " 00:00:00.000") : (DateTime?)null,
                        FPedidoFin = (dtpBFPedidoIni.Checked && dtpBFPedidoFin.Checked) ? Convert.ToDateTime(dtpBFPedidoFin.Value.ToShortDateString() + " 23:59:59.998") : (DateTime?)null,
                        FPedidoNull = chkBFPedidoNull.Checked,

                        FRequerido = dtpBFRequeridoIni.Checked && dtpBFRequeridoFin.Checked,
                        FRequeridoIni = (dtpBFRequeridoIni.Checked && dtpBFRequeridoFin.Checked) ? Convert.ToDateTime(dtpBFRequeridoIni.Value.ToShortDateString() + " 00:00:00.000") : (DateTime?)null,
                        FRequeridoFin = (dtpBFRequeridoIni.Checked && dtpBFRequeridoFin.Checked) ? Convert.ToDateTime(dtpBFRequeridoFin.Value.ToShortDateString() + " 23:59:59.998") : (DateTime?)null,
                        FRequeridoNull = chkBFRequeridoNull.Checked,

                        FEnvio = dtpBFEnvioIni.Checked && dtpBFEnvioFin.Checked,
                        FEnvioIni = (dtpBFEnvioIni.Checked && dtpBFEnvioFin.Checked) ? Convert.ToDateTime(dtpBFEnvioIni.Value.ToShortDateString() + " 00:00:00.000") : (DateTime?)null,
                        FEnvioFin = (dtpBFEnvioIni.Checked && dtpBFEnvioFin.Checked) ? Convert.ToDateTime(dtpBFEnvioFin.Value.ToShortDateString() + " 23:59:59.998") : (DateTime?)null,
                        FEnvioNull = chkBFEnvioNull.Checked,

                        Empleado = txtBEmpleado.Text.Trim(),
                        CompañiaT = txtBCompañiaT.Text.Trim(),
                        DirigidoA = txtBDirigidoa.Text.Trim()
                    };
                }
                else
                    dtoPedidosBuscar = null;
                var dt = new PedidoRepository(cnStr).ObtenerPedidos(dtoPedidosBuscar);
                DgvPedidos.DataSource = dt;
                if (sender == null)
                    MDIPrincipal.ActualizarBarraDeEstado($"Se muestran los últimos {DgvPedidos.RowCount} pedidos registrados");
                else
                    MDIPrincipal.ActualizarBarraDeEstado($"Se encontraron {DgvPedidos.RowCount} registros");
            }
            catch (Exception ex)
            {
                Utils.MsgCatchOue(ex);
            }
        }

        private void ConfDgvPedidos()
        {
            DgvPedidos.Columns["OrderId"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

            DgvPedidos.Columns["OrderDate"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            DgvPedidos.Columns["RequiredDate"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            DgvPedidos.Columns["ShippedDate"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            DgvPedidos.Columns["Shipper"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            DgvPedidos.Columns["Employee"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            DgvPedidos.Columns["OrderDate"].DefaultCellStyle.Format = "ddd dd\" de \"MMM\" de \"yyyy\n hh:mm:ss tt";
            DgvPedidos.Columns["RequiredDate"].DefaultCellStyle.Format = "ddd dd\" de \"MMM\" de \"yyyy\n hh:mm:ss tt";
            DgvPedidos.Columns["ShippedDate"].DefaultCellStyle.Format = "ddd dd\" de \"MMM\" de \"yyyy\n hh:mm:ss tt";

            DgvPedidos.Columns["OrderId"].HeaderText = "Id";
            DgvPedidos.Columns["Customer"].HeaderText = "Cliente";
            DgvPedidos.Columns["ContactName"].HeaderText = "Nombre de contacto";
            DgvPedidos.Columns["OrderDate"].HeaderText = "Fecha de pedido";
            DgvPedidos.Columns["RequiredDate"].HeaderText = "Fecha de entrega";
            DgvPedidos.Columns["ShippedDate"].HeaderText = "Fecha de envío";
            DgvPedidos.Columns["Employee"].HeaderText = "Vendedor";
            DgvPedidos.Columns["Shipper"].HeaderText = "Compañía transportista";
            DgvPedidos.Columns["ShipName"].HeaderText = "Enviar a";
        }

        private void ConfDgvDetalle()
        {
            DgvDetalle.Columns["Id"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            DgvDetalle.Columns["Precio"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            DgvDetalle.Columns["Cantidad"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            DgvDetalle.Columns["Descuento"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            DgvDetalle.Columns["Importe"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            DgvDetalle.Columns["Modificar"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
            DgvDetalle.Columns["Eliminar"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
            DgvDetalle.Columns["Precio"].DefaultCellStyle.Format = "c";
            DgvDetalle.Columns["Cantidad"].DefaultCellStyle.Format = "n0";
            DgvDetalle.Columns["Descuento"].DefaultCellStyle.Format = "n2";
            DgvDetalle.Columns["Importe"].DefaultCellStyle.Format = "c";
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            BorrarDatosPedido();
            BorrarMensajesError();
            BorrarDatosBusqueda();
            DeshabilitarControles();
            BtnNota.Enabled = false;
            DgvPedidos.Focus();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            BorrarDatosPedido();
            BorrarMensajesError();
            DeshabilitarControles();
            BtnNota.Enabled = false;
            LlenarDgvPedidos(sender);
            DgvPedidos.Focus();
        }

        private void btnListar_Click(object sender, EventArgs e)
        {
            btnLimpiar.PerformClick();
            LlenarDgvPedidos(null);
            DgvPedidos.Focus();
        }

        private void BorrarDatosPedido()
        {
            errorProvider1.Clear();
            txtId.Text = txtCliente.Text = "";
            txtId.Tag = null;
            cboCategoria.SelectedIndex = 0;
            cboProducto.DataSource = null;
            txtPrecio.Text = "$0.00";
            txtCantidad.Text = txtUInventario.Text = "0";
            txtDescuento.Text = "0.00";
            txtTotal.Text = "$0.00";
            DgvDetalle.Rows.Clear();
        }

        private void BorrarMensajesError() => errorProvider1.Clear();

        private bool ValidarControles()
        {
            bool valida = true;
            if (cboCategoria.SelectedIndex <= 0)
            {
                valida = false;
                errorProvider1.SetError(cboCategoria, "Seleccione la categoría");
            }
            if (cboProducto.SelectedIndex <= 0)
            {
                valida = false;
                errorProvider1.SetError(cboProducto, "Seleccione el producto");
            }
            if (txtCantidad.Text == "" || int.Parse(txtCantidad.Text) == 0)
            {
                valida = false;
                errorProvider1.SetError(txtCantidad, "Ingrese la cantidad");
            }
            if (int.Parse(txtCantidad.Text) > int.Parse(txtUInventario.Text))
            {
                valida = false;
                errorProvider1.SetError(txtCantidad, "La cantidad de productos en el pedido excede el inventario disponible");
            }
            if (txtDescuento.Text == "")
            {
                valida = false;
                errorProvider1.SetError(txtDescuento, "Ingrese el descuento");
            }
            if (decimal.Parse(txtDescuento.Text) > 1 || decimal.Parse(txtDescuento.Text) < 0)
            {
                valida = false;
                errorProvider1.SetError(txtDescuento, "El descuento no puede ser mayor que 1 o menor que 0");
            }
            if (cboProducto.SelectedIndex > 0)
            {
                int numProd = int.Parse(cboProducto.SelectedValue.ToString());
                bool productoDuplicado = false;
                foreach (DataGridViewRow dgvr in DgvDetalle.Rows)
                {
                    if (int.Parse(dgvr.Cells["ProductoId"].Value.ToString()) == numProd)
                    {
                        productoDuplicado = true;
                        break;
                    }
                }
                if (productoDuplicado)
                {
                    valida = false;
                    errorProvider1.SetError(cboProducto, "No se puede tener un producto duplicado en el detalle del pedido");
                }
            }
            string total = txtTotal.Text;
            total = total.Replace("$", "");
            if (txtTotal.Text == "" || (decimal.Parse(total) + (decimal.Parse(txtPrecio.Text.Replace("$", "")) * int.Parse(txtCantidad.Text) * (1 - decimal.Parse(txtDescuento.Text))) == 0 ))
            {
                valida = false;
                errorProvider1.SetError(btnAgregar, "Ingrese el detalle del pedido");
            }
            return valida;
        }

        private void BorrarDatosBusqueda()
        {
            txtBIdInicial.Text = txtBIdFinal.Text = txtBCliente.Text = txtBEmpleado.Text = txtBCompañiaT.Text = txtBDirigidoa.Text = "";
            dtpBFPedidoIni.Checked = dtpBFPedidoFin.Checked = dtpBFRequeridoIni.Checked = dtpBFRequeridoFin.Checked = dtpBFEnvioIni.Checked = dtpBFEnvioFin.Checked = false;
            chkBFPedidoNull.Checked = chkBFRequeridoNull.Checked = chkBFEnvioNull.Checked = false;
        }

        #region eventosDeControles

        private void txtBIdInicial_KeyPress(object sender, KeyPressEventArgs e) => Utils.ValidarDigitosSinPunto(sender, e);

        private void txtBIdInicial_Leave(object sender, EventArgs e) => Utils.ValidaTxtBIdIni(txtBIdInicial, txtBIdFinal);

        private void txtBIdFinal_KeyPress(object sender, KeyPressEventArgs e) => Utils.ValidarDigitosSinPunto(sender, e);

        private void txtBIdFinal_Leave(object sender, EventArgs e) => Utils.ValidaTxtBIdFin(txtBIdInicial, txtBIdFinal);

        private void dtpBFPedidoIni_ValueChanged(object sender, EventArgs e)
        {
            if (dtpBFPedidoIni.Checked)
            {
                dtpBFPedidoFin.Checked = true;
                chkBFPedidoNull.Checked = false;
            }
            else
                dtpBFPedidoFin.Checked = false;
        }

        private void dtpBFPedidoFin_ValueChanged(object sender, EventArgs e)
        {
            if (dtpBFPedidoFin.Checked)
            {
                dtpBFPedidoIni.Checked = true;
                chkBFPedidoNull.Checked = false;
            }
            else
                dtpBFPedidoIni.Checked = false;
        }

        private void dtpBFRequeridoIni_ValueChanged(object sender, EventArgs e)
        {
            if (dtpBFRequeridoIni.Checked)
            {
                dtpBFRequeridoFin.Checked = true;
                chkBFRequeridoNull.Checked = false;
            }
            else
                dtpBFRequeridoFin.Checked = false;
        }

        private void dtpBFRequeridoFin_ValueChanged(object sender, EventArgs e)
        {
            if (dtpBFRequeridoFin.Checked)
            {
                dtpBFRequeridoIni.Checked = true;
                chkBFRequeridoNull.Checked = false;
            }
            else
                dtpBFRequeridoIni.Checked = false;
        }

        private void dtpBFEnvioIni_ValueChanged(object sender, EventArgs e)
        {
            if (dtpBFEnvioIni.Checked)
            {
                dtpBFEnvioFin.Checked = true;
                chkBFEnvioNull.Checked = false;
            }
            else
                dtpBFEnvioFin.Checked = false;
        }

        private void dtpBFEnvioFin_ValueChanged(object sender, EventArgs e)
        {
            if (dtpBFEnvioFin.Checked)
            {
                dtpBFEnvioIni.Checked = true;
                chkBFEnvioNull.Checked = false;
            }
            else
                dtpBFEnvioIni.Checked = false;
        }

        private void chkBFPedidoNull_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBFPedidoNull.Checked)
            {
                dtpBFPedidoIni.Checked = false;
                dtpBFPedidoFin.Checked = false;
            }
        }

        private void chkBFRequeridoNull_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBFRequeridoNull.Checked)
            {
                dtpBFRequeridoIni.Checked = false;
                dtpBFRequeridoFin.Checked = false;
            }
        }

        private void chkBFEnvioNull_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBFEnvioNull.Checked)
            {
                dtpBFEnvioIni.Checked = false;
                dtpBFEnvioFin.Checked = false;
            }
        }

        private void dtpBFPedidoIni_Leave(object sender, EventArgs e)
        {
            if (dtpBFPedidoIni.Checked && dtpBFPedidoFin.Checked)
                if (dtpBFPedidoFin.Value < dtpBFPedidoIni.Value)
                    dtpBFPedidoFin.Value = dtpBFPedidoIni.Value;
        }

        private void dtpBFPedidoFin_Leave(object sender, EventArgs e)
        {
            if (dtpBFPedidoIni.Checked && dtpBFPedidoFin.Checked)
                if (dtpBFPedidoFin.Value < dtpBFPedidoIni.Value)
                    dtpBFPedidoIni.Value = dtpBFPedidoFin.Value;
        }

        private void dtpBFRequeridoIni_Leave(object sender, EventArgs e)
        {
            if (dtpBFRequeridoIni.Checked && dtpBFRequeridoFin.Checked)
                if (dtpBFRequeridoFin.Value < dtpBFRequeridoIni.Value)
                    dtpBFRequeridoFin.Value = dtpBFRequeridoIni.Value;
        }

        private void dtpBFRequeridoFin_Leave(object sender, EventArgs e)
        {
            if (dtpBFRequeridoIni.Checked && dtpBFRequeridoFin.Checked)
                if (dtpBFRequeridoFin.Value < dtpBFRequeridoIni.Value)
                    dtpBFRequeridoIni.Value = dtpBFRequeridoFin.Value;
        }

        private void dtpBFEnvioIni_Leave(object sender, EventArgs e)
        {
            if (dtpBFEnvioIni.Checked && dtpBFEnvioFin.Checked)
                if (dtpBFEnvioFin.Value < dtpBFEnvioIni.Value)
                    dtpBFEnvioFin.Value = dtpBFEnvioIni.Value;
        }

        private void dtpBFEnvioFin_Leave(object sender, EventArgs e)
        {
            if (dtpBFEnvioIni.Checked && dtpBFEnvioFin.Checked)
                if (dtpBFEnvioFin.Value < dtpBFEnvioIni.Value)
                    dtpBFEnvioIni.Value = dtpBFEnvioFin.Value;
        }

        #endregion

        private void cboCategoria_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtPrecio.Text = "$0.00";
            txtUInventario.Text = txtCantidad.Text = "0";
            BorrarMensajesError();
            if (cboCategoria.SelectedIndex > 0)
            {
                try
                {
                    MDIPrincipal.ActualizarBarraDeEstado(Utils.clbdd);
                    var dt = new PedidoRepository(cnStr).ObtenerProductosPorCategorias(int.Parse(cboCategoria.SelectedValue.ToString()));
                    cboProducto.DataSource = dt;
                    cboProducto.DisplayMember = "Producto";
                    cboProducto.ValueMember = "Id";
                    MDIPrincipal.ActualizarBarraDeEstado($"Se muestran {DgvPedidos.RowCount} registros en pedidos");
                }
                catch (Exception ex)
                {
                    Utils.MsgCatchOue(ex);
                }
            }
            else
            {
                MDIPrincipal.ActualizarBarraDeEstado(Utils.clbdd);
                DataTable tbl = new DataTable();
                tbl.Columns.Add("Id", typeof(int));
                tbl.Columns.Add("Producto", typeof(string));
                DataRow dr = tbl.NewRow();
                dr["Id"] = 0;
                dr["Producto"] = "«--- Seleccione ---»";
                tbl.Rows.Add(dr);
                cboProducto.DataSource = tbl;
                cboProducto.DisplayMember = "Producto";
                cboProducto.ValueMember = "Id";
                MDIPrincipal.ActualizarBarraDeEstado($"Se muestran {DgvPedidos.RowCount} registros en pedidos");
            }
        }

        private void cboProducto_SelectedIndexChanged(object sender, EventArgs e)
        {
            BorrarMensajesError();
            if (cboProducto.SelectedIndex > 0)
            {
                try
                {
                    MDIPrincipal.ActualizarBarraDeEstado(Utils.clbdd);
                    var productId = cboProducto.SelectedValue?.ToString();
                    var dtoProductoCostoInventario = new PedidoRepository(cnStr).ObtenerProductoCostoInventario(int.Parse(productId));
                    if (dtoProductoCostoInventario != null)
                    {
                        txtPrecio.Text = dtoProductoCostoInventario.UnitPrice.ToString("c");
                        txtUInventario.Text = dtoProductoCostoInventario.UnitsInStock.ToString();
                        if (dtoProductoCostoInventario.UnitsInStock == 0)
                        {
                            DeshabilitarControlesProducto();
                            Utils.MensajeExclamation("No hay este producto en existencia");
                            cboProducto.SelectedIndex = 0;
                            txtPrecio.Text = "$0.00";
                            txtUInventario.Text = txtCantidad.Text = "0";
                            txtDescuento.Text = "0.00";
                        }
                        else
                            HabilitarControlesProducto();
                    }
                    else
                    {
                        DeshabilitarControlesProducto();
                        txtPrecio.Text = "$0.00";
                        txtUInventario.Text = txtCantidad.Text = "0";
                        txtDescuento.Text = "0.00";
                    }
                    MDIPrincipal.ActualizarBarraDeEstado($"Se muestran {DgvPedidos.RowCount} registros en pedidos");
                }
                catch (Exception ex)
                {
                    Utils.MsgCatchOue(ex);
                }
            }
            else
            {
                DeshabilitarControlesProducto();
                txtPrecio.Text = "$0.00";
                txtUInventario.Text = txtCantidad.Text = "0";
                txtDescuento.Text = "0.00";
            }
        }

        private void DgvPedidos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            BtnNota.Enabled = false;
            BorrarDatosPedido();
            DataGridViewRow dgvr = DgvPedidos.CurrentRow;
            txtId.Text = dgvr.Cells["OrderId"].Value.ToString();
            txtCliente.Text = dgvr.Cells["Customer"].Value.ToString();
            LlenarDatosPedido();
            LlenarDatosDetallePedido();
            HabilitarControles();
        }

        private void LlenarDatosPedido()
        {
            try
            {
                MDIPrincipal.ActualizarBarraDeEstado(Utils.clbdd);
                var pedido = new PedidoRepository(cnStr).ObtenerPedidoPorId(int.Parse(txtId.Text));
                if (pedido != null)
                    txtId.Tag = pedido.RowVersion;
                else
                    txtId.Tag = null;
                MDIPrincipal.ActualizarBarraDeEstado($"Se muestran {DgvPedidos.RowCount} registros en pedidos");
            }
            catch (Exception ex)
            {
                Utils.MsgCatchOue(ex);
            }
        }

        private void LlenarDatosDetallePedido()
        {
            try
            {
                numDetalle = 1;
                MDIPrincipal.ActualizarBarraDeEstado(Utils.clbdd);
                using (var repo = new PedidoRepository(cnStr))
                {
                    var detalles = repo.ObtenerDetallePedidoPorPedidoId(int.Parse(txtId.Text));
                    if (detalles.Count == 0)
                        Utils.MensajeExclamation("No se encontraron detalles para el pedido especificado");
                    else
                    {
                        foreach (var pedidoDetalle in detalles)
                        {
                            var totalLinea = (pedidoDetalle.UnitPrice * pedidoDetalle.Quantity) * (1 - pedidoDetalle.Discount);
                            DgvDetalle.Rows.Add(new object[] 
                            {
                                numDetalle,
                                pedidoDetalle.ProductName,
                                pedidoDetalle.UnitPrice,
                                pedidoDetalle.Quantity,
                                pedidoDetalle.Discount,
                                totalLinea,
                                "  Modificar  ",
                                "  Eliminar  ",
                                pedidoDetalle.ProductID,
                                pedidoDetalle.RowVersion
                            });
                            ++numDetalle;
                        }
                    }
                }
                CalcularTotal();
                MDIPrincipal.ActualizarBarraDeEstado($"Se muestran {DgvPedidos.RowCount} registros en pedidos");
            }
            catch (Exception ex)
            {
                Utils.MsgCatchOue(ex);
            }
        }

        private void CalcularTotal()
        {
            decimal total = 0;
            foreach (DataGridViewRow dgvr in DgvDetalle.Rows)
            {
                decimal importe = (decimal)dgvr.Cells["Importe"].Value;
                total += importe;
            }
            txtTotal.Text = string.Format("{0:c}", total);
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (!chkRowVersion())
            {
                Utils.MensajeExclamation("El registro ha sido modificado por otro usuario de la red, vuelva a cargar el registro para que se actualice con los datos proporcionados por el otro usuario");
                return;
            }
            int numRegs = 0;
            BorrarMensajesError();
            if (ValidarControles())
            {
                try
                {
                    MDIPrincipal.ActualizarBarraDeEstado(Utils.insertandoRegistro);
                    DeshabilitarControles();
                    DeshabilitarControlesProducto();
                    PedidoDetalle pedidoDetalle = new PedidoDetalle();
                    pedidoDetalle.OrderID = int.Parse(txtId.Text);
                    pedidoDetalle.ProductID = int.Parse(cboProducto.SelectedValue.ToString());
                    pedidoDetalle.UnitPrice = decimal.Parse(txtPrecio.Text.Replace("$", ""));
                    pedidoDetalle.Quantity = short.Parse(txtCantidad.Text);
                    pedidoDetalle.Discount = decimal.Parse(txtDescuento.Text);
                    pedidoDetalle.ProductName = cboProducto.Text;
                    numRegs = new PedidoRepository(cnStr).Insertar(pedidoDetalle);
                    if (numRegs > 0)
                        Utils.MensajeInformation($"El producto: {pedidoDetalle.ProductName} del Pedido: {pedidoDetalle.OrderID}, se registró satisfactoriamente");
                    else
                        Utils.MensajeError($"El producto: {pedidoDetalle.ProductName} del Pedido: {pedidoDetalle.OrderID}, NO se registró en la base de datos");
                    MDIPrincipal.ActualizarBarraDeEstado($"Se muestran {DgvPedidos.RowCount} registros de pedidos");
                }
                catch (Exception ex)
                {
                    Utils.MsgCatchOue(ex);
                }
                if (numRegs > 0)
                {
                    BorrarDatosDetallePedido();
                    LlenarDatosDetallePedido();
                    HabilitarControles();
                    MDIPrincipal.ActualizarBarraDeEstado($"Se muestran {DgvPedidos.RowCount} registros de pedidos");
                    BtnNota.Enabled = true;
                    DgvDetalle.Focus();
                }
            }
        }

        private void BorrarDatosAgregarProducto()
        {
            cboCategoria.SelectedIndex = 0;
            cboProducto.DataSource = null;
            txtPrecio.Text = "$0.00";
            txtCantidad.Text = txtUInventario.Text = "0";
            txtDescuento.Text = "0.00";
        }

        private void BorrarDatosDetallePedido()
        {
            cboCategoria.SelectedIndex = 0;
            cboProducto.DataSource = null;
            txtPrecio.Text = "$0.00";
            txtCantidad.Text = txtUInventario.Text = "0";
            txtDescuento.Text = "0.00";
            txtTotal.Text = "$0.00";
            DgvDetalle.Rows.Clear();
        }

        private void DgvDetalle_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (!chkRowVersion())
            {
                Utils.MensajeInformation("El registro ha sido modificado por otro usuario de la red, vuelva a cargar el registro para que se actualice con los datos proporcionados por el otro usuario");
                return;
            }
            if (e.ColumnIndex == DgvDetalle.Columns["Eliminar"].Index)
            {
                DataGridViewRow dgvr = DgvDetalle.CurrentRow;
                PedidoDetalle pedidoDetalle = new PedidoDetalle();
                pedidoDetalle.OrderID = int.Parse(txtId.Text);
                pedidoDetalle.ProductID = (int)dgvr.Cells["ProductoId"].Value;
                pedidoDetalle.ProductName = dgvr.Cells["Producto"].Value.ToString();
                EliminarProducto(pedidoDetalle);
                BtnNota.Enabled = true;
            }
            if (e.ColumnIndex == DgvDetalle.Columns["Modificar"].Index)
            {
                DataGridViewRow dgvr = DgvDetalle.CurrentRow;
                using (FrmPedidosDetalleModificar frmPedidosDetalleModificar = new FrmPedidosDetalleModificar())
                {
                    frmPedidosDetalleModificar.Owner = this;
                    frmPedidosDetalleModificar.PedidoId = int.Parse(txtId.Text);
                    frmPedidosDetalleModificar.ProductoId = int.Parse(dgvr.Cells["ProductoId"].Value.ToString());
                    frmPedidosDetalleModificar.Producto = dgvr.Cells["Producto"].Value.ToString();
                    frmPedidosDetalleModificar.Precio = decimal.Parse(dgvr.Cells["Precio"].Value.ToString());
                    frmPedidosDetalleModificar.Cantidad = short.Parse(dgvr.Cells["Cantidad"].Value.ToString());
                    frmPedidosDetalleModificar.Descuento = decimal.Parse(dgvr.Cells["Descuento"].Value.ToString());
                    frmPedidosDetalleModificar.Importe = decimal.Parse(dgvr.Cells["Importe"].Value.ToString());
                    DialogResult dialogResult = frmPedidosDetalleModificar.ShowDialog();
                    if (dialogResult == DialogResult.OK)
                    {
                        BtnNota.Enabled = true;
                        BorrarDatosDetallePedido();
                        LlenarDatosDetallePedido();
                    }
                }
            }
            DgvDetalle.Focus();
        }

        private void EliminarProducto(PedidoDetalle pedidoDetalle)
        {
            int numRegs = 0;
            BorrarMensajesError();
            BorrarDatosAgregarProducto();
            try
            {
                if (Utils.MensajeQuestion($"¿Esta seguro de eliminar el producto: {pedidoDetalle.ProductName} del pedido: {pedidoDetalle.OrderID}?") == DialogResult.Yes)
                {
                    MDIPrincipal.ActualizarBarraDeEstado(Utils.eliminandoRegistro);
                    DeshabilitarControles();
                    DeshabilitarControlesProducto();
                    numRegs = new PedidoRepository(cnStr).Eliminar(pedidoDetalle);
                    if (numRegs > 0)
                        Utils.MensajeInformation($"El producto: {pedidoDetalle.ProductName} del Pedido: {pedidoDetalle.OrderID}, se eliminó satisfactoriamente");
                    else
                        Utils.MensajeError($"El producto: {pedidoDetalle.ProductName} del Pedido: {pedidoDetalle.OrderID}, NO se eliminó en la base de datos, es posible que el registro se haya eliminado por otro usuario de la red");
                }
            }
            catch (Exception ex)
            {
                Utils.MsgCatchOue(ex);
            }
            if (numRegs > 0)
            {
                BorrarDatosDetallePedido();
                LlenarDatosDetallePedido();
                HabilitarControles();
                MDIPrincipal.ActualizarBarraDeEstado($"Se muestran {DgvPedidos.RowCount} registros de pedidos");
            }
        }

        private void BtnNota_Click(object sender, EventArgs e)
        {
            if (!chkRowVersion())
            {
                Utils.MensajeExclamation("El registro ha sido modificado por otro usuario de la red, se mostrará la nota de remisión con los datos proporcionados por el otro usuario");
            }
            FrmRptNotaRemision8 frmRptNotaRemision8 = new FrmRptNotaRemision8();
            frmRptNotaRemision8.Id = int.Parse(txtId.Text);
            frmRptNotaRemision8.ShowDialog();
        }

        private bool chkRowVersion()
        {
            bool rowVersionOk = true;
            if (txtId.Tag == null)
                return true;
            if (!int.TryParse(txtId.Text, out int pedidoId))
                return false;
            int rowVersion = (int)txtId.Tag;
            try
            {
                MDIPrincipal.ActualizarBarraDeEstado(Utils.clbdd);
                var repo = new PedidoRepository(cnStr);
                Pedido pedido = repo.ObtenerPedidoPorId(pedidoId);
                if (pedido == null || pedido.RowVersion != rowVersion)
                    return false;
                // Validar filas del grid contra DB 
                // 1) Validar que cada fila del grid exista en DB y coincida RowVersion
                foreach (DataGridViewRow dgvr in DgvDetalle.Rows)
                {
                    if (!int.TryParse(dgvr.Cells["ProductoId"].Value?.ToString(), out int productoId))
                        return false;
                    if (!int.TryParse(dgvr.Cells["RowVersion"].Value?.ToString(), out int rowVersionGrid))
                        return false;
                    int? rowVersionDetalleEnDB = repo.DetallePedidosChkRowVersion(pedidoId, productoId);
                    if (!rowVersionDetalleEnDB.HasValue || rowVersionGrid != rowVersionDetalleEnDB.Value)
                        return false;
                }
                // Validar que DB no tenga detalles adicionales
                // 2) Validar que cada detalle en DB exista en el grid y coincida RowVersion
                List<PedidoDetalle> pedidoDetalles = repo.ObtenerDetallePedidoPorPedidoId(pedidoId);

                if (pedidoDetalles != null)
                {
                    // Construir diccionario del grid para búsquedas O(1)
                    var gridMap = new Dictionary<int, int>(); // productoId -> rowVersionGrid
                    foreach (DataGridViewRow dgvr in DgvDetalle.Rows)
                    {
                        if (int.TryParse(dgvr.Cells["ProductoId"].Value?.ToString(), out int pid) &&
                            int.TryParse(dgvr.Cells["RowVersion"].Value?.ToString(), out int rv))
                        {
                            gridMap[pid] = rv;
                        }
                    }
                    foreach (var pd in pedidoDetalles)
                    {
                        if (!gridMap.TryGetValue(pd.ProductID, out int rowVersionGrid) || rowVersionGrid != pd.RowVersion)
                            return false;
                    }
                }
                else
                {
                    // Política: si DB no tiene detalles y el grid sí, considerarlo inconsistente
                    if (DgvDetalle.Rows.Count > 0)
                        return false;
                }
                MDIPrincipal.ActualizarBarraDeEstado($"Se muestran {DgvPedidos.RowCount} registros en pedidos");
            }
            catch (Exception ex)
            {
                Utils.MsgCatchOue(ex);
                return false;
            }
            return rowVersionOk;
        }
    }
}
