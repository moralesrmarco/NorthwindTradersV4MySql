using System;
using System.Drawing;
using System.Windows.Forms;

namespace NorthwindTradersV4MySql
{
    public partial class MDIPrincipal : Form
    {
        private int childFormNumber = 0;
        public static MDIPrincipal Instance { get; private set; }

        public MDIPrincipal()
        {
            InitializeComponent();
            Instance = this;
            WindowState = FormWindowState.Maximized;
        }

        public ToolStripStatusLabel ToolStripEstado
        {
            get { return toolStripStatus; }
            set { toolStripStatus = value; }
        }

        public static void ActualizarBarraDeEstado(string mensaje = "Listo.", bool error = false)
        {
            if (Instance != null && !Instance.IsDisposed)
            {
                if (mensaje != "Listo.")
                {
                    if (error)
                        Instance.ToolStripEstado.BackColor = System.Drawing.Color.Red;
                    else
                        Instance.ToolStripEstado.BackColor = SystemColors.ActiveCaption;
                }
                else
                {
                    if (error)
                    {
                        Instance.ToolStripEstado.ForeColor = System.Drawing.Color.White;
                        Instance.ToolStripEstado.Font = new Font(Instance.ToolStripEstado.Font, FontStyle.Bold);
                    }
                    else
                    {
                        Instance.ToolStripEstado.ForeColor = SystemColors.ControlText;
                        Instance.ToolStripEstado.BackColor = SystemColors.Control;
                        Instance.ToolStripEstado.Font = new Font(Instance.ToolStripEstado.Font, FontStyle.Regular);
                    }
                }
                Instance.ToolStripEstado.Text = mensaje;
                Instance.Refresh();
            }
        }

        private void ShowNewForm(object sender, EventArgs e)
        {
            Form childForm = new Form();
            childForm.MdiParent = this;
            childForm.Text = "Ventana " + childFormNumber++;
            childForm.Show();
        }

