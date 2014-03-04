
using System;
using System.ComponentModel;
using System.Diagnostics;
using Smrf.NodeXL.Core;
using Smrf.NodeXL.Algorithms;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: CalculateGraphMetricsContext
//
/// <summary>
/// Provides access to objects needed for calculating graph metrics.
/// </summary>
///
/// <remarks>
/// An instance of this class gets passed to <see
/// cref="IGraphMetricCalculator2.TryCalculateGraphMetrics" />.
/// </remarks>
//*****************************************************************************

public class CalculateGraphMetricsContext : Object
{
    //*************************************************************************
    //  Constructor: CalculateGraphMetricsContext()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="CalculateGraphMetricsContext" /> class.
    /// </summary>
    ///
    /// <param name="graphMetricUserSettings">
    /// Stores the user's settings for calculating graph metrics.
    /// </param>
    ///
    /// <param name="backgroundWorker">
    /// The BackgroundWorker object that is performing all graph metric
    /// calculations.
    /// </param>
    //*************************************************************************

    public CalculateGraphMetricsContext
    (
        GraphMetricUserSettings graphMetricUserSettings,
        BackgroundWorker backgroundWorker
    )
    {
        m_oGraphMetricUserSettings = graphMetricUserSettings;
        m_oBackgroundWorker = backgroundWorker;

        AssertValid();
    }

    //*************************************************************************
    //  Property: GraphMetricUserSettings
    //
    /// <summary>
    /// Gets the object that stores the user's settings for calculating graph
    /// metrics.
    /// </summary>
    ///
    /// <value>
    /// A GraphMetricUserSettings object.
    /// </value>
    //*************************************************************************

    public GraphMetricUserSettings
    GraphMetricUserSettings
    {
        get
        {
            AssertValid();

            return (m_oGraphMetricUserSettings);
        }
    }

    //*************************************************************************
    //  Property: BackgroundWorker
    //
    /// <summary>
    /// Gets the BackgroundWorker object that is performing all graph metric
    /// calculations.
    /// </summary>
    ///
    /// <value>
    /// The BackgroundWorker object that is performing all graph metric
    /// calculations.
    /// </value>
    //*************************************************************************

    public BackgroundWorker
    BackgroundWorker
    {
        get
        {
            AssertValid();

            return (m_oBackgroundWorker);
        }
    }

    //*************************************************************************
    //  Method: ShouldCalculateGraphMetrics
    //
    /// <summary>
    /// Gets a flag specifying whether any one of a set of specified graph
    /// metrics should be calculated.
    /// </summary>
    ///
    /// <param name="graphMetrics">
    /// An ORed combination of GraphMetrics flags.  If any of the flags are
    /// set, true is returned.
    /// </param>
    ///
    /// <returns>
    /// true if any of the specified graph metrics should be calculated.
    /// </returns>
    //*************************************************************************

    public Boolean
    ShouldCalculateGraphMetrics
    (
        GraphMetrics graphMetrics
    )
    {
        AssertValid();

        return (m_oGraphMetricUserSettings.ShouldCalculateGraphMetrics(
            graphMetrics) );
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
        Debug.Assert(m_oGraphMetricUserSettings != null);
        Debug.Assert(m_oBackgroundWorker != null);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Stores the user's settings for calculating graph metrics.

    protected GraphMetricUserSettings m_oGraphMetricUserSettings;

    /// Object that is performing all graph metric calculations.

    protected BackgroundWorker m_oBackgroundWorker;
}

}
