

using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;
using System.Diagnostics;
using Smrf.AppLib;
using Smrf.NodeXL.Core;
using Smrf.NodeXL.Visualization.Wpf;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: AttributesDialogBase
//
/// <summary>
/// Base class for dialogs that let the user edit the attributes of the
/// selected edges or vertices in a NodeXLControl.
/// </summary>
//*****************************************************************************

public partial class AttributesDialogBase : ExcelTemplateForm
{
    //*************************************************************************
    //  Constructor: AttributesDialogBase()
    //
    /// <overloads>
    /// Initializes a new instance of the <see cref="AttributesDialogBase" />
    /// class.
    /// </overloads>
    ///
    /// <summary>
    /// Initializes a new instance of the <see cref="AttributesDialogBase" />
    /// class.
    /// </summary>
    ///
    /// <param name="nodeXLControl">
    /// NodeXLControl whose edge or vertex attributes need to be edited.
    /// </param>
    //*************************************************************************

    public AttributesDialogBase
    (
        NodeXLControl nodeXLControl
    )
    {
        m_oNodeXLControl = nodeXLControl;

        // AssertValid();
    }

    //*************************************************************************
    //  Constructor: AttributesDialogBase()
    //
    /// <summary>
    /// Do not use this constructor.  It is for the Visual Studio designer
    /// only.
    /// </summary>
    //*************************************************************************

    public AttributesDialogBase()
    {
        // (Do nothing.)

        // AssertValid();
    }

    //*************************************************************************
    //  Method: SetVisibilityHelpText()
    //
    /// <summary>
    /// Sets the help text on the HelpLinkLabel the user can click to learn
    /// more about what happens when the visibility is changed.
    /// </summary>
    ///
    /// <param name="oVisibilityHelpLinkLabel">
    /// The HelpLinkLabel.
    /// </param>
    ///
    /// <param name="sHideText">
    /// The visibility "Hide" value.
    /// </param>
    //*************************************************************************

    protected void
    SetVisibilityHelpText
    (
        HelpLinkLabel oVisibilityHelpLinkLabel,
        String sHideText
    )
    {
        Debug.Assert(oVisibilityHelpLinkLabel != null);
        Debug.Assert( !String.IsNullOrEmpty(sHideText) );
        // AssertValid();

        oVisibilityHelpLinkLabel.Tag = String.Format(

            "Changing the Visibility to anything except \"{0}\" causes the"
            + " graph to be refreshed."
            ,
            sHideText
            );
    }

    //*************************************************************************
    //  Method: GetInitialSingleAttributeValue()
    //
    /// <summary>
    /// Gets a Single attribute value that should be shown to the user when the
    /// dialog opens.
    /// </summary>
    ///
    /// <param name="oSelectedEdgesOrVertices">
    /// Collection of selected edges or vertices.
    /// </param>
    ///
    /// <param name="sKey">
    /// Key under which the optional attribute value is stored on each edge or
    /// vertex.
    /// </param>
    ///
    /// <param name="oNumericValueConverter">
    /// Object that will convert a numeric graph value to a workbook value.
    /// </param>
    ///
    /// <returns>
    /// The attribute value to show to the user, or null.
    /// </returns>
    //*************************************************************************

    protected Nullable<Single>
    GetInitialSingleAttributeValue
    (
        ICollection<IMetadataProvider> oSelectedEdgesOrVertices,
        String sKey,
        INumericValueConverter oNumericValueConverter
    )
    {
        Debug.Assert(oSelectedEdgesOrVertices != null);
        Debug.Assert( !String.IsNullOrEmpty(sKey) );
        Debug.Assert(oNumericValueConverter != null);
        // AssertValid();

        Nullable<Single> fInitialValue =
            GetInitialStructAttributeValue<Single>(
                oSelectedEdgesOrVertices, sKey);

        if (!fInitialValue.HasValue)
        {
            return (null);
        }

        return ( oNumericValueConverter.GraphToWorkbook(
            fInitialValue.Value) );
    }

    //*************************************************************************
    //  Method: GetInitialStructAttributeValue()
    //
    /// <summary>
    /// Gets a struct attribute value that should be shown to the user when the
    /// dialog opens.
    /// </summary>
    ///
    /// <typeparam name="T">
    /// The type of the value.  Must be a struct.
    /// </typeparam>
    ///
    /// <param name="oSelectedEdgesOrVertices">
    /// Collection of selected edges or vertices.
    /// </param>
    ///
    /// <param name="sKey">
    /// Key under which the optional attribute value is stored on each edge or
    /// vertex.
    /// </param>
    ///
    /// <returns>
    /// The initial value to use.
    /// </returns>
    //*************************************************************************

