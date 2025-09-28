using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Windows.Forms;

namespace NorthwindTradersV4MySql
{
    public partial class FrmCategoriasProductos : Form
    {

        string cnStr = ConfigurationManager.ConnectionStrings["NorthwindMySql"].ConnectionString;
        BindingSource bsCategorias = new BindingSource();
        BindingSource bsProductos = new BindingSource();

        public FrmCategoriasProductos()
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
        }

        private void GrbPaint(object sender, PaintEventArgs e) => Utils.GrbPaint2(this, sender, e);

        private void FrmCategoriasProductos_FormClosed(object sender, FormClosedEventArgs e) => MDIPrincipal.ActualizarBarraDeEstado();

        private void FrmCategoriasProductos_Load(object sender, EventArgs e)
        {
            dgvCategorias.DataSource = bsCategorias;
            dgvProductos.DataSource = bsProductos;
            GetData();
            Utils.ConfDgv(dgvCategorias);
            Utils.ConfDgv(dgvProductos);
            ConfDgvCategorias(dgvCategorias);
            ConfDgvProductos(dgvProductos);
            MDIPrincipal.ActualizarBarraDeEstado($"Se encontraron {dgvCategorias.RowCount} registros en categorías y {dgvProductos.RowCount} registros de productos en la categoría {dgvCategorias.CurrentRow.Cells["CategoryName"].Value}");
        }

        private void GetData()
        {
            try
            {
                MDIPrincipal.ActualizarBarraDeEstado(Utils.clbdd);
                var ds = new CategoriaRepository(cnStr).ObtenerCategoriasProductosDataSet();
                bsCategorias.DataSource = ds;
                bsCategorias.DataMember = "Categorias";
                bsProductos.DataSource = bsCategorias;
                bsProductos.DataMember = "CategoriasProductos";
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

        private void ConfDgvCategorias(DataGridView dgv)
        {
            dgv.Columns["CategoryId"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgv.Columns["CategoryName"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgv.Columns["Picture"].Width = 50;
            dgv.Columns["Picture"].DefaultCellStyle.Padding = new Padding(4, 4, 4, 4);
            ((DataGridViewImageColumn)dgv.Columns["Picture"]).ImageLayout = DataGridViewImageCellLayout.Zoom;

            dgv.Columns["CategoryId"].HeaderText = "Id";
            dgv.Columns["CategoryName"].HeaderText = "Categoría";
            dgv.Columns["Description"].HeaderText = "Descripción";
            dgv.Columns["Picture"].HeaderText = "Foto";
        }

        private void ConfDgvProductos(DataGridView dgv)
        {
            dgv.Columns["UnitPrice"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.Columns["UnitPrice"].DefaultCellStyle.Format = "c";
            dgv.Columns["UnitsInStock"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.Columns["UnitsOnOrder"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.Columns["ReorderLevel"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgv.Columns["ProductId"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgv.Columns["ProductName"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgv.Columns["QuantityPerUnit"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgv.Columns["UnitPrice"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgv.Columns["UnitsInStock"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgv.Columns["UnitsOnOrder"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgv.Columns["ReorderLevel"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgv.Columns["CompanyName"].AutoSizeMode= DataGridViewAutoSizeColumnMode.AllCells;

            dgv.Columns["Discontinued"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgv.Columns["CategoryName"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgv.Columns["Description"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            dgv.Columns["CategoryId"].Visible = false;
            dgv.Columns["SupplierId"].Visible = false;

            dgv.Columns["ProductId"].HeaderText = "Id";
            dgv.Columns["CategoryName"].HeaderText = "Categoría";
            dgv.Columns["ProductName"].HeaderText = "Producto";
            dgv.Columns["QuantityPerUnit"].HeaderText = "Cantidad por unidad";
            dgv.Columns["UnitPrice"].HeaderText = "Precio";
            dgv.Columns["UnitsInStock"].HeaderText = "Unidades en inventario";
            dgv.Columns["UnitsOnOrder"].HeaderText = "Unidades en pedido";
            dgv.Columns["ReorderLevel"].HeaderText = "Punto de pedido";
            dgv.Columns["Discontinued"].HeaderText = "Descontinuado";
            dgv.Columns["Description"].HeaderText = "Descripción de categoría";
            dgv.Columns["CompanyName"].HeaderText = "Proveedor";

            dgv.Columns["ProductId"].DisplayIndex = 0;
            dgv.Columns["CategoryName"].DisplayIndex = 1;
            dgv.Columns["ProductName"].DisplayIndex = 2;
            dgv.Columns["QuantityPerUnit"].DisplayIndex = 3;
            dgv.Columns["UnitPrice"].DisplayIndex = 4;
            dgv.Columns["UnitsInStock"].DisplayIndex = 5;
            dgv.Columns["UnitsOnOrder"].DisplayIndex = 6;
            dgv.Columns["ReorderLevel"].DisplayIndex = 7;
            dgv.Columns["Discontinued"].DisplayIndex = 8;
            dgv.Columns["Description"].DisplayIndex = 9;
            dgv.Columns["CompanyName"].DisplayIndex = 10;
        }


        private void dgvCategorias_SelectionChanged(object sender, EventArgs e)
        {
            MDIPrincipal.ActualizarBarraDeEstado($"Se encontraron {dgvCategorias.RowCount} registros en categorías y {dgvProductos.RowCount} registros de productos, en la categoría {dgvCategorias.CurrentRow.Cells["CategoryName"].Value}");
        }

        private void dgvCategorias_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            //Utils.ActualizarBarraDeEstado(this, $"Se encontraron {dgvCategorias.RowCount} registros en categorías y {dgvProductos.RowCount} registros de productos, en la categoría seleccionada.");
        }

    }
}
