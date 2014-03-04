
using System;
using System.Xml;
using System.Collections.Generic;
using System.Diagnostics;

namespace Smrf.NodeXL.GraphMLLib
{
//*****************************************************************************
//  Class: TwitterUser
//
/// <summary>
/// Represents a user in a Twitter network.
/// </summary>
///
/// <remarks>
/// This class wraps the GraphML XML node that represents a user in a Twitter
/// network, together with one or more statuses for the user.
///
/// <para>
/// This is meant for use while creating Twitter GraphML XML documents for use
/// with the NodeXL Excel Template.
/// </para>
///
/// </remarks>
//*****************************************************************************

public class TwitterUser : Object
{
    //*************************************************************************
    //  Constructor: TwitterUser()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="TwitterUser" /> class.
    /// </summary>
    ///
    /// <param name="screenName">
    /// The user's screen name.
    /// </param>
    ///
    /// <param name="vertexXmlNode">
    /// The vertex XmlNode from a GraphMLXmlDocument that represents the user
    /// in a Twitter network.
    /// </param>
    //*************************************************************************

    public TwitterUser
    (
        String screenName,
        XmlNode vertexXmlNode
    )
    {
        m_ScreenName = screenName;
        m_VertexXmlNode = vertexXmlNode;
        m_Statuses = new LinkedList<TwitterStatus>();

        AssertValid();
    }

    //*************************************************************************
    //  Property: VertexXmlNode
    //
    /// <summary>
    /// Gets the vertex XmlNode that represents the user.
    /// </summary>
    ///
    /// <value>
    /// The XmlNode from a GraphMLXmlDocument that represents the user.
    /// </value>
    //*************************************************************************

    public XmlNode
    VertexXmlNode
    {
        get
        {
            AssertValid();

            return (m_VertexXmlNode);
        }
    }

    //*************************************************************************
    //  Property: ScreenName
    //
    /// <summary>
    /// Gets the user's screen name.
    /// </summary>
    ///
    /// <value>
    /// The user's screen name.
    /// </value>
    //*************************************************************************

    public String
    ScreenName
    {
        get
        {
            AssertValid();

            return (m_ScreenName);
        }
    }

    //*************************************************************************
    //  Property: Statuses
    //
    /// <summary>
    /// Gets a collection of statuses associated with the user.
    /// </summary>
    ///
    /// <value>
    /// A collection of zero or more statuses associated with the user.
    /// </value>
    //*************************************************************************

    public ICollection<TwitterStatus>
    Statuses
    {
        get
        {
            AssertValid();

            return (m_Statuses);
        }
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
        Debug.Assert( !String.IsNullOrEmpty(m_ScreenName) );
        Debug.Assert(m_VertexXmlNode != null);
        Debug.Assert(m_Statuses != null);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// The user's screen name.

    protected String m_ScreenName;

    /// The vertex XmlNode from a GraphMLXmlDocument that represents the user
    /// in a Twitter network.

    protected XmlNode m_VertexXmlNode;

    /// Zero or more statuses associated with the user.

    protected LinkedList<TwitterStatus> m_Statuses;
}

}
