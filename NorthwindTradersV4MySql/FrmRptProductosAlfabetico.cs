using Microsoft.Reporting.WinForms;
using System;
using System.Configuration;
using System.Data;
using System.Windows.Forms;

namespace NorthwindTradersV4MySql
{
    public partial class FrmRptProductosAlfabetico: Form
    {
        public FrmRptProductosAlfabetico()
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
        }

        private void GrbPaint(object sender, PaintEventArgs e) => Utils.GrbPaint(this, sender, e);

        private void FrmRptProductosAlfabetico_FormClosed(object sender, FormClosedEventArgs e) => MDIPrincipal.ActualizarBarraDeEstado();

        private void FrmRptProductosAlfabetico_Load(object sender, EventArgs e)
        {
            try
            {
                MDIPrincipal.ActualizarBarraDeEstado(Utils.clbdd);
                DtoProductosBuscar dtoProductosBuscar = null;
                dtoProductosBuscar = new DtoProductosBuscar
                {
                    OrdenadoPor = "ProductName",
                    AscDesc = "Asc"
                };
                string titulo = "» Reporte de productos en orden alfabético «";
                string subtitulo = $" Ordenado por: [ Producto ] [ Ascendente ]";
                groupBox1.Text = titulo + " | » " + subtitulo + " «";
                string strProcedure = "spProductosV2";
                var dt = new ProductoRepository(ConfigurationManager.ConnectionStrings["NorthwindMySql"].ConnectionString).RptProductosListado(dtoProductosBuscar, strProcedure);
                MDIPrincipal.ActualizarBarraDeEstado($"Se encontraron {dt.Rows.Count} registros");
                if (dt.Rows.Count > 0)
                {
                    ReportDataSource reportDataSource = new ReportDataSource("DataSet1", dt);
                    reportViewer1.LocalReport.DataSources.Clear();
                    reportViewer1.LocalReport.DataSources.Add(reportDataSource);
                    ReportParameter rp = new ReportParameter("titulo", titulo);
                    ReportParameter rp2 = new ReportParameter("subtitulo", subtitulo);
                    reportViewer1.LocalReport.SetParameters(new ReportParameter[] { rp, rp2 });
                    reportViewer1.RefreshReport();
                }
                else
                {
                    reportViewer1.LocalReport.DataSources.Clear();
                    ReportDataSource reportDataSource = new ReportDataSource("DataSet1", new DataTable());
                    reportViewer1.LocalReport.DataSources.Add(reportDataSource);
                    ReportParameter rp = new ReportParameter("titulo", titulo);
                    ReportParameter rp2 = new ReportParameter("subtitulo", subtitulo);
                    reportViewer1.LocalReport.SetParameters(new ReportParameter[] { rp, rp2 });
                    reportViewer1.RefreshReport();
                    Utils.MensajeExclamation(Utils.noDatos);
                }
            }
            catch (Exception ex) { Utils.MsgCatchOue(ex); }
        }
    }
}
