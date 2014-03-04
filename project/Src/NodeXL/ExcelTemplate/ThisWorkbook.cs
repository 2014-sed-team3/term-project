
using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.VisualStudio.Tools.Applications.Runtime;
using Microsoft.Office.Interop.Excel;
using Microsoft.Office.Core;
using System.Reflection;
using Smrf.NodeXL.Common;
using Smrf.NodeXL.Core;
using Smrf.NodeXL.Visualization.Wpf;
using Smrf.NodeXL.Algorithms;
using Smrf.NodeXL.Adapters;
using Smrf.NodeXL.ApplicationUtil;
using Microsoft.NodeXL.ExcelTemplatePlugIns;
using Smrf.AppLib;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: ThisWorkbook
//
/// <summary>
/// Represent's a workbook created from the template.
/// </summary>
//*****************************************************************************

public partial class ThisWorkbook
{
    //*************************************************************************
    //  Property: WorksheetContextMenuManager
    //
    /// <summary>
    /// Gets the object that adds custom menu items to the Excel context menus
    /// that appear when the vertex or edge table is right-clicked.
    /// </summary>
    ///
    /// <value>
    /// A WorksheetContextMenuManager object.
    /// </value>
    //*************************************************************************

    public WorksheetContextMenuManager
    WorksheetContextMenuManager
    {
        get
        {
            AssertValid();

            return (m_oWorksheetContextMenuManager);
        }
    }

    //*************************************************************************
    //  Property: GraphDirectedness
    //
    /// <summary>
    /// Gets or sets the graph directedness of the workbook.
    /// </summary>
    ///
    /// <value>
    /// A GraphDirectedness value.
    /// </value>
    //*************************************************************************

