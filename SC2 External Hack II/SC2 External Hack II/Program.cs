using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SC2_External_Hack_II
{
    static class Program
    {
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Form1());
            }

            catch
            {
                new Exception();
            }
        
        }
    }
}
