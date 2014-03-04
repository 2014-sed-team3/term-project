
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace Smrf.AppLib
{
//*****************************************************************************
//  Class: SentenceConcatenator
//
/// <summary>
/// String collection designed to concatenate multiple sentences.
/// </summary>
///
/// <remarks>
/// Call <see cref="AddSentence" /> to add a sentence to the collection.  Call
/// <see cref="ConcatenateSentences" /> to get a concatenation of all the added
/// sentences.
/// </remarks>
//*****************************************************************************

public class SentenceConcatenator : List<String>
{
    //*************************************************************************
    //  Constructor: SentenceConcatenator()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="SentenceConcatenator" />
    /// class.
    /// </summary>
    //*************************************************************************

    public SentenceConcatenator()
    {
        // (Do nothing.)

        AssertValid();
    }

    //*************************************************************************
    //  Method: AddSentence()
    //
    /// <summary>
    /// Formats a sentence and adds it to the collection.
    /// </summary>
    ///
    /// <param name="format">
    /// A composite format string.
    /// </param>
    ///
    /// <param name="args">
    /// An Object array containing zero or more objects to format. 
    /// </param>
    //*************************************************************************

    public void
    AddSentence
    (
        String format,
        params Object [] args
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(format) );
        Debug.Assert(args != null);
        AssertValid();

        this.Add( String.Format(format, args) );
    }

    //*************************************************************************
    //  Method: ConcatenateSentences()
    //
    /// <summary>
    /// Concatenates the sentences in the collection.
    /// </summary>
    ///
    /// <returns>
    /// A concatenation of the sentences in the collection.  The sentences are
    /// separated by spaces.
    /// </returns>
    //*************************************************************************

    public String
    ConcatenateSentences()
    {
        AssertValid();

        StringBuilder oStringBuilder = new StringBuilder();

        foreach (String sSentence in this)
        {
            if (oStringBuilder.Length > 0)
            {
                oStringBuilder.Append("  ");
            }

            oStringBuilder.Append(sSentence);
        }

        return ( oStringBuilder.ToString() );
    }


    //*************************************************************************
    //  Method: AssertValid()
    //
    /// <summary>
    /// Asserts if the object is in an invalid state.  Debug-only.
    /// </summary>
    //*************************************************************************

    [Conditional("DEBUG")]

    public virtual void
    AssertValid()
    {
        // (Do nothing.)
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}

}
