using System.Drawing;
namespace Smrf.NodeXL.GraphDataProviders.Facebook
{
    partial class FacebookDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FacebookDialog));
            this.btnCancel = new System.Windows.Forms.Button();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.grpAttributes = new System.Windows.Forms.GroupBox();
            this.chkSelectAll = new System.Windows.Forms.CheckBox();
            this.dgAttributes = new System.Windows.Forms.DataGridView();
            this.attributeColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.includeColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.attributeValueColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.grpOptions = new System.Windows.Forms.GroupBox();
            this.chkIncludeMe = new System.Windows.Forms.CheckBox();
            this.chkTooltips = new System.Windows.Forms.CheckBox();
            this.nuLimit = new System.Windows.Forms.NumericUpDown();
            this.chkLimit = new System.Windows.Forms.CheckBox();
            this.dtEndDate = new System.Windows.Forms.DateTimePicker();
            this.lblAnd = new System.Windows.Forms.Label();
            this.dtStartDate = new System.Windows.Forms.DateTimePicker();
            this.rbBetween = new System.Windows.Forms.RadioButton();
            this.lblToPost = new System.Windows.Forms.Label();
            this.nuFromPost = new System.Windows.Forms.NumericUpDown();
            this.rbFromPost = new System.Windows.Forms.RadioButton();
            this.chkLike = new System.Windows.Forms.CheckBox();
            this.chkComment = new System.Windows.Forms.CheckBox();
            this.chkUsername = new System.Windows.Forms.CheckBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.slStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblMainText = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.ttRateLimit = new System.Windows.Forms.ToolTip(this.components);
            this.ttLogoutLogin = new System.Windows.Forms.ToolTip(this.components);
            this.ttCallPerSecond = new System.Windows.Forms.ToolTip(this.components);
            this.gbAddEdge = new System.Windows.Forms.GroupBox();
            this.chkAuthor = new System.Windows.Forms.CheckBox();
            this.gbFor = new System.Windows.Forms.GroupBox();
            this.nuToPost = new System.Windows.Forms.NumericUpDown();
            this.chkSelectAllEdges = new System.Windows.Forms.CheckBox();
            this.grpOfFeed = new System.Windows.Forms.GroupBox();
            this.chkMyFriendTimeline = new System.Windows.Forms.CheckBox();
            this.chkMyTimeline = new System.Windows.Forms.CheckBox();
            this.grpAttributes.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgAttributes)).BeginInit();
            this.grpOptions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nuLimit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nuFromPost)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.gbAddEdge.SuspendLayout();
            this.gbFor.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nuToPost)).BeginInit();
            this.grpOfFeed.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(547, 461);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.button1_Click);
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(15, 54);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(175, 13);
            this.linkLabel1.TabIndex = 12;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "Click here to logout from Facebook.";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // grpAttributes
            // 
            this.grpAttributes.Controls.Add(this.chkSelectAll);
            this.grpAttributes.Controls.Add(this.dgAttributes);
            this.grpAttributes.Location = new System.Drawing.Point(12, 78);
            this.grpAttributes.Name = "grpAttributes";
            this.grpAttributes.Size = new System.Drawing.Size(300, 406);
            this.grpAttributes.TabIndex = 11;
            this.grpAttributes.TabStop = false;
            this.grpAttributes.Text = "Attributes";
            // 
            // chkSelectAll
            // 
            this.chkSelectAll.Location = new System.Drawing.Point(215, 19);
            this.chkSelectAll.Name = "chkSelectAll";
            this.chkSelectAll.Size = new System.Drawing.Size(14, 15);
            this.chkSelectAll.TabIndex = 1;
            this.chkSelectAll.UseVisualStyleBackColor = true;
            this.chkSelectAll.CheckedChanged += new System.EventHandler(this.chkSelectAll_CheckedChanged);
            // 
            // dgAttributes
            // 
            this.dgAttributes.AllowUserToAddRows = false;
            this.dgAttributes.AllowUserToDeleteRows = false;
            this.dgAttributes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgAttributes.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.attributeColumn,
            this.includeColumn,
            this.attributeValueColumn});
            this.dgAttributes.Location = new System.Drawing.Point(9, 17);
            this.dgAttributes.Name = "dgAttributes";
            this.dgAttributes.RowHeadersVisible = false;
            this.dgAttributes.RowTemplate.Height = 24;
            this.dgAttributes.Size = new System.Drawing.Size(285, 383);
            this.dgAttributes.TabIndex = 0;
            // 
            // attributeColumn
            // 
            this.attributeColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.attributeColumn.HeaderText = "Attribute";
            this.attributeColumn.Name = "attributeColumn";
            this.attributeColumn.ReadOnly = true;
            // 
            // includeColumn
            // 
            this.includeColumn.HeaderText = "Include";
            this.includeColumn.Name = "includeColumn";
            this.includeColumn.Width = 61;
            // 
            // attributeValueColumn
            // 
            this.attributeValueColumn.HeaderText = "Attribute";
            this.attributeValueColumn.Name = "attributeValueColumn";
            this.attributeValueColumn.Visible = false;
            // 
            // grpOptions
            // 
            this.grpOptions.Controls.Add(this.chkIncludeMe);
            this.grpOptions.Controls.Add(this.chkTooltips);
            this.grpOptions.Controls.Add(this.nuLimit);
            this.grpOptions.Controls.Add(this.chkLimit);
            this.grpOptions.Location = new System.Drawing.Point(323, 365);
            this.grpOptions.Name = "grpOptions";
            this.grpOptions.Size = new System.Drawing.Size(325, 90);
            this.grpOptions.TabIndex = 10;
            this.grpOptions.TabStop = false;
            this.grpOptions.Text = "Options";
            // 
            // chkIncludeMe
            // 
            this.chkIncludeMe.AutoSize = true;
            this.chkIncludeMe.Location = new System.Drawing.Point(15, 64);
            this.chkIncludeMe.Name = "chkIncludeMe";
            this.chkIncludeMe.Size = new System.Drawing.Size(79, 17);
            this.chkIncludeMe.TabIndex = 11;
            this.chkIncludeMe.Text = "Include Me";
            this.chkIncludeMe.UseVisualStyleBackColor = true;
            // 
            // chkTooltips
            // 
            this.chkTooltips.AutoSize = true;
            this.chkTooltips.Location = new System.Drawing.Point(15, 42);
            this.chkTooltips.Name = "chkTooltips";
            this.chkTooltips.Size = new System.Drawing.Size(114, 17);
            this.chkTooltips.TabIndex = 10;
            this.chkTooltips.Text = "Download Tooltips";
            this.chkTooltips.UseVisualStyleBackColor = true;
            // 
            // nuLimit
            // 
            this.nuLimit.Location = new System.Drawing.Point(213, 19);
            this.nuLimit.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.nuLimit.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.nuLimit.Name = "nuLimit";
            this.nuLimit.Size = new System.Drawing.Size(48, 20);
            this.nuLimit.TabIndex = 9;
            this.nuLimit.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            // 
            // chkLimit
            // 
            this.chkLimit.AutoSize = true;
            this.chkLimit.Location = new System.Drawing.Point(15, 19);
            this.chkLimit.Name = "chkLimit";
            this.chkLimit.Size = new System.Drawing.Size(192, 17);
            this.chkLimit.TabIndex = 8;
            this.chkLimit.Text = "Limit nr. comments/likes per post to";
            this.chkLimit.UseVisualStyleBackColor = true;
            // 
            // dtEndDate
            // 
            this.dtEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtEndDate.Location = new System.Drawing.Point(231, 43);
            this.dtEndDate.Name = "dtEndDate";
            this.dtEndDate.Size = new System.Drawing.Size(80, 20);
            this.dtEndDate.TabIndex = 19;
            // 
            // lblAnd
            // 
            this.lblAnd.AutoSize = true;
            this.lblAnd.Location = new System.Drawing.Point(205, 45);
            this.lblAnd.Name = "lblAnd";
            this.lblAnd.Size = new System.Drawing.Size(25, 13);
            this.lblAnd.TabIndex = 18;
            this.lblAnd.Text = "and";
            // 
            // dtStartDate
            // 
            this.dtStartDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtStartDate.Location = new System.Drawing.Point(122, 43);
            this.dtStartDate.Name = "dtStartDate";
            this.dtStartDate.Size = new System.Drawing.Size(80, 20);
            this.dtStartDate.TabIndex = 17;
            // 
            // rbBetween
            // 
            this.rbBetween.AutoSize = true;
            this.rbBetween.Location = new System.Drawing.Point(11, 43);
            this.rbBetween.Name = "rbBetween";
            this.rbBetween.Size = new System.Drawing.Size(105, 17);
            this.rbBetween.TabIndex = 16;
            this.rbBetween.Text = "in posts between";
            this.rbBetween.UseVisualStyleBackColor = true;
            // 
            // lblToPost
            // 
            this.lblToPost.AutoSize = true;
            this.lblToPost.Location = new System.Drawing.Point(130, 21);
            this.lblToPost.Name = "lblToPost";
            this.lblToPost.Size = new System.Drawing.Size(39, 13);
            this.lblToPost.TabIndex = 15;
            this.lblToPost.Text = "to post";
            // 
            // nuFromPost
            // 
            this.nuFromPost.Location = new System.Drawing.Point(83, 16);
            this.nuFromPost.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nuFromPost.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nuFromPost.Name = "nuFromPost";
            this.nuFromPost.Size = new System.Drawing.Size(41, 20);
            this.nuFromPost.TabIndex = 14;
            this.nuFromPost.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nuFromPost.ValueChanged += new System.EventHandler(this.nuFromPost_ValueChanged);
            // 
            // rbFromPost
            // 
            this.rbFromPost.AutoSize = true;
            this.rbFromPost.Checked = true;
            this.rbFromPost.Location = new System.Drawing.Point(11, 19);
            this.rbFromPost.Name = "rbFromPost";
            this.rbFromPost.Size = new System.Drawing.Size(68, 17);
            this.rbFromPost.TabIndex = 13;
            this.rbFromPost.TabStop = true;
            this.rbFromPost.Text = "from post";
            this.rbFromPost.UseVisualStyleBackColor = true;
            // 
            // chkLike
            // 
            this.chkLike.AutoSize = true;
            this.chkLike.Location = new System.Drawing.Point(21, 88);
            this.chkLike.Name = "chkLike";
            this.chkLike.Size = new System.Drawing.Size(46, 17);
            this.chkLike.TabIndex = 11;
            this.chkLike.Text = "Like";
            this.chkLike.UseVisualStyleBackColor = true;
            // 
            // chkComment
            // 
            this.chkComment.AutoSize = true;
            this.chkComment.Location = new System.Drawing.Point(21, 65);
            this.chkComment.Name = "chkComment";
            this.chkComment.Size = new System.Drawing.Size(70, 17);
            this.chkComment.TabIndex = 10;
            this.chkComment.Text = "Comment";
            this.chkComment.UseVisualStyleBackColor = true;
            // 
            // chkUsername
            // 
            this.chkUsername.AutoSize = true;
            this.chkUsername.Location = new System.Drawing.Point(21, 42);
            this.chkUsername.Name = "chkUsername";
            this.chkUsername.Size = new System.Drawing.Size(84, 17);
            this.chkUsername.TabIndex = 8;
            this.chkUsername.Text = "User tagged";
            this.chkUsername.UseVisualStyleBackColor = true;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.slStatusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 487);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(660, 22);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 4;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // slStatusLabel
            // 
            this.slStatusLabel.Name = "slStatusLabel";
            this.slStatusLabel.Size = new System.Drawing.Size(0, 17);
            // 
            // lblMainText
            // 
            this.lblMainText.Location = new System.Drawing.Point(12, 9);
            this.lblMainText.Name = "lblMainText";
            this.lblMainText.Size = new System.Drawing.Size(610, 47);
            this.lblMainText.TabIndex = 3;
            this.lblMainText.Text = resources.GetString("lblMainText.Text");
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(349, 461);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "Login";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // btnOK
            // 
            this.btnOK.Enabled = false;
            this.btnOK.Location = new System.Drawing.Point(448, 461);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "Download";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // ttRateLimit
            // 
            this.ttRateLimit.AutoPopDelay = 10000;
            this.ttRateLimit.InitialDelay = 500;
            this.ttRateLimit.ReshowDelay = 100;
            this.ttRateLimit.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.ttRateLimit.ToolTipTitle = "Prevent Rate Limit";
            // 
            // ttLogoutLogin
            // 
            this.ttLogoutLogin.AutoPopDelay = 15000;
            this.ttLogoutLogin.InitialDelay = 500;
            this.ttLogoutLogin.ReshowDelay = 100;
            this.ttLogoutLogin.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.ttLogoutLogin.ToolTipTitle = "Logout/Login";
            // 
            // ttCallPerSecond
            // 
            this.ttCallPerSecond.AutoPopDelay = 10000;
            this.ttCallPerSecond.InitialDelay = 500;
            this.ttCallPerSecond.ReshowDelay = 100;
            this.ttCallPerSecond.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.ttCallPerSecond.ToolTipTitle = "One call per second";
            // 
            // gbAddEdge
            // 
            this.gbAddEdge.Controls.Add(this.chkAuthor);
            this.gbAddEdge.Controls.Add(this.chkUsername);
            this.gbAddEdge.Controls.Add(this.chkComment);
            this.gbAddEdge.Controls.Add(this.chkLike);
            this.gbAddEdge.Location = new System.Drawing.Point(323, 78);
            this.gbAddEdge.Name = "gbAddEdge";
            this.gbAddEdge.Size = new System.Drawing.Size(325, 116);
            this.gbAddEdge.TabIndex = 13;
            this.gbAddEdge.TabStop = false;
            // 
            // chkAuthor
            // 
            this.chkAuthor.AutoSize = true;
            this.chkAuthor.Location = new System.Drawing.Point(21, 19);
            this.chkAuthor.Name = "chkAuthor";
            this.chkAuthor.Size = new System.Drawing.Size(80, 17);
            this.chkAuthor.TabIndex = 12;
            this.chkAuthor.Text = "Post author";
            this.chkAuthor.UseVisualStyleBackColor = true;
            // 
            // gbFor
            // 
            this.gbFor.Controls.Add(this.nuToPost);
            this.gbFor.Controls.Add(this.rbFromPost);
            this.gbFor.Controls.Add(this.dtEndDate);
            this.gbFor.Controls.Add(this.nuFromPost);
            this.gbFor.Controls.Add(this.lblAnd);
            this.gbFor.Controls.Add(this.lblToPost);
            this.gbFor.Controls.Add(this.dtStartDate);
            this.gbFor.Controls.Add(this.rbBetween);
            this.gbFor.Location = new System.Drawing.Point(323, 200);
            this.gbFor.Name = "gbFor";
            this.gbFor.Size = new System.Drawing.Size(325, 84);
            this.gbFor.TabIndex = 20;
            this.gbFor.TabStop = false;
            this.gbFor.Text = "found";
            // 
            // nuToPost
            // 
            this.nuToPost.Location = new System.Drawing.Point(175, 16);
            this.nuToPost.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nuToPost.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nuToPost.Name = "nuToPost";
            this.nuToPost.Size = new System.Drawing.Size(41, 20);
            this.nuToPost.TabIndex = 20;
            this.nuToPost.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // chkSelectAllEdges
            // 
            this.chkSelectAllEdges.AutoSize = true;
            this.chkSelectAllEdges.Location = new System.Drawing.Point(338, 74);
            this.chkSelectAllEdges.Name = "chkSelectAllEdges";
            this.chkSelectAllEdges.Size = new System.Drawing.Size(129, 17);
            this.chkSelectAllEdges.TabIndex = 21;
            this.chkSelectAllEdges.Text = "Add an edge for each";
            this.chkSelectAllEdges.UseVisualStyleBackColor = true;
            this.chkSelectAllEdges.CheckedChanged += new System.EventHandler(this.chkSelectAllEdges_CheckedChanged);
            // 
            // grpOfFeed
            // 
            this.grpOfFeed.Controls.Add(this.chkMyFriendTimeline);
            this.grpOfFeed.Controls.Add(this.chkMyTimeline);
            this.grpOfFeed.Location = new System.Drawing.Point(323, 290);
            this.grpOfFeed.Name = "grpOfFeed";
            this.grpOfFeed.Size = new System.Drawing.Size(325, 69);
            this.grpOfFeed.TabIndex = 22;
            this.grpOfFeed.TabStop = false;
            this.grpOfFeed.Text = "of";
            // 
            // chkMyFriendTimeline
            // 
            this.chkMyFriendTimeline.AutoSize = true;
            this.chkMyFriendTimeline.Location = new System.Drawing.Point(11, 43);
            this.chkMyFriendTimeline.Name = "chkMyFriendTimeline";
            this.chkMyFriendTimeline.Size = new System.Drawing.Size(113, 17);
            this.chkMyFriendTimeline.TabIndex = 1;
            this.chkMyFriendTimeline.Text = "my friend\'s timeline";
            this.chkMyFriendTimeline.UseVisualStyleBackColor = true;
            // 
            // chkMyTimeline
            // 
            this.chkMyTimeline.AutoSize = true;
            this.chkMyTimeline.Checked = true;
            this.chkMyTimeline.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkMyTimeline.Location = new System.Drawing.Point(11, 19);
            this.chkMyTimeline.Name = "chkMyTimeline";
            this.chkMyTimeline.Size = new System.Drawing.Size(77, 17);
            this.chkMyTimeline.TabIndex = 0;
            this.chkMyTimeline.Text = "my timeline";
            this.chkMyTimeline.UseVisualStyleBackColor = true;
            // 
            // FacebookDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(660, 509);
            this.Controls.Add(this.grpOfFeed);
            this.Controls.Add(this.chkSelectAllEdges);
            this.Controls.Add(this.gbFor);
            this.Controls.Add(this.gbAddEdge);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.grpAttributes);
            this.Controls.Add(this.grpOptions);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.lblMainText);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FacebookDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Facebook Personal and Timeline Network (v.1.9.2)";
            this.Load += new System.EventHandler(this.FacebookDialog_Load);
            this.grpAttributes.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgAttributes)).EndInit();
            this.grpOptions.ResumeLayout(false);
            this.grpOptions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nuLimit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nuFromPost)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.gbAddEdge.ResumeLayout(false);
            this.gbAddEdge.PerformLayout();
            this.gbFor.ResumeLayout(false);
            this.gbFor.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nuToPost)).EndInit();
            this.grpOfFeed.ResumeLayout(false);
            this.grpOfFeed.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }        

        #endregion

        private System.Windows.Forms.Button btnCancel;
        public System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label lblMainText;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel slStatusLabel;
        private System.Windows.Forms.GroupBox grpAttributes;
        public System.Windows.Forms.CheckBox chkSelectAll;
        public System.Windows.Forms.DataGridView dgAttributes;
        private System.Windows.Forms.GroupBox grpOptions;
        private System.Windows.Forms.DataGridViewTextBoxColumn attributeColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn includeColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn attributeValueColumn;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.ToolTip ttRateLimit;
        private System.Windows.Forms.ToolTip ttLogoutLogin;
        private System.Windows.Forms.ToolTip ttCallPerSecond;
        private System.Windows.Forms.CheckBox chkUsername;
        private System.Windows.Forms.CheckBox chkLike;
        private System.Windows.Forms.CheckBox chkComment;
        private System.Windows.Forms.DateTimePicker dtEndDate;
        private System.Windows.Forms.Label lblAnd;
        private System.Windows.Forms.DateTimePicker dtStartDate;
        private System.Windows.Forms.RadioButton rbBetween;
        private System.Windows.Forms.Label lblToPost;
        private System.Windows.Forms.NumericUpDown nuFromPost;
        private System.Windows.Forms.RadioButton rbFromPost;
        private System.Windows.Forms.GroupBox gbAddEdge;
        private System.Windows.Forms.GroupBox gbFor;
        private System.Windows.Forms.CheckBox chkLimit;
        private System.Windows.Forms.NumericUpDown nuLimit;
        private System.Windows.Forms.CheckBox chkAuthor;
        private System.Windows.Forms.CheckBox chkSelectAllEdges;
        private System.Windows.Forms.GroupBox grpOfFeed;
        private System.Windows.Forms.CheckBox chkMyFriendTimeline;
        private System.Windows.Forms.CheckBox chkMyTimeline;
        private System.Windows.Forms.CheckBox chkTooltips;
        private System.Windows.Forms.CheckBox chkIncludeMe;
        private System.Windows.Forms.NumericUpDown nuToPost;
    }
}