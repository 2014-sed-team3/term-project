
using System;
using Smrf.NodeXL.Core;

namespace Smrf.NodeXL.Layouts
{
    //*****************************************************************************
    //  Enum: BoxLayoutAlgorithm
    //
    /// <summary>
    /// Specifies which layout algorithm should be used for the group-in-a-box layout.
    /// It maps from solely 
    /// </summary>
    /// 
    /// <remarks>
    /// See <see cref="LayoutStyle" /> for more information.
    /// </remarks>
    //*****************************************************************************

    public enum
    BoxLayoutAlgorithm
    {
        /// <summary>
        /// Squarified treemap group-in-a-box layout.
        /// </summary>

        Treemap,

        /// <summary>
        /// Packed rectangles group-in-a-box layout.
        /// </summary>

        PackedRectangles,

        /// <summary>
        /// Force-directed group-in-a-box layout.
        /// </summary>
        /// 
        ForceDirected,
    }
}
