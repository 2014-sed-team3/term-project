
using System;
using System.Diagnostics;
using Smrf.WpfGraphicsLib;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: FontSizeConverter
//
/// <summary>
/// Class that converts font size values between those used in the Excel
/// workbook and those used in the NodeXL graph.
/// </summary>
//*****************************************************************************

public class FontSizeConverter : Object
{
    //*************************************************************************
    //  Constructor: FontSizeConverter()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="FontSizeConverter" />
    /// class.
    /// </summary>
    //*************************************************************************

    public FontSizeConverter()
    {
        // (Do nothing.)

        AssertValid();
    }

    //*************************************************************************
    //  Method: WorkbookToGraph()
    //
    /// <summary>
    /// Converts an Excel workbook value to a value suitable for use in a NodeXL
    /// graph.
    /// </summary>
    ///
    /// <param name="valueWorkbook">
    /// Value read from the Excel workbook.
    /// </param>
    ///
    /// <returns>
    /// A value suitable for use in a NodeXL graph.  If the value is less than
    /// <see cref="MinimumFontSizeWorkbook" />, <see
    /// cref="MinimumFontSizeWorkbook" /> is used.  If the value is greater
    /// than <see cref="MaximumFontSizeWorkbook" />, <see
    /// cref="MaximumFontSizeWorkbook" /> is used.
    /// </returns>
    //*************************************************************************

    public Single
    WorkbookToGraph
    (
        Single valueWorkbook
    )
    {
        AssertValid();

        // Pin the value to the font size limits.

        valueWorkbook = Math.Max(valueWorkbook, MinimumFontSizeWorkbook);
        valueWorkbook = Math.Min(valueWorkbook, MaximumFontSizeWorkbook);

        // Values in the workbook are in the same units as those used by the
        // standard FontDialog.  They need to be converted to WPF units.

        return ( (Single)WpfGraphicsUtil.SystemDrawingFontSizeToWpfFontSize(
            valueWorkbook) );
    }

    //*************************************************************************
    //  Method: GraphToWorkbook()
    //
    /// <summary>
    /// Converts a NodeXL graph value to a value suitable for use in an Excel
    /// workbook.
    /// </summary>
    ///
    /// <param name="graphValue">
    /// Value stored in a NodeXL graph.
    /// </param>
    ///
    /// <returns>
    /// A value suitable for use in an Excel workbook.
    /// </returns>
    //*************************************************************************

    public Single
    GraphToWorkbook
    (
        Single graphValue
    )
    {
        AssertValid();

        return ( (Single)WpfGraphicsUtil.WpfFontSizeToSystemDrawingFontSize(
            graphValue) );
    }


    //*************************************************************************
    //  Method: AssertValid()
    //
    /// <summary>
    /// Asserts if the object is in an invalid state.  Debug-only.
    /// </summary>
    //*************************************************************************

    [Conditional("DEBUG")]

    public virtual void
    AssertValid()
    {
        // (Do nothing.)
    }


    //*************************************************************************
    //  Public constants
    //*************************************************************************

    /// <summary>
    /// Minimum value that can be specified in the workbook for a font size.
    /// </summary>

    public static readonly Single MinimumFontSizeWorkbook = 8F;

    /// <summary>
    /// Maximum value that can be specified in the workbook for a font size.
    /// </summary>

    public static readonly Single MaximumFontSizeWorkbook = 72F;


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}

}
