
using System;
using System.Configuration;
using System.Diagnostics;

namespace Smrf.NodeXL.GraphMLFileProcessor
{
//*****************************************************************************
//  Class: GeneralUserSettings
//
/// <summary>
/// Stores the user's general settings.
/// </summary>
//*****************************************************************************

[ SettingsGroupNameAttribute("GeneralUserSettings") ]

public class GeneralUserSettings : ApplicationSettingsBase
{
    //*************************************************************************
    //  Constructor: GeneralUserSettings()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="GeneralUserSettings" />
    /// class.
    /// </summary>
    //*************************************************************************

    public GeneralUserSettings()
    {
        // (Do nothing.)

        AssertValid();
    }

    //*************************************************************************
    //  Property: GraphMLFolderPath
    //
    /// <summary>
    /// Gets or sets the path to the GraphML file folder.
    /// </summary>
    ///
    /// <value>
    /// The path to the GraphML file folder.  The default value is
    /// String.Empty.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("") ]

    public String
    GraphMLFolderPath
    {
        get
        {
            AssertValid();

            return ( (String)this[GraphMLFolderPathKey] );
        }

        set
        {
            this[GraphMLFolderPathKey] = value;

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

    [Conditional("DEBUG")]

    public void
    AssertValid()
    {
        // (Do nothing.)
    }


    //*************************************************************************
    //  Protected constants
    //*************************************************************************

    /// Name of the settings key for the GraphMLFolderPath property.

    protected const String GraphMLFolderPathKey =
        "GraphMLFolderPath";


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}

}
