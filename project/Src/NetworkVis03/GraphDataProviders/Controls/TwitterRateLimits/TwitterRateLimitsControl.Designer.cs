namespace Smrf.NodeXL.GraphDataProviders.Twitter
{
    partial class TwitterRateLimitsControl
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
            this.lnkRateLimiting = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // lnkRateLimiting
            // 
            this.lnkRateLimiting.AutoSize = true;
            this.lnkRateLimiting.Location = new System.Drawing.Point(0, 0);
            this.lnkRateLimiting.Name = "lnkRateLimiting";
            this.lnkRateLimiting.Size = new System.Drawing.Size(90, 13);
            this.lnkRateLimiting.TabIndex = 0;
            this.lnkRateLimiting.TabStop = true;
            this.lnkRateLimiting.Text = "[Gets set in code]";
            this.lnkRateLimiting.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkRateLimiting_LinkClicked);
            // 
            // TwitterRateLimitsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lnkRateLimiting);
            this.Name = "TwitterRateLimitsControl";
            this.Size = new System.Drawing.Size(352, 14);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.LinkLabel lnkRateLimiting;

    }
}
