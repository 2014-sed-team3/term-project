
using System;
using System.Globalization;
using System.Diagnostics;
using Smrf.AppLib;
using Smrf.DateTimeLib;

namespace Smrf.NodeXL.GraphDataProviders
{
//*****************************************************************************
//  Class: NetworkDescriber
//
/// <summary>
/// Version of <see cref="SentenceConcatenator" /> specialized with additional
/// methods for describing a network.
/// </summary>
//*****************************************************************************

public class NetworkDescriber : SentenceConcatenator
{
    //*************************************************************************
    //  Constructor: NetworkDescriber()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="NetworkDescriber" />
    /// class.
    /// </summary>
    //*************************************************************************

    public NetworkDescriber()
    {
        // (Do nothing.)

        AssertValid();
    }

    //*************************************************************************
    //  Method: AddNetworkTime()
    //
    /// <summary>
    /// Adds a sentence to the network description that says when the network
    /// was obtained.
    /// </summary>
    ///
    /// <param name="networkSource">
    /// Where the network came from.  Sample: "Twitter".
    /// </param>
    //*************************************************************************

    public void
    AddNetworkTime
    (
        String networkSource
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(networkSource) );
        AssertValid();

        AddEventTime("The network was obtained from " + networkSource,
            DateTime.UtcNow);
    }

    //*************************************************************************
    //  Method: AddEventTime()
    //
    /// <summary>
    /// Adds a sentence to the network description that says when an event
    /// occurred.
    /// </summary>
    ///
    /// <param name="prefix">
    /// Description of the event.  Sample: "The network was obtained from
    /// Twitter".
    /// </param>
    ///
    /// <param name="dateTimeUtc">
    /// Time the event occurred, in UTC.
    /// </param>
    //*************************************************************************

    public void
    AddEventTime
    (
        String prefix,
        DateTime dateTimeUtc
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(prefix) );
        AssertValid();

        // Sample: "The network was obtained from Twitter on Monday,
        // 05 March 2012 at 22:13 UTC."

        this.AddSentence(

            "{0} on {1}."
            ,
            prefix,
            FormatEventTime(dateTimeUtc)
            );
    }

    //*************************************************************************
    //  Method: FormatEventTime()
    //
    /// <summary>
    /// Formats the time that an event occurred.
    /// </summary>
    ///
    /// <param name="dateTimeUtc">
    /// Time the event occurred, in UTC.
    /// </param>
    ///
    /// <returns>
    /// Sample: "Monday, 05 March 2012 at 22:13 UTC"
    /// </returns>
    //*************************************************************************

    public String
    FormatEventTime
    (
        DateTime dateTimeUtc
    )
    {
        AssertValid();

        return ( String.Format(

            "{0} at {1} UTC"
            ,
            dateTimeUtc.ToString("D", DateTimeFormatInfo.InvariantInfo),
            dateTimeUtc.ToString("t", DateTimeFormatInfo.InvariantInfo)
            ) );
    }

    //*************************************************************************
    //  Method: FormatDuration()
    //
    /// <summary>
    /// Formats the duration of a period of time.
    /// </summary>
    ///
    /// <param name="startOfPeriodUtc">
    /// Start time of the period, in UTC.
    /// </param>
    ///
    /// <param name="endOfPeriodUtc">
    /// End time of the period, in UTC.
    /// </param>
    ///
    /// <returns>
    /// Sample: "2-day, 23-hour, 4-minute".
    /// </returns>
    //*************************************************************************

    public String
    FormatDuration
    (
        DateTime startOfPeriodUtc,
        DateTime endOfPeriodUtc
    )
    {
        AssertValid();

        TimeSpan oDuration = endOfPeriodUtc - startOfPeriodUtc;

        return ( oDuration.Ticks < 0 ? String.Empty :
            DateTimeUtil2.FormatDuration(oDuration) );
    }

    //*************************************************************************
    //  Method: AddNetworkLimit()
    //
    /// <summary>
    /// Adds a sentence to the network description that says how the network
    /// was limited, if it was limited.
    /// </summary>
    ///
    /// <param name="maximumObjects">
    /// Maximum number of objects in the network, or Int32.MaxValue for no
    /// limit.
    /// </param>
    ///
    /// <param name="objectDescriptionPlural">
    /// Plural form of a description of the objects in the network.  Sample:
    /// "people".
    /// </param>
    //*************************************************************************

    public void
    AddNetworkLimit
    (
        Int32 maximumObjects,
        String objectDescriptionPlural
    )
    {
        Debug.Assert(maximumObjects > 0);
        Debug.Assert( !String.IsNullOrEmpty(objectDescriptionPlural) );
        AssertValid();

        if (maximumObjects != Int32.MaxValue)
        {
            this.AddSentence(

                "The network was limited to {0} {1}."
                ,
                maximumObjects,
                objectDescriptionPlural
                );
        }
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
    //  Protected fields
    //*************************************************************************

    // (None.)
}

}
