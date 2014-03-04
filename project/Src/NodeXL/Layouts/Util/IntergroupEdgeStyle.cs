
using System;
using Smrf.NodeXL.Core;

namespace Smrf.NodeXL.Layouts
{
//*****************************************************************************
//  Enum: IntergroupEdgeStyle
//
/// <summary>
/// Specifies how the edges that connect vertices in different groups should
/// be shown.
/// </summary>
///
/// <remarks>
/// See <see cref="LayoutBase.IntergroupEdgeStyle" /> for more information.
/// </remarks>
//*****************************************************************************

public enum
IntergroupEdgeStyle
{
    /// <summary>
    /// The intergroup edges are shown in the same manner as other edges.
    /// </summary>

    Show,

    /// <summary>
    /// The intergroup edges are hidden.
    /// </summary>

    Hide,

    /// <summary>
    /// All edges between group A and group B are shown as a single edge whose
    /// width is proportional to the number of combined edges.
    /// </summary>

    Combine,
}

}
