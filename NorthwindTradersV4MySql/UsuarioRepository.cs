using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.IO.Pipelines;

namespace NorthwindTradersV4MySql
{
    internal class UsuarioRepository
    {
        private readonly string _connectionString;

        public UsuarioRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public DataTable ObtenerUsuarios(DtoUsuariosBuscar dtoUsuariosBuscar)
        {
            var dt = new DataTable();
            string query;
            if (dtoUsuariosBuscar == null)
                query = "SELECT Id, Paterno, Materno, Nombres, Usuario, Password, FechaCaptura, FechaModificacion, Estatus FROM Usuarios ORDER BY Id DESC LIMIT 20;";
            else
                query = "spUsuariosBuscar";
            try
            {
                using (var cn = new MySqlConnection(_connectionString))
                using (var cmd = new MySqlCommand(query, cn))
                {
                    if (dtoUsuariosBuscar != null)
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("pIdIni", dtoUsuariosBuscar.IdIni);
                        cmd.Parameters.AddWithValue("pIdFin", dtoUsuariosBuscar.IdFin);
                        cmd.Parameters.AddWithValue("pPaterno", dtoUsuariosBuscar.Paterno);
                        cmd.Parameters.AddWithValue("pMaterno", dtoUsuariosBuscar.Materno);
                        cmd.Parameters.AddWithValue("pNombres", dtoUsuariosBuscar.Nombres);
                        cmd.Parameters.AddWithValue("pUsuario", dtoUsuariosBuscar.Usuario);
                    }
                    using (var da = new MySqlDataAdapter(cmd))
                        da.Fill(dt);
                }
            }
            catch (MySqlException ex)
            {
                throw new Exception("Error al obtener los usuarios: " + ex.Message);
            }
            return dt;
        }

        public bool ValidarExisteUsuario(string usuario)
        {
            bool existe = false;
            using (var cn = new MySqlConnection(_connectionString))
            using (var cmd = new MySqlCommand("SELECT COUNT(*) FROM Usuarios WHERE Usuario = @Usuario;", cn))
            {
                cmd.Parameters.AddWithValue("@Usuario", usuario);
                cn.Open();
                byte count = Convert.ToByte(cmd.ExecuteScalar());
                if (count > 0)
                    existe = true;
            }
            return existe;
        }

