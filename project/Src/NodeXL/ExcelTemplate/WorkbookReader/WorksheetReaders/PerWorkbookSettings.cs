
using System;
using System.Collections.Generic;
using Microsoft.Office.Interop.Excel;
using System.Globalization;
using System.Text;
using System.Linq;
using Smrf.AppLib;
using System.Diagnostics;
using Smrf.NodeXL.Core;
using Smrf.NodeXL.ApplicationUtil;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: PerWorkbookSettings
//
/// <summary>
/// Provides access to settings that are stored on a per-workbook basis.
/// </summary>
///
/// <remarks>
/// The settings are stored in a table in a hidden worksheet.
/// </remarks>
//*****************************************************************************

public class PerWorkbookSettings : WorksheetReaderBase
{
    //*************************************************************************
    //  Constructor: PerWorkbookSettings()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="PerWorkbookSettings" />
    /// class.
    /// </summary>
    ///
    /// <param name="workbook">
    /// The workbook containing the settings.
    /// </param>
    //*************************************************************************

    public PerWorkbookSettings
    (
        Microsoft.Office.Interop.Excel.Workbook workbook
    )
    {
        m_oWorkbook = workbook;
        m_oSettings = null;

        AssertValid();
    }

    //*************************************************************************
    //  Property: TemplateVersion
    //
    /// <summary>
    /// Gets the version number of the template the workbook is based on.
    /// </summary>
    ///
    /// <value>
    /// A template version number.
    /// </value>
    //*************************************************************************

    public Int32
    TemplateVersion
    {
        get
        {
            AssertValid();

            // The template version is stored in the workbook as a Double with
            // zero decimal places.  Sample: 51.

            Object oTemplateVersion;

            if ( TryGetValue(TemplateVersionSettingName, typeof(Double),
                out oTemplateVersion) )
            {
                return ( (Int32)(Double)oTemplateVersion );
            }

            return (DefaultTemplateVersion);
        }
    }

    //*************************************************************************
    //  Property: HasWorkbookSettings
    //
    /// <summary>
    /// Gets a flag indicating whether the workbook has user settings.
    /// </summary>
    ///
    /// <value>
    /// true if the workbook has user settings.
    /// </value>
    ///
    /// <remarks>
    /// Workbook settings are added to the workbook when a new workbook is
    /// created.  If this is a new workbook, or if it's an old saved workbook
    /// created in an earlier version of the program, false is returned.
    /// </remarks>
    ///
    /// <seealso cref="WorkbookSettings" />
    //*************************************************************************

    public Boolean
    HasWorkbookSettings
    {
        get
        {
            AssertValid();

            Object oWorkbookSettingsCellCount;

            return (TryGetValue(WorkbookSettingsCellCountSettingName,
                typeof(Double), out oWorkbookSettingsCellCount) );
        }
    }

    //*************************************************************************
    //  Property: WorkbookSettings
    //
    /// <summary>
    /// Gets or sets the user settings for this workbook.
    /// </summary>
    ///
    /// <value>
    /// The workbook's user settings, as an XML string.  If the workbook
    /// doesn't have user settings yet, a default set of settings is returned.
    /// </value>
    ///
    /// <remarks>
    /// See <see cref="NodeXLApplicationSettingsBase" /> for details on how
    /// the workbook's user settings are used.
    /// </remarks>
    ///
    /// <seealso cref="HasWorkbookSettings" />
    //*************************************************************************

