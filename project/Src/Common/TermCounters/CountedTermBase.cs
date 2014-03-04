
using System;
using System.Diagnostics;

namespace Smrf.AppLib
{
//*****************************************************************************
//  Class: CountedTermBase
//
/// <summary>
/// Base class for classes that store a term that has been counted.
/// </summary>
///
/// <remarks>
/// A "term" can be a word, for example, or a word pair.  The meaning of "term"
/// is determined by the derived class.
/// </remarks>
//*****************************************************************************

public class CountedTermBase : Object
{
    //*************************************************************************
    //  Constructor: CountedTermBase()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="CountedTermBase" /> class.
    /// </summary>
    //*************************************************************************

    public CountedTermBase()
    {
        m_iCount = 0;
        m_iDocumentsInWhichTermWasCounted = 0;
        m_dSalience = 0;

        AssertValid();
    }

    //*************************************************************************
    //  Property: Count
    //
    /// <summary>
    /// Gets or sets the number of times the term was counted in all documents.
    /// </summary>
    ///
    /// <value>
    /// The number of times the term was counted, as an Int32.  The default is
    /// zero.
    /// </value>
    //*************************************************************************

    public Int32
    Count
    {
        get
        {
            AssertValid();

            return (m_iCount);
        }

        set
        {
            m_iCount = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: DocumentsInWhichTermWasCounted
    //
    /// <summary>
    /// Gets or sets the number of documents in which the term was counted.
    /// </summary>
    ///
    /// <value>
    /// The number of documents in which the term was counted, as an Int32.
    /// The default is zero.
    /// </value>
    ///
    /// <remarks>
    /// If the term occurred once in document A and twice in document B, then
    /// <see cref="Count" /> will be 3 and <see
    /// cref="DocumentsInWhichTermWasCounted" /> will be 2.
    /// </remarks>
    //*************************************************************************

    public Int32
    DocumentsInWhichTermWasCounted
    {
        get
        {
            AssertValid();

            return (m_iDocumentsInWhichTermWasCounted);
        }

        set
        {
            m_iDocumentsInWhichTermWasCounted = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: Salience
    //
    /// <summary>
    /// Gets or sets the term's salience.
    /// </summary>
    ///
    /// <value>
    /// The term's salience within the documents.  The default is zero.
    /// </value>
    //*************************************************************************

    public Double
    Salience
    {
        get
        {
            AssertValid();

            return (m_dSalience);
        }

        set
        {
            m_dSalience = value;

            AssertValid();
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
        Debug.Assert(m_iCount >= 0);
        Debug.Assert(m_iDocumentsInWhichTermWasCounted >= 0);
        // m_dSalience
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// The number of times the term was counted in all documents.

    protected Int32 m_iCount;

    /// The number of documents in which the term was counted.

    protected Int32 m_iDocumentsInWhichTermWasCounted;

    /// The term's salience within the documents.

    protected Double m_dSalience;
}

}
