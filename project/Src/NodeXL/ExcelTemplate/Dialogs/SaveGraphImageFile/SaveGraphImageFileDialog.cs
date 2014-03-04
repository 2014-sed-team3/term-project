
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Forms;
using System.Windows.Documents;
using System.Windows.Xps;
using System.Windows.Xps.Packaging;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using Smrf.NodeXL.Visualization.Wpf;
using Smrf.AppLib;
using Smrf.WpfGraphicsLib;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: SaveGraphImageFileDialog
//
/// <summary>
/// Represents a dialog box for saving an image of a graph to a file.
/// </summary>
///
/// <remarks>
/// Call ShowDialogAndSaveImage() to allow the user to save an image in a
/// format of his choice to a location of his choice.
///
/// <para>
/// This class extends the <see cref="SaveImageFileDialog" /> base class by
/// adding an option to save the graph to an XPS file.
/// </para>
///
/// </remarks>
//*****************************************************************************

public class SaveGraphImageFileDialog : SaveImageFileDialog
{
    //*************************************************************************
    //  Constructor: SaveGraphImageFileDialog()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="SaveGraphImageFileDialog" /> class.
    /// </summary>
    ///
    /// <param name="intialDirectory">
    /// Initial directory the dialog will display.  Use an empty string to let
    /// the dialog select an initial directory.
    /// </param>
    ///
    /// <param name="intialFileName">
    /// Initial file name.  Can be a complete path, a path without an
    /// extension, a file name, or a file name without an extension.
    /// </param>
    //*************************************************************************

    public SaveGraphImageFileDialog
    (
        String intialDirectory,
        String intialFileName

    ) : base(intialDirectory, intialFileName)
    {
        // (Do nothing.)

        AssertValid();
    }

    //*************************************************************************
    //  Method: ShowDialogAndSaveGraphImage()
    //
    /// <summary>
    /// Shows the file save dialog and saves the image to the selected file.
    /// </summary>
    ///
    /// <param name="nodeXLControl">
    /// The control the image will come from.
    /// </param>
    ///
    /// <param name="width">
    /// Width of the image to save.  If saving to XPS, the units are 1/100 of
    /// an inch.  Otherwise, the units are pixels.
    /// </param>
    ///
    /// <param name="height">
    /// Height of the image to save.  If saving to XPS, the units are 1/100 of
    /// an inch.  Otherwise, the units are pixels.  If a header or footer is
    /// included, the saved image is the specified height but the graph is
    /// shortened to accommodate the header or footer.
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
    /// The font to use for the header and footer text.
    /// </param>
    ///
    /// <param name="legendControls">
    /// Zero or more legend controls to include.  Can't be null.
    /// </param>
    ///
    /// <returns>
    /// DialogResult.OK if the user selected a file name and the image was
    /// successfully saved.
    /// </returns>
    ///
    /// <remarks>
    /// This method allows the user to select an image file name and format.
    /// It then saves the image in the selected format.
    /// </remarks>
    //*************************************************************************

    public DialogResult
    ShowDialogAndSaveGraphImage
    (
        NodeXLControl nodeXLControl,
        Int32 width,
        Int32 height,
        String headerText,
        String footerText,
        System.Drawing.Font headerFooterFont,
        IEnumerable<LegendControlBase> legendControls
    )
    {
        Debug.Assert(nodeXLControl != null);
        Debug.Assert(width > 0);
        Debug.Assert(height > 0);
        Debug.Assert(headerFooterFont != null);
        Debug.Assert(legendControls != null);
        AssertValid();

        // Let the base class do most of the work.  The actual saving will be
        // done by SaveObject() in this class.  Wrap the information required
        // by SaveObject().

        GraphImageInfo oGraphImageInfo = new GraphImageInfo();

        oGraphImageInfo.NodeXLControl = nodeXLControl;
        oGraphImageInfo.Width = width;
        oGraphImageInfo.Height = height;
        oGraphImageInfo.HeaderText = headerText;
        oGraphImageInfo.FooterText = footerText;
        oGraphImageInfo.HeaderFooterFont = headerFooterFont;
        oGraphImageInfo.LegendControls = legendControls;

        return ( ShowDialogAndSaveObject(oGraphImageInfo) );
    }

    //*************************************************************************
    //  Method: GetFilter()
    //
    /// <summary>
    /// Returns the filter to use for the dialog.
    /// </summary>
    ///
    /// <returns>
    /// Filter to use for the dialog.
    /// </returns>
    //*************************************************************************

    protected override String
    GetFilter()
    {
        // Extend the base class file types to include XPS.

        return (SaveableImageFormats.Filter + "|XPS (*.xps)|*.xps" );
    }