    protected Nullable<T>
    GetInitialStructAttributeValue<T>
    (
        ICollection<IMetadataProvider> oSelectedEdgesOrVertices,
        String sKey
    )
    where T : struct
    {
        Debug.Assert(oSelectedEdgesOrVertices != null);
        Debug.Assert( !String.IsNullOrEmpty(sKey) );
        // AssertValid();

        Object oInitialAttributeValue = GetInitialClassAttributeValue<Object>(
            oSelectedEdgesOrVertices, sKey);

        if (oInitialAttributeValue != null)
        {
            return ( (T)oInitialAttributeValue );
        }

        return (null);
    }

    //*************************************************************************
    //  Method: GetInitialClassAttributeValue()
    //
    /// <summary>
    /// Gets a class attribute value that should be shown to the user when the
    /// dialog opens.
    /// </summary>
    ///
    /// <typeparam name="T">
    /// The type of the value.  Must be a class.
    /// </typeparam>
    ///
    /// <param name="oSelectedEdgesOrVertices">
    /// Collection of selected edges or vertices.
    /// </param>
    ///
    /// <param name="sKey">
    /// Key under which the optional attribute value is stored on each edge or
    /// vertex.
    /// </param>
    ///
    /// <returns>
    /// The initial value to use.
    /// </returns>
    //*************************************************************************

    protected T
    GetInitialClassAttributeValue<T>
    (
        ICollection<IMetadataProvider> oSelectedEdgesOrVertices,
        String sKey
    )
    where T : class
    {
        Debug.Assert(oSelectedEdgesOrVertices != null);
        Debug.Assert( !String.IsNullOrEmpty(sKey) );
        // AssertValid();

        // If every edge or vertex has the same value, that should be the
        // initial value shown to the user.  Otherwise, null should be shown.

        Object oFirstValue = null;

        foreach (IMetadataProvider oEdgeOrVertex in oSelectedEdgesOrVertices)
        {
            Object oValueForThisVertex;

            if (
                !oEdgeOrVertex.TryGetValue(sKey, out oValueForThisVertex)
                ||
                oValueForThisVertex == null
                )
            {
                return (null);
            }

            if (oFirstValue == null)
            {
                oFirstValue = oValueForThisVertex;
            }
            else if ( !oFirstValue.Equals(oValueForThisVertex) )
            {
                return (null);
            }
        }

        return ( (T)oFirstValue );
    }

    //*************************************************************************
    //  Method: ValidateSingleNumericUpDown()
    //
    /// <summary>
    /// Validates a NumericUpDown control that contains a Nullable Single.
    /// </summary>
    ///
    /// <param name="oNumericUpDown">
    /// NumericUpDown to validate.
    /// </param>
    ///
    /// <param name="sValueDescription">
    /// Description of what the control contains, in lower case.  Sample:
    /// "a length".
    /// </param>
    ///
    /// <param name="fValue">
    /// Where the validated value gets stored.
    /// </param>
    ///
    /// <returns>
    /// true if the validation was successful.
    /// </returns>
    //*************************************************************************

    protected Boolean
    ValidateSingleNumericUpDown
    (
        NumericUpDown oNumericUpDown,
        String sValueDescription,
        out Nullable<Single> fValue
    )
    {
        Debug.Assert(oNumericUpDown != null);
        Debug.Assert( !String.IsNullOrEmpty(sValueDescription) );

        fValue = null;
        Single fValue2;

        if (oNumericUpDown.Text == NotEditedMarker)
        {
            fValue = null;
        }
        else if ( ValidateNumericUpDown(
            oNumericUpDown, sValueDescription, out fValue2) )
        {
            fValue = fValue2;
        }
        else
        {
            return (false);
        }

        return (true);
    }

    //*************************************************************************
    //  Method: SetSingleNumericUpDown()
    //
    /// <summary>
    /// Sets a Nullable Single on a NumericUpDown control.
    /// </summary>
    ///
    /// <param name="oNumericUpDown">
    /// The NumericUpDown control to set the Nullable Single on.
    /// </param>
    ///
    /// <param name="oSingle">
    /// The Nullable Single to set.
    /// </param>
    //*************************************************************************

    protected void
    SetSingleNumericUpDown
    (
        NumericUpDown oNumericUpDown,
        Nullable<Single> oSingle
    )
    {
        Debug.Assert(oNumericUpDown != null);

        if (oSingle.HasValue)
        {
            oNumericUpDown.Value = (Decimal)oSingle.Value;
        }
        else
        {
            oNumericUpDown.Text = NotEditedMarker;
        }
    }

    //*************************************************************************
    //  Method: GetColorPickerColor()
    //
    /// <summary>
    /// Gets a Nullable Color from a ColorPicker control.
    /// </summary>
    ///
    /// <param name="oColorPicker">
    /// The ColorPicker control to get the Nullable Color from.
    /// </param>
    ///
    /// <returns>
    /// The Nullable Color.
    /// </returns>
    //*************************************************************************

    protected Nullable<Color>
    GetColorPickerColor
    (
        ColorPicker oColorPicker
    )
    {
        Debug.Assert(oColorPicker != null);

        Color oColor = oColorPicker.Color;

        if (oColor == ColorNotEditedMarker)
        {
            return (null);
        }

        return (oColor);
    }

