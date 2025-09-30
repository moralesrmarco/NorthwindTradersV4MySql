using MySql.Data.MySqlClient;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace NorthwindTradersV4MySql
{
    public partial class FrmEmpleadosCrud : Form
    {
        static string cnStr = System.Configuration.ConfigurationManager.ConnectionStrings["NorthwindMySql"].ConnectionString;
        static MySqlConnection cn = new MySqlConnection(cnStr);
        bool EventoCargado = true;
        OpenFileDialog openFileDialog;
        private readonly EmpleadoRepository _empleadoRepository;

        public FrmEmpleadosCrud()
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
            _empleadoRepository = new EmpleadoRepository(cnStr);
        }

        void GrbPaint(object sender, PaintEventArgs e) => Utils.GrbPaint(this, sender, e);

        void FrmEmpleadosCrud_FormClosed(object sender, FormClosedEventArgs e) => MDIPrincipal.ActualizarBarraDeEstado();

        private void FrmEmpleadosCrud_Load(object sender, EventArgs e)
        {
            DeshabilitarControles();
            LlenarCboPais();
            LlenarCboReportaA();
            LlenarDgv(null);
        }

        void DeshabilitarControles()
        {
            txtNombres.ReadOnly = txtApellidos.ReadOnly = txtTitulo.ReadOnly = txtTitCortesia.ReadOnly = true;
            txtDomicilio.ReadOnly = txtCiudad.ReadOnly = txtRegion.ReadOnly = txtCodigoP.ReadOnly = true;
            txtPais.ReadOnly = txtTelefono.ReadOnly = txtExtension.ReadOnly = true;
            dtpFNacimiento.Enabled = dtpFContratacion.Enabled = false;
            txtNotas.ReadOnly = false;
            cboReportaA.Enabled = false;
            picFoto.Enabled = false;
            btnCargar.Enabled = false;
        }

        void HabilitarControles()
        {
            txtNombres.ReadOnly = txtApellidos.ReadOnly = txtTitulo.ReadOnly = false;
            txtTitCortesia.ReadOnly = false;
            txtDomicilio.ReadOnly = txtCiudad.ReadOnly = txtRegion.ReadOnly = txtCodigoP.ReadOnly = false;
            txtPais.ReadOnly = txtTelefono.ReadOnly = txtExtension.ReadOnly = false;
            txtNotas.ReadOnly = false;
            dtpFNacimiento.Enabled = dtpFContratacion.Enabled = cboReportaA.Enabled = true;
            picFoto.Enabled = true;
            //btnCargar.Enabled = true;  // no se debe habilitar este control para los registros 1 al 8
        }

        void LlenarCboPais()
        {
            try
            {
                MDIPrincipal.ActualizarBarraDeEstado(Utils.clbdd);
                var dt = _empleadoRepository.ObtenerPaisesEmpleados();
                MDIPrincipal.ActualizarBarraDeEstado();
                cboBPais.DataSource = dt;
                cboBPais.ValueMember = "Id";
                cboBPais.DisplayMember = "Pais";
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

        void LlenarCboReportaA()
        {
            try
            {
                MDIPrincipal.ActualizarBarraDeEstado(Utils.clbdd);
                var dt = _empleadoRepository.ObtenerReportaAEmpleados();
                MDIPrincipal.ActualizarBarraDeEstado();
                cboReportaA.DataSource = dt;
                cboReportaA.ValueMember = "Id";
                cboReportaA.DisplayMember = "Nombre";
                cboReportaA.SelectedIndex = 0;
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
                DtoEmpleadosBuscar empleadosBuscar = new DtoEmpleadosBuscar
                {
                    IdIni = string.IsNullOrEmpty(txtBIdIni.Text) ? 0 : Convert.ToInt32(txtBIdIni.Text),
                    IdFin = string.IsNullOrEmpty(txtBIdFin.Text) ? 0 : Convert.ToInt32(txtBIdFin.Text),
                    Nombres = txtBNombres.Text.Trim(),
                    Apellidos = txtBApellidos.Text.Trim(),
                    Titulo = txtBTitulo.Text.Trim(),
                    Domicilio = txtBDomicilio.Text.Trim(),
                    Ciudad = txtBCiudad.Text.Trim(),
                    Region = txtBRegion.Text.Trim(),
                    CodigoP = txtBCodigoP.Text.Trim(),
                    Pais = cboBPais.SelectedValue.ToString(),
                    Telefono = txtBTelefono.Text.Trim()
                };
                var dt = _empleadoRepository.ObtenerEmpleados(sender, empleadosBuscar);
                dgv.DataSource = dt;
                Utils.ConfDgv(dgv);
                ConfDgv();
                if (sender == null)
                    MDIPrincipal.ActualizarBarraDeEstado($"Se muestran los últimos {dgv.RowCount} empleados registrados");
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

        void ConfDgv()
        {
            dgv.Columns["Id"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgv.Columns["Foto"].Width = 20;
            dgv.Columns["Foto"].DefaultCellStyle.Padding = new Padding(2, 2, 2, 2);
            ((DataGridViewImageColumn)dgv.Columns["Foto"]).ImageLayout = DataGridViewImageCellLayout.Zoom;
            dgv.Columns["Título"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgv.Columns["Fecha de nacimiento"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgv.Columns["Ciudad"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgv.Columns["País"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgv.Columns["Reporta a"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgv.Columns["Título"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.Columns["Fecha de nacimiento"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.Columns["Ciudad"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.Columns["País"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.Columns["Reporta a"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.Columns["Fecha de nacimiento"].DefaultCellStyle.Format = "dd \" de \"MMM\" de \"yyyy";
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            BorrarDatosEmpleado();
            BorrarMensajesError();
            if (tabcOperacion.SelectedTab != tbpRegistrar)
                DeshabilitarControles();
            LlenarDgv(sender);
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            BorrarMensajesError();
            BorrarDatosBusqueda();
            BorrarDatosEmpleado();
            if (tabcOperacion.SelectedTab != tbpRegistrar)
                DeshabilitarControles();
            LlenarDgv(null);
        }

        void BorrarMensajesError() => errorProvider1.Clear();

        void BorrarDatosBusqueda()
        {
            txtBIdIni.Text = txtBIdFin.Text = txtBNombres.Text = txtBApellidos.Text = string.Empty;
            txtBTitulo.Text = txtBDomicilio.Text = txtBCiudad.Text = string.Empty;
            txtBRegion.Text = txtBCodigoP.Text = txtBTelefono.Text = string.Empty;
            cboBPais.SelectedIndex = 0;
        }

        void BorrarDatosEmpleado()
        {
            txtId.Text = txtNombres.Text = txtApellidos.Text = txtTitulo.Text = string.Empty;
            txtTitCortesia.Text = txtDomicilio.Text = txtCiudad.Text = string.Empty;
            txtRegion.Text = txtCodigoP.Text = txtTelefono.Text = txtPais.Text = string.Empty;
            txtExtension.Text = txtNotas.Text = string.Empty;
            cboReportaA.SelectedIndex = -1;
            picFoto.Image = Properties.Resources.FotoPerfil;
            dtpFNacimiento.Value = dtpFNacimiento.MinDate;
            dtpFContratacion.Value = dtpFContratacion.MinDate;
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
            if (txtNombres.Text.Trim() == "")
            {
                valida = false;
                errorProvider1.SetError(txtNombres, "Ingrese el nombre");
            }
            if (txtApellidos.Text.Trim() == "")
            {
                valida = false;
                errorProvider1.SetError(txtApellidos, "Ingrese el apellido");
            }
            if (txtTitulo.Text.Trim() == "")
            {
                valida = false;
                errorProvider1.SetError(txtTitulo, "Ingrese el título");
            }
            if (txtTitCortesia.Text.Trim() == "")
            {
                valida = false;
                errorProvider1.SetError(txtTitCortesia, "Ingrese el título de cortesia");
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
            if (picFoto.Image == null)
            {
                valida = false;
                errorProvider1.SetError(btnCargar, "Ingrese la foto");
            }
            if (dtpFNacimiento.Value == new DateTime(1753, 1, 1))
            {
                valida = false;
                errorProvider1.SetError(dtpFNacimiento, "Ingrese la fecha de nacimiento");
            }
            if (dtpFContratacion.Value == new DateTime(1753, 1, 1))
            {
                valida = false;
                errorProvider1.SetError(dtpFContratacion, "Ingrese la fecha de contratación");
            }
            if (cboReportaA.SelectedValue == null || cboReportaA.SelectedValue.ToString() == "-1")
            {
                valida = false;
                errorProvider1.SetError(cboReportaA, "Seleccione a quien reporta el empleado");
            }
            return valida;
        }

        private void FrmEmpleadosCrud_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (tabcOperacion.SelectedTab != tbpListar)
                if (txtId.Text.Trim() != "" || txtNombres.Text.Trim() != "" || txtApellidos.Text.Trim() != "" || txtTitulo.Text.Trim() != "" || txtTitCortesia.Text.Trim() != "" || txtDomicilio.Text.Trim() != "" || txtCiudad.Text.Trim() != "" || txtRegion.Text.Trim() != "" || txtCodigoP.Text.Trim() != "" || txtPais.Text.Trim() != "" || txtTelefono.Text.Trim() != "" || txtExtension.Text.Trim() != "" || dtpFNacimiento.Value != dtpFNacimiento.MinDate || dtpFContratacion.Value != dtpFContratacion.MinDate || txtNotas.Text.Trim() != "" || cboReportaA.SelectedIndex > 0)
                {
                    if (Utils.MensajeCerrarForm() == DialogResult.No)
                        e.Cancel = true;
                }

        }

        private void dgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (tabcOperacion.SelectedTab != tbpRegistrar)
            {
                DeshabilitarControles();
                DataGridViewRow dgvr = dgv.CurrentRow;
                txtId.Text = dgvr.Cells["Id"].Value.ToString();
                Empleado empleado = new Empleado();
                empleado.EmployeeID = Convert.ToInt32(txtId.Text);
                try
                {
                    empleado = _empleadoRepository.ObtenerEmpleado(empleado);
                    if (empleado != null)
                    {
                        if (empleado.BirthDate != null)
                            dtpFNacimiento.Value = empleado.BirthDate.Value;
                        else
                            dtpFNacimiento.Value = dtpFNacimiento.MinDate;
                        if (empleado.HireDate != null)
                            dtpFContratacion.Value = empleado.HireDate.Value;
                        else
                            dtpFContratacion.Value = dtpFContratacion.MinDate;
                        if (empleado.Photo != null)
                        {
                            if (empleado.EmployeeID <= 8)
                                btnCargar.Enabled = false;
                            else
                                btnCargar.Enabled = true;
                            using (var ms = new MemoryStream(empleado.Photo))
                                picFoto.Image = Image.FromStream(ms);
                        }
                        else
                            picFoto.Image = null;
                        if (empleado.ReportsTo != null)
                            cboReportaA.SelectedValue = empleado.ReportsTo.Value;
                        else
                            cboReportaA.SelectedValue = 0;
                        txtId.Tag = empleado.RowVersion;
                        txtNombres.Text = empleado.FirstName;
                        txtApellidos.Text = empleado.LastName;
                        txtTitulo.Text = empleado.Title;
                        txtTitCortesia.Text = empleado.TitleOfCourtesy;
                        txtDomicilio.Text = empleado.Address;
                        txtCiudad.Text = empleado.City; 
                        txtRegion.Text = empleado.Region;
                        txtCodigoP.Text = empleado.PostalCode;
                        txtPais.Text = empleado.Country;
                        txtTelefono.Text = empleado.HomePhone;
                        txtExtension.Text = empleado.Extension;
                        txtNotas.Text = empleado.Notes;
                    }
                    else
                    {
                        Utils.MensajeError($"No se encontró el empleado con Id: {txtId.Text}, es posible que otro usuario lo haya eliminado previamente");
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
                if (tabcOperacion.SelectedTab == tbpListar)
                {
                    btnOperacion.Visible = true;
                    btnOperacion.Enabled = true;
                    btnCargar.Visible = false;
                }
                if (tabcOperacion.SelectedTab == tbpModificar)
                {
                    HabilitarControles();
                    btnOperacion.Visible = true;
                    btnOperacion.Enabled = true;
                    btnCargar.Visible = true;
                }
                else if (tabcOperacion.SelectedTab == tbpEliminar)
                {
                    btnOperacion.Enabled = true;
                    btnOperacion.Visible = true;
                    btnCargar.Visible = false;
                }
            }
        }

        void ActualizaDgv() => btnLimpiar.PerformClick();

        private void tabcOperacion_Selected(object sender, TabControlEventArgs e)
        {
            BorrarDatosEmpleado();
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
                btnOperacion.Text = "Registrar empleado";
                btnOperacion.Visible = true;
                btnOperacion.Enabled = true;
                btnCargar.Enabled = true;
                btnCargar.Visible = true;
                cboReportaA.SelectedValue = -1;
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
                btnCargar.Enabled = false;
                if (tabcOperacion.SelectedTab == tbpListar)
                {
                    btnOperacion.Text = "Imprimir empleado";
                    btnOperacion.Visible = true;
                    btnOperacion.Enabled = true;
                    btnCargar.Visible = false;
                    btnCargar.Enabled = false;
                }
                else if (tabcOperacion.SelectedTab == tbpModificar)
                {
                    btnOperacion.Text = "Modificar empleado";
                    btnOperacion.Visible = true;
                    btnOperacion.Enabled = false;
                    btnCargar.Visible = true;
                    btnCargar.Enabled = false;
                }
                else if (tabcOperacion.SelectedTab == tbpEliminar)
                {
                    btnOperacion.Text = "Eliminar empleado";
                    btnOperacion.Visible = true;
                    btnOperacion.Enabled = false;
                    btnCargar.Visible = false;
                    btnCargar.Enabled = false;
                }
            }
        }

        private void btnCargar_Click(object sender, EventArgs e)
        {
            // Mostrar el cuadro de diálogo OpenFileDialog
            //La instrucción siguiente es para que nos muestre todos los tipos juntos
            openFileDialog = new OpenFileDialog();
            //openFileDialog.Filter = "Archivos de imagen (*.jpg, *.jpeg, *.png, *.bmp)|*.jpg;*.jpeg;*.png;*.bmp";
            openFileDialog.InitialDirectory = "c:\\Imágenes\\";
            //La instrucción siguiente es para que nos muestre varias filas en el openfiledialog que nos permita abrir por un tipo especifico
            openFileDialog.Filter = "Archivos jpg (*.jpg)|*.jpg|Archivos jpeg (*.jpeg)|*.jpeg|Archivos png (*.png)|*.png|Archivos bmp (*.bmp)|*.bmp";
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = true;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Cargar la imagen seleccionada en un objeto Image
                Image image = Image.FromFile(openFileDialog.FileName);

                // Mostrar la imagen en un control PictureBox
                picFoto.Image = image;
                errorProvider1.SetError(btnCargar, "");
            }
        }

        private void LlenarCombos()
        {
            LlenarCboPais();
            LlenarCboReportaA();
        }

        private void btnOperacion_Click(object sender, EventArgs e)
        {
            BorrarMensajesError();
            if (tabcOperacion.SelectedTab == tbpListar)
            {
                FrmRptEmpleado frmRptEmpleado = new FrmRptEmpleado();
                frmRptEmpleado.Owner = this;
                frmRptEmpleado.Id = int.Parse(txtId.Text);
                frmRptEmpleado.ShowDialog();
            }
            else if (tabcOperacion.SelectedTab == tbpRegistrar)
            {
                if (ValidarControles())
                {
                    MDIPrincipal.ActualizarBarraDeEstado(Utils.insertandoRegistro);
                    DeshabilitarControles();
                    btnOperacion.Enabled = false;
                    try
                    {
                        var empleado = new Empleado
                        {
                            FirstName = txtNombres.Text.Trim(),
                            LastName = txtApellidos.Text.Trim(),
                            Title = txtTitulo.Text.Trim(),
                            TitleOfCourtesy = txtTitCortesia.Text.Trim(),
                            BirthDate = dtpFNacimiento.Value == dtpFNacimiento.MinDate ? (DateTime?)null : dtpFNacimiento.Value,
                            HireDate = dtpFContratacion.Value == dtpFContratacion.MinDate ? (DateTime?)null : dtpFContratacion.Value,
                            Address = txtDomicilio.Text.Trim(),
                            City = txtCiudad.Text.Trim(),
                            Region = txtRegion.Text.Trim(),
                            PostalCode = txtCodigoP.Text.Trim(),
                            Country = txtPais.Text.Trim(),
                            HomePhone = txtTelefono.Text.Trim(),
                            Extension = txtExtension.Text.Trim(),
                            Notes = txtNotas.Text.Trim(),
                            ReportsTo = cboReportaA.SelectedValue.ToString() == "0" ? (int?)null : Convert.ToInt32(cboReportaA.SelectedValue),
                            Photo = picFoto.Image != null ? Utils.ImageToByteArray(picFoto.Image) : null
                        };
                        int numRegs = _empleadoRepository.Insertar(empleado);
                        if (numRegs > 0)
                        {
                            txtId.Text = empleado.EmployeeID.ToString();
                            Utils.MensajeInformation($"El empleado con Id: {txtId.Text} y Nombre: {txtNombres.Text} {txtApellidos.Text} se registró satisfactoriamente");
                        }
                        else
                        {
                            Utils.MensajeError($"El empleado con Nombre: {txtNombres.Text} {txtApellidos.Text} NO fue registrado en la base de datos");
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
                    HabilitarControles();
                    btnOperacion.Enabled = true;
                    btnCargar.Enabled = true;
                    LlenarCombos();
                    ActualizaDgv();
                }
            }
            else if (tabcOperacion.SelectedTab == tbpModificar)
            {
                if (txtId.Text == "")
                {
                    Utils.MensajeExclamation("Seleccione el empleado a modificar");
                    return;
                }
                if (ValidarControles())
                {
                    MDIPrincipal.ActualizarBarraDeEstado(Utils.modificandoRegistro);
                    DeshabilitarControles();
                    btnOperacion.Enabled = false;
                    try
                    {
                        var empleado = new Empleado
                        {
                            EmployeeID = Convert.ToInt32(txtId.Text),
                            FirstName = txtNombres.Text.Trim(),
                            LastName = txtApellidos.Text.Trim(),
                            Title = txtTitulo.Text.Trim(),
                            TitleOfCourtesy = txtTitCortesia.Text.Trim(),
                            BirthDate = dtpFNacimiento.Value == dtpFNacimiento.MinDate ? (DateTime?)null : dtpFNacimiento.Value,
                            HireDate = dtpFContratacion.Value == dtpFContratacion.MinDate ? (DateTime?)null : dtpFContratacion.Value,
                            Address = txtDomicilio.Text.Trim(),
                            City = txtCiudad.Text.Trim(),
                            Region = txtRegion.Text.Trim(),
                            PostalCode = txtCodigoP.Text.Trim(),
                            Country = txtPais.Text.Trim(),
                            HomePhone = txtTelefono.Text.Trim(),
                            Extension = txtExtension.Text.Trim(),
                            Notes = txtNotas.Text.Trim(),
                            ReportsTo = cboReportaA.SelectedValue.ToString() == "0" ? (int?)null : Convert.ToInt32(cboReportaA.SelectedValue),
                            RowVersion = (int)txtId.Tag
                        };
                        if (picFoto.Image != null && empleado.EmployeeID > 8)
                            empleado.Photo = Utils.ImageToByteArray(picFoto.Image);
                        else
                            empleado.Photo = null;
                        int numRegs = _empleadoRepository.Actualizar(empleado);
                        if (numRegs > 0)
                            Utils.MensajeInformation($"El empleado con Id: {txtId.Text} y Nombre: {txtNombres.Text} {txtApellidos.Text} se modificó satisfactoriamente");
                        else
                            Utils.MensajeError($"El empleado con Id: {txtId.Text} y Nombre: {txtNombres.Text} {txtApellidos.Text} NO fue modificado en la base de datos, es posible que otro usuario lo haya modificado o eliminado previamente");
                    }
                    catch (MySqlException ex)
                    {
                        Utils.MsgCatchOueclbdd(ex);
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
                    Utils.MensajeExclamation("Seleccione el empleado a eliminar");
                    return;
                }
                if (Utils.MensajeQuestion($"¿Está seguro de eliminar el empleado con Id: {txtId.Text} y Nombre: {txtNombres.Text} {txtApellidos.Text}?") == DialogResult.Yes)
                {
                    MDIPrincipal.ActualizarBarraDeEstado(Utils.eliminandoRegistro);
                    btnOperacion.Enabled = false;
                    try
                    {
                        var empleado = new Empleado();
                        empleado.EmployeeID = Convert.ToInt32(txtId.Text);
                        empleado.RowVersion = (int)txtId.Tag;
                        int numRegs = _empleadoRepository.Eliminar(empleado);
                        if (numRegs > 0)
                            Utils.MensajeInformation($"El empleado con Id: {txtId.Text} y Nombre: {txtNombres.Text} {txtApellidos.Text} se eliminó satisfactoriamente");
                        else
                            Utils.MensajeExclamation($"El empleado con Id: {txtId.Text} y Nombre: {txtNombres.Text} {txtApellidos.Text} NO fue eliminado de la base de datos, es posible que otro usuario lo haya modificado o eliminado previamente");
                    }
                    catch (MySqlException ex)
                    {
                        Utils.MsgCatchOueclbdd(ex);
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
    }
}

