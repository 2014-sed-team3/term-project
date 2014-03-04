
using System;
using System.Diagnostics;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: RunGroupCommandEventArgs
//
/// <summary>
/// Provides information for a group command that needs to be run.
/// </summary>
///
/// <remarks>
/// See <see cref="RunCommandEventArgs" /> for information about how NodeXL
/// sends commands from one UI object to another.
/// </remarks>
//*****************************************************************************

public class RunGroupCommandEventArgs : RunCommandEventArgs
{
    //*************************************************************************
    //  Constructor: RunGroupCommandEventArgs()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="RunGroupCommandEventArgs" /> class.
    /// </summary>
    ///
    /// <param name="groupCommand">
    /// The single group command that needs to be run.
    /// </param>
    ///
    /// <remarks>
    /// Note that although <see cref="GroupCommands" /> flags can be ORed
    /// together, the <paramref name="groupCommand" /> argument should include
    /// only one of these flags.
    /// </remarks>
    //*************************************************************************

    public RunGroupCommandEventArgs
    (
        GroupCommands groupCommand
    )
    {
        m_eGroupCommand = groupCommand;

        AssertValid();
    }

    //*************************************************************************
    //  Property: GroupCommand
    //
    /// <summary>
    /// Gets the single group command that needs to be run.
    /// </summary>
    ///
    /// <value>
    /// The group single command that needs to be run.
    /// </value>
    //*************************************************************************

    public GroupCommands
    GroupCommand
    {
        get
        {
            AssertValid();

            return (m_eGroupCommand);
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

        // m_eGroupCommand
    }


    //*************************************************************************
    //  Protected member data
    //*************************************************************************

    /// The single group command that needs to be run.

    protected GroupCommands m_eGroupCommand;
}

}
