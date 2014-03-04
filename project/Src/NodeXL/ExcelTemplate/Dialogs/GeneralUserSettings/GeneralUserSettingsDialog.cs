
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Configuration;
using System.Diagnostics;
using Smrf.AppLib;
using Smrf.NodeXL.Visualization.Wpf;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: GeneralUserSettingsDialog
//
/// <summary>
/// Edits a <see cref="GeneralUserSettings" /> object.
/// </summary>
///
/// <remarks>
/// Pass a <see cref="GeneralUserSettings" /> object to the constructor.  If
/// the user edits the object, <see cref="Form.ShowDialog()" /> returns
/// DialogResult.OK.  Otherwise, the object is not modified and <see
/// cref="Form.ShowDialog()" /> returns DialogResult.Cancel.
/// </remarks>
//*****************************************************************************

public partial class GeneralUserSettingsDialog : ExcelTemplateForm
{
    //*************************************************************************
    //  Constructor: GeneralUserSettingsDialog()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="GeneralUserSettingsDialog" /> class.
    /// </summary>
    ///
    /// <param name="generalUserSettings">
    /// The object being edited.
    /// </param>
    ///
    /// <param name="workbook">
    /// Workbook containing the graph data.
    /// </param>
    //*************************************************************************

    public GeneralUserSettingsDialog
    (
        GeneralUserSettings generalUserSettings,
        Microsoft.Office.Interop.Excel.Workbook workbook
    )
    {
        Debug.Assert(generalUserSettings != null);
        Debug.Assert(workbook != null);
        generalUserSettings.AssertValid();

        m_oGeneralUserSettings = generalUserSettings;
        m_oWorkbook = workbook;
        m_oAxisFont = m_oGeneralUserSettings.AxisFont;
        m_oLabelUserSettings = m_oGeneralUserSettings.LabelUserSettings;

        m_oOpenFileDialog = new OpenFileDialog();

        m_oOpenFileDialog.Filter =
            "All files (*.*)|*.*|" + SaveableImageFormats.Filter
            ;

        m_oOpenFileDialog.Title = "Browse for Background Image";

        // Instantiate an object that saves and retrieves the position of this
        // dialog.  Note that the object automatically saves the settings when
        // the form closes.

        m_oGeneralUserSettingsDialogUserSettings =
            new GeneralUserSettingsDialogUserSettings(this);

        InitializeComponent();

        nudEdgeWidth.Minimum =
            (Decimal)EdgeWidthConverter.MinimumWidthWorkbook;

        nudEdgeWidth.Maximum =
            (Decimal)EdgeWidthConverter.MaximumWidthWorkbook;

        nudRelativeArrowSize.Minimum =
            (Decimal)EdgeDrawer.MinimumRelativeArrowSize;

        nudRelativeArrowSize.Maximum =
            (Decimal)EdgeDrawer.MaximumRelativeArrowSize;

        cbxEdgeBezierDisplacementFactor.PopulateWithObjectsAndText(
            0.1, "Low",
            0.2, "Medium",
            0.6, "High",
            1.2, "Very High"
            );

        cbxEdgeBundlerStraightening.PopulateWithObjectsAndText(
            0.15F, "Tight",
            0.40F, "Medium",
            0.60F, "Loose"
            );

        nudVertexRadius.Minimum = nudVertexImageSize.Minimum =
            (Decimal)VertexRadiusConverter.MinimumRadiusWorkbook;

        nudVertexRadius.Maximum = nudVertexImageSize.Maximum =
            (Decimal)VertexRadiusConverter.MaximumRadiusWorkbook;

        ( new VertexShapeConverter() ).PopulateComboBox(cbxVertexShape, false);

        nudVertexAlpha.Minimum = nudEdgeAlpha.Minimum =
            (Decimal)AlphaConverter.MinimumAlphaWorkbook;

        nudVertexAlpha.Maximum = nudEdgeAlpha.Maximum =
            (Decimal)AlphaConverter.MaximumAlphaWorkbook;

        nudVertexRelativeOuterGlowSize.Minimum =
            (Decimal)VertexDrawer.MinimumRelativeOuterGlowSize;

        nudVertexRelativeOuterGlowSize.Maximum =
            (Decimal)VertexDrawer.MaximumRelativeOuterGlowSize;

        DoDataExchange(false);

        AssertValid();
    }

