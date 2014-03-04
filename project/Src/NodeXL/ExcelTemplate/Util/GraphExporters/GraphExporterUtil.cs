
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Diagnostics;
using Smrf.NodeXL.Core;
using Smrf.NodeXL.Adapters;
using Smrf.NodeXL.Visualization.Wpf;
using Smrf.AppLib;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: GraphExporterUtil
//
/// <summary>
/// Utility methods for exporting a graph.
/// </summary>
///
/// <remarks>
/// This static class contains methods that can be used while exporting a
/// graph.
///
/// <para>
/// Call <see cref="GetDataToExport" /> to get images of the graph, along with
/// optional workbook contents, workbook settings, and GraphML.
/// </para>
///
/// </remarks>
//*****************************************************************************

public static class GraphExporterUtil
{
    //*************************************************************************
    //  Method: GetDataToExport()
    //
    /// <summary>
    /// Gets the data to export.
    /// </summary>
    ///
    /// <param name="workbook">
    /// Workbook containing the graph data.
    /// </param>
    ///
    /// <param name="nodeXLControl">
    /// NodeXLControl containing the graph.  The control's ActualWidth and
    /// ActualHeight properties must be at least <see
    /// cref="GraphExporterUtil.MinimumNodeXLControlWidth" /> and <see 
    /// cref="GraphExporterUtil.MinimumNodeXLControlHeight" />, respectively.
    /// </param>
    ///
    /// <param name="exportWorkbookAndSettings">
    /// true if the caller will be exporting the workbook and its settings.
    /// </param>
    ///
    /// <param name="exportGraphML">
    /// true if the caller will be exporting the graph's data as GraphML.
    /// </param>
    ///
    /// <param name="zippedGraphMLFileName">
    /// Name to use for the zipped GraphML file, including an extension.
    /// Sample: "NodeXLGraph-GraphML.xml".  This must be specified regardless
    /// of the value of <paramref name="exportGraphML" />.
    /// </param>
    ///
    /// <param name="useFixedAspectRatio">
    /// true to use a fixed aspect ratio, false to use the aspect ratio of the
    /// graph pane.
    /// </param>
    ///
    /// <param name="fullSizeImage">
    /// Where the full-size PNG image gets stored, as an array of bytes.
    /// </param>
    ///
    /// <param name="thumbnail">
    /// Where the PNG thumbnail gets stored, as an array of bytes.
    /// </param>
    ///
    /// <param name="workbookContents">
    /// Where the workbook contents get stored if <paramref
    /// name="exportWorkbookAndSettings" /> is true.
    /// </param>
    ///
    /// <param name="workbookSettings">
    /// Where the workbook settings get stored if <paramref
    /// name="exportWorkbookAndSettings" /> is true.
    /// </param>
    ///
    /// <param name="graphMLZipped">
    /// Where the GraphML gets stored if <paramref name="exportGraphML" /> is
    /// true, as an array of zipped bytes.
    /// </param>
    //*************************************************************************

    public static void
    GetDataToExport
    (
        Microsoft.Office.Interop.Excel.Workbook workbook,
        NodeXLControl nodeXLControl,
        Boolean exportWorkbookAndSettings,
        Boolean exportGraphML,
        String zippedGraphMLFileName,
        Boolean useFixedAspectRatio,
        out Byte [] fullSizeImage,
        out Byte [] thumbnail,
        out Byte [] workbookContents,
        out String workbookSettings,
        out Byte [] graphMLZipped
    )
    {
        Debug.Assert(workbook != null);
        Debug.Assert(nodeXLControl != null);
        Debug.Assert(nodeXLControl.ActualWidth >= MinimumNodeXLControlWidth);
        Debug.Assert(nodeXLControl.ActualHeight >= MinimumNodeXLControlHeight);
        Debug.Assert( !String.IsNullOrEmpty(zippedGraphMLFileName) );

        GetImages(nodeXLControl, useFixedAspectRatio, out fullSizeImage,
            out thumbnail);

        if (exportWorkbookAndSettings)
        {
            workbookContents = GetWorkbookContents(workbook);
            workbookSettings = GetWorkbookSettings(workbook);
        }
        else
        {
            workbookContents = null;
            workbookSettings = null;
        }

        if (exportGraphML)
        {
            graphMLZipped = ZipGraphML( GetGraphML(workbook),
                zippedGraphMLFileName );
        }
        else
        {
            graphMLZipped = null;
        }
    }

