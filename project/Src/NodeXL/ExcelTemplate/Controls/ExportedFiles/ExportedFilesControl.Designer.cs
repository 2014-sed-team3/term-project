
namespace Smrf.NodeXL.ExcelTemplate
{
    partial class ExportedFilesControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.radUseFixedAspectRatio = new System.Windows.Forms.RadioButton();
            this.radUseGraphPaneAspectRatio = new System.Windows.Forms.RadioButton();
            this.chkExportWorkbookAndSettings = new System.Windows.Forms.CheckBox();
            this.chkExportGraphML = new System.Windows.Forms.CheckBox();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.radUseFixedAspectRatio);
            this.groupBox2.Controls.Add(this.radUseGraphPaneAspectRatio);
            this.groupBox2.Location = new System.Drawing.Point(0, 54);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(396, 74);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Appearance";
            // 
            // radUseFixedAspectRatio
            // 
            this.radUseFixedAspectRatio.AutoSize = true;
            this.radUseFixedAspectRatio.Location = new System.Drawing.Point(14, 42);
            this.radUseFixedAspectRatio.Name = "radUseFixedAspectRatio";
            this.radUseFixedAspectRatio.Size = new System.Drawing.Size(363, 17);
            this.radUseFixedAspectRatio.TabIndex = 1;
            this.radUseFixedAspectRatio.TabStop = true;
            this.radUseFixedAspectRatio.Text = "I may &be exporting other images, and I want them all to be the same size";
            this.radUseFixedAspectRatio.UseVisualStyleBackColor = true;
            // 
            // radUseGraphPaneAspectRatio
            // 
            this.radUseGraphPaneAspectRatio.AutoSize = true;
            this.radUseGraphPaneAspectRatio.Location = new System.Drawing.Point(14, 19);
            this.radUseGraphPaneAspectRatio.Name = "radUseGraphPaneAspectRatio";
            this.radUseGraphPaneAspectRatio.Size = new System.Drawing.Size(281, 17);
            this.radUseGraphPaneAspectRatio.TabIndex = 0;
            this.radUseGraphPaneAspectRatio.TabStop = true;
            this.radUseGraphPaneAspectRatio.Text = "Ma&ke the exported image look just like the graph pane";
            this.radUseGraphPaneAspectRatio.UseVisualStyleBackColor = true;
            // 
            // chkExportWorkbookAndSettings
            // 
            this.chkExportWorkbookAndSettings.AutoSize = true;
            this.chkExportWorkbookAndSettings.Location = new System.Drawing.Point(0, 0);
            this.chkExportWorkbookAndSettings.Name = "chkExportWorkbookAndSettings";
            this.chkExportWorkbookAndSettings.Size = new System.Drawing.Size(217, 17);
            this.chkExportWorkbookAndSettings.TabIndex = 0;
            this.chkExportWorkbookAndSettings.Text = "Also export the &workbook and its options";
            this.chkExportWorkbookAndSettings.UseVisualStyleBackColor = true;
            // 
            // chkExportGraphML
            // 
            this.chkExportGraphML.AutoSize = true;
            this.chkExportGraphML.Location = new System.Drawing.Point(0, 23);
            this.chkExportGraphML.Name = "chkExportGraphML";
            this.chkExportGraphML.Size = new System.Drawing.Size(211, 17);
            this.chkExportGraphML.TabIndex = 1;
            this.chkExportGraphML.Text = "Also export the &graph data as GraphML";
            this.chkExportGraphML.UseVisualStyleBackColor = true;
            // 
            // ExportedFilesControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.chkExportWorkbookAndSettings);
            this.Controls.Add(this.chkExportGraphML);
            this.Name = "ExportedFilesControl";
            this.Size = new System.Drawing.Size(396, 133);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton radUseFixedAspectRatio;
        private System.Windows.Forms.RadioButton radUseGraphPaneAspectRatio;
        private System.Windows.Forms.CheckBox chkExportWorkbookAndSettings;
        private System.Windows.Forms.CheckBox chkExportGraphML;
    }
}
