

namespace Smrf.NodeXL.ExcelTemplate
{
    partial class AboutDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutDialog));
            this.btnOK = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.lblVersion = new System.Windows.Forms.Label();
            this.lnkNodeXLTeamMembers = new Smrf.AppLib.StartProcessLinkLabel();
            this.label3 = new System.Windows.Forms.Label();
            this.lnkSocialMediaResearchFoundation = new Smrf.AppLib.StartProcessLinkLabel();
            this.lnkDonate = new Smrf.AppLib.StartProcessLinkLabel();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnOK.Location = new System.Drawing.Point(552, 391);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(400, 392);
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            // 
            // lblVersion
            // 
            this.lblVersion.AutoSize = true;
            this.lblVersion.Location = new System.Drawing.Point(424, 281);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(126, 13);
            this.lblVersion.TabIndex = 4;
            this.lblVersion.Text = "Version [gets set in code]";
            // 
            // lnkNodeXLTeamMembers
            // 
            this.lnkNodeXLTeamMembers.AutoSize = true;
            this.lnkNodeXLTeamMembers.LinkArea = new System.Windows.Forms.LinkArea(22, 11);
            this.lnkNodeXLTeamMembers.Location = new System.Drawing.Point(424, 86);
            this.lnkNodeXLTeamMembers.Margin = new System.Windows.Forms.Padding(0);
            this.lnkNodeXLTeamMembers.Name = "lnkNodeXLTeamMembers";
            this.lnkNodeXLTeamMembers.Size = new System.Drawing.Size(183, 17);
            this.lnkNodeXLTeamMembers.TabIndex = 2;
            this.lnkNodeXLTeamMembers.TabStop = true;
            this.lnkNodeXLTeamMembers.Text = "It was created by the NodeXL team.";
            this.lnkNodeXLTeamMembers.UseCompatibleTextRendering = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label3.Location = new System.Drawing.Point(346, 407);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(66, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Cody Dunne";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lnkSocialMediaResearchFoundation
            // 
            this.lnkSocialMediaResearchFoundation.LinkArea = new System.Windows.Forms.LinkArea(32, 32);
            this.lnkSocialMediaResearchFoundation.Location = new System.Drawing.Point(424, 35);
            this.lnkSocialMediaResearchFoundation.Name = "lnkSocialMediaResearchFoundation";
            this.lnkSocialMediaResearchFoundation.Size = new System.Drawing.Size(203, 40);
            this.lnkSocialMediaResearchFoundation.TabIndex = 1;
            this.lnkSocialMediaResearchFoundation.TabStop = true;
            this.lnkSocialMediaResearchFoundation.Text = "NodeXL is brought to you by the Social Media Research Foundation.";
            this.lnkSocialMediaResearchFoundation.UseCompatibleTextRendering = true;
            // 
            // lnkDonate
            // 
            this.lnkDonate.LinkArea = new System.Windows.Forms.LinkArea(127, 8);
            this.lnkDonate.Location = new System.Drawing.Point(424, 123);
            this.lnkDonate.Name = "lnkDonate";
            this.lnkDonate.Size = new System.Drawing.Size(203, 111);
            this.lnkDonate.TabIndex = 3;
            this.lnkDonate.TabStop = true;
            this.lnkDonate.Text = resources.GetString("lnkDonate.Text");
            this.lnkDonate.UseCompatibleTextRendering = true;
            // 
            // AboutDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnOK;
            this.ClientSize = new System.Drawing.Size(639, 426);
            this.Controls.Add(this.lnkDonate);
            this.Controls.Add(this.lnkSocialMediaResearchFoundation);
            this.Controls.Add(this.lnkNodeXLTeamMembers);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblVersion);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutDialog";
            this.Text = "About NodeXL";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label lblVersion;
        private Smrf.AppLib.StartProcessLinkLabel lnkNodeXLTeamMembers;
        private System.Windows.Forms.Label label3;
        private Smrf.AppLib.StartProcessLinkLabel lnkSocialMediaResearchFoundation;
        private Smrf.AppLib.StartProcessLinkLabel lnkDonate;
    }
}