    //*************************************************************************
    //  Method: GetImages()
    //
    /// <summary>
    /// Gets a full-size image and a thumbnail of the graph.
    /// </summary>
    ///
    /// <param name="oNodeXLControl">
    /// NodeXLControl containing the graph.
    /// </param>
    ///
    /// <param name="bUseFixedAspectRatio">
    /// true to use a fixed aspect ratio, false to use the aspect ratio of the
    /// graph pane.
    /// </param>
    ///
    /// <param name="abtFullSizeImage">
    /// Where the full-size PNG image gets stored, as an array of bytes.
    /// </param>
    ///
    /// <param name="abtThumbnail">
    /// Where the thumbnail PNG gets stored, as an array of bytes.
    /// </param>
    //*************************************************************************

    private static void
    GetImages
    (
        NodeXLControl oNodeXLControl,
        Boolean bUseFixedAspectRatio,
        out Byte [] abtFullSizeImage,
        out Byte [] abtThumbnail
    )
    {
        Debug.Assert(oNodeXLControl != null);
        Debug.Assert(oNodeXLControl.ActualWidth >= MinimumNodeXLControlWidth);
        Debug.Assert(oNodeXLControl.ActualHeight >= MinimumNodeXLControlHeight);

        Size oFullSizeImageSizePx = GetFullSizeImageSizePx(oNodeXLControl,
            bUseFixedAspectRatio);

        Image oFullSizeImage = oNodeXLControl.CopyGraphToBitmap(
            oFullSizeImageSizePx.Width, oFullSizeImageSizePx.Height);

        Size oThumbnailImageSizePx = GetThumbnailImageSizePx(
            oFullSizeImageSizePx);

        Image oThumbnail = oFullSizeImage.GetThumbnailImage(
            oThumbnailImageSizePx.Width, oThumbnailImageSizePx.Height,
            () => {return false;}, IntPtr.Zero);

        abtFullSizeImage = ImageToBytes(oFullSizeImage);
        abtThumbnail = ImageToBytes(oThumbnail);

        oFullSizeImage.Dispose();
        oThumbnail.Dispose();
    }

    //*************************************************************************
    //  Method: GetWorkbookContents()
    //
    /// <summary>
    /// Gets the workbook contents.
    /// </summary>
    ///
    /// <param name="oWorkbook">
    /// Workbook containing the graph data.
    /// </param>
    ///
    /// <returns>
    /// The workbook contents, as an array of bytes.
    /// </returns>
    //*************************************************************************

    private static Byte []
    GetWorkbookContents
    (
        Microsoft.Office.Interop.Excel.Workbook oWorkbook
    )
    {
        Debug.Assert(oWorkbook != null);

        // Save the workbook to a temporary file, then read the temporary file.

        String sTempFilePath = Path.GetTempFileName();

        try
        {
            oWorkbook.SaveCopyAs(sTempFilePath);

            return ( FileUtil.ReadBinaryFile(sTempFilePath) );
        }
        finally
        {
            if ( File.Exists(sTempFilePath) )
            {
                File.Delete(sTempFilePath);
            }
        }
    }

    //*************************************************************************
    //  Method: GetWorkbookSettings()
    //
    /// <summary>
    /// Gets the workbook settings from a workbook.
    /// </summary>
    ///
    /// <param name="oWorkbook">
    /// Workbook containing the graph data.
    /// </param>
    ///
    /// <returns>
    /// The workbook settings, as an XML string.
    /// </returns>
    //*************************************************************************

