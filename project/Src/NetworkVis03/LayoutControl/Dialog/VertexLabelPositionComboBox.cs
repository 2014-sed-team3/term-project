using System;
using System.Diagnostics;
using Smrf.AppLib;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: VertexLabelPositionComboBox
//
/// <summary>
/// Represents a ComboBox that lists the options for vertex label positions.
/// </summary>
///
/// <remarks>
/// Call <see cref="Populate" /> to populate the ComboBox with vertex label
/// positions.
/// </remarks>
//*****************************************************************************

public class VertexLabelPositionComboBox : ComboBoxPlus
{
    //*************************************************************************
    //  Constructor: VertexLabelPositionComboBox()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="VertexLabelPositionComboBox" /> class.
    /// </summary>
    //*************************************************************************

    public VertexLabelPositionComboBox()
    {
        // (Do nothing.)

        AssertValid();
    }


    //*************************************************************************
    //  Method: Populate()
    //
    /// <summary>
    /// Populates the ComboBox with vertex label positions.
    /// </summary>
    //*************************************************************************

    public void
    Populate()
    {
        ( new VertexLabelPositionConverter() ).PopulateComboBox(this, false);
    }


    //*************************************************************************
    //  Method: AssertValid()
    //
    /// <summary>
    /// Asserts if the object is in an invalid state.  Debug-only.
    /// </summary>
    //*************************************************************************

    [Conditional("DEBUG")]

    public void
    AssertValid()
    {
        // (Do nothing.)
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.);
}

}
