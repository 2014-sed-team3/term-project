
using System;
using Smrf.AppLib;
using System.Diagnostics;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: NumericValueConverterBase
//
/// <summary>
/// Base class for a family of classes that convert numeric values in the Excel
/// workbook to and from numeric values in the NodeXL graph.
/// </summary>
//*****************************************************************************

public class NumericValueConverterBase : Object, INumericValueConverter
{
    //*************************************************************************
    //  Constructor: NumericValueConverterBase()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="NumericValueConverterBase" /> class.
    /// </summary>
    ///
    /// <param name="minimumValueWorkbook">
    /// Minimum value that can be specified in the workbook.
    /// </param>
    ///
    /// <param name="maximumValueWorkbook">
    /// Maximum value that can be specified in the workbook.  Must be greater
    /// than <paramref name="minimumValueWorkbook" />.
    /// </param>
    ///
    /// <param name="minimumValueGraph">
    /// Minimum value in the NodeXL graph.
    /// </param>
    ///
    /// <param name="maximumValueGraph">
    /// Maximum value in the NodeXL graph.  Must be greater than <paramref
    /// name="minimumValueGraph" />.
    /// </param>
    //*************************************************************************

    public NumericValueConverterBase
    (
        Single minimumValueWorkbook,
        Single maximumValueWorkbook,
        Single minimumValueGraph,
        Single maximumValueGraph
    )
    {
        m_fMinimumValueWorkbook = minimumValueWorkbook;
        m_fMaximumValueWorkbook = maximumValueWorkbook;
        m_fMinimumValueGraph = minimumValueGraph;
        m_fMaximumValueGraph = maximumValueGraph;

        // AssertValid();
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
    /// A value suitable for use in a NodeXL graph.  The value is pinned to the
    /// graph limits specified in the constructor.
    /// </returns>
    //*************************************************************************

    public Single
    WorkbookToGraph
    (
        Single valueWorkbook
    )
    {
        AssertValid();

        return ( MathUtil.TransformValueToRange(valueWorkbook,
            m_fMinimumValueWorkbook, m_fMaximumValueWorkbook,
            m_fMinimumValueGraph, m_fMaximumValueGraph) );
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
    /// A value suitable for use in an Excel workbook.  The value is pinned to
    /// the workbook limits specified in the constructor.
    /// </returns>
    //*************************************************************************

    public Single
    GraphToWorkbook
    (
        Single valueGraph
    )
    {
        AssertValid();

        return ( MathUtil.TransformValueToRange(valueGraph,
            m_fMinimumValueGraph, m_fMaximumValueGraph,
            m_fMinimumValueWorkbook, m_fMaximumValueWorkbook) );
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
        Debug.Assert(m_fMaximumValueWorkbook > m_fMinimumValueWorkbook);
        Debug.Assert(m_fMaximumValueGraph > m_fMinimumValueGraph);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Minimum value that can be specified in the workbook.

    protected Single m_fMinimumValueWorkbook;

    /// Maximum value that can be specified in the workbook.

    protected Single m_fMaximumValueWorkbook;

    /// Minimum value in the NodeXL graph.

    protected Single m_fMinimumValueGraph;

    /// Maximum value in the NodeXL graph.

    protected Single m_fMaximumValueGraph;
}

}
