
using System;
using System.Configuration;
using System.Diagnostics;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: ReadabilityMetricUserSettings
//
/// <summary>
/// Stores the user's settings for calculating readability metrics.
/// </summary>
//*****************************************************************************

[ SettingsGroupNameAttribute("ReadabilityMetricUserSettings") ]

public class ReadabilityMetricUserSettings : NodeXLApplicationSettingsBase
{
    //*************************************************************************
    //  Constructor: ReadabilityMetricUserSettings()
    //
    /// <summary>
    /// Initializes a new instance of the ReadabilityMetricUserSettings class.
    /// </summary>
    //*************************************************************************

    public ReadabilityMetricUserSettings()
    {
        // (Do nothing.)

        AssertValid();
    }

    //*************************************************************************
    //  Property: ReadabilityMetricsToCalculate
    //
    /// <summary>
    /// Gets or sets the readability metrics to calculate.
    /// </summary>
    ///
    /// <value>
    /// The readability metrics to calculate, as an ORed combination of <see
    /// cref="ReadabilityMetrics" /> flags.  The default value is
    /// ReadabilityMetrics.OverallEdgeCrossings |
    /// ReadabilityMetrics.OverallVertexOverlap.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute(
        "OverallEdgeCrossings, OverallVertexOverlap") ]

    public ReadabilityMetrics
    ReadabilityMetricsToCalculate
    {
        get
        {
            AssertValid();

            return ( (ReadabilityMetrics)this[
                ReadabilityMetricsToCalculateKey] );
        }

        set
        {
            this[ReadabilityMetricsToCalculateKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Method: ShouldCalculateReadabilityMetrics
    //
    /// <summary>
    /// Gets a flag specifying whether any one of a set of specified
    /// readability metrics should be calculated.
    /// </summary>
    ///
    /// <param name="readabilityMetrics">
    /// An ORed combination of ReadabilityMetrics flags.  If any of the flags
    /// are set, true is returned.
    /// </param>
    ///
    /// <returns>
    /// true if any of the specified readability metrics should be calculated.
    /// </returns>
    //*************************************************************************

    public Boolean
    ShouldCalculateReadabilityMetrics
    (
        ReadabilityMetrics readabilityMetrics
    )
    {
        AssertValid();

        return ( (this.ReadabilityMetricsToCalculate & readabilityMetrics)
            != 0 );
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

    /// Name of the settings key for the ReadabilityMetricsToCalculate
    /// property.

    protected const String ReadabilityMetricsToCalculateKey =
        "ReadabilityMetricsToCalculate";


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}

}