    private static String
    GetWorkbookSettings
    (
        Microsoft.Office.Interop.Excel.Workbook oWorkbook
    )
    {
        Debug.Assert(oWorkbook != null);

        // The workbook settings are stored within the workbook.  Retrieve
        // them.

        return (new PerWorkbookSettings(oWorkbook).WorkbookSettings);
    }

    //*************************************************************************
    //  Method: GetGraphML()
    //
    /// <summary>
    /// Gets the graph as GraphML.
    /// </summary>
    ///
    /// <param name="oWorkbook">
    /// Workbook containing the graph data.
    /// </param>
    ///
    /// <returns>
    /// The graph as GraphML.
    /// </returns>
    //*************************************************************************

    private static String
    GetGraphML
    (
        Microsoft.Office.Interop.Excel.Workbook oWorkbook
    )
    {
        Debug.Assert(oWorkbook != null);

        String sGraphML = null;

        // The graph owned by the NodeXLControl can't be used, because it
        // doesn't include all the edge and vertex column data needed for
        // GraphML.  Instead, read the graph from the workbook and include all
        // the necessary data.

        ReadWorkbookContext oReadWorkbookContext = new ReadWorkbookContext();
        oReadWorkbookContext.ReadAllEdgeAndVertexColumns = true;

        WorkbookReader oWorkbookReader = new WorkbookReader();

        IGraph oGraphForGraphML = oWorkbookReader.ReadWorkbook(
            oWorkbook, oReadWorkbookContext);

        GraphMLGraphAdapter oGraphMLGraphAdapter = new GraphMLGraphAdapter();

        using ( MemoryStream oMemoryStream = new MemoryStream() )
        {
            oGraphMLGraphAdapter.SaveGraph(oGraphForGraphML, oMemoryStream);
            oMemoryStream.Position = 0;

            using ( StreamReader oStreamReader =
                new StreamReader(oMemoryStream) )
            {
                sGraphML = oStreamReader.ReadToEnd();
            }
        }

        return (sGraphML);
    }

    //*************************************************************************
    //  Method: ZipGraphML()
    //
    /// <summary>
    /// Zips a GraphML string.
    /// </summary>
    ///
    /// <param name="sGraphML">
    /// The GraphML to zip.
    /// </param>
    ///
    /// <param name="sZippedGraphMLFileName">
    /// Name to use for the zipped GraphML file, including an extension.
    /// Sample: "NodeXLGraph-GraphML.xml".
    /// </param>
    ///
    /// <returns>
    /// The zipped GraphML, as an array of bytes.
    /// </returns>
    //*************************************************************************

