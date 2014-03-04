﻿
using System;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: LegendControlBase
//
/// <summary>
/// Base class for several classes that display a graph legend.
/// </summary>
//*****************************************************************************

public abstract class LegendControlBase : Control
{
    //*************************************************************************
    //  Constructor: LegendControlBase()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="LegendControlBase" />
    /// class.
    /// </summary>
    //*************************************************************************

    public LegendControlBase()
    {
        // If the font isn't explicitly set, the control inherits the font of
        // its parent control.  Unfortunately, the parent's font changes within
        // the lifetime of the control.  For example, on a Vista development
        // machine, the font was Microsoft Sans Serif 8.25 within the
        // constructor, but Tahoma 8.25 during all the paint calls.
        //
        // The changing font causes a problem within the derived controls,
        // which size themselves before they get painted.  The font is part of
        // the size calculation, but because the initial font differs from the
        // font actually used for painting, the computed size ends up being
        // wrong.
        //
        // Explicitly setting the font prevents it from changing, and therefore
        // the calculated height will be correct.
        //
        // TODO: What font should be used?  Neither Control.Default nor any of
        // the SystemFonts properties returns Tahoma 8.25 on the development
        // machine, yet something is setting the font of the parent control to
        // Tahoma 8.25.  Hard-coding a font isn't a good solution.

        this.Font = new Font("Tahoma", 8.25F);

        // AssertValid();
    }

    //*************************************************************************
    //  Method: DrawOnBitmap()
    //
    /// <summary>
    /// Draws the legend on a new Bitmap object.
    /// </summary>
    ///
    /// <param name="bitmapWidth">
    /// Width of the bitmap.
    /// </param>
    ///
    /// <returns>
    /// A new Bitmap.  The width is <paramref name="bitmapWidth" /> and the
    /// height is whatever height is necessary to draw the entire legend.
    /// </returns>
    //*************************************************************************

    public Bitmap
    DrawOnBitmap
    (
        Int32 bitmapWidth
    )
    {
        Debug.Assert(bitmapWidth > 0);
        AssertValid();

        // Note that the control height is independent of the control width.

        Int32 iBitmapHeight = CalculateHeight();

        Bitmap oBitmap = new Bitmap(bitmapWidth, iBitmapHeight);

        using ( Graphics oGraphics = Graphics.FromImage(oBitmap) )
        {
            oGraphics.Clear(this.BackColor);

            Draw( CreateDrawingObjects( oGraphics,
                new Rectangle(0, 0, bitmapWidth, iBitmapHeight) ) );
        }

        return (oBitmap);
    }

    //*************************************************************************
    //  Method: CalculateHeight()
    //
    /// <summary>
    /// Calculates what the control height should be.
    /// </summary>
    ///
    /// <returns>
    /// The calculated control height.
    /// </returns>
    ///
    /// <remarks>
    /// The height is calculated to contain the entire legend.
    /// </remarks>
    //*************************************************************************

    protected Int32
    CalculateHeight()
    {
        AssertValid();

        // Draw the control using a completely clipped region.

        return ( Draw( CreateClippedDrawingObjects() ) );
    }

    //*************************************************************************
    //  Method: CreateDrawingObjects()
    //
    /// <summary>
    /// Create the objects used to draw the control.
    /// </summary>
    ///
    /// <param name="oGraphics">
    /// Object to draw with.
    /// </param>
    ///
    /// <param name="oControlRectangle">
    /// Rectangle to draw the control within.
    /// </param>
    ///
    /// <returns>
    /// A DrawingObjects objects.
    /// </returns>
    //*************************************************************************

    protected DrawingObjects
    CreateDrawingObjects
    (
        Graphics oGraphics,
        Rectangle oControlRectangle
    )
    {
        Debug.Assert(oGraphics != null);
        AssertValid();

        DrawingObjects oDrawingObjects = new DrawingObjects();

        oDrawingObjects.Graphics = oGraphics;
        oDrawingObjects.ControlRectangle = oControlRectangle;
        oDrawingObjects.Font = this.Font;

        oDrawingObjects.FontHeight =
            oDrawingObjects.Font.GetHeight(oDrawingObjects.Graphics);

        oDrawingObjects.TrimmingStringFormat = new StringFormat();

        oDrawingObjects.TrimmingStringFormat.Trimming =
            StringTrimming.EllipsisCharacter;

        oDrawingObjects.RightAlignStringFormat = new StringFormat();
        oDrawingObjects.RightAlignStringFormat.Alignment = StringAlignment.Far;

        oDrawingObjects.CenterAlignStringFormat = new StringFormat();

        oDrawingObjects.CenterAlignStringFormat.Alignment =
            StringAlignment.Center;

        return (oDrawingObjects);
    }