    //*************************************************************************
    //  Method: DoDataExchange()
    //
    /// <summary>
    /// Transfers data between the dialog's fields and its controls.
    /// </summary>
    ///
    /// <param name="bFromControls">
    /// true to transfer data from the dialog's controls to its fields, false
    /// for the other direction.
    /// </param>
    ///
    /// <returns>
    /// true if the transfer was successful.
    /// </returns>
    //*************************************************************************

    protected Boolean
    DoDataExchange
    (
        Boolean bFromControls
    )
    {
        if (bFromControls)
        {
            Boolean bUseSpecifiedVertexImageSize =
                radUseSpecifiedVertexImageSize.Checked;

            Single fEdgeWidth, fRelativeArrowSize, fVertexRadius,
                fVertexRelativeOuterGlowSize, fVertexAlpha, fEdgeAlpha;

            Single fVertexImageSize = 0;

            if (
                !ValidateNumericUpDown(nudVertexRadius,
                    "a size for simple vertex shapes", out fVertexRadius)
                ||
                ( bUseSpecifiedVertexImageSize &&
                    !ValidateNumericUpDown(nudVertexImageSize,
                    "a size for vertex images", out fVertexImageSize) )
                ||
                !ValidateNumericUpDown(nudVertexAlpha, "a vertex opacity",
                    out fVertexAlpha)
                ||
                !ValidateNumericUpDown(nudVertexRelativeOuterGlowSize,
                    "a vertex glow width", out fVertexRelativeOuterGlowSize)
                ||
                !ValidateNumericUpDown(nudEdgeWidth,
                    "a width for unselected edges", out fEdgeWidth)
                ||
                !ValidateNumericUpDown(nudRelativeArrowSize,
                    "an edge arrow size", out fRelativeArrowSize)
                ||
                !ValidateNumericUpDown(nudEdgeAlpha, "an edge opacity",
                    out fEdgeAlpha)
                )
            {
                return (false);
            }

            m_oGeneralUserSettingsDialogUserSettings.TabControlSelectedIndex =
                tcTabControl.SelectedIndex;

            m_oGeneralUserSettings.BackColor = usrBackColor.Color;

            m_oGeneralUserSettings.BackgroundImageUri =
                txbBackgroundImageUri.Text;

            m_oGeneralUserSettings.EdgeWidth = fEdgeWidth;
            m_oGeneralUserSettings.RelativeArrowSize = fRelativeArrowSize;
            m_oGeneralUserSettings.EdgeColor = usrEdgeColor.Color;
            m_oGeneralUserSettings.EdgeAlpha = fEdgeAlpha;

            m_oGeneralUserSettings.SelectedEdgeColor =
                usrSelectedEdgeColor.Color;

            DoDataExchangeEdgeCurvature(true);

            m_oGeneralUserSettings.VertexShape =
                (VertexShape)cbxVertexShape.SelectedValue;

            m_oGeneralUserSettings.VertexRadius = fVertexRadius;

            m_oGeneralUserSettings.VertexImageSize =
                bUseSpecifiedVertexImageSize ? fVertexImageSize :
                new Nullable<Single>();

            m_oGeneralUserSettings.VertexColor = usrVertexColor.Color;
            m_oGeneralUserSettings.VertexAlpha = fVertexAlpha;

            m_oGeneralUserSettings.VertexRelativeOuterGlowSize =
                fVertexRelativeOuterGlowSize;

            DoDataExchangeVertexEffect(true);

            m_oGeneralUserSettings.SelectedVertexColor =
                usrSelectedVertexColor.Color;

            m_oGeneralUserSettings.AutoSelect = chkAutoSelect.Checked;
            m_oGeneralUserSettings.AxisFont = m_oAxisFont;
            m_oGeneralUserSettings.LabelUserSettings = m_oLabelUserSettings;

            m_oGeneralUserSettings.AutoReadWorkbook =
                chkAutoReadWorkbook.Checked;
        }
        else
        {
            tcTabControl.SelectedIndex =
                m_oGeneralUserSettingsDialogUserSettings.
                TabControlSelectedIndex;

            usrBackColor.Color = m_oGeneralUserSettings.BackColor;

            txbBackgroundImageUri.Text =
                m_oGeneralUserSettings.BackgroundImageUri;

            nudEdgeWidth.Value = (Decimal)m_oGeneralUserSettings.EdgeWidth;

            nudRelativeArrowSize.Value =
                (Decimal)m_oGeneralUserSettings.RelativeArrowSize;

            usrEdgeColor.Color = m_oGeneralUserSettings.EdgeColor;
            nudEdgeAlpha.Value = (Decimal)m_oGeneralUserSettings.EdgeAlpha;

            usrSelectedEdgeColor.Color =
                m_oGeneralUserSettings.SelectedEdgeColor;

            DoDataExchangeEdgeCurvature(false);

            cbxVertexShape.SelectedValue = m_oGeneralUserSettings.VertexShape;

            nudVertexRadius.Value =
                (Decimal)m_oGeneralUserSettings.VertexRadius;

            Nullable<Single> oVertexImageSize =
                m_oGeneralUserSettings.VertexImageSize;

            if (oVertexImageSize.HasValue)
            {
                radUseSpecifiedVertexImageSize.Checked = true;
                nudVertexImageSize.Value = (Decimal)oVertexImageSize.Value;
            }
            else
            {
                radUseActualVertexImageSize.Checked = true;
            }

            usrVertexColor.Color = m_oGeneralUserSettings.VertexColor;
            nudVertexAlpha.Value = (Decimal)m_oGeneralUserSettings.VertexAlpha;

            nudVertexRelativeOuterGlowSize.Value =
                (Decimal)m_oGeneralUserSettings.VertexRelativeOuterGlowSize;

            DoDataExchangeVertexEffect(false);

            usrSelectedVertexColor.Color =
                m_oGeneralUserSettings.SelectedVertexColor;

            chkAutoSelect.Checked = m_oGeneralUserSettings.AutoSelect;

            m_oAxisFont = m_oGeneralUserSettings.AxisFont;

            m_oLabelUserSettings =
                m_oGeneralUserSettings.LabelUserSettings.Copy();

            chkAutoReadWorkbook.Checked =
                m_oGeneralUserSettings.AutoReadWorkbook;

            EnableControls();
        }

        return (true);
    }

