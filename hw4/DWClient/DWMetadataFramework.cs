using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public static string GenerateSqlQuery(TableMetadata fTable, IEnumerable<Measurement> measurements, IEnumerable<Dimension> dimensions)
        {
            var query = new StringBuilder();
            var dims = dimensions as IList<Dimension> ?? dimensions.ToList();
            var dinstinctDimensions = new HashSet<Dimension>(dims);

            // SELECT part
            var selectPart = GenerateSqlSelectPart(measurements, dims, fTable);
            if (string.IsNullOrWhiteSpace(selectPart))
                return null;

            query.Append($"SELECT {selectPart}");

            // FROM part
            var fromPart = GenerateSqlFromPart(fTable, dinstinctDimensions);
            query.Append(string.IsNullOrWhiteSpace(fromPart) ? string.Empty : $"{Environment.NewLine}FROM\t{fromPart}");

            // WHERE PART
            var wherePart = GenerateSqlWherePart(fTable, dinstinctDimensions);
            query.Append(string.IsNullOrWhiteSpace(wherePart) ? string.Empty : $"{Environment.NewLine}WHERE\t{wherePart}");

            // GROUP BY part
            var groupByPart = GenerateSqlGroupByPart(dims);
            query.Append(string.IsNullOrWhiteSpace(groupByPart) ? string.Empty : $"{Environment.NewLine}GROUP BY {groupByPart}");

            return query.ToString();
        }

        private static string GenerateSqlWherePart(TableMetadata fTable, HashSet<Dimension> joinedDimensions)
        {
            var query = new StringBuilder();
            var whereConditions = new HashSet<string>();
            foreach (var table in joinedDimensions)
            {
                whereConditions.Add(
                    $"{fTable.SqlName.Value}.{table.FactTableAttributeSqlName} = {table.DimTableSqlName}.{table.DimTableAttributeSqlName}");
            }
            var wherePart = string.Join($"{Environment.NewLine}  AND\t", whereConditions);
            query.Append($"{wherePart}");
            return query.ToString();
        }

        private static string GenerateSqlFromPart(TableMetadata fTable, HashSet<Dimension> joinedDimensions)
        {
            var query = new StringBuilder();
            var fromTables = new HashSet<string>(joinedDimensions.Select(d => d.DimTableSqlName)) { fTable.SqlName.Value };
            var fromPart = string.Join($"{Environment.NewLine}\t,", fromTables);
            query.Append($" {fromPart}");
            return query.ToString();
        }

        private static string GenerateSqlSelectPart(IEnumerable<Measurement> measurements, IEnumerable<Dimension> dimensions,
            TableMetadata fTable)
        {
            var query = new StringBuilder();
            var selectList = new List<string>();
            foreach (var m in measurements)
            {
                selectList.Add(
                    $"{m.AggrFunMetadata.Name.Value}({fTable.SqlName.Value}.{m.AttributeMetadata.SqlName.Value}) AS '{m.AttributeAggrFunName.Value}'");
            }

            foreach (var d in dimensions)
            {
                selectList.Add($"{d.DimTableSqlName}.{d.TableAttributeMetadata.SqlName.Value} AS '{d.TableAttributeMetadata.Name.Value}'");
            }

            var selectPart = string.Join($"{Environment.NewLine}\t,", selectList);
            query.Append($" {selectPart}");

            return query.ToString();
        }

        private static string GenerateSqlGroupByPart(IEnumerable<Dimension> dimensions)
        {
            var query = new StringBuilder();
            var list = new List<string>();
            foreach (var d in dimensions)
            {
                list.Add($"{d.DimTableSqlName}.{d.TableAttributeMetadata.SqlName.Value}");
            }

            var part = string.Join($"{Environment.NewLine}\t,", list);
            query.Append($"{part}");

            return query.ToString();
        }

        public static string GetAlias(string selectQueryPart)
        {
            var startIdx = selectQueryPart.IndexOf('\'');
            var endIdx = selectQueryPart.LastIndexOf('\'');
            if (startIdx == endIdx)
                return selectQueryPart;
            return selectQueryPart.Substring(startIdx + 1, endIdx - startIdx - 1);
        }
    }
}