    //*************************************************************************
    //  Method: CreateClippedDrawingObjects()
    //
    /// <summary>
    /// Create the objects used to draw the control using a clipped Graphics
    /// object.
    /// </summary>
    ///
    /// <returns>
    /// A DrawingObjects object with a clipped Graphics object.
    /// </returns>
    ///
    /// <remarks>
    /// The Graphics object within the returned DrawingObjects has its clip
    /// region set to empty.  This can be used to calculate the control's
    /// height without actually drawing it.
    /// </remarks>
    //*************************************************************************

    protected DrawingObjects
    CreateClippedDrawingObjects()
    {
        AssertValid();

        Graphics oGraphics = this.CreateGraphics();
        Region oRegion = new Region();
        oRegion.MakeEmpty();
        oGraphics.Clip = oRegion;

        // Use an arbitrary height, which none of the drawing methods pay
        // attention to.  If a non-zero width is available, use it.  (The width
        // is zero when the control is being resized as it is first shown.)

        Int32 iWidth = this.ClientRectangle.Width;

        Rectangle oControlRectangle = new Rectangle(0, 0,
            iWidth == 0 ? 100 : iWidth, 100);

        return ( CreateDrawingObjects(oGraphics, oControlRectangle) );
    }

    //*************************************************************************
    //  Method: Draw()
    //
    /// <summary>
    /// Draws the control.
    /// </summary>
    ///
    /// <param name="oDrawingObjects">
    /// Objects to draw with.
    /// </param>
    ///
    /// <returns>
    /// The bottom of the area that was drawn.
    /// </returns>
    //*************************************************************************

    protected abstract Int32
    Draw
    (
        DrawingObjects oDrawingObjects
    );

    //*************************************************************************
    //  Method: ControlRectangleToTwoColumns()
    //
    /// <summary>
    /// Splits the control rectangle into 2 columns.
    /// </summary>
    ///
    /// <param name="oDrawingObjects">
    /// Objects to draw with.
    /// </param>
    ///
    /// <param name="oColumn1Rectangle">
    /// Where the first column's rectangle gets stored.
    /// </param>
    ///
    /// <param name="oColumn2Rectangle">
    /// Where the second column's rectangle gets stored.
    /// </param>
    //*************************************************************************

    protected void
    ControlRectangleToTwoColumns
    (
        DrawingObjects oDrawingObjects,
        out Rectangle oColumn1Rectangle,
        out Rectangle oColumn2Rectangle
    )
    {
        Debug.Assert(oDrawingObjects != null);
        AssertValid();

        // Compute the rectangles so that the margin to the left of the first
        // column's rectangle, the margin to the right of the second column's
        // rectangle, and the margin between the rectangles is the constant
        // value of ColumnHorizontalMargin.

        Rectangle oControlRectangle = oDrawingObjects.ControlRectangle;
        Int32 iTwoColumnWidth = GetTwoColumnWidth(oDrawingObjects);

        oColumn1Rectangle = new Rectangle(
            oControlRectangle.Left + ColumnHorizontalMargin,
            oControlRectangle.Top,
            iTwoColumnWidth - ( (3 * ColumnHorizontalMargin) / 2 ),
            oControlRectangle.Height
            );

        oColumn2Rectangle = oColumn1Rectangle;

        oColumn2Rectangle.Offset(
            oColumn1Rectangle.Width + ColumnHorizontalMargin, 0);
    }

    //*************************************************************************
    //  Method: AddMarginsToColumnRectangle()
    //
    /// <summary>
    /// Adds margins to the left and right edge of a column rectangle.
    /// </summary>
    ///
    /// <param name="oColumnRectangle">
    /// The column rectangle to add margins to.
    /// </param>
    //*************************************************************************

    protected void
    AddMarginsToColumnRectangle
    (
        ref Rectangle oColumnRectangle
    )
    {
        AssertValid();

        oColumnRectangle.Inflate(-ColumnHorizontalMargin, 0);
    }

    //*************************************************************************
    //  Method: GetTwoColumnWidth()
    //
    /// <summary>
    /// Gets the width to use for each column of a two-column legend.
    /// </summary>
    ///
    /// <param name="oDrawingObjects">
    /// Objects to draw with.
    /// </param>
    ///
    /// <returns>
    /// The width of each of two columns.
    /// </returns>
    ///
    /// <remarks>
    /// The returned width is either an arbitrary width based on the font size
    /// or half the control width, whichever is greater.
    /// </remarks>
    //*************************************************************************

