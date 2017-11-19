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
            string[] tables = {"tablica", "tabAtribut", "agrFun", "tabAtributAgrFun"};
            var condition = $"{tables[1]}.sifTablica = {table.ID.Value} " +
                            $"AND {tables[1]}.sifTablica = {tables[0]}.sifTablica " +
                            $"AND {tables[1]}.sifTablica = {tables[3]}.sifTablica " +
                            $"AND {tables[1]}.rbrAtrib = {tables[3]}.rbrAtrib " +
                            $"AND {tables[3]}.sifAgrFun = {tables[2]}.sifAgrFun " +
                            $"AND {tables[1]}.sifTipAtrib = 1 " +
                            $"ORDER BY {tables[1]}.rbrAtrib";
            var result = connectionString.GetTypedTableData(string.Join(", ",tables), condition);
            return result.Select(r => new Measurement(tables[0], tables[1], tables[2], tables[3], r.Row));
        }

        public IEnumerable<Dimension> GetDimensions(TableMetadata table)
        {
            string[] tables =
            {
                "tabAtribut", "dimCinj", "tablica dimTablica", "tablica cinjTablica", "tabAtribut cinjTabAtribut",
                "tabAtribut dimTabAtribut"
            };
            string[] columns =
            {
                "dimTablica.nazTablica", "dimTablica.nazSQLTablica AS nazSqlDimTablica",
                "cinjTablica.nazSQLTablica AS nazSqlCinjTablica", "cinjTabAtribut.imeSQLAtrib",
                "dimTabAtribut.imeSqlAtrib",
                "tabAtribut.sifTablica", "tabAtribut.rbrAtrib", "tabAtribut.imeSQLAtrib", "tabAtribut.sifTipAtrib", "tabAtribut.imeAtrib"
            };
            var condition =
                $"dimCinj.sifDimTablica = dimTablica.sifTablica " +
                $"AND dimCinj.sifCinjTablica = cinjTablica.sifTablica " +
                $"AND dimCinj.sifCinjTablica = cinjTabAtribut.sifTablica " +
                $"AND dimCinj.rbrCinj = cinjTabAtribut.rbrAtrib " +
                $"AND dimCinj.sifDimTablica = dimTabAtribut.sifTablica " +
                $"AND dimCinj.rbrDim = dimTabAtribut.rbrAtrib " +
                $"AND tabAtribut.sifTablica = dimCinj.sifDimTablica " +
                $"AND sifCinjTablica = {table.ID.Value}" +
                $"AND tabAtribut.sifTipAtrib = 2 " +
                $"ORDER BY dimTablica.nazTablica, rbrAtrib";
            var result = connectionString.GetTypedTableData(string.Join(", ", tables), condition, columns);
            return result.Select(r => new Dimension(r.Row));
        }

    }
}
