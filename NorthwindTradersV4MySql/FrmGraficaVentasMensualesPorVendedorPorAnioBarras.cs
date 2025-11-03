using System;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace NorthwindTradersV4MySql
{
    public partial class FrmGraficaVentasMensualesPorVendedorPorAnioBarras : Form
    {

        private readonly string cnStr = ConfigurationManager.ConnectionStrings["NorthwindMySql"].ConnectionString;

        public FrmGraficaVentasMensualesPorVendedorPorAnioBarras()
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
        }

        private void GrbPaint(object sender, PaintEventArgs e) => Utils.GrbPaint(this, sender, e);

        private void FrmGraficaVentasMensualesPorVendedorPorAnioBarras_Load(object sender, EventArgs e)
        {
            LlenarCmbVentasDelAño();
        }

        private void LlenarCmbVentasDelAño()
        {
            MDIPrincipal.ActualizarBarraDeEstado(Utils.clbdd);
            try
            {
                var dt = new GraficaRepository(cnStr).ObtenerAñosDePedidos();
                foreach (DataRow row in dt.Rows)
                    CmbVentasDelAño.Items.Add(Convert.ToInt32(row["YearOrderDate"]));
            }
            catch (Exception ex)
            {
                Utils.MsgCatchOue(ex);
            }
            MDIPrincipal.ActualizarBarraDeEstado();
            CmbVentasDelAño.SelectedIndex = 0;
        }

        private void CmbVentasDelAño_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarGrafica(Convert.ToInt32(CmbVentasDelAño.SelectedItem));
        }

        private void CargarGrafica(int anio)
        {
            chart1.Series.Clear();
            chart1.Titles.Clear();

            Title titulo = new Title()
            {
                Text = $"Ventas mensuales por vendedores del año {anio}",
                Font = new Font("Arial", 16, FontStyle.Bold)
            };
            chart1.Titles.Add(titulo);
            groupBox1.Text = $"» {titulo.Text} «";

            var area = chart1.ChartAreas[0];
            area.AxisX.Interval = 1;
            area.AxisX.LabelStyle.Angle = -45;
            area.AxisX.CustomLabels.Clear();
            area.AxisX.MajorGrid.Enabled = true;
            area.AxisX.MajorGrid.LineColor = Color.LightGray;
            area.AxisX.MajorGrid.LineDashStyle = ChartDashStyle.Dash;
            area.AxisX.LabelStyle.Font = new Font("Segoe UI", 8, FontStyle.Regular);
            area.AxisX.Title = "Meses";

            area.AxisY.Title = "Ventas totales";
            area.AxisY.LabelStyle.Format = "C0";
            area.AxisY.MajorGrid.Enabled = true;
            area.AxisY.MajorGrid.LineColor = Color.Gray;
            area.AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Solid;
            area.AxisY.MinorGrid.Enabled = true;
            area.AxisY.MinorGrid.LineColor = Color.LightGray;
            area.AxisY.MinorGrid.LineDashStyle = ChartDashStyle.Dash;
            area.AxisY.LabelStyle.Font = new Font("Segoe UI", 8, FontStyle.Regular);
            area.AxisY.LabelStyle.Angle = -45;

            // Aquí habilitas 3D
            area.Area3DStyle.Enable3D = true;
            area.Area3DStyle.Inclination = 30;
            area.Area3DStyle.Rotation = 30;
            area.Area3DStyle.PointGapDepth = 25;
            area.Area3DStyle.WallWidth = 0;
            area.Area3DStyle.LightStyle = LightStyle.Realistic;

            DataTable dt = null;

            try
            {
                MDIPrincipal.ActualizarBarraDeEstado(Utils.clbdd);
                dt = new GraficaRepository(cnStr).ObtenerVentasMensualesPorVendedor(anio);
            }
            catch (Exception ex)
            {
                Utils.MsgCatchOue(ex);
                return;
            }

            var grupos = dt.AsEnumerable().GroupBy(row => row.Field<string>("Vendedor"));

            var mesesAbrev = new[] { "Ene.", "Feb.", "Mar.", "Abr.", "May.", "Jun.", "Jul.", "Ago.", "Sep.", "Oct.", "Nov.", "Dic." };

            foreach (var grupo in grupos)
            {
                var serie = new Series(grupo.Key)
                {
                    ChartType = SeriesChartType.Column,
                    IsValueShownAsLabel = false,
                    Font = new Font("Segoe UI", 8, FontStyle.Regular),
                    ToolTip = "#SERIESNAME\nMes: #AXISLABEL\nVentas: #VALY{C2}",
                    LabelFormat = "C2"
                };
                for (int mes = 1; mes <= 12; mes++)
                    serie.Points.AddXY(mesesAbrev[mes - 1], 0D);
                foreach (var row in grupo)
                {
                    int mes = row.Field<int>("Mes");
                    object raw = row["TotalVentas"];
                    double ventas = raw != DBNull.Value ? Convert.ToDouble(raw) : 0D;
                    serie.Points[mes - 1].YValues[0] = ventas;
                }
                // filtro para mostrar etiqueta solo si Y > 0
                foreach (DataPoint p in serie.Points)
                {
                    if (p.YValues[0] > 0)
                    {
                        p.IsValueShownAsLabel = true;
                        p.LabelForeColor = Color.Black; // Color de la etiqueta
                        p.Font = new Font("Segoe UI", 8, FontStyle.Regular); // Fuente de la etiqueta
                    }
                }
                // Sumar todos los valores Y de la serie
                double totalVendedor = serie.Points.Sum(p => p.YValues[0]);
                serie.LegendText = $"{serie.Name} (Total: {totalVendedor:C2})";
                chart1.Series.Add(serie);
            }
            Title subTitulo = new Title
            {
                Text = $"Total de ventas del año: {dt.Compute("SUM(TotalVentas)", string.Empty):C2}",
                Font = new Font("Arial", 8, FontStyle.Bold),
                Alignment = ContentAlignment.TopLeft,
                IsDockedInsideChartArea = false,
                DockingOffset = -5
            };
            chart1.Titles.Add(subTitulo);
            chart1.ResetAutoValues();
        }
    }
}
