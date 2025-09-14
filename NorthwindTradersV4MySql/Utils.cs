using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NorthwindTradersV4MySql
{
    internal class Utils
    {
        #region VariablesGlobales
        public static string clbdd = "Consultando la base de datos... ";
        public static string oueclbdd = "Ocurrio un error con la base de datos:\n";
        public static string oue = "Ocurrio un error:\n";
        public static string nwtr = "» Northwind Traders Ver 4.0 MySql «";
        public static string preguntaCerrar = "¿Esta seguro de querer cerrar el formulario?, si responde SI, se perderan los datos no guardados";
        public static string insertandoRegistro = "Insertando registro en la base de datos...";
        public static string modificandoRegistro = "Modificando registro en la base de datos...";
        public static string eliminandoRegistro = "Eliminando registro en la base de datos...";
        public static string errorCriterioSelec = "Error: Proporcione los criterios de selección";
        public static string noDatos = "No se encontraron datos para mostrar en el reporte";
        #endregion

        // <summary>
        /// Elimina un posible encabezado OLE de un array de bytes de imagen.
        /// </summary>
        /// <param name="oleBytes">Bytes crudos con encabezado OLE + datos de imagen.</param>
        /// <param name="employeeId">
        /// ID del empleado; si es menor o igual a 9, se asume encabezado de 78 bytes.
        /// </param>
        /// <returns>Bytes de la imagen sin encabezado OLE.</returns>
        public static byte[] StripOleHeader(byte[] oleBytes, int employeeId)
        {
            if (oleBytes == null || oleBytes.Length == 0)
                return oleBytes;
            const int OLE_HEADER_LENGTH = 78; // Tamaño típico del encabezado OLE
            int offset = (employeeId <= 8 && oleBytes.Length > OLE_HEADER_LENGTH)
            ? OLE_HEADER_LENGTH
            : 0;

            int length = oleBytes.Length - offset;
            if (length <= 0)
                return Array.Empty<byte>();

            var imageBytes = new byte[length];
            Buffer.BlockCopy(oleBytes, offset, imageBytes, 0, length);
            return imageBytes;
        }

        public static byte[] ImageToByteArray(Image image)
        {
            byte[] foto = null;
            if (image != null)
            {
                using (var ms = new System.IO.MemoryStream())
                {
                    image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    foto = ms.ToArray();
                }
            }
            return foto;
        }

        public static void ValidaTxtBIdIni(TextBox txtBIdIni, TextBox txtBIdFin)
        {
            int numBIdIni = 0, numBIdFin = 0;
            if (txtBIdIni.Text != "")
            {
                if (int.TryParse(txtBIdIni.Text, out int numTxtBIdIni))
                {
                    if (numTxtBIdIni == 0)
                    {
                        MessageBox.Show("El valor del Id inicial no puede ser cero", Utils.nwtr, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtBIdIni.Text = "1";
                        txtBIdIni.Focus();
                        return;
                    }
                    numBIdIni = numTxtBIdIni;
                    if (txtBIdFin.Text == "")
                        txtBIdFin.Text = txtBIdIni.Text;
                }
                else
                    MessageBox.Show("Por favor ingrese un número valido", Utils.nwtr, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            if (txtBIdFin.Text != "")
            {
                if (int.TryParse(txtBIdFin.Text, out int numTxtBIdFin))
                {
                    numBIdFin = numTxtBIdFin;
                }
                else
                    MessageBox.Show("Por favor ingrese un número valido", Utils.nwtr, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            if (numBIdFin < numBIdIni)
                txtBIdFin.Text = txtBIdIni.Text;
        }

        public static void ValidaTxtBIdFin(TextBox txtBIdIni, TextBox txtBIdFin)
        {
            int numBIdIni = 0, numBIdFin = 0;
            if (txtBIdIni.Text != "")
            {
                if (int.TryParse(txtBIdIni.Text, out int numTxtBIdIni))
                {
                    numBIdIni = numTxtBIdIni;
                }
                else
                    MessageBox.Show("Por favor ingrese un número valido", Utils.nwtr, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                txtBIdIni.Text = txtBIdFin.Text;
            }
            if (txtBIdFin.Text != "")
            {
                if (int.TryParse(txtBIdFin.Text, out int numTxtBIdFin))
                {
                    if (numTxtBIdFin == 0)
                    {
                        MessageBox.Show("El valor del Id final no puede ser cero", Utils.nwtr, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtBIdFin.Text = "1";
                        txtBIdFin.Focus();
                        Utils.ValidaTxtBIdIni(txtBIdIni, txtBIdFin);
                        return;
                    }
                    numBIdFin = numTxtBIdFin;
                }
                else
                    MessageBox.Show("Por favor ingrese un número valido", Utils.nwtr, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            if (numBIdIni > numBIdFin)
                txtBIdIni.Text = txtBIdFin.Text;
        }

        public static void ValidarDigitosConPunto(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
                e.Handled = true;
            // valida que exista solo un punto decimal
            if (e.KeyChar == '.' && (sender as TextBox).Text.IndexOf('.') > -1)
                e.Handled = true;
            // forzar que solo se capturen como máximo dos dígitos despues del punto decimal
            if (e.KeyChar != 8)
            {
                string numsDecimales = (sender as TextBox).Text + e.KeyChar;
                if ((sender as TextBox).Text.IndexOf('.') > -1)
                {
                    int posComienzo = (sender as TextBox).Text.IndexOf('.');
                    numsDecimales = numsDecimales.Substring(posComienzo, numsDecimales.Length - posComienzo);
                    if (numsDecimales.Length > 3)
                        e.Handled = true;
                }
            }
        }

        public static void ValidarDigitosSinPunto(object sender, KeyPressEventArgs e)
        {
            e.Handled = !(char.IsDigit(e.KeyChar) || (int)e.KeyChar == 8);
        }

        public static void ConfDgv(DataGridView dgv)
        {
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.AllowUserToOrderColumns = false;
            dgv.MultiSelect = false;
            dgv.ReadOnly = true;
            dgv.EnableHeadersVisualStyles = false;
            dgv.AlternatingRowsDefaultCellStyle.BackColor = SystemColors.GradientActiveCaption;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = SystemColors.GradientActiveCaption;
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold);
            dgv.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular);
            dgv.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.BackgroundColor = SystemColors.Control;
            dgv.RowHeadersVisible = false;
            dgv.BorderStyle = BorderStyle.FixedSingle;
            dgv.AutoResizeColumns();
        }

        public static void MsgCatchOueclbdd(MySqlException ex)
        {
            // Códigos típicos de error de conexión en MySQL
            int[] codigosConexion = { 1040, 1042, 1043, 1044, 1045, 1046, 1049, 1050, 1051, 1054, 1064, 1105, 1114, 1129, 1130, 1153, 1159, 1160, 1161, 1205, 1227, 1235, 1451, 1452, 2002, 2003, 2005 };

            if (codigosConexion.Contains(ex.Number)) // Error de conexión
                MessageBox.Show("No se pudo conectar a la base de datos.\n\nVerifique\n"+ex.Message, Utils.nwtr, MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
                MessageBox.Show(Utils.oueclbdd + ex.Message, Utils.nwtr, MessageBoxButtons.OK, MessageBoxIcon.Error);
            MDIPrincipal.ActualizarBarraDeEstado();
        }

        public static void MsgCatchOue(Exception ex)
        {
            MessageBox.Show(Utils.oue + ex.Message, Utils.nwtr, MessageBoxButtons.OK, MessageBoxIcon.Error);
            MDIPrincipal.ActualizarBarraDeEstado();
        }

        public static void MsgCatchOueclbdd(SqlException ex)
        {
            if (ex.Number == 53) // Error de conexión
                MessageBox.Show("No se pudo conectar a la base de datos.\n\nVerifique su conexión.", Utils.nwtr, MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
                MessageBox.Show(Utils.oueclbdd + ex.Message, Utils.nwtr, MessageBoxButtons.OK, MessageBoxIcon.Error);
            MDIPrincipal.ActualizarBarraDeEstado();
        }

        public static DialogResult MensajeCerrarForm()
        {
            DialogResult resultado = MessageBox.Show(preguntaCerrar, Utils.nwtr, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            return resultado;
        }

        public static void Mensaje(string mensaje)
        {
            MessageBox.Show(mensaje, nwtr, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static void GrbPaint(Form form, object sender, PaintEventArgs e)
        {
            GroupBox groupBox = sender as GroupBox;
            Utils.DrawGroupBox(form, groupBox, e.Graphics, Color.Black, Color.Black);
        }

        public static void GrbPaint2(Form form, object sender, PaintEventArgs e)
        {
            GroupBox groupBox = sender as GroupBox;
            Utils.DrawGroupBox(form, groupBox, e.Graphics, Color.Black, Color.LightSlateGray);
        }

        public static void DrawGroupBox(Form form, GroupBox box, Graphics g, Color textColor, Color borderColor)
        {
            if (box != null)
            {
                Brush textBrush = new SolidBrush(textColor);
                Brush borderBrush = new SolidBrush(borderColor);
                Pen borderPen = new Pen(borderBrush);
                SizeF strSize = g.MeasureString(box.Text, box.Font);
                Rectangle rect = new Rectangle(box.ClientRectangle.X,
                                                box.ClientRectangle.Y + (int)(strSize.Height / 2),
                                                box.ClientRectangle.Width - 1,
                                                box.ClientRectangle.Height - (int)(strSize.Height / 2) - 1);
                // Clear text and border
                g.Clear(form.BackColor);
                // Draw text
                g.DrawString(box.Text, box.Font, textBrush, box.Padding.Left, 0);
                // Drawing border
                // Left
                g.DrawLine(borderPen, rect.Location, new Point(rect.X, rect.Y + rect.Height));
                // Right
                g.DrawLine(borderPen, new Point(rect.X + rect.Width, rect.Y), new Point(rect.X + rect.Width, rect.Y + rect.Height));
                // Bottom
                g.DrawLine(borderPen, new Point(rect.X, rect.Y + rect.Height), new Point(rect.X + rect.Width, rect.Y + rect.Height));
                // Top1
                g.DrawLine(borderPen, new Point(rect.X, rect.Y), new Point(rect.X + box.Padding.Left, rect.Y));
                // Top2
                g.DrawLine(borderPen, new Point(rect.X + box.Padding.Left + (int)(strSize.Width), rect.Y), new Point(rect.X + rect.Width, rect.Y));
            }
        }

        public static void CerrarFormularios()
        {
            //Declaramos una lista de tipo Form
            List<Form> formularios = new List<Form>();
            //Recorremos Application.OpenForms el cual tiene la lista de formularios y metemos todos los forms en la lista que declarmos
            foreach (Form form in Application.OpenForms)
                formularios.Add(form);
            // recorremos la lista de formularios
            for (int i = 0; i < formularios.Count; i++)
            {
                // validamos que el nombre de los formularios sean distintos al unico formulario que queremos abierto
                if (formularios[i].Name != "MDIPrincipal")
                    formularios[i].Close();
                else
                    MDIPrincipal.ActualizarBarraDeEstado();
            }

        }
    }
}
