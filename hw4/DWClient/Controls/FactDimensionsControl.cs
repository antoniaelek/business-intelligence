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

        public FactDimensionsControl(DWMetadataFramework framework)
        {
            this.framework = framework;
            InitializeComponent();
            Fill();
        }

        private void Fill()
        {
            fTables = framework.GetFactTables();
            foreach (var fTable in fTables)
            {
                fTablesComboBox.Items.Add(new ComboBoxItem(fTable.Name.Value, fTable));
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

        private void RefreshPanels()
        {
            var selectedObject = (fTablesComboBox.SelectedItem as ComboBoxItem)?.Value as TableMetadata;
            if (selectedObject == null) return;

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
        }
    }
}
