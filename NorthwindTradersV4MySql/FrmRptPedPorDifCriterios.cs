// https://www.youtube.com/watch?v=2-YkNo1Os3Y&list=PL_1AVI-bgZKQ2MSDejVmaaxNenhETwwx_&index=7
// https://www.youtube.com/watch?v=7AvCaq7a1fc&list=PL_1AVI-bgZKQ2MSDejVmaaxNenhETwwx_&index=5
using Microsoft.Reporting.WinForms;
using System;
using System.Configuration;
using System.Data;
using System.Windows.Forms;

namespace NorthwindTradersV4MySql
{
    public partial class FrmRptPedPorDifCriterios: Form
    {
        private readonly string cnStr = ConfigurationManager.ConnectionStrings["NorthwindMySql"].ConnectionString;

        public FrmRptPedPorDifCriterios()
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
        }

        private void GrbPaint(object sender, PaintEventArgs e) => Utils.GrbPaint(this, sender, e);

        private void FrmRptPedPorDifCriterios_FormClosed(object sender, FormClosedEventArgs e) => MDIPrincipal.ActualizarBarraDeEstado();

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtBIdInicial.Text = txtBIdFinal.Text = txtBCliente.Text = txtBEmpleado.Text = txtBCompañiaT.Text = txtBDirigidoa.Text = "";
            dtpBFPedidoIni.Value = dtpBFPedidoFin.Value = dtpBFRequeridoIni.Value = dtpBFRequeridoFin.Value = dtpBFEnvioIni.Value = dtpBFEnvioFin.Value = DateTime.Today;
            dtpBFPedidoIni.Checked = dtpBFPedidoFin.Checked = dtpBFRequeridoIni.Checked = dtpBFRequeridoFin.Checked = dtpBFEnvioIni.Checked = dtpBFEnvioFin.Checked = false;
            chkBFPedidoNull.Checked = chkBFRequeridoNull.Checked = chkBFEnvioNull.Checked = false;
        }

        private void btnMostrarRep_Click(object sender, EventArgs e)
        {
            try
            {
                string subtitulo = string.Empty;
                if (txtBIdInicial.Text != "")
                    subtitulo += $"[Id inicial: {txtBIdInicial.Text}] - [Id final: {txtBIdFinal.Text}] ";
                if (txtBCliente.Text != "")
                    subtitulo += $"[Cliente: %{txtBCliente.Text}%] ";
                if (dtpBFPedidoIni.Checked)
                    subtitulo += $"[Fecha de pedido inicial: {dtpBFPedidoIni.Value.ToShortDateString()}] - [Fecha de pedido final: {dtpBFPedidoFin.Value.ToShortDateString()}] ";
                if (chkBFPedidoNull.Checked)
                    subtitulo += "[Fecha de pedido inicial: Nulo] - [Fecha de pedido final: Nulo] ";
                if (dtpBFRequeridoIni.Checked)
                    subtitulo += $"[Fecha de entrega inicial: {dtpBFRequeridoIni.Value.ToShortDateString()}] - [Fecha de entrega final: {dtpBFRequeridoFin.Value.ToShortDateString()}] ";
                if (chkBFRequeridoNull.Checked)
                    subtitulo += "[Fecha de entrega inicial: Nulo] - [Fecha de entrega final: Nulo] ";
                if (dtpBFEnvioIni.Checked)
                    subtitulo += $"[Fecha de envío inicial: {dtpBFEnvioIni.Value.ToShortDateString()}] - [Fecha de envío final: {dtpBFEnvioFin.Value.ToShortDateString()}] ";
                if (chkBFEnvioNull.Checked)
                    subtitulo += "[Fecha de envío inicial: Nulo] - [Fecha de envío final: Nulo] ";
                if (txtBEmpleado.Text != "")
                    subtitulo += $"[Vendedor: %{txtBEmpleado.Text}%] ";
                if (txtBCompañiaT.Text != "")
                    subtitulo += $"[Transportista: %{txtBCompañiaT.Text}%] ";
                if (txtBDirigidoa.Text != "")
                    subtitulo += $"[Enviar a: %{txtBDirigidoa.Text}%]";
                if (subtitulo == "")
                    subtitulo = "Ningun criterio  de selección fue especificado ( incluye todos los registros de pedidos )";
                MDIPrincipal.ActualizarBarraDeEstado(Utils.clbdd);
                DtoPedidosBuscar dtoPedidosBuscar = new DtoPedidosBuscar
                {
                    IdIni = string.IsNullOrEmpty(txtBIdInicial.Text) ? 0 : int.Parse(txtBIdInicial.Text),
                    IdFin = string.IsNullOrEmpty(txtBIdFinal.Text) ? 0 : int.Parse(txtBIdFinal.Text),
                    Cliente = txtBCliente.Text,
                    FPedido = dtpBFPedidoIni.Checked && dtpBFPedidoFin.Checked,
                    FPedidoIni = dtpBFPedidoIni.Checked ? dtpBFPedidoIni.Value.Date : (DateTime?)null,
                    FPedidoFin = dtpBFPedidoFin.Checked ? dtpBFPedidoFin.Value.Date.AddDays(1) : (DateTime?)null,
                    FPedidoNull = chkBFPedidoNull.Checked,
                    FRequerido = dtpBFRequeridoIni.Checked && dtpBFRequeridoFin.Checked,
                    FRequeridoIni =  dtpBFRequeridoIni.Checked ? dtpBFRequeridoIni.Value.Date : (DateTime?)null,
                    FRequeridoFin = dtpBFRequeridoFin.Checked ? dtpBFRequeridoFin.Value.Date.AddDays(1) : (DateTime?)null,
                    FRequeridoNull = chkBFRequeridoNull.Checked,
                    FEnvio = dtpBFEnvioIni.Checked && dtpBFEnvioFin.Checked,
                    FEnvioIni = dtpBFEnvioIni.Checked ? dtpBFEnvioIni.Value.Date : (DateTime?)null,
                    FEnvioFin = dtpBFEnvioFin.Checked ? dtpBFEnvioFin.Value.Date.AddDays(1) : (DateTime?)null,
                    FEnvioNull = chkBFEnvioNull.Checked,
                    Empleado = txtBEmpleado.Text,
                    CompañiaT = txtBCompañiaT.Text,
                    DirigidoA = txtBDirigidoa.Text
                };
                DataTable dt = new PedidoRepository(cnStr).ObtenerPedidos2(dtoPedidosBuscar);
                MDIPrincipal.ActualizarBarraDeEstado($"Se encontraron {dt.Rows.Count} registros");
                if (dt.Rows.Count > 0)
                {
                    ReportDataSource reportDataSource = new ReportDataSource("DataSet1", dt);
                    reportViewer1.LocalReport.DataSources.Clear();
                    reportViewer1.LocalReport.DataSources.Add(reportDataSource);
                    ReportParameter reportParameter = new ReportParameter("subtitulo", subtitulo);
                    reportViewer1.LocalReport.SetParameters(new  ReportParameter[] { reportParameter });
                    reportViewer1.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(OrderDetailsSubReportProcessing);
                    reportViewer1.RefreshReport();
                }
                else
                {
                    reportViewer1.LocalReport.DataSources.Clear();
                    ReportDataSource reportDataSource = new ReportDataSource("DataSet1", new DataTable());
                    reportViewer1.LocalReport.DataSources.Add(reportDataSource);
                    ReportParameter reportParameter = new ReportParameter("subtitulo", subtitulo);
                    reportViewer1.LocalReport.SetParameters(new ReportParameter[] { reportParameter });
                    reportViewer1.RefreshReport();
                    MessageBox.Show(Utils.noDatos, Utils.nwtr, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex) { Utils.MsgCatchOue(ex); }
        }

        private void OrderDetailsSubReportProcessing(object sender, SubreportProcessingEventArgs e)
        {
            int orderID = int.Parse(e.Parameters["OrderID"].Values[0].ToString());
            DataTable dt = new PedidoRepository(cnStr).ObtenerDetallePedidoPorOrderID(orderID);
            ReportDataSource reportDataSource = new ReportDataSource("DataSet1", dt);
            e.DataSources.Add(reportDataSource);
        }

        private void txtBIdInicial_KeyPress(object sender, KeyPressEventArgs e) => Utils.ValidarDigitosSinPunto(sender, e);

        private void txtBIdInicial_Leave(object sender, EventArgs e) => Utils.ValidaTxtBIdIni(txtBIdInicial, txtBIdFinal);

        private void txtBIdFinal_KeyPress(object sender, KeyPressEventArgs e) => Utils.ValidarDigitosSinPunto(sender, e);

        private void txtBIdFinal_Leave(object sender, EventArgs e) => Utils.ValidaTxtBIdFin(txtBIdInicial, txtBIdFinal);

        private void chkBFPedidoNull_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBFPedidoNull.Checked)
            {
                dtpBFPedidoIni.Checked = false;
                dtpBFPedidoFin.Checked = false;
            }
        }

        private void chkBFRequeridoNull_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBFRequeridoNull.Checked)
            {
                dtpBFRequeridoIni.Checked = false;
                dtpBFRequeridoFin.Checked = false;
            }
        }

        private void chkBFEnvioNull_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBFEnvioNull.Checked)
            {
                dtpBFEnvioIni.Checked = false;
                dtpBFEnvioFin.Checked = false;
            }
        }

        private void dtpBFPedidoIni_ValueChanged(object sender, EventArgs e)
        {
            if (dtpBFPedidoIni.Checked)
            {
                dtpBFPedidoFin.Checked = true;
                chkBFPedidoNull.Checked = false;
            }
            else
                dtpBFPedidoFin.Checked = false;
        }

        private void dtpBFPedidoIni_Leave(object sender, EventArgs e)
        {
            if (dtpBFPedidoIni.Checked && dtpBFPedidoFin.Checked)
                if (dtpBFPedidoFin.Value < dtpBFPedidoIni.Value)
                    dtpBFPedidoFin.Value = dtpBFPedidoIni.Value;
        }

        private void dtpBFPedidoFin_ValueChanged(object sender, EventArgs e)
        {
            if (dtpBFPedidoFin.Checked)
            {
                dtpBFPedidoIni.Checked = true;
                chkBFPedidoNull.Checked = false;
            }
            else
                dtpBFPedidoIni.Checked = false;
        }

        private void dtpBFPedidoFin_Leave(object sender, EventArgs e)
        {
            if (dtpBFPedidoIni.Checked && dtpBFPedidoFin.Checked)
                if (dtpBFPedidoFin.Value < dtpBFPedidoIni.Value)
                    dtpBFPedidoIni.Value = dtpBFPedidoFin.Value;
        }

        private void dtpBFRequeridoIni_ValueChanged(object sender, EventArgs e)
        {
            if (dtpBFRequeridoIni.Checked)
            {
                dtpBFRequeridoFin.Checked = true;
                chkBFRequeridoNull.Checked = false;
            }
            else
                dtpBFRequeridoFin.Checked = false;
        }

        private void dtpBFRequeridoIni_Leave(object sender, EventArgs e)
        {
            if (dtpBFRequeridoIni.Checked && dtpBFRequeridoFin.Checked)
                if (dtpBFRequeridoFin.Value < dtpBFRequeridoIni.Value)
                    dtpBFRequeridoFin.Value = dtpBFRequeridoIni.Value;
        }

        private void dtpBFRequeridoFin_ValueChanged(object sender, EventArgs e)
        {
            if (dtpBFRequeridoFin.Checked)
            {
                dtpBFRequeridoIni.Checked = true;
                chkBFRequeridoNull.Checked = false;
            }
            else
                dtpBFRequeridoIni.Checked = false;
        }

        private void dtpBFRequeridoFin_Leave(object sender, EventArgs e)
        {
            if (dtpBFRequeridoIni.Checked && dtpBFRequeridoFin.Checked)
                if (dtpBFRequeridoFin.Value < dtpBFRequeridoIni.Value)
                    dtpBFRequeridoIni.Value = dtpBFRequeridoFin.Value;
        }

        private void dtpBFEnvioIni_ValueChanged(object sender, EventArgs e)
        {
            if (dtpBFEnvioIni.Checked)
            {
                dtpBFEnvioFin.Checked = true;
                chkBFEnvioNull.Checked = false;
            }
            else
                dtpBFEnvioFin.Checked = false;
        }

        private void dtpBFEnvioIni_Leave(object sender, EventArgs e)
        {
            if (dtpBFEnvioIni.Checked && dtpBFEnvioFin.Checked)
                if (dtpBFEnvioFin.Value < dtpBFEnvioIni.Value)
                    dtpBFEnvioFin.Value = dtpBFEnvioIni.Value;
        }

        private void dtpBFEnvioFin_ValueChanged(object sender, EventArgs e)
        {
            if (dtpBFEnvioFin.Checked)
            {
                dtpBFEnvioIni.Checked = true;
                chkBFEnvioNull.Checked = false;
            }
            else
                dtpBFEnvioIni.Checked = false;
        }

        private void dtpBFEnvioFin_Leave(object sender, EventArgs e)
        {
            if (dtpBFEnvioIni.Checked && dtpBFEnvioFin.Checked)
                if (dtpBFEnvioFin.Value < dtpBFEnvioIni.Value)
                    dtpBFEnvioIni.Value = dtpBFEnvioFin.Value;
        }
    }
}