    //*************************************************************************
    //  Method: SetColorPickerColor()
    //
    /// <summary>
    /// Sets a Nullable Color on a ColorPicker control.
    /// </summary>
    ///
    /// <param name="oColorPicker">
    /// The ColorPicker control to set the Nullable Color on.
    /// </param>
    ///
    /// <param name="oColor">
    /// The Nullable Color to set.
    /// </param>
    //*************************************************************************

    protected void
    SetColorPickerColor
    (
        ColorPicker oColorPicker,
        Nullable<Color> oColor
    )
    {
        Debug.Assert(oColorPicker != null);

        if (oColor.HasValue)
        {
            oColorPicker.Color = oColor.Value;
        }
        else
        {
            oColorPicker.Color = ColorNotEditedMarker;
        }
    }

    //*************************************************************************
    //  Method: SetValue()
    //
    /// <summary>
    /// Sets a metadata value on an edge or vertex if the value is not null.
    /// </summary>
    ///
    /// <typeparam name="T">
    /// The type of the value.  Must be a struct.
    /// </typeparam>
    ///
    /// <param name="oEdgeOrVertex">
    /// The edge or vertex to set the value on.
    /// </param>
    ///
    /// <param name="sKey">
    /// Key to set.
    /// </param>
    ///
    /// <param name="oValue">
    /// Value to set.  Can be null.
    /// </param>
    //*************************************************************************

    protected void
    SetValue<T>
    (
        IMetadataProvider oEdgeOrVertex,
        String sKey,
        Nullable<T> oValue
    )
    where T : struct
    {
        Debug.Assert(oEdgeOrVertex != null);
        Debug.Assert( !String.IsNullOrEmpty(sKey) );
        AssertValid();

        if (oValue.HasValue)
        {
            oEdgeOrVertex.SetValue(sKey, oValue.Value);
        }
    }

    //*************************************************************************
    //  Method: SetSingleValue()
    //
    /// <summary>
    /// Sets a Single metadata value on an edge or vertex if the value is not
    /// null.
    /// </summary>
    ///
    /// <param name="oEdgeOrVertex">
    /// The edge or vertex to set the value on.
    /// </param>
    ///
    /// <param name="sKey">
    /// Key to set.
    /// </param>
    ///
    /// <param name="oValue">
    /// Value to set.  Can be null.
    /// </param>
    ///
    /// <param name="oNumericValueConverter">
    /// Object that will convert a numeric graph value to a workbook value.
    /// </param>
    //*************************************************************************

    protected void
    SetSingleValue
    (
        IMetadataProvider oEdgeOrVertex,
        String sKey,
        Nullable<Single> oValue,
        INumericValueConverter oNumericValueConverter
    )
    {
        Debug.Assert(oEdgeOrVertex != null);
        Debug.Assert( !String.IsNullOrEmpty(sKey) );
        Debug.Assert(oNumericValueConverter != null);
        AssertValid();

        if (oValue.HasValue)
        {
            oEdgeOrVertex.SetValue( sKey,
                oNumericValueConverter.WorkbookToGraph(oValue.Value) );
        }
    }

    //*************************************************************************
    //  Method: SetStringValue()
    //
    /// <summary>
    /// Sets a String metadata value on an edge or vertex if the value is not
    /// null or empty.
    /// </summary>
    ///
    /// <param name="oEdgeOrVertex">
    /// The edge or vertex to set the value on.
    /// </param>
    ///
    /// <param name="sKey">
    /// Key to set.
    /// </param>
    ///
    /// <param name="sValue">
    /// Value to set.  Can be null or empty.
    /// </param>
    //*************************************************************************

    protected void
    SetStringValue
    (
        IMetadataProvider oEdgeOrVertex,
        String sKey,
        String sValue
    )
    {
        Debug.Assert(oEdgeOrVertex != null);
        Debug.Assert( !String.IsNullOrEmpty(sKey) );
        AssertValid();

        if ( !String.IsNullOrEmpty(sValue) )
        {
            oEdgeOrVertex.SetValue(sKey, sValue);
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

        Debug.Assert(m_oNodeXLControl != null);
    }


    //*************************************************************************
    //  Protected constants
    //*************************************************************************

    /// Object stored in a ComboBox as the value that represents "not edited."
    /// (Unfortunately, you can't set ComboBox.SelectedValue to null, so null
    /// can't be used as the marker.)

    protected String NotEditedMarker = String.Empty;

    /// Object stored in a ColorPicker as the value that represents "not
    /// edited."  This is arbitrary.  It is different from any color the user
    /// might actually select because it has an alpha value of 0.

    protected Color ColorNotEditedMarker = Color.FromArgb(0, 1, 1, 1);


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// NodeXLControl whose vertex attributes need to be edited.

    protected NodeXLControl m_oNodeXLControl;
}

}
