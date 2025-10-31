using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Transactions;

namespace NorthwindTradersV4MySql
{
    internal class PermisoRepository
    {
        private readonly string _connectionString;

        public PermisoRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public DataTable LlenarListBoxCatalogo()
        {
            DataTable dt = new DataTable();
            try
            {
                using (var cn = new MySqlConnection(_connectionString))
                using (var cmd = new MySqlCommand("Select PermisoId, Descripción from CatalogoPermisos Where Estatus = 1 Order By PermisoId", cn))
                using (var da = new MySqlDataAdapter(cmd))
                    da.Fill(dt);
            }
            catch (MySqlException ex)
            {
                throw new Exception("Error al llenar el ListBox de permisos: " + ex.Message);
            }
            return dt;
        }

        public DataTable ObtenerUsuarios(DtoUsuariosBuscar dtoUsuariosBuscar)
        {
            DataTable dt = new DataTable();
            try
            {
                using (var cn = new MySqlConnection(_connectionString))
                using (var cmd = new MySqlCommand())
                {
                    cmd.Connection = cn;
                    if (dtoUsuariosBuscar == null)
                        cmd.CommandText = "Select Id, Paterno, Materno, Nombres, Usuario, FechaCaptura, FechaModificacion, Estatus from Usuarios Where Estatus = 1 Order By Id Desc Limit 20;";
                    else
                    {
                        // 1) Base de la consulta
                        var sql = new StringBuilder();
                        sql.AppendLine("Select ");
                        sql.AppendLine("Id, Paterno, Materno, Nombres, Usuario, FechaCaptura, FechaModificacion, Estatus ");
                        sql.AppendLine("From Usuarios ");
                        sql.AppendLine("Where 1 = 1"); // Facilita agregar AND condicionales
                        // 2) Filtros opcionales
                        if (dtoUsuariosBuscar.IdIni > 0)
                        {
                            sql.AppendLine(" And Id Between @IdIni And @IdFin ");
                            cmd.Parameters.AddWithValue("@IdIni", dtoUsuariosBuscar.IdIni);
                            cmd.Parameters.AddWithValue("@IdFin", dtoUsuariosBuscar.IdFin);
                        }
                        if (!string.IsNullOrWhiteSpace(dtoUsuariosBuscar.Paterno))
                        {
                            sql.AppendLine(" And Paterno Like @Paterno ");
                            cmd.Parameters.AddWithValue("@Paterno", $"%{dtoUsuariosBuscar.Paterno.Trim()}%");
                        }
                        if (!string.IsNullOrWhiteSpace(dtoUsuariosBuscar.Materno))
                        {
                            sql.AppendLine(" And Materno Like @Materno ");
                            cmd.Parameters.AddWithValue("@Materno", $"%{dtoUsuariosBuscar.Materno.Trim()}%");
                        }
                        if (!string.IsNullOrWhiteSpace(dtoUsuariosBuscar.Nombres))
                        {
                            sql.AppendLine(" And Nombres Like @Nombres ");
                            cmd.Parameters.AddWithValue("@Nombres", $"%{dtoUsuariosBuscar.Nombres.Trim()}%");
                        }
                        if (!string.IsNullOrWhiteSpace(dtoUsuariosBuscar.Usuario))
                        {
                            sql.AppendLine(" And Usuario Like @Usuario ");
                            cmd.Parameters.AddWithValue("@Usuario", $"%{dtoUsuariosBuscar.Usuario.Trim()}%");
                        }
                        sql.AppendLine(" And Estatus = 1 ");
                        // 3) Ordenamiento
                        sql.AppendLine(" Order By Paterno, Materno, Nombres, Usuario;");
                        cmd.CommandText = sql.ToString();
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

        public DataTable LlenarListBoxConcedidos(int idUsuario)
        {
            DataTable dt = new DataTable();
            try
            {
                using (var cn = new MySqlConnection(_connectionString))
                using (var cmd = new MySqlCommand("Select P.PermisoId, CP.Descripción from Permisos P Inner join CatalogoPermisos CP On P.PermisoId = CP.PermisoId Where P.UsuarioId = @UsuarioId And CP.Estatus = 1 Order By P.PermisoId", cn))
                {
                    cmd.Parameters.AddWithValue("@UsuarioId", idUsuario);
                    using (var da = new MySqlDataAdapter(cmd))
                        da.Fill(dt);
                }
            }
            catch (MySqlException ex)
            {
                throw new Exception("Error al llenar el ListBox de permisos concedidos: " + ex.Message);
            }
            return dt;
        }

        public void InsertarPermisos(int idUsuario, IEnumerable<int> permisosIds)
        {
            try
            {
                using (var cn = new MySqlConnection(_connectionString))
                {
                    cn.Open();
                    using (var transaction = cn.BeginTransaction())
                    {
                        using (var cmd = cn.CreateCommand())
                        {
                            cmd.Transaction = transaction;
                            cmd.CommandText = "INSERT INTO Permisos (UsuarioId, PermisoId) VALUES (@UsuarioId, @PermisoId);";
                            cmd.Parameters.Add("@UsuarioId", MySqlDbType.Int32);
                            var pPermiso = cmd.Parameters.Add("@PermisoId", MySqlDbType.Int32);
                            cmd.Parameters["@UsuarioId"].Value = idUsuario;
                            try
                            {
                                cmd.Prepare();
                                foreach (var pid in permisosIds)
                                {
                                    pPermiso.Value = pid;
                                    cmd.ExecuteNonQuery();
                                }
                                transaction.Commit();
                            }
                            catch
                            {
                                try { transaction.Rollback(); } catch { /* opcional: log */ }
                                throw;
                            }
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        public int EliminarPermisos(int idUsuario)
        {
            int filasAfectadas = 0;
            try
            {
                using (var cn = new MySqlConnection(_connectionString))
                using (var cmd = new MySqlCommand("DELETE FROM Permisos WHERE UsuarioId = @UsuarioId;", cn))
                {
                    cmd.Parameters.AddWithValue("@UsuarioId", idUsuario);
                    cn.Open();
                    filasAfectadas = cmd.ExecuteNonQuery();
                }
            }
            catch
            {
                throw;
            }
            return filasAfectadas;
        }

        public void InsertarPermiso(int idUsuario, int permisoId)
        {
            try
            {
                using (var cn = new MySqlConnection(_connectionString))
                using (var cmd = new MySqlCommand("INSERT INTO Permisos (UsuarioId, PermisoId) VALUES (@UsuarioId, @PermisoId);", cn))
                {
                    cmd.Parameters.AddWithValue("@UsuarioId", idUsuario);
                    cmd.Parameters.AddWithValue("@PermisoId", permisoId);
                    cn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (MySqlException ex) when (ex.Number == 1062)
            {
                // Ignorar error de clave duplicada (permiso ya asignado)
            }
            catch
            {
                throw;
            }
        }

        public void EliminarPermiso(int idUsuario, int permisoId)
        {
            try
            {
                using (var cn = new MySqlConnection(_connectionString))
                using (var cmd = new MySqlCommand("DELETE FROM Permisos WHERE UsuarioId = @UsuarioId AND PermisoId = @PermisoId;", cn))
                {
                    cmd.Parameters.AddWithValue("@UsuarioId", idUsuario);
                    cmd.Parameters.AddWithValue("@PermisoId", permisoId);
                    cn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
