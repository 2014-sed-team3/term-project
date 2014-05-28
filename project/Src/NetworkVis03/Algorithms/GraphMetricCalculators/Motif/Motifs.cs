
using System;

namespace Smrf.NodeXL.Algorithms
{
//*****************************************************************************
//  Enum: Motifs
//
/// <summary>
/// Specifies one or more motif types.
/// </summary>
///
/// <remarks>
/// The flags can be ORed together.
/// </remarks>
//*****************************************************************************

[System.FlagsAttribute]

public enum
Motifs
{
    /// <summary>
    /// No motifs.
    /// </summary>

    None = 0,

    /// <summary>
    /// A fan motif consists of a head vertex and the edges connecting it to
    /// N>=2 leaf vertices that each have exactly one adjacent vertex. The head
    /// vertex, edges, and the leaf vertices are all part of the fan motif.  We
    /// exclude the degenerate case of the "barbell" component with only two
    /// vertices and one edge.
    /// </summary>

    Fan = 1,

    /// <summary>
    /// A D-connector motif consists of D anchor vertices, each connected to a
    /// set of N>=2 span vertices by span edges. The span vertices have exactly
    /// D adjacent vertices, which are the anchor vertices. The anchor
    /// vertices have at least N adjacent vertices, N of which are the span
    /// vertices.
    /// </summary>

    DConnector = 2,

    /// <summary>
    /// A clique motif consists of a maximum set of vertices who have all
    /// possible ties present among themselves.
    /// </summary>
    Clique = 4,
}

}
