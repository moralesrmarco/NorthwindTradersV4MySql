using Microsoft.Reporting.WinForms;
using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;
using System.Windows.Forms;

namespace NorthwindTradersV4MySql
{
    public partial class FrmRptEmpleados : Form
    {
        public FrmRptEmpleados()
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
        }

        private void GrbPaint(object sender, PaintEventArgs e) => Utils.GrbPaint2(this, sender, e);

        private void FrmRptEmpleados_FormClosed(object sender, FormClosedEventArgs e) => MDIPrincipal.ActualizarBarraDeEstado();

        private void FrmRptEmpleados_Load(object sender, EventArgs e)
        {
            try
            {
                MDIPrincipal.ActualizarBarraDeEstado(Utils.clbdd);
                var ds = new DataSet();
                string cnStr = ConfigurationManager.ConnectionStrings["NorthwindMySql"].ConnectionString;
                using (var cn = new MySqlConnection(cnStr))
                using (var da = new MySqlDataAdapter("SELECT e1.*, e2.FirstName As ReportsToFirstName, e2.LastName As ReportsToLastName FROM Employees e1 Left Join Employees e2 On e1.ReportsTo = e2.EmployeeID", cn))
                {
                    da.Fill(ds, "Employees");
                }
                MDIPrincipal.ActualizarBarraDeEstado($"Se encontraron {ds.Tables["Employees"].Rows.Count} registros");
                var rds = new ReportDataSource("DataSet1", ds.Tables["Employees"]);
                reportViewer1.LocalReport.DataSources.Clear();
                reportViewer1.LocalReport.DataSources.Add(rds);

                // Asignar el RDLC incrustado
                reportViewer1.LocalReport.ReportEmbeddedResource = "NorthwindTradersV4MySql.RptEmpleados.rdlc";
                reportViewer1.RefreshReport();

            }
            catch (MySqlException ex)
            {
                Utils.MsgCatchOueclbdd(ex);
            }
            catch (Exception ex)
            {
                Utils.MsgCatchOue(ex);
            }
        }
    }
}
