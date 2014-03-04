
using System;
using System.Configuration;
using System.Diagnostics;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: ImportDataUserSettings
//
/// <summary>
/// Stores the user's settings for importing data into the workbook.
/// </summary>
//*****************************************************************************

[ SettingsGroupNameAttribute("ImportDataUserSettings") ]

public class ImportDataUserSettings : NodeXLApplicationSettingsBase
{
    //*************************************************************************
    //  Constructor: ImportDataUserSettings()
    //
    /// <summary>
    /// Initializes a new instance of the ImportDataUserSettings class.
    /// </summary>
    //*************************************************************************

    public ImportDataUserSettings()
    {
        // (Do nothing.)

        AssertValid();
    }

    //*************************************************************************
    //  Property: ClearTablesBeforeImport
    //
    /// <summary>
    /// Gets or sets a flag indicating whether NodeXL tables should be cleared
    /// before data is imported into the workbook.
    /// </summary>
    ///
    /// <value>
    /// If true, the NodeXL tables are cleared before data is imported into the
    /// workbook.  If false, the imported contents are appended to the
    /// workbook.  The default value is true.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("true") ]

    public Boolean
    ClearTablesBeforeImport
    {
        get
        {
            AssertValid();

            return ( (Boolean)this[ClearTablesBeforeImportKey] );
        }

        set
        {
            this[ClearTablesBeforeImportKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: SaveImportDescription
    //
    /// <summary>
    /// Gets or sets a flag indicating whether the description of the import
    /// technique should be saved in the graph's history.
    /// </summary>
    ///
    /// <value>
    /// true to save the description of the import technique in the graph's
    /// history.  The default value is false.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("false") ]

    public Boolean
    SaveImportDescription
    {
        get
        {
            AssertValid();

            return ( (Boolean)this[SaveImportDescriptionKey] );
        }

        set
        {
            this[SaveImportDescriptionKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: AutomateAfterImport
    //
    /// <summary>
    /// Gets or sets a flag indicating whether automation should be run after
    /// data is imported into the workbook.
    /// </summary>
    ///
    /// <value>
    /// true to run automation after data is imported.  The default value is
    /// false.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("false") ]

    public Boolean
    AutomateAfterImport
    {
        get
        {
            AssertValid();

            return ( (Boolean)this[AutomateAfterImportKey] );
        }

        set
        {
            this[AutomateAfterImportKey] = value;

            AssertValid();
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

        // (Do nothing else.)
    }


    //*************************************************************************
    //  Protected constants
    //*************************************************************************

    /// Name of the settings key for the ClearTablesBeforeImport property.

    protected const String ClearTablesBeforeImportKey =
        "ClearTablesBeforeImport";

    /// Name of the settings key for the SaveImportDescription property.

    protected const String SaveImportDescriptionKey =
        "SaveImportDescription";

    /// Name of the settings key for the AutomateAfterImport property.

    protected const String AutomateAfterImportKey =
        "AutomateAfterImport";


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}

}