    protected Int32
    GetTwoColumnWidth
    (
        DrawingObjects oDrawingObjects
    )
    {
        Debug.Assert(oDrawingObjects != null);
        AssertValid();

        // Note for the DynamicFiltersLegendControl derived class:
        //
        // The widest filter will be one that filters on a date and time, so
        // the reliable way to compute a minimum width would be to base it on
        // the width of a date and time string.  This yields too great a width
        // for most other filters, however, so just use an arbitrary multiple
        // of the font size instead.

        return ( (Int32)Math.Max(
            oDrawingObjects.ControlRectangle.Width / 2F,
            8.5F * oDrawingObjects.FontHeight
            ) );
    }

    //*************************************************************************
    //  Method: DrawColumnHeader()
    //
    /// <summary>
    /// Draws a header at the top of a legend column.
    /// </summary>
    ///
    /// <param name="oDrawingObjects">
    /// Objects to draw with.
    /// </param>
    ///
    /// <param name="sHeaderText">
    /// Text to draw.
    /// </param>
    ///
    /// <param name="iLeft">
    /// Left x-coordinate.
    /// </param>
    ///
    /// <param name="iRight">
    /// Right x-coordinate.
    /// </param>
    ///
    /// <param name="iTop">
    /// Top y-coordinate.  Gets updated.
    /// </param>
    ///
    /// <remarks>
    /// This method assumes that the legend will be drawn as one or more
    /// columns, with a heading at the top of each column.
    /// </remarks>
    //*************************************************************************

    protected void
    DrawColumnHeader
    (
        DrawingObjects oDrawingObjects,
        String sHeaderText,
        Int32 iLeft,
        Int32 iRight,
        ref Int32 iTop
    )
    {
        Debug.Assert(oDrawingObjects != null);
        Debug.Assert( !String.IsNullOrEmpty(sHeaderText) );
        AssertValid();

        iTop += oDrawingObjects.GetFontHeightMultiple(0.2F);

        Int32 iColumnHeaderHeight =
            oDrawingObjects.GetFontHeightMultiple(1.3F);

        oDrawingObjects.Graphics.FillRectangle(SystemBrushes.ControlLight,
            iLeft, iTop, iRight - iLeft - 1, iColumnHeaderHeight);

        oDrawingObjects.Graphics.DrawString(sHeaderText, oDrawingObjects.Font,
            SystemBrushes.ControlText,
            iLeft + oDrawingObjects.GetFontHeightMultiple(0.1F),
            iTop + oDrawingObjects.GetFontHeightMultiple(0.15F)
            );

        iTop += oDrawingObjects.GetFontHeightMultiple(1.5F);
    }

    //*************************************************************************
    //  Method: DrawExcelColumnName()
    //
    /// <summary>
    /// Draws the name of an Excel column.
    /// </summary>
    ///
    /// <param name="oDrawingObjects">
    /// Objects to draw with.
    /// </param>
    ///
    /// <param name="sColumnName">
    /// Name of the Excel column.
    /// </param>
    ///
    /// <param name="iLeft">
    /// Left x-coordinate where the column name should be drawn.
    /// </param>
    ///
    /// <param name="iRight">
    /// Right x-coordinate where the column name should be drawn.
    /// </param>
    ///
    /// <param name="iTop">
    /// Top y-coordinate where the column name should be drawn.  Gets updated.
    /// </param>
    //*************************************************************************

    protected void
    DrawExcelColumnName
    (
        DrawingObjects oDrawingObjects,
        String sColumnName,
        Int32 iLeft,
        Int32 iRight,
        ref Int32 iTop
    )
    {
        Debug.Assert(oDrawingObjects != null);
        Debug.Assert( !String.IsNullOrEmpty(sColumnName) );

        iTop += oDrawingObjects.GetFontHeightMultiple(0.2F);

        oDrawingObjects.Graphics.DrawString(sColumnName, oDrawingObjects.Font,
            SystemBrushes.ControlText,

            Rectangle.FromLTRB( iLeft, iTop, iRight,
                (Int32)(iTop + oDrawingObjects.FontHeight) ),

            oDrawingObjects.TrimmingStringFormat);

        iTop += oDrawingObjects.GetFontHeightMultiple(1.05F);
    }

