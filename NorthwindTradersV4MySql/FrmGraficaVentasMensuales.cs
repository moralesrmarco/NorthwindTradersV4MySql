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
    public partial class FrmGraficaVentasMensuales : Form
    {
        private readonly string cnStr = ConfigurationManager.ConnectionStrings["NorthwindMySql"].ConnectionString;

        public FrmGraficaVentasMensuales()
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
        }

        private void GrbPaint(object sender, PaintEventArgs e) => Utils.GrbPaint(this, sender, e);

        private void FrmGraficaVentasMensuales_Load(object sender, EventArgs e)
        {
            LlenarComboBox();
            CargarVentasMensuales(DateTime.Now.Year);
        }

        private void btnMostrar_Click(object sender, EventArgs e)
        {
            if (ComboBox.SelectedIndex == 0)
            {
                Utils.MensajeExclamation("Seleccione un año válido.");
                return;
            }
            CargarVentasMensuales(Convert.ToInt32(ComboBox.SelectedItem));
        }

        private void LlenarComboBox()
        {
            MDIPrincipal.ActualizarBarraDeEstado(Utils.clbdd);
            try
            {
                var dt = new GraficaRepository(cnStr).ObtenerAñosDePedidos();
                ComboBox.Items.Add("»--- Seleccione ---«");
                foreach (DataRow row in dt.Rows)
                    ComboBox.Items.Add(Convert.ToInt32(row["YearOrderDate"]));
                ComboBox.SelectedIndex = 0; // Selecciona el primer elemento
            }
            catch (Exception ex)
            {
                Utils.MsgCatchOue(ex);
            }
            MDIPrincipal.ActualizarBarraDeEstado();
        }

        private void CargarVentasMensuales(int year)
        {
            var datos = Enumerable.Empty<DtoVentasMensuales>();
            try
            {
                MDIPrincipal.ActualizarBarraDeEstado(Utils.clbdd);
                // 1. Obtiene los datos ADO.NET
                datos = new GraficaRepository(cnStr).ObtenerVentasMensuales(year);
                MDIPrincipal.ActualizarBarraDeEstado();
            }
            catch (Exception ex)
            {
                Utils.MsgCatchOue(ex);
                return;
            }
            // 2. Prepara la serie del Chart
            var serie = chartVentas.Series["Ventas mensuales"];
            serie.Points.Clear();
            serie.ChartType = SeriesChartType.Line;
            serie.BorderWidth = 3;
            serie.ToolTip = "Ventas de #VALX: #VALY{C2}";
            serie.IsValueShownAsLabel = true;
            serie.LabelFormat = "C2"; // Formato de moneda con 2 decimales
            serie.MarkerStyle = MarkerStyle.Circle;
            serie.MarkerSize = 10;
            // 3. Agrega puntos al gráfico
            foreach (var punto in datos)
            {
                string nombreMes = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(punto.Mes);
                serie.Points.AddXY(nombreMes, punto.Total);
            }
            var area = chartVentas.ChartAreas[0];

            // PRIMERO: forzar cada mes
            area.AxisX.Interval = 1;
            // LUEGO: asignar formato al label
            area.AxisX.LabelStyle.Angle = -45;
            area.AxisX.Title = "Meses";
            area.AxisX.MajorGrid.Enabled = true;
            area.AxisX.MajorGrid.LineColor = Color.LightGray;
            area.AxisX.MajorGrid.LineDashStyle = ChartDashStyle.Dash;

            area.AxisY.LabelStyle.Format = "C0";
            area.AxisY.Title = "Ventas Totales";
            area.AxisY.MajorGrid.Enabled = true;
            area.AxisY.MajorGrid.LineColor = Color.Gray;
            area.AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Solid;
            area.AxisY.MinorGrid.Enabled = true;
            area.AxisY.MinorGrid.LineColor = Color.LightGray;
            area.AxisY.MinorGrid.LineDashStyle = ChartDashStyle.Dash;

            chartVentas.Legends[0].Enabled = false;

            // Crear el título
            Title titulo = new Title();
            titulo.Text = $"Ventas mensuales del año: {year}";
            titulo.Font = new Font("Arial", 14, FontStyle.Bold);
            titulo.Alignment = ContentAlignment.TopCenter;

            decimal totalVentas = datos.Sum(x => x.Total);
            Title subTitulo = new Title();
            subTitulo.Text = $"Total de ventas del año {year}: {totalVentas:C2}";
            subTitulo.Docking = Docking.Top;
            subTitulo.Font = new Font("Arial", 8, FontStyle.Bold);
            subTitulo.Alignment = ContentAlignment.TopRight;
            //subTitulo.DockingOffset = 30;
            subTitulo.IsDockedInsideChartArea = false;

            // Agregar el título al chart
            chartVentas.Titles.Clear(); // Limpiar títulos previos
            chartVentas.Titles.Add(titulo);
            chartVentas.Titles.Add(subTitulo);

            GroupBox.Text = $"» Ventas mensuales del año: {year} «";
        }
    }
}
