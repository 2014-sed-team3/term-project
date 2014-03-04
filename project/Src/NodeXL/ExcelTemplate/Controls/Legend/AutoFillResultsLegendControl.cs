
using System;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections.Generic;
using System.Diagnostics;
using Smrf.NodeXL.Visualization.Wpf;
using Smrf.AppLib;
using Smrf.GraphicsLib;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: AutoFillResultsLegendControl
//
/// <summary>
/// Displays a graph legend containing the results of the autofill feature.
/// </summary>
///
/// <remarks>
/// Call <see cref="Update" /> to specify the autofill results to display in
/// the legend.
///
/// <para>
/// Call <see cref="Clear" /> to clear the legend.
/// </para>
///
/// <para>
/// <see cref="Update" /> sets the control's height to allow the entire legend
/// to fit within the control.
/// </para>
///
/// <para>
/// See the <see cref="WorkbookAutoFiller" /> class for details on the autofill
/// feature.
/// </para>
///
/// </remarks>
//*****************************************************************************

public partial class AutoFillResultsLegendControl : LegendControlBase
{
    //*************************************************************************
    //  Constructor: AutoFillResultsLegendControl()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="AutoFillResultsLegendControl" /> class.
    /// </summary>
    //*************************************************************************

    public AutoFillResultsLegendControl()
    {
        m_iLastResizeWidth = Int32.MinValue;

        // Start with an empty (but not null) object.

        m_oAutoFillWorkbookResults = new AutoFillWorkbookResults();

        this.DoubleBuffered = true;
        this.BackColor = SystemColors.Window;

        AssertValid();
    }

    //*************************************************************************
    //  Method: Update()
    //
    /// <summary>
    /// Updates the graph legend with results for the autofill feature.
    /// </summary>
    ///
    /// <param name="autoFillWorkbookResults">
    /// The results of the autofill.
    /// </param>
    //*************************************************************************

    public void
    Update
    (
        AutoFillWorkbookResults autoFillWorkbookResults
    )
    {
        Debug.Assert(autoFillWorkbookResults != null);
        AssertValid();

        m_oAutoFillWorkbookResults = autoFillWorkbookResults;

        this.Height = CalculateHeight();
        Invalidate();
    }

    //*************************************************************************
    //  Method: Clear()
    //
    /// <summary>
    /// Clears the graph legend.
    /// </summary>
    //*************************************************************************

    public void
    Clear()
    {
        AssertValid();

        Update( new AutoFillWorkbookResults() );
    }

    //*************************************************************************
    //  Method: OnPaint()
    //
    /// <summary>
    /// Handles the Paint event.
    /// </summary>
    ///
    /// <param name="e">
    /// Standard event arguments.
    /// </param>
    //*************************************************************************

    protected override void
    OnPaint
    (
        PaintEventArgs e
    )
    {
        AssertValid();

        base.OnPaint(e);
        Draw( CreateDrawingObjects(e.Graphics, this.ClientRectangle) );
    }

    //*************************************************************************
    //  Method: OnResize()
    //
    /// <summary>
    /// Handles the Resize event.
    /// </summary>
    ///
    /// <param name="e">
    /// Standard event arguments.
    /// </param>
    //*************************************************************************

