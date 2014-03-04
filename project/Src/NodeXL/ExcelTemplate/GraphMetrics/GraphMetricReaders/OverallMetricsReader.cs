
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Office.Interop.Excel;
using Smrf.NodeXL.Core;
using Smrf.NodeXL.Algorithms;
using Smrf.AppLib;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: OverallMetricsReader
//
/// <summary>
/// Class that knows how to read the overall metrics from a NodeXL workbook.
/// </summary>
///
/// <remarks>
/// Call <see cref="TryReadMetrics" /> to attempt to read the overall metrics.
/// </remarks>
//*****************************************************************************

public class OverallMetricsReader : Object
{
    //*************************************************************************
    //  Constructor: OverallMetricsReader()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="OverallMetricsReader" />
    /// class.
    /// </summary>
    //*************************************************************************

    public OverallMetricsReader()
    {
        // (Do nothing.)

        AssertValid();
    }

    //*************************************************************************
    //  Method: TryReadMetrics()
    //
    /// <summary>
    /// Attempts to read the overall metrics from a workbook.
    /// </summary>
    ///
    /// <param name="workbook">
    /// Workbook containing the graph data.
    /// </param>
    ///
    /// <param name="overallMetrics">
    /// Where the overall metrics get stored if true is returned.  Gets set to
    /// null if false is returned.
    /// </param>
    ///
    /// <returns>
    /// true if successful.
    /// </returns>
    ///
    /// <remarks>
    /// This method attempts to read the overall metrics that have already been
    /// calculated and written to a workbook.  If it successfully reads all of
    /// them, they get stored at <paramref name="overallMetrics" /> and true is
    /// returned.  false is returned if any are missing or invalid.
    /// </remarks>
    //*************************************************************************

