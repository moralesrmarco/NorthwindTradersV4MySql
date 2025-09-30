using System;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Windows.Forms;

namespace NorthwindTradersV4MySql
{
    public partial class FrmProductosCrud : Form
    {

        string cnStr = ConfigurationManager.ConnectionStrings["NorthwindMySql"].ConnectionString;
        private readonly ProductoRepository repo;
        bool EventoCargado = true; // esta variable es necesaria para controlar el manejador de eventos de la celda del dgv ojo no quitar

        public FrmProductosCrud()
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
            repo = new ProductoRepository(cnStr);
        }

        private void GrbPaint(object sender, PaintEventArgs e) => Utils.GrbPaint(this, sender, e);

        private void FrmProductosCrud_FormClosed(object sender, FormClosedEventArgs e) => MDIPrincipal.ActualizarBarraDeEstado();

        private void FrmProductosCrud_Load(object sender, EventArgs e)
        {
            DeshabilitarControles();
            LlenarCboCategoria();
            LlenarCboProveedor();
            LlenarDgv(null);
        }

        private void DeshabilitarControles()
        {
            txtProducto.ReadOnly = txtCantidadxU.ReadOnly = txtPrecio.ReadOnly = true;
            txtUInventario.ReadOnly = txtUPedido.ReadOnly = txtPPedido.ReadOnly = true;
            chkbDescontinuado.Enabled = false;
            cboCategoria.Enabled = cboProveedor.Enabled = false;
        }

        private void HabilitarControles()
        {
            txtProducto.ReadOnly = txtCantidadxU.ReadOnly = txtPrecio.ReadOnly = false;
            txtUInventario.ReadOnly = txtUPedido.ReadOnly = txtPPedido.ReadOnly = false;
            chkbDescontinuado.Enabled = true;
            cboCategoria.Enabled = cboProveedor.Enabled = true;
        }

        private void LlenarCboCategoria()
        {
            try
            {
                MDIPrincipal.ActualizarBarraDeEstado(Utils.clbdd);
                var dtCboCategoria = repo.ObtenerComboCategorias();
                var dtBCboCategoria = dtCboCategoria.Copy();
                cboCategoria.DataSource = dtCboCategoria;
                cboCategoria.DisplayMember = "CategoryName";
                cboCategoria.ValueMember = "CategoryID";
                cboBCategoria.DataSource = dtBCboCategoria;
                cboBCategoria.DisplayMember = "CategoryName";
                cboBCategoria.ValueMember = "CategoryID";
                MDIPrincipal.ActualizarBarraDeEstado();
            }
            catch (Exception ex)
            {
                Utils.MsgCatchOue(ex);
            }
        }

        private void LlenarCboProveedor()
        {
            try
            {
                MDIPrincipal.ActualizarBarraDeEstado(Utils.clbdd);
                var dtCboProveedor = repo.ObtenerComboProveedores();
                var dtBCboProveedor = dtCboProveedor.Copy();
                cboProveedor.DataSource = dtCboProveedor;
                cboProveedor.DisplayMember = "CompanyName";
                cboProveedor.ValueMember = "SupplierID";
                cboBProveedor.DataSource = dtBCboProveedor;
                cboBProveedor.DisplayMember = "CompanyName";
                cboBProveedor.ValueMember = "SupplierID";
                MDIPrincipal.ActualizarBarraDeEstado();
            }
            catch (Exception ex)
            {
                Utils.MsgCatchOue(ex);
            }
        }
        
        private void LlenarDgv(object sender)
        {
            try
            {
                MDIPrincipal.ActualizarBarraDeEstado(Utils.clbdd);
                DtoProductosBuscar dtoProductosBuscar = new DtoProductosBuscar
                {
                    IdIni = string.IsNullOrEmpty(txtBIdIni.Text) ? 0 : int.Parse(txtBIdIni.Text),
                    IdFin = string.IsNullOrEmpty(txtBIdFin.Text) ? 0 : int.Parse(txtBIdFin.Text),
                    Producto = txtBProducto.Text.Trim(),
                    Categoria = cboBCategoria.SelectedValue == null ? 0 : Convert.ToInt32(cboBCategoria.SelectedValue),
                    Proveedor = cboBProveedor.SelectedValue == null ? 0 : Convert.ToInt32(cboBProveedor.SelectedValue)
                };
                DataTable dt;
                if (sender == null)
                    dtoProductosBuscar = null;
                dt = repo.ObtenerProductos(dtoProductosBuscar);
                Dgv.DataSource = dt;
                Utils.ConfDgv(Dgv);
                ConfDgv();
                if (sender == null)
                    MDIPrincipal.ActualizarBarraDeEstado($"Se muestran los últimos {Dgv.RowCount} productos registrados");
                else
                    MDIPrincipal.ActualizarBarraDeEstado($"Se encontraron {Dgv.RowCount} registros");
            }
            catch (Exception ex)
            {
                Utils.MsgCatchOue(ex);
            }
        }

