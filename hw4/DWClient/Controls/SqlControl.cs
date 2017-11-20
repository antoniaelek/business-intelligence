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
        public SqlControl(FactDimensionsControl factDimControl)
        {
            this.factDimControl = factDimControl;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var fTable = (factDimControl.fTablesComboBox.SelectedItem as ComboBoxItem)?.Value as TableMetadata;
            var measurements = GetMeasurements();
            var dimensions = GetDimensions();
            var query = GenerateSqlQuery(fTable, measurements, dimensions);
            textBox1.Text = query;
        }

        private string GenerateSqlQuery(TableMetadata fTable, IEnumerable<Measurement> measurements, IEnumerable<Dimension> dimensions)
        {
            var query = new StringBuilder("SELECT");
            HashSet<Dimension> joinedDimensions;

            // SELECT part
            query.Append(GenerateSqlSelectPart(measurements, dimensions, fTable, out joinedDimensions));

            // FROM part
            query.Append(GenerateSqlFromPart(fTable, joinedDimensions));

            // WHERE PART
            query.Append(GenerateSqlWherePart(fTable, joinedDimensions));

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
            TableMetadata fTable, out HashSet<Dimension> joinedDimensions)
        {
            joinedDimensions = new HashSet<Dimension>();
            var query = new StringBuilder();
            var selectList = new List<string>();
            foreach (var m in measurements)
            {
                selectList.Add(
                    $"{m.AggrFunMetadata.Name.Value}({fTable.SqlName.Value}.{m.AttributeMetadata.SqlName.Value}) AS '{m.AttributeAggrFunName.Value}'");
            }

            foreach (var d in dimensions)
            {
                selectList.Add(
                    $"{d.DimTableSqlName}.{d.TableAttributeMetadata.SqlName.Value} AS '{d.TableAttributeMetadata.Name.Value}'");
                joinedDimensions.Add(d);
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
