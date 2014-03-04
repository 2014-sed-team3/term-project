
using System;
using System.Text;
using System.Diagnostics;
using Microsoft.Office.Interop.Excel;
using Smrf.AppLib;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: TwitterSearchNetworkTopItemsReader
//
/// <summary>
/// Class that knows how to read the the "top items in the tweets within a
/// Twitter search network" from a NodeXL workbook.
/// </summary>
///
/// <remarks>
/// Call <see cref="TopMetricsReaderBase.TryReadMetrics" /> to attempt to read
/// the top Twitter items.
///
/// <para>
/// This class does not calculate the top items.  Instead, it reads the top
/// items that were calculated by <see
/// cref="TwitterSearchNetworkTopItemsCalculator2" /> and written to the
/// workbook.
/// </para>
///
/// </remarks>
//*****************************************************************************

public class TwitterSearchNetworkTopItemsReader : TopMetricsReaderBase
{
    //*************************************************************************
    //  Constructor: TwitterSearchNetworkTopItemsReader()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="TwitterSearchNetworkTopItemsReader" /> class.
    /// </summary>
    //*************************************************************************

    public TwitterSearchNetworkTopItemsReader()
    :
    base(WorksheetNames.TwitterSearchNetworkTopItems,
        TableNames.TwitterSearchNetworkTopItemsRoot)
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
