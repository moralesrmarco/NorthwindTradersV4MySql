using Microsoft.Reporting.WinForms;
using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Windows.Forms;

namespace NorthwindTradersV4MySql
{
    public partial class FrmRptProveedores : Form
    {
        string cnStr = ConfigurationManager.ConnectionStrings["NorthwindMySql"].ConnectionString;
        ProveedorRepository repo;

        public FrmRptProveedores()
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
            repo = new ProveedorRepository(cnStr);
        }

        private void GrbPaint(object sender, PaintEventArgs e) => Utils.GrbPaint2(this, sender, e);

        private void FrmRptProveedores_FormClosed(object sender, FormClosedEventArgs e) => MDIPrincipal.ActualizarBarraDeEstado();

        private void FrmRptProveedores_Load(object sender, EventArgs e)
        {
            try
            {
                MDIPrincipal.ActualizarBarraDeEstado(Utils.clbdd);
                var proveedores = repo.ObtenerProveedoresList();
                MDIPrincipal.ActualizarBarraDeEstado($"Se encontraron {proveedores.Count} registros");
                reportViewer1.LocalReport.DataSources.Clear();
                reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", proveedores));
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
