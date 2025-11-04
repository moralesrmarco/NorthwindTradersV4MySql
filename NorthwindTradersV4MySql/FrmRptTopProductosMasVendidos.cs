using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Windows.Forms;

namespace NorthwindTradersV4MySql
{
    public partial class FrmRptTopProductosMasVendidos : Form
    {

        private readonly string cnStr = ConfigurationManager.ConnectionStrings["NorthwindMySql"].ConnectionString;

        public FrmRptTopProductosMasVendidos()
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
        }

        private void GrbPaint(object sender, PaintEventArgs e) => Utils.GrbPaint(this, sender, e);

        private void FrmRptTopProductosMasVendidos_Load(object sender, EventArgs e)
        {
            LlenarCmbTopProductos();
        }

        private void LlenarCmbTopProductos()
        {
            List<KeyValuePair<string, int>> items = new List<KeyValuePair<string, int>>();
            for (int i = 10; i <= 50; i = i + 5)
            {
                items.Add(new KeyValuePair<string, int>($"{i} productos", i));
            }
            CmbTopProductos.SelectedIndexChanged -= CmbTopProductos_SelectedIndexChanged;
            CmbTopProductos.DisplayMember = "Key";
            CmbTopProductos.ValueMember = "Value";
            CmbTopProductos.DataSource = items;
            CmbTopProductos.SelectedIndex = -1;
            CmbTopProductos.SelectedIndexChanged += CmbTopProductos_SelectedIndexChanged;
            CmbTopProductos.SelectedIndex = 0;
        }

        private void CmbTopProductos_SelectedIndexChanged(object sender, EventArgs e)
        {
            int topProductos = Convert.ToInt32(CmbTopProductos.SelectedValue);
            CargarTopProductos(topProductos);
        }

        private void CargarTopProductos(int topProductos)
        {
            groupBox1.Text = $"» Reporte gráfico top {topProductos} productos más vendidos «";
            DataTable dt = null;
            try
            {
                dt = ReportDataTableAdapter.ConvertirProductoMasVendido(new GraficaRepository(cnStr).ObtenerTopProductosRpt(topProductos));
            }
            catch (Exception ex)
            {
                Utils.MsgCatchOue(ex);
            }
            if (dt != null)
            {
                // 1. Limpia fuentes previas
                reportViewer1.LocalReport.DataSources.Clear();
                // 2. Usa el nombre EXACTO del DataSet del RDLC
                var rds = new ReportDataSource("DataSet1", dt);
                reportViewer1.LocalReport.DataSources.Add(rds);
                reportViewer1.LocalReport.SetParameters(new ReportParameter("NumProductos", CmbTopProductos.SelectedValue.ToString()));
                reportViewer1.LocalReport.SetParameters(new ReportParameter("Subtitulo", $"Top {CmbTopProductos.SelectedValue.ToString()} productos más vendidos"));
                // 3. Refresca el reporte
                reportViewer1.RefreshReport();
            }
        }
    }
}
