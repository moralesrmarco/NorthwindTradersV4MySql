using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace NorthwindTradersV4MySql
{
    public partial class FrmGraficaTop10ProductosMasVendidos : Form
    {

        private readonly string cnStr = ConfigurationManager.ConnectionStrings["NorthwindMySql"].ConnectionString;

        public FrmGraficaTop10ProductosMasVendidos()
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
        }

        private void GrbPaint(object sender, PaintEventArgs e) => Utils.GrbPaint(this, sender, e);

        private void FrmGraficaTop10ProductosMasVendidos_Load(object sender, EventArgs e)
        {
            LlenarComboBox();
            GroupBox.Text = $"» Top 10 productos más vendidos «";
            CargarTopProductos(10); // Cargar los 10 productos por defecto
        }

        private void LlenarComboBox()
        {
            var items = new List<KeyValuePair<string, int>>();
            items.Add(new KeyValuePair<string, int>("»--- Seleccione ---«", 0));
            for (int i = 10; i <= 30; i = i + 5)
            {
                items.Add(new KeyValuePair<string, int>($"{i} productos", i));
            }
            ComboBox.DataSource = items;
            ComboBox.DisplayMember = "Key";
            ComboBox.ValueMember = "Value";
        }

        private void BtnMostrar_Click(object sender, EventArgs e)
        {
            if (ComboBox.SelectedIndex == 0)
            {
                Utils.MensajeExclamation("Seleccione un número de productos válido.");
                return;
            }
            CargarTopProductos(Convert.ToInt32(ComboBox.SelectedValue));
        }

        private void CargarTopProductos(int cantidad)
        {
            ChartTopProductos.Series.Clear();
            ChartTopProductos.Titles.Clear();

            // Título y GroupBox
            Title titulo = new Title();
            if (cantidad <= 0)
            {
                titulo.Text = "Top 10 productos más vendidos";
                GroupBox.Text = $"» {titulo.Text} «";
            }
            else
            {
                titulo.Text = $"Top {cantidad} productos más vendidos";
                GroupBox.Text = $"» {titulo.Text} «";
            }
            titulo.Font = new Font("Arial", 14, FontStyle.Bold);
            titulo.Alignment = ContentAlignment.TopCenter;
            ChartTopProductos.Titles.Add(titulo);
            DataTable datos;
            try
            {
                MDIPrincipal.ActualizarBarraDeEstado(Utils.clbdd);
                // Datos
                datos = new GraficaRepository(cnStr).ObtenerTopProductos(cantidad);
                MDIPrincipal.ActualizarBarraDeEstado();
            }
            catch (Exception ex)
            {
                Utils.MsgCatchOue(ex);
                return;
            }
            // 1 serie única
            var series = ChartTopProductos.Series.Add("Productos más vendidos");
            series.ChartType = SeriesChartType.Column;
            series.IsValueShownAsLabel = true;
            series.Label = "#VALY{n0}";
            series.BorderWidth = 2;
            series.ToolTip = "Producto: #VALX, Cantidad vendida: #VALY{n0}";
            series.Font = new Font("Arial", 10, FontStyle.Bold);
            series.Points.Clear();

            // Paleta de 10 colores (ajusta a tu gusto)
                Color[] paleta = {
                Color.SteelBlue, Color.Orange, Color.MediumSeaGreen,
                Color.Goldenrod, Color.Crimson, Color.MediumPurple,
                Color.Tomato, Color.Teal, Color.SlateGray, Color.DeepPink
            };

            // Agregar puntos asignando color a cada uno
            int idx = 0;
            foreach (DataRow row in datos.Rows)
            {
                string nombre = (idx + 1).ToString() + ".- " + row["NombreProducto"].ToString();
                int qty = Convert.ToInt32(row["CantidadVendida"]);

                int pointIndex = series.Points.AddXY(nombre, qty);
                series.Points[pointIndex].Color = paleta[idx % paleta.Length];
                idx++;
            }

            ChartTopProductos.Legends.Clear();

            // Configurar ChartArea en 3D y ejes
            var area = ChartTopProductos.ChartAreas[0];

            area.Area3DStyle.Enable3D = true;
            area.Area3DStyle.Inclination = 30;
            area.Area3DStyle.Rotation = 20;
            area.Area3DStyle.LightStyle = LightStyle.Realistic;

            area.AxisX.Interval = 1;
            area.AxisX.LabelStyle.Angle = -45;
            area.AxisX.Title = "Productos más vendidos";
            area.AxisX.MajorGrid.Enabled = true;
            area.AxisX.MajorGrid.LineColor = Color.Black;
            area.AxisX.MajorGrid.LineDashStyle = ChartDashStyle.Dash;

            area.AxisY.LabelStyle.Format = "N0";
            area.AxisY.LabelStyle.Angle = -45;
            area.AxisY.LabelStyle.Font = new Font("Arial", 8, FontStyle.Regular);
            area.AxisY.Title = "Cantidad vendida (unidades)";
            area.AxisY.MajorGrid.Enabled = true;
            area.AxisY.MajorGrid.LineColor = Color.Black;
            area.AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dash;
            area.AxisY.MinorGrid.Enabled = true;
            area.AxisY.MinorGrid.LineColor = Color.Black;
            area.AxisY.MinorGrid.LineDashStyle = ChartDashStyle.Dash;
        }
    }
}
