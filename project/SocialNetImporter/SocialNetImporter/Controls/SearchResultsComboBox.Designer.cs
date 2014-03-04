namespace Smrf.NodeXL.GraphDataProviders.Facebook
{
    partial class SearchResultsComboBox
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
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblDescription = new System.Windows.Forms.Label();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.lblLikes = new System.Windows.Forms.Label();
            this.lblTalking = new System.Windows.Forms.Label();
            this.lblMembers = new System.Windows.Forms.Label();
            this.pbProfilePicture = new System.Windows.Forms.PictureBox();
            this.flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbProfilePicture)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Location = new System.Drawing.Point(72, 4);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(39, 16);
            this.lblTitle.TabIndex = 1;
            this.lblTitle.Text = "Title";
            this.lblTitle.MouseLeave += new System.EventHandler(this.SearchResultsComboBox_MouseLeave);
            this.lblTitle.Click += new System.EventHandler(this.pbProfilePicture_Click);
            this.lblTitle.MouseEnter += new System.EventHandler(this.SearchResultsComboBox_MouseEnter);
            // 
            // lblDescription
            // 
            this.lblDescription.AutoSize = true;
            this.lblDescription.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDescription.Location = new System.Drawing.Point(72, 20);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(69, 15);
            this.lblDescription.TabIndex = 2;
            this.lblDescription.Text = "Description";
            this.lblDescription.MouseLeave += new System.EventHandler(this.SearchResultsComboBox_MouseLeave);
            this.lblDescription.Click += new System.EventHandler(this.pbProfilePicture_Click);
            this.lblDescription.MouseEnter += new System.EventHandler(this.SearchResultsComboBox_MouseEnter);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.lblLikes);
            this.flowLayoutPanel1.Controls.Add(this.lblTalking);
            this.flowLayoutPanel1.Controls.Add(this.lblMembers);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(75, 37);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(300, 20);
            this.flowLayoutPanel1.TabIndex = 5;
            this.flowLayoutPanel1.MouseLeave += new System.EventHandler(this.SearchResultsComboBox_MouseLeave);
            this.flowLayoutPanel1.Click += new System.EventHandler(this.pbProfilePicture_Click);
            this.flowLayoutPanel1.MouseEnter += new System.EventHandler(this.SearchResultsComboBox_MouseEnter);
            // 
            // lblLikes
            // 
            this.lblLikes.AutoSize = true;
            this.lblLikes.Location = new System.Drawing.Point(3, 0);
            this.lblLikes.Name = "lblLikes";
            this.lblLikes.Size = new System.Drawing.Size(57, 13);
            this.lblLikes.TabIndex = 5;
            this.lblLikes.Text = "12 like this";
            this.lblLikes.MouseLeave += new System.EventHandler(this.SearchResultsComboBox_MouseLeave);
            this.lblLikes.Click += new System.EventHandler(this.pbProfilePicture_Click);
            this.lblLikes.MouseEnter += new System.EventHandler(this.SearchResultsComboBox_MouseEnter);
            // 
            // lblTalking
            // 
            this.lblTalking.AutoSize = true;
            this.lblTalking.Location = new System.Drawing.Point(66, 0);
            this.lblTalking.Name = "lblTalking";
            this.lblTalking.Size = new System.Drawing.Size(88, 13);
            this.lblTalking.TabIndex = 6;
            this.lblTalking.Text = "25 talk about this";
            this.lblTalking.MouseLeave += new System.EventHandler(this.SearchResultsComboBox_MouseLeave);
            this.lblTalking.Click += new System.EventHandler(this.pbProfilePicture_Click);
            this.lblTalking.MouseEnter += new System.EventHandler(this.SearchResultsComboBox_MouseEnter);
            // 
            // lblMembers
            // 
            this.lblMembers.AutoSize = true;
            this.lblMembers.Location = new System.Drawing.Point(160, 0);
            this.lblMembers.Name = "lblMembers";
            this.lblMembers.Size = new System.Drawing.Size(64, 13);
            this.lblMembers.TabIndex = 7;
            this.lblMembers.Text = "20 members";
            this.lblMembers.MouseLeave += new System.EventHandler(this.SearchResultsComboBox_MouseLeave);
            this.lblMembers.Click += new System.EventHandler(this.pbProfilePicture_Click);
            this.lblMembers.MouseEnter += new System.EventHandler(this.SearchResultsComboBox_MouseEnter);
            // 
            // pbProfilePicture
            // 
            this.pbProfilePicture.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbProfilePicture.Location = new System.Drawing.Point(5, 4);
            this.pbProfilePicture.Name = "pbProfilePicture";
            this.pbProfilePicture.Size = new System.Drawing.Size(50, 50);
            this.pbProfilePicture.TabIndex = 0;
            this.pbProfilePicture.TabStop = false;
            this.pbProfilePicture.MouseLeave += new System.EventHandler(this.SearchResultsComboBox_MouseLeave);
            this.pbProfilePicture.Click += new System.EventHandler(this.pbProfilePicture_Click);
            this.pbProfilePicture.MouseEnter += new System.EventHandler(this.SearchResultsComboBox_MouseEnter);
            // 
            // SearchResultsComboBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.pbProfilePicture);
            this.Controls.Add(this.lblTitle);
            this.Name = "SearchResultsComboBox";
            this.Size = new System.Drawing.Size(397, 60);
            this.MouseLeave += new System.EventHandler(this.SearchResultsComboBox_MouseLeave);
            this.Click += new System.EventHandler(this.pbProfilePicture_Click);
            this.MouseEnter += new System.EventHandler(this.SearchResultsComboBox_MouseEnter);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbProfilePicture)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pbProfilePicture;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Label lblTalking;
        private System.Windows.Forms.Label lblLikes;
        private System.Windows.Forms.Label lblMembers;
    }
}
