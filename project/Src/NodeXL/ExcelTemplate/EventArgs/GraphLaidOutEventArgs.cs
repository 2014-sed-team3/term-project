
using System;
using System.Drawing;
using System.Collections.Generic;
using System.Diagnostics;
using Smrf.NodeXL.Core;
using Smrf.NodeXL.Visualization.Wpf;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: GraphLaidOutEventArgs
//
/// <summary>
/// Provides event information for the <see cref="TaskPane.GraphLaidOut" /> and
/// <see cref="ThisWorkbook.GraphLaidOut" /> events.
/// </summary>
//*****************************************************************************

public class GraphLaidOutEventArgs : GraphRectangleEventArgs
{
    //*************************************************************************
    //  Constructor: GraphLaidOutEventArgs()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="GraphLaidOutEventArgs" />
    /// class.
    /// </summary>
    ///
    /// <param name="graphRectangle">
    /// The rectangle the graph was drawn within.
    /// </param>
    ///
    /// <param name="edgeIDDictionary">
    /// Dictionary that maps edge IDs stored in the edge worksheet to edge
    /// objects in the graph.
    /// </param>
    ///
    /// <param name="vertexIDDictionary">
    /// Dictionary that maps vertex IDs stored in the vertex worksheet to
    /// vertex objects in the graph.
    /// </param>
    ///
    /// <param name="nodeXLControl">
    /// The control in which the graph was laid out.
    /// </param>
    ///
    /// <param name="legendControls">
    /// Zero or more legend controls associated with <paramref
    /// name="nodeXLControl" />.  Can't be null.
    /// </param>
    //*************************************************************************

    public GraphLaidOutEventArgs
    (
        Rectangle graphRectangle,
        Dictionary<Int32, IIdentityProvider> edgeIDDictionary,
        Dictionary<Int32, IIdentityProvider> vertexIDDictionary,
        NodeXLControl nodeXLControl,
        IEnumerable<LegendControlBase> legendControls
    )
    :
    base(graphRectangle, nodeXLControl)
    {
        m_oEdgeIDDictionary = edgeIDDictionary;
        m_oVertexIDDictionary = vertexIDDictionary;
        m_oLegendControls = legendControls;

        AssertValid();
    }

    //*************************************************************************
    //  Property: EdgeIDDictionary
    //
    /// <summary>
    /// Gets a dictionary that maps edge IDs stored in the edge worksheet to
    /// edge objects in the graph.
    /// </summary>
    ///
    /// <value>
    /// A dictionary that maps edge IDs stored in the edge worksheet to edge
    /// objects in the graph.
    /// </value>
    ///
    /// <remarks>
    /// The IDs are those that are stored in the edge worksheet's ID column and
    /// are different from the IEdge.ID values in the graph, which the edge
    /// worksheet knows nothing about.
    /// </remarks>
    //*************************************************************************

    public Dictionary<Int32, IIdentityProvider>
    EdgeIDDictionary
    {
        get
        {
            AssertValid();

            return (m_oEdgeIDDictionary);
        }
    }

    //*************************************************************************
    //  Property: VertexIDDictionary
    //
    /// <summary>
    /// Gets a dictionary that maps vertex IDs stored in the vertex worksheet
    /// to vertex objects in the graph.
    /// </summary>
    ///
    /// <value>
    /// A dictionary that maps vertex IDs stored in the vertex worksheet to
    /// vertex objects in the graph.
    /// </value>
    ///
    /// <remarks>
    /// The IDs are those that are stored in the vertex worksheet's ID column
    /// and are different from the IVertex.ID values in the graph, which the
    /// vertex worksheet knows nothing about.
    /// </remarks>
    //*************************************************************************

    public Dictionary<Int32, IIdentityProvider>
    VertexIDDictionary
    {
        get
        {
            AssertValid();

            return (m_oVertexIDDictionary);
        }
    }

    //*************************************************************************
    //  Property: LegendControls
    //
    /// <summary>
    /// Gets the legend controls associated with the <see
    /// name="NodeXLControl" />.
    /// </summary>
    ///
    /// <value>
    /// Zero or more legend controls.
    /// </value>
    //*************************************************************************

    public IEnumerable<LegendControlBase>
    LegendControls
    {
        get
        {
            AssertValid();

            return (m_oLegendControls);
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

        Debug.Assert(m_oEdgeIDDictionary != null);
        Debug.Assert(m_oVertexIDDictionary != null);
        Debug.Assert(m_oLegendControls != null);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Dictionary that maps edge IDs stored in the edge worksheet to edge
    /// objects in the graph.  The edge IDs stored in the worksheet are
    /// different from IEdge.ID, which the edge worksheet knows nothing about.

    protected Dictionary<Int32, IIdentityProvider> m_oEdgeIDDictionary;

    /// Ditto for vertices.

    protected Dictionary<Int32, IIdentityProvider> m_oVertexIDDictionary;

    /// Zero or more legend controls associated with the NodeXLControl.

    protected IEnumerable<LegendControlBase> m_oLegendControls;
}

}
