using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace NorthwindTradersV4MySql
{
    public partial class FrmCategoriasCrud : Form
    {

        string cnStr = ConfigurationManager.ConnectionStrings["NorthwindMySql"].ConnectionString;
        private readonly CategoriaRepository repo;
        bool EventoCargado = true; // esta variable es necesaria para controlar el manejador de eventos de la celda del dgv, ojo no quitar
        OpenFileDialog openFileDialog;

        public FrmCategoriasCrud()
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
            repo = new CategoriaRepository(cnStr);
        }

        private void GrbPaint(object sender, PaintEventArgs e) => Utils.GrbPaint(this, sender, e);

        private void FrmCategoriasCrud_FormClosed(object sender, FormClosedEventArgs e) => MDIPrincipal.ActualizarBarraDeEstado();

        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {
            splitContainer1.SplitterDistance = 250;
        }

        private void FrmCategoriasCrud_Load(object sender, EventArgs e)
        {
            // Establecer el tamaño inicial
            splitContainer1.SplitterDistance = 250;

            // Asociar el evento
            splitContainer1.SplitterMoved += new SplitterEventHandler(splitContainer1_SplitterMoved);

            DeshabilitarControles();
            LlenarDgv(null);
        }

        private void DeshabilitarControles()
        {
            txtCategoria.ReadOnly = txtDescripcion.ReadOnly = true;
            picFoto.Enabled = false;
        }

        private void HabilitarControles()
        {
            txtCategoria.ReadOnly = txtDescripcion.ReadOnly = false;
            picFoto.Enabled = true;
        }

        private void LlenarDgv(object sender)
        {
            try
            {
                MDIPrincipal.ActualizarBarraDeEstado(Utils.clbdd);
                DtoCategoriasBuscar dtoCategoriasBuscar = new DtoCategoriasBuscar()
                {
                    IdIni = string.IsNullOrEmpty(txtBIdIni.Text) ? 0 : int.Parse(txtBIdIni.Text),
                    IdFin = string.IsNullOrEmpty(txtBIdFin.Text) ? 0 : int.Parse(txtBIdFin.Text),
                    CategoryName = txtBCategoria.Text.Trim()
                };
                var dt = repo.ObtenerCategorias(sender, dtoCategoriasBuscar);
                Dgv.DataSource = dt;
                Utils.ConfDgv(Dgv);
                ConfDgv();
                if (sender == null)
                    MDIPrincipal.ActualizarBarraDeEstado($"Se muestran las últimas {Dgv.RowCount} categorías registradas");
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
            Dgv.Columns["CategoryID"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            Dgv.Columns["CategoryName"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            Dgv.Columns["Picture"].Width = 50;
            Dgv.Columns["Picture"].DefaultCellStyle.Padding = new Padding(4, 4, 4, 4);
            ((DataGridViewImageColumn)Dgv.Columns["Picture"]).ImageLayout = DataGridViewImageCellLayout.Zoom;

            Dgv.Columns["CategoryID"].HeaderText = "Id";
            Dgv.Columns["CategoryName"].HeaderText = "Categoría";
            Dgv.Columns["Description"].HeaderText = "Descripción";
            Dgv.Columns["Picture"].HeaderText = "Foto";
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            BorrarMensajesError();
            BorrarDatosCategoria();
            if (tabcOperacion.SelectedTab != tbpRegistrar)
                DeshabilitarControles();
            LlenarDgv(sender);
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            BorrarDatosBusqueda();
            BorrarDatosCategoria();
            BorrarMensajesError();
            if (tabcOperacion.SelectedTab != tbpRegistrar)
                DeshabilitarControles();
            LlenarDgv(null);
        }

        private void BorrarMensajesError() => errorProvider1.Clear();

        private void BorrarDatosBusqueda()
        {
            txtBCategoria.Text = txtBIdIni.Text = txtBIdFin.Text = "";
        }

        private void BorrarDatosCategoria()
        {
            txtCategoria.Text = txtDescripcion.Text = txtId.Text = "";
            picFoto.Image = null;
            picFoto.BackgroundImage = Properties.Resources.Categorias;
        }

        private void txtBIdIni_KeyPress(object sender, KeyPressEventArgs e)
        {
            Utils.ValidarDigitosSinPunto(sender, e);
        }

        private void txtBIdFin_KeyPress(object sender, KeyPressEventArgs e)
        {
            Utils.ValidarDigitosSinPunto(sender, e);
        }

        private void txtBIdIni_Leave(object sender, EventArgs e)
        {
            Utils.ValidaTxtBIdIni(txtBIdIni, txtBIdFin);
        }

        private void txtBIdFin_Leave(object sender, EventArgs e)
        {
            Utils.ValidaTxtBIdFin(txtBIdIni, txtBIdFin);
        }

        private bool ValidarControles()
        {
            bool valida = true;
            if (txtCategoria.Text.Trim() == "")
            {
                valida = false;
                errorProvider1.SetError(txtCategoria, "Ingrese la categoría");
            }
            if (txtDescripcion.Text.Trim() == "")
            {
                valida = false;
                errorProvider1.SetError(txtDescripcion, "Ingrese la descripción");
            }
            if (picFoto.Image == null)
            {
                valida = false;
                errorProvider1.SetError(btnCargar, "Ingrese la imagen");
            }
            return valida;
        }

        private void FrmCategoriasCrud_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (tabcOperacion.SelectedTab != tbpConsultar)
            {
                if (txtId.Text.Trim() != "" || txtCategoria.Text.Trim() != "" || txtDescripcion.Text.Trim() != "" || picFoto.Image != null)
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
                txtId.Text = dgvr.Cells["CategoryId"].Value.ToString();
                txtCategoria.Text = dgvr.Cells["CategoryName"].Value.ToString();
                txtDescripcion.Text = dgvr.Cells["Description"].Value.ToString();
                if (dgvr.Cells["Picture"].Value != DBNull.Value)
                {
                    byte[] foto = (byte[])dgvr.Cells["Picture"].Value;
                    MemoryStream ms;
                    if (int.Parse(txtId.Text) <= 8)
                    {
                        ms = new MemoryStream(foto, 78, foto.Length - 78);
                        btnCargar.Enabled = false; // no se permite modificar porque desconozco el formato de la imagen
                    }
                    else
                    {
                        ms = new MemoryStream(foto);
                        btnCargar.Enabled = true;
                    }
                    picFoto.Image = Image.FromStream(ms);
                    picFoto.BackgroundImage = null;
                }
                else
                {
                    picFoto.Image = null;
                    picFoto.BackgroundImage = Properties.Resources.Categorias;
                }
                if (tabcOperacion.SelectedTab == tbpConsultar)
                {
                    btnCargar.Visible = false;
                    btnOperacion.Visible = false;
                }
                else if (tabcOperacion.SelectedTab == tbpModificar)
                {
                    HabilitarControles();
                    btnOperacion.Visible = true;
                    btnCargar.Visible = true;
                    btnOperacion.Enabled = true;
                }
                else if (tabcOperacion.SelectedTab == tbpEliminar)
                {
                    btnCargar.Visible = false;
                    btnOperacion.Visible = true;
                    btnOperacion.Enabled = true;
                }
            }
        }

        void ActualizaDgv() => btnLimpiar.PerformClick();

        private void tabcOperacion_Selected(object sender, TabControlEventArgs e)
        {
            BorrarDatosCategoria();
            BorrarMensajesError();
            picFoto.BackgroundImage = Properties.Resources.Categorias;
            if (tabcOperacion.SelectedTab == tbpRegistrar)
            {
                if (EventoCargado)
                {
                    Dgv.CellClick -= new DataGridViewCellEventHandler(Dgv_CellClick);
                    EventoCargado = false;
                }
                HabilitarControles();
                btnOperacion.Text = "Registrar categoría";
                btnOperacion.Visible = true;
                btnOperacion.Enabled = true;
                btnCargar.Visible = true;
                btnCargar.Enabled = true;
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
                btnCargar.Enabled = false;
                if (tabcOperacion.SelectedTab == tbpConsultar)
                {
                    btnOperacion.Visible = false;
                    btnCargar.Visible = false;
                }
                else if (tabcOperacion.SelectedTab == tbpModificar)
                {
                    btnOperacion.Text = "Modificar categoría";
                    btnOperacion.Enabled = false;
                    btnOperacion.Visible = true;
                    btnCargar.Visible = true;
                }
                else if (tabcOperacion.SelectedTab == tbpEliminar)
                {
                    btnOperacion.Text = "Eliminar categoría";
                    btnOperacion.Enabled = false;
                    btnOperacion.Visible = true;
                    btnCargar.Visible = false;
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
                    btnCargar.Enabled = false;
                    byte[] fileFoto = null;
                    Image image = picFoto.Image;
                    ImageConverter converter = new ImageConverter();
                    fileFoto = (byte[])converter.ConvertTo(image, typeof(byte[]));
                    try
                    {
                        var categoria = new Categoria
                        {
                            CategoryName = txtCategoria.Text.Trim(),
                            Description = txtDescripcion.Text.Trim(),
                            Picture = fileFoto
                        };
                        int numRegs = repo.Insertar(categoria);
                        if (numRegs > 0)
                        {
                            txtId.Text = categoria.CategoryID.ToString();
                            Utils.MensajeInformation($"La categoría con Id: {txtId.Text} y Nombre: {txtCategoria.Text} se registró satisfactoriamente");
                        }
                        else
                            Utils.MensajeError($"La categoria con Nombre: {txtCategoria.Text} NO fue registrada en la base de datos");
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
                    picFoto.BackgroundImage = Properties.Resources.Categorias;
                    ActualizaDgv();
                }
            }
            else if (tabcOperacion.SelectedTab == tbpModificar)
            {
                if (txtId.Text == "")
                {
                    Utils.MensajeExclamation("Seleccione la categoría a modificar");
                    return;
                }
                if (ValidarControles())
                {
                    btnOperacion.Enabled = false;
                    btnCargar.Enabled = false;
                    MDIPrincipal.ActualizarBarraDeEstado(Utils.modificandoRegistro);
                    DeshabilitarControles();
                    byte[] fileFoto = null;
                    Image image = picFoto.Image;
                    ImageConverter converter = new ImageConverter();
                    fileFoto = (byte[])converter.ConvertTo(image, typeof(byte[]));
                    try
                    {
                        var categoria = new Categoria
                        {
                            CategoryID = int.Parse(txtId.Text),
                            CategoryName = txtCategoria.Text.Trim(),
                            Description = txtDescripcion.Text.Trim(),
                            Picture = fileFoto
                        };
                        int numRegs = repo.Actualizar(categoria);
                        if (numRegs > 0)
                            Utils.MensajeInformation($"La categoría con Id: {txtId.Text} y Nombre: {txtCategoria.Text} se modificó satisfactoriamente");
                        else
                            Utils.MensajeError($"La categoría con Id: {txtId.Text} y Nombre: {txtCategoria.Text} NO fue modificada en la base de datos");
                    }
                    catch (MySqlException ex)
                    {
                        Utils.MsgCatchOueclbdd(ex);
                    }
                    catch (Exception ex)
                    {
                        Utils.MsgCatchOue(ex);
                    }
                    picFoto.BackgroundImage = Properties.Resources.Categorias;
                    ActualizaDgv();
                }
            }
            else if (tabcOperacion.SelectedTab == tbpEliminar)
            {
                if (txtId.Text == "")
                {
                    Utils.MensajeExclamation("Seleccione la categoría a eliminar");
                    return;
                }
                if (Utils.MensajeQuestion($"¿Esta seguro de eliminar la categoría con Id: {txtId.Text} y Nombre: {txtCategoria.Text}?")  == DialogResult.Yes)
                {
                    btnOperacion.Enabled = false;
                    MDIPrincipal.ActualizarBarraDeEstado(Utils.eliminandoRegistro);
                    try
                    {
                        var categoria = new Categoria
                        {
                            CategoryID = int.Parse(txtId.Text)
                        };
                        int numRegs = repo.Eliminar(categoria);
                        if (numRegs > 0)
                            Utils.MensajeInformation($"La categoría con Id: {txtId.Text} y Nombre: {txtCategoria.Text} se eliminó satisfactoriamente");
                        else
                            Utils.MensajeExclamation($"La categoría con Id: {txtId.Text} y Nombre: {txtCategoria.Text} NO se eliminó en la base de datos");
                    }
                    catch (MySqlException ex)
                    {
                        Utils.MsgCatchOueclbdd(ex);
                    }
                    catch (Exception ex)
                    {
                        Utils.MsgCatchOue(ex);
                    }
                    picFoto.BackgroundImage = Properties.Resources.Categorias;
                    ActualizaDgv();
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
                picFoto.BackgroundImage = null;
                errorProvider1.SetError(btnCargar, "");
            }
        }
    }
}
