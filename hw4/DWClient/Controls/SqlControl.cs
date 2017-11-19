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
            var joinedTables = new HashSet<TableMetadata>();
            //joinedTables.Add(fTable);

            // SELECT list
            var selectList = new List<string>();
            foreach (var m in measurements)
            {
                selectList.Add($"{m.AggrFunMetadata.Name.Value}({fTable.SqlName.Value}.{m.AttributeMetadata.SqlName.Value}) AS '{m.AttributeAggrFunName.Value}'");
                //joinedTables.Add(m.TableMetadata);
            }

            var selectPart = string.Join($"{Environment.NewLine}\t,", selectList);
            query.Append($"\t {selectPart}");

            // FROM part
            query.AppendLine($"{Environment.NewLine}FROM\t {fTable.SqlName.Value}");
            var fromPart = string.Join($"{Environment.NewLine}\t,", joinedTables.Select(t => t.SqlName.Value));
            query.Append($"\t {fromPart}");

            return query.ToString();
        }

        private IEnumerable<Dimension> GetDimensions()
        {
            return new List<Dimension>(); // todo
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
