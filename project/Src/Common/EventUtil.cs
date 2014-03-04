
using System;
using System.Diagnostics;

namespace Smrf.AppLib
{
//*****************************************************************************
//  Class: EventUtil
//
/// <summary>
/// Utility methods for dealing with events.
/// </summary>
///
/// <remarks>
/// All methods are static.
/// </remarks>
//*****************************************************************************

public static class EventUtil
{
    //*************************************************************************
    //  Method: FireEvent()
    //
    /// <overloads>
    /// Fires an event if appropriate.
    /// </overloads>
    ///
    /// <summary>
    /// Fires an event with an <see cref="EventHandler" /> signature if
    /// appropriate.
    /// </summary>
    ///
    /// <param name="eventHandler">
    /// Event handler, or null if no clients have subscribed to the event.
    /// </param>
    ///
    /// <param name="sender">
    /// Sender of the event.
    /// </param>
    ///
    /// <remarks>
    /// If <paramref name="eventHandler" /> is not null, this method fires the
    /// event represented by <paramref name="eventHandler" />.  Otherwise, it
    /// does nothing.
    /// </remarks>
    //*************************************************************************

    public static void
    FireEvent
    (
        Object sender,
        EventHandler eventHandler
    )
    {
        Debug.Assert(sender != null);

        if (eventHandler != null)
        {
            eventHandler(sender, EventArgs.Empty);
        }
    }

    //*************************************************************************
    //  Method: FireEvent()
    //
    /// <summary>
    /// Fires an event with a specified signature if appropriate.
    /// </summary>
    ///
    /// <typeparam name="TEventArgs">
    /// The type of the event arguments.
    /// </typeparam>
    ///
    /// <param name="sender">
    /// Sender of the event.
    /// </param>
    ///
    /// <param name="e">
    /// The event arguments.
    /// </param>
    ///
    /// <param name="eventHandler">
    /// Event handler, or null if no clients have subscribed to the event.
    /// </param>
    ///
    /// <remarks>
    /// If <paramref name="eventHandler" /> is not null, this method fires the
    /// event represented by <paramref name="eventHandler" />.  Otherwise, it
    /// does nothing.
    /// </remarks>
    //*************************************************************************

    public static void
    FireEvent<TEventArgs>
    (
        Object sender,
        TEventArgs e,
        EventHandler<TEventArgs> eventHandler
    )
    where TEventArgs : EventArgs
    {
        Debug.Assert(sender != null);
        Debug.Assert(e != null);

        if (eventHandler != null)
        {
            eventHandler(sender, e);
        }
    }
}

}
