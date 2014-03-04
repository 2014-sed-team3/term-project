
using System;
using System.Diagnostics;

namespace Smrf.NodeXL.GraphMLFileProcessor
{
//*****************************************************************************
//  Class: ProcessingUtil
//
/// <summary>
/// Utility methods for processing GraphML files.
/// </summary>
//*****************************************************************************

public static class ProcessingUtil
{
    //*************************************************************************
    //  Method: AppendTimestamp()
    //
    /// <summary>
    /// Appends a timestamp to a message.
    /// </summary>
    ///
    /// <param name="message">
    /// Message to append a timestamp to.
    /// </param>
    ///
    /// <returns>
    /// The message with a timestamp appended to it.
    /// </returns>
    //*************************************************************************

    public static String
    AppendTimestamp
    (
        String message
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(message) );

        DateTime now = DateTime.Now;

        return ( String.Format(

            "{0}  [{1}, {2}]"
            ,
            message,
            now.ToShortDateString(),
            now.ToLongTimeString()
            ) );
    }
}

}
