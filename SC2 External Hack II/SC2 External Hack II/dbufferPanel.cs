using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace SC2_External_Hack_II
{
    public class DoubleBufferPanel : System.Windows.Forms.Panel
    {
    
        public DoubleBufferPanel()
        {
            // Set the value of the double-buffering style bits to true.
            this.SetStyle(ControlStyles.DoubleBuffer |
            ControlStyles.UserPaint |
            ControlStyles.AllPaintingInWmPaint,
            true);

            this.UpdateStyles();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // DoubleBufferPanel
            // 
            this.AllowDrop = true;
            this.ResumeLayout(false);

        }
    }
}
