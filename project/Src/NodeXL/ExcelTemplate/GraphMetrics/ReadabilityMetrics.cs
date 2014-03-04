
using System;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Enum: ReadabilityMetrics
//
/// <summary>
/// Specifies one or more readability metrics.
/// </summary>
///
/// <remarks>
/// The flags can be ORed together.
/// </remarks>
//*****************************************************************************

[System.FlagsAttribute]

public enum
ReadabilityMetrics
{
    /// <summary>
    /// No readability metrics.
    /// </summary>

    None = 0,

    /// <summary>
    /// The edge crossings for an edge.
    /// </summary>

    PerEdgeEdgeCrossings = 1,

    /// <summary>
    /// The edge crossings for a vertex's incident edges.
    /// </summary>

    PerVertexEdgeCrossings = 2,

    /// <summary>
    /// The vertex overlap for a vertex.
    /// </summary>

    PerVertexVertexOverlap = 4,

    /// <summary>
    /// The edge crossings for the graph as a whole.
    /// </summary>

    OverallEdgeCrossings = 8,

    /// <summary>
    /// The vertex overlap for the graph as a whole.
    /// </summary>

    OverallVertexOverlap = 16,

    /// <summary>
    /// The ratio of the area used by the graph's vertices and edges to the
    /// area of the graph rectangle.
    /// </summary>

    GraphRectangleCoverage = 32,
}

}
