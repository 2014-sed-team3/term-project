
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Diagnostics;
using Smrf.NodeXL.Visualization.Wpf;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: VerticesMovedEventArgs2
//
/// <summary>
/// Provides event information for the <see cref="TaskPane.VerticesMoved" />
/// and <see cref="ThisWorkbook.VerticesMoved" /> events.
/// </summary>
//*****************************************************************************

public class VerticesMovedEventArgs2 : GraphRectangleEventArgs
{
    //*************************************************************************
    //  Constructor: VerticesMovedEventArgs2()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="VerticesMovedEventArgs2" /> class.
    /// </summary>
    ///
    /// <param name="verticesAndRowIDs">
    /// Collection of <see cref="VertexAndRowID" /> objects, one for each
    /// vertex that was moved.
    /// </param>
    ///
    /// <param name="graphRectangle">
    /// The rectangle the graph was drawn within.
    /// </param>
    ///
    /// <param name="nodeXLControl">
    /// The control in which the graph was drawn.
    /// </param>
    //*************************************************************************

    public VerticesMovedEventArgs2
    (
        ICollection<VertexAndRowID> verticesAndRowIDs,
        Rectangle graphRectangle,
        NodeXLControl nodeXLControl
    )
    :
    base(graphRectangle, nodeXLControl)
    {
        m_oVerticesAndRowIDs = verticesAndRowIDs;

        AssertValid();
    }

    //*************************************************************************
    //  Property: VerticesAndRowIDs
    //
    /// <summary>
    /// Gets the vertices that were moved.
    /// </summary>
    ///
    /// <value>
    /// Collection of <see cref="VertexAndRowID" /> objects, one for each
    /// vertex that was moved.
    /// </value>
    //*************************************************************************

    public ICollection<VertexAndRowID>
    VerticesAndRowIDs
    {
        get
        {
            AssertValid();

            return (m_oVerticesAndRowIDs);
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

        Debug.Assert(m_oVerticesAndRowIDs != null);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Collection of VertexAndRowID objects, one for each vertex that was
    /// moved.

    protected ICollection<VertexAndRowID> m_oVerticesAndRowIDs;
}

}
