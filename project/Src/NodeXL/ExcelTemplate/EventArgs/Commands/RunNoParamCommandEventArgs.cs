
using System;
using System.Diagnostics;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: RunNoParamCommandEventArgs
//
/// <summary>
/// Provides information for a command that needs to be run, where the command
/// does not require any parameters.
/// </summary>
///
/// <remarks>
/// See <see cref="RunCommandEventArgs" /> for information about how NodeXL
/// sends commands from one UI object to another.
///
/// <para>
/// There are many commands that do not require any parameters.  Instead of
/// deriving a class from RunCommandEventArgs for each such command, this class
/// consolidates all of them.  They are distinguished by a <see
/// cref="NoParamCommand" /> value passed to the constructor.
/// </para>
///
/// </remarks>
//*****************************************************************************

public class RunNoParamCommandEventArgs : RunCommandEventArgs
{
    //*************************************************************************
    //  Constructor: RunNoParamCommandEventArgs()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="RunNoParamCommandEventArgs" /> class.
    /// </summary>
    ///
    /// <param name="noParamCommand">
    /// The no-parameter command that needs to be run.
    /// </param>
    //*************************************************************************

    public RunNoParamCommandEventArgs
    (
        NoParamCommand noParamCommand
    )
    {
        m_eNoParamCommand = noParamCommand;

        AssertValid();
    }

    //*************************************************************************
    //  Property: NoParamCommand
    //
    /// <summary>
    /// Gets the no-parameter command that needs to be run.
    /// </summary>
    ///
    /// <value>
    /// The no-parameter command that needs to be run.
    /// </value>
    //*************************************************************************

