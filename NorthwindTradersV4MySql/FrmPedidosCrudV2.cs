using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Windows.Forms;

namespace NorthwindTradersV4MySql
{
    public partial class FrmPedidosCrudV2 : Form
    {
        string cnStr = ConfigurationManager.ConnectionStrings["NorthwindMySql"].ConnectionString;
        private TabPage lastSelectedTab;
        bool EventoCargardo = true; // esta variable es necesaria para controlar el manejador de eventos de la celda del dgv, ojo no quitar
        int IdDetalle = 1;
        bool PedidoGenerado = false;

        public FrmPedidosCrudV2()
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
        }

        private void GrbPaint(object sender, PaintEventArgs e) => Utils.GrbPaint(this, sender, e);

        private void GrbPaint2(object sender, PaintEventArgs e) => Utils.GrbPaint2(this, sender, e);

        private void FrmPedidosCrudV2_FormClosed(object sender, FormClosedEventArgs e) => MDIPrincipal.ActualizarBarraDeEstado();

        private void FrmPedidosCrudV2_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (tabcOperacion.SelectedTab == tabpRegistrar)
                if (cboCliente.SelectedIndex > 0 || cboEmpleado.SelectedIndex > 0 || cboTransportista.SelectedIndex > 0 || cboCategoria.SelectedIndex > 0 || cboProducto.SelectedIndex > 0 || dgvDetalle.RowCount > 0)
                    if (Utils.MensajeQuestion(Utils.preguntaCerrar) == DialogResult.No)
                        e.Cancel = true;
        }

        private void FrmPedidosCrudV2_Load(object sender, EventArgs e)
        {
            dtpHoraRequerido.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            dtpHoraEnvio.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            DeshabilitarControles();
            DeshabilitarControlesProducto();
            Utils.LlenarCbo(cboCliente, "spClientesSeleccionar", "CompanyName", "CustomerId");
            Utils.LlenarCbo(cboEmpleado, "spEmpleadosSeleccionar", "EmployeeName", "EmployeeId");
            Utils.LlenarCbo(cboTransportista, "spTransportistasSeleccionar", "CompanyName", "ShipperId");
            Utils.LlenarCbo(cboCategoria, "spCategoriasSeleccionar", "CategoryName", "CategoryId");
            Utils.ConfDgv(dgvPedidos);
            Utils.ConfDgv(dgvDetalle);
            LlenarDgvPedidos(null);
            ConfDgvPedidos();
            ConfDgvDetalle();
            OcultarCols();
            InicializarValores();
        }

        private void DeshabilitarControles()
        {
            cboCliente.Enabled = cboEmpleado.Enabled = cboTransportista.Enabled = false;
            dtpPedido.Enabled = dtpHoraPedido.Enabled = dtpRequerido.Enabled = dtpHoraRequerido.Enabled = dtpEnvio.Enabled = dtpHoraEnvio.Enabled = false;
            txtDirigidoa.ReadOnly = txtDomicilio.ReadOnly = txtCiudad.ReadOnly = txtRegion.ReadOnly = txtCP.ReadOnly = txtPais.ReadOnly = txtFlete.ReadOnly = true;
            btnGenerar.Enabled = false;
        }

        private void HabilitarControles()
        {
            cboCliente.Enabled = cboEmpleado.Enabled = cboTransportista.Enabled = true;
            dtpPedido.Enabled = dtpRequerido.Enabled = dtpEnvio.Enabled = true;
            txtDirigidoa.ReadOnly = txtDomicilio.ReadOnly = txtCiudad.ReadOnly = txtRegion.ReadOnly = txtCP.ReadOnly = txtPais.ReadOnly = txtFlete.ReadOnly = false;
            btnGenerar.Enabled = true;
        }

        private void DeshabilitarControlesProducto()
        {
            cboCategoria.Enabled = cboProducto.Enabled = false;
            txtCantidad.Enabled = txtDescuento.Enabled = false;
            btnAgregar.Enabled = false;
        }

        private void HabilitarControlesProducto()
        {
            txtCantidad.Enabled = txtDescuento.Enabled = true;
            btnAgregar.Enabled = true;
        }

        private bool ValidarControles()
        {
            errorProvider1.Clear();
            bool valida = true;
            if (cboCliente.SelectedIndex <= 0)
            {
                valida = false;
                errorProvider1.SetError(cboCliente, "Ingrese el cliente");
            }
            if (cboEmpleado.SelectedIndex <= 0)
            {
                valida = false;
                errorProvider1.SetError(cboEmpleado, "Ingrese el empleado");
            }
            if (dtpPedido.Checked == false)
            {
                valida = false;
                errorProvider1.SetError(dtpPedido, "Ingrese la fecha de pedido");
            }
            if (cboTransportista.SelectedIndex <= 0)
            {
                valida = false;
                errorProvider1.SetError(cboTransportista, "Ingrese la compañía transportista");
            }
            if (cboProducto.SelectedIndex > 0) 
            {
                valida = false;
                errorProvider1.SetError(cboProducto, "Ha seleccionado un producto y no lo ha agregado al pedido");
            }
            string total = txtTotal.Text.Replace("$", "");
            if (txtTotal.Text == "" || decimal.Parse(total) == 0)
            {
                valida = false;
                errorProvider1.SetError(btnAgregar, "Ingrese el detalle del pedido");
                errorProvider1.SetError(txtTotal, "El total del pedido no puede ser cero");
            }
            return valida;
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
                dgvPedidos.DataSource = dt;
                if (sender == null)
                    MDIPrincipal.ActualizarBarraDeEstado($"Se muestran los últimos {dgvPedidos.RowCount} pedidos registrados");
                else
                    MDIPrincipal.ActualizarBarraDeEstado($"Se encontraron {dgvPedidos.RowCount} registros");
            }
            catch (Exception ex)
            {
                Utils.MsgCatchOue(ex);
            }
        }

        private void ConfDgvPedidos()
        {
            dgvPedidos.Columns["OrderId"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

            dgvPedidos.Columns["OrderDate"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvPedidos.Columns["RequiredDate"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvPedidos.Columns["ShippedDate"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvPedidos.Columns["Shipper"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvPedidos.Columns["Employee"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgvPedidos.Columns["OrderDate"].DefaultCellStyle.Format = "ddd dd\" de \"MMM\" de \"yyyy\n hh:mm:ss tt";
            dgvPedidos.Columns["RequiredDate"].DefaultCellStyle.Format = "ddd dd\" de \"MMM\" de \"yyyy\n hh:mm:ss tt";
            dgvPedidos.Columns["ShippedDate"].DefaultCellStyle.Format = "ddd dd\" de \"MMM\" de \"yyyy\n hh:mm:ss tt";

            dgvPedidos.Columns["OrderId"].HeaderText = "Id";
            dgvPedidos.Columns["Customer"].HeaderText = "Cliente";
            dgvPedidos.Columns["ContactName"].HeaderText = "Nombre de contacto";
            dgvPedidos.Columns["OrderDate"].HeaderText = "Fecha de pedido";
            dgvPedidos.Columns["RequiredDate"].HeaderText = "Fecha de entrega";
            dgvPedidos.Columns["ShippedDate"].HeaderText = "Fecha de envío";
            dgvPedidos.Columns["Employee"].HeaderText = "Vendedor";
            dgvPedidos.Columns["Shipper"].HeaderText = "Compañía transportista";
            dgvPedidos.Columns["ShipName"].HeaderText = "Enviar a";
        }

        private void ConfDgvDetalle()
        {
            dgvDetalle.Columns["Id"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvDetalle.Columns["Precio"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvDetalle.Columns["Cantidad"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvDetalle.Columns["Descuento"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvDetalle.Columns["Importe"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        }

        private void OcultarCols() => dgvDetalle.Columns["Modificar"].Visible = dgvDetalle.Columns["Eliminar"].Visible = false;

        private void MostrarCols() => dgvDetalle.Columns["Modificar"].Visible = dgvDetalle.Columns["Eliminar"].Visible = true;

        private void InicializarValores()
        {
            txtFlete.Text = txtTotal.Text = txtPrecio.Text = "$0.00";
            txtDescuento.Text = "0.00";
            txtUInventario.Text = txtCantidad.Text = "0";
        }

        private void InicializarValoresProducto()
        {
            txtPrecio.Text = "$0.00";
            txtUInventario.Text = txtCantidad.Text = "0";
            txtDescuento.Text = "0.00";
        }

        private void InicializarValoresTransportar() => txtDirigidoa.Text = txtDomicilio.Text = txtCiudad.Text = txtRegion.Text = txtCP.Text = txtPais.Text = "";

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            BorrarDatosPedido();
            BorrarMensajesError();
            BorrarDatosBusqueda();
            if (tabcOperacion.SelectedTab != tabpRegistrar)
            {
                DeshabilitarControles();
                DeshabilitarControlesProducto();
            }
            btnNota.Enabled = false;
            LlenarDgvPedidos(null);
            dgvPedidos.Focus();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            BorrarDatosPedido();
            BorrarMensajesError();
            if (tabcOperacion.SelectedTab != tabpRegistrar)
            {
                DeshabilitarControles();
                DeshabilitarControlesProducto();
            }
            btnNota.Enabled=false;
            LlenarDgvPedidos(sender);
            dgvPedidos.Focus();
        }

        private void BorrarDatosPedido()
        {
            txtId.Text = "";
            txtId.Tag = null;
            cboCliente.SelectedIndex = cboEmpleado.SelectedIndex = cboTransportista.SelectedIndex = cboCategoria.SelectedIndex = 0;
            cboProducto.DataSource = null;
            dtpPedido.Value = dtpRequerido.Value = dtpEnvio.Value = DateTime.Now;
            dtpHoraPedido.Value = DateTime.Now;
            dtpHoraRequerido.Value = dtpHoraEnvio.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            dtpRequerido.Checked = dtpEnvio.Checked = false;
            txtDirigidoa.Text = txtDomicilio.Text = txtCiudad.Text = txtRegion.Text = txtCP.Text = txtPais.Text = "";
            InicializarValores();
            btnNota.Visible = false;
            dgvDetalle.Rows.Clear();
        }

        private void BorrarMensajesError() => errorProvider1.Clear();

        private void BorrarDatosBusqueda()
        {
            txtBIdInicial.Text = txtBIdFinal.Text = txtBCliente.Text = txtBEmpleado.Text = txtBCompañiaT.Text = txtBDirigidoa.Text = "";
            dtpBFPedidoIni.Value = dtpBFPedidoFin.Value = dtpBFRequeridoIni.Value = dtpBFRequeridoFin.Value = dtpBFEnvioIni.Value = dtpBFEnvioFin.Value = DateTime.Today;
            dtpBFPedidoIni.Checked = dtpBFPedidoFin.Checked = dtpBFRequeridoIni.Checked = dtpBFRequeridoFin.Checked = dtpBFEnvioIni.Checked = dtpBFEnvioFin.Checked = false;
            chkBFPedidoNull.Checked = chkBFRequeridoNull.Checked = chkBFEnvioNull.Checked = false;
        }

        private void txtBIdInicial_KeyPress(object sender, KeyPressEventArgs e) => Utils.ValidarDigitosSinPunto(sender, e);

        private void txtBIdFinal_KeyPress(object sender, KeyPressEventArgs e) => Utils.ValidarDigitosSinPunto(sender, e);

        private void txtBIdInicial_Leave(object sender, EventArgs e) => Utils.ValidaTxtBIdIni(txtBIdInicial, txtBIdFinal);

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

        private void cboCategoria_SelectedIndexChanged(object sender, EventArgs e)
        {
            InicializarValoresProducto();
            if (cboCategoria.SelectedIndex > 0)
            {
                Utils.LlenarCbo(cboProducto, "spProductosSeleccionar", "Producto", "Id", "p_Categoria", cboCategoria.SelectedValue);
                cboProducto.Enabled = true;
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
            }
            MDIPrincipal.ActualizarBarraDeEstado($"Se muestran {dgvPedidos.RowCount} registros en pedidos");
        }

        private void cboCliente_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboCliente.SelectedIndex > 0)
            {
                try
                {
                    MDIPrincipal.ActualizarBarraDeEstado(Utils.clbdd);
                    var customerId = cboCliente.SelectedValue?.ToString();
                    var dtoEnvioInformacion = new PedidoRepository(cnStr).ObtenerInformacionEnvio(customerId);
                    if (dtoEnvioInformacion != null)
                    {
                        txtDirigidoa.Text = dtoEnvioInformacion.ShipName ?? "";
                        txtDomicilio.Text = dtoEnvioInformacion.ShipAddress ?? "";
                        txtCiudad.Text = dtoEnvioInformacion.ShipCity ?? "";
                        txtRegion.Text = dtoEnvioInformacion.ShipRegion ?? "";
                        txtCP.Text = dtoEnvioInformacion.ShipPostalCode ?? "";
                        txtPais.Text = dtoEnvioInformacion.ShipCountry ?? "";
                    }
                    else
                        InicializarValoresTransportar();
                    MDIPrincipal.ActualizarBarraDeEstado($"Se muestran {dgvPedidos.RowCount} registros en pedidos");
                }
                catch (Exception ex)
                {
                    Utils.MsgCatchOue(ex);
                }
            }
            else
                InicializarValoresTransportar();
        }

        private void cboProducto_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboProducto.SelectedIndex > 0)
            {
                try
                {
                    MDIPrincipal.ActualizarBarraDeEstado(Utils.clbdd);
                    InicializarValoresProducto();
                    var productId = cboProducto.SelectedValue?.ToString();
                    var dtoProductoCostoInventario = new PedidoRepository(cnStr).ObtenerProductoCostoInventario(int.Parse(productId));
                    if (dtoProductoCostoInventario != null)
                    {
                        txtPrecio.Text = dtoProductoCostoInventario.UnitPrice.ToString("c");
                        txtUInventario.Text = dtoProductoCostoInventario.UnitsInStock.ToString();
                        if (dtoProductoCostoInventario.UnitsInStock == 0)
                        {
                            //txtCantidad.Leave -= new EventHandler(txtCantidad_Leave);
                            txtCantidad.Validating -= new CancelEventHandler(txtCantidad_Validating);
                            DeshabilitarControlesProducto();
                            //txtCantidad.Leave += new EventHandler(txtCantidad_Leave);
                            txtCantidad.Validating += new CancelEventHandler(txtCantidad_Validating);
                            Utils.MensajeExclamation("No hay este producto en existencia");
                            InicializarValoresProducto();
                            BorrarMensajesError();
                            cboCategoria.Enabled = true;
                            cboProducto.SelectedIndex = 0;
                        }
                        else
                            HabilitarControlesProducto();
                    }
                    else
                    {
                        DeshabilitarControlesProducto();
                        InicializarValoresProducto();
                    }
                    MDIPrincipal.ActualizarBarraDeEstado($"Se muestran {dgvPedidos.RowCount} registros en pedidos");
                }
                catch (Exception ex)
                {
                    Utils.MsgCatchOue(ex);
                }
            }
            else
            {
                DeshabilitarControlesProducto();
                cboCategoria.Enabled = true;
                InicializarValoresProducto();
            }
        }

        private void CalcularTotal()
        {
            decimal total = 0, importe = 0;
            foreach (DataGridViewRow dgvr in dgvDetalle.Rows)
            {
                importe = decimal.Parse(dgvr.Cells["Importe"].Value.ToString());
                total += importe;
            }
            txtTotal.Text = string.Format("{0:c}", total);
        }

        private void txtDescuento_Enter(object sender, EventArgs e) => txtDescuento.Text = "";

        private void txtDescuento_Leave(object sender, EventArgs e)
        {
            if (txtDescuento.Text == "")
                txtDescuento.Text = "0.00";
        }

        private void txtFlete_Enter(object sender, EventArgs e)
        {
            if (txtFlete.Text.Contains("$")) txtFlete.Text = txtFlete.Text.Replace("$", "");
            if (decimal.Parse(txtFlete.Text) == 0) txtFlete.Text = "";
        }

        private void txtFlete_Leave(object sender, EventArgs e)
        {
            if (txtFlete.Text == "") txtFlete.Text = "0.00";
            decimal flete = decimal.Parse(txtFlete.Text);
            txtFlete.Text = flete.ToString("c");
        }

        private void dtpPedido_ValueChanged(object sender, EventArgs e)
        {
            if (dtpPedido.Checked)
            {
                dtpHoraPedido.Value = DateTime.Now; // este es para que me ponga el componente del time
                dtpHoraPedido.Enabled = true;
            }
            else
            {
                dtpHoraPedido.Value = DateTime.Today; // este es para que no me ponga el componente del time
                dtpHoraPedido.Enabled = false;
            }
        }

        private void dtpRequerido_ValueChanged(object sender, EventArgs e)
        {
            if (dtpRequerido.Checked)
            {
                dtpHoraRequerido.Value = Convert.ToDateTime(DateTime.Today.ToShortDateString() + " 12:00:00.000");
                dtpHoraRequerido.Enabled = true;
            }
            else
            {
                dtpHoraRequerido.Value = DateTime.Today;
                dtpHoraRequerido.Enabled = false;
            }
        }

        private void dtpEnvio_ValueChanged(object sender, EventArgs e)
        {
            if (dtpEnvio.Checked)
            {
                dtpHoraEnvio.Value = Convert.ToDateTime(DateTime.Today.ToShortDateString() + " 12:00:00.000");
                dtpHoraEnvio.Enabled = true;
            }
            else
            {
                dtpHoraEnvio.Value = DateTime.Today;
                dtpHoraEnvio.Enabled = false;
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (!chkRowVersion())
            {
                Utils.MensajeExclamation("El registro ha sido modificado por otro usuario de la red, vuelva a cargar el registro para que se actualice con los datos proporcionados por el otro usuario");
                return;
            }
            BorrarMensajesError();
            if (cboCategoria.SelectedIndex <= 0)
            {
                errorProvider1.SetError(cboCategoria, "Seleccione la categoría");
                return;
            }
            if (cboProducto.SelectedIndex <= 0)
            {
                errorProvider1.SetError(cboProducto, "Seleccione el producto");
                return;
            }
            if (txtCantidad.Text == "" || int.Parse(txtCantidad.Text) == 0)
            {
                errorProvider1.SetError(txtCantidad, "Ingrese la cantidad");
                return;
            }
            if (decimal.Parse(txtDescuento.Text) > 1 || decimal.Parse(txtDescuento.Text) < 0)
            {
                errorProvider1.SetError(txtDescuento, "El descuento no puede ser mayor que 1 o menor que 0");
                return;
            }
            if (int.Parse(txtCantidad.Text) > int.Parse(txtUInventario.Text))
            {
                errorProvider1.SetError(txtCantidad, "La cantidad de productos en el pedido excede el inventario disponible");
                return;
            }
            int numProd = int.Parse(cboProducto.SelectedValue.ToString());
            bool productoDuplicado = false;
            foreach (DataGridViewRow dgvr in dgvDetalle.Rows)
            {
                if (int.Parse(dgvr.Cells["ProductoId"].Value.ToString()) == numProd)
                {
                    productoDuplicado = true;
                    break;
                }
            }
            if (productoDuplicado)
            {
                errorProvider1.SetError(cboProducto, "No se puede tener un producto duplicado en el detalle del pedido");
                return;
            }
            string total = txtTotal.Text.Replace("$", "");
            if (txtTotal.Text == "" || (decimal.Parse(total) + (decimal.Parse(txtPrecio.Text.Replace("$", "")) * int.Parse(txtCantidad.Text) * (1 - decimal.Parse(txtDescuento.Text))) == 0))
            {
                errorProvider1.SetError(btnAgregar, "Ingrese el detalle del pedido");
                return;
            }
            DeshabilitarControlesProducto();
            if (tabcOperacion.SelectedTab == tabpRegistrar & !PedidoGenerado)
            {
                txtPrecio.Text = txtPrecio.Text.Replace("$", "");
                dgvDetalle.Rows.Add(new object[] { IdDetalle, cboProducto.Text, txtPrecio.Text, txtCantidad.Text, txtDescuento.Text, ((decimal.Parse(txtPrecio.Text) * decimal.Parse(txtCantidad.Text)) * (1 - decimal.Parse(txtDescuento.Text))).ToString(), "Modificar", "Eliminar", cboProducto.SelectedValue, txtUInventario.Text });
                CalcularTotal();
                ++IdDetalle;
                cboCategoria.SelectedIndex = cboProducto.SelectedIndex = 0;
                InicializarValoresProducto();
                cboCategoria.Focus();
            }
            else if (tabcOperacion.SelectedTab == tabpModificar | (tabcOperacion.SelectedTab == tabpRegistrar & PedidoGenerado))
            {
                int numRegs = 0;
                try
                {
                    MDIPrincipal.ActualizarBarraDeEstado(Utils.insertandoRegistro);
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
                    MDIPrincipal.ActualizarBarraDeEstado($"Se muestran {dgvPedidos.RowCount} registros de pedidos");
                }
                catch (Exception ex)
                {
                    if (ex.InnerException != null && ex.InnerException is MySqlException sqlEx && sqlEx.Number == 1062)
                    {
                        BorrarDatosAgregarProducto();
                        HabilitarControlesProducto();
                    }
                    Utils.MsgCatchOue(ex);
                }
                if (numRegs > 0)
                {
                    BorrarDatosDetallePedido();
                    LlenarDatosDetallePedido();
                    cboCategoria.Enabled = true;
                    btnAgregar.Enabled = true;
                    btnNota.Enabled = true;
                    btnNota.Visible = true;
                    MDIPrincipal.ActualizarBarraDeEstado($"Se muestran {dgvPedidos.RowCount} registros de pedidos");
                    dgvDetalle.Focus();
                }
            }
        }

        private void dgvDetalle_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == 2 && e.Value != null) e.Value = decimal.Parse(e.Value.ToString()).ToString("c");
            if (e.ColumnIndex == 3 && e.Value != null) e.Value = decimal.Parse(e.Value.ToString()).ToString("n0");
            if (e.ColumnIndex == 4 && e.Value != null) e.Value = decimal.Parse(e.Value.ToString()).ToString("n2");
            if (e.ColumnIndex == 5 && e.Value != null) e.Value = decimal.Parse(e.Value.ToString()).ToString("c");
        }

        private void txtFlete_KeyPress(object sender, KeyPressEventArgs e) => Utils.ValidarDigitosConPunto(sender, e);

        private void txtCantidad_KeyPress(object sender, KeyPressEventArgs e) => Utils.ValidarDigitosSinPunto(sender, e);

        private void txtDescuento_KeyPress(object sender, KeyPressEventArgs e) => Utils.ValidarDigitosConPunto(sender, e);

        private void txtCantidad_Validating(object sender, CancelEventArgs e)
        {
            if (txtCantidad.Text != "")
            {
                if (int.Parse(txtCantidad.Text.Replace(",", "")) > 32767)
                {
                    errorProvider1.SetError(txtCantidad, "La cantidad no puede ser mayor a 32,767");
                    e.Cancel = true;
                    return;
                }
                else
                    errorProvider1.SetError(txtCantidad, "");
                if (int.Parse(txtCantidad.Text) > int.Parse(txtUInventario.Text))
                {
                    errorProvider1.SetError(txtCantidad, "La cantidad de productos en el pedido excede el inventario disponible");
                    e.Cancel = true;
                }
            }
        }

        private void tabcOperacion_Selected(object sender, TabControlEventArgs e)
        {
            lastSelectedTab = e.TabPage; // actualizar la pestaña actual
            IdDetalle = 1;
            BorrarDatosPedido();
            BorrarMensajesError();
            if (tabcOperacion.SelectedTab == tabpRegistrar)
            {
                if (EventoCargardo)
                {
                    dgvPedidos.CellClick -= new DataGridViewCellEventHandler(dgvPedidos_CellClick);
                    EventoCargardo = false;
                }
                PedidoGenerado = false;
                BorrarDatosBusqueda();
                HabilitarControles();
                cboCategoria.Enabled = true;
                btnGenerar.Text = "Generar pedido";
                btnGenerar.Visible = true;
                btnGenerar.Enabled = true;
                btnAgregar.Visible = true;
                btnAgregar.Enabled = true;
                dgvDetalle.Columns["Eliminar"].Visible = true;
                dgvDetalle.Columns["Modificar"].Visible = true;
                grbProducto.Enabled = true;
                btnNota.Visible = true;
                btnNota.Enabled = false;
                btnNuevo.Visible = true;
                btnNuevo.Enabled = false;
            }
            else
            {
                if (!EventoCargardo)
                {
                    dgvPedidos.CellClick += new DataGridViewCellEventHandler(dgvPedidos_CellClick);
                    EventoCargardo = true;
                }
                DeshabilitarControles();
                DeshabilitarControlesProducto();
                OcultarCols();
                if (tabcOperacion.SelectedTab == tabpConsultar)
                {
                    btnGenerar.Visible = false;
                    btnAgregar.Visible = false;
                    btnNota.Visible = true;
                    btnNota.Enabled = false;
                    btnNuevo.Visible = false;
                    btnNuevo.Enabled = false;
                }
                else if (tabcOperacion.SelectedTab == tabpModificar)
                {
                    PedidoGenerado = false;
                    btnGenerar.Text = "Modificar pedido";
                    btnGenerar.Visible = true;
                    btnAgregar.Visible = true;
                    btnNota.Visible = true;
                    btnNota.Enabled = false;
                    btnNuevo.Visible = false;
                    btnNuevo.Enabled = false;
                    MostrarCols();
                }
                else if (tabcOperacion.SelectedTab == tabpEliminar)
                {
                    btnGenerar.Text = "Eliminar pedido";
                    btnGenerar.Visible = true;
                    btnAgregar.Visible = false;
                    btnNota.Visible = false;
                    btnNota.Enabled = false;
                    btnNuevo.Visible = false;
                    btnNuevo.Enabled = false;
                }
            }
        }

        private void dgvPedidos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            btnNota.Enabled=false;
            if (tabcOperacion.SelectedTab != tabpRegistrar)
            {
                BorrarDatosPedido();
                DataGridViewRow dgvr = dgvPedidos.CurrentRow;
                txtId.Text = dgvr.Cells["OrderId"].Value.ToString();
                LlenarDatosPedido();
                LlenarDatosDetallePedido();
                DeshabilitarControles();
                DeshabilitarControlesProducto();
                if (tabcOperacion.SelectedTab == tabpConsultar)
                {
                    btnNota.Visible = true;
                    btnNota.Enabled = true;
                    btnNuevo.Visible = false;
                }
                else if (tabcOperacion.SelectedTab == tabpModificar)
                {
                    HabilitarControles();
                    btnGenerar.Enabled = true;
                    cboCategoria.Enabled = true;
                    btnNota.Visible = true;
                    btnNota.Enabled = false;
                    btnNuevo.Visible = false;
                }
                else if (tabcOperacion.SelectedTab == tabpEliminar)
                {
                    btnGenerar.Enabled = true;
                    btnNota.Visible = false;
                    btnNuevo .Visible = false;
                }
            }
        }

        private void LlenarDatosPedido()
        {
            try
            {
                MDIPrincipal.ActualizarBarraDeEstado(Utils.clbdd);
                var pedido = new PedidoRepository(cnStr).ObtenerPedidoPorId(int.Parse(txtId.Text));
                if (pedido != null)
                {
                    txtId.Text = pedido.OrderID.ToString();
                    cboCliente.SelectedIndexChanged -= new EventHandler(cboCliente_SelectedIndexChanged);
                    cboCliente.SelectedValue = string.IsNullOrEmpty(pedido.CustomerID) ? 0 : (object)pedido.CustomerID;
                    cboCliente.SelectedIndexChanged += new EventHandler(cboCliente_SelectedIndexChanged);
                    cboEmpleado.SelectedValue = pedido.EmployeeID ?? 0;
                    cboTransportista.SelectedValue = pedido.ShipVia ?? 0;
                    txtDirigidoa.Text = pedido.ShipName ?? "";
                    txtDomicilio.Text = pedido.ShipAddress ?? "";
                    txtCiudad.Text = pedido.ShipCity ?? "";
                    txtRegion.Text = pedido.ShipRegion ?? "";
                    txtCP.Text = pedido.ShipPostalCode ?? "";
                    txtPais.Text = pedido.ShipCountry ?? "";
                    decimal flete = pedido.Freight ?? 0;
                    txtFlete.Text = flete.ToString("c2");
                    if (pedido.OrderDate.HasValue)
                    {
                        dtpPedido.Value = pedido.OrderDate.Value;
                        dtpHoraPedido.Value = pedido.OrderDate.Value;
                        dtpPedido.Checked = true;
                        dtpHoraPedido.Enabled = true;
                    }
                    else
                    {
                        dtpPedido.Value = dtpPedido.MinDate;
                        dtpPedido.Checked = false;
                        dtpHoraPedido.Value = dtpHoraPedido.MinDate;
                        dtpHoraPedido.Enabled = false;
                    }
                    if (pedido.RequiredDate.HasValue)
                    {
                        dtpRequerido.Value = pedido.RequiredDate.Value;
                        dtpHoraRequerido.Value = pedido.RequiredDate.Value;
                        dtpRequerido.Checked = true;
                        dtpHoraRequerido.Enabled = true;
                    }
                    else
                    {
                        dtpRequerido.Value = dtpRequerido.MinDate;
                        dtpRequerido.Checked = false;
                        dtpHoraRequerido.Value = dtpHoraRequerido.MinDate;
                        dtpHoraRequerido.Enabled = false;
                    }
                    if (pedido.ShippedDate.HasValue)
                    {
                        dtpEnvio.Value = pedido.ShippedDate.Value;
                        dtpHoraEnvio.Value = pedido.ShippedDate.Value;
                        dtpEnvio.Checked = true;
                        dtpHoraEnvio.Enabled = true;
                    }
                    else
                    {
                        dtpEnvio.Value = dtpEnvio.MinDate;
                        dtpEnvio.Checked = false;
                        dtpHoraEnvio.Value = dtpHoraEnvio.MinDate;
                        dtpHoraEnvio.Enabled = false;
                    }
                    txtId.Tag = pedido.RowVersion;
                    MDIPrincipal.ActualizarBarraDeEstado($"Se muestran {dgvPedidos.RowCount} registros en pedidos");
                }
                else
                    Utils.MensajeExclamation("No se encontró el pedido especificado, es posible que el registro haya sido eliminado por otro usuario de la red");
                MDIPrincipal.ActualizarBarraDeEstado($"Se muestran {dgvPedidos.RowCount} registros en pedidos");
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
                IdDetalle = 1;
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
                            dgvDetalle.Rows.Add(new object[]
                            {
                                IdDetalle,
                                pedidoDetalle.ProductName,
                                pedidoDetalle.UnitPrice,
                                pedidoDetalle.Quantity,
                                pedidoDetalle.Discount,
                                totalLinea,
                                "Modificar",
                                "Eliminar",
                                pedidoDetalle.ProductID,
                                null,
                                pedidoDetalle.RowVersion
                            });
                            ++IdDetalle;
                        }
                    }
                }
                CalcularTotal();
                MDIPrincipal.ActualizarBarraDeEstado($"Se muestran {dgvPedidos.RowCount} registros en pedidos");
            }
            catch (Exception ex)
            {
                Utils.MsgCatchOue(ex);
            }
        }

        private void tabcOperacion_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (!PedidoGenerado & (lastSelectedTab == tabpRegistrar && e.TabPage != tabpRegistrar && dgvDetalle.RowCount > 0))
                if (Utils.MensajeQuestion("Se han agregado productos al detalle del pedido, si cambia de pestaña se perderan los datos no guardados.\n¿Desea cambiar de pestaña?") == DialogResult.No)
                    e.Cancel = true;
        }

        private void btnGenerar_Click(object sender, EventArgs e)
        {
            int numRegs = 0;
            BorrarMensajesError();
            if (tabcOperacion.SelectedTab == tabpRegistrar)
            {
                try
                {
                    if (ValidarControles())
                    {
                        MDIPrincipal.ActualizarBarraDeEstado(Utils.insertandoRegistro);
                        DeshabilitarControles();
                        DeshabilitarControlesProducto();
                        List<PedidoDetalle> lstDetalle = new List<PedidoDetalle>();
                        // llenado de elementos hijos
                        foreach (DataGridViewRow dgvr in dgvDetalle.Rows)
                        {
                            PedidoDetalle detalle = new PedidoDetalle();
                            detalle.ProductID = int.Parse(dgvr.Cells["ProductoId"].Value.ToString());
                            detalle.ProductName = dgvr.Cells["Producto"].Value.ToString();
                            detalle.UnitPrice = decimal.Parse(dgvr.Cells["Precio"].Value.ToString());
                            detalle.Quantity = short.Parse(dgvr.Cells["Cantidad"].Value.ToString());
                            detalle.Discount = decimal.Parse(dgvr.Cells["Descuento"].Value.ToString());
                            lstDetalle.Add(detalle);
                        }
                        Pedido pedido = new Pedido();
                        pedido.CustomerID = cboCliente.SelectedValue.ToString();
                        pedido.EmployeeID = int.Parse(cboEmpleado.SelectedValue.ToString());
                        if (dtpPedido != null && dtpHoraPedido != null)
                            pedido.OrderDate = Utils.ObtenerFechaHora(dtpPedido, dtpHoraPedido);
                        if (dtpRequerido != null && dtpHoraRequerido != null)
                            pedido.RequiredDate = Utils.ObtenerFechaHora(dtpRequerido, dtpHoraRequerido);
                        if (dtpEnvio != null && dtpHoraEnvio != null)
                            pedido.ShippedDate = Utils.ObtenerFechaHora(dtpEnvio, dtpHoraEnvio);
                        pedido.ShipVia = int.Parse(cboTransportista.SelectedValue.ToString());
                        pedido.ShipName = txtDirigidoa.Text;
                        pedido.ShipAddress = txtDomicilio.Text;
                        pedido.ShipCity = txtCiudad.Text;
                        pedido.ShipRegion = txtRegion.Text;
                        pedido.ShipPostalCode = txtCP.Text;
                        pedido.ShipCountry = txtPais.Text;
                        if (txtFlete.Text.Contains("$")) txtFlete.Text = txtFlete.Text.Replace("$", "");
                        pedido.Freight = decimal.Parse(txtFlete.Text);
                        int orderId = 0;
                        numRegs = new PedidoRepository(cnStr).Insertar(pedido, lstDetalle, out orderId);
                        txtId.Text = orderId.ToString();
                        txtId.Tag = 1;
                        if (numRegs > 0) Utils.MensajeInformation($"El pedido con Id: {txtId.Text} del Cliente: {cboCliente.Text}, se registró satisfactoriamente");
                        else Utils.MensajeExclamation("No se pudo realizar el registro, es posible que el pedido ya exista");
                    }
                }
                catch (Exception ex)
                {
                    Utils.MsgCatchOue(ex);
                }
                if (numRegs > 0)
                {
                    PedidoGenerado = true;
                    IdDetalle = 1;
                    btnNota.Enabled = true;
                    btnNota.Visible = true;
                    btnNuevo.Enabled = true;
                    btnNuevo.Visible = true;
                    cboCategoria.SelectedIndex = 0;
                    cboCategoria.Enabled = true;
                    BorrarDatosBusqueda();
                    LlenarDgvPedidos(null);
                    dgvDetalle.Rows.Clear();
                    LlenarDatosDetallePedido();
                }
            }
            else if (tabcOperacion.SelectedTab == tabpModificar)
            {
                try
                {
                    if (ValidarControles())
                    {
                        if (!chkRowVersion())
                        {
                            Utils.MensajeExclamation("El registro ha sido modificado por otro usuario de la red, no se realizará la actualización del registro, vuelva a cargar el registro para que se muestre el pedido con los datos proporcionados por el otro usuario");
                            return;
                        }
                        MDIPrincipal.ActualizarBarraDeEstado(Utils.modificandoRegistro);
                        DeshabilitarControles();
                        DeshabilitarControlesProducto();
                        Pedido pedido = new Pedido();
                        pedido.OrderID = int.Parse(txtId.Text);
                        pedido.CustomerID = cboCliente.SelectedValue.ToString();
                        pedido.EmployeeID = int.Parse(cboEmpleado.SelectedValue.ToString());
                        if (!dtpPedido.Checked) pedido.OrderDate = null;
                        else pedido.OrderDate = Convert.ToDateTime(dtpPedido.Value.ToShortDateString() + " " + dtpHoraPedido.Value.ToLongTimeString());
                        if (!dtpRequerido.Checked) pedido.RequiredDate = null;
                        else pedido.RequiredDate = Convert.ToDateTime(dtpRequerido.Value.ToShortDateString() + " " + dtpHoraRequerido.Value.ToLongTimeString());
                        if (!dtpEnvio.Checked) pedido.ShippedDate = null;
                        else pedido.ShippedDate = Convert.ToDateTime(dtpEnvio.Value.ToShortDateString() + " " + dtpHoraEnvio.Value.ToLongTimeString());
                        pedido.ShipVia = int.Parse(cboTransportista.SelectedValue.ToString());
                        pedido.ShipName = txtDirigidoa.Text;
                        pedido.ShipAddress = txtDomicilio.Text;
                        pedido.ShipCity = txtCiudad.Text;
                        pedido.ShipRegion = txtRegion.Text;
                        pedido.ShipPostalCode = txtCP.Text;
                        pedido.ShipCountry = txtPais.Text;
                        if (txtFlete.Text.Contains("$")) txtFlete.Text = txtFlete.Text.Replace("$", "");
                        pedido.Freight = decimal.Parse(txtFlete.Text);
                        numRegs = new PedidoRepository(cnStr).Actualizar(pedido, out int rowVersion);
                        if (rowVersion > 0)
                            txtId.Tag = rowVersion;
                        if (numRegs > 0)
                            Utils.MensajeInformation($"El pedido con Id: {pedido.OrderID} del Cliente: {cboCliente.Text}, se actualizó satisfactoriamente");
                        else
                            Utils.MensajeError("No se pudo realizar la modificación, es posible que el registro se haya eliminado previamente por otro usuario de la red");
                        if (numRegs > 0)
                        {
                            PedidoGenerado = true;
                            btnNota.Enabled = true;
                            btnNota.Visible = true;
                            btnNuevo.Visible = false;
                            cboCategoria.Enabled = true;
                            btnAgregar.Enabled = true;
                            LlenarDgvPedidos(null);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Utils.MsgCatchOue(ex);
                }
            }
            else if (tabcOperacion.SelectedTab == tabpEliminar)
            {
                if (txtId.Text == "")
                {
                    Utils.MensajeExclamation("Seleccione el pedido a eliminar");
                    return;
                }
                if (Utils.MensajeQuestion($"¿Esta seguro de eliminar el pedido con Id: {txtId.Text} del Cliente: {cboCliente.Text}?") == DialogResult.Yes)
                {
                    if (!chkRowVersion())
                    {
                        Utils.MensajeExclamation("El registro ha sido modificado por otro usuario de la red, no se realizará la eliminación del registro, vuelva a cargar el registro para que se muestre el pedido con los datos proporcionados por el otro usuario");
                        return;
                    }
                    MDIPrincipal.ActualizarBarraDeEstado(Utils.eliminandoRegistro);
                    btnGenerar.Enabled = false;
                    try
                    {
                        Pedido pedido = new Pedido();
                        pedido.OrderID = int.Parse(txtId.Text);
                        //PedidosDB pedidosDB = new PedidosDB();
                        //numRegs = pedidosDB.Delete(pedido);
                        numRegs = new PedidoRepository(cnStr).Eliminar(pedido);
                        if (numRegs > 0)
                            Utils.MensajeInformation($"El pedido con Id: {pedido.OrderID} del Cliente: {cboCliente.Text}, se eliminó satisfactoriamente");
                        else
                            Utils.MensajeError("No se pudo realizar la eliminación, es posible que el registro haya sido eliminado previamente por otro usuario de la red");
                    }
                    catch (Exception ex)
                    {
                        Utils.MsgCatchOue(ex);
                    }
                    if (numRegs > 0)
                    {
                        BorrarDatosBusqueda();
                        LlenarDgvPedidos(null);
                    }
                }
                else
                {
                    BorrarDatosPedido();
                    btnGenerar.Enabled = false;
                }
            }
        }

        private void dgvDetalle_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || (e.ColumnIndex != dgvDetalle.Columns["Eliminar"].Index & e.ColumnIndex != dgvDetalle.Columns["Modificar"].Index))
                return;
            if (!PedidoGenerado & e.ColumnIndex == dgvDetalle.Columns["Eliminar"].Index & tabcOperacion.SelectedTab == tabpRegistrar)
            {
                dgvDetalle.Rows.RemoveAt(e.RowIndex);
                CalcularTotal();
            }
            if (!PedidoGenerado & e.ColumnIndex == dgvDetalle.Columns["Modificar"].Index & tabcOperacion.SelectedTab == tabpRegistrar)
            {
                DataGridViewRow dgvr = dgvDetalle.CurrentRow;
                using (FrmPedidosDetalleModificar2 frmPedidosDetalleModificar2 = new FrmPedidosDetalleModificar2())
                {
                    frmPedidosDetalleModificar2.Owner = this;
                    frmPedidosDetalleModificar2.ProductoId = int.Parse(dgvr.Cells["ProductoId"].Value.ToString());
                    frmPedidosDetalleModificar2.Producto = dgvr.Cells["Producto"].Value.ToString();
                    frmPedidosDetalleModificar2.Precio = float.Parse(dgvr.Cells["Precio"].Value.ToString());
                    frmPedidosDetalleModificar2.Cantidad = short.Parse(dgvr.Cells["Cantidad"].Value.ToString());
                    frmPedidosDetalleModificar2.Descuento = float.Parse(dgvr.Cells["Descuento"].Value.ToString());
                    frmPedidosDetalleModificar2.Importe = float.Parse(dgvr.Cells["Importe"].Value.ToString());
                    frmPedidosDetalleModificar2.UInventario = short.Parse(dgvr.Cells["UInventario"].Value.ToString());
                    DialogResult dialogResult = frmPedidosDetalleModificar2.ShowDialog();
                    if (dialogResult == DialogResult.OK)
                    {
                        // Actualiza los valores en la fila actual del DataGridView
                        dgvr.Cells["Cantidad"].Value = frmPedidosDetalleModificar2.Cantidad;
                        dgvr.Cells["Descuento"].Value = frmPedidosDetalleModificar2.Descuento;
                        dgvr.Cells["Importe"].Value = frmPedidosDetalleModificar2.Importe;

                        CalcularTotal();
                    }
                }
            }
            if ((e.ColumnIndex == dgvDetalle.Columns["Eliminar"].Index & tabcOperacion.SelectedTab == tabpModificar) | (PedidoGenerado & e.ColumnIndex == dgvDetalle.Columns["Eliminar"].Index & tabcOperacion.SelectedTab == tabpRegistrar))
            {
                if (!chkRowVersion())
                {
                    Utils.MensajeExclamation("El registro ha sido modificado por otro usuario de la red, vuelva a cargar el registro para que se actualice con los datos proporcionados por el otro usuario");
                    return;
                }
                DataGridViewRow dgvr = dgvDetalle.CurrentRow;
                PedidoDetalle pedidoDetalle = new PedidoDetalle();
                pedidoDetalle.OrderID = int.Parse(txtId.Text);
                pedidoDetalle.ProductID = int.Parse(dgvr.Cells["ProductoId"].Value.ToString());
                pedidoDetalle.ProductName = dgvr.Cells["Producto"].Value.ToString();
                EliminarProducto(pedidoDetalle);
            }
            if ((e.ColumnIndex == dgvDetalle.Columns["Modificar"].Index & tabcOperacion.SelectedTab == tabpModificar) | (PedidoGenerado & e.ColumnIndex == dgvDetalle.Columns["Modificar"].Index & tabcOperacion.SelectedTab == tabpRegistrar))
            {
                if (!chkRowVersion())
                {
                    Utils.MensajeExclamation("El registro ha sido modificado por otro usuario de la red, vuelva a cargar el registro para que se actualice con los datos proporcionados por el otro usuario");
                    return;
                }
                DataGridViewRow dgvr = dgvDetalle.CurrentRow;
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
                    frmPedidosDetalleModificar.RowVersion = int.Parse(dgvr.Cells["RowVersion"].Value.ToString());
                    DialogResult dialogResult = frmPedidosDetalleModificar.ShowDialog();
                    if (dialogResult == DialogResult.OK)
                    {
                        btnNota.Enabled = true;
                        btnNota.Visible = true;
                        BorrarDatosDetallePedido();
                        LlenarDatosDetallePedido();
                    }
                }
            }
        }

        private void EliminarProducto(PedidoDetalle pedidoDetalle)
        {
            int numRegs = 0;
            BorrarMensajesError();
            cboCategoria.SelectedIndex = 0;
            cboProducto.DataSource = null;
            InicializarValoresProducto();
            try
            {
                if (Utils.MensajeQuestion($"¿Esta seguro de eliminar el producto: {pedidoDetalle.ProductName} del pedido: {pedidoDetalle.OrderID}?") == DialogResult.Yes)
                {
                    MDIPrincipal.ActualizarBarraDeEstado(Utils.eliminandoRegistro);
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
                cboCategoria.Enabled = true;
                btnNota.Enabled = true;
                btnNota.Visible = true;
                MDIPrincipal.ActualizarBarraDeEstado($"Se muestran {dgvPedidos.RowCount} registros de pedidos");
            }
        }

        private void BorrarDatosDetallePedido()
        {
            cboCategoria.SelectedIndex = 0;
            cboProducto.DataSource = null;
            txtPrecio.Text = "$0.00";
            txtCantidad.Text = txtUInventario.Text = "0";
            txtDescuento.Text = "0.00";
            txtTotal.Text = "$0.00";
            dgvDetalle.Rows.Clear();
        }

        private void BorrarDatosAgregarProducto()
        {
            cboCategoria.SelectedIndex = 0;
            cboProducto.DataSource = null;
            txtPrecio.Text = "$0.00";
            txtCantidad.Text = txtUInventario.Text = "0";
            txtDescuento.Text = "0.00";
        }

        private void btnNota_Click(object sender, EventArgs e)
        {
            if (!chkRowVersion())
            {
                MessageBox.Show("El registro ha sido modificado por otro usuario de la red, se mostrará la nota de remisión con los datos proporcionados por el otro usuario", Utils.nwtr, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            FrmRptNotaRemision8 frmRptNotaRemision8 = new FrmRptNotaRemision8();
            frmRptNotaRemision8.Id = int.Parse(txtId.Text);
            frmRptNotaRemision8.ShowDialog();
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            BorrarDatosPedido();
            HabilitarControles();
            btnNota.Enabled = false;
            btnNota.Visible = true;
            btnNuevo.Enabled = false;
            btnNuevo.Visible = true;
            PedidoGenerado = false;
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
                foreach (DataGridViewRow dgvr in dgvDetalle.Rows)
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
                    foreach (DataGridViewRow dgvr in dgvDetalle.Rows)
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
                    if (dgvDetalle.Rows.Count > 0)
                        return false;
                }
                MDIPrincipal.ActualizarBarraDeEstado($"Se muestran {dgvPedidos.RowCount} registros en pedidos");
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
