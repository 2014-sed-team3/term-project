namespace WindowsFormsApplication1
{
    partial class Form1
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.importToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fromUciNETToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fromGraphMLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fromGraphMLFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fromPajkeFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.fromFacebookToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fromFacebookPersoanlTimelineToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fromFlickrRelatedTagsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fromFlickrUsersNetworkToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fromTwittersSearchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fromTwitterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fromYoutubeUsersNetworkToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fromYoutubeVideoNetworkToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toUciNETFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toGraphMLFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dataProcessingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.countAndMergeDuplicatesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupByClusterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.graphMetricsComputingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.autofillColumnsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.subgraphImageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resultToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showGraphToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.graphSummaryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectTypesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectLayoutsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dataGridViewofEdge = new System.Windows.Forms.DataGridView();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataManagementToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewofEdge)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.importToolStripMenuItem,
            this.exportToolStripMenuItem,
            this.dataProcessingToolStripMenuItem,
            this.dataManagementToolStripMenuItem,
            this.resultToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1201, 27);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // importToolStripMenuItem
            // 
            this.importToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fromUciNETToolStripMenuItem,
            this.fromGraphMLToolStripMenuItem,
            this.fromGraphMLFilesToolStripMenuItem,
            this.fromPajkeFileToolStripMenuItem,
            this.toolStripMenuItem2,
            this.toolStripMenuItem3,
            this.fromFacebookToolStripMenuItem,
            this.fromFacebookPersoanlTimelineToolStripMenuItem,
            this.fromFlickrRelatedTagsToolStripMenuItem,
            this.fromFlickrUsersNetworkToolStripMenuItem,
            this.fromTwittersSearchToolStripMenuItem,
            this.fromTwitterToolStripMenuItem,
            this.fromYoutubeUsersNetworkToolStripMenuItem,
            this.fromYoutubeVideoNetworkToolStripMenuItem});
            this.importToolStripMenuItem.Name = "importToolStripMenuItem";
            this.importToolStripMenuItem.Size = new System.Drawing.Size(68, 23);
            this.importToolStripMenuItem.Text = "Import";
            // 
            // exportToolStripMenuItem
            // 
            this.exportToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toUciNETFilesToolStripMenuItem,
            this.toGraphMLFileToolStripMenuItem,
            this.toToolStripMenuItem});
            this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            this.exportToolStripMenuItem.Size = new System.Drawing.Size(65, 23);
            this.exportToolStripMenuItem.Text = "Export";
            // 
            // fromUciNETToolStripMenuItem
            // 
            this.fromUciNETToolStripMenuItem.Name = "fromUciNETToolStripMenuItem";
            this.fromUciNETToolStripMenuItem.Size = new System.Drawing.Size(314, 24);
            this.fromUciNETToolStripMenuItem.Text = "From UciNET File";
            // 
            // fromGraphMLToolStripMenuItem
            // 
            this.fromGraphMLToolStripMenuItem.Name = "fromGraphMLToolStripMenuItem";
            this.fromGraphMLToolStripMenuItem.Size = new System.Drawing.Size(314, 24);
            this.fromGraphMLToolStripMenuItem.Text = "From GraphML File";
            // 
            // fromGraphMLFilesToolStripMenuItem
            // 
            this.fromGraphMLFilesToolStripMenuItem.Name = "fromGraphMLFilesToolStripMenuItem";
            this.fromGraphMLFilesToolStripMenuItem.Size = new System.Drawing.Size(314, 24);
            this.fromGraphMLFilesToolStripMenuItem.Text = "From GraphML Files";
            // 
            // fromPajkeFileToolStripMenuItem
            // 
            this.fromPajkeFileToolStripMenuItem.Name = "fromPajkeFileToolStripMenuItem";
            this.fromPajkeFileToolStripMenuItem.Size = new System.Drawing.Size(314, 24);
            this.fromPajkeFileToolStripMenuItem.Text = "From Pajek File";
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(311, 6);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(314, 24);
            this.toolStripMenuItem3.Text = "From Facebook Fans Pages";
            this.toolStripMenuItem3.Click += new System.EventHandler(this.toolStripMenuItem3_Click);
            // 
            // fromFacebookToolStripMenuItem
            // 
            this.fromFacebookToolStripMenuItem.Name = "fromFacebookToolStripMenuItem";
            this.fromFacebookToolStripMenuItem.Size = new System.Drawing.Size(314, 24);
            this.fromFacebookToolStripMenuItem.Text = "From Facebook Group Network";
            // 
            // fromFacebookPersoanlTimelineToolStripMenuItem
            // 
            this.fromFacebookPersoanlTimelineToolStripMenuItem.Name = "fromFacebookPersoanlTimelineToolStripMenuItem";
            this.fromFacebookPersoanlTimelineToolStripMenuItem.Size = new System.Drawing.Size(314, 24);
            this.fromFacebookPersoanlTimelineToolStripMenuItem.Text = "From Facebook Persoanl Timeline";
            // 
            // fromFlickrRelatedTagsToolStripMenuItem
            // 
            this.fromFlickrRelatedTagsToolStripMenuItem.Name = "fromFlickrRelatedTagsToolStripMenuItem";
            this.fromFlickrRelatedTagsToolStripMenuItem.Size = new System.Drawing.Size(314, 24);
            this.fromFlickrRelatedTagsToolStripMenuItem.Text = "From Flickr Related Tags Network";
            // 
            // fromFlickrUsersNetworkToolStripMenuItem
            // 
            this.fromFlickrUsersNetworkToolStripMenuItem.Name = "fromFlickrUsersNetworkToolStripMenuItem";
            this.fromFlickrUsersNetworkToolStripMenuItem.Size = new System.Drawing.Size(314, 24);
            this.fromFlickrUsersNetworkToolStripMenuItem.Text = "From Flickr User\'s Network";
            // 
            // fromTwittersSearchToolStripMenuItem
            // 
            this.fromTwittersSearchToolStripMenuItem.Name = "fromTwittersSearchToolStripMenuItem";
            this.fromTwittersSearchToolStripMenuItem.Size = new System.Drawing.Size(314, 24);
            this.fromTwittersSearchToolStripMenuItem.Text = "From Twitter Search Network";
            // 
            // fromTwitterToolStripMenuItem
            // 
            this.fromTwitterToolStripMenuItem.Name = "fromTwitterToolStripMenuItem";
            this.fromTwitterToolStripMenuItem.Size = new System.Drawing.Size(314, 24);
            this.fromTwitterToolStripMenuItem.Text = "From Twitter User\'s Network";
            // 
            // fromYoutubeUsersNetworkToolStripMenuItem
            // 
            this.fromYoutubeUsersNetworkToolStripMenuItem.Name = "fromYoutubeUsersNetworkToolStripMenuItem";
            this.fromYoutubeUsersNetworkToolStripMenuItem.Size = new System.Drawing.Size(314, 24);
            this.fromYoutubeUsersNetworkToolStripMenuItem.Text = "From Youtube User\'s Network";
            // 
            // fromYoutubeVideoNetworkToolStripMenuItem
            // 
            this.fromYoutubeVideoNetworkToolStripMenuItem.Name = "fromYoutubeVideoNetworkToolStripMenuItem";
            this.fromYoutubeVideoNetworkToolStripMenuItem.Size = new System.Drawing.Size(314, 24);
            this.fromYoutubeVideoNetworkToolStripMenuItem.Text = "From Youtube Video Network";
            // 
            // toUciNETFilesToolStripMenuItem
            // 
            this.toUciNETFilesToolStripMenuItem.Name = "toUciNETFilesToolStripMenuItem";
            this.toUciNETFilesToolStripMenuItem.Size = new System.Drawing.Size(192, 24);
            this.toUciNETFilesToolStripMenuItem.Text = "To UciNET File";
            // 
            // toGraphMLFileToolStripMenuItem
            // 
            this.toGraphMLFileToolStripMenuItem.Name = "toGraphMLFileToolStripMenuItem";
            this.toGraphMLFileToolStripMenuItem.Size = new System.Drawing.Size(192, 24);
            this.toGraphMLFileToolStripMenuItem.Text = "To GraphML File";
            // 
            // toToolStripMenuItem
            // 
            this.toToolStripMenuItem.Name = "toToolStripMenuItem";
            this.toToolStripMenuItem.Size = new System.Drawing.Size(192, 24);
            this.toToolStripMenuItem.Text = "To Pajek File";
            // 
            // dataProcessingToolStripMenuItem
            // 
            this.dataProcessingToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.countAndMergeDuplicatesToolStripMenuItem,
            this.groupByClusterToolStripMenuItem,
            this.graphMetricsComputingToolStripMenuItem,
            this.autofillColumnsToolStripMenuItem,
            this.subgraphImageToolStripMenuItem});
            this.dataProcessingToolStripMenuItem.Name = "dataProcessingToolStripMenuItem";
            this.dataProcessingToolStripMenuItem.Size = new System.Drawing.Size(133, 23);
            this.dataProcessingToolStripMenuItem.Text = "Data Processing";
            // 
            // countAndMergeDuplicatesToolStripMenuItem
            // 
            this.countAndMergeDuplicatesToolStripMenuItem.Name = "countAndMergeDuplicatesToolStripMenuItem";
            this.countAndMergeDuplicatesToolStripMenuItem.Size = new System.Drawing.Size(277, 24);
            this.countAndMergeDuplicatesToolStripMenuItem.Text = "Count and Merge Duplicates";
            // 
            // groupByClusterToolStripMenuItem
            // 
            this.groupByClusterToolStripMenuItem.Name = "groupByClusterToolStripMenuItem";
            this.groupByClusterToolStripMenuItem.Size = new System.Drawing.Size(277, 24);
            this.groupByClusterToolStripMenuItem.Text = "Group by Cluster";
            // 
            // graphMetricsComputingToolStripMenuItem
            // 
            this.graphMetricsComputingToolStripMenuItem.Name = "graphMetricsComputingToolStripMenuItem";
            this.graphMetricsComputingToolStripMenuItem.Size = new System.Drawing.Size(277, 24);
            this.graphMetricsComputingToolStripMenuItem.Text = "Graph Metrics Computing";
            // 
            // autofillColumnsToolStripMenuItem
            // 
            this.autofillColumnsToolStripMenuItem.Name = "autofillColumnsToolStripMenuItem";
            this.autofillColumnsToolStripMenuItem.Size = new System.Drawing.Size(277, 24);
            this.autofillColumnsToolStripMenuItem.Text = "Autofill Columns";
            // 
            // subgraphImageToolStripMenuItem
            // 
            this.subgraphImageToolStripMenuItem.Name = "subgraphImageToolStripMenuItem";
            this.subgraphImageToolStripMenuItem.Size = new System.Drawing.Size(277, 24);
            this.subgraphImageToolStripMenuItem.Text = "Subgraph Image";
            // 
            // resultToolStripMenuItem
            // 
            this.resultToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.selectTypesToolStripMenuItem,
            this.selectLayoutsToolStripMenuItem,
            this.showGraphToolStripMenuItem,
            this.graphSummaryToolStripMenuItem});
            this.resultToolStripMenuItem.Name = "resultToolStripMenuItem";
            this.resultToolStripMenuItem.Size = new System.Drawing.Size(64, 23);
            this.resultToolStripMenuItem.Text = "Result";
            // 
            // showGraphToolStripMenuItem
            // 
            this.showGraphToolStripMenuItem.Name = "showGraphToolStripMenuItem";
            this.showGraphToolStripMenuItem.Size = new System.Drawing.Size(193, 24);
            this.showGraphToolStripMenuItem.Text = "Show Graph";
            // 
            // graphSummaryToolStripMenuItem
            // 
            this.graphSummaryToolStripMenuItem.Name = "graphSummaryToolStripMenuItem";
            this.graphSummaryToolStripMenuItem.Size = new System.Drawing.Size(193, 24);
            this.graphSummaryToolStripMenuItem.Text = "Graph Summary";
            // 
            // selectTypesToolStripMenuItem
            // 
            this.selectTypesToolStripMenuItem.Name = "selectTypesToolStripMenuItem";
            this.selectTypesToolStripMenuItem.Size = new System.Drawing.Size(193, 24);
            this.selectTypesToolStripMenuItem.Text = "Select Types";
            // 
            // selectLayoutsToolStripMenuItem
            // 
            this.selectLayoutsToolStripMenuItem.Name = "selectLayoutsToolStripMenuItem";
            this.selectLayoutsToolStripMenuItem.Size = new System.Drawing.Size(193, 24);
            this.selectLayoutsToolStripMenuItem.Text = "Select Layouts";
            // 
            // dataGridViewofEdge
            // 
            this.dataGridViewofEdge.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewofEdge.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3});
            this.dataGridViewofEdge.Location = new System.Drawing.Point(6, 16);
            this.dataGridViewofEdge.Name = "dataGridViewofEdge";
            this.dataGridViewofEdge.RowTemplate.Height = 27;
            this.dataGridViewofEdge.Size = new System.Drawing.Size(576, 150);
            this.dataGridViewofEdge.TabIndex = 1;
            this.dataGridViewofEdge.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(590, 55);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(595, 417);
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(12, 30);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(576, 465);
            this.tabControl1.TabIndex = 3;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.dataGridView1);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(603, 436);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Nodes";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.dataGridViewofEdge);
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(568, 436);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Edges";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column4});
            this.dataGridView1.Location = new System.Drawing.Point(6, 6);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 27;
            this.dataGridView1.Size = new System.Drawing.Size(591, 206);
            this.dataGridView1.TabIndex = 0;
            // 
            // Column4
            // 
            this.Column4.HeaderText = "VertexID";
            this.Column4.Name = "Column4";
            // 
            // Column1
            // 
            this.Column1.HeaderText = "VertexID";
            this.Column1.Name = "Column1";
            // 
            // Column2
            // 
            this.Column2.HeaderText = "VertexID";
            this.Column2.Name = "Column2";
            // 
            // Column3
            // 
            this.Column3.HeaderText = "EdgeID";
            this.Column3.Name = "Column3";
            // 
            // dataManagementToolStripMenuItem
            // 
            this.dataManagementToolStripMenuItem.Name = "dataManagementToolStripMenuItem";
            this.dataManagementToolStripMenuItem.Size = new System.Drawing.Size(150, 23);
            this.dataManagementToolStripMenuItem.Text = "Data Management";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1201, 683);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Form1";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewofEdge)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem importToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fromUciNETToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fromGraphMLToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fromGraphMLFilesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fromPajkeFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fromFacebookToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fromFacebookPersoanlTimelineToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fromFlickrRelatedTagsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fromFlickrUsersNetworkToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fromTwittersSearchToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fromTwitterToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fromYoutubeUsersNetworkToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fromYoutubeVideoNetworkToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toUciNETFilesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toGraphMLFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dataProcessingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem countAndMergeDuplicatesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem groupByClusterToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem graphMetricsComputingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem autofillColumnsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem subgraphImageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem resultToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectTypesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectLayoutsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showGraphToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem graphSummaryToolStripMenuItem;
        private System.Windows.Forms.DataGridView dataGridViewofEdge;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.ToolStripMenuItem dataManagementToolStripMenuItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
    }
}

