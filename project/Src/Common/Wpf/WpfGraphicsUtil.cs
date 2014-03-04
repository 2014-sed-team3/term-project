
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using System.Windows.Interop;
using System.IO;
using System.Diagnostics;
using Smrf.GraphicsLib;

namespace Smrf.WpfGraphicsLib
{
//*****************************************************************************
//  Class: WpfGraphicsUtil
//
/// <summary>
/// Utility methods for drawing with WPF.
/// </summary>
///
/// <remarks>
/// All methods are static.
/// </remarks>
//*****************************************************************************

public static class WpfGraphicsUtil
{
    //*************************************************************************
    //  Static constructor: WpfGraphicsUtil()
    //
    /// <summary>
    /// Static constructor for the WpfGraphicsUtil class.
    /// </summary>
    //*************************************************************************

    static WpfGraphicsUtil()
    {
        Double d30DegreesInRadians = Math.PI / 6.0;

        Cosine30Degrees = Math.Cos(d30DegreesInRadians);
        Tangent30Degrees = Math.Tan(d30DegreesInRadians);
    }

    //*************************************************************************
    //  Method: GetScreenDpi()
    //
    /// <summary>
    /// Gets the DPI of the screen.
    /// </summary>
    ///
    /// <param name="visual">
    /// The Visual to use when getting the DPI.
    /// </param>
    ///
    /// <returns>
    /// The DPI of the screen, as a Size.
    /// </returns>
    //*************************************************************************

    public static Size
    GetScreenDpi
    (
        Visual visual
    )
    {
        Debug.Assert(visual != null);

        // This was modified from code found at http://www.codeproject.com/
        // Messages/3126941/How-to-get-Windows-wide-DPI-Scaling-number-
        // within-.aspx

        Matrix oMatrix = PresentationSource.FromVisual(visual).
            CompositionTarget.TransformToDevice;

        Double dDpiXScalingFactor = oMatrix.M11;
        Double dDpiYScalingFactor = oMatrix.M22;

        return ( new Size(dDpiXScalingFactor * 96.0,
            dDpiYScalingFactor * 96.0) );
    }

    //*************************************************************************
    //  Method: WpfToPx()
    //
    /// <summary>
    /// Converts a length in WPF units to screen pixels.
    /// </summary>
    ///
    /// <param name="lengthWpf">
    /// The length to convert, in WPF units.
    /// </param>
    ///
    /// <param name="screenDpi">
    /// The DPI of the screen.  Sample: 96.0.
    /// </param>
    //*************************************************************************

    public static Double
    WpfToPx
    (
        Double lengthWpf,
        Double screenDpi
    )
    {
        Debug.Assert(screenDpi > 0);

        return ( (lengthWpf / 96.0) * screenDpi );
    }

    //*************************************************************************
    //  Method: DrawPixelAlignedRectangle()
    //
    /// <summary>
    /// Draws a rectangle with pixel-aligned edges.
    /// </summary>
    ///
    /// <param name="drawingContext">
    /// The DrawingContext object to draw with.
    /// </param>
    ///
    /// <param name="brush">
    /// The Brush to fill the rectangle with, or null for no fill.
    /// </param>
    ///
    /// <param name="pen">
    /// The Pen to draw the rectangle with.  Can't be null.
    /// </param>
    ///
    /// <param name="rect">
    /// The rectangle to draw.
    /// </param>
    ///
    /// <remarks>
    /// Because of antialiasing, a rectangle edge that falls on a pixel
    /// boundary (meaning it has an integer coorindate) may appear blurry.
    /// This method aligns the edge so that it isn't blurry.
    /// </remarks>
    //*************************************************************************

    public static void
    DrawPixelAlignedRectangle
    (
        DrawingContext drawingContext,
        Brush brush,
        Pen pen,
        Rect rect
    )
    {
        Debug.Assert(drawingContext != null);
        Debug.Assert(pen != null);

        // This technique was described at "Draw lines excactly on physical
        // device pixels," by Christian Mosers, at
        // http://wpftutorial.net/DrawOnPhysicalDevicePixels.html.

        Double dHalfPenWidth = pen.Thickness / 2.0;
 
        GuidelineSet oGuidelineSet = new GuidelineSet();
        DoubleCollection oGuidelinesX = oGuidelineSet.GuidelinesX;
        DoubleCollection oGuidelinesY = oGuidelineSet.GuidelinesY;

        oGuidelinesX.Add(rect.Left + dHalfPenWidth);
        oGuidelinesX.Add(rect.Right + dHalfPenWidth);
        oGuidelinesY.Add(rect.Top + dHalfPenWidth);
        oGuidelinesY.Add(rect.Bottom + dHalfPenWidth);
 
        drawingContext.PushGuidelineSet(oGuidelineSet);
        drawingContext.DrawRectangle(brush, pen, rect);
        drawingContext.Pop();
    }

