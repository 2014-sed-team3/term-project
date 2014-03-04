

using System;
using System.Diagnostics;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: RunCommandEventArgs
//
/// <summary>
/// Provides information for a command that needs to be run.
/// </summary>
///
/// <remarks>
/// NodeXL uses the "chain of responsibility" pattern for sending commands from
/// one UI object to another -- from the Ribbon to the TaskPane, for example,
/// and from the TaskPane to the workbook.  With this pattern, the sender sends
/// a command to one or more receivers without knowing which receiver will
/// handle the command.
///
/// <para>
/// In NodeXL's case, .NET's standard event mechanism is used to send and
/// receive the commands.  A command is distinguished by a class derived from
/// this <see cref="RunCommandEventArgs" /> base class.  The derived class may
/// include event information as properties when necessary.  A command
/// receiver, which implements a <see cref="RunCommandEventHandler" /> method,
/// determines which command needs to be run by checking the derived type of
/// the <see cref="RunCommandEventArgs" /> object that gets passed to the
/// method.
/// </para>
///
/// <para>
/// The <see cref="CommandDispatcher" /> static class is used by senders to
/// send commands.  Receivers receive commands by subscribing to the
/// CommandDispatcher.<see cref="CommandDispatcher.CommandSent" /> event.
/// </para>
///
/// </remarks>
//*****************************************************************************

public class RunCommandEventArgs : EventArgs
{
    //*************************************************************************
    //  Constructor: RunCommandEventArgs()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="RunCommandEventArgs" />
    /// class.
    /// </summary>
    //*************************************************************************

    public RunCommandEventArgs()
    {
        // (Do nothing.)

        // AssertValid();
    }


    //*************************************************************************
    //  Method: AssertValid()
    //
    /// <summary>
    /// Asserts if the object is in an invalid state.  Debug-only.
    /// </summary>
    //*************************************************************************

    [Conditional("DEBUG")] 

    public virtual void
    AssertValid()
    {
        // (Do nothing.)
    }


    //*************************************************************************
    //  Protected member data
    //*************************************************************************

    // (None.)
}


//*****************************************************************************
//  Delegate: RunCommandEventHandler
//
/// <summary>
/// Represents a method that will handle a command that needs to be run.
/// </summary>
///
/// <param name="sender">
/// The source of the event.
/// </param>
///
/// <param name="e">
/// A <see cref="RunCommandEventArgs" /> object that contains the event
/// data.  The method handling the command should determine which command needs
/// to be run by checking the derived type of this object.
/// </param>
//*****************************************************************************

public delegate void
RunCommandEventHandler
(
    Object sender,
    RunCommandEventArgs e
);

}
