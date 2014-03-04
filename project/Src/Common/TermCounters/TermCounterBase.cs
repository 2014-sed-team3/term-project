
using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace Smrf.AppLib
{
//*****************************************************************************
//  Class: TermCounterBase
//
/// <summary>
/// Base class for classes that count terms in one or more documents.
/// </summary>
///
/// <remarks>
/// Call <see cref="CountTermsInDocument" /> for each document that contains
/// terms that need to be counted.  When done, call <see
/// cref="CalculateSalienceOfCountedTerms" /> to calculate the salience of each
/// term within all the documents, then use <see cref="CountedTerms" /> to get
/// a collection of the terms that were counted in all the documents.
///
/// <para>
/// By default, URLs and punctuation are skipped when terms are counted.  Use
/// the <see cref="SkipUrlsAndPunctuation" /> property to change this behavior.
/// </para>
///
/// </remarks>
//*****************************************************************************

public abstract class TermCounterBase<TCountedTerm> : Object
    where TCountedTerm : CountedTermBase
{
    //*************************************************************************
    //  Constructor: TermCounterBase()
    //
    /// <summary>
    /// Initializes a new instance of the TermCounterBase class.
    /// </summary>
    ///
    /// <param name="convertToLower">
    /// true if terms should all be converted to lower case.
    /// </param>
    ///
    /// <param name="wordsToSkip">
    /// An array of words that should be skipped when counting terms.  Can be
    /// empty but not null.
    /// </param>
    //*************************************************************************

    public TermCounterBase
    (
        Boolean convertToLower,
        String [] wordsToSkip
    )
    {
        Debug.Assert(wordsToSkip != null);

        m_bConvertToLower = convertToLower;
        m_oWordsToSkip = new HashSet<String>();
        m_bSkipUrlsAndPunctuation = true;

        foreach (String sWordToSkip in wordsToSkip)
        {
            m_oWordsToSkip.Add(
                m_bConvertToLower ? sWordToSkip.ToLower() : sWordToSkip);
        }

        // This is a simple, dumb regular expression that doesn't attempt to
        // validate an URL; it just recognizes what seems to be an URL.

        m_oUrlRemover = new Regex(@"(^|\s)https?://\S+");

        m_oCountedTerms = new Dictionary<String, TCountedTerm>();
        m_iTotalDocuments = 0;
        m_iTotalWordsInDocuments = 0;

        AssertValid();
    }

    //*************************************************************************
    //  Property: SkipUrlsAndPunctuation
    //
    /// <summary>
    /// Gets or sets a value specifying whether URLs and punctuation should be
    /// skipped when counting terms.
    /// </summary>
    ///
    /// <value>
    /// true to skip URLs and punctuation, false to count them.  The default
    /// value is true.
    /// </value>
    //*************************************************************************

    public Boolean
    SkipUrlsAndPunctuation
    {
        get
        {
            AssertValid();

            return (m_bSkipUrlsAndPunctuation);
        }

        set
        {
            m_bSkipUrlsAndPunctuation = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: CountedTerms
    //
    /// <summary>
    /// Gets all the terms that were counted.
    /// </summary>
    ///
    /// <value>
    /// A collection of zero or more counted terms, one for each term that was
    /// counted by all calls to <see cref="CountTermsInDocument" />.
    /// </value>
    ///
    /// <remarks>
    /// The counted terms are sorted by descending <see
    /// cref="CountedTermBase.Count" /> values.
    /// </remarks>
    //*************************************************************************

    public IEnumerable<TCountedTerm>
    CountedTerms
    {
        get
        {
            AssertValid();

            return (
                from oCountedTerm in m_oCountedTerms.Values
                orderby oCountedTerm.Count descending
                select oCountedTerm
                );
        }
    }

    //*************************************************************************
    //  Property: TotalDocuments
    //
    /// <summary>
    /// Gets the number of documents for which terms were counted.
    /// </summary>
    ///
    /// <value>
    /// The number of times <see cref="CountTermsInDocument" /> was called.
    /// </value>
    //*************************************************************************

    public Int32
    TotalDocuments
    {
        get
        {
            AssertValid();

            return (m_iTotalDocuments);
        }
    }

    //*************************************************************************
    //  Property: TotalWordsInDocuments
    //
    /// <summary>
    /// Gets the number of words in the documents for which terms were counted.
    /// </summary>
    ///
    /// <value>
    /// The number of words in the documents for which <see
    /// cref="CountTermsInDocument" /> was called, excluding URLs, punctuation,
    /// and the "words to skip" that were specified in the constructor.
    /// </value>
    //*************************************************************************

    public Int32
    TotalWordsInDocuments
    {
        get
        {
            AssertValid();

            return (m_iTotalWordsInDocuments);
        }
    }

    //*************************************************************************
    //  Method: CountTermsInDocument()
    //
    /// <summary>
    /// Counts the terms in a document.
    /// </summary>
    ///
    /// <param name="document">
    /// The document to count terms in.  Can't be null.
    /// </param>
    ///
    /// <remarks>
    /// The terms counted in this call are added to the terms counted in
    /// previous calls to this method.  The cumulative results can be obtained
    /// via the <see cref="CountedTerms" /> property.
    /// </remarks>
    //*************************************************************************

    public void
    CountTermsInDocument
    (
        String document
    )
    {
        Debug.Assert(document != null);
        AssertValid();

        List<String> oWords = RemoveSkippedWords(
            StringUtil.SplitOnCommonDelimiters( CleanDocument(document) ) );

        m_iTotalDocuments++;
        m_iTotalWordsInDocuments += oWords.Count;
        HashSet<String> oKeysCountedInThisCall = new HashSet<String>();

        // Let the derived class do most of the work.

        CountTermsInWords(oWords, m_oCountedTerms, oKeysCountedInThisCall);

        // Increment DocumentsInWhichTermWasCounted for each key that was
        // counted in this call.

        foreach (String sKeyCountedInThisCall in oKeysCountedInThisCall)
        {
            m_oCountedTerms[sKeyCountedInThisCall]
                .DocumentsInWhichTermWasCounted++;
        }
    }

    //*************************************************************************
    //  Method: CalculateSalienceOfCountedTerms()
    //
    /// <summary>
    /// Calculates the salience of each counted term within all the
    /// documents.
    /// </summary>
    ///
    /// <remarks>
    /// This should be called after the final call to <see
    /// cref="CountTermsInDocument" />.  It sets the <see
    /// cref="CountedTermBase.Salience" /> property on each term that was
    /// counted within all the documents.
    /// </remarks>
    //*************************************************************************

    public void
    CalculateSalienceOfCountedTerms()
    {
        AssertValid();

        // See SalienceAlgorithm.xlsx in this folder for an explanation of the
        // following calculations.

        foreach (TCountedTerm oCountedTerm in m_oCountedTerms.Values)
        {
            Int32 iDocumentsInWhichTermWasCounted =
                oCountedTerm.DocumentsInWhichTermWasCounted;

            Double dSalience = 0;

            if (
                m_iTotalWordsInDocuments > 0
                &&
                m_iTotalDocuments > 0
                &&
                iDocumentsInWhichTermWasCounted > 0
                )
            {
                dSalience =
                    ( (Double)oCountedTerm.Count /
                        (Double)m_iTotalWordsInDocuments ) *

                    Math.Log10( (Double)m_iTotalDocuments /
                         (Double)iDocumentsInWhichTermWasCounted )
                    ;
            }

            oCountedTerm.Salience = dSalience;
        }
    }

    //*************************************************************************
    //  Method: Clear()
    //
    /// <summary>
    /// Clears the counted terms.
    /// </summary>
    //*************************************************************************

    public void
    Clear()
    {
        AssertValid();

        m_oCountedTerms.Clear();
        m_iTotalDocuments = 0;
        m_iTotalWordsInDocuments = 0;
    }

    //*************************************************************************
    //  Method: CountTermsInWords()
    //
    /// <summary>
    /// Counts the terms in a list of words that have been extracted from a
    /// document.
    /// </summary>
    ///
    /// <param name="oWords">
    /// The collection of words to count the terms in.  Excludes URLs,
    /// punctuation, and the "words to skip" that were specified in the
    /// constructor.  The words are all in lower case.
    /// </param>
    ///
    /// <param name="oCountedTerms">
    /// The dictionary of terms that have been counted so far. The key is
    /// determined by the derived class and the value is the counted term.
    /// </param>
    ///
    /// <param name="oKeysCountedInThisCall">
    /// The keys in <paramref name="oCountedTerms" /> for which terms were
    /// counted.
    /// </param>
    ///
    /// <remarks>
    /// For each term found in <paramref name="oWords" />, this method
    /// increments the term's count in <paramref name="oCountedTerms" /> if the
    /// term was previously counted, or adds a key/value pair to <paramref
    /// name="oCountedTerms" /> with a count of 1 if the term was not
    /// previously counted.  It also adds the key to <paramref
    /// name="oKeysCountedInThisCall" />.
    /// </remarks>
    //*************************************************************************

    protected abstract void
    CountTermsInWords
    (
        IList<String> oWords,
        Dictionary<String, TCountedTerm> oCountedTerms,
        HashSet<String> oKeysCountedInThisCall
    );

    //*************************************************************************
    //  Method: CleanDocument()
    //
    /// <summary>
    /// Removes unwanted characters from the document whose terms need to be
    /// counted.
    /// </summary>
    ///
    /// <param name="sDocument">
    /// The document that needs to be cleaned.
    /// </param>
    ///
    /// <returns>
    /// The cleaned document.
    /// </returns>
    //*************************************************************************

    protected String
    CleanDocument
    (
        String sDocument
    )
    {
        Debug.Assert(sDocument != null);
        AssertValid();

        if (!this.SkipUrlsAndPunctuation)
        {
            return (sDocument);
        }

        sDocument = m_oUrlRemover.Replace(sDocument, " ");

        StringBuilder oStringBuilder = new StringBuilder();

        // Replace all punctuation except the single quote and underscore with
        // spaces.  It's more straightforward to do this with a StringBuilder
        // than with a regular expression.

        foreach (Char c in sDocument)
        {
            oStringBuilder.Append(
                ( c == '\'' || c == '_' || Char.IsLetterOrDigit(c) ) ?
                c : ' '
                );
        }

        return ( oStringBuilder.ToString() );
    }

    //*************************************************************************
    //  Method: RemoveSkippedWords()
    //
    /// <summary>
    /// Removes words that should be skipped from a collection of words.
    /// </summary>
    ///
    /// <param name="oWords">
    /// The words to remove the skipped words from.
    /// </param>
    ///
    /// <returns>
    /// A collection of the words in <paramref name="oWords" />, excluding the
    /// words to skip.  The words in the returned collection are in lower case.
    /// </returns>
    //*************************************************************************

    protected List<String>
    RemoveSkippedWords
    (
        IEnumerable<String> oWords
    )
    {
        Debug.Assert(oWords != null);
        AssertValid();

        List<String> oFilteredWords = new List<String>();

        foreach (String sWord in oWords)
        {
            String sWordToTest = m_bConvertToLower ? sWord.ToLower() : sWord;

            if ( !m_oWordsToSkip.Contains(sWordToTest) )
            {
                oFilteredWords.Add(sWordToTest);
            }
        }

        return (oFilteredWords);
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
        // m_bConvertToLower
        Debug.Assert(m_oWordsToSkip != null);
        // m_bSkipUrlsAndPunctuation
        Debug.Assert(m_oUrlRemover != null);
        Debug.Assert(m_oCountedTerms != null);
        Debug.Assert(m_iTotalDocuments >= 0);
        Debug.Assert(m_iTotalWordsInDocuments >= 0);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// true if terms should all be converted to lower case.

    protected Boolean m_bConvertToLower;

    /// The words that should be skipped when counting terms.

    protected HashSet<String> m_oWordsToSkip;

    /// true to skip URLs and punctuation, false to count them.  The default
    /// value is true.

    protected Boolean m_bSkipUrlsAndPunctuation;

    /// Removes URLs from the text.

    protected Regex m_oUrlRemover;

    /// The key is created from the term by the derived class and the value is
    /// the counted term.

    protected Dictionary<String, TCountedTerm> m_oCountedTerms;

    /// The number of times CountTermsInDocuments() was called.

    protected Int32 m_iTotalDocuments;

    /// The number of words in the documents for which terms were counted.

    protected Int32 m_iTotalWordsInDocuments;
}

}