        private void ConfDgv()
        {
            Dgv.Columns["CategoryId"].Visible = false;
            Dgv.Columns["SupplierId"].Visible = false;

            Dgv.Columns["ProductId"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            Dgv.Columns["ProductName"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            Dgv.Columns["QuantityPerUnit"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            Dgv.Columns["UnitPrice"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            Dgv.Columns["UnitsInStock"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            Dgv.Columns["UnitsOnOrder"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            Dgv.Columns["ReorderLevel"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            Dgv.Columns["Discontinued"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            Dgv.Columns["CategoryName"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            Dgv.Columns["CompanyName"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

            Dgv.Columns["UnitPrice"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            Dgv.Columns["UnitsInStock"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            Dgv.Columns["UnitsOnOrder"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            Dgv.Columns["ReorderLevel"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            Dgv.Columns["UnitPrice"].DefaultCellStyle.Format = "c";
            Dgv.Columns["UnitsInStock"].DefaultCellStyle.Format = "N0";
            Dgv.Columns["UnitsOnOrder"].DefaultCellStyle.Format = "N0";
            Dgv.Columns["ReorderLevel"].DefaultCellStyle.Format = "N0";

            Dgv.Columns["ProductId"].HeaderText = "Id";
            Dgv.Columns["ProductName"].HeaderText = "Producto";
            Dgv.Columns["QuantityPerUnit"].HeaderText = "Cantidad por unidad";
            Dgv.Columns["UnitPrice"].HeaderText = "Precio";
            Dgv.Columns["UnitsInStock"].HeaderText = "Unidades en inventario";
            Dgv.Columns["UnitsOnOrder"].HeaderText = "Unidades en pedido";
            Dgv.Columns["ReorderLevel"].HeaderText = "Nivel de reorden";
            Dgv.Columns["Discontinued"].HeaderText = "Descontinuado";
            Dgv.Columns["CategoryName"].HeaderText = "Categoría";
            Dgv.Columns["Description"].HeaderText = "Descripción de categoría";
            Dgv.Columns["CompanyName"].HeaderText = "Proveedor";
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            BorrarDatosProducto();
            BorrarMensajesError();
            if (tabcOperacion.SelectedTab != tbpRegistrar)
                DeshabilitarControles();
            LlenarDgv(sender);
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            BorrarDatosProducto();
            BorrarMensajesError();
            BorrarDatosBusqueda();
            if (tabcOperacion.SelectedTab != tbpRegistrar)
                DeshabilitarControles();
            LlenarDgv(null);
        }

        private void BorrarDatosProducto()
        {
            txtId.Text = txtProducto.Text = txtCantidadxU.Text = txtPrecio.Text = "";
            txtUInventario.Text = txtUPedido.Text = txtPPedido.Text = "";
            chkbDescontinuado.Checked = false;
            cboCategoria.SelectedIndex = cboProveedor.SelectedIndex = 0;
        }

        private void BorrarMensajesError() => errorProvider1.Clear();

        private void BorrarDatosBusqueda()
        {
            txtBIdIni.Text = txtBIdFin.Text = txtBProducto.Text = "";
            cboBCategoria.SelectedIndex = cboBProveedor.SelectedIndex = 0;
        }

        private void txtBIdIni_KeyPress(object sender, KeyPressEventArgs e) => Utils.ValidarDigitosSinPunto(sender, e);

        private void txtBIdFin_KeyPress(object sender, KeyPressEventArgs e) => Utils.ValidarDigitosSinPunto(sender, e);

        private void txtBIdIni_Leave(object sender, EventArgs e) => Utils.ValidaTxtBIdIni(txtBIdIni, txtBIdFin);

        private void txtBIdFin_Leave(object sender, EventArgs e) => Utils.ValidaTxtBIdFin(txtBIdIni, txtBIdFin);

        private bool ValidarControles()
        {
            bool valida = true;
            if (cboCategoria.SelectedIndex == 0 || cboCategoria.SelectedIndex == -1)
            {
                valida = false;
                errorProvider1.SetError(cboCategoria, "Seleccione una categoría");
            }
            if (cboProveedor.SelectedIndex == 0 || cboProveedor.SelectedIndex == -1)
            {
                valida = false;
                errorProvider1.SetError(cboProveedor, "Seleccione un proveedor");
            }
            if (txtProducto.Text.Trim() == "")
            {
                valida = false;
                errorProvider1.SetError(txtProducto, "Ingrese producto");
            }
            if (txtPrecio.Text.Trim() == "")
            {
                valida = false;
                errorProvider1.SetError(txtPrecio, "Ingrese precio");
            }
            if (txtUInventario.Text.Trim() == "")
            {
                valida = false;
                errorProvider1.SetError(txtUInventario, "Ingrese unidades en inventario");
            }
            if (txtUPedido.Text.Trim() == "")
            {
                valida = false;
                errorProvider1.SetError(txtUPedido, "Ingrese unidades en pedido");
            }
            if (txtPPedido.Text.Trim() == "")
            {
                valida = false;
                errorProvider1.SetError(txtPPedido, "Ingrese punto de pedido");
            }
            return valida;
        }

        private void FrmProductosCrud_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (tabcOperacion.SelectedTab != tbpConsultar)
            {
                if (cboCategoria.SelectedIndex != 0 || cboProveedor.SelectedIndex != 0 || txtId.Text.Trim() != "" || txtProducto.Text.Trim() != "" || txtCantidadxU.Text.Trim() != "" || txtPrecio.Text.Trim() != "" || txtUInventario.Text.Trim() != "" || txtUPedido.Text.Trim() != "" || txtPPedido.Text.Trim() != "")
                    if (Utils.MensajeCerrarForm() == DialogResult.No)
                        e.Cancel = true;
            }
        }

        private void Dgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (tabcOperacion.SelectedTab != tbpRegistrar)
            {
                DeshabilitarControles();
                DataGridViewRow dgvr = Dgv.CurrentRow;
                txtId.Text = dgvr.Cells["ProductId"].Value.ToString();
                Producto producto = new Producto();
                producto.ProductID = int.Parse(txtId.Text);
                try
                {
                    producto = repo.ObtenerProducto(producto);
                    if (producto != null)
                    {
                        txtId.Tag = producto.RowVersion;
                        cboCategoria.SelectedValue = producto.CategoryID ?? 0;
                        cboProveedor.SelectedValue = producto.SupplierID ?? 0;
                        txtProducto.Text = producto.ProductName;
                        txtCantidadxU.Text = producto.QuantityPerUnit ?? "";
                        txtPrecio.Text = producto.UnitPrice.ToString("F2");
                        txtUInventario.Text = producto.UnitsInStock.ToString();
                        txtUPedido.Text = producto.UnitsOnOrder.ToString();
                        txtPPedido.Text = producto.ReorderLevel.ToString();
                        chkbDescontinuado.Checked = producto.Discontinued;
                    }
                    else
                    {
                        Utils.MensajeError($"No se encontró el producto con Id: {txtId.Text}, es posible que otro usuario lo haya eliminado previamente");
                        ActualizaDgv();
                        return;
                    }
                }
                catch (Exception ex)
                {
                    Utils.MsgCatchOue(ex);
                }
                if (tabcOperacion.SelectedTab == tbpModificar)
                {
                    HabilitarControles();
                    btnOperacion.Enabled = true;
                }
                else if (tabcOperacion.SelectedTab == tbpEliminar)
                    btnOperacion.Enabled = true;
            }
        }

        private void tabcOperacion_Selected(object sender, TabControlEventArgs e)
        {
            BorrarDatosProducto();
            BorrarMensajesError();
            if (tabcOperacion.SelectedTab == tbpRegistrar)
            {
                if (EventoCargado)
                {
                    Dgv.CellClick -= new DataGridViewCellEventHandler(Dgv_CellClick);
                    EventoCargado = false;
                }
                BorrarDatosBusqueda();
                HabilitarControles();
                btnOperacion.Text = "Registrar producto";
                btnOperacion.Visible = true;
                btnOperacion.Enabled = true;
            }
            else
            {
                if (!EventoCargado)
                {
                    Dgv.CellClick += new DataGridViewCellEventHandler(Dgv_CellClick);
                    EventoCargado = true;
                }
                DeshabilitarControles();
                btnOperacion.Enabled = false;
                if (tabcOperacion.SelectedTab == tbpConsultar)
                {
                    btnOperacion.Visible = false;
                    btnOperacion.Enabled = false;
                }
                else if (tabcOperacion.SelectedTab == tbpModificar)
                {
                    btnOperacion.Text = "Modificar producto";
                    btnOperacion.Visible = true;
                    btnOperacion.Enabled = false;
                }
                else if (tabcOperacion.SelectedTab == tbpEliminar)
                {
                    btnOperacion.Text = "Eliminar producto";
                    btnOperacion.Visible = true;
                    btnOperacion.Enabled = false;
                }
            }
        }

        private void txtUInventario_Validating(object sender, CancelEventArgs e)
        {
            if (txtUInventario.Text.Trim() != "")
            {
                if (int.Parse(txtUInventario.Text) > 32767)
                {
                    errorProvider1.SetError(txtUInventario, "La cantidad no puede ser mayor a 32767");
                    e.Cancel = true;
                }
                else
                    errorProvider1.SetError(txtUInventario, "");
            }
        }

        private void txtUPedido_Validating(object sender, CancelEventArgs e)
        {
            if (txtUPedido.Text.Trim() != "")
            {
                if (int.Parse(txtUPedido.Text) > 32767)
                {
                    errorProvider1.SetError(txtUPedido, "La cantidad no puede ser mayor a 32767");
                    e.Cancel = true;
                }
                else
                    errorProvider1.SetError(txtUPedido, "");
            }
        }

        private void txtPPedido_Validating(object sender, CancelEventArgs e)
        {
            if (txtPPedido.Text.Trim() != "")
            {
                if (int.Parse(txtPPedido.Text) > 32767)
                {
                    errorProvider1.SetError(txtPPedido, "La cantidad no puede ser mayor a 32767");
                    e.Cancel = true;
                }
                else
                    errorProvider1.SetError(txtPPedido, "");
            }
        }

        private void txtPrecio_KeyPress(object sender, KeyPressEventArgs e) => Utils.ValidarDigitosConPunto(sender, e);

        private void txtUInventario_KeyPress(object sender, KeyPressEventArgs e) => Utils.ValidarDigitosSinPunto(sender, e);

        private void txtUPedido_KeyPress(object sender, KeyPressEventArgs e) => Utils.ValidarDigitosSinPunto(sender, e);

        private void txtPPedido_KeyPress(object sender, KeyPressEventArgs e) => Utils.ValidarDigitosSinPunto(sender, e);

        private void btnOperacion_Click(object sender, EventArgs e)
        {
            BorrarMensajesError();
            if (tabcOperacion.SelectedTab == tbpRegistrar)
            {
                if (ValidarControles())
                {
                    MDIPrincipal.ActualizarBarraDeEstado(Utils.insertandoRegistro);
                    DeshabilitarControles();
                    btnOperacion.Enabled = false;
                    try
                    {
                        var producto = new Producto
                        {
                            CategoryID = Convert.ToInt32(cboCategoria.SelectedValue),
                            SupplierID = Convert.ToInt32(cboProveedor.SelectedValue),
                            ProductName = txtProducto.Text,
                            QuantityPerUnit = string.IsNullOrEmpty(txtCantidadxU.Text) ? null : txtCantidadxU.Text,
                            UnitPrice = decimal.Parse(txtPrecio.Text),
                            UnitsInStock = short.Parse(txtUInventario.Text),
                            UnitsOnOrder = short.Parse(txtUPedido.Text),
                            ReorderLevel = short.Parse(txtPPedido.Text),
                            Discontinued = chkbDescontinuado.Checked
                        };
                        int numRegs = repo.Insertar(producto);
                        if (numRegs > 0)
                        {
                            txtId.Text = producto.ProductID.ToString();
                            Utils.MensajeInformation($"El producto con Id: {txtId.Text} y Nombre de producto: {txtProducto.Text} se registró satisfactoriamente");
                        }
                        else
                             Utils.MensajeError($"El producto con Id: {txtId.Text} y Nombre de producto: {txtProducto.Text} NO fue registrado en la base de datos");
                    }
                    catch (Exception ex)
                    {
                        Utils.MsgCatchOue(ex);
                    }
                    HabilitarControles();
                    btnOperacion.Enabled = true;
                    LlenarCombos();
                    ActualizaDgv();
                }
            }
            else if (tabcOperacion.SelectedTab == tbpModificar)
            {
                if (txtId.Text == "")
                {
                    Utils.MensajeExclamation("Seleccione el producto a modificar");
                    return;
                }
                if (ValidarControles())
                {
                    MDIPrincipal.ActualizarBarraDeEstado(Utils.modificandoRegistro);
                    DeshabilitarControles();
                    btnOperacion.Enabled = false;
                    try
                    {
                        var producto = new Producto
                        {
                            ProductID = int.Parse(txtId.Text),
                            CategoryID = Convert.ToInt32(cboCategoria.SelectedValue),
                            SupplierID = Convert.ToInt32(cboProveedor.SelectedValue),
                            ProductName = txtProducto.Text,
                            QuantityPerUnit = string.IsNullOrEmpty(txtCantidadxU.Text) ? null : txtCantidadxU.Text,
                            UnitPrice = decimal.Parse(txtPrecio.Text),
                            UnitsInStock = short.Parse(txtUInventario.Text),
                            UnitsOnOrder = short.Parse(txtUPedido.Text),
                            ReorderLevel = short.Parse(txtPPedido.Text),
                            Discontinued = chkbDescontinuado.Checked,
                            RowVersion = (int)txtId.Tag
                        };
                        int numRegs = repo.Actualizar(producto);
                        if (numRegs > 0)
                            Utils.MensajeInformation($"El producto con Id: {txtId.Text} y Nombre de producto: {txtProducto.Text} se modificó satisfactoriamente");
                        else
                            Utils.MensajeError($"El producto con Id: {txtId.Text} y Nombre de producto: {txtProducto.Text} NO fue modificado en la base de datos, es posible que otro usuario lo haya modificado o eliminado previamente");
                    }
                    catch (Exception ex)
                    {
                        Utils.MsgCatchOue(ex);
                    }
                    LlenarCombos();
                    ActualizaDgv();
                }
            }
            else if (tabcOperacion.SelectedTab == tbpEliminar)
            {
                if (txtId.Text == "")
                {
                    Utils.MensajeExclamation("Seleccione el producto a eliminar");
                    return;
                }
                if (Utils.MensajeQuestion($"¿Está seguro de eliminar el producto con Id: {txtId.Text} y Nombre de producto: {txtProducto.Text}?") == DialogResult.Yes)
                {
                    MDIPrincipal.ActualizarBarraDeEstado(Utils.eliminandoRegistro);
                    btnOperacion.Enabled = false;
                    try
                    {
                        var producto = new Producto
                        {
                            ProductID = int.Parse(txtId.Text),
                            RowVersion = (int)txtId.Tag
                        };
                        int numRegs = repo.Eliminar(producto);
                        if (numRegs > 0)
                            Utils.MensajeInformation($"El producto con Id: {txtId.Text} y Nombre de producto: {txtProducto.Text} se eliminó satisfactoriamente");
                        else
                            Utils.MensajeExclamation($"El producto con Id: {txtId.Text} y Nombre de producto: {txtProducto.Text} NO se eliminó en la base de datos, es posible que otro usuario de la red lo haya modificado o eliminado previamente");
                    }
                    catch (Exception ex)
                    {
                        Utils.MsgCatchOue(ex);
                    }
                    LlenarCombos();
                    ActualizaDgv();
                }
            }
        }

        private void ActualizaDgv() => btnLimpiar.PerformClick();

        private void LlenarCombos()
        {
            LlenarCboCategoria();
            LlenarCboProveedor();
        }
    }
}
