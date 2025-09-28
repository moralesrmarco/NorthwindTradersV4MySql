using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Windows.Forms;

namespace NorthwindTradersV4MySql
{
    public partial class FrmProductosPorCategoriasListado : Form
    {

        string cnStr = ConfigurationManager.ConnectionStrings["NorthwindMySql"].ConnectionString;

        private void GrbPaint(object sender, PaintEventArgs e) => Utils.GrbPaint2(this, sender, e);

        private void FrmProductosPorCategoriasListado_FormClosed(object sender, FormClosedEventArgs e) => MDIPrincipal.ActualizarBarraDeEstado();

        public FrmProductosPorCategoriasListado()
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
        }


        private void FrmProductosPorCategoriasListado_Load(object sender, EventArgs e)
        {
            Utils.ConfDgv(DgvListado);
            LlenarDgv();
            ConfDgv();
        }

        private void LlenarDgv()
        {
            try
            {
                MDIPrincipal.ActualizarBarraDeEstado(Utils.clbdd);
                var dt = new CategoriaRepository(cnStr).ObtenerProductosPorCategoriaListado();
                DgvListado.DataSource = dt;
                MDIPrincipal.ActualizarBarraDeEstado($"Se encontraron {DgvListado.RowCount} registros");
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
            DgvListado.Columns["ProductID"].Visible = false;

            DgvListado.Columns["CategoryName"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            DgvListado.Columns["UnitPrice"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            DgvListado.Columns["UnitsInStock"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            DgvListado.Columns["UnitsOnOrder"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            DgvListado.Columns["ReorderLevel"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            DgvListado.Columns["Discontinued"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

            DgvListado.Columns["UnitPrice"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            DgvListado.Columns["UnitsInStock"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            DgvListado.Columns["UnitsOnOrder"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            DgvListado.Columns["ReorderLevel"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            DgvListado.Columns["UnitPrice"].DefaultCellStyle.Format = "c";

            DgvListado.Columns["CategoryName"].HeaderText = "Categoría";
            DgvListado.Columns["ProductName"].HeaderText = "Producto";
            DgvListado.Columns["QuantityPerUnit"].HeaderText = "Cantidad por unidad";
            DgvListado.Columns["UnitPrice"].HeaderText = "Precio";
            DgvListado.Columns["UnitsInStock"].HeaderText = "Unidades en inventario";
            DgvListado.Columns["UnitsOnOrder"].HeaderText = "Unidades en pedido";
            DgvListado.Columns["ReorderLevel"].HeaderText = "Punto de pedido";
            DgvListado.Columns["Discontinued"].HeaderText = "Descontinuado";
            DgvListado.Columns["CompanyName"].HeaderText = "Proveedor";
        }
    }
}