    //*************************************************************************
    //  Method: ColorToWpfColor()
    //
    /// <overloads>
    /// Converts a System.Drawing.Color to a System.Windows.Media.Color.
    /// </overloads>
    ///
    /// <summary>
    /// Converts a System.Drawing.Color to a System.Windows.Media.Color using
    /// the alpha value of the System.Drawing.Color.
    /// </summary>
    ///
    /// <param name="color">
    /// The System.Drawing.Color to convert.
    /// </param>
    ///
    /// <returns>
    /// <paramref name="color" /> converted to a System.Windows.Media color.
    /// The alpha value is the same as the alpha value of <paramref
    /// name="color" />.
    /// </returns>
    //*************************************************************************

    public static System.Windows.Media.Color
    ColorToWpfColor
    (
        System.Drawing.Color color
    )
    {
        return ( ColorToWpfColor(color, color.A) );
    }

    //*************************************************************************
    //  Method: ColorToWpfColor()
    //
    /// <summary>
    /// Converts a System.Drawing.Color to a System.Windows.Media.Color using a
    /// new alpha value.
    /// </summary>
    ///
    /// <param name="color">
    /// The System.Drawing.Color to convert.
    /// </param>
    ///
    /// <param name="newAlpha">
    /// The new alpha value to use.  The alpha value of <paramref
    /// name="color" /> is ignored.
    /// </param>
    ///
    /// <returns>
    /// <paramref name="color" /> converted to a System.Windows.Media color,
    /// with an alpha value of <paramref name="newAlpha" />.
    /// </returns>
    //*************************************************************************

    public static System.Windows.Media.Color
    ColorToWpfColor
    (
        System.Drawing.Color color,
        Byte newAlpha
    )
    {
        return ( System.Windows.Media.Color.FromArgb(newAlpha,
            color.R, color.G, color.B) );
    }

    //*************************************************************************
    //  Method: SetWpfColorAlpha()
    //
    /// <summary>
    /// Converts a System.Windows.Media.Color to a System.Windows.Media.Color
    /// using a new alpha value.
    /// </summary>
    ///
    /// <param name="color">
    /// The System.Windows.Media.Color to convert.
    /// </param>
    ///
    /// <param name="newAlpha">
    /// The new alpha value to use.  The alpha value of <paramref
    /// name="color" /> is ignored.
    /// </param>
    ///
    /// <returns>
    /// <paramref name="color" /> with an alpha value of <paramref
    /// name="newAlpha" />.
    /// </returns>
    //*************************************************************************

    public static System.Windows.Media.Color
    SetWpfColorAlpha
    (
        System.Windows.Media.Color color,
        Byte newAlpha
    )
    {
        return ( System.Windows.Media.Color.FromArgb(newAlpha,
            color.R, color.G, color.B) );
    }

    //*************************************************************************
    //  Method: GetContrastingColor()
    //
    /// <overloads>
    /// Gets a color that contrasts with a specified color.
    /// </overloads>
    ///
    /// <summary>
    /// Gets a color that provides maximum contrast with a specified color.
    /// </summary>
    ///
    /// <param name="color">
    /// The System.Windows.Media.Color to get a contrasting color for.
    /// </param>
    ///
    /// <returns>
    /// A color that provides maximum contrast with <paramref name="color" />.
    /// The contrasting color has the same alpha value as <paramref
    /// name="color" />.
    /// </returns>
    //*************************************************************************

    public static System.Windows.Media.Color
    GetContrastingColor
    (
        System.Windows.Media.Color color
    )
    {
        return ( GetContrastingColor(color, true) );
    }

    //*************************************************************************
    //  Method: GetContrastingColor()
    //
    /// <summary>
    /// Gets a color that provides a specified contrast with a specified color.
    /// </summary>
    ///
    /// <param name="color">
    /// The System.Windows.Media.Color to get a contrasting color for.
    /// </param>
    ///
    /// <param name="useMaximumContrast">
    /// true to get a color that provides maximum contrast with <paramref
    /// name="color" />, false to get a color that provides less contrast.
    /// </param>
    ///
    /// <returns>
    /// A color that contrasts with <paramref name="color" />.  The
    /// contrasting color has the same alpha value as <paramref
    /// name="color" />.
    /// </returns>
    //*************************************************************************

