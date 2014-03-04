﻿

namespace Smrf.NodeXL.ExcelTemplate
{
    partial class ColorColumnAutoFillUserSettingsDialog
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
            this.txbSourceNumber2 = new System.Windows.Forms.TextBox();
            this.radUseMaximumSourceNumber = new System.Windows.Forms.RadioButton();
            this.chkIgnoreOutliers = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.radUseSourceNumber2 = new System.Windows.Forms.RadioButton();
            this.lnkOutliersAndLogs = new Smrf.AppLib.HelpLinkLabel();
            this.label3 = new System.Windows.Forms.Label();
            this.lblMaximum = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.usrDestinationColor2 = new Smrf.AppLib.ColorPicker();
            this.radUseMinimumSourceNumber = new System.Windows.Forms.RadioButton();
            this.lblMinimum = new System.Windows.Forms.Label();
            this.pnlColorGradient = new Smrf.AppLib.ColorGradientPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.txbSourceNumber1 = new System.Windows.Forms.TextBox();
            this.radUseSourceNumber1 = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnSwapDestinationColors = new System.Windows.Forms.Button();
            this.usrDestinationColor1 = new Smrf.AppLib.ColorPicker();
            this.chkUseLogs = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cbxSourceColumnContainsNumbers = new Smrf.AppLib.ComboBoxPlus();
            this.pnlSourceColumnContainsNumbers = new System.Windows.Forms.Panel();
            this.lblSourceColumnContainsCategories = new System.Windows.Forms.Label();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.pnlSourceColumnContainsNumbers.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(400, 309);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 23);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(314, 309);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(80, 23);
            this.btnOK.TabIndex = 3;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // txbSourceNumber2
            // 
            this.txbSourceNumber2.Enabled = false;
            this.txbSourceNumber2.Location = new System.Drawing.Point(44, 58);
            this.txbSourceNumber2.MaxLength = 50;
            this.txbSourceNumber2.Name = "txbSourceNumber2";
            this.txbSourceNumber2.Size = new System.Drawing.Size(85, 20);
            this.txbSourceNumber2.TabIndex = 3;
            // 
            // radUseMaximumSourceNumber
            // 
            this.radUseMaximumSourceNumber.AutoSize = true;
            this.radUseMaximumSourceNumber.Checked = true;
            this.radUseMaximumSourceNumber.Location = new System.Drawing.Point(24, 35);
            this.radUseMaximumSourceNumber.Name = "radUseMaximumSourceNumber";
            this.radUseMaximumSourceNumber.Size = new System.Drawing.Size(182, 17);
            this.radUseMaximumSourceNumber.TabIndex = 1;
            this.radUseMaximumSourceNumber.TabStop = true;
            this.radUseMaximumSourceNumber.Text = "The largest number in the column";
            this.radUseMaximumSourceNumber.UseVisualStyleBackColor = true;
            this.radUseMaximumSourceNumber.CheckedChanged += new System.EventHandler(this.OnEventThatRequiresControlEnabling);
            // 
            // chkIgnoreOutliers
            // 
            this.chkIgnoreOutliers.AutoSize = true;
            this.chkIgnoreOutliers.Location = new System.Drawing.Point(0, 225);
            this.chkIgnoreOutliers.Name = "chkIgnoreOutliers";
            this.chkIgnoreOutliers.Size = new System.Drawing.Size(92, 17);
            this.chkIgnoreOutliers.TabIndex = 4;
            this.chkIgnoreOutliers.Text = "&Ignore outliers";
            this.chkIgnoreOutliers.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 98);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(68, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "To this &color:";
            // 
            // radUseSourceNumber2
            // 
            this.radUseSourceNumber2.AutoSize = true;
            this.radUseSourceNumber2.Location = new System.Drawing.Point(24, 61);
            this.radUseSourceNumber2.Name = "radUseSourceNumber2";
            this.radUseSourceNumber2.Size = new System.Drawing.Size(14, 13);
            this.radUseSourceNumber2.TabIndex = 2;
            this.radUseSourceNumber2.UseVisualStyleBackColor = true;
            this.radUseSourceNumber2.CheckedChanged += new System.EventHandler(this.OnEventThatRequiresControlEnabling);
            // 
            // lnkOutliersAndLogs
            // 
            this.lnkOutliersAndLogs.AutoSize = true;
            this.lnkOutliersAndLogs.Location = new System.Drawing.Point(268, 226);
            this.lnkOutliersAndLogs.Name = "lnkOutliersAndLogs";
            this.lnkOutliersAndLogs.Size = new System.Drawing.Size(86, 13);
            this.lnkOutliersAndLogs.TabIndex = 6;
            this.lnkOutliersAndLogs.TabStop = true;
            this.lnkOutliersAndLogs.Text = "What are these?";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(11, 13);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(160, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Ma&p this source column number:";
            // 
            // lblMaximum
            // 
            this.lblMaximum.AutoSize = true;
            this.lblMaximum.Location = new System.Drawing.Point(13, 13);
            this.lblMaximum.Name = "lblMaximum";
            this.lblMaximum.Size = new System.Drawing.Size(160, 13);
            this.lblMaximum.TabIndex = 0;
            this.lblMaximum.Text = "M&ap this source column number:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.usrDestinationColor2);
            this.groupBox2.Controls.Add(this.txbSourceNumber2);
            this.groupBox2.Controls.Add(this.radUseMaximumSourceNumber);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.radUseSourceNumber2);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Location = new System.Drawing.Point(238, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(230, 154);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            // 
            // usrDestinationColor2
            // 
            this.usrDestinationColor2.Color = System.Drawing.Color.White;
            this.usrDestinationColor2.Location = new System.Drawing.Point(25, 114);
            this.usrDestinationColor2.Name = "usrDestinationColor2";
            this.usrDestinationColor2.ShowButton = true;
            this.usrDestinationColor2.Size = new System.Drawing.Size(64, 32);
            this.usrDestinationColor2.TabIndex = 5;
            this.usrDestinationColor2.ColorChanged += new System.EventHandler(this.ColorPicker_ColorChanged);
            // 
            // radUseMinimumSourceNumber
            // 
            this.radUseMinimumSourceNumber.AutoSize = true;
            this.radUseMinimumSourceNumber.Checked = true;
            this.radUseMinimumSourceNumber.Location = new System.Drawing.Point(24, 35);
            this.radUseMinimumSourceNumber.Name = "radUseMinimumSourceNumber";
            this.radUseMinimumSourceNumber.Size = new System.Drawing.Size(188, 17);
            this.radUseMinimumSourceNumber.TabIndex = 1;
            this.radUseMinimumSourceNumber.TabStop = true;
            this.radUseMinimumSourceNumber.Text = "The smallest number in the column";
            this.radUseMinimumSourceNumber.UseVisualStyleBackColor = true;
            this.radUseMinimumSourceNumber.CheckedChanged += new System.EventHandler(this.OnEventThatRequiresControlEnabling);
            // 
            // lblMinimum
            // 
            this.lblMinimum.AutoSize = true;
            this.lblMinimum.Location = new System.Drawing.Point(11, 98);
            this.lblMinimum.Name = "lblMinimum";
            this.lblMinimum.Size = new System.Drawing.Size(68, 13);
            this.lblMinimum.TabIndex = 4;
            this.lblMinimum.Text = "T&o this color:";
            // 
            // pnlColorGradient
            // 
            this.pnlColorGradient.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnlColorGradient.Location = new System.Drawing.Point(0, 187);
            this.pnlColorGradient.MaximumColor = System.Drawing.Color.Black;
            this.pnlColorGradient.MinimumColor = System.Drawing.Color.White;
            this.pnlColorGradient.Name = "pnlColorGradient";
            this.pnlColorGradient.Size = new System.Drawing.Size(468, 27);
            this.pnlColorGradient.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(0, 163);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(356, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "And map the in-between source column numbers to the in-between colors:";
            // 
            // txbSourceNumber1
            // 
            this.txbSourceNumber1.Enabled = false;
            this.txbSourceNumber1.Location = new System.Drawing.Point(44, 58);
            this.txbSourceNumber1.MaxLength = 50;
            this.txbSourceNumber1.Name = "txbSourceNumber1";
            this.txbSourceNumber1.Size = new System.Drawing.Size(86, 20);
            this.txbSourceNumber1.TabIndex = 3;
            // 
            // radUseSourceNumber1
            // 
            this.radUseSourceNumber1.AutoSize = true;
            this.radUseSourceNumber1.Location = new System.Drawing.Point(24, 61);
            this.radUseSourceNumber1.Name = "radUseSourceNumber1";
            this.radUseSourceNumber1.Size = new System.Drawing.Size(14, 13);
            this.radUseSourceNumber1.TabIndex = 2;
            this.radUseSourceNumber1.UseVisualStyleBackColor = true;
            this.radUseSourceNumber1.CheckedChanged += new System.EventHandler(this.OnEventThatRequiresControlEnabling);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnSwapDestinationColors);
            this.groupBox1.Controls.Add(this.usrDestinationColor1);
            this.groupBox1.Controls.Add(this.txbSourceNumber1);
            this.groupBox1.Controls.Add(this.radUseMinimumSourceNumber);
            this.groupBox1.Controls.Add(this.lblMinimum);
            this.groupBox1.Controls.Add(this.radUseSourceNumber1);
            this.groupBox1.Controls.Add(this.lblMaximum);
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(230, 154);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // btnSwapDestinationColors
            // 
            this.btnSwapDestinationColors.Location = new System.Drawing.Point(102, 118);
            this.btnSwapDestinationColors.Name = "btnSwapDestinationColors";
            this.btnSwapDestinationColors.Size = new System.Drawing.Size(116, 23);
            this.btnSwapDestinationColors.TabIndex = 6;
            this.btnSwapDestinationColors.Text = "<- &Swap Colors ->";
            this.btnSwapDestinationColors.UseVisualStyleBackColor = true;
            this.btnSwapDestinationColors.Click += new System.EventHandler(this.btnSwapDestinationColors_Click);
            // 
            // usrDestinationColor1
            // 
            this.usrDestinationColor1.Color = System.Drawing.Color.White;
            this.usrDestinationColor1.Location = new System.Drawing.Point(25, 114);
            this.usrDestinationColor1.Name = "usrDestinationColor1";
            this.usrDestinationColor1.ShowButton = true;
            this.usrDestinationColor1.Size = new System.Drawing.Size(64, 32);
            this.usrDestinationColor1.TabIndex = 5;
            this.usrDestinationColor1.ColorChanged += new System.EventHandler(this.ColorPicker_ColorChanged);
            // 
            // chkUseLogs
            // 
            this.chkUseLogs.AutoSize = true;
            this.chkUseLogs.Location = new System.Drawing.Point(106, 225);
            this.chkUseLogs.Name = "chkUseLogs";
            this.chkUseLogs.Size = new System.Drawing.Size(150, 17);
            this.chkUseLogs.TabIndex = 5;
            this.chkUseLogs.Text = "&Use a logarithmic mapping";
            this.chkUseLogs.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(160, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "The source column\'s &values are:";
            // 
            // cbxSourceColumnContainsNumbers
            // 
            this.cbxSourceColumnContainsNumbers.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxSourceColumnContainsNumbers.FormattingEnabled = true;
            this.cbxSourceColumnContainsNumbers.Location = new System.Drawing.Point(15, 27);
            this.cbxSourceColumnContainsNumbers.Name = "cbxSourceColumnContainsNumbers";
            this.cbxSourceColumnContainsNumbers.Size = new System.Drawing.Size(121, 21);
            this.cbxSourceColumnContainsNumbers.TabIndex = 1;
            this.cbxSourceColumnContainsNumbers.SelectedIndexChanged += new System.EventHandler(this.OnEventThatRequiresControlEnabling);
            // 
            // pnlSourceColumnContainsNumbers
            // 
            this.pnlSourceColumnContainsNumbers.Controls.Add(this.groupBox1);
            this.pnlSourceColumnContainsNumbers.Controls.Add(this.label1);
            this.pnlSourceColumnContainsNumbers.Controls.Add(this.pnlColorGradient);
            this.pnlSourceColumnContainsNumbers.Controls.Add(this.chkUseLogs);
            this.pnlSourceColumnContainsNumbers.Controls.Add(this.groupBox2);
            this.pnlSourceColumnContainsNumbers.Controls.Add(this.chkIgnoreOutliers);
            this.pnlSourceColumnContainsNumbers.Controls.Add(this.lnkOutliersAndLogs);
            this.pnlSourceColumnContainsNumbers.Location = new System.Drawing.Point(12, 54);
            this.pnlSourceColumnContainsNumbers.Name = "pnlSourceColumnContainsNumbers";
            this.pnlSourceColumnContainsNumbers.Size = new System.Drawing.Size(485, 253);
            this.pnlSourceColumnContainsNumbers.TabIndex = 2;
            // 
            // lblSourceColumnContainsCategories
            // 
            this.lblSourceColumnContainsCategories.AutoSize = true;
            this.lblSourceColumnContainsCategories.Location = new System.Drawing.Point(207, 27);
            this.lblSourceColumnContainsCategories.Name = "lblSourceColumnContainsCategories";
            this.lblSourceColumnContainsCategories.Size = new System.Drawing.Size(234, 13);
            this.lblSourceColumnContainsCategories.TabIndex = 5;
            this.lblSourceColumnContainsCategories.Text = "Each category will be mapped to a unique color.";
            // 
            // ColorColumnAutoFillUserSettingsDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(497, 344);
            this.Controls.Add(this.lblSourceColumnContainsCategories);
            this.Controls.Add(this.pnlSourceColumnContainsNumbers);
            this.Controls.Add(this.cbxSourceColumnContainsNumbers);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ColorColumnAutoFillUserSettingsDialog";
            this.Text = "[Gets set in code]";
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.pnlSourceColumnContainsNumbers.ResumeLayout(false);
            this.pnlSourceColumnContainsNumbers.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.TextBox txbSourceNumber2;
        private System.Windows.Forms.RadioButton radUseMaximumSourceNumber;
        private System.Windows.Forms.CheckBox chkIgnoreOutliers;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton radUseSourceNumber2;
        private Smrf.AppLib.HelpLinkLabel lnkOutliersAndLogs;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblMaximum;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton radUseMinimumSourceNumber;
        private System.Windows.Forms.Label lblMinimum;
        private Smrf.AppLib.ColorGradientPanel pnlColorGradient;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txbSourceNumber1;
        private System.Windows.Forms.RadioButton radUseSourceNumber1;
        private System.Windows.Forms.GroupBox groupBox1;
        private Smrf.AppLib.ColorPicker usrDestinationColor2;
        private Smrf.AppLib.ColorPicker usrDestinationColor1;
        private System.Windows.Forms.Button btnSwapDestinationColors;
        private System.Windows.Forms.CheckBox chkUseLogs;
        private System.Windows.Forms.Label label4;
        private Smrf.AppLib.ComboBoxPlus cbxSourceColumnContainsNumbers;
        private System.Windows.Forms.Panel pnlSourceColumnContainsNumbers;
        private System.Windows.Forms.Label lblSourceColumnContainsCategories;
    }
}
