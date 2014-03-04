

namespace Smrf.NodeXL.ExcelTemplate
{
    partial class GraphMetricsDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GraphMetricsDialog));
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btnCheckAll = new System.Windows.Forms.Button();
            this.btnUncheckAll = new System.Windows.Forms.Button();
            this.lnkDuplicateEdges = new Smrf.AppLib.HelpLinkLabel();
            this.clbGraphMetrics = new Smrf.AppLib.CheckedListBoxPlus();
            this.btnOptions = new System.Windows.Forms.Button();
            this.wbDescription = new System.Windows.Forms.WebBrowser();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(388, 459);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 23);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(276, 459);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(106, 23);
            this.btnOK.TabIndex = 7;
            this.btnOK.Text = "Calculate Metrics";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(239, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "&Metrics to calculate and insert into the workbook:";
            // 
            // btnCheckAll
            // 
            this.btnCheckAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCheckAll.Location = new System.Drawing.Point(393, 47);
            this.btnCheckAll.Name = "btnCheckAll";
            this.btnCheckAll.Size = new System.Drawing.Size(75, 23);
            this.btnCheckAll.TabIndex = 2;
            this.btnCheckAll.Text = "&Select All";
            this.btnCheckAll.UseVisualStyleBackColor = true;
            this.btnCheckAll.Click += new System.EventHandler(this.OnCheckOrUncheckAll);
            // 
            // btnUncheckAll
            // 
            this.btnUncheckAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUncheckAll.Location = new System.Drawing.Point(393, 76);
            this.btnUncheckAll.Name = "btnUncheckAll";
            this.btnUncheckAll.Size = new System.Drawing.Size(75, 23);
            this.btnUncheckAll.TabIndex = 3;
            this.btnUncheckAll.Text = "&Deselect All";
            this.btnUncheckAll.UseVisualStyleBackColor = true;
            this.btnUncheckAll.Click += new System.EventHandler(this.OnCheckOrUncheckAll);
            // 
            // lnkDuplicateEdges
            // 
            this.lnkDuplicateEdges.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lnkDuplicateEdges.AutoSize = true;
            this.lnkDuplicateEdges.Location = new System.Drawing.Point(12, 464);
            this.lnkDuplicateEdges.Name = "lnkDuplicateEdges";
            this.lnkDuplicateEdges.Size = new System.Drawing.Size(113, 13);
            this.lnkDuplicateEdges.TabIndex = 6;
            this.lnkDuplicateEdges.TabStop = true;
            this.lnkDuplicateEdges.Tag = resources.GetString("lnkDuplicateEdges.Tag");
            this.lnkDuplicateEdges.Text = "About duplicate edges";
            this.lnkDuplicateEdges.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // clbGraphMetrics
            // 
            this.clbGraphMetrics.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.clbGraphMetrics.CheckOnClick = true;
            this.clbGraphMetrics.FormattingEnabled = true;
            this.clbGraphMetrics.Location = new System.Drawing.Point(15, 32);
            this.clbGraphMetrics.Name = "clbGraphMetrics";
            this.clbGraphMetrics.Size = new System.Drawing.Size(363, 214);
            this.clbGraphMetrics.TabIndex = 1;
            this.clbGraphMetrics.SelectedIndexChanged += new System.EventHandler(this.clbGraphMetrics_SelectedIndexChanged);
            // 
            // btnOptions
            // 
            this.btnOptions.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOptions.Enabled = false;
            this.btnOptions.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnOptions.Location = new System.Drawing.Point(393, 123);
            this.btnOptions.Name = "btnOptions";
            this.btnOptions.Size = new System.Drawing.Size(75, 23);
            this.btnOptions.TabIndex = 4;
            this.btnOptions.Text = "&Options...";
            this.btnOptions.UseVisualStyleBackColor = true;
            this.btnOptions.Click += new System.EventHandler(this.btnOptions_Click);
            // 
            // wbDescription
            // 
            this.wbDescription.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.wbDescription.Location = new System.Drawing.Point(15, 261);
            this.wbDescription.MinimumSize = new System.Drawing.Size(20, 20);
            this.wbDescription.Name = "wbDescription";
            this.wbDescription.Size = new System.Drawing.Size(453, 181);
            this.wbDescription.TabIndex = 5;
            this.wbDescription.Navigating += new System.Windows.Forms.WebBrowserNavigatingEventHandler(this.wbDescription_Navigating);
            // 
            // GraphMetricsDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(483, 496);
            this.Controls.Add(this.wbDescription);
            this.Controls.Add(this.btnOptions);
            this.Controls.Add(this.clbGraphMetrics);
            this.Controls.Add(this.lnkDuplicateEdges);
            this.Controls.Add(this.btnUncheckAll);
            this.Controls.Add(this.btnCheckAll);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(499, 534);
            this.Name = "GraphMetricsDialog";
            this.Text = "Graph Metrics";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnCheckAll;
        private System.Windows.Forms.Button btnUncheckAll;
        private Smrf.AppLib.HelpLinkLabel lnkDuplicateEdges;
        private Smrf.AppLib.CheckedListBoxPlus clbGraphMetrics;
        private System.Windows.Forms.Button btnOptions;
        private System.Windows.Forms.WebBrowser wbDescription;
    }
}