    protected override void
    OnResize
    (
        EventArgs e
    )
    {
        AssertValid();

        base.OnResize(e);

        Int32 iWidth = this.ClientRectangle.Width;

        if (
            iWidth != m_iLastResizeWidth &&
            (
                m_oAutoFillWorkbookResults.
                    EdgeColorResults.ColumnAutoFilledWithCategories
                ||
                m_oAutoFillWorkbookResults.
                    VertexColorResults.ColumnAutoFilledWithCategories
            )
           )
        {
            // The edge color or vertex color was autofilled with categories,
            // which means that the control height depends on the control
            // width, which just changed.  Recalculate the control height.

            this.Height = CalculateHeight();
        }

        m_iLastResizeWidth = iWidth;
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

    protected override Int32
    Draw
    (
        DrawingObjects oDrawingObjects
    )
    {
        Debug.Assert(oDrawingObjects != null);
        AssertValid();

        if (m_oAutoFillWorkbookResults.AutoFilledNonXYColumnCount > 0)
        {
            return ( DrawAutoFillWorkbookResults(oDrawingObjects) );
        }

        return (oDrawingObjects.ControlRectangle.Top);
    }

    //*************************************************************************
    //  Method: DrawAutoFillWorkbookResults()
    //
    /// <summary>
    /// Draws the results of the autofill.
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

    protected Int32
    DrawAutoFillWorkbookResults
    (
        DrawingObjects oDrawingObjects
    )
    {
        Debug.Assert(oDrawingObjects != null);
        AssertValid();

        // If there are autofilled edge and vertex results, two columns are
        // drawn, with the edge results in the first column and the vertex
        // results in the second.  If there are only edge or vertex results,
        // one column is drawn.

        Rectangle oColumn1Rectangle, oColumn2Rectangle;
        Boolean bHasTwoColumns = false;

        if (m_oAutoFillWorkbookResults.AutoFilledEdgeColumnCount > 0 &&
            m_oAutoFillWorkbookResults.AutoFilledVertexNonXYColumnCount > 0)
        {
            ControlRectangleToTwoColumns(oDrawingObjects,
                out oColumn1Rectangle, out oColumn2Rectangle);

            bHasTwoColumns = true;
        }
        else
        {
            oColumn1Rectangle = oDrawingObjects.ControlRectangle;
            AddMarginsToColumnRectangle(ref oColumn1Rectangle);
            oColumn2Rectangle = oColumn1Rectangle;
        }

        Int32 iBottom = Math.Max(
            DrawAutoFilledEdgeResults(oDrawingObjects, oColumn1Rectangle),
            DrawAutoFilledVertexResults(oDrawingObjects, oColumn2Rectangle)
            );

        if (bHasTwoColumns)
        {
            DrawColumnSeparator(oDrawingObjects, oColumn2Rectangle);
        }

        return (iBottom);
    }

    //*************************************************************************
    //  Method: DrawAutoFilledEdgeResults()
    //
    /// <summary>
    /// Draws the results for autofilled edge columns.
    /// </summary>
    ///
    /// <param name="oDrawingObjects">
    /// Objects to draw with.
    /// </param>
    ///
    /// <param name="oColumnRectangle">
    /// Rectangle to draw the results within.
    /// </param>
    ///
    /// <returns>
    /// The bottom of the area that was drawn.
    /// </returns>
    //*************************************************************************

    protected Int32
    DrawAutoFilledEdgeResults
    (
        DrawingObjects oDrawingObjects,
        Rectangle oColumnRectangle
    )
    {
        Debug.Assert(oDrawingObjects != null);
        AssertValid();

        Int32 iTop = oColumnRectangle.Top;

        if (m_oAutoFillWorkbookResults.AutoFilledEdgeColumnCount > 0)
        {
            DrawColumnHeader(oDrawingObjects, EdgePropertyHeader,
                oColumnRectangle.Left, oColumnRectangle.Right, ref iTop);

            Int32 iResultsLeft = oColumnRectangle.Left;
            Int32 iResultsRight = oColumnRectangle.Right;

            AutoFillColorColumnResults oEdgeColorResults =
                m_oAutoFillWorkbookResults.EdgeColorResults;

            if (oEdgeColorResults.ColumnAutoFilled)
            {
                DrawColorColumnResults(oDrawingObjects, ColorCaption,
                    oEdgeColorResults, VertexShape.SolidSquare,
                    oColumnRectangle, iResultsLeft, iResultsRight, ref iTop);
            }

            AutoFillNumericRangeColumnResults oEdgeWidthResults =
                m_oAutoFillWorkbookResults.EdgeWidthResults;

            if (oEdgeWidthResults.ColumnAutoFilled)
            {
                DrawRampResults(oDrawingObjects,
                    oEdgeWidthResults.SourceColumnName, EdgeWidthCaption,
                    oEdgeWidthResults.SourceCalculationNumber1,
                    oEdgeWidthResults.SourceCalculationNumber2,
                    oEdgeWidthResults.DecimalPlaces, oColumnRectangle,
                    iResultsLeft, iResultsRight, ref iTop);
            }

            AutoFillNumericRangeColumnResults oEdgeAlphaResults =
                m_oAutoFillWorkbookResults.EdgeAlphaResults;

            if (oEdgeAlphaResults.ColumnAutoFilled)
            {
                DrawAutoFilledAlphaResults(oDrawingObjects,
                    oEdgeAlphaResults.SourceColumnName,
                    oEdgeAlphaResults.SourceCalculationNumber1,
                    oEdgeAlphaResults.SourceCalculationNumber2,
                    oEdgeAlphaResults.DecimalPlaces,
                    oEdgeAlphaResults.DestinationNumber1,
                    oEdgeAlphaResults.DestinationNumber2,
                    oColumnRectangle, iResultsLeft, iResultsRight, ref iTop);
            }
        }

        return (iTop);
    }

    //*************************************************************************
    //  Method: DrawAutoFilledVertexResults()
    //
    /// <summary>
    /// Draws the results for autofilled vertex columns.
    /// </summary>
    ///
    /// <param name="oDrawingObjects">
    /// Objects to draw with.
    /// </param>
    ///
    /// <param name="oColumnRectangle">
    /// Rectangle to draw the results within.
    /// </param>
    ///
    /// <returns>
    /// The bottom of the area that was drawn.
    /// </returns>
    //*************************************************************************

    protected Int32
    DrawAutoFilledVertexResults
    (
        DrawingObjects oDrawingObjects,
        Rectangle oColumnRectangle
    )
    {
        Debug.Assert(oDrawingObjects != null);
        AssertValid();

        Int32 iTop = oColumnRectangle.Top;

        if (m_oAutoFillWorkbookResults.AutoFilledVertexNonXYColumnCount > 0)
        {
            DrawColumnHeader(oDrawingObjects, VertexPropertyHeader,
                oColumnRectangle.Left, oColumnRectangle.Right, ref iTop);

            Int32 iResultsLeft = oColumnRectangle.Left;
            Int32 iResultsRight = oColumnRectangle.Right;

            AutoFillColorColumnResults oVertexColorResults =
                m_oAutoFillWorkbookResults.VertexColorResults;

            if (oVertexColorResults.ColumnAutoFilled)
            {
                DrawColorColumnResults(oDrawingObjects, ColorCaption,
                    oVertexColorResults, VertexShape.Disk,
                    oColumnRectangle, iResultsLeft, iResultsRight, ref iTop);
            }

            AutoFillNumericRangeColumnResults oVertexRadiusResults =
                m_oAutoFillWorkbookResults.VertexRadiusResults;

            if (oVertexRadiusResults.ColumnAutoFilled)
            {
                DrawRampResults(oDrawingObjects,
                    oVertexRadiusResults.SourceColumnName, "Size",
                    oVertexRadiusResults.SourceCalculationNumber1,
                    oVertexRadiusResults.SourceCalculationNumber2,
                    oVertexRadiusResults.DecimalPlaces, oColumnRectangle,
                    iResultsLeft, iResultsRight, ref iTop);
            }

            AutoFillNumericRangeColumnResults oVertexAlphaResults =
                m_oAutoFillWorkbookResults.VertexAlphaResults;

            if (oVertexAlphaResults.ColumnAutoFilled)
            {
                DrawAutoFilledAlphaResults(oDrawingObjects,
                    oVertexAlphaResults.SourceColumnName,
                    oVertexAlphaResults.SourceCalculationNumber1,
                    oVertexAlphaResults.SourceCalculationNumber2,
                    oVertexAlphaResults.DecimalPlaces,
                    oVertexAlphaResults.DestinationNumber1,
                    oVertexAlphaResults.DestinationNumber2,
                    oColumnRectangle, iResultsLeft, iResultsRight, ref iTop);
            }
        }

        return (iTop);
    }

    //*************************************************************************
    //  Method: DrawAutoFilledAlphaResults()
    //
    /// <summary>
    /// Draws the results for one autofilled alpha column.
    /// </summary>
    ///
    /// <param name="oDrawingObjects">
    /// Objects to draw with.
    /// </param>
    ///
    /// <param name="sSourceColumnName">
    /// The name of the source column.
    /// </param>
    ///
    /// <param name="dSourceCalculationNumber1">
    /// The actual first source number used in the calculations.
    /// </param>
    ///
    /// <param name="dSourceCalculationNumber2">
    /// The actual second source number used in the calculations.
    /// </param>
    ///
    /// <param name="iDecimalPlaces">
    /// The number of decimal places displayed in the source column.
    /// </param>
    ///
    /// <param name="dDestinationNumber1">
    /// The first number used in the destination column.
    /// </param>
    ///
    /// <param name="dDestinationNumber2">
    /// The second number used in the destination column.
    /// </param>
    ///
    /// <param name="oColumnRectangle">
    /// Rectangle to draw the results within.
    /// </param>
    ///
    /// <param name="iResultsLeft">
    /// Left x-coordinate where the results should be drawn.
    /// </param>
    ///
    /// <param name="iResultsRight">
    /// Right x-coordinate where the results should be drawn.
    /// </param>
    ///
    /// <param name="iTop">
    /// Top y-coordinate where the results should be drawn.  Gets updated.
    /// </param>
    //*************************************************************************

    protected void
    DrawAutoFilledAlphaResults
    (
        DrawingObjects oDrawingObjects,
        String sSourceColumnName,
        Double dSourceCalculationNumber1,
        Double dSourceCalculationNumber2,
        Int32 iDecimalPlaces,
        Double dDestinationNumber1,
        Double dDestinationNumber2,
        Rectangle oColumnRectangle,
        Int32 iResultsLeft,
        Int32 iResultsRight,
        ref Int32 iTop
    )
    {
        Debug.Assert(oDrawingObjects != null);
        Debug.Assert( !String.IsNullOrEmpty(sSourceColumnName) );
        Debug.Assert(iDecimalPlaces >= 0);
        AssertValid();

        // The alpha results look like the color results, with the colors set
        // to black with different alphas.  Calculate the colors.

        AlphaConverter oAlphaConverter = new AlphaConverter();

        Int32 iDestinationAlpha1 = (Int32)oAlphaConverter.WorkbookToGraph(
            (Single)dDestinationNumber1 );

        Int32 iDestinationAlpha2 = (Int32)oAlphaConverter.WorkbookToGraph(
            (Single)dDestinationNumber2 );

        Color oDestinationColor1 =
            Color.FromArgb(iDestinationAlpha1, SystemColors.WindowText);

        Color oDestinationColor2 =
            Color.FromArgb(iDestinationAlpha2, SystemColors.WindowText);

        DrawColorBarResults(oDrawingObjects, sSourceColumnName, "Opacity",
            dSourceCalculationNumber1, dSourceCalculationNumber2,
            iDecimalPlaces, oDestinationColor1, oDestinationColor2,
            oColumnRectangle, iResultsLeft, iResultsRight, ref iTop);
    }

    //*************************************************************************
    //  Method: DrawColorColumnResults()
    //
    /// <summary>
    /// Draws the results for one autofilled color column.
    /// </summary>
    ///
    /// <param name="oDrawingObjects">
    /// Objects to draw with.
    /// </param>
    ///
    /// <param name="sCaption">
    /// Caption to draw above the color gradient bar.
    /// </param>
    ///
    /// <param name="oColorResults">
    /// Results for the autofilled color column.
    /// </param>
    ///
    /// <param name="eCategoryShape">
    /// Shape to use for each category.
    /// </param>
    ///
    /// <param name="oColumnRectangle">
    /// Rectangle to draw the results within.
    /// </param>
    ///
    /// <param name="iResultsLeft">
    /// Left x-coordinate where the results should be drawn.
    /// </param>
    ///
    /// <param name="iResultsRight">
    /// Right x-coordinate where the results should be drawn.
    /// </param>
    ///
    /// <param name="iTop">
    /// Top y-coordinate where the results should be drawn.  Gets updated.
    /// </param>
    //*************************************************************************

    protected void
    DrawColorColumnResults
    (
        DrawingObjects oDrawingObjects,
        String sCaption,
        AutoFillColorColumnResults oColorResults,
        VertexShape eCategoryShape,
        Rectangle oColumnRectangle,
        Int32 iResultsLeft,
        Int32 iResultsRight,
        ref Int32 iTop
    )
    {
        Debug.Assert(oDrawingObjects != null);
        Debug.Assert( !String.IsNullOrEmpty(sCaption) );
        Debug.Assert(oColorResults != null);

        if (oColorResults.SourceColumnContainsNumbers)
        {
            DrawColorBarResults(oDrawingObjects,
                oColorResults.SourceColumnName, ColorCaption,
                oColorResults.SourceCalculationNumber1,
                oColorResults.SourceCalculationNumber2,
                oColorResults.DecimalPlaces,
                oColorResults.DestinationColor1,
                oColorResults.DestinationColor2,
                oColumnRectangle, iResultsLeft, iResultsRight,
                ref iTop);
        }
        else
        {
            DrawColorCategoryResults(oDrawingObjects,
                oColorResults.CategoryNames,

                (Int32 categoryIndex, out VertexShape categoryShape,
                    out Color categoryColor) =>
                {
                    categoryShape = eCategoryShape;

                    categoryColor = ColorUtil.GetUniqueColor(categoryIndex,
                        oColorResults.CategoryNames.Count);
                },

                oColumnRectangle, ref iTop
                );
        }
    }

    //*************************************************************************
    //  Method: DrawColorBarResults()
    //
    /// <overloads>
    /// Draws the results for one autofilled column using a color gradient bar.
    /// </overloads>
    ///
    /// <summary>
    /// Draws the results for one autofilled column using a color gradient bar
    /// and source calculation numbers.
    /// </summary>
    ///
    /// <param name="oDrawingObjects">
    /// Objects to draw with.
    /// </param>
    ///
    /// <param name="sSourceColumnName">
    /// The name of the source column.
    /// </param>
    ///
    /// <param name="sCaption">
    /// Caption to draw above the color gradient bar.
    /// </param>
    ///
    /// <param name="dSourceCalculationNumber1">
    /// The actual first source number used in the calculations.
    /// </param>
    ///
    /// <param name="dSourceCalculationNumber2">
    /// The actual second source number used in the calculations.
    /// </param>
    ///
    /// <param name="iDecimalPlaces">
    /// The number of decimal places displayed in the source column.
    /// </param>
    ///
    /// <param name="oColor1">
    /// The color to use at the left edge of the color gradient bar.
    /// </param>
    ///
    /// <param name="oColor2">
    /// The color to use at the left edge of the color gradient bar.
    /// </param>
    ///
    /// <param name="oColumnRectangle">
    /// Rectangle to draw the results within.
    /// </param>
    ///
    /// <param name="iResultsLeft">
    /// Left x-coordinate where the results should be drawn.
    /// </param>
    ///
    /// <param name="iResultsRight">
    /// Right x-coordinate where the results should be drawn.
    /// </param>
    ///
    /// <param name="iTop">
    /// Top y-coordinate where the results should be drawn.  Gets updated.
    /// </param>
    //*************************************************************************

    protected void
    DrawColorBarResults
    (
        DrawingObjects oDrawingObjects,
        String sSourceColumnName,
        String sCaption,
        Double dSourceCalculationNumber1,
        Double dSourceCalculationNumber2,
        Int32 iDecimalPlaces,
        Color oColor1,
        Color oColor2,
        Rectangle oColumnRectangle,
        Int32 iResultsLeft,
        Int32 iResultsRight,
        ref Int32 iTop
    )
    {
        Debug.Assert(oDrawingObjects != null);
        Debug.Assert( !String.IsNullOrEmpty(sSourceColumnName) );
        Debug.Assert( !String.IsNullOrEmpty(sCaption) );
        Debug.Assert(iDecimalPlaces >= 0);
        AssertValid();

        if (dSourceCalculationNumber2 == dSourceCalculationNumber1)
        {
            // All the source numbers were the same.  Draw one color only.

            oColor2 = oColor1;
        }

        DrawColorBarResults(oDrawingObjects, sSourceColumnName, sCaption,
            DoubleToString(dSourceCalculationNumber1, iDecimalPlaces),
            DoubleToString(dSourceCalculationNumber2, iDecimalPlaces),
            oColor1, oColor2, oColumnRectangle, iResultsLeft, iResultsRight,
            ref iTop);
    }

    //*************************************************************************
    //  Method: DrawColorBarResults()
    //
    /// <summary>
    /// Draws the results for one autofilled column using a color gradient bar
    /// and source calculation strings.
    /// </summary>
    ///
    /// <param name="oDrawingObjects">
    /// Objects to draw with.
    /// </param>
    ///
    /// <param name="sSourceColumnName">
    /// The name of the source column.
    /// </param>
    ///
    /// <param name="sCaption">
    /// Caption to draw above the color gradient bar.
    /// </param>
    ///
    /// <param name="sSourceCalculationMinimum">
    /// The actual minimum value used in the calculations.
    /// </param>
    ///
    /// <param name="sSourceCalculationMaximum">
    /// The actual maximum value used in the calculations.
    /// </param>
    ///
    /// <param name="oColor1">
    /// The color to use at the left edge of the color gradient bar.
    /// </param>
    ///
    /// <param name="oColor2">
    /// The color to use at the left edge of the color gradient bar.
    /// </param>
    ///
    /// <param name="oColumnRectangle">
    /// Rectangle to draw the results within.
    /// </param>
    ///
    /// <param name="iResultsLeft">
    /// Left x-coordinate where the results should be drawn.
    /// </param>
    ///
    /// <param name="iResultsRight">
    /// Right x-coordinate where the results should be drawn.
    /// </param>
    ///
    /// <param name="iTop">
    /// Top y-coordinate where the results should be drawn.  Gets updated.
    /// </param>
    //*************************************************************************

    protected void
    DrawColorBarResults
    (
        DrawingObjects oDrawingObjects,
        String sSourceColumnName,
        String sCaption,
        String sSourceCalculationMinimum,
        String sSourceCalculationMaximum,
        Color oColor1,
        Color oColor2,
        Rectangle oColumnRectangle,
        Int32 iResultsLeft,
        Int32 iResultsRight,
        ref Int32 iTop
    )
    {
        Debug.Assert(oDrawingObjects != null);
        Debug.Assert( !String.IsNullOrEmpty(sSourceColumnName) );
        Debug.Assert( !String.IsNullOrEmpty(sCaption) );
        Debug.Assert( !String.IsNullOrEmpty(sSourceCalculationMinimum) );
        Debug.Assert( !String.IsNullOrEmpty(sSourceCalculationMaximum) );
        AssertValid();

        iTop += oDrawingObjects.GetFontHeightMultiple(0.2F);

        DrawRangeText(oDrawingObjects, sSourceCalculationMinimum,
            sSourceCalculationMaximum, SystemBrushes.WindowText, iResultsLeft,
            iResultsRight, ref iTop);

        iTop += oDrawingObjects.GetFontHeightMultiple(0.35F);

        Int32 iColorBarMargin = oDrawingObjects.GetFontHeightMultiple(0.1F);

        Rectangle oColorBarRectangle = Rectangle.FromLTRB(
            iResultsLeft + iColorBarMargin,
            iTop,
            iResultsRight - iColorBarMargin,
            iTop + ColorBarHeight
            );

        // There is a known bug in the LinearGradientBrush that sometimes
        // causes a single line of the wrong color to be drawn at the start of
        // the gradient.  The workaround is to use a brush rectangle that is
        // one pixel larger than the fill rectangle on all sides.

        Rectangle oBrushRectangle = oColorBarRectangle;
        oBrushRectangle.Inflate(1, 1);

        LinearGradientBrush oBrush = new LinearGradientBrush(
            oBrushRectangle, oColor1, oColor2, 0F);

        oDrawingObjects.Graphics.FillRectangle(oBrush, oColorBarRectangle);
        oBrush.Dispose();

        iTop += ColorBarHeight + oDrawingObjects.GetFontHeightMultiple(0.05F);

        DrawExcelColumnNameAndCaption(oDrawingObjects, sSourceColumnName,
            sCaption, iResultsLeft, iResultsRight, ref iTop);

        // Draw a line separating these results from the next.

        DrawHorizontalSeparator(oDrawingObjects, oColumnRectangle, ref iTop);
    }

    //*************************************************************************
    //  Delegate: GetShapeAndColorForCategory
    //
    /// <summary>
    /// Represents a method that gets the shape and color to use for a
    /// category.
    /// </summary>
    ///
    /// <param name="categoryIndex">
    /// Index of the category.
    /// </param>
    ///
    /// <param name="categoryShape">
    /// Where the shape to use for the category gets stored.
    /// </param>
    ///
    /// <param name="categoryColor">
    /// Where the color to use for the category gets stored.
    /// </param>
    //*************************************************************************

    protected delegate void
    GetShapeAndColorForCategory
    (
        Int32 categoryIndex,
        out VertexShape categoryShape,
        out Color categoryColor
    );

    //*************************************************************************
    //  Method: DrawColorCategoryResults()
    //
    /// <summary>
    /// Draws the results for a color column that was autofilled using category
    /// names.
    /// </summary>
    ///
    /// <param name="oDrawingObjects">
    /// Objects to draw with.
    /// </param>
    ///
    /// <param name="oCategoryNames">
    /// Collection of category names.
    /// </param>
    ///
    /// <param name="oShapeAndColorGetter">
    /// Gets the shape and color to use for a category.
    /// </param>
    ///
    /// <param name="oColumnRectangle">
    /// Rectangle to draw the results within.
    /// </param>
    ///
    /// <param name="iTop">
    /// Top y-coordinate where the results should be drawn.  Gets updated.
    /// </param>
    //*************************************************************************

    protected void
    DrawColorCategoryResults
    (
        DrawingObjects oDrawingObjects,
        ICollection<String> oCategoryNames,
        GetShapeAndColorForCategory oShapeAndColorGetter,
        Rectangle oColumnRectangle,
        ref Int32 iTop
    )
    {
        Debug.Assert(oDrawingObjects != null);
        Debug.Assert(oCategoryNames != null);
        AssertValid();

        Graphics oGraphics = oDrawingObjects.Graphics;
        iTop += oDrawingObjects.GetFontHeightMultiple(0.2F);

        // The categories are drawn from left to right.  Each category is drawn
        // as a shape followed by the category name.

        // Calculate the width of one category column.

        Single fShapeWidth = CategoryShapeFactor * oDrawingObjects.FontHeight;
        Single fShapeHalfWidth = fShapeWidth / 2F;

        Single fColumnWidth = (Single)Math.Ceiling( fShapeWidth +
            (MaximumCategoryNameLength * oDrawingObjects.FontHeight) );

        // How many columns will fit?  There must be at least one, even if
        // there isn't enough room for one column.

        Debug.Assert(fColumnWidth > 0);

        Int32 iColumns = Math.Max( 1,
            (Int32)(oColumnRectangle.Width / fColumnWidth) );

        fColumnWidth = (Single)oColumnRectangle.Width / (Single)iColumns;

        Int32 iCategories = oCategoryNames.Count;
        Int32 iColumn = 0;
        Single fLeft = oColumnRectangle.Left;

        Pen oPen = new Pen(Color.Black, 1.0F);
        oPen.Alignment = PenAlignment.Inset;

        SolidBrush oBrush = new SolidBrush(Color.Black);

        SmoothingMode eOldSmoothingMode = oGraphics.SmoothingMode;
        oGraphics.SmoothingMode = SmoothingMode.HighQuality;
        Int32 i = 0;

        foreach (String sCategoryName in oCategoryNames)
        {
            // Get the shape and color for this category.

            VertexShape eVertexShape;
            Color oVertexColor;

            oShapeAndColorGetter(i, out eVertexShape, out oVertexColor);

            DrawVertexCategoryShape(oDrawingObjects, eVertexShape, oPen,
                oBrush, oVertexColor,
                fLeft + fShapeHalfWidth + 2,
                iTop + fShapeHalfWidth + 1,
                fShapeHalfWidth);

            // Don't let the category name spill over into the next column.

            Single fTopOffset = fShapeWidth * 0.15F;

            Rectangle oNameRectangle = Rectangle.FromLTRB(
                (Int32)(fLeft + fShapeWidth * 1.4F),
                (Int32)(iTop + fTopOffset),
                (Int32)(fLeft + fColumnWidth),
                (Int32)(iTop + oDrawingObjects.FontHeight + fTopOffset)
                );

            oGraphics.DrawString(sCategoryName, oDrawingObjects.Font,
                SystemBrushes.WindowText, oNameRectangle,
                oDrawingObjects.TrimmingStringFormat);

            if (iCategories > MaximumCategoriesToDraw &&
                i == MaximumCategoriesToDraw - 1)
            {
                oGraphics.DrawString(
                    "There are additional categories that are not shown here.",
                    oDrawingObjects.Font, SystemBrushes.WindowText,
                    oColumnRectangle.Left,
                    iTop + 1.5F * oDrawingObjects.FontHeight);

                iTop += oDrawingObjects.GetFontHeightMultiple(3F);
                break;
            }

            oGraphics.DrawLine( SystemPens.ControlLight,
                oNameRectangle.Right - 1,
                iTop,
                oNameRectangle.Right - 1,
                iTop + oDrawingObjects.GetFontHeightMultiple(1.4F)
                );

            iColumn++;
            fLeft += fColumnWidth;
            Boolean bIncrementTop = false;

            if (iColumn == iColumns)
            {
                iColumn = 0;
                fLeft = oColumnRectangle.Left;
                bIncrementTop = true;
            }
            else if (i == iCategories - 1)
            {
                bIncrementTop = true;
            }

            if (bIncrementTop)
            {
                iTop += oDrawingObjects.GetFontHeightMultiple(1.4F);

                // Draw a line separating these results from the next.

                DrawHorizontalSeparator(oDrawingObjects, oColumnRectangle,
                    ref iTop);

                iTop += oDrawingObjects.GetFontHeightMultiple(0.2F);
            }

            i++;
        }

        oPen.Dispose();
        oBrush.Dispose();
        oGraphics.SmoothingMode = eOldSmoothingMode;
    }

    //*************************************************************************
    //  Method: DrawRampResults()
    //
    /// <summary>
    /// Draws the results for one autofilled column using a ramp that increases
    /// in height from left to right.
    /// </summary>
    ///
    /// <param name="oDrawingObjects">
    /// Objects to draw with.
    /// </param>
    ///
    /// <param name="sSourceColumnName">
    /// The name of the source column.
    /// </param>
    ///
    /// <param name="sCaption">
    /// Caption to draw above the color gradient bar.
    /// </param>
    ///
    /// <param name="dSourceCalculationNumber1">
    /// The actual first source number used in the calculations.
    /// </param>
    ///
    /// <param name="dSourceCalculationNumber2">
    /// The actual second source number used in the calculations.
    /// </param>
    ///
    /// <param name="iDecimalPlaces">
    /// The number of decimal places displayed in the source column.
    /// </param>
    ///
    /// <param name="oColumnRectangle">
    /// Rectangle to draw the results within.
    /// </param>
    ///
    /// <param name="iResultsLeft">
    /// Left x-coordinate where the results should be drawn.
    /// </param>
    ///
    /// <param name="iResultsRight">
    /// Right x-coordinate where the results should be drawn.
    /// </param>
    ///
    /// <param name="iTop">
    /// Top y-coordinate where the results should be drawn.  Gets updated.
    /// </param>
    //*************************************************************************

    protected void
    DrawRampResults
    (
        DrawingObjects oDrawingObjects,
        String sSourceColumnName,
        String sCaption,
        Double dSourceCalculationNumber1,
        Double dSourceCalculationNumber2,
        Int32 iDecimalPlaces,
        Rectangle oColumnRectangle,
        Int32 iResultsLeft,
        Int32 iResultsRight,
        ref Int32 iTop
    )
    {
        Debug.Assert(oDrawingObjects != null);
        Debug.Assert( !String.IsNullOrEmpty(sSourceColumnName) );
        Debug.Assert( !String.IsNullOrEmpty(sCaption) );
        Debug.Assert(iDecimalPlaces >= 0);
        AssertValid();

        iTop += oDrawingObjects.GetFontHeightMultiple(0.2F);

        DrawRangeText(oDrawingObjects,
            DoubleToString(dSourceCalculationNumber1, iDecimalPlaces),
            DoubleToString(dSourceCalculationNumber2, iDecimalPlaces),
            SystemBrushes.WindowText, iResultsLeft, iResultsRight, ref iTop);

        iTop += oDrawingObjects.GetFontHeightMultiple(0.35F);

        Int32 iRampMargin = oDrawingObjects.GetFontHeightMultiple(0.1F);

        Graphics oGraphics = oDrawingObjects.Graphics;
        SmoothingMode eOldSmoothingMode = oGraphics.SmoothingMode;
        oGraphics.SmoothingMode = SmoothingMode.HighQuality;

        oGraphics.FillPolygon(SystemBrushes.ControlDarkDark, new Point[] {
            new Point(iResultsLeft, iTop + RampHeight - 2),
            new Point(iResultsLeft, iTop + RampHeight - 1),
            new Point(iResultsRight - iRampMargin, iTop + RampHeight - 1),
            new Point(iResultsRight - iRampMargin, iTop - 1)
            } );

        oGraphics.SmoothingMode = eOldSmoothingMode;

        iTop += RampHeight + oDrawingObjects.GetFontHeightMultiple(0.05F);

        DrawExcelColumnNameAndCaption(oDrawingObjects, sSourceColumnName,
            sCaption, iResultsLeft, iResultsRight, ref iTop);

        // Draw a line separating these results from the next.

        DrawHorizontalSeparator(oDrawingObjects, oColumnRectangle, ref iTop);
    }

    //*************************************************************************
    //  Method: DrawExcelColumnNameAndCaption()
    //
    /// <summary>
    /// Draws the name of an Excel column and a caption.
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
    /// <param name="sCaption">
    /// Caption to draw.
    /// </param>
    ///
    /// <param name="iLeft">
    /// Left x-coordinate where the column name should be drawn.
    /// </param>
    ///
    /// <param name="iRight">
    /// Right x-coordinate where the caption should be drawn.
    /// </param>
    ///
    /// <param name="iTop">
    /// Top y-coordinate where the column name and caption should be drawn.
    /// Gets updated.
    /// </param>
    //*************************************************************************

    protected void
    DrawExcelColumnNameAndCaption
    (
        DrawingObjects oDrawingObjects,
        String sColumnName,
        String sCaption,
        Int32 iLeft,
        Int32 iRight,
        ref Int32 iTop
    )
    {
        Debug.Assert(oDrawingObjects != null);
        Debug.Assert( !String.IsNullOrEmpty(sColumnName) );
        Debug.Assert( !String.IsNullOrEmpty(sCaption) );

        oDrawingObjects.Graphics.DrawString(sCaption, oDrawingObjects.Font,
            SystemBrushes.GrayText, iRight,
            iTop + oDrawingObjects.GetFontHeightMultiple(0.2F),
            oDrawingObjects.RightAlignStringFormat);

        DrawExcelColumnName(oDrawingObjects, sColumnName, iLeft,
            iRight - MeasureTextWidth(oDrawingObjects, sCaption), ref iTop);
    }

    //*************************************************************************
    //  Method: DrawVertexCategoryShape()
    //
    /// <summary>
    /// Draws the shape for a vertex category.
    /// </summary>
    ///
    /// <param name="oDrawingObjects">
    /// Objects to draw with.
    /// </param>
    ///
    /// <param name="eVertexShape">
    /// The shape to draw.
    /// </param>
    ///
    /// <param name="oPen">
    /// The pen to draw with.
    /// </param>
    ///
    /// <param name="oBrush">
    /// The brush to draw with.
    /// </param>
    ///
    /// <param name="oColor">
    /// The color to use.
    /// </param>
    ///
    /// <param name="fXShapeCenter">
    /// x-coordinate of the shape's center.
    /// </param>
    ///
    /// <param name="fYShapeCenter">
    /// y-coordinate of the shape's center.
    /// </param>
    ///
    /// <param name="fShapeHalfWidth">
    /// One half the width of the shape.
    /// </param>
    //*************************************************************************

    protected void
    DrawVertexCategoryShape
    (
        DrawingObjects oDrawingObjects,
        VertexShape eVertexShape,
        Pen oPen,
        SolidBrush oBrush,
        Color oColor,
        Single fXShapeCenter,
        Single fYShapeCenter,
        Single fShapeHalfWidth
    )
    {
        Debug.Assert(oDrawingObjects != null);
        Debug.Assert(oPen != null);
        Debug.Assert(oBrush != null);
        Debug.Assert(fShapeHalfWidth >= 0);
        AssertValid();

        Graphics oGraphics = oDrawingObjects.Graphics;
        oPen.Color = oColor;
        oBrush.Color = oColor;

        switch (eVertexShape)
        {
            case VertexShape.Circle:

                GraphicsUtil.DrawCircle(oGraphics, oPen, fXShapeCenter,
                    fYShapeCenter, fShapeHalfWidth);

                break;

            case VertexShape.Disk:

                GraphicsUtil.FillCircle(oGraphics, oBrush, fXShapeCenter,
                    fYShapeCenter, fShapeHalfWidth);

                break;

            case VertexShape.Sphere:

                GraphicsUtil.FillCircle3D(oGraphics, oColor,
                    fXShapeCenter, fYShapeCenter, fShapeHalfWidth);

                break;

            case VertexShape.Square:

                GraphicsUtil.DrawSquare(oGraphics, oPen, fXShapeCenter,
                    fYShapeCenter, fShapeHalfWidth);

                break;

            case VertexShape.SolidSquare:

                GraphicsUtil.FillSquare(oGraphics, oBrush, fXShapeCenter,
                    fYShapeCenter, fShapeHalfWidth);

                break;

            case VertexShape.Diamond:

                GraphicsUtil.DrawDiamond(oGraphics, oPen, fXShapeCenter,
                    fYShapeCenter, fShapeHalfWidth);

                break;

            case VertexShape.SolidDiamond:

                GraphicsUtil.FillDiamond(oGraphics, oBrush, fXShapeCenter,
                    fYShapeCenter, fShapeHalfWidth);

                break;

            case VertexShape.Triangle:

                GraphicsUtil.DrawTriangle(oGraphics, oPen, fXShapeCenter,
                    fYShapeCenter, fShapeHalfWidth);

                break;

            case VertexShape.SolidTriangle:

                GraphicsUtil.FillTriangle(oGraphics, oBrush, fXShapeCenter,
                    fYShapeCenter, fShapeHalfWidth);

                break;

            default:

                Debug.Assert(false);
                break;
        }
    }

    //*************************************************************************
    //  Method: DoubleToString()
    //
    /// <summary>
    /// Converts a double to a string suitable for use within the control.
    /// </summary>
    ///
    /// <param name="dDouble">
    /// The number to convert.
    /// </param>
    ///
    /// <param name="iDecimalPlaces">
    /// The number of decimal places to use.
    /// </param>
    ///
    /// <returns>
    /// <paramref name="dDouble" /> converted to a string.
    /// </returns>
    //*************************************************************************

    protected String
    DoubleToString
    (
        Double dDouble,
        Int32 iDecimalPlaces
    )
    {
        AssertValid();
        Debug.Assert(iDecimalPlaces >= 0);

        return ( dDouble.ToString( "f" + iDecimalPlaces.ToString() ) );
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

        Debug.Assert(m_oAutoFillWorkbookResults != null);
        // m_iLastResizeWidth
    }


    //*************************************************************************
    //  Protected string constants
    //*************************************************************************

    /// Caption for edge and vertex color results.

    protected const String ColorCaption = "Color";

    /// Caption for edge width results.

    protected const String EdgeWidthCaption = "Width";

    /// Column header for edge properties.

    protected const String EdgePropertyHeader = "Edge Properties";

    /// Column header for vertex properties.

    protected const String VertexPropertyHeader = "Vertex Properties";


    //*************************************************************************
    //  Protected dimension constants
    //*************************************************************************

    /// Height of the color bar drawn by DrawColorBarResults().

    protected const Int32 ColorBarHeight = 10;

    /// Height of the color bar drawn by DrawRampResults().

    protected const Int32 RampHeight = 10;

    /// Width of the shapes used to draw categories, as a multiple of the font
    /// height.

    protected const Single CategoryShapeFactor = 1.2F;

    /// Maximum categories to draw.

    protected const Int32 MaximumCategoriesToDraw = 96;

    /// Maximum number of characters to use for a category name.

    protected const Int32 MaximumCategoryNameLength = 10;


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// The results of the autofill.

    protected AutoFillWorkbookResults m_oAutoFillWorkbookResults;

    /// Width of the control during the last Resize event.

    protected Int32 m_iLastResizeWidth;
}
}
