
using System;
using System.Configuration;
using System.Diagnostics;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: DynamicFiltersUserSettings
//
/// <summary>
/// Stores the user's settings for dynamic filters.
/// </summary>
//*****************************************************************************

[ SettingsGroupNameAttribute("DynamicFiltersUserSettings") ]

public class DynamicFiltersUserSettings : NodeXLApplicationSettingsBase
{
    //*************************************************************************
    //  Constructor: DynamicFiltersUserSettings()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="DynamicFiltersUserSettings" /> class.
    /// </summary>
    //*************************************************************************

    public DynamicFiltersUserSettings()
    {
        // (Do nothing.)

        AssertValid();
    }

    //*************************************************************************
    //  Property: FilteredAlpha
    //
    /// <summary>
    /// Gets or sets the alpha component to use for vertices and edges that are
    /// filtered.
    /// </summary>
    ///
    /// <value>
    /// The alpha component to use for vertices and edges that are filtered, as
    /// a Single.  The default value is 0.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("0") ]

    public Single
    FilteredAlpha
    {
        get
        {
            AssertValid();

            return ( (Single)this[FilteredAlphaKey] );
        }

        set
        {
            this[FilteredAlphaKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: FilterNonNumericCells
    //
    /// <summary>
    /// Gets or sets a flag specifying whether a non-numeric cell should cause
    /// a vertex or edge to get filtered out of the graph.
    /// </summary>
    ///
    /// <value>
    /// true if a non-numeric cell should cause a vertex or edge to get
    /// filtered out of the graph.  The default value is false.
    /// </value>
    ///
    /// <remarks>
    /// An empty cell is considered non-numeric.
    /// </remarks>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("false") ]

    public Boolean
    FilterNonNumericCells
    {
        get
        {
            AssertValid();

            return ( (Boolean)this[FilterNonNumericCellsKey] );
        }

        set
        {
            this[FilterNonNumericCellsKey] = value;

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

    /// Name of the settings key for the FilteredAlpha property.

    protected const String FilteredAlphaKey =
        "FilteredAlpha";

    /// Name of the settings key for the FilterNonNumericCells property.

    protected const String FilterNonNumericCellsKey =
        "FilterNonNumericCells";


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}

}
