
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Collections.Generic;
using System.Diagnostics;
using Smrf.NodeXL.Layouts;
using Smrf.NodeXL.Visualization.Wpf;
using Smrf.WpfGraphicsLib;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: GraphImageCompositor
//
/// <summary>
/// Composites a NodeXLControl and other elements into a UIElement.
/// </summary>
///
/// <remarks>
/// Call the <see cref="Composite" /> method to composite a NodeXLControl with
/// other elements into a new UIElement.  Call <see
/// cref="RestoreNodeXLControl" /> when you are done with the UIElement.  The
/// UIElement can be used to create an image that can be saved to disk.
/// </remarks>
//*****************************************************************************

public class GraphImageCompositor : Object
{
    //*************************************************************************
    //  Constructor: GraphImageCompositor()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="GraphImageCompositor" />
    /// class.
    /// </summary>
    ///
    /// <param name="nodeXLControl">
    /// The NodeXLControl that will be composited by <see cref="Composite" />.
    /// This is assumed to be hosted in a Panel.
    /// </param>
    //*************************************************************************

    public GraphImageCompositor
    (
        NodeXLControl nodeXLControl
    )
    {
        m_oNodeXLControl = nodeXLControl;
        m_oLayoutSaver = null;
        m_oGraphImageScaler = null;
        m_oGraphImageCenterer = null;
        m_oParentPanel = null;
        m_iChildIndex = Int32.MinValue;
        m_oGrid = null;

        AssertValid();
    }

    //*************************************************************************
    //  Method: Composite()
    //
    /// <summary>
    /// Composites a NodeXLControl and other elements into a new UIElement.
    /// </summary>
    ///
    /// <param name="compositeHeight">
    /// Height of the returned composite UIElement, in WPS units.
    /// </param>
    ///
    /// <param name="compositeWidth">
    /// Width of the returned composite UIElement, in WPS units.
    /// </param>
    ///
    /// <param name="headerText">
    /// Optional header text.  If null, no header is included.  If
    /// String.Empty, an empty header is included.
    /// </param>
    ///
    /// <param name="footerText">
    /// Optional footer text.  If null, no footer is included.  If
    /// String.Empty, an empty footer is included.
    /// </param>
    ///
    /// <param name="headerFooterFont">
    /// The font to use for the header and footer text.  Can't be null.
    /// </param>
    ///
    /// <param name="legendControls">
    /// One or more legend controls to include.  Can't be null.
    /// </param>
    ///
    /// <returns>
    /// A new UIElement containing the NodeXL control and other optional
    /// elements.
    /// </returns>
    ///
    /// <remarks>
    /// This method composites the NodeXLControl into a new UIElement that also
    /// contains an optional header, legend, and footer.  The new UIElement,
    /// whose lifetime is assumed to be short, can be used to save an image of
    /// the graph.
    ///
    /// <para>
    /// Because this method modifies the parent and properties of the
    /// NodeXLControl, the <see cref="RestoreNodeXLControl" /> method must be
    /// called when you are done with the UIElement.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public UIElement
    Composite
    (
        Double compositeWidth,
        Double compositeHeight,
        String headerText,
        String footerText,
        System.Drawing.Font headerFooterFont,
        IEnumerable<LegendControlBase> legendControls
    )
    {
        Debug.Assert(compositeWidth > 0);
        Debug.Assert(compositeHeight > 0);
        Debug.Assert(headerFooterFont != null);
        Debug.Assert(legendControls != null);
        AssertValid();

        // Note:
        //
        // Don't try taking a shortcut by using
        // NodeXLControl.CopyGraphToBitmap() to get an image of the graph and
        // then compositing the image with the other elements.  That would work
        // if the caller were creating an image, but if it were creating an XPS
        // document, the graph would no longer be scalable.

        Double dScreenDpi =
            WpfGraphicsUtil.GetScreenDpi(m_oNodeXLControl).Width;

        // The NodeXLControl can't be a child of two logical trees, so
        // disconnect it from its parent after saving the current vertex
        // locations.

        m_oLayoutSaver = new LayoutSaver(m_oNodeXLControl.Graph);

        Debug.Assert(m_oNodeXLControl.Parent is Panel);
        m_oParentPanel = (Panel)m_oNodeXLControl.Parent;
        UIElementCollection oParentChildren = m_oParentPanel.Children;
        m_iChildIndex = oParentChildren.IndexOf(m_oNodeXLControl);
        oParentChildren.Remove(m_oNodeXLControl);

        m_oGraphImageScaler = new GraphImageScaler(m_oNodeXLControl);

        m_oGraphImageCenterer = new NodeXLControl.GraphImageCenterer(
            m_oNodeXLControl);

        // The header and footer are rendered as Label controls.  The legend is
        // rendered as a set of Image controls.

        Label oHeaderLabel, oFooterLabel;
        IEnumerable<Image> oLegendImages;
        Double dHeaderHeight, dTotalLegendHeight, dFooterHeight;

        CreateHeaderOrFooterLabel(headerText, compositeWidth, headerFooterFont,
            out oHeaderLabel, out dHeaderHeight);

        CreateLegendImages(legendControls, compositeWidth, out oLegendImages,
            out dTotalLegendHeight);

        CreateHeaderOrFooterLabel(footerText, compositeWidth, headerFooterFont,
            out oFooterLabel, out dFooterHeight);

        m_oNodeXLControl.Width = compositeWidth;

        m_oNodeXLControl.Height = Math.Max(10,
            compositeHeight - dHeaderHeight - dTotalLegendHeight
            - dFooterHeight);

        // Adjust the control's graph scale so that the graph's vertices and
        // edges will be the same relative size in the composite that they are
        // in the control.

        m_oGraphImageScaler.SetGraphScale(
            (Int32)WpfGraphicsUtil.WpfToPx(compositeWidth, dScreenDpi),
            (Int32)WpfGraphicsUtil.WpfToPx(m_oNodeXLControl.Height, dScreenDpi),
            dScreenDpi);

        // Adjust the NodeXLControl's translate transforms so that the
        // composite will be centered on the same point on the graph that the
        // NodeXLControl is centered on.

        m_oGraphImageCenterer.CenterGraphImage( new Size(compositeWidth,
            m_oNodeXLControl.Height) );

        StackPanel oStackPanel = new StackPanel();
        UIElementCollection oStackPanelChildren = oStackPanel.Children;

        // To avoid a solid black line at the bottom of the header or the top
        // of the footer, which is caused by rounding errors, make the
        // StackPanel background color the same as the header and footer.

        oStackPanel.Background = HeaderFooterBackgroundBrush;

        if (oHeaderLabel != null)
        {
            oStackPanelChildren.Add(oHeaderLabel);
        }

        // Wrap the NodeXLControl in a Grid to clip it.

        m_oGrid = new Grid();
        m_oGrid.Width = m_oNodeXLControl.Width;
        m_oGrid.Height = m_oNodeXLControl.Height;
        m_oGrid.ClipToBounds = true;
        m_oGrid.Children.Add(m_oNodeXLControl);

        oStackPanelChildren.Add(m_oGrid);

        foreach (Image oLegendImage in oLegendImages)
        {
            oStackPanelChildren.Add(oLegendImage);
        }

        if (oFooterLabel != null)
        {
            oStackPanelChildren.Add(oFooterLabel);
        }

        Size oCompositeSize = new Size(compositeWidth, compositeHeight);
        Rect oCompositeRectangle = new Rect(new Point(), oCompositeSize);

        oStackPanel.Measure(oCompositeSize);
        oStackPanel.Arrange(oCompositeRectangle);
        oStackPanel.UpdateLayout();

        return (oStackPanel);
    }

