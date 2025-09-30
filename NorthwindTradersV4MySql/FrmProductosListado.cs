using System;
using System.Configuration;
using System.Windows.Forms;

namespace NorthwindTradersV4MySql
{
    public partial class FrmProductosListado : Form
    {
        string cnStr = ConfigurationManager.ConnectionStrings["NorthwindMySql"].ConnectionString;

        string strProcedure = "";

        public FrmProductosListado()
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
        }

        private void GrbPaint(object sender, PaintEventArgs e) => Utils.GrbPaint2(this, sender, e);

        private void FrmProductosListado_FormClosed(object sender, FormClosedEventArgs e) => MDIPrincipal.ActualizarBarraDeEstado();

        private void FrmProductosListado_Load(object sender, EventArgs e)
        {
            LlenarCboCategoria();
            LlenarCboProveedor();
            Utils.ConfDgv(Dgv);
        }

        private void LlenarCboCategoria()
        {
            try
            {
                MDIPrincipal.ActualizarBarraDeEstado(Utils.clbdd);
                var dt = new ProductoRepository(cnStr).ObtenerComboCategorias();
                cboCategoria.DataSource = dt;
                cboCategoria.DisplayMember = "CategoryName";
                cboCategoria.ValueMember = "CategoryId";
                MDIPrincipal.ActualizarBarraDeEstado();
            }
            catch (Exception ex)
            {
                Utils.MsgCatchOue(ex);
            }
        }

        private void LlenarCboProveedor()
        {
            try
            {
                MDIPrincipal.ActualizarBarraDeEstado(Utils.clbdd);
                var dt = new ProductoRepository(cnStr).ObtenerComboProveedores();
                cboProveedor.DataSource = dt;
                cboProveedor.DisplayMember = "CompanyName";
                cboProveedor.ValueMember = "SupplierId";
                MDIPrincipal.ActualizarBarraDeEstado();
            }
            catch (Exception ex)
            {
                Utils.MsgCatchOue(ex);
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtIdInicial.Text = txtIdFinal.Text = txtProducto.Text = "";
            cboCategoria.SelectedIndex = cboProveedor.SelectedIndex = 0;
            Dgv.DataSource = null;
            MDIPrincipal.ActualizarBarraDeEstado();
        }

        private void btnListarTodos_Click(object sender, EventArgs e)
        {
            strProcedure = "spProductos";
            LlenarDgv(sender);
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            strProcedure = "spProductosBuscar";
            LlenarDgv(sender);
        }

        private void txtIdInicial_KeyPress(object sender, KeyPressEventArgs e) => Utils.ValidarDigitosSinPunto(sender, e);

        private void txtIdFinal_KeyPress(object sender, KeyPressEventArgs e) => Utils.ValidarDigitosSinPunto(sender, e);

        private void txtIdInicial_Leave(object sender, EventArgs e) => Utils.ValidaTxtBIdIni(txtIdInicial, txtIdFinal);

        private void txtIdFinal_Leave(object sender, EventArgs e) => Utils.ValidaTxtBIdFin(txtIdInicial, txtIdFinal);

        private void LlenarDgv(object sender)
        {
            try
            {
                MDIPrincipal.ActualizarBarraDeEstado(Utils.clbdd);
                DtoProductosBuscar dtoProductosBuscar = null;
                if (((Button)sender).Tag.ToString() == "Buscar")
                {
                    dtoProductosBuscar = new DtoProductosBuscar
                    {
                        IdIni = string.IsNullOrEmpty(txtIdInicial.Text) ? 0 : Convert.ToInt32(txtIdInicial.Text),
                        IdFin = string.IsNullOrEmpty(txtIdFinal.Text) ? 0 : Convert.ToInt32(txtIdFinal.Text),
                        Producto = txtProducto.Text.Trim(),
                        Categoria = Convert.ToInt32(cboCategoria.SelectedValue),
                        Proveedor = Convert.ToInt32(cboProveedor.SelectedValue)
                    };
                }
                var dt = new ProductoRepository(cnStr).ProductosListado(dtoProductosBuscar, strProcedure);
                Dgv.DataSource = dt;
                ConfDgv();
                MDIPrincipal.ActualizarBarraDeEstado($"Se encontraron {Dgv.RowCount} registros");
            }
            catch (Exception ex)
            {
                Utils.MsgCatchOue(ex);
            }
        }

        private void ConfDgv()
        {
            Dgv.Columns["CategoryId"].Visible = false;
            Dgv.Columns["SupplierId"].Visible = false;

            Dgv.Columns["ProductId"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            Dgv.Columns["ProductName"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            Dgv.Columns["QuantityPerUnit"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            Dgv.Columns["UnitPrice"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            Dgv.Columns["UnitsInStock"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            Dgv.Columns["UnitsOnOrder"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            Dgv.Columns["ReorderLevel"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            Dgv.Columns["Discontinued"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            Dgv.Columns["CategoryName"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

            Dgv.Columns["UnitPrice"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            Dgv.Columns["UnitsInStock"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            Dgv.Columns["UnitsOnOrder"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            Dgv.Columns["ReorderLevel"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;


            Dgv.Columns["UnitPrice"].DefaultCellStyle.Format = "c";
            Dgv.Columns["UnitsInStock"].DefaultCellStyle.Format = "n0";
            Dgv.Columns["UnitsOnOrder"].DefaultCellStyle.Format = "n0";
            Dgv.Columns["ReorderLevel"].DefaultCellStyle.Format = "n0";

            Dgv.Columns["ProductId"].HeaderText = "Id";
            Dgv.Columns["ProductName"].HeaderText = "Producto";
            Dgv.Columns["QuantityPerUnit"].HeaderText = "Cantidad por unidad";
            Dgv.Columns["UnitPrice"].HeaderText = "Precio";
            Dgv.Columns["UnitsInStock"].HeaderText = "Unidades en inventario";
            Dgv.Columns["UnitsOnOrder"].HeaderText = "Unidades en pedido";
            Dgv.Columns["ReorderLevel"].HeaderText = "Nivel de reorden";
            Dgv.Columns["Discontinued"].HeaderText = "Descontinuado";
            Dgv.Columns["CategoryName"].HeaderText = "Categoría";
            Dgv.Columns["Description"].HeaderText = "Descripción de categoría";
            Dgv.Columns["CompanyName"].HeaderText = "Proveedor";
        }

        private void tabcOperacion_Selected(object sender, TabControlEventArgs e)
        {
            btnLimpiar.PerformClick();
        }

        private void tabcOperacion_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnLimpiar.PerformClick();
        }
    }
}
