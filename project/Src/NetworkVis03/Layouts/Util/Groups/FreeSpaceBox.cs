
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using Smrf.NodeXL.Core;
using Smrf.NodeXL.Algorithms;

namespace Smrf.NodeXL.Layouts
{
//*****************************************************************************
//  Class: FreespaceBox
//
/// <summary>
/// Manages the various free space boxes that are available
/// </summary>
///
/// <remarks>
/// All methods are static.
/// </remarks>
//*****************************************************************************

public class FreeSpaceBox
{
    //*************************************************************************
    //  Variable: boxRectangle
    //
    /// <summary>
    /// The underlying RectangleF of this FreeSpaceBox
    /// </summary>
    //*************************************************************************

    public RectangleF boxRectangle;

    //*****************************************************************************
    //  Enum: Orientation
    //
    /// <summary>
    /// Specifies horizontal or vertical orientation for a group box
    /// </summary>
    //*****************************************************************************

    public enum 
    Orientation { 

        /// <summary>
        /// Horizonal orientation
        /// </summary>
        Horiz, 

        /// <summary>
        /// Vertical orientation
        /// </summary>
        Vert
    };

    /// <summary>
    /// Defines the orientation of this box
    /// </summary>
    public Orientation orientation;

    //*************************************************************************
    //  Class constructor
    //
    /// <summary>
    /// This constructor sets the box boundaries and orientation
    /// </summary>
    //*************************************************************************

    public FreeSpaceBox(float left, float top, float right, float bottom, Orientation orientation)
    {
        boxRectangle = RectangleF.FromLTRB(left, top, right, bottom);
        this.orientation = orientation;
    }

    
    //*************************************************************************
    //  Method: GetArea
    //
    /// <summary>
    /// Gets the area of a FreeSpaceBox
    /// </summary>
    /// 
    /// <returns>
    /// The area of this box's RectangleF
    /// </returns>
    //*************************************************************************

    public double GetArea()
    {   
        return (boxRectangle.Width * boxRectangle.Height);        
    }
}

}
