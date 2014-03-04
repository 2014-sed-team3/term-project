
using System;
using System.Diagnostics;

namespace Smrf.AppLib
{
//*****************************************************************************
//  Class: CountedWordPair
//
/// <summary>
/// Stores a pair of words that have been counted.
/// </summary>
//*****************************************************************************

public class CountedWordPair : CountedTermBase
{
    //*************************************************************************
    //  Constructor: CountedWordPair()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="CountedWordPair" /> class.
    /// </summary>
    ///
    /// <param name="word1">
    /// The pair's first word.  Can't be null or empty.
    /// </param>
    ///
    /// <param name="word2">
    /// The pair's second word.  Can't be null or empty.
    /// </param>
    //*************************************************************************

    public CountedWordPair
    (
        String word1,
        String word2
    )
    {
        m_sWord1 = word1;
        m_sWord2 = word2;
        m_dMutualInformation = 0;

        AssertValid();
    }

    //*************************************************************************
    //  Property: Word1
    //
    /// <summary>
    /// Gets the pair's first word.
    /// </summary>
    ///
    /// <value>
    /// The pair's first word, as a String.  Never null or empty.
    /// </value>
    //*************************************************************************

    public String
    Word1
    {
        get
        {
            AssertValid();

            return (m_sWord1);
        }
    }

    //*************************************************************************
    //  Property: Word2
    //
    /// <summary>
    /// Gets the pair's second word.
    /// </summary>
    ///
    /// <value>
    /// The pair's second word, as a String.  Never null or empty.
    /// </value>
    //*************************************************************************

    public String
    Word2
    {
        get
        {
            AssertValid();

            return (m_sWord2);
        }
    }

    //*************************************************************************
    //  Property: MutualInformation
    //
    /// <summary>
    /// Gets or sets the word pair's mutual information score.
    /// </summary>
    ///
    /// <value>
    /// The term's mutual information score within the documents.  The default
    /// is zero.
    /// </value>
    ///
    /// <remarks>
    /// You must call <see
    /// cref="WordPairCounter.CalculateMutualInformationOfCountedTerms" />
    /// before reading this property.
    /// </remarks>
    //*************************************************************************

    public Double
    MutualInformation
    {
        get
        {
            AssertValid();

            return (m_dMutualInformation);
        }

        set
        {
            m_dMutualInformation = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Method: ToString()
    //
    /// <summary>
    /// Formats the value of the current instance.
    /// </summary>
    ///
    /// <returns>
    /// The formatted string.  Sample: "President Obama".
    /// </returns>
    //*************************************************************************

    public override String
    ToString()
    {
        AssertValid();

        return (m_sWord1 + " " + m_sWord2);
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

        Debug.Assert( !String.IsNullOrEmpty(m_sWord1) );
        Debug.Assert( !String.IsNullOrEmpty(m_sWord2) );
        // m_dMutualInformation
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// The first word.

    protected String m_sWord1;

    /// The second word.

    protected String m_sWord2;

    /// The word pair's mutual information score within the documents.

    protected Double m_dMutualInformation;
}

}
