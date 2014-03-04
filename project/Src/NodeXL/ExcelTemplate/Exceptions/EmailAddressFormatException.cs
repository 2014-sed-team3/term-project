
using System;
using System.Runtime.Serialization;
using System.Diagnostics;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: EmailAddressFormatException
//
/// <summary>
/// Represents an exception thrown by the <see cref="EmailExporter" /> class
/// when an email address has an invalid format.
/// </summary>
//*****************************************************************************

[System.SerializableAttribute()]

public class EmailAddressFormatException : Exception
{
    //*************************************************************************
    //  Constructor: EmailAddressFormatException()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="EmailAddressFormatException" /> class.
    /// </summary>
    ///
    /// <param name="emailAddressType">
    /// The type of the address that has an invalid format.
    /// </param>
    //*************************************************************************

    public EmailAddressFormatException
    (
        EmailAddressType emailAddressType
    )
    {
        m_eEmailAddressType = emailAddressType;

        AssertValid();
    }

    //*************************************************************************
    //  Property: EmailAddressType
    //
    /// <summary>
    /// Gets the type of the address that has an invalid format.
    /// </summary>
    ///
    /// <value>
    /// The email address type, as a <see
    /// cref="ExcelTemplate.EmailAddressType" />.
    /// </value>
    //*************************************************************************

    public EmailAddressType
    EmailAddressType
    {
        get
        {
            return (m_eEmailAddressType);
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
        // m_eEmailAddressType
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// The type of the address that has an invalid format.

    protected EmailAddressType m_eEmailAddressType;
}


//*****************************************************************************
//  Enum: EmailAddressType
//
/// <summary>
/// Specifies the type of an email address.
/// </summary>
//*****************************************************************************

public enum
EmailAddressType
{
    /// <summary>
    /// A "from" email address.
    /// </summary>

    From,

    /// <summary>
    /// A "to" email address.
    /// </summary>

    To,
}

}
