

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
//  Class: VertexAttributesDialog
//
/// <summary>
/// Dialog that lets the user edit the attributes of the selected vertices in a
/// NodeXLControl.
/// </summary>
///
/// <remarks>
/// Pass the NodeXLControl to the constructor.  If the user performs any edits
/// and the edits don't require that the workbook be read again, this dialog
/// applies the edits to the NodeXLControl's selected vertices and returns
/// DialogResult.OK from <see cref="Form.ShowDialog()" />.  The list of edited
/// attributes can then be obtained from the <see
/// cref="EditedVertexAttributes" /> property.
///
/// <para>
/// If the user performs edits that require that the workbook be read again,
/// no edits are applied to the selected vertices.  Instead, the caller must
/// force the workbook to be read again.  The caller can determine if this is
/// required by checking the <see
/// cref="ExcelTemplate.EditedVertexAttributes.WorkbookMustBeReread" />
/// property.
/// </para>
///
/// </remarks>
//*****************************************************************************

public partial class VertexAttributesDialog : AttributesDialogBase
{
    //*************************************************************************
    //  Constructor: VertexAttributesDialog()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="VertexAttributesDialog" />
    /// class.
    /// </summary>
    ///
    /// <param name="nodeXLControl">
    /// NodeXLControl whose vertex attributes need to be edited.
    /// </param>
    //*************************************************************************

    public VertexAttributesDialog
    (
        NodeXLControl nodeXLControl
    )
    : base(nodeXLControl)
    {
        InitializeComponent();

        // Instantiate an object that retrieves and saves the user settings for
        // this dialog.  Note that the object automatically saves the settings
        // when the form closes.

        m_oVertexAttributesDialogUserSettings =
            new VertexAttributesDialogUserSettings(this);

        m_oEditedVertexAttributes = GetInitialVertexAttributes();

        ( new VertexShapeConverter() ).PopulateComboBox(cbxShape, true);

        nudRadius.Minimum =
            (Decimal)VertexRadiusConverter.MinimumRadiusWorkbook;

        nudRadius.Maximum = 
            (Decimal)VertexRadiusConverter.MaximumRadiusWorkbook;

        nudAlpha.Minimum = (Decimal)AlphaConverter.MinimumAlphaWorkbook;
        nudAlpha.Maximum = (Decimal)AlphaConverter.MaximumAlphaWorkbook;

        VertexVisibilityConverter oVertexVisibilityConverter = 
            new VertexVisibilityConverter();

        String sHide = oVertexVisibilityConverter.GraphToWorkbook(
            VertexWorksheetReader.Visibility.Hide);

        cbxVisibility.PopulateWithObjectsAndText(

            NotEditedMarker, String.Empty,

            VertexWorksheetReader.Visibility.ShowIfInAnEdge,
                oVertexVisibilityConverter.GraphToWorkbook(
                    VertexWorksheetReader.Visibility.ShowIfInAnEdge),

            VertexWorksheetReader.Visibility.Skip,
                oVertexVisibilityConverter.GraphToWorkbook(
                    VertexWorksheetReader.Visibility.Skip),

            VertexWorksheetReader.Visibility.Hide,
                sHide,

            VertexWorksheetReader.Visibility.Show,
                oVertexVisibilityConverter.GraphToWorkbook(
                    VertexWorksheetReader.Visibility.Show)
            );

        SetVisibilityHelpText(lnkVisibility, sHide);

        ( new VertexLabelPositionConverter() ).PopulateComboBox(
            cbxLabelPosition, true);

        BooleanConverter oBooleanConverter = new BooleanConverter();

        oBooleanConverter.PopulateComboBox(cbxLocked, true);
        oBooleanConverter.PopulateComboBox(cbxMarked, true);

        DoDataExchange(false);

        AssertValid();
    }

