

namespace Smrf.NodeXL.ExcelTemplate
{
    partial class GroupUserSettingsDialog
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
            this.chkDoNotReadGroups = new System.Windows.Forms.CheckBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.radReadVertexColorFromGroups = new System.Windows.Forms.RadioButton();
            this.radReadVertexColorFromVertices = new System.Windows.Forms.RadioButton();
            this.grpVertexColor = new System.Windows.Forms.GroupBox();
            this.grpVertexShape = new System.Windows.Forms.GroupBox();
            this.radReadVertexShapeFromVertices = new System.Windows.Forms.RadioButton();
            this.radReadVertexShapeFromGroups = new System.Windows.Forms.RadioButton();
            this.lblLayout = new System.Windows.Forms.Label();
            this.grpVertexColor.SuspendLayout();
            this.grpVertexShape.SuspendLayout();
            this.SuspendLayout();
            // 
            // chkDoNotReadGroups
            // 
            this.chkDoNotReadGroups.AutoSize = true;
            this.chkDoNotReadGroups.Location = new System.Drawing.Point(12, 12);
            this.chkDoNotReadGroups.Name = "chkDoNotReadGroups";
            this.chkDoNotReadGroups.Size = new System.Drawing.Size(260, 17);
            this.chkDoNotReadGroups.TabIndex = 0;
            this.chkDoNotReadGroups.Text = "S&kip groups -- don\'t show them in the Graph Pane";
            this.chkDoNotReadGroups.UseVisualStyleBackColor = true;
            this.chkDoNotReadGroups.CheckedChanged += new System.EventHandler(this.chkDoNotReadGroups_CheckedChanged);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(334, 326);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(248, 326);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(80, 23);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // radReadVertexColorFromGroups
            // 
            this.radReadVertexColorFromGroups.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
            this.radReadVertexColorFromGroups.Location = new System.Drawing.Point(10, 21);
            this.radReadVertexColorFromGroups.Name = "radReadVertexColorFromGroups";
            this.radReadVertexColorFromGroups.Size = new System.Drawing.Size(380, 32);
            this.radReadVertexColorFromGroups.TabIndex = 0;
            this.radReadVertexColorFromGroups.TabStop = true;
            this.radReadVertexColorFromGroups.Text = "&The colors specified in the Vertex Color column on the Groups worksheet.  (The C" +
                "olor column on the Vertices worksheet is ignored.)";
            this.radReadVertexColorFromGroups.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.radReadVertexColorFromGroups.UseVisualStyleBackColor = true;
            // 
            // radReadVertexColorFromVertices
            // 
            this.radReadVertexColorFromVertices.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
            this.radReadVertexColorFromVertices.Location = new System.Drawing.Point(10, 60);
            this.radReadVertexColorFromVertices.Name = "radReadVertexColorFromVertices";
            this.radReadVertexColorFromVertices.Size = new System.Drawing.Size(380, 44);
            this.radReadVertexColorFromVertices.TabIndex = 1;
            this.radReadVertexColorFromVertices.TabStop = true;
            this.radReadVertexColorFromVertices.Text = "T&he colors specified in the Color column on the Vertices worksheet.  (The Vertex" +
                " Color column on the Groups worksheet is ignored.)";
            this.radReadVertexColorFromVertices.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.radReadVertexColorFromVertices.UseVisualStyleBackColor = true;
            // 
            // grpVertexColor
            // 
            this.grpVertexColor.Controls.Add(this.radReadVertexColorFromVertices);
            this.grpVertexColor.Controls.Add(this.radReadVertexColorFromGroups);
            this.grpVertexColor.Location = new System.Drawing.Point(12, 43);
            this.grpVertexColor.Name = "grpVertexColor";
            this.grpVertexColor.Size = new System.Drawing.Size(403, 109);
            this.grpVertexColor.TabIndex = 1;
            this.grpVertexColor.TabStop = false;
            this.grpVertexColor.Text = "What colors should be used for the groups\' vertices?";
            // 
            // grpVertexShape
            // 
            this.grpVertexShape.Controls.Add(this.radReadVertexShapeFromVertices);
            this.grpVertexShape.Controls.Add(this.radReadVertexShapeFromGroups);
            this.grpVertexShape.Location = new System.Drawing.Point(12, 164);
            this.grpVertexShape.Name = "grpVertexShape";
            this.grpVertexShape.Size = new System.Drawing.Size(402, 109);
            this.grpVertexShape.TabIndex = 2;
            this.grpVertexShape.TabStop = false;
            this.grpVertexShape.Text = "What shapes should be used for the groups\' vertices?";
            // 
            // radReadVertexShapeFromVertices
            // 
            this.radReadVertexShapeFromVertices.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
            this.radReadVertexShapeFromVertices.Location = new System.Drawing.Point(10, 60);
            this.radReadVertexShapeFromVertices.Name = "radReadVertexShapeFromVertices";
            this.radReadVertexShapeFromVertices.Size = new System.Drawing.Size(380, 44);
            this.radReadVertexShapeFromVertices.TabIndex = 1;
            this.radReadVertexShapeFromVertices.TabStop = true;
            this.radReadVertexShapeFromVertices.Text = "The sh&apes specified in the Shape column on the Vertices worksheet.  (The Vertex" +
                " Shape column on the Groups worksheet is ignored.)";
            this.radReadVertexShapeFromVertices.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.radReadVertexShapeFromVertices.UseVisualStyleBackColor = true;
            // 
            // radReadVertexShapeFromGroups
            // 
            this.radReadVertexShapeFromGroups.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
            this.radReadVertexShapeFromGroups.Location = new System.Drawing.Point(10, 21);
            this.radReadVertexShapeFromGroups.Name = "radReadVertexShapeFromGroups";
            this.radReadVertexShapeFromGroups.Size = new System.Drawing.Size(380, 32);
            this.radReadVertexShapeFromGroups.TabIndex = 0;
            this.radReadVertexShapeFromGroups.TabStop = true;
            this.radReadVertexShapeFromGroups.Text = "The &shapes specified in the Vertex Shape column on the Groups worksheet.  (The S" +
                "hape column on the Vertices worksheet is ignored.)";
            this.radReadVertexShapeFromGroups.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.radReadVertexShapeFromGroups.UseVisualStyleBackColor = true;
            // 
            // lblLayout
            // 
            this.lblLayout.Location = new System.Drawing.Point(12, 284);
            this.lblLayout.Name = "lblLayout";
            this.lblLayout.Size = new System.Drawing.Size(402, 36);
            this.lblLayout.TabIndex = 3;
            this.lblLayout.Text = "You can also lay out each group in its own box.  Go to NodeXL, Graph, Layout, Lay" +
                "out Options.";
            // 
            // GroupUserSettingsDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(426, 362);
            this.Controls.Add(this.lblLayout);
            this.Controls.Add(this.grpVertexShape);
            this.Controls.Add(this.grpVertexColor);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.chkDoNotReadGroups);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GroupUserSettingsDialog";
            this.Text = "Group Options";
            this.grpVertexColor.ResumeLayout(false);
            this.grpVertexShape.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox chkDoNotReadGroups;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.RadioButton radReadVertexColorFromVertices;
        private System.Windows.Forms.RadioButton radReadVertexColorFromGroups;
        private System.Windows.Forms.GroupBox grpVertexColor;
        private System.Windows.Forms.GroupBox grpVertexShape;
        private System.Windows.Forms.RadioButton radReadVertexShapeFromVertices;
        private System.Windows.Forms.RadioButton radReadVertexShapeFromGroups;
        private System.Windows.Forms.Label lblLayout;
    }
}
