

using System;
using System.Configuration;
using System.Diagnostics;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: ExportToEmailUserSettings
//
/// <summary>
/// Stores the user's settings for exporting the graph to email.
/// </summary>
//*****************************************************************************

[ SettingsGroupNameAttribute("ExportToEmailUserSettings") ]

public class ExportToEmailUserSettings : NodeXLApplicationSettingsBase
{
    //*************************************************************************
    //  Constructor: ExportToEmailUserSettings()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="ExportToEmailUserSettings" /> class.
    /// </summary>
    //*************************************************************************

    public ExportToEmailUserSettings()
    {
        // (Do nothing.)

        AssertValid();
    }

    //*************************************************************************
    //  Property: SpaceDelimitedToAddresses
    //
    /// <summary>
    /// Gets or sets a space-delimited set of "to" email addresses.
    /// </summary>
    ///
    /// <value>
    /// A space-delimited set of "to" email addresses.  The default value is
    /// String.Empty.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("") ]

    public String
    SpaceDelimitedToAddresses
    {
        get
        {
            AssertValid();

            return ( (String)this[SpaceDelimitedToAddressesKey] );
        }

        set
        {
            this[SpaceDelimitedToAddressesKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: FromAddress
    //
    /// <summary>
    /// Gets or sets the "from" email address.
    /// </summary>
    ///
    /// <value>
    /// The "from" email address.  The default value is String.Empty.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("") ]

    public String
    FromAddress
    {
        get
        {
            AssertValid();

            return ( (String)this[FromAddressKey] );
        }

        set
        {
            this[FromAddressKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: Subject
    //
    /// <summary>
    /// Gets or sets the email subject.
    /// </summary>
    ///
    /// <value>
    /// The email subject.  The default value is String.Empty.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("") ]

    public String
    Subject
    {
        get
        {
            AssertValid();

            return ( (String)this[SubjectKey] );
        }

        set
        {
            this[SubjectKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: MessageBody
    //
    /// <summary>
    /// Gets or sets the email's message body.
    /// </summary>
    ///
    /// <value>
    /// The message body subject.  The default value is String.Empty.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("") ]

    public String
    MessageBody
    {
        get
        {
            AssertValid();

            return ( (String)this[MessageBodyKey] );
        }

        set
        {
            this[MessageBodyKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: SmtpHost
    //
    /// <summary>
    /// Gets or sets the SMTP host name.
    /// </summary>
    ///
    /// <value>
    /// The SMTP host name.  The default value is String.Empty.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("") ]

    public String
    SmtpHost
    {
        get
        {
            AssertValid();

            return ( (String)this[SmtpHostKey] );
        }

        set
        {
            this[SmtpHostKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: SmtpPort
    //
    /// <summary>
    /// Gets or sets the SMTP port.
    /// </summary>
    ///
    /// <value>
    /// The SMTP port.  The default value is 587.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("587") ]

    public Int32
    SmtpPort
    {
        get
        {
            AssertValid();

            return ( (Int32)this[SmtpPortKey] );
        }

        set
        {
            this[SmtpPortKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: UseSslForSmtp
    //
    /// <summary>
    /// Gets or sets a flag specifying whether SSL should be used.
    /// </summary>
    ///
    /// <value>
    /// true to use SSL.  The default value is true.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("true") ]

    public Boolean
    UseSslForSmtp
    {
        get
        {
            AssertValid();

            return ( (Boolean)this[UseSslForSmtpKey] );
        }

        set
        {
            this[UseSslForSmtpKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: SmtpUserName
    //
    /// <summary>
    /// Gets or sets the SMTP user name.
    /// </summary>
    ///
    /// <value>
    /// The SMTP user name.  The default value is String.Empty.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("") ]

    public String
    SmtpUserName
    {
        get
        {
            AssertValid();

            return ( (String)this[SmtpUserNameKey] );
        }

        set
        {
            this[SmtpUserNameKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: ExportWorkbookAndSettings
    //
    /// <summary>
    /// Gets or sets a flag indicating whether the workbook and its settings
    /// should be exported.
    /// </summary>
    ///
    /// <value>
    /// true to export the workbook and its settings.  The default value is
    /// false.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("false") ]

    public Boolean
    ExportWorkbookAndSettings
    {
        get
        {
            AssertValid();

            return ( (Boolean)this[ExportWorkbookAndSettingsKey] );
        }

        set
        {
            this[ExportWorkbookAndSettingsKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: ExportGraphML
    //
    /// <summary>
    /// Gets or sets a flag indicating whether GraphML should be exported.
    /// </summary>
    ///
    /// <value>
    /// true to export GraphML.  The default value is false.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("false") ]

    public Boolean
    ExportGraphML
    {
        get
        {
            AssertValid();

            return ( (Boolean)this[ExportGraphMLKey] );
        }

        set
        {
            this[ExportGraphMLKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: UseFixedAspectRatio
    //
    /// <summary>
    /// Gets or sets a flag indicating whether the exported image should have
    /// a fixed aspect ratio.
    /// </summary>
    ///
    /// <value>
    /// true to use a fixed aspect ratio, false to use the aspect ratio of the
    /// graph pane.  The default value is false.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("false") ]

    public Boolean
    UseFixedAspectRatio
    {
        get
        {
            AssertValid();

            return ( (Boolean)this[UseFixedAspectRatioKey] );
        }

        set
        {
            this[UseFixedAspectRatioKey] = value;

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

    /// Name of the settings key for the SpaceDelimitedToAddresses property.

    protected const String SpaceDelimitedToAddressesKey =
        "SpaceDelimitedToAddresses";

    /// Name of the settings key for the FromAddress property.

    protected const String FromAddressKey =
        "FromAddress";

    /// Name of the settings key for the Subject property.

    protected const String SubjectKey =
        "Subject";

    /// Name of the settings key for the MessageBody property.

    protected const String MessageBodyKey =
        "MessageBody";

    /// Name of the settings key for the SmtpHost property.

    protected const String SmtpHostKey =
        "SmtpHost";

    /// Name of the settings key for the SmtpPort property.

    protected const String SmtpPortKey =
        "SmtpPort";

    /// Name of the settings key for the UseSslForSmtp property.

    protected const String UseSslForSmtpKey =
        "UseSslForSmtp";

    /// Name of the settings key for the SmtpUserName property.

    protected const String SmtpUserNameKey =
        "SmtpUserName";

    /// Name of the settings key for the ExportWorkbookAndSettings property.

    protected const String ExportWorkbookAndSettingsKey =
        "ExportWorkbookAndSettings";

    /// Name of the settings key for the ExportGraphML property.

    protected const String ExportGraphMLKey =
        "ExportGraphML";

    /// Name of the settings key for the UseFixedAspectRatio property.

    protected const String UseFixedAspectRatioKey =
        "UseFixedAspectRatio";
}
}
