using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Text.RegularExpressions;

public class ConstraintMigrator
{
    private readonly string sqlServerConnStr;
    private readonly string mySqlConnStr;
    private readonly string mySqlDatabase;

    public ConstraintMigrator()
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

    public void MigrarTodosConstraints()
    {
        var constraints = ObtenerConstraintsSqlServer();

        using (var myConn = new MySqlConnection(mySqlConnStr))
        {
            myConn.Open();
            // DEBUG: muestra todo lo que trajimos de SQL Server
            Console.WriteLine("=== Constraints detectados en SQL Server ===");
            foreach (var c in constraints)
            {
                Console.WriteLine(
                  $"{c.Type} `{c.Name}` en `{c.Table}`"
                  + (c.Columns != null ? $" cols:[{string.Join(",", c.Columns)}]" : "")
                  + (c.Type == "DEFAULT" ? $" def:{c.Default}" : "")
                  + (c.Type == "CHECK" ? $" chk:{c.CheckClause}" : "")
                );
            }
            Console.WriteLine("==========================================");

            foreach (var c in constraints)
            {
                if (ExisteConstraint(myConn, c))
                {
                    Console.WriteLine(
                        $"Omitido {c.Type} `{c.Name}` en `{c.Table}` (ya existe).");
                    continue;
                }

                string ddl = GenerarDdl(c);
                using (var cmd = new MySqlCommand(ddl, myConn))
                {
                    Console.WriteLine(" -> DDL MySQL: " + ddl);

                    //cmd.ExecuteNonQuery();
                    Console.WriteLine(
                        $"Creado {c.Type} `{c.Name}` en `{c.Table}`.");
                }
            }
        }
    }

