﻿
using System;
using System.Windows;
using System.Windows.Media;
using System.Diagnostics;
using Smrf.NodeXL.Core;

namespace Smrf.NodeXL.Visualization.Wpf
{
//*****************************************************************************
//  Class: LabelVertexDrawingHistory
//
/// <summary>
/// Retains information about how one vertex was drawn as a label shape.
/// </summary>
//*****************************************************************************

public class LabelVertexDrawingHistory : RectangleVertexDrawingHistory
{
    //*************************************************************************
    //  Constructor: LabelVertexDrawingHistory()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="LabelVertexDrawingHistory" /> class.
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
    /// <param name="rectangle">
    /// The rectangle that was drawn for <paramref name="vertex" />.
    /// </param>
    //*************************************************************************

    public LabelVertexDrawingHistory
    (
        IVertex vertex,
        DrawingVisual drawingVisual,
        Boolean drawnAsSelected,
        Rect rectangle
    )
    : base(vertex, drawingVisual, drawnAsSelected, rectangle)
    {
        // (Do nothing.)

        // AssertValid();
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

        // (Do nothing else.)
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}

}
