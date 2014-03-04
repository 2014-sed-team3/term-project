
using System;
using System.Diagnostics;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: RunGroupByClusterCommandEventArgs
//
/// <summary>
/// Provides information for a "group by cluster" command that needs to be run.
/// </summary>
///
/// <remarks>
/// See <see cref="RunCommandEventArgs" /> for information about how NodeXL
/// sends commands from one UI object to another.
/// </remarks>
//*****************************************************************************

public class RunGroupByClusterCommandEventArgs : RunEditableCommandEventArgs
{
    //*************************************************************************
    //  Constructor: RunGroupByClusterCommandEventArgs()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="RunGroupByClusterCommandEventArgs" /> class.
    /// </summary>
    ///
    /// <param name="editUserSettings">
    /// true to allow the user to edit the user settings for the command before
    /// the command is run.
    /// </param>
    //*************************************************************************

    public RunGroupByClusterCommandEventArgs
    (
        Boolean editUserSettings
    )
    : base(editUserSettings)
    {
        // (Do nothing else.)

        AssertValid();
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
    //  Protected member data
    //*************************************************************************

    // (None.)
}

}
