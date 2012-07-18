namespace SC2_External_Hack_II
{
    partial class Form1
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.tmrTick = new System.Windows.Forms.Timer(this.components);
            this.pnlNotification = new SC2_External_Hack_II.DoubleBufferPanel();
            this.pnlArmy = new SC2_External_Hack_II.DoubleBufferPanel();
            this.label2 = new System.Windows.Forms.Label();
            this.pnlAPM = new SC2_External_Hack_II.DoubleBufferPanel();
            this.label3 = new System.Windows.Forms.Label();
            this.pnlProduction = new SC2_External_Hack_II.DoubleBufferPanel();
            this.pnlMaphack = new SC2_External_Hack_II.DoubleBufferPanel();
            this.pnlStates = new SC2_External_Hack_II.DoubleBufferPanel();
            this.pnlIncome = new SC2_External_Hack_II.DoubleBufferPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.pnlWorkers = new SC2_External_Hack_II.DoubleBufferPanel();
            this.pnlRessources = new SC2_External_Hack_II.DoubleBufferPanel();
            this.pnlArmy.SuspendLayout();
            this.pnlAPM.SuspendLayout();
            this.pnlIncome.SuspendLayout();
            this.SuspendLayout();
            // 
            // tmrTick
            // 
            this.tmrTick.Interval = 500;
            this.tmrTick.Tick += new System.EventHandler(this.TmrTickTick);
            // 
            // pnlNotification
            // 
            this.pnlNotification.Location = new System.Drawing.Point(299, 346);
            this.pnlNotification.Name = "pnlNotification";
            this.pnlNotification.Size = new System.Drawing.Size(404, 47);
            this.pnlNotification.TabIndex = 8;
            this.pnlNotification.Paint += new System.Windows.Forms.PaintEventHandler(this.PnlNotificationPaint);
            // 
            // pnlArmy
            // 
            this.pnlArmy.Controls.Add(this.label2);
            this.pnlArmy.Location = new System.Drawing.Point(681, 409);
            this.pnlArmy.Name = "pnlArmy";
            this.pnlArmy.Size = new System.Drawing.Size(583, 178);
            this.pnlArmy.TabIndex = 7;
            this.pnlArmy.Paint += new System.Windows.Forms.PaintEventHandler(this.PnlArmyPaint);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.label2.Font = new System.Drawing.Font("Century Gothic", 20.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Silver;
            this.label2.Location = new System.Drawing.Point(230, 14);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(132, 32);
            this.label2.TabIndex = 1;
            this.label2.Text = "Armysize";
            // 
            // pnlAPM
            // 
            this.pnlAPM.Controls.Add(this.label3);
            this.pnlAPM.Location = new System.Drawing.Point(628, 132);
            this.pnlAPM.Name = "pnlAPM";
            this.pnlAPM.Size = new System.Drawing.Size(288, 198);
            this.pnlAPM.TabIndex = 6;
            this.pnlAPM.Paint += new System.Windows.Forms.PaintEventHandler(this.PnlApmPaint);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.label3.Font = new System.Drawing.Font("Century Gothic", 20.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Silver;
            this.label3.Location = new System.Drawing.Point(104, 28);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(74, 32);
            this.label3.TabIndex = 1;
            this.label3.Text = "APM";
            // 
            // pnlProduction
            // 
            this.pnlProduction.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.pnlProduction.Location = new System.Drawing.Point(379, 12);
            this.pnlProduction.Name = "pnlProduction";
            this.pnlProduction.Size = new System.Drawing.Size(578, 145);
            this.pnlProduction.TabIndex = 7;
            this.pnlProduction.Paint += new System.Windows.Forms.PaintEventHandler(this.PnlProductionPaint);
            // 
            // pnlMaphack
            // 
            this.pnlMaphack.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.pnlMaphack.Location = new System.Drawing.Point(12, 221);
            this.pnlMaphack.Name = "pnlMaphack";
            this.pnlMaphack.Size = new System.Drawing.Size(262, 259);
            this.pnlMaphack.TabIndex = 4;
            this.pnlMaphack.Paint += new System.Windows.Forms.PaintEventHandler(this.PnlMaphackPaint);
            // 
            // pnlStates
            // 
            this.pnlStates.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.pnlStates.Location = new System.Drawing.Point(13, 12);
            this.pnlStates.Name = "pnlStates";
            this.pnlStates.Size = new System.Drawing.Size(360, 280);
            this.pnlStates.TabIndex = 3;
            this.pnlStates.Paint += new System.Windows.Forms.PaintEventHandler(this.PnlStatesPaint);
            // 
            // pnlIncome
            // 
            this.pnlIncome.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.pnlIncome.Controls.Add(this.label1);
            this.pnlIncome.Location = new System.Drawing.Point(73, 515);
            this.pnlIncome.Name = "pnlIncome";
            this.pnlIncome.Size = new System.Drawing.Size(583, 464);
            this.pnlIncome.TabIndex = 2;
            this.pnlIncome.Paint += new System.Windows.Forms.PaintEventHandler(this.PnlIncomePaint);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.label1.Font = new System.Drawing.Font("Century Gothic", 20.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Silver;
            this.label1.Location = new System.Drawing.Point(225, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(115, 32);
            this.label1.TabIndex = 0;
            this.label1.Text = "Income";
            // 
            // pnlWorkers
            // 
            this.pnlWorkers.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.pnlWorkers.Location = new System.Drawing.Point(1298, 169);
            this.pnlWorkers.Name = "pnlWorkers";
            this.pnlWorkers.Size = new System.Drawing.Size(200, 49);
            this.pnlWorkers.TabIndex = 1;
            this.pnlWorkers.Paint += new System.Windows.Forms.PaintEventHandler(this.PnlWorkersPaint);
            // 
            // pnlRessources
            // 
            this.pnlRessources.Location = new System.Drawing.Point(709, 3);
            this.pnlRessources.Name = "pnlRessources";
            this.pnlRessources.Size = new System.Drawing.Size(583, 464);
            this.pnlRessources.TabIndex = 0;
            this.pnlRessources.Paint += new System.Windows.Forms.PaintEventHandler(this.PnlRessourcesPaint);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.ClientSize = new System.Drawing.Size(1540, 848);
            this.Controls.Add(this.pnlNotification);
            this.Controls.Add(this.pnlArmy);
            this.Controls.Add(this.pnlAPM);
            this.Controls.Add(this.pnlProduction);
            this.Controls.Add(this.pnlMaphack);
            this.Controls.Add(this.pnlStates);
            this.Controls.Add(this.pnlIncome);
            this.Controls.Add(this.pnlWorkers);
            this.Controls.Add(this.pnlRessources);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "Form1";
            this.TransparencyKey = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1FormClosing);
            this.Load += new System.EventHandler(this.Form1Load);
            this.pnlArmy.ResumeLayout(false);
            this.pnlArmy.PerformLayout();
            this.pnlAPM.ResumeLayout(false);
            this.pnlAPM.PerformLayout();
            this.pnlIncome.ResumeLayout(false);
            this.pnlIncome.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer tmrTick;      
        private DoubleBufferPanel pnlWorkers;
        private DoubleBufferPanel pnlRessources;
        private DoubleBufferPanel pnlIncome;
        private DoubleBufferPanel pnlStates;
        private DoubleBufferPanel pnlMaphack;
        private DoubleBufferPanel pnlProduction;
        private DoubleBufferPanel pnlAPM;
        private DoubleBufferPanel pnlArmy;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private DoubleBufferPanel pnlNotification;
    }
}

