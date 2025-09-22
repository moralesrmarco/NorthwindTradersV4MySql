using Microsoft.Reporting.WinForms;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NorthwindTradersV4MySql
{
    public partial class FrmRptClientesyProveedoresDirectorio : Form
    {
        static string cnStr = ConfigurationManager.ConnectionStrings["NorthwindMySql"].ConnectionString;
        ClientesyProveedoresRepository repo = new ClientesyProveedoresRepository(cnStr);

        public FrmRptClientesyProveedoresDirectorio()
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
            repo = new ClientesyProveedoresRepository(cnStr);
        }

        private void GrbPaint(object sender, PaintEventArgs e) => Utils.GrbPaint2(this, sender, e);

        private void FrmRptClientesyProveedoresDirectorio_FormClosed(object sender, FormClosedEventArgs e) => MDIPrincipal.ActualizarBarraDeEstado();

        private void BtnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                if (!checkBoxClientes.Checked & !checkBoxProveedores.Checked)
                {
                    Utils.MensajeExclamation(Utils.errorCriterioSelec);
                    return;
                }
                MDIPrincipal.ActualizarBarraDeEstado(Utils.clbdd);
                string titulo = string.Empty;
                if (checkBoxClientes.Checked & checkBoxProveedores.Checked)
                {
                    titulo = "» Reporte directorio de clientes y proveedores «";
                }
                else if (checkBoxClientes.Checked & !checkBoxProveedores.Checked)
                {
                    titulo = "» Reporte directorio de clientes «";
                }
                else if (!checkBoxClientes.Checked & checkBoxProveedores.Checked)
                {
                    titulo = "» Reporte directorio de proveedores «";
                }
                groupBox1.Text = titulo;
                var clientesyProveedores = repo.ObtenerClientesyProveedores(checkBoxClientes.Checked, checkBoxProveedores.Checked);
                MDIPrincipal.ActualizarBarraDeEstado($"Se encontraron {clientesyProveedores.Count} registros");
                reportViewer1.LocalReport.DataSources.Clear();
                reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", clientesyProveedores));
                ReportParameter rp = new ReportParameter("titulo", titulo);
                reportViewer1.LocalReport.SetParameters(new ReportParameter[] { rp });
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

        private void FrmRptClientesyProveedoresDirectorio_Load(object sender, EventArgs e)
        {

        }
    }
}
