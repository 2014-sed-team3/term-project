

using System;
using System.Configuration;
using System.Diagnostics;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: PasswordUserSettings
//
/// <summary>
/// Stores some of the user's passwords.
/// </summary>
///
/// <remarks>
/// The passwords get saved in the user's local profile, NOT the workbook.
/// </remarks>
//*****************************************************************************

[ SettingsGroupNameAttribute("PasswordUserSettings") ]

public class PasswordUserSettings : NodeXLApplicationSettingsBase
{
    //*************************************************************************
    //  Constructor: PasswordUserSettings()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="PasswordUserSettings" /> class.
    /// </summary>
    //*************************************************************************

    public PasswordUserSettings()
    :
    base(false)
    {
        // (Note that the NodeXLApplicationSettingsBase base class is told to
        // use the standard settings file instead of the workbook settings for
        // password user settings.  We do not want the password user settings
        // to travel with the workbook.)

        AssertValid();
    }

    //*************************************************************************
    //  Property: NodeXLGraphGalleryPassword
    //
    /// <summary>
    /// Gets or sets the user's password for the NodeXL Graph Gallery.
    /// </summary>
    ///
    /// <value>
    /// The user's password.  The default value is String.Empty.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("") ]

    public String
    NodeXLGraphGalleryPassword
    {
        get
        {
            AssertValid();

            return ( (String)this[NodeXLGraphGalleryPasswordKey] );
        }

        set
        {
            this[NodeXLGraphGalleryPasswordKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: SmtpPassword
    //
    /// <summary>
    /// Gets or sets the user's password for the SMTP server he uses to export
    /// the graph to email.
    /// </summary>
    ///
    /// <value>
    /// The user's password.  The default value is String.Empty.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("") ]

    public String
    SmtpPassword
    {
        get
        {
            AssertValid();

            return ( (String)this[SmtpPasswordKey] );
        }

        set
        {
            this[SmtpPasswordKey] = value;

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
    //  Protected fields
    //*************************************************************************

    /// Name of the settings key for the NodeXLGraphGalleryPassword property.

    protected const String NodeXLGraphGalleryPasswordKey =
        "NodeXLGraphGalleryPassword";

    /// Name of the settings key for the SmtpPassword property.

    protected const String SmtpPasswordKey =
        "SmtpPassword";
}
}
