

namespace Smrf.NodeXL.ExcelTemplate
{
    partial class EdgeAttributesDialog
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
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.cbxStyle = new Smrf.AppLib.ComboBoxPlus();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.nudWidth = new System.Windows.Forms.NumericUpDown();
            this.nudAlpha = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.lblAlpha = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cbxVisibility = new Smrf.AppLib.ComboBoxPlus();
            this.lnkVisibility = new Smrf.AppLib.HelpLinkLabel();
            this.usrColor = new Smrf.AppLib.ColorPicker();
            this.label8 = new System.Windows.Forms.Label();
            this.txbLabel = new System.Windows.Forms.TextBox();
            this.usrLabelTextColor = new Smrf.AppLib.ColorPicker();
            this.nudLabelFontSize = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.nudWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudAlpha)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudLabelFontSize)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(179, 289);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 18;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(260, 289);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 19;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // cbxStyle
            // 
            this.cbxStyle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxStyle.FormattingEnabled = true;
            this.cbxStyle.Location = new System.Drawing.Point(98, 83);
            this.cbxStyle.Name = "cbxStyle";
            this.cbxStyle.Size = new System.Drawing.Size(118, 21);
            this.cbxStyle.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 85);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(33, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "&Style:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(34, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "&Color:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 52);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(38, 13);
            this.label7.TabIndex = 2;
            this.label7.Text = "&Width:";
            // 
            // nudWidth
            // 
            this.nudWidth.DecimalPlaces = 1;
            this.nudWidth.Location = new System.Drawing.Point(98, 49);
            this.nudWidth.Name = "nudWidth";
            this.nudWidth.Size = new System.Drawing.Size(56, 20);
            this.nudWidth.TabIndex = 3;
            // 
            // nudAlpha
            // 
            this.nudAlpha.DecimalPlaces = 1;
            this.nudAlpha.Location = new System.Drawing.Point(98, 116);
            this.nudAlpha.Name = "nudAlpha";
            this.nudAlpha.Size = new System.Drawing.Size(56, 20);
            this.nudAlpha.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 118);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(46, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "O&pacity:";
            // 
            // lblAlpha
            // 
            this.lblAlpha.AutoSize = true;
            this.lblAlpha.Location = new System.Drawing.Point(157, 118);
            this.lblAlpha.Name = "lblAlpha";
            this.lblAlpha.Size = new System.Drawing.Size(15, 13);
            this.lblAlpha.TabIndex = 8;
            this.lblAlpha.Text = "%";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 217);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(82, 13);
            this.label5.TabIndex = 14;
            this.label5.Text = "Label &text color:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 250);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(78, 13);
            this.label6.TabIndex = 16;
            this.label6.Text = "Label &font size:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 151);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(46, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "&Visibility:";
            // 
            // cbxVisibility
            // 
            this.cbxVisibility.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxVisibility.FormattingEnabled = true;
            this.cbxVisibility.Location = new System.Drawing.Point(98, 148);
            this.cbxVisibility.Name = "cbxVisibility";
            this.cbxVisibility.Size = new System.Drawing.Size(94, 21);
            this.cbxVisibility.TabIndex = 10;
            // 
            // lnkVisibility
            // 
            this.lnkVisibility.AutoSize = true;
            this.lnkVisibility.Location = new System.Drawing.Point(205, 151);
            this.lnkVisibility.Name = "lnkVisibility";
            this.lnkVisibility.Size = new System.Drawing.Size(120, 13);
            this.lnkVisibility.TabIndex = 11;
            this.lnkVisibility.TabStop = true;
            this.lnkVisibility.Text = "Graph may be refreshed";
            // 
            // usrColor
            // 
            this.usrColor.Color = System.Drawing.Color.White;
            this.usrColor.Location = new System.Drawing.Point(96, 9);
            this.usrColor.Name = "usrColor";
            this.usrColor.ShowButton = true;
            this.usrColor.Size = new System.Drawing.Size(64, 32);
            this.usrColor.TabIndex = 1;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(12, 184);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(36, 13);
            this.label8.TabIndex = 12;
            this.label8.Text = "&Label:";
            // 
            // txbLabel
            // 
            this.txbLabel.Location = new System.Drawing.Point(96, 181);
            this.txbLabel.MaxLength = 1000;
            this.txbLabel.Name = "txbLabel";
            this.txbLabel.Size = new System.Drawing.Size(189, 20);
            this.txbLabel.TabIndex = 13;
            // 
            // usrLabelTextColor
            // 
            this.usrLabelTextColor.Color = System.Drawing.Color.White;
            this.usrLabelTextColor.Location = new System.Drawing.Point(96, 208);
            this.usrLabelTextColor.Name = "usrLabelTextColor";
            this.usrLabelTextColor.ShowButton = true;
            this.usrLabelTextColor.Size = new System.Drawing.Size(64, 32);
            this.usrLabelTextColor.TabIndex = 15;
            // 
            // nudLabelFontSize
            // 
            this.nudLabelFontSize.DecimalPlaces = 1;
            this.nudLabelFontSize.Location = new System.Drawing.Point(98, 247);
            this.nudLabelFontSize.Name = "nudLabelFontSize";
            this.nudLabelFontSize.Size = new System.Drawing.Size(56, 20);
            this.nudLabelFontSize.TabIndex = 17;
            // 
            // EdgeAttributesDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(347, 327);
            this.Controls.Add(this.nudLabelFontSize);
            this.Controls.Add(this.usrLabelTextColor);
            this.Controls.Add(this.txbLabel);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.usrColor);
            this.Controls.Add(this.lnkVisibility);
            this.Controls.Add(this.cbxVisibility);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.lblAlpha);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.nudAlpha);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.nudWidth);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbxStyle);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EdgeAttributesDialog";
            this.Text = "Edge Properties";
            ((System.ComponentModel.ISupportInitialize)(this.nudWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudAlpha)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudLabelFontSize)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private Smrf.AppLib.ComboBoxPlus cbxStyle;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown nudWidth;
        private System.Windows.Forms.NumericUpDown nudAlpha;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblAlpha;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label4;
        private Smrf.AppLib.ComboBoxPlus cbxVisibility;
        private Smrf.AppLib.HelpLinkLabel lnkVisibility;
        private Smrf.AppLib.ColorPicker usrColor;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txbLabel;
        private Smrf.AppLib.ColorPicker usrLabelTextColor;
        private System.Windows.Forms.NumericUpDown nudLabelFontSize;
    }
}
