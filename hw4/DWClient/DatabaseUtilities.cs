using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using DWClient.Models;

namespace DWClient
{
    public static class DatabaseUtilities
    {
        public static List<TypedDatabaseResult> GetTypedTableData(this string connectionString, string query)
        {
            // Return variable
            var results = new List<TypedDatabaseResult>();

            // Parse query
            var columns = GetColumns(query);
            
            // Get data
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();


                try
                {
                    using (var reader = new SqlCommand(query, sqlConnection).ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var rowResult = new TypedDatabaseResult();
                            int i = 0;
                            foreach (var colName in columns)
                            {
                                rowResult.Row.Add(colName, reader[i++].ToString().Trim());
                            }
                            results.Add(rowResult);
                        }
                    }
                }
                catch (Exception)
                {
                    return new List<TypedDatabaseResult>();
                }
            }

            return results;
        }

        public static string[] GetColumns(this string query)
        {
            var startSequence = "SELECT";
            var endSequence = "FROM";
            var startIdx = query.IndexOf(startSequence) + startSequence.Length;
            var endIdx = query.IndexOf(endSequence);

            if (startIdx == startSequence.Length - 1 || endIdx == -1)
                throw new Exception($"Unable to parse column names from sql query '{query}'");

            return query.Substring(startIdx, endIdx - startIdx).Split(',').Select(c => c.Trim()).ToArray();
        }
    }
}
