

using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Office.Interop.Excel;
using Microsoft.Office.Tools.Ribbon;
using Smrf.NodeXL.Common;
using Smrf.NodeXL.Core;
using Smrf.NodeXL.Algorithms;
using Smrf.NodeXL.Visualization.Wpf;
using Smrf.NodeXL.ApplicationUtil;
using Microsoft.NodeXL.ExcelTemplatePlugIns;
using Smrf.AppLib;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: Ribbon
//
/// <summary>
/// Represents the template's ribbon tab.
/// </summary>
///
/// <remarks>
/// How the Ribbon is Connected to the Rest of the Application
///
/// <para>
/// The ribbon has direct access to the public methods and properties on the
/// ThisWorkbook object.  It does not have direct access to the TaskPane.
/// </para>
///
/// <para>
/// When the user clicks a ribbon control that is best handled by ThisWorkbook,
/// the ribbon calls the appropriate public method on ThisWorkbook, passing the
/// method whatever arguments are required to perform the requested task.
/// </para>
///
/// <para>
/// When the user clicks a ribbon control that is best handled by the TaskPane,
/// the ribbon fires an event that gets handled by the TaskPane.  The
/// arguments required to perform the requested task get passed to the TaskPane
/// via event arguments.
/// </para>
///
/// </remarks>
//*****************************************************************************

public partial class Ribbon : RibbonBase
{
    //*************************************************************************
    //  Constructor: Ribbon()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="Ribbon" /> class.
    /// </summary>
    //*************************************************************************

    public Ribbon()
    :
    base( Globals.Factory.GetRibbonFactory() )
    {
        InitializeComponent();

        btnGetExchangeGraphDataProvider.Tag =
            ProjectInformation.ExchangeGraphDataProviderUrl;

        btnGetMediaWikiGraphDataProvider.Tag =
            ProjectInformation.MediaWikiGraphDataProviderUrl;

        btnGetOnaSurveysGraphDataProvider.Tag =
            ProjectInformation.OnaSurveysGraphDataProviderUrl;

        btnGetSocialNetworkGraphDataProvider.Tag =
            ProjectInformation.SocialNetworkGraphDataProviderUrl;

        btnGetVosonGraphDataProvider.Tag =
            ProjectInformation.VosonGraphDataProviderUrl;

        btnDonate.Tag = ProjectInformation.DonateUrl;

        // Populate the rddLayout RibbonDropDown.

        m_oLayoutManagerForRibbonDropDown =
            new LayoutManagerForRibbonDropDown();

        m_oLayoutManagerForRibbonDropDown.AddRibbonDropDownItems(
            this.rddLayout);

        m_oLayoutManagerForRibbonDropDown.LayoutChanged += new
            EventHandler(this.m_oLayoutManagerForRibbonDropDown_LayoutChanged);

        AssertValid();
    }

    //*************************************************************************
    //  Property: ReadVertexLabels
    //
    /// <summary>
    /// Gets or sets a flag indicating whether vertex labels should be read
    /// from the vertex worksheet.
    /// </summary>
    ///
    /// <value>
    /// true to read vertex labels.
    /// </value>
    //*************************************************************************

