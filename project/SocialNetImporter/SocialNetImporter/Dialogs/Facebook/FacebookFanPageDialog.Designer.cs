using System.Drawing;
namespace Smrf.NodeXL.GraphDataProviders.Facebook
{
    partial class FacebookFanPageDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FacebookFanPageDialog));
            this.btnCancel = new System.Windows.Forms.Button();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.gbOptions = new System.Windows.Forms.GroupBox();
            this.nuToPost = new System.Windows.Forms.NumericUpDown();
            this.nudLimit = new System.Windows.Forms.NumericUpDown();
            this.chkLimit = new System.Windows.Forms.CheckBox();
            this.lblAnd = new System.Windows.Forms.Label();
            this.dtEndDate = new System.Windows.Forms.DateTimePicker();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.chkIncludeOthers = new System.Windows.Forms.CheckBox();
            this.dtStartDate = new System.Windows.Forms.DateTimePicker();
            this.rbDateDownload = new System.Windows.Forms.RadioButton();
            this.rbDownloadFromPost = new System.Windows.Forms.RadioButton();
            this.lblToPost = new System.Windows.Forms.Label();
            this.chkWallPosts = new System.Windows.Forms.CheckBox();
            this.nudFromPost = new System.Windows.Forms.NumericUpDown();
            this.chkStatusUpdates = new System.Windows.Forms.CheckBox();
            this.grpAttributes = new System.Windows.Forms.GroupBox();
            this.chkSelectAll = new System.Windows.Forms.CheckBox();
            this.dgAttributes = new System.Windows.Forms.DataGridView();
            this.attributeColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.includeColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.attributeValueColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gbNetwork = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chkPostPostComments = new System.Windows.Forms.CheckBox();
            this.lblPostPostNetwork = new System.Windows.Forms.Label();
            this.lblUseUserNetwork = new System.Windows.Forms.Label();
            this.chkPostPostLikes = new System.Windows.Forms.CheckBox();
            this.chkUserUserLikes = new System.Windows.Forms.CheckBox();
            this.chkUserUserComments = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblUserPostNetwork = new System.Windows.Forms.Label();
            this.chkUserPostLikes = new System.Windows.Forms.CheckBox();
            this.chkUserPostComments = new System.Windows.Forms.CheckBox();
            this.gbFanPage = new System.Windows.Forms.GroupBox();
            this.txtPageUsernameID = new System.Windows.Forms.TextBox();
            this.lblNameID = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.slStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblMainText = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.toolTip2 = new System.Windows.Forms.ToolTip(this.components);
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.flpResults = new System.Windows.Forms.FlowLayoutPanel();
            this.bgLoadResults = new System.ComponentModel.BackgroundWorker();
            this.pnResults = new System.Windows.Forms.Panel();
            this.piLoading = new Smrf.NodeXL.GraphDataProviders.Facebook.ProgressIndicator();
            this.gbOptions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nuToPost)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudLimit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudFromPost)).BeginInit();
            this.grpAttributes.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgAttributes)).BeginInit();
            this.gbNetwork.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.gbFanPage.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.pnResults.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(619, 472);
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
            this.linkLabel1.Location = new System.Drawing.Point(15, 58);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(175, 13);
            this.linkLabel1.TabIndex = 17;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "Click here to logout from Facebook.";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // gbOptions
            // 
            this.gbOptions.Controls.Add(this.nuToPost);
            this.gbOptions.Controls.Add(this.nudLimit);
            this.gbOptions.Controls.Add(this.chkLimit);
            this.gbOptions.Controls.Add(this.lblAnd);
            this.gbOptions.Controls.Add(this.dtEndDate);
            this.gbOptions.Controls.Add(this.pictureBox1);
            this.gbOptions.Controls.Add(this.chkIncludeOthers);
            this.gbOptions.Controls.Add(this.dtStartDate);
            this.gbOptions.Controls.Add(this.rbDateDownload);
            this.gbOptions.Controls.Add(this.rbDownloadFromPost);
            this.gbOptions.Controls.Add(this.lblToPost);
            this.gbOptions.Controls.Add(this.chkWallPosts);
            this.gbOptions.Controls.Add(this.nudFromPost);
            this.gbOptions.Controls.Add(this.chkStatusUpdates);
            this.gbOptions.Location = new System.Drawing.Point(358, 330);
            this.gbOptions.Name = "gbOptions";
            this.gbOptions.Size = new System.Drawing.Size(366, 136);
            this.gbOptions.TabIndex = 16;
            this.gbOptions.TabStop = false;
            this.gbOptions.Text = "Options";
            // 
            // nuToPost
            // 
            this.nuToPost.Location = new System.Drawing.Point(241, 18);
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
            this.nuToPost.Size = new System.Drawing.Size(50, 20);
            this.nuToPost.TabIndex = 25;
            this.nuToPost.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // nudLimit
            // 
            this.nudLimit.Location = new System.Drawing.Point(211, 66);
            this.nudLimit.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudLimit.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudLimit.Name = "nudLimit";
            this.nudLimit.Size = new System.Drawing.Size(51, 20);
            this.nudLimit.TabIndex = 24;
            this.nudLimit.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            // 
            // chkLimit
            // 
            this.chkLimit.AutoSize = true;
            this.chkLimit.Location = new System.Drawing.Point(14, 67);
            this.chkLimit.Name = "chkLimit";
            this.chkLimit.Size = new System.Drawing.Size(192, 17);
            this.chkLimit.TabIndex = 23;
            this.chkLimit.Text = "Limit nr. comments/likes per post to";
            this.chkLimit.UseVisualStyleBackColor = true;
            // 
            // lblAnd
            // 
            this.lblAnd.AutoSize = true;
            this.lblAnd.Location = new System.Drawing.Point(249, 46);
            this.lblAnd.Name = "lblAnd";
            this.lblAnd.Size = new System.Drawing.Size(25, 13);
            this.lblAnd.TabIndex = 22;
            this.lblAnd.Text = "and";
            // 
            // dtEndDate
            // 
            this.dtEndDate.CustomFormat = "M/dd/yyyy";
            this.dtEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtEndDate.Location = new System.Drawing.Point(277, 44);
            this.dtEndDate.Name = "dtEndDate";
            this.dtEndDate.Size = new System.Drawing.Size(83, 20);
            this.dtEndDate.TabIndex = 21;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(268, 90);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(15, 15);
            this.pictureBox1.TabIndex = 20;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // chkIncludeOthers
            // 
            this.chkIncludeOthers.AutoSize = true;
            this.chkIncludeOthers.Location = new System.Drawing.Point(13, 90);
            this.chkIncludeOthers.Name = "chkIncludeOthers";
            this.chkIncludeOthers.Size = new System.Drawing.Size(249, 17);
            this.chkIncludeOthers.TabIndex = 19;
            this.chkIncludeOthers.Text = "Include also posts not made by the page owner";
            this.chkIncludeOthers.UseVisualStyleBackColor = true;
            // 
            // dtStartDate
            // 
            this.dtStartDate.CustomFormat = "M/dd/yyyy";
            this.dtStartDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtStartDate.Location = new System.Drawing.Point(161, 44);
            this.dtStartDate.Name = "dtStartDate";
            this.dtStartDate.Size = new System.Drawing.Size(83, 20);
            this.dtStartDate.TabIndex = 18;
            // 
            // rbDateDownload
            // 
            this.rbDateDownload.AutoSize = true;
            this.rbDateDownload.Location = new System.Drawing.Point(14, 44);
            this.rbDateDownload.Name = "rbDateDownload";
            this.rbDateDownload.Size = new System.Drawing.Size(145, 17);
            this.rbDateDownload.TabIndex = 17;
            this.rbDateDownload.Text = "Download posts between";
            this.rbDateDownload.UseVisualStyleBackColor = true;
            // 
            // rbDownloadFromPost
            // 
            this.rbDownloadFromPost.AutoSize = true;
            this.rbDownloadFromPost.Checked = true;
            this.rbDownloadFromPost.Location = new System.Drawing.Point(14, 21);
            this.rbDownloadFromPost.Name = "rbDownloadFromPost";
            this.rbDownloadFromPost.Size = new System.Drawing.Size(119, 17);
            this.rbDownloadFromPost.TabIndex = 16;
            this.rbDownloadFromPost.TabStop = true;
            this.rbDownloadFromPost.Text = "Download from post";
            this.rbDownloadFromPost.UseVisualStyleBackColor = true;
            // 
            // lblToPost
            // 
            this.lblToPost.AutoSize = true;
            this.lblToPost.Location = new System.Drawing.Point(196, 23);
            this.lblToPost.Name = "lblToPost";
            this.lblToPost.Size = new System.Drawing.Size(39, 13);
            this.lblToPost.TabIndex = 4;
            this.lblToPost.Text = "to post";
            // 
            // chkWallPosts
            // 
            this.chkWallPosts.AutoSize = true;
            this.chkWallPosts.Location = new System.Drawing.Point(150, 113);
            this.chkWallPosts.Name = "chkWallPosts";
            this.chkWallPosts.Size = new System.Drawing.Size(92, 17);
            this.chkWallPosts.TabIndex = 15;
            this.chkWallPosts.Text = "Get wall posts";
            this.chkWallPosts.UseVisualStyleBackColor = true;
            // 
            // nudFromPost
            // 
            this.nudFromPost.Location = new System.Drawing.Point(139, 18);
            this.nudFromPost.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudFromPost.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudFromPost.Name = "nudFromPost";
            this.nudFromPost.Size = new System.Drawing.Size(50, 20);
            this.nudFromPost.TabIndex = 2;
            this.nudFromPost.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudFromPost.ValueChanged += new System.EventHandler(this.nudFromPost_ValueChanged);
            // 
            // chkStatusUpdates
            // 
            this.chkStatusUpdates.AutoSize = true;
            this.chkStatusUpdates.Location = new System.Drawing.Point(13, 113);
            this.chkStatusUpdates.Name = "chkStatusUpdates";
            this.chkStatusUpdates.Size = new System.Drawing.Size(115, 17);
            this.chkStatusUpdates.TabIndex = 14;
            this.chkStatusUpdates.Text = "Get status updates";
            this.chkStatusUpdates.UseVisualStyleBackColor = true;
            // 
            // grpAttributes
            // 
            this.grpAttributes.Controls.Add(this.chkSelectAll);
            this.grpAttributes.Controls.Add(this.dgAttributes);
            this.grpAttributes.Location = new System.Drawing.Point(15, 150);
            this.grpAttributes.Name = "grpAttributes";
            this.grpAttributes.Size = new System.Drawing.Size(327, 345);
            this.grpAttributes.TabIndex = 13;
            this.grpAttributes.TabStop = false;
            this.grpAttributes.Text = "Attributes";
            // 
            // chkSelectAll
            // 
            this.chkSelectAll.Location = new System.Drawing.Point(268, 19);
            this.chkSelectAll.Name = "chkSelectAll";
            this.chkSelectAll.Size = new System.Drawing.Size(14, 15);
            this.chkSelectAll.TabIndex = 1;
            this.chkSelectAll.UseVisualStyleBackColor = true;
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
            this.dgAttributes.Size = new System.Drawing.Size(308, 322);
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
            // gbNetwork
            // 
            this.gbNetwork.Controls.Add(this.groupBox2);
            this.gbNetwork.Controls.Add(this.groupBox1);
            this.gbNetwork.Location = new System.Drawing.Point(358, 84);
            this.gbNetwork.Name = "gbNetwork";
            this.gbNetwork.Size = new System.Drawing.Size(366, 240);
            this.gbNetwork.TabIndex = 12;
            this.gbNetwork.TabStop = false;
            this.gbNetwork.Text = "Network";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.chkPostPostComments);
            this.groupBox2.Controls.Add(this.lblPostPostNetwork);
            this.groupBox2.Controls.Add(this.lblUseUserNetwork);
            this.groupBox2.Controls.Add(this.chkPostPostLikes);
            this.groupBox2.Controls.Add(this.chkUserUserLikes);
            this.groupBox2.Controls.Add(this.chkUserUserComments);
            this.groupBox2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.groupBox2.Location = new System.Drawing.Point(14, 20);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(346, 136);
            this.groupBox2.TabIndex = 14;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Unimodal Networks";
            this.toolTip1.SetToolTip(this.groupBox2, "Unimodal networks contain only one type of object.");
            // 
            // chkPostPostComments
            // 
            this.chkPostPostComments.AutoSize = true;
            this.chkPostPostComments.Location = new System.Drawing.Point(162, 113);
            this.chkPostPostComments.Name = "chkPostPostComments";
            this.chkPostPostComments.Size = new System.Drawing.Size(122, 17);
            this.chkPostPostComments.TabIndex = 18;
            this.chkPostPostComments.Text = "Based on comments";
            this.chkPostPostComments.UseVisualStyleBackColor = true;
            this.chkPostPostComments.CheckedChanged += new System.EventHandler(this.ChkCheckedChanged);
            // 
            // lblPostPostNetwork
            // 
            this.lblPostPostNetwork.AutoSize = true;
            this.lblPostPostNetwork.Location = new System.Drawing.Point(39, 102);
            this.lblPostPostNetwork.Name = "lblPostPostNetwork";
            this.lblPostPostNetwork.Size = new System.Drawing.Size(95, 13);
            this.lblPostPostNetwork.TabIndex = 17;
            this.lblPostPostNetwork.Text = "Post-Post Network";
            // 
            // lblUseUserNetwork
            // 
            this.lblUseUserNetwork.AutoSize = true;
            this.lblUseUserNetwork.Location = new System.Drawing.Point(39, 32);
            this.lblUseUserNetwork.Name = "lblUseUserNetwork";
            this.lblUseUserNetwork.Size = new System.Drawing.Size(97, 13);
            this.lblUseUserNetwork.TabIndex = 16;
            this.lblUseUserNetwork.Text = "User-User Network";
            // 
            // chkPostPostLikes
            // 
            this.chkPostPostLikes.AutoSize = true;
            this.chkPostPostLikes.Location = new System.Drawing.Point(162, 88);
            this.chkPostPostLikes.Name = "chkPostPostLikes";
            this.chkPostPostLikes.Size = new System.Drawing.Size(95, 17);
            this.chkPostPostLikes.TabIndex = 15;
            this.chkPostPostLikes.Text = "Based on likes\r\n";
            this.chkPostPostLikes.UseVisualStyleBackColor = true;
            this.chkPostPostLikes.CheckedChanged += new System.EventHandler(this.ChkCheckedChanged);
            // 
            // chkUserUserLikes
            // 
            this.chkUserUserLikes.AutoSize = true;
            this.chkUserUserLikes.Location = new System.Drawing.Point(162, 19);
            this.chkUserUserLikes.Name = "chkUserUserLikes";
            this.chkUserUserLikes.Size = new System.Drawing.Size(110, 17);
            this.chkUserUserLikes.TabIndex = 14;
            this.chkUserUserLikes.Text = "Based on co-likes";
            this.chkUserUserLikes.UseVisualStyleBackColor = true;
            this.chkUserUserLikes.CheckedChanged += new System.EventHandler(this.ChkCheckedChanged);
            // 
            // chkUserUserComments
            // 
            this.chkUserUserComments.AutoSize = true;
            this.chkUserUserComments.Checked = true;
            this.chkUserUserComments.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkUserUserComments.Location = new System.Drawing.Point(162, 42);
            this.chkUserUserComments.Name = "chkUserUserComments";
            this.chkUserUserComments.Size = new System.Drawing.Size(137, 17);
            this.chkUserUserComments.TabIndex = 13;
            this.chkUserUserComments.Text = "Based on co-comments";
            this.chkUserUserComments.UseVisualStyleBackColor = true;
            this.chkUserUserComments.CheckedChanged += new System.EventHandler(this.ChkCheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblUserPostNetwork);
            this.groupBox1.Controls.Add(this.chkUserPostLikes);
            this.groupBox1.Controls.Add(this.chkUserPostComments);
            this.groupBox1.Location = new System.Drawing.Point(13, 162);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(347, 72);
            this.groupBox1.TabIndex = 13;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Bi-Modal Networks";
            this.toolTip2.SetToolTip(this.groupBox1, "Bi-Modal networks contain two types of objects.");
            // 
            // lblUserPostNetwork
            // 
            this.lblUserPostNetwork.AutoSize = true;
            this.lblUserPostNetwork.Location = new System.Drawing.Point(40, 32);
            this.lblUserPostNetwork.Name = "lblUserPostNetwork";
            this.lblUserPostNetwork.Size = new System.Drawing.Size(96, 13);
            this.lblUserPostNetwork.TabIndex = 10;
            this.lblUserPostNetwork.Text = "User-Post Network";
            // 
            // chkUserPostLikes
            // 
            this.chkUserPostLikes.AutoSize = true;
            this.chkUserPostLikes.Location = new System.Drawing.Point(163, 19);
            this.chkUserPostLikes.Name = "chkUserPostLikes";
            this.chkUserPostLikes.Size = new System.Drawing.Size(95, 17);
            this.chkUserPostLikes.TabIndex = 6;
            this.chkUserPostLikes.Text = "Based on likes";
            this.chkUserPostLikes.UseVisualStyleBackColor = true;
            this.chkUserPostLikes.CheckedChanged += new System.EventHandler(this.ChkCheckedChanged);
            // 
            // chkUserPostComments
            // 
            this.chkUserPostComments.AutoSize = true;
            this.chkUserPostComments.Location = new System.Drawing.Point(163, 42);
            this.chkUserPostComments.Name = "chkUserPostComments";
            this.chkUserPostComments.Size = new System.Drawing.Size(122, 17);
            this.chkUserPostComments.TabIndex = 11;
            this.chkUserPostComments.Text = "Based on comments";
            this.chkUserPostComments.UseVisualStyleBackColor = true;
            this.chkUserPostComments.CheckedChanged += new System.EventHandler(this.ChkCheckedChanged);
            // 
            // gbFanPage
            // 
            this.gbFanPage.Controls.Add(this.txtPageUsernameID);
            this.gbFanPage.Controls.Add(this.lblNameID);
            this.gbFanPage.Location = new System.Drawing.Point(15, 84);
            this.gbFanPage.Name = "gbFanPage";
            this.gbFanPage.Size = new System.Drawing.Size(327, 60);
            this.gbFanPage.TabIndex = 11;
            this.gbFanPage.TabStop = false;
            this.gbFanPage.Text = "Fan Page";
            // 
            // txtPageUsernameID
            // 
            this.txtPageUsernameID.Enabled = false;
            this.txtPageUsernameID.Location = new System.Drawing.Point(64, 24);
            this.txtPageUsernameID.Name = "txtPageUsernameID";
            this.txtPageUsernameID.Size = new System.Drawing.Size(253, 20);
            this.txtPageUsernameID.TabIndex = 1;
            this.txtPageUsernameID.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtPageUsernameID_KeyUp);
            // 
            // lblNameID
            // 
            this.lblNameID.AutoSize = true;
            this.lblNameID.Location = new System.Drawing.Point(6, 27);
            this.lblNameID.Name = "lblNameID";
            this.lblNameID.Size = new System.Drawing.Size(54, 13);
            this.lblNameID.TabIndex = 0;
            this.lblNameID.Text = "Name/ID:";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.slStatusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 506);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(736, 22);
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
            this.lblMainText.Location = new System.Drawing.Point(15, 9);
            this.lblMainText.Name = "lblMainText";
            this.lblMainText.Size = new System.Drawing.Size(645, 72);
            this.lblMainText.TabIndex = 3;
            this.lblMainText.Text = resources.GetString("lblMainText.Text");
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(394, 472);
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
            this.btnOK.Location = new System.Drawing.Point(509, 472);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "Download";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // toolTip1
            // 
            this.toolTip1.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.toolTip1.ToolTipTitle = "Unimodal Networks";
            // 
            // toolTip2
            // 
            this.toolTip2.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.toolTip2.ToolTipTitle = "Bi-Modal Networks";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(118, 17);
            this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            // 
            // flpResults
            // 
            this.flpResults.AutoScroll = true;
            this.flpResults.BackColor = System.Drawing.Color.White;
            this.flpResults.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.flpResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpResults.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flpResults.Location = new System.Drawing.Point(0, 0);
            this.flpResults.Name = "flpResults";
            this.flpResults.Size = new System.Drawing.Size(429, 293);
            this.flpResults.TabIndex = 2;
            this.flpResults.WrapContents = false;
            this.flpResults.Enter += new System.EventHandler(this.flpResults_Enter);
            this.flpResults.Leave += new System.EventHandler(this.flpResults_Leave);
            this.flpResults.MouseHover += new System.EventHandler(this.flpResults_MouseHover);
            // 
            // bgLoadResults
            // 
            this.bgLoadResults.WorkerReportsProgress = true;
            this.bgLoadResults.WorkerSupportsCancellation = true;
            this.bgLoadResults.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgLoadResults_DoWork);
            this.bgLoadResults.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bgLoadResults_ProgressChanged);
            // 
            // pnResults
            // 
            this.pnResults.Controls.Add(this.piLoading);
            this.pnResults.Controls.Add(this.flpResults);
            this.pnResults.Location = new System.Drawing.Point(79, 130);
            this.pnResults.Name = "pnResults";
            this.pnResults.Size = new System.Drawing.Size(429, 293);
            this.pnResults.TabIndex = 18;
            this.pnResults.Visible = false;
            // 
            // piLoading
            // 
            this.piLoading.AnimationSpeed = 90;
            this.piLoading.BackColor = System.Drawing.Color.White;
            this.piLoading.CircleColor = System.Drawing.Color.SteelBlue;
            this.piLoading.CircleSize = 0.7F;
            this.piLoading.Location = new System.Drawing.Point(174, 103);
            this.piLoading.Name = "piLoading";
            this.piLoading.NumberOfCircles = 90;
            this.piLoading.NumberOfVisibleCircles = 90;
            this.piLoading.Percentage = 0F;
            this.piLoading.Size = new System.Drawing.Size(90, 90);
            this.piLoading.TabIndex = 3;
            this.piLoading.Text = "progressIndicator1";
            // 
            // FacebookFanPageDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(736, 528);
            this.Controls.Add(this.pnResults);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.gbOptions);
            this.Controls.Add(this.grpAttributes);
            this.Controls.Add(this.gbNetwork);
            this.Controls.Add(this.gbFanPage);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.lblMainText);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FacebookFanPageDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Facebook Fan Page Network (v.1.9.2)";
            this.Load += new System.EventHandler(this.FacebookFanPageDialog_Load);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.FacebookFanPageDialog_MouseClick);
            this.gbOptions.ResumeLayout(false);
            this.gbOptions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nuToPost)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudLimit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudFromPost)).EndInit();
            this.grpAttributes.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgAttributes)).EndInit();
            this.gbNetwork.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.gbFanPage.ResumeLayout(false);
            this.gbFanPage.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.pnResults.ResumeLayout(false);
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
        private System.Windows.Forms.GroupBox gbFanPage;
        public System.Windows.Forms.TextBox txtPageUsernameID;
        private System.Windows.Forms.Label lblNameID;
        private System.Windows.Forms.GroupBox gbNetwork;
        private System.Windows.Forms.Label lblToPost;
        public System.Windows.Forms.NumericUpDown nudFromPost;
        public System.Windows.Forms.DataGridView dgAttributes;
        private System.Windows.Forms.DataGridViewTextBoxColumn attributeColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn includeColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn attributeValueColumn;
        private System.Windows.Forms.GroupBox grpAttributes;
        public System.Windows.Forms.CheckBox chkSelectAll;
        public System.Windows.Forms.CheckBox chkWallPosts;
        public System.Windows.Forms.CheckBox chkStatusUpdates;
        private System.Windows.Forms.GroupBox gbOptions;
        private System.Windows.Forms.LinkLabel linkLabel1;
        public System.Windows.Forms.DateTimePicker dtStartDate;
        public System.Windows.Forms.RadioButton rbDateDownload;
        public System.Windows.Forms.RadioButton rbDownloadFromPost;
        public System.Windows.Forms.CheckBox chkUserPostLikes;
        private System.Windows.Forms.GroupBox groupBox2;
        public System.Windows.Forms.CheckBox chkPostPostComments;
        private System.Windows.Forms.Label lblPostPostNetwork;
        private System.Windows.Forms.Label lblUseUserNetwork;
        public System.Windows.Forms.CheckBox chkPostPostLikes;
        public System.Windows.Forms.CheckBox chkUserUserLikes;
        public System.Windows.Forms.CheckBox chkUserUserComments;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblUserPostNetwork;
        public System.Windows.Forms.CheckBox chkUserPostComments;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ToolTip toolTip2;
        private System.Windows.Forms.CheckBox chkIncludeOthers;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.Label lblAnd;
        public System.Windows.Forms.DateTimePicker dtEndDate;
        private System.Windows.Forms.FlowLayoutPanel flpResults;
        private System.ComponentModel.BackgroundWorker bgLoadResults;
        private System.Windows.Forms.Panel pnResults;
        private ProgressIndicator piLoading;
        private System.Windows.Forms.CheckBox chkLimit;
        private System.Windows.Forms.NumericUpDown nudLimit;
        public System.Windows.Forms.NumericUpDown nuToPost;
    }
}