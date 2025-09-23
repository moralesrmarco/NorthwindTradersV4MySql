using Microsoft.Reporting.WinForms;
using System;
using System.Windows.Forms;

namespace NorthwindTradersV4MySql
{
    public partial class FrmRptProductosPorProveedor : Form
    {
        string cnStr = System.Configuration.ConfigurationManager.ConnectionStrings["NorthwindMySql"].ConnectionString;
        ProductosxProveedoresRepository repo;

        public FrmRptProductosPorProveedor()
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
            repo = new ProductosxProveedoresRepository(cnStr);
        }

        private void GrbPaint(object sender, PaintEventArgs e) => Utils.GrbPaint2(this, sender, e);

        private void FrmRptProductosPorProveedor_FormClosed(object sender, FormClosedEventArgs e) => MDIPrincipal.ActualizarBarraDeEstado();

        private void FrmRptProductosPorProveedor_Load(object sender, EventArgs e)
        {
            try
            {
                MDIPrincipal.ActualizarBarraDeEstado(Utils.clbdd);
                var productosPorProveedor = repo.ObtenerProductosxProveedoresList();
                MDIPrincipal.ActualizarBarraDeEstado($"Se encontraron {productosPorProveedor.Count} registros");
                reportViewer1.LocalReport.DataSources.Clear();
                reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", productosPorProveedor));
                reportViewer1.RefreshReport();
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                Utils.MsgCatchOueclbdd(ex);
            }
            catch (Exception ex)
            {
                Utils.MsgCatchOue(ex);
            }
        }
    }
}
