namespace Smrf.NodeXL.TestGraphDataProviders
{
    partial class MainForm
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
            this.wbWebBrowser = new System.Windows.Forms.WebBrowser();
            this.btnFlickrRelatedTags = new System.Windows.Forms.Button();
            this.btnTwitterUser = new System.Windows.Forms.Button();
            this.btnTwitterSearch = new System.Windows.Forms.Button();
            this.btnYouTubeUsers = new System.Windows.Forms.Button();
            this.btnYouTubeVideos = new System.Windows.Forms.Button();
            this.btnFlickrUsers = new System.Windows.Forms.Button();
            this.btnTwitterList = new System.Windows.Forms.Button();
            this.btnGraphServer = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // wbWebBrowser
            // 
            this.wbWebBrowser.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.wbWebBrowser.Location = new System.Drawing.Point(12, 113);
            this.wbWebBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.wbWebBrowser.Name = "wbWebBrowser";
            this.wbWebBrowser.Size = new System.Drawing.Size(530, 258);
            this.wbWebBrowser.TabIndex = 8;
            // 
            // btnFlickrRelatedTags
            // 
            this.btnFlickrRelatedTags.Location = new System.Drawing.Point(282, 41);
            this.btnFlickrRelatedTags.Name = "btnFlickrRelatedTags";
            this.btnFlickrRelatedTags.Size = new System.Drawing.Size(120, 23);
            this.btnFlickrRelatedTags.TabIndex = 6;
            this.btnFlickrRelatedTags.Text = "Flickr Related Tags";
            this.btnFlickrRelatedTags.UseVisualStyleBackColor = true;
            this.btnFlickrRelatedTags.Click += new System.EventHandler(this.btnFlickrRelatedTags_Click);
            // 
            // btnTwitterUser
            // 
            this.btnTwitterUser.Location = new System.Drawing.Point(12, 12);
            this.btnTwitterUser.Name = "btnTwitterUser";
            this.btnTwitterUser.Size = new System.Drawing.Size(120, 23);
            this.btnTwitterUser.TabIndex = 0;
            this.btnTwitterUser.Text = "Twitter User";
            this.btnTwitterUser.UseVisualStyleBackColor = true;
            this.btnTwitterUser.Click += new System.EventHandler(this.btnTwitterUser_Click);
            // 
            // btnTwitterSearch
            // 
            this.btnTwitterSearch.Location = new System.Drawing.Point(12, 41);
            this.btnTwitterSearch.Name = "btnTwitterSearch";
            this.btnTwitterSearch.Size = new System.Drawing.Size(120, 23);
            this.btnTwitterSearch.TabIndex = 1;
            this.btnTwitterSearch.Text = "Twitter Search";
            this.btnTwitterSearch.UseVisualStyleBackColor = true;
            this.btnTwitterSearch.Click += new System.EventHandler(this.btnTwitterSearch_Click);
            // 
            // btnYouTubeUsers
            // 
            this.btnYouTubeUsers.Location = new System.Drawing.Point(147, 12);
            this.btnYouTubeUsers.Name = "btnYouTubeUsers";
            this.btnYouTubeUsers.Size = new System.Drawing.Size(120, 23);
            this.btnYouTubeUsers.TabIndex = 3;
            this.btnYouTubeUsers.Text = "YouTube Users";
            this.btnYouTubeUsers.UseVisualStyleBackColor = true;
            this.btnYouTubeUsers.Click += new System.EventHandler(this.btnYouTubeUsers_Click);
            // 
            // btnYouTubeVideos
            // 
            this.btnYouTubeVideos.Location = new System.Drawing.Point(147, 41);
            this.btnYouTubeVideos.Name = "btnYouTubeVideos";
            this.btnYouTubeVideos.Size = new System.Drawing.Size(120, 23);
            this.btnYouTubeVideos.TabIndex = 4;
            this.btnYouTubeVideos.Text = "YouTube Videos";
            this.btnYouTubeVideos.UseVisualStyleBackColor = true;
            this.btnYouTubeVideos.Click += new System.EventHandler(this.btnYouTubeVideos_Click);
            // 
            // btnFlickrUsers
            // 
            this.btnFlickrUsers.Location = new System.Drawing.Point(282, 12);
            this.btnFlickrUsers.Name = "btnFlickrUsers";
            this.btnFlickrUsers.Size = new System.Drawing.Size(120, 23);
            this.btnFlickrUsers.TabIndex = 5;
            this.btnFlickrUsers.Text = "Flickr Users";
            this.btnFlickrUsers.UseVisualStyleBackColor = true;
            this.btnFlickrUsers.Click += new System.EventHandler(this.btnFlickrUsers_Click);
            // 
            // btnTwitterList
            // 
            this.btnTwitterList.Location = new System.Drawing.Point(12, 70);
            this.btnTwitterList.Name = "btnTwitterList";
            this.btnTwitterList.Size = new System.Drawing.Size(120, 23);
            this.btnTwitterList.TabIndex = 2;
            this.btnTwitterList.Text = "Twitter List";
            this.btnTwitterList.UseVisualStyleBackColor = true;
            this.btnTwitterList.Click += new System.EventHandler(this.btnTwitterList_Click);
            // 
            // btnGraphServer
            // 
            this.btnGraphServer.Location = new System.Drawing.Point(422, 12);
            this.btnGraphServer.Name = "btnGraphServer";
            this.btnGraphServer.Size = new System.Drawing.Size(120, 23);
            this.btnGraphServer.TabIndex = 7;
            this.btnGraphServer.Text = "Graph Server";
            this.btnGraphServer.UseVisualStyleBackColor = true;
            this.btnGraphServer.Click += new System.EventHandler(this.btnGraphServer_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(553, 394);
            this.Controls.Add(this.btnGraphServer);
            this.Controls.Add(this.btnTwitterList);
            this.Controls.Add(this.btnFlickrUsers);
            this.Controls.Add(this.btnYouTubeVideos);
            this.Controls.Add(this.btnYouTubeUsers);
            this.Controls.Add(this.btnTwitterSearch);
            this.Controls.Add(this.btnTwitterUser);
            this.Controls.Add(this.btnFlickrRelatedTags);
            this.Controls.Add(this.wbWebBrowser);
            this.Name = "MainForm";
            this.Text = "Test Graph Data Provider Plug-Ins";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.WebBrowser wbWebBrowser;
        private System.Windows.Forms.Button btnFlickrRelatedTags;
        private System.Windows.Forms.Button btnTwitterUser;
        private System.Windows.Forms.Button btnTwitterSearch;
        private System.Windows.Forms.Button btnYouTubeUsers;
        private System.Windows.Forms.Button btnYouTubeVideos;
        private System.Windows.Forms.Button btnFlickrUsers;
        private System.Windows.Forms.Button btnTwitterList;
        private System.Windows.Forms.Button btnGraphServer;
    }
}

