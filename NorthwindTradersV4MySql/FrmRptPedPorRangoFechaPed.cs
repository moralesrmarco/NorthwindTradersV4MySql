// https://www.youtube.com/watch?v=2-YkNo1Os3Y&list=PL_1AVI-bgZKQ2MSDejVmaaxNenhETwwx_&index=7
// https://www.youtube.com/watch?v=7AvCaq7a1fc&list=PL_1AVI-bgZKQ2MSDejVmaaxNenhETwwx_&index=5
using Microsoft.Reporting.WinForms;
using System;
using System.Configuration;
using System.Data;
using System.Windows.Forms;

namespace NorthwindTradersV4MySql
{
    public partial class FrmRptPedPorRangoFechaPed: Form
    {
        private string cnStr = ConfigurationManager.ConnectionStrings["NorthwindMySql"].ConnectionString;

        public FrmRptPedPorRangoFechaPed()
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
        }

        private void GrbPaint(object sender, PaintEventArgs e) => Utils.GrbPaint(this, sender, e);

        private void FrmRptPedPorRangoFechaPed_FormClosed(object sender, FormClosedEventArgs e) => MDIPrincipal.ActualizarBarraDeEstado();

        private void btnImprimir_Click(object sender, EventArgs e)
        {
            MostrarReporte();
        }

        private void MostrarReporte()
        {
            string subtitulo;
            if (dateTimePicker1.Checked & dateTimePicker2.Checked)
                subtitulo = $"[ Fecha de pedido inicial: {dateTimePicker1.Value.ToShortDateString()} ] - [ Fecha de pedido final: {dateTimePicker2.Value.ToShortDateString()} ]";
            else
                subtitulo = "[ Fecha de pedido inicial: Nulo ] - [ Fecha de pedido final: Nulo ]";
            MDIPrincipal.ActualizarBarraDeEstado(Utils.clbdd);
            DataTable dt = new DataTable();
            if (dateTimePicker1.Checked & dateTimePicker2.Checked)
                dt = new PedidoRepository(cnStr).ObtenerPedidosPorFechaPedido(dateTimePicker1.Value.Date, dateTimePicker2.Value.Date.AddDays(1));
            else
                dt = new PedidoRepository(cnStr).ObtenerPedidosPorFechaPedido(null, null);
            MDIPrincipal.ActualizarBarraDeEstado($"Se encontraron {dt.Rows.Count} registros");
            if (dt.Rows.Count > 0)
            {
                ReportDataSource reportDataSource = new ReportDataSource("DataSet1", dt);
                reportViewer1.LocalReport.DataSources.Clear();
                reportViewer1.LocalReport.DataSources.Add(reportDataSource);
                ReportParameter reportParameter = new ReportParameter("subtitulo", subtitulo);
                reportViewer1.LocalReport.SetParameters(new ReportParameter[] { reportParameter });
                reportViewer1.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(OrderDetailsSubReportProcessing);
                reportViewer1.RefreshReport();
            }
            else
            {
                reportViewer1.LocalReport.DataSources.Clear();
                ReportDataSource reportDataSource = new ReportDataSource("DataSet1", new DataTable());
                reportViewer1.LocalReport.DataSources.Add(reportDataSource);
                ReportParameter reportParameter = new ReportParameter("subtitulo", subtitulo);
                reportViewer1.LocalReport.SetParameters(new ReportParameter[] { reportParameter });
                reportViewer1.RefreshReport();
                MessageBox.Show(Utils.noDatos, Utils.nwtr, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void OrderDetailsSubReportProcessing(object sender, SubreportProcessingEventArgs e)
        {
            int orderID = int.Parse(e.Parameters["OrderID"].Values[0].ToString());
            DataTable dt = new PedidoRepository(cnStr).ObtenerDetallePedidoPorOrderID(orderID);
            ReportDataSource reportDataSource = new ReportDataSource("DataSet1", dt);
            e.DataSources.Add(reportDataSource);
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            if (dateTimePicker1.Checked)
                dateTimePicker2.Checked = true;
            else
                dateTimePicker2.Checked = false;
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            if (dateTimePicker2.Checked)
                dateTimePicker1.Checked = true;
            else
                dateTimePicker1.Checked = false;
        }

        private void dateTimePicker1_Leave(object sender, EventArgs e)
        {
            if (dateTimePicker1.Checked && dateTimePicker2.Checked)
                if (dateTimePicker2.Value < dateTimePicker1.Value)
                    dateTimePicker2.Value = dateTimePicker1.Value;
        }

        private void dateTimePicker2_Leave(object sender, EventArgs e)
        {
            if (dateTimePicker1.Checked && dateTimePicker2.Checked)
                if ( dateTimePicker2.Value < dateTimePicker1.Value)
                    dateTimePicker1.Value = dateTimePicker2.Value;
        }
    }
}
