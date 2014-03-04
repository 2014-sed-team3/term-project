
using System;
using System.Configuration;
using System.Diagnostics;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: ColumnGroupUserSettings
//
/// <summary>
/// Stores the user's settings for which column groups to show.
/// </summary>
//*****************************************************************************

[ SettingsGroupNameAttribute("ColumnGroupUserSettings") ]

public class ColumnGroupUserSettings : NodeXLApplicationSettingsBase
{
    //*************************************************************************
    //  Constructor: ColumnGroupUserSettings()
    //
    /// <summary>
    /// Initializes a new instance of the ColumnGroupUserSettings class.
    /// </summary>
    //*************************************************************************

    public ColumnGroupUserSettings()
    {
        // (Do nothing.)

        AssertValid();
    }

    //*************************************************************************
    //  Property: ColumnGroupsToShow
    //
    /// <summary>
    /// Gets or sets the column groups to show.
    /// </summary>
    ///
    /// <value>
    /// The column groups to show, as an ORed combination of <see
    /// cref="ColumnGroups" /> flags.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]

    [ DefaultSettingValueAttribute(

        "EdgeDoNotHide,EdgeVisualAttributes,EdgeLabels,EdgeOtherColumns,"

        + "VertexDoNotHide,VertexVisualAttributes,VertexLabels,"
        + "VertexOtherColumns,"
        
        + "GroupDoNotHide,GroupVisualAttributes,GroupLabels,"

        + "GroupEdgeDoNotHide"
        ) ]

    public ColumnGroups
    ColumnGroupsToShow
    {
        get
        {
            AssertValid();

            return ( (ColumnGroups)this[ColumnGroupsToShowKey] );
        }

        set
        {
            this[ColumnGroupsToShowKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Method: ShouldShowColumnGroup
    //
    /// <summary>
    /// Gets a flag indicating whether a specified column group should be
    /// shown.
    /// </summary>
    ///
    /// <param name="columnGroup">
    /// One column group to test, NOT an ORed combination.
    /// </param>
    ///
    /// <param name="columnGroupsToShow">
    /// The column groups to show, as an ORed combination of <see
    /// cref="ColumnGroups" /> flags.  This should be obtained from the <see
    /// cref="ColumnGroupsToShow" /> property.
    /// </param>
    ///
    /// <returns>
    /// true if <paramref name="columnGroup" /> should be shown.
    /// </returns>
    ///
    /// <remarks>
    /// Although this method could read the <see cref="ColumnGroupsToShow" />
    /// property itself, that involves some file copying (see <see
    /// cref="NodeXLApplicationSettingsBase" />) and is therefore expensive.
    /// The caller should read the property and then call this method multiple
    /// times if necessary using the saved property value.
    /// </remarks>
    //*************************************************************************

    public Boolean
    ShouldShowColumnGroup
    (
        ColumnGroups columnGroup,
        ColumnGroups columnGroupsToShow
    )
    {
        AssertValid();

        return ( (columnGroupsToShow & columnGroup) != 0 );
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

    /// Name of the settings key for the ColumnGroupsToShow property.

    protected const String ColumnGroupsToShowKey =
        "ColumnGroupsToShow";


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}

}