    private List<ConstraintInfo> ObtenerConstraintsSqlServer()
    {
        var lista = new List<ConstraintInfo>();

        using (var conn = new SqlConnection(sqlServerConnStr))
        {
            conn.Open();

            // 1) PK, UNIQUE y FOREIGN KEY
            const string sqlPkUqFk = @"
SELECT
  tc.TABLE_NAME,
  tc.CONSTRAINT_NAME,
  tc.CONSTRAINT_TYPE,
  STUFF((
    SELECT ', ' + kcu2.COLUMN_NAME
    FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE kcu2
    WHERE
      kcu2.TABLE_SCHEMA       = tc.TABLE_SCHEMA
      AND kcu2.TABLE_NAME     = tc.TABLE_NAME
      AND kcu2.CONSTRAINT_NAME = tc.CONSTRAINT_NAME
    ORDER BY kcu2.ORDINAL_POSITION
    FOR XML PATH(''), TYPE
  ).value('.', 'NVARCHAR(MAX)'), 1, 2, '') AS ColumnsList,
  rc.UPDATE_RULE,
  rc.DELETE_RULE
FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS tc
LEFT JOIN INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS rc
  ON rc.CONSTRAINT_SCHEMA = tc.TABLE_SCHEMA
     AND rc.CONSTRAINT_NAME = tc.CONSTRAINT_NAME
WHERE
  tc.TABLE_SCHEMA   = DB_NAME()
  AND tc.CONSTRAINT_TYPE IN ('PRIMARY KEY','UNIQUE','FOREIGN KEY');";

            using (var cmd1 = new SqlCommand(sqlPkUqFk, conn))
            using (var reader1 = cmd1.ExecuteReader())
            {
                while (reader1.Read())
                {
                    lista.Add(new ConstraintInfo
                    {
                        Table = reader1.GetString(0),
                        Name = reader1.GetString(1),
                        Type = reader1.GetString(2),
                        Columns = reader1.GetString(3)
                                             .Split(new[] { ',' },
                                                    StringSplitOptions.RemoveEmptyEntries),
                        UpdateRule = reader1.IsDBNull(4) ? null : reader1.GetString(4),
                        DeleteRule = reader1.IsDBNull(5) ? null : reader1.GetString(5)
                    });
                }
            }

            // 2) DEFAULT constraints (con DATA_TYPE y longitud/precisión)
            const string sqlDefaults = @"
SELECT
  t.name          AS TableName,
  dc.name         AS ConstraintName,
  c.name          AS ColumnName,
  dc.definition   AS DefaultValue,
  -- Construimos aquí la definición completa del tipo:
  CASE
    WHEN ic.DATA_TYPE IN ('varchar','char','nvarchar','nchar')
      THEN ic.DATA_TYPE + '(' 
           + CAST(ic.CHARACTER_MAXIMUM_LENGTH AS VARCHAR(10)) 
           + ')'
    WHEN ic.DATA_TYPE IN ('decimal','numeric')
      THEN ic.DATA_TYPE + '(' 
           + CAST(ic.NUMERIC_PRECISION AS VARCHAR(10)) 
           + ',' 
           + CAST(ic.NUMERIC_SCALE AS VARCHAR(10)) 
           + ')'
    ELSE ic.DATA_TYPE
  END AS DataType
FROM sys.default_constraints dc
JOIN sys.tables t
  ON dc.parent_object_id = t.object_id
JOIN sys.columns c
  ON dc.parent_object_id = c.object_id
 AND dc.parent_column_id  = c.column_id
JOIN INFORMATION_SCHEMA.COLUMNS ic
  ON ic.TABLE_SCHEMA = DB_NAME()
 AND ic.TABLE_NAME   = t.name
 AND ic.COLUMN_NAME  = c.name
WHERE t.schema_id = SCHEMA_ID();";

            using (var cmd2 = new SqlCommand(sqlDefaults, conn))
            using (var reader2 = cmd2.ExecuteReader())
            {
                while (reader2.Read())
                {
                    lista.Add(new ConstraintInfo
                    {
                        Table = reader2.GetString(0),
                        Name = reader2.GetString(1),
                        Type = "DEFAULT",
                        Columns = new[] { reader2.GetString(2) },
                        Default = reader2.GetString(3),
                        DataType = reader2.GetString(4)
                    });
                }
            }

            // 3) CHECK constraints
            const string sqlChecks = @"
SELECT
  t.name          AS TableName,
  cc.name         AS ConstraintName,
  cc.definition   AS CheckClause
FROM sys.check_constraints cc
JOIN sys.tables t
  ON cc.parent_object_id = t.object_id
WHERE t.schema_id = SCHEMA_ID();";

            using (var cmd3 = new SqlCommand(sqlChecks, conn))
            using (var reader3 = cmd3.ExecuteReader())
            {
                while (reader3.Read())
                {
                    lista.Add(new ConstraintInfo
                    {
                        Table = reader3.GetString(0),
                        Name = reader3.GetString(1),
                        Type = "CHECK",
                        CheckClause = reader3.GetString(2)
                    });
                }
            }
        }

        return lista;
    }

    private bool ExisteConstraint(MySqlConnection conn, ConstraintInfo c)
    {
        if (c.Type == "DEFAULT")
        {
            const string sql = @"
SELECT COUNT(*) 
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_SCHEMA   = @schema
  AND TABLE_NAME     = @table
  AND COLUMN_NAME    = @col
  AND COLUMN_DEFAULT = @def;";

            using (var cmd = new MySqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@schema", mySqlDatabase);
                cmd.Parameters.AddWithValue("@table", c.Table);
                cmd.Parameters.AddWithValue("@col", c.Columns[0]);
                cmd.Parameters.AddWithValue("@def", MapDefaultValue(c.Default));
                return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
            }
        }
        else
        {
            const string sql = @"
SELECT COUNT(*)
FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS tc
WHERE tc.TABLE_SCHEMA    = @schema
  AND tc.TABLE_NAME      = @table
  AND tc.CONSTRAINT_NAME = @name
  AND tc.CONSTRAINT_TYPE = @type;";

            using (var cmd = new MySqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@schema", mySqlDatabase);
                cmd.Parameters.AddWithValue("@table", c.Table);
                cmd.Parameters.AddWithValue("@name", c.Name);
                cmd.Parameters.AddWithValue("@type", c.Type);
                return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
            }
        }
    }

