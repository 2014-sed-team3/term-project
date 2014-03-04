

namespace Smrf.NodeXL.ExcelTemplate
{
    partial class NumericComparisonColumnAutoFillUserSettingsDialog
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
            this.txbSourceNumber = new System.Windows.Forms.TextBox();
            this.cbxComparisonOperator = new Smrf.AppLib.ComboBoxPlus();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.cbxDestination1 = new Smrf.AppLib.ComboBoxPlus();
            this.lblDestination1 = new System.Windows.Forms.Label();
            this.chkDestination2 = new System.Windows.Forms.CheckBox();
            this.cbxDestination2 = new Smrf.AppLib.ComboBoxPlus();
            this.SuspendLayout();
            // 
            // txbSourceNumber
            // 
            this.txbSourceNumber.Location = new System.Drawing.Point(175, 29);
            this.txbSourceNumber.MaxLength = 50;
            this.txbSourceNumber.Name = "txbSourceNumber";
            this.txbSourceNumber.Size = new System.Drawing.Size(72, 20);
            this.txbSourceNumber.TabIndex = 2;
            // 
            // cbxComparisonOperator
            // 
            this.cbxComparisonOperator.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxComparisonOperator.FormattingEnabled = true;
            this.cbxComparisonOperator.Location = new System.Drawing.Point(12, 29);
            this.cbxComparisonOperator.Name = "cbxComparisonOperator";
            this.cbxComparisonOperator.Size = new System.Drawing.Size(157, 21);
            this.cbxComparisonOperator.TabIndex = 1;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(167, 183);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 23);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(81, 183);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(80, 23);
            this.btnOK.TabIndex = 7;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(154, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "&If the source column number is:";
            // 
            // cbxDestination1
            // 
            this.cbxDestination1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxDestination1.FormattingEnabled = true;
            this.cbxDestination1.Location = new System.Drawing.Point(12, 82);
            this.cbxDestination1.Name = "cbxDestination1";
            this.cbxDestination1.Size = new System.Drawing.Size(133, 21);
            this.cbxDestination1.TabIndex = 4;
            // 
            // lblDestination1
            // 
            this.lblDestination1.AutoSize = true;
            this.lblDestination1.Location = new System.Drawing.Point(12, 62);
            this.lblDestination1.Name = "lblDestination1";
            this.lblDestination1.Size = new System.Drawing.Size(99, 13);
            this.lblDestination1.TabIndex = 3;
            this.lblDestination1.Text = "Then &set the {0} to:";
            // 
            // chkDestination2
            // 
            this.chkDestination2.AutoSize = true;
            this.chkDestination2.Location = new System.Drawing.Point(12, 117);
            this.chkDestination2.Name = "chkDestination2";
            this.chkDestination2.Size = new System.Drawing.Size(143, 17);
            this.chkDestination2.TabIndex = 5;
            this.chkDestination2.Text = "&Otherwise, set the {0} to:";
            this.chkDestination2.UseVisualStyleBackColor = true;
            this.chkDestination2.CheckedChanged += new System.EventHandler(this.OnEventThatRequiresControlEnabling);
            // 
            // cbxDestination2
            // 
            this.cbxDestination2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxDestination2.FormattingEnabled = true;
            this.cbxDestination2.Location = new System.Drawing.Point(12, 141);
            this.cbxDestination2.Name = "cbxDestination2";
            this.cbxDestination2.Size = new System.Drawing.Size(133, 21);
            this.cbxDestination2.TabIndex = 6;
            // 
            // NumericComparisonColumnAutoFillUserSettingsDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(263, 225);
            this.Controls.Add(this.cbxDestination2);
            this.Controls.Add(this.chkDestination2);
            this.Controls.Add(this.cbxDestination1);
            this.Controls.Add(this.lblDestination1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.txbSourceNumber);
            this.Controls.Add(this.cbxComparisonOperator);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NumericComparisonColumnAutoFillUserSettingsDialog";
            this.Text = "{0} Options";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txbSourceNumber;
        private Smrf.AppLib.ComboBoxPlus cbxComparisonOperator;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label label1;
        private Smrf.AppLib.ComboBoxPlus cbxDestination1;
        private System.Windows.Forms.Label lblDestination1;
        private System.Windows.Forms.CheckBox chkDestination2;
        private Smrf.AppLib.ComboBoxPlus cbxDestination2;
    }
}
