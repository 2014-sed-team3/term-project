
using System;
using System.Diagnostics;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: RunMergeDuplicateEdgesCommandEventArgs
//
/// <summary>
/// Provides information for a "merge duplicate edges" command that needs to be
/// run.
/// </summary>
///
/// <remarks>
/// See <see cref="RunCommandEventArgs" /> for information about how NodeXL
/// sends commands from one UI object to another.
/// </remarks>
//*****************************************************************************

public class RunMergeDuplicateEdgesCommandEventArgs :
    RunEditableCommandEventArgs
{
    //*************************************************************************
    //  Constructor: RunMergeDuplicateEdgesCommandEventArgs()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="RunMergeDuplicateEdgesCommandEventArgs" /> class.
    /// </summary>
    ///
    /// <param name="editUserSettings">
    /// true to allow the user to edit the user settings for the command before
    /// the command is run.
    /// </param>
    //*************************************************************************

    public RunMergeDuplicateEdgesCommandEventArgs
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