    //*************************************************************************
    //  Method: RestoreNodeXLControl()
    //
    /// <summary>
    /// Restores the NodeXLControl to the state it was in before <see
    /// cref="Composite" /> was called.
    /// </summary>
    ///
    /// <remarks>
    /// This method must be called when you are done with the UIElement that
    /// was returned by <see cref="Composite" />.
    /// </remarks>
    //*************************************************************************

    public void
    RestoreNodeXLControl()
    {
        AssertValid();

        // Reconnect the NodeXLControl to its original parent and reset the
        // size to Auto.

        m_oGrid.Children.Remove(m_oNodeXLControl);
        m_oNodeXLControl.Width = Double.NaN;
        m_oNodeXLControl.Height = Double.NaN;

        m_oGraphImageCenterer.RestoreCenter();
        m_oGraphImageScaler.RestoreGraphScale();

        m_oParentPanel.Children.Insert(m_iChildIndex, m_oNodeXLControl);

        // The graph may have shrunk, and even though it will be expanded to
        // its original dimensions when UpdateLayout() is called below, the
        // layout may have lost "resolution" and the results may be poor.
        //
        // Fix this by restoring the original layout and redrawing the graph.

        m_oNodeXLControl.UpdateLayout();
        m_oLayoutSaver.RestoreLayout();
        m_oNodeXLControl.DrawGraph(false);
    }

    //*************************************************************************
    //  Method: CreateHeaderOrFooterLabel()
    //
    /// <summary>
    /// Creates a Label control to use for a header or footer.
    /// </summary>
    ///
    /// <param name="sLabelText">
    /// The Label text.  Can be null.
    /// </param>
    ///
    /// <param name="dLabelWidth">
    /// Width of the Label control, in WPS units.
    /// </param>
    ///
    /// <param name="oLabelFont">
    /// The Font to use for the Label control.
    /// </param>
    ///
    /// <param name="oLabel">
    /// Where the new Label control gets stored.  If <paramref
    /// name="sLabelText" /> is null, this gets set to null.
    /// </param>
    ///
    /// <param name="dLabelHeight">
    /// Where the height of the new Label control gets stored.  If <paramref
    /// name="sLabelText" /> is null, this gets set to 0.
    /// </param>
    //*************************************************************************

