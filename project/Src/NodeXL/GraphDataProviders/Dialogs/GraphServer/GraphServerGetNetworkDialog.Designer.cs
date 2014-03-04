

namespace Smrf.NodeXL.GraphDataProviders.GraphServer
{
    partial class GraphServerGetNetworkDialog
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
            this.pnlUserInputs = new System.Windows.Forms.Panel();
            this.chkExpandStatusUrls = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txbGraphServerPassword = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txbGraphServerUserName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.dtpMaximumStatusDateUtc = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.dtpMinimumStatusDateUtc = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.txbSearchTerm = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.slStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.pnlUserInputs.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(208, 293);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(80, 23);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(294, 294);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // pnlUserInputs
            // 
            this.pnlUserInputs.Controls.Add(this.chkExpandStatusUrls);
            this.pnlUserInputs.Controls.Add(this.groupBox1);
            this.pnlUserInputs.Controls.Add(this.dtpMaximumStatusDateUtc);
            this.pnlUserInputs.Controls.Add(this.label3);
            this.pnlUserInputs.Controls.Add(this.dtpMinimumStatusDateUtc);
            this.pnlUserInputs.Controls.Add(this.label2);
            this.pnlUserInputs.Controls.Add(this.txbSearchTerm);
            this.pnlUserInputs.Controls.Add(this.label1);
            this.pnlUserInputs.Location = new System.Drawing.Point(12, 12);
            this.pnlUserInputs.Name = "pnlUserInputs";
            this.pnlUserInputs.Size = new System.Drawing.Size(365, 275);
            this.pnlUserInputs.TabIndex = 0;
            // 
            // chkExpandStatusUrls
            // 
            this.chkExpandStatusUrls.AutoSize = true;
            this.chkExpandStatusUrls.Location = new System.Drawing.Point(0, 113);
            this.chkExpandStatusUrls.Name = "chkExpandStatusUrls";
            this.chkExpandStatusUrls.Size = new System.Drawing.Size(176, 17);
            this.chkExpandStatusUrls.TabIndex = 6;
            this.chkExpandStatusUrls.Text = "&Expand URLs in tweets (slower)";
            this.chkExpandStatusUrls.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txbGraphServerPassword);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.txbGraphServerUserName);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Location = new System.Drawing.Point(0, 149);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(362, 121);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Your NodeXL Graph Server account";
            // 
            // txbGraphServerPassword
            // 
            this.txbGraphServerPassword.Location = new System.Drawing.Point(13, 85);
            this.txbGraphServerPassword.MaxLength = 100;
            this.txbGraphServerPassword.Name = "txbGraphServerPassword";
            this.txbGraphServerPassword.PasswordChar = '*';
            this.txbGraphServerPassword.Size = new System.Drawing.Size(205, 20);
            this.txbGraphServerPassword.TabIndex = 3;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(10, 66);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(56, 13);
            this.label5.TabIndex = 2;
            this.label5.Text = "&Password:";
            // 
            // txbGraphServerUserName
            // 
            this.txbGraphServerUserName.Location = new System.Drawing.Point(13, 39);
            this.txbGraphServerUserName.MaxLength = 100;
            this.txbGraphServerUserName.Name = "txbGraphServerUserName";
            this.txbGraphServerUserName.Size = new System.Drawing.Size(205, 20);
            this.txbGraphServerUserName.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 20);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(61, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "&User name:";
            // 
            // dtpMaximumStatusDateUtc
            // 
            this.dtpMaximumStatusDateUtc.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpMaximumStatusDateUtc.Location = new System.Drawing.Point(163, 80);
            this.dtpMaximumStatusDateUtc.Name = "dtpMaximumStatusDateUtc";
            this.dtpMaximumStatusDateUtc.Size = new System.Drawing.Size(108, 20);
            this.dtpMaximumStatusDateUtc.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(114, 83);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(43, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "through";
            // 
            // dtpMinimumStatusDateUtc
            // 
            this.dtpMinimumStatusDateUtc.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpMinimumStatusDateUtc.Location = new System.Drawing.Point(0, 80);
            this.dtpMinimumStatusDateUtc.Name = "dtpMinimumStatusDateUtc";
            this.dtpMinimumStatusDateUtc.Size = new System.Drawing.Size(108, 20);
            this.dtpMinimumStatusDateUtc.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(0, 61);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(125, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Tweet &date range (UTC):";
            // 
            // txbSearchTerm
            // 
            this.txbSearchTerm.Location = new System.Drawing.Point(0, 32);
            this.txbSearchTerm.MaxLength = 200;
            this.txbSearchTerm.Name = "txbSearchTerm";
            this.txbSearchTerm.Size = new System.Drawing.Size(271, 20);
            this.txbSearchTerm.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(362, 26);
            this.label1.TabIndex = 0;
            this.label1.Text = "Add a verte&x for each person who tweeted this search term, or who was replied to" +
    " or mentioned in those tweets:";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.slStatusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 327);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(389, 22);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 3;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // slStatusLabel
            // 
            this.slStatusLabel.Name = "slStatusLabel";
            this.slStatusLabel.Size = new System.Drawing.Size(374, 17);
            this.slStatusLabel.Spring = true;
            this.slStatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // GraphServerGetNetworkDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(389, 349);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.pnlUserInputs);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GraphServerGetNetworkDialog";
            this.Text = "[Gets set in code]";
            this.pnlUserInputs.ResumeLayout(false);
            this.pnlUserInputs.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Panel pnlUserInputs;
        private System.Windows.Forms.TextBox txbSearchTerm;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel slStatusLabel;
        private System.Windows.Forms.DateTimePicker dtpMinimumStatusDateUtc;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dtpMaximumStatusDateUtc;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txbGraphServerPassword;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txbGraphServerUserName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox chkExpandStatusUrls;
    }
}