    //*************************************************************************
    //  Method: DoDataExchangeEdgeCurvature()
    //
    /// <summary>
    /// Transfers data between the dialog's edge curvature fields and its
    /// controls.
    /// </summary>
    ///
    /// <param name="bFromControls">
    /// true to transfer data from the dialog's controls to its fields, false
    /// for the other direction.
    /// </param>
    ///
    /// <returns>
    /// true if the transfer was successful.
    /// </returns>
    //*************************************************************************

    protected Boolean
    DoDataExchangeEdgeCurvature
    (
        Boolean bFromControls
    )
    {
        if (bFromControls)
        {
            EdgeCurveStyle eEdgeCurveStyle;

            if (radEdgeCurveStyleStraight.Checked)
            {
                eEdgeCurveStyle = EdgeCurveStyle.Straight;
            }
            else if (radEdgeCurveStyleBezier.Checked)
            {
                eEdgeCurveStyle = EdgeCurveStyle.Bezier;
            }
            else
            {
                eEdgeCurveStyle =
                    EdgeCurveStyle.CurveThroughIntermediatePoints;
            }

            m_oGeneralUserSettings.EdgeCurveStyle = eEdgeCurveStyle;

            m_oGeneralUserSettings.EdgeBezierDisplacementFactor =
                (Double)cbxEdgeBezierDisplacementFactor.SelectedValue;

            m_oGeneralUserSettings.EdgeBundlerStraightening =
                (Single)cbxEdgeBundlerStraightening.SelectedValue;
        }
        else
        {
            switch (m_oGeneralUserSettings.EdgeCurveStyle)
            {
                case EdgeCurveStyle.Straight:

                    radEdgeCurveStyleStraight.Checked = true;
                    break;

                case EdgeCurveStyle.Bezier:

                    radEdgeCurveStyleBezier.Checked = true;
                    break;

                case EdgeCurveStyle.CurveThroughIntermediatePoints:

                    radEdgeCurveStyleCurveThroughIntermediatePoints.Checked
                        = true;

                    break;

                default:

                    Debug.Assert(false);
                    break;
            }

            cbxEdgeBezierDisplacementFactor.SelectedValue =
                m_oGeneralUserSettings.EdgeBezierDisplacementFactor;

            cbxEdgeBundlerStraightening.SelectedValue =
                m_oGeneralUserSettings.EdgeBundlerStraightening;
        }

        return (true);
    }

