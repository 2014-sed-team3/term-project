

using System;
using System.Windows.Forms;
using System.Configuration;
using System.Collections.Generic;
using System.Drawing;
using System.Diagnostics;
using Smrf.AppLib;
using Smrf.NodeXL.Core;
using Smrf.NodeXL.Visualization.Wpf;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: EdgeAttributesDialog
//
/// <summary>
/// Dialog that lets the user edit the attributes of the selected edges in a
/// NodeXLControl.
/// </summary>
///
/// <remarks>
/// Pass the NodeXLControl to the constructor.  If the user performs any edits
/// and the edits don't require that the workbook be read again, this dialog
/// applies the edits to the NodeXLControl's selected edges and returns
/// DialogResult.OK from <see cref="Form.ShowDialog()" />.  The list of edited
/// attributes can then be obtained from the <see
/// cref="EditedEdgeAttributes" /> property.
///
/// <para>
/// If the user performs edits that require that the workbook be read again,
/// no edits are applied to the selected edges.  Instead, the caller must force
/// the workbook to be read again.  The caller can determine if this is
/// required by checking the <see
/// cref="ExcelTemplate.EditedEdgeAttributes.WorkbookMustBeReread" /> property.
/// </para>
///
/// </remarks>
//*****************************************************************************

public partial class EdgeAttributesDialog : AttributesDialogBase
{
    //*************************************************************************
    //  Constructor: EdgeAttributesDialog()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="EdgeAttributesDialog" />
    /// class.
    /// </summary>
    ///
    /// <param name="nodeXLControl">
    /// NodeXLControl whose edge attributes need to be edited.
    /// </param>
    //*************************************************************************

    public EdgeAttributesDialog
    (
        NodeXLControl nodeXLControl
    )
    : base(nodeXLControl)
    {
        InitializeComponent();

        // Instantiate an object that retrieves and saves the user settings for
        // this dialog.  Note that the object automatically saves the settings
        // when the form closes.

        m_oEdgeAttributesDialogUserSettings =
            new EdgeAttributesDialogUserSettings(this);

        m_oEditedEdgeAttributes = GetInitialEdgeAttributes();

        nudWidth.Minimum = (Decimal)EdgeWidthConverter.MinimumWidthWorkbook;
        nudWidth.Maximum = (Decimal)EdgeWidthConverter.MaximumWidthWorkbook;

        ( new EdgeStyleConverter() ).PopulateComboBox(cbxStyle, true);

        nudAlpha.Minimum = (Decimal)AlphaConverter.MinimumAlphaWorkbook;
        nudAlpha.Maximum = (Decimal)AlphaConverter.MaximumAlphaWorkbook;

        EdgeVisibilityConverter oEdgeVisibilityConverter =
            new EdgeVisibilityConverter();

        String sHide = oEdgeVisibilityConverter.GraphToWorkbook(
            EdgeWorksheetReader.Visibility.Hide);

        cbxVisibility.PopulateWithObjectsAndText(

            NotEditedMarker, String.Empty,

            EdgeWorksheetReader.Visibility.Show,
                oEdgeVisibilityConverter.GraphToWorkbook(
                    EdgeWorksheetReader.Visibility.Show),

            EdgeWorksheetReader.Visibility.Skip,
                oEdgeVisibilityConverter.GraphToWorkbook(
                    EdgeWorksheetReader.Visibility.Skip),

            EdgeWorksheetReader.Visibility.Hide, sHide
            );

        SetVisibilityHelpText(lnkVisibility, sHide);

        nudLabelFontSize.Minimum =
            (Decimal)FontSizeConverter.MinimumFontSizeWorkbook;

        nudLabelFontSize.Maximum =
            (Decimal)FontSizeConverter.MaximumFontSizeWorkbook;

        DoDataExchange(false);

        AssertValid();
    }

    //*************************************************************************
    //  Property: EditedEdgeAttributes
    //
    /// <summary>
    /// Gets the attributes that were applied to the NodeXLControl's selected
    /// edges.
    /// </summary>
    ///
    /// <value>
    /// The attributes that were applied to the NodeXLControl's selected
    /// edges.
    /// </value>
    ///
    /// <remarks>
    /// Do not read this property if <see cref="Form.ShowDialog()" /> doesn't
    /// return DialogResult.OK.
    /// </remarks>
    //*************************************************************************

