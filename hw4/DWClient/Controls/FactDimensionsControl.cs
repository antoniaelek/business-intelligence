using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DWClient.Models;

namespace DWClient.Controls
{
    public partial class FactDimensionsControl : UserControl
    {
        private readonly DWMetadataFramework framework;
        private IEnumerable<TableMetadata> fTables;
        public SqlControl SqlControl { get; set; }

        public FactDimensionsControl(DWMetadataFramework framework)
        {
            this.framework = framework;
            InitializeComponent();
            Fill();
        }

        private void Fill()
        {
            try
            {
                fTables = framework.GetFactTables();
                foreach (var fTable in fTables)
                {
                    fTablesComboBox.Items.Add(new ComboBoxItem(fTable.Name.Value, fTable));
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($@"{e.Message}", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            RefreshPanels();
        }

        private void comboBoxFactTables_SelectionChangeCommitted(object sender, EventArgs e)
        {
            RefreshPanels();
        }

        private void dimensionsTreeView_AfterCheck(object sender, TreeViewEventArgs e)
        {
            CheckTreeViewNode(e.Node, e.Node.Checked);
        }

        private void measuresCheckedListBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            var checkedItems = new List<object>();
            foreach (var item in measuresCheckedListBox.CheckedItems)
                checkedItems.Add(item);

            if (e.NewValue == CheckState.Checked)
                checkedItems.Add(measuresCheckedListBox.Items[e.Index]);
            else if (e.NewValue == CheckState.Unchecked)
                checkedItems.Remove(measuresCheckedListBox.Items[e.Index]);

            var query = GetSqlQuery(checkedItems);
            SqlControl.SetQuery(query);
        }

        private void RefreshPanels()
        {
            var selectedObject = (fTablesComboBox.SelectedItem as ComboBoxItem)?.Value as TableMetadata;
            if (selectedObject == null) return;

            SqlControl.SetQuery(string.Empty);
            RefreshMeasurements(selectedObject);
            RefreshDimensions(selectedObject);
        }

        private void RefreshDimensions(TableMetadata selectedObject)
        {
            var dimensions = framework.GetDimensions(selectedObject);
            var grouppedDimensions = dimensions.GroupBy(d => d.DimTableName);
            dimensionsTreeView.Nodes.Clear();
            dimensionsTreeView.CheckBoxes = true;
            foreach (var dimension in grouppedDimensions)
            {
                var nodes = new TreeNode[dimension.Count()];
                int i = 0;
                foreach (var dimAttr in dimension)
                {
                    nodes[i++] = new DimensionTreeNode(dimAttr, dimAttr.TableAttributeMetadata.Name.Value);
                }
                dimensionsTreeView.Nodes.Add(new TreeNode(dimension.Key, nodes));
            }
        }

        private void RefreshMeasurements(TableMetadata selectedObject)
        {
            var measurements = framework.GetMeasurements(selectedObject);
            measuresCheckedListBox.Items.Clear();
            foreach (var measurement in measurements)
            {
                measuresCheckedListBox.Items.Add(new ListBoxItem(measurement.AttributeAggrFunName.Value, measurement));
            }
        }

        private void CheckTreeViewNode(TreeNode node, bool isChecked)
        {
            foreach (TreeNode item in node.Nodes)
            {
                item.Checked = isChecked;

                if (item.Nodes.Count > 0)
                {
                    CheckTreeViewNode(item, isChecked);
                }
            }

            SqlControl.SetQuery(GetSqlQuery());
        }

        public IEnumerable<Dimension> GetDimensions()
        {
            var dimensions = new List<Dimension>();
            foreach (TreeNode nodeDimension in dimensionsTreeView.Nodes)
            {
                foreach (DimensionTreeNode node in nodeDimension.Nodes)
                {
                    if (!node.Checked) continue;
                    dimensions.Add(node.Dimension);
                }
            }
            return dimensions;
        }

        public IEnumerable<Dimension> GetDimensions(List<object> checkedDimensions)
        {
            return checkedDimensions.Select(d => d as Dimension);
        }

        public IEnumerable<Measurement> GetMeasurements()
        {
            var measurements = new List<Measurement>(measuresCheckedListBox.CheckedItems.Count);
            foreach (var checkedItem in measuresCheckedListBox.CheckedItems)
            {
                measurements.Add((checkedItem as ListBoxItem)?.Value as Measurement);
            }
            return measurements;
        }

        public IEnumerable<Measurement> GetMeasurements(List<object> checkedMeasurements)
        {
            var measurements = new List<Measurement>(measuresCheckedListBox.CheckedItems.Count);
            foreach (var checkedItem in checkedMeasurements)
            {
                measurements.Add((checkedItem as ListBoxItem)?.Value as Measurement);
            }
            return measurements;
        }

        public string GetSqlQuery(List<object> checkedMeasurements = null, List<object> checkedDimensions = null)
        {
            var fTable = (fTablesComboBox.SelectedItem as ComboBoxItem)?.Value as TableMetadata;

            // Get data
            var measurements = checkedMeasurements == null ? GetMeasurements() : GetMeasurements(checkedMeasurements);
            var dimensions = checkedDimensions == null ? GetDimensions() : GetDimensions(checkedDimensions);

            // Display sql query
            return DWMetadataFramework.GenerateSqlQuery(fTable, measurements, dimensions);
        }
    }
}
