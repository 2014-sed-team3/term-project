
using System;
using System.Text;
using System.Diagnostics;
using Microsoft.Office.Interop.Excel;
using Smrf.AppLib;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: TopNByMetricsReader
//
/// <summary>
/// Class that knows how to read the top-N-by metrics from a NodeXL workbook.
/// </summary>
///
/// <remarks>
/// Call <see cref="TopMetricsReaderBase.TryReadMetrics" /> to attempt to read
/// the top-N-by metrics.
///
/// <para>
/// This class does not calculate the metrics.  Instead, it reads the metrics
/// that were calculated by <see cref="TopNByMetricCalculator2" /> and written
/// to the workbook.
/// </para>
///
/// </remarks>
//*****************************************************************************

public class TopNByMetricsReader : TopMetricsReaderBase
{
    //*************************************************************************
    //  Constructor: TopNByMetricsReader()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="TopNByMetricsReader" />
    /// class.
    /// </summary>
    //*************************************************************************

    public TopNByMetricsReader()
    :
    base(WorksheetNames.TopNByMetrics, TableNames.TopNByMetricsRoot)
    {
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

    [Conditional("DEBUG")]

    public new void
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
