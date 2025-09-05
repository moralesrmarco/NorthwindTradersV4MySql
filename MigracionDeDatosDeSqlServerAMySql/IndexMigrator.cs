using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using MySql.Data.MySqlClient;

public class IndexMigrator
{
    private readonly string sqlServerConnStr;
    private readonly string mySqlConnStr;
    private readonly string mySqlDatabase;

    public IndexMigrator()
    {
        sqlServerConnStr = ConfigurationManager
            .ConnectionStrings["SqlServerConn"]
            .ConnectionString;

        mySqlConnStr = ConfigurationManager
            .ConnectionStrings["MySqlConn"]
            .ConnectionString;

        var csb = new MySqlConnectionStringBuilder(mySqlConnStr);
        mySqlDatabase = csb.Database;
    }

    public void MigrarIndices()
    {
        var indices = ObtenerIndicesDesdeSqlServer();

        using (var myConn = new MySqlConnection(mySqlConnStr))
        {
            myConn.Open();

            foreach (var idx in indices)
            {
                if (ExisteIndice(myConn, idx))
                {
                    Console.WriteLine(
                        $"Omitido índice `{idx.IndexName}` en `{idx.TableName}` (ya existe).");
                    continue;
                }

                string ddl = GenerarDdl(idx);
                using (var cmd = new MySqlCommand(ddl, myConn))
                {
                    cmd.ExecuteNonQuery();
                    Console.WriteLine(
                        $"Creado índice `{idx.IndexName}` en `{idx.TableName}`.");
                }
            }
        }
    }

    private List<IndexInfo> ObtenerIndicesDesdeSqlServer()
    {
        var list = new List<IndexInfo>();
        const string sql = @"
SELECT
  t.name AS TableName,
  i.name AS IndexName,
  i.is_unique AS IsUnique,
  STUFF((
    SELECT ',' + QUOTENAME(c.name)
    FROM sys.index_columns ic
    JOIN sys.columns c
      ON ic.object_id = c.object_id
     AND ic.column_id = c.column_id
    WHERE ic.object_id = i.object_id
      AND ic.index_id  = i.index_id
    ORDER BY ic.key_ordinal
    FOR XML PATH(''), TYPE
  ).value('.', 'NVARCHAR(MAX)'), 1, 1, '') AS ColumnsList
FROM sys.indexes i
JOIN sys.tables t
  ON i.object_id = t.object_id
WHERE t.is_ms_shipped = 0
  AND i.is_primary_key = 0
  AND i.name IS NOT NULL
ORDER BY t.name, i.name;";

        using (var sqlConn = new SqlConnection(sqlServerConnStr))
        using (var cmd = new SqlCommand(sql, sqlConn))
        {
            sqlConn.Open();
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    // Limpio corchetes y espacios, luego lo convierto en lista
                    var rawCols = reader.GetString(3)
                                        .Replace("[", "")
                                        .Replace("]", "");
                    var cols = rawCols
                                .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                .Select(c => c.Trim())
                                .ToList();

                    list.Add(new IndexInfo
                    {
                        TableName = reader.GetString(0),
                        IndexName = reader.GetString(1),
                        IsUnique = reader.GetBoolean(2),
                        Columns = cols
                    });
                }
            }
        }

        return list;
    }

    private bool ExisteIndice(MySqlConnection conn, IndexInfo idx)
    {
        const string checkSql = @"
SELECT COUNT(*)
FROM information_schema.STATISTICS
WHERE TABLE_SCHEMA = @schema
  AND TABLE_NAME   = @table
  AND INDEX_NAME   = @idxName;
";
        using (var cmd = new MySqlCommand(checkSql, conn))
        {
            cmd.Parameters.AddWithValue("@schema", mySqlDatabase);
            cmd.Parameters.AddWithValue("@table", idx.TableName);
            cmd.Parameters.AddWithValue("@idxName", idx.IndexName);
            return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
        }
    }

    private string GenerarDdl(IndexInfo idx)
    {
        // Envolvemos cada columna en backticks para MySQL
        string cols = string.Join(", ",
            idx.Columns.Select(c => $"`{c}`"));

        string ixType = idx.IsUnique
            ? "ADD UNIQUE INDEX"
            : "ADD INDEX";

        return $@"
ALTER TABLE `{idx.TableName}`
  {ixType} `{idx.IndexName}` ({cols});
".Trim();
    }

    private class IndexInfo
    {
        public string TableName;
        public string IndexName;
        public bool IsUnique;
        public List<string> Columns;
    }
}