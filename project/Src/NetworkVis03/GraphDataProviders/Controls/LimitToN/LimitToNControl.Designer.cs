namespace Smrf.NodeXL.GraphDataProviders
{
    partial class LimitToNControl
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
            this.lblObjectName = new System.Windows.Forms.Label();
            this.chkLimitToN = new System.Windows.Forms.CheckBox();
            this.nudN = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.nudN)).BeginInit();
            this.SuspendLayout();
            // 
            // lblObjectName
            // 
            this.lblObjectName.AutoSize = true;
            this.lblObjectName.Location = new System.Drawing.Point(122, 3);
            this.lblObjectName.Name = "lblObjectName";
            this.lblObjectName.Size = new System.Drawing.Size(39, 13);
            this.lblObjectName.TabIndex = 2;
            this.lblObjectName.Text = "people";
            // 
            // chkLimitToN
            // 
            this.chkLimitToN.AutoSize = true;
            this.chkLimitToN.Location = new System.Drawing.Point(0, 2);
            this.chkLimitToN.Name = "chkLimitToN";
            this.chkLimitToN.Size = new System.Drawing.Size(59, 17);
            this.chkLimitToN.TabIndex = 0;
            this.chkLimitToN.Text = "Limi&t to";
            this.chkLimitToN.UseVisualStyleBackColor = true;
            this.chkLimitToN.CheckedChanged += new System.EventHandler(this.chkLimitToN_CheckedChanged);
            // 
            // nudN
            // 
            this.nudN.Enabled = false;
            this.nudN.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.nudN.Location = new System.Drawing.Point(60, 0);
            this.nudN.Name = "nudN";
            this.nudN.Size = new System.Drawing.Size(56, 20);
            this.nudN.TabIndex = 1;
            this.nudN.ThousandsSeparator = true;
            this.nudN.Validating += new System.ComponentModel.CancelEventHandler(this.nudN_Validating);
            // 
            // LimitToNControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.nudN);
            this.Controls.Add(this.lblObjectName);
            this.Controls.Add(this.chkLimitToN);
            this.Name = "LimitToNControl";
            this.Size = new System.Drawing.Size(168, 27);
            ((System.ComponentModel.ISupportInitialize)(this.nudN)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblObjectName;
        private System.Windows.Forms.CheckBox chkLimitToN;
        private System.Windows.Forms.NumericUpDown nudN;
    }
}
