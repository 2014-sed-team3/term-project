
using System;
using System.Diagnostics;

namespace Smrf.AppLib
{
//*****************************************************************************
//  Class: CountedWord
//
/// <summary>
/// Stores a word that has been counted.
/// </summary>
//*****************************************************************************

public class CountedWord : CountedTermBase
{
    //*************************************************************************
    //  Constructor: CountedWord()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="CountedWord" /> class.
    /// </summary>
    ///
    /// <param name="word">
    /// The word.  Can't be null or empty.
    /// </param>
    //*************************************************************************

    public CountedWord
    (
        String word
    )
    {
        m_sWord = word;

        AssertValid();
    }

    //*************************************************************************
    //  Property: Word
    //
    /// <summary>
    /// Gets the word.
    /// </summary>
    ///
    /// <value>
    /// The word, as a String.  Never null or empty.
    /// </value>
    //*************************************************************************

    public String
    Word
    {
        get
        {
            AssertValid();

            return (m_sWord);
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

    public new void
    AssertValid()
    {
        base.AssertValid();

        Debug.Assert( !String.IsNullOrEmpty(m_sWord) );
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// The word.

    protected String m_sWord;
}

}
