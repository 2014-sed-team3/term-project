
namespace Smrf.NodeXL.ExcelTemplate
{
    partial class MergeDuplicateEdgesUserSettingsDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MergeDuplicateEdgesUserSettingsDialog));
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.chkCountDuplicates = new System.Windows.Forms.CheckBox();
            this.chkDeleteDuplicates = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cbxThirdColumnNameForDuplicateDetection = new Smrf.NodeXL.ExcelTemplate.EdgeColumnComboBox();
            this.radVerticesAndColumn = new System.Windows.Forms.RadioButton();
            this.radVerticesOnly = new System.Windows.Forms.RadioButton();
            this.lnkHelp = new Smrf.AppLib.HelpLinkLabel();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(232, 178);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(80, 23);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(318, 178);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // chkCountDuplicates
            // 
            this.chkCountDuplicates.AutoSize = true;
            this.chkCountDuplicates.Location = new System.Drawing.Point(12, 12);
            this.chkCountDuplicates.Name = "chkCountDuplicates";
            this.chkCountDuplicates.Size = new System.Drawing.Size(371, 17);
            this.chkCountDuplicates.TabIndex = 0;
            this.chkCountDuplicates.Text = "&Count duplicate edges and insert the counts into an Edge Weight column";
            this.chkCountDuplicates.UseVisualStyleBackColor = true;
            // 
            // chkDeleteDuplicates
            // 
            this.chkDeleteDuplicates.AutoSize = true;
            this.chkDeleteDuplicates.Location = new System.Drawing.Point(12, 35);
            this.chkDeleteDuplicates.Name = "chkDeleteDuplicates";
            this.chkDeleteDuplicates.Size = new System.Drawing.Size(134, 17);
            this.chkDeleteDuplicates.TabIndex = 1;
            this.chkDeleteDuplicates.Text = "&Merge duplicate edges";
            this.chkDeleteDuplicates.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cbxThirdColumnNameForDuplicateDetection);
            this.groupBox1.Controls.Add(this.radVerticesAndColumn);
            this.groupBox1.Controls.Add(this.radVerticesOnly);
            this.groupBox1.Location = new System.Drawing.Point(12, 66);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(386, 102);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Columns that determine whether edges are duplicates";
            // 
            // cbxThirdColumnNameForDuplicateDetection
            // 
            this.cbxThirdColumnNameForDuplicateDetection.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cbxThirdColumnNameForDuplicateDetection.FormattingEnabled = true;
            this.cbxThirdColumnNameForDuplicateDetection.Location = new System.Drawing.Point(42, 67);
            this.cbxThirdColumnNameForDuplicateDetection.MaxLength = 100;
            this.cbxThirdColumnNameForDuplicateDetection.Name = "cbxThirdColumnNameForDuplicateDetection";
            this.cbxThirdColumnNameForDuplicateDetection.Size = new System.Drawing.Size(134, 21);
            this.cbxThirdColumnNameForDuplicateDetection.TabIndex = 2;
            // 
            // radVerticesAndColumn
            // 
            this.radVerticesAndColumn.AutoSize = true;
            this.radVerticesAndColumn.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
            this.radVerticesAndColumn.Location = new System.Drawing.Point(10, 42);
            this.radVerticesAndColumn.Name = "radVerticesAndColumn";
            this.radVerticesAndColumn.Size = new System.Drawing.Size(189, 17);
            this.radVerticesAndColumn.TabIndex = 1;
            this.radVerticesAndColumn.TabStop = true;
            this.radVerticesAndColumn.Text = "V&ertex 1, Vertex 2 and this column:";
            this.radVerticesAndColumn.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.radVerticesAndColumn.UseVisualStyleBackColor = true;
            this.radVerticesAndColumn.CheckedChanged += new System.EventHandler(this.OnEventThatRequiresControlEnabling);
            // 
            // radVerticesOnly
            // 
            this.radVerticesOnly.AutoSize = true;
            this.radVerticesOnly.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
            this.radVerticesOnly.Location = new System.Drawing.Point(10, 19);
            this.radVerticesOnly.Name = "radVerticesOnly";
            this.radVerticesOnly.Size = new System.Drawing.Size(127, 17);
            this.radVerticesOnly.TabIndex = 0;
            this.radVerticesOnly.TabStop = true;
            this.radVerticesOnly.Text = "&Vertex 1 and Vertex 2";
            this.radVerticesOnly.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.radVerticesOnly.UseVisualStyleBackColor = true;
            this.radVerticesOnly.CheckedChanged += new System.EventHandler(this.OnEventThatRequiresControlEnabling);
            // 
            // lnkHelp
            // 
            this.lnkHelp.AutoSize = true;
            this.lnkHelp.Location = new System.Drawing.Point(12, 183);
            this.lnkHelp.Name = "lnkHelp";
            this.lnkHelp.Size = new System.Drawing.Size(113, 13);
            this.lnkHelp.TabIndex = 3;
            this.lnkHelp.TabStop = true;
            this.lnkHelp.Tag = resources.GetString("lnkHelp.Tag");
            this.lnkHelp.Text = "About duplicate edges";
            // 
            // MergeDuplicateEdgesUserSettingsDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(413, 215);
            this.Controls.Add(this.lnkHelp);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.chkDeleteDuplicates);
            this.Controls.Add(this.chkCountDuplicates);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MergeDuplicateEdgesUserSettingsDialog";
            this.Text = "Count and Merge Duplicate Edges";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.CheckBox chkCountDuplicates;
        private System.Windows.Forms.CheckBox chkDeleteDuplicates;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radVerticesAndColumn;
        private System.Windows.Forms.RadioButton radVerticesOnly;
        private EdgeColumnComboBox cbxThirdColumnNameForDuplicateDetection;
        private Smrf.AppLib.HelpLinkLabel lnkHelp;
    }
}