        private void OpenFile(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            openFileDialog.Filter = "Archivos de texto (*.txt)|*.txt|Todos los archivos (*.*)|*.*";
            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string FileName = openFileDialog.FileName;
            }
        }

        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            saveFileDialog.Filter = "Archivos de texto (*.txt)|*.txt|Todos los archivos (*.*)|*.*";
            if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string FileName = saveFileDialog.FileName;
            }
        }

        private void ExitToolsStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CutToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void CopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void PasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void ToolBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStrip.Visible = toolBarToolStripMenuItem.Checked;
        }

        private void StatusBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            statusStrip.Visible = statusBarToolStripMenuItem.Checked;
        }

        private void CascadeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.Cascade);
        }

        private void TileVerticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileVertical);
        }

        private void TileHorizontalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void ArrangeIconsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.ArrangeIcons);
        }

        private void CloseAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form childForm in MdiChildren)
            {
                childForm.Close();
            }
        }

        private void mantenimientoDeEmpleadosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Utils.CerrarFormularios();
            FrmEmpleadosCrud frmEmpleadosCrud = new FrmEmpleadosCrud
            {
                MdiParent = this
            };
            frmEmpleadosCrud.Show();
        }

        private void reporteDeEmpleadosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Utils.CerrarFormularios();
            FrmRptEmpleados frmRptEmpleados = new FrmRptEmpleados
            {
                MdiParent = this
            };
            frmRptEmpleados.Show();
        }

        private void reporteDeEmpleadosConFotoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Utils.CerrarFormularios();
            FrmRptEmpleadosConFoto frmRptEmpleadosConFoto = new FrmRptEmpleadosConFoto
            {
                MdiParent = this
            };
            frmRptEmpleadosConFoto.Show();
        }

        private void reporteDeEmpleadosConFoto2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Utils.CerrarFormularios();
            FrmRptEmpleado2 frmRptEmpleado2 = new FrmRptEmpleado2
            {
                MdiParent = this
            };
            frmRptEmpleado2.Show();
        }

        private void mantenimientoDeClientesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Utils.CerrarFormularios();
            FrmClientesCrud frmClientesCrud = new FrmClientesCrud
            {
                MdiParent = this
            };
            frmClientesCrud.Show();
        }

        private void directorioDeClientesYProveedoresToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Utils.CerrarFormularios();
            FrmClientesyProveedoresDirectorio frmClientesyProveedoresDirectorio = new FrmClientesyProveedoresDirectorio
            {
                MdiParent = this
            };
            frmClientesyProveedoresDirectorio.Show();
        }

        private void directorioDeClientesYProveedoresPorCiudadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Utils.CerrarFormularios();
            FrmClientesyProveedoresDirectorioxCiudad frmClientesyProveedoresDirectorioxCiudad = new FrmClientesyProveedoresDirectorioxCiudad
            {
                MdiParent = this
            };
            frmClientesyProveedoresDirectorioxCiudad.Show();
        }

        private void directorioDeClientesYProveedoresPorPaísToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Utils.CerrarFormularios();
            FrmClientesyProveedoresDirectorioxPais frmClientesyProveedoresDirectorioxPais = new FrmClientesyProveedoresDirectorioxPais
            {
                MdiParent = this
            };
            frmClientesyProveedoresDirectorioxPais.Show();
        }

        private void directorioDeClientesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Utils.CerrarFormularios();
            FrmRptClientes frmRptClientes = new FrmRptClientes
            {
                MdiParent = this
            };
            frmRptClientes.Show();
        }

        private void directorioDeClientesYProveedoresToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Utils.CerrarFormularios();
            FrmRptClientesyProveedoresDirectorio frmRptClientesyProveedoresDirectorio = new FrmRptClientesyProveedoresDirectorio
            {
                MdiParent = this
            };
            frmRptClientesyProveedoresDirectorio.Show();
        }

        private void directorioDeClientesYProveedoresPorCiudadToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Utils.CerrarFormularios();
            FrmRptClientesyProveedoresDirectorioxCiudad frmRptClientesyProveedoresDirectorioxCiudad = new FrmRptClientesyProveedoresDirectorioxCiudad
            {
                MdiParent = this
            };
            frmRptClientesyProveedoresDirectorioxCiudad.Show();
        }

        private void directorioDeClientesYProveedoresPorPaísToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Utils.CerrarFormularios();
            FrmRptClientesyProveedoresDirectorioxPais frmRptClientesyProveedoresDirectorioxPais = new FrmRptClientesyProveedoresDirectorioxPais
            {
                MdiParent = this
            };
            frmRptClientesyProveedoresDirectorioxPais.Show();
        }

        private void mantenimientoDeProveedoresToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Utils.CerrarFormularios();
            FrmProveedoresCrud frmProveedoresCrud = new FrmProveedoresCrud
            {
                MdiParent = this
            };
            frmProveedoresCrud.Show();
        }

        private void directorioDeClientesYProveedoresToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            Utils.CerrarFormularios();
            FrmClientesyProveedoresDirectorio frmClientesyProveedoresDirectorio = new FrmClientesyProveedoresDirectorio
            {
                MdiParent = this
            };
            frmClientesyProveedoresDirectorio.Show();
        }

        private void directorioDeClientesYProveedoresPorCiudadToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            Utils.CerrarFormularios();
            FrmClientesyProveedoresDirectorioxCiudad frmClientesyProveedoresDirectorioxCiudad = new FrmClientesyProveedoresDirectorioxCiudad
            {
                MdiParent = this
            };
            frmClientesyProveedoresDirectorioxCiudad.Show();
        }

        private void directorioDeClientesYProveedoresPorPaísToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            Utils.CerrarFormularios();
            FrmClientesyProveedoresDirectorioxPais frmClientesyProveedoresDirectorioxPais = new FrmClientesyProveedoresDirectorioxPais
            {
                MdiParent = this
            };
            frmClientesyProveedoresDirectorioxPais.Show();
        }

        private void consultaDeProductosPorProveedorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Utils.CerrarFormularios();
            FrmProveedoresProductos frmProveedoresProductos = new FrmProveedoresProductos
            {
                MdiParent = this
            };
            frmProveedoresProductos.Show();
        }

        private void directorioDeProveedoresToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Utils.CerrarFormularios();
            FrmRptProveedores frmRptProveedores = new FrmRptProveedores
            {
                MdiParent = this
            };
            frmRptProveedores.Show();
        }

        private void directorioDeClientesYProveedoresToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            Utils.CerrarFormularios();
            FrmRptClientesyProveedoresDirectorio frmRptClientesyProveedoresDirectorio = new FrmRptClientesyProveedoresDirectorio
            {
                MdiParent = this
            };
            frmRptClientesyProveedoresDirectorio.Show();
        }

        private void directorioDeClientesYProveedoresPorCiudadToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            Utils.CerrarFormularios();
            FrmRptClientesyProveedoresDirectorioxCiudad frmRptClientesyProveedoresDirectorioxCiudad = new FrmRptClientesyProveedoresDirectorioxCiudad
            {
                MdiParent = this
            };
            frmRptClientesyProveedoresDirectorioxCiudad.Show();
        }

        private void directorioDeClientesYProveedoresPorPaísToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            Utils.CerrarFormularios();
            FrmRptClientesyProveedoresDirectorioxPais frmRptClientesyProveedoresDirectorioxPais = new FrmRptClientesyProveedoresDirectorioxPais
            {
                MdiParent = this
            };
            frmRptClientesyProveedoresDirectorioxPais.Show();
        }

        private void reporteDeProductosPorProveedorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Utils.CerrarFormularios();
            FrmRptProductosPorProveedor frmRptProductosPorProveedor = new FrmRptProductosPorProveedor
            {
                MdiParent = this
            };
            frmRptProductosPorProveedor.Show();
        }

        private void reporteDeProductosPorProveedorConDetalleDelProveedorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Utils.CerrarFormularios();
            FrmRptProdPorProvConDetProv frmRptProdPorProvConDetProv = new FrmRptProdPorProvConDetProv
            {
                MdiParent = this
            };
            frmRptProdPorProvConDetProv.Show();
        }

        private void mantenimientoDeCategoríasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Utils.CerrarFormularios();
            FrmCategoriasCrud frmCategoriasCrud = new FrmCategoriasCrud
            {
                MdiParent = this
            };
            frmCategoriasCrud.Show();
        }

        private void consultaDeProductosPorCategoríaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Utils.CerrarFormularios();
            FrmCategoriasProductos frmCategoriasProductos = new FrmCategoriasProductos
            {
                MdiParent = this
            };
            frmCategoriasProductos.Show();
        }

        private void listadoDeProductosPorCategoríaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Utils.CerrarFormularios();
            FrmProductosPorCategoriasListado frmProductosPorCategoriasListado = new FrmProductosPorCategoriasListado
            {
                MdiParent = this
            };
            frmProductosPorCategoriasListado.Show();
        }

        private void reporteDeCategoríasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Utils.CerrarFormularios();
            FrmRptCategorias frmRptCategorias = new FrmRptCategorias
            {
                MdiParent = this
            };
            frmRptCategorias.Show();
        }

        private void reporteDeProductosPorCategoríaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Utils.CerrarFormularios();
            FrmRptProductosPorCategorias frmRptProductosPorCategorias = new FrmRptProductosPorCategorias
            {
                MdiParent = this
            };
            frmRptProductosPorCategorias.Show();
        }

        private void mantenimientoDeProductosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Utils.CerrarFormularios();
            FrmProductosCrud frmProductosCrud = new FrmProductosCrud
            {
                MdiParent = this
            };
            frmProductosCrud.Show();
        }

        private void listadoDeProductosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Utils.CerrarFormularios();
            FrmProductosListado frmProductosListado = new FrmProductosListado
            {
                MdiParent = this
            };
            frmProductosListado.Show();
        }

        private void consultaDeProductosPorCategoríaToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Utils.CerrarFormularios();
            FrmCategoriasProductos frmCategoriasProductos = new FrmCategoriasProductos
            {
                MdiParent = this
            };
            frmCategoriasProductos.Show();
        }

        private void consultaDeProductosPorProveedorToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Utils.CerrarFormularios();
            FrmProveedoresProductos frmProveedoresProductos = new FrmProveedoresProductos
            {
                MdiParent = this
            };
            frmProveedoresProductos.Show();
        }

        private void consultaAlfabéticaDeProductosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Utils.CerrarFormularios();
            FrmProductosConsultaAlfabetica frmProductosConsultaAlfabetica = new FrmProductosConsultaAlfabetica
            {
                MdiParent = this
            };
            frmProductosConsultaAlfabetica.Show();
        }

        private void listadoDeProductosPorCategoríasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Utils.CerrarFormularios();
            FrmProductosPorCategoriasListado frmProductosPorCategoriasListado = new FrmProductosPorCategoriasListado
            {
                MdiParent = this
            };
            frmProductosPorCategoriasListado.Show();
        }

        private void productosPorEncimaDelPrecioPromedioToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void reporteDeProductosToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void reporteDeProductosPorCategoríaToolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void reporteDeProductosPorProveedorToolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void reporteAlfabéticoDeProductosToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void reporteDeProductosPorProveedorConDetalleDelProveedorToolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }
    }
}
