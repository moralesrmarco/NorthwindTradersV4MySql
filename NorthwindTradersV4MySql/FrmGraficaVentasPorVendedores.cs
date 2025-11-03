using System;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace NorthwindTradersV4MySql
{
    public partial class FrmGraficaVentasPorVendedores : Form
    {
        private readonly string cnStr = ConfigurationManager.ConnectionStrings["NorthwindMySql"].ConnectionString;

        public FrmGraficaVentasPorVendedores()
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
        }

        private void GrbPaint(object sender, PaintEventArgs e) => Utils.GrbPaint(this, sender, e);

        private void FrmGraficaVentasPorVendedores_Load(object sender, EventArgs e)
        {
            CargarVentasPorVendedores();
        }

        private void CargarVentasPorVendedores()
        {
            ChartVentasPorVendedores.Series.Clear();
            ChartVentasPorVendedores.Titles.Clear();
            // Título del gráfico
            Title titulo = new Title
            {
                Text = "Gráfica ventas por vendedores de todos los años",
                Font = new Font("Arial", 16, FontStyle.Bold)
            };
            ChartVentasPorVendedores.Titles.Add(titulo);
            // Configuración de la serie
            Series serie = new Series
            {
                Name = "Ventas",
                Color = Color.FromArgb(0, 51, 102),
                IsValueShownAsLabel = true,
                ChartType = SeriesChartType.Doughnut,
                Label = "#VALX: #VALY{C2}"
            };
            serie["PieLabelStyle"] = "Outside";
            serie.SmartLabelStyle.Enabled = true;
            serie.SmartLabelStyle.AllowOutsidePlotArea = LabelOutsidePlotAreaStyle.Yes;
            serie.SmartLabelStyle.CalloutLineColor = Color.Black;
            serie.LabelForeColor = Color.DarkSlateGray;
            serie.LabelBackColor = Color.WhiteSmoke;
            ChartVentasPorVendedores.Series.Add(serie);
            try
            {
                var ventas = new GraficaRepository(cnStr).ObtenerVentasPorVendedores();
                foreach (var (vendedor, totalVentas) in ventas)
                {
                    serie.Points.AddXY(vendedor, totalVentas);
                }
            }
            catch (Exception ex)
            {
                Utils.MsgCatchOue(ex);
            }
            Title subTitulo = new Title
            {
                Text = $"Total de ventas: {serie.Points.Sum(pt => pt.YValues[0]):C2}",
                Docking = Docking.Top,
                Font = new Font("Arial", 8, FontStyle.Bold),
                ForeColor = Color.Black,
                IsDockedInsideChartArea = false,
                Alignment = ContentAlignment.TopLeft,
                DockingOffset = -3 
            };
            ChartVentasPorVendedores.Titles.Add(subTitulo);
        }
    }
}