    public static System.Windows.Media.Color
    GetContrastingColor
    (
        System.Windows.Media.Color color,
        Boolean useMaximumContrast
    )
    {
        // This algorithm is based on the YIC color model used by televisions.
        // If the calculated brightness is less than a constant threshold,
        // white (or dark gray) is used for the contrasting color.  Otherwise,
        // black (or light gray) is used.
        //
        // See this thread:
        //
        // http://visual-c.itags.org/visual-c-c++/56778/

        Double dBrightness = 0.299 * color.R + 0.587 * color.G +
            0.114 * color.B;

        Byte btRgbComponent;

        if (dBrightness < 127)
        {
            btRgbComponent = (Byte)(useMaximumContrast ? 0xff : 0x44);
        }
        else
        {
            btRgbComponent = (Byte)(useMaximumContrast ? 0x00 : 0xbb);
        }

        return ( System.Windows.Media.Color.FromArgb(
            color.A, btRgbComponent, btRgbComponent, btRgbComponent) );
    }

    //*************************************************************************
    //  Method: FontToTypeface()
    //
    /// <summary>
    /// Converts a System.Drawing.Font to a System.Windows.Media.Typeface.
    /// </summary>
    ///
    /// <param name="font">
    /// The System.Drawing.Font to convert.
    /// </param>
    //*************************************************************************

    public static Typeface
    FontToTypeface
    (
        System.Drawing.Font font
    )
    {
        Debug.Assert(font != null);

        return ( new System.Windows.Media.Typeface(
            new FontFamily(font.Name),
            font.Italic ? FontStyles.Italic : FontStyles.Normal,
            font.Bold ? FontWeights.Bold : FontWeights.Normal,
            FontStretches.Normal
            ) );
    }

    //*************************************************************************
    //  Method: SystemDrawingFontSizeToWpfFontSize()
    //
    /// <summary>
    /// Converts a System.Drawing font size to a WPF font size.
    /// </summary>
    ///
    /// <param name="windowsFormsFontSize">
    /// Font size as used by Windows Forms.
    /// </param>
    ///
    /// <returns>
    /// Equivalent font size as used by WPF.
    /// </returns>
    //*************************************************************************

    public static Double
    SystemDrawingFontSizeToWpfFontSize
    (
        Double windowsFormsFontSize
    )
    {
        Debug.Assert(windowsFormsFontSize >= 0);

        // The 0.75 comes from page 571 of "Windows Presentation Foundation
        // Unleashed," by Adam Nathan.

        return (windowsFormsFontSize / 0.75);
    }

    //*************************************************************************
    //  Method: WpfFontSizeToSystemDrawingFontSize()
    //
    /// <summary>
    /// Converts a WPF font size to a System.Drawing font size.
    /// </summary>
    ///
    /// <param name="wpfFontSize">
    /// Font size as used by WPF.
    /// </param>
    ///
    /// <returns>
    /// Equivalent font size as used by Windows Forms.
    /// </returns>
    //*************************************************************************

    public static Double
    WpfFontSizeToSystemDrawingFontSize
    (
        Double wpfFontSize
    )
    {
        Debug.Assert(wpfFontSize >= 0);

        return (wpfFontSize * 0.75);
    }

    //*************************************************************************
    //  Method: SetTextBlockFont()
    //
    /// <summary>
    /// Sets the font properties of a TextBlock using the properties of a
    /// System.Drawing.Font.
    /// </summary>
    ///
    /// <param name="textBlock">
    /// TextBlock to set the font properties on.
    /// </param>
    ///
    /// <param name="font">
    /// The System.Drawing.Font to copy font properties from.
    /// </param>
    //*************************************************************************

    public static void
    SetTextBlockFont
    (
        TextBlock textBlock,
        System.Drawing.Font font
    )
    {
        Debug.Assert(textBlock != null);
        Debug.Assert(font != null);

        // Use the common Typeface-related code, just for convenience.

        Typeface oTypeface = FontToTypeface(font);
        textBlock.FontFamily = oTypeface.FontFamily;
        textBlock.FontStyle = oTypeface.Style;
        textBlock.FontWeight = oTypeface.Weight;
        textBlock.FontSize = SystemDrawingFontSizeToWpfFontSize(font.Size);
    }

    //*************************************************************************
    //  Method: GetFormattedTextSize()
    //
    /// <summary>
    /// Gets the size of the actual text within a FormattedText object.
    /// </summary>
    ///
    /// <param name="formattedText">
    /// The object to get the size for.
    /// </param>
    ///
    /// <returns>
    /// The size of the actual text.
    /// </returns>
    ///
    /// <remarks>
    /// You can't use FormattedText.Width to get the width of the actual text
    /// when text wrapping is enabled (FormattedText.MaxTextWidth > 0).
    /// </remarks>
    //*************************************************************************