    public EditedEdgeAttributes
    EditedEdgeAttributes
    {
        get
        {
            Debug.Assert(this.DialogResult == DialogResult.OK);
            AssertValid();

            return (m_oEditedEdgeAttributes);
        }
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
            // Validate the controls.

            Nullable<Single> fWidth, fAlpha, fLabelFontSize;

            if (
                !ValidateSingleNumericUpDown(
                    nudWidth, "a width", out fWidth)
                ||
                !ValidateSingleNumericUpDown(
                    nudAlpha, "an opacity", out fAlpha)
                ||
                !ValidateSingleNumericUpDown(
                    nudLabelFontSize, "a label font size", out fLabelFontSize)
                )
            {
                return (false);
            }

            // The controls are valid.

            // Color.

            m_oEditedEdgeAttributes.Color = GetColorPickerColor(usrColor);

            // Width.

            m_oEditedEdgeAttributes.Width = fWidth;

            // Style.

            Object oSelectedStyle = cbxStyle.SelectedValue;

            m_oEditedEdgeAttributes.Style = (oSelectedStyle is String) ?
                null : ( Nullable<EdgeStyle> )oSelectedStyle;

            // Alpha.

            m_oEditedEdgeAttributes.Alpha = fAlpha;

            // Visibility.

            Object oSelectedVisibility = cbxVisibility.SelectedValue;
            m_oEditedEdgeAttributes.WorkbookMustBeReread = false;

            if (oSelectedVisibility is String)
            {
                m_oEditedEdgeAttributes.Visibility = null;
            }
            else
            {
                m_oEditedEdgeAttributes.Visibility = 
                    ( Nullable<EdgeWorksheetReader.Visibility> )
                        oSelectedVisibility;

                if (m_oEditedEdgeAttributes.Visibility !=
                    EdgeWorksheetReader.Visibility.Hide)
                {
                    // Doing anything except hiding an edge involves more than
                    // just editing edge metadata.  Force the workbook to be
                    // reread.

                    m_oEditedEdgeAttributes.WorkbookMustBeReread = true;
                }
            }

            // Label.

            m_oEditedEdgeAttributes.Label = txbLabel.Text;

            // Label text color.

            m_oEditedEdgeAttributes.LabelTextColor =
                GetColorPickerColor(usrLabelTextColor);

            // LabelFontSize.

            m_oEditedEdgeAttributes.LabelFontSize = fLabelFontSize;
        }
        else
        {
            // Color.

            SetColorPickerColor(usrColor, m_oEditedEdgeAttributes.Color);

            // Width.

            SetSingleNumericUpDown(nudWidth, m_oEditedEdgeAttributes.Width);

            // Style.

            if (m_oEditedEdgeAttributes.Style.HasValue)
            {
                cbxStyle.SelectedValue = m_oEditedEdgeAttributes.Style.Value;
            }
            else
            {
                cbxStyle.SelectedValue = NotEditedMarker;
            }

            // Alpha.

            SetSingleNumericUpDown(nudAlpha, m_oEditedEdgeAttributes.Alpha);

            // Visibility.

            if (m_oEditedEdgeAttributes.Visibility.HasValue)
            {
                cbxVisibility.SelectedValue =
                    m_oEditedEdgeAttributes.Visibility.Value;
            }
            else
            {
                cbxVisibility.SelectedValue = NotEditedMarker;
            }

            // Label.

            txbLabel.Text = m_oEditedEdgeAttributes.Label;

            // Label text color.

            SetColorPickerColor(usrLabelTextColor,
                m_oEditedEdgeAttributes.LabelTextColor);

            // LabelFontSize.

            SetSingleNumericUpDown(nudLabelFontSize,
                m_oEditedEdgeAttributes.LabelFontSize);
        }

