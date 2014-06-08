namespace StandaloneNode
{
    partial class SearchForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SearchForm));
            this.keywordBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.searchPostCheckBox = new System.Windows.Forms.CheckBox();
            this.searchCommentCheckBox = new System.Windows.Forms.CheckBox();
            this.searchButton = new System.Windows.Forms.Button();
            this.levelOneList = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader9 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader8 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.commentsList = new System.Windows.Forms.ListView();
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.detailMessage = new System.Windows.Forms.TextBox();
            this.filterWithTimeCheck = new System.Windows.Forms.CheckBox();
            this.filterStartDate = new System.Windows.Forms.DateTimePicker();
            this.filterEndDate = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.counterLabel = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.page_list = new System.Windows.Forms.ComboBox();
            this.single_page = new System.Windows.Forms.CheckBox();
            this.DisaplayMessage = new System.Windows.Forms.Button();
            this.talking_msg_group = new System.Windows.Forms.GroupBox();
            this.checkedCountLabel = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.createGroup = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.nameOfGroup = new System.Windows.Forms.TextBox();
            this.selectAllNameBox = new System.Windows.Forms.CheckBox();
            this.nameListBox = new System.Windows.Forms.CheckedListBox();
            this.groupBox1.SuspendLayout();
            this.talking_msg_group.SuspendLayout();
            this.SuspendLayout();
            // 
            // keywordBox
            // 
            this.keywordBox.Location = new System.Drawing.Point(61, 12);
            this.keywordBox.Name = "keywordBox";
            this.keywordBox.Size = new System.Drawing.Size(164, 22);
            this.keywordBox.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "關鍵詞";
            // 
            // searchPostCheckBox
            // 
            this.searchPostCheckBox.AutoSize = true;
            this.searchPostCheckBox.Checked = true;
            this.searchPostCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.searchPostCheckBox.Location = new System.Drawing.Point(14, 21);
            this.searchPostCheckBox.Name = "searchPostCheckBox";
            this.searchPostCheckBox.Size = new System.Drawing.Size(67, 16);
            this.searchPostCheckBox.TabIndex = 2;
            this.searchPostCheckBox.Text = "Post文章";
            this.searchPostCheckBox.UseVisualStyleBackColor = true;
            this.searchPostCheckBox.CheckedChanged += new System.EventHandler(this.searchPostCheckBox_CheckedChanged);
            // 
            // searchCommentCheckBox
            // 
            this.searchCommentCheckBox.AutoSize = true;
            this.searchCommentCheckBox.Checked = true;
            this.searchCommentCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.searchCommentCheckBox.Location = new System.Drawing.Point(99, 21);
            this.searchCommentCheckBox.Name = "searchCommentCheckBox";
            this.searchCommentCheckBox.Size = new System.Drawing.Size(94, 16);
            this.searchCommentCheckBox.TabIndex = 3;
            this.searchCommentCheckBox.Text = "Comment文章";
            this.searchCommentCheckBox.UseVisualStyleBackColor = true;
            this.searchCommentCheckBox.CheckedChanged += new System.EventHandler(this.searchCommentCheckBox_CheckedChanged);
            // 
            // searchButton
            // 
            this.searchButton.Location = new System.Drawing.Point(14, 46);
            this.searchButton.Name = "searchButton";
            this.searchButton.Size = new System.Drawing.Size(211, 38);
            this.searchButton.TabIndex = 4;
            this.searchButton.Text = "查詢";
            this.searchButton.UseVisualStyleBackColor = true;
            this.searchButton.Click += new System.EventHandler(this.searchButton_Click);
            // 
            // levelOneList
            // 
            this.levelOneList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader7,
            this.columnHeader9,
            this.columnHeader8,
            this.columnHeader5});
            this.levelOneList.FullRowSelect = true;
            this.levelOneList.Location = new System.Drawing.Point(12, 101);
            this.levelOneList.MultiSelect = false;
            this.levelOneList.Name = "levelOneList";
            this.levelOneList.Size = new System.Drawing.Size(583, 244);
            this.levelOneList.TabIndex = 5;
            this.levelOneList.UseCompatibleStateImageBehavior = false;
            this.levelOneList.View = System.Windows.Forms.View.Details;
            this.levelOneList.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.levelOneList_ColumnClick);
            this.levelOneList.ColumnWidthChanging += new System.Windows.Forms.ColumnWidthChangingEventHandler(this.levelOneList_ColumnWidthChanging);
            this.levelOneList.SelectedIndexChanged += new System.EventHandler(this.levelOneList_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "粉絲頁";
            this.columnHeader1.Width = 124;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "類型";
            this.columnHeader2.Width = 63;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "訊息內容";
            this.columnHeader3.Width = 167;
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "日期";
            this.columnHeader7.Width = 100;
            // 
            // columnHeader9
            // 
            this.columnHeader9.Text = "按讚數";
            this.columnHeader9.Width = 51;
            // 
            // columnHeader8
            // 
            this.columnHeader8.Text = "回應數";
            this.columnHeader8.Width = 51;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "ID";
            this.columnHeader5.Width = 0;
            // 
            // commentsList
            // 
            this.commentsList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader4,
            this.columnHeader6});
            this.commentsList.Enabled = false;
            this.commentsList.FullRowSelect = true;
            this.commentsList.Location = new System.Drawing.Point(601, 101);
            this.commentsList.MultiSelect = false;
            this.commentsList.Name = "commentsList";
            this.commentsList.Size = new System.Drawing.Size(201, 244);
            this.commentsList.TabIndex = 5;
            this.commentsList.UseCompatibleStateImageBehavior = false;
            this.commentsList.View = System.Windows.Forms.View.Details;
            this.commentsList.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.commentsList_ColumnClick);
            this.commentsList.ColumnWidthChanging += new System.Windows.Forms.ColumnWidthChangingEventHandler(this.commentsList_ColumnWidthChanging);
            this.commentsList.SelectedIndexChanged += new System.EventHandler(this.commentsList_SelectedIndexChanged);
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "回覆的訊息內容";
            this.columnHeader4.Width = 176;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "ID";
            this.columnHeader6.Width = 0;
            // 
            // detailMessage
            // 
            this.detailMessage.Location = new System.Drawing.Point(12, 351);
            this.detailMessage.Multiline = true;
            this.detailMessage.Name = "detailMessage";
            this.detailMessage.ReadOnly = true;
            this.detailMessage.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.detailMessage.Size = new System.Drawing.Size(790, 193);
            this.detailMessage.TabIndex = 6;
            // 
            // filterWithTimeCheck
            // 
            this.filterWithTimeCheck.AutoSize = true;
            this.filterWithTimeCheck.Location = new System.Drawing.Point(14, 47);
            this.filterWithTimeCheck.Name = "filterWithTimeCheck";
            this.filterWithTimeCheck.Size = new System.Drawing.Size(72, 16);
            this.filterWithTimeCheck.TabIndex = 7;
            this.filterWithTimeCheck.Text = "過瀘時間";
            this.filterWithTimeCheck.UseVisualStyleBackColor = true;
            this.filterWithTimeCheck.CheckedChanged += new System.EventHandler(this.filterWithTimeCheck_CheckedChanged);
            // 
            // filterStartDate
            // 
            this.filterStartDate.Enabled = false;
            this.filterStartDate.Location = new System.Drawing.Point(99, 43);
            this.filterStartDate.Name = "filterStartDate";
            this.filterStartDate.Size = new System.Drawing.Size(117, 22);
            this.filterStartDate.TabIndex = 8;
            this.filterStartDate.CloseUp += new System.EventHandler(this.filterStartDate_CloseUp);
            // 
            // filterEndDate
            // 
            this.filterEndDate.Enabled = false;
            this.filterEndDate.Location = new System.Drawing.Point(254, 43);
            this.filterEndDate.Name = "filterEndDate";
            this.filterEndDate.Size = new System.Drawing.Size(115, 22);
            this.filterEndDate.TabIndex = 9;
            this.filterEndDate.CloseUp += new System.EventHandler(this.filterEndDate_CloseUp);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(227, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(17, 12);
            this.label2.TabIndex = 10;
            this.label2.Text = "～";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(449, 86);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 11;
            this.label3.Text = "一共有";
            // 
            // counterLabel
            // 
            this.counterLabel.AutoSize = true;
            this.counterLabel.Location = new System.Drawing.Point(501, 86);
            this.counterLabel.Name = "counterLabel";
            this.counterLabel.Size = new System.Drawing.Size(47, 12);
            this.counterLabel.TabIndex = 12;
            this.counterLabel.Text = "0則訊息";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.page_list);
            this.groupBox1.Controls.Add(this.single_page);
            this.groupBox1.Controls.Add(this.DisaplayMessage);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.filterEndDate);
            this.groupBox1.Controls.Add(this.filterStartDate);
            this.groupBox1.Controls.Add(this.filterWithTimeCheck);
            this.groupBox1.Controls.Add(this.searchCommentCheckBox);
            this.groupBox1.Controls.Add(this.searchPostCheckBox);
            this.groupBox1.Location = new System.Drawing.Point(255, 8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(547, 71);
            this.groupBox1.TabIndex = 13;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "資料顯示設定";
            // 
            // page_list
            // 
            this.page_list.Enabled = false;
            this.page_list.FormattingEnabled = true;
            this.page_list.Location = new System.Drawing.Point(254, 17);
            this.page_list.Name = "page_list";
            this.page_list.Size = new System.Drawing.Size(171, 20);
            this.page_list.TabIndex = 13;
            // 
            // single_page
            // 
            this.single_page.AutoSize = true;
            this.single_page.Location = new System.Drawing.Point(229, 21);
            this.single_page.Name = "single_page";
            this.single_page.Size = new System.Drawing.Size(15, 14);
            this.single_page.TabIndex = 12;
            this.single_page.UseVisualStyleBackColor = true;
            this.single_page.CheckedChanged += new System.EventHandler(this.single_page_CheckedChanged);
            // 
            // DisaplayMessage
            // 
            this.DisaplayMessage.Location = new System.Drawing.Point(431, 21);
            this.DisaplayMessage.Name = "DisaplayMessage";
            this.DisaplayMessage.Size = new System.Drawing.Size(101, 37);
            this.DisaplayMessage.TabIndex = 11;
            this.DisaplayMessage.Text = "更新顯示資料";
            this.DisaplayMessage.UseVisualStyleBackColor = true;
            this.DisaplayMessage.Click += new System.EventHandler(this.DisaplayMessage_Click);
            // 
            // talking_msg_group
            // 
            this.talking_msg_group.Controls.Add(this.checkedCountLabel);
            this.talking_msg_group.Controls.Add(this.label5);
            this.talking_msg_group.Controls.Add(this.createGroup);
            this.talking_msg_group.Controls.Add(this.label4);
            this.talking_msg_group.Controls.Add(this.nameOfGroup);
            this.talking_msg_group.Controls.Add(this.selectAllNameBox);
            this.talking_msg_group.Controls.Add(this.nameListBox);
            this.talking_msg_group.Location = new System.Drawing.Point(815, 15);
            this.talking_msg_group.Name = "talking_msg_group";
            this.talking_msg_group.Size = new System.Drawing.Size(272, 529);
            this.talking_msg_group.TabIndex = 14;
            this.talking_msg_group.TabStop = false;
            this.talking_msg_group.Text = "聊天訊息群組";
            // 
            // checkedCountLabel
            // 
            this.checkedCountLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkedCountLabel.Location = new System.Drawing.Point(6, 502);
            this.checkedCountLabel.Name = "checkedCountLabel";
            this.checkedCountLabel.Size = new System.Drawing.Size(52, 12);
            this.checkedCountLabel.TabIndex = 6;
            this.checkedCountLabel.Text = "0";
            this.checkedCountLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(64, 502);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(101, 12);
            this.label5.TabIndex = 5;
            this.label5.Text = "個使用者已被選取";
            // 
            // createGroup
            // 
            this.createGroup.Location = new System.Drawing.Point(217, 18);
            this.createGroup.Name = "createGroup";
            this.createGroup.Size = new System.Drawing.Size(49, 36);
            this.createGroup.TabIndex = 4;
            this.createGroup.Text = "建 立 群 組";
            this.createGroup.UseVisualStyleBackColor = true;
            this.createGroup.Click += new System.EventHandler(this.createGroup_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(152, 29);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(61, 12);
            this.label4.TabIndex = 3;
            this.label4.Text = "(群組名稱)";
            // 
            // nameOfGroup
            // 
            this.nameOfGroup.Location = new System.Drawing.Point(60, 22);
            this.nameOfGroup.Name = "nameOfGroup";
            this.nameOfGroup.Size = new System.Drawing.Size(86, 22);
            this.nameOfGroup.TabIndex = 2;
            // 
            // selectAllNameBox
            // 
            this.selectAllNameBox.AutoSize = true;
            this.selectAllNameBox.Location = new System.Drawing.Point(6, 29);
            this.selectAllNameBox.Name = "selectAllNameBox";
            this.selectAllNameBox.Size = new System.Drawing.Size(48, 16);
            this.selectAllNameBox.TabIndex = 1;
            this.selectAllNameBox.Text = "全選";
            this.selectAllNameBox.UseVisualStyleBackColor = true;
            this.selectAllNameBox.CheckedChanged += new System.EventHandler(this.selectAllNameBox_CheckedChanged);
            // 
            // nameListBox
            // 
            this.nameListBox.FormattingEnabled = true;
            this.nameListBox.Location = new System.Drawing.Point(6, 60);
            this.nameListBox.Name = "nameListBox";
            this.nameListBox.Size = new System.Drawing.Size(260, 429);
            this.nameListBox.TabIndex = 0;
            this.nameListBox.SelectedIndexChanged += new System.EventHandler(this.nameListBox_SelectedIndexChanged);
            // 
            // SearchForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1091, 556);
            this.Controls.Add(this.talking_msg_group);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.counterLabel);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.detailMessage);
            this.Controls.Add(this.commentsList);
            this.Controls.Add(this.levelOneList);
            this.Controls.Add(this.searchButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.keywordBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SearchForm";
            this.Text = "文章內容查詢";
            this.Shown += new System.EventHandler(this.SearchForm_Shown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.talking_msg_group.ResumeLayout(false);
            this.talking_msg_group.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox keywordBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox searchPostCheckBox;
        private System.Windows.Forms.CheckBox searchCommentCheckBox;
        private System.Windows.Forms.Button searchButton;
        private System.Windows.Forms.ListView levelOneList;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ListView commentsList;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.TextBox detailMessage;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.ColumnHeader columnHeader8;
        private System.Windows.Forms.ColumnHeader columnHeader9;
        private System.Windows.Forms.CheckBox filterWithTimeCheck;
        private System.Windows.Forms.DateTimePicker filterStartDate;
        private System.Windows.Forms.DateTimePicker filterEndDate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label counterLabel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button DisaplayMessage;
        private System.Windows.Forms.ComboBox page_list;
        private System.Windows.Forms.CheckBox single_page;
        private System.Windows.Forms.GroupBox talking_msg_group;
        private System.Windows.Forms.Button createGroup;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox nameOfGroup;
        private System.Windows.Forms.CheckBox selectAllNameBox;
        private System.Windows.Forms.CheckedListBox nameListBox;
        private System.Windows.Forms.Label checkedCountLabel;
        private System.Windows.Forms.Label label5;

    }
}