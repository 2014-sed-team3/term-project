
namespace Smrf.NodeXL.ExcelTemplate
{
    partial class ExportedFilesDescriptionControl
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
            this.btnInsertGraphSummary = new System.Windows.Forms.Button();
            this.txbDescription = new System.Windows.Forms.TextBox();
            this.lblDescription = new System.Windows.Forms.Label();
            this.txbTitle = new System.Windows.Forms.TextBox();
            this.lblTitle = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnInsertGraphSummary
            // 
            this.btnInsertGraphSummary.Location = new System.Drawing.Point(3, 52);
            this.btnInsertGraphSummary.Name = "btnInsertGraphSummary";
            this.btnInsertGraphSummary.Size = new System.Drawing.Size(145, 23);
            this.btnInsertGraphSummary.TabIndex = 9;
            this.btnInsertGraphSummary.Text = "Insert Graph &Summary ->";
            this.btnInsertGraphSummary.UseVisualStyleBackColor = true;
            this.btnInsertGraphSummary.Click += new System.EventHandler(this.btnInsertGraphSummary_Click);
            // 
            // txbDescription
            // 
            this.txbDescription.AcceptsReturn = true;
            this.txbDescription.Location = new System.Drawing.Point(158, 28);
            this.txbDescription.MaxLength = 80000;
            this.txbDescription.Multiline = true;
            this.txbDescription.Name = "txbDescription";
            this.txbDescription.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txbDescription.Size = new System.Drawing.Size(395, 72);
            this.txbDescription.TabIndex = 8;
            // 
            // lblDescription
            // 
            this.lblDescription.AutoSize = true;
            this.lblDescription.Location = new System.Drawing.Point(0, 28);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(90, 13);
            this.lblDescription.TabIndex = 7;
            this.lblDescription.Text = "[Gets set in code]";
            // 
            // txbTitle
            // 
            this.txbTitle.Location = new System.Drawing.Point(158, 0);
            this.txbTitle.MaxLength = 100;
            this.txbTitle.Name = "txbTitle";
            this.txbTitle.Size = new System.Drawing.Size(395, 20);
            this.txbTitle.TabIndex = 6;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Location = new System.Drawing.Point(0, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(90, 13);
            this.lblTitle.TabIndex = 5;
            this.lblTitle.Text = "[Gets set in code]";
            // 
            // ExportedFilesDescriptionControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnInsertGraphSummary);
            this.Controls.Add(this.txbDescription);
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.txbTitle);
            this.Controls.Add(this.lblTitle);
            this.Name = "ExportedFilesDescriptionControl";
            this.Size = new System.Drawing.Size(553, 100);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnInsertGraphSummary;
        private System.Windows.Forms.TextBox txbDescription;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.TextBox txbTitle;
        private System.Windows.Forms.Label lblTitle;
    }
}
