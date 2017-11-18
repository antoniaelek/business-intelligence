using System;
using System.Configuration;
using System.Windows.Forms;

namespace DWClient
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var framework = new DWMetadataFramework(ConfigurationManager.ConnectionStrings["db"].ToString());
            
            // Create form controls
            var factTablesMenu = FillFactTablesMenu(framework);
            
            // Main form
            var form = new Form1 {WindowState = FormWindowState.Normal};

            // Add controls to main form
            form.Controls.Add(factTablesMenu);
            Application.Run(form);
        }

        private static FactTablesMenu FillFactTablesMenu(DWMetadataFramework framework)
        {
            var factTables = framework.GetFactTables();
            var factTablesMenu =
                new FactTablesMenu
                {
                    Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top
                };

            foreach (var fTable in factTables)
            {
                factTablesMenu.comboBoxFactTables.Items.Add(new ComboBoxItem(fTable.Name.Value, fTable.SqlName.Value));
            }
            return factTablesMenu;
        }
    }
}
