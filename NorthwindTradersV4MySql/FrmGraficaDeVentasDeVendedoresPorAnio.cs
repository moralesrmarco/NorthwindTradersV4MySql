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
    public partial class FrmGraficaDeVentasDeVendedoresPorAnio : Form
    {

        private readonly string cnStr = ConfigurationManager.ConnectionStrings["NorthwindMySql"].ConnectionString;

        public FrmGraficaDeVentasDeVendedoresPorAnio()
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
        }

        private void GrbPaint(object sender, PaintEventArgs e) => Utils.GrbPaint(this, sender, e);

        private void FrmGraficaDeVentasDeVendedoresPorAnio_Load(object sender, EventArgs e)
        {
            LlenarComboBox();
            CargarVentasPorVendedores(DateTime.Now.Year); // Cargar ventas del año actual por defecto
        }

        private void btnMostrar_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)
            {
                Utils.MensajeExclamation("Seleccione un año válido.");
                return;
            }
            CargarVentasPorVendedores(Convert.ToInt32(comboBox1.SelectedItem));
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

        private void CargarVentasPorVendedores(int anio)
        {
            chart1.Series.Clear();
            chart1.Titles.Clear();
            chart1.Legends.Clear();

            var leyenda = new Legend("Vendedores")
            {
                Title = "Vendedores",
                TitleFont = new Font("Arial", 10, FontStyle.Bold),
                Docking = Docking.Right,
                LegendStyle = LegendStyle.Table
            };
            chart1.Legends.Add(leyenda);

            // Título del gráfico
            Title titulo = new Title
            {
                Text = $"» Gráfica de ventas por vendedores del año {anio} «",
                Font = new Font("Arial", 16, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 51, 102)
            };
            chart1.Titles.Add(titulo);
            groupBox1.Text = titulo.Text; // Actualizar el texto del GroupBox
            // Configuración de la serie
            Series serie = new Series
            {
                Name = "Ventas",
                Color = Color.FromArgb(0, 51, 102),
                IsValueShownAsLabel = true,
                ChartType = SeriesChartType.Doughnut,
                Label = "#AXISLABEL: #VALY{C2}",
                ToolTip = "Vendedor: #AXISLABEL\nTotal ventas: #VALY{C2}",
                Legend = leyenda.Name,
                LegendText = "#AXISLABEL: #VALY{C2}"
            };
            // 1. Configurar ChartArea 3D
            var area = chart1.ChartAreas[0];
            area.Area3DStyle.Enable3D = true;
            area.Area3DStyle.Inclination = 40;
            area.Area3DStyle.Rotation = 60;
            area.Area3DStyle.LightStyle = LightStyle.Realistic;
            area.Area3DStyle.WallWidth = 0;

            serie["PieLabelStyle"] = "Outside";
            serie["PieDrawingStyle"] = "Cylinder";
            serie["DoughnutRadius"] = "60";
            chart1.Series.Add(serie);
            // Consulta SQL para obtener las ventas por vendedor del año seleccionado
            try
            {
                MDIPrincipal.ActualizarBarraDeEstado(Utils.clbdd);
                var ventas = new GraficaRepository(cnStr).ObtenerVentasPorVendedor(anio);
                foreach (var (vendedor, totalVentas) in ventas)
                {
                    int idx = serie.Points.AddXY(vendedor, totalVentas);
                    serie.Points[idx].LegendText = string.Format(
                    CultureInfo.GetCultureInfo("es-MX"),
                    "{0}: {1:C2}",
                    vendedor,
                    totalVentas
                    );
                }
            }
            catch (Exception ex)
            {
                Utils.MsgCatchOue(ex);
            }
            Title subTitulo = new Title
            {
                Text = $"Total de ventas del año {anio}: {serie.Points.Sum(p => p.YValues[0]):C2}",
                Docking = Docking.Top,
                Font = new Font("Arial", 8, FontStyle.Bold),
                IsDockedInsideChartArea = false,
                Alignment = ContentAlignment.TopLeft,
                DockingOffset = -3
            };
            // Agregar el subtítulo al chart
            chart1.Titles.Add(subTitulo);
            MDIPrincipal.ActualizarBarraDeEstado();
        }
    }
}