        public byte Insertar(Usuario usuario)
        {
            byte numRegs = 0;
            try
            {
                using (var cn = new MySqlConnection(_connectionString))
                using (var cmd = new MySqlCommand("spUsuarioInsertar", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("pId", 0);
                    cmd.Parameters["pId"].Direction = ParameterDirection.Output;
                    cmd.Parameters.AddWithValue("pPaterno", usuario.Paterno);
                    cmd.Parameters.AddWithValue("pMaterno", usuario.Materno);
                    cmd.Parameters.AddWithValue("pNombres", usuario.Nombres);
                    cmd.Parameters.AddWithValue("pUsuario", usuario.User);
                    cmd.Parameters.AddWithValue("pPassword", usuario.Password);
                    cmd.Parameters.AddWithValue("pFechaCaptura", usuario.FechaCaptura);
                    cmd.Parameters.AddWithValue("pFechaModificacion", usuario.FechaModificacion);
                    cmd.Parameters.AddWithValue("pEstatus", usuario.Estatus);
                    cn.Open();
                    numRegs = Convert.ToByte(cmd.ExecuteNonQuery());
                    usuario.Id = Convert.ToInt32(cmd.Parameters["pId"].Value);
                }
            }
            catch (MySqlException ex) when (ex.Number == 1062) // Duplicate entry
            {
                throw new Exception("El usuario ya existe. Por favor, elije otro.");
            }
            catch (MySqlException ex)
            {
                throw new Exception("Error al insertar el usuario: " + ex.Message);
            }
            return numRegs;
        }

        public byte Actualizar(Usuario usuario)
        {
            byte numRegs = 0;
            try
            {
                using (var cn = new MySqlConnection(_connectionString))
                using (var cmd = new MySqlCommand("spUsuarioActualizar", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("pId", usuario.Id);
                    cmd.Parameters.AddWithValue("pPaterno", usuario.Paterno);
                    cmd.Parameters.AddWithValue("pMaterno", usuario.Materno);
                    cmd.Parameters.AddWithValue("pNombres", usuario.Nombres);
                    cmd.Parameters.AddWithValue("pUsuario", usuario.User);
                    cmd.Parameters.AddWithValue("pPassword", usuario.Password);
                    cmd.Parameters.AddWithValue("pFechaModificacion", usuario.FechaModificacion);
                    cmd.Parameters.AddWithValue("pEstatus", usuario.Estatus);
                    cn.Open();
                    numRegs = Convert.ToByte(cmd.ExecuteNonQuery());
                }
            }
            catch (MySqlException ex) when (ex.Number == 1062) // Duplicate entry
            {
                throw new Exception("El usuario ya existe. Por favor, elije otro.");
            }
            catch (MySqlException ex)
            {
                throw new Exception("Error al modificar el usuario: " + ex.Message);
            }
            return numRegs;
        }

        public byte Eliminar(Usuario usuario)
        {
            byte numRegs = 0;
            try
            {
                using (var cn = new MySqlConnection(_connectionString))
                using (var cmd = new MySqlCommand("spUsuarioEliminar", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("pId", usuario.Id);
                    cmd.Parameters.AddWithValue("pRowsAfectados", 0);
                    cmd.Parameters["pRowsAfectados"].Direction = ParameterDirection.Output;
                    cn.Open();
                    cmd.ExecuteNonQuery();
                    numRegs = Convert.ToByte(cmd.Parameters["pRowsAfectados"].Value);
                }
            }
            catch (MySqlException ex)
            {
                throw new Exception("Error al eliminar el usuario: " + ex.Message);
            }
            return numRegs;
        }

        public int ValidarUsuario(string usuario, string password)
        {
            int idUsuario = 0;
            try
            {
                using (var cn = new MySqlConnection(_connectionString))
                using (var cmd = new MySqlCommand("SELECT Id FROM Usuarios WHERE Usuario = @Usuario AND Password = @Password AND Estatus = 1", cn))
                {
                    cmd.Parameters.AddWithValue("@Usuario", usuario);
                    cmd.Parameters.AddWithValue("@Password", password);
                    cn.Open();
                    object result = cmd.ExecuteScalar();
                    if (!(result == null || result == DBNull.Value))
                        idUsuario = Convert.ToInt32(result);
                }
            }
            catch (MySqlException ex)
            {
                throw new Exception("Error al obtener el usuario: " + ex.Message);
            }
            return idUsuario;
        }

        public byte ActualizarContrasena(string usuario, string nuevaContrasena)
        {
            byte numRegs = 0;
            try
            {
                using (var cn = new MySqlConnection(_connectionString))
                using (var cmd = new MySqlCommand("UPDATE Usuarios SET Password = @password WHERE Usuario = @usuario AND Estatus = 1", cn))
                {
                    cmd.Parameters.AddWithValue("@Usuario", usuario);
                    cmd.Parameters.AddWithValue("@password", nuevaContrasena);
                    cn.Open();
                    numRegs = Convert.ToByte(cmd.ExecuteNonQuery());
                }
            }
            catch (MySqlException ex)
            {
                throw new Exception("Error al cambiar la contraseña: " + ex.Message);
            }
            return numRegs;
        }

        public byte ValidarContrasenaActual(string usuario, string contrasenaActual)
        {
            byte numRegs = 0;
            try
            {
                using (var cn = new MySqlConnection(_connectionString))
                using (var cmd = new MySqlCommand("SELECT COUNT(0) FROM Usuarios WHERE Usuario = @usuario AND Password = @Password AND Estatus = 1", cn))
                {
                    cmd.Parameters.AddWithValue("@usuario", usuario);
                    cmd.Parameters.AddWithValue("@Password", contrasenaActual);
                    cn.Open();
                    numRegs = Convert.ToByte(cmd.ExecuteScalar());
                }
            }
            catch (MySqlException ex)
            {
                throw new Exception("Error al validar la contraseña actual: " + ex.Message);
            }
            return numRegs;
        }
    }
}