    public Boolean
    ReadVertexLabels
    {
        get
        {
            AssertValid();

            return (chkReadVertexLabels.Checked);
        }

        set
        {
            chkReadVertexLabels.Checked = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: ReadEdgeLabels
    //
    /// <summary>
    /// Gets or sets a flag indicating whether edge labels should be read from
    /// the edge worksheet.
    /// </summary>
    ///
    /// <value>
    /// true to read edge labels.
    /// </value>
    //*************************************************************************

    public Boolean
    ReadEdgeLabels
    {
        get
        {
            AssertValid();

            return (chkReadEdgeLabels.Checked);
        }

        set
        {
            chkReadEdgeLabels.Checked = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: ReadGroupLabels
    //
    /// <summary>
    /// Gets or sets a flag indicating whether group labels should be read from
    /// the group worksheet.
    /// </summary>
    ///
    /// <value>
    /// true to read group labels.
    /// </value>
    //*************************************************************************

    public Boolean
    ReadGroupLabels
    {
        get
        {
            AssertValid();

            return (chkReadGroupLabels.Checked);
        }

        set
        {
            chkReadGroupLabels.Checked = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: GraphDirectedness
    //
    /// <summary>
    /// Gets or sets the graph directedness of the active workbook.
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

            // The user selects the directedness with the rddGraphDirectedness
            // RibbonDropDown.  The GraphDirectedness for each item is stored
            // in the item's Tag.

            return ( (GraphDirectedness)rddGraphDirectedness.SelectedItem.Tag );
        }

        set
        {
            foreach (RibbonDropDownItem oItem in rddGraphDirectedness.Items)
            {
                if ( (GraphDirectedness)oItem.Tag == value )
                {
                    rddGraphDirectedness.SelectedItem = oItem;

                    break;
                }
            }

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: Layout
    //
    /// <summary>
    /// Gets or sets the layout type to use.
    /// </summary>
    ///
    /// <value>
    /// The layout type to use, as a <see cref="LayoutType" />.
    /// </value>
    //*************************************************************************

    public LayoutType
    Layout
    {
        get
        {
            AssertValid();

            return (m_oLayoutManagerForRibbonDropDown.Layout);
        }

        set
        {
            m_oLayoutManagerForRibbonDropDown.Layout = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: ReadWorkbookButtonText
    //
    /// <summary>
    /// Sets the text for the read workbook button.
    /// </summary>
    ///
    /// <value>
    /// The text for the read workbook button.
    /// </value>
    //*************************************************************************

    public String
    ReadWorkbookButtonText
    {
        set
        {
            btnReadWorkbook.Label = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: EnableShowDynamicFilters
    //
    /// <summary>
    /// Sets a flag indicating whether the "show dynamic filters" button should
    /// be enabled.
    /// </summary>
    ///
    /// <value>
    /// true to enable the dynamic filters button.
    /// </value>
    //*************************************************************************

    public Boolean
    EnableShowDynamicFilters
    {
        set
        {
            btnShowDynamicFilters.Enabled = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Method: EnableSetVisualAttributes
    //
    /// <summary>
    /// Enables the "set visual attribute" buttons.
    /// </summary>
    ///
    /// <param name="visualAttributes">
    /// ORed flags specifying which buttons to enable.
    /// </param>
    //*************************************************************************

    public void
    EnableSetVisualAttributes
    (
        VisualAttributes visualAttributes
    )
    {
        AssertValid();

        this.btnSetColor.Enabled =
            ( (visualAttributes & VisualAttributes.Color) != 0 );

        this.btnSetAlpha.Enabled =
            ( (visualAttributes & VisualAttributes.Alpha) != 0 );

        this.btnSetEdgeWidth.Enabled =
            ( (visualAttributes & VisualAttributes.EdgeWidth) != 0 );

        this.mnuSetVertexShape.Enabled =
            ( (visualAttributes & VisualAttributes.VertexShape) != 0 );

        this.btnSetVertexRadius.Enabled =
            ( (visualAttributes & VisualAttributes.VertexRadius) != 0 );

        // The mnuSetVisibility menu has a set of child buttons for edge
        // visibility and another set for vertex visibility.  Show only one
        // of the sets.

        Boolean bShowSetEdgeVisibility =
            ( (visualAttributes & VisualAttributes.EdgeVisibility) != 0 );

        Boolean bShowSetVertexVisibility =
            ( (visualAttributes & VisualAttributes.VertexVisibility) != 0 );

        this.mnuSetVisibility.Enabled =
            (bShowSetEdgeVisibility || bShowSetVertexVisibility);

        this.btnSetEdgeVisibilityShow.Visible =
            this.btnSetEdgeVisibilitySkip.Visible =
            this.btnSetEdgeVisibilityHide.Visible =
            bShowSetEdgeVisibility;

        this.btnSetVertexVisibilityShowIfInAnEdge.Visible =
            this.btnSetVertexVisibilitySkip.Visible =
            this.btnSetVertexVisibilityHide.Visible =
            this.btnSetVertexVisibilityShow.Visible =
            bShowSetVertexVisibility;
    }

    //*************************************************************************
    //  Property: ShowGraphLegend
    //
    /// <summary>
    /// Gets or sets a flag indicating whether the graph legend should be shown
    /// in the graph pane.
    /// </summary>
    ///
    /// <value>
    /// true to show the graph legend in the graph pane.
    /// </value>
    //*************************************************************************

    protected Boolean
    ShowGraphLegend
    {
        get
        {
            AssertValid();

            return (chkShowGraphLegend.Checked);
        }

        set
        {
            chkShowGraphLegend.Checked = value;

            // Notify the TaskPane.

            SendNoParamCommand(value ? NoParamCommand.ShowGraphLegend
                : NoParamCommand.HideGraphLegend);

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: ShowGraphAxes
    //
    /// <summary>
    /// Gets or sets a flag indicating whether the graph axes should be shown
    /// in the graph pane.
    /// </summary>
    ///
    /// <value>
    /// true to show the graph axes in the graph pane.
    /// </value>
    //*************************************************************************

    protected Boolean
    ShowGraphAxes
    {
        get
        {
            AssertValid();

            return (chkShowGraphAxes.Checked);
        }

        set
        {
            chkShowGraphAxes.Checked = value;

            // Notify the TaskPane.

            SendNoParamCommand(value ? NoParamCommand.ShowGraphAxes
                : NoParamCommand.HideGraphAxes);

            AssertValid();
        }
    }

    //*************************************************************************
    //  Method: AutomateTasksOnOpen()
    //
    /// <summary>
    /// Runs task automation on the workbook immediately after it is opened.
    /// </summary>
    //*************************************************************************

    private void
    AutomateTasksOnOpen()
    {
        AssertValid();

        // TaskAutomator.AutomateFolder() is automating this workbook.

        this.ThisWorkbook.AutomateTasksOnOpen();
    }

    //*************************************************************************
    //  Method: OnWorkbookSettingsReplaced()
    //
    /// <summary>
    /// Performs tasks required when this workbook's settings are replaced with
    /// other settings.
    /// </summary>
    //*************************************************************************

    protected void
    OnWorkbookSettingsReplaced()
    {
        AssertValid();

        SendLoadUserSettingsCommand();
        this.Layout = ( new LayoutUserSettings() ).Layout;

        SendNoParamCommand(NoParamCommand.ShowAndHideColumnGroups);
    }

    //*************************************************************************
    //  Property: ThisWorkbook
    //
    /// <summary>
    /// Gets the workbook this ribbon is attached to.
    /// </summary>
    ///
    /// <value>
    /// The workbook this ribbon is attached to.
    /// </value>
    //*************************************************************************

    protected ThisWorkbook
    ThisWorkbook
    {
        get
        {
            AssertValid();

            return (Globals.ThisWorkbook);
        }
    }

    //*************************************************************************
    //  Method: SendLoadUserSettingsCommand()
    //
    /// <summary>
    /// Sends a command to load user settings.
    /// </summary>
    ///
    /// <remarks>
    /// This method sends a command to all command listeners to load any user
    /// settings they maintain.  The Ribbon is one such listener, so calling
    /// this method from the Ribbon sends the command to itself.
    /// </remarks>
    //*************************************************************************

    private void
    SendLoadUserSettingsCommand()
    {
        AssertValid();

        SendNoParamCommand(NoParamCommand.LoadUserSettings);
    }

    //*************************************************************************
    //  Method: LoadUserSettings()
    //
    /// <summary>
    /// Loads the user settings that are controlled directly by the Ribbon.
    /// </summary>
    ///
    /// <remarks>
    /// This gets called only when a NoParamCommand.LoadUserSettings command is
    /// received.  It should not be called directly.  Call <see
    /// cref="SendLoadUserSettingsCommand" /> instead.
    /// </remarks>
    //*************************************************************************

    private void
    LoadUserSettings()
    {
        AssertValid();

        GeneralUserSettings oGeneralUserSettings = new GeneralUserSettings();

        this.ReadVertexLabels = oGeneralUserSettings.ReadVertexLabels;
        this.ReadEdgeLabels = oGeneralUserSettings.ReadEdgeLabels;
        this.ReadGroupLabels = oGeneralUserSettings.ReadGroupLabels;
        this.ShowGraphLegend = oGeneralUserSettings.ShowGraphLegend;
        this.ShowGraphAxes = oGeneralUserSettings.ShowGraphAxes;
    }

    //*************************************************************************
    //  Method: SendSaveUserSettingsCommand()
    //
    /// <summary>
    /// Sends a command to save user settings.
    /// </summary>
    ///
    /// <remarks>
    /// This method sends a command to all command listeners to save any user
    /// settings they maintain.  The Ribbon is one such listener, so calling
    /// this method from the Ribbon sends the command to itself.
    /// </remarks>
    //*************************************************************************

    private void
    SendSaveUserSettingsCommand()
    {
        AssertValid();

        SendNoParamCommand(NoParamCommand.SaveUserSettings);
    }

    //*************************************************************************
    //  Method: SaveUserSettings()
    //
    /// <summary>
    /// Saves the user settings that are controlled directly by the Ribbon and
    /// the TaskPane.
    /// </summary>
    //*************************************************************************

    private void
    SaveUserSettings()
    {
        AssertValid();

        GeneralUserSettings oGeneralUserSettings = new GeneralUserSettings();

        oGeneralUserSettings.ReadVertexLabels = this.ReadVertexLabels;
        oGeneralUserSettings.ReadEdgeLabels = this.ReadEdgeLabels;
        oGeneralUserSettings.ReadGroupLabels = this.ReadGroupLabels;
        oGeneralUserSettings.ShowGraphLegend = this.ShowGraphLegend;
        oGeneralUserSettings.ShowGraphAxes = this.ShowGraphAxes;

        oGeneralUserSettings.Save();
    }

    //*************************************************************************
    //  Method: GetPerWorkbookSettings()
    //
    /// <summary>
    /// Gets a new PerWorkbookSettings object.
    /// </summary>
    ///
    /// <returns>
    /// A new PerWorkbookSettings object.
    /// </returns>
    //*************************************************************************

    protected PerWorkbookSettings
    GetPerWorkbookSettings()
    {
        AssertValid();

        return ( new PerWorkbookSettings(this.ThisWorkbook.InnerObject) );
    }

    //*************************************************************************
    //  Method: SendNoParamCommand()
    //
    /// <summary>
    /// Sends a no-parameter command via the static <see
    /// cref="CommandDispatcher" /> class.
    /// </summary>
    ///
    /// <param name="eNoParamCommand">
    /// The command that needs to be run, where the command does not require
    /// any parameters.
    /// </param>
    //*************************************************************************

    protected void
    SendNoParamCommand
    (
        NoParamCommand eNoParamCommand
    )
    {
        AssertValid();

        CommandDispatcher.SendNoParamCommand(this, eNoParamCommand);
    }

    //*************************************************************************
    //  Method: SendCommand()
    //
    /// <summary>
    /// Sends a command via the static <see cref="CommandDispatcher" /> class.
    /// </summary>
    ///
    /// <param name="oRunCommandEventArgs">
    /// The event arguments to include with the command.
    /// </param>
    //*************************************************************************

    protected void
    SendCommand
    (
        RunCommandEventArgs oRunCommandEventArgs
    )
    {
        AssertValid();

        CommandDispatcher.SendCommand(this, oRunCommandEventArgs);
    }

    //*************************************************************************
    //  Method: Ribbon_Load()
    //
    /// <summary>
    /// Handles the Load event on the ribbon.
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
    Ribbon_Load
    (
        object sender,
        RibbonUIEventArgs e
    )
    {
        AssertValid();

        CommandDispatcher.CommandSent +=
            new RunCommandEventHandler(this.CommandDispatcher_CommandSent);

        SendLoadUserSettingsCommand();

        // The graph directedness RibbonDropDown should be enabled only if the
        // template the workbook is based on supports changing the
        // directedness.
        //
        // This is done here rather than in the constructor because a
        // PerWorkbookSettings object can't be created until this.ThisWorkbook
        // is available, which doesn't happen until the Ribbon is fully loaded.
        //
        // The order of workbook/task pane/ribbon events is:
        //
        // 1. ThisWorkbook_Startup.
        //
        // 2. TaskPane gets created from the ThisWorkbook_Startup handler.
        //
        // 3. ThisWorkbook_ActivateEvent.
        //
        // 4. Ribbon_Load.

        PerWorkbookSettings oPerWorkbookSettings = GetPerWorkbookSettings();

        Int32 iTemplateVersion = oPerWorkbookSettings.TemplateVersion;

        rddGraphDirectedness.Enabled = (iTemplateVersion >= 51);

        // The menu for groups should be enabled only if the template supports
        // groups.

        mnuGroups.Enabled =
            (iTemplateVersion >= GroupManager.MinimumTemplateVersionForGroups);

        // Should the layout automatically be set and the workbook read?  (This
        // feature was added in January 2010 for the Microsoft Biology
        // Foundation project, which "drives" the NodeXL template
        // programatically and needs to be able to set the layout and read the
        // workbook by writing to workbook cells.  It may be useful for other
        // purposes as well.)

        Nullable<LayoutType> oAutoLayoutOnOpen =
            oPerWorkbookSettings.AutoLayoutOnOpen;

        if (oAutoLayoutOnOpen.HasValue)
        {
            // Yes.

            this.Layout = oAutoLayoutOnOpen.Value;

            // Unfortunately, there is no way to programatically click a
            // RibbonButton.  Simulate a click.

            SendNoParamCommand(NoParamCommand.ShowGraphAndReadWorkbook);
        }

        // Should task automation be run on the workbook when it is opened?

        if (oPerWorkbookSettings.AutomateTasksOnOpen)
        {
            // Yes.

            AutomateTasksOnOpen();
        }
    }

    //*************************************************************************
    //  Method: Ribbon_Close()
    //
    /// <summary>
    /// Handles the Close event on the ribbon.
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
    Ribbon_Close
    (
        object sender,
        EventArgs e
    )
    {
        AssertValid();

        // (Do nothing.)
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

        if (e is RunNoParamCommandEventArgs)
        {
            switch ( ( (RunNoParamCommandEventArgs)e ).NoParamCommand )
            {
                case NoParamCommand.LoadUserSettings:

                    LoadUserSettings();
                    break;

                case NoParamCommand.SaveUserSettings:

                    SaveUserSettings();
                    break;

                default:

                    break;
            }
        }
    }

    //*************************************************************************
    //  Method: btnRunNoParamCommand_Click()
    //
    /// <summary>
    /// Handles the Click event on buttons that represent commands that require
    /// no parameters to run.
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
    /// This handles the click event for all "run no-parameter command"
    /// buttons.  The Tag of each button stores the <see
    /// cref="NoParamCommand" /> value that indicates which command needs to
    /// be run.
    /// </remarks>
    //*************************************************************************

    private void
    btnRunNoParamCommand_Click
    (
        object sender,
        RibbonControlEventArgs e
    )
    {
        AssertValid();

        // A NoParamCommand value is stored in the button's Tag.

        Debug.Assert(sender is RibbonComponent);
        Debug.Assert( ( (RibbonComponent)sender ).Tag is NoParamCommand );

        SendNoParamCommand( (NoParamCommand)( (RibbonComponent)sender ).Tag );
    }

    //*************************************************************************
    //  Method: btnStartProcess_Click()
    //
    /// <summary>
    /// Handles the Click event on buttons that start a Windows process.
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
    /// The Tag of each such button stores the URL or file name of the process
    /// to start.
    /// </remarks>
    //*************************************************************************

    private void
    btnStartProcess_Click
    (
        object sender,
        RibbonControlEventArgs e
    )
    {
        AssertValid();

        Debug.Assert(sender is RibbonComponent);
        Debug.Assert( ( (RibbonComponent)sender ).Tag is String );

        Process.Start( (String)( (RibbonComponent)sender ).Tag );
    }

    //*************************************************************************
    //  Method: btnCreateNodeXLWorkbook_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnCreateNodeXLWorkbook button.
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
    btnCreateNodeXLWorkbook_Click
    (
        object sender,
        RibbonControlEventArgs e
    )
    {
        AssertValid();

        SendNoParamCommand(NoParamCommand.CreateNodeXLWorkbook);
    }

    //*************************************************************************
    //  Method: mnuImport_ItemsLoading()
    //
    /// <summary>
    /// Handles the ItemsLoading event on the mnuImport RibbonMenu.
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
    mnuImport_ItemsLoading
    (
        object sender,
        RibbonControlEventArgs e
    )
    {
        AssertValid();

        if (mnuImport.Tag is Boolean)
        {
            // This method has already been called.

            return;
        }

        // Get an array of plug-in classes that implement either the newer
        // IGraphDataProvider2 interface or the older IGraphDataProvider
        // interface.

        Object [] oGraphDataProviders;
        this.ThisWorkbook.ShowWaitCursor = true;

        try
        {
            oGraphDataProviders = PlugInManager.GetGraphDataProviders();
        }
        finally
        {
            this.ThisWorkbook.ShowWaitCursor = false;
        }

        Int32 iGraphDataProviders = oGraphDataProviders.Length;

        if (iGraphDataProviders == 0)
        {
            return;
        }

        // For each such class, add a child menu item to the Import menu.

        IList<RibbonControl> oImportItems = mnuImport.Items;

        Int32 iInsertionIndex =
            oImportItems.IndexOf(sepGraphDataProvidersGoHere);

        Debug.Assert(iInsertionIndex > 0);

        oImportItems.Insert( iInsertionIndex,
            this.Factory.CreateRibbonSeparator() );

        iInsertionIndex++;

        foreach (Object oGraphDataProvider in oGraphDataProviders)
        {
            RibbonButton oRibbonButton = this.Factory.CreateRibbonButton();

            String sGraphDataProviderName =
                PlugInManager.GetGraphDataProviderName(oGraphDataProvider);

            oRibbonButton.Label = "From " + sGraphDataProviderName + "...";
            oRibbonButton.ScreenTip = "Import from " + sGraphDataProviderName;
            oRibbonButton.Tag = oGraphDataProvider;

            oRibbonButton.SuperTip =
                "Optionally clear the NodeXL workbook, then "

                + PlugInManager.GetGraphDataProviderDescription(
                    oGraphDataProvider) + "."
                ;

            oRibbonButton.Click += new RibbonControlEventHandler(
                this.btnImportFromGraphDataProvider_Click);

            oImportItems.Insert(iInsertionIndex, oRibbonButton);
            iInsertionIndex++;
        }

        // Prevent menu items from being added again by using the Tag to store
        // a flag.

        mnuImport.Tag = true;
    }

    //*************************************************************************
    //  Method: btnImportFromGraphDataProvider_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnImportFromGraphDataProvider buttons.
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
    btnImportFromGraphDataProvider_Click
    (
        object sender,
        RibbonControlEventArgs e
    )
    {
        AssertValid();

        // The plug-in class that implements IGraphDataProvider2 or
        // IGraphDataProvider was stored in the Tag of the RibbonButton by
        // mnuImport_ItemsLoading().

        Debug.Assert(sender is RibbonControl);

        Object oGraphDataProvider = ( (RibbonControl)sender ).Tag;

        Debug.Assert(oGraphDataProvider is IGraphDataProvider2 ||
            oGraphDataProvider is IGraphDataProvider );

        this.ThisWorkbook.ImportFromGraphDataProvider(oGraphDataProvider);
    }

    //*************************************************************************
    //  Method: rddGraphDirectedness_SelectionChanged()
    //
    /// <summary>
    /// Handles the SelectionChanged event on the rddGraphDirectedness
    /// RibbonDropDown.
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
    rddGraphDirectedness_SelectionChanged
    (
        object sender,
        RibbonControlEventArgs e
    )
    {
        AssertValid();

        GraphDirectedness eGraphDirectedness = this.GraphDirectedness;

        if ( !this.ThisWorkbook.ExcelApplicationIsReady(true) )
        {
            // Cancel the change.

            this.GraphDirectedness =
                (eGraphDirectedness == GraphDirectedness.Directed) ?
                GraphDirectedness.Undirected : GraphDirectedness.Directed;
        }
        else
        {
            // Notify the workbook.

            this.ThisWorkbook.GraphDirectedness = eGraphDirectedness;
        }
    }

    //*************************************************************************
    //  Method: m_oLayoutManagerForRibbonDropDown_LayoutChanged()
    //
    /// <summary>
    /// Handles the LayoutChanged event on the
    /// m_oLayoutManagerForRibbonDropDown.
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
    m_oLayoutManagerForRibbonDropDown_LayoutChanged
    (
        object sender,
        EventArgs e
    )
    {
        AssertValid();

        // Tell the TaskPane to update the layout.

        SendNoParamCommand(NoParamCommand.UpdateLayout);
    }

    //*************************************************************************
    //  Method: rddLayout_ButtonClick()
    //
    /// <summary>
    /// Handles the ButtonClick event on the rddLayout RibbonDropDown.
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
    rddLayout_ButtonClick
    (
        object sender,
        RibbonControlEventArgs e
    )
    {
        if (sender == this.btnEditLayoutUserSettings)
        {
            SendNoParamCommand(NoParamCommand.EditLayoutUserSettings);
        }
        else if (sender == this.btnShowReadabilityMetrics)
        {
            SendNoParamCommand(NoParamCommand.ShowReadabilityMetrics);
        }
    }

    //*************************************************************************
    //  Method: btnMergeDuplicateEdges_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnMergeDuplicateEdges button.
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
    btnMergeDuplicateEdges_Click
    (
        object sender,
        RibbonControlEventArgs e
    )
    {
        AssertValid();

        SendCommand( new RunMergeDuplicateEdgesCommandEventArgs(true) );
    }

    //*************************************************************************
    //  Method: btnPopulateVertexWorksheet_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnPopulateVertexWorksheet button.
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
    btnPopulateVertexWorksheet_Click
    (
        object sender,
        RibbonControlEventArgs e
    )
    {
        AssertValid();

        this.ThisWorkbook.PopulateVertexWorksheet(true, true);
    }

    //*************************************************************************
    //  Method: btnSetVisualAttribute_Click()
    //
    /// <summary>
    /// Handles the Click event on several of the "set visual attribute"
    /// buttons.
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
    /// This handles the click event for all "set visual attribute" buttons
    /// for which the visual attribute value isn't known yet and must be
    /// obtained from the user.
    /// </remarks>
    //*************************************************************************

    private void
    btnSetVisualAttribute_Click
    (
        object sender,
        RibbonControlEventArgs e
    )
    {
        AssertValid();

        // A VisualAttributes flag is stored in the Tag of each visual
        // attribute button.

        Debug.Assert(sender is RibbonComponent);
        Debug.Assert( ( (RibbonComponent)sender ).Tag is VisualAttributes );

        SendCommand( new RunSetVisualAttributeCommandEventArgs(
            (VisualAttributes)( (RibbonComponent)sender ).Tag, null) );
    }

    //*************************************************************************
    //  Method: btnSetEdgeVisibility_Click()
    //
    /// <summary>
    /// Handles the Click event on all of the "set edge visibility" buttons.
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
    btnSetEdgeVisibility_Click
    (
        object sender,
        RibbonControlEventArgs e
    )
    {
        AssertValid();

        // An EdgeVisibility value is stored in the Tag of each "set edge
        // visibility" button.

        Debug.Assert(sender is RibbonComponent);

        Debug.Assert( ( (RibbonComponent)sender ).Tag is
            EdgeWorksheetReader.Visibility );

        SendCommand( new RunSetVisualAttributeCommandEventArgs(
            VisualAttributes.EdgeVisibility,
            (EdgeWorksheetReader.Visibility)( (RibbonComponent)sender ).Tag)
            );
    }

    //*************************************************************************
    //  Method: btnSetVertexVisibility_Click()
    //
    /// <summary>
    /// Handles the Click event on all of the "set vertex visibility" buttons.
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
    btnSetVertexVisibility_Click
    (
        object sender,
        RibbonControlEventArgs e
    )
    {
        AssertValid();

        // An VertexVisibility value is stored in the Tag of each "set vertex
        // visibility" button.

        Debug.Assert(sender is RibbonComponent);

        Debug.Assert( ( (RibbonComponent)sender ).Tag is
            VertexWorksheetReader.Visibility );

        SendCommand( new RunSetVisualAttributeCommandEventArgs(
            VisualAttributes.VertexVisibility,
            (VertexWorksheetReader.Visibility)( (RibbonComponent)sender ).Tag)
            );
    }

    //*************************************************************************
    //  Method: btnSetVertexShape_Click()
    //
    /// <summary>
    /// Handles the Click event on all of the "set vertex shape" buttons.
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
    btnSetVertexShape_Click
    (
        object sender,
        RibbonControlEventArgs e
    )
    {
        AssertValid();

        // A VertexShape value is stored in the Tag of each "set vertex shape"
        // button.

        Debug.Assert(sender is RibbonComponent);
        Debug.Assert( ( (RibbonComponent)sender ).Tag is VertexShape );

        SendCommand( new RunSetVisualAttributeCommandEventArgs(
            VisualAttributes.VertexShape,
            (VertexShape)( (RibbonComponent)sender ).Tag)
            );
    }

    //*************************************************************************
    //  Method: btnOpenSampleNodeXLWorkbook_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnOpenSampleNodeXLWorkbook button.
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
    btnOpenSampleNodeXLWorkbook_Click
    (
        object sender,
        RibbonControlEventArgs e
    )
    {
        AssertValid();

        ApplicationUtil.OpenSampleNodeXLWorkbook();
    }

    //*************************************************************************
    //  Method: mnuGroups_ItemsLoading()
    //
    /// <summary>
    /// Handles the ItemsLoading event on the mnuGroups RibbonMenu.
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
    mnuGroups_ItemsLoading
    (
        object sender,
        RibbonControlEventArgs e
    )
    {
        AssertValid();

        GroupCommands eGroupCommandsToEnable =
            GroupManager.GetGroupCommandsToEnable(
                this.ThisWorkbook.InnerObject);

        btnCollapseSelectedGroups.Enabled =
            GroupManager.GroupCommandsIncludeGroupCommand(
                eGroupCommandsToEnable, GroupCommands.CollapseSelectedGroups);

        btnExpandSelectedGroups.Enabled =
            GroupManager.GroupCommandsIncludeGroupCommand(
                eGroupCommandsToEnable, GroupCommands.ExpandSelectedGroups);

        btnCollapseAllGroups.Enabled =
            GroupManager.GroupCommandsIncludeGroupCommand(
                eGroupCommandsToEnable, GroupCommands.CollapseAllGroups);

        btnExpandAllGroups.Enabled =
            GroupManager.GroupCommandsIncludeGroupCommand(
                eGroupCommandsToEnable, GroupCommands.ExpandAllGroups);

        btnSelectGroupsWithSelectedVertices.Enabled =
            GroupManager.GroupCommandsIncludeGroupCommand(
                eGroupCommandsToEnable,
                GroupCommands.SelectGroupsWithSelectedVertices);

        btnSelectAllGroups.Enabled =
            GroupManager.GroupCommandsIncludeGroupCommand(
                eGroupCommandsToEnable, GroupCommands.SelectAllGroups);

        btnAddSelectedVerticesToGroup.Enabled =
            GroupManager.GroupCommandsIncludeGroupCommand(
                eGroupCommandsToEnable,
                GroupCommands.AddSelectedVerticesToGroup);

        btnRemoveSelectedVerticesFromGroups.Enabled =
            GroupManager.GroupCommandsIncludeGroupCommand(
                eGroupCommandsToEnable,
                GroupCommands.RemoveSelectedVerticesFromGroups);

        btnRemoveSelectedGroups.Enabled =
            GroupManager.GroupCommandsIncludeGroupCommand(
                eGroupCommandsToEnable, GroupCommands.RemoveSelectedGroups);

        btnRemoveAllGroups.Enabled =
            GroupManager.GroupCommandsIncludeGroupCommand(
                eGroupCommandsToEnable, GroupCommands.RemoveAllGroups);

        // This is required to force the ItemsLoading event to fire the next
        // time the mnuGroups menu is opened.

        this.RibbonUI.InvalidateControl(this.mnuGroups.Name);
    }

    //*************************************************************************
    //  Method: btnGroupByCluster_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnGroupByCluster button.
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
    btnGroupByCluster_Click
    (
        object sender,
        RibbonControlEventArgs e
    )
    {
        AssertValid();

        SendCommand( new RunGroupByClusterCommandEventArgs(true) );
    }

    //*************************************************************************
    //  Method: GroupCommandButton_Click()
    //
    /// <summary>
    /// Handles the Click event on the buttons corresponding to the
    /// GroupCommands enumeration values.
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
    GroupCommandButton_Click
    (
        object sender,
        RibbonControlEventArgs e
    )
    {
        AssertValid();

        // The buttons corresponding to the GroupCommands enumeration values
        // have the enumeration value stored in their Tag.

        Debug.Assert(sender is RibbonButton);
        Debug.Assert( ( (RibbonButton)sender ).Tag is GroupCommands );

        SendCommand( new RunGroupCommandEventArgs(
            (GroupCommands)( (RibbonButton)sender ).Tag
            ) );
    }

    //*************************************************************************
    //  Method: mnuShowColumnGroups_ItemsLoading()
    //
    /// <summary>
    /// Handles the ItemsLoading event on the mnuShowColumnGroups RibbonMenu.
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
    mnuShowColumnGroups_ItemsLoading
    (
        object sender,
        RibbonControlEventArgs e
    )
    {
        AssertValid();

        ColumnGroupUserSettings oColumnGroupUserSettings =
            new ColumnGroupUserSettings();

        ColumnGroups eColumnGroupsToShow =
            oColumnGroupUserSettings.ColumnGroupsToShow;

        // Note that the chkShowVisualAttributeColumnGroups RibbonCheckBox
        // controls the visibility of three column groups:
        // ColumnGroup.EdgeVisualAttributes,
        // ColumnGroup.VertexVisualAttributes, and
        // ColumnGroup.GroupVisualAttributes.

        chkShowVisualAttributeColumnGroups.Checked =
            oColumnGroupUserSettings.ShouldShowColumnGroup(
                ColumnGroups.EdgeVisualAttributes, eColumnGroupsToShow);

        // Note that the chkShowLabelColumnGroups RibbonCheckBox controls the
        // visibility of three column groups: ColumnGroup.EdgeLabels,
        // ColumnGroup.VertexLabels, and ColumnGroup.GroupLabels.

        chkShowLabelColumnGroups.Checked =
            oColumnGroupUserSettings.ShouldShowColumnGroup(
                ColumnGroups.EdgeLabels, eColumnGroupsToShow);

        // Note that the chkShowLayoutColumnGroups RibbonCheckBox controls the
        // visibility of two column groups: ColumnGroup.VertexLayout and
        // ColumnGroup.GroupLayout.

        chkShowLayoutColumnGroups.Checked =
            oColumnGroupUserSettings.ShouldShowColumnGroup(
                ColumnGroups.VertexLayout, eColumnGroupsToShow);

        // Note that the chkShowGraphMetricColumnGroups RibbonCheckBox
        // controls the visibility of four column groups:
        // ColumnGroup.EdgeGraphMetrics, ColumnGroup.VertexGraphMetrics,
        // ColumnGroup.GroupGraphMetrics, and
        // ColumnGroup.GroupEdgeGraphMetrics.

        chkShowGraphMetricColumnGroups.Checked =
            oColumnGroupUserSettings.ShouldShowColumnGroup(
                ColumnGroups.VertexGraphMetrics, eColumnGroupsToShow);

        // Note that the chkShowOtherColumnGroups RibbonCheckBox controls the
        // visibility of three column groups: ColumnGroup.EdgeOtherColumns,
        // ColumnGroup.VertexOtherColumns, and ColumnGroup.GroupOtherColumns.

        chkShowOtherColumnGroups.Checked =
            oColumnGroupUserSettings.ShouldShowColumnGroup(
                ColumnGroups.EdgeOtherColumns, eColumnGroupsToShow);

        // This is required to force the ItemsLoading event to fire the next
        // time the mnuShowColumnGroups menu is opened.

        this.RibbonUI.InvalidateControl(this.mnuShowColumnGroups.Name);
    }

    //*************************************************************************
    //  Method: chkShowColumnGroups_Click()
    //
    /// <summary>
    /// Handles the Click event on the RibbonCheckBoxes that show or hide
    /// column groups.
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
    chkShowColumnGroups_Click
    (
        object sender,
        RibbonControlEventArgs e
    )
    {
        AssertValid();

        // This method handles the Click event for several RibbonCheckBoxes.
        // The RibbonCheckBoxes are distinguished by their Tags, which specify
        // an ORed combination of ColumnGroups flags.

        Debug.Assert(sender is RibbonCheckBox);
        RibbonCheckBox oCheckBox = (RibbonCheckBox)sender;
        Debug.Assert(oCheckBox.Tag is ColumnGroups);

        this.ThisWorkbook.ShowOrHideColumnGroups( (ColumnGroups)oCheckBox.Tag,
            oCheckBox.Checked );
    }

    //*************************************************************************
    //  Method: btnShowOrHideAllColumnGroups_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnShowAllColumnGroups and
    /// btnHideAllColumnGroups buttons.
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
    btnShowOrHideAllColumnGroups_Click
    (
        object sender,
        RibbonControlEventArgs e
    )
    {
        AssertValid();

        // This method handles the Click event for two buttons.  The buttons
        // are distinguished by their Boolean Tags, which specify true to show
        // or false to hide.

        Debug.Assert(sender is RibbonButton);
        RibbonButton oButton = (RibbonButton)sender;
        Debug.Assert(oButton.Tag is Boolean);

        this.ThisWorkbook.ShowOrHideColumnGroups(ColumnGroups.All,
            (Boolean)oButton.Tag);
    }

    //*************************************************************************
    //  Method: btnEnableAllNotifications_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnEnableAllNotifications button.
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
    btnEnableAllNotifications_Click
    (
        object sender,
        RibbonControlEventArgs e
    )
    {
        AssertValid();

        NotificationUserSettings oNotificationUserSettings =
            new NotificationUserSettings();

        oNotificationUserSettings.EnableAllNotifications();
        oNotificationUserSettings.Save();
    }

    //*************************************************************************
    //  Method: chkImportOptions_Click()
    //
    /// <summary>
    /// Handles the Click event on the chkImportOptions button.
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
    btnImportOptions_Click
    (
        object sender,
        RibbonControlEventArgs e
    )
    {
        AssertValid();

        String sWorkbookSettings;

        if ( ( new OpenWorkbookSettingsFileDialog() )
            .ShowDialogAndOpenWorkbookSettingsFile(out sWorkbookSettings) ==
            DialogResult.OK )
        {
            GetPerWorkbookSettings().WorkbookSettings = sWorkbookSettings;
            OnWorkbookSettingsReplaced();
        }
    }

    //*************************************************************************
    //  Method: chkExportOptions_Click()
    //
    /// <summary>
    /// Handles the Click event on the chkExportOptions button.
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
    btnExportOptions_Click
    (
        object sender,
        RibbonControlEventArgs e
    )
    {
        AssertValid();

        SendSaveUserSettingsCommand();

        ( new SaveWorkbookSettingsFileDialog(String.Empty, String.Empty) ).
            ShowDialogAndSaveWorkbookSettings(
                GetPerWorkbookSettings().WorkbookSettings);
    }

    //*************************************************************************
    //  Method: btnUseCurrentOptionsForNew_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnUseCurrentOptionsForNew button.
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
    btnUseCurrentOptionsForNew_Click
    (
        object sender,
        RibbonControlEventArgs e
    )
    {
        AssertValid();

        const String Message =
            "Do you want this workbook's options to be used for all new NodeXL"
            + " workbooks?"
            ;

        if (MessageBox.Show(Message, FormUtil.ApplicationName,
            MessageBoxButtons.YesNo, MessageBoxIcon.Information)
            == DialogResult.Yes)
        {
            // All user setings must be saved to the workbook settings.

            SendSaveUserSettingsCommand();

            ( new NodeXLApplicationSettingsBase() ).
                UseWorkbookSettingsForNewWorkbooks();
        }
    }

    //*************************************************************************
    //  Method: btnResetCurrentOptions_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnResetCurrentOptions button.
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
    btnResetCurrentOptions_Click
    (
        object sender,
        RibbonControlEventArgs e
    )
    {
        AssertValid();

        const String Message =
            "Do you want to reset this workbook's options to the options"
            + " used for new NodeXL workbooks?"
            ;

        if (MessageBox.Show(Message, FormUtil.ApplicationName,
            MessageBoxButtons.YesNo, MessageBoxIcon.Information)
            == DialogResult.Yes)
        {
            ( new NodeXLApplicationSettingsBase() ).
                UseNewWorkbookSettingsForThisWorkbook();

            OnWorkbookSettingsReplaced();
        }
    }

    //*************************************************************************
    //  Method: chkShowGraphLegend_Click()
    //
    /// <summary>
    /// Handles the Click event on the chkShowGraphLegend CheckBox.
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
    chkShowGraphLegend_Click
    (
        object sender,
        RibbonControlEventArgs e
    )
    {
        AssertValid();

        this.ShowGraphLegend = chkShowGraphLegend.Checked;
    }

    //*************************************************************************
    //  Method: chkShowGraphAxes_Click()
    //
    /// <summary>
    /// Handles the Click event on the chkShowGraphAxes CheckBox.
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
    chkShowGraphAxes_Click
    (
        object sender,
        RibbonControlEventArgs e
    )
    {
        AssertValid();

        this.ShowGraphAxes = chkShowGraphAxes.Checked;
    }

    //*************************************************************************
    //  Method: btnShowOrHideAllGraphElements_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnShowAllGraphElements and
    /// btnHideAllGraphElements buttons.
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
    btnShowOrHideAllGraphElements_Click
    (
        object sender,
        RibbonControlEventArgs e
    )
    {
        AssertValid();

        // This method handles the Click event for two buttons.  The buttons
        // are distinguished by their Boolean Tags, which specify true to show
        // or false to hide.

        Debug.Assert(sender is RibbonButton);
        RibbonButton oButton = (RibbonButton)sender;
        Debug.Assert(oButton.Tag is Boolean);

        Boolean bShow = (Boolean)oButton.Tag;

        chkReadVertexLabels.Checked = chkReadEdgeLabels.Checked
            = chkReadGroupLabels.Checked = this.ShowGraphLegend
            = this.ShowGraphAxes = bShow;
    }

    //*************************************************************************
    //  Method: btnHelp_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnHelp button.
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
    btnHelp_Click
    (
        object sender,
        RibbonControlEventArgs e
    )
    {
        AssertValid();

        ApplicationUtil.OpenHelp();
    }

    //*************************************************************************
    //  Method: btnAbout_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnAbout button.
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
    btnAbout_Click
    (
        object sender,
        RibbonControlEventArgs e
    )
    {
        AssertValid();

        ( new AboutDialog() ).ShowDialog();
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
        Debug.Assert(m_oLayoutManagerForRibbonDropDown != null);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Helper objects for managing layouts.

    protected LayoutManagerForRibbonDropDown m_oLayoutManagerForRibbonDropDown;
}

}
