
namespace Smrf.NodeXL.ExcelTemplate
{
    partial class ImportDataUserSettingsDialog
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
            this.chkClearTablesBeforeImport = new System.Windows.Forms.CheckBox();
            this.chkSaveImportDescription = new System.Windows.Forms.CheckBox();
            this.chkAutomateAfterImport = new System.Windows.Forms.CheckBox();
            this.btnAutomateTasksUserSettings = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.usrPlugInFolderPath = new Smrf.AppLib.FolderPathControl();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(172, 172);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(80, 23);
            this.btnOK.TabIndex = 6;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(258, 172);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 23);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // chkClearTablesBeforeImport
            // 
            this.chkClearTablesBeforeImport.AutoSize = true;
            this.chkClearTablesBeforeImport.Location = new System.Drawing.Point(12, 12);
            this.chkClearTablesBeforeImport.Name = "chkClearTablesBeforeImport";
            this.chkClearTablesBeforeImport.Size = new System.Drawing.Size(288, 17);
            this.chkClearTablesBeforeImport.TabIndex = 0;
            this.chkClearTablesBeforeImport.Text = "&Clear the NodeXL workbook before the data is imported";
            this.chkClearTablesBeforeImport.UseVisualStyleBackColor = true;
            // 
            // chkSaveImportDescription
            // 
            this.chkSaveImportDescription.AutoSize = true;
            this.chkSaveImportDescription.Location = new System.Drawing.Point(12, 35);
            this.chkSaveImportDescription.Name = "chkSaveImportDescription";
            this.chkSaveImportDescription.Size = new System.Drawing.Size(309, 17);
            this.chkSaveImportDescription.TabIndex = 1;
            this.chkSaveImportDescription.Text = "Add a &description of the imported data to the graph summary";
            this.chkSaveImportDescription.UseVisualStyleBackColor = true;
            // 
            // chkAutomateAfterImport
            // 
            this.chkAutomateAfterImport.AutoSize = true;
            this.chkAutomateAfterImport.Location = new System.Drawing.Point(12, 58);
            this.chkAutomateAfterImport.Name = "chkAutomateAfterImport";
            this.chkAutomateAfterImport.Size = new System.Drawing.Size(238, 17);
            this.chkAutomateAfterImport.TabIndex = 2;
            this.chkAutomateAfterImport.Text = "&Automate the graph after the data is imported";
            this.chkAutomateAfterImport.UseVisualStyleBackColor = true;
            // 
            // btnAutomateTasksUserSettings
            // 
            this.btnAutomateTasksUserSettings.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAutomateTasksUserSettings.Image = global::Smrf.NodeXL.ExcelTemplate.Properties.Resources.Options;
            this.btnAutomateTasksUserSettings.Location = new System.Drawing.Point(258, 54);
            this.btnAutomateTasksUserSettings.Name = "btnAutomateTasksUserSettings";
            this.btnAutomateTasksUserSettings.Size = new System.Drawing.Size(23, 23);
            this.btnAutomateTasksUserSettings.TabIndex = 3;
            this.toolTip1.SetToolTip(this.btnAutomateTasksUserSettings, "Automate options");
            this.btnAutomateTasksUserSettings.UseVisualStyleBackColor = true;
            this.btnAutomateTasksUserSettings.Click += new System.EventHandler(this.btnAutomateTasksUserSettings_Click);
            // 
            // usrPlugInFolderPath
            // 
            this.usrPlugInFolderPath.AllowEmptyFolderPath = true;
            this.usrPlugInFolderPath.BrowsePrompt = "Browse for the folder containing third-party graph data importers.";
            this.usrPlugInFolderPath.FolderPath = "";
            this.usrPlugInFolderPath.Location = new System.Drawing.Point(12, 110);
            this.usrPlugInFolderPath.Name = "usrPlugInFolderPath";
            this.usrPlugInFolderPath.Size = new System.Drawing.Size(326, 24);
            this.usrPlugInFolderPath.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 92);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(239, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "&Folder containing third-party graph data importers:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 139);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(260, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "(Folder change takes effect after NodeXL is restarted)";
            // 
            // ImportDataUserSettingsDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(350, 209);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.usrPlugInFolderPath);
            this.Controls.Add(this.btnAutomateTasksUserSettings);
            this.Controls.Add(this.chkAutomateAfterImport);
            this.Controls.Add(this.chkSaveImportDescription);
            this.Controls.Add(this.chkClearTablesBeforeImport);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ImportDataUserSettingsDialog";
            this.Text = "Import Data Options";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.CheckBox chkClearTablesBeforeImport;
        private System.Windows.Forms.CheckBox chkSaveImportDescription;
        private System.Windows.Forms.CheckBox chkAutomateAfterImport;
        private System.Windows.Forms.Button btnAutomateTasksUserSettings;
        private System.Windows.Forms.ToolTip toolTip1;
        private AppLib.FolderPathControl usrPlugInFolderPath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}
