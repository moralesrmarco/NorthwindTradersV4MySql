using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;
using System;
using System.Configuration;
using System.Data.SqlClient;

namespace MigracionDeDatosDeSqlServerAMySql
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // llamar a los metodos de migracion
            //MigrarCatalogoPermisos();
            //MigrarCategories();
            //MigrarConcepto();
            //MigrarCustomers();
            //MigrarEmployees();
            //MigrarEmployeesTerritories();
            //MigrarProducts();
            //MigrarRegion();
            //MigrarShippers();
            //MigrarSuppliers();
            //MigrarTerritories();
            //MigrarUsuarios();
            //MigrarVenta();
            // los dejo hasta el final porque tienen foreign keys y es mejor migrarlos al final
            //MigrarOrders();
            //MigrarOrderDetails();
            //MigrarPermisos();

            // Migrar Foreign Keys evitando duplicados al crear foreign keys en MySQL
            //var migrator = new ForeignKeyMigrator(
            //    sqlServerConn: "Server=.;Database=Northwind;Trusted_Connection=True;",
            //    mySqlConn: "Server=localhost;Database=northwind;Uid=root;Pwd=123456;",
            //    mySqlDatabase: "northwind"
            //);
            //migrator.MigrarForeignKeys();
            //Console.WriteLine("¡Migración de foreign keys completada!");

            //Migrar índices de SQL Server a MySQL
            //var migrator = new IndexMigrator();
            //migrator.MigrarIndices();
            //Console.WriteLine("¡Migración de indices completada!");

            //LlevarmeCategoryId();

            //Migrar Constraints de SQL Server a MySQL
            // No sirvio el siguiente procedimiento lo tuve que hacer a mano
            //var migrator = new ConstraintMigrator();
            //migrator.MigrarTodosConstraints();
            //Console.WriteLine("¡Migración de constraints completada!");

            //// Ejemplo de conversión de una cantidad a letras para cheque en C#
            //decimal monto = 1234.56m;
            //string leyenda = monto.ToLetrasCheque();
            //Console.WriteLine(leyenda); // Salida: "PESOS MIL DOSCIENTOS TREINTA Y CUATRO 56/100 M.N."

        }

        static void MigrarCatalogoPermisos()
        {
            // Lectura de las cadenas de conexión
            string sqlConnStr = ConfigurationManager.ConnectionStrings["SqlServerConn"].ConnectionString;
            string mySqlConnStr = ConfigurationManager.ConnectionStrings["MySqlConn"].ConnectionString;

            using (var sqlConn = new SqlConnection(sqlConnStr))
            using (var mySqlConn = new MySqlConnection(mySqlConnStr))
            {
                sqlConn.Open();
                mySqlConn.Open();

                // Opcional: acelerar inserciones desactivando temporalmente restricciones
                ExecuteNonQuery(mySqlConn, "SET FOREIGN_KEY_CHECKS=0;");

                // Consulta en SQL Server
                var selectQuery = @"
                    SELECT PermisoId, Descripción, Estatus
                    FROM CatalogoPermisos
                    ";

                using (var cmdSql = new SqlCommand(selectQuery, sqlConn))
                {

                    using (var reader = cmdSql.ExecuteReader())
                    {
                        // Preparar el INSERT para MySQL
                        var insertQuery = @"
                            INSERT INTO catalogopermisos (PermisoId, Descripción, Estatus)
                            VALUES (@PermisoId, @Descripción, @Estatus)";

                        using (var cmdMy = new MySqlCommand(insertQuery, mySqlConn))
                        {
                            cmdMy.Parameters.Add("@PermisoId", MySqlDbType.Int32);
                            cmdMy.Parameters.Add("@Descripción", MySqlDbType.VarChar);
                            cmdMy.Parameters.Add("@Estatus", MySqlDbType.Byte);

                            // Iterar y migrar fila por fila
                            while (reader.Read())
                            {
                                cmdMy.Parameters["@PermisoId"].Value = reader.GetInt32(0);
                                cmdMy.Parameters["@Descripción"].Value = reader.GetString(1);
                                cmdMy.Parameters["@Estatus"].Value = reader.GetBoolean(2);

                                try
                                {
                                    cmdMy.ExecuteNonQuery();
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(
                                        $"Error insertando PermisoId={reader.GetInt32(0)}: {ex.Message}"
                                    );
                                }
                            }
                        }
                    }
                }

                // Reactivar restricciones
                ExecuteNonQuery(mySqlConn, "SET FOREIGN_KEY_CHECKS=1;");

                Console.WriteLine("Migración CatalogoPermisos completada.");
            }
        }

        static void ExecuteNonQuery(MySqlConnection conn, string sql)
        {
            using (var cmd = new MySqlCommand(sql, conn))
                cmd.ExecuteNonQuery();
        }

        static void MigrarCategories()
        {
            // Lectura de las cadenas de conexión
            string sqlConnStr = ConfigurationManager.ConnectionStrings["SqlServerConn"].ConnectionString;
            string mySqlConnStr = ConfigurationManager.ConnectionStrings["MySqlConn"].ConnectionString;
            using (var sqlConn = new SqlConnection(sqlConnStr))
            using (var mySqlConn = new MySqlConnection(mySqlConnStr))
            {
                sqlConn.Open();
                mySqlConn.Open();
                // Opcional: acelerar inserciones desactivando temporalmente restricciones
                ExecuteNonQuery(mySqlConn, "SET FOREIGN_KEY_CHECKS=0;");
                // Consulta en SQL Server
                var selectQuery = @"
                    SELECT CategoryId, CategoryName, Description, Picture
                    FROM Categories
                    ";
                using (var cmdSql = new SqlCommand(selectQuery, sqlConn))
                {
                    using (var reader = cmdSql.ExecuteReader())
                    {
                        // Preparar el INSERT para MySQL
                        var insertQuery = @"
                            INSERT INTO categories (CategoryId, CategoryName, Description, Picture)
                            VALUES (@CategoryId, @CategoryName, @Description, @Picture)";
                        using (var cmdMy = new MySqlCommand(insertQuery, mySqlConn))
                        {
                            cmdMy.Parameters.Add("@CategoryId", MySqlDbType.Int32);
                            cmdMy.Parameters.Add("@CategoryName", MySqlDbType.VarChar);
                            cmdMy.Parameters.Add("@Description", MySqlDbType.LongText);
                            cmdMy.Parameters.Add("@Picture", MySqlDbType.LongBlob);
                            // Iterar y migrar fila por fila
                            while (reader.Read())
                            {
                                byte[] pictureBytes = null;
                                if (!reader.IsDBNull(3))
                                    pictureBytes = (byte[])reader.GetValue(3);
                                cmdMy.Parameters["@CategoryId"].Value = reader.GetInt32(0);
                                cmdMy.Parameters["@CategoryName"].Value = reader.GetString(1);
                                cmdMy.Parameters["@Description"].Value =
                                    reader.IsDBNull(2)
                                        ? (object)DBNull.Value
                                        : (object)reader.GetString(2);
                                cmdMy.Parameters["@Picture"].Value = (object)pictureBytes ?? DBNull.Value;
                                try
                                {
                                    cmdMy.ExecuteNonQuery();
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(
                                        $"Error insertando CategoryId={reader.GetInt32(0)}: {ex.Message}"
                                    );
                                }
                            }
                        }
                    }
                }
                // Reactivar restricciones
                ExecuteNonQuery(mySqlConn, "SET FOREIGN_KEY_CHECKS=1;");
                Console.WriteLine("Migración Categories completada.");
            }
        }

        // este metodo es una version para abstraccion y reutilizacion del codigo de migracion
        static void MigrarTabla(
            string selectSql,
            string insertSql,
            Action<MySqlCommand> configurarParametros,
            Action<SqlDataReader, MySqlCommand> mapearFila)
        {
            string sqlConnStr = ConfigurationManager
                                  .ConnectionStrings["SqlServerConn"].ConnectionString;
            string mySqlConnStr = ConfigurationManager
                                  .ConnectionStrings["MySqlConn"].ConnectionString;

            using (var sqlConn = new SqlConnection(sqlConnStr))
            using (var mySqlConn = new MySqlConnection(mySqlConnStr))
            {
                sqlConn.Open();
                mySqlConn.Open();

                // Desactivar FK
                ExecuteNonQuery(mySqlConn, "SET FOREIGN_KEY_CHECKS=0;");

                using (var cmdSql = new SqlCommand(selectSql, sqlConn))
                using (var reader = cmdSql.ExecuteReader())
                using (var cmdMy = new MySqlCommand(insertSql, mySqlConn))
                {
                    configurarParametros(cmdMy);

                    while (reader.Read())
                    {
                        mapearFila(reader, cmdMy);
                        try
                        {
                            cmdMy.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error insertando: {ex.Message}");
                        }
                    }
                }

                // Reactivar FK
                ExecuteNonQuery(mySqlConn, "SET FOREIGN_KEY_CHECKS=1;");
            }
        }
        // helper para manejar strings nulos
        private static object GetNullableString(SqlDataReader reader, int index)
        {
            return reader.IsDBNull(index)
                ? (object)DBNull.Value
                : reader.GetString(index);
        }

        // Helper opcional si prefieres encapsular la lógica de fechas nulas
        static object GetNullableDateTime(SqlDataReader reader, int index)
        {
            return reader.IsDBNull(index)
                ? (object)DBNull.Value
                : reader.GetDateTime(index);
        }


        // en este metodo ya se usa la funcion MigrarTabla para hacer la migracion
        static void MigrarConcepto()
        {
            var selectSql = @"
                SELECT Id, Id_Venta, PrecioUnitario, Cantidad, Descripcion, Importe
                FROM Concepto
                ";
            var insertSql = @"
                INSERT INTO concepto (Id, Id_Venta, PrecioUnitario, Cantidad, Descripcion, Importe)
                VALUES (@Id, @Id_Venta, @PrecioUnitario, @Cantidad, @Descripcion, @Importe)";
            Action<MySqlCommand> configurarParametros = cmd =>
            {
                cmd.Parameters.Add("@Id", MySqlDbType.Int32);
                cmd.Parameters.Add("@Id_Venta", MySqlDbType.Int32);
                cmd.Parameters.Add("@PrecioUnitario", MySqlDbType.Decimal);
                cmd.Parameters.Add("@Cantidad", MySqlDbType.Int32);
                cmd.Parameters.Add("@Descripcion", MySqlDbType.VarChar);
                cmd.Parameters.Add("@Importe", MySqlDbType.Decimal);
            };
            Action<SqlDataReader, MySqlCommand> mapearFila = (reader, cmd) =>
            {
                cmd.Parameters["@Id"].Value = reader.GetInt32(0);
                cmd.Parameters["@Id_Venta"].Value = reader.GetInt32(1);
                cmd.Parameters["@PrecioUnitario"].Value = reader.GetDecimal(2);
                cmd.Parameters["@Cantidad"].Value = reader.GetInt32(3);
                cmd.Parameters["@Descripcion"].Value = reader.IsDBNull(4)
                    ? (object)DBNull.Value
                    : reader.GetString(4);
                cmd.Parameters["@Importe"].Value = reader.GetDecimal(5);
            };
            MigrarTabla(selectSql, insertSql, configurarParametros, mapearFila);
            Console.WriteLine("Migración Concepto completada.");
            // El siguiente codigo es equivalente al uso del codigo anterior con la funcion MigrarTabla
            //MigrarTabla(
            //    selectSql: @"SELECT Id, Id_Venta, PrecioUnitario, Cantidad, Descripcion, Importe FROM Concepto;",
            //    insertSql: @"
            //        INSERT INTO concepto (Id, Id_Venta, PrecioUnitario, Cantidad, Descripcion, Importe)
            //        VALUES (@Id, @Id_Venta, @PrecioUnitario, @Cantidad, @Descripcion, @Importe);",
            //    configurarParametros: cmd =>
            //    {
            //        cmd.Parameters.Add("@Id", MySqlDbType.Int32);
            //        cmd.Parameters.Add("@Id_Venta", MySqlDbType.Int32);
            //        cmd.Parameters.Add("@PrecioUnitario", MySqlDbType.Decimal);
            //        cmd.Parameters.Add("@Cantidad", MySqlDbType.Int32);
            //        cmd.Parameters.Add("@Descripcion", MySqlDbType.VarChar);
            //        cmd.Parameters.Add("@Importe", MySqlDbType.Decimal);
            //    },
            //    mapearFila: (reader, cmd) =>
            //    {
            //        cmd.Parameters["@Id"].Value = reader.GetInt32(0);
            //        cmd.Parameters["@Id_Venta"].Value = reader.GetInt32(1);
            //        cmd.Parameters["@PrecioUnitario"].Value = reader.GetDecimal(2);
            //        cmd.Parameters["@Cantidad"].Value = reader.GetInt32(3);
            //        cmd.Parameters["@Descripcion"].Value = reader.IsDBNull(4)
            //            ? (object)DBNull.Value
            //            : reader.GetString(4);
            //        cmd.Parameters["@Importe"].Value = reader.GetDecimal(5);
            //    }
            //);
            //Console.WriteLine("Migración Concepto completada.");
        }

        static void MigrarCustomers()
        {
            var selectSql = @"
                SELECT CustomerId, CompanyName, ContactName, ContactTitle, Address, City, Region, PostalCode, Country, Phone, Fax
                FROM customers
                ";
            var insertSql = @"
                INSERT INTO customers (CustomerId, CompanyName, ContactName, ContactTitle, Address, City, Region, PostalCode, Country, Phone, Fax)
                VALUES (@CustomerId, @CompanyName, @ContactName, @ContactTitle, @Address, @City, @Region, @PostalCode, @Country, @Phone, @Fax)";
            Action<MySqlCommand> configurarParametros = cmd =>
            {
                cmd.Parameters.Add("@CustomerId", MySqlDbType.VarChar);
                cmd.Parameters.Add("@CompanyName", MySqlDbType.VarChar);
                cmd.Parameters.Add("@ContactName", MySqlDbType.VarChar);
                cmd.Parameters.Add("@ContactTitle", MySqlDbType.VarChar);
                cmd.Parameters.Add("@Address", MySqlDbType.VarChar);
                cmd.Parameters.Add("@City", MySqlDbType.VarChar);
                cmd.Parameters.Add("@Region", MySqlDbType.VarChar);
                cmd.Parameters.Add("@PostalCode", MySqlDbType.VarChar);
                cmd.Parameters.Add("@Country", MySqlDbType.VarChar);
                cmd.Parameters.Add("@Phone", MySqlDbType.VarChar);
                cmd.Parameters.Add("@Fax", MySqlDbType.VarChar);
            };
            Action<SqlDataReader, MySqlCommand> mapearFila = (reader, cmd) =>
            {
                cmd.Parameters["@CustomerId"].Value = reader.GetString(0);
                cmd.Parameters["@CompanyName"].Value = reader.GetString(1);
                // pueden tener valores nulos
                cmd.Parameters["@ContactName"].Value = GetNullableString(reader, 2);
                cmd.Parameters["@ContactTitle"].Value = GetNullableString(reader, 3);
                cmd.Parameters["@Address"].Value = GetNullableString(reader, 4);
                cmd.Parameters["@City"].Value = GetNullableString(reader, 5);
                cmd.Parameters["@Region"].Value = GetNullableString(reader, 6);
                cmd.Parameters["@PostalCode"].Value = GetNullableString(reader, 7);
                cmd.Parameters["@Country"].Value = GetNullableString(reader, 8);
                cmd.Parameters["@Phone"].Value = GetNullableString(reader, 9);
                cmd.Parameters["@Fax"].Value = GetNullableString(reader, 10);
            };
            MigrarTabla(selectSql, insertSql, configurarParametros, mapearFila);
            Console.WriteLine("Migración Customers completada.");
        }

        static void MigrarEmployees()
        {
            var selectSql = @"
                SELECT EmployeeId, LastName, FirstName, Title, TitleOfCourtesy, BirthDate, HireDate, Address, City, Region, PostalCode, Country, HomePhone, Extension, Photo, Notes, ReportsTo, PhotoPath
                FROM Employees
                ";
            var insertSql = @"
                INSERT INTO employees (EmployeeId, LastName, FirstName, Title, TitleOfCourtesy, BirthDate, HireDate, Address, City, Region, PostalCode, Country, HomePhone, Extension, Photo, Notes, ReportsTo, PhotoPath)
                VALUES (@EmployeeId, @LastName, @FirstName, @Title, @TitleOfCourtesy, @BirthDate, @HireDate, @Address, @City, @Region, @PostalCode, @Country, @HomePhone, @Extension, @Photo, @Notes, @ReportsTo, @PhotoPath)";
            Action<MySqlCommand> configurarParametros = cmd =>
            {
                cmd.Parameters.Add("@EmployeeId", MySqlDbType.Int32);
                cmd.Parameters.Add("@LastName", MySqlDbType.VarChar);
                cmd.Parameters.Add("@FirstName", MySqlDbType.VarChar);
                cmd.Parameters.Add("@Title", MySqlDbType.VarChar);
                cmd.Parameters.Add("@TitleOfCourtesy", MySqlDbType.VarChar);
                cmd.Parameters.Add("@BirthDate", MySqlDbType.DateTime);
                cmd.Parameters.Add("@HireDate", MySqlDbType.DateTime);
                cmd.Parameters.Add("@Address", MySqlDbType.VarChar);
                cmd.Parameters.Add("@City", MySqlDbType.VarChar);
                cmd.Parameters.Add("@Region", MySqlDbType.VarChar);
                cmd.Parameters.Add("@PostalCode", MySqlDbType.VarChar);
                cmd.Parameters.Add("@Country", MySqlDbType.VarChar);
                cmd.Parameters.Add("@HomePhone", MySqlDbType.VarChar);
                cmd.Parameters.Add("@Extension", MySqlDbType.VarChar);
                cmd.Parameters.Add("@Photo", MySqlDbType.LongBlob);
                cmd.Parameters.Add("@Notes", MySqlDbType.LongText);
                cmd.Parameters.Add("@ReportsTo", MySqlDbType.Int32);
                cmd.Parameters.Add("@PhotoPath", MySqlDbType.VarChar);
            };
            Action<SqlDataReader, MySqlCommand> mapearFila = (reader, cmd) =>
            {
                // EmployeeId nunca es NULL
                cmd.Parameters["@EmployeeId"].Value = reader.GetInt32(0);

                // Campos VARCHARS (pueden ser NULL)
                cmd.Parameters["@LastName"].Value = GetNullableString(reader, 1);
                cmd.Parameters["@FirstName"].Value = GetNullableString(reader, 2);
                cmd.Parameters["@Title"].Value = GetNullableString(reader, 3);
                cmd.Parameters["@TitleOfCourtesy"].Value = GetNullableString(reader, 4);

                // Fechas (pueden ser NULL)
                cmd.Parameters["@BirthDate"].Value = reader.IsDBNull(5)
                    ? (object)DBNull.Value
                    : reader.GetDateTime(5);
                cmd.Parameters["@HireDate"].Value = reader.IsDBNull(6)
                    ? (object)DBNull.Value
                    : reader.GetDateTime(6);

                // Más VARCHARS
                cmd.Parameters["@Address"].Value = GetNullableString(reader, 7);
                cmd.Parameters["@City"].Value = GetNullableString(reader, 8);
                cmd.Parameters["@Region"].Value = GetNullableString(reader, 9);
                cmd.Parameters["@PostalCode"].Value = GetNullableString(reader, 10);
                cmd.Parameters["@Country"].Value = GetNullableString(reader, 11);
                cmd.Parameters["@HomePhone"].Value = GetNullableString(reader, 12);
                cmd.Parameters["@Extension"].Value = GetNullableString(reader, 13);

                // Foto (LONG BLOB)
                byte[] photoBytes = null;
                if (!reader.IsDBNull(14))
                    photoBytes = (byte[])reader.GetValue(14);
                cmd.Parameters["@Photo"].Value = (object)photoBytes ?? DBNull.Value;

                // Notas (LONGTEXT)
                cmd.Parameters["@Notes"].Value = GetNullableString(reader, 15);

                // ReportsTo (INT, puede ser NULL)
                cmd.Parameters["@ReportsTo"].Value = reader.IsDBNull(16)
                    ? (object)DBNull.Value
                    : reader.GetInt32(16);

                // PhotoPath (VARCHAR)
                cmd.Parameters["@PhotoPath"].Value = GetNullableString(reader, 17);
            };
            MigrarTabla(selectSql, insertSql, configurarParametros, mapearFila);
            Console.WriteLine("Migración Employees completada.");
        }

        static void MigrarEmployeesTerritories()
        {
            var selectSql = @"
                SELECT EmployeeId, TerritoryId
                FROM EmployeeTerritories
                ";
            var insertSql = @"
                INSERT INTO employeeterritories (EmployeeId, TerritoryId)
                VALUES (@EmployeeId, @TerritoryId)";
            Action<MySqlCommand> configurarParametros = cmd =>
            {
                cmd.Parameters.Add("@EmployeeId", MySqlDbType.Int32);
                cmd.Parameters.Add("@TerritoryId", MySqlDbType.VarChar);
            };
            Action<SqlDataReader, MySqlCommand> mapearFila = (reader, cmd) =>
            {
                cmd.Parameters["@EmployeeId"].Value = reader.GetInt32(0);
                cmd.Parameters["@TerritoryId"].Value = reader.GetString(1);
            };
            MigrarTabla(selectSql, insertSql, configurarParametros, mapearFila);
            Console.WriteLine("Migración EmployeesTerritories completada.");
        }

        static void MigrarProducts()
        {
            var selectSql = @"
                SELECT ProductId, ProductName, SupplierId, CategoryId, QuantityPerUnit, UnitPrice, UnitsInStock, UnitsOnOrder, ReorderLevel, Discontinued
                FROM Products
                ";
            var insertSql = @"
                INSERT INTO products (ProductId, ProductName, SupplierId, CategoryId, QuantityPerUnit, UnitPrice, UnitsInStock, UnitsOnOrder, ReorderLevel, Discontinued)
                VALUES (@ProductId, @ProductName, @SupplierId, @CategoryId, @QuantityPerUnit, @UnitPrice, @UnitsInStock, @UnitsOnOrder, @ReorderLevel, @Discontinued)";
            Action<MySqlCommand> configurarParametros = cmd =>
            {
                cmd.Parameters.Add("@ProductId", MySqlDbType.Int32);
                cmd.Parameters.Add("@ProductName", MySqlDbType.VarChar);
                cmd.Parameters.Add("@SupplierId", MySqlDbType.Int32);
                cmd.Parameters.Add("@CategoryId", MySqlDbType.Int32);
                cmd.Parameters.Add("@QuantityPerUnit", MySqlDbType.VarChar);
                cmd.Parameters.Add("@UnitPrice", MySqlDbType.Decimal);
                cmd.Parameters.Add("@UnitsInStock", MySqlDbType.Int16);
                cmd.Parameters.Add("@UnitsOnOrder", MySqlDbType.Int16);
                cmd.Parameters.Add("@ReorderLevel", MySqlDbType.Int16);
                cmd.Parameters.Add("@Discontinued", MySqlDbType.Byte);
            };
            Action<SqlDataReader, MySqlCommand> mapearFila = (reader, cmd) =>
            {
                cmd.Parameters["@ProductId"].Value = reader.GetInt32(0);
                cmd.Parameters["@ProductName"].Value = reader.GetString(1);
                // pueden ser nulos
                cmd.Parameters["@SupplierId"].Value = reader.IsDBNull(2)
                    ? (object)DBNull.Value
                    : reader.GetInt32(2);
                cmd.Parameters["@CategoryId"].Value = reader.IsDBNull(3)
                    ? (object)DBNull.Value
                    : reader.GetInt32(3);
                cmd.Parameters["@QuantityPerUnit"].Value = GetNullableString(reader, 4);
                cmd.Parameters["@UnitPrice"].Value = reader.IsDBNull(5)
                    ? (object)DBNull.Value
                    : reader.GetDecimal(5);
                cmd.Parameters["@UnitsInStock"].Value = reader.IsDBNull(6)
                    ? (object)DBNull.Value
                    : reader.GetInt16(6);
                cmd.Parameters["@UnitsOnOrder"].Value = reader.IsDBNull(7)
                    ? (object)DBNull.Value
                    : reader.GetInt16(7);
                cmd.Parameters["@ReorderLevel"].Value = reader.IsDBNull(8)
                    ? (object)DBNull.Value
                    : reader.GetInt16(8);
                cmd.Parameters["@Discontinued"].Value = reader.GetBoolean(9);
            };
            MigrarTabla(selectSql, insertSql, configurarParametros, mapearFila);
            Console.WriteLine("Migración Products completada.");
        }

        static void MigrarRegion()
        {
            var selectSql = @"
                SELECT RegionId, RegionDescription
                FROM Region
                ";
            var insertSql = @"
                INSERT INTO region (RegionId, RegionDescription)
                VALUES (@RegionId, @RegionDescription)";
            Action<MySqlCommand> configurarParametros = cmd =>
            {
                cmd.Parameters.Add("@RegionId", MySqlDbType.Int32);
                cmd.Parameters.Add("@RegionDescription", MySqlDbType.VarChar);
            };
            Action<SqlDataReader, MySqlCommand> mapearFila = (reader, cmd) =>
            {
                cmd.Parameters["@RegionId"].Value = reader.GetInt32(0);
                cmd.Parameters["@RegionDescription"].Value = reader.GetString(1);
            };
            MigrarTabla(selectSql, insertSql, configurarParametros, mapearFila);
            Console.WriteLine("Migración Region completada.");
        }

        static void MigrarShippers()
        {
            var selectSql = @"
                SELECT ShipperId, CompanyName, Phone
                FROM Shippers
                ";
            var insertSql = @"
                INSERT INTO shippers (ShipperId, CompanyName, Phone)
                VALUES (@ShipperId, @CompanyName, @Phone)";
            Action<MySqlCommand> configurarParametros = cmd =>
            {
                cmd.Parameters.Add("@ShipperId", MySqlDbType.Int32);
                cmd.Parameters.Add("@CompanyName", MySqlDbType.VarChar);
                cmd.Parameters.Add("@Phone", MySqlDbType.VarChar);
            };
            Action<SqlDataReader, MySqlCommand> mapearFila = (reader, cmd) =>
            {
                cmd.Parameters["@ShipperId"].Value = reader.GetInt32(0);
                cmd.Parameters["@CompanyName"].Value = reader.GetString(1);
                cmd.Parameters["@Phone"].Value = GetNullableString(reader, 2);
            };
            MigrarTabla(selectSql, insertSql, configurarParametros, mapearFila);
            Console.WriteLine("Migración Shippers completada.");
        }

        static void MigrarSuppliers()
        {
            var selectSql = @"
                SELECT SupplierId, CompanyName, ContactName, ContactTitle, Address, City, Region, PostalCode, Country, Phone, Fax, HomePage
                FROM Suppliers
                ";
            var insertSql = @"
                INSERT INTO suppliers (SupplierId, CompanyName, ContactName, ContactTitle, Address, City, Region, PostalCode, Country, Phone, Fax, HomePage)
                VALUES (@SupplierId, @CompanyName, @ContactName, @ContactTitle, @Address, @City, @Region, @PostalCode, @Country, @Phone, @Fax, @HomePage)";
            Action<MySqlCommand> configurarParametros = cmd =>
            {
                cmd.Parameters.Add("@SupplierId", MySqlDbType.Int32);
                cmd.Parameters.Add("@CompanyName", MySqlDbType.VarChar);
                cmd.Parameters.Add("@ContactName", MySqlDbType.VarChar);
                cmd.Parameters.Add("@ContactTitle", MySqlDbType.VarChar);
                cmd.Parameters.Add("@Address", MySqlDbType.VarChar);
                cmd.Parameters.Add("@City", MySqlDbType.VarChar);
                cmd.Parameters.Add("@Region", MySqlDbType.VarChar);
                cmd.Parameters.Add("@PostalCode", MySqlDbType.VarChar);
                cmd.Parameters.Add("@Country", MySqlDbType.VarChar);
                cmd.Parameters.Add("@Phone", MySqlDbType.VarChar);
                cmd.Parameters.Add("@Fax", MySqlDbType.VarChar);
                cmd.Parameters.Add("@HomePage", MySqlDbType.LongText);
            };
            Action<SqlDataReader, MySqlCommand> mapearFila = (reader, cmd) =>
            {
                cmd.Parameters["@SupplierId"].Value = reader.GetInt32(0);
                cmd.Parameters["@CompanyName"].Value = reader.GetString(1);

                // Campos que pueden ser nulos
                cmd.Parameters["@ContactName"].Value = GetNullableString(reader, 2);
                cmd.Parameters["@ContactTitle"].Value = GetNullableString(reader, 3);
                cmd.Parameters["@Address"].Value = GetNullableString(reader, 4);
                cmd.Parameters["@City"].Value = GetNullableString(reader, 5);
                cmd.Parameters["@Region"].Value = GetNullableString(reader, 6);
                cmd.Parameters["@PostalCode"].Value = GetNullableString(reader, 7);
                cmd.Parameters["@Country"].Value = GetNullableString(reader, 8);
                cmd.Parameters["@Phone"].Value = GetNullableString(reader, 9);
                cmd.Parameters["@Fax"].Value = GetNullableString(reader, 10);
                cmd.Parameters["@HomePage"].Value = GetNullableString(reader, 11);
            };
            MigrarTabla(selectSql, insertSql, configurarParametros, mapearFila);
            Console.WriteLine("Migración Suppliers completada.");
        }

        static void MigrarTerritories()
        {
            var selectSql = @"
                SELECT TerritoryId, TerritoryDescription, RegionId
                FROM Territories
                ";
            var insertSql = @"
                INSERT INTO territories (TerritoryId, TerritoryDescription, RegionId)
                VALUES (@TerritoryId, @TerritoryDescription, @RegionId)";
            Action<MySqlCommand> configurarParametros = cmd =>
            {
                cmd.Parameters.Add("@TerritoryId", MySqlDbType.VarChar);
                cmd.Parameters.Add("@TerritoryDescription", MySqlDbType.VarChar);
                cmd.Parameters.Add("@RegionId", MySqlDbType.Int32);
            };
            Action<SqlDataReader, MySqlCommand> mapearFila = (reader, cmd) =>
            {
                cmd.Parameters["@TerritoryId"].Value = reader.GetString(0);
                cmd.Parameters["@TerritoryDescription"].Value = reader.GetString(1);
                cmd.Parameters["@RegionId"].Value = reader.GetInt32(2);
            };
            MigrarTabla(selectSql, insertSql, configurarParametros, mapearFila);
            Console.WriteLine("Migración Territories completada.");
        }

        static void MigrarUsuarios()
        {
            var selectSql = @"
                SELECT Id, Paterno, Materno, Nombres, Usuario, Password, FechaCaptura, FechaModificacion, Estatus
                FROM Usuarios
                ";
            var insertSql = @"
                INSERT INTO usuarios (Id, Paterno, Materno, Nombres, Usuario, Password, FechaCaptura, FechaModificacion, Estatus)
                VALUES (@Id, @Paterno, @Materno, @Nombres, @Usuario, @Password, @FechaCaptura, @FechaModificacion, @Estatus)";
            Action<MySqlCommand> configurarParametros = cmd =>
            {
                cmd.Parameters.Add("@Id", MySqlDbType.Int32);
                cmd.Parameters.Add("@Paterno", MySqlDbType.VarChar);
                cmd.Parameters.Add("@Materno", MySqlDbType.VarChar);
                cmd.Parameters.Add("@Nombres", MySqlDbType.VarChar);
                cmd.Parameters.Add("@Usuario", MySqlDbType.VarChar);
                cmd.Parameters.Add("@Password", MySqlDbType.VarChar);
                cmd.Parameters.Add("@FechaCaptura", MySqlDbType.DateTime);
                cmd.Parameters.Add("@FechaModificacion", MySqlDbType.DateTime);
                cmd.Parameters.Add("@Estatus", MySqlDbType.Bit);
            };
            Action<SqlDataReader, MySqlCommand> mapearFila = (reader, cmd) =>
            {
                cmd.Parameters["@Id"].Value = reader.GetInt32(0);
                // Paterno y Materno pueden ser nulos
                cmd.Parameters["@Paterno"].Value = GetNullableString(reader, 1);
                cmd.Parameters["@Materno"].Value = GetNullableString(reader, 2);
                cmd.Parameters["@Nombres"].Value = reader.GetString(3);
                cmd.Parameters["@Usuario"].Value = reader.GetString(4);
                cmd.Parameters["@Password"].Value = reader.GetString(5);

                // FechaCaptura no debería ser null
                cmd.Parameters["@FechaCaptura"].Value = reader.GetDateTime(6);

                // FechaModificacion puede ser null
                cmd.Parameters["@FechaModificacion"].Value = GetNullableDateTime(reader, 7);

                // Estatus es BIT en SQL Server; aquí lo leemos como bool y MySQL lo guarda como bit(1)
                cmd.Parameters["@Estatus"].Value = reader.GetBoolean(8);
            };
            MigrarTabla(selectSql, insertSql, configurarParametros, mapearFila);
            Console.WriteLine("Migración Usuarios completada.");
        }

        static void MigrarVenta()
        {
            // 1. SELECT desde SQL Server
            var selectSql = @"
                SELECT 
                    Id, 
                    Total, 
                    Fecha
                FROM Venta;
            ";

            // 2. INSERT en MySQL
            var insertSql = @"
                INSERT INTO venta 
                    (Id, Total, Fecha)
                VALUES
                    (@Id, @Total, @Fecha);
            ";

            // 3. Configuración de parámetros
            Action<MySqlCommand> configurarParametros = cmd =>
            {
                cmd.Parameters.Add("@Id", MySqlDbType.Int32);
                cmd.Parameters.Add("@Total", MySqlDbType.Decimal);
                cmd.Parameters.Add("@Fecha", MySqlDbType.DateTime);
            };

            // 4. Mapeo de cada fila leída al comando MySqlCommand
            Action<SqlDataReader, MySqlCommand> mapearFila = (reader, cmd) =>
            {
                cmd.Parameters["@Id"].Value = reader.GetInt32(0);
                cmd.Parameters["@Total"].Value = reader.GetDecimal(1);
                cmd.Parameters["@Fecha"].Value = reader.GetDateTime(2);
            };

            // 5. Ejecutar la migración
            MigrarTabla(selectSql, insertSql, configurarParametros, mapearFila);
            Console.WriteLine("Migración de Venta completada.");
        }

        static void MigrarOrders()
        {
            var selectSql = @"
                SELECT OrderId, CustomerId, EmployeeId, OrderDate, RequiredDate, ShippedDate, ShipVia, Freight, ShipName, ShipAddress, ShipCity, ShipRegion, ShipPostalCode, ShipCountry
                FROM Orders
                ";
            var insertSql = @"
                INSERT INTO orders (OrderId, CustomerId, EmployeeId, OrderDate, RequiredDate, ShippedDate, ShipVia, Freight, ShipName, ShipAddress, ShipCity, ShipRegion, ShipPostalCode, ShipCountry)
                VALUES (@OrderId, @CustomerId, @EmployeeId, @OrderDate, @RequiredDate, @ShippedDate, @ShipVia, @Freight, @ShipName, @ShipAddress, @ShipCity, @ShipRegion, @ShipPostalCode, @ShipCountry)";
            Action<MySqlCommand> configurarParametros = cmd =>
            {
                cmd.Parameters.Add("@OrderId", MySqlDbType.Int32);
                cmd.Parameters.Add("@CustomerId", MySqlDbType.VarChar);
                cmd.Parameters.Add("@EmployeeId", MySqlDbType.Int32);
                cmd.Parameters.Add("@OrderDate", MySqlDbType.DateTime);
                cmd.Parameters.Add("@RequiredDate", MySqlDbType.DateTime);
                cmd.Parameters.Add("@ShippedDate", MySqlDbType.DateTime);
                cmd.Parameters.Add("@ShipVia", MySqlDbType.Int32);
                cmd.Parameters.Add("@Freight", MySqlDbType.Decimal);
                cmd.Parameters.Add("@ShipName", MySqlDbType.VarChar);
                cmd.Parameters.Add("@ShipAddress", MySqlDbType.VarChar);
                cmd.Parameters.Add("@ShipCity", MySqlDbType.VarChar);
                cmd.Parameters.Add("@ShipRegion", MySqlDbType.VarChar);
                cmd.Parameters.Add("@ShipPostalCode", MySqlDbType.VarChar);
                cmd.Parameters.Add("@ShipCountry", MySqlDbType.VarChar);
            };

            Action<SqlDataReader, MySqlCommand> mapearFila = (reader, cmd) =>
            {
                cmd.Parameters["@OrderId"].Value = reader.GetInt32(0);
                cmd.Parameters["@CustomerId"].Value = GetNullableString(reader, 1);
                cmd.Parameters["@EmployeeId"].Value = reader.IsDBNull(2)
                    ? (object)DBNull.Value
                    : reader.GetInt32(2);
                cmd.Parameters["@OrderDate"].Value = GetNullableDateTime(reader, 3);
                cmd.Parameters["@RequiredDate"].Value = GetNullableDateTime(reader, 4);
                cmd.Parameters["@ShippedDate"].Value = GetNullableDateTime(reader, 5);
                cmd.Parameters["@ShipVia"].Value = reader.IsDBNull(6)
                    ? (object)DBNull.Value
                    : reader.GetInt32(6);
                cmd.Parameters["@Freight"].Value = reader.IsDBNull(7)
                    ? (object)DBNull.Value
                    : reader.GetDecimal(7);
                cmd.Parameters["@ShipName"].Value = GetNullableString(reader, 8);
                cmd.Parameters["@ShipAddress"].Value = GetNullableString(reader, 9);
                cmd.Parameters["@ShipCity"].Value = GetNullableString(reader, 10);
                cmd.Parameters["@ShipRegion"].Value = GetNullableString(reader, 11);
                cmd.Parameters["@ShipPostalCode"].Value = GetNullableString(reader, 12);
                cmd.Parameters["@ShipCountry"].Value = GetNullableString(reader, 13);
            };
            MigrarTabla(selectSql, insertSql, configurarParametros, mapearFila);
            Console.WriteLine("Migración Orders completada.");
        }

        static void MigrarOrderDetails()
        {
            var selectSql = @"
                SELECT OrderId, ProductId, UnitPrice, Quantity, Discount
                FROM [Order Details]
                ";
            var insertSql = @"
                INSERT INTO `order details` (OrderId, ProductId, UnitPrice, Quantity, Discount)
                VALUES (@OrderId, @ProductId, @UnitPrice, @Quantity, @Discount)";
            Action<MySqlCommand> configurarParametros = cmd =>
            {
                cmd.Parameters.Add("@OrderId", MySqlDbType.Int32);
                cmd.Parameters.Add("@ProductId", MySqlDbType.Int32);
                cmd.Parameters.Add("@UnitPrice", MySqlDbType.Decimal);
                cmd.Parameters.Add("@Quantity", MySqlDbType.Int16);
                cmd.Parameters.Add("@Discount", MySqlDbType.Float);
            };
            Action<SqlDataReader, MySqlCommand> mapearFila = (reader, cmd) =>
            {
                cmd.Parameters["@OrderId"].Value = reader.GetInt32(0);
                cmd.Parameters["@ProductId"].Value = reader.GetInt32(1);
                cmd.Parameters["@UnitPrice"].Value = reader.GetDecimal(2);
                cmd.Parameters["@Quantity"].Value = reader.GetInt16(3);
                cmd.Parameters["@Discount"].Value = reader.GetFloat(4);
            };
            MigrarTabla(selectSql, insertSql, configurarParametros, mapearFila);
            Console.WriteLine("Migración OrderDetails completada.");
        }

        static void MigrarPermisos()
        {
            var selectSql = @"
                SELECT UsuarioId, PermisoId
                FROM Permisos
                ";
            var insertSql = @"
                INSERT INTO permisos (UsuarioId, PermisoId)
                VALUES (@UsuarioId, @PermisoId)";
            Action<MySqlCommand> configurarParametros = cmd =>
            {
                cmd.Parameters.Add("@UsuarioId", MySqlDbType.Int32);
                cmd.Parameters.Add("@PermisoId", MySqlDbType.Int32);
            };
            Action<SqlDataReader, MySqlCommand> mapearFila = (reader, cmd) =>
            {
                cmd.Parameters["@UsuarioId"].Value = reader.GetInt32(0);
                cmd.Parameters["@PermisoId"].Value = reader.GetInt32(1);
            };
            MigrarTabla(selectSql, insertSql, configurarParametros, mapearFila);
            Console.WriteLine("Migración Permisos completada.");
        }

        static void LlevarmeCategoryId()
        {
            // 1. Configura tus cadenas de conexión
            var sqlServerConnString = "Server=(local);Database=Northwind;User Id=sa;Password=123456;";
            var mySqlConnString = "Server=localhost;Database=northwind;Uid=root;Pwd=123456;";

            try
            {
                // 2. Conexión a SQL Server y lectura de datos
                using (var sqlConn = new SqlConnection(sqlServerConnString))
                {
                    sqlConn.Open();
                    using (var sqlCmd = new SqlCommand("SELECT ProductID, CategoryID FROM Products", sqlConn))
                    using (var reader = sqlCmd.ExecuteReader())
                    {
                        // 3. Conexión a MySQL y preparación de la transacción
                        using (var mySqlConn = new MySqlConnection(mySqlConnString))
                        {
                            mySqlConn.Open();
                            using (var tx = mySqlConn.BeginTransaction())
                            using (var updateCmd = new MySqlCommand(
                                @"UPDATE Products
                                  SET CategoryID = @CategoryID
                                  WHERE ProductID  = @ProductID;",
                                mySqlConn, tx))
                            {
                                // Define parámetros
                                updateCmd.Parameters.Add("@CategoryID", MySqlDbType.Int32);
                                updateCmd.Parameters.Add("@ProductID", MySqlDbType.Int32);

                                // 4. Recorre cada fila leída de SQL Server
                                while (reader.Read())
                                {
                                    updateCmd.Parameters["@ProductID"].Value = reader.GetInt32(0);
                                    updateCmd.Parameters["@CategoryID"].Value =
                                        reader.IsDBNull(1)
                                        ? (object)DBNull.Value
                                        : reader.GetInt32(1);

                                    updateCmd.ExecuteNonQuery();
                                }

                                // 5. Confirma todos los cambios en MySQL
                                tx.Commit();
                            }
                        }
                    }
                }

                Console.WriteLine("Migración de CategoryID completada con éxito.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error durante la migración: " + ex.Message);
            }
        }
    }
}
