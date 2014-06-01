namespace TestPureNodeXLControlWF
{
    partial class LayoutControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LayoutControl));
            this.ehNodeXLControlHost = new System.Windows.Forms.Integration.ElementHost();
            this.tsMouse = new Smrf.AppLib.ToolStripPlus();
            this.tsbMouseModeSelect = new System.Windows.Forms.ToolStripButton();
            this.tsbMouseModeAddToSelection = new System.Windows.Forms.ToolStripButton();
            this.tsbMouseModeSubstractFromSelection = new System.Windows.Forms.ToolStripButton();
            this.tsbMouseModeZoomIn = new System.Windows.Forms.ToolStripButton();
            this.tsbMouseModeZoomOut = new System.Windows.Forms.ToolStripButton();
            this.tsbMouseModeTranslate = new System.Windows.Forms.ToolStripButton();
            this.tsTop = new Smrf.AppLib.ToolStripPlus();
            this.tsbShowGraph = new System.Windows.Forms.ToolStripButton();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.tssbLayout = new System.Windows.Forms.ToolStripSplitButton();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.msiLayoutOptions = new System.Windows.Forms.ToolStripMenuItem();
            this.tsbOptions = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.tsMouse.SuspendLayout();
            this.tsTop.SuspendLayout();
            this.SuspendLayout();
            // 
            // ehNodeXLControlHost
            // 
            this.ehNodeXLControlHost.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ehNodeXLControlHost.Location = new System.Drawing.Point(0, 50);
            this.ehNodeXLControlHost.Name = "ehNodeXLControlHost";
            this.ehNodeXLControlHost.Size = new System.Drawing.Size(793, 404);
            this.ehNodeXLControlHost.TabIndex = 2;
            this.ehNodeXLControlHost.Text = "elementHost1";
            this.ehNodeXLControlHost.Child = null;
            // 
            // tsMouse
            // 
            this.tsMouse.ClickThrough = true;
            this.tsMouse.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.tsMouse.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbMouseModeSelect,
            this.tsbMouseModeAddToSelection,
            this.tsbMouseModeSubstractFromSelection,
            this.toolStripSeparator1,
            this.tsbMouseModeZoomIn,
            this.tsbMouseModeZoomOut,
            this.toolStripSeparator2,
            this.tsbMouseModeTranslate,
            this.toolStripSeparator3,
            this.toolStripSeparator4});
            this.tsMouse.Location = new System.Drawing.Point(0, 25);
            this.tsMouse.Name = "tsMouse";
            this.tsMouse.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.tsMouse.Size = new System.Drawing.Size(793, 25);
            this.tsMouse.TabIndex = 1;
            this.tsMouse.Text = "toolStripPlus2";
            // 
            // tsbMouseModeSelect
            // 
            this.tsbMouseModeSelect.Checked = true;
            this.tsbMouseModeSelect.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tsbMouseModeSelect.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbMouseModeSelect.Image = ((System.Drawing.Image)(resources.GetObject("tsbMouseModeSelect.Image")));
            this.tsbMouseModeSelect.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbMouseModeSelect.Name = "tsbMouseModeSelect";
            this.tsbMouseModeSelect.Size = new System.Drawing.Size(23, 22);
            this.tsbMouseModeSelect.Tag = Smrf.NodeXL.Visualization.Wpf.MouseMode.Select;
            this.tsbMouseModeSelect.Text = "tsbMouseModeSelect";
            this.tsbMouseModeSelect.ToolTipText = resources.GetString("tsbMouseModeSelect.ToolTipText");
            this.tsbMouseModeSelect.Click += new System.EventHandler(this.tsbMouseModeButton_Click);
            // 
            // tsbMouseModeAddToSelection
            // 
            this.tsbMouseModeAddToSelection.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbMouseModeAddToSelection.Image = ((System.Drawing.Image)(resources.GetObject("tsbMouseModeAddToSelection.Image")));
            this.tsbMouseModeAddToSelection.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbMouseModeAddToSelection.Name = "tsbMouseModeAddToSelection";
            this.tsbMouseModeAddToSelection.Size = new System.Drawing.Size(23, 22);
            this.tsbMouseModeAddToSelection.Tag = Smrf.NodeXL.Visualization.Wpf.MouseMode.AddToSelection;
            this.tsbMouseModeAddToSelection.Text = "tsbMouseModeAddToSelection";
            this.tsbMouseModeAddToSelection.ToolTipText = resources.GetString("tsbMouseModeAddToSelection.ToolTipText");
            this.tsbMouseModeAddToSelection.Click += new System.EventHandler(this.tsbMouseModeButton_Click);
            // 
            // tsbMouseModeSubstractFromSelection
            // 
            this.tsbMouseModeSubstractFromSelection.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbMouseModeSubstractFromSelection.Image = ((System.Drawing.Image)(resources.GetObject("tsbMouseModeSubstractFromSelection.Image")));
            this.tsbMouseModeSubstractFromSelection.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbMouseModeSubstractFromSelection.Name = "tsbMouseModeSubstractFromSelection";
            this.tsbMouseModeSubstractFromSelection.Size = new System.Drawing.Size(23, 22);
            this.tsbMouseModeSubstractFromSelection.Tag = Smrf.NodeXL.Visualization.Wpf.MouseMode.SubtractFromSelection;
            this.tsbMouseModeSubstractFromSelection.Text = "tsbMouseModeSubstractFromSelection";
            this.tsbMouseModeSubstractFromSelection.ToolTipText = resources.GetString("tsbMouseModeSubstractFromSelection.ToolTipText");
            this.tsbMouseModeSubstractFromSelection.Click += new System.EventHandler(this.tsbMouseModeButton_Click);
            // 
            // tsbMouseModeZoomIn
            // 
            this.tsbMouseModeZoomIn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbMouseModeZoomIn.Image = ((System.Drawing.Image)(resources.GetObject("tsbMouseModeZoomIn.Image")));
            this.tsbMouseModeZoomIn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbMouseModeZoomIn.Name = "tsbMouseModeZoomIn";
            this.tsbMouseModeZoomIn.Size = new System.Drawing.Size(23, 22);
            this.tsbMouseModeZoomIn.Tag = Smrf.NodeXL.Visualization.Wpf.MouseMode.ZoomIn;
            this.tsbMouseModeZoomIn.Text = "toolStripButton4";
            this.tsbMouseModeZoomIn.ToolTipText = "Zoom In\r\n\r\n• Click on the graph to zoom in.\r\n\r\n• (You can also zoom in and out at" +
    " any time with the mouse wheel or the Zoom slider.)";
            this.tsbMouseModeZoomIn.Click += new System.EventHandler(this.tsbMouseModeButton_Click);
            // 
            // tsbMouseModeZoomOut
            // 
            this.tsbMouseModeZoomOut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbMouseModeZoomOut.Image = ((System.Drawing.Image)(resources.GetObject("tsbMouseModeZoomOut.Image")));
            this.tsbMouseModeZoomOut.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbMouseModeZoomOut.Name = "tsbMouseModeZoomOut";
            this.tsbMouseModeZoomOut.Size = new System.Drawing.Size(23, 22);
            this.tsbMouseModeZoomOut.Tag = Smrf.NodeXL.Visualization.Wpf.MouseMode.ZoomOut;
            this.tsbMouseModeZoomOut.Text = "toolStripButton5";
            this.tsbMouseModeZoomOut.ToolTipText = "Zoom Out\r\n\r\n• Click on the graph to zoom out.\r\n\r\n• (You can also zoom in and out " +
    "at any time with the mouse wheel or the Zoom slider.)";
            this.tsbMouseModeZoomOut.Click += new System.EventHandler(this.tsbMouseModeButton_Click);
            // 
            // tsbMouseModeTranslate
            // 
            this.tsbMouseModeTranslate.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbMouseModeTranslate.Image = ((System.Drawing.Image)(resources.GetObject("tsbMouseModeTranslate.Image")));
            this.tsbMouseModeTranslate.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbMouseModeTranslate.Name = "tsbMouseModeTranslate";
            this.tsbMouseModeTranslate.Size = new System.Drawing.Size(23, 22);
            this.tsbMouseModeTranslate.Tag = Smrf.NodeXL.Visualization.Wpf.MouseMode.Translate;
            this.tsbMouseModeTranslate.Text = "toolStripButton6";
            this.tsbMouseModeTranslate.ToolTipText = resources.GetString("tsbMouseModeTranslate.ToolTipText");
            this.tsbMouseModeTranslate.Click += new System.EventHandler(this.tsbMouseModeButton_Click);
            // 
            // tsTop
            // 
            this.tsTop.ClickThrough = true;
            this.tsTop.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.tsTop.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbShowGraph,
            this.toolStripLabel1,
            this.tssbLayout,
            this.tsbOptions});
            this.tsTop.Location = new System.Drawing.Point(0, 0);
            this.tsTop.Name = "tsTop";
            this.tsTop.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.tsTop.Size = new System.Drawing.Size(793, 25);
            this.tsTop.TabIndex = 0;
            this.tsTop.Text = "toolStripPlus1";
            // 
            // tsbShowGraph
            // 
            this.tsbShowGraph.Image = ((System.Drawing.Image)(resources.GetObject("tsbShowGraph.Image")));
            this.tsbShowGraph.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbShowGraph.Margin = new System.Windows.Forms.Padding(0, 1, 2, 2);
            this.tsbShowGraph.Name = "tsbShowGraph";
            this.tsbShowGraph.Size = new System.Drawing.Size(97, 22);
            this.tsbShowGraph.Text = "Show Graph";
            this.tsbShowGraph.ToolTipText = "Show or refresh the graph in the graph pane";
            this.tsbShowGraph.Click += new System.EventHandler(this.tsbShowGraph_Click);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripLabel1.Image")));
            this.toolStripLabel1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripLabel1.Margin = new System.Windows.Forms.Padding(2, 1, 2, 2);
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(16, 22);
            this.toolStripLabel1.ToolTipText = "Select the algorithm used to lay out the graph";
            // 
            // tssbLayout
            // 
            this.tssbLayout.AutoSize = false;
            this.tssbLayout.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tssbLayout.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator7,
            this.msiLayoutOptions});
            this.tssbLayout.Image = ((System.Drawing.Image)(resources.GetObject("tssbLayout.Image")));
            this.tssbLayout.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tssbLayout.Name = "tssbLayout";
            this.tssbLayout.Size = new System.Drawing.Size(130, 22);
            this.tssbLayout.Text = "Set in code";
            this.tssbLayout.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.tssbLayout.ToolTipText = "Select the algorithm used to lay out the graph";
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(168, 6);
            // 
            // msiLayoutOptions
            // 
            this.msiLayoutOptions.Name = "msiLayoutOptions";
            this.msiLayoutOptions.Size = new System.Drawing.Size(171, 22);
            this.msiLayoutOptions.Text = "Layout Options...";
            this.msiLayoutOptions.ToolTipText = "Specify the options that control the selected layout algorithm";
            this.msiLayoutOptions.Click += new System.EventHandler(this.msiLayoutOptions_Click);
            // 
            // tsbOptions
            // 
            this.tsbOptions.Image = ((System.Drawing.Image)(resources.GetObject("tsbOptions.Image")));
            this.tsbOptions.ImageTransparentColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.tsbOptions.Margin = new System.Windows.Forms.Padding(2, 1, 2, 2);
            this.tsbOptions.Name = "tsbOptions";
            this.tsbOptions.Size = new System.Drawing.Size(111, 22);
            this.tsbOptions.Text = "Graph Options";
            this.tsbOptions.ToolTipText = "Specify options that control the graph\'s appearance";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // LayoutControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ehNodeXLControlHost);
            this.Controls.Add(this.tsMouse);
            this.Controls.Add(this.tsTop);
            this.Name = "LayoutControl";
            this.Size = new System.Drawing.Size(793, 454);
            this.tsMouse.ResumeLayout(false);
            this.tsMouse.PerformLayout();
            this.tsTop.ResumeLayout(false);
            this.tsTop.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Smrf.AppLib.ToolStripPlus tsTop;
        private Smrf.AppLib.ToolStripPlus tsMouse;
        private System.Windows.Forms.Integration.ElementHost ehNodeXLControlHost;
        private System.Windows.Forms.ToolStripButton tsbMouseModeSelect;
        private System.Windows.Forms.ToolStripButton tsbMouseModeAddToSelection;
        private System.Windows.Forms.ToolStripButton tsbMouseModeSubstractFromSelection;
        private System.Windows.Forms.ToolStripButton tsbMouseModeZoomIn;
        private System.Windows.Forms.ToolStripButton tsbMouseModeZoomOut;
        private System.Windows.Forms.ToolStripButton tsbMouseModeTranslate;
        private System.Windows.Forms.ToolStripButton tsbShowGraph;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripSplitButton tssbLayout;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripMenuItem msiLayoutOptions;
        private System.Windows.Forms.ToolStripButton tsbOptions;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
    }
}