    protected void
    CreateHeaderOrFooterLabel
    (
        String sLabelText,
        Double dLabelWidth,
        System.Drawing.Font oLabelFont,
        out Label oLabel,
        out Double dLabelHeight
    )
    {
        Debug.Assert(dLabelWidth > 0);
        Debug.Assert(oLabelFont != null);
        AssertValid();

        if (sLabelText == null)
        {
            oLabel = null;
            dLabelHeight = 0;
            return;
        }

        // Use a TextBlock to provide wrapping.

        TextBlock oTextBlock = new TextBlock();
        oTextBlock.Text = sLabelText;
        oTextBlock.TextWrapping = TextWrapping.Wrap;
        WpfGraphicsUtil.SetTextBlockFont(oTextBlock, oLabelFont);

        oLabel = new Label();
        oLabel.Background = HeaderFooterBackgroundBrush;
        oLabel.MaxWidth = dLabelWidth;
        oLabel.Content = oTextBlock;

        // Determine the height of the control.  This will likely be a floating
        // point number.

        oLabel.Measure( new Size(dLabelWidth, Double.PositiveInfinity) );

        // The label height must be an integer.  If it's not an integer, any
        // Image controls contained in the parent StackPanel will be distorted
        // because they don't fall on a pixel boundary.

        oLabel.Height = Math.Ceiling(oLabel.DesiredSize.Height);
        oLabel.Measure( new Size(dLabelWidth, Double.PositiveInfinity) );

        dLabelHeight = oLabel.DesiredSize.Height;
    }

    //*************************************************************************
    //  Method: CreateLegendImages()
    //
    /// <summary>
    /// Creates an Image control to use for each of the graph's legend
    /// controls.
    /// </summary>
    ///
    /// <param name="oLegendControls">
    /// Zero or more legend controls to include.  Can't be null.
    /// </param>
    ///
    /// <param name="dImageWidth">
    /// Width of the Images, in WPS units.
    /// </param>
    ///
    /// <param name="oImages">
    /// Where the new Image controls gets stored.  If there are no visible
    /// legend controls, this enumeration is empty.
    /// </param>
    ///
    /// <param name="dTotalImageHeight">
    /// Where the total height of all the new Image controls gets stored.  If
    /// there are no visible legend controls, this gets set to 0.
    /// </param>
    //*************************************************************************

    protected void
    CreateLegendImages
    (
        IEnumerable<LegendControlBase> oLegendControls,
        Double dImageWidth,
        out IEnumerable<Image> oImages,
        out Double dTotalImageHeight
    )
    {
        Debug.Assert(oLegendControls != null);
        Debug.Assert(dImageWidth > 0);
        AssertValid();

        List<Image> oLegendImages = new List<Image>();
        oImages = oLegendImages;
        dTotalImageHeight = 0;

        foreach (LegendControlBase oLegendControl in oLegendControls)
        {
            if (oLegendControl.Visible && oLegendControl.Height > 0)
            {
                System.Windows.Media.Imaging.BitmapSource oBitmapSource;

                using ( System.Drawing.Bitmap oBitmap =
                    oLegendControl.DrawOnBitmap(
                        (Int32)Math.Ceiling(dImageWidth) ) )
                {
                    oBitmapSource = WpfGraphicsUtil.BitmapToBitmapSource(
                        oBitmap);
                }

                Image oLegendImage = new Image();
                oLegendImage.Source = oBitmapSource;
                oLegendImage.Width = dImageWidth;

                oLegendImage.Measure(new Size(
                    dImageWidth, Double.PositiveInfinity) );

                oLegendImages.Add(oLegendImage);
                dTotalImageHeight += oLegendImage.DesiredSize.Height;
            }
        }
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
        // m_oLayoutSaver
        // m_oGraphImageScaler
        // m_oGraphImageCenterer
        // m_oParentPanel
        // m_iChildIndex;
        // m_oGrid
    }


    //*************************************************************************
    //  Private constants
    //*************************************************************************

    /// Header and footer background color.

    private static SolidColorBrush HeaderFooterBackgroundBrush = Brushes.White;


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// NodeXLControl being composited.

    protected NodeXLControl m_oNodeXLControl;

    /// Object used to save the layout, or null if Composite() hasn't been
    /// called yet.

    protected LayoutSaver m_oLayoutSaver;

    /// Object used to scale the graph's vertices and edges, or null if
    /// Composite() hasn't been called yet.

    protected GraphImageScaler m_oGraphImageScaler;

    /// Object used to center the graph image, or null if Composite() hasn't
    /// been called yet.

    protected NodeXLControl.GraphImageCenterer m_oGraphImageCenterer;

    /// The NodeXLControl's original parent, or null if Composite() hasn't been
    /// called yet.

    protected Panel m_oParentPanel;

    /// The NodeXLControl's original index within its parent's child
    /// collection, or Int32.MinValue if Composite() hasn't been called yet.

    protected Int32 m_iChildIndex;

    /// Grid used to clip the NodeXLControl, or null if Composite() hasn't been
    /// called yet.

    protected Grid m_oGrid;
}

}