    //*************************************************************************
    //  Method: DrawRangeText()
    //
    /// <overloads>
    /// Draws text describing a range.
    /// </overloads>
    ///
    /// <summary>
    /// Draws text describing a range.
    /// </summary>
    ///
    /// <param name="oDrawingObjects">
    /// Objects to draw with.
    /// </param>
    ///
    /// <param name="sLeftText">
    /// The text to draw at the left edge of the control.
    /// </param>
    ///
    /// <param name="sRightText">
    /// The text to draw at the right edge of the control.
    /// </param>
    ///
    /// <param name="oBrush">
    /// The brush to use.
    /// </param>
    ///
    /// <param name="iLeft">
    /// Left x-coordinate.
    /// </param>
    ///
    /// <param name="iRight">
    /// Right x-coordinate.
    /// </param>
    ///
    /// <param name="iTop">
    /// Top y-coordinate.  Gets updated.
    /// </param>
    //*************************************************************************

    protected void
    DrawRangeText
    (
        DrawingObjects oDrawingObjects,
        String sLeftText,
        String sRightText,
        Brush oBrush,
        Int32 iLeft,
        Int32 iRight,
        ref Int32 iTop
    )
    {
        Debug.Assert(oDrawingObjects != null);
        Debug.Assert( !String.IsNullOrEmpty(sLeftText) );
        Debug.Assert( !String.IsNullOrEmpty(sRightText) );
        Debug.Assert(oBrush != null);
        AssertValid();

        DrawRangeText(oDrawingObjects, sLeftText, sRightText, oBrush,
            ref iLeft, ref iRight, ref iTop);
    }

    //*************************************************************************
    //  Method: DrawRangeText()
    //
    /// <summary>
    /// Draws text describing a range and updates the left and right
    /// x-coordinates.
    /// </summary>
    ///
    /// <param name="oDrawingObjects">
    /// Objects to draw with.
    /// </param>
    ///
    /// <param name="sLeftText">
    /// The text to draw at the left edge of the control.
    /// </param>
    ///
    /// <param name="sRightText">
    /// The text to draw at the right edge of the control.
    /// </param>
    ///
    /// <param name="oBrush">
    /// The brush to use.
    /// </param>
    ///
    /// <param name="iLeft">
    /// Left x-coordinate.  Gets updated.
    /// </param>
    ///
    /// <param name="iRight">
    /// Right x-coordinate.  Gets updated.
    /// </param>
    ///
    /// <param name="iTop">
    /// Top y-coordinate.  Gets updated.
    /// </param>
    //*************************************************************************

    protected void
    DrawRangeText
    (
        DrawingObjects oDrawingObjects,
        String sLeftText,
        String sRightText,
        Brush oBrush,
        ref Int32 iLeft,
        ref Int32 iRight,
        ref Int32 iTop
    )
    {
        Debug.Assert(oDrawingObjects != null);
        Debug.Assert( !String.IsNullOrEmpty(sLeftText) );
        Debug.Assert( !String.IsNullOrEmpty(sRightText) );
        Debug.Assert(oBrush != null);
        AssertValid();

        oDrawingObjects.Graphics.DrawString(sLeftText, oDrawingObjects.Font,
            oBrush, iLeft, iTop);

        oDrawingObjects.Graphics.DrawString(sRightText, oDrawingObjects.Font,
            oBrush, iRight, iTop, oDrawingObjects.RightAlignStringFormat);

        iLeft += MeasureTextWidth(oDrawingObjects, sLeftText);
        iRight -= MeasureTextWidth(oDrawingObjects, sRightText);
        iTop += oDrawingObjects.GetFontHeightMultiple(1.0F);
    }

    //*************************************************************************
    //  Method: DrawHorizontalSeparator()
    //
    /// <summary>
    /// Draws a horizontal line at the bottom of a legend element.
    /// </summary>
    ///
    /// <param name="oDrawingObjects">
    /// Objects to draw with.
    /// </param>
    ///
    /// <param name="oColumnRectangle">
    /// Column rectangle without margin.
    /// </param>
    ///
    /// <param name="iTop">
    /// Top y-coordinate where the separator should be drawn.  Gets updated.
    /// </param>
    //*************************************************************************

    protected void
    DrawHorizontalSeparator
    (
        DrawingObjects oDrawingObjects,
        Rectangle oColumnRectangle,
        ref Int32 iTop
    )
    {
        Debug.Assert(oDrawingObjects != null);
        AssertValid();

        iTop += oDrawingObjects.GetFontHeightMultiple(0.6F);

        oDrawingObjects.Graphics.DrawLine(SystemPens.ControlLight,
            oColumnRectangle.Left, iTop, oColumnRectangle.Right, iTop);

        iTop += oDrawingObjects.GetFontHeightMultiple(0.3F);
    }

