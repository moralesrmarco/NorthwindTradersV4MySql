using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Windows.Forms;

namespace NorthwindTradersV4MySql
{
    public partial class FrmRptGraficaVentasAnuales : Form
    {

        private readonly string cnStr = ConfigurationManager.ConnectionStrings["NorthwindMySql"].ConnectionString;

        public FrmRptGraficaVentasAnuales()
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
        }

        private void GrbPaint(object sender, PaintEventArgs e) => Utils.GrbPaint(this, sender, e);

        private void FrmRptGraficaVentasAnuales_Load(object sender, EventArgs e)
        {
            LlenarCmbVentasAnuales();
        }

        private void LlenarCmbVentasAnuales()
        {
            var items = new List<KeyValuePair<string, int>>();
            for (int i = 2; i <= 10; i++)
            {
                items.Add(new KeyValuePair<string, int>($"{i} Años ", i));
            }
            CmbVentasAnuales.SelectedIndexChanged -= CmbVentasAnuales_SelectedIndexChanged;
            CmbVentasAnuales.DisplayMember = "Key";
            CmbVentasAnuales.ValueMember = "Value";
            CmbVentasAnuales.DataSource = items;
            CmbVentasAnuales.SelectedIndex = -1;
            CmbVentasAnuales.SelectedIndexChanged += CmbVentasAnuales_SelectedIndexChanged;
            CmbVentasAnuales.SelectedIndex = 0;
        }

        private void CmbVentasAnuales_SelectedIndexChanged(object sender, EventArgs e)
        {
            int years = Convert.ToInt32(CmbVentasAnuales.SelectedValue);
            if (years >= 6)
            {
                Utils.MensajeExclamation("Solo existen datos en la base de datos hasta el año 1996");
                return;
            }
            CargarComparativoVentasAnuales(years);
        }

        private void CargarComparativoVentasAnuales(int years)
        {
            groupBox1.Text = $"» Comparativo de ventas anuales de los últimos {years} años «";
            int year = DateTime.Now.Year;
            List<int> listaAños = new List<int>();
            for (int i = 1; i <= years; i++)
            {
                if (year == 2023)
                    year = 1998;
                else if (year == 1995)
                    break;
                listaAños.Add(year);
                year--;
            }
            DataTable dtComparativo = null;
            try
            {
                dtComparativo = ReportDataTableAdapter.ConvertirVentaAnualComparativa(new GraficaRepository(cnStr).ObtenerVentasComparativas(listaAños));
            }
            catch (Exception ex)
            {
                Utils.MsgCatchOue(ex);
                return;
            }
            reportViewer1.LocalReport.DataSources.Clear();
            var rds = new ReportDataSource("DataSet1", dtComparativo);
            reportViewer1.LocalReport.DataSources.Add(rds);
            reportViewer1.LocalReport.SetParameters(new ReportParameter("Anio", CmbVentasAnuales.Text));
            reportViewer1.LocalReport.SetParameters(new ReportParameter("Subtitulo", $"Comparativo de ventas anuales de los últimos {years} años"));
            reportViewer1.RefreshReport();
        }
    }
}
