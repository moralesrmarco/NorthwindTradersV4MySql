using MySql.Data.MySqlClient;
using System;
using System.Windows.Forms;

namespace NorthwindTradersV4MySql
{
    public partial class FrmProveedoresProductos : Form
    {
        string cnStr = System.Configuration.ConfigurationManager.ConnectionStrings["NorthwindMySql"].ConnectionString;
        BindingSource bsProveedores = new BindingSource();
        BindingSource bsProductos = new BindingSource();

        public FrmProveedoresProductos()
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
        }

        private void GrbPaint(object sender, PaintEventArgs e) => Utils.GrbPaint2(this, sender, e);

        private void FrmProveedoresProductos_FormClosed(object sender, FormClosedEventArgs e) => MDIPrincipal.ActualizarBarraDeEstado();

        private void FrmProveedoresProductos_Load(object sender, EventArgs e)
        {
            DgvProveedores.DataSource = bsProveedores;
            DgvProductos.DataSource = bsProductos;
            Utils.ConfDgv(DgvProveedores);
            Utils.ConfDgv(DgvProductos);
            GetData();
            ConfDgvProveedores();
            ConfDgvProductos();
        }

        private void GetData()
        {
            try
            {
                MDIPrincipal.ActualizarBarraDeEstado(Utils.clbdd);
                var ds = new ProductosxProveedoresRepository(cnStr).ObtenerProveedoresProductosDataSet();
                bsProveedores.DataSource = ds;
                bsProveedores.DataMember = "Proveedores";
                bsProductos.DataSource = bsProveedores;
                bsProductos.DataMember = "ProveedoresProductos";
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

        private void ConfDgvProveedores()
        {
            DgvProveedores.Columns["SupplierID"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            DgvProveedores.Columns["ContactTitle"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            DgvProveedores.Columns["City"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            DgvProveedores.Columns["Region"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            DgvProveedores.Columns["PostalCode"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            DgvProveedores.Columns["Country"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            DgvProveedores.Columns["Phone"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            DgvProveedores.Columns["Fax"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

            DgvProveedores.Columns["City"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            DgvProveedores.Columns["Region"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            DgvProveedores.Columns["PostalCode"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            DgvProveedores.Columns["Country"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            DgvProveedores.Columns["HomePage"].Visible = false;
            DgvProveedores.Columns["RowVersion"].Visible = false;

            DgvProveedores.Columns["SupplierID"].HeaderText = "ID";
            DgvProveedores.Columns["CompanyName"].HeaderText = "Nombre de compañía";
            DgvProveedores.Columns["ContactName"].HeaderText = "Nombre de contacto";
            DgvProveedores.Columns["ContactTitle"].HeaderText = "Título de contacto";
            DgvProveedores.Columns["Address"].HeaderText = "Dirección";
            DgvProveedores.Columns["City"].HeaderText = "Ciudad";
            DgvProveedores.Columns["Region"].HeaderText = "Región";
            DgvProveedores.Columns["PostalCode"].HeaderText = "Código postal";
            DgvProveedores.Columns["Country"].HeaderText = "País";
            DgvProveedores.Columns["Phone"].HeaderText = "Teléfono";
        }

        private void ConfDgvProductos()
        {
            DgvProductos.Columns["CategoryID"].Visible = false;
            DgvProductos.Columns["SupplierID"].Visible = false;

            DgvProductos.Columns["UnitPrice"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            DgvProductos.Columns["UnitsInStock"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            DgvProductos.Columns["UnitsOnOrder"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            DgvProductos.Columns["ReorderLevel"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            DgvProductos.Columns["ProductID"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            DgvProductos.Columns["UnitPrice"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            DgvProductos.Columns["UnitsInStock"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            DgvProductos.Columns["UnitsOnOrder"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            DgvProductos.Columns["ReorderLevel"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            DgvProductos.Columns["Discontinued"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            DgvProductos.Columns["CategoryName"].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
            DgvProductos.Columns["CompanyName"].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;

            DgvProductos.Columns["UnitPrice"].DefaultCellStyle.Format = "c";
            DgvProductos.Columns["UnitsInStock"].DefaultCellStyle.Format = "n0";
            DgvProductos.Columns["UnitsOnOrder"].DefaultCellStyle.Format = "n0";
            DgvProductos.Columns["ReorderLevel"].DefaultCellStyle.Format = "n0";

            DgvProductos.Columns["ProductID"].HeaderText = "ID";
            DgvProductos.Columns["ProductName"].HeaderText = "Producto";
            DgvProductos.Columns["QuantityPerUnit"].HeaderText = "Cantidad por unidad";
            DgvProductos.Columns["UnitPrice"].HeaderText = "Precio";
            DgvProductos.Columns["UnitsInStock"].HeaderText = "Unidades en inventario";
            DgvProductos.Columns["UnitsOnOrder"].HeaderText = "Unidades en pedido";
            DgvProductos.Columns["ReorderLevel"].HeaderText = "Nivel de reorden";
            DgvProductos.Columns["Discontinued"].HeaderText = "Descontinuado";
            DgvProductos.Columns["CategoryName"].HeaderText = "Categoría";
            DgvProductos.Columns["Description"].HeaderText = "Descripción de categoría";
            DgvProductos.Columns["CompanyName"].HeaderText = "Proveedor";
        }

        private void DgvProveedores_SelectionChanged(object sender, EventArgs e) => MDIPrincipal.ActualizarBarraDeEstado($"Se encontraron {DgvProveedores.RowCount} registros en proveedores y {DgvProductos.RowCount} registros de productos; del proveedor {DgvProveedores.CurrentRow.Cells["CompanyName"].Value}");

        private void DgvProveedores_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e) => MDIPrincipal.ActualizarBarraDeEstado($"Se encontraron {DgvProveedores.RowCount} registros en proveedores");
    }
}
