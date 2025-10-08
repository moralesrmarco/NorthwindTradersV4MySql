using System;
using System.Configuration;
using System.Windows.Forms;

namespace NorthwindTradersV4MySql
{
    public partial class FrmProductosPorEncimaPrecioPromedio : Form
    {

        string cnStr = ConfigurationManager.ConnectionStrings["NorthwindMySql"].ConnectionString;

        public FrmProductosPorEncimaPrecioPromedio()
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
        }

        private void GrbPaint(object sender, PaintEventArgs e) => Utils.GrbPaint2(this, sender, e);

        private void FrmProductosPorEncimaPrecioPromedio_FormClosed(object sender, FormClosedEventArgs e) => MDIPrincipal.ActualizarBarraDeEstado();

        private void FrmProductosPorEncimaPrecioPromedio_Load(object sender, EventArgs e)
        {
            CalcularPrecioPromedio();
            Utils.ConfDgv(Dgv);
            LlenarDgv();
            ConfDgv();
        }

        private void CalcularPrecioPromedio()
        {
            try
            {
                MDIPrincipal.ActualizarBarraDeEstado(Utils.clbdd);
                var precioPromedio = new ProductoRepository(cnStr).ObtenerPrecioPromedio();
                string strPrecioPromedio = precioPromedio.ToString("c");
                Grb.Text = $"» Listado de productos con el precio por encima del precio promedio: {strPrecioPromedio} «";
                MDIPrincipal.ActualizarBarraDeEstado();
            }
            catch (Exception ex)
            {
                Utils.MsgCatchOue(ex);
            }
        }

        private void LlenarDgv()
        {
            try
            {
                var dt = new ProductoRepository(cnStr).ObtenerProductosPorEncimaDelPrecioPromedio();
                Dgv.DataSource = dt;
                MDIPrincipal.ActualizarBarraDeEstado($"Se encontraron {Dgv.RowCount} registros");
            }
            catch (Exception ex)
            {
                Utils.MsgCatchOue(ex);
            }
        }

        private void ConfDgv()
        {
            Dgv.Columns["Fila"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            Dgv.Columns["Precio"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            Dgv.Columns["Precio"].DefaultCellStyle.Format = "c";
            Dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

    }
}
