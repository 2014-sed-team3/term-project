
using System;
using System.Windows;
using System.Windows.Media;
using System.Diagnostics;
using Smrf.AppLib;
using Smrf.WpfGraphicsLib;

namespace Smrf.NodeXL.Visualization.Wpf
{
//*****************************************************************************
//  Class: GraphDrawingContext
//
/// <summary>
/// Provides access to objects needed for graph-drawing operations.
/// </summary>
//*****************************************************************************

public class GraphDrawingContext : VisualizationBase
{
    //*************************************************************************
    //  Constructor: GraphDrawingContext()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="GraphDrawingContext" />
    /// class.
    /// </summary>
    ///
    /// <param name="graphRectangle">
    /// The <see cref="Rect" /> the graph is being drawn within.
    /// </param>
    ///
    /// <param name="margin">
    /// The margin that was used to lay out the graph.  If <paramref
    /// name="graphRectangle" /> is {L=0, T=0, R=50, B=30} and <paramref
    /// name="margin" /> is 5, for example, then the graph was laid out within
    /// the rectangle {L=5, T=5, R=45, B=25}.
    /// </param>
    ///
    /// <param name="backColor">
    /// The graph's background color.
    /// </param>
    //*************************************************************************

    public GraphDrawingContext
    (
        Rect graphRectangle,
        Int32 margin,
        Color backColor
    )
    {
        const String MethodName = "Constructor";

        ArgumentChecker oArgumentChecker = this.ArgumentChecker;

        oArgumentChecker.CheckArgumentNotNegative(
            MethodName, "margin", margin);

        m_oGraphRectangle = graphRectangle;
        m_iMargin = margin;
        m_oBackColor = backColor;

        AssertValid();
    }

    //*************************************************************************
    //  Property: GraphRectangle
    //
    /// <summary>
    /// Gets the rectangle the graph is being drawn within.
    /// </summary>
    ///
    /// <value>
    /// The rectangle the graph is being drawn within, as a <see
    /// cref="Rect" />.
    /// </value>
    //*************************************************************************

    public Rect
    GraphRectangle
    {
        get
        {
            AssertValid();

            return (m_oGraphRectangle);
        }
    }

    //*************************************************************************
    //  Property: GraphRectangleMinusMargin
    //
    /// <summary>
    /// Gets the rectangle the graph is being drawn within, reduced on all
    /// sides by the margin.
    /// </summary>
    ///
    /// <value>
    /// The rectangle the graph is being drawn within, as a <see
    /// cref="Rect" />, reduced by <see cref="Margin" />.
    /// </value>
    ///
    /// <remarks>
    /// If the graph rectangle is narrower or shorter than twice the <see
    /// cref="Margin" />, Rect.Empty is returned.
    /// </remarks>
    //*************************************************************************

    public Rect
    GraphRectangleMinusMargin
    {
        get
        {
            AssertValid();

            return ( WpfGraphicsUtil.GetRectangleMinusMargin(
                m_oGraphRectangle, m_iMargin) );
        }
    }

    //*************************************************************************
    //  Property: GraphRectangleMinusMarginIsEmpty
    //
    /// <summary>
    /// Gets a flag indicating whether the rectangle the graph is being drawn
    /// within, reduced on all sides by the margin, is empty.
    /// </summary>
    ///
    /// <value>
    /// true if the rectangle the graph is being drawn within, reduced on all
    /// sides by the margin, is empty.
    /// </value>
    //*************************************************************************

    public Boolean
    GraphRectangleMinusMarginIsEmpty
    {
        get
        {
            AssertValid();

            Rect oGraphRectangleMinusMargin = this.GraphRectangleMinusMargin;

            return (oGraphRectangleMinusMargin.Width <= 0 ||
                oGraphRectangleMinusMargin.Height <= 0);
        }
    }

    //*************************************************************************
    //  Property: Margin
    //
    /// <summary>
    /// Gets the margin the graph was laid out within.
    /// </summary>
    ///
    /// <value>
    /// The margin that was used to lay out the graph.  Always greater than or
    /// equal to zero.
    /// </value>
    ///
    /// <remarks>
    /// If <see cref="GraphRectangle" /> is {L=0, T=0, R=50, B=30} and <see
    /// cref="Margin" /> is 5, for example, then the graph was laid out within
    /// the rectangle {L=5, T=5, R=45, B=25}.
    /// </remarks>
    //*************************************************************************

    public Int32
    Margin
    {
        get
        {
            AssertValid();

            return (m_iMargin);
        }
    }

    //*************************************************************************
    //  Property: BackColor
    //
    /// <summary>
    /// Gets the graph's background color.
    /// </summary>
    ///
    /// <value>
    /// The graph's background color.
    /// </value>
    //*************************************************************************

    public Color
    BackColor
    {
        get
        {
            AssertValid();

            return (m_oBackColor);
        }
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

        // m_oGraphRectangle
        Debug.Assert(m_iMargin >= 0);
        // m_oBackColor
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Rectangle to draw within.

    protected Rect m_oGraphRectangle;

    /// Margin the graph was laid out within.

    protected Int32 m_iMargin;

    /// The graph's background color.

    protected Color m_oBackColor;
}

}
