using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace NorthwindTradersV4MySql
{
    public partial class FrmRptProductos: Form
    {

        string cnStr = System.Configuration.ConfigurationManager.ConnectionStrings["NorthwindMySql"].ConnectionString;
        string strProcedure = "";
        string titulo = "» Reporte de productos «";
        string subtitulo = "";

        public FrmRptProductos()
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
        }

        private void GrbPaint(object sender, PaintEventArgs e) => Utils.GrbPaint2(this, sender, e);

        private void FrmRptProductos_FormClosed(object sender, FormClosedEventArgs e) => MDIPrincipal.ActualizarBarraDeEstado();

        private void FrmRptProductos_Load(object sender, EventArgs e)
        {
            Utils.LlenarCbo(cboCategoria, "spCategoriasSeleccionar", "CategoryName", "CategoryId");
            Utils.LlenarCbo(cboProveedor, "spProveedoresSeleccionar", "CompanyName", "SupplierId");
            var itemsCbo1OrdenadoPor = new List<KeyValuePair<string, string>> 
            {
                new KeyValuePair<string, string>("ProductID", "ID Producto"),
                new KeyValuePair<string, string>("ProductName", "Producto"),
                new KeyValuePair<string, string>("QuantityPerUnit", "Cantidad por unidad"),
                new KeyValuePair<string, string>("UnitPrice", "Precio"),
                new KeyValuePair<string, string>("UnitsInStock", "Unidades en inventario"),
                new KeyValuePair<string, string>("UnitsOnOrder", "Unidades en pedido"),
                new KeyValuePair<string, string>("ReorderLevel", "Nivel de reorden"),
                new KeyValuePair<string, string>("Discontinued", "Descontinuado"),
                new KeyValuePair<string, string>("CategoryName", "Categoría"),
                new KeyValuePair<string, string>("CompanyName", "Proveedor")
            };
            var itemsCbo2OrdenadoPor = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("ProductID", "ID Producto"),
                new KeyValuePair<string, string>("ProductName", "Producto"),
                new KeyValuePair<string, string>("QuantityPerUnit", "Cantidad por unidad"),
                new KeyValuePair<string, string>("UnitPrice", "Precio"),
                new KeyValuePair<string, string>("UnitsInStock", "Unidades en inventario"),
                new KeyValuePair<string, string>("UnitsOnOrder", "Unidades en pedido"),
                new KeyValuePair<string, string>("ReorderLevel", "Nivel de reorden"),
                new KeyValuePair<string, string>("Discontinued", "Descontinuado"),
                new KeyValuePair<string, string>("CategoryName", "Categoría"),
                new KeyValuePair<string, string>("CompanyName", "Proveedor")
            };
            var items1AscDesc = new List<KeyValuePair<string, string>> 
            {
                new KeyValuePair<string, string>("ASC", "Ascendente"),
                new KeyValuePair<string, string>("DESC", "Descendente")
            };
            var items2AscDesc = new List<KeyValuePair<string, string>> 
            {
                new KeyValuePair<string, string>("ASC", "Ascendente"),
                new KeyValuePair<string, string>("DESC", "Descendente")
            };
            Cbo1OrdenadoPor.DataSource = itemsCbo1OrdenadoPor;
            Cbo1OrdenadoPor.DisplayMember = "Value";
            Cbo1OrdenadoPor.ValueMember = "Key";
            Cbo2OrdenadoPor.DataSource = itemsCbo2OrdenadoPor;
            Cbo2OrdenadoPor.DisplayMember = "Value";
            Cbo2OrdenadoPor.ValueMember = "Key";
            Cbo1AscDesc.DataSource = items1AscDesc;
            Cbo1AscDesc.DisplayMember = "Value";
            Cbo1AscDesc.ValueMember = "Key";
            Cbo2AscDesc.DataSource = items2AscDesc;
            Cbo2AscDesc.DisplayMember = "Value";
            Cbo2AscDesc.ValueMember = "Key";
            Cbo1OrdenadoPor.SelectedIndex = Cbo1AscDesc.SelectedIndex = 0;
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtIdInicial.Text = txtIdFinal.Text = txtProducto.Text = "";
            cboCategoria.SelectedIndex = cboProveedor.SelectedIndex = 0;
            Cbo1OrdenadoPor.SelectedIndex = Cbo1AscDesc.SelectedIndex = 0;
            Cbo2OrdenadoPor.SelectedIndex = Cbo2AscDesc.SelectedIndex = 0;
            MDIPrincipal.ActualizarBarraDeEstado();
        }

        private void btnImprimirTodos_Click(object sender, EventArgs e)
        {
            strProcedure = "spProductosV2";
            LlenarReporte(sender);
        }

        private void btnImprimir_Click(object sender, EventArgs e)
        {
            strProcedure = "spProductosBuscarV2";
            LlenarReporte(sender);
        }

        private void txtIdInicial_KeyPress(object sender, KeyPressEventArgs e) => Utils.ValidarDigitosSinPunto(sender, e);

        private void txtIdFinal_KeyPress(object sender, KeyPressEventArgs e) => Utils.ValidarDigitosSinPunto(sender, e);

        private void txtIdInicial_Leave(object sender, EventArgs e) => Utils.ValidaTxtBIdIni(txtIdInicial, txtIdFinal);

        private void txtIdFinal_Leave(object sender, EventArgs e) => Utils.ValidaTxtBIdFin(txtIdInicial, txtIdFinal);

        private void tabcOperacion_Selected(object sender, TabControlEventArgs e) => btnLimpiar.PerformClick();
        
        private void tabcOperacion_SelectedIndexChanged(object sender, EventArgs e) => btnLimpiar.PerformClick();

        private void LlenarReporte(object sender)
        {
            try
            {
                titulo = "» Reporte de todos los productos «";
                subtitulo = "";
                MDIPrincipal.ActualizarBarraDeEstado(Utils.clbdd);
                DtoProductosBuscar dtoProductosBuscar = null;
                if (((Button)sender).Tag.ToString() == "Imprimir")
                {
                    dtoProductosBuscar = new DtoProductosBuscar
                    {
                        IdIni = string.IsNullOrEmpty(txtIdInicial.Text) ? 0 : Convert.ToInt32(txtIdInicial.Text),
                        IdFin = string.IsNullOrEmpty(txtIdFinal.Text) ? 0 : Convert.ToInt32(txtIdFinal.Text),
                        Producto = txtProducto.Text.Trim(),
                        Categoria = cboCategoria.SelectedIndex <= 0 ? 0 : Convert.ToInt32(cboCategoria.SelectedValue),
                        Proveedor = cboProveedor.SelectedIndex <= 0 ? 0 : Convert.ToInt32(cboProveedor.SelectedValue),
                        OrdenadoPor = Cbo2OrdenadoPor.SelectedValue.ToString(),
                        AscDesc = Cbo2AscDesc.SelectedValue.ToString()
                    };
                    titulo = "» Reporte filtrado de productos «";
                    subtitulo = $"Filtrado por: ";
                    if (txtIdInicial.Text != "" & txtIdFinal.Text != "")
                        subtitulo += $" [ Id: {txtIdInicial.Text} al {txtIdFinal.Text} ] ";
                    if (txtProducto.Text != "")
                        subtitulo += $" [ Producto: {txtProducto.Text} ] ";
                    if (cboCategoria.SelectedIndex > 0)
                        subtitulo += $" [ Categoría: {cboCategoria.Text}] ";
                    if (cboProveedor.SelectedIndex > 0)
                        subtitulo += $" [ Proveedor: {cboProveedor.Text}] ";
                    if (subtitulo == "Filtrado por: ")
                    {
                        titulo = "» Reporte de todos los productos «";
                        subtitulo = "";
                    }
                    subtitulo += $" Ordenado por: [ {Cbo2OrdenadoPor.Text} ] [ {Cbo2AscDesc.Text} ]";
                }
                else
                {
                    dtoProductosBuscar = new DtoProductosBuscar
                    {
                        OrdenadoPor = Cbo1OrdenadoPor.SelectedValue.ToString(),
                        AscDesc = Cbo1AscDesc.SelectedValue.ToString()
                    };
                    subtitulo = $"Ordenado por: [ {Cbo1OrdenadoPor.Text} ] [ {Cbo1AscDesc.Text} ]";
                }
                groupBox1.Text = titulo + " | » " + subtitulo + " «";
                var dt = new ProductoRepository(cnStr).RptProductosListado(dtoProductosBuscar, strProcedure);
                MDIPrincipal.ActualizarBarraDeEstado($"Se encontraron {dt.Rows.Count} registros");
                if (dt.Rows.Count > 0)
                {
                    ReportDataSource reportDataSource = new ReportDataSource("DataSet1", dt);
                    reportViewer1.LocalReport.DataSources.Clear();
                    reportViewer1.LocalReport.DataSources.Add(reportDataSource);
                    ReportParameter rp = new ReportParameter("titulo", titulo);
                    ReportParameter rp2 = new ReportParameter("subtitulo", subtitulo);
                    reportViewer1.LocalReport.SetParameters(new ReportParameter[] { rp, rp2 });
                    reportViewer1.RefreshReport();
                }
                else
                {
                    reportViewer1.LocalReport.DataSources.Clear();
                    ReportDataSource reportDataSource = new ReportDataSource("DataSet1", new DataTable());
                    reportViewer1.LocalReport.DataSources.Add(reportDataSource);
                    ReportParameter rp = new ReportParameter("titulo", titulo);
                    ReportParameter rp2 = new ReportParameter("subtitulo", subtitulo);
                    reportViewer1.LocalReport.SetParameters(new ReportParameter[] { rp, rp2 });
                    reportViewer1.RefreshReport();
                    Utils.MensajeExclamation(Utils.noDatos);
                }
            }
            catch (Exception ex) { Utils.MsgCatchOue(ex); }
        }
    }
}
