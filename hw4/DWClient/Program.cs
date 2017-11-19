using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Windows.Forms;
using DWClient.Models;

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
            var factTablesMenu = new FactTablesMenu(form, Framework)
                {
                    Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top
                };

            // Add controls to main form
            form.splitContainer1.Panel1.Controls.Add(factTablesMenu);
            Application.Run(form);
        }
    }
}
