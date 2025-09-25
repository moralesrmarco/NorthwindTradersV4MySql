using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NorthwindTradersV4MySql
{
    public partial class FrmProveedoresCrud : Form
    {
        string cnStr = System.Configuration.ConfigurationManager.ConnectionStrings["NorthwindMySql"].ConnectionString;
        bool EventoCargado = true;
        private readonly ProveedorRepository repo;

        public FrmProveedoresCrud()
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
            repo = new ProveedorRepository(cnStr);
        }

        private void GrbPaint(object sender, PaintEventArgs e) => Utils.GrbPaint(this, sender, e);

        private void FrmProveedoresCrud_FormClosed(object sender, FormClosedEventArgs e) => MDIPrincipal.ActualizarBarraDeEstado();

        private void FrmProveedoresCrud_Load(object sender, EventArgs e)
        {
            DeshabilitarControles();
            LlenarCboPais();
            LlenarDgv(null);
        }

        private void DeshabilitarControles()
        {
            txtCompañia.ReadOnly = txtContacto.ReadOnly = txtTitulo.ReadOnly = true;
            txtDomicilio.ReadOnly = txtCiudad.ReadOnly = txtRegion.ReadOnly = txtCodigoP.ReadOnly = true;
            txtPais.ReadOnly = txtTelefono.ReadOnly = txtFax.ReadOnly = true;
        }

        private void HabilitarControles()
        {
            txtCompañia.ReadOnly = txtContacto.ReadOnly = txtTitulo.ReadOnly = false;
            txtDomicilio.ReadOnly = txtCiudad.ReadOnly = txtRegion.ReadOnly = txtCodigoP.ReadOnly = false;
            txtPais.ReadOnly = txtTelefono.ReadOnly = txtFax.ReadOnly = false;
        }

        void LlenarCboPais()
        {
            try
            {
                MDIPrincipal.ActualizarBarraDeEstado(Utils.clbdd);
                var dt = repo.ObtenerPaisesProveedores();
                MDIPrincipal.ActualizarBarraDeEstado();
                cboBPais.DataSource = dt;
                cboBPais.ValueMember = "Id";
                cboBPais.DisplayMember = "Pais";
            }
            catch (MySqlException ex)
            {
                Utils.MsgCatchOueclbdd(ex);
            }
            catch (Exception ex)
            {
                Utils.MsgCatchOue(ex);
            }
        }

        void LlenarDgv(object sender)
        {
            try
            {
                MDIPrincipal.ActualizarBarraDeEstado(Utils.clbdd);
                DtoProveedoresBuscar dtoProveedoresBuscar = new DtoProveedoresBuscar
                {
                    IdIni = string.IsNullOrEmpty(txtBIdIni.Text) ? 0 : int.Parse(txtBIdIni.Text),
                    IdFin = string.IsNullOrEmpty(txtBIdFin.Text) ? 0 : int.Parse(txtBIdFin.Text),
                    CompanyName = txtBCompañia.Text.Trim(),
                    ContactName = txtBContacto.Text.Trim(),
                    Address = txtBDomicilio.Text.Trim(),
                    City = txtBCiudad.Text.Trim(),
                    Region = txtBRegion.Text.Trim(),
                    PostalCode = txtBCodigoP.Text.Trim(),
                    Country = cboBPais.SelectedValue.ToString(),
                    Phone = txtBTelefono.Text.Trim(),
                    Fax = txtBFax.Text.Trim()
                };
                var dt = repo.ObtenerProveedores(sender, dtoProveedoresBuscar);
                Dgv.DataSource = dt;
                Utils.ConfDgv(Dgv);
                ConfDgv();
                if (sender == null)
                    MDIPrincipal.ActualizarBarraDeEstado($"Se muestran los últimos {Dgv.RowCount} proveedores registrados");
                else
                    MDIPrincipal.ActualizarBarraDeEstado($"Se encontraron {Dgv.RowCount} registros");
            }
            catch (MySqlException ex)
            {
                Utils.MsgCatchOueclbdd(ex);
            }
            catch (Exception ex)
            {
                Utils.MsgCatchOue(ex);
            }
        }

        private void ConfDgv()
        {
            Dgv.Columns["SupplierID"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            Dgv.Columns["ContactTitle"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            Dgv.Columns["City"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            Dgv.Columns["Region"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            Dgv.Columns["PostalCode"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            Dgv.Columns["Country"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            Dgv.Columns["Phone"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            Dgv.Columns["Fax"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

            Dgv.Columns["City"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            Dgv.Columns["Region"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            Dgv.Columns["PostalCode"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            Dgv.Columns["Country"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            Dgv.Columns["SupplierID"].HeaderText = "Id";
            Dgv.Columns["CompanyName"].HeaderText = "Nombre de compañía";
            Dgv.Columns["ContactName"].HeaderText = "Nombre de contacto";
            Dgv.Columns["ContactTitle"].HeaderText = "Título de contacto";
            Dgv.Columns["Address"].HeaderText = "Domicilio";
            Dgv.Columns["City"].HeaderText = "Ciudad";
            Dgv.Columns["Region"].HeaderText = "Región";
            Dgv.Columns["PostalCode"].HeaderText = "Código postal";
            Dgv.Columns["Country"].HeaderText = "País";
            Dgv.Columns["Phone"].HeaderText = "Teléfono";
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            BorrarDatosProveedor();
            BorrarMensajesError();
            if (tabcOperacion.SelectedTab != tbpRegistrar)
                DeshabilitarControles();
            LlenarDgv(sender);
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            BorrarMensajesError();
            BorrarDatosBusqueda();
            BorrarDatosProveedor();
            if (tabcOperacion.SelectedTab != tbpRegistrar)
                DeshabilitarControles();
            LlenarDgv(null);
        }

        void BorrarMensajesError() => errorProvider1.Clear();

        private void BorrarDatosBusqueda()
        {
            txtBIdIni.Text = txtBIdFin.Text = "";
            txtBCompañia.Text = txtContacto.Text = txtDomicilio.Text = txtCiudad.Text = txtRegion.Text = txtCodigoP.Text = "";
            cboBPais.SelectedIndex = 0;
            txtBTelefono.Text = txtBFax.Text = "";
        }

        private void BorrarDatosProveedor()
        {
            txtId.Text = txtCompañia.Text = txtContacto.Text = txtTitulo.Text = "";
            txtDomicilio.Text = txtCiudad.Text = txtRegion.Text = txtCodigoP.Text = "";
            txtPais.Text = txtTelefono.Text = txtFax.Text = "";
        }

        void txtBIdIni_KeyPress(object sender, KeyPressEventArgs e)
        {
            Utils.ValidarDigitosSinPunto(sender, e);
        }

        void txtBIdFin_KeyPress(object sender, KeyPressEventArgs e)
        {
            Utils.ValidarDigitosSinPunto(sender, e);
        }

        void txtBIdIni_Leave(object sender, EventArgs e)
        {
            Utils.ValidaTxtBIdIni(txtBIdIni, txtBIdFin);
        }

        void txtBIdFin_Leave(object sender, EventArgs e)
        {
            Utils.ValidaTxtBIdFin(txtBIdIni, txtBIdFin);
        }

        private bool ValidarControles()
        {
            bool valida = true;
            if (txtCompañia.Text.Trim() == "")
            {
                valida = false;
                errorProvider1.SetError(txtCompañia, "Ingrese el nombre de la compañía");
            }
            if (txtContacto.Text.Trim() == "")
            {
                valida = false;
                errorProvider1.SetError(txtContacto, "Ingrese el nombre de contacto");
            }
            if (txtTitulo.Text.Trim() == "")
            {
                valida = false;
                errorProvider1.SetError(txtTitulo, "Ingrese el título del contacto");
            }
            if (txtDomicilio.Text.Trim() == "")
            {
                valida = false;
                errorProvider1.SetError(txtDomicilio, "Ingrese el domicilio");
            }
            if (txtCiudad.Text.Trim() == "")
            {
                valida = false;
                errorProvider1.SetError(txtCiudad, "Ingrese la ciudad");
            }
            if (txtPais.Text.Trim() == "")
            {
                valida = false;
                errorProvider1.SetError(txtPais, "Ingrese el país");
            }
            if (txtTelefono.Text.Trim() == "")
            {
                valida = false;
                errorProvider1.SetError(txtTelefono, "Ingrese el teléfono");
            }
            return valida;
        }

        private void FrmProveedoresCrud_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (tabcOperacion.SelectedTab != tbpConsultar)
            {
                if (txtId.Text.Trim() != "" || txtCompañia.Text.Trim() != "" || txtContacto.Text.Trim() != "" || txtTitulo.Text.Trim() != "" || txtDomicilio.Text.Trim() != "" || txtCiudad.Text.Trim() != "" || txtRegion.Text.Trim() != "" || txtCodigoP.Text.Trim() != "" || txtPais.Text.Trim() != "" || txtTelefono.Text.Trim() != "" || txtFax.Text.Trim() != "")
                {
                    if (Utils.MensajeCerrarForm() == DialogResult.No)
                        e.Cancel = true;
                }
            }
        }

        private void Dgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (tabcOperacion.SelectedTab != tbpRegistrar)
            {
                DeshabilitarControles();
                DataGridViewRow dgvr = Dgv.CurrentRow;
                txtId.Text = dgvr.Cells["SupplierID"].Value.ToString();
                Proveedor proveedor = new Proveedor();
                proveedor.SupplierID = Convert.ToInt32(txtId.Text);
                try
                {
                    proveedor = repo.ObtenerProveedor(proveedor);
                    if (proveedor != null) 
                    { 
                        txtId.Tag = proveedor.RowVersion;
                        txtCompañia.Text = proveedor.CompanyName;
                        txtContacto.Text = proveedor.ContactName;
                        txtTitulo.Text = proveedor.ContactTitle;
                        txtDomicilio.Text = proveedor.Address;
                        txtCiudad.Text = proveedor.City;
                        txtRegion.Text = proveedor.Region;
                        txtCodigoP.Text = proveedor.PostalCode;
                        txtPais.Text = proveedor.Country;
                        txtTelefono.Text = proveedor.Phone;
                        txtFax.Text = proveedor.Fax;
                    }
                    else
                    {
                        Utils.MensajeError($"No se encontró el proveedor con Id: {txtId.Text}, es posible que otro usuario lo haya eliminado previamente");
                        ActualizaDgv();
                        return;
                    }
                }
                catch (MySqlException ex)
                {
                    Utils.MsgCatchOueclbdd(ex);
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

        void ActualizaDgv() => btnLimpiar.PerformClick();

        private void tabcOperacion_Selected(object sender, TabControlEventArgs e)
        {
            BorrarMensajesError();
            BorrarDatosProveedor();
            if (tabcOperacion.SelectedTab == tbpRegistrar)
            {
                if (EventoCargado)
                {
                    Dgv.CellClick -= new DataGridViewCellEventHandler(Dgv_CellClick);
                    EventoCargado = false;
                }
                BorrarDatosBusqueda();
                HabilitarControles();
                btnOperacion.Text = "Registrar proveedor";
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
                    btnOperacion.Visible = false;
                else if (tabcOperacion.SelectedTab == tbpModificar)
                {
                    btnOperacion.Text = "Modificar proveedor";
                    btnOperacion.Visible = true;
                }
                else if (tabcOperacion.SelectedTab == tbpEliminar)
                {
                    btnOperacion.Text = "Eliminar proveedor";
                    btnOperacion.Visible = true;
                }
            }
        }

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
                        var proveedor = new Proveedor
                        {
                            CompanyName = txtCompañia.Text.Trim(),
                            ContactName = txtContacto.Text.Trim(),
                            ContactTitle = txtTitulo.Text.Trim(),
                            Address = txtDomicilio.Text.Trim(),
                            City = txtCiudad.Text.Trim(),
                            Region = txtRegion.Text.Trim(),
                            PostalCode = txtCodigoP.Text.Trim(),
                            Country = txtPais.Text.Trim(),
                            Phone = txtTelefono.Text.Trim(),
                            Fax = txtFax.Text.Trim()
                        };
                        int numRegs = repo.Insertar(proveedor);
                        if (numRegs > 0)
                        {
                            txtId.Text = proveedor.SupplierID.ToString();
                            Utils.MensajeInformation($"El proveedor con Id: {txtId.Text} y Nombre de Compañía: {txtCompañia.Text} se registró satisfactoriamente");
                        }
                        else
                            Utils.MensajeError($"El proveedor con Nombre de Compañía: {txtCompañia.Text} NO fue registrado en la base de datos");
                    }
                    catch (MySqlException ex)
                    {
                        Utils.MsgCatchOueclbdd(ex);
                    }
                    catch (Exception ex)
                    {
                        Utils.MsgCatchOue(ex);
                    }
                    LlenarCboPais();
                    HabilitarControles();
                    btnOperacion.Enabled = true;
                    ActualizaDgv();
                }
            }
            else if (tabcOperacion.SelectedTab == tbpModificar)
            {
                if (txtId.Text == "")
                {
                    Utils.MensajeExclamation("Seleccione el proveedor a modificar");
                    return;
                }
                if (ValidarControles())
                {
                    MDIPrincipal.ActualizarBarraDeEstado(Utils.modificandoRegistro);
                    DeshabilitarControles();
                    btnOperacion.Enabled = false;
                    try
                    {
                        var proveedor = new Proveedor
                        {
                            SupplierID = int.Parse(txtId.Text),
                            CompanyName = txtCompañia.Text.Trim(),
                            ContactName = txtContacto.Text.Trim(),
                            ContactTitle = txtTitulo.Text.Trim(),
                            Address = txtDomicilio.Text.Trim(),
                            City = txtCiudad.Text.Trim(),
                            Region = txtRegion.Text.Trim(),
                            PostalCode = txtCodigoP.Text.Trim(),
                            Country = txtPais.Text.Trim(),
                            Phone = txtTelefono.Text.Trim(),
                            Fax = txtFax.Text.Trim(),
                            RowVersion = (int)txtId.Tag
                        };
                        int numRegs = repo.Actualizar(proveedor);
                        if (numRegs > 0)
                        {
                            Utils.MensajeInformation($"El proveedor con Id: {txtId.Text} y Nombre de Compañía: {txtCompañia.Text} se modificó satisfactoriamente");
                        }
                        else
                        {
                            Utils.MensajeError($"El proveedor con Id: {txtId.Text} y Nombre de Compañía: {txtCompañia.Text} NO fue modificado en la base de datos, es posible que otro usuario lo haya modificado o eliminado previamente");
                            ActualizaDgv();
                            return;
                        }
                    }
                    catch (MySqlException ex)
                    {
                        Utils.MsgCatchOueclbdd(ex);
                    }
                    catch (Exception ex)
                    {
                        Utils.MsgCatchOue(ex);
                    }
                    LlenarCboPais();
                    ActualizaDgv();
                }
            }
            else if (tabcOperacion.SelectedTab == tbpEliminar)
            {
                if (txtId.Text == "")
                {
                    Utils.MensajeExclamation("Seleccione el proveedor a eliminar");
                    return;
                }
                if (Utils.MensajeQuestion($"¿Está seguro de eliminar el proveedor con Id: {txtId.Text} y Nombre de compañía: {txtCompañia.Text}?") == DialogResult.Yes)
                {
                    MDIPrincipal.ActualizarBarraDeEstado(Utils.eliminandoRegistro);
                    btnOperacion.Enabled = false;
                    try
                    {
                        var proveedor = new Proveedor();
                        proveedor.SupplierID = Convert.ToInt32(txtId.Text);
                        proveedor.RowVersion = (int)txtId.Tag;
                        int numRegs = repo.Eliminar(proveedor);
                        if (numRegs > 0)
                            Utils.MensajeInformation($"El proveedor con Id: {txtId.Text} y Nombre de compañía: {txtCompañia.Text} se eliminó satisfactoriamente");
                        else
                            Utils.MensajeError($"El proveedor con Id: {txtId.Text} y Nombre de Compañía: {txtCompañia.Text} NO se eliminó en la base de datos, es posible que otro usuario lo haya modificado o eliminado previamente");
                    }
                    catch (MySqlException ex)
                    {
                        Utils.MsgCatchOueclbdd(ex);
                    }
                    catch (Exception ex)
                    {
                        Utils.MsgCatchOue(ex);
                    }
                    LlenarCboPais();
                    ActualizaDgv();
                }
            }
        }
    }
}
