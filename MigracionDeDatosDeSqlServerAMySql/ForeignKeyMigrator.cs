using System;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

public class ForeignKeyMigrator
{
    private readonly string sqlServerConnStr;
    private readonly string mySqlConnStr;
    private readonly string mySqlDatabase;

    public ForeignKeyMigrator(string sqlServerConn, string mySqlConn, string mySqlDatabase)
    {
        sqlServerConnStr = sqlServerConn;
        mySqlConnStr = mySqlConn;
        this.mySqlDatabase = mySqlDatabase;
    }

    public void MigrarForeignKeys()
    {
        var fkDefinitions = ObtenerDefinitionsDesdeSqlServer();

        using (var myConn = new MySqlConnection(mySqlConnStr))
        {
            myConn.Open();

            foreach (var fk in fkDefinitions)
            {
                // 1. Verificar si ya existe
                bool exists = false;
                string checkSql = @"
SELECT COUNT(*) 
FROM information_schema.TABLE_CONSTRAINTS
WHERE CONSTRAINT_SCHEMA = @schema
  AND TABLE_NAME       = @parentTable
  AND CONSTRAINT_NAME  = @fkName
  AND CONSTRAINT_TYPE  = 'FOREIGN KEY';";

                using (var checkCmd = new MySqlCommand(checkSql, myConn))
                {
                    checkCmd.Parameters.AddWithValue("@schema", mySqlDatabase);
                    checkCmd.Parameters.AddWithValue("@parentTable", fk.ParentTable);
                    checkCmd.Parameters.AddWithValue("@fkName", fk.Name);
                    long count = (long)checkCmd.ExecuteScalar();
                    exists = count > 0;
                }

                if (exists)
                {
                    Console.WriteLine($"Omitida FK `{fk.Name}` en `{fk.ParentTable}` porque ya existe.");
                    continue;
                }

                // 2. Crear la FK si no existe
                string ddl = $@"
ALTER TABLE `{fk.ParentTable}`
ADD CONSTRAINT `{fk.Name}`
FOREIGN KEY (`{fk.ParentColumn}`)
REFERENCES `{fk.ReferencedTable}` (`{fk.ReferencedColumn}`)
ON DELETE {MapAction(fk.DeleteAction)}
ON UPDATE {MapAction(fk.UpdateAction)};";

                using (var cmd = new MySqlCommand(ddl, myConn))
                {
                    cmd.ExecuteNonQuery();
                    Console.WriteLine($"Creada FK `{fk.Name}` en `{fk.ParentTable}`");
                }
            }
        }
    }

    private List<ForeignKeyInfo> ObtenerDefinitionsDesdeSqlServer()
    {
        var list = new List<ForeignKeyInfo>();
        const string sql = @"
SELECT
  fk.name                        AS ForeignKeyName,
  tp.name                        AS ParentTable,
  cp.name                        AS ParentColumn,
  tr.name                        AS ReferencedTable,
  cr.name                        AS ReferencedColumn,
  fk.update_referential_action_desc AS UpdateAction,
  fk.delete_referential_action_desc AS DeleteAction
FROM sys.foreign_keys fk
JOIN sys.foreign_key_columns fkc
  ON fk.object_id = fkc.constraint_object_id
JOIN sys.tables tp
  ON fkc.parent_object_id = tp.object_id
JOIN sys.columns cp
  ON fkc.parent_object_id = cp.object_id
 AND fkc.parent_column_id = cp.column_id
JOIN sys.tables tr
  ON fkc.referenced_object_id = tr.object_id
JOIN sys.columns cr
  ON fkc.referenced_object_id = cr.object_id
 AND fkc.referenced_column_id = cr.column_id
ORDER BY fk.name, fkc.constraint_column_id;";

        using (var sqlConn = new SqlConnection(sqlServerConnStr))
        using (var cmd = new SqlCommand(sql, sqlConn))
        {
            sqlConn.Open();
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    list.Add(new ForeignKeyInfo
                    {
                        Name = reader.GetString(0),
                        ParentTable = reader.GetString(1),
                        ParentColumn = reader.GetString(2),
                        ReferencedTable = reader.GetString(3),
                        ReferencedColumn = reader.GetString(4),
                        UpdateAction = reader.GetString(5),
                        DeleteAction = reader.GetString(6)
                    });
                }
            }
        }

        return list;
    }

    private string MapAction(string sqlServerAction)
    {
        switch (sqlServerAction.ToUpper())
        {
            case "NO_ACTION": return "RESTRICT";
            case "CASCADE": return "CASCADE";
            case "SET_NULL": return "SET NULL";
            case "SET_DEFAULT": return "RESTRICT";
            default: return sqlServerAction;
        }
    }

    private class ForeignKeyInfo
    {
        public string Name;
        public string ParentTable;
        public string ParentColumn;
        public string ReferencedTable;
        public string ReferencedColumn;
        public string UpdateAction;
        public string DeleteAction;
    }
}