    //*************************************************************************
    //  Method: DoDataExchangeVertexEffect()
    //
    /// <summary>
    /// Transfers data between the dialog's vertex effect fields and its
    /// controls.
    /// </summary>
    ///
    /// <param name="bFromControls">
    /// true to transfer data from the dialog's controls to its fields, false
    /// for the other direction.
    /// </param>
    ///
    /// <returns>
    /// true if the transfer was successful.
    /// </returns>
    //*************************************************************************

    protected Boolean
    DoDataExchangeVertexEffect
    (
        Boolean bFromControls
    )
    {
        if (bFromControls)
        {
            VertexEffect eVertexEffect;

            if (radVertexEffectNone.Checked)
            {
                eVertexEffect = VertexEffect.None;
            }
            else if (radVertexEffectDropShadow.Checked)
            {
                eVertexEffect = VertexEffect.DropShadow;
            }
            else
            {
                eVertexEffect = VertexEffect.OuterGlow;
            }

            m_oGeneralUserSettings.VertexEffect = eVertexEffect;
        }
        else
        {
            switch (m_oGeneralUserSettings.VertexEffect)
            {
                case VertexEffect.None:

                    radVertexEffectNone.Checked = true;
                    break;

                case VertexEffect.DropShadow:

                    radVertexEffectDropShadow.Checked = true;
                    break;

                case VertexEffect.OuterGlow:

                    radVertexEffectOuterGlow.Checked = true;
                    break;

                default:

                    Debug.Assert(false);
                    break;
            }
        }

        return (true);
    }

    //*************************************************************************
    //  Method: EnableControls()
    //
    /// <summary>
    /// Enables or disables the dialog's controls.
    /// </summary>
    //*************************************************************************

    protected void
    EnableControls()
    {
        AssertValid();

        cbxEdgeBezierDisplacementFactor.Enabled =
            radEdgeCurveStyleBezier.Checked;

        cbxEdgeBundlerStraightening.Enabled =
            radEdgeCurveStyleCurveThroughIntermediatePoints.Checked;

        nudVertexImageSize.Enabled = radUseSpecifiedVertexImageSize.Checked;

        EnableControls(radVertexEffectOuterGlow.Checked,
            lblVertexRelativeOuterGlowSize, nudVertexRelativeOuterGlowSize);
    }

    //*************************************************************************
    //  Method: btnCustomizeVertexMenu_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnCustomizeVertexMenu button.
    /// </summary>
    ///
    /// <param name="sender">
    /// Standard event argument.
    /// </param>
    ///
    /// <param name="e">
    /// Standard event argument.
    /// </param>
    //*************************************************************************

