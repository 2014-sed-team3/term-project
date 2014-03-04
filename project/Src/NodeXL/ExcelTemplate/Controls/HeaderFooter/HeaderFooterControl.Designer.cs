
namespace Smrf.NodeXL.ExcelTemplate
{
    partial class HeaderFooterControl
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
            this.chkIncludeHeader = new System.Windows.Forms.CheckBox();
            this.txbHeaderText = new System.Windows.Forms.TextBox();
            this.chkIncludeFooter = new System.Windows.Forms.CheckBox();
            this.txbFooterText = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // chkIncludeHeader
            // 
            this.chkIncludeHeader.AutoSize = true;
            this.chkIncludeHeader.Location = new System.Drawing.Point(0, 0);
            this.chkIncludeHeader.Name = "chkIncludeHeader";
            this.chkIncludeHeader.Size = new System.Drawing.Size(109, 17);
            this.chkIncludeHeader.TabIndex = 0;
            this.chkIncludeHeader.Text = "&Include a header:";
            this.chkIncludeHeader.UseVisualStyleBackColor = true;
            this.chkIncludeHeader.CheckedChanged += new System.EventHandler(this.OnEventThatRequiresControlEnabling);
            // 
            // txbHeaderText
            // 
            this.txbHeaderText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txbHeaderText.Location = new System.Drawing.Point(0, 23);
            this.txbHeaderText.MaxLength = 2000;
            this.txbHeaderText.Name = "txbHeaderText";
            this.txbHeaderText.Size = new System.Drawing.Size(149, 20);
            this.txbHeaderText.TabIndex = 1;
            // 
            // chkIncludeFooter
            // 
            this.chkIncludeFooter.AutoSize = true;
            this.chkIncludeFooter.Location = new System.Drawing.Point(0, 53);
            this.chkIncludeFooter.Name = "chkIncludeFooter";
            this.chkIncludeFooter.Size = new System.Drawing.Size(103, 17);
            this.chkIncludeFooter.TabIndex = 2;
            this.chkIncludeFooter.Text = "Include a &footer:";
            this.chkIncludeFooter.UseVisualStyleBackColor = true;
            this.chkIncludeFooter.CheckedChanged += new System.EventHandler(this.OnEventThatRequiresControlEnabling);
            // 
            // txbFooterText
            // 
            this.txbFooterText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txbFooterText.Location = new System.Drawing.Point(0, 76);
            this.txbFooterText.MaxLength = 2000;
            this.txbFooterText.Name = "txbFooterText";
            this.txbFooterText.Size = new System.Drawing.Size(149, 20);
            this.txbFooterText.TabIndex = 3;
            // 
            // HeaderFooterControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.txbFooterText);
            this.Controls.Add(this.chkIncludeFooter);
            this.Controls.Add(this.txbHeaderText);
            this.Controls.Add(this.chkIncludeHeader);
            this.Name = "HeaderFooterControl";
            this.Size = new System.Drawing.Size(149, 96);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox chkIncludeHeader;
        private System.Windows.Forms.TextBox txbHeaderText;
        private System.Windows.Forms.CheckBox chkIncludeFooter;
        private System.Windows.Forms.TextBox txbFooterText;
    }
}
