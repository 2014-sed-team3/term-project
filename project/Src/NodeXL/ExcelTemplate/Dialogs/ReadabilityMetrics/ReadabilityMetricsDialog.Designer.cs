
namespace Smrf.NodeXL.ExcelTemplate
{
    partial class ReadabilityMetricsDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReadabilityMetricsDialog));
            this.btnCalculateNow = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lnkGraphRectangleCoverage = new Smrf.AppLib.HelpLinkLabel();
            this.chkGraphRectangleCoverage = new System.Windows.Forms.CheckBox();
            this.lnkOverallEdgeCrossings = new Smrf.AppLib.HelpLinkLabel();
            this.lnkOverallVertexOverlap = new Smrf.AppLib.HelpLinkLabel();
            this.chkOverallEdgeCrossings = new System.Windows.Forms.CheckBox();
            this.chkOverallVertexOverlap = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lnkPerEdgeEdgeCrossings = new Smrf.AppLib.HelpLinkLabel();
            this.chkPerEdgeEdgeCrossings = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.lnkPerVertexEdgeCrossings = new Smrf.AppLib.HelpLinkLabel();
            this.lnkPerVertexVertexOverlap = new Smrf.AppLib.HelpLinkLabel();
            this.chkPerVertexEdgeCrossings = new System.Windows.Forms.CheckBox();
            this.chkPerVertexVertexOverlap = new System.Windows.Forms.CheckBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.chkRecalculate = new System.Windows.Forms.CheckBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnUncheckAll = new System.Windows.Forms.Button();
            this.btnCheckAll = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCalculateNow
            // 
            this.btnCalculateNow.Location = new System.Drawing.Point(13, 23);
            this.btnCalculateNow.Name = "btnCalculateNow";
            this.btnCalculateNow.Size = new System.Drawing.Size(109, 23);
            this.btnCalculateNow.TabIndex = 0;
            this.btnCalculateNow.Text = "Calculate &Now";
            this.btnCalculateNow.UseVisualStyleBackColor = true;
            this.btnCalculateNow.Click += new System.EventHandler(this.btnCalculateNow_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(251, 32);
            this.label1.TabIndex = 0;
            this.label1.Text = "Select the metrics to calculate and insert into the workbook";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lnkGraphRectangleCoverage);
            this.groupBox1.Controls.Add(this.chkGraphRectangleCoverage);
            this.groupBox1.Controls.Add(this.lnkOverallEdgeCrossings);
            this.groupBox1.Controls.Add(this.lnkOverallVertexOverlap);
            this.groupBox1.Controls.Add(this.chkOverallEdgeCrossings);
            this.groupBox1.Controls.Add(this.chkOverallVertexOverlap);
            this.groupBox1.Location = new System.Drawing.Point(15, 185);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(248, 96);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Overall";
            // 
            // lnkGraphRectangleCoverage
            // 
            this.lnkGraphRectangleCoverage.AutoSize = true;
            this.lnkGraphRectangleCoverage.Location = new System.Drawing.Point(185, 68);
            this.lnkGraphRectangleCoverage.Name = "lnkGraphRectangleCoverage";
            this.lnkGraphRectangleCoverage.Size = new System.Drawing.Size(39, 13);
            this.lnkGraphRectangleCoverage.TabIndex = 5;
            this.lnkGraphRectangleCoverage.TabStop = true;
            this.lnkGraphRectangleCoverage.Tag = resources.GetString("lnkGraphRectangleCoverage.Tag");
            this.lnkGraphRectangleCoverage.Text = "Details";
            // 
            // chkGraphRectangleCoverage
            // 
            this.chkGraphRectangleCoverage.AutoSize = true;
            this.chkGraphRectangleCoverage.Location = new System.Drawing.Point(13, 67);
            this.chkGraphRectangleCoverage.Name = "chkGraphRectangleCoverage";
            this.chkGraphRectangleCoverage.Size = new System.Drawing.Size(130, 17);
            this.chkGraphRectangleCoverage.TabIndex = 4;
            this.chkGraphRectangleCoverage.Tag = Smrf.NodeXL.ExcelTemplate.ReadabilityMetrics.GraphRectangleCoverage;
            this.chkGraphRectangleCoverage.Text = "Graph p&ane coverage";
            this.chkGraphRectangleCoverage.UseVisualStyleBackColor = true;
            // 
            // lnkOverallEdgeCrossings
            // 
            this.lnkOverallEdgeCrossings.AutoSize = true;
            this.lnkOverallEdgeCrossings.Location = new System.Drawing.Point(185, 22);
            this.lnkOverallEdgeCrossings.Name = "lnkOverallEdgeCrossings";
            this.lnkOverallEdgeCrossings.Size = new System.Drawing.Size(39, 13);
            this.lnkOverallEdgeCrossings.TabIndex = 1;
            this.lnkOverallEdgeCrossings.TabStop = true;
            this.lnkOverallEdgeCrossings.Tag = resources.GetString("lnkOverallEdgeCrossings.Tag");
            this.lnkOverallEdgeCrossings.Text = "Details";
            // 
            // lnkOverallVertexOverlap
            // 
            this.lnkOverallVertexOverlap.AutoSize = true;
            this.lnkOverallVertexOverlap.Location = new System.Drawing.Point(185, 45);
            this.lnkOverallVertexOverlap.Name = "lnkOverallVertexOverlap";
            this.lnkOverallVertexOverlap.Size = new System.Drawing.Size(39, 13);
            this.lnkOverallVertexOverlap.TabIndex = 3;
            this.lnkOverallVertexOverlap.TabStop = true;
            this.lnkOverallVertexOverlap.Tag = resources.GetString("lnkOverallVertexOverlap.Tag");
            this.lnkOverallVertexOverlap.Text = "Details";
            // 
            // chkOverallEdgeCrossings
            // 
            this.chkOverallEdgeCrossings.AutoSize = true;
            this.chkOverallEdgeCrossings.Location = new System.Drawing.Point(13, 21);
            this.chkOverallEdgeCrossings.Name = "chkOverallEdgeCrossings";
            this.chkOverallEdgeCrossings.Size = new System.Drawing.Size(143, 17);
            this.chkOverallEdgeCrossings.TabIndex = 0;
            this.chkOverallEdgeCrossings.Tag = Smrf.NodeXL.ExcelTemplate.ReadabilityMetrics.OverallEdgeCrossings;
            this.chkOverallEdgeCrossings.Text = "Edge &crossing readability";
            this.chkOverallEdgeCrossings.UseVisualStyleBackColor = true;
            // 
            // chkOverallVertexOverlap
            // 
            this.chkOverallVertexOverlap.AutoSize = true;
            this.chkOverallVertexOverlap.Location = new System.Drawing.Point(13, 44);
            this.chkOverallVertexOverlap.Name = "chkOverallVertexOverlap";
            this.chkOverallVertexOverlap.Size = new System.Drawing.Size(144, 17);
            this.chkOverallVertexOverlap.TabIndex = 2;
            this.chkOverallVertexOverlap.Tag = Smrf.NodeXL.ExcelTemplate.ReadabilityMetrics.OverallVertexOverlap;
            this.chkOverallVertexOverlap.Text = "Ver&tex overlap readability";
            this.chkOverallVertexOverlap.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lnkPerEdgeEdgeCrossings);
            this.groupBox2.Controls.Add(this.chkPerEdgeEdgeCrossings);
            this.groupBox2.Location = new System.Drawing.Point(15, 48);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(248, 50);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Edges";
            // 
            // lnkPerEdgeEdgeCrossings
            // 
            this.lnkPerEdgeEdgeCrossings.AutoSize = true;
            this.lnkPerEdgeEdgeCrossings.Location = new System.Drawing.Point(185, 22);
            this.lnkPerEdgeEdgeCrossings.Name = "lnkPerEdgeEdgeCrossings";
            this.lnkPerEdgeEdgeCrossings.Size = new System.Drawing.Size(39, 13);
            this.lnkPerEdgeEdgeCrossings.TabIndex = 1;
            this.lnkPerEdgeEdgeCrossings.TabStop = true;
            this.lnkPerEdgeEdgeCrossings.Tag = resources.GetString("lnkPerEdgeEdgeCrossings.Tag");
            this.lnkPerEdgeEdgeCrossings.Text = "Details";
            // 
            // chkPerEdgeEdgeCrossings
            // 
            this.chkPerEdgeEdgeCrossings.AutoSize = true;
            this.chkPerEdgeEdgeCrossings.Location = new System.Drawing.Point(13, 21);
            this.chkPerEdgeEdgeCrossings.Name = "chkPerEdgeEdgeCrossings";
            this.chkPerEdgeEdgeCrossings.Size = new System.Drawing.Size(143, 17);
            this.chkPerEdgeEdgeCrossings.TabIndex = 0;
            this.chkPerEdgeEdgeCrossings.Tag = Smrf.NodeXL.ExcelTemplate.ReadabilityMetrics.PerEdgeEdgeCrossings;
            this.chkPerEdgeEdgeCrossings.Text = "&Edge crossing readability";
            this.chkPerEdgeEdgeCrossings.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.lnkPerVertexEdgeCrossings);
            this.groupBox3.Controls.Add(this.lnkPerVertexVertexOverlap);
            this.groupBox3.Controls.Add(this.chkPerVertexEdgeCrossings);
            this.groupBox3.Controls.Add(this.chkPerVertexVertexOverlap);
            this.groupBox3.Location = new System.Drawing.Point(15, 105);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(248, 73);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Vertices";
            // 
            // lnkPerVertexEdgeCrossings
            // 
            this.lnkPerVertexEdgeCrossings.AutoSize = true;
            this.lnkPerVertexEdgeCrossings.Location = new System.Drawing.Point(185, 22);
            this.lnkPerVertexEdgeCrossings.Name = "lnkPerVertexEdgeCrossings";
            this.lnkPerVertexEdgeCrossings.Size = new System.Drawing.Size(39, 13);
            this.lnkPerVertexEdgeCrossings.TabIndex = 1;
            this.lnkPerVertexEdgeCrossings.TabStop = true;
            this.lnkPerVertexEdgeCrossings.Tag = resources.GetString("lnkPerVertexEdgeCrossings.Tag");
            this.lnkPerVertexEdgeCrossings.Text = "Details";
            // 
            // lnkPerVertexVertexOverlap
            // 
            this.lnkPerVertexVertexOverlap.AutoSize = true;
            this.lnkPerVertexVertexOverlap.Location = new System.Drawing.Point(185, 45);
            this.lnkPerVertexVertexOverlap.Name = "lnkPerVertexVertexOverlap";
            this.lnkPerVertexVertexOverlap.Size = new System.Drawing.Size(39, 13);
            this.lnkPerVertexVertexOverlap.TabIndex = 3;
            this.lnkPerVertexVertexOverlap.TabStop = true;
            this.lnkPerVertexVertexOverlap.Tag = resources.GetString("lnkPerVertexVertexOverlap.Tag");
            this.lnkPerVertexVertexOverlap.Text = "Details";
            // 
            // chkPerVertexEdgeCrossings
            // 
            this.chkPerVertexEdgeCrossings.AutoSize = true;
            this.chkPerVertexEdgeCrossings.Location = new System.Drawing.Point(13, 21);
            this.chkPerVertexEdgeCrossings.Name = "chkPerVertexEdgeCrossings";
            this.chkPerVertexEdgeCrossings.Size = new System.Drawing.Size(143, 17);
            this.chkPerVertexEdgeCrossings.TabIndex = 0;
            this.chkPerVertexEdgeCrossings.Tag = Smrf.NodeXL.ExcelTemplate.ReadabilityMetrics.PerVertexEdgeCrossings;
            this.chkPerVertexEdgeCrossings.Text = "Ed&ge crossing readability";
            this.chkPerVertexEdgeCrossings.UseVisualStyleBackColor = true;
            // 
            // chkPerVertexVertexOverlap
            // 
            this.chkPerVertexVertexOverlap.AutoSize = true;
            this.chkPerVertexVertexOverlap.Location = new System.Drawing.Point(13, 44);
            this.chkPerVertexVertexOverlap.Name = "chkPerVertexVertexOverlap";
            this.chkPerVertexVertexOverlap.Size = new System.Drawing.Size(144, 17);
            this.chkPerVertexVertexOverlap.TabIndex = 2;
            this.chkPerVertexVertexOverlap.Tag = Smrf.NodeXL.ExcelTemplate.ReadabilityMetrics.PerVertexVertexOverlap;
            this.chkPerVertexVertexOverlap.Text = "&Vertex overlap readability";
            this.chkPerVertexVertexOverlap.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.chkRecalculate);
            this.groupBox4.Controls.Add(this.btnCalculateNow);
            this.groupBox4.Location = new System.Drawing.Point(15, 335);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(248, 100);
            this.groupBox4.TabIndex = 6;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "When to calculate the metrics";
            // 
            // chkRecalculate
            // 
            this.chkRecalculate.Enabled = false;
            this.chkRecalculate.Location = new System.Drawing.Point(13, 52);
            this.chkRecalculate.Name = "chkRecalculate";
            this.chkRecalculate.Size = new System.Drawing.Size(221, 42);
            this.chkRecalculate.TabIndex = 1;
            this.chkRecalculate.Text = "Ever&y time a vertex is moved while this window is open";
            this.chkRecalculate.UseVisualStyleBackColor = true;
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(188, 448);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 7;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnUncheckAll
            // 
            this.btnUncheckAll.Location = new System.Drawing.Point(96, 294);
            this.btnUncheckAll.Name = "btnUncheckAll";
            this.btnUncheckAll.Size = new System.Drawing.Size(75, 23);
            this.btnUncheckAll.TabIndex = 5;
            this.btnUncheckAll.Text = "&Deselect All";
            this.btnUncheckAll.UseVisualStyleBackColor = true;
            this.btnUncheckAll.Click += new System.EventHandler(this.OnCheckOrUncheckAll);
            // 
            // btnCheckAll
            // 
            this.btnCheckAll.Location = new System.Drawing.Point(15, 293);
            this.btnCheckAll.Name = "btnCheckAll";
            this.btnCheckAll.Size = new System.Drawing.Size(75, 23);
            this.btnCheckAll.TabIndex = 4;
            this.btnCheckAll.Text = "&Select All";
            this.btnCheckAll.UseVisualStyleBackColor = true;
            this.btnCheckAll.Click += new System.EventHandler(this.OnCheckOrUncheckAll);
            // 
            // ReadabilityMetricsDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(278, 485);
            this.Controls.Add(this.btnUncheckAll);
            this.Controls.Add(this.btnCheckAll);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ReadabilityMetricsDialog";
            this.Text = "Readability Metrics";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ReadabilityMetricsDialog_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCalculateNow;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox chkPerEdgeEdgeCrossings;
        private System.Windows.Forms.CheckBox chkPerVertexEdgeCrossings;
        private System.Windows.Forms.CheckBox chkPerVertexVertexOverlap;
        private System.Windows.Forms.CheckBox chkOverallEdgeCrossings;
        private System.Windows.Forms.CheckBox chkOverallVertexOverlap;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.CheckBox chkRecalculate;
        private System.Windows.Forms.Button btnClose;
        private Smrf.AppLib.HelpLinkLabel lnkPerVertexVertexOverlap;
        private Smrf.AppLib.HelpLinkLabel lnkOverallEdgeCrossings;
        private Smrf.AppLib.HelpLinkLabel lnkOverallVertexOverlap;
        private Smrf.AppLib.HelpLinkLabel lnkPerEdgeEdgeCrossings;
        private Smrf.AppLib.HelpLinkLabel lnkPerVertexEdgeCrossings;
        private System.Windows.Forms.Button btnUncheckAll;
        private System.Windows.Forms.Button btnCheckAll;
        private System.Windows.Forms.CheckBox chkGraphRectangleCoverage;
        private Smrf.AppLib.HelpLinkLabel lnkGraphRectangleCoverage;
    }
}
