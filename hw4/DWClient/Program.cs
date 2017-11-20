using System;
using System.Configuration;
using System.Windows.Forms;
using DWClient.Controls;

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
            var factsDimensionsControl = new FactDimensionsControl(Framework)
                {
                    Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Bottom
            };

            var resultsControl = new ResultsControl()
            {
                Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Bottom,
                Dock = DockStyle.Fill
            };

            var sqlControl = new SqlControl(factsDimensionsControl, resultsControl, Framework)
            {
                Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Bottom
            };



            // Add controls to main form
            form.splitContainerInner.Panel1.Controls.Add(factsDimensionsControl);
            form.splitContainerInner.Panel2.Controls.Add(sqlControl);
            form.splitContainerOuter.Panel2.Controls.Add(resultsControl);
            Application.Run(form);
        }
    }
}