    public Boolean
    TryReadMetrics
    (
        Microsoft.Office.Interop.Excel.Workbook workbook,
        out OverallMetrics overallMetrics
    )
    {
        Debug.Assert(workbook != null);
        AssertValid();

        overallMetrics = null;

        Object [,] aoValues;

        // The key is an overall metric name and the value is the one-based row
        // number of the overall metric.

        Dictionary<String, Int32> oValueRowsOneBased =
            new Dictionary<String, Int32>();

        if ( !TryGetValues(workbook, out aoValues, out oValueRowsOneBased) )
        {
            return (false);
        }

        Int32 iRowOneBased;
        String sDirectedness;
        GraphDirectedness eDirectedness;

        if (
            !oValueRowsOneBased.TryGetValue(
                OverallMetricNames.Directedness, out iRowOneBased)
            ||
            !ExcelUtil.TryGetNonEmptyStringFromCell(aoValues, iRowOneBased, 1,
                out sDirectedness)
            )
        {
            return (false);
        }

        try
        {
            eDirectedness = (GraphDirectedness)Enum.Parse(
                typeof(GraphDirectedness), sDirectedness);
        }
        catch (ArgumentException)
        {
            return (false);
        }

        Int32 iUniqueEdges, iEdgesWithDuplicates, iSelfLoops, iVertices,
            iConnectedComponents, iSingleVertexConnectedComponents,
            iMaximumConnectedComponentVertices,
            iMaximumConnectedComponentEdges;

        Nullable<Double> dGraphDensity, dModularity, dAverageGeodesicDistance,
            dReciprocatedVertexPairRatio, dReciprocatedEdgeRatio;

        Nullable<Int32> iMaximumGeodesicDistance;

        if (
            !TryGetInt32Value(oValueRowsOneBased, aoValues,
                OverallMetricNames.UniqueEdges, out iUniqueEdges)
            ||
            !TryGetInt32Value(oValueRowsOneBased, aoValues,
                OverallMetricNames.EdgesWithDuplicates,
                out iEdgesWithDuplicates)
            ||
            !TryGetInt32Value(oValueRowsOneBased, aoValues,
                OverallMetricNames.SelfLoops, out iSelfLoops)
            ||
            !TryGetInt32Value(oValueRowsOneBased, aoValues,
                OverallMetricNames.Vertices, out iVertices)
            ||
            !TryGetNullableDoubleValue(oValueRowsOneBased, aoValues,
                OverallMetricNames.GraphDensity, out dGraphDensity)
            ||
            !TryGetNullableDoubleValue(oValueRowsOneBased, aoValues,
                OverallMetricNames.Modularity, out dModularity)
            ||
            !TryGetInt32Value(oValueRowsOneBased, aoValues,
                OverallMetricNames.ConnectedComponents,
                out iConnectedComponents)
            ||
            !TryGetInt32Value(oValueRowsOneBased, aoValues,
                OverallMetricNames.SingleVertexConnectedComponents,
                out iSingleVertexConnectedComponents)
            ||
            !TryGetInt32Value(oValueRowsOneBased, aoValues,
                OverallMetricNames.MaximumConnectedComponentVertices,
                out iMaximumConnectedComponentVertices)
            ||
            !TryGetInt32Value(oValueRowsOneBased, aoValues,
                OverallMetricNames.MaximumConnectedComponentEdges,
                out iMaximumConnectedComponentEdges)
            ||
            !TryGetNullableInt32Value(oValueRowsOneBased, aoValues,
                OverallMetricNames.MaximumGeodesicDistance,
                out iMaximumGeodesicDistance)
            ||
            !TryGetNullableDoubleValue(oValueRowsOneBased, aoValues,
                OverallMetricNames.AverageGeodesicDistance,
                out dAverageGeodesicDistance)
            ||
            !TryGetNullableDoubleValue(oValueRowsOneBased, aoValues,
                OverallMetricNames.ReciprocatedVertexPairRatio,
                out dReciprocatedVertexPairRatio)
            ||
            !TryGetNullableDoubleValue(oValueRowsOneBased, aoValues,
                OverallMetricNames.ReciprocatedEdgeRatio,
                out dReciprocatedEdgeRatio)
            )
        {
            return (false);
        }

        overallMetrics = new OverallMetrics(eDirectedness, iUniqueEdges,
            iEdgesWithDuplicates, iSelfLoops, iVertices, dGraphDensity,
            dModularity, iConnectedComponents,
            iSingleVertexConnectedComponents,
            iMaximumConnectedComponentVertices,
            iMaximumConnectedComponentEdges, iMaximumGeodesicDistance,
            dAverageGeodesicDistance, dReciprocatedVertexPairRatio,
            dReciprocatedEdgeRatio);

        return (true);
    }

    //*************************************************************************
    //  Method: TryGetValues()
    //
    /// <summary>
    /// Attempts to get the overall metric values from a workbook.
    /// </summary>
    ///
    /// <param name="oWorkbook">
    /// Workbook containing the graph data.
    /// </param>
    ///
    /// <param name="aoValues">
    /// Where the values from the value column gets stored.
    /// </param>
    ///
    /// <param name="oValueRowsOneBased">
    /// Where a Dictionary gets stored if true is returned.  The key is an
    /// overall metric name and the value is the one-based row number of the
    /// overall metric.
    /// </param>
    ///
    /// <returns>
    /// true if successful.
    /// </returns>
    //*************************************************************************

    protected Boolean
    TryGetValues
    (
        Microsoft.Office.Interop.Excel.Workbook oWorkbook,
        out Object [,] aoValues,
        out Dictionary<String, Int32> oValueRowsOneBased
    )
    {
        Debug.Assert(oWorkbook != null);
        AssertValid();

        Object [,] aoNames;

        if ( TryGetNamesAndValues(oWorkbook, out aoNames, out aoValues) )
        {
            oValueRowsOneBased = new Dictionary<String, Int32>();

            for (Int32 iRowOneBased = 1;
                iRowOneBased <= aoNames.GetUpperBound(0);
                iRowOneBased++)
            {
                String sName;

                if ( ExcelUtil.TryGetNonEmptyStringFromCell(aoNames,
                    iRowOneBased, 1, out sName) )
                {
                    oValueRowsOneBased[sName] = iRowOneBased;
                }
            }

            return (true);
        }

        oValueRowsOneBased = null;
        return (false);
    }

