namespace StandaloneNode
{
    partial class GroupSettingDialog
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
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.numericUpDown2 = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.radDConnectorMotif = new System.Windows.Forms.RadioButton();
            this.radFanMotif = new System.Windows.Forms.RadioButton();
            this.radWakitaTsurumi = new System.Windows.Forms.RadioButton();
            this.flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.label1);
            this.flowLayoutPanel1.Controls.Add(this.numericUpDown1);
            this.flowLayoutPanel1.Controls.Add(this.label2);
            this.flowLayoutPanel1.Controls.Add(this.numericUpDown2);
            this.flowLayoutPanel1.Controls.Add(this.label3);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(252, 94);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(4);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(388, 86);
            this.flowLayoutPanel1.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 0);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 15);
            this.label1.TabIndex = 2;
            this.label1.Text = "between";
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(65, 4);
            this.numericUpDown1.Margin = new System.Windows.Forms.Padding(4);
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(59, 25);
            this.numericUpDown1.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(132, 0);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(27, 15);
            this.label2.TabIndex = 4;
            this.label2.Text = "and";
            // 
            // numericUpDown2
            // 
            this.numericUpDown2.Location = new System.Drawing.Point(167, 4);
            this.numericUpDown2.Margin = new System.Windows.Forms.Padding(4);
            this.numericUpDown2.Name = "numericUpDown2";
            this.numericUpDown2.Size = new System.Drawing.Size(52, 25);
            this.numericUpDown2.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(227, 0);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(92, 15);
            this.label3.TabIndex = 5;
            this.label3.Text = "anchor vertices";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.flowLayoutPanel1);
            this.panel1.Controls.Add(this.radDConnectorMotif);
            this.panel1.Controls.Add(this.radFanMotif);
            this.panel1.Controls.Add(this.radWakitaTsurumi);
            this.panel1.Location = new System.Drawing.Point(13, 13);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(653, 195);
            this.panel1.TabIndex = 1;
            // 
            // radDConnectorMotif
            // 
            this.radDConnectorMotif.AutoSize = true;
            this.radDConnectorMotif.Location = new System.Drawing.Point(16, 69);
            this.radDConnectorMotif.Margin = new System.Windows.Forms.Padding(4);
            this.radDConnectorMotif.Name = "radDConnectorMotif";
            this.radDConnectorMotif.Size = new System.Drawing.Size(193, 19);
            this.radDConnectorMotif.TabIndex = 2;
            this.radDConnectorMotif.TabStop = true;
            this.radDConnectorMotif.Text = "Group By DConnector Motif";
            this.radDConnectorMotif.UseVisualStyleBackColor = true;
            // 
            // radFanMotif
            // 
            this.radFanMotif.AutoSize = true;
            this.radFanMotif.Location = new System.Drawing.Point(16, 42);
            this.radFanMotif.Margin = new System.Windows.Forms.Padding(4);
            this.radFanMotif.Name = "radFanMotif";
            this.radFanMotif.Size = new System.Drawing.Size(142, 19);
            this.radFanMotif.TabIndex = 1;
            this.radFanMotif.TabStop = true;
            this.radFanMotif.Text = "Group By FanMotif";
            this.radFanMotif.UseVisualStyleBackColor = true;
            // 
            // radWakitaTsurumi
            // 
            this.radWakitaTsurumi.AutoSize = true;
            this.radWakitaTsurumi.Location = new System.Drawing.Point(16, 15);
            this.radWakitaTsurumi.Margin = new System.Windows.Forms.Padding(4);
            this.radWakitaTsurumi.Name = "radWakitaTsurumi";
            this.radWakitaTsurumi.Size = new System.Drawing.Size(180, 19);
            this.radWakitaTsurumi.TabIndex = 0;
            this.radWakitaTsurumi.TabStop = true;
            this.radWakitaTsurumi.Text = "Group By Wakita Tsurumi";
            this.radWakitaTsurumi.UseVisualStyleBackColor = true;
            // 
            // GroupSettingDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(675, 214);
            this.Controls.Add(this.panel1);
            this.Name = "GroupSettingDialog";
            this.Text = "GroupSettingDialog";
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numericUpDown2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton radDConnectorMotif;
        private System.Windows.Forms.RadioButton radFanMotif;
        private System.Windows.Forms.RadioButton radWakitaTsurumi;
    }
}