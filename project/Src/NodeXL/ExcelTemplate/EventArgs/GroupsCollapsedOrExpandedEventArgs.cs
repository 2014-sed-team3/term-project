
using System;
using System.Drawing;
using System.Diagnostics;
using Smrf.NodeXL.Visualization.Wpf;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: GroupsCollapsedOrExpandedEventArgs
//
/// <summary>
/// Provides event information for the <see
/// cref="TaskPane.GroupsCollapsedOrExpanded" /> event.
/// </summary>
//*****************************************************************************

public class GroupsCollapsedOrExpandedEventArgs : GraphRectangleEventArgs
{
    //*************************************************************************
    //  Constructor: GroupsCollapsedOrExpandedEventArgs()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="GroupsCollapsedOrExpandedEventArgs" /> class.
    /// </summary>
    ///
    /// <param name="graphRectangle">
    /// The rectangle the graph was drawn within.
    /// </param>
    ///
    /// <param name="nodeXLControl">
    /// The control in which the graph was drawn.
    /// </param>
    ///
    /// <param name="groupsRedrawnImmediately">
    /// true if the groups were redrawn immediately after they were collapsed
    /// or expanded, false if they won't be redrawn until the graph is redrawn.
    /// </param>
    //*************************************************************************

    public GroupsCollapsedOrExpandedEventArgs
    (
        Rectangle graphRectangle,
        NodeXLControl nodeXLControl,
        Boolean groupsRedrawnImmediately
    )
    :
    base(graphRectangle, nodeXLControl)
    {
        m_bGroupsRedrawnImmediately = groupsRedrawnImmediately;

        AssertValid();
    }

    //*************************************************************************
    //  Property: GroupsRedrawnImmediately
    //
    /// <summary>
    /// Gets a flag indicating whether the groups were redrawn immediately
    /// after they were collapsed or expanded.
    /// </summary>
    ///
    /// <value>
    /// true if the groups were redrawn immediately after they were collapsed
    /// or expanded, false if they won't be redrawn until the graph is redrawn.
    /// </value>
    //*************************************************************************

    public Boolean
    GroupsRedrawnImmediately
    {
        get
        {
            AssertValid();

            return (m_bGroupsRedrawnImmediately);
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

        // m_bGroupsRedrawnImmediately
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// true if the groups were redrawn immediately after they were collapsed
    /// or expanded.

    protected Boolean m_bGroupsRedrawnImmediately;
}

}