    private void
    btnCustomizeVertexMenu_Click
    (
        object sender,
        EventArgs e
    )
    {
        AssertValid();

        const String Message =
            "Use this to add custom menu items to the menu that appears when"
            + " you right-click a vertex in the graph pane."
            + "\r\n\r\n"
            + "Clicking \"Yes\" below will add a pair of columns to the"
            + " Vertices worksheet -- one for menu item text and another for"
            + " the action to take when the menu item is selected."
            + "\r\n\r\n"
            + "For example, if you add the column pair and enter \"Send Mail"
            + " To\" for a vertex's menu item text and \"mailto:bob@msn.com\""
            + " for the action, then right-clicking the vertex in the NodeXL"
            + " graph and selecting \"Send Mail To\" from the right-click menu"
            + " will open a new email message addressed to bob@msn.com."
            + "\r\n\r\n"
            + "If you want to open a Web page when the menu item is selected,"
            + " enter an URL for the action."
            + "\r\n\r\n"
            + "If you want to add more than one custom menu item to a vertex's"
            + " right-click menu, run this again to add another pair of"
            + " columns."
            + "\r\n\r\n"
            + "Do you want to add a pair of columns to the Vertices worksheet?"
            ;

        if (MessageBox.Show(Message, this.ApplicationName,
                MessageBoxButtons.YesNo, MessageBoxIcon.Information) !=
                DialogResult.Yes)
        {
            return;
        }

        // Create and use the object that adds the columns to the vertex
        // table.

        TableColumnAdder oTableColumnAdder = new TableColumnAdder();

        this.UseWaitCursor = true;

        try
        {
            oTableColumnAdder.AddColumnPair(m_oWorkbook,
                WorksheetNames.Vertices, TableNames.Vertices,
                VertexTableColumnNames.CustomMenuItemTextBase,
                CustomMenuItemTextColumnWidth,
                VertexTableColumnNames.CustomMenuItemActionBase,
                CustomMenuItemActionColumnWidth
                );

            this.UseWaitCursor = false;
        }
        catch (Exception oException)
        {
            this.UseWaitCursor = false;

            ErrorUtil.OnException(oException);
        }
    }

    //*************************************************************************
    //  Method: btnAxisFont_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnAxisFont button.
    /// </summary>
    ///
    /// <param name="sender">
    /// Standard event argument.
    /// </param>
    ///
    /// <param name="e">
    /// Standard event argument.
    /// </param>
    //*************************************************************************

    private void
    btnAxisFont_Click
    (
        object sender,
        EventArgs e
    )
    {
        AssertValid();

        EditFont(ref m_oAxisFont);
    }

    //*************************************************************************
    //  Method: btnLabels_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnLabels button.
    /// </summary>
    ///
    /// <param name="sender">
    /// Standard event argument.
    /// </param>
    ///
    /// <param name="e">
    /// Standard event argument.
    /// </param>
    //*************************************************************************

    private void
    btnLabels_Click
    (
        object sender,
        EventArgs e
    )
    {
        AssertValid();

        LabelUserSettingsDialog oLabelUserSettingsDialog =
            new LabelUserSettingsDialog(m_oLabelUserSettings);

        oLabelUserSettingsDialog.ShowDialog();
    }

    //*************************************************************************
    //  Method: btnBrowseBackgroundImageUri_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnBrowseBackgroundImageUri button.
    /// </summary>
    ///
    /// <param name="sender">
    /// Standard event argument.
    /// </param>
    ///
    /// <param name="e">
    /// Standard event argument.
    /// </param>
    //*************************************************************************

    private void
    btnBrowseBackgroundImageUri_Click
    (
        object sender,
        EventArgs e
    )
    {
        AssertValid();

        if (m_oOpenFileDialog.ShowDialog() == DialogResult.OK)
        {
            txbBackgroundImageUri.Text = m_oOpenFileDialog.FileName;
        }
    }

    //*************************************************************************
    //  Method: btnResetAll_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnResetAll button.
    /// </summary>
    ///
    /// <param name="sender">
    /// Standard event argument.
    /// </param>
    ///
    /// <param name="e">
    /// Standard event argument.
    /// </param>
    //*************************************************************************

    private void
    btnResetAll_Click
    (
        object sender,
        EventArgs e
    )
    {
        AssertValid();

        m_oGeneralUserSettings.Reset();
        DoDataExchange(false);
    }

    //*************************************************************************
    //  Method: OnEventThatRequiresControlEnabling()
    //
    /// <summary>
    /// Handles any event that should changed the enabled state of the dialog's
    /// controls.
    /// </summary>
    ///
    /// <param name="sender">
    /// Standard event argument.
    /// </param>
    ///
    /// <param name="e">
    /// Standard event argument.
    /// </param>
    //*************************************************************************

    private void
    OnEventThatRequiresControlEnabling
    (
        object sender,
        EventArgs e
    )
    {
        AssertValid();

        EnableControls();
    }

    //*************************************************************************
    //  Method: btnOK_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnOK button.
    /// </summary>
    ///
    /// <param name="sender">
    /// Standard event argument.
    /// </param>
    ///
    /// <param name="e">
    /// Standard event argument.
    /// </param>
    //*************************************************************************

