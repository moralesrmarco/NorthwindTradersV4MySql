using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace NorthwindTradersV4MySql
{
    public partial class FrmPermisosCrud : Form
    {

        private readonly string cnStr = ConfigurationManager.ConnectionStrings["NorthwindMySql"].ConnectionString;

        public FrmPermisosCrud()
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
        }

        private void GrbPaint(object sender, PaintEventArgs e) => Utils.GrbPaint(this, sender, e);

        private void GrbPaint2(object sender, PaintEventArgs e) => Utils.GrbPaint2(this, sender, e);

        private void FrmPermisosCrud_FormClosed(object sender, FormClosedEventArgs e) => MDIPrincipal.ActualizarBarraDeEstado();

        private void FrmPermisosCrud_Load(object sender, EventArgs e)
        {
            DeshabilitarControles();
            LlenarListBoxCatalogo();
            LlenarDgv(null);
        }

        private void DeshabilitarControles()
        {
            listBoxCatalogo.Enabled = false;
            listBoxConcedidos.Enabled = false;
            listBoxCatalogo.Visible = false;
            listBoxConcedidos.Visible = false;
            txtUsuario.Visible = false;
            txtId.Visible = false;
            txtNombre.Visible = false;
            BtnAgregar.Enabled = BtnQuitar.Enabled = BtnAgregarTodos.Enabled = BtnQuitarTodos.Enabled = false;
        }

        private void HabilitarControles()
        {
            listBoxCatalogo.Enabled = true;
            listBoxConcedidos.Enabled = true;
            listBoxCatalogo.Visible = true;
            listBoxConcedidos.Visible = true;
            txtUsuario.Visible = true;
            txtId.Visible = true;
            txtNombre.Visible = true;
            BtnAgregar.Enabled = BtnQuitar.Enabled = BtnAgregarTodos.Enabled = BtnQuitarTodos.Enabled = true;
        }

        private void LlenarListBoxCatalogo()
        {
            try
            {
                MDIPrincipal.ActualizarBarraDeEstado(Utils.clbdd);
                var dt = new PermisoRepository(cnStr).LlenarListBoxCatalogo();
                listBoxCatalogo.DataSource = dt;
                listBoxCatalogo.DisplayMember = "Descripción";
                listBoxCatalogo.ValueMember = "PermisoId";
                listBoxCatalogo.SelectedIndex = -1;
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
                DtoUsuariosBuscar dtoUsuarioBuscar;
                if (sender == null)
                    dtoUsuarioBuscar = null;
                else
                {
                    dtoUsuarioBuscar = new DtoUsuariosBuscar
                    {
                        IdIni = string.IsNullOrWhiteSpace(txtBIdIni.Text) ? 0 : Convert.ToInt32(txtBIdIni.Text),
                        IdFin = string.IsNullOrWhiteSpace(txtBIdFin.Text) ? 0 : Convert.ToInt32(txtBIdFin.Text),
                        Paterno = string.IsNullOrWhiteSpace(txtBPaterno.Text) ? null : txtBPaterno.Text.Trim(),
                        Materno = string.IsNullOrWhiteSpace(txtBMaterno.Text) ? null : txtBMaterno.Text.Trim(),
                        Nombres = string.IsNullOrWhiteSpace(txtBNombres.Text) ? null : txtBNombres.Text.Trim(),
                        Usuario = string.IsNullOrWhiteSpace(txtBUsuario.Text) ? null : txtBUsuario.Text.Trim()
                    };
                }
                var dt = new PermisoRepository(cnStr).ObtenerUsuarios(dtoUsuarioBuscar);

                // Agrega una nueva columna "EstatusTexto" de tipo string
                dt.Columns.Add("EstatusTexto", typeof(string));

                // Llena la nueva columna con el texto equivalente
                foreach (DataRow row in dt.Rows)
                {
                    bool estatus = Convert.ToBoolean(row["Estatus"]);
                    row["EstatusTexto"] = estatus ? "Activo" : "Inactivo";
                }

                // Opcional: eliminar la columna original si ya no la necesitas
                dt.Columns.Remove("Estatus");

                // Opcional: renombrar la columna nueva para mantener el nombre original
                dt.Columns["EstatusTexto"].ColumnName = "Estatus";

                Dgv.DataSource = dt;
                Utils.ConfDgv(Dgv);
                ConfDgv();
                if (sender == null)
                    MDIPrincipal.ActualizarBarraDeEstado($"Se muestran los últimos {Dgv.RowCount} usuarios registrados");
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
            Dgv.Columns["Id"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            Dgv.Columns["Paterno"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            Dgv.Columns["Materno"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            Dgv.Columns["Nombres"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            Dgv.Columns["Usuario"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            Dgv.Columns["FechaCaptura"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            Dgv.Columns["FechaModificacion"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            Dgv.Columns["Estatus"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

            Dgv.Columns["Usuario"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            Dgv.Columns["FechaCaptura"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            Dgv.Columns["FechaModificacion"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            Dgv.Columns["Estatus"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            Dgv.Columns["Paterno"].HeaderText = "Apellido Paterno";
            Dgv.Columns["Materno"].HeaderText = "Apellido Materno";
            Dgv.Columns["FechaCaptura"].HeaderText = "Fecha de creación";
            Dgv.Columns["FechaModificacion"].HeaderText = "Fecha de modificación";

            Dgv.Columns["FechaCaptura"].DefaultCellStyle.Format = "dd/MMMM/yyyy\nhh:mm:ss tt";
            Dgv.Columns["FechaModificacion"].DefaultCellStyle.Format = "dd/MMMM/yyyy\nhh:mm:ss tt";
        }

        private void txtBIdIni_KeyPress(object sender, KeyPressEventArgs e) => Utils.ValidarDigitosSinPunto(sender, e);

        private void txtBIdFin_KeyPress(object sender, KeyPressEventArgs e) => Utils.ValidarDigitosSinPunto(sender, e);

        private void txtBIdIni_Leave(object sender, EventArgs e) => Utils.ValidaTxtBIdIni(txtBIdIni, txtBIdFin);

        private void txtBIdFin_Leave(object sender, EventArgs e) => Utils.ValidaTxtBIdFin(txtBIdIni, txtBIdFin);

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            BorrarDatosPermisos();
            BorrarDatosBusqueda();
            DeshabilitarControles();
            LlenarDgv(null);
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            BorrarDatosPermisos();
            DeshabilitarControles();
            LlenarDgv(sender);
        }

        private void BorrarDatosPermisos()
        {
            listBoxConcedidos.DataSource = null;
            listBoxConcedidos.Items.Clear();
        }

        private void BorrarDatosBusqueda()
        {
            txtBIdIni.Text = string.Empty;
            txtBIdFin.Text = string.Empty;
            txtBPaterno.Text = string.Empty;
            txtBMaterno.Text = string.Empty;
            txtBNombres.Text = string.Empty;
            txtBUsuario.Text = string.Empty;
        }

        private void Dgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            HabilitarControles();
            if (e.RowIndex >= 0 && e.RowIndex < Dgv.RowCount)
            {
                DataGridViewRow row = Dgv.Rows[e.RowIndex];
                txtId.Text = row.Cells["Id"].Value.ToString();
                txtUsuario.Text = row.Cells["Usuario"].Value.ToString();
                txtNombre.Text = $"{row.Cells["Paterno"].Value} {row.Cells["Materno"].Value} {row.Cells["Nombres"].Value}";
                LlenarListBoxConcedidos();
            }
        }

        private void LlenarListBoxConcedidos()
        {
            try
            {
                MDIPrincipal.ActualizarBarraDeEstado(Utils.clbdd);
                var dt = new PermisoRepository(cnStr).LlenarListBoxConcedidos(Convert.ToInt32(txtId.Text));
                listBoxConcedidos.DataSource = dt;
                listBoxConcedidos.DisplayMember = "Descripción";
                listBoxConcedidos.ValueMember = "PermisoId";
                listBoxConcedidos.SelectedIndex = -1;
                MDIPrincipal.ActualizarBarraDeEstado();
            }
            catch (Exception ex)
            {
                Utils.MsgCatchOue(ex);
            }
        }

        private void BtnAgregarTodos_Click(object sender, EventArgs e)
        {
            try
            {
                MDIPrincipal.ActualizarBarraDeEstado(Utils.insertandoRegistro);
                // Leer catálogo de permisos desde la lista
                var pesmisosCatalogo = new List<int>();
                foreach (DataRowView drv in listBoxCatalogo.Items)
                    pesmisosCatalogo.Add(Convert.ToInt32(drv["PermisoId"]));
                var existentes = new HashSet<int>(
                    listBoxConcedidos.Items
                        .OfType<DataRowView>()
                        .Select(drv => Convert.ToInt32(drv["PermisoId"]))
                );
                // Filtrar los permisos que no existen aún
                var aInsertar = new List<int>();
                foreach (var pid in pesmisosCatalogo)
                    if (!existentes.Contains(pid))
                        aInsertar.Add(pid);
                if (aInsertar.Count == 0)
                {
                    Utils.MensajeInformation("No hay nuevos permisos para insertar");;
                    MDIPrincipal.ActualizarBarraDeEstado();
                    return;
                }
                // Insertar en bloque (transacción manejada por el repositorio)
                new PermisoRepository(cnStr).InsertarPermisos(Convert.ToInt32(txtId.Text), aInsertar);
                LlenarListBoxConcedidos();
                Utils.MensajeInformation($"Se concedieron todos los permisos al usuario {txtUsuario.Text}");
            }
            catch (Exception ex)
            {
                Utils.MsgCatchOue(ex);
            }
            finally
            {
                MDIPrincipal.ActualizarBarraDeEstado();
            }
        }

        private void BtnQuitarTodos_Click(object sender, EventArgs e)
        {
            try
            {
                MDIPrincipal.ActualizarBarraDeEstado(Utils.eliminandoRegistro);
                int filasAfectadas = new PermisoRepository(cnStr).EliminarPermisos(Convert.ToInt32(txtId.Text));
                if (filasAfectadas > 0)
                {
                    LlenarListBoxConcedidos();
                    Utils.MensajeInformation($"Se eliminaron {filasAfectadas} permisos concedidos al usuario {txtUsuario.Text}");
                }
                else
                    Utils.MensajeInformation($"No se encontraron permisos concedidos al usuario {txtUsuario.Text}");
            }
            catch (Exception ex)
            {
                Utils.MsgCatchOue(ex);
            }
            finally
            {
                MDIPrincipal.ActualizarBarraDeEstado();
            }
        }

        private void BtnAgregar_Click(object sender, EventArgs e)
        {
            if (listBoxCatalogo.SelectedIndex < 0)
            {
                Utils.MensajeExclamation("Debe seleccionar un permiso del catálogo para agregarlo.");
                return;
            }
            try
            {
                MDIPrincipal.ActualizarBarraDeEstado(Utils.insertandoRegistro);
                int permisoId = Convert.ToInt32(listBoxCatalogo.SelectedValue);
                new PermisoRepository(cnStr).InsertarPermiso(Convert.ToInt32(txtId.Text), permisoId);
                LlenarListBoxConcedidos();

            }
            catch (Exception ex)
            {
                Utils.MsgCatchOue(ex);
            }
            finally
            {
                MDIPrincipal.ActualizarBarraDeEstado();
            }
        }

        private void BtnQuitar_Click(object sender, EventArgs e)
        {
            try
            {
                if (listBoxConcedidos.SelectedIndex < 0)
                {
                    Utils.MensajeExclamation("Debe seleccionar un permiso concedido para eliminarlo.");
                    return;
                }
                MDIPrincipal.ActualizarBarraDeEstado(Utils.eliminandoRegistro);
                int permisoId = Convert.ToInt32(listBoxConcedidos.SelectedValue);
                new PermisoRepository(cnStr).EliminarPermiso(Convert.ToInt32(txtId.Text), permisoId);
                LlenarListBoxConcedidos();
            }
            catch (Exception ex)
            {
                Utils.MsgCatchOue(ex);
            }
            finally
            {
                MDIPrincipal.ActualizarBarraDeEstado();
            }
        }

        private void Dgv_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            string estado = e.Value.ToString();
            if (estado == "Activo")
            {
                e.CellStyle.BackColor = Color.LightGreen;
                e.CellStyle.ForeColor = Color.Black;
            }
            else if (estado == "Inactivo")
            {
                e.CellStyle.BackColor = Color.Red;
                e.CellStyle.ForeColor = Color.White;
            }
        }
    }
}
