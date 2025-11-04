using Microsoft.Reporting.WinForms;
using System;
using System.Configuration;
using System.Data;
using System.Windows.Forms;

namespace NorthwindTradersV4MySql
{
    public partial class FrmRptGraficaVentasPorVendedores : Form
    {

        private readonly string cnStr = ConfigurationManager.ConnectionStrings["NorthwindMySql"].ConnectionString;

        public FrmRptGraficaVentasPorVendedores()
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
        }

        private void GrbPaint(object sender, PaintEventArgs e) => Utils.GrbPaint(this, sender, e);

        private void FrmRptGraficaVentasPorVendedores_Load(object sender, EventArgs e)
        {
            DataTable dt = null;
            try
            {
                dt = new GraficaRepository(cnStr).ObtenerVentasPorVendedor();
            }
            catch (Exception ex)
            {
                Utils.MsgCatchOue(ex);
            }
            if (dt == null)
                return;
            reportViewer1.LocalReport.DataSources.Clear();
            var rds = new ReportDataSource("DataSet1", dt);
            reportViewer1.LocalReport.DataSources.Add(rds);
            reportViewer1.LocalReport.SetParameters(new ReportParameter("Subtitulo", "Ventas por vendedores de todos los años"));
            reportViewer1.RefreshReport();
        }
    }
}
