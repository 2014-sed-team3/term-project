namespace facebook_pages_crawler
{
    partial class Form1
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置 Managed 資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改這個方法的內容。
        ///
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.dbConnInfo = new System.Windows.Forms.GroupBox();
            this.dbConnect = new System.Windows.Forms.Button();
            this.dbTableBox = new System.Windows.Forms.TextBox();
            this.dbPasswordBox = new System.Windows.Forms.TextBox();
            this.dbAcoountBox = new System.Windows.Forms.TextBox();
            this.dbServerBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.accessTokenGroup = new System.Windows.Forms.GroupBox();
            this.SetAccessToken = new System.Windows.Forms.Button();
            this.accessTokenBox = new System.Windows.Forms.TextBox();
            this.pagesEditGroup = new System.Windows.Forms.GroupBox();
            this.addWithManualPageID = new System.Windows.Forms.Button();
            this.mainualPageIDs = new System.Windows.Forms.TextBox();
            this.recentlyDays = new System.Windows.Forms.NumericUpDown();
            this.pagesLogUpdateF = new System.Windows.Forms.NumericUpDown();
            this.searchLimit = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.unselectAllResultPageButton = new System.Windows.Forms.Button();
            this.unselectAllPageButton = new System.Windows.Forms.Button();
            this.selectAllResultPageButton = new System.Windows.Forms.Button();
            this.selectAllPageButton = new System.Windows.Forms.Button();
            this.post_comment_search = new System.Windows.Forms.Button();
            this.cancleUpdate = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.searchResultList = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.searchButton = new System.Windows.Forms.Button();
            this.searchKeyword = new System.Windows.Forms.TextBox();
            this.reflash = new System.Windows.Forms.Button();
            this.pagesInfo = new System.Windows.Forms.ListView();
            this.pageID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.pageName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.newPageID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.timeSpent = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.newPageIDSinceCheck = new System.Windows.Forms.CheckBox();
            this.updateSinceDate = new System.Windows.Forms.DateTimePicker();
            this.startUpdate = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.updatePageLogsCheck = new System.Windows.Forms.CheckBox();
            this.showMessage = new System.Windows.Forms.TextBox();
            this.deletePageID = new System.Windows.Forms.Button();
            this.insertPageID = new System.Windows.Forms.Button();
            this.userInfoTimer = new System.Windows.Forms.Timer(this.components);
            this.showErrorMessageCheck = new System.Windows.Forms.CheckBox();
            this.dbConnInfo.SuspendLayout();
            this.accessTokenGroup.SuspendLayout();
            this.pagesEditGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.recentlyDays)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pagesLogUpdateF)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchLimit)).BeginInit();
            this.SuspendLayout();
            // 
            // dbConnInfo
            // 
            this.dbConnInfo.Controls.Add(this.dbConnect);
            this.dbConnInfo.Controls.Add(this.dbTableBox);
            this.dbConnInfo.Controls.Add(this.dbPasswordBox);
            this.dbConnInfo.Controls.Add(this.dbAcoountBox);
            this.dbConnInfo.Controls.Add(this.dbServerBox);
            this.dbConnInfo.Controls.Add(this.label4);
            this.dbConnInfo.Controls.Add(this.label3);
            this.dbConnInfo.Controls.Add(this.label2);
            this.dbConnInfo.Controls.Add(this.label1);
            this.dbConnInfo.Location = new System.Drawing.Point(13, 13);
            this.dbConnInfo.Margin = new System.Windows.Forms.Padding(2);
            this.dbConnInfo.Name = "dbConnInfo";
            this.dbConnInfo.Padding = new System.Windows.Forms.Padding(2);
            this.dbConnInfo.Size = new System.Drawing.Size(478, 79);
            this.dbConnInfo.TabIndex = 0;
            this.dbConnInfo.TabStop = false;
            this.dbConnInfo.Text = "資料庫連線設定";
            // 
            // dbConnect
            // 
            this.dbConnect.Location = new System.Drawing.Point(396, 31);
            this.dbConnect.Margin = new System.Windows.Forms.Padding(2);
            this.dbConnect.Name = "dbConnect";
            this.dbConnect.Size = new System.Drawing.Size(74, 29);
            this.dbConnect.TabIndex = 8;
            this.dbConnect.Text = "資料庫連線";
            this.dbConnect.UseVisualStyleBackColor = true;
            this.dbConnect.Click += new System.EventHandler(this.dbConnect_Click);
            // 
            // dbTableBox
            // 
            this.dbTableBox.Location = new System.Drawing.Point(264, 47);
            this.dbTableBox.Margin = new System.Windows.Forms.Padding(2);
            this.dbTableBox.Name = "dbTableBox";
            this.dbTableBox.Size = new System.Drawing.Size(128, 22);
            this.dbTableBox.TabIndex = 7;
            this.dbTableBox.Text = "gamer";
            // 
            // dbPasswordBox
            // 
            this.dbPasswordBox.Location = new System.Drawing.Point(264, 21);
            this.dbPasswordBox.Margin = new System.Windows.Forms.Padding(2);
            this.dbPasswordBox.Name = "dbPasswordBox";
            this.dbPasswordBox.PasswordChar = '*';
            this.dbPasswordBox.Size = new System.Drawing.Size(128, 22);
            this.dbPasswordBox.TabIndex = 6;
            this.dbPasswordBox.Text = "111platform!";
            // 
            // dbAcoountBox
            // 
            this.dbAcoountBox.Location = new System.Drawing.Point(51, 46);
            this.dbAcoountBox.Margin = new System.Windows.Forms.Padding(2);
            this.dbAcoountBox.Name = "dbAcoountBox";
            this.dbAcoountBox.Size = new System.Drawing.Size(128, 22);
            this.dbAcoountBox.TabIndex = 5;
            this.dbAcoountBox.Text = "root";
            // 
            // dbServerBox
            // 
            this.dbServerBox.Location = new System.Drawing.Point(51, 21);
            this.dbServerBox.Margin = new System.Windows.Forms.Padding(2);
            this.dbServerBox.Name = "dbServerBox";
            this.dbServerBox.Size = new System.Drawing.Size(128, 22);
            this.dbServerBox.TabIndex = 4;
            this.dbServerBox.Text = "127.0.0.1";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(183, 49);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(77, 12);
            this.label4.TabIndex = 3;
            this.label4.Text = "資料庫名稱：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(183, 24);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "密碼：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 48);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "帳號：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 23);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "主機：";
            // 
            // accessTokenGroup
            // 
            this.accessTokenGroup.Controls.Add(this.SetAccessToken);
            this.accessTokenGroup.Controls.Add(this.accessTokenBox);
            this.accessTokenGroup.Enabled = false;
            this.accessTokenGroup.Location = new System.Drawing.Point(495, 13);
            this.accessTokenGroup.Margin = new System.Windows.Forms.Padding(2);
            this.accessTokenGroup.Name = "accessTokenGroup";
            this.accessTokenGroup.Padding = new System.Windows.Forms.Padding(2);
            this.accessTokenGroup.Size = new System.Drawing.Size(317, 79);
            this.accessTokenGroup.TabIndex = 1;
            this.accessTokenGroup.TabStop = false;
            this.accessTokenGroup.Text = "Access Token 設定";
            // 
            // SetAccessToken
            // 
            this.SetAccessToken.Location = new System.Drawing.Point(4, 43);
            this.SetAccessToken.Margin = new System.Windows.Forms.Padding(2);
            this.SetAccessToken.Name = "SetAccessToken";
            this.SetAccessToken.Size = new System.Drawing.Size(132, 32);
            this.SetAccessToken.TabIndex = 1;
            this.SetAccessToken.Text = "設定/修改 Access Token";
            this.SetAccessToken.UseVisualStyleBackColor = true;
            this.SetAccessToken.Click += new System.EventHandler(this.SetAccessToken_Click);
            // 
            // accessTokenBox
            // 
            this.accessTokenBox.Location = new System.Drawing.Point(4, 18);
            this.accessTokenBox.Margin = new System.Windows.Forms.Padding(2);
            this.accessTokenBox.Name = "accessTokenBox";
            this.accessTokenBox.Size = new System.Drawing.Size(309, 22);
            this.accessTokenBox.TabIndex = 0;
            // 
            // pagesEditGroup
            // 
            this.pagesEditGroup.Controls.Add(this.showErrorMessageCheck);
            this.pagesEditGroup.Controls.Add(this.addWithManualPageID);
            this.pagesEditGroup.Controls.Add(this.mainualPageIDs);
            this.pagesEditGroup.Controls.Add(this.recentlyDays);
            this.pagesEditGroup.Controls.Add(this.pagesLogUpdateF);
            this.pagesEditGroup.Controls.Add(this.searchLimit);
            this.pagesEditGroup.Controls.Add(this.label9);
            this.pagesEditGroup.Controls.Add(this.unselectAllResultPageButton);
            this.pagesEditGroup.Controls.Add(this.unselectAllPageButton);
            this.pagesEditGroup.Controls.Add(this.selectAllResultPageButton);
            this.pagesEditGroup.Controls.Add(this.selectAllPageButton);
            this.pagesEditGroup.Controls.Add(this.post_comment_search);
            this.pagesEditGroup.Controls.Add(this.cancleUpdate);
            this.pagesEditGroup.Controls.Add(this.label5);
            this.pagesEditGroup.Controls.Add(this.searchResultList);
            this.pagesEditGroup.Controls.Add(this.searchButton);
            this.pagesEditGroup.Controls.Add(this.searchKeyword);
            this.pagesEditGroup.Controls.Add(this.reflash);
            this.pagesEditGroup.Controls.Add(this.pagesInfo);
            this.pagesEditGroup.Controls.Add(this.label8);
            this.pagesEditGroup.Controls.Add(this.label7);
            this.pagesEditGroup.Controls.Add(this.newPageIDSinceCheck);
            this.pagesEditGroup.Controls.Add(this.updateSinceDate);
            this.pagesEditGroup.Controls.Add(this.startUpdate);
            this.pagesEditGroup.Controls.Add(this.label6);
            this.pagesEditGroup.Controls.Add(this.updatePageLogsCheck);
            this.pagesEditGroup.Controls.Add(this.showMessage);
            this.pagesEditGroup.Controls.Add(this.deletePageID);
            this.pagesEditGroup.Controls.Add(this.insertPageID);
            this.pagesEditGroup.Enabled = false;
            this.pagesEditGroup.Location = new System.Drawing.Point(13, 96);
            this.pagesEditGroup.Margin = new System.Windows.Forms.Padding(2);
            this.pagesEditGroup.Name = "pagesEditGroup";
            this.pagesEditGroup.Padding = new System.Windows.Forms.Padding(2);
            this.pagesEditGroup.Size = new System.Drawing.Size(799, 547);
            this.pagesEditGroup.TabIndex = 2;
            this.pagesEditGroup.TabStop = false;
            this.pagesEditGroup.Text = "粉絲頁擷取";
            // 
            // addWithManualPageID
            // 
            this.addWithManualPageID.Location = new System.Drawing.Point(659, 168);
            this.addWithManualPageID.Name = "addWithManualPageID";
            this.addWithManualPageID.Size = new System.Drawing.Size(88, 22);
            this.addWithManualPageID.TabIndex = 54;
            this.addWithManualPageID.Text = "加入粉絲頁ID";
            this.addWithManualPageID.UseVisualStyleBackColor = true;
            this.addWithManualPageID.Click += new System.EventHandler(this.addWithManualPageID_Click);
            // 
            // mainualPageIDs
            // 
            this.mainualPageIDs.Location = new System.Drawing.Point(146, 168);
            this.mainualPageIDs.Name = "mainualPageIDs";
            this.mainualPageIDs.Size = new System.Drawing.Size(508, 22);
            this.mainualPageIDs.TabIndex = 53;
            // 
            // recentlyDays
            // 
            this.recentlyDays.Location = new System.Drawing.Point(330, 407);
            this.recentlyDays.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.recentlyDays.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.recentlyDays.Name = "recentlyDays";
            this.recentlyDays.Size = new System.Drawing.Size(42, 22);
            this.recentlyDays.TabIndex = 52;
            this.recentlyDays.Value = new decimal(new int[] {
            7,
            0,
            0,
            0});
            // 
            // pagesLogUpdateF
            // 
            this.pagesLogUpdateF.Enabled = false;
            this.pagesLogUpdateF.Location = new System.Drawing.Point(128, 407);
            this.pagesLogUpdateF.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.pagesLogUpdateF.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.pagesLogUpdateF.Name = "pagesLogUpdateF";
            this.pagesLogUpdateF.Size = new System.Drawing.Size(43, 22);
            this.pagesLogUpdateF.TabIndex = 51;
            this.pagesLogUpdateF.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // searchLimit
            // 
            this.searchLimit.Location = new System.Drawing.Point(231, 16);
            this.searchLimit.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.searchLimit.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.searchLimit.Name = "searchLimit";
            this.searchLimit.Size = new System.Drawing.Size(54, 22);
            this.searchLimit.TabIndex = 50;
            this.searchLimit.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(18, 22);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(53, 12);
            this.label9.TabIndex = 49;
            this.label9.Text = "搜尋詞：";
            // 
            // unselectAllResultPageButton
            // 
            this.unselectAllResultPageButton.Location = new System.Drawing.Point(706, 45);
            this.unselectAllResultPageButton.Name = "unselectAllResultPageButton";
            this.unselectAllResultPageButton.Size = new System.Drawing.Size(41, 32);
            this.unselectAllResultPageButton.TabIndex = 48;
            this.unselectAllResultPageButton.Text = "取消全選";
            this.unselectAllResultPageButton.UseVisualStyleBackColor = true;
            this.unselectAllResultPageButton.Click += new System.EventHandler(this.unselectAllResultPageButton_Click);
            // 
            // unselectAllPageButton
            // 
            this.unselectAllPageButton.Location = new System.Drawing.Point(706, 198);
            this.unselectAllPageButton.Name = "unselectAllPageButton";
            this.unselectAllPageButton.Size = new System.Drawing.Size(41, 32);
            this.unselectAllPageButton.TabIndex = 48;
            this.unselectAllPageButton.Text = "取消全選";
            this.unselectAllPageButton.UseVisualStyleBackColor = true;
            this.unselectAllPageButton.Click += new System.EventHandler(this.unselectAllPageButton_Click);
            // 
            // selectAllResultPageButton
            // 
            this.selectAllResultPageButton.Location = new System.Drawing.Point(659, 45);
            this.selectAllResultPageButton.Name = "selectAllResultPageButton";
            this.selectAllResultPageButton.Size = new System.Drawing.Size(41, 32);
            this.selectAllResultPageButton.TabIndex = 48;
            this.selectAllResultPageButton.Text = "全選";
            this.selectAllResultPageButton.UseVisualStyleBackColor = true;
            this.selectAllResultPageButton.Click += new System.EventHandler(this.selectAllResultPageButton_Click);
            // 
            // selectAllPageButton
            // 
            this.selectAllPageButton.Location = new System.Drawing.Point(659, 198);
            this.selectAllPageButton.Name = "selectAllPageButton";
            this.selectAllPageButton.Size = new System.Drawing.Size(41, 32);
            this.selectAllPageButton.TabIndex = 48;
            this.selectAllPageButton.Text = "全選";
            this.selectAllPageButton.UseVisualStyleBackColor = true;
            this.selectAllPageButton.Click += new System.EventHandler(this.selectAllPageButton_Click);
            // 
            // post_comment_search
            // 
            this.post_comment_search.Location = new System.Drawing.Point(659, 323);
            this.post_comment_search.Name = "post_comment_search";
            this.post_comment_search.Size = new System.Drawing.Size(89, 36);
            this.post_comment_search.TabIndex = 47;
            this.post_comment_search.Text = "文章內容查詢";
            this.post_comment_search.UseVisualStyleBackColor = true;
            this.post_comment_search.Click += new System.EventHandler(this.post_comment_search_Click);
            // 
            // cancleUpdate
            // 
            this.cancleUpdate.Enabled = false;
            this.cancleUpdate.Location = new System.Drawing.Point(564, 407);
            this.cancleUpdate.Name = "cancleUpdate";
            this.cancleUpdate.Size = new System.Drawing.Size(115, 29);
            this.cancleUpdate.TabIndex = 46;
            this.cancleUpdate.Text = "取消更新";
            this.cancleUpdate.UseVisualStyleBackColor = true;
            this.cancleUpdate.Click += new System.EventHandler(this.cancleUpdate_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(14, 174);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(113, 12);
            this.label5.TabIndex = 45;
            this.label5.Text = "粉絲頁擷取候選清單";
            // 
            // searchResultList
            // 
            this.searchResultList.CheckBoxes = true;
            this.searchResultList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4});
            this.searchResultList.FullRowSelect = true;
            this.searchResultList.Location = new System.Drawing.Point(13, 45);
            this.searchResultList.Name = "searchResultList";
            this.searchResultList.Size = new System.Drawing.Size(641, 117);
            this.searchResultList.TabIndex = 44;
            this.searchResultList.UseCompatibleStateImageBehavior = false;
            this.searchResultList.View = System.Windows.Forms.View.Details;
            this.searchResultList.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.searchResultList_ColumnClick);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Page ID";
            this.columnHeader1.Width = 150;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Page Name";
            this.columnHeader2.Width = 218;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Page Fans";
            this.columnHeader3.Width = 91;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Category";
            this.columnHeader4.Width = 170;
            // 
            // searchButton
            // 
            this.searchButton.Location = new System.Drawing.Point(291, 15);
            this.searchButton.Name = "searchButton";
            this.searchButton.Size = new System.Drawing.Size(101, 23);
            this.searchButton.TabIndex = 43;
            this.searchButton.Text = "粉絲頁搜尋";
            this.searchButton.UseVisualStyleBackColor = true;
            this.searchButton.Click += new System.EventHandler(this.searchButton_Click);
            // 
            // searchKeyword
            // 
            this.searchKeyword.Location = new System.Drawing.Point(77, 17);
            this.searchKeyword.Name = "searchKeyword";
            this.searchKeyword.Size = new System.Drawing.Size(137, 22);
            this.searchKeyword.TabIndex = 42;
            // 
            // reflash
            // 
            this.reflash.Location = new System.Drawing.Point(659, 282);
            this.reflash.Margin = new System.Windows.Forms.Padding(2);
            this.reflash.Name = "reflash";
            this.reflash.Size = new System.Drawing.Size(89, 36);
            this.reflash.TabIndex = 41;
            this.reflash.Text = "重整清單";
            this.reflash.UseVisualStyleBackColor = true;
            this.reflash.Click += new System.EventHandler(this.reflash_Click);
            // 
            // pagesInfo
            // 
            this.pagesInfo.CheckBoxes = true;
            this.pagesInfo.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.pageID,
            this.pageName,
            this.newPageID,
            this.timeSpent});
            this.pagesInfo.FullRowSelect = true;
            this.pagesInfo.Location = new System.Drawing.Point(13, 196);
            this.pagesInfo.Margin = new System.Windows.Forms.Padding(2);
            this.pagesInfo.MultiSelect = false;
            this.pagesInfo.Name = "pagesInfo";
            this.pagesInfo.Size = new System.Drawing.Size(641, 166);
            this.pagesInfo.TabIndex = 40;
            this.pagesInfo.UseCompatibleStateImageBehavior = false;
            this.pagesInfo.View = System.Windows.Forms.View.Details;
            // 
            // pageID
            // 
            this.pageID.Text = "Page ID";
            this.pageID.Width = 150;
            // 
            // pageName
            // 
            this.pageName.Text = "Page Name";
            this.pageName.Width = 226;
            // 
            // newPageID
            // 
            this.newPageID.Text = "New Page ID";
            this.newPageID.Width = 100;
            // 
            // timeSpent
            // 
            this.timeSpent.Text = "更新花費時間";
            this.timeSpent.Width = 150;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(377, 413);
            this.label8.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(53, 12);
            this.label8.TabIndex = 39;
            this.label8.Text = "天內資訊";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(48, 385);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(109, 12);
            this.label7.TabIndex = 37;
            this.label7.Text = "(未勾選表示全擷取)";
            // 
            // newPageIDSinceCheck
            // 
            this.newPageIDSinceCheck.AutoSize = true;
            this.newPageIDSinceCheck.Location = new System.Drawing.Point(30, 372);
            this.newPageIDSinceCheck.Margin = new System.Windows.Forms.Padding(2);
            this.newPageIDSinceCheck.Name = "newPageIDSinceCheck";
            this.newPageIDSinceCheck.Size = new System.Drawing.Size(133, 16);
            this.newPageIDSinceCheck.TabIndex = 36;
            this.newPageIDSinceCheck.Text = "新Page ID更新起始日";
            this.newPageIDSinceCheck.UseVisualStyleBackColor = true;
            // 
            // updateSinceDate
            // 
            this.updateSinceDate.Location = new System.Drawing.Point(178, 368);
            this.updateSinceDate.Margin = new System.Windows.Forms.Padding(2);
            this.updateSinceDate.Name = "updateSinceDate";
            this.updateSinceDate.Size = new System.Drawing.Size(102, 22);
            this.updateSinceDate.TabIndex = 35;
            // 
            // startUpdate
            // 
            this.startUpdate.Location = new System.Drawing.Point(431, 407);
            this.startUpdate.Margin = new System.Windows.Forms.Padding(2);
            this.startUpdate.Name = "startUpdate";
            this.startUpdate.Size = new System.Drawing.Size(117, 29);
            this.startUpdate.TabIndex = 34;
            this.startUpdate.Text = "開始更新";
            this.startUpdate.UseVisualStyleBackColor = true;
            this.startUpdate.Click += new System.EventHandler(this.startUpdate_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(176, 413);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(149, 12);
            this.label6.TabIndex = 33;
            this.label6.Text = "天更新一次，每次更新最近";
            // 
            // updatePageLogsCheck
            // 
            this.updatePageLogsCheck.AutoSize = true;
            this.updatePageLogsCheck.Location = new System.Drawing.Point(30, 412);
            this.updatePageLogsCheck.Margin = new System.Windows.Forms.Padding(2);
            this.updatePageLogsCheck.Name = "updatePageLogsCheck";
            this.updatePageLogsCheck.Size = new System.Drawing.Size(96, 16);
            this.updatePageLogsCheck.TabIndex = 31;
            this.updatePageLogsCheck.Text = "定期更新頻率";
            this.updatePageLogsCheck.UseVisualStyleBackColor = true;
            this.updatePageLogsCheck.CheckedChanged += new System.EventHandler(this.updatePageLogsCheck_CheckedChanged);
            // 
            // showMessage
            // 
            this.showMessage.Location = new System.Drawing.Point(16, 440);
            this.showMessage.Margin = new System.Windows.Forms.Padding(2);
            this.showMessage.Multiline = true;
            this.showMessage.Name = "showMessage";
            this.showMessage.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.showMessage.Size = new System.Drawing.Size(663, 91);
            this.showMessage.TabIndex = 30;
            // 
            // deletePageID
            // 
            this.deletePageID.Location = new System.Drawing.Point(659, 235);
            this.deletePageID.Margin = new System.Windows.Forms.Padding(2);
            this.deletePageID.Name = "deletePageID";
            this.deletePageID.Size = new System.Drawing.Size(89, 43);
            this.deletePageID.TabIndex = 29;
            this.deletePageID.Text = "從粉絲頁清單刪除粉絲頁";
            this.deletePageID.UseVisualStyleBackColor = true;
            this.deletePageID.Click += new System.EventHandler(this.deletePageID_Click);
            // 
            // insertPageID
            // 
            this.insertPageID.Location = new System.Drawing.Point(658, 82);
            this.insertPageID.Margin = new System.Windows.Forms.Padding(2);
            this.insertPageID.Name = "insertPageID";
            this.insertPageID.Size = new System.Drawing.Size(89, 41);
            this.insertPageID.TabIndex = 28;
            this.insertPageID.Text = "新增粉絲頁至清單";
            this.insertPageID.UseVisualStyleBackColor = true;
            this.insertPageID.Click += new System.EventHandler(this.insertPageID_Click);
            // 
            // userInfoTimer
            // 
            this.userInfoTimer.Tick += new System.EventHandler(this.userInfoTimer_Tick);
            // 
            // showErrorMessageCheck
            // 
            this.showErrorMessageCheck.AutoSize = true;
            this.showErrorMessageCheck.Location = new System.Drawing.Point(684, 442);
            this.showErrorMessageCheck.Name = "showErrorMessageCheck";
            this.showErrorMessageCheck.Size = new System.Drawing.Size(107, 16);
            this.showErrorMessageCheck.TabIndex = 55;
            this.showErrorMessageCheck.Text = "顯示錯誤訊LOG";
            this.showErrorMessageCheck.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(823, 654);
            this.Controls.Add(this.pagesEditGroup);
            this.Controls.Add(this.accessTokenGroup);
            this.Controls.Add(this.dbConnInfo);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Form1";
            this.Text = "粉絲頁資料擷取";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Shown += new System.EventHandler(this.Form1_Shown);
            this.dbConnInfo.ResumeLayout(false);
            this.dbConnInfo.PerformLayout();
            this.accessTokenGroup.ResumeLayout(false);
            this.accessTokenGroup.PerformLayout();
            this.pagesEditGroup.ResumeLayout(false);
            this.pagesEditGroup.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.recentlyDays)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pagesLogUpdateF)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchLimit)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox dbConnInfo;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox dbAcoountBox;
        private System.Windows.Forms.TextBox dbServerBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox dbPasswordBox;
        private System.Windows.Forms.Button dbConnect;
        private System.Windows.Forms.TextBox dbTableBox;
        private System.Windows.Forms.GroupBox accessTokenGroup;
        private System.Windows.Forms.Button SetAccessToken;
        private System.Windows.Forms.TextBox accessTokenBox;
        private System.Windows.Forms.GroupBox pagesEditGroup;
        private System.Windows.Forms.Button deletePageID;
        private System.Windows.Forms.Button insertPageID;
        private System.Windows.Forms.TextBox showMessage;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button startUpdate;
        private System.Windows.Forms.CheckBox newPageIDSinceCheck;
        private System.Windows.Forms.DateTimePicker updateSinceDate;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox updatePageLogsCheck;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ListView pagesInfo;
        private System.Windows.Forms.ColumnHeader pageID;
        private System.Windows.Forms.ColumnHeader pageName;
        private System.Windows.Forms.ColumnHeader newPageID;
        private System.Windows.Forms.Button reflash;
        private System.Windows.Forms.ColumnHeader timeSpent;
        private System.Windows.Forms.Button searchButton;
        private System.Windows.Forms.TextBox searchKeyword;
        private System.Windows.Forms.ListView searchResultList;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button cancleUpdate;
        private System.Windows.Forms.Button post_comment_search;
        private System.Windows.Forms.Button unselectAllPageButton;
        private System.Windows.Forms.Button selectAllPageButton;
        private System.Windows.Forms.Button unselectAllResultPageButton;
        private System.Windows.Forms.Button selectAllResultPageButton;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.NumericUpDown searchLimit;
        private System.Windows.Forms.NumericUpDown recentlyDays;
        private System.Windows.Forms.NumericUpDown pagesLogUpdateF;
        private System.Windows.Forms.Timer userInfoTimer;
        private System.Windows.Forms.Button addWithManualPageID;
        private System.Windows.Forms.TextBox mainualPageIDs;
        private System.Windows.Forms.CheckBox showErrorMessageCheck;
    }
}