    //*************************************************************************
    //  Property: EditedVertexAttributes
    //
    /// <summary>
    /// Gets the attributes that were applied to the NodeXLControl's selected
    /// vertices.
    /// </summary>
    ///
    /// <value>
    /// The attributes that were applied to the NodeXLControl's selected
    /// vertices.
    /// </value>
    ///
    /// <remarks>
    /// Do not read this property if <see cref="Form.ShowDialog()" /> doesn't
    /// return DialogResult.OK.
    /// </remarks>
    //*************************************************************************

    public EditedVertexAttributes
    EditedVertexAttributes
    {
        get
        {
            Debug.Assert(this.DialogResult == DialogResult.OK);
            AssertValid();

            return (m_oEditedVertexAttributes);
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

            Nullable<Single> fRadius, fAlpha;

            if (
                !ValidateSingleNumericUpDown(
                    nudRadius, "a size", out fRadius)
                ||
                !ValidateSingleNumericUpDown(
                    nudAlpha, "an opacity", out fAlpha)
                )
            {
                return (false);
            }

            // The controls are valid.

            // Color.

            m_oEditedVertexAttributes.Color = GetColorPickerColor(usrColor);

            // Shape.

            Object oSelectedShape = cbxShape.SelectedValue;

            m_oEditedVertexAttributes.Shape = (oSelectedShape is String) ?
                null : ( Nullable<VertexShape> )oSelectedShape;

            m_oEditedVertexAttributes.Radius = fRadius;
            m_oEditedVertexAttributes.Alpha = fAlpha;

            // Visibility.

            Object oSelectedVisibility = cbxVisibility.SelectedValue;
            m_oEditedVertexAttributes.WorkbookMustBeReread = false;

            if (oSelectedVisibility is String)
            {
                m_oEditedVertexAttributes.Visibility = null;
            }
            else
            {
                m_oEditedVertexAttributes.Visibility = 
                    ( Nullable<VertexWorksheetReader.Visibility> )
                        oSelectedVisibility;

                if (m_oEditedVertexAttributes.Visibility !=
                    VertexWorksheetReader.Visibility.Hide)
                {
                    // Doing anything except hiding a vertex involves more than
                    // just editing vertex metadata.  Force the workbook to be
                    // reread.

                    m_oEditedVertexAttributes.WorkbookMustBeReread = true;
                }
            }

            // Label.

            m_oEditedVertexAttributes.Label = txbLabel.Text;

            // Label fill color.

            m_oEditedVertexAttributes.LabelFillColor =
                GetColorPickerColor(usrLabelFillColor);

            // Label position.

            Object oSelectedLabelPosition = cbxLabelPosition.SelectedValue;

            m_oEditedVertexAttributes.LabelPosition =
                (oSelectedLabelPosition is String) ?
                null :
                ( Nullable<VertexLabelPosition> )oSelectedLabelPosition;

            // Tooltip.

            m_oEditedVertexAttributes.ToolTip = txbToolTip.Text;

            // Locked.

            Object oSelectedLocked = cbxLocked.SelectedValue;

            m_oEditedVertexAttributes.Locked = (oSelectedLocked is String) ?
                null : ( Nullable<Boolean> )oSelectedLocked;

            // Marked.

            Object oSelectedMarked = cbxMarked.SelectedValue;

            m_oEditedVertexAttributes.Marked = (oSelectedMarked is String) ?
                null : ( Nullable<Boolean> )oSelectedMarked;
        }
        else
        {
            // Color.

            SetColorPickerColor(usrColor, m_oEditedVertexAttributes.Color);

            // Shape.

            if (m_oEditedVertexAttributes.Shape.HasValue)
            {
                cbxShape.SelectedValue =
                    m_oEditedVertexAttributes.Shape.Value;
            }
            else
            {
                cbxShape.SelectedValue = NotEditedMarker;
            }

            // Radius.

            SetSingleNumericUpDown(
                nudRadius, m_oEditedVertexAttributes.Radius);

            // Alpha.

            SetSingleNumericUpDown(nudAlpha, m_oEditedVertexAttributes.Alpha);

            // Visibility.

            if (m_oEditedVertexAttributes.Visibility.HasValue)
            {
                cbxVisibility.SelectedValue =
                    m_oEditedVertexAttributes.Visibility.Value;
            }
            else
            {
                cbxVisibility.SelectedValue = NotEditedMarker;
            }

            // Label.

            txbLabel.Text = m_oEditedVertexAttributes.Label;

            // Label fill color.

            SetColorPickerColor(usrLabelFillColor,
                m_oEditedVertexAttributes.LabelFillColor);

            // Label position.

            if (m_oEditedVertexAttributes.LabelPosition.HasValue)
            {
                cbxLabelPosition.SelectedValue =
                    m_oEditedVertexAttributes.LabelPosition.Value;
            }
            else
            {
                cbxLabelPosition.SelectedValue = NotEditedMarker;
            }

            // Tooltip.

            txbToolTip.Text = m_oEditedVertexAttributes.ToolTip;

            // Locked.

            if (m_oEditedVertexAttributes.Locked.HasValue)
            {
                cbxLocked.SelectedValue =
                    m_oEditedVertexAttributes.Locked.Value;
            }
            else
            {
                cbxLocked.SelectedValue = NotEditedMarker;
            }

            // Marked.

            if (m_oEditedVertexAttributes.Marked.HasValue)
            {
                cbxMarked.SelectedValue =
                    m_oEditedVertexAttributes.Marked.Value;
            }
            else
            {
                cbxMarked.SelectedValue = NotEditedMarker;
            }
        }

        return (true);
    }

