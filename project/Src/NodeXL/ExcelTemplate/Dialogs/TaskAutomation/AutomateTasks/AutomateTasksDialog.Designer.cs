
namespace Smrf.NodeXL.ExcelTemplate
{
    partial class AutomateTasksDialog
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
            this.components = new System.ComponentModel.Container();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnUncheckAll = new System.Windows.Forms.Button();
            this.btnCheckAll = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.btnOptions = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.usrFolderToAutomate = new Smrf.AppLib.FolderPathControl();
            this.lblNote = new System.Windows.Forms.Label();
            this.radAutomateThisWorkbookOnly = new System.Windows.Forms.RadioButton();
            this.radAutomateFolder = new System.Windows.Forms.RadioButton();
            this.clbTasksToRun = new Smrf.AppLib.CheckedListBoxPlus();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(276, 318);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 6;
            this.btnOK.Text = "Run";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(357, 318);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnUncheckAll
            // 
            this.btnUncheckAll.Location = new System.Drawing.Point(357, 76);
            this.btnUncheckAll.Name = "btnUncheckAll";
            this.btnUncheckAll.Size = new System.Drawing.Size(75, 23);
            this.btnUncheckAll.TabIndex = 3;
            this.btnUncheckAll.Text = "&Deselect All";
            this.btnUncheckAll.UseVisualStyleBackColor = true;
            this.btnUncheckAll.Click += new System.EventHandler(this.OnCheckOrUncheckAll);
            // 
            // btnCheckAll
            // 
            this.btnCheckAll.Location = new System.Drawing.Point(357, 47);
            this.btnCheckAll.Name = "btnCheckAll";
            this.btnCheckAll.Size = new System.Drawing.Size(75, 23);
            this.btnCheckAll.TabIndex = 2;
            this.btnCheckAll.Text = "&Select All";
            this.btnCheckAll.UseVisualStyleBackColor = true;
            this.btnCheckAll.Click += new System.EventHandler(this.OnCheckOrUncheckAll);
            // 
            // btnOptions
            // 
            this.btnOptions.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnOptions.Location = new System.Drawing.Point(357, 113);
            this.btnOptions.Name = "btnOptions";
            this.btnOptions.Size = new System.Drawing.Size(75, 23);
            this.btnOptions.TabIndex = 4;
            this.btnOptions.Text = "&Options...";
            this.toolTip1.SetToolTip(this.btnOptions, "Edit the options for the selected task");
            this.btnOptions.UseVisualStyleBackColor = true;
            this.btnOptions.Click += new System.EventHandler(this.btnOptions_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.usrFolderToAutomate);
            this.groupBox2.Controls.Add(this.lblNote);
            this.groupBox2.Controls.Add(this.radAutomateThisWorkbookOnly);
            this.groupBox2.Controls.Add(this.radAutomateFolder);
            this.groupBox2.Location = new System.Drawing.Point(12, 193);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(335, 115);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            // 
            // usrFolderToAutomate
            // 
            this.usrFolderToAutomate.BrowsePrompt = "Browse for a folder containing NodeXL workbooks.";
            this.usrFolderToAutomate.FolderPath = "";
            this.usrFolderToAutomate.Location = new System.Drawing.Point(32, 64);
            this.usrFolderToAutomate.Name = "usrFolderToAutomate";
            this.usrFolderToAutomate.Size = new System.Drawing.Size(290, 24);
            this.usrFolderToAutomate.TabIndex = 2;
            // 
            // lblNote
            // 
            this.lblNote.AutoSize = true;
            this.lblNote.Location = new System.Drawing.Point(32, 88);
            this.lblNote.Name = "lblNote";
            this.lblNote.Size = new System.Drawing.Size(138, 13);
            this.lblNote.TabIndex = 3;
            this.lblNote.Text = "(Excludes open workbooks)";
            // 
            // radAutomateThisWorkbookOnly
            // 
            this.radAutomateThisWorkbookOnly.AutoSize = true;
            this.radAutomateThisWorkbookOnly.Location = new System.Drawing.Point(13, 16);
            this.radAutomateThisWorkbookOnly.Name = "radAutomateThisWorkbookOnly";
            this.radAutomateThisWorkbookOnly.Size = new System.Drawing.Size(108, 17);
            this.radAutomateThisWorkbookOnly.TabIndex = 0;
            this.radAutomateThisWorkbookOnly.TabStop = true;
            this.radAutomateThisWorkbookOnly.Text = "On &this workbook";
            this.radAutomateThisWorkbookOnly.UseVisualStyleBackColor = true;
            this.radAutomateThisWorkbookOnly.CheckedChanged += new System.EventHandler(this.OnEventThatRequiresControlEnabling);
            // 
            // radAutomateFolder
            // 
            this.radAutomateFolder.AutoSize = true;
            this.radAutomateFolder.Location = new System.Drawing.Point(13, 41);
            this.radAutomateFolder.Name = "radAutomateFolder";
            this.radAutomateFolder.Size = new System.Drawing.Size(222, 17);
            this.radAutomateFolder.TabIndex = 1;
            this.radAutomateFolder.TabStop = true;
            this.radAutomateFolder.Text = "On every &NodeXL workbook in this folder:";
            this.radAutomateFolder.UseVisualStyleBackColor = true;
            this.radAutomateFolder.CheckedChanged += new System.EventHandler(this.OnEventThatRequiresControlEnabling);
            // 
            // clbTasksToRun
            // 
            this.clbTasksToRun.CheckOnClick = true;
            this.clbTasksToRun.FormattingEnabled = true;
            this.clbTasksToRun.Location = new System.Drawing.Point(12, 32);
            this.clbTasksToRun.Name = "clbTasksToRun";
            this.clbTasksToRun.Size = new System.Drawing.Size(335, 154);
            this.clbTasksToRun.TabIndex = 1;
            this.clbTasksToRun.SelectedIndexChanged += new System.EventHandler(this.OnEventThatRequiresControlEnabling);
            this.clbTasksToRun.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.clbTasksToRun_ItemCheck);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(87, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "&Run these tasks:";
            // 
            // AutomateTasksDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(444, 355);
            this.Controls.Add(this.btnOptions);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.clbTasksToRun);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCheckAll);
            this.Controls.Add(this.btnUncheckAll);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AutomateTasksDialog";
            this.Text = "Automate";
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnUncheckAll;
        private System.Windows.Forms.Button btnCheckAll;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.RadioButton radAutomateFolder;
        private System.Windows.Forms.RadioButton radAutomateThisWorkbookOnly;
        private System.Windows.Forms.Label lblNote;
        private System.Windows.Forms.GroupBox groupBox2;
        private Smrf.AppLib.FolderPathControl usrFolderToAutomate;
        private Smrf.AppLib.CheckedListBoxPlus clbTasksToRun;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnOptions;
    }
}

