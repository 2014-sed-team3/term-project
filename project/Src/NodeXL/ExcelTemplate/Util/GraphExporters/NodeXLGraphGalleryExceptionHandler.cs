

using System;
using System.ServiceModel;
using System.Diagnostics;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: NodeXLGraphGalleryExceptionHandler
//
/// <summary>
/// Handles exceptions caught when a graph is exported to the NodeXL Graph
/// Gallery.
/// </summary>
//*****************************************************************************

public static class NodeXLGraphGalleryExceptionHandler : Object
{
    //*************************************************************************
    //  Method: TryGetMessageForRecognizedException()
    //
    /// <summary>
    /// Provides a friendly error message if the exception is one that is
    /// recognized.
    /// </summary>
    ///
    /// <param name="exception">
    /// The exception that was caught while attempting to export a graph to the
    /// NodeXL Graph Gallery.
    /// </param>
    ///
    /// <param name="message">
    /// Where a friendly message gets stored if true is returned.
    /// </param>
    ///
    /// <returns>
    /// true if the exception was recognized.
    /// </returns>
    //*************************************************************************

    public static Boolean 
    TryGetMessageForRecognizedException
    (
        Exception exception,
        out String message
    )
    {
        Debug.Assert(exception != null);

        const String TryAgainMessage = "Try again later.";

        if (exception is GraphTooLargeException)
        {
            message =
                "The graph is too large to export.  Uncheck the"
                + " \"Also export the workbook and its options\" or"
                + " \"Also export the graph data as GraphML\" checkboxes and"
                + " try again."
                ;
        }
        else if ( exception is FaultException<String> )
        {
            // The GraphDataSource.AddGraph() method throws a
            // FaultException<String> with a friendly error message when the
            // graph can't be added for a known reason, such as an invalid
            // author.

            message = ( ( FaultException<String> )exception ).Detail;
        }
        else if (exception is FaultException)
        {
            // This is an unexpected error.

            message =
                "A problem occurred within the NodeXL Graph Gallery.  "
                + TryAgainMessage;
        }
        else if (
            exception is TimeoutException
            ||
            exception is CommunicationException
            )
        {
            message =
                "The NodeXL Graph Gallery couldn't be reached.  "
                + TryAgainMessage;
        }
        else
        {
            message = null;

            return (false);
        }

        return (true);
    }
}
}
