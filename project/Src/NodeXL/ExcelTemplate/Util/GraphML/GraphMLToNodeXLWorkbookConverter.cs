

using System;
using System.Xml;
using System.IO;
using System.Diagnostics;
using System.Reflection;
using Microsoft.Office.Interop.Excel;
using Smrf.NodeXL.Core;
using Smrf.NodeXL.Adapters;
using Smrf.AppLib;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: GraphMLToNodeXLWorkbookConverter
//
/// <summary>
/// Saves a GraphML XML document to a new NodeXL workbook.
/// </summary>
//*****************************************************************************

public class GraphMLToNodeXLWorkbookConverter
{
    //*************************************************************************
    //  Method: SaveGraphToNodeXLWorkbook()
    //
    /// <summary>
    /// Saves a GraphML XML document to a new NodeXL workbook.
    /// </summary>
    ///
    /// <param name="graphMLDocument">
    /// The XML document containing GraphML.
    /// </param>
    ///
    /// <param name="graphMLFilePath">
    /// Path to the file that contained the GraphML, or null if the GraphML
    /// didn't come from a file.
    /// </param>
    ///
    /// <param name="nodeXLWorkbookPath">
    /// The path where the NodeXL workbook should be saved, or null.  If null,
    /// the workbook is left open and is not saved.
    /// </param>
    ///
    /// <param name="nodeXLWorkbookSettingsFilePath">
    /// The full path to the workbook settings file to use for the NodeXL
    /// workbook.  If null, no workbook settings file is used.
    /// </param>
    ///
    /// <param name="setAutomateTasksOnOpen">
    /// true to set an "automate tasks on open" flag in the new workbook.
    /// </param>
    ///
    /// <remarks>
    /// If an error occurs, a <see
    /// cref="ConvertGraphMLToNodeXLWorkbookException" /> is thrown.
    /// </remarks>
    //*************************************************************************

    public static void
    SaveGraphToNodeXLWorkbook
    (
        XmlDocument graphMLDocument,
        String graphMLFilePath,
        String nodeXLWorkbookPath,
        String nodeXLWorkbookSettingsFilePath,
        Boolean setAutomateTasksOnOpen
    )
    {
        Debug.Assert(graphMLDocument != null);

        Debug.Assert(nodeXLWorkbookPath == null ||
            nodeXLWorkbookPath.Length > 0);

        // Open Excel.

        Application oExcelApplication = new Application();

        if (oExcelApplication == null)
        {
            throw new ConvertGraphMLToNodeXLWorkbookException(
                ErrorCode.CouldNotOpenExcel,
                "Excel couldn't be opened.  Is it installed on this computer?"
                );
        }

        // ExcelApplicationKiller requires that the application be visible.

        oExcelApplication.Visible = true;

        // Suppress alerts about overwriting an existing file.

        oExcelApplication.DisplayAlerts = false;

        ExcelApplicationKiller oExcelApplicationKiller =
            new ExcelApplicationKiller(oExcelApplication);

        try
        {
            SaveGraphToNodeXLWorkbook(graphMLDocument, graphMLFilePath,
                nodeXLWorkbookPath, nodeXLWorkbookSettingsFilePath,
                setAutomateTasksOnOpen, oExcelApplication);
        }
        finally
        {
            if (nodeXLWorkbookPath != null)
            {
                oExcelApplication.Quit();

                // Quitting the Excel application does not remove it from
                // memory.  Kill its process.

                oExcelApplicationKiller.KillExcelApplication();
            }
        }
    }

    //*************************************************************************
    //  Enum: ErrorCode
    //
    /// <summary>
    /// Specifies an error that occurred while converting GraphML to a new
    /// NodeXL workbook.
    /// </summary>
    //*************************************************************************

    public enum
    ErrorCode
    {
        /// <summary>
        /// The NodeXL Excel template file could not be found.
        /// </summary>

        CouldNotFindNodeXLTemplate,

        /// <summary>
        /// Excel could not be opened.
        /// </summary>

        CouldNotOpenExcel,

        /// <summary>
        /// A NodeXL workbook could not be created from the NodeXL template.
        /// </summary>

        CouldNotCreateNodeXLWorkbook,

        /// <summary>
        /// A NodeXL workbook was created but the GraphML could not be imported
        /// into it.
        /// </summary>

        CouldNotImportGraphMLIntoNodeXLWorkbook,

        /// <summary>
        /// An error occcurred while saving the NodeXL workbook.
        /// </summary>

        SaveNodeXLWorkbookFileError,

        /// <summary>
        /// The workbook settings file specified for the NodeXL workbook
        /// couldn't be read.
        /// </summary>

        CouldNotReadWorkbookSettingsFile,
    }

