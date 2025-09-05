using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Windows.Forms;

namespace NorthwindTradersV4MySql
{
    public class DatabaseHelper
    {
        //private readonly string _connectionString = "Server=localhost;Database=northwind;User ID=r...;Password=12...;";
        private readonly string _connectionString = ConfigurationManager.ConnectionStrings["NorthwindMySql"].ConnectionString;

        public void ProbarConexion()
        {
            using (var cn = new MySqlConnection(_connectionString))
            {
                try
                {
                    cn.Open();
                    // En este punto la conexión está abierta
                    MessageBox.Show(
                        "¡Conexión exitosa!",
                        Utils.nwtr,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );

                }
                catch (MySqlException ex)
                {
                    // Manejo de errores de conexión
                    MessageBox.Show(
                        $"Error de MySQL: {ex.Message}",
                        Utils.nwtr,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                }
                catch (Exception ex)
                {
                    // Manejo de otros errores
                    MessageBox.Show(
                        $"Ocurrió un error inesperado: {ex.Message}",
                        Utils.nwtr,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                }
                // Al salir del using, la conexión se cierra automáticamente
            }
        }

        public List<Dictionary<string, object>> EjecutarSelect(string sql, Dictionary<string, object> parametros = null)
        {
            var resultados = new List<Dictionary<string, object>>();

            try
            {
                var connString = ConfigurationManager
                    .ConnectionStrings["NorthwindMySql"]
                    .ConnectionString;

                using (var conn = new MySqlConnection(connString))
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    // Agregar parámetros si hay
                    if (parametros != null)
                    {
                        foreach (var par in parametros)
                        {
                            cmd.Parameters.AddWithValue(par.Key, par.Value ?? DBNull.Value);
                        }
                    }

                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var fila = new Dictionary<string, object>();
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                fila[reader.GetName(i)] = reader.IsDBNull(i)
                                    ? null
                                    : reader.GetValue(i);
                            }
                            resultados.Add(fila);
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show($"Error MySQL: {ex.Message}", Utils.nwtr,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error inesperado: {ex.Message}", Utils.nwtr,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return resultados;
        }

        public int EjecutarNoQuery(string sql, Dictionary<string, object> parametros = null)
        {
            int filasAfectadas = 0;

            try
            {
                var connString = ConfigurationManager
                    .ConnectionStrings["NorthwindMySql"]
                    .ConnectionString;

                using (var conn = new MySqlConnection(connString))
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    if (parametros != null)
                    {
                        foreach (var par in parametros)
                        {
                            cmd.Parameters.AddWithValue(par.Key, par.Value ?? DBNull.Value);
                        }
                    }

                    conn.Open();
                    filasAfectadas = cmd.ExecuteNonQuery();
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show($"Error MySQL: {ex.Message}", Utils.nwtr,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error inesperado: {ex.Message}", Utils.nwtr,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return filasAfectadas;
        }
    }
}
