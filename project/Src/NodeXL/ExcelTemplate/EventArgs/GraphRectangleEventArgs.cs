
using System;
using System.Drawing;
using System.Diagnostics;
using Smrf.NodeXL.Visualization.Wpf;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: GraphRectangleEventArgs
//
/// <summary>
/// Provides event information for events that involve the graph rectangle and
/// the NodeXLControl.
/// </summary>
//*****************************************************************************

public class GraphRectangleEventArgs : EventArgs
{
    //*************************************************************************
    //  Constructor: GraphRectangleEventArgs()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="GraphRectangleEventArgs" /> class.
    /// </summary>
    ///
    /// <param name="graphRectangle">
    /// The rectangle the graph was drawn within.
    /// </param>
    ///
    /// <param name="nodeXLControl">
    /// The control in which the graph was drawn.
    /// </param>
    //*************************************************************************

    public GraphRectangleEventArgs
    (
        Rectangle graphRectangle,
        NodeXLControl nodeXLControl
    )
    {
        m_oGraphRectangle = graphRectangle;
        m_oNodeXLControl = nodeXLControl;

        // AssertValid();
    }

    //*************************************************************************
    //  Property: GraphRectangle
    //
    /// <summary>
    /// Gets the rectangle the graph was drawn within.
    /// </summary>
    ///
    /// <value>
    /// The rectangle the graph was drawn within.
    /// </value>
    //*************************************************************************

    public Rectangle
    GraphRectangle
    {
        get
        {
            AssertValid();

            return (m_oGraphRectangle);
        }
    }

    //*************************************************************************
    //  Property: NodeXLControl
    //
    /// <summary>
    /// Gets the control in which the graph was drawn.
    /// </summary>
    ///
    /// <value>
    /// The control in which the graph was drawn.
    /// </value>
    //*************************************************************************

    public NodeXLControl
    NodeXLControl
    {
        get
        {
            AssertValid();

            return (m_oNodeXLControl);
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

    public virtual void
    AssertValid()
    {
        // m_oGraphRectangle
        Debug.Assert(m_oNodeXLControl != null);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// The rectangle the graph was drawn within.

    protected Rectangle m_oGraphRectangle;

    /// The control in which the graph was drawn.

    protected NodeXLControl m_oNodeXLControl;
}

}
