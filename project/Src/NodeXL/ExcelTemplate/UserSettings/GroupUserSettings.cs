
using System;
using System.Configuration;
using System.Diagnostics;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: GroupUserSettings
//
/// <summary>
/// Stores the user's group settings.
/// </summary>
//*****************************************************************************

[ SettingsGroupNameAttribute("GroupUserSettings") ]

public class GroupUserSettings : NodeXLApplicationSettingsBase
{
    //*************************************************************************
    //  Constructor: GroupUserSettings()
    //
    /// <summary>
    /// Initializes a new instance of the GroupUserSettings class.
    /// </summary>
    //*************************************************************************

    public GroupUserSettings()
    {
        // (Do nothing.)

        AssertValid();
    }

    //*************************************************************************
    //  Property: ReadGroups
    //
    /// <summary>
    /// Gets or sets a flag indicating whether the group worksheets should be
    /// read when the workbook is read into the graph.
    /// </summary>
    ///
    /// <value>
    /// true to read the group worksheets, false to completely ignore them.
    /// The default value is true.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("true") ]

    public Boolean
    ReadGroups
    {
        get
        {
            AssertValid();

            return ( (Boolean)this[ReadGroupsKey] );
        }

        set
        {
            this[ReadGroupsKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: ReadVertexColorFromGroups
    //
    /// <summary>
    /// Gets or sets a flag indicating whether the color of a group's vertices
    /// should be read from the Groups worksheet.
    /// </summary>
    ///
    /// <value>
    /// true to set the color of a group's vertices to the vertex color
    /// specified on the Groups worksheet, false to not modify the color of the
    /// vertices.  The default value is true.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("true") ]

    public Boolean
    ReadVertexColorFromGroups
    {
        get
        {
            AssertValid();

            return ( (Boolean)this[ReadVertexColorFromGroupsKey] );
        }

        set
        {
            this[ReadVertexColorFromGroupsKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: ReadVertexShapeFromGroups
    //
    /// <summary>
    /// Gets or sets a flag indicating whether the shape of a group's vertices
    /// should be read from the Groups worksheet.
    /// </summary>
    ///
    /// <value>
    /// true to set the shape of a group's vertices to the vertex shape
    /// specified on the Groups worksheet, false to not modify the shape of the
    /// vertices.  The default value is true.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("true") ]

    public Boolean
    ReadVertexShapeFromGroups
    {
        get
        {
            AssertValid();

            return ( (Boolean)this[ReadVertexShapeFromGroupsKey] );
        }

        set
        {
            this[ReadVertexShapeFromGroupsKey] = value;

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

    /// Name of the settings key for the ReadGroups property.

    protected const String ReadGroupsKey =
        "ReadGroups";

    /// Name of the settings key for the ReadVertexColorFromGroups property.

    protected const String ReadVertexColorFromGroupsKey =
        "ReadVertexColorFromGroups";

    /// Name of the settings key for the ReadVertexShapeFromGroups property.

    protected const String ReadVertexShapeFromGroupsKey =
        "ReadVertexShapeFromGroups";


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}

}