    public NoParamCommand
    NoParamCommand
    {
        get
        {
            AssertValid();

            return (m_eNoParamCommand);
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

        // m_eNoParamCommand
    }


    //*************************************************************************
    //  Protected member data
    //*************************************************************************

    /// The no-parameter command that needs to be run.

    protected NoParamCommand m_eNoParamCommand;
}


//*****************************************************************************
//  Enum: NoParamCommand
//
/// <summary>
/// Specifies a command that needs to be run, where the command does not
/// require any parameters.
/// </summary>
//*****************************************************************************

public enum
NoParamCommand
{
    /// <summary>
    /// Group by vertex attribute.
    /// </summary>

    GroupByVertexAttribute,

    /// <summary>
    /// Calculate connected components.
    /// </summary>

    CalculateConnectedComponents,

    /// <summary>
    /// Group by motif.
    /// </summary>

    GroupByMotif,

    /// <summary>
    /// Let the user edit the group user settings.
    /// </summary>

    EditGroupUserSettings,

    /// <summary>
    /// Show the dynamic filters dialog.
    /// </summary>

    ShowDynamicFilters,

    /// <summary>
    /// Read the workbook into the graph pane.
    /// </summary>

    ReadWorkbook,

    /// <summary>
    /// Show the graph pane, then read the workbook into the graph pane.
    /// </summary>

    ShowGraphAndReadWorkbook,

    /// <summary>
    /// Let the user edit the layout settings.
    /// </summary>

    EditLayoutUserSettings,

    /// <summary>
    /// Show the readability metrics dialog.
    /// </summary>

    ShowReadabilityMetrics,

    /// <summary>
    /// Export the graph to the NodeXL Graph Gallery.
    /// </summary>

    ExportToNodeXLGraphGallery,

    /// <summary>
    /// Export the graph to email.
    /// </summary>

    ExportToEmail,

    /// <summary>
    /// Open the application's home page in a browser window.
    /// </summary>

    OpenHomePage,

    /// <summary>
    /// Open the application's discussion page in a browser window.
    /// </summary>

    OpenDiscussionPage,

    /// <summary>
    /// Open the NodeXL Graph Gallery in a browser window.
    /// </summary>

    OpenNodeXLGraphGallery,

    /// <summary>
    /// Load the user settings maintained by the command handler.
    /// </summary>

    LoadUserSettings,

    /// <summary>
    /// Save the user settings maintained by the command handler.
    /// </summary>

    SaveUserSettings,

    /// <summary>
    /// Show the graph legend in the TaskPane.
    /// </summary>

    ShowGraphLegend,

    /// <summary>
    /// Hide the graph legend in the TaskPane.
    /// </summary>

    HideGraphLegend,

    /// <summary>
    /// Show the graph axes in the TaskPane.
    /// </summary>

    ShowGraphAxes,

    /// <summary>
    /// Hide the graph axes in the TaskPane.
    /// </summary>

    HideGraphAxes,

    /// <summary>
    /// Update the layout in the TaskPane.
    /// </summary>

    UpdateLayout,

    /// <summary>
    /// Show and hide the column groups specified in the user settings.
    /// </summary>

    ShowAndHideColumnGroups,

    /// <summary>
    /// Let the user edit the import data user settings.
    /// </summary>

    EditImportDataUserSettings,

    /// <summary>
    /// Import edges from another open workbook (to be specified by the user)
    /// that contains a graph represented as an adjacency matrix.
    /// </summary>

    ImportFromMatrixWorkbook,

    /// <summary>
    /// Import edges and vertices from another open workbook (to be specified
    /// by the user).
    /// </summary>

    ImportFromWorkbook,

    /// <summary>
    /// Import the contents of a UCINET full matrix DL file into the workbook.
    /// </summary>

    ImportFromUcinetFile,

    /// <summary>
    /// Import the contents of a GraphML file into the workbook.
    /// </summary>

    ImportFromGraphMLFile,

    /// <summary>
    /// Import a set of GraphML files into a set of new NodeXL workbooks.
    /// </summary>

    ImportFromGraphMLFiles,

    /// <summary>
    /// Import the contents of a Pajek file into the workbook.
    /// </summary>

    ImportFromPajekFile,

    /// <summary>
    /// Export the selected rows of the edge and vertex tables to a new NodeXL
    /// workbook.
    /// </summary>

    ExportSelectionToNewNodeXLWorkbook,

    /// <summary>
    /// Export the edge and vertex tables to a new UCINET full matrix DL file.
    /// </summary>

    ExportToUcinetFile,

    /// <summary>
    /// Export the edge and vertex tables to a new GraphML file.
    /// </summary>

    ExportToGraphMLFile,

    /// <summary>
    /// Export the edge and vertex tables to a new Pajek text file.
    /// </summary>

    ExportToPajekFile,

    /// <summary>
    /// Export the edge table to a new workbook as an adjacency matrix.
    /// </summary>

    ExportToNewMatrixWorkbook,

    /// <summary>
    /// Aggregate graph metrics from multiple workbooks.
    /// </summary>

    AggregateGraphMetrics,

    /// <summary>
    /// Open a dialog that lets the user run multiple tasks.
    /// </summary>

    AutomateTasks,

    /// <summary>
    /// Immediately run multiple tasks on this workbook.  The task automation
    /// dialog is not shown.
    /// </summary>

    AutomateThisWorkbook,

    /// <summary>
    /// Delete any subgraph image thumbnails in the vertex worksheet.
    /// </summary>

    DeleteSubgraphThumbnails,

    /// Show the dialog that analyzes a user's email network and write the
    /// results to the edge worksheet.

    AnalyzeEmailNetwork,

    /// <summary>
    /// Copy a NodeXL workbook created on another machine and convert the copy
    /// to work on this machine.
    /// </summary>

    ConvertNodeXLWorkbook,

    /// <summary>
    /// Allow the user to register for email updates.
    /// </summary>

    RegisterUser,

    /// Show the dialog that lists available graph metrics and calculate them
    /// if requested by the user.

    ShowGraphMetrics,

    /// Create a subgraph of each of the graph's vertices and save the images
    /// to disk or the workbook.

    CreateSubgraphImages,

    /// Show the dialog that fills edge and vertex attribute columns using
    /// values from user-specified source columns.

    AutoFillWorkbook,

    /// Create a new NodeXL workbook.

    CreateNodeXLWorkbook,

    /// Show the graph summary dialog.

    ShowGraphSummary,
}

}