    private static Byte []
    ZipGraphML
    (
        String sGraphML,
        String sZippedGraphMLFileName
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sGraphML) );
        Debug.Assert( !String.IsNullOrEmpty(sZippedGraphMLFileName) );

        return ( ZipUtil.ZipOneTextFile(sGraphML, sZippedGraphMLFileName) );
    }

    //*************************************************************************
    //  Method: GetFullSizeImageSizePx()
    //
    /// <summary>
    /// Gets the size to use for the full-size image.
    /// </summary>
    ///
    /// <param name="oNodeXLControl">
    /// NodeXLControl containing the graph.
    /// </param>
    ///
    /// <param name="bUseFixedAspectRatio">
    /// true to use a fixed aspect ratio, false to use the aspect ratio of the
    /// graph pane.
    /// </param>
    ///
    /// <returns>
    /// The size to use for the full-size image, in pixels, as a Size.
    /// </returns>
    //*************************************************************************

    private static Size
    GetFullSizeImageSizePx
    (
        NodeXLControl oNodeXLControl,
        Boolean bUseFixedAspectRatio
    )
    {
        Debug.Assert(oNodeXLControl != null);

        Debug.Assert(oNodeXLControl.ActualHeight > 0);

        Double dAspectRatio = bUseFixedAspectRatio ? FixedAspectRatio :
            (oNodeXLControl.ActualWidth / oNodeXLControl.ActualHeight);

        Debug.Assert(dAspectRatio > 0);

        return ( new Size(
            FullSizeImageWidthPx,

            (Int32)Math.Round( (Double)FullSizeImageWidthPx / dAspectRatio )
            ) );
    }

    //*************************************************************************
    //  Method: GetThumbnailImageSizePx()
    //
    /// <summary>
    /// Gets the size to use for the thumbnail image.
    /// </summary>
    ///
    /// <param name="oFullSizeImageSizePx">
    /// The size used for the full-size image, in pixels.
    /// </param>
    ///
    /// <returns>
    /// The size to use for the thumbnail image, in pixels, as a Size.
    /// </returns>
    //*************************************************************************

    private static Size
    GetThumbnailImageSizePx
    (
        Size oFullSizeImageSizePx
    )
    {
        Debug.Assert(oFullSizeImageSizePx.Height > 0);

        Double dFullSizeImageAspectRatio = (Double)oFullSizeImageSizePx.Width /
            (Double)oFullSizeImageSizePx.Height;

        if (dFullSizeImageAspectRatio >=
            (Double)ThumbnailImageWidthIfWiderPx /
            (Double)ThumbnailImageHeightIfTallerPx)
        {
            Debug.Assert(dFullSizeImageAspectRatio > 0);

            return ( new Size(
                ThumbnailImageWidthIfWiderPx,

                (Int32)Math.Ceiling( (Double)ThumbnailImageWidthIfWiderPx /
                    dFullSizeImageAspectRatio )
                ) );
        }
        else
        {
            return ( new Size(
                (Int32)Math.Ceiling( (Double)ThumbnailImageHeightIfTallerPx *
                    dFullSizeImageAspectRatio),

                ThumbnailImageHeightIfTallerPx
                ) );
        }
    }

    //*************************************************************************
    //  Method: ImageToBytes()
    //
    /// <summary>
    /// Gets an Image's data bytes.
    /// </summary>
    ///
    /// <param name="oImage">
    /// Image to get the data bytes from.
    /// </param>
    ///
    /// <returns>
    /// The image data as an array of bytes, in PNG format.
    /// </returns>
    ///
    /// <remarks>
    /// The Image is not disposed by this method.
    /// </remarks>
    //*************************************************************************

    private static Byte []
    ImageToBytes
    (
        Image oImage
    )
    {
        Debug.Assert(oImage != null);

        using ( MemoryStream oMemoryStream = new MemoryStream() )
        {
            oImage.Save(oMemoryStream, ImageFormat.Png);

            return ( oMemoryStream.ToArray() );
        }
    }


    //*************************************************************************
    //  Public constants
    //*************************************************************************

    /// Minimum actual width and height of the NodeXLControl required by this
    /// class, in WPF units.  These were chosen arbitrarily.

    public const Double MinimumNodeXLControlWidth = 100;
    ///
    public const Double MinimumNodeXLControlHeight = 100;


    //*************************************************************************
    //  Private constants
    //*************************************************************************

    /// Width of the full-size image, in pixels.  This is constrained by the
    /// width of the page that displays the graph on the NodeXL Graph Gallery
    /// site.

    private static readonly Int32 FullSizeImageWidthPx = 950;

    /// Aspect ratio to use for the full-size image, if a fixed aspect ratio is
    /// specified.

    private static readonly Double FixedAspectRatio = 950.0 / 688.0;

    /// Width of the thumbnail image if the full-size image is wider than it is
    /// tall, in pixels.  This is constrained by the width of the page that
    /// displays thumbnails on the NodeXL Graph Gallery site.

    private static readonly Int32 ThumbnailImageWidthIfWiderPx = 175;

    /// Height of the thumbnail image if the full-size image is taller than it
    /// is wide, in pixels.  This is constrained by the maximum desired table
    /// row height on the page that displays thumbnails on the NodeXL Graph
    /// Gallery site.

    private static readonly Int32 ThumbnailImageHeightIfTallerPx = 125;
}

}
