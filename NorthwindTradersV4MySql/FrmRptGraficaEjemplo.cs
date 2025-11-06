using Microsoft.Reporting.WinForms;
using System;
using System.Data;
using System.Windows.Forms;

namespace NorthwindTradersV4MySql
{

    public partial class FrmRptGraficaEjemplo : Form
    {

        // Datos fijos: categorías y valores
        private readonly string[] categorias =
            { "Ene", "Feb", "Mar", "Abr", "May", "Jun", "Jul", "Ago", "Sep", "Oct", "Nov", "Dic" };
        private readonly double[] valores =
            { 15,    30,    45,    20,    35,    50,    25,    40, 45, 30, 40, 50 };

        public FrmRptGraficaEjemplo()
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
        }

        private void FrmRptGraficaVentasMensuales_Load(object sender, EventArgs e)
        {
            // 1. Limpia fuentes previas
            reportViewer1.LocalReport.DataSources.Clear();

            // 2. Obtén tu DataTable
            DataTable datos = GetTableGrafica();

            // 3. Usa el nombre EXACTO del DataSet del RDLC
            var rds = new ReportDataSource("DataSet1", datos);
            reportViewer1.LocalReport.DataSources.Add(rds);

            // 4. Refresca el reporte
            reportViewer1.RefreshReport();
        }

        private DataTable GetTableGrafica()
        {
            // Crea una tabla de datos para la gráfica
            DataTable dt = new DataTable();
            dt.Columns.Add("Categoria", typeof(string));
            dt.Columns.Add("Valor", typeof(double));
            dt.Columns.Add("MesNumero", typeof(int));
            // Llena la tabla con los datos fijos
            for (int i = 0; i < categorias.Length; i++)
            {
                dt.Rows.Add(categorias[i], valores[i], i+1);
            }
            return dt;
        }
    }
}
