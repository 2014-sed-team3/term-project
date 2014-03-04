

namespace Smrf.NodeXL.ExcelTemplate
{
    partial class LayoutUserSettingsDialog
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
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.nudFruchtermanReingoldIterations = new System.Windows.Forms.NumericUpDown();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.nudFruchtermanReingoldC = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.nudMargin = new System.Windows.Forms.NumericUpDown();
            this.label10 = new System.Windows.Forms.Label();
            this.pnlLayoutStyleUseBinning = new System.Windows.Forms.Panel();
            this.nudBinLength = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.nudMaximumVerticesPerBin = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.btnResetAll = new System.Windows.Forms.Button();
            this.radLayoutStyleUseGroups = new System.Windows.Forms.RadioButton();
            this.radLayoutStyleNormal = new System.Windows.Forms.RadioButton();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.pnlLayoutStyleUseGroups = new System.Windows.Forms.Panel();
            this.cbxBoxLayoutAlgorithm = new Smrf.AppLib.ComboBoxPlus();
            this.label8 = new System.Windows.Forms.Label();
            this.cbxIntergroupEdgeStyle = new Smrf.AppLib.ComboBoxPlus();
            this.label6 = new System.Windows.Forms.Label();
            this.chkImproveLayoutOfGroups = new System.Windows.Forms.CheckBox();
            this.nudGroupRectanglePenWidth = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.radLayoutStyleUseBinning = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.nudFruchtermanReingoldIterations)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudFruchtermanReingoldC)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMargin)).BeginInit();
            this.pnlLayoutStyleUseBinning.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudBinLength)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMaximumVerticesPerBin)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.pnlLayoutStyleUseGroups.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudGroupRectanglePenWidth)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(276, 479);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 23);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(190, 479);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(80, 23);
            this.btnOK.TabIndex = 5;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(9, 59);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(102, 13);
            this.label7.TabIndex = 2;
            this.label7.Text = "&Iterations per layout:";
            // 
            // nudFruchtermanReingoldIterations
            // 
            this.nudFruchtermanReingoldIterations.Location = new System.Drawing.Point(181, 57);
            this.nudFruchtermanReingoldIterations.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudFruchtermanReingoldIterations.Name = "nudFruchtermanReingoldIterations";
            this.nudFruchtermanReingoldIterations.Size = new System.Drawing.Size(56, 20);
            this.nudFruchtermanReingoldIterations.TabIndex = 3;
            this.nudFruchtermanReingoldIterations.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.nudFruchtermanReingoldC);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.nudFruchtermanReingoldIterations);
            this.groupBox1.Location = new System.Drawing.Point(12, 372);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(344, 90);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Fruchterman-Reingold layout";
            // 
            // nudFruchtermanReingoldC
            // 
            this.nudFruchtermanReingoldC.DecimalPlaces = 1;
            this.nudFruchtermanReingoldC.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.nudFruchtermanReingoldC.Location = new System.Drawing.Point(181, 24);
            this.nudFruchtermanReingoldC.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.nudFruchtermanReingoldC.Name = "nudFruchtermanReingoldC";
            this.nudFruchtermanReingoldC.Size = new System.Drawing.Size(56, 20);
            this.nudFruchtermanReingoldC.TabIndex = 1;
            this.nudFruchtermanReingoldC.Value = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(9, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(166, 33);
            this.label1.TabIndex = 0;
            this.label1.Text = "&Strength of the repulsive force between vertices:";
            // 
            // nudMargin
            // 
            this.nudMargin.Location = new System.Drawing.Point(74, 7);
            this.nudMargin.Name = "nudMargin";
            this.nudMargin.Size = new System.Drawing.Size(56, 20);
            this.nudMargin.TabIndex = 1;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(12, 9);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(42, 13);
            this.label10.TabIndex = 0;
            this.label10.Text = "&Margin:";
            // 
            // pnlLayoutStyleUseBinning
            // 
            this.pnlLayoutStyleUseBinning.Controls.Add(this.nudBinLength);
            this.pnlLayoutStyleUseBinning.Controls.Add(this.label4);
            this.pnlLayoutStyleUseBinning.Controls.Add(this.label3);
            this.pnlLayoutStyleUseBinning.Controls.Add(this.nudMaximumVerticesPerBin);
            this.pnlLayoutStyleUseBinning.Controls.Add(this.label2);
            this.pnlLayoutStyleUseBinning.Location = new System.Drawing.Point(38, 256);
            this.pnlLayoutStyleUseBinning.Name = "pnlLayoutStyleUseBinning";
            this.pnlLayoutStyleUseBinning.Size = new System.Drawing.Size(292, 61);
            this.pnlLayoutStyleUseBinning.TabIndex = 4;
            // 
            // nudBinLength
            // 
            this.nudBinLength.Location = new System.Drawing.Point(181, 35);
            this.nudBinLength.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.nudBinLength.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudBinLength.Name = "nudBinLength";
            this.nudBinLength.Size = new System.Drawing.Size(49, 20);
            this.nudBinLength.TabIndex = 4;
            this.nudBinLength.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 37);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(49, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Box si&ze:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(236, 7);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(44, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "vertices";
            // 
            // nudMaximumVerticesPerBin
            // 
            this.nudMaximumVerticesPerBin.Location = new System.Drawing.Point(181, 5);
            this.nudMaximumVerticesPerBin.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudMaximumVerticesPerBin.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudMaximumVerticesPerBin.Name = "nudMaximumVerticesPerBin";
            this.nudMaximumVerticesPerBin.Size = new System.Drawing.Size(49, 20);
            this.nudMaximumVerticesPerBin.TabIndex = 1;
            this.nudMaximumVerticesPerBin.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(3, 3);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(172, 32);
            this.label2.TabIndex = 0;
            this.label2.Text = "Ma&ximum size of the connected components to lay out in boxes:";
            // 
            // btnResetAll
            // 
            this.btnResetAll.Location = new System.Drawing.Point(12, 479);
            this.btnResetAll.Name = "btnResetAll";
            this.btnResetAll.Size = new System.Drawing.Size(80, 23);
            this.btnResetAll.TabIndex = 4;
            this.btnResetAll.Text = "Reset All";
            this.btnResetAll.UseVisualStyleBackColor = true;
            this.btnResetAll.Click += new System.EventHandler(this.btnResetAll_Click);
            // 
            // radLayoutStyleUseGroups
            // 
            this.radLayoutStyleUseGroups.Location = new System.Drawing.Point(10, 48);
            this.radLayoutStyleUseGroups.Name = "radLayoutStyleUseGroups";
            this.radLayoutStyleUseGroups.Size = new System.Drawing.Size(320, 30);
            this.radLayoutStyleUseGroups.TabIndex = 1;
            this.radLayoutStyleUseGroups.TabStop = true;
            this.radLayoutStyleUseGroups.Text = "L&ay out each of the graph\'s groups in its own box";
            this.radLayoutStyleUseGroups.UseVisualStyleBackColor = true;
            this.radLayoutStyleUseGroups.CheckedChanged += new System.EventHandler(this.OnEventThatRequiresControlEnabling);
            // 
            // radLayoutStyleNormal
            // 
            this.radLayoutStyleNormal.Location = new System.Drawing.Point(10, 19);
            this.radLayoutStyleNormal.Name = "radLayoutStyleNormal";
            this.radLayoutStyleNormal.Size = new System.Drawing.Size(320, 30);
            this.radLayoutStyleNormal.TabIndex = 0;
            this.radLayoutStyleNormal.TabStop = true;
            this.radLayoutStyleNormal.Text = "&Lay out the entire graph in the entire graph pane (typical case)";
            this.radLayoutStyleNormal.UseVisualStyleBackColor = true;
            this.radLayoutStyleNormal.CheckedChanged += new System.EventHandler(this.OnEventThatRequiresControlEnabling);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.pnlLayoutStyleUseGroups);
            this.groupBox3.Controls.Add(this.radLayoutStyleUseBinning);
            this.groupBox3.Controls.Add(this.pnlLayoutStyleUseBinning);
            this.groupBox3.Controls.Add(this.radLayoutStyleNormal);
            this.groupBox3.Controls.Add(this.radLayoutStyleUseGroups);
            this.groupBox3.Location = new System.Drawing.Point(12, 37);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(344, 329);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Layout style";
            // 
            // pnlLayoutStyleUseGroups
            // 
            this.pnlLayoutStyleUseGroups.Controls.Add(this.cbxBoxLayoutAlgorithm);
            this.pnlLayoutStyleUseGroups.Controls.Add(this.label8);
            this.pnlLayoutStyleUseGroups.Controls.Add(this.cbxIntergroupEdgeStyle);
            this.pnlLayoutStyleUseGroups.Controls.Add(this.label6);
            this.pnlLayoutStyleUseGroups.Controls.Add(this.chkImproveLayoutOfGroups);
            this.pnlLayoutStyleUseGroups.Controls.Add(this.nudGroupRectanglePenWidth);
            this.pnlLayoutStyleUseGroups.Controls.Add(this.label5);
            this.pnlLayoutStyleUseGroups.Location = new System.Drawing.Point(38, 80);
            this.pnlLayoutStyleUseGroups.Name = "pnlLayoutStyleUseGroups";
            this.pnlLayoutStyleUseGroups.Size = new System.Drawing.Size(292, 129);
            this.pnlLayoutStyleUseGroups.TabIndex = 2;
            // 
            // cbxBoxLayoutAlgorithm
            // 
            this.cbxBoxLayoutAlgorithm.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxBoxLayoutAlgorithm.FormattingEnabled = true;
            this.cbxBoxLayoutAlgorithm.Location = new System.Drawing.Point(150, 3);
            this.cbxBoxLayoutAlgorithm.Name = "cbxBoxLayoutAlgorithm";
            this.cbxBoxLayoutAlgorithm.Size = new System.Drawing.Size(130, 21);
            this.cbxBoxLayoutAlgorithm.TabIndex = 1;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(3, 7);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(104, 13);
            this.label8.TabIndex = 0;
            this.label8.Text = "&Box layout algorithm:";
            // 
            // cbxIntergroupEdgeStyle
            // 
            this.cbxIntergroupEdgeStyle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxIntergroupEdgeStyle.FormattingEnabled = true;
            this.cbxIntergroupEdgeStyle.Location = new System.Drawing.Point(150, 60);
            this.cbxIntergroupEdgeStyle.Name = "cbxIntergroupEdgeStyle";
            this.cbxIntergroupEdgeStyle.Size = new System.Drawing.Size(80, 21);
            this.cbxIntergroupEdgeStyle.TabIndex = 5;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(3, 64);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(90, 13);
            this.label6.TabIndex = 4;
            this.label6.Text = "Intergroup &edges:";
            // 
            // chkImproveLayoutOfGroups
            // 
            this.chkImproveLayoutOfGroups.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
            this.chkImproveLayoutOfGroups.Location = new System.Drawing.Point(3, 91);
            this.chkImproveLayoutOfGroups.Name = "chkImproveLayoutOfGroups";
            this.chkImproveLayoutOfGroups.Size = new System.Drawing.Size(277, 34);
            this.chkImproveLayoutOfGroups.TabIndex = 6;
            this.chkImproveLayoutOfGroups.Text = "&Use the Grid layout for groups that don\'t have many edges";
            this.chkImproveLayoutOfGroups.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.chkImproveLayoutOfGroups.UseVisualStyleBackColor = true;
            // 
            // nudGroupRectanglePenWidth
            // 
            this.nudGroupRectanglePenWidth.Location = new System.Drawing.Point(150, 32);
            this.nudGroupRectanglePenWidth.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudGroupRectanglePenWidth.Name = "nudGroupRectanglePenWidth";
            this.nudGroupRectanglePenWidth.Size = new System.Drawing.Size(49, 20);
            this.nudGroupRectanglePenWidth.TabIndex = 3;
            this.nudGroupRectanglePenWidth.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 36);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(127, 13);
            this.label5.TabIndex = 2;
            this.label5.Text = "&Width of the box outlines:";
            // 
            // radLayoutStyleUseBinning
            // 
            this.radLayoutStyleUseBinning.Location = new System.Drawing.Point(9, 215);
            this.radLayoutStyleUseBinning.Name = "radLayoutStyleUseBinning";
            this.radLayoutStyleUseBinning.Size = new System.Drawing.Size(321, 38);
            this.radLayoutStyleUseBinning.TabIndex = 3;
            this.radLayoutStyleUseBinning.TabStop = true;
            this.radLayoutStyleUseBinning.Text = "La&y out the graph\'s smaller connected components in boxes at the bottom of the g" +
    "raph pane";
            this.radLayoutStyleUseBinning.UseVisualStyleBackColor = true;
            this.radLayoutStyleUseBinning.CheckedChanged += new System.EventHandler(this.OnEventThatRequiresControlEnabling);
            // 
            // LayoutUserSettingsDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(369, 513);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.btnResetAll);
            this.Controls.Add(this.nudMargin);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LayoutUserSettingsDialog";
            this.Text = "Layout Options";
            ((System.ComponentModel.ISupportInitialize)(this.nudFruchtermanReingoldIterations)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudFruchtermanReingoldC)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMargin)).EndInit();
            this.pnlLayoutStyleUseBinning.ResumeLayout(false);
            this.pnlLayoutStyleUseBinning.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudBinLength)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMaximumVerticesPerBin)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.pnlLayoutStyleUseGroups.ResumeLayout(false);
            this.pnlLayoutStyleUseGroups.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudGroupRectanglePenWidth)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown nudFruchtermanReingoldIterations;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown nudFruchtermanReingoldC;
        private System.Windows.Forms.NumericUpDown nudMargin;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Panel pnlLayoutStyleUseBinning;
        private System.Windows.Forms.NumericUpDown nudBinLength;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown nudMaximumVerticesPerBin;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnResetAll;
        private System.Windows.Forms.RadioButton radLayoutStyleUseGroups;
        private System.Windows.Forms.RadioButton radLayoutStyleNormal;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Panel pnlLayoutStyleUseGroups;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown nudGroupRectanglePenWidth;
        private System.Windows.Forms.CheckBox chkImproveLayoutOfGroups;
        private System.Windows.Forms.Label label6;
        private Smrf.AppLib.ComboBoxPlus cbxIntergroupEdgeStyle;
        private System.Windows.Forms.RadioButton radLayoutStyleUseBinning;
        private Smrf.AppLib.ComboBoxPlus cbxBoxLayoutAlgorithm;
        private System.Windows.Forms.Label label8;
    }
}
