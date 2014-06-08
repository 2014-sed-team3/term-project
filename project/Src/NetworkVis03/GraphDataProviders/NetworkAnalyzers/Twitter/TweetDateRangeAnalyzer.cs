
using System;
using System.Xml;
using System.Diagnostics;
using Smrf.XmlLib;
using Smrf.DateTimeLib;
using Smrf.NodeXL.GraphMLLib;

namespace Smrf.NodeXL.GraphDataProviders.Twitter
{
//*****************************************************************************
//  Class: TweetDateRangeAnalyzer
//
/// <summary>
/// Analyzes the range of tweet dates in a Twitter search network.
/// </summary>
//*****************************************************************************

public static class TweetDateRangeAnalyzer : Object
{
    //*************************************************************************
    //  Method: AddTweetDateRangeToNetworkDescription()
    //
    /// <summary>
    /// Adds the range of tweet dates in a Twitter search network to a network
    /// description.
    /// </summary>
    ///
    /// <param name="graphMLXmlDocument">
    /// The GraphMLXmlDocument that contains the Twitter search network.
    /// </param>
    ///
    /// <param name="networkDescriber">
    /// Concatenates sentences into a network description.
    /// </param>
    //*************************************************************************

    public static void
    AddTweetDateRangeToNetworkDescription
    (
        XmlDocument graphMLXmlDocument,
        NetworkDescriber networkDescriber
    )
    {
        Debug.Assert(graphMLXmlDocument != null);
        Debug.Assert(networkDescriber != null);

        XmlNamespaceManager oXmlNamespaceManager = new XmlNamespaceManager(
            graphMLXmlDocument.NameTable);

        oXmlNamespaceManager.AddNamespace("g",
            GraphMLXmlDocument.GraphMLNamespaceUri);

        DateTime oMinimumRelationshipDateUtc = DateTime.MaxValue;
        DateTime oMaximumRelationshipDateUtc = DateTime.MinValue;

        // Loop through the graph's edges.

        foreach ( XmlNode oEdgeXmlNode in
            graphMLXmlDocument.DocumentElement.SelectNodes(
                "g:graph/g:edge", oXmlNamespaceManager) )
        {
            // Get the value of the edge's "relationship" GraphML-Attribute.

            String sRelationship;

            if ( !TryGetEdgeGraphMLAttributeValue(oEdgeXmlNode,
                NodeXLGraphMLUtil.EdgeRelationshipID, oXmlNamespaceManager,
                out sRelationship) )
            {
                continue;
            }

            switch (sRelationship)
            {
                case TwitterGraphMLUtil.RepliesToRelationship:
                case TwitterGraphMLUtil.MentionsRelationship:
                case TwitterGraphMLUtil.NonRepliesToNonMentionsRelationship:

                    // Get the value of the edge's "relationship date"
                    // GraphML-Attribute.

                    String sRelationshipDateUtc;

                    if ( !TryGetEdgeGraphMLAttributeValue(oEdgeXmlNode,
                        TwitterGraphMLUtil.EdgeRelationshipDateUtcID,
                        oXmlNamespaceManager, out sRelationshipDateUtc) )
                    {
                        break;
                    }

                    DateTime oRelationshipDateUtc;

                    try
                    {
                        // Note that the relationship date may be in an
                        // unrecognized format.

                        oRelationshipDateUtc =
                            DateTimeUtil2.FromCultureInvariantString(
                                sRelationshipDateUtc);
                    }
                    catch (FormatException)
                    {
                        break;
                    }

                    if (oRelationshipDateUtc < oMinimumRelationshipDateUtc)
                    {
                        oMinimumRelationshipDateUtc = oRelationshipDateUtc;
                    }

                    if (oRelationshipDateUtc > oMaximumRelationshipDateUtc)
                    {
                        oMaximumRelationshipDateUtc = oRelationshipDateUtc;
                    }

                    break;

                default:

                    break;
            }
        }

        if (oMinimumRelationshipDateUtc != DateTime.MaxValue)
        {
            networkDescriber.AddSentence(
                "The tweets in the network were tweeted over the {0} period"
                + " from {1} to {2}."
                ,
                networkDescriber.FormatDuration(oMinimumRelationshipDateUtc, 
                    oMaximumRelationshipDateUtc),

                networkDescriber.FormatEventTime(oMinimumRelationshipDateUtc),
                networkDescriber.FormatEventTime(oMaximumRelationshipDateUtc)
                );
        }
    }

    //*************************************************************************
    //  Method: TryGetEdgeGraphMLAttributeValue()
    //
    /// <summary>
    /// Attempts to get the value of a specified GraphML-Attribute.
    /// </summary>
    ///
    /// <param name="oEdgeXmlNode">
    /// The XmlNode that represents the edge.
    /// </param>
    ///
    /// <param name="sAttributeID">
    /// The GraphML-Attribute's ID.
    /// </param>
    ///
    /// <param name="oXmlNamespaceManager">
    /// The XmlNamespaceManager to use.  It's assumed that "g" is used as the
    /// GraphML namespace prefix.
    /// </param>
    ///
    /// <param name="sAttributeValue">
    /// Where the value of the specified GraphML-Attribute gets stored if true
    /// is returned.
    /// </param>
    ///
    /// <returns>
    /// true if the specified value was found and was a non-empty string.
    /// </returns>
    //*************************************************************************

    private static Boolean
    TryGetEdgeGraphMLAttributeValue
    (
        XmlNode oEdgeXmlNode,
        String sAttributeID,
        XmlNamespaceManager oXmlNamespaceManager,
        out String sAttributeValue
    )
    {
        Debug.Assert(oEdgeXmlNode != null);
        Debug.Assert( !String.IsNullOrEmpty(sAttributeID) );
        Debug.Assert(oXmlNamespaceManager != null);

        String sXPath = String.Format(

            "g:data[@key=\"{0}\"]/text()"
            ,
            sAttributeID
            );

        return ( XmlUtil2.TrySelectSingleNodeAsString(
            oEdgeXmlNode, sXPath, oXmlNamespaceManager, out sAttributeValue) );
    }
}

}