    public static Size
    GetFormattedTextSize
    (
        FormattedText formattedText
    )
    {
        Debug.Assert(formattedText != null);

        // Thanks to Forrester in the following post for the technique used
        // here and in GetFormattedTextBounds():
        //
        // http://social.msdn.microsoft.com/Forums/en-US/wpf/thread/
        // 4f84b34a-5d48-46b2-ac3b-86620a1d0f1b

        Double dFormattedTextWidth = formattedText.Width;

        if (dFormattedTextWidth == 0)
        {
            return ( new Size(0, 0) );
        }

        Double dWidth = dFormattedTextWidth - formattedText.OverhangLeading
            - formattedText.OverhangTrailing;

        return ( new Size(dWidth, formattedText.Extent) );
    }

    //*************************************************************************
    //  Method: GetFormattedTextBounds()
    //
    /// <summary>
    /// Gets the bounds of the actual text withing a FormattedText object.
    /// </summary>
    ///
    /// <param name="formattedText">
    /// The object to get the bounds for.
    /// </param>
    ///
    /// <param name="origin">
    /// The point at which <paramref name="formattedText" /> will be drawn.
    /// </param>
    ///
    /// <returns>
    /// The bounds of the actual text, as a Rect.
    /// </returns>
    //*************************************************************************

    public static Rect
    GetFormattedTextBounds
    (
        FormattedText formattedText,
        Point origin
    )
    {
        Debug.Assert(formattedText != null);

        Double dTextBottom =
            formattedText.Height + formattedText.OverhangAfter;

        Double dTextTop = dTextBottom - formattedText.Extent;
        Size oSize = GetFormattedTextSize(formattedText);

        Rect oBounds = new Rect(formattedText.OverhangLeading, dTextTop,
            oSize.Width, oSize.Height);

        oBounds.Offset(origin.X, origin.Y);

        return (oBounds);
    }

    //*************************************************************************
    //  Method: CreateOnePixelPen()
    //
    /// <summary>
    /// Creates a pen that is one pixel wide, regardless of the screen
    /// resolution.
    /// </summary>
    ///
    /// <param name="visual">
    /// The Visual being drawn.
    /// </param>
    ///
    /// <param name="brush">
    /// The brush to use for the pen.
    /// </param>
    //*************************************************************************

    public static Pen
    CreateOnePixelPen
    (
        Visual visual,
        Brush brush
    )
    {
        Debug.Assert(visual != null);

        // This technique was suggested here:
        //
        // http://www.wpftutorial.net/DrawOnPhysicalDevicePixels.html

        Matrix oMatrix = PresentationSource.FromVisual(visual).
            CompositionTarget.TransformToDevice;

        Debug.Assert(oMatrix.M11 != 0);

        Double dDpiFactor = 1/oMatrix.M11;

        return ( new Pen(brush, dDpiFactor) );
    }

    //*************************************************************************
    //  Method: VisualToBitmap()
    //
    /// <summary>
    /// Renders a Visual to a System.Drawing.Bitmap.
    /// </summary>
    ///
    /// <param name="visual">
    /// The Visual to render.
    /// </param>
    ///
    /// <param name="bitmapWidthPx">
    /// Bitmap width, in pixels.
    /// </param>
    ///
    /// <param name="bitmapHeightPx">
    /// Bitmap height, in pixels.
    /// </param>
    ///
    /// <returns>
    /// A System.Drawing.Bitmap with the specified dimensions.
    /// </returns>
    //*************************************************************************

    public static System.Drawing.Bitmap
    VisualToBitmap
    (
        Visual visual,
        Int32 bitmapWidthPx,
        Int32 bitmapHeightPx
    )
    {
        Debug.Assert(visual != null);
        Debug.Assert(bitmapWidthPx > 0);
        Debug.Assert(bitmapHeightPx > 0);

        RenderTargetBitmap oRenderTargetBitmap = new RenderTargetBitmap(
            bitmapWidthPx, bitmapHeightPx, 96, 96, PixelFormats.Default);

        oRenderTargetBitmap.Render(visual);

        BmpBitmapEncoder oBmpBitmapEncoder = new BmpBitmapEncoder();

        oBmpBitmapEncoder.Frames.Add(
            BitmapFrame.Create(oRenderTargetBitmap) );

        MemoryStream oMemoryStream = new MemoryStream();

        oBmpBitmapEncoder.Save(oMemoryStream);

        System.Drawing.Bitmap oBitmap =
            new System.Drawing.Bitmap(oMemoryStream);

        // (The stream must be kept open during the lifetime of the bitmap.

        return (oBitmap);
    }

    //*************************************************************************
    //  Method: BitmapToBitmapSource()
    //
    /// <summary>
    /// Converts a System.Drawing.Bitmap to a
    /// System.Windows.Media.Imaging.BitmapSource.
    /// </summary>
    ///
    /// <param name="bitmap">
    /// The Bitmap to convert.
    /// </param>
    ///
    /// <returns>
    /// The new BitmapSource.
    /// </returns>
    //*************************************************************************

