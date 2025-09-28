using Microsoft.Reporting.WinForms;
using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;
using System.Windows.Forms;

namespace NorthwindTradersV4MySql
{
    public partial class FrmRptProductosPorCategorias: Form
    {

        string cnStr = ConfigurationManager.ConnectionStrings["NorthwindMySql"].ConnectionString;

        public FrmRptProductosPorCategorias()
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
        }

        private void GrbPaint(object sender, PaintEventArgs e) => Utils.GrbPaint2(this, sender, e);

        private void FrmRptProductosPorCategorias_FormClosed(object sender, FormClosedEventArgs e) => MDIPrincipal.ActualizarBarraDeEstado();

        private void FrmRptProductosPorCategorias_Load(object sender, EventArgs e)
        {
            try
            {
                MDIPrincipal.ActualizarBarraDeEstado(Utils.clbdd);
                DataTable tbl = new CategoriaRepository(cnStr).ObtenerProductosPorCategoriaListado();
                MDIPrincipal.ActualizarBarraDeEstado($"Se encontraron {tbl.Rows.Count} registros");
                if (tbl.Rows.Count > 0)
                {
                    ReportDataSource reportDataSource = new ReportDataSource("DataSet1", tbl);
                    reportViewer1.LocalReport.DataSources.Clear();
                    reportViewer1.LocalReport.DataSources.Add(reportDataSource);
                    reportViewer1.LocalReport.Refresh();
                    reportViewer1.RefreshReport();
                }
                else
                {
                    reportViewer1.LocalReport.DataSources.Clear();
                    ReportDataSource reportDataSource = new ReportDataSource("DataSet1", new DataTable());
                    reportViewer1.LocalReport.DataSources.Add(reportDataSource);
                    reportViewer1.LocalReport.Refresh();
                    reportViewer1.RefreshReport();
                    Utils.MensajeExclamation(Utils.noDatos);
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
        }
    }
}