    //*************************************************************************
    //  Method: GetInitialVertexAttributes()
    //
    /// <summary>
    /// Gets the list of attributes that should be shown to the user when the
    /// dialog opens.
    /// </summary>
    ///
    /// <returns>
    /// A <see cref="EditedVertexAttributes" /> object.
    /// </returns>
    //*************************************************************************

    protected EditedVertexAttributes
    GetInitialVertexAttributes()
    {
        // AssertValid();

        EditedVertexAttributes oInitialVertexAttributes =
            new EditedVertexAttributes();

        ICollection<IMetadataProvider> oSelectedVertices =
            ( ICollection<IMetadataProvider> )m_oNodeXLControl.SelectedVertices;

        // Color.

        oInitialVertexAttributes.Color =
            GetInitialStructAttributeValue<Color>(
                ReservedMetadataKeys.PerColor);

        // Shape.

        oInitialVertexAttributes.Shape =
            GetInitialStructAttributeValue<VertexShape>(
                ReservedMetadataKeys.PerVertexShape);

        // Radius.

        oInitialVertexAttributes.Radius = GetInitialSingleAttributeValue(
            oSelectedVertices, ReservedMetadataKeys.PerVertexRadius,
            new VertexRadiusConverter() );

        // Alpha.

        oInitialVertexAttributes.Alpha = GetInitialSingleAttributeValue(
            oSelectedVertices, ReservedMetadataKeys.PerAlpha,
            new AlphaConverter() );

        // Visibility.

        oInitialVertexAttributes.Visibility = null;

        // Label.

        oInitialVertexAttributes.Label = GetInitialClassAttributeValue<String>(
            oSelectedVertices, ReservedMetadataKeys.PerVertexLabel);

        // Label fill color.

        oInitialVertexAttributes.LabelFillColor =
            GetInitialStructAttributeValue<Color>(
                ReservedMetadataKeys.PerVertexLabelFillColor);

        // Label position.

        oInitialVertexAttributes.LabelPosition =
            GetInitialStructAttributeValue<VertexLabelPosition>(
                ReservedMetadataKeys.PerVertexLabelPosition);

        // Tooltip.

        oInitialVertexAttributes.ToolTip =
            GetInitialClassAttributeValue<String>(oSelectedVertices,
                ReservedMetadataKeys.PerVertexToolTip);

        // Locked.

        oInitialVertexAttributes.Locked =
            GetInitialStructAttributeValue<Boolean>(
                ReservedMetadataKeys.LockVertexLocation);

        // Marked.

        oInitialVertexAttributes.Marked =
            GetInitialStructAttributeValue<Boolean>(
                ReservedMetadataKeys.Marked);

        return (oInitialVertexAttributes);
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
    /// Key under which the optional attribute value is stored in each vertex.
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
            ( ICollection<IMetadataProvider> )m_oNodeXLControl.SelectedVertices,
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
        // is no point in editing the vertices' metadata.

        if (!m_oEditedVertexAttributes.WorkbookMustBeReread)
        {
            this.UseWaitCursor = true;

            VertexRadiusConverter oVertexRadiusConverter =
                new VertexRadiusConverter();

            AlphaConverter oAlphaConverter = new AlphaConverter();

            foreach (IVertex oVertex in m_oNodeXLControl.SelectedVertices)
            {
                SetValue<Color>(oVertex, ReservedMetadataKeys.PerColor,
                    m_oEditedVertexAttributes.Color);

                SetValue<VertexShape>(oVertex,
                    ReservedMetadataKeys.PerVertexShape,
                    m_oEditedVertexAttributes.Shape);

                SetSingleValue(oVertex, ReservedMetadataKeys.PerVertexRadius,
                    m_oEditedVertexAttributes.Radius, oVertexRadiusConverter);

                SetSingleValue(oVertex, ReservedMetadataKeys.PerAlpha,
                    m_oEditedVertexAttributes.Alpha, oAlphaConverter);

                if (m_oEditedVertexAttributes.Visibility.HasValue)
                {
                    Debug.Assert(m_oEditedVertexAttributes.Visibility.Value ==
                        VertexWorksheetReader.Visibility.Hide);

                    // Hide the vertex and its incident edges.

                    oVertex.SetValue(ReservedMetadataKeys.Visibility,
                        VisibilityKeyValue.Hidden);

                    foreach (IEdge oIncidentEdge in oVertex.IncidentEdges)
                    {
                        oIncidentEdge.SetValue(ReservedMetadataKeys.Visibility, 
                            VisibilityKeyValue.Hidden);
                    }
                }

                SetStringValue(oVertex, ReservedMetadataKeys.PerVertexLabel,
                    m_oEditedVertexAttributes.Label);

                SetValue<Color>(oVertex,
                    ReservedMetadataKeys.PerVertexLabelFillColor,
                    m_oEditedVertexAttributes.LabelFillColor);

                SetValue<VertexLabelPosition>(oVertex,
                    ReservedMetadataKeys.PerVertexLabelPosition,
                    m_oEditedVertexAttributes.LabelPosition);

                SetStringValue(oVertex, ReservedMetadataKeys.PerVertexToolTip,
                    m_oEditedVertexAttributes.ToolTip);

                SetValue<Boolean>(oVertex,
                    ReservedMetadataKeys.LockVertexLocation,
                    m_oEditedVertexAttributes.Locked);

                SetValue<Boolean>(oVertex, ReservedMetadataKeys.Marked,
                    m_oEditedVertexAttributes.Marked);
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

        Debug.Assert(m_oVertexAttributesDialogUserSettings != null);
        Debug.Assert(m_oEditedVertexAttributes != null);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// User settings for this dialog.

    protected VertexAttributesDialogUserSettings
        m_oVertexAttributesDialogUserSettings;

    /// List of vertex attributes that were edited.

    protected EditedVertexAttributes m_oEditedVertexAttributes;
}


//*****************************************************************************
//  Class: VertexAttributesDialogUserSettings
//
/// <summary>
/// Stores the user's settings for the <see cref="VertexAttributesDialog" />.
/// </summary>
///
/// <remarks>
/// The user settings include the form size and location.
/// </remarks>
//*****************************************************************************

[ SettingsGroupNameAttribute("VertexAttributesDialog5") ]

public class VertexAttributesDialogUserSettings : FormSettings
{
    //*************************************************************************
    //  Constructor: VertexAttributesDialogUserSettings()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="VertexAttributesDialogUserSettings" /> class.
    /// </summary>
    ///
    /// <param name="oForm">
    /// The form to save settings for.
    /// </param>
    //*************************************************************************

    public VertexAttributesDialogUserSettings
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
