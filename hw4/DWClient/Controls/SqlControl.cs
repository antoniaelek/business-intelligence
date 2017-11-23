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
            try
            {
                // Clear displays
                ClearResultDisplays();

                // Display sql query
                var query = factDimControl.GetSqlQuery();
                SetQuery(query);

                // Execute sql query
                var result = framework.ExecuteQuery(query);

                // Display results
                DisplayResults(query, result);
            }
            catch (Exception exc)
            {
                MessageBox.Show($@"{exc.Message}", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }

        public void SetQuery(string query)
        {
            textBox1.Text = query ?? string.Empty;
            button1.Enabled = !string.IsNullOrWhiteSpace(query);
        }

        private void ClearResultDisplays()
        {
            textBox1.Clear();
            resultsControl.dataGridView1.Columns.Clear();
            resultsControl.dataGridView1.Rows.Clear();
        }

        private void DisplayResults(string query, IEnumerable<TypedDatabaseResult> result)
        {
            resultsControl.dataGridView1.Columns.Clear();
            resultsControl.dataGridView1.Rows.Clear();

            var columns = query.GetColumns().Select(c => new { colName = c, colAlias = DWMetadataFramework.GetAlias(c) }).ToList();
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
    }
}