    //*************************************************************************
    //  Method: DrawColumnSeparator()
    //
    /// <summary>
    /// Draws a vertical separator between the control's two columns.
    /// </summary>
    ///
    /// <param name="oDrawingObjects">
    /// Objects to draw with.
    /// </param>
    ///
    /// <param name="oColumn2Rectangle">
    /// The second column's rectangle.
    /// </param>
    //*************************************************************************

    protected void
    DrawColumnSeparator
    (
        DrawingObjects oDrawingObjects,
        Rectangle oColumn2Rectangle
    )
    {
        Debug.Assert(oDrawingObjects != null);
        AssertValid();

        // This code was removed in April 2011.  It is kept as a placeholder in
        // case the separator is requested again.

        #if false

        oDrawingObjects.Graphics.DrawLine(SystemPens.ControlLight,
            oColumn2Rectangle.Left, oColumn2Rectangle.Top,
            oColumn2Rectangle.Left, oColumn2Rectangle.Bottom);

        #endif
    }

    //*************************************************************************
    //  Method: MeasureTextWidth()
    //
    /// <summary>
    /// Gets the width of some text.
    /// </summary>
    ///
    /// <param name="oDrawingObjects">
    /// Objects to draw with.
    /// </param>
    ///
    /// <param name="sText">
    /// The text to measure.
    /// </param>
    ///
    /// <returns>
    /// The width of the text, rounded up.
    /// </returns>
    //*************************************************************************

    protected Int32
    MeasureTextWidth
    (
        DrawingObjects oDrawingObjects,
        String sText
    )
    {
        Debug.Assert(oDrawingObjects != null);
        Debug.Assert( !String.IsNullOrEmpty(sText) );
        AssertValid();

        return ( (Int32)Math.Ceiling(oDrawingObjects.Graphics.MeasureString(
            sText, oDrawingObjects.Font).Width) );
    }


    //*************************************************************************
    //  Method: AssertValid()
    //
    /// <summary>
    /// Asserts if the object is in an invalid state.  Debug-only.
    /// </summary>
    //*************************************************************************

    [Conditional("DEBUG")] 

    public virtual void
    AssertValid()
    {
        // (Do nothing.)
    }


    //*************************************************************************
    //  Protected constants
    //*************************************************************************

    /// Margin added to the left and right edges of each column in the control
    /// rectangle.

    protected const Int32 ColumnHorizontalMargin = 12;


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)


    //*************************************************************************
    //  Embedded class: DrawingObjects
    //
    /// <summary>
    /// Contains objects used to draw the control.
    /// </summary>
    ///
    /// <remarks>
    /// One of these is created by <see cref="CreateDrawingObjects" /> and
    /// passed to the various drawing methods.  It saves having to pass too
    /// many arguments, and having to modify multiple method signatures when a
    /// new drawing object is added in the future.
    /// </remarks>
    //*************************************************************************

    protected class
    DrawingObjects
    {
        /// <summary>
        /// Object to draw with.
        /// </summary>

        public Graphics Graphics;

        /// <summary>
        /// Rectangle to draw the control within.
        /// </summary>

        public Rectangle ControlRectangle;

        /// <summary>
        /// Font to use.
        /// </summary>

        public Font Font;

        /// <summary>
        /// Height of the font.
        /// </summary>

        public Single FontHeight;

        /// <summary>
        /// StringFormat object for drawing trimmed text.
        /// </summary>

        public StringFormat TrimmingStringFormat;

        /// <summary>
        /// StringFormat object for drawing right-aligned text.
        /// </summary>

        public StringFormat RightAlignStringFormat;

        /// <summary>
        /// StringFormat object for drawing centered text.
        /// </summary>

        public StringFormat CenterAlignStringFormat;


        //*********************************************************************
        //  Method: GetFontHeightMultiple()
        //
        /// <summary>
        /// Gets a multiple of the font height.
        /// </summary>
        ///
        /// <param name="multiple">
        /// Multiple to use.
        /// </param>
        ///
        /// <returns>
        /// <paramref name="multiple" /> times the font height, truncated to
        /// an Int32.
        /// </returns>
        //*********************************************************************

        public Int32
        GetFontHeightMultiple
        (
            Single multiple
        )
        {
            Debug.Assert(multiple >= 0);

            return ( (Int32)(this.FontHeight * multiple) );
        }
    }
}
}
