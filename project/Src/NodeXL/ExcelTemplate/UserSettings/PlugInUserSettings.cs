
using System;
using System.Configuration;
using System.Diagnostics;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: PlugInUserSettings
//
/// <summary>
/// Stores the user's settings for NodeXL plug-ins.
/// </summary>
//*****************************************************************************

[ SettingsGroupNameAttribute("PlugInUserSettings") ]

public class PlugInUserSettings : NodeXLApplicationSettingsBase
{
    //*************************************************************************
    //  Constructor: PlugInUserSettings()
    //
    /// <summary>
    /// Initializes a new instance of the PlugInUserSettings class.
    /// </summary>
    //*************************************************************************

    public PlugInUserSettings()
    :
    base(false)
    {
        // (Note that the NodeXLApplicationSettingsBase base class is told to
        // use the standard settings file instead of the workbook settings for
        // plug-in user settings.  We do not want the plug-in user settings to
        // travel with the workbook.)

        AssertValid();
    }

    //*************************************************************************
    //  Property: PlugInFolderPath
    //
    /// <summary>
    /// Gets or sets the path to the plug-in folder.
    /// </summary>
    ///
    /// <value>
    /// The full path to the plug-in folder.  The default value is
    /// String.Empty.
    /// </value>
    ///
    /// <remarks>
    /// The plug-in folder is where the user can place NodeXL plug-in
    /// assemblies.
    /// </remarks>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("") ]

    public String
    PlugInFolderPath
    {
        get
        {
            AssertValid();

            return ( (String)this[PlugInFolderPathKey] );
        }

        set
        {
            this[PlugInFolderPathKey] = value;

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

    /// Name of the settings key for the PlugInFolderPath property.

    protected const String PlugInFolderPathKey =
        "PlugInFolderPath";


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}

}
