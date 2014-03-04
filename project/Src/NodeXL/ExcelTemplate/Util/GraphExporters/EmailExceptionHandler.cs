

using System;
using System.Net.Mail;
using System.Diagnostics;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: EmailExceptionHandler
//
/// <summary>
/// Handles exceptions caught when a graph is exported to email.
/// </summary>
//*****************************************************************************

public static class EmailExceptionHandler : Object
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
    /// The exception that was caught while attempting to export a graph to
    /// email.
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

        if (exception is EmailAddressFormatException)
        {
            const String FormatMessage =
                "  An email address should look like \"john@yahoo.com\".";

            switch ( ( (EmailAddressFormatException)exception )
                .EmailAddressType )
            {
                case EmailAddressType.To:

                    message =
                        "One of the \"To\" addresses is not in a recognized"
                        + " format." + FormatMessage
                        ;

                    break;

                case EmailAddressType.From:

                    message =
                        "The \"from\" address is not in a recognized format."
                        + FormatMessage
                        ;

                    break;

                default:

                    Debug.Assert(false);
                    throw exception;
            }
        }
        else if (exception is SmtpException)
        {
            message =
                "A problem occurred while sending the email.  Details:"
                + "\r\n\r\n"
                + exception.Message
                ;
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
