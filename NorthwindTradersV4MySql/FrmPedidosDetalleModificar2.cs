using System;
using System.Windows.Forms;

namespace NorthwindTradersV4MySql
{
    public partial class FrmPedidosDetalleModificar2 : Form
    {

        public int ProductoId { get; set; }
        public string Producto { get; set; }
        public float Precio { get; set; }
        public short Cantidad { get; set; }
        public float Descuento { get; set; }
        public float Importe { get; set; }
        public short? UInventario { get; set; }
        short CantidadOld = 0;
        float DescuentoOld = 0;

        public FrmPedidosDetalleModificar2()
        {
            InitializeComponent();
        }

        private void FrmPedidosDetalleModificar2_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (int.Parse(txtCantidad.Text.Replace(",", "")) != CantidadOld || float.Parse(txtDescuento.Text) != DescuentoOld)
                if (Utils.MensajeQuestion(Utils.preguntaCerrar) == DialogResult.No)
                    e.Cancel = true;
        }

        private void btnCancelar_Click(object sender, EventArgs e) => this.Close();

        private void FrmPedidosDetalleModificar2_Load(object sender, EventArgs e)
        {
            txtProducto.Text = Producto;
            txtPrecio.Text = Precio.ToString("c");
            txtUinventario.Text = string.Format("{0:n0}", UInventario);
            txtCantidad.Text = Cantidad.ToString("n0");
            txtDescuento.Text = Descuento.ToString("n2");
            txtImporte.Text = Importe.ToString("c");
            CantidadOld = Cantidad;
            DescuentoOld = Descuento;
        }

        private void txtCantidad_Leave(object sender, EventArgs e)
        {
            if (ValidarControles())
                CalcularImporte();
        }

        private void txtDescuento_Leave(object sender, EventArgs e)
        {
            if (ValidarControles())
                CalcularImporte();
        }

        private bool ValidarControles()
        {
            txtCantidad.Text = txtCantidad.Text.Replace(",", "");
            bool valida = true;
            btnModificar.Enabled = false;
            errorProvider1.Clear();

            int cantidadInt = 0, unidadesInventario = 0;
            short cantidad = 0;
            float descuento = 0;

            // Validar txtCantidad
            if (!short.TryParse(txtCantidad.Text.Replace(",", ""), out cantidad))
            {
                errorProvider1.SetError(txtCantidad, "Ingrese una cantidad valida, la cantidad debe ser mayor que 1 y menor que 32,767");
                valida = false;
            }
            if (valida && cantidad == 0)
            {
                errorProvider1.SetError(txtCantidad, "Ingrese la cantidad");
                valida = false;
            }
            // Validar descuento
            if (string.IsNullOrWhiteSpace(txtDescuento.Text) || !float.TryParse(txtDescuento.Text, out descuento))
            {
                errorProvider1.SetError(txtDescuento, "Ingrese el descuento");
                valida = false;
            }
            else if (descuento > 1 || descuento < 0)
            {
                errorProvider1.SetError(txtDescuento, "El descuento no puede ser mayor que 1 o menor que 0");
                valida = false;
            }
            if (valida)
            {
                // Validar cantidad y unidades en inventario sean números válidos
                if (!int.TryParse(txtCantidad.Text.Replace(",", ""), out cantidadInt) || !int.TryParse(txtUinventario.Text.Replace(",", ""), out unidadesInventario))
                {
                    errorProvider1.SetError(txtCantidad, "Ingrese una cantidad valida");
                    valida = false;
                }
                // Verificar la disponibilidad en el inventario
                if (valida && UInventario != null)
                {
                    if (short.Parse(txtCantidad.Text) > short.Parse(txtUinventario.Text.Replace(",", "")))
                    {
                        errorProvider1.SetError(txtCantidad, "La cantidad de productos en el pedido excede el inventario disponible");
                        valida = false;
                    }
                }
                else if (UInventario == null)
                {
                    errorProvider1.SetError(txtCantidad, "Es posible que el producto haya sido eliminado por otro usuario en la red");
                    valida = false;
                }
            }
            // Habilitar el botón Modificar si las cantidades y descuento son válidos y han cambiado
            if (valida && (cantidad != CantidadOld || descuento != DescuentoOld))
                btnModificar.Enabled = true;
            return valida;
        }

        private void CalcularImporte()
        {
            Cantidad = short.Parse(txtCantidad.Text.Replace(",", ""));
            Descuento = float.Parse(txtDescuento.Text);
            Importe = (Precio * Cantidad) * (1 - Descuento);
            txtImporte.Text = Importe.ToString("c");
        }

        private void txtCantidad_KeyPress(object sender, KeyPressEventArgs e) => Utils.ValidarDigitosSinPunto(sender, e);

        private void txtDescuento_KeyPress(object sender, KeyPressEventArgs e) => Utils.ValidarDigitosConPunto(sender, e);

        private void btnModificar_Click(object sender, EventArgs e)
        {
            btnModificar.Enabled = false;
            // Asigno los valores en la pantalla a las propiedades del formulario
            Cantidad = short.Parse(txtCantidad.Text);
            Descuento = float.Parse(txtDescuento.Text);
            Importe = float.Parse(txtImporte.Text.Replace("$", ""));

            // Las siguientes dos lineas son necesarias para que se permita cerrar la ventana. 
            // ya que se validan las variables en FrmPedidosDetalleModificar_FormClosing
            CantidadOld = short.Parse(txtCantidad.Text);
            DescuentoOld = float.Parse(txtDescuento.Text);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
