

namespace Smrf.NodeXL.ExcelTemplate
{
    partial class ImportFromWorkbookDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ImportFromWorkbookDialog));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lbxSourceWorkbook = new Smrf.AppLib.ExcelWorkbookListBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.cbxSourceColumnsHaveHeaders = new System.Windows.Forms.CheckBox();
            this.lblVertex1 = new System.Windows.Forms.Label();
            this.cbxVertex1 = new System.Windows.Forms.ComboBox();
            this.cbxVertex2 = new System.Windows.Forms.ComboBox();
            this.lblVertex2 = new System.Windows.Forms.Label();
            this.pnlVertices = new System.Windows.Forms.Panel();
            this.lnkVertexHelp = new Smrf.AppLib.HelpLinkLabel();
            this.dgvSourceColumns = new System.Windows.Forms.DataGridView();
            this.colColumnName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colIsEdgeColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colIsVertex1Property = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colIsVertex2Property = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.btnCheckAllIsEdgeColumnCheckBoxes = new System.Windows.Forms.Button();
            this.btnUncheckAllIsEdgeColumnCheckBoxes = new System.Windows.Forms.Button();
            this.lnkEdgeOrderHelp = new Smrf.AppLib.HelpLinkLabel();
            this.pnlVertices.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSourceColumns)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(506, 37);
            this.label1.TabIndex = 0;
            this.label1.Text = "Use this to import edges and vertices from an open workbook that contains an edge" +
                " list.  At least two edge columns must be imported: one for Vertex 1 and one for" +
                " Vertex 2.";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(152, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "&Open workbook to import from:";
            // 
            // lbxSourceWorkbook
            // 
            this.lbxSourceWorkbook.FormattingEnabled = true;
            this.lbxSourceWorkbook.Location = new System.Drawing.Point(15, 73);
            this.lbxSourceWorkbook.Name = "lbxSourceWorkbook";
            this.lbxSourceWorkbook.Size = new System.Drawing.Size(280, 69);
            this.lbxSourceWorkbook.TabIndex = 2;
            this.lbxSourceWorkbook.SelectedIndexChanged += new System.EventHandler(this.lbxSourceWorkbook_SelectedIndexChanged);
            // 
            // btnOK
            // 
            this.btnOK.Enabled = false;
            this.btnOK.Location = new System.Drawing.Point(362, 461);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 10;
            this.btnOK.Text = "Import";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(443, 461);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 11;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // cbxSourceColumnsHaveHeaders
            // 
            this.cbxSourceColumnsHaveHeaders.AutoSize = true;
            this.cbxSourceColumnsHaveHeaders.Location = new System.Drawing.Point(15, 155);
            this.cbxSourceColumnsHaveHeaders.Name = "cbxSourceColumnsHaveHeaders";
            this.cbxSourceColumnsHaveHeaders.Size = new System.Drawing.Size(134, 17);
            this.cbxSourceColumnsHaveHeaders.TabIndex = 3;
            this.cbxSourceColumnsHaveHeaders.Text = "&Columns have headers";
            this.cbxSourceColumnsHaveHeaders.UseVisualStyleBackColor = true;
            this.cbxSourceColumnsHaveHeaders.CheckedChanged += new System.EventHandler(this.cbxSourceColumnsHaveHeaders_CheckedChanged);
            // 
            // lblVertex1
            // 
            this.lblVertex1.AutoSize = true;
            this.lblVertex1.Location = new System.Drawing.Point(0, 0);
            this.lblVertex1.Name = "lblVertex1";
            this.lblVertex1.Size = new System.Drawing.Size(160, 13);
            this.lblVertex1.TabIndex = 0;
            this.lblVertex1.Text = "Which edge column is Vertex &1?";
            // 
            // cbxVertex1
            // 
            this.cbxVertex1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxVertex1.FormattingEnabled = true;
            this.cbxVertex1.Location = new System.Drawing.Point(0, 20);
            this.cbxVertex1.Name = "cbxVertex1";
            this.cbxVertex1.Size = new System.Drawing.Size(243, 21);
            this.cbxVertex1.TabIndex = 1;
            this.cbxVertex1.SelectedIndexChanged += new System.EventHandler(this.cbxVertex_SelectedIndexChanged);
            // 
            // cbxVertex2
            // 
            this.cbxVertex2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxVertex2.FormattingEnabled = true;
            this.cbxVertex2.Location = new System.Drawing.Point(0, 75);
            this.cbxVertex2.Name = "cbxVertex2";
            this.cbxVertex2.Size = new System.Drawing.Size(243, 21);
            this.cbxVertex2.TabIndex = 3;
            this.cbxVertex2.SelectedIndexChanged += new System.EventHandler(this.cbxVertex_SelectedIndexChanged);
            // 
            // lblVertex2
            // 
            this.lblVertex2.AutoSize = true;
            this.lblVertex2.Location = new System.Drawing.Point(0, 55);
            this.lblVertex2.Name = "lblVertex2";
            this.lblVertex2.Size = new System.Drawing.Size(160, 13);
            this.lblVertex2.TabIndex = 2;
            this.lblVertex2.Text = "Which edge column is Vertex &2?";
            // 
            // pnlVertices
            // 
            this.pnlVertices.Controls.Add(this.lblVertex1);
            this.pnlVertices.Controls.Add(this.cbxVertex2);
            this.pnlVertices.Controls.Add(this.cbxVertex1);
            this.pnlVertices.Controls.Add(this.lblVertex2);
            this.pnlVertices.Location = new System.Drawing.Point(15, 356);
            this.pnlVertices.Name = "pnlVertices";
            this.pnlVertices.Size = new System.Drawing.Size(248, 103);
            this.pnlVertices.TabIndex = 7;
            // 
            // lnkVertexHelp
            // 
            this.lnkVertexHelp.Location = new System.Drawing.Point(307, 376);
            this.lnkVertexHelp.Name = "lnkVertexHelp";
            this.lnkVertexHelp.Size = new System.Drawing.Size(211, 41);
            this.lnkVertexHelp.TabIndex = 8;
            this.lnkVertexHelp.TabStop = true;
            this.lnkVertexHelp.Tag = resources.GetString("lnkVertexHelp.Tag");
            this.lnkVertexHelp.Text = "What\'s the difference between edge columns and vertex property columns?";
            // 
            // dgvSourceColumns
            // 
            this.dgvSourceColumns.AllowUserToAddRows = false;
            this.dgvSourceColumns.AllowUserToDeleteRows = false;
            this.dgvSourceColumns.AllowUserToResizeColumns = false;
            this.dgvSourceColumns.AllowUserToResizeRows = false;
            this.dgvSourceColumns.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSourceColumns.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colColumnName,
            this.colIsEdgeColumn,
            this.colIsVertex1Property,
            this.colIsVertex2Property});
            this.dgvSourceColumns.Location = new System.Drawing.Point(15, 182);
            this.dgvSourceColumns.Name = "dgvSourceColumns";
            this.dgvSourceColumns.Size = new System.Drawing.Size(503, 143);
            this.dgvSourceColumns.TabIndex = 4;
            this.dgvSourceColumns.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSourceColumns_CellContentClick);
            // 
            // colColumnName
            // 
            this.colColumnName.HeaderText = "Column";
            this.colColumnName.Name = "colColumnName";
            this.colColumnName.ReadOnly = true;
            this.colColumnName.Width = 250;
            // 
            // colIsEdgeColumn
            // 
            this.colIsEdgeColumn.HeaderText = "Is Edge Column";
            this.colIsEdgeColumn.Name = "colIsEdgeColumn";
            this.colIsEdgeColumn.Width = 70;
            // 
            // colIsVertex1Property
            // 
            this.colIsVertex1Property.HeaderText = "Is Vertex 1 Property Column";
            this.colIsVertex1Property.Name = "colIsVertex1Property";
            this.colIsVertex1Property.Width = 70;
            // 
            // colIsVertex2Property
            // 
            this.colIsVertex2Property.HeaderText = "Is Vertex 2 Property Column";
            this.colIsVertex2Property.Name = "colIsVertex2Property";
            this.colIsVertex2Property.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colIsVertex2Property.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.colIsVertex2Property.Width = 70;
            // 
            // btnCheckAllIsEdgeColumnCheckBoxes
            // 
            this.btnCheckAllIsEdgeColumnCheckBoxes.Location = new System.Drawing.Point(362, 333);
            this.btnCheckAllIsEdgeColumnCheckBoxes.Name = "btnCheckAllIsEdgeColumnCheckBoxes";
            this.btnCheckAllIsEdgeColumnCheckBoxes.Size = new System.Drawing.Size(75, 23);
            this.btnCheckAllIsEdgeColumnCheckBoxes.TabIndex = 5;
            this.btnCheckAllIsEdgeColumnCheckBoxes.Text = "&Select All";
            this.btnCheckAllIsEdgeColumnCheckBoxes.UseVisualStyleBackColor = true;
            this.btnCheckAllIsEdgeColumnCheckBoxes.Click += new System.EventHandler(this.btnCheckAllIsEdgeColumnCheckBoxes_Click);
            // 
            // btnUncheckAllIsEdgeColumnCheckBoxes
            // 
            this.btnUncheckAllIsEdgeColumnCheckBoxes.Location = new System.Drawing.Point(443, 333);
            this.btnUncheckAllIsEdgeColumnCheckBoxes.Name = "btnUncheckAllIsEdgeColumnCheckBoxes";
            this.btnUncheckAllIsEdgeColumnCheckBoxes.Size = new System.Drawing.Size(75, 23);
            this.btnUncheckAllIsEdgeColumnCheckBoxes.TabIndex = 6;
            this.btnUncheckAllIsEdgeColumnCheckBoxes.Text = "&Deselect All";
            this.btnUncheckAllIsEdgeColumnCheckBoxes.UseVisualStyleBackColor = true;
            this.btnUncheckAllIsEdgeColumnCheckBoxes.Click += new System.EventHandler(this.btnUncheckAllIsEdgeColumnCheckBoxes_Click);
            // 
            // lnkEdgeOrderHelp
            // 
            this.lnkEdgeOrderHelp.AutoSize = true;
            this.lnkEdgeOrderHelp.Location = new System.Drawing.Point(307, 421);
            this.lnkEdgeOrderHelp.Name = "lnkEdgeOrderHelp";
            this.lnkEdgeOrderHelp.Size = new System.Drawing.Size(82, 13);
            this.lnkEdgeOrderHelp.TabIndex = 9;
            this.lnkEdgeOrderHelp.TabStop = true;
            this.lnkEdgeOrderHelp.Tag = "The order of the rows may be different after they are imported.  You can sort the" +
                " imported rows using Data, Sort if necessary.";
            this.lnkEdgeOrderHelp.Text = "About row order";
            // 
            // ImportFromWorkbookDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(536, 498);
            this.Controls.Add(this.lnkEdgeOrderHelp);
            this.Controls.Add(this.pnlVertices);
            this.Controls.Add(this.lnkVertexHelp);
            this.Controls.Add(this.btnCheckAllIsEdgeColumnCheckBoxes);
            this.Controls.Add(this.btnUncheckAllIsEdgeColumnCheckBoxes);
            this.Controls.Add(this.dgvSourceColumns);
            this.Controls.Add(this.cbxSourceColumnsHaveHeaders);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.lbxSourceWorkbook);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ImportFromWorkbookDialog";
            this.Text = "Import from Open Workbook";
            this.pnlVertices.ResumeLayout(false);
            this.pnlVertices.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSourceColumns)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private Smrf.AppLib.ExcelWorkbookListBox lbxSourceWorkbook;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.CheckBox cbxSourceColumnsHaveHeaders;
        private System.Windows.Forms.Label lblVertex1;
        private System.Windows.Forms.ComboBox cbxVertex1;
        private System.Windows.Forms.ComboBox cbxVertex2;
        private System.Windows.Forms.Label lblVertex2;
        private System.Windows.Forms.Panel pnlVertices;
        private Smrf.AppLib.HelpLinkLabel lnkVertexHelp;
        private System.Windows.Forms.DataGridView dgvSourceColumns;
        private System.Windows.Forms.Button btnCheckAllIsEdgeColumnCheckBoxes;
        private System.Windows.Forms.Button btnUncheckAllIsEdgeColumnCheckBoxes;
        private Smrf.AppLib.HelpLinkLabel lnkEdgeOrderHelp;
        private System.Windows.Forms.DataGridViewTextBoxColumn colColumnName;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colIsEdgeColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colIsVertex1Property;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colIsVertex2Property;
    }
}
