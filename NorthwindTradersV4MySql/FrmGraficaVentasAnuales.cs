using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace NorthwindTradersV4MySql
{
    public partial class FrmGraficaVentasAnuales : Form
    {
        private readonly string cnStr = ConfigurationManager.ConnectionStrings["NorthwindMySql"].ConnectionString;

        public FrmGraficaVentasAnuales()
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
        }

        private void GrbPaint(object sender, PaintEventArgs e) => Utils.GrbPaint(this, sender, e);

        private void FrmGraficaVentasAnuales_Load(object sender, EventArgs e)
        {
            LlenarComboBox();
            GroupBox.Text = $"» Comparativo de ventas mensuales de los últimos 2 años «";
            CargarComparativoVentasMensuales(2);
        }

        private void btnMostrar_Click(object sender, EventArgs e)
        {
            if (ComboBox.SelectedIndex == 0)
            {
                Utils.MensajeExclamation("Seleccione un número de años válido.");
                return;
            }
            if (Convert.ToInt32(ComboBox.SelectedValue) >= 6)
            {
                Utils.MensajeExclamation("Solo existen datos en la base de datos hasta el año 1996");
                return;
            }
            CargarComparativoVentasMensuales(Convert.ToInt32(ComboBox.SelectedValue));
        }

        private void LlenarComboBox()
        {
            var items = new List<KeyValuePair<string, int>>();
            items.Add(new KeyValuePair<string, int>("»--- Seleccione ---«", 0));
            for (int i = 2; i <= 10; i++)
            {
                items.Add(new KeyValuePair<string, int>($"{i} Años ", i));
            }
            ComboBox.DataSource = items;
            ComboBox.DisplayMember = "Key";
            ComboBox.ValueMember = "Value";
        }

        private void CargarComparativoVentasMensuales(int years)
        {
            ChartVentasAnuales.Series.Clear();
            ChartVentasAnuales.Titles.Clear(); // Limpiar títulos previos
            ChartVentasAnuales.Legends.Clear(); // Limpiar leyendas previas

            var legend = new Legend("Default")
            {
                Docking = Docking.Top,
                Alignment = StringAlignment.Center,
                Font = new Font("Arial", 10, FontStyle.Regular)
            };
            ChartVentasAnuales.Legends.Add(legend);

            int yearActual = DateTime.Now.Year;
            for (int i = 1; i <= years; i++)
            {   
                if (yearActual == 2023)
                    yearActual = 1998; // Si el año actual es 2023, se inicia desde 1998
                else if (yearActual == 1995)
                    break;
                var datos = new List<DtoVentasMensuales>();
                try
                {
                    MDIPrincipal.ActualizarBarraDeEstado(Utils.clbdd);
                    datos = new GraficaRepository(cnStr).ObtenerVentasMensuales(yearActual);
                    MDIPrincipal.ActualizarBarraDeEstado();
                }
                catch (Exception ex)
                {
                    Utils.MsgCatchOue(ex);
                    return;
                }
                decimal totalAnual = datos.Sum(d => d.Total);
                ChartVentasAnuales.Series.Add($"Ventas {yearActual}");
                string nombreSerie = $"Ventas {yearActual}"; // Nombre de la serie para la leyenda
                ChartVentasAnuales.Series[$"Ventas {yearActual}"].ChartType = SeriesChartType.Line;
                ChartVentasAnuales.Series[$"Ventas {yearActual}"].IsValueShownAsLabel = false;
                ChartVentasAnuales.Series[$"Ventas {yearActual}"].Label = "#VALY{C}"; 
                ChartVentasAnuales.Series[$"Ventas {yearActual}"].BorderWidth = 2;
                ChartVentasAnuales.Series[$"Ventas {yearActual}"].ToolTip = $"{nombreSerie} #VALX: #VALY{{C2}}";
                ChartVentasAnuales.Series[$"Ventas {yearActual}"].Legend = legend.Name; // Asignar leyenda a la serie
                ChartVentasAnuales.Series[$"Ventas {yearActual}"].LegendText = $"{nombreSerie} (Total: {totalAnual:C2})";

                foreach (var dato in datos)
                {
                    string nombreMes = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(dato.Mes);
                    int index = ChartVentasAnuales.Series[$"Ventas {yearActual}"].Points.AddXY(nombreMes, dato.Total);
                    DataPoint dataPoint = ChartVentasAnuales.Series[$"Ventas {yearActual}"].Points[index];
                    if (dato.Total != 0)
                    {
                        dataPoint.Label = $"${dato.Total:#,##0.00}";
                        dataPoint.MarkerStyle = MarkerStyle.Circle; // Estilo de marcador
                        dataPoint.MarkerSize = 10;
                    }
                    else
                        dataPoint.Label = "";
                }
                yearActual--;
            }


            var area = ChartVentasAnuales.ChartAreas[0];
            area.AxisX.Interval = 1;
            area.AxisX.LabelStyle.Angle = -45;
            area.AxisX.Title = "Meses";
            area.AxisX.MajorGrid.Enabled = true;
            area.AxisX.MajorGrid.LineColor = Color.LightGray;
            area.AxisX.MajorGrid.LineDashStyle = ChartDashStyle.Dash;

            area.AxisY.LabelStyle.Format = "C0";
            area.AxisY.LabelStyle.Angle = -45;
            area.AxisY.Title = "Ventas Totales";
            area.AxisY.MinorGrid.Enabled = true;
            area.AxisY.MinorGrid.LineColor = Color.LightGray;
            area.AxisY.MinorGrid.LineDashStyle = ChartDashStyle.Dash;

            // Crear el título
            Title titulo = new Title();
            if (ComboBox.SelectedIndex <= 0 & ComboBox.SelectedIndex < 1)
            {
                titulo.Text = "» Comparativo de ventas mensuales de los últimos 2 años «";
                GroupBox.Text = "» Comparativo de ventas mensuales de los últimos 2 años «";
            }
            else
            {
                titulo.Text = $"» Comparativo de ventas mensuales de los últimos {ComboBox.Text} «";
                GroupBox.Text = $"» Comparativo de ventas mensuales de los últimos {ComboBox.Text} «";
            }
            titulo.Font = new Font("Arial", 14, FontStyle.Bold);
            titulo.ForeColor = Color.DarkBlue;
            titulo.Alignment = ContentAlignment.TopCenter;

            // Agregar el título al chart
            ChartVentasAnuales.Titles.Add(titulo);
        }
    }
}
