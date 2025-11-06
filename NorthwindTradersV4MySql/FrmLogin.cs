using System;
using System.Configuration;
using System.Windows.Forms;

namespace NorthwindTradersV4MySql
{
    public partial class FrmLogin : Form
    {
        private readonly string cnStr = ConfigurationManager.ConnectionStrings["NorthwindMySql"].ConnectionString;
        public bool IsAuthenticated { get; private set; } = false;
        public string UsuarioLogueado;
        public int IdUsuarioLogueado;
        public string NombreUsuarioLogueado;
        bool _imagenMostrada = true;
        byte numeroIntentos = 0;

        public FrmLogin()
        {
            InitializeComponent();
            this.Text = Utils.nwtr;
        }

        private void btnEntrar_Click(object sender, EventArgs e)
        {
            var usuario = txtUsuario.Text.Trim();
            var pass = txtPwd.Text.Trim();
            if (ValidateUser(usuario, pass))
            {
                IsAuthenticated = true;
                UsuarioLogueado = usuario;
                this.Close();
            }
            else
            {
                numeroIntentos++;
                if (numeroIntentos >= 3)
                {
                    Utils.MensajeError("Demasiados intentos fallidos. La aplicación se cerrará.");
                    Application.Exit();
                    return;
                }
                Utils.MensajeExclamation("Error de autenticación.\nUsuario o contraseña incorrectos.");
                txtPwd.Clear();
                txtPwd.Focus();
            }
        }

        private bool ValidateUser(string usuario, string pass)
        {
            try
            {
                string hashed = Utils.ComputeSha256Hash(pass);
                string nombreUsuarioLogueado;
                IdUsuarioLogueado = new UsuarioRepository(cnStr).ValidarUsuario(usuario, hashed, out nombreUsuarioLogueado);
                NombreUsuarioLogueado = nombreUsuarioLogueado;
                return IdUsuarioLogueado > 0;
            }
            catch (Exception ex)
            {
                Utils.MsgCatchOue(ex);
                return false;
            }
        }

        private void btnTogglePwd_Click(object sender, EventArgs e)
        {
            _imagenMostrada = !_imagenMostrada;
            txtPwd.UseSystemPasswordChar = !txtPwd.UseSystemPasswordChar;
            btnTogglePwd.Image = _imagenMostrada ? Properties.Resources.mostrarCh : Properties.Resources.ocultarCh;
        }
    }
}