    public static System.Windows.Media.Imaging.BitmapSource
    BitmapToBitmapSource
    (
        System.Drawing.Bitmap bitmap
    )
    {
        Debug.Assert(bitmap != null);

        // This is based on code found at http://stackoverflow.com/questions/
        // 1546091/wpf-createbitmapsourcefromhbitmap-memory-leak.

        IntPtr hBitmap = bitmap.GetHbitmap();

        try
        {
            return ( Imaging.CreateBitmapSourceFromHBitmap(
                hBitmap, IntPtr.Zero, Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions() ) );
        }
        finally
        {
            DeleteObject(hBitmap);
        }
    }

    //*************************************************************************
    //  Method: RectToRectangle()
    //
    /// <summary>
    /// Converts a System.Windows.Rect to a System.Drawing.Rectangle.
    /// </summary>
    ///
    /// <param name="rect">
    /// The System.Windows.Rect to convert.
    /// </param>
    ///
    /// <returns>
    /// <paramref name="rect" /> converted to a System.Drawing.Rectangle.
    /// </returns>
    ///
    /// <remarks>
    /// The conversion is performed via a ceiling on the left and top
    /// coordinates and a floor on the right and bottom coordinates.  This
    /// forces the converted rectangle to be no larger than the original.
    /// </remarks>
    //*************************************************************************

    public static System.Drawing.Rectangle
    RectToRectangle
    (
        System.Windows.Rect rect
    )
    {
        return ( System.Drawing.Rectangle.FromLTRB(
            (Int32)Math.Ceiling(rect.Left), (Int32)Math.Ceiling(rect.Top),
            (Int32)Math.Floor(rect.Right), (Int32)Math.Floor(rect.Bottom)
            ) );
    }

    //*************************************************************************
    //  Method: RectangleToRect()
    //
    /// <summary>
    /// Converts a System.Drawing.Rectangle to a System.Windows.Rect.
    /// </summary>
    ///
    /// <param name="rectangle">
    /// The System.Drawing.Rectangle to convert.
    /// </param>
    ///
    /// <returns>
    /// <paramref name="rectangle" /> converted to a System.Windows.Rect.
    /// </returns>
    //*************************************************************************

    public static System.Windows.Rect
    RectangleToRect
    (
        System.Drawing.Rectangle rectangle
    )
    {
        return ( new System.Windows.Rect(
            rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height)
            );
    }

    //*************************************************************************
    //  Method: WpfPointToPoint()
    //
    /// <summary>
    /// Converts a System.Windows.Point to a System.Drawing.Point.
    /// </summary>
    ///
    /// <param name="point">
    /// The System.Windows.Point to convert.
    /// </param>
    ///
    /// <returns>
    /// <paramref name="point" /> converted to a System.Drawing.Point.
    /// </returns>
    ///
    /// <remarks>
    /// The conversion from Double coordinates to Int32 coordinates is
    /// performed via truncation.
    /// </remarks>
    //*************************************************************************

    public static System.Drawing.Point
    WpfPointToPoint
    (
        System.Windows.Point point
    )
    {
        return ( new System.Drawing.Point( (Int32)point.X,  (Int32)point.Y ) );
    }

    //*************************************************************************
    //  Method: PointFToWpfPoint()
    //
    /// <summary>
    /// Converts a System.Drawing.PointF to a System.Windows.Point.
    /// </summary>
    ///
    /// <param name="pointF">
    /// The System.Drawing.PointF to convert.
    /// </param>
    ///
    /// <returns>
    /// <paramref name="pointF" /> converted to a System.Windows.Point.
    /// </returns>
    //*************************************************************************

    public static System.Windows.Point
    PointFToWpfPoint
    (
        System.Drawing.PointF pointF
    )
    {
        return ( new System.Windows.Point(pointF.X, pointF.Y) );
    }

    //*************************************************************************
    //  Method: GetRectCenter()
    //
    /// <summary>
    /// Gets the center point of a System.Windows.Rect.
    /// </summary>
    ///
    /// <param name="rect">
    /// The System.Windows.Rect to get the center point for.
    /// </param>
    ///
    /// <returns>
    /// The center point of <paramref name="rect" />.
    /// </returns>
    //*************************************************************************

    public static Point
    GetRectCenter
    (
        System.Windows.Rect rect
    )
    {
        return ( new Point(
            rect.Left + rect.Width / 2.0,
            rect.Top + rect.Height / 2.0
            ) );
    }

