
namespace Smrf.NodeXL.ExcelTemplate
{
    partial class AggregateGraphMetricsDialog
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
            this.pnlEnable = new System.Windows.Forms.Panel();
            this.lblNote = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.usrSourceFolder = new Smrf.AppLib.FolderPathControl();
            this.label3 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.slStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.pnlEnable.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlEnable
            // 
            this.pnlEnable.Controls.Add(this.lblNote);
            this.pnlEnable.Controls.Add(this.label1);
            this.pnlEnable.Controls.Add(this.usrSourceFolder);
            this.pnlEnable.Controls.Add(this.label3);
            this.pnlEnable.Location = new System.Drawing.Point(12, 12);
            this.pnlEnable.Name = "pnlEnable";
            this.pnlEnable.Size = new System.Drawing.Size(383, 109);
            this.pnlEnable.TabIndex = 0;
            // 
            // lblNote
            // 
            this.lblNote.AutoSize = true;
            this.lblNote.Location = new System.Drawing.Point(3, 44);
            this.lblNote.Name = "lblNote";
            this.lblNote.Size = new System.Drawing.Size(138, 13);
            this.lblNote.TabIndex = 2;
            this.lblNote.Text = "(Excludes open workbooks)";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(340, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "&Aggregate the overall metrics for each NodeXL workbook in this folder:";
            // 
            // usrSourceFolder
            // 
            this.usrSourceFolder.BrowsePrompt = "Browse for a folder containing NodeXL workbooks.";
            this.usrSourceFolder.FolderPath = "";
            this.usrSourceFolder.Location = new System.Drawing.Point(3, 20);
            this.usrSourceFolder.Name = "usrSourceFolder";
            this.usrSourceFolder.Size = new System.Drawing.Size(377, 24);
            this.usrSourceFolder.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(0, 71);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(380, 32);
            this.label3.TabIndex = 3;
            this.label3.Text = "The aggregated overall metrics will be inserted into a new Excel workbook.";
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(317, 122);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(236, 122);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "Aggregate";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.slStatusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 155);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(407, 22);
            this.statusStrip1.TabIndex = 3;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // slStatusLabel
            // 
            this.slStatusLabel.Name = "slStatusLabel";
            this.slStatusLabel.Size = new System.Drawing.Size(361, 17);
            this.slStatusLabel.Spring = true;
            this.slStatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // AggregateGraphMetricsDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(407, 177);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.pnlEnable);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AggregateGraphMetricsDialog";
            this.Text = "Aggregate Overall Metrics";
            this.pnlEnable.ResumeLayout(false);
            this.pnlEnable.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlEnable;
        private System.Windows.Forms.Label label1;
        private Smrf.AppLib.FolderPathControl usrSourceFolder;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel slStatusLabel;
        private System.Windows.Forms.Label lblNote;
    }
}
