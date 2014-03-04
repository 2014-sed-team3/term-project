
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Smrf.AppLib
{
//*****************************************************************************
//  Class: WordCounter
//
/// <summary>
/// Counts words in one or more documents.
/// </summary>
///
/// <remarks>
/// Call <see cref="TermCounterBase{TCountedTerm}.CountTermsInDocument" /> for
/// each document that contains words that need to be counted.  When done, call
/// <see
/// cref="TermCounterBase{TCountedTerm}.CalculateSalienceOfCountedTerms" /> to
/// calculate the salience of each word within all the documents, then use <see
/// cref="TermCounterBase{TCountedTerm}.CountedTerms" /> to get a collection of
/// the words that were counted in all the documents.
///
/// <para>
/// If you use the single-argument constructor, then when words are counted,
/// the words "B" and "b" are considered the same word, and the <see
/// cref="CountedWord.Word" /> string is always in lower case.
/// </para>
///
/// </remarks>
//*****************************************************************************

public class WordCounter : TermCounterBase<CountedWord>
{
    //*************************************************************************
    //  Constructor: WordCounter()
    //
    /// <overloads>
    /// Initializes a new instance of the <see cref="WordCounter" /> class.
    /// </overloads>
    ///
    /// <summary>
    /// Initializes a new instance of the <see cref="WordCounter" /> class that
    /// converts all terms to lower case.
    /// </summary>
    ///
    /// <param name="wordsToSkip">
    /// An array of words that should be skipped when counting words.  Can be
    /// empty but not null.
    /// </param>
    //*************************************************************************

    public WordCounter
    (
        String [] wordsToSkip
    )
    : base(true, wordsToSkip)
    {
        // (Do nothing else.)

        AssertValid();
    }

    //*************************************************************************
    //  Constructor: WordCounter()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="WordCounter" /> class.
    /// </summary>
    ///
    /// <param name="convertToLowerCase">
    /// true if terms should all be converted to lower case.
    /// </param>
    ///
    /// <param name="wordsToSkip">
    /// An array of words that should be skipped when counting words.  Can be
    /// empty but not null.
    /// </param>
    //*************************************************************************

    public WordCounter
    (
        Boolean convertToLowerCase,
        String [] wordsToSkip
    )
    : base(convertToLowerCase, wordsToSkip)
    {
        // (Do nothing else.)

        AssertValid();
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
        Dictionary<String, CountedWord> oCountedTerms,
        HashSet<String> oKeysCountedInThisCall
    )
    {
        Debug.Assert(oWords != null);
        Debug.Assert(oCountedTerms != null);
        Debug.Assert(oKeysCountedInThisCall != null);
        AssertValid();

        CountedWord oCountedWord;

        foreach (String sWord in oWords)
        {
            if ( !oCountedTerms.TryGetValue(sWord, out oCountedWord) )
            {
                oCountedWord = new CountedWord(sWord);
                oCountedTerms.Add(sWord, oCountedWord);
            }

            oCountedWord.Count++;
            oKeysCountedInThisCall.Add(sWord);
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
    }


    //*************************************************************************
    //  Public constants
    //*************************************************************************

    /// <summary>
    /// Space-delimited list of sample English stop words.
    /// </summary>

    // (Thanks to Scott at http://www.textfixer.com/ for this list.)

    public const String SampleSpaceDelimitedEnglishStopWords =

        "a able about across after ain't all almost also am among an and any are aren't as at be because been but by can can't cannot could could've couldn't did didn't do does doesn't don't either else ever every for from get got had has hasn't have he he'd he'll he's her hers him his how how'd how'll how's however i i'd i'll i'm i've if in into is isn't it it's its just least let like likely may me might might've most must must've mustn't my neither no nor not of off often on only or other our own rather said say says she she'd she'll she's should should've shouldn't since so some than that that'll that's the their them then there there's these they they'd they'll they're they've this to too us wants was wasn't we we'd we'll we're were weren't what what's when where where'd where'll where's which while who who'd who'll who's whom why why'd will with won't would would've wouldn't yet you you'd you'll you're you've your"
        ;


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}

}