    //*************************************************************************
    //  Method: GetDistanceBetweenPoints()
    //
    /// <summary>
    /// Gets the distance between two points.
    /// </summary>
    ///
    /// <param name="point1">
    /// The first point.
    /// </param>
    ///
    /// <param name="point2">
    /// The second point.
    /// </param>
    ///
    /// <returns>
    /// The distance between the two points.
    /// </returns>
    //*************************************************************************

    public static Double
    GetDistanceBetweenPoints
    (
        Point point1,
        Point point2
    )
    {
        return ( (point2 - point1).Length );
    }

    //*************************************************************************
    //  Method: GetAngleBetweenPointsRadians()
    //
    /// <summary>
    /// Gets the angle between two points, in radians.
    /// </summary>
    ///
    /// <param name="point1">
    /// The first point.
    /// </param>
    ///
    /// <param name="point2">
    /// The second point.
    /// </param>
    ///
    /// <returns>
    /// The angle between the two points, in radians, as computed by
    /// Math.Atan2.  Ranges between 0 and PI (0 to 180 degrees) and 0 to -PI
    /// (0 to -180 degrees).
    /// </returns>
    //*************************************************************************

    public static Double
    GetAngleBetweenPointsRadians
    (
        Point point1,
        Point point2
    )
    {
        return ( Math.Atan2(point1.Y - point2.Y, point2.X - point1.X) );
    }

    //*************************************************************************
    //  Method: SquareFromCenterAndHalfWidth()
    //
    /// <summary>
    /// Returns a square given a center point and half-width.
    /// </summary>
    ///
    /// <param name="center">
    /// The square's center.
    /// </param>
    ///
    /// <param name="halfWidth">
    /// One half the width of the square.
    /// </param>
    ///
    /// <returns>
    /// The specified square, as a System.Windows.Rect.
    /// </returns>
    //*************************************************************************

    public static System.Windows.Rect
    SquareFromCenterAndHalfWidth
    (
        System.Windows.Point center,
        Double halfWidth
    )
    {
        Debug.Assert(halfWidth >= 0);

        center.Offset(-halfWidth, -halfWidth);
        Double dWidth = 2 * halfWidth;

        return ( new System.Windows.Rect( center,
            new System.Windows.Size(dWidth, dWidth) ) );
    }

    //*************************************************************************
    //  Method: TriangleBoundsFromCenterAndHalfWidth()
    //
    /// <summary>
    /// Creates a rectangle that bounds a triangle centered on a specified
    /// point.
    /// </summary>
    ///
    /// <param name="center">
    /// The triangle's center.
    /// </param>
    ///
    /// <param name="halfWidth">
    /// One half the width of the square that bounds the triangle.
    /// </param>
    ///
    /// <returns>
    /// A rectangle that bounds the specified triangle.
    /// </returns>
    //*************************************************************************

    public static Rect
    TriangleBoundsFromCenterAndHalfWidth
    (
        System.Windows.Point center,
        Double halfWidth
    )
    {
        Debug.Assert(halfWidth >= 0);

        Double dCenterX = center.X;
        Double dCenterY = center.Y;

        Double dApexY = dCenterY - (halfWidth / Cosine30Degrees);
        Double dBaseY = dCenterY + (halfWidth * Tangent30Degrees);

        return ( new Rect(
            dCenterX - halfWidth,
            dApexY,
            2.0 * halfWidth,
            dBaseY - dApexY
            ) );
    }

    //*************************************************************************
    //  Method: GetRectangleMinusMargin()
    //
    /// <summary>
    /// Subtracts a margin from a rectangle.
    /// </summary>
    ///
    /// <param name="rectangle">
    /// The rectangle to subtract the margin from.
    /// </param>
    ///
    /// <param name="margin">
    /// The margin to subtract from each edge.  Must be greater than or equal
    /// to zero.
    /// </param>
    ///
    /// <returns>
    /// <paramref name="rectangle" /> with <paramref name="margin" />
    /// subtracted from each side, or Rect.Empty.
    /// </returns>
    ///
    /// <remarks>
    /// If <paramref name="rectangle" /> is narrower or shorter than twice the
    /// <paramref name="margin" />, Rect.Empty is returned.
    /// </remarks>
    //*************************************************************************

    public static Rect
    GetRectangleMinusMargin
    (
        Rect rectangle,
        Double margin
    )
    {
        Debug.Assert(margin >= 0);

        rectangle.Inflate(-margin, -margin);

        if (rectangle.Width <= 0 || rectangle.Height <= 0)
        {
            return (Rect.Empty);
        }

        return (rectangle);
    }

