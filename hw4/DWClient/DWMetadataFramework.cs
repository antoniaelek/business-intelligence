using System;
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
            var condition = $"{new TableMetadata(tableName).Type.SqlName} = 1";
            var result = connectionString.GetTypedTableData(tableName, condition);
            return result.Select(r => new TableMetadata(tableName, r.Row));
        }

        public IEnumerable<Measurement> GetMeasurements(TableMetadata table)
        {
            string[] tableNames = {"tabAtribut", "agrFun", "tablica", "tabAtributAgrFun"};
            var condition = $"tabAtribut.sifTablica = {table.ID.Value} " +
                            "AND tabAtribut.sifTablica = tablica.sifTablica " +
                            "AND tabAtribut.sifTablica = tabAtributAgrFun.sifTablica " +
                            "AND tabAtribut.rbrAtrib = tabAtributAgrFun.rbrAtrib " +
                            "AND tabAtributAgrFun.sifAgrFun = agrFun.sifAgrFun " +
                            "AND tabAtribut.sifTipAtrib = 1";
            var result = connectionString.GetUntypedTableData(string.Join(", ",tableNames), condition);
            return result.Select(r => new Measurement(r.Row));
        }
        
    }
}
