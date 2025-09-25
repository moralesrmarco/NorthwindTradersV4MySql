using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Windows.Forms;

namespace NorthwindTradersV4MySql
{
    public partial class FrmClientesCrud : Form
    {
        static string cnStr = ConfigurationManager.ConnectionStrings["NorthwindMySql"].ConnectionString;
        bool EventoCargado = true;
        private readonly ClienteRepository clienteRepository;

        public FrmClientesCrud()
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
            clienteRepository = new ClienteRepository(cnStr);
        }

        private void GrbPaint(object sender, PaintEventArgs e) => Utils.GrbPaint(this, sender, e);

        private void FrmClientesCrud_FormClosed(object sender, FormClosedEventArgs e) => MDIPrincipal.ActualizarBarraDeEstado();

        private void FrmClientesCrud_Load(object sender, EventArgs e)
        {
            DeshabilitarControles();
            LlenarCboPais();
            LlenarDgv(null);
        }

        private void DeshabilitarControles()
        {
            txtId.ReadOnly = txtCompañia.ReadOnly = txtContacto.ReadOnly = txtTitulo.ReadOnly = true;
            txtDomicilio.ReadOnly = txtCiudad.ReadOnly = txtRegion.ReadOnly = txtCodigoP.ReadOnly = true;
            txtPais.ReadOnly = txtTelefono.ReadOnly = txtFax.ReadOnly = true;
            btnOperacion.Visible = false;
        }

        private void HabilitarControles()
        {
            txtId.ReadOnly = txtCompañia.ReadOnly = txtContacto.ReadOnly = txtTitulo.ReadOnly = false;
            txtDomicilio.ReadOnly = txtCiudad.ReadOnly = txtRegion.ReadOnly = txtCodigoP.ReadOnly = false;
            txtPais.ReadOnly = txtTelefono.ReadOnly = txtFax.ReadOnly = false;
            btnOperacion.Visible = true;
        }