    //*************************************************************************
    //  Method: MovePointWithinBounds()
    //
    /// <summary>
    /// Moves a point so it is contained within a bounding rectangle.
    /// </summary>
    ///
    /// <param name="point">
    /// The point that needs to be contained within <paramref
    /// name="boundingRectangle" />.
    /// </param>
    ///
    /// <param name="boundingRectangle">
    /// The rectangle that <paramref name="point" /> needs to be contained
    /// within.
    /// </param>
    ///
    /// <returns>
    /// A copy of <paramref name="point" /> that is contained within <paramref
    /// name="boundingRectangle" />.
    /// </returns>
    //*************************************************************************

    public static Point
    MovePointWithinBounds
    (
        Point point,
        Rect boundingRectangle
    )
    {
        Double dX = point.X;
        dX = Math.Max(dX, boundingRectangle.Left);
        dX = Math.Min(dX, boundingRectangle.Right);

        Double dY = point.Y;
        dY = Math.Max(dY, boundingRectangle.Top);
        dY = Math.Min(dY, boundingRectangle.Bottom);

        return ( new Point(dX, dY) );
    }

    //*************************************************************************
    //  Method: MoveRectangleWithinBounds()
    //
    /// <summary>
    /// Moves a rectangle so it is contained within an outer bounding
    /// rectangle.
    /// </summary>
    ///
    /// <param name="rectangle">
    /// The rectangle that needs to be contained within <paramref
    /// name="boundingRectangle" />.
    /// </param>
    ///
    /// <param name="boundingRectangle">
    /// The rectangle that <paramref name="rectangle" /> needs to be contained
    /// within.
    /// </param>
    ///
    /// <param name="resizeRectangleIfNecessary">
    /// If this is true and moving <paramref name="rectangle" /> isn't enough
    /// to contain it within <paramref name="boundingRectangle" />, <paramref
    /// name="rectangle" /> is resized to force it to be contained.  If this is
    /// false, any excess is left hanging over the right and bottom edges of
    /// <paramref name="boundingRectangle" />.
    /// </param>
    ///
    /// <returns>
    /// A copy of <paramref name="rectangle" /> that is contained within
    /// <paramref name="boundingRectangle" />.
    /// </returns>
    //*************************************************************************

    public static Rect
    MoveRectangleWithinBounds
    (
        Rect rectangle,
        Rect boundingRectangle,
        Boolean resizeRectangleIfNecessary
    )
    {
        Rect movedRectangle = rectangle;

        if (!rectangle.IsEmpty)
        {
            if (resizeRectangleIfNecessary)
            {
                movedRectangle.Width = Math.Min(
                    movedRectangle.Width, boundingRectangle.Width);

                movedRectangle.Height = Math.Min(
                    movedRectangle.Height, boundingRectangle.Height);
            }

            Double dXOffset = boundingRectangle.Left - movedRectangle.Left;

            if (movedRectangle.Width > boundingRectangle.Width || dXOffset > 0)
            {
                movedRectangle.Offset(dXOffset, 0);
            }
            else
            {
                dXOffset = boundingRectangle.Right - movedRectangle.Right;

                if (dXOffset < 0)
                {
                    movedRectangle.Offset(dXOffset, 0);
                }
            }

            Double dYOffset = boundingRectangle.Top - movedRectangle.Top;

            if (movedRectangle.Height > boundingRectangle.Height ||
                dYOffset > 0)
            {
                movedRectangle.Offset(0, dYOffset);
            }
            else
            {
                dYOffset = boundingRectangle.Bottom - movedRectangle.Bottom;

                if (dYOffset < 0)
                {
                    movedRectangle.Offset(0, dYOffset);
                }
            }
        }

        return (movedRectangle);
    }

    //*************************************************************************
    //  Method: GetFarthestRectangleEdge()
    //
    /// <summary>
    /// Determines which edge of a rectangle is farthest from a point within
    /// the rectangle.
    /// </summary>
    ///
    /// <param name="point">
    /// The point to use.  Should be contained within <paramref
    /// name="rectangle" />.
    /// </param>
    ///
    /// <param name="rectangle">
    /// The rectangle to use.
    /// </param>
    ///
    /// <returns>
    /// The edge of <paramref name="rectangle" /> that is farthest from
    /// <paramref name="point" />, as a <see cref="RectangleEdge" />.
    /// </returns>
    ///
    /// <remarks>
    /// If the width or height of <paramref name="rectangle" /> is zero, or
    /// <paramref name="point" /> is not contained within <paramref
    /// name="rectangle" />, RectangleEdge.Left is arbitrarily returned.
    /// </remarks>
    //*************************************************************************

