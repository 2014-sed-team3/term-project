namespace GraphStorageManagement
{
    partial class GenerateGraph
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.listView1 = new System.Windows.Forms.ListView();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.listView6 = new System.Windows.Forms.ListView();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.listView7 = new System.Windows.Forms.ListView();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.VertexColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.listView2 = new System.Windows.Forms.ListView();
            this.PostColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.listView3 = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.listView4 = new System.Windows.Forms.ListView();
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.listView5 = new System.Windows.Forms.ListView();
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.panel1 = new System.Windows.Forms.Panel();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(447, 333);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.panel1);
            this.tabPage1.Controls.Add(this.listView1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.tabPage1.Size = new System.Drawing.Size(439, 307);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Vertex";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // listView1
            // 
            this.listView1.AllowColumnReorder = true;
            this.listView1.CheckBoxes = true;
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.VertexColumn});
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.LabelEdit = true;
            this.listView1.Location = new System.Drawing.Point(18, 17);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(149, 260);
            this.listView1.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.listView3);
            this.tabPage2.Controls.Add(this.listView2);
            this.tabPage2.Controls.Add(this.listView6);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.tabPage2.Size = new System.Drawing.Size(439, 307);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Edge";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // listView6
            // 
            this.listView6.Location = new System.Drawing.Point(253, 16);
            this.listView6.Name = "listView6";
            this.listView6.Size = new System.Drawing.Size(175, 260);
            this.listView6.TabIndex = 3;
            this.listView6.UseCompatibleStateImageBehavior = false;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.listView4);
            this.tabPage3.Controls.Add(this.listView5);
            this.tabPage3.Controls.Add(this.listView7);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(439, 307);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Filter";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // listView7
            // 
            this.listView7.Location = new System.Drawing.Point(253, 16);
            this.listView7.Name = "listView7";
            this.listView7.Size = new System.Drawing.Size(175, 260);
            this.listView7.TabIndex = 5;
            this.listView7.UseCompatibleStateImageBehavior = false;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(262, 339);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Generate";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(372, 339);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 2;
            this.button2.Text = "Cancel";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // VertexColumn
            // 
            this.VertexColumn.Text = "Columns";
            this.VertexColumn.Width = 144;
            // 
            // listView2
            // 
            this.listView2.AllowColumnReorder = true;
            this.listView2.CheckBoxes = true;
            this.listView2.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.PostColumn});
            this.listView2.FullRowSelect = true;
            this.listView2.GridLines = true;
            this.listView2.LabelEdit = true;
            this.listView2.Location = new System.Drawing.Point(8, 16);
            this.listView2.Name = "listView2";
            this.listView2.Size = new System.Drawing.Size(95, 260);
            this.listView2.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.listView2.TabIndex = 4;
            this.listView2.UseCompatibleStateImageBehavior = false;
            this.listView2.View = System.Windows.Forms.View.Details;
            // 
            // PostColumn
            // 
            this.PostColumn.Text = "Columns";
            this.PostColumn.Width = 90;
            // 
            // listView3
            // 
            this.listView3.AllowColumnReorder = true;
            this.listView3.CheckBoxes = true;
            this.listView3.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.listView3.FullRowSelect = true;
            this.listView3.GridLines = true;
            this.listView3.LabelEdit = true;
            this.listView3.Location = new System.Drawing.Point(124, 16);
            this.listView3.Name = "listView3";
            this.listView3.Size = new System.Drawing.Size(95, 260);
            this.listView3.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.listView3.TabIndex = 5;
            this.listView3.UseCompatibleStateImageBehavior = false;
            this.listView3.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Columns";
            this.columnHeader1.Width = 90;
            // 
            // listView4
            // 
            this.listView4.AllowColumnReorder = true;
            this.listView4.CheckBoxes = true;
            this.listView4.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader2});
            this.listView4.FullRowSelect = true;
            this.listView4.GridLines = true;
            this.listView4.LabelEdit = true;
            this.listView4.Location = new System.Drawing.Point(124, 16);
            this.listView4.Name = "listView4";
            this.listView4.Size = new System.Drawing.Size(95, 260);
            this.listView4.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.listView4.TabIndex = 7;
            this.listView4.UseCompatibleStateImageBehavior = false;
            this.listView4.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Columns";
            this.columnHeader2.Width = 90;
            // 
            // listView5
            // 
            this.listView5.AllowColumnReorder = true;
            this.listView5.CheckBoxes = true;
            this.listView5.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader3});
            this.listView5.FullRowSelect = true;
            this.listView5.GridLines = true;
            this.listView5.LabelEdit = true;
            this.listView5.Location = new System.Drawing.Point(8, 16);
            this.listView5.Name = "listView5";
            this.listView5.Size = new System.Drawing.Size(95, 260);
            this.listView5.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.listView5.TabIndex = 6;
            this.listView5.UseCompatibleStateImageBehavior = false;
            this.listView5.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Columns";
            this.columnHeader3.Width = 90;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.checkBox1);
            this.panel1.Location = new System.Drawing.Point(190, 17);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(227, 260);
            this.panel1.TabIndex = 1;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(23, 32);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(123, 16);
            this.checkBox1.TabIndex = 0;
            this.checkBox1.Text = "Select as main Vertex";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // GenerateGraph
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(459, 376);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.tabControl1);
            this.Name = "GenerateGraph";
            this.Text = "GenerateGraph";
            this.Load += new System.EventHandler(this.GenerateGraph_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.ListView listView6;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.ListView listView7;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ColumnHeader VertexColumn;
        private System.Windows.Forms.ListView listView3;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ListView listView2;
        private System.Windows.Forms.ColumnHeader PostColumn;
        private System.Windows.Forms.ListView listView4;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ListView listView5;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckBox checkBox1;
    }
}