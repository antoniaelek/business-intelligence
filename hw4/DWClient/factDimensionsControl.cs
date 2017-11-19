using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            SetMeasurements();
        }

        private void comboBoxFactTables_SelectionChangeCommitted(object sender, EventArgs e)
        {
            SetMeasurements();
        }

        private void SetMeasurements()
        {
            var selectedObject = fTables.FirstOrDefault(f =>
                f.SqlName.Value == (comboBoxFactTables.SelectedItem as ComboBoxItem)?.Value.ToString());

            if (selectedObject == null)
                return;

            var measurements = framework.GetMeasurements(selectedObject);
            checkedListBox1.Items.Clear();
            foreach (var measurement in measurements)
            {
                string val = "";
                checkedListBox1.Items.Add(new ListBoxItem(measurement.AttributeAggrFunName.Value, val));
            }

            var dimensions = framework.GetDimensions(selectedObject);
            checkedListBox2.Items.Clear();
            foreach (var dimension in dimensions)
            {
                string val = "";
                checkedListBox2.Items.Add(new ListBoxItem(dimension.DimTableName + "-" + dimension.TableAttributeMetadata.Name.Value, val));
            }

        }
    }
}
