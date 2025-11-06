using Microsoft.Reporting.WinForms;
using System;
using System.Configuration;
using System.Data;
using System.Windows.Forms;

namespace NorthwindTradersV4MySql
{
    public partial class FrmRptGraficaVentasMensualesPorVendedorPorAnioBarras : Form
    {

        private readonly string cnStr = ConfigurationManager.ConnectionStrings["NorthwindMySql"].ConnectionString;

        public FrmRptGraficaVentasMensualesPorVendedorPorAnioBarras()
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
        }

        private void GrbPaint(object sender, PaintEventArgs e) => Utils.GrbPaint(this, sender, e);

        private void FrmRptGraficaVentasMensualesPorVendedorPorAnioBarras_Load(object sender, EventArgs e)
        {
            LlenarCmbVentasDelAño();
        }

        private void LlenarCmbVentasDelAño()
        {
            DataTable dt = null;
            try
            {
                MDIPrincipal.ActualizarBarraDeEstado(Utils.clbdd);
                dt = new GraficaRepository(cnStr).ObtenerAñosDePedidos();
                foreach (DataRow row in dt.Rows)
                    CmbVentasDelAño.Items.Add(Convert.ToInt32(row["YearOrderDate"]));
                MDIPrincipal.ActualizarBarraDeEstado();
            }
            catch (Exception ex)
            {
                Utils.MsgCatchOue(ex);
            }
            CmbVentasDelAño.SelectedIndex = 0;
        }

        private void CmbVentasDelAño_SelectedIndexChanged(object sender, EventArgs e)
        {
            LlenarGrafico(Convert.ToInt32(CmbVentasDelAño.Text.ToString()));
        }

        private void LlenarGrafico(int year) 
        {
            groupBox1.Text = $"» Reporte gráfico comparativo de ventas mensuales por vendedores del año {year} «";
            DataTable dt = null;
            try
            {
                MDIPrincipal.ActualizarBarraDeEstado(Utils.clbdd);
                dt = new GraficaRepository(cnStr).ObtenerVentasMensualesPorVendedorRpt(year);
                MDIPrincipal.ActualizarBarraDeEstado();
            }
            catch (Exception ex)
            {
                Utils.MsgCatchOue(ex);
                return;
            }
            reportViewer1.LocalReport.DataSources.Clear();
            var rds = new ReportDataSource("DataSet1", dt);
            reportViewer1.LocalReport.DataSources.Add(rds);
            reportViewer1.LocalReport.SetParameters(new ReportParameter("Subtitulo", $"Ventas mensuales por vendedores del año {year}"));
            reportViewer1.LocalReport.SetParameters(new ReportParameter("Anio", year.ToString()));
            reportViewer1.RefreshReport();
        }
    }
}
