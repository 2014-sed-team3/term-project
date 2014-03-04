

namespace Smrf.NodeXL.ExcelTemplate
{
    partial class WordMetricUserSettingsDialog
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cbxVertexColumnName = new Smrf.NodeXL.ExcelTemplate.VertexColumnComboBox();
            this.cbxEdgeColumnName = new Smrf.NodeXL.ExcelTemplate.EdgeColumnComboBox();
            this.radOnVertexWorksheet = new System.Windows.Forms.RadioButton();
            this.radOnEdgeWorksheet = new System.Windows.Forms.RadioButton();
            this.chkCountByGroup = new System.Windows.Forms.CheckBox();
            this.chkSkipSingleTerms = new System.Windows.Forms.CheckBox();
            this.txbWordsToSkip = new System.Windows.Forms.TextBox();
            this.lblScreenNamesHelp = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(184, 353);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 23);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(98, 353);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(80, 23);
            this.btnOK.TabIndex = 6;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cbxVertexColumnName);
            this.groupBox1.Controls.Add(this.cbxEdgeColumnName);
            this.groupBox1.Controls.Add(this.radOnVertexWorksheet);
            this.groupBox1.Controls.Add(this.radOnEdgeWorksheet);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(253, 136);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Count words and word pairs in this column";
            // 
            // cbxVertexColumnName
            // 
            this.cbxVertexColumnName.FormattingEnabled = true;
            this.cbxVertexColumnName.Location = new System.Drawing.Point(32, 98);
            this.cbxVertexColumnName.MaxLength = 100;
            this.cbxVertexColumnName.Name = "cbxVertexColumnName";
            this.cbxVertexColumnName.Size = new System.Drawing.Size(204, 21);
            this.cbxVertexColumnName.TabIndex = 3;
            // 
            // cbxEdgeColumnName
            // 
            this.cbxEdgeColumnName.FormattingEnabled = true;
            this.cbxEdgeColumnName.Location = new System.Drawing.Point(32, 46);
            this.cbxEdgeColumnName.MaxLength = 100;
            this.cbxEdgeColumnName.Name = "cbxEdgeColumnName";
            this.cbxEdgeColumnName.Size = new System.Drawing.Size(204, 21);
            this.cbxEdgeColumnName.TabIndex = 1;
            // 
            // radOnVertexWorksheet
            // 
            this.radOnVertexWorksheet.AutoSize = true;
            this.radOnVertexWorksheet.Location = new System.Drawing.Point(14, 75);
            this.radOnVertexWorksheet.Name = "radOnVertexWorksheet";
            this.radOnVertexWorksheet.Size = new System.Drawing.Size(150, 17);
            this.radOnVertexWorksheet.TabIndex = 2;
            this.radOnVertexWorksheet.TabStop = true;
            this.radOnVertexWorksheet.Text = "On the &Vertices worksheet";
            this.radOnVertexWorksheet.UseVisualStyleBackColor = true;
            this.radOnVertexWorksheet.CheckedChanged += new System.EventHandler(this.OnEventThatRequiresControlEnabling);
            // 
            // radOnEdgeWorksheet
            // 
            this.radOnEdgeWorksheet.AutoSize = true;
            this.radOnEdgeWorksheet.Location = new System.Drawing.Point(14, 23);
            this.radOnEdgeWorksheet.Name = "radOnEdgeWorksheet";
            this.radOnEdgeWorksheet.Size = new System.Drawing.Size(142, 17);
            this.radOnEdgeWorksheet.TabIndex = 0;
            this.radOnEdgeWorksheet.TabStop = true;
            this.radOnEdgeWorksheet.Text = "On the &Edges worksheet";
            this.radOnEdgeWorksheet.UseVisualStyleBackColor = true;
            this.radOnEdgeWorksheet.CheckedChanged += new System.EventHandler(this.OnEventThatRequiresControlEnabling);
            // 
            // chkCountByGroup
            // 
            this.chkCountByGroup.AutoSize = true;
            this.chkCountByGroup.Location = new System.Drawing.Point(12, 162);
            this.chkCountByGroup.Name = "chkCountByGroup";
            this.chkCountByGroup.Size = new System.Drawing.Size(98, 17);
            this.chkCountByGroup.TabIndex = 1;
            this.chkCountByGroup.Text = "&Count by group";
            this.chkCountByGroup.UseVisualStyleBackColor = true;
            // 
            // chkSkipSingleTerms
            // 
            this.chkSkipSingleTerms.AutoSize = true;
            this.chkSkipSingleTerms.Location = new System.Drawing.Point(12, 185);
            this.chkSkipSingleTerms.Name = "chkSkipSingleTerms";
            this.chkSkipSingleTerms.Size = new System.Drawing.Size(250, 17);
            this.chkSkipSingleTerms.TabIndex = 2;
            this.chkSkipSingleTerms.Text = "&Skip words and word pairs that occur only once";
            this.chkSkipSingleTerms.UseVisualStyleBackColor = true;
            // 
            // txbWordsToSkip
            // 
            this.txbWordsToSkip.AcceptsReturn = true;
            this.txbWordsToSkip.Location = new System.Drawing.Point(12, 234);
            this.txbWordsToSkip.MaxLength = 8000;
            this.txbWordsToSkip.Multiline = true;
            this.txbWordsToSkip.Name = "txbWordsToSkip";
            this.txbWordsToSkip.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txbWordsToSkip.Size = new System.Drawing.Size(253, 82);
            this.txbWordsToSkip.TabIndex = 4;
            // 
            // lblScreenNamesHelp
            // 
            this.lblScreenNamesHelp.AutoSize = true;
            this.lblScreenNamesHelp.Location = new System.Drawing.Point(12, 321);
            this.lblScreenNamesHelp.Name = "lblScreenNamesHelp";
            this.lblScreenNamesHelp.Size = new System.Drawing.Size(207, 13);
            this.lblScreenNamesHelp.TabIndex = 5;
            this.lblScreenNamesHelp.Text = "(Separate with spaces, commas or returns)";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 214);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Skip these &words:";
            // 
            // WordMetricUserSettingsDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(280, 388);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblScreenNamesHelp);
            this.Controls.Add(this.txbWordsToSkip);
            this.Controls.Add(this.chkSkipSingleTerms);
            this.Controls.Add(this.chkCountByGroup);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "WordMetricUserSettingsDialog";
            this.Text = "Word and Word Pair Metrics";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radOnVertexWorksheet;
        private System.Windows.Forms.RadioButton radOnEdgeWorksheet;
        private EdgeColumnComboBox cbxEdgeColumnName;
        private VertexColumnComboBox cbxVertexColumnName;
        private System.Windows.Forms.CheckBox chkCountByGroup;
        private System.Windows.Forms.CheckBox chkSkipSingleTerms;
        private System.Windows.Forms.TextBox txbWordsToSkip;
        private System.Windows.Forms.Label lblScreenNamesHelp;
        private System.Windows.Forms.Label label1;
    }
}
