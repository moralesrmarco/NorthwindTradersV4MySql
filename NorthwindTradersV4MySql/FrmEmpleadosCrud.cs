using MySql.Data.MySqlClient;
using System;
using System.Data;
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
            //btnCargar.Enabled = true;  // no se debe habilitar este control para los registros 1 al nueve
        }

        void LlenarCboPais()
        {
            MDIPrincipal.ActualizarBarraDeEstado(Utils.clbdd);
            try
            {
                const string query = "SELECT '' As Id, '»--- Seleccione ---«' As Pais UNION ALL SELECT DISTINCT Country As Id, Country As Pais FROM Employees ORDER BY Pais";
                var dt = new DataTable();
                using (var cn = new MySqlConnection(cnStr))
                using (var cmd = new MySqlCommand(query, cn))
                using (var da = new MySqlDataAdapter(cmd))
                    da.Fill(dt);
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
            MDIPrincipal.ActualizarBarraDeEstado(Utils.clbdd);
            try
            {
                const string query = "SELECT -1 As Id, '»--- Seleccione ---«' As Nombre, '000' As Orden UNION ALL SELECT 0 As Id, '' As Nombre, '111' As Orden UNION ALL SELECT EmployeeID As Id, CONCAT(LastName, ', ', FirstName) As Nombre, Concat(LastName, ', ', FirstName) As Orden FROM Employees Order by Orden";
                var dt = new DataTable();
                using (var cn = new MySqlConnection(cnStr))
                using (var cmd = new MySqlCommand(query, cn))
                using (var da = new MySqlDataAdapter(cmd))
                    da.Fill(dt);
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
                string query;
                if (sender == null)
                {
                    query = @"
                            SELECT
                                e.EmployeeID   AS Id,
                                e.FirstName    AS Nombres,
                                e.LastName     AS Apellidos,
                                e.Title        AS `Título`,
                                e.BirthDate    AS `Fecha de nacimiento`,
                                e.City         AS Ciudad,
                                e.Country      AS País,
                                e.Photo        AS Foto,
                                CONCAT(e2.LastName, ', ', e2.FirstName) AS `Reporta a`
                            FROM Employees AS e
                            LEFT JOIN Employees AS e2
                                ON e.ReportsTo = e2.EmployeeID
                            ORDER BY Id DESC
                            LIMIT 20;
                            ";
                }
                else
                {
                    query = @"
                            SELECT
                              e.EmployeeID AS Id,
                              e.FirstName AS Nombres,
                              e.LastName AS Apellidos,
                              e.Title AS `Título`,
                              e.BirthDate AS `Fecha de nacimiento`,
                              e.City AS Ciudad,
                              e.Country AS País,
                              e.Photo AS Foto,
                              CONCAT(e2.LastName, ', ', e2.FirstName) AS `Reporta a`
                            FROM Employees AS e
                            LEFT JOIN Employees AS e2
                              ON e.ReportsTo = e2.EmployeeID
                            WHERE
                              (@IdIni = 0 OR e.EmployeeID BETWEEN @IdIni AND @IdFin)
                              AND(@Nombres = '' OR e.FirstName  LIKE CONCAT('%', @Nombres, '%'))
                              AND(@Apellidos = '' OR e.LastName   LIKE CONCAT('%', @Apellidos, '%'))
                              AND(@Titulo = '' OR e.Title      LIKE CONCAT('%', @Titulo, '%'))
                              AND(@Domicilio = '' OR e.Address    LIKE CONCAT('%', @Domicilio, '%'))
                              AND(@Ciudad = '' OR e.City       LIKE CONCAT('%', @Ciudad, '%'))
                              AND(@Region = '' OR e.Region     LIKE CONCAT('%', @Region, '%'))
                              AND(@CodigoP = '' OR e.PostalCode LIKE CONCAT('%', @CodigoP, '%'))
                              AND(@Pais = '' OR e.Country    LIKE CONCAT('%', @Pais, '%'))
                              AND(@Telefono = '' OR e.HomePhone  LIKE CONCAT('%', @Telefono, '%'))
                            ORDER BY Id DESC;
                            ";
                }
                var dt = new DataTable();
                using (var cn = new MySqlConnection(cnStr))
                {
                    using (var cmd = new MySqlCommand(query, cn))
                    {
                        if (sender != null)
                        {
                            cmd.Parameters.AddWithValue("@IdIni", string.IsNullOrEmpty(txtBIdIni.Text) ? 0 : Convert.ToInt32(txtBIdIni.Text));
                            cmd.Parameters.AddWithValue("@IdFin", string.IsNullOrEmpty(txtBIdFin.Text) ? 0 : Convert.ToInt32(txtBIdFin.Text));
                            cmd.Parameters.AddWithValue("@Nombres", txtBNombres.Text.Trim());
                            cmd.Parameters.AddWithValue("@Apellidos", txtBApellidos.Text.Trim());
                            cmd.Parameters.AddWithValue("@Titulo", txtBTitulo.Text.Trim());
                            cmd.Parameters.AddWithValue("@Domicilio", txtBDomicilio.Text.Trim());
                            cmd.Parameters.AddWithValue("@Ciudad", txtBCiudad.Text.Trim());
                            cmd.Parameters.AddWithValue("@Region", txtBRegion.Text.Trim());
                            cmd.Parameters.AddWithValue("@CodigoP", txtBCodigoP.Text.Trim());
                            cmd.Parameters.AddWithValue("@Pais", cboBPais.SelectedValue.ToString());
                            cmd.Parameters.AddWithValue("@Telefono", txtBTelefono.Text.Trim());
                        }
                        using (var da = new MySqlDataAdapter(cmd))
                            da.Fill(dt);
                    }
                }
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
            if (txtNombres.Text == "")
            {
                valida = false;
                errorProvider1.SetError(txtNombres, "Ingrese el nombre");
            }
            if (txtApellidos.Text == "")
            {
                valida = false;
                errorProvider1.SetError(txtApellidos, "Ingrese el apellido");
            }
            if (txtTitulo.Text == "")
            {
                valida = false;
                errorProvider1.SetError(txtTitulo, "Ingrese el título");
            }
            if (txtTitCortesia.Text == "")
            {
                valida = false;
                errorProvider1.SetError(txtTitCortesia, "Ingrese el título de cortesia");
            }
            if (txtDomicilio.Text == "")
            {
                valida = false;
                errorProvider1.SetError(txtDomicilio, "Ingrese el domicilio");
            }
            if (txtCiudad.Text == "")
            {
                valida = false;
                errorProvider1.SetError(txtCiudad, "Ingrese la ciudad");
            }
            if (txtPais.Text == "")
            {
                valida = false;
                errorProvider1.SetError(txtPais, "Ingrese el país");
            }
            if (txtTelefono.Text == "")
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
            if (cboReportaA.SelectedValue.ToString() == "-1")
            {
                valida = false;
                errorProvider1.SetError(cboReportaA, "Seleccione a quien reporta el empleado");
            }
            return valida;
        }

        private void FrmEmpleadosCrud_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (tabcOperacion.SelectedTab != tbpListar)
                if (txtId.Text != "" || txtNombres.Text != "" || txtApellidos.Text != "" || txtTitulo.Text != "" || txtTitCortesia.Text != "" || txtDomicilio.Text != "" || txtCiudad.Text != "" || txtRegion.Text != "" || txtCodigoP.Text != "" || txtPais.Text != "" || txtTelefono.Text != "" || txtExtension.Text != "" || dtpFNacimiento.Value != dtpFNacimiento.MinDate || dtpFContratacion.Value != dtpFContratacion.MinDate || txtNotas.Text.Trim() != "" || cboReportaA.SelectedIndex > 0)
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
                try
                {
                    MySqlCommand cmd = new MySqlCommand(@"
                                                        SELECT
                                                            e.EmployeeID                  AS Id,
                                                            e.FirstName                   AS Nombres,
                                                            e.LastName                    AS Apellidos,
                                                            e.Title                       AS Título,
                                                            e.TitleOfCourtesy             AS `Título de cortesía`,
                                                            e.BirthDate                   AS `Fecha de nacimiento`,
                                                            e.HireDate                    AS `Fecha de contratación`,
                                                            e.Address                     AS Domicilio,
                                                            e.City                        AS Ciudad,
                                                            e.Region                      AS Región,
                                                            e.PostalCode                  AS `Código postal`,
                                                            e.Country                     AS País,
                                                            e.HomePhone                   AS Teléfono,
                                                            e.Extension                   AS Extensión,
                                                            e.Photo                       AS Foto,
                                                            e.Notes                       AS Notas,
                                                            e.ReportsTo                   AS Reportaa,
                                                            CONCAT(e1.LastName, ', ', e1.FirstName) AS `Reporta a`,
                                                            e.RowVersion
                                                        FROM Employees AS e
                                                        LEFT JOIN Employees AS e1
                                                            ON e.ReportsTo = e1.EmployeeID
                                                        WHERE e.EmployeeID = @Id;
                                                        ", cn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("Id", txtId.Text);
                    if (cn.State != ConnectionState.Open) cn.Open();
                    using (MySqlDataReader rdr = cmd.ExecuteReader())
                    {
                        if (rdr.Read())
                        {
                            txtId.Tag = rdr["RowVersion"];
                            txtNombres.Text = rdr["Nombres"].ToString();
                            txtApellidos.Text = rdr["Apellidos"].ToString();
                            txtTitulo.Text = rdr["Título"].ToString();
                            txtTitCortesia.Text = rdr["Título de cortesía"].ToString();
                            txtDomicilio.Text = rdr["Domicilio"].ToString();
                            txtCiudad.Text = rdr["Ciudad"].ToString();
                            txtRegion.Text = rdr["Región"].ToString();
                            txtCodigoP.Text = rdr["Código postal"].ToString();
                            txtPais.Text = rdr["País"].ToString();
                            txtTelefono.Text = rdr["Teléfono"].ToString();
                            txtExtension.Text = rdr["Extensión"].ToString();
                            if (rdr["Fecha de nacimiento"] != DBNull.Value)
                                dtpFNacimiento.Value = DateTime.Parse(rdr["Fecha de nacimiento"].ToString());
                            else
                                dtpFNacimiento.Value = dtpFNacimiento.MinDate;
                            if (rdr["Fecha de contratación"] != DBNull.Value)
                                dtpFContratacion.Value = DateTime.Parse(rdr["Fecha de contratación"].ToString());
                            else
                                dtpFContratacion.Value = dtpFContratacion.MinDate;
                            if (rdr["Foto"] != DBNull.Value)
                            {
                                byte[] foto = (byte[])rdr["Foto"];
                                MemoryStream ms;
                                if (int.Parse(txtId.Text) <= 8)
                                {
                                    ms = new MemoryStream(foto, 78, foto.Length - 78);
                                    btnCargar.Enabled = false; // no se permite modificar porque desconozco el formato de la imagen.
                                }
                                else
                                {
                                    ms = new MemoryStream(foto);
                                    btnCargar.Enabled = true;
                                }
                                picFoto.Image = Image.FromStream(ms);
                            }
                            else
                                picFoto.Image = null;
                            txtNotas.Text = rdr["Notas"].ToString();
                            if (rdr["Reportaa"] != DBNull.Value)
                                cboReportaA.SelectedValue = rdr["Reportaa"].ToString();
                            else
                                cboReportaA.SelectedValue = 0;
                        }
                        else
                        {
                            MessageBox.Show($"No se encontró el empleado con Id: {txtId.Text}, es posible que otro usuario lo haya eliminado previamente", Utils.nwtr, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            if (cn.State == ConnectionState.Open) cn.Close();
                            ActualizaDgv();
                            return;
                        }
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
                finally
                {
                    if (cn.State == ConnectionState.Open) cn.Close();
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
            else if (tabcOperacion .SelectedTab == tbpRegistrar)
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
                            MessageBox.Show($"El empleado con Id: {txtId.Text} y Nombre: {txtNombres.Text} {txtApellidos.Text} se registró satisfactoriamente", Utils.nwtr, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show($"El empleado con Nombre: {txtNombres.Text} {txtApellidos.Text} NO fue registrado en la base de datos", Utils.nwtr, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
        }
    }
}
