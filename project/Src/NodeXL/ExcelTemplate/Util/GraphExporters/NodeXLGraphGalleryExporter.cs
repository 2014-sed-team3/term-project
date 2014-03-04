
using System;
using System.Xml;
using System.ServiceModel;
using System.Diagnostics;
using Smrf.NodeXL.Common;
using Smrf.NodeXL.Visualization.Wpf;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: NodeXLGraphGalleryExporter
//
/// <summary>
/// Exports a graph to the NodeXL Graph Gallery.
/// </summary>
///
/// <remarks>
/// Call <see cref="ExportToNodeXLGraphGallery" /> to export a graph to the
/// NodeXL Graph Gallery website.
/// </remarks>
//*****************************************************************************

public class NodeXLGraphGalleryExporter : Object
{
    //*************************************************************************
    //  Constructor: NodeXLGraphGalleryExporter()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="NodeXLGraphGalleryExporter" /> class.
    /// </summary>
    //*************************************************************************

    public NodeXLGraphGalleryExporter()
    {
        // (Do nothing.)

        AssertValid();
    }

    //*************************************************************************
    //  Method: ExportToNodeXLGraphGallery()
    //
    /// <summary>
    /// Exports a graph to the NodeXL Graph Gallery.
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
    /// <param name="title">
    /// The graph's title.  Can't be null or empty.
    /// </param>
    ///
    /// <param name="description">
    /// The graph's description.  Can be null or empty.
    /// </param>
    ///
    /// <param name="spaceDelimitedTags">
    /// The graph's space-delimited tags.  Can be null or empty.
    /// </param>
    ///
    /// <param name="author">
    /// The graph's author.  Can't be null or empty.
    /// </param>
    ///
    /// <param name="password">
    /// The password for <paramref name="author" /> if using credentials, or
    /// null if not.
    /// </param>
    ///
    /// <param name="exportWorkbookAndSettings">
    /// true to export the workbook and its settings.
    /// </param>
    ///
    /// <param name="exportGraphML">
    /// true to export the graph's data as GraphML.
    /// </param>
    ///
    /// <param name="useFixedAspectRatio">
    /// true to use a fixed aspect ratio, false to use the aspect ratio of the
    /// graph pane.
    /// </param>
    ///
    /// <exception cref="GraphTooLargeException">
    /// Thrown when the graph is too large to export.
    /// </exception>
    //*************************************************************************

    public void
    ExportToNodeXLGraphGallery
    (
        Microsoft.Office.Interop.Excel.Workbook workbook,
        NodeXLControl nodeXLControl,
        String title,
        String description,
        String spaceDelimitedTags,
        String author,
        String password,
        Boolean exportWorkbookAndSettings,
        Boolean exportGraphML,
        Boolean useFixedAspectRatio
    )
    {
        Debug.Assert(workbook != null);
        Debug.Assert(nodeXLControl != null);

        Debug.Assert(nodeXLControl.ActualWidth >=
            GraphExporterUtil.MinimumNodeXLControlWidth);

        Debug.Assert(nodeXLControl.ActualHeight >=
            GraphExporterUtil.MinimumNodeXLControlHeight);

        Debug.Assert( !String.IsNullOrEmpty(title) );
        Debug.Assert( !String.IsNullOrEmpty(author) );
        AssertValid();

        Byte [] abtFullSizeImage, abtThumbnail, abtWorkbookContents,
            abtGraphML;

        String sWorkbookSettings;

        // Note that the name of the Zipped GraphML is not important.  When a
        // user downloads the GraphML from the Graph Gallery website, the
        // website will rename the GraphML before giving it to the user.

        GraphExporterUtil.GetDataToExport(workbook, nodeXLControl,
            exportWorkbookAndSettings, exportGraphML, "GraphML.xml",
            useFixedAspectRatio, out abtFullSizeImage, out abtThumbnail,
            out abtWorkbookContents, out sWorkbookSettings, out abtGraphML);

        NodeXLGraphGalleryServiceClient oClient =
            new NodeXLGraphGalleryServiceClient(GetWcfServiceBinding(),
                new EndpointAddress(
                    ProjectInformation.NodeXLGraphGalleryWcfServiceUrl)
                );

        oClient.Endpoint.Binding.SendTimeout =
            new TimeSpan(0, SendTimeoutMinutes, 0);

        try
        {
            oClient.AddGraph4(title, author, password, description,
                spaceDelimitedTags, abtFullSizeImage, abtThumbnail,
                abtWorkbookContents, sWorkbookSettings, abtGraphML);
        }
        catch (ProtocolException oProtocolException)
        {
            // The following text search detects an exception thrown by the WCF
            // service when the exported byte count exceeds the maximum that
            // can be handled by the WCF service.
            //
            // This isn't a very robust test.  Is there a better way to do
            // it?  An earlier version attempted to calculate the number of
            // bytes that would be exported before attempting to export them,
            // but it didn't seem to be doing so accurately.
            //
            // See the MaximumBytes constant for more details.

            if (oProtocolException.Message.IndexOf("(400) Bad Request") >= 0)
            {
                throw new GraphTooLargeException();
            }

            throw (oProtocolException);
        }
    }

    //*************************************************************************
    //  Method: GetWcfServiceBinding()
    //
    /// <summary>
    /// Returns the binding to use for communicating with the NodeXL Graph
    /// Gallery's WCF service.
    /// </summary>
    ///
    /// <returns>
    /// A <see cref="BasicHttpBinding" /> object.
    /// </returns>
    //*************************************************************************

    protected BasicHttpBinding
    GetWcfServiceBinding()
    {
        // Communicate over SSL.

        BasicHttpBinding oBasicHttpBinding =
            new BasicHttpBinding(BasicHttpSecurityMode.Transport);

        // These settings were determined by running the svcutil.exe utility
        // on the NodeXLGraphGalleryService.svc file and looking at the config
        // file generated by the utility.

        oBasicHttpBinding.Name = "BasicHttpBinding_INodeXLGraphGalleryService";
        oBasicHttpBinding.ReceiveTimeout = new TimeSpan(0, 1, 0);
        oBasicHttpBinding.MaxBufferSize = MaximumBytes;
        oBasicHttpBinding.MaxReceivedMessageSize = MaximumBytes;
        oBasicHttpBinding.TransferMode = TransferMode.Buffered;

        XmlDictionaryReaderQuotas oReaderQuotas =
            new XmlDictionaryReaderQuotas();

        oReaderQuotas.MaxArrayLength = MaximumBytes;
        oBasicHttpBinding.ReaderQuotas = oReaderQuotas;

        return (oBasicHttpBinding);
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
        // (Do nothing.)
    }


    //*************************************************************************
    //  Protected constants
    //*************************************************************************

    /// Maximum number of bytes that can be exported to the WCF service.  This
    /// is the value to use for length-related parameters in the WCF binding
    /// object.  If this maximum is exceeded, a
    /// System.ServiceModel.CommunicationException will occur when attempting
    /// to export a graph.
    ///
    /// This must be the same value that is specified many times in the
    /// Web.config file in the NodeXLGraphGaller\WcfService project.  Search
    /// for the following text in the Web.config file to find the value:
    //
    //     maxRequestLength
    //     maxReceivedMessageSize
    //     maxArrayLength
    //     maxStringContentLength

    protected const Int32 MaximumBytes = 50000000;

    /// Number of minutes to wait while sending a graph.  If it takes longer
    /// than this to send the graph, a TimeoutException is thrown on the client
    /// end.

    protected const Int32 SendTimeoutMinutes = 20;


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}

}
