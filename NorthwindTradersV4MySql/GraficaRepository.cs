using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace NorthwindTradersV4MySql
{
    internal class GraficaRepository
    {
        private readonly string _connectionString;

        public GraficaRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public DataTable ObtenerAñosDePedidos()
        {
            var dt = new DataTable();
            string query = "SELECT DISTINCT YEAR(OrderDate) AS YearOrderDate FROM Orders WHERE OrderDate IS NOT NULL ORDER BY YearOrderDate DESC;";
            try
            {
                using (var cn = new MySqlConnection(_connectionString))
                using (var cmd = new MySqlCommand(query, cn))
                using (var da = new MySqlDataAdapter(cmd))
                {
                    da.Fill(dt);
                }
            }
            catch (MySqlException ex)
            {
                throw new Exception("Error al obtener los años de pedidos: " + ex.Message);
            }
            return dt;
        }

        public List<DtoVentasMensuales> ObtenerVentasMensuales(int year)
        {
            var lista = new List<DtoVentasMensuales>();
            const string sql = @"
                                WITH Meses AS (
                                SELECT 1 AS Mes UNION ALL SELECT 2 UNION ALL SELECT 3 UNION ALL
                                SELECT 4 UNION ALL SELECT 5 UNION ALL SELECT 6 UNION ALL
                                SELECT 7 UNION ALL SELECT 8 UNION ALL SELECT 9 UNION ALL
                                SELECT 10 UNION ALL SELECT 11 UNION ALL SELECT 12
                                ),
                                VentasMensuales AS (
                                SELECT 
                                    MONTH(o.OrderDate) AS Mes,
                                    SUM(od.UnitPrice * od.Quantity * (1 - od.Discount)) AS Total
                                FROM Orders AS o
                                INNER JOIN `Order Details` AS od
                                    ON o.OrderID = od.OrderID
                                WHERE YEAR(o.OrderDate) = @year
                                GROUP BY MONTH(o.OrderDate)
                                )
                                SELECT m.Mes, IFNULL(v.Total, 0) AS Total
                                FROM Meses AS m
                                LEFT JOIN VentasMensuales AS v
                                    ON m.Mes = v.Mes
                                ORDER BY m.Mes;
                            ";
            try
            {
                using (var cn = new MySqlConnection(_connectionString))
                using (var cmd = new MySqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@year", year);
                    cn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var dto = new DtoVentasMensuales
                            {
                                Mes = reader.GetInt32("Mes"),
                                Total = reader.GetDecimal("Total")
                            };
                            lista.Add(dto);
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                throw new Exception("Error al obtener las ventas mensuales: " + ex.Message);
            }
            return lista;
        }
    }
}