    public String
    WorkbookSettings
    {
        get
        {
            AssertValid();

            // The workbook settings are a long XML string.  If you write a
            // string longer than about 8,200 character to an Excel cell via
            // Range.set_Value(), a COMException occurs ("Exception from
            // HRESULT: 0x800A03EC").  I don't know why this occurs, but it's
            // easily reproducible.
            //
            // To work around this problem, the XML string is split into
            // multiple cells in the property setter, and reassembled in the
            // getter.  An extra "workbook settings cell count" cell is used
            // to store the number of cells that contain the XML.

            Object oWorkbookSettingsCellCount;

            if (TryGetValue(WorkbookSettingsCellCountSettingName,
                typeof(Double), out oWorkbookSettingsCellCount))
            {
                StringBuilder oWorkbookSettings = new StringBuilder();

                Int32 iWorkbookSettingsCellCount =
                    (Int32)(Double)oWorkbookSettingsCellCount;

                for (Int32 i = 1; i <= iWorkbookSettingsCellCount; i++)
                {
                    Object oCellValue;

                    if ( TryGetValue(GetWorkbookSettingsCellSettingName(i),
                        typeof(String), out oCellValue) )
                    {
                        oWorkbookSettings.Append( (String)oCellValue );
                    }
                    else
                    {
                        // The workbook settings have become corrupted.

                        goto ReturnDefault;
                    }
                }

                return ( oWorkbookSettings.ToString() );
            }

            ReturnDefault:

            return (NodeXLApplicationSettingsBase.
                DefaultStandardSettingsFileContents);
        }

        set
        {
            Debug.Assert( !String.IsNullOrEmpty(value) );

			value = PreserveWorkbookSettingsBackwardCompatibility(value);

            Int32 iWorkbookSettingsCellCount = 1;

            while (value.Length > 0)
            {
                Int32 iLengthForThisCell =
                    Math.Min(value.Length, MaximumCharactersPerCell);

                String sCellValue = value.Substring(0, iLengthForThisCell);

                SetValue(
                    GetWorkbookSettingsCellSettingName(
                        iWorkbookSettingsCellCount),
                    sCellValue
                    );

                value = value.Substring(iLengthForThisCell);

                iWorkbookSettingsCellCount++;
            }

            SetValue(WorkbookSettingsCellCountSettingName,
                iWorkbookSettingsCellCount - 1);

            AssertValid();
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

            // The directedness is stored in the workbook as a String.  Sample:
            // "Undirected".

            Object oGraphDirectedness;

            if ( TryGetValue(GraphDirectednessSettingName, typeof(String),
                out oGraphDirectedness) )
            {
                try
                {
                    return ( (GraphDirectedness)Enum.Parse(
                        typeof(GraphDirectedness),
                        (String)oGraphDirectedness) );
                }
                catch (ArgumentException)
                {
                }
            }

            return (DefaultGraphDirectedness);
        }

        set
        {
            SetValue( GraphDirectednessSettingName, value.ToString() );

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: AutoLayoutOnOpen
    //
    /// <summary>
    /// Gets the layout to use when the workbook is opened to automatically lay
    /// out and show the graph.
    /// </summary>
    ///
    /// <value>
    /// The <see cref="LayoutType" /> to use when the workbook is opened, or
    /// null to not automatically lay out and show the graph.
    /// </value>
    ///
    /// <value>
    /// If this returns null, no special action should be taken when the
    /// workbook is opened.  If it is non-null, the layout should be set to the
    /// returned value and the workbook should be automatically read.
    /// </value>
    //*************************************************************************

    public Nullable<LayoutType>
    AutoLayoutOnOpen
    {
        get
        {
            AssertValid();

            // The LayoutType is stored in the workbook as a String.  Sample:
            // "Grid".

            Object oLayoutType;

            if ( TryGetValue(AutoLayoutOnOpenSettingName, typeof(String),
                out oLayoutType) )
            {
                try
                {
                    return ( new Nullable<LayoutType>(
                        (LayoutType)Enum.Parse(typeof(LayoutType),
                            (String)oLayoutType) ) );
                }
                catch (ArgumentException)
                {
                }
            }

            return ( new Nullable<LayoutType>() );
        }
    }

    //*************************************************************************
    //  Property: AutomateTasksOnOpen
    //
    /// <summary>
    /// Gets or sets a flag indicating whether task automation should be run on
    /// the workbook when it is opened.
    /// </summary>
    ///
    /// <value>
    /// true if task automation should be run on the workbook when it is
    /// opened.
    /// </value>
    ///
    /// <value>
    /// If this returns true, the tasks specified by the <see
    /// cref="AutomateTasksUserSettings" /> class should be run on the workbook
    /// when it is opened.
    /// </value>
    //*************************************************************************

    public Boolean
    AutomateTasksOnOpen
    {
        get
        {
            AssertValid();

            // The flag is stored in the workbook as a Boolean.  Sample:
            // "TRUE".

            Object oAutomateTasksOnOpen;

            if ( TryGetValue(AutomateTasksOnOpenSettingName, typeof(Boolean),
                out oAutomateTasksOnOpen) )
            {
                return ( (Boolean)oAutomateTasksOnOpen );
            }

            return (false);
        }

        set
        {
            SetValue(AutomateTasksOnOpenSettingName, value);

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: AutoFillWorkbookResults
    //
    /// <summary>
    /// Gets or sets the results of running the application's autofill feature.
    /// </summary>
    ///
    /// <value>
    /// The results of running the application's autofill feature, as an <see
    /// cref="ExcelTemplate.AutoFillWorkbookResults" /> object.  If the feature
    /// hasn't been run on the workbook, all TryXX() methods on the returned
    /// object will return false.
    /// </value>
    //*************************************************************************

    public AutoFillWorkbookResults
    AutoFillWorkbookResults
    {
        get
        {
            AssertValid();

            // The results are stored in the workbook as a String, using the
            // AutoFillWorkbookResults.ConvertToString() and
            // ConvertFromString() methods.

            Object oAutoFillWorkbookResults;

            if ( !TryGetValue(AutoFillWorkbookResultsSettingName,
                typeof(String), out oAutoFillWorkbookResults) )
            {
                // Return an object whose TryXX() methods will all return
                // false.

                return ( new AutoFillWorkbookResults() );
            }

            return ( AutoFillWorkbookResults.FromString(
                (String)oAutoFillWorkbookResults) );
        }

        set
        {
            String sAutoFillWorkbookResults = value.ConvertToString();

            if (sAutoFillWorkbookResults.Length > MaximumCharactersPerCell)
            {
                // Don't exceed Excel's maximum character count.
                //
                // A very long string can result if the user autofills a color
                // column by category, for example, and there are many
                // categories or the categories have long names.
                //
                // The proper way to handle this is to split the long string
                // among multiple Excel cells, as is done in the
                // WorkbookSettings property setter.
                //
                // The short-term fix implemented here, (which might suffice
                // for the long term), is to not save the results.  The
                // consequences are minor: there will be no autofill results in
                // the graph summary or graph legend.

                sAutoFillWorkbookResults = String.Empty;
            }

            SetValue(AutoFillWorkbookResultsSettingName,
                sAutoFillWorkbookResults);

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: GraphHistory
    //
    /// <summary>
    /// Gets or sets attributes that describe how a graph was created.
    /// </summary>
    ///
    /// <value>
    /// An <see cref="ExcelTemplate.GraphHistory" /> object that stores
    /// key/value string pairs describing how a graph was created.
    /// </value>
    //*************************************************************************

    public GraphHistory
    GraphHistory
    {
        get
        {
            AssertValid();

            // The results are stored in the workbook as a String, using the
            // GraphHistory.LoadFromString() and SaveToString() methods.

            Object oGraphHistory;
            String sGraphHistory;

            if ( TryGetValue(GraphHistorySettingName, typeof(String),
                out oGraphHistory) )
            {
                sGraphHistory = (String)oGraphHistory;
            }
            else
            {
                sGraphHistory = String.Empty;
            }

            return ( GraphHistory.FromString(sGraphHistory) );
        }

        set
        {
            SetValue( GraphHistorySettingName, value.ToString() );

            AssertValid();
        }
    }

    //*************************************************************************
    //  Method: SetGraphHistoryValue()
    //
    /// <summary>
    /// Sets a value on the <see cref="GraphHistory" /> object.
    /// </summary>
    ///
    /// <param name="graphHistoryKey">
    /// The key to set the value on.  Should be a member of the <see
    /// cref="GraphHistoryKeys" /> class.
    /// </param>
    ///
    /// <param name="graphHistoryValue">
    /// The value to set.  Can be null or empty.
    /// </param>
    //*************************************************************************

    public void
    SetGraphHistoryValue
    (
        String graphHistoryKey,
        String graphHistoryValue
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(graphHistoryKey) );
        AssertValid();

        GraphHistory oGraphHistory = this.GraphHistory;
        oGraphHistory[graphHistoryKey] = graphHistoryValue;
        this.GraphHistory = oGraphHistory;
    }

    //*************************************************************************
    //  Method: SetGraphHistoryGroupingDescription()
    //
    /// <summary>
    /// Sets the description of the grouping technique on the <see
    /// cref="GraphHistory" /> object.
    /// </summary>
    ///
    /// <param name="groupingDescription">
    /// Description of the technique that was used to group the graph's
    /// vertices.  Specify String.Empty to indicate that all groups have been
    /// removed.
    /// </param>
    //*************************************************************************

    public void
    SetGraphHistoryGroupingDescription
    (
        String groupingDescription
    )
    {
        Debug.Assert(groupingDescription != null);
        AssertValid();

        SetGraphHistoryValue(GraphHistoryKeys.GroupingDescription,
            groupingDescription);
    }

    //*************************************************************************
    //  Method: OnWorkbookTablesCleared()
    //
    /// <summary>
    /// Clears properties that are no longer valid after the workbook's tables
    /// have been cleared.
    /// </summary>
    //*************************************************************************

    public void
    OnWorkbookTablesCleared()
    {
        AssertValid();

        ClearAutoFillWorkbookResults();
        ClearGraphHistory();
    }

    //*************************************************************************
    //  Method: ClearAutoFillWorkbookResults()
    //
    /// <summary>
    /// Clears the results of running the application's autofill feature.
    /// </summary>
    //*************************************************************************

    protected void
    ClearAutoFillWorkbookResults()
    {
        AssertValid();

        SetValue( AutoFillWorkbookResultsSettingName,
            ( new AutoFillWorkbookResults() ).ConvertToString() );
    }

    //*************************************************************************
    //  Method: ClearGraphHistory()
    //
    /// <summary>
    /// Clears the attributes that describe how a graph was created.
    /// </summary>
    //*************************************************************************

    protected void
    ClearGraphHistory()
    {
        AssertValid();

        // Clear everything except the comments, if there are any.

        GraphHistory oNewGraphHistory = new GraphHistory();

        String sComments;
        
        if ( this.GraphHistory.TryGetValue(GraphHistoryKeys.Comments,
            out sComments) )
        {
            oNewGraphHistory[GraphHistoryKeys.Comments] = sComments;
        }

        this.GraphHistory = oNewGraphHistory;
    }

    //*************************************************************************
    //  Method: PreserveWorkbookSettingsBackwardCompatibility()
    //
    /// <summary>
    /// Preserves backward compatibility when a workbook created by a newer
    /// version of NodeXL is opened in an older version.
    /// </summary>
    ///
    /// <param name="sWorkbookSettings">
    /// The workbook's user settings.
    /// </param>
    ///
    /// <returns>
    /// A modified copy of <paramref name="sWorkbookSettings" />.
    /// </returns>
    //*************************************************************************

    protected String
    PreserveWorkbookSettingsBackwardCompatibility
    (
        String sWorkbookSettings
    )
    {
        AssertValid();
        Debug.Assert( !String.IsNullOrEmpty(sWorkbookSettings) );

        // This is to allow older versions of NodeXL to open workbooks created
        // with newer versions of NodeXL.
        //
        // Older versions of NodeXL used .NET 3.5.  When a configuration file
        // was created, the application settings framework created sectionGroup
        // elements that looked like this:
        //
        //   <sectionGroup
        //     name="userSettings"
        //     type="System.Configuration.UserSettingsGroup, System,
        //       Version=2.0.0.0, Culture=neutral,
        //       PublicKeyToken=b77a5c561934e089">
        //
        // Note the "Version=2.0.0.0", which was the version of the
        // UserSettingsGroup class in .NET 3.5.
        //
        // (The application settings framework also created section elements
        // that used Version=2.0.0.0, so these comments about versions pertain
        // to section elements as well.)
        //
        // Newer versions of NodeXL use .NET 4.0, which creates sectionGroup
        // elements that look like this:
        //
        //   <sectionGroup
        //     name="userSettings"
        //     type="System.Configuration.UserSettingsGroup, System,
        //       Version=4.0.0.0, Culture=neutral,
        //       PublicKeyToken=b77a5c561934e089">
        //
        // Note the "Version=4.0.0.0", which is the version of the
        // UserSettingsGroup class in .NET 4.0.
        //
        // .NET 4.0 can handle sectionGroup elements that specify
        // Version=2.0.0.0, so newer versions of NodeXL can handle
        // configurations created with older versions.  However, .NET 3.5
        // CANNOT handle sectionGroup elements that specify Version=4.0.0.0.
        // Therefore, this method replaces the "4.0.0.0" with "2.0.0.0" in case
        // this workbook is ever opened with an older version of NodeXL.
        //
        // (Note that .NET 4.0 can also handle a configuration that specifies
        // both Version=2.0.0.0 and Version=4.0.0.0, so no attempt is made to
        // modify Versions that get saved in the standard settings file on this
        // computer.)

        return ( sWorkbookSettings.Replace(
            "Version=4.0.0.0, Culture=",
            "Version=2.0.0.0, Culture="
            ) );
    }

    //*************************************************************************
    //  Method: GetWorkbookSettingsCellSettingName()
    //
    /// <summary>
    /// Gets the setting name to use for one piece of the workbook settings.
    /// </summary>
    ///
    /// <param name="iWorkbookSettingsCellNumberOneBased">
    /// The one-based workbook settings cell number.
    /// </param>
    ///
    /// <returns>
    /// The setting name to use.
    /// </returns>
    //*************************************************************************

    protected String
    GetWorkbookSettingsCellSettingName
    (
        Int32 iWorkbookSettingsCellNumberOneBased
    )
    {
        Debug.Assert(iWorkbookSettingsCellNumberOneBased >= 1);
        AssertValid();

        return ( WorkbookSettingsCellBaseSettingName +
            iWorkbookSettingsCellNumberOneBased.ToString(
                CultureInfo.InvariantCulture) );
    }

    //*************************************************************************
    //  Method: SetValue()
    //
    /// <summary>
    /// Sets the value of a setting.
    /// </summary>
    ///
    /// <param name="settingName">
    /// The setting's name.
    /// </param>
    ///
    /// <param name="value">
    /// The value to set.  Can be null.
    /// </param>
    //*************************************************************************

    protected void
    SetValue
    (
        String settingName,
        Object value
    )
    {
        AssertValid();

        Dictionary<String, Object> oSettings = GetAllSettings();

        oSettings[settingName] = value;

        WriteAllSettings();
    }

    //*************************************************************************
    //  Method: TryGetValue()
    //
    /// <summary>
    /// Attempts to get the value of a specified setting.
    /// </summary>
    ///
    /// <param name="settingName">
    /// The setting's name.
    /// </param>
    ///
    /// <param name="valueType">
    /// Expected type of the requested value.  Sample: typeof(String).
    /// </param>
    ///
    /// <param name="value">
    /// Where the value gets stored if true is returned, as an <see
    /// cref="Object" />.
    /// </param>
    ///
    /// <returns>
    /// true if the specified value was found, or false if not.
    /// </returns>
    //*************************************************************************

    protected Boolean
    TryGetValue
    (
        String settingName,
        Type valueType,
        out Object value
    )
    {
        AssertValid();

        value = null;

        Dictionary<String, Object> oSettings = GetAllSettings();

        // Although the worksheet that contains the settings is hidden, the
        // user may have unhidden it and edited the settings.  Therefore, don't
        // throw an exception if the value type is incorrect.

        if (oSettings.TryGetValue(settingName, out value) && value != null &&
            value.GetType() == valueType)
        {
            return (true);
        }

        return (false);
    }

    //*************************************************************************
    //  Method: GetAllSettings()
    //
    /// <summary>
    /// Gets all settings from the workbook.
    /// </summary>
    ///
    /// <returns>
    /// A dictionary of all settings.  The key is the setting name and the
    /// value is the setting value.
    /// </returns>
    ///
    /// <remarks>
    /// The settings are read once and then cached.
    ///
    /// <para>
    /// If the settings can't be read, the returned dictionary is empty.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    protected Dictionary<String, Object>
    GetAllSettings()
    {
        AssertValid();

        if (m_oSettings == null)
        {
            m_oSettings = new Dictionary<String, Object>();

            // Attempt to get the optional table and table columns that contain
            // the settings.

            ListObject oPerWorkbookSettingsTable;
            Range oNameColumnData, oValueColumnData;
            Object [,] aoNameColumnValues, aoValueColumnValues;

            if (
                TryGetPerWorkbookSettingsTable(out oPerWorkbookSettingsTable)
                &&
                ExcelTableUtil.TryGetTableColumnDataAndValues(
                    oPerWorkbookSettingsTable,
                    PerWorkbookSettingsTableColumnNames.Name,
                    out oNameColumnData, out aoNameColumnValues)
                &&
                ExcelTableUtil.TryGetTableColumnDataAndValues(
                    oPerWorkbookSettingsTable,
                    PerWorkbookSettingsTableColumnNames.Value,
                    out oValueColumnData, out aoValueColumnValues)
                )
            {
                Int32 iRows = oNameColumnData.Rows.Count;

                for (Int32 iRowOneBased = 1; iRowOneBased <= iRows;
                    iRowOneBased++)
                {
                    String sName;

                    if ( ExcelUtil.TryGetNonEmptyStringFromCell(
                        aoNameColumnValues, iRowOneBased, 1, out sName) )
                    {
                        m_oSettings[sName] =
                            aoValueColumnValues[iRowOneBased, 1];
                    }
                }
            }
        }

        return (m_oSettings);
    }

    //*************************************************************************
    //  Method: WriteAllSettings()
    //
    /// <summary>
    /// Writes all settings from the workbook.
    /// </summary>
    //*************************************************************************

    protected void
    WriteAllSettings()
    {
        AssertValid();
        Debug.Assert(m_oSettings != null);

        ListObject oPerWorkbookSettingsTable;

        if ( !TryGetPerWorkbookSettingsTable(out oPerWorkbookSettingsTable) )
        {
            return;
        }

        // Don't do this.  For some reason, it causes the table to misbehave
        // in the ExcelUtil.SetRangeValues() calls below when the workbook is
        // saved via TaskAutomator.
        //
        // The consequence of not clearing the table is that names and values
        // that are no longer used will remain in the table.
        //
        // ExcelTableUtil.ClearTable(oPerWorkbookSettingsTable);

        // Attempt to get the optional table columns that contain the settings.

        Range oNameColumnData, oValueColumnData;

        if (
            !ExcelTableUtil.TryGetTableColumnData(oPerWorkbookSettingsTable,
                PerWorkbookSettingsTableColumnNames.Name, out oNameColumnData)
            ||
            !ExcelTableUtil.TryGetTableColumnData(oPerWorkbookSettingsTable,
                PerWorkbookSettingsTableColumnNames.Value,
                out oValueColumnData)
            )
        {
            return;
        }

        // Copy the settings to arrays.

        Int32 iSettings = m_oSettings.Count;

        Object [,] aoNameColumnValues =
            ExcelUtil.GetSingleColumn2DArray(iSettings);

        Object [,] aoValueColumnValues =
            ExcelUtil.GetSingleColumn2DArray(iSettings);

        Int32 i = 1;

        foreach (KeyValuePair<String, Object> oKeyValuePair in m_oSettings)
        {
            aoNameColumnValues[i, 1] = oKeyValuePair.Key;

            aoValueColumnValues[i, 1] = 
                ExcelUtil.RemoveFormulaFromValue(oKeyValuePair.Value);

            i++;
        }

        // Write the arrays to the columns.

        ExcelUtil.SetRangeValues(oNameColumnData, aoNameColumnValues);
        ExcelUtil.SetRangeValues(oValueColumnData, aoValueColumnValues);
    }

    //*************************************************************************
    //  Method: TryGetPerWorkbookSettingsTable()
    //
    /// <summary>
    /// Attempts to get the per-workbook settings table.
    /// </summary>
    ///
    /// <param name="oPerWorkbookSettingsTable">
    /// Where the table gets stored if true is returned.
    /// </param>
    ///
    /// <returns>
    /// true if successful.
    /// </returns>
    //*************************************************************************

    protected Boolean
    TryGetPerWorkbookSettingsTable
    (
        out ListObject oPerWorkbookSettingsTable
    )
    {
        AssertValid();

        return (ExcelTableUtil.TryGetTable(m_oWorkbook,
            WorksheetNames.Miscellaneous, TableNames.PerWorkbookSettings,
            out oPerWorkbookSettingsTable) );
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

        Debug.Assert(m_oWorkbook != null);
        // m_oSettings
    }


    //*************************************************************************
    //  Setting name constants
    //*************************************************************************

    /// Name of the TemplateVersion setting.

    protected const String TemplateVersionSettingName = "Template Version";

    /// Name of the WorkbookSettingsCellCount setting.

    protected const String WorkbookSettingsCellCountSettingName =
        "Workbook Settings Cell Count";

    /// Base name for each setting cell that contains part of the split user
    /// settings.

    protected const String WorkbookSettingsCellBaseSettingName =
        "Workbook Settings ";

    /// Name of the GraphDirectedness setting.

    protected const String GraphDirectednessSettingName = "Graph Directedness";

    /// Name of the AutoLayoutOnOpen setting.

    protected const String AutoLayoutOnOpenSettingName =
        "Auto Layout on Open";

    /// Name of the AutomateTasksOnOpen setting.

    protected const String AutomateTasksOnOpenSettingName =
        "Automate Tasks on Open";

    /// Name of the AutoFillWorkbookResults setting.

    protected const String AutoFillWorkbookResultsSettingName =
        "Autofill Workbook Results";

    /// Name of the GraphHistory setting.

    protected const String GraphHistorySettingName =
        "Graph History";


    //*************************************************************************
    //  Default property value constants
    //*************************************************************************

    /// Default value of the TemplateVersion property.

    protected const Int32 DefaultTemplateVersion = 1;

    /// Default value of the GraphDirectedness property.

    protected const GraphDirectedness DefaultGraphDirectedness =
        GraphDirectedness.Undirected;


    //*************************************************************************
    //  Separator constants
    //*************************************************************************

    /// Field separator used by various classes that store joined strings in
    /// the per-workbook settings.  This is defined in both character array and
    /// string versions to accommodate String.Split() and String.Join().

    public static readonly Char [] FieldSeparator = StringUtil.FieldSeparator;
    ///
    public const String FieldSeparatorString = StringUtil.FieldSeparatorString;

    /// Sub-field separator.

    public static readonly Char [] SubFieldSeparator =
        StringUtil.SubFieldSeparator;
    ///
    public const String SubFieldSeparatorString =
        StringUtil.SubFieldSeparatorString;


    //*************************************************************************
    //  Other constants
    //*************************************************************************

    /// Maximum characters that can be written to an Excel cell.  See the
    /// WorkbookSettings property setter for details.

    protected const Int32 MaximumCharactersPerCell = 6000;


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// The workbook containing the settings.

    protected Microsoft.Office.Interop.Excel.Workbook m_oWorkbook;

    /// Dictionary of settings, or null if the settings haven't been read from
    /// the workbook yet.  The key is the setting name and the value is the
    /// setting value.
    ///
    /// Do not use this directly.  Use GetAllSettings() and WriteAllSettings()
    /// instead.

    protected Dictionary<String, Object> m_oSettings;
}
}