    //*************************************************************************
    //  Method: SaveGraphToNodeXLWorkbook()
    //
    /// <summary>
    /// Saves a graph to a new NodeXL workbook given an Excel Application
    /// object.
    /// </summary>
    ///
    /// <param name="oGraphMLDocument">
    /// The XML document containing GraphML.
    /// </param>
    ///
    /// <param name="sGraphMLFilePath">
    /// Path to the file that contained the GraphML, or null if the GraphML
    /// didn't come from a file.
    /// </param>
    ///
    /// <param name="sNodeXLWorkbookPath">
    /// The path where the NodeXL workbook should be saved, or null.  If null,
    /// the workbook is left open and is not saved.
    /// </param>
    ///
    /// <param name="sNodeXLWorkbookSettingsFilePath">
    /// The full path to the workbook settings file to use for the NodeXL
    /// workbook.  If null, no workbook settings file is used.
    /// </param>
    ///
    /// <param name="bSetAutomateTasksOnOpen">
    /// true to set an "automate tasks on open" flag in the new workbook.
    /// </param>
    ///
    /// <param name="oExcelApplication">
    /// An open Excel Application object.
    /// </param>
    ///
    /// <remarks>
    /// If an error occurs, a <see
    /// cref="ConvertGraphMLToNodeXLWorkbookException" /> is thrown.
    /// </remarks>
    //*************************************************************************

    private static void
    SaveGraphToNodeXLWorkbook
    (
        XmlDocument oGraphMLDocument,
        String sGraphMLFilePath,
        String sNodeXLWorkbookPath,
        String sNodeXLWorkbookSettingsFilePath,
        Boolean bSetAutomateTasksOnOpen,
        Application oExcelApplication
    )
    {
        Debug.Assert(oGraphMLDocument != null);

        Debug.Assert(sNodeXLWorkbookPath == null ||
            sNodeXLWorkbookPath.Length > 0);

        Debug.Assert(oExcelApplication != null);

        String sWorkbookSettings = null;

        if (sNodeXLWorkbookSettingsFilePath != null)
        {
            try
            {
                sWorkbookSettings = GetWorkbookSettings(
                    sNodeXLWorkbookSettingsFilePath);
            }
            catch (Exception oException)
            {
                OnException(oException,
                    ErrorCode.CouldNotReadWorkbookSettingsFile,

                    String.Format(

                        "The NodeXL options file \"{0}\" couldn't be read."
                        ,
                        sNodeXLWorkbookSettingsFilePath
                        )
                    );
            }
        }

        // Create a new workbook from the NodeXL template.

        Workbook oNodeXLWorkbook = null;

        try
        {
            oNodeXLWorkbook = ApplicationUtil.CreateNodeXLWorkbook(
                oExcelApplication);
        }
        catch (IOException oIOException)
        {
            throw new ConvertGraphMLToNodeXLWorkbookException(
                ErrorCode.CouldNotFindNodeXLTemplate, oIOException.Message);
        }
        catch (Exception oException)
        {
            OnException(oException,
                ErrorCode.CouldNotCreateNodeXLWorkbook,
                "A NodeXL workbook couldn't be created."
                );
        }

        // Create a NodeXL graph from the XML document.

        IGraph oGraph = ( new GraphMLGraphAdapter() ).LoadGraphFromString(
            oGraphMLDocument.OuterXml);

        try
        {
            // Turn off text wrap if necessary to speed up the import.

            GraphImportTextWrapManager.ManageTextWrapBeforeImport(
                oGraph, oNodeXLWorkbook, false);

            // Import the graph into the workbook.
            //
            // Note that the GraphMLGraphAdapter stored String arrays on the
            // IGraph object that specify the names of the attributes that it
            // added to the graph's edges and vertices.  These get used by the
            // ImportGraph method to determine which columns need to be added
            // to the edge and vertex worksheets.

            GraphImporter.ImportGraph(oGraph,

                ( String[] )oGraph.GetRequiredValue(
                    ReservedMetadataKeys.AllEdgeMetadataKeys,
                    typeof( String[] ) ),

                ( String[] )oGraph.GetRequiredValue(
                    ReservedMetadataKeys.AllVertexMetadataKeys,
                    typeof( String[] ) ),

                false, oNodeXLWorkbook);

            // Store the graph's directedness in the workbook.

            PerWorkbookSettings oPerWorkbookSettings =
                new ExcelTemplate.PerWorkbookSettings(oNodeXLWorkbook);

            oPerWorkbookSettings.GraphDirectedness = oGraph.Directedness;

            if (sWorkbookSettings != null)
            {
                oPerWorkbookSettings.WorkbookSettings = sWorkbookSettings;
            }

            Object oGraphDescriptionAsObject;

            if ( !String.IsNullOrEmpty(sGraphMLFilePath) )
            {
                // The GraphML came from a file.

                GraphImporter.UpdateGraphHistoryAfterImport(oNodeXLWorkbook,

                    GraphImporter.GetImportedGraphMLFileDescription(
                        sGraphMLFilePath, oGraph),

                    null);
            }
            else if ( oGraph.TryGetValue(ReservedMetadataKeys.GraphDescription,
                typeof(String), out oGraphDescriptionAsObject) )
            {
                // The GraphML came from the NetworkServer program.
                //
                // Note that we can't have GraphImporter check the user's
                // ImportUserSettings object here to determine if the import
                // description should be saved.  Accessing user setting objects
                // requires access to Globals.ThisWorkbook, which is null when
                // GraphImporter is called from another process.

                GraphImporter
                    .UpdateGraphHistoryAfterImportWithoutPermissionCheck(
                        oNodeXLWorkbook, (String)oGraphDescriptionAsObject,
                        null, oPerWorkbookSettings);
            }

            if (bSetAutomateTasksOnOpen)
            {
                // Store an "automate tasks on open" flag in the workbook,
                // indicating that task automation should be run on it the next
                // time it's opened.  (It is up to the user of this class to
                // open the workbook to trigger automation.)

                oPerWorkbookSettings.AutomateTasksOnOpen = true;
            }
        }
        catch (Exception oException)
        {
            OnException(oException,
                ErrorCode.CouldNotImportGraphMLIntoNodeXLWorkbook,
                "The GraphML couldn't be imported into the NodeXL workbook."
                );
        }

        if (sNodeXLWorkbookPath == null)
        {
            return;
        }

        try
        {
            ExcelUtil.SaveWorkbookAs(oNodeXLWorkbook, sNodeXLWorkbookPath);
        }
        catch (Exception oException)
        {
            OnException(oException, ErrorCode.SaveNodeXLWorkbookFileError,
                "The NodeXL workbook couldn't be saved."
                );
        }

        try
        {
            oNodeXLWorkbook.Close(false, Missing.Value, Missing.Value);
        }
        catch (Exception oException)
        {
            OnException(oException, ErrorCode.SaveNodeXLWorkbookFileError,
                "The NodeXL workbook couldn't be closed."
                );
        }
    }

