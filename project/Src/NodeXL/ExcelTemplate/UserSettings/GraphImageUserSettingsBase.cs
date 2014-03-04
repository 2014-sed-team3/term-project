
using System;
using System.Configuration;
using System.Drawing;
using System.Diagnostics;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: GraphImageUserSettingsBase
//
/// <summary>
/// Base class for several classes that store the user's settings for saving
/// graph images.
/// </summary>
//*****************************************************************************

public class GraphImageUserSettingsBase : NodeXLApplicationSettingsBase
{
    //*************************************************************************
    //  Constructor: GraphImageUserSettingsBase()
    //
    /// <summary>
    /// Initializes a new instance of the GraphImageUserSettingsBase class.
    /// </summary>
    //*************************************************************************

    public GraphImageUserSettingsBase()
    {
        // (Do nothing.)

        AssertValid();
    }

    //*************************************************************************
    //  Property: IncludeHeader
    //
    /// <summary>
    /// Gets or sets a flag specifying whether the saved graph image should
    /// include a header.
    /// </summary>
    ///
    /// <value>
    /// true to include a header.
    /// </value>
    ///
    /// <remarks>
    /// If true, the header text must be specified with <see
    /// cref="HeaderText" />.
    /// </remarks>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("false") ]

    public Boolean
    IncludeHeader
    {
        get
        {
            AssertValid();

            return ( (Boolean)this[IncludeHeaderKey] );
        }

        set
        {
            this[IncludeHeaderKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: HeaderText
    //
    /// <summary>
    /// Gets or sets the header text to include in the saved graph image.
    /// </summary>
    ///
    /// <value>
    /// The header text to include.  Can be empty, but can't be null.
    /// </value>
    ///
    /// <remarks>
    /// This is used only if <see cref="IncludeHeader" /> is true.
    /// </remarks>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("") ]

    public String
    HeaderText
    {
        get
        {
            AssertValid();

            return ( (String)this[HeaderTextKey] );
        }

        set
        {
            this[HeaderTextKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: IncludeFooter
    //
    /// <summary>
    /// Gets or sets a flag specifying whether the saved graph image should
    /// include a footer.
    /// </summary>
    ///
    /// <value>
    /// true to include a footer.
    /// </value>
    ///
    /// <remarks>
    /// If true, the footer text must be specified with <see
    /// cref="FooterText" />.
    /// </remarks>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("true") ]

    public Boolean
    IncludeFooter
    {
        get
        {
            AssertValid();

            return ( (Boolean)this[IncludeFooterKey] );
        }

        set
        {
            this[IncludeFooterKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: FooterText
    //
    /// <summary>
    /// Gets or sets the footer text to include in the saved graph image.
    /// </summary>
    ///
    /// <value>
    /// The footer text to include.  Can be empty, but can't be null.
    /// </value>
    ///
    /// <remarks>
    /// This is used only if <see cref="IncludeFooter" /> is true.
    /// </remarks>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute(
    "Created with NodeXL (http://nodexl.codeplex.com)") ]

    public String
    FooterText
    {
        get
        {
            AssertValid();

            return ( (String)this[FooterTextKey] );
        }

        set
        {
            this[FooterTextKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: HeaderFooterFont
    //
    /// <summary>
    /// Gets or sets the font used for the header and footer.
    /// </summary>
    ///
    /// <value>
    /// The header and footer font, as a Font.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute(GeneralUserSettings.DefaultFont) ]

    public Font
    HeaderFooterFont
    {
        get
        {
            AssertValid();

            return ( (Font)this[HeaderFooterFontKey] );
        }

        set
        {
            this[HeaderFooterFontKey] = value;

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

    /// Name of the settings key for the IncludeHeader property.

    protected const String IncludeHeaderKey =
        "IncludeHeader";

    /// Name of the settings key for the HeaderText property.

    protected const String HeaderTextKey =
        "HeaderText";

    /// Name of the settings key for the IncludeFooter property.

    protected const String IncludeFooterKey =
        "IncludeFooter";

    /// Name of the settings key for the FooterText property.

    protected const String FooterTextKey =
        "FooterText";

    /// Name of the settings key for the HeaderFooterFont property.

    protected const String HeaderFooterFontKey =
        "HeaderFooterFont";


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}

}
