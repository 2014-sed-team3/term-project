
using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace Smrf.AppLib
{
//*****************************************************************************
//  Class: WordPairCounter
//
/// <summary>
/// Counts pairs of words in one or more documents.
/// </summary>
///
/// <remarks>
/// Call <see cref="TermCounterBase{TCountedTerm}.CountTermsInDocument" /> for
/// each document that contains word pairs that need to be counted.  When done,
/// call <see
/// cref="TermCounterBase{TCountedTerm}.CalculateSalienceOfCountedTerms" /> and
/// <see cref="WordPairCounter.CalculateMutualInformationOfCountedTerms" /> to
/// calculate the salience and mutual information of each word pair within all
/// the documents, then use <see
/// cref="TermCounterBase{TCountedTerm}.CountedTerms" /> to get a collection of
/// the word pairs that were counted in all the documents.
///
/// <para>
/// When word pairs are counted, the pairs "A B", "A b", and "a b" are all
/// considered the same pair.  If you call <see
/// cref="TermCounterBase{TCountedTerm}.CountTermsInDocument" /> for each of
/// these strings, then <see
/// cref="TermCounterBase{TCountedTerm}.CountedTerms" /> will return a
/// collection containing a single <see cref="CountedWordPair" /> object, with
/// its <see cref="CountedTermBase.Count" /> property set to 3.
/// </para>
///
/// <para>
/// The <see cref="CountedWordPair.Word1" /> and <see
/// cref="CountedWordPair.Word2" /> strings are always in lower case.  The
/// order of the two words is significant, so that "a b" and "b a" are counted
/// as two separate word pairs.
/// </para>
///
/// </remarks>
//*****************************************************************************

public class WordPairCounter : TermCounterBase<CountedWordPair>
{
    //*************************************************************************
    //  Constructor: WordPairCounter()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="WordPairCounter" /> class.
    /// </summary>
    ///
    /// <param name="wordsToSkip">
    /// An array of words that should be skipped when counting words.  Can be
    /// empty but not null.  The case of the words is irrelevant.
    /// </param>
    //*************************************************************************

    public WordPairCounter
    (
        String [] wordsToSkip
    )
    : base(true, wordsToSkip)
    {
        // (Do nothing else.)

        AssertValid();
    }

    //*************************************************************************
    //  Method: CalculateMutualInformationOfCountedTerms()
    //
    /// <summary>
    /// Calculates the mutual information of each counted term within all the
    /// documents.
    /// </summary>
    ///
    /// <remarks>
    /// This should be called after the final call to <see
    /// cref="TermCounterBase{TCountedTerm}.CountTermsInDocument" />.  It sets
    /// the <see cref="CountedWordPair.MutualInformation" /> property on each
    /// term that was counted within all the documents.
    /// </remarks>
    //*************************************************************************

    public void
    CalculateMutualInformationOfCountedTerms()
    {
        AssertValid();

        // See MutualInformationAlgorithm.xlsx in this folder for an
        // explanation of the following calculations.

        IEnumerable<CountedWordPair> oCountedWordPairs =
            m_oCountedTerms.Values;

        Double dNWordPairs = oCountedWordPairs.Sum(
            oCountedWordPair => oCountedWordPair.Count);

        foreach (CountedWordPair oCountedTerm in oCountedWordPairs)
        {
            Double dNXY = oCountedTerm.Count;
            Double dPXY = dNXY / dNWordPairs;

            Double dNXStar = oCountedWordPairs.Sum( oCountedWordPair =>
                (oCountedWordPair.Word1 == oCountedTerm.Word1) ?
                oCountedWordPair.Count : 0);

            Debug.Assert(dNWordPairs != 0);

            Double dPXStar = dNXStar / dNWordPairs;

            Double dNStarY = oCountedWordPairs.Sum( oCountedWordPair =>
                (oCountedWordPair.Word2 == oCountedTerm.Word2) ?
                oCountedWordPair.Count : 0);

            Double dPStarY = dNStarY / dNWordPairs;

            Double dMutualInformation;

            if (dPXStar == 0 || dPStarY == 0)
            {
                dMutualInformation = 0;
            }
            else
            {
                dMutualInformation = Math.Log10( dPXY / (dPXStar * dPStarY) );
            }

            oCountedTerm.MutualInformation = dMutualInformation;
        }
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

    protected override void
    CountTermsInWords
    (
        IList<String> oWords,
        Dictionary<String, CountedWordPair> oCountedTerms,
        HashSet<String> oKeysCountedInThisCall
    )
    {
        Debug.Assert(oWords != null);
        Debug.Assert(oCountedTerms != null);
        Debug.Assert(oKeysCountedInThisCall != null);
        AssertValid();

        Int32 iWords = oWords.Count;
        CountedWordPair oCountedWordPair;

        for (Int32 i = 0; i < iWords - 1; i++)
        {
            String sWord1 = oWords[i];
            String sWord2 = oWords[i + 1];
            String sKey = GetKey(sWord1, sWord2);

            if ( !oCountedTerms.TryGetValue(sKey, out oCountedWordPair) )
            {
                oCountedWordPair = new CountedWordPair(sWord1, sWord2);
                oCountedTerms.Add(sKey, oCountedWordPair);
            }

            oCountedWordPair.Count++;
            oKeysCountedInThisCall.Add(sKey);
        }
    }

    //*************************************************************************
    //  Method: GetKey()
    //
    /// <summary>
    /// Gets a key to use for the word pair dictionary.
    /// </summary>
    ///
    /// <param name="sWord1">
    /// The first word in the WordPair that will be stored in the dictionary.
    /// Can't be null or empty and must be in lower case.
    /// </param>
    ///
    /// <param name="sWord2">
    /// The second word in the WordPair that will be stored in the dictionary.
    /// Can't be null or empty and must be in lower case.
    /// </param>
    ///
    /// <returns>
    /// A key to use for the dictionary.
    /// </returns>
    //*************************************************************************

    protected String
    GetKey
    (
        String sWord1,
        String sWord2
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sWord1) );
        Debug.Assert( !String.IsNullOrEmpty(sWord2) );
        AssertValid();

        return (sWord1 + ' ' + sWord2);
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
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}

}
