

namespace Smrf.NodeXL.ExcelTemplate
{
    partial class ExportToNodeXLGraphGalleryDialog
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
            this.lnkNodeXLGraphGallery = new Smrf.AppLib.StartProcessLinkLabel();
            this.label4 = new System.Windows.Forms.Label();
            this.txbSpaceDelimitedTags = new System.Windows.Forms.TextBox();
            this.txbAuthor = new System.Windows.Forms.TextBox();
            this.lblAuthor = new System.Windows.Forms.Label();
            this.lblPassword = new System.Windows.Forms.Label();
            this.txbPassword = new System.Windows.Forms.TextBox();
            this.radAsGuest = new System.Windows.Forms.RadioButton();
            this.txbGuestName = new System.Windows.Forms.TextBox();
            this.lblGuestName = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lnkCreateAccountHelp = new Smrf.AppLib.HelpLinkLabel();
            this.lnkCreateAccount = new Smrf.AppLib.StartProcessLinkLabel();
            this.radUseCredentials = new System.Windows.Forms.RadioButton();
            this.usrExportedFiles = new Smrf.NodeXL.ExcelTemplate.ExportedFilesControl();
            this.usrExportedFilesDescription = new Smrf.NodeXL.ExcelTemplate.ExportedFilesDescriptionControl();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(485, 523);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 23);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(399, 523);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(80, 23);
            this.btnOK.TabIndex = 5;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // lnkNodeXLGraphGallery
            // 
            this.lnkNodeXLGraphGallery.AutoSize = true;
            this.lnkNodeXLGraphGallery.LinkArea = new System.Windows.Forms.LinkArea(42, 20);
            this.lnkNodeXLGraphGallery.Location = new System.Drawing.Point(12, 9);
            this.lnkNodeXLGraphGallery.Name = "lnkNodeXLGraphGallery";
            this.lnkNodeXLGraphGallery.Size = new System.Drawing.Size(372, 17);
            this.lnkNodeXLGraphGallery.TabIndex = 7;
            this.lnkNodeXLGraphGallery.TabStop = true;
            this.lnkNodeXLGraphGallery.Text = "This exports an image of the graph to the NodeXL Graph Gallery website.";
            this.lnkNodeXLGraphGallery.UseCompatibleTextRendering = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 148);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(143, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "T&ags (separate with spaces):";
            // 
            // txbSpaceDelimitedTags
            // 
            this.txbSpaceDelimitedTags.Location = new System.Drawing.Point(170, 148);
            this.txbSpaceDelimitedTags.MaxLength = 200;
            this.txbSpaceDelimitedTags.Name = "txbSpaceDelimitedTags";
            this.txbSpaceDelimitedTags.Size = new System.Drawing.Size(395, 20);
            this.txbSpaceDelimitedTags.TabIndex = 2;
            // 
            // txbAuthor
            // 
            this.txbAuthor.Location = new System.Drawing.Point(125, 97);
            this.txbAuthor.MaxLength = 50;
            this.txbAuthor.Name = "txbAuthor";
            this.txbAuthor.Size = new System.Drawing.Size(211, 20);
            this.txbAuthor.TabIndex = 5;
            // 
            // lblAuthor
            // 
            this.lblAuthor.AutoSize = true;
            this.lblAuthor.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAuthor.Location = new System.Drawing.Point(48, 100);
            this.lblAuthor.Name = "lblAuthor";
            this.lblAuthor.Size = new System.Drawing.Size(61, 13);
            this.lblAuthor.TabIndex = 4;
            this.lblAuthor.Text = "&User name:";
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(48, 123);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(53, 13);
            this.lblPassword.TabIndex = 6;
            this.lblPassword.Text = "&Password";
            // 
            // txbPassword
            // 
            this.txbPassword.Location = new System.Drawing.Point(125, 123);
            this.txbPassword.MaxLength = 128;
            this.txbPassword.Name = "txbPassword";
            this.txbPassword.Size = new System.Drawing.Size(211, 20);
            this.txbPassword.TabIndex = 7;
            this.txbPassword.UseSystemPasswordChar = true;
            // 
            // radAsGuest
            // 
            this.radAsGuest.AutoSize = true;
            this.radAsGuest.Location = new System.Drawing.Point(14, 21);
            this.radAsGuest.Name = "radAsGuest";
            this.radAsGuest.Size = new System.Drawing.Size(138, 17);
            this.radAsGuest.TabIndex = 0;
            this.radAsGuest.TabStop = true;
            this.radAsGuest.Text = "&I don\'t have an account";
            this.radAsGuest.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.radAsGuest.UseVisualStyleBackColor = true;
            this.radAsGuest.CheckedChanged += new System.EventHandler(this.OnEventThatRequiresControlEnabling);
            // 
            // txbGuestName
            // 
            this.txbGuestName.Location = new System.Drawing.Point(125, 43);
            this.txbGuestName.MaxLength = 50;
            this.txbGuestName.Name = "txbGuestName";
            this.txbGuestName.Size = new System.Drawing.Size(211, 20);
            this.txbGuestName.TabIndex = 2;
            // 
            // lblGuestName
            // 
            this.lblGuestName.AutoSize = true;
            this.lblGuestName.Location = new System.Drawing.Point(48, 46);
            this.lblGuestName.Name = "lblGuestName";
            this.lblGuestName.Size = new System.Drawing.Size(67, 13);
            this.lblGuestName.TabIndex = 1;
            this.lblGuestName.Text = "Guest &name:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lnkCreateAccountHelp);
            this.groupBox1.Controls.Add(this.lnkCreateAccount);
            this.groupBox1.Controls.Add(this.lblAuthor);
            this.groupBox1.Controls.Add(this.radUseCredentials);
            this.groupBox1.Controls.Add(this.txbAuthor);
            this.groupBox1.Controls.Add(this.txbPassword);
            this.groupBox1.Controls.Add(this.lblGuestName);
            this.groupBox1.Controls.Add(this.lblPassword);
            this.groupBox1.Controls.Add(this.radAsGuest);
            this.groupBox1.Controls.Add(this.txbGuestName);
            this.groupBox1.Location = new System.Drawing.Point(12, 182);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(553, 189);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "NodeXL Graph Gallery account";
            // 
            // lnkCreateAccountHelp
            // 
            this.lnkCreateAccountHelp.AutoSize = true;
            this.lnkCreateAccountHelp.Location = new System.Drawing.Point(219, 156);
            this.lnkCreateAccountHelp.Name = "lnkCreateAccountHelp";
            this.lnkCreateAccountHelp.Size = new System.Drawing.Size(125, 13);
            this.lnkCreateAccountHelp.TabIndex = 9;
            this.lnkCreateAccountHelp.TabStop = true;
            this.lnkCreateAccountHelp.Tag = "If you have an account on the NodeXL Graph Gallery website, you can sign in to th" +
                "e website later and edit or delete any graphs you\'ve exported.  Graphs exported " +
                "by guests cannot be edited or deleted.";
            this.lnkCreateAccountHelp.Text = "Why create an account?";
            // 
            // lnkCreateAccount
            // 
            this.lnkCreateAccount.AutoSize = true;
            this.lnkCreateAccount.Location = new System.Drawing.Point(11, 156);
            this.lnkCreateAccount.Name = "lnkCreateAccount";
            this.lnkCreateAccount.Size = new System.Drawing.Size(198, 13);
            this.lnkCreateAccount.TabIndex = 8;
            this.lnkCreateAccount.TabStop = true;
            this.lnkCreateAccount.Text = "Create a NodeXL Graph Gallery account";
            // 
            // radUseCredentials
            // 
            this.radUseCredentials.AutoSize = true;
            this.radUseCredentials.Location = new System.Drawing.Point(14, 75);
            this.radUseCredentials.Name = "radUseCredentials";
            this.radUseCredentials.Size = new System.Drawing.Size(102, 17);
            this.radUseCredentials.TabIndex = 3;
            this.radUseCredentials.TabStop = true;
            this.radUseCredentials.Text = "Use &my account";
            this.radUseCredentials.UseVisualStyleBackColor = true;
            this.radUseCredentials.CheckedChanged += new System.EventHandler(this.OnEventThatRequiresControlEnabling);
            // 
            // usrExportedFiles
            // 
            this.usrExportedFiles.ExportGraphML = false;
            this.usrExportedFiles.ExportWorkbookAndSettings = false;
            this.usrExportedFiles.Location = new System.Drawing.Point(12, 385);
            this.usrExportedFiles.Name = "usrExportedFiles";
            this.usrExportedFiles.Size = new System.Drawing.Size(553, 133);
            this.usrExportedFiles.TabIndex = 4;
            this.usrExportedFiles.UseFixedAspectRatio = false;
            // 
            // usrExportedFilesDescription
            // 
            this.usrExportedFilesDescription.Description = "";
            this.usrExportedFilesDescription.DescriptionLabel = "&Description";
            this.usrExportedFilesDescription.Location = new System.Drawing.Point(12, 38);
            this.usrExportedFilesDescription.Name = "usrExportedFilesDescription";
            this.usrExportedFilesDescription.Size = new System.Drawing.Size(553, 100);
            this.usrExportedFilesDescription.TabIndex = 0;
            this.usrExportedFilesDescription.Title = "";
            this.usrExportedFilesDescription.TitleLabel = "&Title";
            // 
            // ExportToNodeXLGraphGalleryDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(580, 561);
            this.Controls.Add(this.usrExportedFilesDescription);
            this.Controls.Add(this.usrExportedFiles);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.txbSpaceDelimitedTags);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lnkNodeXLGraphGallery);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ExportToNodeXLGraphGalleryDialog";
            this.Text = "Export to NodeXL Graph Gallery";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private Smrf.AppLib.StartProcessLinkLabel lnkNodeXLGraphGallery;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txbSpaceDelimitedTags;
        private System.Windows.Forms.TextBox txbAuthor;
        private System.Windows.Forms.Label lblAuthor;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.TextBox txbPassword;
        private System.Windows.Forms.RadioButton radAsGuest;
        private System.Windows.Forms.TextBox txbGuestName;
        private System.Windows.Forms.Label lblGuestName;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radUseCredentials;
		private Smrf.AppLib.HelpLinkLabel lnkCreateAccountHelp;
        private Smrf.AppLib.StartProcessLinkLabel lnkCreateAccount;
        private ExportedFilesControl usrExportedFiles;
        private ExportedFilesDescriptionControl usrExportedFilesDescription;
    }
}
