
using System;
using System.Drawing;
using System.Collections.Generic;
using System.Diagnostics;
using Smrf.NodeXL.Core;

namespace Smrf.NodeXL.Layouts
{
//*****************************************************************************
//  Class: LayoutSaver
//
/// <summary>
/// Saves and restores a layout.
/// </summary>
///
/// <remarks>
/// Pass a laid-out graph to the constructor, which saves the graph's vertex
/// locations and group rectangles in private data.   Call <see
/// cref="RestoreLayout" /> to restore the vertex locations and group
/// rectangles to their saved values.
///
/// <para>
/// The number of vertices and group rectangles must remain constant between
/// the two calls.
/// </para>
///
/// </remarks>
//*****************************************************************************

public class LayoutSaver : LayoutsBase
{
    //*************************************************************************
    //  Constructor: LayoutSaver()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="LayoutSaver" /> class.
    /// </summary>
    //*************************************************************************

    public LayoutSaver
    (
        IGraph graph
    )
    {
        Debug.Assert(graph != null);

        m_oGraph = graph;

        IVertexCollection oVertices = m_oGraph.Vertices;
        Int32 iVertices = oVertices.Count;
        m_aoVertexLocations = new PointF[iVertices];

        Int32 iVertex = 0;

        foreach (IVertex oVertex in oVertices)
        {
            m_aoVertexLocations[iVertex] = oVertex.Location;
            iVertex++;
        }

        IList<GroupInfo> oGroupsToDraw;

        if ( TryGetGroupsToDraw(out oGroupsToDraw) )
        {
            Int32 iGroupsToDraw = oGroupsToDraw.Count;
            m_aoGroupRectangles = new Rectangle[iGroupsToDraw];

            for (Int32 i = 0; i < iGroupsToDraw; i++)
            {
                m_aoGroupRectangles[i] = oGroupsToDraw[i].Rectangle;
            }
        }

        AssertValid();
    }

    //*************************************************************************
    //  Method: RestoreLayout()
    //
    /// <summary>
    /// Restores the graph's vertices to their saved locations.
    /// </summary>
    //*************************************************************************

    public void
    RestoreLayout()
    {
        AssertValid();

        IVertexCollection oVertices = m_oGraph.Vertices;
        Int32 iVertices = oVertices.Count;

        Debug.Assert(m_aoVertexLocations != null);
        Debug.Assert(m_aoVertexLocations.Length == iVertices);

        Int32 iVertex = 0;

        foreach (IVertex oVertex in oVertices)
        {
            oVertex.Location = m_aoVertexLocations[iVertex];
            iVertex++;
        }

        IList<GroupInfo> oGroupsToDraw;

        if ( TryGetGroupsToDraw(out oGroupsToDraw) )
        {
            Int32 iGroupsToDraw = oGroupsToDraw.Count;

            Debug.Assert(m_aoGroupRectangles != null);
            Debug.Assert(m_aoGroupRectangles.Length == iGroupsToDraw);

            for (Int32 i = 0; i < iGroupsToDraw; i++)
            {
                oGroupsToDraw[i].Rectangle = m_aoGroupRectangles[i];
            }
        }
    }

    //*************************************************************************
    //  Method: TryGetGroupsToDraw()
    //
    /// <summary>
    /// Attempts to information about the groups that are drawn.
    /// </summary>
    ///
    /// <param name="oGroupsToDraw">
    /// Where the information get stored if true is returned.
    /// </param>
    ///
    /// <returns>
    /// true if the information was obtained.
    /// </returns>
    //*************************************************************************

    protected Boolean
    TryGetGroupsToDraw
    (
        out IList<GroupInfo> oGroupsToDraw
    )
    {
        AssertValid();

        oGroupsToDraw = null;

        GroupLayoutDrawingInfo oGroupLayoutDrawingInfo;

        if ( GroupMetadataManager.TryGetGroupLayoutDrawingInfo(m_oGraph,
            out oGroupLayoutDrawingInfo) )
        {
            oGroupsToDraw = oGroupLayoutDrawingInfo.GroupsToDraw;
            return (true);
        }

        return (false);
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

        Debug.Assert(m_oGraph != null);
        Debug.Assert(m_aoVertexLocations != null);
        // m_aoGroupRectangles
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Graph whose vertex locations have been saved in m_aoVertexLocations.

    protected IGraph m_oGraph;

    /// Array of the graph's vertex locations.

    protected PointF [] m_aoVertexLocations;

    /// Group rectangles, or null if the graph isn't laid out using groups.

    protected Rectangle [] m_aoGroupRectangles;
}

}
