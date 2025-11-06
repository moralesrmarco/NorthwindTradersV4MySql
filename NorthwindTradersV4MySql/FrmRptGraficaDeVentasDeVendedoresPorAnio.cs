using Microsoft.Reporting.WinForms;
using System;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace NorthwindTradersV4MySql
{
    public partial class FrmRptGraficaDeVentasDeVendedoresPorAnio : Form
    {

        private readonly string cnStr = ConfigurationManager.ConnectionStrings["NorthwindMySql"].ConnectionString;

        public FrmRptGraficaDeVentasDeVendedoresPorAnio()
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
        }

        private void GrbPaint(object sender, PaintEventArgs e) => Utils.GrbPaint(this, sender, e);

        private void FrmRptGraficaDeVentasDeVendedoresPorAnio_Load(object sender, EventArgs e)
        {
            LlenarCmbVentas();
        }

        private void LlenarCmbVentas()
        {
            try
            {
                MDIPrincipal.ActualizarBarraDeEstado(Utils.clbdd);
                var dt = new GraficaRepository(cnStr).ObtenerAñosDePedidos();
                foreach (DataRow row in dt.Rows)
                    CmbVentas.Items.Add(Convert.ToInt32(row["YearOrderDate"]));
                MDIPrincipal.ActualizarBarraDeEstado();
            }
            catch (Exception ex)
            {
                Utils.MsgCatchOue(ex);
            }
            CmbVentas.SelectedIndex = 0; // Seleccionar el primer elemento por defecto
        }

        private void CmbVentas_SelectedIndexChanged(object sender, EventArgs e)
        {
            LlenarGrafico(Convert.ToInt32(CmbVentas.Text.ToString()));
        }

        private void LlenarGrafico(int year)
        {
            groupBox1.Text = $"» Reporte gráfico ventas por vendedores del año {year} «";
            DataTable dt = null;
            try
            {
                MDIPrincipal.ActualizarBarraDeEstado(Utils.clbdd);
                var datos = new GraficaRepository(cnStr).ObtenerVentasPorVendedor(year);
                var dtos = datos.Select(x => new DtoVendedorTotalVentas
                {
                    Vendedor = x.Vendedor,
                    TotalVentas = x.TotalVentas
                }).ToList();
                dt = ReportDataTableAdapter.ConvertirVendedorTotalVentas(dtos);
                MDIPrincipal.ActualizarBarraDeEstado();
            }
            catch (Exception ex)
            {
                Utils.MsgCatchOue(ex);
            }
            reportViewer1.LocalReport.DataSources.Clear();
            var rds = new ReportDataSource("DataSet1", dt);
            reportViewer1.LocalReport.DataSources.Add(rds);
            reportViewer1.LocalReport.SetParameters(new ReportParameter("Subtitulo", $"Ventas por vendedores del año {year}"));
            reportViewer1.LocalReport.SetParameters(new ReportParameter("Anio", year.ToString()));
            reportViewer1.RefreshReport();
        }
    }
}