    public GraphDirectedness
    GraphDirectedness
    {
        get
        {
            AssertValid();

            // Retrive the directedness from the per-workbook settings.

            return (this.PerWorkbookSettings.GraphDirectedness);
        }

        set
        {
            // Store the directedness in the per-workbook settings.

            this.PerWorkbookSettings.GraphDirectedness = value;

            // Update the user settings.

            GeneralUserSettings oGeneralUserSettings =
                new GeneralUserSettings();

            oGeneralUserSettings.NewWorkbookGraphDirectedness = value;
            oGeneralUserSettings.Save();

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: ShowWaitCursor
    //
    /// <summary>
    /// Sets a flag specifying whether the wait cursor should be shown.
    /// </summary>
    ///
    /// <value>
    /// true to show the wait cursor.
    /// </value>
    //*************************************************************************

    public Boolean
    ShowWaitCursor
    {
        set
        {
            this.Application.Cursor = value ?
                Microsoft.Office.Interop.Excel.XlMousePointer.xlWait
                :
                Microsoft.Office.Interop.Excel.XlMousePointer.xlDefault;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Method: ExcelApplicationIsReady
    //
    /// <summary>
    /// Determines whether the Excel application is ready to accept method
    /// calls.
    /// </summary>
    ///
    /// <param name="showBusyMessage">
    /// true if a busy message should be displayed if the application is not
    /// ready.
    /// </param>
    ///
    /// <returns>
    /// true if the Excel application is ready to accept method calls.
    /// </returns>
    //*************************************************************************

    public Boolean
    ExcelApplicationIsReady
    (
        Boolean showBusyMessage
    )
    {
        AssertValid();

        if ( !ExcelUtil.ExcelApplicationIsReady(this.Application) )
        {
            if (showBusyMessage)
            {
                FormUtil.ShowWarning(
                    "This feature isn't available while a worksheet cell is"
                    + " being edited.  Press Enter to finish editing the cell,"
                    + " then try again."
                    );
            }

            return (false);
        }

        return (true);
    }

    //*************************************************************************
    //  Method: ImportFromGraphDataProvider()
    //
    /// <summary>
    /// Imports a graph from a graph data provider.
    /// </summary>
    ///
    /// <param name="graphDataProvider">
    /// An object that implements either <see cref="IGraphDataProvider2" /> or
    /// <see cref="IGraphDataProvider" />.
    /// </param>
    //*************************************************************************

    public void
    ImportFromGraphDataProvider
    (
        Object graphDataProvider
    )
    {
        Debug.Assert(graphDataProvider != null);

        Debug.Assert(graphDataProvider is IGraphDataProvider2 ||
            graphDataProvider is IGraphDataProvider);

        AssertValid();

        if ( !this.ExcelApplicationIsReady(true) )
        {
            return;
        }

        ShowWaitCursor = true;

        try
        {
            IGraph oGraph;

            if ( !PlugInManager.TryGetGraphFromGraphDataProvider(
                graphDataProvider, out oGraph) )
            {
                return;
            }

            ImportGraph(oGraph, 

                ( String[] )oGraph.GetRequiredValue(
                    ReservedMetadataKeys.AllEdgeMetadataKeys,
                    typeof( String[] ) ),

                ( String[] )oGraph.GetRequiredValue(
                    ReservedMetadataKeys.AllVertexMetadataKeys,
                    typeof( String[] ) ),

                (String)oGraph.GetValue(
                    ReservedMetadataKeys.GraphDescription,
                    typeof(String) ),

                (String)oGraph.GetValue(
                    ReservedMetadataKeys.SuggestedFileNameNoExtension,
                    typeof(String) )
                );
        }
        catch (Exception oException)
        {
            ErrorUtil.OnException(oException);
        }
        finally
        {
            ShowWaitCursor = false;
        }
    }

    //*************************************************************************
    //  Method: AutomateTasksOnOpen()
    //
    /// <summary>
    /// Runs task automation on the workbook immediately after it is opened.
    /// </summary>
    //*************************************************************************

    public void
    AutomateTasksOnOpen()
    {
        AssertValid();

        // Reset the flag before the workbook is automated.  The workbook may
        // get exported during automation, and the exported workbook shouldn't
        // have the flag set.

        this.PerWorkbookSettings.AutomateTasksOnOpen = false;

        try
        {
            AutomateThisWorkbook();
        }
        catch (Exception oException)
        {
            ErrorUtil.OnException(oException);
            return;
        }

        AutomateTasksUserSettings oAutomateTasksUserSettings =
            new AutomateTasksUserSettings();

        if ( (oAutomateTasksUserSettings.TasksToRun &
            AutomationTasks.ReadWorkbook) != 0 )
        {
            // The graph is being asynchronously laid out in the graph pane.
            // Wait for the layout to complete.

            EventHandler<GraphLaidOutEventArgs> oGraphLaidOutEventHandler =
                null;

            oGraphLaidOutEventHandler =
                delegate(Object sender, GraphLaidOutEventArgs e)
            {
                // Prevent this delegate from being called again.

                this.GraphLaidOut -= oGraphLaidOutEventHandler;

                OnTasksAutomatedOnOpen();
            };

            this.GraphLaidOut += oGraphLaidOutEventHandler;
        }
        else
        {
            OnTasksAutomatedOnOpen();
        }
    }

    //*************************************************************************
    //  Method: PopulateVertexWorksheet()
    //
    /// <summary>
    /// Populates the vertex worksheet with the name of each unique vertex in
    /// the edge worksheet.
    /// </summary>
    ///
    /// <param name="activateVertexWorksheetWhenDone">
    /// true to activate the vertex worksheet after it is populated.
    /// </param>
    ///
    /// <param name="notifyUserOnError">
    /// If true, the user is notified when an error occurs.  If false, an
    /// exception is thrown when an error occurs.
    /// </param>
    ///
    /// <returns>
    /// true if successful.
    /// </returns>
    //*************************************************************************

    public Boolean
    PopulateVertexWorksheet
    (
        Boolean activateVertexWorksheetWhenDone,
        Boolean notifyUserOnError
    )
    {
        AssertValid();

        if ( !this.ExcelApplicationIsReady(true) )
        {
            return (false);
        }

        // Create and use the object that fills in the vertex worksheet.

        VertexWorksheetPopulator oVertexWorksheetPopulator =
            new VertexWorksheetPopulator();

        this.ScreenUpdating = false;

        try
        {
            oVertexWorksheetPopulator.PopulateVertexWorksheet(
                this.InnerObject, activateVertexWorksheetWhenDone);

            this.ScreenUpdating = true;

            return (true);
        }
        catch (Exception oException)
        {
            // Don't let Excel handle unhandled exceptions.

            this.ScreenUpdating = true;

            if (notifyUserOnError)
            {
                ErrorUtil.OnException(oException);

                return (false);
            }
            else
            {
                throw oException;
            }
        }
    }

    //*************************************************************************
    //  Method: CreateSubgraphImages()
    //
    /// <summary>
    /// Creates a subgraph of each of the graph's vertices and saves the images
    /// to disk or the workbook.
    /// </summary>
    ///
    /// <param name="mode">
    /// Indicates the mode in which the CreateSubgraphImagesDialog is being
    /// used.
    /// </param>
    //*************************************************************************

    public void
    CreateSubgraphImages
    (
        CreateSubgraphImagesDialog.DialogMode mode
    )
    {
        AssertValid();

        if ( !this.ExcelApplicationIsReady(true) )
        {
            return;
        }

        // Populate the vertex worksheet.  This is necessary in case the user
        // opts to insert images into the vertex worksheet.  Note that
        // PopulateVertexWorksheet() returns false if the vertex worksheet
        // or table is missing, and that it activates the vertex worksheet.

        if ( !PopulateVertexWorksheet(true, true) )
        {
            return;
        }

        ICollection<String> oSelectedVertexNames = new String[0];

        if (mode == CreateSubgraphImagesDialog.DialogMode.Normal)
        {
            // (PopulateVertexWorksheet() should have selected the vertex
            // worksheet.)

            Debug.Assert(this.Application.ActiveSheet is Worksheet);

            Debug.Assert( ( (Worksheet)this.Application.ActiveSheet ).Name ==
                WorksheetNames.Vertices);

            // Get an array of vertex names that are selected in the vertex
            // worksheet.

            oSelectedVertexNames = Globals.Sheet2.GetSelectedVertexNames();
        }

        ( new CreateSubgraphImagesDialog(mode, this.InnerObject,
                oSelectedVertexNames) ).ShowDialog();
    }

    //*************************************************************************
    //  Method: AutoFillWorkbook()
    //
    /// <summary>
    /// Shows the dialog that fills edge and vertex attribute columns using
    /// values from user-specified source columns.
    /// </summary>
    ///
    /// <param name="mode">
    /// Indicates the mode in which the AutoFillWorkbookDialog is being used.
    /// </param>
    //*************************************************************************

    public void
    AutoFillWorkbook
    (
        AutoFillWorkbookDialog.DialogMode mode
    )
    {
        AssertValid();

        if ( !this.ExcelApplicationIsReady(true) )
        {
            return;
        }

        if (m_oAutoFillWorkbookDialog != null)
        {
            m_oAutoFillWorkbookDialog.Activate();
            return;
        }

        // The dialog is created on demand.

        m_oAutoFillWorkbookDialog =
            new AutoFillWorkbookDialog(mode, this.InnerObject);

        Int32 iHwnd = this.Application.Hwnd;

        m_oAutoFillWorkbookDialog.WorkbookAutoFilled += delegate
        {
            OnWorkbookAutoFilled(
                ( new GeneralUserSettings() ).AutoReadWorkbook );
        };

        m_oAutoFillWorkbookDialog.Closed += delegate
        {
            // Activate the Excel window.
            //
            // This is a workaround for an annoying and frustrating bug
            // involving the AutoFillWorkbookDialog when it runs modeless.  If
            // the user takes no action in AutoFillWorkbookDialog before
            // closing it, the Excel window gets activated when
            // AutoFillWorkbookDialog closes, as expected.  However, if he
            // opens one of AutoFillWorkbookDialog's modal dialogs, such as
            // NumericRangeColumnAutoFillUserSettingsDialog, and then closes
            // the modal dialog and AutoFillWorkbookDialog, some other
            // application besides Excel gets activated.
            //
            // Setting the owner of the modal dialog to the Excel window
            // (this.Application.Hwnd) didn't help.  Neither did setting the
            // owner of the modal dialog to AutoFillWorkbookDialog.  Nothing
            // worked except explicitly activating the Excel window when the
            // AutoFillWorkbookDialog closes.

            Win32Functions.SetForegroundWindow( new IntPtr(iHwnd) );

            m_oAutoFillWorkbookDialog = null;
        };

        if (mode == AutoFillWorkbookDialog.DialogMode.Normal)
        {
            m_oAutoFillWorkbookDialog.Show( new Win32Window(iHwnd) );
        }
        else
        {
            m_oAutoFillWorkbookDialog.ShowDialog();
        }
    }

    //*************************************************************************
    //  Method: OnWorkbookAutoFilled()
    //
    /// <summary>
    /// Performs tasks required after the workbook is autofilled.
    /// </summary>
    ///
    /// <param name="readWorkbook">
    /// true to read the workbook.
    /// </param>
    //*************************************************************************

    public void
    OnWorkbookAutoFilled
    (
        Boolean readWorkbook
    )
    {
        AssertValid();

        if (this.PerWorkbookSettings.AutoFillWorkbookResults.VertexXResults
            .ColumnAutoFilled )
        {
            // When the X and Y columns are autofilled, the graph shouldn't be
            // laid out again.

            this.Ribbon.Layout = LayoutType.Null;
        }

        if (readWorkbook)
        {
            ShowGraphAndReadWorkbook();
        }
    }

    //*************************************************************************
    //  Method: ShowOrHideColumnGroups()
    //
    /// <summary>
    /// Shows or hides the specified groups of related columns.
    /// </summary>
    ///
    /// <param name="columnGroups">
    /// The column groups to show or hide, as an ORed combination of <see
    /// cref="ColumnGroups" /> flags.  Column groups that aren't specified are
    /// not modified.
    /// </param>
    ///
    /// <param name="show">
    /// true to show the column groups, false to hide them.
    /// </param>
    ///
    /// <remarks>
    /// Columns that don't exist are ignored.
    /// </remarks>
    //*************************************************************************

    public void
    ShowOrHideColumnGroups
    (
        ColumnGroups columnGroups,
        Boolean show
    )
    {
        AssertValid();

        if ( !this.ExcelApplicationIsReady(true) )
        {
            return;
        }

        // Uncomment the ScreenUpdating calls to prevent screen updates as the
        // groups are shown.  The updates may provide useful feedback, so show
        // the updates for now.

        // this.ScreenUpdating = false;

        try
        {
            ColumnGroupManager.ShowOrHideColumnGroups(this.InnerObject,
                columnGroups, show, true);
        }
        finally
        {
            // this.ScreenUpdating = true;
        }
    }

    //*************************************************************************
    //  Method: EnableSetVisualAttributes
    //
    /// <summary>
    /// Enables the "set visual attribute" buttons in the Ribbon.
    /// </summary>
    ///
    /// <remarks>
    /// This method tells the Ribbon to enable or disable its visual attribute
    /// buttons when something happens in the workbook that might change their
    /// enabled state.  Note that this information is "pushed" into the Ribbon.
    /// The Ribbon can't pull the information from the workbook when it needs
    /// it because unlike menu items that appear only when a menu is opened,
    /// the visual attribute buttons are visible in the Ribbon at all times.
    /// </remarks>
    //*************************************************************************

    public void
    EnableSetVisualAttributes()
    {
        AssertValid();

        VisualAttributes eVisualAttributes = VisualAttributes.None;
        String sWorksheetName = null;
        Object oActiveSheet = this.ActiveSheet;

        if (oActiveSheet is Worksheet)
        {
            sWorksheetName = ( (Worksheet)oActiveSheet ).Name;
        }

        // Determine whether the active worksheet has a NodeXL table that
        // includes a selection.

        String sTableName = null;

        switch (sWorksheetName)
        {
            case WorksheetNames.Edges:

                sTableName = TableNames.Edges;
                break;

            case WorksheetNames.Vertices:

                sTableName = TableNames.Vertices;
                break;

            case WorksheetNames.Groups:

                sTableName = TableNames.Groups;
                break;

            default:

                break;
        }

        ListObject oTable;
        Range oSelectedTableRange;

        if (
            sTableName != null
            &&
            ExcelTableUtil.TryGetSelectedTableRange(this.InnerObject,
                sWorksheetName, sTableName, out oTable,
                out oSelectedTableRange)
            )
        {
            if (sWorksheetName == WorksheetNames.Edges)
            {
                eVisualAttributes |= VisualAttributes.Color;
                eVisualAttributes |= VisualAttributes.Alpha;
                eVisualAttributes |= VisualAttributes.EdgeWidth;
                eVisualAttributes |= VisualAttributes.EdgeVisibility;
            }
            else if (sWorksheetName == WorksheetNames.Vertices)
            {
                eVisualAttributes |= VisualAttributes.Color;
                eVisualAttributes |= VisualAttributes.Alpha;
                eVisualAttributes |= VisualAttributes.VertexShape;
                eVisualAttributes |= VisualAttributes.VertexRadius;
                eVisualAttributes |= VisualAttributes.VertexVisibility;
            }
            else if (sWorksheetName == WorksheetNames.Groups)
            {
                eVisualAttributes |= VisualAttributes.Color;
                eVisualAttributes |= VisualAttributes.VertexShape;
            }
        }

        this.Ribbon.EnableSetVisualAttributes(eVisualAttributes);
    }

    //*************************************************************************
    //  Event: VisualAttributeSetInWorkbook
    //
    /// <summary>
    /// Occurs when a visual attribute is set in the selected rows of the
    /// active worksheet.
    /// </summary>
    //*************************************************************************

    public event EventHandler VisualAttributeSetInWorkbook;


    //*************************************************************************
    //  Event: AttributesEditedInGraph
    //
    /// <summary>
    /// Occurs when edge or vertex attributes are edited in the NodeXL graph.
    /// </summary>
    //*************************************************************************

    public event EventHandler<AttributesEditedEventArgs>
        AttributesEditedInGraph;


    //*************************************************************************
    //  Event: GraphLaidOut
    //
    /// <summary>
    /// Occurs after graph layout completes.
    /// </summary>
    ///
    /// <remarks>
    /// Graph layout occurs asynchronously.  This event fires when the graph
    /// is successfully laid out.
    /// </remarks>
    //*************************************************************************

    public event EventHandler<GraphLaidOutEventArgs> GraphLaidOut;


    //*************************************************************************
    //  Event: VerticesMoved
    //
    /// <summary>
    /// Occurs after one or more vertices are moved to new locations in the
    /// graph.
    /// </summary>
    ///
    /// <remarks>
    /// This event is fired when the user releases the mouse button after
    /// dragging one or more vertices to new locations in the graph.
    /// </remarks>
    //*************************************************************************

    public event EventHandler<VerticesMovedEventArgs2> VerticesMoved;


    //*************************************************************************
    //  Event: GroupsCollapsedOrExpanded
    //
    /// <summary>
    /// Occurs after a set of groups is collapsed or expanded.
    /// </summary>
    //*************************************************************************

    public event EventHandler<GroupsCollapsedOrExpandedEventArgs>
        GroupsCollapsedOrExpanded;


    //*************************************************************************
    //  Property: Ribbon
    //
    /// <summary>
    /// Gets the application's ribbon.
    /// </summary>
    ///
    /// <value>
    /// The application's ribbon.
    /// </value>
    //*************************************************************************

    private Ribbon
    Ribbon
    {
        get
        {
            AssertValid();

            return (Globals.Ribbons.Ribbon);
        }
    }

    //*************************************************************************
    //  Property: GraphVisibility
    //
    /// <summary>
    /// Gets or sets the visibility of the NodeXL graph.
    /// </summary>
    ///
    /// <value>
    /// true if the NodeXL graph is visible.
    /// </value>
    //*************************************************************************

    private Boolean
    GraphVisibility
    {
        get
        {
            AssertValid();

            return (this.DocumentActionsCommandBar.Visible &&
                m_bTaskPaneCreated);
        }

        set
        {
            if (value && !m_bTaskPaneCreated)
            {
                // The NodeXL task pane is created in a lazy manner.

                TaskPane oTaskPane = new TaskPane(this, this.Ribbon);

                this.ActionsPane.Clear();
                this.ActionsPane.Controls.Add(oTaskPane);

                oTaskPane.Dock = DockStyle.Fill;

                oTaskPane.AttributesEditedInGraph += (sender, e) =>
                {
                    ForwardEvent<AttributesEditedEventArgs>(
                        e, this.AttributesEditedInGraph);
                };

                oTaskPane.GraphLaidOut += (sender, e) =>
                {
                    ForwardEvent<GraphLaidOutEventArgs>(
                        e, this.GraphLaidOut);
                };

                oTaskPane.VerticesMoved += (sender, e) =>
                {
                    ForwardEvent<VerticesMovedEventArgs2>(
                        e, this.VerticesMoved);
                };

                oTaskPane.GroupsCollapsedOrExpanded += (sender, e) =>
                {
                    ForwardEvent<GroupsCollapsedOrExpandedEventArgs>(
                        e, this.GroupsCollapsedOrExpanded);
                };

                Sheet1 oSheet1 = Globals.Sheet1;
                Sheet2 oSheet2 = Globals.Sheet2;
                Sheet5 oSheet5 = Globals.Sheet5;

                m_oSelectionCoordinator = new SelectionCoordinator(this,
                    oSheet1, oSheet1.Edges, oSheet2, oSheet2.Vertices,
                    oSheet5, oSheet5.Groups, Globals.Sheet6, oTaskPane);

                m_bTaskPaneCreated = true;
            }

            this.DocumentActionsCommandBar.Visible = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: DocumentActionsCommandBar
    //
    /// <summary>
    /// Gets the document actions CommandBar.
    /// </summary>
    ///
    /// <value>
    /// The document actions CommandBar, which is where the NodeXL graph is
    /// displayed.
    /// </value>
    //*************************************************************************

    private Microsoft.Office.Core.CommandBar
    DocumentActionsCommandBar
    {
        get
        {
            AssertValid();

            return ( Application.CommandBars["Document Actions"] );
        }
    }

    //*************************************************************************
    //  Property: PerWorkbookSettings
    //
    /// <summary>
    /// Gets a new PerWorkbookSettings object.
    /// </summary>
    ///
    /// <value>
    /// A new PerWorkbookSettings object.
    /// </value>
    //*************************************************************************

    private PerWorkbookSettings
    PerWorkbookSettings
    {
        get
        {
            AssertValid();

            return ( new PerWorkbookSettings(this.InnerObject) );
        }
    }

    //*************************************************************************
    //  Property: ScreenUpdating
    //
    /// <summary>
    /// Gets or sets a flag specifying whether Excel's screen updating is on or
    /// off.
    /// </summary>
    ///
    /// <value>
    /// true to turn on screen updating.
    /// </value>
    //*************************************************************************

    private Boolean
    ScreenUpdating
    {
        get
        {
            AssertValid();

            return (this.Application.ScreenUpdating);
        }

        set
        {
            this.Application.ScreenUpdating = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Method: ShowGraph()
    //
    /// <summary>
    /// Shows the graph pane.
    /// </summary>
    ///
    /// <returns>
    /// true if the graph pane was shown, false if the application isn't ready.
    /// </returns>
    //*************************************************************************

    private Boolean
    ShowGraph()
    {
        AssertValid();

        if ( !this.ExcelApplicationIsReady(true) )
        {
            return (false);
        }

        this.GraphVisibility = true;
        return (true);
    }

    //*************************************************************************
    //  Method: EditImportDataUserSettings()
    //
    /// <summary>
    /// Lets the user edit the import data user settings.
    /// </summary>
    //*************************************************************************

    private void
    EditImportDataUserSettings()
    {
        Debug.Assert( this.ExcelApplicationIsReady(false) );
        AssertValid();

        ImportDataUserSettings oImportDataUserSettings =
            new ImportDataUserSettings();

        PlugInUserSettings oPlugInUserSettings =
            new PlugInUserSettings();

        if ( ( new ImportDataUserSettingsDialog(
            oImportDataUserSettings, oPlugInUserSettings, this) ).ShowDialog()
			== DialogResult.OK )
        {
            oImportDataUserSettings.Save();
            oPlugInUserSettings.Save();
        }
    }

    //*************************************************************************
    //  Method: ImportFromMatrixWorkbook()
    //
    /// <summary>
    /// Imports edges from another open workbook that contains a graph
    /// represented as an adjacency matrix.
    /// </summary>
    //*************************************************************************

    private void
    ImportFromMatrixWorkbook()
    {
        Debug.Assert( this.ExcelApplicationIsReady(false) );
        AssertValid();

        ImportFromMatrixWorkbookDialog oImportFromMatrixWorkbookDialog =
            new ImportFromMatrixWorkbookDialog(this.InnerObject);

        if (oImportFromMatrixWorkbookDialog.ShowDialog() == DialogResult.OK)
        {
            // Import the graph's edges and vertices into the workbook.

            ImportGraphWithEdgeWeight(oImportFromMatrixWorkbookDialog.Graph,

                GraphImporter.GetImportedFileDescription(
                    "open matrix workbook",
                    oImportFromMatrixWorkbookDialog.SourceWorkbookName),

                null
                );
        }
    }

    //*************************************************************************
    //  Method: RegisterUser()
    //
    /// <summary>
    /// Allows the user to register for email updates.
    /// </summary>
    //*************************************************************************

    private void
    RegisterUser()
    {
        Debug.Assert( this.ExcelApplicationIsReady(false) );
        AssertValid();

        ( new RegisterUserDialog() ).ShowDialog();
    }

    //*************************************************************************
    //  Method: ImportFromWorkbook()
    //
    /// <summary>
    /// Imports edges and vertices from another open workbook.
    /// </summary>
    //*************************************************************************

    private void
    ImportFromWorkbook()
    {
        Debug.Assert( this.ExcelApplicationIsReady(false) );
        AssertValid();

        ImportFromWorkbookDialog oImportFromWorkbookDialog =
            new ImportFromWorkbookDialog(this.InnerObject);

        if (oImportFromWorkbookDialog.ShowDialog() == DialogResult.OK)
        {
            // Import the graph's edges and vertices into the workbook.

            IGraph oGraph = oImportFromWorkbookDialog.Graph;

            ImportGraph(oGraph,

                ( String[] )oGraph.GetRequiredValue(
                    ReservedMetadataKeys.AllEdgeMetadataKeys,
                    typeof( String[] ) ),

                ( String[] )oGraph.GetRequiredValue(
                    ReservedMetadataKeys.AllVertexMetadataKeys,
                    typeof( String[] ) ),

                GraphImporter.GetImportedFileDescription("open workbook",
                    oImportFromWorkbookDialog.SourceWorkbookName),

                null
                );
        }
    }

    //*************************************************************************
    //  Method: ExportToUcinetFile()
    //
    /// <summary>
    /// Exports the edge and vertex tables to a new UCINET full matrix DL file.
    /// </summary>
    //*************************************************************************

    private void
    ExportToUcinetFile()
    {
        Debug.Assert( this.ExcelApplicationIsReady(false) );
        AssertValid();

        if ( !MergeIsApproved(
                "add an Edge Weight column, and export the edges to a new"
                + " UCINET full matrix DL file.") )
        {
            return;
        }

        ReadWorkbookContext oReadWorkbookContext = new ReadWorkbookContext();
        oReadWorkbookContext.ReadEdgeWeights = true;

        SaveUcinetFileDialog oSaveUcinetFileDialog =
            new SaveUcinetFileDialog(String.Empty, String.Empty);

        ExportToFile(oReadWorkbookContext, oSaveUcinetFileDialog);
    }

    //*************************************************************************
    //  Method: ExportToGraphMLFile()
    //
    /// <summary>
    /// Exports the edge and vertex tables to a new GraphML file.
    /// </summary>
    //*************************************************************************

    private void
    ExportToGraphMLFile()
    {
        Debug.Assert( this.ExcelApplicationIsReady(false) );
        AssertValid();

        if ( !MergeIsApproved(
                "add an Edge Weight column, and export the edges and vertices"
                + " to a new GraphML file.") )
        {
            return;
        }

        ReadWorkbookContext oReadWorkbookContext = new ReadWorkbookContext();
        oReadWorkbookContext.ReadAllEdgeAndVertexColumns = true;

        SaveGraphMLFileDialog oSaveGraphMLFileDialog =
            new SaveGraphMLFileDialog(String.Empty, String.Empty);

        ExportToFile(oReadWorkbookContext, oSaveGraphMLFileDialog);
    }

    //*************************************************************************
    //  Method: ExportToPajekFile()
    //
    /// <summary>
    /// Exports the edge and vertex tables to a new Pajek text file.
    /// </summary>
    //*************************************************************************

    private void
    ExportToPajekFile()
    {
        Debug.Assert( this.ExcelApplicationIsReady(false) );
        AssertValid();

        if ( !MergeIsApproved(
                "add an Edge Weight column, and export the edges and vertices"
                + " to a new Pajek file.") )
        {
            return;
        }

        ReadWorkbookContext oReadWorkbookContext = new ReadWorkbookContext();
        oReadWorkbookContext.ReadEdgeWeights = true;

        // Map any vertex coordinates stored in the workbook to an arbitrary
        // rectangle.  PajekGraphAdapter will in turn map these to Pajek
        // coordinates.

        oReadWorkbookContext.IgnoreVertexLocations = false;

        oReadWorkbookContext.GraphRectangle =
            new System.Drawing.Rectangle(0, 0, 10000, 10000);

        SavePajekFileDialog oSavePajekFileDialog =
            new SavePajekFileDialog(String.Empty, String.Empty);

        ExportToFile(oReadWorkbookContext, oSavePajekFileDialog);
    }

    //*************************************************************************
    //  Method: ExportSelectionToNewNodeXLWorkbook()
    //
    /// <summary>
    /// Exports the selected rows of the edge and vertex tables to a new NodeXL
    /// workbook.
    /// </summary>
    //*************************************************************************

    private void
    ExportSelectionToNewNodeXLWorkbook()
    {
        Debug.Assert( this.ExcelApplicationIsReady(false) );
        AssertValid();

        // Exporting the workbook changes the active worksheet several times.
        // Save the current active worksheet so it can be restored later.

        Object oOldActiveSheet = this.Application.ActiveSheet;

        this.ScreenUpdating = false;

        try
        {
            WorkbookExporter oWorkbookExporter =
                new WorkbookExporter(this.InnerObject);

            Workbook oNewWorkbook =
                oWorkbookExporter.ExportSelectionToNewNodeXLWorkbook();

            // Reactivate the original active worksheet.

            if (oOldActiveSheet is Worksheet)
            {
                ExcelUtil.ActivateWorksheet( (Worksheet)oOldActiveSheet );
            }

            // Activate the edge worksheet in the new workbook.

            // Note: When run in the debugger, activating the new workbook
            // causes a "System.Runtime.InteropServices.ExternalException
            // crossed a native/managed boundary" error.  There is no inner
            // exception.  This does not occur outside the debugger.  Does this
            // have something to do with Visual Studio security contexts?`

            ExcelUtil.ActivateWorkbook(oNewWorkbook);

            Worksheet oNewEdgeWorksheet;

            if ( ExcelUtil.TryGetWorksheet(oNewWorkbook, WorksheetNames.Edges,
                out oNewEdgeWorksheet) )
            {
                ExcelUtil.ActivateWorksheet(oNewEdgeWorksheet);
            }

            this.ScreenUpdating = true;
        }
        catch (ExportWorkbookException oExportWorkbookException)
        {
            this.ScreenUpdating = true;

            FormUtil.ShowWarning(oExportWorkbookException.Message);
        }
        catch (Exception oException)
        {
            this.ScreenUpdating = true;

            ErrorUtil.OnException(oException);
        }
    }

    //*************************************************************************
    //  Method: ExportToNewMatrixWorkbook()
    //
    /// <summary>
    /// Exports the edge table to a new workbook as an adjacency matrix.
    /// </summary>
    //*************************************************************************

    private void
    ExportToNewMatrixWorkbook()
    {
        Debug.Assert( this.ExcelApplicationIsReady(false) );
        AssertValid();

        if ( !MergeIsApproved(
                "add an Edge Weight column, and export the edges to a new"
                + " workbook as an adjacency matrix.") )
        {
            return;
        }

        ShowWaitCursor = true;

        this.ScreenUpdating = false;

        try
        {
            WorkbookExporter oWorkbookExporter =
                new WorkbookExporter(this.InnerObject);

            oWorkbookExporter.ExportToNewMatrixWorkbook();

            this.ScreenUpdating = true;
        }
        catch (ExportWorkbookException oExportWorkbookException)
        {
            this.ScreenUpdating = true;

            FormUtil.ShowWarning(oExportWorkbookException.Message);
        }
        catch (Exception oException)
        {
            this.ScreenUpdating = true;

            ErrorUtil.OnException(oException);
        }

        ShowWaitCursor = false;
    }

    //*************************************************************************
    //  Method: MergeDuplicateEdges()
    //
    /// <summary>
    /// Merges duplicate edges in the edge worksheet.
    /// </summary>
    ///
    /// <param name="bEditMergeDuplicateEdgesUserSettings">
    /// true to allow the user to edit the user settings before duplicate edges
    /// are merged.
    /// </param>
    ///
    /// <returns>
    /// true if the duplicate edges were successfully merged.
    /// </returns>
    //*************************************************************************

    private Boolean
    MergeDuplicateEdges
    (
        Boolean bEditMergeDuplicateEdgesUserSettings
    )
    {
        AssertValid();

        if ( !this.ExcelApplicationIsReady(true) )
        {
            return (false);
        }

        MergeDuplicateEdgesUserSettings oMergeDuplicateEdgesUserSettings =
            new MergeDuplicateEdgesUserSettings();

        if (bEditMergeDuplicateEdgesUserSettings)
        {
            if ( new MergeDuplicateEdgesUserSettingsDialog(
                MergeDuplicateEdgesUserSettingsDialog.DialogMode.Normal,
                oMergeDuplicateEdgesUserSettings,
                this.InnerObject).ShowDialog() == DialogResult.OK) 
            {
                oMergeDuplicateEdgesUserSettings.Save();
            }
            else
            {
                return (false);
            }
        }

        DuplicateEdgeMerger oDuplicateEdgeMerger = new DuplicateEdgeMerger();

        ShowWaitCursor = true;
        this.ScreenUpdating = false;

        try
        {
            oDuplicateEdgeMerger.MergeDuplicateEdges(this.InnerObject,
                oMergeDuplicateEdgesUserSettings.CountDuplicates,
                oMergeDuplicateEdgesUserSettings.DeleteDuplicates,

                oMergeDuplicateEdgesUserSettings
                    .ThirdColumnNameForDuplicateDetection);

            this.ScreenUpdating = true;

            return (true);
        }
        catch (Exception oException)
        {
            // Don't let Excel handle unhandled exceptions.

            this.ScreenUpdating = true;
            ErrorUtil.OnException(oException);

            return (false);
        }
        finally
        {
            ShowWaitCursor = false;
        }
    }

    //*************************************************************************
    //  Method: ConvertNodeXLWorkbook()
    //
    /// <summary>
    /// Copies a NodeXL workbook created on another machine and converts the
    /// copy to work on this machine.
    /// </summary>
    //*************************************************************************

    private void
    ConvertNodeXLWorkbook()
    {
        Debug.Assert( this.ExcelApplicationIsReady(false) );
        AssertValid();

        ConvertNodeXLWorkbookDialog oConvertNodeXLWorkbookDialog =
            new ConvertNodeXLWorkbookDialog(this.Application);

        oConvertNodeXLWorkbookDialog.ShowDialog();
    }

    //*************************************************************************
    //  Method: AutomateTasksAfterImport()
    //
    /// <summary>
    /// Runs task automation on the workbook after a graph is imported, if
    /// appropriate.
    /// </summary>
    //*************************************************************************

    private void
    AutomateTasksAfterImport()
    {
        AssertValid();

        if ( ( new ImportDataUserSettings() ).AutomateAfterImport )
        {
            try
            {
                AutomateThisWorkbook();
            }
            catch (Exception oException)
            {
                ErrorUtil.OnException(oException);
            }
        }
    }

    //*************************************************************************
    //  Method: ShowGraphAndReadWorkbook()
    //
    /// <summary>
    /// Shows the graph pane and reads the workbook.
    /// </summary>
    //*************************************************************************

    private void
    ShowGraphAndReadWorkbook()
    {
        AssertValid();

        // Make sure the graph is showing, then tell the TaskPane to read the
        // workbook.

        if ( this.ShowGraph() )
        {
            CommandDispatcher.SendNoParamCommand(this,
                NoParamCommand.ReadWorkbook);
        }
    }

    //*************************************************************************
    //  Method: ShowGraphSummary()
    //
    /// <summary>
    /// Opens a dialog box that provides a summary of how the graph was
    /// created.
    /// </summary>
    //*************************************************************************

    private void
    ShowGraphSummary()
    {
        Debug.Assert( this.ExcelApplicationIsReady(false) );
        AssertValid();

        ( new GraphSummaryDialog(this.InnerObject) ).ShowDialog();
    }

    //*************************************************************************
    //  Method: DeleteSubgraphThumbnails()
    //
    /// <summary>
    /// Deletes any subgraph image thumbnails in the vertex worksheet.
    /// </summary>
    //*************************************************************************

    private void
    DeleteSubgraphThumbnails()
    {
        Debug.Assert( this.ExcelApplicationIsReady(false) );
        AssertValid();

        if ( MessageBox.Show(

                "This will delete any subgraph thumbnails in the Vertices"
                + " worksheet.  It will not delete any subgraph image files"
                + " you saved in a folder."
                + "\r\n\r\n"
                + "Do you want to delete subgraph thumbnails?"
                ,
                FormUtil.ApplicationName, MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2)
                == DialogResult.Yes
            )
        {
            TableImagePopulator.DeleteImagesInColumn(this.InnerObject,
                WorksheetNames.Vertices, VertexTableColumnNames.SubgraphImage);
        }
    }

    //*************************************************************************
    //  Method: ShowGraphMetrics()
    //
    /// <summary>
    /// Shows the dialog that lists available graph metrics and calculates them
    /// if requested by the user.
    /// </summary>
    //*************************************************************************

    private void
    ShowGraphMetrics()
    {
        AssertValid();

        ( new GraphMetricsDialog( GraphMetricsDialog.DialogMode.Normal,
            this.InnerObject) ).ShowDialog();
    }

    //*************************************************************************
    //  Method: AnalyzeEmailNetwork()
    //
    /// <summary>
    /// Shows the dialog that analyzes a user's email network and writes the
    /// results to the edge worksheet.
    /// </summary>
    //*************************************************************************

    private void
    AnalyzeEmailNetwork()
    {
        Debug.Assert( this.ExcelApplicationIsReady(false) );
        AssertValid();

        AnalyzeEmailNetworkDialog oAnalyzeEmailNetworkDialog =
            new AnalyzeEmailNetworkDialog(this.InnerObject,
                ( new ImportDataUserSettings() ).ClearTablesBeforeImport);

        if (oAnalyzeEmailNetworkDialog.ShowDialog() == DialogResult.OK)
        {
            GraphImporter.UpdateGraphHistoryAfterImport(this.InnerObject,
                "The graph was obtained by analyzing an email network.",
                null
                );

            // Note that the ribbon won't actually update until the modal
            // dialog closes.  I haven't found a way to work around this
            // odd Excel behavior.

            this.Ribbon.GraphDirectedness = this.GraphDirectedness =
                GraphDirectedness.Directed;

            AutomateTasksAfterImport();
        };
    }

    //*************************************************************************
    //  Method: ImportFromUcinetFile()
    //
    /// <summary>
    /// Imports the contents of a UCINET full matrix DL file into the workbook.
    /// </summary>
    //*************************************************************************

    private void
    ImportFromUcinetFile()
    {
        Debug.Assert( this.ExcelApplicationIsReady(false) );
        AssertValid();

        // Create a graph from a Ucinet file selected by the user.

        OpenUcinetFileDialog oOpenUcinetFileDialog =
            new OpenUcinetFileDialog();

        if (oOpenUcinetFileDialog.ShowDialog() == DialogResult.OK)
        {
            ImportGraphWithEdgeWeight(oOpenUcinetFileDialog.Graph,

                GraphImporter.GetImportedFileDescription("UCINET file",
                    oOpenUcinetFileDialog.FileName),

                null
                );
        }
    }

    //*************************************************************************
    //  Method: ImportFromPajekFile()
    //
    /// <summary>
    /// Imports the contents of a Pajek file into the workbook.
    /// </summary>
    //*************************************************************************

    private void
    ImportFromPajekFile()
    {
        Debug.Assert( this.ExcelApplicationIsReady(false) );
        AssertValid();

        // Create a graph from a Pajek file selected by the user.

        IGraph oGraph;
        OpenPajekFileDialog oDialog = new OpenPajekFileDialog();

        if (oDialog.ShowDialogAndOpenPajekFile(out oGraph) == DialogResult.OK)
        {
            // Import the graph's edges and vertices into the workbook.

            ImportGraphWithEdgeWeight(oGraph, 

                GraphImporter.GetImportedFileDescription(
                    "Pajek file", oDialog.FileName),

                null
                );
        }
    }

    //*************************************************************************
    //  Method: ImportFromGraphMLFile()
    //
    /// <summary>
    /// Imports the contents of a GraphML file into the workbook.
    /// </summary>
    //*************************************************************************

    private void
    ImportFromGraphMLFile()
    {
        Debug.Assert( this.ExcelApplicationIsReady(false) );
        AssertValid();

        // Create a graph from a GraphML file selected by the user.

        IGraph oGraph;
        OpenGraphMLFileDialog oDialog = new OpenGraphMLFileDialog();

        if (oDialog.ShowDialogAndOpenGraphMLFile(out oGraph) ==
            DialogResult.OK)
        {
            // Import the graph's edges and vertices into the workbook.

            ImportGraph(oGraph,

                ( String[] )oGraph.GetRequiredValue(
                    ReservedMetadataKeys.AllEdgeMetadataKeys,
                    typeof( String[] ) ),

                ( String[] )oGraph.GetRequiredValue(
                    ReservedMetadataKeys.AllVertexMetadataKeys,
                    typeof( String[] ) ),

                GraphImporter.GetImportedGraphMLFileDescription(
                    oDialog.FileName, oGraph),

                null
                );
        }
    }

    //*************************************************************************
    //  Method: ImportFromGraphMLFiles()
    //
    /// <summary>
    /// Imports a set of GraphML files into a set of new NodeXL workbooks.
    /// </summary>
    //*************************************************************************

    private void
    ImportFromGraphMLFiles()
    {
        Debug.Assert( this.ExcelApplicationIsReady(false) );
        AssertValid();

        ImportFromGraphMLFilesDialog oDialog =
            new ImportFromGraphMLFilesDialog();

        oDialog.ShowDialog();
    }

    //*************************************************************************
    //  Method: AggregateGraphMetrics()
    //
    /// <summary>
    /// Aggregates graph metrics from multiple workbooks.
    /// </summary>
    //*************************************************************************

    private void
    AggregateGraphMetrics()
    {
        Debug.Assert( this.ExcelApplicationIsReady(false) );
        AssertValid();

        ( new AggregateGraphMetricsDialog(this.InnerObject) ).ShowDialog();
    }

    //*************************************************************************
    //  Method: ShowAndHideColumnGroups()
    //
    /// <summary>
    /// Shows and hides the column groups specified in the user settings.
    /// </summary>
    //*************************************************************************

    private void
    ShowAndHideColumnGroups()
    {
        Debug.Assert( this.ExcelApplicationIsReady(false) );
        AssertValid();

        ColumnGroups eColumnGroupsToShow =
            ( new ColumnGroupUserSettings() ).ColumnGroupsToShow;

        ColumnGroupManager.ShowOrHideColumnGroups(this.InnerObject,
            eColumnGroupsToShow, true, false);

        ColumnGroupManager.ShowOrHideColumnGroups(this.InnerObject,
            ~eColumnGroupsToShow, false, false);
    }

    //*************************************************************************
    //  Method: ExportToFile()
    //
    /// <summary>
    /// Merges duplicate edges and exports the edge and vertex tables to a file
    /// using a provided dialog.
    /// </summary>
    ///
    /// <param name="oReadWorkbookContext">
    /// Provides access to objects needed for converting an Excel workbook to a
    /// NodeXL graph.
    /// </param>
    ///
    /// <param name="oSaveGraphFileDialog">
    /// The dialog to use to save the graph.
    /// </param>
    //*************************************************************************

    private void
    ExportToFile
    (
        ReadWorkbookContext oReadWorkbookContext,
        SaveGraphFileDialog oSaveGraphFileDialog
    )
    {
        Debug.Assert(oReadWorkbookContext != null);
        Debug.Assert(oSaveGraphFileDialog != null);
        AssertValid();

        WorkbookReader oWorkbookReader = new WorkbookReader();

        ShowWaitCursor = true;
        this.ScreenUpdating = false;

        try
        {
            ( new DuplicateEdgeMerger() ).MergeDuplicateEdges(
                this.InnerObject);

            this.ScreenUpdating = true;

            // Read the workbook and let the user save it.

            IGraph oGraph = oWorkbookReader.ReadWorkbook(
                this.InnerObject, oReadWorkbookContext);

            oSaveGraphFileDialog.ShowDialogAndSaveGraph(oGraph);
        }
        catch (Exception oException)
        {
            this.ScreenUpdating = true;
            ErrorUtil.OnException(oException);
        }
        finally
        {
            this.ScreenUpdating = true;
            ShowWaitCursor = false;
        }
    }

    //*************************************************************************
    //  Method: ImportGraphWithEdgeWeight()
    //
    /// <summary>
    /// Imports a graph's edges and vertices into the workbook, including edge
    /// weights if available.
    /// </summary>
    ///
    /// <param name="oGraph">
    /// The graph to import.
    /// </param>
    ///
    /// <param name="sImportDescription">
    /// Description of the technique that was used to import the graph.  Can be
    /// empty or null.
    /// </param>
    ///
    /// <param name="sSuggestedFileNameNoExtension">
    /// File name suggested for the NodeXL workbook, without a path or
    /// extension.  Can be empty or null.
    /// </param>
    ///
    /// <remarks>
    /// If the graph's edges have weights, specified with the <see
    /// cref="ReservedMetadataKeys.EdgeWeight" /> metadata key, the edge
    /// weights get imported into the workbook.
    /// </remarks>
    //*************************************************************************

    private void
    ImportGraphWithEdgeWeight
    (
        IGraph oGraph,
        String sImportDescription,
        String sSuggestedFileNameNoExtension
    )
    {
        Debug.Assert(oGraph != null);
        AssertValid();

        // To accommodate the requirements of the general-purpose ImportGraph()
        // method, duplicate the standard ReservedMetadataKeys.EdgeWeight key,
        // which is some arbitrary string, with the name of the Excel column
        // name where the key's values should get written.

        foreach (IEdge oEdge in oGraph.Edges)
        {
            Object oEdgeWeight;

            if ( oEdge.TryGetValue(ReservedMetadataKeys.EdgeWeight,
                typeof(Double), out oEdgeWeight) )
            {
                oEdge.SetValue(EdgeTableColumnNames.EdgeWeight,
                    (Double)oEdgeWeight);
            }
        }

        ImportGraph(oGraph,
            new String[] {EdgeTableColumnNames.EdgeWeight}, null,
            sImportDescription, sSuggestedFileNameNoExtension);
    }

    //*************************************************************************
    //  Method: ImportGraph()
    //
    /// <summary>
    /// Imports a graph's edges and vertices into the workbook.
    /// </summary>
    ///
    /// <param name="oGraph">
    /// The graph to import.
    /// </param>
    ///
    /// <param name="oEdgeAttributes">
    /// Array of edge attribute keys that have been added to the metadata of
    /// the graph's vertices.  Can be null.
    /// </param>
    ///
    /// <param name="oVertexAttributes">
    /// Array of vertex attribute keys that have been added to the metadata of
    /// the graph's vertices.  Can be null.
    /// </param>
    ///
    /// <param name="sImportDescription">
    /// Description of the technique that was used to import the graph.  Can be
    /// empty or null.
    /// </param>
    ///
    /// <param name="sSuggestedFileNameNoExtension">
    /// File name suggested for the NodeXL workbook, without a path or
    /// extension.  Can be empty or null.
    /// </param>
    //*************************************************************************

    private void
    ImportGraph
    (
        IGraph oGraph,
        String [] oEdgeAttributes,
        String [] oVertexAttributes,
        String sImportDescription,
        String sSuggestedFileNameNoExtension
    )
    {
        Debug.Assert(oGraph != null);
        AssertValid();

        // Turn off text wrap if necessary to speed up the import.

        if ( !GraphImportTextWrapManager.ManageTextWrapBeforeImport(
            oGraph, this.InnerObject, true) )
        {
            return;
        }

        this.ScreenUpdating = false;

        try
        {
            GraphImporter.ImportGraph(oGraph, oEdgeAttributes,
                oVertexAttributes,
                ( new ImportDataUserSettings() ).ClearTablesBeforeImport,
                this.InnerObject);

            this.ScreenUpdating = true;
        }
        catch (Exception oException)
        {
            this.ScreenUpdating = true;
            ErrorUtil.OnException(oException);
            return;
        }

        GraphDirectedness eGraphDirectedness = GraphDirectedness.Undirected;

        switch (oGraph.Directedness)
        {
            case GraphDirectedness.Undirected:

                break;

            case GraphDirectedness.Directed:

                eGraphDirectedness = GraphDirectedness.Directed;
                break;

            case GraphDirectedness.Mixed:

                FormUtil.ShowInformation( String.Format(

                    "The file contains both undirected and directed edges,"
                    + " which {0} does not allow.  All edges are being"
                    + " converted to directed edges."
                    ,
                    FormUtil.ApplicationName
                    ) );

                eGraphDirectedness = GraphDirectedness.Directed;
                break;

            default:

                Debug.Assert(false);
                break;
        }

        GraphImporter.UpdateGraphHistoryAfterImport(this.InnerObject,
            sImportDescription, sSuggestedFileNameNoExtension);

        this.GraphDirectedness = eGraphDirectedness;
        this.Ribbon.GraphDirectedness = eGraphDirectedness;

        AutomateTasksAfterImport();
    }

    //*************************************************************************
    //  Method: AutomateThisWorkbook()
    //
    /// <summary>
    /// Runs task automation on the workbook.
    /// </summary>
    //*************************************************************************

    private void
    AutomateThisWorkbook()
    {
        AssertValid();

        // The TaskPane does the work, because it has access to the
        // NodeXLControl needed for some automation tasks.

        CommandDispatcher.SendNoParamCommand(this,
            NoParamCommand.AutomateThisWorkbook);
    }

    //*************************************************************************
    //  Method: RunGroupCommand()
    //
    /// <summary>
    /// Runs a group command.
    /// </summary>
    ///
    /// <param name="groupCommand">
    /// One of the flags in the <see cref="GroupCommands" /> enumeration.
    /// </param>
    //*************************************************************************

    private void
    RunGroupCommand
    (
        GroupCommands groupCommand
    )
    {
        AssertValid();

        if ( !this.ExcelApplicationIsReady(true) )
        {
            return;
        }

        // Various worksheets must be activated for reading and writing.  Save
        // the active worksheet state.

        ExcelActiveWorksheetRestorer oExcelActiveWorksheetRestorer =
            new ExcelActiveWorksheetRestorer(this.InnerObject);

        ExcelActiveWorksheetState oExcelActiveWorksheetState =
            oExcelActiveWorksheetRestorer.GetActiveWorksheetState();

        try
        {
            // Tell the GroupManager to update the workbook as necessary.  Note
            // that the GroupManager does NOT update the graph pane.

            if ( !GroupManager.TryRunGroupCommand(groupCommand,
                this.InnerObject, Globals.Sheet2, Globals.Sheet5) )
            {
                return;
            }

            // Now update the graph pane and perform other tasks as necessary.

            switch (groupCommand)
            {
                case GroupCommands.CollapseSelectedGroups:
                case GroupCommands.ExpandSelectedGroups:

                    CollapseOrExpandGroups(
                        groupCommand == GroupCommands.CollapseSelectedGroups,
                        Globals.Sheet5.GetSelectedGroupNames() );

                    break;

                case GroupCommands.CollapseAllGroups:
                case GroupCommands.ExpandAllGroups:

                    ICollection<String> oUniqueGroupNames;

                    if (ExcelTableUtil.TryGetUniqueTableColumnStringValues(
                            this.InnerObject, WorksheetNames.Groups,
                            TableNames.Groups, GroupTableColumnNames.Name,
                            out oUniqueGroupNames) )
                    {
                        CollapseOrExpandGroups(
                            groupCommand == GroupCommands.CollapseAllGroups,
                            oUniqueGroupNames);
                    }

                    break;

                case GroupCommands.RemoveAllGroups:

                    // Update the graph's history.

                    this.PerWorkbookSettings.
                        SetGraphHistoryGroupingDescription(String.Empty);

                    break;

                default:

                    break;
            }
        }
        catch (Exception oException)
        {
            ErrorUtil.OnException(oException);
        }
        finally
        {
            oExcelActiveWorksheetRestorer.Restore(oExcelActiveWorksheetState);
        }
    }

    //*************************************************************************
    //  Method: GroupByVertexAttribute()
    //
    /// <summary>
    /// Partitions the graph into groups based on the values in a vertex
    /// worksheet column.
    /// </summary>
    //*************************************************************************

    private void
    GroupByVertexAttribute()
    {
        AssertValid();

        CreateGroupsWithDialog(
            new GroupByVertexAttributeDialog(this.InnerObject)
            );
    }

    //*************************************************************************
    //  Method: CalculateConnectedComponents()
    //
    /// <summary>
    /// Partitions the graph into connected components.
    /// </summary>
    ///
    /// <returns>
    /// true if the graph was successfully partitioned into connected
    /// components.
    /// </returns>
    //*************************************************************************

    private Boolean
    CalculateConnectedComponents()
    {
        AssertValid();

        return ( CalculateGroups(new ConnectedComponentCalculator2(),
            "Finding Connected Components", "by connected component")
            );
    }

    //*************************************************************************
    //  Method: GroupByCluster()
    //
    /// <summary>
    /// Partitions the graph into groups based on clusters.
    /// </summary>
    ///
    /// <param name="bEditClusterUserSettings">
    /// true to allow the user to edit the cluster user settings before the
    /// graph is partitioned.
    /// </param>
    ///
    /// <returns>
    /// true if the graph was successfully partitioned into clusters.
    /// </returns>
    //*************************************************************************

    private Boolean
    GroupByCluster
    (
        Boolean bEditClusterUserSettings
    )
    {
        AssertValid();

        ClusterUserSettings oClusterUserSettings = new ClusterUserSettings();

        if (bEditClusterUserSettings)
        {
            ClusterUserSettingsDialog oDialog = new ClusterUserSettingsDialog(
                ClusterUserSettingsDialog.DialogMode.Normal,
                oClusterUserSettings);

            if (oDialog.ShowDialog() == DialogResult.OK)
            {
                oClusterUserSettings.Save();
            }
            else
            {
                return (false);
            }
        }

        ClusterCalculator2 oClusterCalculator2 = new ClusterCalculator2();

        ClusterAlgorithm eClusterAlgorithm =
            oClusterUserSettings.ClusterAlgorithm;

        oClusterCalculator2.Algorithm = eClusterAlgorithm;

        oClusterCalculator2.PutNeighborlessVerticesInOneCluster =
            oClusterUserSettings.PutNeighborlessVerticesInOneCluster;

        String sClusterAlgorithm = EnumUtil.SplitName(
            eClusterAlgorithm.ToString(),
            EnumSplitStyle.AllWordsStartUpperCase);

        String sGroupingDescription = String.Format(
            "by cluster using the {0} cluster algorithm"
            ,
            sClusterAlgorithm.Replace(' ', '-')
            );

        return ( CalculateGroups(oClusterCalculator2, "Finding Clusters",
            sGroupingDescription) );
    }

    //*************************************************************************
    //  Method: GroupByMotif()
    //
    /// <summary>
    /// Partitions the graph into groups based on motifs.
    /// </summary>
    //*************************************************************************

    private void
    GroupByMotif()
    {
        AssertValid();

        // Let the user select the motifs to group by.

        MotifUserSettings oMotifUserSettings = new MotifUserSettings();

        MotifUserSettingsDialog oDialog =
            new MotifUserSettingsDialog(oMotifUserSettings);

        if (oDialog.ShowDialog() == DialogResult.OK)
        {
            oMotifUserSettings.Save();
            MotifCalculator2 oMotifCalculator2 = new MotifCalculator2();
            Motifs eMotifsToCalculate = oMotifUserSettings.MotifsToCalculate;
            oMotifCalculator2.MotifsToCalculate = eMotifsToCalculate;

            oMotifCalculator2.DConnectorMinimumAnchorVertices =
                oMotifUserSettings.DConnectorMinimumAnchorVertices;

            oMotifCalculator2.DConnectorMaximumAnchorVertices =
                oMotifUserSettings.DConnectorMaximumAnchorVertices;

            oMotifCalculator2.CliqueMaximumMemberVertices =
                oMotifUserSettings.CliqueMaximumMemberVertices;

            oMotifCalculator2.CliqueMinimumMemberVertices =
                oMotifUserSettings.CliqueMinimumMemberVertices;

            String sGroupingDescription = null;

            switch (eMotifsToCalculate)
            {
                case Motifs.Fan:

                    sGroupingDescription = "by fan motif";
                    break;

                case Motifs.DConnector:

                    sGroupingDescription = "by D-connector motif";
                    break;

                case Motifs.Clique:

                    sGroupingDescription = "by clique motif";
                    break;

                case Motifs.Fan | Motifs.DConnector:

                    sGroupingDescription =
                        "by fan and D-connector motifs";

                    break;

                case Motifs.Fan | Motifs.Clique:

                    sGroupingDescription =
                        "by fan and clique motifs";

                    break;

                case Motifs.DConnector | Motifs.Clique:

                    sGroupingDescription =
                        "by D-connector and clique motifs";

                    break;

                case Motifs.Fan | Motifs.DConnector | Motifs.Clique:

                    sGroupingDescription =
                        "by fan, D-connector, and clique motifs";

                    break;

                default:

                    Debug.Assert(false);
                    break;
            }

            CalculateGroups(oMotifCalculator2, "Finding Motifs",
                sGroupingDescription);
        }
    }

    //*************************************************************************
    //  Method: CalculateGroups()
    //
    /// <summary>
    /// Group's the graph's vertices using a graph metric calculator.
    /// </summary>
    ///
    /// <param name="oGraphMetricCalculator">
    /// The IGraphMetricCalculator2 to use.
    /// </param>
    ///
    /// <param name="sDialogTitle">
    /// Title for the CalculateGraphMetricsDialog, or null to use a default
    /// title.
    /// </param>
    ///
    /// <param name="sGroupingDescription">
    /// Description of the grouping technique, suitable for storing in the
    /// graph's history.  Sample: "by fan motif".
    /// </param>
    ///
    /// <returns>
    /// true if the graph metrics were successfully calculated, or false if
    /// they were cancelled or the application isn't ready.
    /// </returns>
    //*************************************************************************

    private Boolean
    CalculateGroups
    (
        IGraphMetricCalculator2 oGraphMetricCalculator,
        String sDialogTitle,
        String sGroupingDescription
    )
    {
        Debug.Assert(oGraphMetricCalculator != null);
        Debug.Assert( !String.IsNullOrEmpty(sDialogTitle) );
        Debug.Assert( !String.IsNullOrEmpty(sGroupingDescription) );
        AssertValid();

        if ( CreateGroupsWithDialog(
            new CalculateGraphMetricsDialog(
                null, this.InnerObject,
                new IGraphMetricCalculator2[] {oGraphMetricCalculator},
                sDialogTitle, true
            ) ) )
        {
            // Update the graph's history.

            this.PerWorkbookSettings.SetGraphHistoryGroupingDescription(
                String.Format(
                    "The graph's vertices were grouped {0}."
                    ,
                    sGroupingDescription
                ) );

            return (true);
        }

        return (false);
    }

    //*************************************************************************
    //  Method: CollapseOrExpandGroups()
    //
    /// <summary>
    /// Collapses or expands a collection of groups.
    /// </summary>
    ///
    /// <param name="bCollapse">
    /// true to collapse the groups, false to expand them.
    /// </param>
    ///
    /// <param name="oGroupNames">
    /// Collection of group names, one for each group that needs to be
    /// collapsed or expanded.
    /// </param>
    //*************************************************************************

    private void
    CollapseOrExpandGroups
    (
        Boolean bCollapse,
        ICollection<String> oGroupNames
    )
    {
        Debug.Assert(oGroupNames != null);
        AssertValid();

        // Tell the TaskPane to collapse or expand the groups.

        CommandDispatcher.SendCommand( this,
            new RunCollapseOrExpandGroupsCommandEventArgs(
                bCollapse, oGroupNames) );
    }

    //*************************************************************************
    //  Method: CreateGroupsWithDialog()
    //
    /// <summary>
    /// Partitions the graph into groups using a dialog provided by the caller.
    /// </summary>
    ///
    /// <param name="oDialog">
    /// The dialog that will partition the graph into groups.  This method
    /// shows the dialog.
    /// </param>
    ///
    /// <returns>
    /// true if the graph was successfully partitioned into groups.
    /// </returns>
    //*************************************************************************

    private Boolean
    CreateGroupsWithDialog
    (
        Form oDialog
    )
    {
        Debug.Assert( this.ExcelApplicationIsReady(false) );
        Debug.Assert(oDialog != null);
        AssertValid();

        if (oDialog.ShowDialog() == DialogResult.OK)
        {
            // Make sure that groups will be read from the workbook.

            GroupUserSettings oGroupUserSettings = new GroupUserSettings();
            oGroupUserSettings.ReadGroups = true;
            oGroupUserSettings.Save();

            return (true);
        }

        return (false);
    }

    //*************************************************************************
    //  Method: EditGroupUserSettings()
    //
    /// <summary>
    /// Lets the user edit the group user settings.
    /// </summary>
    //*************************************************************************

    private void
    EditGroupUserSettings()
    {
        Debug.Assert( this.ExcelApplicationIsReady(false) );
        AssertValid();

        GroupUserSettings oGroupUserSettings = new GroupUserSettings();

        if ( ( new GroupUserSettingsDialog(oGroupUserSettings) ).ShowDialog()
            == DialogResult.OK )
        {
            oGroupUserSettings.Save();
        }
    }

    //*************************************************************************
    //  Method: MergeIsApproved()
    //
    /// <summary>
    /// Requests approval from the user to merge duplicate edges and perform
    /// other tasks.
    /// </summary>
    ///
    /// <param name="sMessageSuffix">
    /// Suffix to append to base approval message.
    /// </param>
    ///
    /// <returns>
    /// true if the user approved the merge.
    /// </returns>
    //*************************************************************************

    private Boolean
    MergeIsApproved
    (
        String sMessageSuffix
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sMessageSuffix) );
        AssertValid();

        String sMessage =
            "This will remove any filters that are applied to the Edges"
            + " worksheet, merge any duplicate edges, " + sMessageSuffix
            + "\r\n\r\n"
            + "Do you want to continue?"
            ;

        return (MessageBox.Show(sMessage, FormUtil.ApplicationName,
            MessageBoxButtons.YesNo, MessageBoxIcon.Information)
            == DialogResult.Yes);
    }

    //*************************************************************************
    //  Method: OnTasksAutomatedOnOpen()
    //
    /// <summary>
    /// Gets called after task automation has been run on the workbook
    /// immediately after it was opened.
    /// </summary>
    //*************************************************************************

    private void
    OnTasksAutomatedOnOpen()
    {
        AssertValid();

        // Make sure the workbook won't be automated again the next time it is
        // opened.

        Debug.Assert(!this.PerWorkbookSettings.AutomateTasksOnOpen);

        this.Save();

        // The workbook and Excel need to be closed.  If this method is being
        // called from the GraphLaidOut event, that shouldn't be done before
        // the event completes.  Use a timer to delay the closing.
        //
        // The timer interval isn't critical, because Windows gives low
        // priority to timer events in its message queue, allowing the event
        // to complete before the timer is serviced.

        Timer oTimer = new Timer();
        oTimer.Interval = 1;

        oTimer.Tick += delegate(Object sender2, EventArgs e2)
        {
            oTimer.Stop();
            oTimer = null;

            // Unfortunately, calling Application.Quit() directly after closing
            // the workbook doesn't do anything -- an empty Excel window
            // remains.  It does close the application if done in the
            // workbook's Shutdown event, however, so set a flag that the
            // Shutdown event handler will detect.
            //
            // (This could be implemented inline using another delegate, this
            // one on the Shutdown event, but the flag makes things more
            // explicit.)

            m_bCloseExcelWhenWorkbookCloses = true;

            this.Close(false, System.Reflection.Missing.Value,
                System.Reflection.Missing.Value);
        };

        oTimer.Start();
    }

    //*************************************************************************
    //  Method: FireVisualAttributeSetInWorkbook()
    //
    /// <summary>
    /// Fires the <see cref="VisualAttributeSetInWorkbook" /> event if
    /// appropriate.
    /// </summary>
    //*************************************************************************

    private void
    FireVisualAttributeSetInWorkbook()
    {
        AssertValid();

        // The TaskPane handles this event by reading the workbook, which
        // clears the selection in the graph.  That fires the TaskPane's
        // SelectionChangedInGraph event, which SelectionCoorindator handles by
        // clearing the selection in the workbook.  That can be jarring to the
        // user, who just made a selection, set a visual attribute on the
        // selection, and then saw the selection disappear.
        //
        // Fix this by temporarily igoring the SelectionChangedInGraph event,
        // which causes the selection in the workbook to be retained.  The
        // selection in the graph (which is empty) will then be out of sync
        // with the selection in the workbook, but I think that is tolerable.

        if (m_oSelectionCoordinator != null)
        {
            m_oSelectionCoordinator.IgnoreSelectionEvents = true;
        }

        EventUtil.FireEvent(this, this.VisualAttributeSetInWorkbook);

        if (m_oSelectionCoordinator != null)
        {
            m_oSelectionCoordinator.IgnoreSelectionEvents = false;
        }
    }

    //*************************************************************************
    //  Method: ForwardEvent()
    //
    /// <summary>
    /// Forwards an event.
    /// </summary>
    ///
    /// <typeparam name="TEventArgs">
    /// The type of the event arguments.
    /// </typeparam>
    ///
    /// <param name="e">
    /// The event arguments.
    /// </param>
    ///
    /// <param name="oEventHandler">
    /// The event handler that handles the event fired by this class, or null
    /// if no clients have subscribed to the event.
    /// </param>
    ///
    /// <remarks>
    /// This class handles several events by forwarding them to other clients.
    /// The <see cref="TaskPane.GraphLaidOut" /> event, for example, is handled
    /// by this class, then forwarded to various worksheets via this class's
    /// <see cref="GraphLaidOut" /> event.  This method does the forwarding,
    /// assuming the Excel application is ready.
    /// </remarks>
    //*************************************************************************

    private void
    ForwardEvent<TEventArgs>
    (
        TEventArgs e,
        EventHandler<TEventArgs> oEventHandler
    )
    where TEventArgs : EventArgs
    {
        Debug.Assert(e != null);
        AssertValid();

        if ( this.ExcelApplicationIsReady(false) )
        {
            try
            {
                EventUtil.FireEvent<TEventArgs>(this, e, oEventHandler);
            }
            catch (Exception oException)
            {
                ErrorUtil.OnException(oException);
            }
        }
    }

    //*************************************************************************
    //  Method: Workbook_Startup()
    //
    /// <summary>
    /// Handles the Startup event on the workbook.
    /// </summary>
    ///
    /// <param name="sender">
    /// Standard event argument.
    /// </param>
    ///
    /// <param name="e">
    /// Standard event argument.
    /// </param>
    //*************************************************************************

    private void
    ThisWorkbook_Startup
    (
        object sender,
        System.EventArgs e
    )
    {
        ApplicationUtil.OnWorkbookStartup(this.Application);

        m_bTaskPaneCreated = false;
        m_bCloseExcelWhenWorkbookCloses = false;
        m_oAutoFillWorkbookDialog = null;

        // If this is an older NodeXL workbook, update the worksheets, tables,
        // and columns if neccessary.

        NodeXLWorkbookUpdater.UpdateWorkbook(this.InnerObject);

        Sheet1 oSheet1 = Globals.Sheet1;
        Sheet2 oSheet2 = Globals.Sheet2;
        Sheet5 oSheet5 = Globals.Sheet5;

        m_oWorksheetContextMenuManager = new WorksheetContextMenuManager(
            this, oSheet1, oSheet1.Edges, oSheet2, oSheet2.Vertices, oSheet5,
            oSheet5.Groups);

        // In message boxes, show the name of this document customization
        // instead of the default, which is the name of the Excel application.

        FormUtil.ApplicationName = ApplicationUtil.ApplicationName;

        this.New += new
            Microsoft.Office.Tools.Excel.WorkbookEvents_NewEventHandler(
                ThisWorkbook_New);

        this.ActivateEvent += new
            Microsoft.Office.Interop.Excel.WorkbookEvents_ActivateEventHandler(
                ThisWorkbook_ActivateEvent);

        this.SheetActivate += new WorkbookEvents_SheetActivateEventHandler(
            ThisWorkbook_SheetActivate);

        this.BeforeSave +=
            new WorkbookEvents_BeforeSaveEventHandler(ThisWorkbook_BeforeSave);

        if (!this.PerWorkbookSettings.HasWorkbookSettings)
        {
            // This is a new workbook, or it's an old saved workbook created
            // in an earlier version of the program.  Perform settings-related
            // tasks required when a new workbook is created.
            //
            // This must be done here rather than in ThisWorkbook_New() because
            // setting the GraphVisibility property below creates the TaskPane,
            // and the TaskPane requires that user settings be already
            // initialized.

            ( new NodeXLApplicationSettingsBase() ).OnNewWorkbook();
        }

        CommandDispatcher.CommandSent +=
            new RunCommandEventHandler(this.CommandDispatcher_CommandSent);

        // Show the NodeXL graph by default.

        this.GraphVisibility = true;

        ShowAndHideColumnGroups();

        AssertValid();
    }

    //*************************************************************************
    //  Method: ThisWorkbook_New()
    //
    /// <summary>
    /// Handles the New event on the workbook.
    /// </summary>
    //*************************************************************************

    private void
    ThisWorkbook_New()
    {
        AssertValid();

        // Get the graph directedness for new workbooks and store it in the
        // per-workbook settings.

        GeneralUserSettings oGeneralUserSettings = new GeneralUserSettings();

        this.PerWorkbookSettings.GraphDirectedness =
            oGeneralUserSettings.NewWorkbookGraphDirectedness;
    }

    //*************************************************************************
    //  Method: ThisWorkbook_ActivateEvent()
    //
    /// <summary>
    /// Handles the ActivateEvent event on the workbook.
    /// </summary>
    //*************************************************************************

    private void
    ThisWorkbook_ActivateEvent()
    {
        AssertValid();

        // Update the Ribbon.

        this.Ribbon.GraphDirectedness = this.GraphDirectedness;
        EnableSetVisualAttributes();
    }

    //*************************************************************************
    //  Method: ThisWorkbook_SheetActivate()
    //
    /// <summary>
    /// Handles the SheetActivate event on the workbook.
    /// </summary>
    ///
    /// <param name="Sh">
    /// The activated sheet.
    /// </param>
    //*************************************************************************

    private void
    ThisWorkbook_SheetActivate
    (
        object Sh
    )
    {
        AssertValid();
        Debug.Assert(Sh != null);

        EnableSetVisualAttributes();
    }

    //*************************************************************************
    //  Method: ThisWorkbook_BeforeSave()
    //
    /// <summary>
    /// Handles the BeforeSave event on the workbook.
    /// </summary>
    ///
    /// <param name="saveAsUI">
    /// Standard event argument.
    /// </param>
    ///
    /// <param name="cancel">
    /// Standard event argument.
    /// </param>
    //*************************************************************************

    private void
    ThisWorkbook_BeforeSave
    (
        Boolean saveAsUI,
        ref Boolean cancel
    )
    {
        AssertValid();

        // In most cases, the Ribbon and TaskPane should save their user
        // settings in the workbook before the workbook is closed.  However,
        // when a folder of workbooks is automated by
        // TaskAutomator.AutomateFolder(), a set of workbook settings is stored
        // in each workbook by TaskAutomator, and these settings should not be
        // overwritten.

        if (!this.PerWorkbookSettings.AutomateTasksOnOpen)
        {
            CommandDispatcher.SendNoParamCommand(this,
                NoParamCommand.SaveUserSettings);
        }
    }

    //*************************************************************************
    //  Method: ThisWorkbook_Shutdown()
    //
    /// <summary>
    /// Handles the Shutdown event on the workbook.
    /// </summary>
    ///
    /// <param name="sender">
    /// Standard event argument.
    /// </param>
    ///
    /// <param name="e">
    /// Standard event argument.
    /// </param>
    //*************************************************************************

    private void
    ThisWorkbook_Shutdown
    (
        object sender,
        System.EventArgs e
    )
    {
        AssertValid();

        ApplicationUtil.OnWorkbookShutdown();

        if (m_bCloseExcelWhenWorkbookCloses)
        {
            this.Application.Quit();
        }
    }

    //*************************************************************************
    //  Method: CommandDispatcher_CommandSent()
    //
    /// <summary>
    /// Handles the CommandSent event on the <see cref="CommandDispatcher" />
    /// static class.
    /// </summary>
    ///
    /// <param name="sender">
    /// Standard event argument.
    /// </param>
    ///
    /// <param name="e">
    /// Standard event argument.
    /// </param>
    ///
    /// <remarks>
    /// See the comments in <see cref="RunCommandEventArgs" /> for information
    /// on how commands are sent from one UI object to another in NodeXL.
    /// </remarks>
    //*************************************************************************

    private void
    CommandDispatcher_CommandSent
    (
        Object sender,
        RunCommandEventArgs e
    )
    {
        Debug.Assert(e != null);
        AssertValid();

        // To prevent multiple "busy" messages from being displayed, this class
        // passes true for the showBusyMessage argument to
        // ExcelApplicationIsReady(), while Sheet1, Sheet2 and Sheet5, all of
        // which have their own OnRunCommand() methods that call
        // ExcelApplicationIsReady(), pass false.

        if ( !this.ExcelApplicationIsReady(true) )
        {
            return;
        }

        if (e is RunNoParamCommandEventArgs)
        {
            switch ( ( (RunNoParamCommandEventArgs)e ).NoParamCommand )
            {
                case NoParamCommand.GroupByVertexAttribute:

                    GroupByVertexAttribute();
                    break;

                case NoParamCommand.CalculateConnectedComponents:

                    CalculateConnectedComponents();
                    break;

                case NoParamCommand.GroupByMotif:

                    GroupByMotif();
                    break;

                case NoParamCommand.EditGroupUserSettings:

                    EditGroupUserSettings();
                    break;

                case NoParamCommand.OpenHomePage:

                    Process.Start(ProjectInformation.HomePageUrl);
                    break;

                case NoParamCommand.OpenDiscussionPage:

                    Process.Start(ProjectInformation.DiscussionPageUrl);
                    break;

                case NoParamCommand.OpenNodeXLGraphGallery:

                    Process.Start(ProjectInformation.NodeXLGraphGalleryUrl);
                    break;

                case NoParamCommand.ShowAndHideColumnGroups:

                    ShowAndHideColumnGroups();
                    break;

                case NoParamCommand.EditImportDataUserSettings:

                    EditImportDataUserSettings();
                    break;

                case NoParamCommand.ImportFromMatrixWorkbook:

                    ImportFromMatrixWorkbook();
                    break;

                case NoParamCommand.ImportFromWorkbook:

                    ImportFromWorkbook();
                    break;

                case NoParamCommand.ExportSelectionToNewNodeXLWorkbook:

                    ExportSelectionToNewNodeXLWorkbook();
                    break;

                case NoParamCommand.ExportToUcinetFile:

                    ExportToUcinetFile();
                    break;

                case NoParamCommand.ExportToGraphMLFile:

                    ExportToGraphMLFile();
                    break;

                case NoParamCommand.ExportToPajekFile:

                    ExportToPajekFile();
                    break;

                case NoParamCommand.ExportToNewMatrixWorkbook:

                    ExportToNewMatrixWorkbook();
                    break;

                case NoParamCommand.AggregateGraphMetrics:

                    AggregateGraphMetrics();
                    break;

                case NoParamCommand.DeleteSubgraphThumbnails:

                    DeleteSubgraphThumbnails();
                    break;

                case NoParamCommand.AnalyzeEmailNetwork:

                    AnalyzeEmailNetwork();
                    break;

                case NoParamCommand.ImportFromUcinetFile:

                    ImportFromUcinetFile();
                    break;

                case NoParamCommand.ImportFromGraphMLFile:

                    ImportFromGraphMLFile();
                    break;

                case NoParamCommand.ImportFromGraphMLFiles:

                    ImportFromGraphMLFiles();
                    break;

                case NoParamCommand.ImportFromPajekFile:

                    ImportFromPajekFile();
                    break;

                case NoParamCommand.ConvertNodeXLWorkbook:

                    ConvertNodeXLWorkbook();
                    break;

                case NoParamCommand.RegisterUser:

                    RegisterUser();
                    break;

                case NoParamCommand.ShowGraphAndReadWorkbook:

                    ShowGraphAndReadWorkbook();
                    break;

                case NoParamCommand.ShowGraphMetrics:

                    ShowGraphMetrics();
                    break;

                case NoParamCommand.CreateSubgraphImages:

                    CreateSubgraphImages(
                        CreateSubgraphImagesDialog.DialogMode.Normal);

                    break;

                case NoParamCommand.AutoFillWorkbook:

                    AutoFillWorkbook(AutoFillWorkbookDialog.DialogMode.Normal);
                    break;

                case NoParamCommand.CreateNodeXLWorkbook:

                    ApplicationUtil.CreateNodeXLWorkbook(this.Application);
                    break;

                case NoParamCommand.ShowGraphSummary:

                    ShowGraphSummary();
                    break;

                default:

                    break;
            }
        }
        else if (e is RunMergeDuplicateEdgesCommandEventArgs)
        {
            RunMergeDuplicateEdgesCommandEventArgs
                oRunMergeDuplicateEdgesCommandEventArgs =
                (RunMergeDuplicateEdgesCommandEventArgs)e;

            if ( MergeDuplicateEdges(
                oRunMergeDuplicateEdgesCommandEventArgs.EditUserSettings) )
            {
                oRunMergeDuplicateEdgesCommandEventArgs.CommandSuccessfullyRun
                    = true;
            }
        }
        else if (e is RunGroupCommandEventArgs)
        {
            RunGroupCommand( ( (RunGroupCommandEventArgs)e ).GroupCommand );
        }
        else if (e is RunGroupByClusterCommandEventArgs)
        {
            RunGroupByClusterCommandEventArgs
                oRunGroupByClusterCommandEventArgs =
                (RunGroupByClusterCommandEventArgs)e;

            if ( GroupByCluster(
                oRunGroupByClusterCommandEventArgs.EditUserSettings) )
            {
                oRunGroupByClusterCommandEventArgs.CommandSuccessfullyRun
                    = true;
            }
        }
    }


    #region VSTO Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InternalStartup()
    {
        this.Startup += new System.EventHandler(ThisWorkbook_Startup);

        this.Shutdown += new System.EventHandler(ThisWorkbook_Shutdown);
    }
        
    #endregion


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
        Debug.Assert(m_oWorksheetContextMenuManager != null);
        // m_bTaskPaneCreated
        Debug.Assert(!m_bTaskPaneCreated || m_oSelectionCoordinator != null);
        // m_bCloseExcelWhenWorkbookCloses
        // m_oAutoFillWorkbookDialog
    }


    //*************************************************************************
    //  Private fields
    //*************************************************************************

    /// Object that adds custom menu items to the Excel context menus that
    /// appear when the vertex or edge table is right-clicked.

    private WorksheetContextMenuManager m_oWorksheetContextMenuManager;

    /// true if the task pane has been created.

    private Boolean m_bTaskPaneCreated;

    /// If m_bTaskPaneCreated is true, this is the object that coordinates the
    /// edge and vertex selection between the workbook and the TaskPane.  It is
    /// null otherwise.

    private SelectionCoordinator m_oSelectionCoordinator;

    /// true to close Excel when the workbook closes.

    private Boolean m_bCloseExcelWhenWorkbookCloses;

    /// Modeless dialog for autofilling the workbook.  The dialog is created on
    /// demand, so this is initially null, then gets set to a dialog when the
    /// user wants to autofill the workbook, then gets reset to null when the
    /// user closes the dialog.

    private AutoFillWorkbookDialog m_oAutoFillWorkbookDialog;
}

}
