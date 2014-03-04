
using System;
using System.Configuration;
using System.Diagnostics;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: MergeDuplicateEdgesUserSettings
//
/// <summary>
/// Stores the user's settings for merging duplicate edges.
/// </summary>
//*****************************************************************************

[ SettingsGroupNameAttribute("MergeDuplicateEdgesUserSettings") ]

public class MergeDuplicateEdgesUserSettings : NodeXLApplicationSettingsBase
{
    //*************************************************************************
    //  Constructor: MergeDuplicateEdgesUserSettings()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="MergeDuplicateEdgesUserSettings" /> class.
    /// </summary>
    //*************************************************************************

    public MergeDuplicateEdgesUserSettings()
    {
        // (Do nothing.)

        AssertValid();
    }

    //*************************************************************************
    //  Property: CountDuplicates
    //
    /// <summary>
    /// Gets or sets a flag indicating whether duplicates should be counted.
    /// </summary>
    ///
    /// <value>
    /// true to count duplicates and include the counts in a new column.  The
    /// default value is true.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("true") ]

    public Boolean
    CountDuplicates
    {
        get
        {
            AssertValid();

            return ( (Boolean)this[CountDuplicatesKey] );
        }

        set
        {
            this[CountDuplicatesKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: DeleteDuplicates
    //
    /// <summary>
    /// Gets or sets a flag indicating whether duplicates should be deleted.
    /// </summary>
    ///
    /// <value>
    /// true to delete all but one edge in each set of duplicate edges.  The
    /// default value is false.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("false") ]

    public Boolean
    DeleteDuplicates
    {
        get
        {
            AssertValid();

            return ( (Boolean)this[DeleteDuplicatesKey] );
        }

        set
        {
            this[DeleteDuplicatesKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: ThirdColumnNameForDuplicateDetection
    //
    /// <summary>
    /// Gets or sets the name of the third column that is used to determine
    /// whether two edges are duplicates.
    /// </summary>
    ///
    /// <value>
    /// If this is null or empty, only the edges' vertices are used to
    /// determine whether the edges are duplicates.  If a column name is
    /// specified, the edges' vertices and the values in the specified column
    /// are used to determine whether the edges are duplicates.  The default
    /// value is an empty string.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("") ]

    public String
    ThirdColumnNameForDuplicateDetection
    {
        get
        {
            AssertValid();

            return ( (String)this[ThirdColumnNameForDuplicateDetectionKey] );
        }

        set
        {
            this[ThirdColumnNameForDuplicateDetectionKey] = value;

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

    /// Name of the settings key for the CountDuplicates property.

    protected const String CountDuplicatesKey =
        "CountDuplicates";

    /// Name of the settings key for the DeleteDuplicates property.

    protected const String DeleteDuplicatesKey =
        "DeleteDuplicates";

    /// Name of the settings key for the ThirdColumnNameForDuplicateDetection
    /// property.

    protected const String ThirdColumnNameForDuplicateDetectionKey =
        "ThirdColumnNameForDuplicateDetection";


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}

}