        return (true);
    }

    //*************************************************************************
    //  Method: GetInitialEdgeAttributes()
    //
    /// <summary>
    /// Gets the list of attributes that should be shown to the user when the
    /// dialog opens.
    /// </summary>
    ///
    /// <returns>
    /// An <see cref="EditedEdgeAttributes" /> object.
    /// </returns>
    //*************************************************************************

    protected EditedEdgeAttributes
    GetInitialEdgeAttributes()
    {
        // AssertValid();

        EditedEdgeAttributes oInitialEdgeAttributes =
            new EditedEdgeAttributes();

        ICollection<IMetadataProvider> oSelectedEdges =
            ( ICollection<IMetadataProvider> )m_oNodeXLControl.SelectedEdges;

        // Color.

        oInitialEdgeAttributes.Color = GetInitialStructAttributeValue<Color>(
            ReservedMetadataKeys.PerColor);

        // Width.

        oInitialEdgeAttributes.Width = GetInitialSingleAttributeValue(
            oSelectedEdges, ReservedMetadataKeys.PerEdgeWidth,
            new EdgeWidthConverter() );

        // Style.

        oInitialEdgeAttributes.Style =
            GetInitialStructAttributeValue<EdgeStyle>(
                ReservedMetadataKeys.PerEdgeStyle);

        // Alpha.

        oInitialEdgeAttributes.Alpha = GetInitialSingleAttributeValue(
            oSelectedEdges, ReservedMetadataKeys.PerAlpha,
            new AlphaConverter() );

        // Visibility.

        oInitialEdgeAttributes.Visibility = null;

        // Label.

        oInitialEdgeAttributes.Label = GetInitialClassAttributeValue<String>(
            oSelectedEdges, ReservedMetadataKeys.PerEdgeLabel);

        // Label text color.

        oInitialEdgeAttributes.LabelTextColor =
            GetInitialStructAttributeValue<Color>(
                ReservedMetadataKeys.PerEdgeLabelTextColor);

        // Label font size.

        Nullable<Single> oLabelFontSize =
            GetInitialStructAttributeValue<Single>(
                oSelectedEdges, ReservedMetadataKeys.PerEdgeLabelFontSize);

        if (oLabelFontSize.HasValue)
        {
            oLabelFontSize = ( new FontSizeConverter() ).GraphToWorkbook(
                oLabelFontSize.Value);
        }

        oInitialEdgeAttributes.LabelFontSize = oLabelFontSize;

        return (oInitialEdgeAttributes);
    }

    //*************************************************************************
    //  Method: GetInitialStructAttributeValue()
    //
    /// <summary>
    /// Gets an attribute value that should be shown to the user when the
    /// dialog opens.
    /// </summary>
    ///
    /// <typeparam name="T">
    /// The type of the value.  Must be a struct.
    /// </typeparam>
    ///
    /// <param name="sKey">
    /// Key under which the optional attribute value is stored in each edge.
    /// </param>
    ///
    /// <returns>
    /// The initial value to use.
    /// </returns>
    //*************************************************************************

    protected Nullable<T>
    GetInitialStructAttributeValue<T>
    (
        String sKey
    )
    where T : struct
    {
        Debug.Assert(m_oNodeXLControl != null);
        // AssertValid();

        return ( GetInitialStructAttributeValue<T>(
            ( ICollection<IMetadataProvider> )m_oNodeXLControl.SelectedEdges,
            sKey) );
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
        EventArgs e
    )
    {
        AssertValid();

        if ( !DoDataExchange(true) )
        {
            return;
        }

        // If the caller is going to force the workbook to be reread, there
        // is no point in editing the edges' metadata.

        if (!m_oEditedEdgeAttributes.WorkbookMustBeReread)
        {
            this.UseWaitCursor = true;

            EdgeWidthConverter oEdgeWidthConverter = new EdgeWidthConverter();
            EdgeStyleConverter oEdgeStyleConverter = new EdgeStyleConverter();
            AlphaConverter oAlphaConverter = new AlphaConverter();
            FontSizeConverter oFontSizeConverter = new FontSizeConverter();

            foreach (IEdge oEdge in m_oNodeXLControl.SelectedEdges)
            {
                SetValue<Color>(oEdge, ReservedMetadataKeys.PerColor,
                    m_oEditedEdgeAttributes.Color);

                SetSingleValue(oEdge, ReservedMetadataKeys.PerEdgeWidth,
                    m_oEditedEdgeAttributes.Width, oEdgeWidthConverter);

                SetValue<EdgeStyle>(oEdge, ReservedMetadataKeys.PerEdgeStyle,
                    m_oEditedEdgeAttributes.Style);

                SetSingleValue(oEdge, ReservedMetadataKeys.PerAlpha,
                    m_oEditedEdgeAttributes.Alpha, oAlphaConverter);

                if (m_oEditedEdgeAttributes.Visibility.HasValue)
                {
                    Debug.Assert(m_oEditedEdgeAttributes.Visibility.Value ==
                        EdgeWorksheetReader.Visibility.Hide);

                    oEdge.SetValue(ReservedMetadataKeys.Visibility,
                        VisibilityKeyValue.Hidden);
                }

                SetStringValue(oEdge, ReservedMetadataKeys.PerEdgeLabel,
                    m_oEditedEdgeAttributes.Label);

                SetValue<Color>(oEdge,
                    ReservedMetadataKeys.PerEdgeLabelTextColor,
                    m_oEditedEdgeAttributes.LabelTextColor);

                if (m_oEditedEdgeAttributes.LabelFontSize.HasValue)
                {
                    oEdge.SetValue( ReservedMetadataKeys.PerEdgeLabelFontSize,
                        oFontSizeConverter.WorkbookToGraph(
                            m_oEditedEdgeAttributes.LabelFontSize.Value) );
                }
            }

            m_oNodeXLControl.DrawGraph();

            this.UseWaitCursor = false;
        }

        DialogResult = DialogResult.OK;
        this.Close();
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

        Debug.Assert(m_oEdgeAttributesDialogUserSettings != null);
        Debug.Assert(m_oEditedEdgeAttributes != null);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// User settings for this dialog.

    protected EdgeAttributesDialogUserSettings
        m_oEdgeAttributesDialogUserSettings;

    /// List of edge attributes that were edited.

    protected EditedEdgeAttributes m_oEditedEdgeAttributes;
}


//*****************************************************************************
//  Class: EdgeAttributesDialogUserSettings
//
/// <summary>
/// Stores the user's settings for the <see cref="EdgeAttributesDialog" />.
/// </summary>
///
/// <remarks>
/// The user settings include the form size and location.
/// </remarks>
//*****************************************************************************

[ SettingsGroupNameAttribute("EdgeAttributesDialog") ]

public class EdgeAttributesDialogUserSettings : FormSettings
{
    //*************************************************************************
    //  Constructor: EdgeAttributesDialogUserSettings()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="EdgeAttributesDialogUserSettings" /> class.
    /// </summary>
    ///
    /// <param name="oForm">
    /// The form to save settings for.
    /// </param>
    //*************************************************************************

    public EdgeAttributesDialogUserSettings
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
}

}
