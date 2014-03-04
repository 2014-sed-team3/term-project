
namespace Smrf.NodeXL.ExcelTemplate
{
    partial class AutomatedGraphImageUserSettingsDialog
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AutomatedGraphImageUserSettingsDialog));
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.usrImageFormat = new Smrf.AppLib.ImageFormatControl();
            this.label1 = new System.Windows.Forms.Label();
            this.usrHeaderFooter = new Smrf.NodeXL.ExcelTemplate.HeaderFooterControl();
            this.btnHeaderFooterFont = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(193, 339);
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
            this.btnCancel.Location = new System.Drawing.Point(279, 339);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // usrImageFormat
            // 
            this.usrImageFormat.ImageFormat = System.Drawing.Imaging.ImageFormat.Png;
            this.usrImageFormat.ImageSizePx = new System.Drawing.Size(400, 200);
            this.usrImageFormat.Location = new System.Drawing.Point(12, 94);
            this.usrImageFormat.Name = "usrImageFormat";
            this.usrImageFormat.Size = new System.Drawing.Size(282, 89);
            this.usrImageFormat.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(350, 81);
            this.label1.TabIndex = 0;
            this.label1.Text = resources.GetString("label1.Text");
            // 
            // usrHeaderFooter
            // 
            this.usrHeaderFooter.FooterText = "";
            this.usrHeaderFooter.HeaderText = "";
            this.usrHeaderFooter.IncludeFooter = false;
            this.usrHeaderFooter.IncludeHeader = false;
            this.usrHeaderFooter.Location = new System.Drawing.Point(12, 193);
            this.usrHeaderFooter.Name = "usrHeaderFooter";
            this.usrHeaderFooter.Size = new System.Drawing.Size(347, 99);
            this.usrHeaderFooter.TabIndex = 2;
            // 
            // btnHeaderFooterFont
            // 
            this.btnHeaderFooterFont.Location = new System.Drawing.Point(12, 302);
            this.btnHeaderFooterFont.Name = "btnHeaderFooterFont";
            this.btnHeaderFooterFont.Size = new System.Drawing.Size(134, 23);
            this.btnHeaderFooterFont.TabIndex = 3;
            this.btnHeaderFooterFont.Text = "Header/Footer F&ont...";
            this.btnHeaderFooterFont.UseVisualStyleBackColor = true;
            this.btnHeaderFooterFont.Click += new System.EventHandler(this.btnHeaderFooterFont_Click);
            // 
            // AutomatedGraphImageUserSettingsDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(371, 378);
            this.Controls.Add(this.btnHeaderFooterFont);
            this.Controls.Add(this.usrHeaderFooter);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.usrImageFormat);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AutomatedGraphImageUserSettingsDialog";
            this.Text = "Image Options";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ToolTip toolTip1;
        private Smrf.AppLib.ImageFormatControl usrImageFormat;
        private System.Windows.Forms.Label label1;
        private HeaderFooterControl usrHeaderFooter;
        private System.Windows.Forms.Button btnHeaderFooterFont;
    }
}
