

using System;
using System.Configuration;
using System.Diagnostics;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: ExportToNodeXLGraphGalleryUserSettings
//
/// <summary>
/// Stores the user's settings for exporting the graph to the NodeXL Graph
/// Gallery.
/// </summary>
//*****************************************************************************

[ SettingsGroupNameAttribute("ExportToNodeXLGraphGalleryUserSettings") ]

public class ExportToNodeXLGraphGalleryUserSettings :
    NodeXLApplicationSettingsBase
{
    //*************************************************************************
    //  Constructor: ExportToNodeXLGraphGalleryUserSettings()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="ExportToNodeXLGraphGalleryUserSettings" /> class.
    /// </summary>
    //*************************************************************************

    public ExportToNodeXLGraphGalleryUserSettings()
    {
        // (Do nothing.)

        AssertValid();
    }

    //*************************************************************************
    //  Property: Title
    //
    /// <summary>
    /// Gets or sets the graph's title.
    /// </summary>
    ///
    /// <value>
    /// The graph's title.  The default value is String.Empty.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("") ]

    public String
    Title
    {
        get
        {
            AssertValid();

            return ( (String)this[TitleKey] );
        }

        set
        {
            this[TitleKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: Description
    //
    /// <summary>
    /// Gets or sets the graph's description.
    /// </summary>
    ///
    /// <value>
    /// The graph's description.  The default value is String.Empty.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("") ]

    public String
    Description
    {
        get
        {
            AssertValid();

            return ( (String)this[DescriptionKey] );
        }

        set
        {
            this[DescriptionKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: SpaceDelimitedTags
    //
    /// <summary>
    /// Gets or sets the graph's space-delimited tags.
    /// </summary>
    ///
    /// <value>
    /// The graph's space-delimited tags.  The default value is String.Empty.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("") ]

    public String
    SpaceDelimitedTags
    {
        get
        {
            AssertValid();

            return ( (String)this[SpaceDelimitedTagsKey] );
        }

        set
        {
            this[SpaceDelimitedTagsKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: Author
    //
    /// <summary>
    /// Gets or sets the graph's author.
    /// </summary>
    ///
    /// <value>
    /// The graph's author.  The default value is String.Empty.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("") ]

    public String
    Author
    {
        get
        {
            AssertValid();

            return ( (String)this[AuthorKey] );
        }

        set
        {
            this[AuthorKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: UseCredentials
    //
    /// <summary>
    /// Gets or sets a flag indicating whether the user has credentials for the
    /// NodeXL Graph Gallery.
    /// </summary>
    ///
    /// <value>
    /// true if the user has credentials.  The default value is false.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("false") ]

    public Boolean
    UseCredentials
    {
        get
        {
            AssertValid();

            return ( (Boolean)this[UseCredentialsKey] );
        }

        set
        {
            this[UseCredentialsKey] = value;

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

    /// Name of the settings key for the Title property.

    protected const String TitleKey =
        "Title";

    /// Name of the settings key for the Description property.

    protected const String DescriptionKey =
        "Description";

    /// Name of the settings key for the SpaceDelimitedTags property.

    protected const String SpaceDelimitedTagsKey =
        "SpaceDelimitedTags";

    /// Name of the settings key for the Author property.

    protected const String AuthorKey =
        "Author";

    /// Name of the settings key for the ExportWorkbookAndSettings property.

    protected const String ExportWorkbookAndSettingsKey =
        "ExportWorkbookAndSettings";

    /// Name of the settings key for the ExportGraphML property.

    protected const String ExportGraphMLKey =
        "ExportGraphML";

    /// Name of the settings key for the UseFixedAspectRatio property.

    protected const String UseFixedAspectRatioKey =
        "UseFixedAspectRatio";

    /// Name of the settings key for the UseCredentials property.

    protected const String UseCredentialsKey =
        "UseCredentials";
}
}
