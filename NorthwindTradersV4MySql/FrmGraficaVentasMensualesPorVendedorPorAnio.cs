using System;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace NorthwindTradersV4MySql
{
    public partial class FrmGraficaVentasMensualesPorVendedorPorAnio : Form
    {

        private readonly string cnStr = ConfigurationManager.ConnectionStrings["NorthwindMySql"].ConnectionString;

        public FrmGraficaVentasMensualesPorVendedorPorAnio()
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
        }

        private void GrbPaint(object sender, PaintEventArgs e) => Utils.GrbPaint(this, sender, e);

        private void FrmGraficaVentasMensualesPorVendedorPorAnio_Load(object sender, EventArgs e)
        {
            LlenarComboBox();
            CargarVentasMensualesPorVendedorPorAnio(DateTime.Now.Year);
        }

        private void btnMostrar_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)
            {
                Utils.MensajeExclamation("Seleccione un año válido.");
                return;
            }
            CargarVentasMensualesPorVendedorPorAnio(Convert.ToInt32(comboBox1.SelectedItem));
        }

        private void LlenarComboBox()
        {
            MDIPrincipal.ActualizarBarraDeEstado(Utils.clbdd);
            comboBox1.Items.Add("»--- Seleccione ---«");
            try
            {
                var dt = new GraficaRepository(cnStr).ObtenerAñosDePedidos();
                foreach (DataRow row in dt.Rows)
                    comboBox1.Items.Add(Convert.ToInt32(row["YearOrderDate"]));
                comboBox1.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                Utils.MsgCatchOue(ex);
            }
            MDIPrincipal.ActualizarBarraDeEstado();
        }

        private void CargarVentasMensualesPorVendedorPorAnio(int anio)
        {
            chart1.Series.Clear();
            chart1.Titles.Clear();
            Title titulo = new Title
            {
                Text = $"Ventas mensuales por vendedores del año {anio}",
                Font = new Font("Arial", 16, FontStyle.Bold)
            };
            chart1.Titles.Add(titulo);
            groupBox1.Text = $"» {titulo.Text} «";
            // ChartArea
            var area = chart1.ChartAreas[0];
            area.AxisX.Interval = 1;
            area.AxisX.CustomLabels.Clear();
            area.AxisX.MajorGrid.Enabled = true;
            area.AxisX.MajorGrid.LineColor = Color.LightGray;
            area.AxisX.MajorGrid.LineDashStyle = ChartDashStyle.Dash;
            area.AxisX.LabelStyle.Angle = -45;
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
            // Leer datos
            var dt = new DataTable();
            try
            {
                MDIPrincipal.ActualizarBarraDeEstado(Utils.clbdd);
                dt = new GraficaRepository(cnStr).ObtenerVentasMensualesPorVendedorPorAño(anio);
                MDIPrincipal.ActualizarBarraDeEstado();
            }
            catch (Exception ex)
            {
                Utils.MsgCatchOue(ex);
                return;
            }

            // Pivot dinámico por vendedor
            var grupos = dt.AsEnumerable()
                          .GroupBy(r => r.Field<string>("Vendedor"));

            // Nombres abreviados de mes (12 elementos)
            var mesesAbrev = CultureInfo.CurrentCulture
                                        .DateTimeFormat
                                        .AbbreviatedMonthNames
                                        .Take(12)
                                        .ToArray();
            foreach (var grupo in grupos)
            {
                // Serie por vendedor
                var serie = new Series(grupo.Key)
                {
                    ChartType = SeriesChartType.Line,
                    BorderWidth = 2,
                    MarkerStyle = MarkerStyle.Circle,
                    MarkerSize = 6,
                    ToolTip = "#SERIESNAME\nMes: #AXISLABEL\nVentas: #VALY{C2}",
                    LabelForeColor = Color.Black,
                    Font = new Font("Segoe UI", 8f, FontStyle.Regular),
                    IsValueShownAsLabel = false,
                    LabelFormat = "C2"
                };

                for (int mes = 1; mes <= 12; mes++)
                {
                    string nombreMes = mesesAbrev[mes - 1];
                    serie.Points.AddXY(nombreMes, 0D);
                }
                // Llenamos datos reales
                foreach (var row in grupo)
                {
                    // 1) Obtienes el mes
                    int mes = row.Field<int>("Mes");       // 1–12

                    // 2) Tomas el valor crudo y lo conviertes a double
                    object raw = row["TotalVentas"];
                    double ventas = raw != DBNull.Value
                                    ? Convert.ToDouble(raw)
                                    : 0D;

                    // 3) Asignas el valor al punto correspondiente
                    serie.Points[mes - 1].YValues[0] = ventas;
                }

                // filtro para mostrar etiqueta solo si Y > 0
                foreach (DataPoint p in serie.Points)
                {
                    if (p.YValues[0] > 0)
                    {
                        p.IsValueShownAsLabel = true;                        
                    }
                }
                // Sumar todos los valores Y de la serie
                double totalVendedor = serie.Points.Sum(p => p.YValues[0]);

                serie.LegendText = $"{serie.Name} (Total: {totalVendedor:C2})";

                chart1.Series.Add(serie);
            }
            Title subTitulo = new Title();
            subTitulo.Text = $"Total de ventas del año: {dt.Compute("SUM(TotalVentas)", string.Empty):C2}";
            subTitulo.Font = new Font("Arial", 8, FontStyle.Bold);
            subTitulo.Alignment = ContentAlignment.TopLeft;
            subTitulo.IsDockedInsideChartArea = false;
            subTitulo.DockingOffset = -5;
            chart1.Titles.Add(subTitulo);
            // ————— Aquí forzamos el recálculo de la escala del eje Y —————
            chart1.ResetAutoValues();
        }
    }
}
