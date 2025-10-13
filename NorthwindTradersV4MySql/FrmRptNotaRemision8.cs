using Microsoft.Reporting.WinForms;
using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;
using System.Windows.Forms;

namespace NorthwindTradersV4MySql
{
    public partial class FrmRptNotaRemision8 : Form
    {

        public int Id;

        private string cnStr = ConfigurationManager.ConnectionStrings["NorthwindMySql"].ConnectionString;

        public FrmRptNotaRemision8()
        {
            InitializeComponent();
        }

        private void GrbPaint(object sender, PaintEventArgs e) => Utils.GrbPaint(this, sender, e);

        private void FrmRptNotaRemision8_FormClosed(object sender, FormClosedEventArgs e) => MDIPrincipal.ActualizarBarraDeEstado();

        private void FrmRptNotaRemision8_Load(object sender, EventArgs e)
        {
            try
            {
                ReportParameter[] parameters = new ReportParameter[2];
                parameters[0] = new ReportParameter("PedidoId", Id.ToString());
                parameters[1] = new ReportParameter("Para", "2"); // este parametro ya no se utiliza, pero si lo quito deja de funcionar el informe
                this.reportViewer1.LocalReport.SetParameters(parameters);
                MDIPrincipal.ActualizarBarraDeEstado(Utils.clbdd);
                DataTable dt2 = new PedidoRepository(cnStr).ObtenerPedido(Id);
                MDIPrincipal.ActualizarBarraDeEstado();
                ReportDataSource rds2 = new ReportDataSource("DataSet2", dt2);
                reportViewer1.LocalReport.DataSources.Add(rds2);
                MDIPrincipal.ActualizarBarraDeEstado(Utils.clbdd);
                // Obtenemos los datos detallados para la nota de remisión específica  
                DataTable dt3 = new PedidoRepository(cnStr).ObtenerDetallePedidoPorOrderID(Id);
                MDIPrincipal.ActualizarBarraDeEstado();
                ReportDataSource rds3 = new ReportDataSource("DataSet3", dt3);
                reportViewer1.LocalReport.DataSources.Add(rds3);
                reportViewer1.LocalReport.Refresh();
                this.reportViewer1.RefreshReport();
            }
            catch (Exception ex)
            {
                Utils.MsgCatchOue(ex);
            }
        }
    }
}
