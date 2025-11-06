using System;
using System.Windows.Forms;

namespace NorthwindTradersV4MySql
{
    internal static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            // Inicializar el usuario autenticado como null
            string usuarioAutenticado = null;
            int idUsuarioLogueado = 0;
            string nombreUsuarioLogueado = string.Empty;
            using (var login = new FrmLogin())
            {
                Application.Run(login);
                if (!login.IsAuthenticated)
                    // Si el usuario no se autentica, cerramos la aplicación.
                    return; 
                usuarioAutenticado = login.UsuarioLogueado;
                // Asignar el Id del usuario logueado
                idUsuarioLogueado = login.IdUsuarioLogueado;
                nombreUsuarioLogueado = login.NombreUsuarioLogueado;
            }
            // Instanciar el MDIPrincipal, inyectar el usuario y arrancar
            var mdi = new MDIPrincipal();
            mdi.UsuarioLogueado = usuarioAutenticado;
            mdi.IdUsuarioLogueado = idUsuarioLogueado;
            mdi.NombreUsuarioLogueado = nombreUsuarioLogueado;
            Application.Run(mdi);
        }
    }
}
