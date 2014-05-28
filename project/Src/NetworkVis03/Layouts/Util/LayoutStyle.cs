
using System;
using Smrf.NodeXL.Core;

namespace Smrf.NodeXL.Layouts
{
//*****************************************************************************
//  Enum: LayoutStyle
//
/// <summary>
/// Specifies the style to use when laying out the graph.
/// </summary>
///
/// <remarks>
/// A layout style specifies an optional special treatment that can be applied
/// to the graph's groups or strongly connected components when laying out the
/// graph.
/// </remarks>
//*****************************************************************************

public enum
LayoutStyle
{
    /// <summary>
    /// If the entire graph is being laid out, the entire graph rectangle is
    /// used.
    /// </summary>

    Normal,

    /// <summary>
    /// If the <see cref="ReservedMetadataKeys.GroupInfo" /> key is present on
    /// the graph and the entire graph is being laid out, each of the graph's
    /// groups is laid out within a box using the algorithm implemented by the
    /// <see cref="ILayout" /> implementation.  The box size is proportional
    /// to the number of vertices in the group and the boxes are arranged using
    /// the selected <see cref="BoxLayoutAlgorithm" />.  Otherwise, 
    /// <see cref="Normal" /> is used.
    ///
    /// <para>
    /// This layout style is sometimes called "group in a box."
    /// </para>
    ///
    /// </summary>

    UseGroups,

    /// <summary>
    /// If <see cref="ILayout.SupportsBinning" /> is true and the entire graph
    /// is being laid out, the graph is split into strongly connected
    /// components, the smaller components are laid out and placed along the
    /// bottom of the rectangle using the <see
    /// cref="FruchtermanReingoldLayout" />, and the remaining components are
    /// laid out within the remaining rectangle using the algorithm implemented
    /// by the <see cref="ILayout" /> implementation.
    /// </summary>

    UseBinning,
}

}
