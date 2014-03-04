
using System;
using System.Globalization;
using System.Diagnostics;
using Smrf.AppLib;

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
    //*************************************************************************

    public void
    AddNetworkTime()
    {
        AssertValid();

        AddEventTime("The network was obtained", DateTime.UtcNow);
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
    /// Description of the event.  Sample: "The network was obtained".
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

        // Sample: "The network was obtained on Monday, 05 March 2012 at
        // 22:13 UTC."

        this.AddSentence(

            "{0} on {1} at {2} UTC."
            ,
            prefix,
            dateTimeUtc.ToString("D", DateTimeFormatInfo.InvariantInfo),
            dateTimeUtc.ToString("t", DateTimeFormatInfo.InvariantInfo)
            );
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
