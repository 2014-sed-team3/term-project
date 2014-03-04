
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Smrf.NodeXL.Core;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: GroupEdgeInfo
//
/// <summary>
/// Stores information about the edges in a group.
/// </summary>
///
/// <remarks>
/// An edge is considered to be in a group if its first vertex is in the
/// group.
/// </remarks>
//*****************************************************************************

public class GroupEdgeInfo : Object
{
    //*************************************************************************
    //  Constructor: GroupEdgeInfo()
    //
    /// <overloads>
    /// Initializes a new instance of the <see cref="GroupEdgeInfo" /> class.
    /// </overloads>
    ///
    /// <summary>
    /// Initializes a new instance of the <see cref="GroupEdgeInfo" /> class
    /// for a real group.
    /// </summary>
    ///
    /// <param name="edges">
    /// A collection of the group's zero or more edges.
    /// </param>
    ///
    /// <param name="groupInfo">
    /// Contains information about the real group.
    /// </param>
    //*************************************************************************

    public GroupEdgeInfo
    (
        IEnumerable<IEdge> edges,
        ExcelTemplateGroupInfo groupInfo
    )
    : this(edges, groupInfo, null)
    {
        // (Do nothing else.)

        AssertValid();
    }

    //*************************************************************************
    //  Constructor: GroupEdgeInfo()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="GroupEdgeInfo" /> class
    /// for a "dummy" group.
    /// </summary>
    ///
    /// <param name="edges">
    /// A collection of the group's zero or more edges.
    /// </param>
    ///
    /// <param name="dummyGroupName">
    /// Name of the dummy group.  See <see cref="GroupEdgeSorter" /> for
    /// information on what a dummy group is.
    /// </param>
    //*************************************************************************

    public GroupEdgeInfo
    (
        IEnumerable<IEdge> edges,
        String dummyGroupName
    )
    : this(edges, null, dummyGroupName)
    {
        // (Do nothing else.)

        AssertValid();
    }

    //*************************************************************************
    //  Constructor: GroupEdgeInfo()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="GroupEdgeInfo" /> class.
    /// </summary>
    ///
    /// <param name="oEdges">
    /// A collection of the group's zero or more edges.
    /// </param>
    ///
    /// <param name="oGroupInfo">
    /// Contains information about the group, or null if this is a "dummy"
    /// group.
    /// </param>
    ///
    /// <param name="sDummyGroupName">
    /// Name of the dummy group, or null if this is not a dummy group.
    /// </param>
    //*************************************************************************

    protected GroupEdgeInfo
    (
        IEnumerable<IEdge> oEdges,
        ExcelTemplateGroupInfo oGroupInfo,
        String sDummyGroupName
    )
    {
        m_oEdges = oEdges;
        m_oGroupInfo = oGroupInfo;
        m_sDummyGroupName = sDummyGroupName;

        AssertValid();
    }

    //*************************************************************************
    //  Property: GroupName
    //
    /// <summary>
    /// Gets the name of the group.
    /// </summary>
    ///
    /// <value>
    /// The name of the group, whether it's a real group or a dummy group.
    /// </value>
    //*************************************************************************

    public String
    GroupName
    {
        get
        {
            AssertValid();

            return (m_oGroupInfo != null ?
                m_oGroupInfo.Name : m_sDummyGroupName);
        }
    }

    //*************************************************************************
    //  Property: Edges
    //
    /// <summary>
    /// Gets the edges in the group.
    /// </summary>
    ///
    /// <value>
    /// A collection of the group's zero or more edges.
    /// </value>
    //*************************************************************************

    public IEnumerable<IEdge>
    Edges
    {
        get
        {
            AssertValid();

            return (m_oEdges);
        }
    }

    //*************************************************************************
    //  Property: GroupInfo
    //
    /// <summary>
    /// Gets information about the group.
    /// </summary>
    ///
    /// <value>
    /// A GroupInfo object that contains information about the group, or null
    /// if this is a "dummy" group.
    /// </value>
    //*************************************************************************

    public ExcelTemplateGroupInfo
    GroupInfo
    {
        get
        {
            AssertValid();

            return (m_oGroupInfo);
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
        Debug.Assert(m_oEdges != null);

        Debug.Assert( m_oGroupInfo != null ||
            !String.IsNullOrEmpty(m_sDummyGroupName) );
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// The edges whose first vertex is in the group.

    protected IEnumerable<IEdge> m_oEdges;

    /// Information about the group, or null if this is a dummy group.

    protected ExcelTemplateGroupInfo m_oGroupInfo;

    /// Name of the dummy group, or null if this is not a dummy group.

    protected String m_sDummyGroupName;
}

}