    //*************************************************************************
    //  Method: TryGetNamesAndValues()
    //
    /// <summary>
    /// Attempts to get the overall metric name and value column values from
    /// a workbook.
    /// </summary>
    ///
    /// <param name="oWorkbook">
    /// Workbook containing the graph data.
    /// </param>
    ///
    /// <param name="aoNames">
    /// Where the values from the name column gets stored.
    /// </param>
    ///
    /// <param name="aoValues">
    /// Where the values from the value column gets stored.
    /// </param>
    ///
    /// <returns>
    /// true if successful.
    /// </returns>
    //*************************************************************************

    protected Boolean
    TryGetNamesAndValues
    (
        Microsoft.Office.Interop.Excel.Workbook oWorkbook,
        out Object [,] aoNames,
        out Object [,] aoValues
    )
    {
        Debug.Assert(oWorkbook != null);
        AssertValid();

        aoNames = aoValues = null;

        ListObject oOverallMetricsTable;
        Range oRange;

        return (
            ExcelTableUtil.TryGetTable(oWorkbook,
                WorksheetNames.OverallMetrics, TableNames.OverallMetrics,
                out oOverallMetricsTable)
            &&
            ExcelTableUtil.TryGetTableColumnDataAndValues(
                oOverallMetricsTable, OverallMetricsTableColumnNames.Name,
                out oRange, out aoNames)
            &&
            ExcelTableUtil.TryGetTableColumnDataAndValues(
                oOverallMetricsTable, OverallMetricsTableColumnNames.Value,
                out oRange, out aoValues)
            );
    }

    //*************************************************************************
    //  Method: TryGetInt32Value()
    //
    /// <summary>
    /// Attempts to get an Int32 overall metric value.
    /// </summary>
    ///
    /// <param name="oValueRowsOneBased">
    /// The key is an overall metric name and the value is the one-based table
    /// row number of the overall metric.
    /// </param>
    ///
    /// <param name="aoValues">
    /// The values from the value column.
    /// </param>
    ///
    /// <param name="sName">
    /// The name of the overall metric to get.
    /// </param>
    ///
    /// <param name="iValue">
    /// Where the Int32 overall metric value gets stored if true is returned.
    /// </param>
    ///
    /// <returns>
    /// true if successful.
    /// </returns>
    //*************************************************************************

    protected Boolean
    TryGetInt32Value
    (
        Dictionary<String, Int32> oValueRowsOneBased,
        Object [,] aoValues,
        String sName,
        out Int32 iValue
    )
    {
        Debug.Assert(oValueRowsOneBased != null);
        Debug.Assert(aoValues != null);
        Debug.Assert( !String.IsNullOrEmpty(sName) );
        AssertValid();

        Int32 iRowOneBased;
        iValue = Int32.MinValue;

        return (
            oValueRowsOneBased.TryGetValue(sName, out iRowOneBased)
            &&
            ExcelUtil.TryGetInt32FromCell(aoValues, iRowOneBased, 1,
                out iValue)
            );
    }

    //*************************************************************************
    //  Method: TryGetNullableInt32Value()
    //
    /// <summary>
    /// Attempts to get a Nullable Int32 overall metric value.
    /// </summary>
    ///
    /// <param name="oValueRowsOneBased">
    /// The key is an overall metric name and the value is the one-based table
    /// row number of the overall metric.
    /// </param>
    ///
    /// <param name="aoValues">
    /// The values from the value column.
    /// </param>
    ///
    /// <param name="sName">
    /// The name of the overall metric to get.
    /// </param>
    ///
    /// <param name="iValue">
    /// Where the Nullable Int32 overall metric value gets stored if true is
    /// returned.
    /// </param>
    ///
    /// <returns>
    /// true if successful.
    /// </returns>
    //*************************************************************************

