﻿

namespace Smrf.NodeXL.GraphDataProviders.Twitter
{
    partial class TwitterGetUserNetworkDialog
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
            this.chkExpandLatestStatusUrls = new System.Windows.Forms.CheckBox();
            this.usrTwitterAuthorization = new Smrf.NodeXL.GraphDataProviders.Twitter.TwitterAuthorizationControl();
            this.usrLimitToN = new Smrf.NodeXL.GraphDataProviders.LimitToNControl();
            this.chkIncludeLatestStatuses = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radIncludeFollowedAndFollower = new System.Windows.Forms.RadioButton();
            this.radIncludeFollowerVertices = new System.Windows.Forms.RadioButton();
            this.radIncludeFollowedVertices = new System.Windows.Forms.RadioButton();
            this.txbScreenNameToAnalyze = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.usrNetworkLevel = new Smrf.NodeXL.GraphDataProviders.NetworkLevelControl();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chkIncludeMentionsEdges = new System.Windows.Forms.CheckBox();
            this.chkIncludeRepliesToEdges = new System.Windows.Forms.CheckBox();
            this.chkIncludeFollowedFollowerEdges = new System.Windows.Forms.CheckBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.slStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.usrTwitterRateLimits = new Smrf.NodeXL.GraphDataProviders.Twitter.TwitterRateLimitsControl();
            this.pnlUserInputs.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(196, 546);
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
            this.btnCancel.Location = new System.Drawing.Point(282, 546);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // pnlUserInputs
            // 
            this.pnlUserInputs.Controls.Add(this.chkExpandLatestStatusUrls);
            this.pnlUserInputs.Controls.Add(this.usrTwitterAuthorization);
            this.pnlUserInputs.Controls.Add(this.usrLimitToN);
            this.pnlUserInputs.Controls.Add(this.chkIncludeLatestStatuses);
            this.pnlUserInputs.Controls.Add(this.groupBox1);
            this.pnlUserInputs.Controls.Add(this.txbScreenNameToAnalyze);
            this.pnlUserInputs.Controls.Add(this.label1);
            this.pnlUserInputs.Controls.Add(this.usrNetworkLevel);
            this.pnlUserInputs.Controls.Add(this.groupBox2);
            this.pnlUserInputs.Location = new System.Drawing.Point(12, 39);
            this.pnlUserInputs.Name = "pnlUserInputs";
            this.pnlUserInputs.Size = new System.Drawing.Size(359, 500);
            this.pnlUserInputs.TabIndex = 0;
            // 
            // chkExpandLatestStatusUrls
            // 
            this.chkExpandLatestStatusUrls.AutoSize = true;
            this.chkExpandLatestStatusUrls.Location = new System.Drawing.Point(147, 266);
            this.chkExpandLatestStatusUrls.Name = "chkExpandLatestStatusUrls";
            this.chkExpandLatestStatusUrls.Size = new System.Drawing.Size(204, 17);
            this.chkExpandLatestStatusUrls.TabIndex = 6;
            this.chkExpandLatestStatusUrls.Text = "E&xpand URLs in latest tweets (slower)";
            this.chkExpandLatestStatusUrls.UseVisualStyleBackColor = true;
            // 
            // usrTwitterAuthorization
            // 
            this.usrTwitterAuthorization.Location = new System.Drawing.Point(0, 317);
            this.usrTwitterAuthorization.Name = "usrTwitterAuthorization";
            this.usrTwitterAuthorization.Size = new System.Drawing.Size(352, 180);
            this.usrTwitterAuthorization.Status = Smrf.NodeXL.GraphDataProviders.Twitter.TwitterAuthorizationStatus.HasTwitterAccountNotAuthorized;
            this.usrTwitterAuthorization.TabIndex = 8;
            // 
            // usrLimitToN
            // 
            this.usrLimitToN.Location = new System.Drawing.Point(128, 291);
            this.usrLimitToN.MaximumN = 9999;
            this.usrLimitToN.N = 2147483647;
            this.usrLimitToN.Name = "usrLimitToN";
            this.usrLimitToN.ObjectName = "people";
            this.usrLimitToN.Size = new System.Drawing.Size(168, 27);
            this.usrLimitToN.TabIndex = 7;
            // 
            // chkIncludeLatestStatuses
            // 
            this.chkIncludeLatestStatuses.Location = new System.Drawing.Point(128, 227);
            this.chkIncludeLatestStatuses.Name = "chkIncludeLatestStatuses";
            this.chkIncludeLatestStatuses.Size = new System.Drawing.Size(222, 37);
            this.chkIncludeLatestStatuses.TabIndex = 5;
            this.chkIncludeLatestStatuses.Text = "A&dd a Latest Tweet column to the Vertices worksheet";
            this.chkIncludeLatestStatuses.UseVisualStyleBackColor = true;
            this.chkIncludeLatestStatuses.CheckedChanged += new System.EventHandler(this.OnEventThatRequiresControlEnabling);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radIncludeFollowedAndFollower);
            this.groupBox1.Controls.Add(this.radIncludeFollowerVertices);
            this.groupBox1.Controls.Add(this.radIncludeFollowedVertices);
            this.groupBox1.Location = new System.Drawing.Point(0, 47);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(350, 86);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Add a vertex for each";
            // 
            // radIncludeFollowedAndFollower
            // 
            this.radIncludeFollowedAndFollower.AutoSize = true;
            this.radIncludeFollowedAndFollower.Location = new System.Drawing.Point(15, 60);
            this.radIncludeFollowedAndFollower.Name = "radIncludeFollowedAndFollower";
            this.radIncludeFollowedAndFollower.Size = new System.Drawing.Size(47, 17);
            this.radIncludeFollowedAndFollower.TabIndex = 2;
            this.radIncludeFollowedAndFollower.TabStop = true;
            this.radIncludeFollowedAndFollower.Text = "&Both";
            this.radIncludeFollowedAndFollower.UseVisualStyleBackColor = true;
            // 
            // radIncludeFollowerVertices
            // 
            this.radIncludeFollowerVertices.AutoSize = true;
            this.radIncludeFollowerVertices.Location = new System.Drawing.Point(15, 40);
            this.radIncludeFollowerVertices.Name = "radIncludeFollowerVertices";
            this.radIncludeFollowerVertices.Size = new System.Drawing.Size(143, 17);
            this.radIncludeFollowerVertices.TabIndex = 1;
            this.radIncludeFollowerVertices.Text = "P&erson following the user";
            this.radIncludeFollowerVertices.UseVisualStyleBackColor = true;
            // 
            // radIncludeFollowedVertices
            // 
            this.radIncludeFollowedVertices.AutoSize = true;
            this.radIncludeFollowedVertices.Checked = true;
            this.radIncludeFollowedVertices.Location = new System.Drawing.Point(15, 20);
            this.radIncludeFollowedVertices.Name = "radIncludeFollowedVertices";
            this.radIncludeFollowedVertices.Size = new System.Drawing.Size(155, 17);
            this.radIncludeFollowedVertices.TabIndex = 0;
            this.radIncludeFollowedVertices.TabStop = true;
            this.radIncludeFollowedVertices.Text = "&Person followed by the user";
            this.radIncludeFollowedVertices.UseVisualStyleBackColor = true;
            // 
            // txbScreenNameToAnalyze
            // 
            this.txbScreenNameToAnalyze.Location = new System.Drawing.Point(0, 19);
            this.txbScreenNameToAnalyze.MaxLength = 15;
            this.txbScreenNameToAnalyze.Name = "txbScreenNameToAnalyze";
            this.txbScreenNameToAnalyze.Size = new System.Drawing.Size(222, 20);
            this.txbScreenNameToAnalyze.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(264, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "&Get the Twitter network of the user with this username:";
            // 
            // usrNetworkLevel
            // 
            this.usrNetworkLevel.Level = Smrf.SocialNetworkLib.NetworkLevel.One;
            this.usrNetworkLevel.Location = new System.Drawing.Point(0, 230);
            this.usrNetworkLevel.Name = "usrNetworkLevel";
            this.usrNetworkLevel.Size = new System.Drawing.Size(119, 79);
            this.usrNetworkLevel.TabIndex = 4;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.chkIncludeMentionsEdges);
            this.groupBox2.Controls.Add(this.chkIncludeRepliesToEdges);
            this.groupBox2.Controls.Add(this.chkIncludeFollowedFollowerEdges);
            this.groupBox2.Location = new System.Drawing.Point(0, 139);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(350, 85);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Add an edge for each";
            // 
            // chkIncludeMentionsEdges
            // 
            this.chkIncludeMentionsEdges.AutoSize = true;
            this.chkIncludeMentionsEdges.Location = new System.Drawing.Point(15, 60);
            this.chkIncludeMentionsEdges.Name = "chkIncludeMentionsEdges";
            this.chkIncludeMentionsEdges.Size = new System.Drawing.Size(203, 17);
            this.chkIncludeMentionsEdges.TabIndex = 2;
            this.chkIncludeMentionsEdges.Text = "\"&Mentions\" relationship in latest tweet";
            this.chkIncludeMentionsEdges.UseVisualStyleBackColor = true;
            // 
            // chkIncludeRepliesToEdges
            // 
            this.chkIncludeRepliesToEdges.AutoSize = true;
            this.chkIncludeRepliesToEdges.Location = new System.Drawing.Point(15, 40);
            this.chkIncludeRepliesToEdges.Name = "chkIncludeRepliesToEdges";
            this.chkIncludeRepliesToEdges.Size = new System.Drawing.Size(207, 17);
            this.chkIncludeRepliesToEdges.TabIndex = 1;
            this.chkIncludeRepliesToEdges.Text = "\"&Replies-to\" relationship in latest tweet";
            this.chkIncludeRepliesToEdges.UseVisualStyleBackColor = true;
            // 
            // chkIncludeFollowedFollowerEdges
            // 
            this.chkIncludeFollowedFollowerEdges.AutoSize = true;
            this.chkIncludeFollowedFollowerEdges.Checked = true;
            this.chkIncludeFollowedFollowerEdges.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkIncludeFollowedFollowerEdges.Location = new System.Drawing.Point(15, 20);
            this.chkIncludeFollowedFollowerEdges.Name = "chkIncludeFollowedFollowerEdges";
            this.chkIncludeFollowedFollowerEdges.Size = new System.Drawing.Size(170, 17);
            this.chkIncludeFollowedFollowerEdges.TabIndex = 0;
            this.chkIncludeFollowedFollowerEdges.Text = "&Followed/following relationship";
            this.chkIncludeFollowedFollowerEdges.UseVisualStyleBackColor = true;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.slStatusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 579);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(374, 22);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 3;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // slStatusLabel
            // 
            this.slStatusLabel.Name = "slStatusLabel";
            this.slStatusLabel.Size = new System.Drawing.Size(359, 17);
            this.slStatusLabel.Spring = true;
            this.slStatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // usrTwitterRateLimits
            // 
            this.usrTwitterRateLimits.Location = new System.Drawing.Point(12, 12);
            this.usrTwitterRateLimits.Name = "usrTwitterRateLimits";
            this.usrTwitterRateLimits.Size = new System.Drawing.Size(352, 14);
            this.usrTwitterRateLimits.TabIndex = 3;
            // 
            // TwitterGetUserNetworkDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(374, 601);
            this.Controls.Add(this.usrTwitterRateLimits);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.pnlUserInputs);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TwitterGetUserNetworkDialog";
            this.Text = "[Gets set in code]";
            this.pnlUserInputs.ResumeLayout(false);
            this.pnlUserInputs.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Panel pnlUserInputs;
        private System.Windows.Forms.CheckBox chkIncludeLatestStatuses;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radIncludeFollowedAndFollower;
        private System.Windows.Forms.RadioButton radIncludeFollowerVertices;
        private System.Windows.Forms.RadioButton radIncludeFollowedVertices;
        private System.Windows.Forms.TextBox txbScreenNameToAnalyze;
        private System.Windows.Forms.Label label1;
        private NetworkLevelControl usrNetworkLevel;
        private Smrf.NodeXL.GraphDataProviders.LimitToNControl usrLimitToN;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel slStatusLabel;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox chkIncludeMentionsEdges;
        private System.Windows.Forms.CheckBox chkIncludeRepliesToEdges;
        private System.Windows.Forms.CheckBox chkIncludeFollowedFollowerEdges;
        private TwitterAuthorizationControl usrTwitterAuthorization;
        private System.Windows.Forms.CheckBox chkExpandLatestStatusUrls;
        private TwitterRateLimitsControl usrTwitterRateLimits;
    }
}
