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
            var query = GenerateSqlQuery(fTable, measurements, dimensions);
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

            var columns = query.GetColumns().Select(c => new {colName = c, colAlias = GetAlias(c)}).ToList();
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
            var name = selectQueryPart.Substring(selectQueryPart.LastIndexOf(' ') + 1).Trim();
            var startIdx = name.IndexOf('\'');
            var endIdx = name.LastIndexOf('\'');
            return name.Substring(startIdx+1, endIdx - startIdx-1);
        }

        private string GenerateSqlQuery(TableMetadata fTable, IEnumerable<Measurement> measurements, IEnumerable<Dimension> dimensions)
        {
            var query = new StringBuilder("SELECT");
            var dims = dimensions as IList<Dimension> ?? dimensions.ToList();
            var dinstinctDimensions = new HashSet<Dimension>(dims);

            // SELECT part
            query.Append(GenerateSqlSelectPart(measurements, dims, fTable));

            // FROM part
            query.Append(GenerateSqlFromPart(fTable, dinstinctDimensions));

            // WHERE PART
            query.Append(GenerateSqlWherePart(fTable, dinstinctDimensions));

            return query.ToString();
        }

        private static string GenerateSqlWherePart(TableMetadata fTable, HashSet<Dimension> joinedDimensions)
        {
            var query = new StringBuilder();
            query.Append($"{Environment.NewLine}WHERE");
            var whereConditions = new HashSet<string>();
            foreach (var table in joinedDimensions)
            {
                whereConditions.Add(
                    $"{fTable.SqlName.Value}.{table.FactTableAttributeSqlName} = {table.DimTableSqlName}.{table.DimTableAttributeSqlName}");
            }
            var wherePart = string.Join($"{Environment.NewLine}  AND\t", whereConditions);
            query.Append($"\t{wherePart}");
            return query.ToString();
        }

        private static string GenerateSqlFromPart(TableMetadata fTable, HashSet<Dimension> joinedDimensions)
        {
            var query = new StringBuilder();
            query.Append($"{Environment.NewLine}FROM");
            var fromTables = new HashSet<string>(joinedDimensions.Select(d => d.DimTableSqlName)) {fTable.SqlName.Value};
            var fromPart = string.Join($"{Environment.NewLine}\t,", fromTables);
            query.Append($"\t {fromPart}");
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
            query.Append($"\t {selectPart}");

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
