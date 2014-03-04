

namespace Smrf.NodeXL.ExcelTemplate
{
    partial class TopNByMetricUserSettingsDialog
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
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.cbxWorksheetName = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cbxItemNameColumnName = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cbxRankedColumnName = new Smrf.NodeXL.ExcelTemplate.VertexColumnComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.nudN = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.nudN)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(269, 152);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 23);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(183, 152);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(80, 23);
            this.btnOK.TabIndex = 8;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "&Worksheet:";
            // 
            // cbxWorksheetName
            // 
            this.cbxWorksheetName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxWorksheetName.Enabled = false;
            this.cbxWorksheetName.FormattingEnabled = true;
            this.cbxWorksheetName.Location = new System.Drawing.Point(197, 10);
            this.cbxWorksheetName.Name = "cbxWorksheetName";
            this.cbxWorksheetName.Size = new System.Drawing.Size(121, 21);
            this.cbxWorksheetName.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 45);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(171, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "&Column containing the item names:";
            // 
            // cbxItemNameColumnName
            // 
            this.cbxItemNameColumnName.Enabled = false;
            this.cbxItemNameColumnName.FormattingEnabled = true;
            this.cbxItemNameColumnName.Location = new System.Drawing.Point(197, 42);
            this.cbxItemNameColumnName.Name = "cbxItemNameColumnName";
            this.cbxItemNameColumnName.Size = new System.Drawing.Size(152, 21);
            this.cbxItemNameColumnName.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(12, 72);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(171, 30);
            this.label3.TabIndex = 4;
            this.label3.Text = "C&olumn containing the numbers to rank the items by:";
            // 
            // cbxRankedColumnName
            // 
            this.cbxRankedColumnName.FormattingEnabled = true;
            this.cbxRankedColumnName.Location = new System.Drawing.Point(197, 74);
            this.cbxRankedColumnName.MaxLength = 100;
            this.cbxRankedColumnName.Name = "cbxRankedColumnName";
            this.cbxRankedColumnName.Size = new System.Drawing.Size(152, 21);
            this.cbxRankedColumnName.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 108);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(134, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "&Number of top items to get:";
            // 
            // nudN
            // 
            this.nudN.Location = new System.Drawing.Point(197, 106);
            this.nudN.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudN.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudN.Name = "nudN";
            this.nudN.Size = new System.Drawing.Size(56, 20);
            this.nudN.TabIndex = 7;
            this.nudN.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // TopNByMetricUserSettingsDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(363, 187);
            this.Controls.Add(this.nudN);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cbxRankedColumnName);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cbxItemNameColumnName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cbxWorksheetName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TopNByMetricUserSettingsDialog";
            this.Text = "Top Item Metrics";
            ((System.ComponentModel.ISupportInitialize)(this.nudN)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbxWorksheetName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbxItemNameColumnName;
        private System.Windows.Forms.Label label3;
        private VertexColumnComboBox cbxRankedColumnName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown nudN;
    }
}
