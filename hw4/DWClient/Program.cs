using System;
using System.Configuration;
using System.Windows.Forms;

namespace DWClient
{
    static class Program
    {
        internal static DWMetadataFramework Framework;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Framework = new DWMetadataFramework(ConfigurationManager.ConnectionStrings["db"].ToString());

            // Main form
            var form = new Form1 {WindowState = FormWindowState.Normal};

            // Create form controls
            var factsDimensionsControl = new FactDimensionsControl(form, Framework)
                {
                    Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top
                };

            var sqlControl = new SqlControl()
            {
                Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Bottom
            };

            // Add controls to main form
            form.splitContainer1.Panel1.Controls.Add(factsDimensionsControl);
            form.splitContainer1.Panel2.Controls.Add(sqlControl);
            Application.Run(form);
        }
    }
}
