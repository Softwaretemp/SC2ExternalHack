namespace SC2_External_Hack_II
{
    partial class Configure
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Configure));
            this.pgGrid = new System.Windows.Forms.PropertyGrid();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnAdjust = new System.Windows.Forms.Button();
            this.cmbPanel = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // pgGrid
            // 
            this.pgGrid.Location = new System.Drawing.Point(12, 12);
            this.pgGrid.Name = "pgGrid";
            this.pgGrid.Size = new System.Drawing.Size(293, 650);
            this.pgGrid.TabIndex = 0;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(12, 668);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(293, 34);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.BtnSaveClick);
            // 
            // btnAdjust
            // 
            this.btnAdjust.Location = new System.Drawing.Point(97, 12);
            this.btnAdjust.Name = "btnAdjust";
            this.btnAdjust.Size = new System.Drawing.Size(125, 23);
            this.btnAdjust.TabIndex = 2;
            this.btnAdjust.Text = "Adjust Panel Position:";
            this.btnAdjust.UseVisualStyleBackColor = true;
            this.btnAdjust.Click += new System.EventHandler(this.BtnAdjustClick);
            // 
            // cmbPanel
            // 
            this.cmbPanel.FormattingEnabled = true;
            this.cmbPanel.Items.AddRange(new object[] {
            "Ressources",
            "Income",
            "Workers",
            "States",
            "Maphack",
            "Units",
            "APM",
            "Army"});
            this.cmbPanel.Location = new System.Drawing.Point(228, 12);
            this.cmbPanel.Name = "cmbPanel";
            this.cmbPanel.Size = new System.Drawing.Size(77, 21);
            this.cmbPanel.TabIndex = 3;
            // 
            // Configure
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(363, 718);
            this.Controls.Add(this.cmbPanel);
            this.Controls.Add(this.btnAdjust);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.pgGrid);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Configure";
            this.Text = "Configure";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ConfigureFormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ConfigureFormClosed);
            this.Load += new System.EventHandler(this.ConfigureLoad);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PropertyGrid pgGrid;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnAdjust;
        private System.Windows.Forms.ComboBox cmbPanel;
    }
}