using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DWClient.Models;

namespace DWClient
{
    public class DWMetadataFramework
    {
        private readonly string connectionString;

        public DWMetadataFramework(string connString)
        {
            connectionString = connString;
        }

        public IEnumerable<TableMetadata> GetFactTables()
        {
            var tableName = "tablica";
            var condition = $"{new TableMetadata().Type.SqlName} = 1";
            var result = connectionString.GetTypedTableData(tableName, condition);
            return result.Select(r => new TableMetadata(r.Row));
        }

        
    }
}
