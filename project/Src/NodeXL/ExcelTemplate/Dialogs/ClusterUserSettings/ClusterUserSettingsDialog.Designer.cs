

namespace Smrf.NodeXL.ExcelTemplate
{
    partial class ClusterUserSettingsDialog
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
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.radClausetNewmanMoore = new System.Windows.Forms.RadioButton();
            this.radGirvanNewman = new System.Windows.Forms.RadioButton();
            this.radWakitaTsurumi = new System.Windows.Forms.RadioButton();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.chkPutNeighborlessVerticesInOneCluster = new System.Windows.Forms.CheckBox();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(319, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Group the graph\'s vertices into clusters using this cluster algorithm:";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.radClausetNewmanMoore);
            this.panel1.Controls.Add(this.radGirvanNewman);
            this.panel1.Controls.Add(this.radWakitaTsurumi);
            this.panel1.Location = new System.Drawing.Point(15, 25);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(280, 81);
            this.panel1.TabIndex = 1;
            // 
            // radClausetNewmanMoore
            // 
            this.radClausetNewmanMoore.AutoSize = true;
            this.radClausetNewmanMoore.Location = new System.Drawing.Point(11, 6);
            this.radClausetNewmanMoore.Name = "radClausetNewmanMoore";
            this.radClausetNewmanMoore.Size = new System.Drawing.Size(138, 17);
            this.radClausetNewmanMoore.TabIndex = 0;
            this.radClausetNewmanMoore.TabStop = true;
            this.radClausetNewmanMoore.Text = "&Clauset-Newman-Moore";
            this.radClausetNewmanMoore.UseVisualStyleBackColor = true;
            // 
            // radGirvanNewman
            // 
            this.radGirvanNewman.AutoSize = true;
            this.radGirvanNewman.Location = new System.Drawing.Point(11, 52);
            this.radGirvanNewman.Name = "radGirvanNewman";
            this.radGirvanNewman.Size = new System.Drawing.Size(241, 17);
            this.radGirvanNewman.TabIndex = 2;
            this.radGirvanNewman.TabStop = true;
            this.radGirvanNewman.Text = "&Girvan-Newman (slower, for small graphs only)";
            this.radGirvanNewman.UseVisualStyleBackColor = true;
            // 
            // radWakitaTsurumi
            // 
            this.radWakitaTsurumi.AutoSize = true;
            this.radWakitaTsurumi.Location = new System.Drawing.Point(11, 29);
            this.radWakitaTsurumi.Name = "radWakitaTsurumi";
            this.radWakitaTsurumi.Size = new System.Drawing.Size(99, 17);
            this.radWakitaTsurumi.TabIndex = 1;
            this.radWakitaTsurumi.TabStop = true;
            this.radWakitaTsurumi.Text = "&Wakita-Tsurumi";
            this.radWakitaTsurumi.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(279, 141);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 23);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(193, 141);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(80, 23);
            this.btnOK.TabIndex = 3;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // chkPutNeighborlessVerticesInOneCluster
            // 
            this.chkPutNeighborlessVerticesInOneCluster.AutoSize = true;
            this.chkPutNeighborlessVerticesInOneCluster.Location = new System.Drawing.Point(15, 111);
            this.chkPutNeighborlessVerticesInOneCluster.Name = "chkPutNeighborlessVerticesInOneCluster";
            this.chkPutNeighborlessVerticesInOneCluster.Size = new System.Drawing.Size(205, 17);
            this.chkPutNeighborlessVerticesInOneCluster.TabIndex = 2;
            this.chkPutNeighborlessVerticesInOneCluster.Text = "&Put all neighborless vertices into one group";
            this.chkPutNeighborlessVerticesInOneCluster.UseVisualStyleBackColor = true;
            // 
            // ClusterUserSettingsDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(371, 176);
            this.Controls.Add(this.chkPutNeighborlessVerticesInOneCluster);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ClusterUserSettingsDialog";
            this.Text = "Group by Cluster";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton radClausetNewmanMoore;
        private System.Windows.Forms.RadioButton radGirvanNewman;
        private System.Windows.Forms.RadioButton radWakitaTsurumi;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.CheckBox chkPutNeighborlessVerticesInOneCluster;
    }
}
