using Microsoft.Reporting.WinForms;
using System;
using System.Configuration;
using System.Data;
using System.Windows.Forms;

namespace NorthwindTradersV4MySql
{

    public partial class FrmRptGraficaVentasMensuales : Form
    {

        private readonly string cnStr = ConfigurationManager.ConnectionStrings["NorthwindMySql"].ConnectionString;

        public FrmRptGraficaVentasMensuales()
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
        }

        private void GrbPaint(object sender, PaintEventArgs e) => Utils.GrbPaint(this, sender, e);

        private void FrmRptGraficaVentasMensuales_Load(object sender, EventArgs e)
        {
            LlenarCmbVentasMensualesDelAño();
        }

        private void LlenarCmbVentasMensualesDelAño()
        {
            MDIPrincipal.ActualizarBarraDeEstado(Utils.clbdd);
            try
            {
                var dt = new GraficaRepository(cnStr).ObtenerAñosDePedidos();
                foreach (DataRow row in dt.Rows)
                    CmbVentasMensualesDelAño.Items.Add(Convert.ToInt32(row["YearOrderDate"]));
                CmbVentasMensualesDelAño.SelectedIndex = 0; // Selecciona el primer elemento
            }
            catch (Exception ex)
            {
                Utils.MsgCatchOue(ex);
            }
            MDIPrincipal.ActualizarBarraDeEstado();
        }

        private void CmbVentasMensualesDelAño_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt = null;
            try
            {
                MDIPrincipal.ActualizarBarraDeEstado(Utils.clbdd);
                dt = ReportDataTableAdapter.ConvertirVentasMensuales(new GraficaRepository(cnStr).ObtenerVentasMensualesPorAnio(Convert.ToInt32(CmbVentasMensualesDelAño.SelectedItem)));
                MDIPrincipal.ActualizarBarraDeEstado();
            }
            catch (Exception ex)
            {
                Utils.MsgCatchOue(ex);
            }
            if (dt != null)
            {
                groupBox1.Text = $"» Reporte gráfico de ventas mensuales del año {CmbVentasMensualesDelAño.SelectedItem} «";
                // 1. Limpia fuentes previas
                reportViewer1.LocalReport.DataSources.Clear();
                // 2. Usa el nombre EXACTO del DataSet del RDLC
                var rds = new ReportDataSource("DataSet1", dt);
                reportViewer1.LocalReport.DataSources.Add(rds);
                reportViewer1.LocalReport.SetParameters(new ReportParameter("Anio", CmbVentasMensualesDelAño.SelectedItem.ToString()));
                reportViewer1.LocalReport.SetParameters(new ReportParameter("Subtitulo", $"Ventas mensuales del año {CmbVentasMensualesDelAño.SelectedItem}"));
                // 3. Refresca el reporte
                reportViewer1.RefreshReport();
            }
        }
    }
}