    //*************************************************************************
    //  Method: SaveObject()
    //
    /// <summary>
    /// Saves the object to the specified file.
    /// </summary>
    ///
    /// <param name="oObject">
    /// Object to save.
    /// </param>
    ///
    /// <param name="sFileName">
    /// File name to save the object to.
    /// </param>
    ///
    /// <remarks>
    /// This is called by the base-class ShowDialogAndSaveObject() method.
    /// </remarks>
    //*************************************************************************

    protected override void
    SaveObject
    (
        Object oObject,
        String sFileName
    )
    {
        Debug.Assert(oObject is GraphImageInfo);
        Debug.Assert( !String.IsNullOrEmpty(sFileName) );

        GraphImageInfo oGraphImageInfo = (GraphImageInfo)oObject;
        Int32 iWidth = oGraphImageInfo.Width;
        Int32 iHeight = oGraphImageInfo.Height;

        GraphImageCompositor oGraphImageCompositor =
            new GraphImageCompositor(oGraphImageInfo.NodeXLControl);

        UIElement oCompositeElement = null;

        try
        {
            if (m_oSaveFileDialog.FilterIndex <=
                SaveableImageFormats.ImageFormats.Length)
            {
                // The graph must be saved as a bitmap.

                oCompositeElement = oGraphImageCompositor.Composite(
                    iWidth, iHeight, oGraphImageInfo.HeaderText,
                    oGraphImageInfo.FooterText,
                    oGraphImageInfo.HeaderFooterFont,
                    oGraphImageInfo.LegendControls);

                System.Drawing.Bitmap oBitmap = WpfGraphicsUtil.VisualToBitmap(
                    oCompositeElement, iWidth, iHeight);

                base.SaveObject(oBitmap, sFileName);
                oBitmap.Dispose();
            }
            else
            {
                // The graph must be saved as an XPS.

                Double documentWidth = ToWpsUnits(iWidth);
                Double documentHeight = ToWpsUnits(iHeight);

                oCompositeElement = oGraphImageCompositor.Composite(
                    documentWidth, documentHeight, oGraphImageInfo.HeaderText,
                    oGraphImageInfo.FooterText,
                    oGraphImageInfo.HeaderFooterFont,
                    oGraphImageInfo.LegendControls);

                Size oDocumentSize = new Size(documentWidth, documentHeight);
                Rect oDocumentRectangle = new Rect(new Point(), oDocumentSize);

                FixedDocument oFixedDocument = new FixedDocument();
                oFixedDocument.DocumentPaginator.PageSize = oDocumentSize;
                PageContent oPageContent = new PageContent();

                FixedPage oFixedPage = new FixedPage();
                oFixedPage.Width = documentWidth;
                oFixedPage.Height = documentHeight;

                oFixedPage.Children.Add(oCompositeElement);

                ( (System.Windows.Markup.IAddChild)oPageContent ).AddChild(
                    oFixedPage);

                oFixedDocument.Pages.Add(oPageContent);

                XpsDocument oXpsDocument = new XpsDocument(sFileName,
                    FileAccess.Write);

                XpsDocumentWriter oXpsDocumentWriter =
                    XpsDocument.CreateXpsDocumentWriter(oXpsDocument);

                oXpsDocumentWriter.Write(oFixedDocument);
                oXpsDocument.Close();
            }
        }
        finally
        {
            if (oCompositeElement != null)
            {
                oGraphImageCompositor.RestoreNodeXLControl();
            }
        }
    }

    //*************************************************************************
    //  Method: ToWpsUnits()
    //
    /// <summary>
    /// Converts a height or width to WPS units.
    /// </summary>
    ///
    /// <param name="iHeightOrWidth">
    /// Height or width, in 1/100 of an inch.
    /// </param>
    ///
    /// <returns>
    /// Height or width, in WPS units (1/96 of an inch).
    /// </returns>
    //*************************************************************************

    protected Double
    ToWpsUnits
    (
        Int32 iHeightOrWidth
    )
    {
        AssertValid();

        return ( iHeightOrWidth * (96.0 / 100.0) );
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

        // (Do nothing.)
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)


    //*************************************************************************
    //  Embedded class: GraphImageInfo
    //
    /// <summary>
    /// Stores information about the graph image that needs to be saved.
    /// </summary>
    //*************************************************************************

    protected class GraphImageInfo : Object
    {
        ///
        public NodeXLControl NodeXLControl;

        ///
        public Int32 Width;

        ///
        public Int32 Height;

        ///
        public String HeaderText;

        ///
        public String FooterText;

        ///
        public System.Drawing.Font HeaderFooterFont;

        ///
        public IEnumerable<LegendControlBase> LegendControls;
    }
}

}
