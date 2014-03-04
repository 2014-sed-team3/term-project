
using System;
using System.Runtime.Serialization;
using System.Diagnostics;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: ConvertGraphMLToNodeXLWorkbookException
//
/// <summary>
/// Represents an exception thrown by the <see
/// cref="GraphMLToNodeXLWorkbookConverter" /> class
/// </summary>
//*****************************************************************************

[System.SerializableAttribute()]

public class ConvertGraphMLToNodeXLWorkbookException : Exception
{
    //*************************************************************************
    //  Constructor: ConvertGraphMLToNodeXLWorkbookException()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="ConvertGraphMLToNodeXLWorkbookException" /> class.
    /// </summary>
    ///
    /// <param name="errorCode">
    /// The error code.
    /// </param>
    ///
    /// <param name="message">
    /// Error message, suitable for displaying to the user.
    /// </param>
    //*************************************************************************

    public ConvertGraphMLToNodeXLWorkbookException
    (
        GraphMLToNodeXLWorkbookConverter.ErrorCode errorCode,
        String message
    )
    : base(message)
    {
        m_eErrorCode = errorCode;

        AssertValid();
    }

    //*************************************************************************
    //  Property: ErrorCode
    //
    /// <summary>
    /// Gets the error code.
    /// </summary>
    ///
    /// <value>
    /// The error code, as a <see
    /// cref="GraphMLToNodeXLWorkbookConverter.ErrorCode" />.
    /// </value>
    //*************************************************************************

    public GraphMLToNodeXLWorkbookConverter.ErrorCode
    ErrorCode
    {
        get
        {
            return (m_eErrorCode);
        }
    }


    //*************************************************************************
    //  Method: AssertValid()
    //
    /// <summary>
    /// Asserts if the object is in an invalid state.  Debug-only.
    /// </summary>
    //*************************************************************************

    [Conditional("DEBUG")]

    public void
    AssertValid()
    {
        // m_eErrorCode
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// The error code.

    protected GraphMLToNodeXLWorkbookConverter.ErrorCode m_eErrorCode;
}
}