    protected Boolean
    TryGetNullableInt32Value
    (
        Dictionary<String, Int32> oValueRowsOneBased,
        Object [,] aoValues,
        String sName,
        out Nullable<Int32> iValue
    )
    {
        Debug.Assert(oValueRowsOneBased != null);
        Debug.Assert(aoValues != null);
        Debug.Assert( !String.IsNullOrEmpty(sName) );
        AssertValid();

        return ( TryGetNullableValue<Int32>(oValueRowsOneBased, aoValues,
            sName, ExcelUtil.TryGetInt32FromCell, out iValue) );
    }

    //*************************************************************************
    //  Method: TryGetNullableDoubleValue()
    //
    /// <summary>
    /// Attempts to get a Nullable Double overall metric value.
    /// </summary>
    ///
    /// <param name="oValueRowsOneBased">
    /// The key is an overall metric name and the value is the one-based table
    /// row number of the overall metric.
    /// </param>
    ///
    /// <param name="aoValues">
    /// The values from the value column.
    /// </param>
    ///
    /// <param name="sName">
    /// The name of the overall metric to get.
    /// </param>
    ///
    /// <param name="dValue">
    /// Where the Nullable Double overall metric value gets stored if true is
    /// returned.
    /// </param>
    ///
    /// <returns>
    /// true if successful.
    /// </returns>
    //*************************************************************************

    protected Boolean
    TryGetNullableDoubleValue
    (
        Dictionary<String, Int32> oValueRowsOneBased,
        Object [,] aoValues,
        String sName,
        out Nullable<Double> dValue
    )
    {
        Debug.Assert(oValueRowsOneBased != null);
        Debug.Assert(aoValues != null);
        Debug.Assert( !String.IsNullOrEmpty(sName) );
        AssertValid();

        return ( TryGetNullableValue<Double>(oValueRowsOneBased, aoValues,
            sName, ExcelUtil.TryGetDoubleFromCell, out dValue) );
    }

    //*************************************************************************
    //  Method: TryGetNullableValue()
    //
    /// <summary>
    /// Attempts to get a Nullable overall metric value.
    /// </summary>
    ///
    /// <typeparam name="TValue">
    /// The type of the overall metric value to get.
    /// </typeparam>
    ///
    /// <param name="oValueRowsOneBased">
    /// The key is an overall metric name and the value is the one-based table
    /// row number of the overall metric.
    /// </param>
    ///
    /// <param name="aoValues">
    /// The values from the value column.
    /// </param>
    ///
    /// <param name="sName">
    /// The name of the overall metric to get.
    /// </param>
    ///
    /// <param name="oTryGetValueMethod">
    /// Represents a method that attempts to get a value of a specified type
    /// from a worksheet cell given an array of cell values read from the
    /// worksheet.
    /// </param>
    ///
    /// <param name="tValue">
    /// Where the Nullable overall metric value gets stored if true is
    /// returned.
    /// </param>
    ///
    /// <returns>
    /// true if successful.
    /// </returns>
    //*************************************************************************

    protected Boolean
    TryGetNullableValue<TValue>
    (
        Dictionary<String, Int32> oValueRowsOneBased,
        Object [,] aoValues,
        String sName,
        ExcelUtil.TryGetValueFromCell<TValue> oTryGetValueMethod,
        out Nullable<TValue> tValue
    )
    where TValue : struct
    {
        Debug.Assert(oValueRowsOneBased != null);
        Debug.Assert(aoValues != null);
        Debug.Assert( !String.IsNullOrEmpty(sName) );
        AssertValid();

        tValue = null;
        Int32 iRowOneBased;

        if ( !oValueRowsOneBased.TryGetValue(sName, out iRowOneBased) )
        {
            // The row doesn't exist, which is an error.

            return (false);
        }

        TValue tMightBeValue;

        if ( oTryGetValueMethod(aoValues, iRowOneBased, 1, out tMightBeValue) )
        {
            // The row exists and has value of type TValue.  (Otherwise, the
            // value is probably
            // GraphMetricCalculatorBase.NotApplicableMessage, in which case
            // tValue remains null.)

            tValue = tMightBeValue;
        }

        return (true);
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

    // (None.)
}

}
