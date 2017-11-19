using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DWClient.Models;

namespace DWClient
{
    public partial class FactDimensionsControl : UserControl
    {
        private Form parentForm;
        private readonly DWMetadataFramework framework;
        private IEnumerable<TableMetadata> fTables;

        public FactDimensionsControl(Form parentForm, DWMetadataFramework framework)
        {
            this.parentForm = parentForm;
            this.framework = framework;
            InitializeComponent();
            Fill();
        }

        private void Fill()
        {
            fTables = framework.GetFactTables();
            foreach (var fTable in fTables)
            {
                comboBoxFactTables.Items.Add(new ComboBoxItem(fTable.Name.Value, fTable.SqlName.Value));
            }
            comboBoxFactTables.SelectedIndex = 0;
            RefreshPanels();
        }

        private void comboBoxFactTables_SelectionChangeCommitted(object sender, EventArgs e)
        {
            RefreshPanels();
        }

        private void RefreshPanels()
        {
            var selectedObject = fTables.FirstOrDefault(f =>
                f.SqlName.Value == (comboBoxFactTables.SelectedItem as ComboBoxItem)?.Value.ToString());

            if (selectedObject == null)
                return;

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
                    nodes[i++] = new TreeNode(dimAttr.TableAttributeMetadata.Name.Value);
                }
                dimensionsTreeView.Nodes.Add(new TreeNode(dimension.Key, nodes));
            }
        }

        private void RefreshMeasurements(TableMetadata selectedObject)
        {
            var measurements = framework.GetMeasurements(selectedObject);
            checkedListBox1.Items.Clear();
            foreach (var measurement in measurements)
            {
                string val = "";
                checkedListBox1.Items.Add(new ListBoxItem(measurement.AttributeAggrFunName.Value, val));
            }
        }

        private void dimensionsTreeView_AfterCheck(object sender, TreeViewEventArgs e)
        {
            CheckTreeViewNode(e.Node, e.Node.Checked);
        }

        private void CheckTreeViewNode(TreeNode node, bool isChecked)
        {
            foreach (TreeNode item in node.Nodes)
            {
                item.Checked = isChecked;

                if (item.Nodes.Count > 0)
                {
                    this.CheckTreeViewNode(item, isChecked);
                }
            }
        }
    }
}