    public static RectangleEdge
    GetFarthestRectangleEdge
    (
        Point point,
        Rect rectangle
    )
    {
        if ( rectangle.Width <= 0 || rectangle.Height <= 0 ||
            !rectangle.Contains(point) )
        {
            return (RectangleEdge.Left);
        }

        Double dX = point.X;
        Double dY = point.Y;

        Double dDistanceFromLeft = dX - rectangle.Left;
        Double dDistanceFromRight = rectangle.Right - dX;
        Double dDistanceFromTop = dY - rectangle.Top;
        Double dDistanceFromBottom = rectangle.Bottom - dY;

        RectangleEdge eFarthestEdge = RectangleEdge.Left;
        Double dGreatestDistance = dDistanceFromLeft;

        if (dDistanceFromRight > dGreatestDistance)
        {
            eFarthestEdge = RectangleEdge.Right;
            dGreatestDistance = dDistanceFromRight;
        }

        if (dDistanceFromTop > dGreatestDistance)
        {
            eFarthestEdge = RectangleEdge.Top;
            dGreatestDistance = dDistanceFromTop;
        }

        if (dDistanceFromBottom > dGreatestDistance)
        {
            eFarthestEdge = RectangleEdge.Bottom;
            dGreatestDistance = dDistanceFromBottom;
        }

        return (eFarthestEdge);
    }

    //*************************************************************************
    //  Method: GetRotatedMatrix()
    //
    /// <summary>
    /// Gets an identity Matrix rotatated a specified angle about a point.
    /// </summary>
    ///
    /// <param name="centerOfRotation">
    /// The center of rotation.
    /// </param>
    ///
    /// <param name="angleToRotateDegrees">
    /// The angle to rotate the Matrix, in degrees.
    /// </param>
    ///
    /// <returns>
    /// A new identity Matrix rotated <paramref name="angleToRotateDegrees" />
    /// about <paramref name="centerOfRotation" />.
    /// </returns>
    //*************************************************************************

    public static Matrix
    GetRotatedMatrix
    (
        Point centerOfRotation,
        Double angleToRotateDegrees
    )
    {
        Matrix oMatrix = Matrix.Identity;

        oMatrix.RotateAt(angleToRotateDegrees,
            centerOfRotation.X, centerOfRotation.Y);

        return (oMatrix);
    }

    //*************************************************************************
    //  Method: TransformLength()
    //
    /// <summary>
    /// Transforms a length along the x or y direction.
    /// </summary>
    ///
    /// <param name="length">
    /// Length to transform.
    /// </param>
    ///
    /// <param name="transform">
    /// The Transform to use.
    /// </param>
    ///
    /// <param name="transformAlongX">
    /// true to transform the length along the x direction, false to transform
    /// the length along the y direction.
    /// </param>
    ///
    /// <returns>
    /// The transformed distance.
    /// </returns>
    //*************************************************************************

    public static Double
    TransformLength
    (
        Double length,
        Transform transform,
        Boolean transformAlongX
    )
    {
        Debug.Assert(transform != null);

        Point oPoint;

        if (transformAlongX)
        {
            oPoint = new Point(length, 0);
        }
        else
        {
            oPoint = new Point(0, length);
        }

        oPoint = transform.Transform(oPoint);

        return (transformAlongX ? oPoint.X : oPoint.Y);
    }

    //*************************************************************************
    //  Method: SwapPoints()
    //
    /// <summary>
    /// Swaps two Point structures.
    /// </summary>
    ///
    /// <param name="point1">
    /// The first Point to swap.
    /// </param>
    ///
    /// <param name="point2">
    /// The second Point to swap.
    /// </param>
    //*************************************************************************

    public static void
    SwapPoints
    (
        ref Point point1,
        ref Point point2
    )
    {
        Point oTemp = point2;
        point2 = point1;
        point1 = oTemp;
    }

    //*************************************************************************
    //  Method: FreezeIfFreezable()
    //
    /// <summary>
    /// Freezes a freezable object if possible.
    /// </summary>
    ///
    /// <param name="freezable">
    /// The object to freeze.
    /// </param>
    ///
    /// <returns>
    /// true if the object was frozen.
    /// </returns>
    //*************************************************************************

    public static Boolean
    FreezeIfFreezable
    (
        Freezable freezable
    )
    {
        Debug.Assert(freezable != null);

        if (freezable.CanFreeze)
        {
            freezable.Freeze();

            return (true);
        }

        return (false);
    }


    //*************************************************************************
    //  Private interop methods
    //*************************************************************************

    [System.Runtime.InteropServices.DllImport("gdi32.dll")]

    private static extern Boolean DeleteObject(IntPtr hObject);


    //*************************************************************************
    //  Public fields
    //*************************************************************************

    /// Used in equilateral triangle calculations.

    public static Double Cosine30Degrees;
    ///
    public static Double Tangent30Degrees;
}

}
