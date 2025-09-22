using Microsoft.Reporting.WinForms;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NorthwindTradersV4MySql
{
    public partial class FrmRptClientesyProveedoresDirectorioxPais : Form
    {
        string cnStr = ConfigurationManager.ConnectionStrings["NorthwindMySql"].ConnectionString;
        ClienteRepository clienteRepo;
        ClientesyProveedoresRepository repo;

        public FrmRptClientesyProveedoresDirectorioxPais()
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
            clienteRepo = new ClienteRepository(cnStr);
            repo = new ClientesyProveedoresRepository(cnStr);
        }

        private void GrbPaint(object sender, PaintEventArgs e) => Utils.GrbPaint2(this, sender, e);

        private void FrmRptClientesyProveedoresDirectorioxPais_FormClosed(object sender, FormClosedEventArgs e) => MDIPrincipal.ActualizarBarraDeEstado();

        private void FrmRptClientesyProveedoresDirectorioxPais_Load(object sender, EventArgs e)
        {
            LlenarComboBox();
        }

        void LlenarComboBox()
        {
            try
            {
                var paises = clienteRepo.ObtenerComboClientesProveedoresPais();
                comboBox.DataSource = paises;
                comboBox.DisplayMember = "País";
                comboBox.ValueMember = "IdPaís";
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

        private void BtnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                if (comboBox.SelectedIndex <= 0 | (!checkBoxClientes.Checked & !checkBoxProveedores.Checked))
                {
                    Utils.MensajeExclamation(Utils.errorCriterioSelec);
                    return;
                }
                MDIPrincipal.ActualizarBarraDeEstado(Utils.clbdd);
                string titulo = string.Empty;
                if (comboBox.SelectedValue.ToString() == "aaaaa" & checkBoxClientes.Checked & checkBoxProveedores.Checked)
                    titulo = "» Reporte directorio de clientes y proveedores por país [ Todos los países ] «";
                else if (comboBox.SelectedValue.ToString() != "aaaaa" & checkBoxClientes.Checked & checkBoxProveedores.Checked)
                    titulo = $"» Reporte directorio de clientes y proveedores por país [ País: {comboBox.SelectedValue.ToString()} ] «";
                else if (comboBox.SelectedValue.ToString() == "aaaaa" & checkBoxClientes.Checked & !checkBoxProveedores.Checked)
                    titulo = "» Reporte directorio de clientes por país [ Todos los países ] «";
                else if (comboBox.SelectedValue.ToString() == "aaaaa" & !checkBoxClientes.Checked & checkBoxProveedores.Checked)
                    titulo = "» Reporte directorio de proveedores por país [ Todos los países ] «";
                else if (comboBox.SelectedValue.ToString() != "aaaaa" & checkBoxClientes.Checked & !checkBoxProveedores.Checked)
                    titulo = $"» Reporte directorio de clientes por país [ País: {comboBox.SelectedValue.ToString()} ] «";
                else if (comboBox.SelectedValue.ToString() != "aaaaa" & !checkBoxClientes.Checked & checkBoxProveedores.Checked)
                    titulo = $"» Reporte directorio de proveedores por país [ País: {comboBox.SelectedValue.ToString()} ] «";
                groupBox1.Text = titulo;
                string nombreDeFormulario = "FrmRptClientesyProveedoresDirectorioxPais";
                var clientesyProveedores = repo.ObtenerClientesyProveedores(nombreDeFormulario, comboBox.SelectedValue.ToString(), checkBoxClientes.Checked, checkBoxProveedores.Checked);
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
    }
}
