using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DWClient.Models;

namespace DWClient.Controls
{
    public partial class SqlControl : UserControl
    {
        private readonly FactDimensionsControl factDimControl;
        private readonly ResultsControl resultsControl;
        private readonly DWMetadataFramework framework;

        public SqlControl(FactDimensionsControl factDimControl, ResultsControl resultsControl, DWMetadataFramework framework)
        {
            this.factDimControl = factDimControl;
            this.resultsControl = resultsControl;
            this.framework = framework;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var fTable = (factDimControl.fTablesComboBox.SelectedItem as ComboBoxItem)?.Value as TableMetadata;
            var measurements = GetMeasurements();
            var dimensions = GetDimensions();

            // Generate sql query
            textBox1.Clear();
            resultsControl.dataGridView1.Columns.Clear();
            resultsControl.dataGridView1.Rows.Clear();
            var query = GenerateSqlQuery(fTable, measurements, dimensions);
            
            if (query == null) return;

            textBox1.Text = query;

            // Execute sql query
            var result = framework.ExecuteQuery(query);

            // Display results
            DisplayResults(query, result);
        }

        private void DisplayResults(string query, IEnumerable<TypedDatabaseResult> result)
        {
            resultsControl.dataGridView1.Columns.Clear();
            resultsControl.dataGridView1.Rows.Clear();

            var columns = query.GetColumns().Select(c => new { colName = c, colAlias = GetAlias(c) }).ToList();
            foreach (var col in columns)
            {
                resultsControl.dataGridView1.Columns.Add(col.colName, col.colAlias);
            }

            foreach (var row in result)
            {
                var gridRow = new object[columns.Count];
                var i = 0;
                foreach (var column in columns)
                {
                    gridRow[i++] = row.Row[column.colName];
                }
                resultsControl.dataGridView1.Rows.Add(gridRow);
            }

            resultsControl.dataGridView1.Refresh();
        }

        private string GetAlias(string selectQueryPart)
        {
            var startIdx = selectQueryPart.IndexOf('\'');
            var endIdx = selectQueryPart.LastIndexOf('\'');
            if (startIdx == endIdx)
                return selectQueryPart;
            return selectQueryPart.Substring(startIdx + 1, endIdx - startIdx - 1);
        }

        private string GenerateSqlQuery(TableMetadata fTable, IEnumerable<Measurement> measurements, IEnumerable<Dimension> dimensions)
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

        private IEnumerable<Dimension> GetDimensions()
        {
            var dimensions = new List<Dimension>();
            foreach (TreeNode nodeDimension in factDimControl.dimensionsTreeView.Nodes)
            {
                foreach (DimensionTreeNode node in nodeDimension.Nodes)
                {
                    if (!node.Checked) continue;
                    dimensions.Add(node.Dimension);
                }
            }
            return dimensions;
        }

        private IEnumerable<Measurement> GetMeasurements()
        {
            var measurements = new List<Measurement>(factDimControl.measuresCheckedListBox.CheckedItems.Count);
            foreach (var checkedItem in factDimControl.measuresCheckedListBox.CheckedItems)
            {
                measurements.Add((checkedItem as ListBoxItem)?.Value as Measurement);
            }
            return measurements;
        }
    }
}