    private void
    btnOK_Click
    (
        object sender,
        System.EventArgs e
    )
    {
        if ( DoDataExchange(true) )
        {
            DialogResult = DialogResult.OK;
            this.Close();
        }
    }


    //*************************************************************************
    //  Method: AssertValid()
    //
    /// <summary>
    /// Asserts if the object is in an invalid state.  Debug-only.
    /// </summary>
    //*************************************************************************

    // [Conditional("DEBUG")] 

    public  override void
    AssertValid()
    {
        base.AssertValid();

        Debug.Assert(m_oGeneralUserSettings != null);
        Debug.Assert(m_oWorkbook != null);
        Debug.Assert(m_oAxisFont != null);
        Debug.Assert(m_oLabelUserSettings != null);
        Debug.Assert(m_oOpenFileDialog != null);
        Debug.Assert(m_oGeneralUserSettingsDialogUserSettings != null);
    }

    //*************************************************************************
    //  Protected constants
    //*************************************************************************

    /// Width of the custom menu item text columns, in characters.

    protected const Single CustomMenuItemTextColumnWidth = 25.6F;

    /// Width of the custom menu item action columns, in characters.

    protected const Single CustomMenuItemActionColumnWidth = 27.6F;


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Object whose properties are being edited.

    protected GeneralUserSettings m_oGeneralUserSettings;

    /// Workbook containing the graph data.

    protected Microsoft.Office.Interop.Excel.Workbook m_oWorkbook;

    /// AxisFont property of m_oGeneralUserSettings.  This gets edited by a
    /// FontDialog.

    protected Font m_oAxisFont;

    /// A copy of the LabelUserSettings object owned by m_oGeneralUserSettings.
    /// This gets edited by a LabelUserSettingsDialog.

    protected LabelUserSettings m_oLabelUserSettings;

    /// Dialog for selecting an image file.

    protected OpenFileDialog m_oOpenFileDialog;

    /// User settings for this dialog.

    protected GeneralUserSettingsDialogUserSettings
        m_oGeneralUserSettingsDialogUserSettings;
}


//*****************************************************************************
//  Class: GeneralUserSettingsDialogUserSettings
//
/// <summary>
/// Stores the user's settings for the <see
/// cref="GeneralUserSettingsDialog" />.
/// </summary>
///
/// <remarks>
/// The user settings include the form size and location.
/// </remarks>
//*****************************************************************************

[ SettingsGroupNameAttribute("GeneralUserSettingsDialog5") ]

public class GeneralUserSettingsDialogUserSettings : FormSettings
{
    //*************************************************************************
    //  Constructor: GeneralUserSettingsDialogUserSettings()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="GeneralUserSettingsDialogUserSettings" /> class.
    /// </summary>
    ///
    /// <param name="oForm">
    /// The form to save settings for.
    /// </param>
    //*************************************************************************

    public GeneralUserSettingsDialogUserSettings
    (
        Form oForm
    )
    : base (oForm, true)
    {
        Debug.Assert(oForm != null);

        // (Do nothing.)

        AssertValid();
    }

    //*************************************************************************
    //  Property: TabControlSelectedIndex
    //
    /// <summary>
    /// Gets or sets the selected index of the dialog's TabControl.
    /// </summary>
    ///
    /// <value>
    /// The selected index.  The default value is 0.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("0") ]

    public Int32
    TabControlSelectedIndex
    {
        get
        {
            AssertValid();

            return ( (Int32)this[TabControlSelectedIndexKey] );
        }

        set
        {
            this[TabControlSelectedIndexKey] = value;

            AssertValid();
        }
    }


    //*************************************************************************
    //  Method: AssertValid()
    //
    /// <summary>
    /// Asserts if the object is in an invalid state.  Debug-only.
    /// </summary>
    //*************************************************************************

    // [Conditional("DEBUG")]

    public override void
    AssertValid()
    {
        base.AssertValid();

        // (Do nothing else.)
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)


    //*************************************************************************
    //  Protected constants
    //*************************************************************************

    /// Name of the settings key for the TabControlSelectedIndex property.

    protected const String TabControlSelectedIndexKey =
        "TabControlSelectedIndex";
}
}
