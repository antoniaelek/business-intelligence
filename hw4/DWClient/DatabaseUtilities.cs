using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using DWClient.Models;

namespace DWClient
{
    public static class DatabaseUtilities
    {
        private const char EncloseCharOpen = '[';
        private const char EncloseCharClose = ']';

        /// <summary>
        /// Gets unenclosed table name without schema qualifier.
        /// </summary>
        /// <param name="tableName">Tale name</param>
        /// <returns>Simple table name</returns>
        public static string GetSimpleTableName(this string tableName)
        {
            if (tableName.Contains($"{EncloseCharClose}.{EncloseCharOpen}"))
                tableName = tableName.Substring(tableName.IndexOf($"{EncloseCharClose}.{EncloseCharOpen}", StringComparison.Ordinal) + 2);

            if (tableName.Contains("."))
                tableName = tableName.Substring(tableName.IndexOf(".", StringComparison.Ordinal) + 1);

            if (tableName.StartsWith(EncloseCharOpen.ToString()) && tableName.EndsWith(EncloseCharClose.ToString()))
                tableName = tableName.Substring(1, tableName.Length - 2);

            return tableName;
        }

        /// <summary>
        /// Encloses name with specified characters.
        /// </summary>
        /// <param name="name">Name to enclose</param>
        /// <returns>Enclosed name</returns>
        public static string EncloseObjectName(this string name)
        {
            if (name.StartsWith(EncloseCharOpen.ToString()) && name.EndsWith(EncloseCharClose.ToString()))
                return name;

            return $"{EncloseCharOpen}{name}{EncloseCharClose}";
        }

        /// <summary>
        /// Gets column names for the specified database table.
        /// </summary>
        /// <param name="connectionString">Database connectionstring</param>
        /// <param name="tables">Database table name</param>
        /// <returns>Column names for the specified table</returns>
        public static HashSet<string> GetTablesColumns(this string connectionString, string[] tables)
        {
            // We will store result here
            var columnNames = new HashSet<string>();
            foreach (var table in tables)
            {
                var tableColumnNames = GetTableColumns(connectionString, table);
                foreach (var columnName in tableColumnNames)
                {
                    columnNames.Add(columnName);
                }
            }
            

            return columnNames;
        }

        private static HashSet<string> GetTableColumns(string connectionString, string table)
        {
            var query = $"SELECT TOP 0 * FROM {table}";
            var set = new HashSet<string>();
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                using (var reader = new SqlCommand(query, sqlConnection).ExecuteReader(CommandBehavior.SchemaOnly))
                {
                    // We also don't care if reader has more results, it is enough to read only once
                    reader.Read();

                    // Get table schema
                    var tableSchema = reader.GetSchemaTable();

                    // Get column names
                    if (tableSchema?.Rows == null)
                        return set;

                    foreach (DataRow row in tableSchema.Rows)
                        set.Add(table + "." + row["ColumnName"]);
                }
            }
            return set;
        }

        /// <summary>
        /// Gets typed data from database table.
        /// </summary>
        /// <param name="connectionString">Database connectionstring</param>
        /// <param name="table">Database table name</param>
        /// <param name="condition">Optional condition that will be added in WHERE part of SQL query</param>
        /// <returns>Rows of data from table</returns>
        public static List<TypedDatabaseResult> GetTypedTableData(this string connectionString, string table, string condition = null)
        {
            // Get column names
            var tables = table.Split(',').Select(t => t.Trim()).ToArray();
            var columns = GetTablesColumns(connectionString, tables).ToArray();

            // We will store entities from database in this list
            var results = new List<TypedDatabaseResult>();

            // Action to invoke on sql reader
            Action<SqlDataReader> readerAction = (reader) =>
            {
                var rowResult = new TypedDatabaseResult();
                int i = 0;
                foreach (var colName in columns)
                {
                    rowResult.Row.Add(colName, reader[i++].ToString().Trim());
                }
                results.Add(rowResult);
            };

            // Get data
            ReadTableData(connectionString, table, readerAction, condition, columns);

            return results;
        }

        /// <summary>
        /// Gets untyped data from database table.
        /// </summary>
        /// <param name="connectionString">Database connectionstring</param>
        /// <param name="table">Database table name</param>
        /// <param name="condition">Optional condition that will be added in WHERE part of SQL query</param>
        /// <returns>Rows of data from table</returns>
        public static List<UntypedDatabaseResult> GetUntypedTableData(this string connectionString, string table, string condition = null)
        {
            // We will store entities from database in this list
            var results = new List<UntypedDatabaseResult>();

            // Action to invoke on sql reader
            Action<SqlDataReader> readAction = (reader) =>
            {
                var rowResult = new UntypedDatabaseResult();
                for (var i = 0; i < reader.FieldCount; i++)
                {
                    rowResult.Row.Add(reader[i].ToString().Trim());
                }
                results.Add(rowResult);
            };

            // Get data
            ReadTableData(connectionString, table, readAction, condition);

            return results;
        }

        /// <summary>
        /// Gets data from database table.
        /// </summary>
        /// <param name="connectionString">Database connectionstring</param>
        /// <param name="table">Database table name</param>
        /// <param name="readAction">Action to invoke on each row of data from table</param>
        /// <param name="condition">Optional condition that will be added in WHERE part of SQL query</param>
        /// <param name="columns">Optional table column names</param>
        private static void ReadTableData(string connectionString, string table,
            Action<SqlDataReader> readAction, string condition = null, params string[] columns)
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();

                var tables = table.Split(',').Select(t => t.Trim());
                var defaultColumns = tables.Select(t => $"{t}.*").ToArray();

                columns = columns == default(string[]) || columns.Length == 0 ? defaultColumns : columns;

                var query = $"SELECT {string.Join(", ", columns)} FROM {table}";
                if (condition != null)
                    query += $" WHERE {condition}";

                using (var reader = new SqlCommand(query, sqlConnection).ExecuteReader())
                {
                    while (reader.Read())
                    {
                        readAction(reader);
                    }
                }
            }
        }
    }
}
