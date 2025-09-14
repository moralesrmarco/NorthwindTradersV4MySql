using Microsoft.Reporting.WinForms;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace NorthwindTradersV4MySql
{
    public partial class FrmRptEmpleado : Form
    {
        static string cnStr = System.Configuration.ConfigurationManager.ConnectionStrings["NorthwindMySql"].ConnectionString;
        public int Id { get; set; }
        private readonly EmpleadoRepository _empleadoRepository;

        public FrmRptEmpleado()
        {
            InitializeComponent();
            _empleadoRepository = new EmpleadoRepository(cnStr);
        }

        private void GrbPaint(object sender, PaintEventArgs e) => Utils.GrbPaint2(this, sender, e);

        private void FrmRptEmpleado_FormClosed(object sender, FormClosedEventArgs e) => MDIPrincipal.ActualizarBarraDeEstado();

        private void FrmRptEmpleado_Load(object sender, EventArgs e)
        {
            try
            {
                MDIPrincipal.ActualizarBarraDeEstado(Utils.clbdd);
                EmpleadoConReportsTo empleadoConReportsTo = _empleadoRepository.ObtenerEmpleadoConReportsTo(Id);
                MDIPrincipal.ActualizarBarraDeEstado($"Se encontró el registro con el Id: {empleadoConReportsTo.EmployeeID}");
                // Crear una lista con un solo empleado
                List<EmpleadoConReportsTo> empleadosConReportsTo = new List<EmpleadoConReportsTo> { empleadoConReportsTo };
                // Asignar la lista como fuente de datos del informe
                reportViewer1.LocalReport.DataSources.Clear();
                reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", empleadosConReportsTo));
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
