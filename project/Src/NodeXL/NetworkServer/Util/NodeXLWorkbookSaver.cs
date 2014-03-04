

using System;
using System.Xml;
using System.Diagnostics;
using Smrf.NodeXL.ExcelTemplate;

namespace Smrf.NodeXL.NetworkServer
{
//*****************************************************************************
//  Class: NodeXLWorkbookSaver
//
/// <summary>
/// Saves a graph to a NodeXL Excel workbook.
/// </summary>
//*****************************************************************************

class NodeXLWorkbookSaver
{
    //*************************************************************************
    //  Method: SaveGraphToNodeXLWorkbook()
    //
    /// <overloads>
    /// Saves a graph to a NodeXL Excel workbook.
    /// </overloads>
    ///
    /// <summary>
    /// Saves a graph to a NodeXL Excel workbook.
    /// </summary>
    ///
    /// <param name="startTime">
    /// Time at which the network download started.
    /// </param>
    ///
    /// <param name="xmlDocument">
    /// The XML document containing the network as GraphML.
    /// </param>
    ///
    /// <param name="networkConfigurationFilePath">
    /// The path of the specified network configuration file.
    /// </param>
    ///
    /// <param name="networkFileFolderPath">
    /// The full path to the folder where the network files should be written.
    /// </param>
    ///
    /// <param name="nodeXLWorkbookSettingsFilePath">
    /// The full path to the workbook settings file to use for the NodeXL
    /// workbook.  If null, no workbook settings file is used.
    /// </param>
    ///
    /// <param name="automate">
    /// True to automate the NodeXL workbook.
    /// </param>
    ///
    /// <remarks>
    /// If an error occurs, a <see cref="SaveGraphToNodeXLWorkbookException" />
    /// is thrown.
    /// </remarks>
    //*************************************************************************

    public static void
    SaveGraphToNodeXLWorkbook
    (
        DateTime startTime,
        XmlDocument xmlDocument,
        String networkConfigurationFilePath,
        String networkFileFolderPath,
        String nodeXLWorkbookSettingsFilePath,
        Boolean automate
    )
    {
        Debug.Assert(xmlDocument != null);
        Debug.Assert( !String.IsNullOrEmpty(networkConfigurationFilePath) );
        Debug.Assert( !String.IsNullOrEmpty(networkFileFolderPath) );

        // Sample workbook path:
        //
        // C:\NetworkConfiguration_2010-06-01_02-00-00.xlsx
        
        String sWorkbookPath = FileUtil.GetOutputFilePath(startTime,
            networkConfigurationFilePath, networkFileFolderPath, String.Empty,
            "xlsx");

        Console.WriteLine(
            "Saving the network to the NodeXL workbook \"{0}\"."
            ,
            sWorkbookPath
            );

        try
        {
            GraphMLToNodeXLWorkbookConverter.SaveGraphToNodeXLWorkbook(
                xmlDocument, null, sWorkbookPath,
                nodeXLWorkbookSettingsFilePath, automate);
        }
        catch (ConvertGraphMLToNodeXLWorkbookException
            oConvertGraphMLToNodeXLWorkbookException)
        {
            ExitCode eExitCode = ExitCode.UnexpectedException;

            switch (oConvertGraphMLToNodeXLWorkbookException.ErrorCode)
            {
                case GraphMLToNodeXLWorkbookConverter.ErrorCode.
                    CouldNotFindNodeXLTemplate:

                    eExitCode = ExitCode.CouldNotFindNodeXLTemplate;
                    break;

                case GraphMLToNodeXLWorkbookConverter.ErrorCode.
                    CouldNotOpenExcel:

                    eExitCode = ExitCode.CouldNotOpenExcel;
                    break;

                case GraphMLToNodeXLWorkbookConverter.ErrorCode.
                    CouldNotCreateNodeXLWorkbook:

                    eExitCode = ExitCode.CouldNotCreateNodeXLWorkbook;
                    break;

                case GraphMLToNodeXLWorkbookConverter.ErrorCode.
                    CouldNotImportGraphMLIntoNodeXLWorkbook:

                    eExitCode = ExitCode.CouldNotImportGraphIntoNodeXLWorkbook;
                    break;

                case GraphMLToNodeXLWorkbookConverter.ErrorCode.
                    SaveNodeXLWorkbookFileError:

                    eExitCode = ExitCode.SaveNetworkFileError;
                    break;

                case GraphMLToNodeXLWorkbookConverter.ErrorCode.
                    CouldNotReadWorkbookSettingsFile:

                    eExitCode = ExitCode.CouldNotReadWorkbookSettingsFile;
                    break;

                default:

                    break;
            }

            throw new SaveGraphToNodeXLWorkbookException(eExitCode,
                oConvertGraphMLToNodeXLWorkbookException.Message);
        }

        if (automate)
        {
            // SaveGraphToNodeXLWorkbook() stored an "automate tasks on open"
            // flag in the workbook, indicating that task automation should be
            // run on it the next time it's opened.  Open it, then wait for it
            // to close.

            Console.WriteLine(
                "Automating the NodeXL workbook."
                );

            try
            {
                ExcelTemplate.TaskAutomator.OpenWorkbookToAutomate(
                    sWorkbookPath, 60 * 60);
            }
            catch (Exception oException)
            {
                throw new SaveGraphToNodeXLWorkbookException(
                    ExitCode.CouldNotAutomateNodeXLWorkbook,

                    "The NodeXL workbook couldn't be opened to automate"
                    + " it.  Details:"
                    + "\r\n\r\n"
                    + oException.Message
                    );
            }
        }
    }
}

}
