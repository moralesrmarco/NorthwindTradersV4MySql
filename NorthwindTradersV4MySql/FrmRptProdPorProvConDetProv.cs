using Microsoft.Reporting.WinForms;
using System.Windows.Forms;

namespace NorthwindTradersV4MySql
{
    public partial class FrmRptProdPorProvConDetProv : Form
    {
        string cnStr = System.Configuration.ConfigurationManager.ConnectionStrings["NorthwindMySql"].ConnectionString;

        public FrmRptProdPorProvConDetProv()
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
        }

        private void GrbPaint(object sender, PaintEventArgs e) => Utils.GrbPaint2(this, sender, e);

        private void FrmRptProdPorProvConDetProv_FormClosed(object sender, FormClosedEventArgs e) => MDIPrincipal.ActualizarBarraDeEstado();

        private void FrmRptProdPorProvConDetProv_Load(object sender, System.EventArgs e)
        {
            try
            {
                MDIPrincipal.ActualizarBarraDeEstado(Utils.clbdd);
                var productosPorProveedorConDetProv = new ProductosxProveedoresRepository(cnStr).ObtenerProductosxProveedoresConDetProvList();
                MDIPrincipal.ActualizarBarraDeEstado($"Se encontraron {productosPorProveedorConDetProv.Count} registros");
                reportViewer1.LocalReport.DataSources.Clear();
                reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", productosPorProveedorConDetProv));
                reportViewer1.RefreshReport();
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                Utils.MsgCatchOueclbdd(ex);
            }
            catch (System.Exception ex)
            {
                Utils.MsgCatchOue(ex);
            }
        }
    }
}
