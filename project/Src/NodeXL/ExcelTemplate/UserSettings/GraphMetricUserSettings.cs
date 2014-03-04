﻿
using System;
using System.Configuration;
using System.Collections.Generic;
using System.Diagnostics;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: GraphMetricUserSettings
//
/// <summary>
/// Stores the user's settings for calculating graph metrics.
/// </summary>
//*****************************************************************************

[ SettingsGroupNameAttribute("GraphMetricUserSettings") ]

public class GraphMetricUserSettings : NodeXLApplicationSettingsBase
{
    //*************************************************************************
    //  Constructor: GraphMetricUserSettings()
    //
    /// <summary>
    /// Initializes a new instance of the GraphMetricUserSettings class.
    /// </summary>
    //*************************************************************************

    public GraphMetricUserSettings()
    {
        // (Do nothing.)

        AssertValid();
    }

    //*************************************************************************
    //  Property: GraphMetricsToCalculate
    //
    /// <summary>
    /// Gets or sets the graph metrics to calculate.
    /// </summary>
    ///
    /// <value>
    /// The graph metrics to calculate, as an ORed combination of <see
    /// cref="GraphMetrics" /> flags.  The default value is
    /// GraphMetrics.OverallMetrics.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("OverallMetrics") ]

    public GraphMetrics
    GraphMetricsToCalculate
    {
        get
        {
            AssertValid();

            return ( (GraphMetrics)this[GraphMetricsToCalculateKey] );
        }

        set
        {
            this[GraphMetricsToCalculateKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: WordMetricUserSettings
    //
    /// <summary>
    /// Gets or sets user settings for calculating word metrics.
    /// </summary>
    ///
    /// <value>
    /// A <see cref="Smrf.NodeXL.ExcelTemplate.WordMetricUserSettings" />
    /// object.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("") ]

    public WordMetricUserSettings
    WordMetricUserSettings
    {
        get
        {
            AssertValid();

            return (WordMetricUserSettings)this[WordMetricUserSettingsKey];
        }

        set
        {
            this[WordMetricUserSettingsKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: TopNByMetricsToCalculate
    //
    /// <summary>
    /// Gets or sets a collection of user settings for calculating top-N-by
    /// metrics.
    /// </summary>
    ///
    /// <value>
    /// A collection of <see
    /// cref="Smrf.NodeXL.ExcelTemplate.TopNByMetricUserSettings" /> objects.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("") ]

    public List<TopNByMetricUserSettings>
    TopNByMetricsToCalculate
    {
        get
        {
            AssertValid();

            return ( (List<TopNByMetricUserSettings>)
                this[TopNByMetricsToCalculateKey] );
        }

        set
        {
            this[TopNByMetricsToCalculateKey] = value;

            AssertValid();
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

        return ( (this.GraphMetricsToCalculate & graphMetrics) != 0 );
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
    //  Protected constants
    //*************************************************************************

    /// Name of the settings key for the GraphMetricsToCalculate property.

    protected const String GraphMetricsToCalculateKey =
        "GraphMetricsToCalculate";

    /// Name of the settings key for the WordMetricUserSettings property.

    protected const String WordMetricUserSettingsKey =
        "WordMetricUserSettings";

    /// Name of the settings key for the TopNByMetricsToCalculate property.

    protected const String TopNByMetricsToCalculateKey =
        "TopNByMetricsToCalculate";


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}

}
