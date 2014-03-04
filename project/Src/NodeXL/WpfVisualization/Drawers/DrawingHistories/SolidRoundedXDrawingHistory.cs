
using System;
using System.Windows;
using System.Windows.Media;
using System.Diagnostics;
using Smrf.NodeXL.Core;
using Smrf.GraphicsLib;
using Smrf.WpfGraphicsLib;

namespace Smrf.NodeXL.Visualization.Wpf
{
//*****************************************************************************
//  Class: SolidRoundedXVertexDrawingHistory
//
/// <summary>
/// Retains information about how one vertex was drawn as a <see
/// cref="VertexShape.SolidRoundedX" />.
/// </summary>
//*****************************************************************************

public class SolidRoundedXVertexDrawingHistory : SolidTaperedDiamondVertexDrawingHistory
{
    //*************************************************************************
    //  Constructor: SolidRoundedXVertexDrawingHistory()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="SolidRoundedXVertexDrawingHistory" />
    /// class.
    /// </summary>
    ///
    /// <param name="vertex">
    /// The vertex that was drawn.
    /// </param>
    ///
    /// <param name="drawingVisual">
    /// The DrawingVisual object that was used to draw the vertex.
    /// </param>
    ///
    /// <param name="drawnAsSelected">
    /// true if the vertex was drawn as selected.
    /// </param>
    ///
    /// <param name="halfWidth">
    /// One half the width of the rounded X that was drawn for <paramref
    /// name="vertex" />.
    /// </param>
    //*************************************************************************

    public SolidRoundedXVertexDrawingHistory
    (
        IVertex vertex,
        DrawingVisual drawingVisual,
        Boolean drawnAsSelected,
        Double halfWidth
    )
    : base(vertex, drawingVisual, drawnAsSelected, halfWidth)
    {
        // AssertValid();
    }

    //*************************************************************************
    //  Method: GetBounds()
    //
    /// <summary>
    /// Gets the bounds of the object that was drawn.
    /// </summary>
    ///
    /// <returns>
    /// The object's bounds, as a Geometry.
    /// </returns>
    ///
    /// <remarks>
    /// The object's bounds defines the object's boundary.  It is not
    /// necessarily rectangular.
    /// </remarks>
    //*************************************************************************

    public override Geometry
    GetBounds()
    {
        return ( WpfPathGeometryUtil.GetRoundedX(
            this.VertexLocation, m_dHalfWidth) );
    }
}

}