    //*************************************************************************
    //  Method: GetWorkbookSettings()
    //
    /// <summary>
    /// Gets the workbook settings to store in the new NodeXL workbook.
    /// </summary>
    ///
    /// <param name="sNodeXLWorkbookSettingsFilePath">
    /// The full path to the workbook settings file to use for the NodeXL
    /// workbook.  Can't be null or empty.
    /// </param>
    //*************************************************************************

    private static String
    GetWorkbookSettings
    (
        String sNodeXLWorkbookSettingsFilePath
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sNodeXLWorkbookSettingsFilePath) );

        // Use a synchronized TextReader so that multiple threads can
        // simultaneously read the same file.

        using ( TextReader oTextReader = TextReader.Synchronized(
            new StreamReader(sNodeXLWorkbookSettingsFilePath) ) )
        {
            return ( oTextReader.ReadToEnd() );
        }
    }

    //*************************************************************************
    //  Method: OnException()
    //
    /// <summary>
    /// Handles an exception that was caught while attempting to save a graph
    /// to a new NodeXL workbook.
    /// </summary>
    ///
    /// <param name="oException">
    /// The exception that was caught.
    /// </param>
    ///
    /// <param name="eErrorCode">
    /// The error code to use.
    /// </param>
    ///
    /// <param name="sErrorMessage">
    /// Error message, suitable for showing in the UI.
    /// </param>
    ///
    /// <remarks>
    /// This method throws a <see
    /// cref="ConvertGraphMLToNodeXLWorkbookException" />.
    /// </remarks>
    //*************************************************************************

    private static void
    OnException
    (
        Exception oException,
        ErrorCode eErrorCode,
        String sErrorMessage
    )
    {
        Debug.Assert(oException != null);
        Debug.Assert( !String.IsNullOrEmpty(sErrorMessage) );

        throw new ConvertGraphMLToNodeXLWorkbookException(eErrorCode,

            String.Format(

                "{0}  Details:"
                + "\r\n\r\n"
                + "{1}."
                ,
                sErrorMessage,
                oException.Message
                ) );
    }
}

}
