
using System;
using System.Drawing;
using System.Collections.Generic;
using Smrf.NodeXL.Core;
using System.Diagnostics;

namespace Smrf.NodeXL.Layouts
{
//*****************************************************************************
//  Class: GroupLayoutDrawingInfo
//
/// <summary>
/// Stores information about how groups and the edges between them should be
/// drawn when the graph is laid out using groups.
/// </summary>
///
/// <remarks>
/// An instance of this class is added to the graph when <see
/// cref="ILayout.LayoutStyle" /> is set to <see
/// cref="LayoutStyle.UseGroups" />.
/// </remarks>
//*****************************************************************************

public class GroupLayoutDrawingInfo : Object
{
    //*************************************************************************
    //  Constructor: GroupLayoutDrawingInfo()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="GroupLayoutDrawingInfo" />
    /// class.
    /// </summary>
    ///
    /// <param name="groupsToDraw">
    /// The groups to draw.
    /// </param>
    ///
    /// <param name="penWidth">
    /// The width of the pen used to draw group rectangles.  If 0, group
    /// rectangles shouldn't be drawn.
    /// </param>
    ///
    /// <param name="combinedIntergroupEdges">
    /// A collection of combined intergroup edges, or null if intergroup edges
    /// haven't been combined.
    /// </param>
    //*************************************************************************

    public GroupLayoutDrawingInfo
    (
        IList<GroupInfo> groupsToDraw,
        Double penWidth,
        IEnumerable<IntergroupEdgeInfo> combinedIntergroupEdges
    )
    {
        m_oGroupsToDraw = groupsToDraw;
        m_dPenWidth = penWidth;
        m_oCombinedIntergroupEdges = combinedIntergroupEdges;

        AssertValid();
    }

    //*************************************************************************
    //  Property: GroupsToDraw
    //
    /// <summary>
    /// Gets the groups to draw.
    /// </summary>
    ///
    /// <value>
    /// A collection of the groups to draw.
    /// </value>
    //*************************************************************************

    public IList<GroupInfo>
    GroupsToDraw
    {
        get
        {
            AssertValid();

            return (m_oGroupsToDraw);
        }
    }

    //*************************************************************************
    //  Property: PenWidth
    //
    /// <summary>
    /// Gets the width of the pen to use when drawing the group rectangles.
    /// </summary>
    ///
    /// <value>
    /// The pen width, in WPF units.  If 0, group rectangles shouldn't be
    /// drawn.
    /// </value>
    //*************************************************************************

    public Double
    PenWidth
    {
        get
        {
            AssertValid();

            return (m_dPenWidth);
        }
    }

    //*************************************************************************
    //  Property: CombinedIntergroupEdges
    //
    /// <summary>
    /// Gets a collection of the combined intergroup edges.
    /// </summary>
    ///
    /// <value>
    /// A collection of combined intergroup edges, or null if intergroup edges
    /// haven't been combined.
    /// </value>
    //*************************************************************************

    public IEnumerable<IntergroupEdgeInfo>
    CombinedIntergroupEdges
    {
        get
        {
            AssertValid();

            return (m_oCombinedIntergroupEdges);
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
        Debug.Assert(m_oGroupsToDraw != null);
        Debug.Assert(m_dPenWidth >= 0);
        // m_oCombinedIntergroupEdges
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// The groups to draw.

    protected IList<GroupInfo> m_oGroupsToDraw;

    /// The width of the pen to use when drawing the group rectangles, in WPF
    /// units, or 0 to not draw rectangles.

    protected Double m_dPenWidth;

    /// A collection of combined intergroup edges, or null.

    protected IEnumerable<IntergroupEdgeInfo> m_oCombinedIntergroupEdges;
}

}
