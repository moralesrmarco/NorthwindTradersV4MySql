using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;

namespace NorthwindTradersV4MySql
{
    internal static class DataAccess
    {
        private readonly static string cnStr = ConfigurationManager.ConnectionStrings["NorthwindMySql"].ConnectionString;

        public static DataTable LlenarCbo(string storedProcedure, params MySqlParameter[] parameters)
        {
            var dt = new DataTable();
            try
            {
                using (var cn = new MySqlConnection(cnStr))
                using (var cmd = new MySqlCommand(storedProcedure, cn))
                using (var da = new MySqlDataAdapter(cmd))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (parameters != null && parameters.Length > 0)
                        cmd.Parameters.AddRange(parameters);
                    da.Fill(dt);
                }
            }
            catch (MySqlException ex)
            {
                throw new Exception("Error al llenar el ComboBox: " + ex.Message);
            }
            return dt;
        }
    }
}
