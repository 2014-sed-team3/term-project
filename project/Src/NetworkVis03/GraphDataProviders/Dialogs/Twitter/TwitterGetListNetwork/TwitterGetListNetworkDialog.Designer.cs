

namespace Smrf.NodeXL.GraphDataProviders.Twitter
{
    partial class TwitterGetListNetworkDialog
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
            this.txbScreenNames = new System.Windows.Forms.TextBox();
            this.lblScreenNamesHelp = new System.Windows.Forms.Label();
            this.pnlUserInputs = new System.Windows.Forms.Panel();
            this.chkExpandLatestStatusUrls = new System.Windows.Forms.CheckBox();
            this.chkIncludeLatestStatuses = new System.Windows.Forms.CheckBox();
            this.chkIncludeStatistics = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chkIncludeMentionsEdges = new System.Windows.Forms.CheckBox();
            this.chkIncludeRepliesToEdges = new System.Windows.Forms.CheckBox();
            this.chkIncludeFollowedEdges = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radUseScreenNames = new System.Windows.Forms.RadioButton();
            this.radUseListName = new System.Windows.Forms.RadioButton();
            this.txbListName = new System.Windows.Forms.TextBox();
            this.usrTwitterAuthorization = new Smrf.NodeXL.GraphDataProviders.Twitter.TwitterAuthorizationControl();
            this.usrTwitterRateLimits = new Smrf.NodeXL.GraphDataProviders.Twitter.TwitterRateLimitsControl();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.slStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.pnlUserInputs.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txbScreenNames
            // 
            this.txbScreenNames.AcceptsReturn = true;
            this.txbScreenNames.Enabled = false;
            this.txbScreenNames.Location = new System.Drawing.Point(34, 91);
            this.txbScreenNames.Multiline = true;
            this.txbScreenNames.Name = "txbScreenNames";
            this.txbScreenNames.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txbScreenNames.Size = new System.Drawing.Size(300, 38);
            this.txbScreenNames.TabIndex = 3;
            // 
            // lblScreenNamesHelp
            // 
            this.lblScreenNamesHelp.AutoSize = true;
            this.lblScreenNamesHelp.Enabled = false;
            this.lblScreenNamesHelp.Location = new System.Drawing.Point(34, 134);
            this.lblScreenNamesHelp.Name = "lblScreenNamesHelp";
            this.lblScreenNamesHelp.Size = new System.Drawing.Size(207, 13);
            this.lblScreenNamesHelp.TabIndex = 4;
            this.lblScreenNamesHelp.Text = "(Separate with spaces, commas or returns)";
            // 
            // pnlUserInputs
            // 
            this.pnlUserInputs.Controls.Add(this.chkExpandLatestStatusUrls);
            this.pnlUserInputs.Controls.Add(this.chkIncludeLatestStatuses);
            this.pnlUserInputs.Controls.Add(this.chkIncludeStatistics);
            this.pnlUserInputs.Controls.Add(this.groupBox2);
            this.pnlUserInputs.Controls.Add(this.groupBox1);
            this.pnlUserInputs.Controls.Add(this.usrTwitterAuthorization);
            this.pnlUserInputs.Location = new System.Drawing.Point(10, 41);
            this.pnlUserInputs.Name = "pnlUserInputs";
            this.pnlUserInputs.Size = new System.Drawing.Size(362, 512);
            this.pnlUserInputs.TabIndex = 0;
            // 
            // chkExpandLatestStatusUrls
            // 
            this.chkExpandLatestStatusUrls.AutoSize = true;
            this.chkExpandLatestStatusUrls.Location = new System.Drawing.Point(21, 285);
            this.chkExpandLatestStatusUrls.Name = "chkExpandLatestStatusUrls";
            this.chkExpandLatestStatusUrls.Size = new System.Drawing.Size(204, 17);
            this.chkExpandLatestStatusUrls.TabIndex = 3;
            this.chkExpandLatestStatusUrls.Text = "E&xpand URLs in latest tweets (slower)";
            this.chkExpandLatestStatusUrls.UseVisualStyleBackColor = true;
            // 
            // chkIncludeLatestStatuses
            // 
            this.chkIncludeLatestStatuses.AutoSize = true;
            this.chkIncludeLatestStatuses.Location = new System.Drawing.Point(2, 265);
            this.chkIncludeLatestStatuses.Name = "chkIncludeLatestStatuses";
            this.chkIncludeLatestStatuses.Size = new System.Drawing.Size(279, 17);
            this.chkIncludeLatestStatuses.TabIndex = 2;
            this.chkIncludeLatestStatuses.Text = "&Add a Latest Tweet column to the Vertices worksheet";
            this.chkIncludeLatestStatuses.UseVisualStyleBackColor = true;
            this.chkIncludeLatestStatuses.CheckedChanged += new System.EventHandler(this.OnEventThatRequiresControlEnabling);
            // 
            // chkIncludeStatistics
            // 
            this.chkIncludeStatistics.AutoSize = true;
            this.chkIncludeStatistics.Location = new System.Drawing.Point(2, 307);
            this.chkIncludeStatistics.Name = "chkIncludeStatistics";
            this.chkIncludeStatistics.Size = new System.Drawing.Size(248, 17);
            this.chkIncludeStatistics.TabIndex = 4;
            this.chkIncludeStatistics.Text = "A&dd statistic columns to the Vertices worksheet";
            this.chkIncludeStatistics.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.chkIncludeMentionsEdges);
            this.groupBox2.Controls.Add(this.chkIncludeRepliesToEdges);
            this.groupBox2.Controls.Add(this.chkIncludeFollowedEdges);
            this.groupBox2.Location = new System.Drawing.Point(0, 170);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(350, 85);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Add an edge for each";
            // 
            // chkIncludeMentionsEdges
            // 
            this.chkIncludeMentionsEdges.AutoSize = true;
            this.chkIncludeMentionsEdges.Location = new System.Drawing.Point(15, 40);
            this.chkIncludeMentionsEdges.Name = "chkIncludeMentionsEdges";
            this.chkIncludeMentionsEdges.Size = new System.Drawing.Size(203, 17);
            this.chkIncludeMentionsEdges.TabIndex = 1;
            this.chkIncludeMentionsEdges.Text = "\"&Mentions\" relationship in latest tweet";
            this.chkIncludeMentionsEdges.UseVisualStyleBackColor = true;
            // 
            // chkIncludeRepliesToEdges
            // 
            this.chkIncludeRepliesToEdges.AutoSize = true;
            this.chkIncludeRepliesToEdges.Location = new System.Drawing.Point(15, 20);
            this.chkIncludeRepliesToEdges.Name = "chkIncludeRepliesToEdges";
            this.chkIncludeRepliesToEdges.Size = new System.Drawing.Size(207, 17);
            this.chkIncludeRepliesToEdges.TabIndex = 0;
            this.chkIncludeRepliesToEdges.Text = "\"&Replies-to\" relationship in latest tweet";
            this.chkIncludeRepliesToEdges.UseVisualStyleBackColor = true;
            // 
            // chkIncludeFollowedEdges
            // 
            this.chkIncludeFollowedEdges.AutoSize = true;
            this.chkIncludeFollowedEdges.Location = new System.Drawing.Point(15, 60);
            this.chkIncludeFollowedEdges.Name = "chkIncludeFollowedEdges";
            this.chkIncludeFollowedEdges.Size = new System.Drawing.Size(173, 17);
            this.chkIncludeFollowedEdges.TabIndex = 2;
            this.chkIncludeFollowedEdges.Text = "&Follows relationship (very slow!)";
            this.chkIncludeFollowedEdges.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radUseScreenNames);
            this.groupBox1.Controls.Add(this.radUseListName);
            this.groupBox1.Controls.Add(this.txbListName);
            this.groupBox1.Controls.Add(this.txbScreenNames);
            this.groupBox1.Controls.Add(this.lblScreenNamesHelp);
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(350, 162);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Add a vertex for each";
            // 
            // radUseScreenNames
            // 
            this.radUseScreenNames.AutoSize = true;
            this.radUseScreenNames.Location = new System.Drawing.Point(15, 68);
            this.radUseScreenNames.Name = "radUseScreenNames";
            this.radUseScreenNames.Size = new System.Drawing.Size(196, 17);
            this.radUseScreenNames.TabIndex = 2;
            this.radUseScreenNames.Text = "Twitter &user in this set of usernames:";
            this.radUseScreenNames.UseVisualStyleBackColor = true;
            this.radUseScreenNames.CheckedChanged += new System.EventHandler(this.OnEventThatRequiresControlEnabling);
            // 
            // radUseListName
            // 
            this.radUseListName.AutoSize = true;
            this.radUseListName.Checked = true;
            this.radUseListName.Location = new System.Drawing.Point(15, 18);
            this.radUseListName.Name = "radUseListName";
            this.radUseListName.Size = new System.Drawing.Size(167, 17);
            this.radUseListName.TabIndex = 0;
            this.radUseListName.TabStop = true;
            this.radUseListName.Text = "&Twitter user in this Twitter List:";
            this.radUseListName.UseVisualStyleBackColor = true;
            this.radUseListName.CheckedChanged += new System.EventHandler(this.OnEventThatRequiresControlEnabling);
            // 
            // txbListName
            // 
            this.txbListName.Location = new System.Drawing.Point(34, 41);
            this.txbListName.MaxLength = 41;
            this.txbListName.Name = "txbListName";
            this.txbListName.Size = new System.Drawing.Size(222, 20);
            this.txbListName.TabIndex = 1;
            // 
            // usrTwitterAuthorization
            // 
            this.usrTwitterAuthorization.Location = new System.Drawing.Point(0, 330);
            this.usrTwitterAuthorization.Name = "usrTwitterAuthorization";
            this.usrTwitterAuthorization.Size = new System.Drawing.Size(352, 180);
            this.usrTwitterAuthorization.Status = Smrf.NodeXL.GraphDataProviders.Twitter.TwitterAuthorizationStatus.HasTwitterAccountNotAuthorized;
            this.usrTwitterAuthorization.TabIndex = 5;
            // 
            // usrTwitterRateLimits
            // 
            this.usrTwitterRateLimits.Location = new System.Drawing.Point(12, 12);
            this.usrTwitterRateLimits.Name = "usrTwitterRateLimits";
            this.usrTwitterRateLimits.Size = new System.Drawing.Size(352, 14);
            this.usrTwitterRateLimits.TabIndex = 3;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(282, 559);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(196, 559);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(80, 23);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.slStatusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 596);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(374, 22);
            this.statusStrip1.TabIndex = 4;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // slStatusLabel
            // 
            this.slStatusLabel.Name = "slStatusLabel";
            this.slStatusLabel.Size = new System.Drawing.Size(359, 17);
            this.slStatusLabel.Spring = true;
            this.slStatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // TwitterGetListNetworkDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(374, 618);
            this.Controls.Add(this.usrTwitterRateLimits);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.pnlUserInputs);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TwitterGetListNetworkDialog";
            this.Text = "[Gets set in code]";
            this.pnlUserInputs.ResumeLayout(false);
            this.pnlUserInputs.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txbScreenNames;
        private System.Windows.Forms.Label lblScreenNamesHelp;
        private System.Windows.Forms.Panel pnlUserInputs;
        private TwitterAuthorizationControl usrTwitterAuthorization;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel slStatusLabel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radUseListName;
        private System.Windows.Forms.TextBox txbListName;
        private System.Windows.Forms.RadioButton radUseScreenNames;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox chkIncludeMentionsEdges;
        private System.Windows.Forms.CheckBox chkIncludeRepliesToEdges;
        private System.Windows.Forms.CheckBox chkIncludeFollowedEdges;
        private System.Windows.Forms.CheckBox chkIncludeLatestStatuses;
        private System.Windows.Forms.CheckBox chkIncludeStatistics;
        private System.Windows.Forms.CheckBox chkExpandLatestStatusUrls;
        private TwitterRateLimitsControl usrTwitterRateLimits;
    }
}
