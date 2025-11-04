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

        public DataTable ObtenerTopProductos(int cantidad)
        {
            var dt = new DataTable();
            const string query = @"
                SELECT 
                    p.ProductName AS NombreProducto, 
                    SUM(od.Quantity) AS CantidadVendida
                FROM `Order Details` AS od
                INNER JOIN Products AS p ON od.ProductID = p.ProductID
                GROUP BY p.ProductName
                ORDER BY CantidadVendida DESC
                LIMIT @Cantidad;
                ";
            try
            {
                using (var cn = new MySqlConnection(_connectionString))
                using (var cmd = new MySqlCommand(query, cn))
                using (var da = new MySqlDataAdapter(cmd))
                {
                    cmd.Parameters.AddWithValue("@Cantidad", cantidad);
                    da.Fill(dt);
                }
            }
            catch (MySqlException ex)
            {
                throw new Exception("Error al obtener los productos más vendidos: " + ex.Message);
            }
            return dt;
        }

        public List<(string Vendedor, decimal TotalVentas)> ObtenerVentasPorVendedores()
        {
            var resultados = new List<(string Vendedor, decimal TotalVentas)>();

            string query = @"
                            SELECT 
                                CONCAT(e.FirstName, ' ', e.LastName) AS Vendedor,
                                SUM(od.UnitPrice * od.Quantity * (1 - od.Discount)) AS TotalVentas
                            FROM 
                                Employees e
                            JOIN 
                                Orders o ON e.EmployeeID = o.EmployeeID
                            JOIN 
                                `Order Details` od ON o.OrderID = od.OrderID
                            GROUP BY 
                                e.FirstName, e.LastName
                            ORDER BY 
                                TotalVentas DESC;
                            ";
            try
            {
                using (MySqlConnection cn = new MySqlConnection(_connectionString))
                using (MySqlCommand command = new MySqlCommand(query, cn))
                {
                    cn.Open();
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string vendedor = reader["Vendedor"].ToString();
                            decimal totalVentas = Convert.ToDecimal(reader["TotalVentas"]);
                            resultados.Add((vendedor, totalVentas));
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                throw new Exception("Error al obtener las ventas por vendedor: " + ex.Message);
            }
            return resultados;
        }

        public List<(string Vendedor, decimal TotalVentas)> ObtenerVentasPorVendedor(int anio)
        {
            var resultados = new List<(string Vendedor, decimal TotalVentas)>();

            string query = @"
            SELECT 
                CONCAT(e.FirstName, ' ', e.LastName) AS Vendedor,
                ROUND(SUM(od.UnitPrice * od.Quantity * (1 - od.Discount)), 2) AS TotalVentas
            FROM 
                Employees e
            JOIN 
                Orders o ON e.EmployeeID = o.EmployeeID
            JOIN 
                `Order Details` od ON o.OrderID = od.OrderID
            WHERE 
                YEAR(o.OrderDate) = @Anio
            GROUP BY 
                Vendedor
            ORDER BY 
                TotalVentas DESC;
        ";

            using (var cn = new MySqlConnection(_connectionString))
            using (var cmd = new MySqlCommand(query, cn))
            {
                cmd.Parameters.AddWithValue("@Anio", anio);
                cn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string vendedor = reader.GetString("Vendedor");
                        decimal totalVentas = reader.GetDecimal("TotalVentas");
                        resultados.Add((vendedor, totalVentas));
                    }
                }
            }
            return resultados;
        }

        public DataTable ObtenerVentasMensualesPorVendedorPorAño(int anio)
        {
            string query = @"
            SELECT 
                CONCAT(e.FirstName, ' ', e.LastName) AS Vendedor,
                MONTH(o.OrderDate) AS Mes,
                ROUND(SUM(od.UnitPrice * od.Quantity * (1 - od.Discount)), 2) AS TotalVentas
            FROM 
                Employees e
            JOIN 
                Orders o ON e.EmployeeID = o.EmployeeID
            JOIN 
                `Order Details` od ON o.OrderID = od.OrderID
            WHERE 
                YEAR(o.OrderDate) = @Anio
            GROUP BY 
                Vendedor, Mes
            ORDER BY 
                Vendedor, Mes;
        ";
            var dt = new DataTable();

            using (var cn = new MySqlConnection(_connectionString))
            using (var cmd = new MySqlCommand(query, cn))
            {
                cmd.Parameters.AddWithValue("@Anio", anio);
                using (var da = new MySqlDataAdapter(cmd))
                    da.Fill(dt);
            }
            return dt;
        }

        public DataTable ObtenerVentasMensualesPorVendedor(int anio)
        {
            string query = @"
            SELECT 
                CONCAT(e.FirstName, ' ', e.LastName) AS Vendedor,
                MONTH(o.OrderDate) AS Mes,
                ROUND(SUM(od.UnitPrice * od.Quantity * (1 - od.Discount)), 2) AS TotalVentas
            FROM 
                Employees AS e
            INNER JOIN 
                Orders AS o ON e.EmployeeID = o.EmployeeID
            INNER JOIN 
                `Order Details` AS od ON o.OrderID = od.OrderID
            WHERE 
                YEAR(o.OrderDate) = @Anio
            GROUP BY 
                Vendedor, Mes
            ORDER BY 
                Vendedor, Mes;
        ";
            var dt = new DataTable();
            using (var cn = new MySqlConnection(_connectionString))
            using (var cmd = new MySqlCommand(query, cn))
            {
                cmd.Parameters.AddWithValue("@Anio", anio);
                using (var da = new MySqlDataAdapter(cmd))
                    da.Fill(dt);
            }
            return dt;
        }

        public List<DtoVentaMensual> ObtenerVentasMensualesPorAnio(int anio)
        {
            var resultados = new List<DtoVentaMensual>();

            string query = @"
            SELECT 
                m.Mes,
                IFNULL(v.Total, 0) AS Total,
                m.NombreMes
            FROM (
                SELECT 1 AS Mes, 'Ene' AS NombreMes UNION ALL
                SELECT 2, 'Feb' UNION ALL
                SELECT 3, 'Mar' UNION ALL
                SELECT 4, 'Abr' UNION ALL
                SELECT 5, 'May' UNION ALL
                SELECT 6, 'Jun' UNION ALL
                SELECT 7, 'Jul' UNION ALL
                SELECT 8, 'Ago' UNION ALL
                SELECT 9, 'Sep' UNION ALL
                SELECT 10, 'Oct' UNION ALL
                SELECT 11, 'Nov' UNION ALL
                SELECT 12, 'Dic'
            ) AS m
            LEFT JOIN (
                SELECT 
                    MONTH(o.OrderDate) AS Mes,
                    ROUND(SUM(od.UnitPrice * od.Quantity * (1 - od.Discount)), 2) AS Total
                FROM Orders o
                INNER JOIN `Order Details` od ON o.OrderID = od.OrderID
                WHERE YEAR(o.OrderDate) = @Anio
                GROUP BY MONTH(o.OrderDate)
            ) AS v ON m.Mes = v.Mes
            ORDER BY m.Mes;
        ";
            try
            { 
            using (var cn = new MySqlConnection(_connectionString))
            using (var cmd = new MySqlCommand(query, cn))
            {
                cmd.Parameters.AddWithValue("@Anio", anio);
                cn.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var dto = new DtoVentaMensual
                        {
                            Mes = reader.GetInt32("Mes"),
                            Total = reader.GetDecimal("Total"),
                            NombreMes = reader.GetString("NombreMes")
                        };
                        resultados.Add(dto);
                    }
                }
            }
            }
            catch (MySqlException ex)
            {
                throw new Exception("Error al obtener las ventas mensuales por año: " + ex.Message);
            }
            return resultados;
        }

        public List<DtoVentaAnualComparativa> ObtenerVentasComparativas(List<int> años)
        {
            var resultados = new List<DtoVentaAnualComparativa>();
            string filtroAños = string.Join(",", años);

            string query = $@"
            SELECT m.Mes, m.NombreMes, y.Año, IFNULL(v.Total, 0) AS Total
            FROM (
                SELECT 1 AS Mes, 'Ene' AS NombreMes UNION ALL
                SELECT 2, 'Feb' UNION ALL
                SELECT 3, 'Mar' UNION ALL
                SELECT 4, 'Abr' UNION ALL
                SELECT 5, 'May' UNION ALL
                SELECT 6, 'Jun' UNION ALL
                SELECT 7, 'Jul' UNION ALL
                SELECT 8, 'Ago' UNION ALL
                SELECT 9, 'Sep' UNION ALL
                SELECT 10, 'Oct' UNION ALL
                SELECT 11, 'Nov' UNION ALL
                SELECT 12, 'Dic'
            ) AS m
            CROSS JOIN (
                SELECT DISTINCT YEAR(OrderDate) AS Año
                FROM Orders
                WHERE YEAR(OrderDate) IN ({filtroAños})
            ) AS y
            LEFT JOIN (
                SELECT 
                    YEAR(o.OrderDate) AS Año,
                    MONTH(o.OrderDate) AS Mes,
                    ROUND(SUM(od.UnitPrice * od.Quantity * (1 - od.Discount)), 2) AS Total
                FROM Orders o
                INNER JOIN `Order Details` od ON o.OrderID = od.OrderID
                WHERE YEAR(o.OrderDate) IN ({filtroAños})
                GROUP BY Año, Mes
            ) AS v ON m.Mes = v.Mes AND y.Año = v.Año
            ORDER BY m.Mes, y.Año;
        ";
            try
            {
                using (var cn = new MySqlConnection(_connectionString))
                using (var cmd = new MySqlCommand(query, cn))
                {
                    cn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var dto = new DtoVentaAnualComparativa
                            {
                                Mes = reader.GetInt32("Mes"),
                                NombreMes = reader.GetString("NombreMes"),
                                Año = reader.GetInt32("Año"),
                                Total = reader.GetDecimal("Total")
                            };
                            resultados.Add(dto);
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                throw new Exception("Error al obtener las ventas comparativas: " + ex.Message);
            }
            return resultados;
        }

        public List<DtoProductoMasVendido> ObtenerTopProductosRpt(int cantidad)
        {
            var lista = new List<DtoProductoMasVendido>();
            const string query = @"
            SELECT 
                ROW_NUMBER() OVER (ORDER BY SUM(od.Quantity) DESC) AS Posicion,
                CONCAT(ROW_NUMBER() OVER (ORDER BY SUM(od.Quantity) DESC), '. ', p.ProductName) AS NombreProducto,
                SUM(od.Quantity) AS CantidadVendida
            FROM `Order Details` od
            INNER JOIN Products p ON od.ProductID = p.ProductID
            GROUP BY p.ProductName
            ORDER BY CantidadVendida DESC
            LIMIT @Cantidad;
            ";
            try
            {
                using (var cn = new MySqlConnection(_connectionString))
                using (var cmd = new MySqlCommand(query, cn))
                {
                    cmd.Parameters.AddWithValue("@Cantidad", cantidad);
                    cn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(new DtoProductoMasVendido
                            {
                                Posicion = Convert.ToInt32(reader["Posicion"]),
                                NombreProducto = reader["NombreProducto"].ToString() ?? string.Empty,
                                CantidadVendida = Convert.ToInt32(reader["CantidadVendida"])
                            });
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                throw new Exception("Error al obtener los productos más vendidos: " + ex.Message);
            }
            return lista;
        }
    }
}
