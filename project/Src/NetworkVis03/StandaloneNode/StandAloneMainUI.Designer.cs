namespace StandaloneNode
{
    partial class StandAloneMainUI
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newFacebookDataGetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.facebookCrawlerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.youTuBeUserCrawlerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.youTuBeVideoCrawlerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newNetworkToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadGraphMLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadFromDBToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadNetworkToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button4 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.RTFDebugOut = new System.Windows.Forms.RichTextBox();
            this.cdbOpenModel = new System.Windows.Forms.OpenFileDialog();
            this.cdbOpenGraphML = new System.Windows.Forms.OpenFileDialog();
            this.menuStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1164, 28);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newFacebookDataGetToolStripMenuItem,
            this.newNetworkToolStripMenuItem,
            this.loadNetworkToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(45, 24);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // newFacebookDataGetToolStripMenuItem
            // 
            this.newFacebookDataGetToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.facebookCrawlerToolStripMenuItem,
            this.youTuBeUserCrawlerToolStripMenuItem,
            this.youTuBeVideoCrawlerToolStripMenuItem});
            this.newFacebookDataGetToolStripMenuItem.Name = "newFacebookDataGetToolStripMenuItem";
            this.newFacebookDataGetToolStripMenuItem.Size = new System.Drawing.Size(175, 24);
            this.newFacebookDataGetToolStripMenuItem.Text = "NewDataGet";
            // 
            // facebookCrawlerToolStripMenuItem
            // 
            this.facebookCrawlerToolStripMenuItem.Name = "facebookCrawlerToolStripMenuItem";
            this.facebookCrawlerToolStripMenuItem.Size = new System.Drawing.Size(234, 24);
            this.facebookCrawlerToolStripMenuItem.Text = "FacebookCrawler";
            this.facebookCrawlerToolStripMenuItem.Click += new System.EventHandler(this.facebookCrawlerToolStripMenuItem_Click);
            // 
            // youTuBeUserCrawlerToolStripMenuItem
            // 
            this.youTuBeUserCrawlerToolStripMenuItem.Name = "youTuBeUserCrawlerToolStripMenuItem";
            this.youTuBeUserCrawlerToolStripMenuItem.Size = new System.Drawing.Size(234, 24);
            this.youTuBeUserCrawlerToolStripMenuItem.Text = "YouTuBeUserCrawler";
            this.youTuBeUserCrawlerToolStripMenuItem.Click += new System.EventHandler(this.youTuBeUserCrawlerToolStripMenuItem_Click);
            // 
            // youTuBeVideoCrawlerToolStripMenuItem
            // 
            this.youTuBeVideoCrawlerToolStripMenuItem.Name = "youTuBeVideoCrawlerToolStripMenuItem";
            this.youTuBeVideoCrawlerToolStripMenuItem.Size = new System.Drawing.Size(234, 24);
            this.youTuBeVideoCrawlerToolStripMenuItem.Text = "YouTuBeVideoCrawler";
            this.youTuBeVideoCrawlerToolStripMenuItem.Click += new System.EventHandler(this.youTuBeVideoCrawlerToolStripMenuItem_Click);
            // 
            // newNetworkToolStripMenuItem
            // 
            this.newNetworkToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadFileToolStripMenuItem,
            this.loadGraphMLToolStripMenuItem,
            this.loadFromDBToolStripMenuItem});
            this.newNetworkToolStripMenuItem.Name = "newNetworkToolStripMenuItem";
            this.newNetworkToolStripMenuItem.Size = new System.Drawing.Size(175, 24);
            this.newNetworkToolStripMenuItem.Text = "NewNetwork";
            // 
            // loadFileToolStripMenuItem
            // 
            this.loadFileToolStripMenuItem.Name = "loadFileToolStripMenuItem";
            this.loadFileToolStripMenuItem.Size = new System.Drawing.Size(178, 24);
            this.loadFileToolStripMenuItem.Text = "LoadFile";
            this.loadFileToolStripMenuItem.Click += new System.EventHandler(this.loadFileToolStripMenuItem_Click);
            // 
            // loadGraphMLToolStripMenuItem
            // 
            this.loadGraphMLToolStripMenuItem.Name = "loadGraphMLToolStripMenuItem";
            this.loadGraphMLToolStripMenuItem.Size = new System.Drawing.Size(178, 24);
            this.loadGraphMLToolStripMenuItem.Text = "LoadGraphML";
            this.loadGraphMLToolStripMenuItem.Click += new System.EventHandler(this.loadGraphMLToolStripMenuItem_Click);
            // 
            // loadFromDBToolStripMenuItem
            // 
            this.loadFromDBToolStripMenuItem.Name = "loadFromDBToolStripMenuItem";
            this.loadFromDBToolStripMenuItem.Size = new System.Drawing.Size(178, 24);
            this.loadFromDBToolStripMenuItem.Text = "LoadFromDB";
            this.loadFromDBToolStripMenuItem.Click += new System.EventHandler(this.loadFromDBToolStripMenuItem_Click);
            // 
            // loadNetworkToolStripMenuItem
            // 
            this.loadNetworkToolStripMenuItem.Name = "loadNetworkToolStripMenuItem";
            this.loadNetworkToolStripMenuItem.Size = new System.Drawing.Size(175, 24);
            this.loadNetworkToolStripMenuItem.Text = "LoadNetwork";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(3, 21);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(940, 570);
            this.tabControl1.TabIndex = 2;
            // 
            // tabPage1
            // 
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(932, 541);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "SocialNetwork";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.pictureBox1);
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(932, 541);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "NodeRelations";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Location = new System.Drawing.Point(3, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(926, 535);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button4);
            this.groupBox1.Controls.Add(this.button3);
            this.groupBox1.Controls.Add(this.button2);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Location = new System.Drawing.Point(0, 30);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 591);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "ModelControl";
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(0, 163);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(200, 35);
            this.button4.TabIndex = 3;
            this.button4.Text = "CalculateAll";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(0, 124);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(200, 33);
            this.button3.TabIndex = 2;
            this.button3.Text = "ClusteringCoefficientCalculate";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(0, 85);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(200, 33);
            this.button2.TabIndex = 1;
            this.button2.Text = "PageRankCalculate";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(0, 46);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(200, 33);
            this.button1.TabIndex = 0;
            this.button1.Text = "ShowLabels";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.tabControl1);
            this.groupBox2.Location = new System.Drawing.Point(206, 30);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(946, 594);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "GraphDisplay";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.RTFDebugOut);
            this.groupBox3.Location = new System.Drawing.Point(0, 627);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(1137, 100);
            this.groupBox3.TabIndex = 5;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "DebugInformation";
            // 
            // RTFDebugOut
            // 
            this.RTFDebugOut.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RTFDebugOut.Location = new System.Drawing.Point(3, 21);
            this.RTFDebugOut.Name = "RTFDebugOut";
            this.RTFDebugOut.Size = new System.Drawing.Size(1131, 76);
            this.RTFDebugOut.TabIndex = 0;
            this.RTFDebugOut.Text = "";
            // 
            // cdbOpenGraphML
            // 
            this.cdbOpenGraphML.FileName = "openFileDialog1";
            // 
            // StandAloneMainUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1164, 732);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "StandAloneMainUI";
            this.Text = "StandAloneMainUI";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newFacebookDataGetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem facebookCrawlerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newNetworkToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadNetworkToolStripMenuItem;
        private LayoutControls.LayoutControl layoutControl1;
        private System.Windows.Forms.ToolStripMenuItem loadFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadGraphMLToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadFromDBToolStripMenuItem;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RichTextBox RTFDebugOut;
        private System.Windows.Forms.OpenFileDialog cdbOpenModel;
        private System.Windows.Forms.OpenFileDialog cdbOpenGraphML;
        private System.Windows.Forms.ToolStripMenuItem youTuBeUserCrawlerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem youTuBeVideoCrawlerToolStripMenuItem;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
    }
}

