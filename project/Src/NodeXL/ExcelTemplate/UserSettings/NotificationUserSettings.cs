
using System;
using System.Configuration;
using System.Diagnostics;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: NotificationUserSettings
//
/// <summary>
/// Stores the user's settings for notifications that can be disabled by the
/// user.
/// </summary>
//*****************************************************************************

[ SettingsGroupNameAttribute("NotificationUserSettings") ]

public class NotificationUserSettings : NodeXLApplicationSettingsBase
{
    //*************************************************************************
    //  Constructor: NotificationUserSettings()
    //
    /// <summary>
    /// Initializes a new instance of the NotificationUserSettings class.
    /// </summary>
    //*************************************************************************

    public NotificationUserSettings()
    :
    base(false)
    {
        // (Note that the NodeXLApplicationSettingsBase base class is told to
        // use the standard settings file instead of the workbook settings for
        // notification user settings.  We do not want the notification user
        // settings to travel with the workbook.)

        // (Do nothing.)

        AssertValid();
    }

    //*************************************************************************
    //  Property: LayoutTypeIsNull
    //
    /// <summary>
    /// Gets or sets a flag specifying whether the user should be warned that
    /// the layout type is set to LayoutType.Null when the workbook is read.
    /// </summary>
    ///
    /// <value>
    /// true to warn the user.  The default is true.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("true") ]

    public Boolean
    LayoutTypeIsNull
    {
        get
        {
            AssertValid();

            return ( (Boolean)this[LayoutTypeIsNullKey] );
        }

        set
        {
            this[LayoutTypeIsNullKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: TextWrapWillBeTurnedOff
    //
    /// <summary>
    /// Gets or sets a flag specifying whether the user should be warned before
    /// text wrapping is turned off.
    /// </summary>
    ///
    /// <value>
    /// true to warn the user.  The default is true.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("true") ]

    public Boolean
    TextWrapWillBeTurnedOff
    {
        get
        {
            AssertValid();

            return ( (Boolean)this[TextWrapWillBeTurnedOffKey] );
        }

        set
        {
            this[TextWrapWillBeTurnedOffKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Method: EnableAllNotifications()
    //
    /// <summary>
    /// Enables all user notifications maintained by this class.
    /// </summary>
    //*************************************************************************

    public void
    EnableAllNotifications()
    {
        this.LayoutTypeIsNull = true;
        this.TextWrapWillBeTurnedOff = true;
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

    /// Name of the settings key for the LayoutTypeIsNull property.

    protected const String LayoutTypeIsNullKey =
        "LayoutTypeIsNull";

    /// Name of the settings key for the TextWrapWillBeTurnedOff property.

    protected const String TextWrapWillBeTurnedOffKey =
        "TextWrapWillBeTurnedOff";


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}

}
