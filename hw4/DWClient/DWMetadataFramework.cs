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
            var t = "tablica";
            var query = $@"SELECT {t}.sifTablica, {t}.nazTablica, {t}.nazSQLTablica, {t}.sifTipTablica
                           FROM tablica
                           WHERE {t}.sifTipTablica = 1 ";
            var result = connectionString.GetTypedTableData(query);
            return result.Select(r => new TableMetadata(t, r.Row));
        }

        public IEnumerable<Measurement> GetMeasurements(TableMetadata table)
        {
            string[] tables = { "tablica", "tabAtribut", "agrFun", "tabAtributAgrFun" };
            var query = $@"SELECT tabAtribut.sifTablica, tabAtribut.rbrAtrib, tabAtribut.imeSQLAtrib,
                            tabAtribut.sifTipAtrib, tabAtribut.imeAtrib, agrFun.sifAgrFun, agrFun.nazAgrFun,
                            tablica.sifTablica, tablica.nazTablica, tablica.nazSQLTablica, tablica.sifTipTablica,
                            tabAtributAgrFun.sifTablica, tabAtributAgrFun.rbrAtrib, tabAtributAgrFun.sifAgrFun, tabAtributAgrFun.imeAtrib
                        FROM tablica, tabAtribut, agrFun, tabAtributAgrFun
                        WHERE tabAtribut.sifTablica = {table.ID.Value}
                            AND tabAtribut.sifTablica = tablica.sifTablica
                            AND tabAtribut.sifTablica = tabAtributAgrFun.sifTablica
                            AND tabAtribut.rbrAtrib = tabAtributAgrFun.rbrAtrib
                            AND tabAtributAgrFun.sifAgrFun = agrFun.sifAgrFun
                            AND tabAtribut.sifTipAtrib = 1
                        ORDER BY tabAtribut.rbrAtrib";

            var result = connectionString.GetTypedTableData(query);
            return result.Select(r => GetMeasurement(tables[0], tables[1], tables[2], tables[3], r.Row));
        }

        public IEnumerable<Dimension> GetDimensions(TableMetadata table)
        {
            var query =
                $@"SELECT dimTablica.nazTablica, dimTablica.nazSQLTablica AS nazSqlDimTablica,
                        cinjTablica.nazSQLTablica AS nazSqlCinjTablica, cinjTabAtribut.imeSQLAtrib,
                        dimTabAtribut.imeSqlAtrib,
                        tabAtribut.sifTablica, tabAtribut.rbrAtrib, tabAtribut.imeSQLAtrib, tabAtribut.sifTipAtrib, tabAtribut.imeAtrib
                   FROM tabAtribut, dimCinj, tablica dimTablica, tablica cinjTablica, tabAtribut cinjTabAtribut, tabAtribut dimTabAtribut
                   WHERE dimCinj.sifDimTablica = dimTablica.sifTablica
                        AND dimCinj.sifCinjTablica = cinjTablica.sifTablica
                        AND dimCinj.sifCinjTablica = cinjTabAtribut.sifTablica
                        AND dimCinj.rbrCinj = cinjTabAtribut.rbrAtrib
                        AND dimCinj.sifDimTablica = dimTabAtribut.sifTablica
                        AND dimCinj.rbrDim = dimTabAtribut.rbrAtrib
                        AND tabAtribut.sifTablica = dimCinj.sifDimTablica
                        AND sifCinjTablica = {table.ID.Value}
                        AND tabAtribut.sifTipAtrib = 2
                        ORDER BY dimTablica.nazTablica, rbrAtrib";

            var result = connectionString.GetTypedTableData(query);
            return result.Select(r => GetDimension(r.Row));
        }

        private Dimension GetDimension(Dictionary<string, string> result)
        {
            var dim = new Dimension
            {
                TableAttributeMetadata = new AttributeMetadata("tabAtribut", result),
                DimTableAttributeSqlName = result["dimTabAtribut.imeSqlAtrib"],
                FactTableAttributeSqlName = result["cinjTabAtribut.imeSQLAtrib"],
                DimTableSqlName = result["dimTablica.nazSQLTablica AS nazSqlDimTablica"],
                DimTableName = result["dimTablica.nazTablica"],
                FactTableSqlName = result["cinjTablica.nazSQLTablica AS nazSqlCinjTablica"]
            };

            return dim;
        }

        private Measurement GetMeasurement(string tableMetadataTableName, string attributeMetadataTableName,
            string aggrFunMetadataTableName, string attributeAggFun, Dictionary<string, string> result)
        {
            var m = new Measurement
            {
                TableMetadata = new TableMetadata(tableMetadataTableName, result),
                AttributeMetadata = new AttributeMetadata(attributeMetadataTableName, result),
                AggrFunMetadata = new AggregateFunctionMetadata(aggrFunMetadataTableName, result),
                AttributeAggrFunName = new DatabaseObject<string>($"{attributeAggFun}.imeAtrib")
            };
            m.AttributeAggrFunName.Value = result[m.AttributeAggrFunName.SqlName];
            return m;
        }

        public IEnumerable<TypedDatabaseResult> ExecuteQuery(string query)
        {
            return connectionString.GetTypedTableData(query);
        }
    }
}
