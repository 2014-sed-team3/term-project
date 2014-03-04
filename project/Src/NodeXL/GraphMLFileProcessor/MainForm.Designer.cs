
namespace Smrf.NodeXL.GraphMLFileProcessor
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
            this.label1 = new System.Windows.Forms.Label();
            this.txbStatus = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnStartStop = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnHelp = new System.Windows.Forms.Button();
            this.usrGraphMLFolderPath = new Smrf.AppLib.FolderPathControl();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(283, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "&Process new GraphML files in this folder and its subfolders:";
            // 
            // txbStatus
            // 
            this.txbStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txbStatus.Location = new System.Drawing.Point(15, 80);
            this.txbStatus.Multiline = true;
            this.txbStatus.Name = "txbStatus";
            this.txbStatus.ReadOnly = true;
            this.txbStatus.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txbStatus.Size = new System.Drawing.Size(419, 132);
            this.txbStatus.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 60);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "S&tatus:";
            // 
            // btnStartStop
            // 
            this.btnStartStop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStartStop.Location = new System.Drawing.Point(446, 98);
            this.btnStartStop.Name = "btnStartStop";
            this.btnStartStop.Size = new System.Drawing.Size(75, 23);
            this.btnStartStop.TabIndex = 4;
            this.btnStartStop.Text = "&Start";
            this.btnStartStop.UseVisualStyleBackColor = true;
            this.btnStartStop.Click += new System.EventHandler(this.btnStartStop_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(446, 175);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 6;
            this.btnClose.Text = "&Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnHelp
            // 
            this.btnHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnHelp.Location = new System.Drawing.Point(446, 146);
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new System.Drawing.Size(75, 23);
            this.btnHelp.TabIndex = 5;
            this.btnHelp.Text = "&Help";
            this.btnHelp.UseVisualStyleBackColor = true;
            this.btnHelp.Click += new System.EventHandler(this.btnHelp_Click);
            // 
            // usrGraphMLFolderPath
            // 
            this.usrGraphMLFolderPath.BrowsePrompt = "Browse for a folder.";
            this.usrGraphMLFolderPath.FolderPath = "";
            this.usrGraphMLFolderPath.Location = new System.Drawing.Point(15, 29);
            this.usrGraphMLFolderPath.Name = "usrGraphMLFolderPath";
            this.usrGraphMLFolderPath.Size = new System.Drawing.Size(419, 24);
            this.usrGraphMLFolderPath.TabIndex = 1;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(532, 230);
            this.Controls.Add(this.btnHelp);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnStartStop);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txbStatus);
            this.Controls.Add(this.usrGraphMLFolderPath);
            this.Controls.Add(this.label1);
            this.MinimumSize = new System.Drawing.Size(548, 268);
            this.Name = "MainForm";
            this.Text = "GraphML File Processor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private Smrf.AppLib.FolderPathControl usrGraphMLFolderPath;
        private System.Windows.Forms.TextBox txbStatus;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnStartStop;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnHelp;
    }
}

