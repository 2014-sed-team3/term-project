
using System;
using System.Diagnostics;
using Smrf.WpfGraphicsLib;

namespace Smrf.NodeXL.Visualization.Wpf
{
//*****************************************************************************
//  Class: GraphImageScaler
//
/// <summary>
/// Sets the graph scale on a <see cref="NodeXLControl" /> and later restores
/// the original graph scale.
/// </summary>
///
/// <remarks>
/// This prepares the <see cref="NodeXLControl" /> for being saved as an image.
/// It adjusts the control's graph scale so that the graph's vertices and edges
/// will be the same relative size in the image that they are in the control.
///
/// <para>
/// Call <see cref="SetGraphScale" /> before saving the image, then call <see
/// cref="RestoreGraphScale" /> to restore the original graph scale.
/// </para>
///
/// </remarks>
//*****************************************************************************

public class GraphImageScaler : Object
{
    //*************************************************************************
    //  Constructor: GraphImageScaler()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="GraphImageScaler" />
    /// class.
    /// </summary>
    ///
    /// <param name="nodeXLControl">
    /// The control for which a graph image will be created.
    /// </param>
    //*************************************************************************

    public GraphImageScaler
    (
        NodeXLControl nodeXLControl
    )
    {
        Debug.Assert(nodeXLControl != null);

        m_oNodeXLControl = nodeXLControl;
        m_dOriginalGraphScale = 0;
        m_dOriginalGroupLabelScale = 0;

        AssertValid();
    }

    //*************************************************************************
    //  Method: SetGraphScale()
    //
    /// <summary>
    /// Adjusts the graph scale on a <see cref="NodeXLControl" /> so that the
    /// graph's vertices and edges will be the same relative size in an image
    /// that they are in the control.
    /// </summary>
    ///
    /// <param name="bitmapWidthPx">
    /// Width of the bitmap image, in pixels.  Must be greater than 0.
    /// </param>
    ///
    /// <param name="bitmapHeightPx">
    /// Height of the bitmap image, in pixels.  Must be greater than 0.
    /// </param>
    ///
    /// <param name="screenDpi">
    /// The DPI of the screen.  Sample: 96.0.
    /// </param>
    //*************************************************************************

    public void
    SetGraphScale
    (
        Int32 bitmapWidthPx,
        Int32 bitmapHeightPx,
        Double screenDpi
    )
    {
        Debug.Assert(screenDpi > 0);
        Debug.Assert(bitmapWidthPx > 0);
        Debug.Assert(bitmapHeightPx > 0);
        AssertValid();

        GraphDrawer oGraphDrawer = m_oNodeXLControl.GraphDrawer;
        GroupDrawer oGroupDrawer = oGraphDrawer.GroupDrawer;

        // Note that group labels have their own scale, distinct from the scale
        // used by the other graph elements.

        m_dOriginalGraphScale = oGraphDrawer.GraphScale;
        m_dOriginalGroupLabelScale = oGroupDrawer.LabelScale;

        // Get the actual control size, in units of 1/96".

        Double dActualWidthWpf = m_oNodeXLControl.ActualWidth;
        Double dActualHeightWpf = m_oNodeXLControl.ActualHeight;

        // Convert this to pixels.

        Double dActualWidthPx = WpfGraphicsUtil.WpfToPx(
            dActualWidthWpf, screenDpi);

        Double dActualHeightPx = WpfGraphicsUtil.WpfToPx(
            dActualHeightWpf, screenDpi);

        // If the image is smaller (or larger) than the control, adjust the
        // GraphScale property to shrink (or enlarge) the graph's vertices and
        // edges accordingly.  Use the smaller of the two bitmap/actual ratio.

        Double dGraphScaleFactor = Math.Min(
            bitmapWidthPx / dActualWidthPx,
            bitmapHeightPx / dActualHeightPx
            );

        Double dGraphScale =
            PinScale(m_dOriginalGraphScale * dGraphScaleFactor);

        Double dGroupLabelScale =
            PinScale(m_dOriginalGroupLabelScale * dGraphScaleFactor);

        // Don't set the NodeXLControl.GraphScale property, which would cause
        // the NodeXLControl.GraphScaleChanged event to fire.  Set the property
        // on the GraphDrawer instead.

        oGraphDrawer.GraphScale = dGraphScale;

        oGroupDrawer.LabelScale = dGroupLabelScale;

        AssertValid();
    }

    //*************************************************************************
    //  Method: RestoreGraphScale()
    //
    /// <summary>
    /// Restores the control's original <see
    /// cref="NodeXLControl.GraphScale" />.
    /// </summary>
    ///
    /// <remarks>
    /// If <see cref="SetGraphScale" /> hasn't been called, this method does
    /// nothing.
    /// </remarks>
    //*************************************************************************

    public void
    RestoreGraphScale()
    {
        AssertValid();

        GraphDrawer oGraphDrawer = m_oNodeXLControl.GraphDrawer;

        if (m_dOriginalGraphScale != 0)
        {
            oGraphDrawer.GraphScale = m_dOriginalGraphScale;
        }

        if (m_dOriginalGroupLabelScale != 0)
        {
            oGraphDrawer.GroupDrawer.LabelScale = m_dOriginalGroupLabelScale;
        }
    }

    //*************************************************************************
    //  Method: PinScale()
    //
    /// <summary>
    /// Pins a scale value to an allowable range.
    /// </summary>
    ///
    /// <param name="dScale">
    /// The scale value to pin.
    /// </param>
    ///
    /// <returns>
    /// The pinned scale value.
    /// </returns>
    //*************************************************************************

    protected Double
    PinScale
    (
        Double dScale
    )
    {
        AssertValid();

        dScale = Math.Max(dScale, NodeXLControl.MinimumGraphScale);
        dScale = Math.Min(dScale, NodeXLControl.MaximumGraphScale);

        return (dScale);
    }


    //*************************************************************************
    //  Method: AssertValid()
    //
    /// <summary>
    /// Asserts if the object is in an invalid state.  Debug-only.
    /// </summary>
    //*************************************************************************

    [Conditional("DEBUG")]

    public void
    AssertValid()
    {
        Debug.Assert(m_oNodeXLControl != null);
        // m_dOriginalGraphScale
        // m_dOriginalGroupLabelScale
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// The control for which a graph image will be created.

    protected NodeXLControl m_oNodeXLControl;

    /// Original GraphDrawer.GraphScale value, set by SetGraphScale().

    protected Double m_dOriginalGraphScale;

    /// Original GroupDrawer.LabelScale value, set by SetGraphScale().

    protected Double m_dOriginalGroupLabelScale;
}

}
