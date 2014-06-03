
using System;
using Smrf.NodeXL.Visualization.Wpf;
using Smrf.AppLib;
using System.Diagnostics;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: VertexRadiusConverter
//
/// <summary>
/// Class that converts a vertex radius between values used in the Excel
/// workbook and values used in the NodeXL graph.
/// </summary>
//*****************************************************************************

public class VertexRadiusConverter : Object, INumericValueConverter
{
    //*************************************************************************
    //  Constructor: VertexRadiusConverter()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="VertexRadiusConverter" />
    /// class.
    /// </summary>
    //*************************************************************************

    public VertexRadiusConverter()
    {
        // (Do nothing.)

        AssertValid();
    }

    //*************************************************************************
    //  Method: WorkbookToGraph()
    //
    /// <summary>
    /// Converts an Excel workbook value to a value suitable for use in a
    /// NodeXL graph.
    /// </summary>
    ///
    /// <param name="valueWorkbook">
    /// Value read from the Excel workbook.
    /// </param>
    ///
    /// <returns>
    /// A value suitable for use in a NodeXL graph.
    /// </returns>
    //*************************************************************************

    public Single
    WorkbookToGraph
    (
        Single valueWorkbook
    )
    {
        AssertValid();

        // See the VertexRadiusConverterAlgorithm.xlsx file in this folder for
        // information on where this formula and the one in GraphToWorkbook()
        // came from.

        valueWorkbook = Math.Max(valueWorkbook, MinimumRadiusWorkbook);
        valueWorkbook = Math.Min(valueWorkbook, MaximumRadiusWorkbook);

        Double dMinimumDiameterGraph = 2 * MinimumRadiusGraph;

        Double dDiameterGraph =

            2 * Math.Sqrt(
                ( ( Math.PI * Math.Pow( (dMinimumDiameterGraph / 2.0), 2.0) )
                    * valueWorkbook )
                /
                Math.PI
                );

        return ( (Single)(dDiameterGraph / 2.0) );
    }

    //*************************************************************************
    //  Method: GraphToWorkbook()
    //
    /// <summary>
    /// Converts a NodeXL graph value to a value suitable for use in an Excel
    /// workbook.
    /// </summary>
    ///
    /// <param name="valueGraph">
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
        Single valueGraph
    )
    {
        AssertValid();

        Double dMinimumDiameterGraph = 2 * MinimumRadiusGraph;
        Double dDiameterGraph = valueGraph * 2f;

        Double dValueWorkbook =
            ( Math.Pow(dDiameterGraph / 2.0, 2.0) )
            /
            ( Math.Pow( (dMinimumDiameterGraph / 2.0), 2.0) )
            ;
        
        dValueWorkbook = Math.Max(dValueWorkbook, MinimumRadiusWorkbook);
        dValueWorkbook = Math.Min(dValueWorkbook, MaximumRadiusWorkbook);

        return ( (Single)dValueWorkbook );
    }

    //*************************************************************************
    //  Method: WorkbookToLongerImageDimension()
    //
    /// <summary>
    /// Converts an Excel workbook value to an image dimension.
    /// </summary>
    ///
    /// <param name="valueWorkbook">
    /// Value read from the Excel workbook.
    /// </param>
    ///
    /// <returns>
    /// The longer image dimension to use for a vertex image.  This should be
    /// applied to either the width or the height of the image, whichever is
    /// longer.
    /// </returns>
    //*************************************************************************

    public Single
    WorkbookToLongerImageDimension
    (
        Single valueWorkbook
    )
    {
        AssertValid();

        return ( 2.0F * WorkbookToGraph(valueWorkbook) );
    }

    //*************************************************************************
    //  Method: WorkbookToLabelFontSize()
    //
    /// <summary>
    /// Converts an Excel workbook value to a font size to use for a vertex
    /// with the shape <see cref="VertexShape.Label" />.
    /// </summary>
    ///
    /// <param name="valueWorkbook">
    /// Value read from the Excel workbook.
    /// </param>
    ///
    /// <returns>
    /// The font size to use, in WPF units.
    /// </returns>
    //*************************************************************************

    public Single
    WorkbookToLabelFontSize
    (
        Single valueWorkbook
    )
    {
        AssertValid();

        return ( MathUtil.TransformValueToRange(valueWorkbook,
            MinimumRadiusWorkbook, MaximumRadiusWorkbook,
            MinimumLabelFontSize, MaximumLabelFontSize) );
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
    //  Protected constants
    //*************************************************************************

    /// <summary>
    /// Minimum radius in the NodeXL graph.
    /// </summary>

    protected static readonly Double MinimumRadiusGraph =
        25 * VertexDrawer.MinimumRadius;


    //*************************************************************************
    //  Public constants
    //*************************************************************************

    /// <summary>
    /// Minimum value that can be specified in the workbook for a vertex
    /// radius.
    /// </summary>

    public static readonly Single MinimumRadiusWorkbook = 1F;

    /// <summary>
    /// Maximum value that can be specified in the workbook for a vertex
    /// radius.
    /// </summary>

    public static readonly Single MaximumRadiusWorkbook = 1000F;

    /// <summary>
    /// The vertex label font size that corresponds to MinimumRadiusWorkbook,
    /// in WPF units.
    /// </summary>

    public static readonly Single MinimumLabelFontSize = 11F;

    /// <summary>
    /// The vertex label font size that corresponds to MaximumRadiusWorkbook,
    /// in WPF units.
    /// </summary>

    public static readonly Single MaximumLabelFontSize = 293.4F;


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}

}
