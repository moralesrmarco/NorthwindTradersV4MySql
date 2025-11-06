using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace NorthwindTradersV4MySql
{
    public partial class FrmTableroControlVendedores : Form
    {

        private readonly string cnStr = ConfigurationManager.ConnectionStrings["NorthwindMySql"].ConnectionString;
        private readonly string[] categorias = { "Ene", "Feb", "Mar", "Abr", "May", "Jun", "Jul", "Ago", "Sep", "Oct", "Nov", "Dic"};
        private readonly double[] valores = { 15, 30, 45, 20, 35, 50, 25, 40, 45, 40, 30, 50 };

        public FrmTableroControlVendedores()
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
        }
        
        private void GrbPaint(object sender, PaintEventArgs e) => Utils.GrbPaint(this, sender, e);

        private void FrmTableroControlVendedores_Load(object sender, EventArgs e)
        {
            LlenarCmbVentasMensualesPorVendedorPorAño();
            LlenarCmbTipoGrafica1();
            CmbTipoGrafica1.SelectedItem = SeriesChartType.Line;

            LlenarCmbUltimosAnios();
            LLenarCmbTipoGrafica2();
            CmbTipoGrafica2.SelectedItem = SeriesChartType.Line;

            LlenarCmbNumeroProductos();
            LlenarCmbTipoGrafica3();
            CmbTipoGrafica3.SelectedItem = SeriesChartType.Column;

            CargarVentasPorVendedores();

            LlenarCmbVentasVendedorAño();
            LlenarCmbTipoGrafica5();
            CmbTipoGrafica5.SelectedItem = SeriesChartType.Doughnut;

            LlenarCmbTipoGrafica();
        }
        /******************************************************************************************************/
        private void LlenarCmbVentasMensualesPorVendedorPorAño()
        {
            cmbVentasMensualesPorVendedorPorAño.SelectedIndexChanged -= cmbVentasMensualesPorVendedorPorAño_SelectedIndexChanged;
            try
            {
                MDIPrincipal.ActualizarBarraDeEstado(Utils.clbdd);
                var dt = new GraficaRepository(cnStr).ObtenerAñosDePedidos();
                foreach (DataRow row in dt.Rows)
                    cmbVentasMensualesPorVendedorPorAño.Items.Add(Convert.ToInt32(row["YearOrderDate"]));
                MDIPrincipal.ActualizarBarraDeEstado();
                cmbVentasMensualesPorVendedorPorAño.SelectedIndex = 0; // Selecciona el primer elemento
            }
            catch (Exception ex)
            {
                Utils.MsgCatchOue(ex);
            }
            cmbVentasMensualesPorVendedorPorAño.SelectedIndexChanged += cmbVentasMensualesPorVendedorPorAño_SelectedIndexChanged;
        }

        private void LlenarCmbTipoGrafica1()
        {
            CmbTipoGrafica1.SelectedIndexChanged -= CmbTipoGrafica1_SelectedIndexChanged;
            CmbTipoGrafica1.DataSource = Enum.GetValues(typeof(SeriesChartType))
                .Cast<SeriesChartType>()
                .Where(t => t != SeriesChartType.Doughnut && t != SeriesChartType.ErrorBar && t != SeriesChartType.Funnel && t != SeriesChartType.Kagi && t != SeriesChartType.Pie && t != SeriesChartType.PointAndFigure && t != SeriesChartType.Polar && t != SeriesChartType.Pyramid && t != SeriesChartType.Renko && t != SeriesChartType.ThreeLineBreak) // Excluye tipos no deseados
                .OrderBy(t => t.ToString())
                .ToList();
            CmbTipoGrafica1.SelectedIndexChanged += CmbTipoGrafica1_SelectedIndexChanged;
        }

        private void cmbVentasMensualesPorVendedorPorAño_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarVentasMensualesPorVendedorPorAño(Convert.ToInt32(cmbVentasMensualesPorVendedorPorAño.SelectedItem), (SeriesChartType)CmbTipoGrafica1.SelectedItem);
        }

        private void CmbTipoGrafica1_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarVentasMensualesPorVendedorPorAño(Convert.ToInt32(cmbVentasMensualesPorVendedorPorAño.SelectedItem), (SeriesChartType)CmbTipoGrafica1.SelectedItem);
        }

        private void CargarVentasMensualesPorVendedorPorAño(int year, SeriesChartType tipoGrafica)
        {
            DataTable datos = null;
            try
            { 
                MDIPrincipal.ActualizarBarraDeEstado(Utils.clbdd);
                datos = new GraficaRepository(cnStr).ObtenerVentasMensualesPorVendedorPorAño(year);
                MDIPrincipal.ActualizarBarraDeEstado();
            }
            catch (Exception ex)
            {
                Utils.MsgCatchOue(ex);
            }
            if (datos == null || datos.Rows.Count == 0)
                return;
            chart1.Series.Clear();
            chart1.Titles.Clear(); // Limpiar títulos previos
            Title titulo = new Title
            {
                Text = $"Ventas mensuales por vendedor del año: {year}.\nTipo de gráfica: {tipoGrafica}.",
                Font = new Font("Segoe UI", 8, FontStyle.Bold)
            };
            chart1.Titles.Add(titulo);
            groupBox1.Text = $"» Ventas mensuales por vendedor del año: {year}. Tipo de gráfica: {tipoGrafica}. «";
            var area = chart1.ChartAreas[0];
            area.AxisX.Interval = 1;
            area.AxisX.CustomLabels.Clear();

            // Genera etiquetas para cada mes
            for (int i = 1; i <= 12; i++)
            {
                var label = new CustomLabel
                {
                    FromPosition = i - 0.5,
                    ToPosition = i + 0.5,
                    Text = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(i) // “Ene”, “Feb”, …
                };
                area.AxisX.CustomLabels.Add(label);
            }
            area.AxisX.Title = "Meses";
            area.AxisX.TitleFont = new Font("Segoe UI", 7, FontStyle.Bold);
            area.AxisX.LabelStyle.Font = new Font("Segoe UI", 7, FontStyle.Regular);
            area.AxisX.LabelStyle.Angle = -45;
            area.AxisY.Title = "Ventas totales";
            area.AxisY.TitleFont = new Font("Segoe UI", 7, FontStyle.Bold);
            area.AxisY.LabelStyle.Font = new Font("Segoe UI", 7, FontStyle.Regular);
            area.AxisY.LabelStyle.Format = "C0";
            area.AxisY.LabelStyle.Angle = -45;

            var grupos = datos.AsEnumerable()
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
                    ChartType = tipoGrafica,
                    BorderWidth = 2,
                    MarkerStyle = MarkerStyle.Circle,
                    ToolTip = "#SERIESNAME\nMes: #AXISLABEL\nVentas: #VALY{C2}"
                };

                // Inicializamos meses 1–12 en caso de faltantes
                // Inicializo 12 puntos con el nombre del mes como etiqueta X
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

                chart1.Series.Add(serie);
            }
            chart1.Legends[0].Font = new Font("Segoe UI", 7, FontStyle.Regular);
            // ————— Aquí forzamos el recálculo de la escala del eje Y —————
            chart1.ResetAutoValues();
        }
        /******************************************************************************************************/
        private void LlenarCmbUltimosAnios()
        {
            cmbUltimosAnios.SelectedIndexChanged -= cmbUltimosAnios_SelectedIndexChanged;
            var items = new List<KeyValuePair<string, int>>();
            for (int i = 2; i <= 10; i++)
            {
                items.Add(new KeyValuePair<string, int>($"{i} Años ", i));
            }
            cmbUltimosAnios.DataSource = items;
            cmbUltimosAnios.DisplayMember = "Key";
            cmbUltimosAnios.ValueMember = "Value";
            cmbUltimosAnios.SelectedIndex = 0; // Selecciona el primer elemento
            cmbUltimosAnios.SelectedIndexChanged += cmbUltimosAnios_SelectedIndexChanged;
        }

        private void LLenarCmbTipoGrafica2()
        {
            CmbTipoGrafica2.SelectedIndexChanged -= CmbTipoGrafica2_SelectedIndexChanged;
            CmbTipoGrafica2.DataSource = Enum.GetValues(typeof(SeriesChartType))
                .Cast<SeriesChartType>()
                .Where(t => t != SeriesChartType.Kagi && t != SeriesChartType.ErrorBar && t != SeriesChartType.PointAndFigure && t != SeriesChartType.Renko && t != SeriesChartType.StackedArea && t != SeriesChartType.StackedArea100 && t != SeriesChartType.StackedBar100 && t != SeriesChartType.StackedColumn100 && t != SeriesChartType.ThreeLineBreak) // Excluye tipos no deseados
                .OrderBy(t => t.ToString())
                .ToList();
            CmbTipoGrafica2.SelectedIndexChanged += CmbTipoGrafica2_SelectedIndexChanged;
        }

        private void cmbUltimosAnios_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt32(cmbUltimosAnios.SelectedValue) >= 6)
            {
                Utils.MensajeExclamation("Solo existen datos en la base de datos hasta el año 1996");
                return;
            }
            CargarComparativoVentasMensuales(Convert.ToInt32(cmbUltimosAnios.SelectedValue), (SeriesChartType)CmbTipoGrafica2.SelectedItem);
        }

        private void CmbTipoGrafica2_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarComparativoVentasMensuales(Convert.ToInt32(cmbUltimosAnios.SelectedValue), (SeriesChartType)CmbTipoGrafica2.SelectedItem);
        }

        private void CargarComparativoVentasMensuales(int years, SeriesChartType tipoGrafica)
        {
            chart2.Series.Clear();
            chart2.Titles.Clear(); // Limpiar títulos previos
            int yearActual = DateTime.Now.Year;
            for (int i = 1; i <= years; i++)
            {
                if (yearActual == 2023)
                    yearActual = 1998; // Si el año actual es 2023, se inicia desde 1998
                else if (yearActual == 1995)
                    break;
                chart2.Series.Add($"Ventas {yearActual}");
                chart2.Series[$"Ventas {yearActual}"].ChartType = tipoGrafica;
                chart2.Series[$"Ventas {yearActual}"].IsValueShownAsLabel = false;
                chart2.Series[$"Ventas {yearActual}"].Label = "#VALY{C}"; // Formato de moneda
                chart2.Series[$"Ventas {yearActual}"].BorderWidth = 2;
                chart2.Series[$"Ventas {yearActual}"].LegendText = $"Ventas {yearActual}"; // Leyenda personalizada
                chart2.Series[$"Ventas {yearActual}"].ToolTip = "#LEGENDTEXT\nde #AXISLABEL:\n#VALY{C2}"; // tooltip con moneda y 2 decimales

                chart2.Series[$"Ventas {yearActual}"].Points.Clear();
                // 2. Obtiene los datos
                List<DtoVentasMensuales> datos = null;
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
                if (chart2.Legends.Count > 0)
                    chart2.Legends[0].Font = new Font("Segoe UI", 7, FontStyle.Regular);
                // 3. Agrega los puntos al gráfico
                foreach (var dato in datos)
                {
                    string nombreMes = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(dato.Mes);
                    int index = chart2.Series[$"Ventas {yearActual}"].Points.AddXY(nombreMes, dato.Total);
                    DataPoint dataPoint = chart2.Series[$"Ventas {yearActual}"].Points[index];
                    if (dato.Total != 0)
                    {
                        dataPoint.Label = $"${dato.Total:#,##0.00}"; // Formato de moneda con 2 decimales
                        dataPoint.Font = new Font("Segoe UI", 7, FontStyle.Regular);
                        dataPoint.MarkerStyle = MarkerStyle.Circle; // Estilo de marcador
                        dataPoint.MarkerSize = 10;
                    }
                    else
                    {
                        dataPoint.Label = ""; // No mostrar etiqueta si el total es 0
                    }
                }
                yearActual--;
            }
            var area = chart2.ChartAreas[0];
            area.AxisX.Interval = 1;
            area.AxisX.LabelStyle.Angle = -45;
            area.AxisX.Title = "Meses";
            area.AxisX.TitleFont = new Font("Segoe UI", 7, FontStyle.Bold);
            area.AxisX.LabelStyle.Font = new Font("Segoe UI", 7, FontStyle.Regular);

            area.AxisX.MajorGrid.Enabled = true;
            area.AxisX.MajorGrid.LineColor = Color.LightGray;
            area.AxisX.MajorGrid.LineDashStyle = ChartDashStyle.Dash;

            area.AxisY.Title = "Ventas Totales";
            area.AxisY.LabelStyle.Format = "C0";
            area.AxisY.TitleFont = new Font("Segoe UI", 7, FontStyle.Bold);
            area.AxisY.LabelStyle.Font = new Font("Segoe UI", 7, FontStyle.Regular);
            area.AxisY.LabelStyle.Angle = -45;

            // Crear el título
            Title titulo = new Title
            {
                Text = $"Comparativo de ventas mensuales de los últimos {years} años.\nTipo de gráfica: {tipoGrafica}.",
                ForeColor = Color.Black,
                Font = new Font("Segoe UI", 8, FontStyle.Bold),
                Alignment = ContentAlignment.TopCenter
            };
            groupBox2.Text = $"» Comparativo de ventas mensuales de los últimos {years} años. Tipo de gráfica: {tipoGrafica}. «";
            // Agregar el título al chart
            chart2.Titles.Add(titulo);
        }
        /******************************************************************************************************/
        private void LlenarCmbNumeroProductos()
        {
            cmbNumeroProductos.SelectedIndexChanged -= cmbNumeroProductos_SelectedIndexChanged;
            var items = new List<KeyValuePair<string, int>>();
            for (int i = 10; i <= 30; i += 5)
            {
                items.Add(new KeyValuePair<string, int>($"{i} productos", i));
            }
            cmbNumeroProductos.DataSource = items;
            cmbNumeroProductos.DisplayMember = "Key";
            cmbNumeroProductos.ValueMember = "Value";
            cmbNumeroProductos.SelectedIndex = 0; // Selecciona el primer elemento
            cmbNumeroProductos.SelectedIndexChanged += cmbNumeroProductos_SelectedIndexChanged;
        }

        private void LlenarCmbTipoGrafica3()
        {
            CmbTipoGrafica3.SelectedIndexChanged -= CmbTipoGrafica3_SelectedIndexChanged;
            CmbTipoGrafica3.DataSource = Enum.GetValues(typeof(SeriesChartType))
                .Cast<SeriesChartType>()
                .Where(t => t != SeriesChartType.Kagi && t != SeriesChartType.ErrorBar && t != SeriesChartType.PointAndFigure && t != SeriesChartType.Polar && t != SeriesChartType.Renko && t != SeriesChartType.ThreeLineBreak) // Excluye tipos no deseados
                .OrderBy(t => t.ToString())
                .ToList();
            CmbTipoGrafica3.SelectedIndexChanged += CmbTipoGrafica3_SelectedIndexChanged;
        }

        private void cmbNumeroProductos_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarTopProductos(Convert.ToInt32(cmbNumeroProductos.SelectedValue), (SeriesChartType)CmbTipoGrafica3.SelectedItem);
        }

        private void CmbTipoGrafica3_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarTopProductos(Convert.ToInt32(cmbNumeroProductos.SelectedValue), (SeriesChartType)CmbTipoGrafica3.SelectedItem);
        }

        private void CargarTopProductos(int cantidad, SeriesChartType tipoGrafica)
        {
            DataTable productos = null;
            try
            {
                MDIPrincipal.ActualizarBarraDeEstado(Utils.clbdd);
                productos = new GraficaRepository(cnStr).ObtenerTopProductos(cantidad);
                MDIPrincipal.ActualizarBarraDeEstado();
            }
            catch (Exception ex)
            {
                Utils.MsgCatchOue(ex);
                return;
            }
            chart3.Series.Clear();
            chart3.Titles.Clear();
            Title titulo = new Title
            {
                Text = $"Top {cantidad} productos más vendidos.\nTipo de gráfica: {tipoGrafica}.",
                Font = new Font("Segoe UI", 8, FontStyle.Bold),
                Alignment = ContentAlignment.TopCenter
            };
            chart3.Titles.Add(titulo);
            groupBox3.Text = $"» Top {cantidad} productos más vendidos. Tipo de gráfica: {tipoGrafica}. «";
            var series = new Series("Productos")
            {
                ChartType = tipoGrafica,
                IsValueShownAsLabel = true,
                Label = "#VALY{n0}",
                LabelFormat = "C2",
                BorderWidth = 2,
                ToolTip = "Producto: #VALX,\nCantidad vendida: #VALY{n0}",
                Font = new Font("Segoe UI", 7, FontStyle.Bold)
            };
            series.Points.Clear();
            // Paleta de 10 colores (ajusta a tu gusto)
            Color[] paleta = {
                Color.SteelBlue, Color.Orange, Color.MediumSeaGreen,
                Color.Goldenrod, Color.Crimson, Color.MediumPurple,
                Color.Tomato, Color.Teal, Color.SlateGray, Color.DeepPink
            };
            int idx = 0;
            foreach (DataRow row in productos.Rows)
            {
                string nombre = (idx + 1).ToString() + ".- " + row["NombreProducto"].ToString();
                int qty = Convert.ToInt32(row["CantidadVendida"]);

                int pointIndex = series.Points.AddXY(nombre, qty);
                series.Points[pointIndex].Color = paleta[idx % paleta.Length];
                idx++;
            }
            chart3.Series.Add(series);
            chart3.Legends.Clear();
            var area = chart3.ChartAreas[0];
            area.Area3DStyle.Enable3D = true;
            area.Area3DStyle.Inclination = 30;
            area.Area3DStyle.Rotation = 20;
            area.Area3DStyle.LightStyle = LightStyle.Realistic;

            area.AxisX.Interval = 1;
            area.AxisX.LabelStyle.Angle = -45;
            area.AxisX.LabelStyle.Font = new Font("Segoe UI", 7, FontStyle.Regular);
            //area.AxisX.Title = "Productos más vendidos";
            area.AxisX.MajorGrid.Enabled = true;
            area.AxisX.MajorGrid.LineColor = Color.LightGray;
            area.AxisX.MajorGrid.LineDashStyle = ChartDashStyle.Dash;

            area.AxisY.LabelStyle.Format = "N0";
            area.AxisY.LabelStyle.Font = new Font("Segoe UI", 7, FontStyle.Regular);
            area.AxisY.Title = "Cantidad vendida (unidades)";
            area.AxisY.TitleFont = new Font("Segoe UI", 8, FontStyle.Regular);
            area.AxisY.MajorGrid.Enabled = true;
            area.AxisY.MajorGrid.LineColor = Color.LightGray;
            area.AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dash;
        }
        /******************************************************************************************************/
        private void CargarVentasPorVendedores()
        {
            DataTable dt = null;
            try
            {
                MDIPrincipal.ActualizarBarraDeEstado(Utils.clbdd);
                dt = ReportDataTableAdapter.ConvertirVendedorTotalVentas(new GraficaRepository(cnStr).ObtenerVentasPorVendedores());
                MDIPrincipal.ActualizarBarraDeEstado();
            }
            catch (Exception ex)
            {
                Utils.MsgCatchOue(ex);
                return;
            }
            chart4.Series.Clear();
            chart4.Titles.Clear();
            chart4.Titles.Add(new Title
            {
                Text = "» Ventas por vendedores de todos los años «",
                Docking = Docking.Top,
                Font = new Font("Segoe UI", 8, FontStyle.Bold)
            });
            // 1. Configurar ChartArea 3D
            var area = chart4.ChartAreas[0];
            area.Area3DStyle.Enable3D = true;
            area.Area3DStyle.Inclination = 40;
            area.Area3DStyle.Rotation = 60;
            area.Area3DStyle.LightStyle = LightStyle.Realistic;
            area.Area3DStyle.WallWidth = 0;
            // Configuración de la serie
            Series serie = new Series
            {
                Name = "Ventas",
                Color = Color.FromArgb(0, 51, 102),
                IsValueShownAsLabel = false,
                ChartType = SeriesChartType.Doughnut,
                Label = "#VALX, #VALY{c2}",
                ToolTip = "Vendedor: #VALX\nTotal Ventas: #VALY{C2}"
            };
            serie.Points.Clear();
            serie.SmartLabelStyle.Enabled = true;
            serie.SmartLabelStyle.AllowOutsidePlotArea = LabelOutsidePlotAreaStyle.No;
            serie.SmartLabelStyle.CalloutLineColor = Color.Black;
            serie.LabelForeColor = Color.DarkSlateGray;
            serie.LabelBackColor = Color.WhiteSmoke;
            serie["PieLabelStyle"] = "Disabled";
            serie["PieDrawingStyle"] = "Cylinder";
            serie["DoughnutRadius"] = "60";
            chart4.Series.Add(serie);
            foreach (DataRow row in dt.Rows)
            {
                decimal totalVentas = row.Field<decimal>("TotalVentas");
                string vendedor = row.Field<string>("Vendedor");
                int pointIndex = serie.Points.AddXY(vendedor, totalVentas);
                DataPoint dataPoint = serie.Points[pointIndex];
                dataPoint.Label = $"{vendedor}: {totalVentas:C2}";
                dataPoint.Font = new Font("Segoe UI", 7, FontStyle.Regular);
            }
            var legend = chart4.Legends[0];
            legend.Font = new Font("Segoe UI", 7, FontStyle.Regular);
        }
        /******************************************************************************************************/
        private void LlenarCmbVentasVendedorAño()
        {
            cmbVentasVendedorAño.SelectedIndexChanged -= cmbVentasVendedorAño_SelectedIndexChanged;
            try
            {
                MDIPrincipal.ActualizarBarraDeEstado(Utils.clbdd);
                var dt = new GraficaRepository(cnStr).ObtenerAñosDePedidos();
                foreach (DataRow row in dt.Rows)
                    cmbVentasVendedorAño.Items.Add(Convert.ToInt32(row["YearOrderDate"]));
                cmbVentasVendedorAño.SelectedIndex = 0; // Selecciona el primer elemento
                MDIPrincipal.ActualizarBarraDeEstado();
            }
            catch (Exception ex)
            {
                Utils.MsgCatchOue(ex);
            }
            cmbVentasVendedorAño.SelectedIndexChanged += cmbVentasVendedorAño_SelectedIndexChanged;
        }

        private void LlenarCmbTipoGrafica5()
        {
            CmbTipoGrafica5.SelectedIndexChanged -= CmbTipoGrafica5_SelectedIndexChanged;
            CmbTipoGrafica5.DataSource = Enum.GetValues(typeof(SeriesChartType))
                .Cast<SeriesChartType>()
                .Where(t => t != SeriesChartType.BoxPlot && t != SeriesChartType.ErrorBar && t != SeriesChartType.Kagi && t != SeriesChartType.PointAndFigure && t != SeriesChartType.Polar && t != SeriesChartType.Renko && t != SeriesChartType.StackedArea && t != SeriesChartType.StackedArea100 && t != SeriesChartType.StackedColumn100 && t != SeriesChartType.ThreeLineBreak) // Excluye tipos no deseados
                .OrderBy(t => t.ToString())
                .ToList();
            CmbTipoGrafica5.SelectedIndexChanged += CmbTipoGrafica5_SelectedIndexChanged;
        }

        private void cmbVentasVendedorAño_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbVentasVendedorAño.SelectedIndex < 0)
                return;
            CargarVentasPorVendedoresAño(Convert.ToInt32(cmbVentasVendedorAño.SelectedItem), (SeriesChartType)CmbTipoGrafica5.SelectedItem);

        }

        private void CmbTipoGrafica5_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarVentasPorVendedoresAño(Convert.ToInt32(cmbVentasVendedorAño.SelectedItem), (SeriesChartType)CmbTipoGrafica5.SelectedItem);
        }

        private void CargarVentasPorVendedoresAño(int anio, SeriesChartType tipoGrafica)
        {
            DataTable datos = null;
            try
            {
                MDIPrincipal.ActualizarBarraDeEstado(Utils.clbdd);
                datos = ReportDataTableAdapter.ConvertirVendedorTotalVentas(new GraficaRepository(cnStr).ObtenerVentasPorVendedor(anio));
                MDIPrincipal.ActualizarBarraDeEstado();
            }
            catch (Exception ex)
            {
                Utils.MsgCatchOue(ex);
                return;
            }
            chart5.Series.Clear();
            chart5.Titles.Clear();
            chart5.Legends.Clear();
            var leyenda = new Legend("Vendedores")
            {
                Title = "Vendedores",
                TitleFont = new Font("Segoe UI", 7, FontStyle.Bold),
                Docking = Docking.Right,
                LegendStyle = LegendStyle.Table,
                Font = new Font("Segoe UI", 7, FontStyle.Regular),
                IsTextAutoFit = false
            };

            chart5.Legends.Add(leyenda);
            // Título del gráfico
            Title titulo = new Title
            {
                Text = $"Ventas por vendedores del año {anio}",
                Font = new Font("Segoe UI", 8, FontStyle.Bold),
            };
            chart5.Titles.Add(titulo);
            groupBox5.Text = $"» {titulo.Text}. Tipo de grafica: {tipoGrafica} «";
            Series serie = new Series
            {
                Name = "Ventas",
                IsValueShownAsLabel = false,
                ChartType = tipoGrafica,
                Label = "#AXISLABEL: #VALY{C2}",
                ToolTip = "Vendedor: #AXISLABEL\nTotal ventas: #VALY{C2}",
                Legend = leyenda.Name,
                LegendText = "#AXISLABEL: #VALY{C2}"
            };
            if (tipoGrafica == SeriesChartType.Area || tipoGrafica == SeriesChartType.Bar || tipoGrafica == SeriesChartType.Bubble || tipoGrafica == SeriesChartType.Candlestick || tipoGrafica == SeriesChartType.Column || tipoGrafica == SeriesChartType.FastLine || tipoGrafica == SeriesChartType.FastPoint || tipoGrafica == SeriesChartType.Funnel || tipoGrafica == SeriesChartType.Line || tipoGrafica == SeriesChartType.Point || tipoGrafica == SeriesChartType.Pyramid || tipoGrafica == SeriesChartType.Radar || tipoGrafica == SeriesChartType.Range || tipoGrafica == SeriesChartType.RangeBar || tipoGrafica == SeriesChartType.RangeColumn || tipoGrafica == SeriesChartType.Spline || tipoGrafica == SeriesChartType.SplineArea || tipoGrafica == SeriesChartType.SplineRange || tipoGrafica == SeriesChartType.StackedBar || tipoGrafica == SeriesChartType.StackedBar100 || tipoGrafica == SeriesChartType.StackedColumn || tipoGrafica == SeriesChartType.StepLine || tipoGrafica == SeriesChartType.Stock)
            {
                chart5.Legends.Clear();
            }
            // 1. Configurar ChartArea 3D
            var area = chart5.ChartAreas[0];
            area.Area3DStyle.Enable3D = true;
            area.Area3DStyle.Inclination = 30;
            area.Area3DStyle.Rotation = 20;
            area.Area3DStyle.LightStyle = LightStyle.Realistic;
            // Opciones 3D y estilo dona
            serie["PieLabelStyle"] = "Disabled";
            serie["PieDrawingStyle"] = "Cylinder";
            serie["DoughnutRadius"] = "60";

            // 3.Agregar la serie al chart
            chart5.Series.Clear();
            chart5.Series.Add(serie);
            foreach (DataRow row in datos.Rows)
            {
                decimal totalVentas = row.Field<decimal>("TotalVentas");
                string vendedor = row.Field<string>("Vendedor");
                int pointIndex = serie.Points.AddXY(vendedor, totalVentas);
                DataPoint dataPoint = serie.Points[pointIndex];
                dataPoint.Label = $"{vendedor}: {totalVentas:C2}";
                dataPoint.Font = new Font("Segoe UI", 7, FontStyle.Regular);
            }
        }
        /******************************************************************************************************/
        private void LlenarCmbTipoGrafica()
        {
            // Obtiene todos los valores del enum
            var tipos = Enum.GetValues(typeof(SeriesChartType))
                            .Cast<SeriesChartType>()
                            .OrderBy(t => t.ToString());
            // Llena el ComboBox
            cmbTipoGrafica.DataSource = tipos.ToList();
        }

        private void DibujarGraficaChart6(SeriesChartType tipo)
        {
            chart6.Series.Clear();
            chart6.Titles.Clear();
            chart6.Titles.Add(new Title
            {
                Text = $"Tipo de gráfica: {tipo}",
                Docking = Docking.Top,
                Font = new Font("Segoe UI", 8, FontStyle.Bold)
            });
            var serie = new Series("Ventas")
            {
                ChartType = tipo,
                BorderWidth = 2,
                MarkerStyle = MarkerStyle.Circle,
                MarkerSize = 10,
                ToolTip = "#SERIESNAME\nMes: #AXISLABEL\nVentas: #VALY{C2}"
            };
            for (int i = 0; i < categorias.Length; i++)
            {
                serie.Points.AddXY(categorias[i], valores[i]);
            }
            chart6.Series.Add(serie);
            // Ajusta automáticamente las escalas de ejes
            chart6.ResetAutoValues();
            // Configuración del eje X
            chart6.ChartAreas[0].AxisX.LabelStyle.Angle = -45; // Inclina los labels 45 grados hacia la izquierda
            chart6.ChartAreas[0].AxisX.LabelStyle.Font = new Font("Segoe UI", 7); // Fuente más pequeña
            chart6.ChartAreas[0].AxisX.Interval = 1; // Asegura que se muestren todos los meses (cada categoría)
            // Configuración del eje Y
            chart6.ChartAreas[0].AxisY.LabelStyle.Font = new Font("Segoe UI", 7); // Fuente más pequeña
            chart6.ChartAreas[0].AxisY.LabelStyle.Format = "$#,##0"; // Formato con símbolo de dólar
            double maxValor = valores.Max();
            // Configura el eje Y para que el máximo sea justo un poco mayor (opcional para espacio visual)
            chart6.ChartAreas[0].AxisY.Maximum = Math.Ceiling(maxValor * 1.0); // 5% de margen por estética
            // Si lo deseas, también puedes fijar el mínimo
            chart6.ChartAreas[0].AxisY.Minimum = 0; // Para que siempre comience en cero
            // Establecer fuente más pequeña para el nombre de la serie en la leyenda
            chart6.Legends[0].Font = new Font("Segoe UI", 7); // Tamaño de fuente reducido
        }

        private void cmbTipoGrafica_SelectedIndexChanged(object sender, EventArgs e)
        {
            DibujarGraficaChart6((SeriesChartType)cmbTipoGrafica.SelectedItem);
        }
    }
}