    private string GenerarDdl(ConstraintInfo c)
    {
        var tbl = $"`{c.Table}`";

        switch (c.Type)
        {
            case "PRIMARY KEY":
                return $"ALTER TABLE {tbl} ADD PRIMARY KEY ({FormateaCols(c.Columns)});";

            case "UNIQUE":
                return $"ALTER TABLE {tbl} ADD UNIQUE KEY `{c.Name}` ({FormateaCols(c.Columns)});";

            case "FOREIGN KEY":
                return $@"
ALTER TABLE {tbl}
  ADD CONSTRAINT `{c.Name}`
  FOREIGN KEY ({FormateaCols(c.Columns)})
  REFERENCES `{c.ReferencedTable}` ({FormateaCols(c.ReferencedColumns)})
  ON DELETE {MapRule(c.DeleteRule)}
  ON UPDATE {MapRule(c.UpdateRule)};".Trim();

            case "DEFAULT":
                var mapped = MapDefaultValue(c.Default);
                return $@"
ALTER TABLE {tbl}
  MODIFY `{c.Columns[0]}` {c.DataType}
    DEFAULT {mapped};".Trim();

            case "CHECK":
                {
                    // ① Limpiamos la cláusula con tu helper
                    var cleaned = MapCheckClause(c.CheckClause);

                    // ② Usamos la variable recién creada
                    return $@"
ALTER TABLE {tbl}
  ADD CONSTRAINT `{c.Name}`
  CHECK ({cleaned});".Trim();
                }

            default:
                throw new NotSupportedException($"Tipo de constraint no soportado: {c.Type}");
        }
    }

    private string FormateaCols(string[] cols)
        => string.Join(", ", Array.ConvertAll(cols, c => $"`{c}`"));

    private string MapRule(string rule)
    {
        switch (rule?.ToUpperInvariant())
        {
            case "NO ACTION": return "RESTRICT";
            case "SET NULL": return "SET NULL";
            case "CASCADE": return "CASCADE";
            case "SET DEFAULT": return "RESTRICT";
            default: return rule ?? "RESTRICT";
        }
    }

    private string MapDefaultValue(string sqlServerDef)
    {
        var def = sqlServerDef.Trim();

        // Quitar paréntesis envolventes
        if (def.StartsWith("(") && def.EndsWith(")"))
            def = def.Substring(1, def.Length - 2).Trim();

        // Funciones de fecha de SQL Server → MySQL
        if (def.Equals("getdate()", StringComparison.OrdinalIgnoreCase)
         || def.Equals("sysdatetime()", StringComparison.OrdinalIgnoreCase))
            return "CURRENT_TIMESTAMP";

        return def;
    }

    private string MapCheckClause(string sqlServerClause)
    {
        // 1) Quitar espacios y paréntesis envolventes
        var clause = sqlServerClause.Trim();
        while (clause.StartsWith("(") && clause.EndsWith(")"))
            clause = clause.Substring(1, clause.Length - 2).Trim();

        // 2) Quitar corchetes de columnas
        clause = clause.Replace("[", "").Replace("]", "");

        // 3) Reemplazos insensibles a mayúsculas con Regex
        clause = Regex.Replace(
            clause,
            @"getdate\(\)",
            "CURRENT_TIMESTAMP",
            RegexOptions.IgnoreCase);

        clause = Regex.Replace(
            clause,
            @"sysdatetime\(\)",
            "CURRENT_TIMESTAMP",
            RegexOptions.IgnoreCase);

        return clause;
    }
    private class ConstraintInfo
    {
        public string Table;
        public string Name;
        public string Type;
        public string[] Columns;
        public string Default;
        public string DataType;
        public string CheckClause;
        public string UpdateRule;
        public string DeleteRule;
        public string ReferencedTable;
        public string[] ReferencedColumns;
    }
}