using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Windows.Forms;

namespace NorthwindTradersV4MySql
{
    public partial class FrmClientesyProveedoresDirectorio : Form
    {
        static string cnStr = ConfigurationManager.ConnectionStrings["NorthwindMySql"].ConnectionString;
        private readonly ClienteRepository clienteRepository;

        public FrmClientesyProveedoresDirectorio()
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
            clienteRepository = new ClienteRepository(cnStr);
        }

        private void GrbPaint(object sender, PaintEventArgs e) => Utils.GrbPaint2(this, sender, e);

        private void FrmClientesyProveedoresDirectorio_FormClosed(object sender, FormClosedEventArgs e) => MDIPrincipal.ActualizarBarraDeEstado();

        private void FrmClientesyProveedoresDirectorio_Load(object sender, EventArgs e) => Utils.ConfDgv(Dgv);

        private void BtnBuscar_Click(object sender, EventArgs e)
        {
            if (!checkBoxClientes.Checked & !checkBoxProveedores.Checked)
            {
                Dgv.DataSource = null;
                Utils.MensajeExclamation(Utils.errorCriterioSelec);
                return;
            }
            LlenarDgv();
        }

        private void LlenarDgv()
        {
            try
            {
                MDIPrincipal.ActualizarBarraDeEstado(Utils.clbdd);
                if (checkBoxClientes.Checked & checkBoxProveedores.Checked)
                    groupBox1.Text = "» Directorio de clientes y proveedores «";
                else if (checkBoxClientes.Checked & !checkBoxProveedores.Checked)
                    groupBox1.Text = "» Directorio de clientes «";
                else if (!checkBoxClientes.Checked & checkBoxProveedores.Checked)
                    groupBox1.Text = "» Directorio de proveedores «";
                string nombreDeFormulario = "FrmClientesyProveedoresDirectorio";
                var dt = clienteRepository.ObtenerDirectorioClientesProveedores(nombreDeFormulario, string.Empty, checkBoxClientes.Checked, checkBoxProveedores.Checked);
                Dgv.DataSource = dt;
                ConfDgv();
                MDIPrincipal.ActualizarBarraDeEstado($"Se encontraron {dt.Rows.Count} registros");
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

        private void ConfDgv()
        {
            Dgv.Columns["City"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            Dgv.Columns["Country"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            Dgv.Columns["Relation"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            Dgv.Columns["Phone"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            Dgv.Columns["Region"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            Dgv.Columns["PostalCode"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            Dgv.Columns["Fax"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

            Dgv.Columns["Country"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            Dgv.Columns["Relation"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            Dgv.Columns["Phone"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            Dgv.Columns["Region"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            Dgv.Columns["PostalCode"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            Dgv.Columns["Fax"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            Dgv.Columns["CompanyName"].DisplayIndex = 0;
            Dgv.Columns["Contact"].DisplayIndex = 1;
            Dgv.Columns["Relation"].DisplayIndex = 2;
            Dgv.Columns["Address"].DisplayIndex = 3;
            Dgv.Columns["City"].DisplayIndex = 4;
            Dgv.Columns["Region"].DisplayIndex = 5;
            Dgv.Columns["PostalCode"].DisplayIndex = 6;
            Dgv.Columns["Country"].DisplayIndex = 7;
            Dgv.Columns["Phone"].DisplayIndex = 8;
            Dgv.Columns["Fax"].DisplayIndex = 9;

            Dgv.Columns["CompanyName"].HeaderText = "Nombre de compañía";
            Dgv.Columns["Contact"].HeaderText = "Nombre de contacto";
            Dgv.Columns["Relation"].HeaderText = "Relación";
            Dgv.Columns["Address"].HeaderText = "Domicilio";
            Dgv.Columns["City"].HeaderText = "Ciudad";
            Dgv.Columns["Region"].HeaderText = "Región";
            Dgv.Columns["PostalCode"].HeaderText = "Código postal";
            Dgv.Columns["Country"].HeaderText = "País";
            Dgv.Columns["Phone"].HeaderText = "Teléfono";
        }
    }
}
