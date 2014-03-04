
using System;
using System.Diagnostics;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: CommandDispatcher
//
/// <summary>
/// Centralizes the sending and receiving of event-based commands.
/// </summary>
///
/// <remarks>
/// This static class is used by senders to send event-based commands, and by
/// receivers to receive such commands.  See the <see
/// cref="RunCommandEventArgs" /> class for details on the "chain of
/// responsibility" pattern that NodeXL uses for sending commands from
/// one UI object to another -- from the Ribbon to the TaskPane, for example.
/// </remarks>
//*****************************************************************************

public static class CommandDispatcher : Object
{
    //*************************************************************************
    //  Method: SendCommand()
    //
    /// <summary>
    /// Sends a command to all command receivers.
    /// </summary>
    ///
    /// <param name="sender">
    /// The object calling this method.
    /// </param>
    ///
    /// <param name="runCommandEventArgs">
    /// The event arguments to include with the command.
    /// </param>
    //*************************************************************************

    public static void
    SendCommand
    (
        Object sender,
        RunCommandEventArgs runCommandEventArgs
    )
    {
        Debug.Assert(sender != null);
        Debug.Assert(runCommandEventArgs != null);

        RunCommandEventHandler oCommandSent = CommandSent;

        if (oCommandSent != null)
        {
            oCommandSent(sender, runCommandEventArgs);
        }
    }

    //*************************************************************************
    //  Method: SendNoParamCommand()
    //
    /// <summary>
    /// Sends a command to all command receivers, where the command does not
    /// require any parameters.
    /// </summary>
    ///
    /// <param name="sender">
    /// The object calling this method.
    /// </param>
    ///
    /// <param name="noParamCommand">
    /// The command that needs to be run, where the command does not require
    /// any parameters.
    /// </param>
    //*************************************************************************

    public static void
    SendNoParamCommand
    (
        Object sender,
        NoParamCommand noParamCommand
    )
    {
        Debug.Assert(sender != null);

        SendCommand( sender, new RunNoParamCommandEventArgs(noParamCommand) );
    }

    //*************************************************************************
    //  Event: CommandSent
    //
    /// <summary>
    /// Occurs when a command must be executed elsewhere in the application.
    /// </summary>
    //*************************************************************************

    public static event RunCommandEventHandler CommandSent;
}

}
