
namespace Smrf.NodeXL.ExcelTemplate
{
    partial class GeneralUserSettingsDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GeneralUserSettingsDialog));
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnCustomizeVertexMenu = new System.Windows.Forms.Button();
            this.btnResetAll = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.usrSelectedVertexColor = new Smrf.AppLib.ColorPicker();
            this.chkAutoSelect = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.radUseActualVertexImageSize = new System.Windows.Forms.RadioButton();
            this.label15 = new System.Windows.Forms.Label();
            this.radUseSpecifiedVertexImageSize = new System.Windows.Forms.RadioButton();
            this.nudVertexImageSize = new System.Windows.Forms.NumericUpDown();
            this.usrVertexColor = new Smrf.AppLib.ColorPicker();
            this.lblVertexAlpha = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.nudVertexRadius = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.nudVertexAlpha = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.cbxVertexShape = new Smrf.AppLib.ComboBoxPlus();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.usrSelectedEdgeColor = new Smrf.AppLib.ColorPicker();
            this.label24 = new System.Windows.Forms.Label();
            this.usrEdgeColor = new Smrf.AppLib.ColorPicker();
            this.label11 = new System.Windows.Forms.Label();
            this.lblEdgeAlpha = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.nudEdgeAlpha = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.nudEdgeWidth = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.nudRelativeArrowSize = new System.Windows.Forms.NumericUpDown();
            this.label17 = new System.Windows.Forms.Label();
            this.usrBackColor = new Smrf.AppLib.ColorPicker();
            this.chkAutoReadWorkbook = new System.Windows.Forms.CheckBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.btnAxisFont = new System.Windows.Forms.Button();
            this.btnLabels = new System.Windows.Forms.Button();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.btnBrowseBackgroundImageUri = new System.Windows.Forms.Button();
            this.txbBackgroundImageUri = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.tcTabControl = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cbxEdgeBundlerStraightening = new Smrf.AppLib.ComboBoxPlus();
            this.cbxEdgeBezierDisplacementFactor = new Smrf.AppLib.ComboBoxPlus();
            this.radEdgeCurveStyleCurveThroughIntermediatePoints = new System.Windows.Forms.RadioButton();
            this.radEdgeCurveStyleBezier = new System.Windows.Forms.RadioButton();
            this.radEdgeCurveStyleStraight = new System.Windows.Forms.RadioButton();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.nudVertexRelativeOuterGlowSize = new System.Windows.Forms.NumericUpDown();
            this.lblVertexRelativeOuterGlowSize = new System.Windows.Forms.Label();
            this.radVertexEffectDropShadow = new System.Windows.Forms.RadioButton();
            this.radVertexEffectOuterGlow = new System.Windows.Forms.RadioButton();
            this.radVertexEffectNone = new System.Windows.Forms.RadioButton();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.groupBox4.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudVertexImageSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudVertexRadius)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudVertexAlpha)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudEdgeAlpha)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudEdgeWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRelativeArrowSize)).BeginInit();
            this.groupBox5.SuspendLayout();
            this.tcTabControl.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudVertexRelativeOuterGlowSize)).BeginInit();
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(232, 451);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(80, 23);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(318, 451);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnCustomizeVertexMenu
            // 
            this.btnCustomizeVertexMenu.Location = new System.Drawing.Point(183, 117);
            this.btnCustomizeVertexMenu.Name = "btnCustomizeVertexMenu";
            this.btnCustomizeVertexMenu.Size = new System.Drawing.Size(80, 23);
            this.btnCustomizeVertexMenu.TabIndex = 3;
            this.btnCustomizeVertexMenu.Text = "&Menu...";
            this.btnCustomizeVertexMenu.UseVisualStyleBackColor = true;
            this.btnCustomizeVertexMenu.Click += new System.EventHandler(this.btnCustomizeVertexMenu_Click);
            // 
            // btnResetAll
            // 
            this.btnResetAll.Location = new System.Drawing.Point(12, 451);
            this.btnResetAll.Name = "btnResetAll";
            this.btnResetAll.Size = new System.Drawing.Size(80, 23);
            this.btnResetAll.TabIndex = 1;
            this.btnResetAll.Text = "Reset All";
            this.btnResetAll.UseVisualStyleBackColor = true;
            this.btnResetAll.Click += new System.EventHandler(this.btnResetAll_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.usrSelectedVertexColor);
            this.groupBox4.Controls.Add(this.chkAutoSelect);
            this.groupBox4.Controls.Add(this.label6);
            this.groupBox4.Location = new System.Drawing.Point(3, 302);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(222, 100);
            this.groupBox4.TabIndex = 11;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Selected vertices";
            // 
            // usrSelectedVertexColor
            // 
            this.usrSelectedVertexColor.Color = System.Drawing.Color.White;
            this.usrSelectedVertexColor.Location = new System.Drawing.Point(87, 17);
            this.usrSelectedVertexColor.Name = "usrSelectedVertexColor";
            this.usrSelectedVertexColor.ShowButton = false;
            this.usrSelectedVertexColor.Size = new System.Drawing.Size(64, 32);
            this.usrSelectedVertexColor.TabIndex = 1;
            // 
            // chkAutoSelect
            // 
            this.chkAutoSelect.Location = new System.Drawing.Point(12, 54);
            this.chkAutoSelect.Name = "chkAutoSelect";
            this.chkAutoSelect.Size = new System.Drawing.Size(196, 38);
            this.chkAutoSelect.TabIndex = 2;
            this.chkAutoSelect.Text = "When &a vertex is clicked, select its incident edges";
            this.chkAutoSelect.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(9, 28);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(34, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "Colo&r:";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.radUseActualVertexImageSize);
            this.panel1.Controls.Add(this.label15);
            this.panel1.Controls.Add(this.radUseSpecifiedVertexImageSize);
            this.panel1.Controls.Add(this.nudVertexImageSize);
            this.panel1.Location = new System.Drawing.Point(3, 118);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(212, 51);
            this.panel1.TabIndex = 6;
            // 
            // radUseActualVertexImageSize
            // 
            this.radUseActualVertexImageSize.AutoSize = true;
            this.radUseActualVertexImageSize.Location = new System.Drawing.Point(85, 29);
            this.radUseActualVertexImageSize.Name = "radUseActualVertexImageSize";
            this.radUseActualVertexImageSize.Size = new System.Drawing.Size(76, 17);
            this.radUseActualVertexImageSize.TabIndex = 2;
            this.radUseActualVertexImageSize.TabStop = true;
            this.radUseActualVertexImageSize.Text = "Actual size";
            this.radUseActualVertexImageSize.UseVisualStyleBackColor = true;
            this.radUseActualVertexImageSize.CheckedChanged += new System.EventHandler(this.OnEventThatRequiresControlEnabling);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(5, 5);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(72, 13);
            this.label15.TabIndex = 0;
            this.label15.Text = "Size (&images):";
            // 
            // radUseSpecifiedVertexImageSize
            // 
            this.radUseSpecifiedVertexImageSize.AutoSize = true;
            this.radUseSpecifiedVertexImageSize.Location = new System.Drawing.Point(85, 7);
            this.radUseSpecifiedVertexImageSize.Name = "radUseSpecifiedVertexImageSize";
            this.radUseSpecifiedVertexImageSize.Size = new System.Drawing.Size(14, 13);
            this.radUseSpecifiedVertexImageSize.TabIndex = 1;
            this.radUseSpecifiedVertexImageSize.TabStop = true;
            this.radUseSpecifiedVertexImageSize.UseVisualStyleBackColor = true;
            this.radUseSpecifiedVertexImageSize.CheckedChanged += new System.EventHandler(this.OnEventThatRequiresControlEnabling);
            // 
            // nudVertexImageSize
            // 
            this.nudVertexImageSize.DecimalPlaces = 1;
            this.nudVertexImageSize.Location = new System.Drawing.Point(105, 3);
            this.nudVertexImageSize.Name = "nudVertexImageSize";
            this.nudVertexImageSize.Size = new System.Drawing.Size(56, 20);
            this.nudVertexImageSize.TabIndex = 3;
            // 
            // usrVertexColor
            // 
            this.usrVertexColor.Color = System.Drawing.Color.White;
            this.usrVertexColor.Location = new System.Drawing.Point(86, 11);
            this.usrVertexColor.Name = "usrVertexColor";
            this.usrVertexColor.ShowButton = false;
            this.usrVertexColor.Size = new System.Drawing.Size(64, 32);
            this.usrVertexColor.TabIndex = 1;
            // 
            // lblVertexAlpha
            // 
            this.lblVertexAlpha.AutoSize = true;
            this.lblVertexAlpha.Location = new System.Drawing.Point(149, 178);
            this.lblVertexAlpha.Name = "lblVertexAlpha";
            this.lblVertexAlpha.Size = new System.Drawing.Size(15, 13);
            this.lblVertexAlpha.TabIndex = 9;
            this.lblVertexAlpha.Text = "%";
            // 
            // label13
            // 
            this.label13.Location = new System.Drawing.Point(8, 84);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(75, 35);
            this.label13.TabIndex = 4;
            this.label13.Text = "Si&ze (simple shapes):";
            // 
            // nudVertexRadius
            // 
            this.nudVertexRadius.DecimalPlaces = 1;
            this.nudVertexRadius.Location = new System.Drawing.Point(89, 84);
            this.nudVertexRadius.Name = "nudVertexRadius";
            this.nudVertexRadius.Size = new System.Drawing.Size(56, 20);
            this.nudVertexRadius.TabIndex = 5;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(8, 178);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(46, 13);
            this.label5.TabIndex = 7;
            this.label5.Text = "&Opacity:";
            // 
            // nudVertexAlpha
            // 
            this.nudVertexAlpha.Location = new System.Drawing.Point(88, 176);
            this.nudVertexAlpha.Name = "nudVertexAlpha";
            this.nudVertexAlpha.Size = new System.Drawing.Size(56, 20);
            this.nudVertexAlpha.TabIndex = 8;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(34, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "&Color:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(8, 52);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(41, 13);
            this.label8.TabIndex = 2;
            this.label8.Text = "&Shape:";
            // 
            // cbxVertexShape
            // 
            this.cbxVertexShape.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxVertexShape.FormattingEnabled = true;
            this.cbxVertexShape.Location = new System.Drawing.Point(88, 49);
            this.cbxVertexShape.Name = "cbxVertexShape";
            this.cbxVertexShape.Size = new System.Drawing.Size(119, 21);
            this.cbxVertexShape.TabIndex = 3;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.usrSelectedEdgeColor);
            this.groupBox2.Controls.Add(this.label24);
            this.groupBox2.Location = new System.Drawing.Point(11, 268);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(181, 63);
            this.groupBox2.TabIndex = 11;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Selected edges";
            // 
            // usrSelectedEdgeColor
            // 
            this.usrSelectedEdgeColor.Color = System.Drawing.Color.White;
            this.usrSelectedEdgeColor.Location = new System.Drawing.Point(73, 17);
            this.usrSelectedEdgeColor.Name = "usrSelectedEdgeColor";
            this.usrSelectedEdgeColor.ShowButton = false;
            this.usrSelectedEdgeColor.Size = new System.Drawing.Size(64, 32);
            this.usrSelectedEdgeColor.TabIndex = 1;
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(9, 28);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(34, 13);
            this.label24.TabIndex = 0;
            this.label24.Text = "Colo&r:";
            // 
            // usrEdgeColor
            // 
            this.usrEdgeColor.Color = System.Drawing.Color.White;
            this.usrEdgeColor.Location = new System.Drawing.Point(71, 11);
            this.usrEdgeColor.Name = "usrEdgeColor";
            this.usrEdgeColor.ShowButton = false;
            this.usrEdgeColor.Size = new System.Drawing.Size(64, 32);
            this.usrEdgeColor.TabIndex = 1;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(135, 86);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(110, 13);
            this.label11.TabIndex = 6;
            this.label11.Text = "(Directed graphs only)";
            // 
            // lblEdgeAlpha
            // 
            this.lblEdgeAlpha.AutoSize = true;
            this.lblEdgeAlpha.Location = new System.Drawing.Point(132, 123);
            this.lblEdgeAlpha.Name = "lblEdgeAlpha";
            this.lblEdgeAlpha.Size = new System.Drawing.Size(15, 13);
            this.lblEdgeAlpha.TabIndex = 9;
            this.lblEdgeAlpha.Text = "%";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(8, 123);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(46, 13);
            this.label12.TabIndex = 7;
            this.label12.Text = "&Opacity:";
            // 
            // nudEdgeAlpha
            // 
            this.nudEdgeAlpha.Location = new System.Drawing.Point(73, 121);
            this.nudEdgeAlpha.Name = "nudEdgeAlpha";
            this.nudEdgeAlpha.Size = new System.Drawing.Size(56, 20);
            this.nudEdgeAlpha.TabIndex = 8;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 54);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "&Width:";
            // 
            // nudEdgeWidth
            // 
            this.nudEdgeWidth.DecimalPlaces = 1;
            this.nudEdgeWidth.Location = new System.Drawing.Point(73, 52);
            this.nudEdgeWidth.Name = "nudEdgeWidth";
            this.nudEdgeWidth.Size = new System.Drawing.Size(56, 20);
            this.nudEdgeWidth.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(34, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "&Color:";
            // 
            // nudRelativeArrowSize
            // 
            this.nudRelativeArrowSize.DecimalPlaces = 1;
            this.nudRelativeArrowSize.Location = new System.Drawing.Point(73, 84);
            this.nudRelativeArrowSize.Name = "nudRelativeArrowSize";
            this.nudRelativeArrowSize.Size = new System.Drawing.Size(56, 20);
            this.nudRelativeArrowSize.TabIndex = 5;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(8, 86);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(58, 13);
            this.label17.TabIndex = 4;
            this.label17.Text = "&Arrow size:";
            // 
            // usrBackColor
            // 
            this.usrBackColor.Color = System.Drawing.Color.White;
            this.usrBackColor.Location = new System.Drawing.Point(75, 17);
            this.usrBackColor.Name = "usrBackColor";
            this.usrBackColor.ShowButton = false;
            this.usrBackColor.Size = new System.Drawing.Size(64, 32);
            this.usrBackColor.TabIndex = 1;
            // 
            // chkAutoReadWorkbook
            // 
            this.chkAutoReadWorkbook.Location = new System.Drawing.Point(11, 152);
            this.chkAutoReadWorkbook.Name = "chkAutoReadWorkbook";
            this.chkAutoReadWorkbook.Size = new System.Drawing.Size(220, 60);
            this.chkAutoReadWorkbook.TabIndex = 4;
            this.chkAutoReadWorkbook.Text = "&Automatically refresh the graph after setting a visual property in the Ribbon or" +
                " autofilling columns";
            this.toolTip1.SetToolTip(this.chkAutoReadWorkbook, resources.GetString("chkAutoReadWorkbook.ToolTip"));
            this.chkAutoReadWorkbook.UseVisualStyleBackColor = true;
            // 
            // btnAxisFont
            // 
            this.btnAxisFont.Location = new System.Drawing.Point(97, 117);
            this.btnAxisFont.Name = "btnAxisFont";
            this.btnAxisFont.Size = new System.Drawing.Size(80, 23);
            this.btnAxisFont.TabIndex = 2;
            this.btnAxisFont.Text = "A&xis Font...";
            this.btnAxisFont.UseVisualStyleBackColor = true;
            this.btnAxisFont.Click += new System.EventHandler(this.btnAxisFont_Click);
            // 
            // btnLabels
            // 
            this.btnLabels.Location = new System.Drawing.Point(12, 117);
            this.btnLabels.Name = "btnLabels";
            this.btnLabels.Size = new System.Drawing.Size(80, 23);
            this.btnLabels.TabIndex = 1;
            this.btnLabels.Text = "&Labels...";
            this.btnLabels.UseVisualStyleBackColor = true;
            this.btnLabels.Click += new System.EventHandler(this.btnLabels_Click);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.btnBrowseBackgroundImageUri);
            this.groupBox5.Controls.Add(this.txbBackgroundImageUri);
            this.groupBox5.Controls.Add(this.label7);
            this.groupBox5.Controls.Add(this.label4);
            this.groupBox5.Controls.Add(this.usrBackColor);
            this.groupBox5.Location = new System.Drawing.Point(12, 14);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(355, 82);
            this.groupBox5.TabIndex = 0;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Background";
            // 
            // btnBrowseBackgroundImageUri
            // 
            this.btnBrowseBackgroundImageUri.Location = new System.Drawing.Point(268, 50);
            this.btnBrowseBackgroundImageUri.Name = "btnBrowseBackgroundImageUri";
            this.btnBrowseBackgroundImageUri.Size = new System.Drawing.Size(75, 23);
            this.btnBrowseBackgroundImageUri.TabIndex = 4;
            this.btnBrowseBackgroundImageUri.Text = "&Browse...";
            this.btnBrowseBackgroundImageUri.UseVisualStyleBackColor = true;
            this.btnBrowseBackgroundImageUri.Click += new System.EventHandler(this.btnBrowseBackgroundImageUri_Click);
            // 
            // txbBackgroundImageUri
            // 
            this.txbBackgroundImageUri.Location = new System.Drawing.Point(78, 53);
            this.txbBackgroundImageUri.MaxLength = 256;
            this.txbBackgroundImageUri.Name = "txbBackgroundImageUri";
            this.txbBackgroundImageUri.Size = new System.Drawing.Size(184, 20);
            this.txbBackgroundImageUri.TabIndex = 3;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(9, 55);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(39, 13);
            this.label7.TabIndex = 2;
            this.label7.Text = "&Image:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 28);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(34, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "&Color:";
            // 
            // tcTabControl
            // 
            this.tcTabControl.Controls.Add(this.tabPage1);
            this.tcTabControl.Controls.Add(this.tabPage2);
            this.tcTabControl.Controls.Add(this.tabPage3);
            this.tcTabControl.Location = new System.Drawing.Point(12, 12);
            this.tcTabControl.Name = "tcTabControl";
            this.tcTabControl.Size = new System.Drawing.Size(386, 433);
            this.tcTabControl.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Controls.Add(this.nudRelativeArrowSize);
            this.tabPage1.Controls.Add(this.label17);
            this.tabPage1.Controls.Add(this.usrEdgeColor);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.label11);
            this.tabPage1.Controls.Add(this.nudEdgeWidth);
            this.tabPage1.Controls.Add(this.lblEdgeAlpha);
            this.tabPage1.Controls.Add(this.groupBox2);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.label12);
            this.tabPage1.Controls.Add(this.nudEdgeAlpha);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(378, 407);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Edges";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cbxEdgeBundlerStraightening);
            this.groupBox1.Controls.Add(this.cbxEdgeBezierDisplacementFactor);
            this.groupBox1.Controls.Add(this.radEdgeCurveStyleCurveThroughIntermediatePoints);
            this.groupBox1.Controls.Add(this.radEdgeCurveStyleBezier);
            this.groupBox1.Controls.Add(this.radEdgeCurveStyleStraight);
            this.groupBox1.Location = new System.Drawing.Point(11, 153);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(197, 105);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Curvature";
            // 
            // cbxEdgeBundlerStraightening
            // 
            this.cbxEdgeBundlerStraightening.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxEdgeBundlerStraightening.FormattingEnabled = true;
            this.cbxEdgeBundlerStraightening.Location = new System.Drawing.Point(86, 70);
            this.cbxEdgeBundlerStraightening.Name = "cbxEdgeBundlerStraightening";
            this.cbxEdgeBundlerStraightening.Size = new System.Drawing.Size(97, 21);
            this.cbxEdgeBundlerStraightening.TabIndex = 4;
            // 
            // cbxEdgeBezierDisplacementFactor
            // 
            this.cbxEdgeBezierDisplacementFactor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxEdgeBezierDisplacementFactor.FormattingEnabled = true;
            this.cbxEdgeBezierDisplacementFactor.Location = new System.Drawing.Point(86, 43);
            this.cbxEdgeBezierDisplacementFactor.Name = "cbxEdgeBezierDisplacementFactor";
            this.cbxEdgeBezierDisplacementFactor.Size = new System.Drawing.Size(97, 21);
            this.cbxEdgeBezierDisplacementFactor.TabIndex = 3;
            // 
            // radEdgeCurveStyleCurveThroughIntermediatePoints
            // 
            this.radEdgeCurveStyleCurveThroughIntermediatePoints.AutoSize = true;
            this.radEdgeCurveStyleCurveThroughIntermediatePoints.Location = new System.Drawing.Point(10, 73);
            this.radEdgeCurveStyleCurveThroughIntermediatePoints.Name = "radEdgeCurveStyleCurveThroughIntermediatePoints";
            this.radEdgeCurveStyleCurveThroughIntermediatePoints.Size = new System.Drawing.Size(64, 17);
            this.radEdgeCurveStyleCurveThroughIntermediatePoints.TabIndex = 2;
            this.radEdgeCurveStyleCurveThroughIntermediatePoints.TabStop = true;
            this.radEdgeCurveStyleCurveThroughIntermediatePoints.Text = "&Bundled";
            this.radEdgeCurveStyleCurveThroughIntermediatePoints.UseVisualStyleBackColor = true;
            this.radEdgeCurveStyleCurveThroughIntermediatePoints.CheckedChanged += new System.EventHandler(this.OnEventThatRequiresControlEnabling);
            // 
            // radEdgeCurveStyleBezier
            // 
            this.radEdgeCurveStyleBezier.AutoSize = true;
            this.radEdgeCurveStyleBezier.Location = new System.Drawing.Point(10, 46);
            this.radEdgeCurveStyleBezier.Name = "radEdgeCurveStyleBezier";
            this.radEdgeCurveStyleBezier.Size = new System.Drawing.Size(59, 17);
            this.radEdgeCurveStyleBezier.TabIndex = 1;
            this.radEdgeCurveStyleBezier.TabStop = true;
            this.radEdgeCurveStyleBezier.Text = "Cur&ved";
            this.radEdgeCurveStyleBezier.UseVisualStyleBackColor = true;
            this.radEdgeCurveStyleBezier.CheckedChanged += new System.EventHandler(this.OnEventThatRequiresControlEnabling);
            // 
            // radEdgeCurveStyleStraight
            // 
            this.radEdgeCurveStyleStraight.AutoSize = true;
            this.radEdgeCurveStyleStraight.Location = new System.Drawing.Point(10, 19);
            this.radEdgeCurveStyleStraight.Name = "radEdgeCurveStyleStraight";
            this.radEdgeCurveStyleStraight.Size = new System.Drawing.Size(61, 17);
            this.radEdgeCurveStyleStraight.TabIndex = 0;
            this.radEdgeCurveStyleStraight.TabStop = true;
            this.radEdgeCurveStyleStraight.Text = "&Straight";
            this.radEdgeCurveStyleStraight.UseVisualStyleBackColor = true;
            this.radEdgeCurveStyleStraight.CheckedChanged += new System.EventHandler(this.OnEventThatRequiresControlEnabling);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.groupBox3);
            this.tabPage2.Controls.Add(this.panel1);
            this.tabPage2.Controls.Add(this.label2);
            this.tabPage2.Controls.Add(this.usrVertexColor);
            this.tabPage2.Controls.Add(this.cbxVertexShape);
            this.tabPage2.Controls.Add(this.lblVertexAlpha);
            this.tabPage2.Controls.Add(this.label8);
            this.tabPage2.Controls.Add(this.label13);
            this.tabPage2.Controls.Add(this.groupBox4);
            this.tabPage2.Controls.Add(this.nudVertexAlpha);
            this.tabPage2.Controls.Add(this.nudVertexRadius);
            this.tabPage2.Controls.Add(this.label5);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(378, 407);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Vertices";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.nudVertexRelativeOuterGlowSize);
            this.groupBox3.Controls.Add(this.lblVertexRelativeOuterGlowSize);
            this.groupBox3.Controls.Add(this.radVertexEffectDropShadow);
            this.groupBox3.Controls.Add(this.radVertexEffectOuterGlow);
            this.groupBox3.Controls.Add(this.radVertexEffectNone);
            this.groupBox3.Location = new System.Drawing.Point(3, 202);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(222, 92);
            this.groupBox3.TabIndex = 10;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Effects";
            // 
            // nudVertexRelativeOuterGlowSize
            // 
            this.nudVertexRelativeOuterGlowSize.DecimalPlaces = 1;
            this.nudVertexRelativeOuterGlowSize.Location = new System.Drawing.Point(156, 64);
            this.nudVertexRelativeOuterGlowSize.Name = "nudVertexRelativeOuterGlowSize";
            this.nudVertexRelativeOuterGlowSize.Size = new System.Drawing.Size(56, 20);
            this.nudVertexRelativeOuterGlowSize.TabIndex = 4;
            // 
            // lblVertexRelativeOuterGlowSize
            // 
            this.lblVertexRelativeOuterGlowSize.AutoSize = true;
            this.lblVertexRelativeOuterGlowSize.Location = new System.Drawing.Point(108, 66);
            this.lblVertexRelativeOuterGlowSize.Name = "lblVertexRelativeOuterGlowSize";
            this.lblVertexRelativeOuterGlowSize.Size = new System.Drawing.Size(38, 13);
            this.lblVertexRelativeOuterGlowSize.TabIndex = 3;
            this.lblVertexRelativeOuterGlowSize.Text = "&Width:";
            // 
            // radVertexEffectDropShadow
            // 
            this.radVertexEffectDropShadow.AutoSize = true;
            this.radVertexEffectDropShadow.Location = new System.Drawing.Point(10, 41);
            this.radVertexEffectDropShadow.Name = "radVertexEffectDropShadow";
            this.radVertexEffectDropShadow.Size = new System.Drawing.Size(118, 17);
            this.radVertexEffectDropShadow.TabIndex = 1;
            this.radVertexEffectDropShadow.TabStop = true;
            this.radVertexEffectDropShadow.Text = "&Drop shadow (slow)";
            this.radVertexEffectDropShadow.UseVisualStyleBackColor = true;
            this.radVertexEffectDropShadow.CheckedChanged += new System.EventHandler(this.OnEventThatRequiresControlEnabling);
            // 
            // radVertexEffectOuterGlow
            // 
            this.radVertexEffectOuterGlow.AutoSize = true;
            this.radVertexEffectOuterGlow.Location = new System.Drawing.Point(10, 64);
            this.radVertexEffectOuterGlow.Name = "radVertexEffectOuterGlow";
            this.radVertexEffectOuterGlow.Size = new System.Drawing.Size(79, 17);
            this.radVertexEffectOuterGlow.TabIndex = 2;
            this.radVertexEffectOuterGlow.TabStop = true;
            this.radVertexEffectOuterGlow.Text = "&Glow (slow)";
            this.radVertexEffectOuterGlow.UseVisualStyleBackColor = true;
            this.radVertexEffectOuterGlow.CheckedChanged += new System.EventHandler(this.OnEventThatRequiresControlEnabling);
            // 
            // radVertexEffectNone
            // 
            this.radVertexEffectNone.AutoSize = true;
            this.radVertexEffectNone.Location = new System.Drawing.Point(10, 19);
            this.radVertexEffectNone.Name = "radVertexEffectNone";
            this.radVertexEffectNone.Size = new System.Drawing.Size(51, 17);
            this.radVertexEffectNone.TabIndex = 0;
            this.radVertexEffectNone.TabStop = true;
            this.radVertexEffectNone.Text = "&None";
            this.radVertexEffectNone.UseVisualStyleBackColor = true;
            this.radVertexEffectNone.CheckedChanged += new System.EventHandler(this.OnEventThatRequiresControlEnabling);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.groupBox5);
            this.tabPage3.Controls.Add(this.chkAutoReadWorkbook);
            this.tabPage3.Controls.Add(this.btnCustomizeVertexMenu);
            this.tabPage3.Controls.Add(this.btnAxisFont);
            this.tabPage3.Controls.Add(this.btnLabels);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(378, 407);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Other";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // GeneralUserSettingsDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(411, 486);
            this.Controls.Add(this.tcTabControl);
            this.Controls.Add(this.btnResetAll);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GeneralUserSettingsDialog";
            this.Text = "Graph Options";
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudVertexImageSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudVertexRadius)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudVertexAlpha)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudEdgeAlpha)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudEdgeWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRelativeArrowSize)).EndInit();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.tcTabControl.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudVertexRelativeOuterGlowSize)).EndInit();
            this.tabPage3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown nudEdgeWidth;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown nudVertexRadius;
        private System.Windows.Forms.Label label8;
        private Smrf.AppLib.ComboBoxPlus cbxVertexShape;
        private System.Windows.Forms.NumericUpDown nudRelativeArrowSize;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button btnResetAll;
        private System.Windows.Forms.Label lblVertexAlpha;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown nudVertexAlpha;
        private System.Windows.Forms.Label lblEdgeAlpha;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.NumericUpDown nudEdgeAlpha;
        private System.Windows.Forms.CheckBox chkAutoSelect;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button btnCustomizeVertexMenu;
        private Smrf.AppLib.ColorPicker usrBackColor;
        private Smrf.AppLib.ColorPicker usrVertexColor;
        private Smrf.AppLib.ColorPicker usrEdgeColor;
        private Smrf.AppLib.ColorPicker usrSelectedVertexColor;
        private Smrf.AppLib.ColorPicker usrSelectedEdgeColor;
        private System.Windows.Forms.CheckBox chkAutoReadWorkbook;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button btnAxisFont;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.NumericUpDown nudVertexImageSize;
        private System.Windows.Forms.RadioButton radUseActualVertexImageSize;
        private System.Windows.Forms.RadioButton radUseSpecifiedVertexImageSize;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnLabels;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnBrowseBackgroundImageUri;
        private System.Windows.Forms.TextBox txbBackgroundImageUri;
        private System.Windows.Forms.TabControl tcTabControl;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radEdgeCurveStyleBezier;
        private System.Windows.Forms.RadioButton radEdgeCurveStyleStraight;
        private System.Windows.Forms.RadioButton radEdgeCurveStyleCurveThroughIntermediatePoints;
        private Smrf.AppLib.ComboBoxPlus cbxEdgeBundlerStraightening;
        private Smrf.AppLib.ComboBoxPlus cbxEdgeBezierDisplacementFactor;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.NumericUpDown nudVertexRelativeOuterGlowSize;
        private System.Windows.Forms.Label lblVertexRelativeOuterGlowSize;
        private System.Windows.Forms.RadioButton radVertexEffectDropShadow;
        private System.Windows.Forms.RadioButton radVertexEffectOuterGlow;
        private System.Windows.Forms.RadioButton radVertexEffectNone;
    }
}