        void LlenarCboPais()
        {
            try
            {
                MDIPrincipal.ActualizarBarraDeEstado(Utils.clbdd);
                var dt = clienteRepository.ObtenerPaisesClientes();
                MDIPrincipal.ActualizarBarraDeEstado();
                cboBPais.DataSource = dt;
                cboBPais.ValueMember = "Id";
                cboBPais.DisplayMember = "País";
                cboBPais.SelectedIndex = 0;
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
                Cliente clienteBuscar = new Cliente()
                {
                    CustomerID = txtBId.Text,
                    CompanyName = txtBCompañia.Text,
                    ContactName = txtBContacto.Text,
                    Address = txtBDomicilio.Text,
                    City = txtBCiudad.Text,
                    Region = txtBRegion.Text,
                    PostalCode = txtBCodigoP.Text,
                    Country = cboBPais.SelectedValue.ToString(),
                    Phone = txtBTelefono.Text,
                    Fax = txtBFax.Text
                };
                var dt = clienteRepository.ObtenerClientes(sender, clienteBuscar);
                dgv.DataSource = dt;
                Utils.ConfDgv(dgv);
                ConfDgv();
                if (sender == null)
                    MDIPrincipal.ActualizarBarraDeEstado($"Se muestran los primeros {dgv.RowCount} clientes registrados");
                else
                    MDIPrincipal.ActualizarBarraDeEstado($"Se encontraron {dgv.RowCount} registros");
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
            dgv.Columns["Id"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgv.Columns["Código postal"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgv.Columns["País"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgv.Columns["Teléfono"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgv.Columns["Fax"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgv.Columns["Ciudad"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.Columns["Región"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.Columns["Código postal"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.Columns["País"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            BorrarMensajesError();
            BorrarDatosCliente();
            if (tabcOperacion.SelectedTab != tbpRegistrar)
                DeshabilitarControles();
            LlenarDgv(sender);
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            BorrarMensajesError();
            BorrarDatosBusqueda();
            BorrarDatosCliente();
            if (tabcOperacion.SelectedTab != tbpRegistrar)
                DeshabilitarControles();
            LlenarDgv(null);
        }

        void BorrarMensajesError() => errorProvider1.Clear();

        void BorrarDatosCliente()
        {
            txtId.Text = txtCompañia.Text = txtContacto.Text = txtDomicilio.Text = txtCiudad.Text = "";
            txtRegion.Text = txtCodigoP.Text = txtTelefono.Text = txtFax.Text = txtPais.Text = txtTitulo.Text = "";
        }

        void BorrarDatosBusqueda()
        {
            txtBId.Text = txtBCompañia.Text = txtBContacto.Text = txtBDomicilio.Text = txtBCiudad.Text = "";
            txtBRegion.Text = txtBCodigoP.Text = txtBTelefono.Text = txtBFax.Text = "";
            cboBPais.SelectedIndex = 0;
        }

        private bool ValidarControles()
        {
            bool valida = true;
            if (txtId.Text.Trim() == "")
            {
                valida = false;
                errorProvider1.SetError(txtId, "Ingrese el Id del cliente");
            }
            if (txtCompañia.Text.Trim() == "")
            {
                valida = false;
                errorProvider1.SetError(txtCompañia, "Ingrese el nombre de la compañía");
            }
            if (txtContacto.Text.Trim() == "")
            {
                valida = false;
                errorProvider1.SetError(txtContacto, "Ingrese el nombre del contacto");
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

        private void FrmClientesCrud_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (tabcOperacion.SelectedTab != tbpListar)
                if (txtId.Text.Trim() != "" || txtCompañia.Text.Trim() != "" || txtContacto.Text.Trim() != "" || txtTitulo.Text.Trim() != "" || txtDomicilio.Text.Trim() != "" || txtCiudad.Text.Trim() != "" || txtRegion.Text.Trim() != "" || txtCodigoP.Text.Trim() != "" || txtPais.Text.Trim() != "" || txtTelefono.Text.Trim() != "" || txtFax.Text != "")
                {
                    if (Utils.MensajeCerrarForm() == DialogResult.No)
                        e.Cancel = true;
                }
        }

        private void tabcOperacion_Selected(object sender, TabControlEventArgs e)
        {
            BorrarDatosCliente();
            BorrarMensajesError();
            if (tabcOperacion.SelectedTab == tbpRegistrar)
            {
                if (EventoCargado)
                {
                    dgv.CellClick -= new DataGridViewCellEventHandler(dgv_CellClick);
                    EventoCargado = false;
                }
                BorrarDatosBusqueda();
                HabilitarControles();
                txtId.Enabled = true;
                txtId.ReadOnly = false;
                btnOperacion.Text = "Registrar cliente";
                btnOperacion.Enabled = true;
            }
            else
            {
                if (!EventoCargado)
                {
                    dgv.CellClick += new DataGridViewCellEventHandler(dgv_CellClick);
                    EventoCargado = true;
                }
                DeshabilitarControles();
                btnOperacion.Enabled = false;
                if (tabcOperacion.SelectedTab == tbpListar)
                    btnOperacion.Visible = false;
                else if (tabcOperacion.SelectedTab == tbpModificar)
                {
                    btnOperacion.Text = "Modificar cliente";
                    btnOperacion.Visible = true;
                }
                else if (tabcOperacion.SelectedTab == tbpEliminar)
                {
                    btnOperacion.Text = "Eliminar cliente";
                    btnOperacion.Visible = true;
                }
            }
        }

        private void dgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (tabcOperacion.SelectedTab != tbpRegistrar)
            {
                DeshabilitarControles();
                DataGridViewRow dgvr = dgv.CurrentRow;
                txtId.Text = dgvr.Cells["Id"].Value.ToString();
                Cliente cliente = new Cliente();
                cliente.CustomerID = txtId.Text;
                try
                {
                    cliente = clienteRepository.ObtenerCliente(cliente);
                    if (cliente != null)
                    {
                        txtId.Tag = cliente.RowVersion;
                        txtCompañia.Text = cliente.CompanyName;
                        txtContacto.Text = cliente.ContactName;
                        txtTitulo.Text = cliente.ContactTitle;
                        txtDomicilio.Text = cliente.Address;
                        txtCiudad.Text = cliente.City;
                        txtRegion.Text = cliente.Region;
                        txtCodigoP.Text = cliente.PostalCode;
                        txtPais.Text = cliente.Country;
                        txtTelefono.Text = cliente.Phone;
                        txtFax.Text = cliente.Fax;
                    }
                    else
                    {
                        Utils.MensajeError($"No se encontró el cliente con Id: {txtId.Text}, es posible que otro usuario lo haya eliminado previamente");
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
                    txtId.Enabled = false;
                    btnOperacion.Enabled = true;
                }
                else if (tabcOperacion.SelectedTab == tbpEliminar)
                {
                    btnOperacion.Visible = true;
                    btnOperacion.Enabled = true;
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
                        var cliente = new Cliente
                        {
                            CustomerID = txtId.Text.Trim(),
                            CompanyName = txtCompañia.Text.Trim(),
                            ContactName = txtContacto.Text.Trim(),
                            ContactTitle = txtTitulo.Text.Trim(),
                            Address = txtDomicilio.Text.Trim(),
                            City = txtCiudad.Text.Trim(),
                            Region = string.IsNullOrWhiteSpace(txtRegion.Text.Trim()) ? null : txtRegion.Text.Trim(),
                            PostalCode = string.IsNullOrWhiteSpace(txtCodigoP.Text.Trim()) ? null : txtCodigoP.Text.Trim(),
                            Country = txtPais.Text.Trim(),
                            Phone = txtTelefono.Text.Trim(),
                            Fax = string.IsNullOrWhiteSpace(txtFax.Text.Trim()) ? null : txtFax.Text.Trim()
                        };
                        int numRegs = clienteRepository.Insertar(cliente);
                        if (numRegs > 0)
                            Utils.MensajeInformation($"El cliente con Id: {txtId.Text} y Nombre de Compañía: {txtCompañia.Text} se registró satisfactoriamente");
                        else
                            Utils.MensajeError($"El cliente con Id: {txtId.Text} y Nombre de Compañía: {txtCompañia.Text} NO fue registrado en la base de datos");
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
                    Utils.MensajeExclamation("Seleccione el cliente a modificar");
                    return;
                }
                if (ValidarControles())
                {
                    MDIPrincipal.ActualizarBarraDeEstado(Utils.modificandoRegistro);
                    DeshabilitarControles();
                    btnOperacion.Enabled = false;
                    try
                    {
                        var cliente = new Cliente()
                        {
                            CustomerID = txtId.Text,
                            CompanyName = txtCompañia.Text,
                            ContactName = txtContacto.Text,
                            ContactTitle = txtTitulo.Text,
                            Address = txtDomicilio.Text,
                            City = txtCiudad.Text,
                            Region = txtRegion.Text,
                            PostalCode = txtCodigoP.Text,
                            Country = txtPais.Text,
                            Phone = txtTelefono.Text,
                            Fax = txtFax.Text,
                            RowVersion = (int)txtId.Tag
                        };
                        int numRegs = clienteRepository.Actualizar(cliente);
                        if (numRegs > 0)
                            Utils.MensajeInformation($"El cliente con Id: {txtId.Text} y Nombre de Compañía: {txtCompañia.Text} se modificó satisfactoriamente");
                        else
                            Utils.MensajeError($"El cliente con Id: {txtId.Text} y Nombre de Compañía: {txtCompañia.Text} NO fue modificado en la base de datos, es posible que otro usuario lo haya modificado o eliminado previamente");

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
                    Utils.MensajeExclamation("Seleccione el cliente a eliminar");
                    return;
                }
                if (Utils.MensajeQuestion($"¿Esta seguro de eliminar el cliente con Id: {txtId.Text} y Nombre de Compañía: {txtCompañia.Text}?") == DialogResult.Yes)
                {
                    MDIPrincipal.ActualizarBarraDeEstado(Utils.eliminandoRegistro);
                    btnOperacion.Enabled = false;
                    try
                    {
                        var cliente = new Cliente();
                        cliente.CustomerID = txtId.Text;
                        cliente.RowVersion = (int)txtId.Tag;
                        int numRegs = clienteRepository.Eliminar(cliente);
                        if (numRegs > 0)
                            Utils.MensajeInformation($"El cliente con Id: {txtId.Text} y Nombre de Compañía: {txtCompañia.Text} se eliminó satisfactoriamente");
                        else
                            Utils.MensajeError($"El cliente con Id: {txtId.Text} y Nombre de Compañía: {txtCompañia.Text} NO se eliminó en la base de datos, es posible que otro usuario lo haya modificado o eliminado previamente");
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

        void ActualizaDgv() => btnLimpiar.PerformClick();
    }
}
