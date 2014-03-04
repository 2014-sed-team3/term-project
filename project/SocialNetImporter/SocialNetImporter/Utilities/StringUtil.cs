
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Text;
using System.Diagnostics;

namespace Smrf.AppLib
{
//*****************************************************************************
//  Class: StringUtil
//
/// <summary>
/// String utility methods.
/// </summary>
///
/// <remarks>
/// This class contains utility methods for dealing with String objects.  All
/// methods are static.
/// </remarks>
//*****************************************************************************

public static class StringUtil
{
    //*************************************************************************
    //  Method: MakePlural()
    //
    /// <summary>
    /// Adds an "s" to a noun if the noun should be plural.
    /// </summary>
    ///
    /// <param name="sNoun">
    /// Noun to make plural if necessary.  Sample: "orange".
    /// </param>
    ///
    /// <param name="iCount">
    /// Number of things that <paramref name="sNoun" /> describes.  Sample: 2.
    /// </param>
    ///
    /// <returns>
    /// <paramref name="sNoun" /> with "s" appended to it if necessary.
    /// Sample: "oranges".
    /// </returns>
    //*************************************************************************

    public static String
    MakePlural
    (
        String sNoun,
        Int32 iCount
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sNoun) );
        Debug.Assert(iCount >= 0);

        return ( (iCount == 1) ? sNoun : sNoun + "s" );
    }

    //*************************************************************************
    //  Method: BreakIntoLines()
    //
    /// <summary>
    /// Breaks a string into multiple lines if possible.
    /// </summary>
    ///
    /// <param name="stringToBreak">
    /// The string to break into multiple lines.  Can't be null.
    /// </param>
    ///
    /// <param name="lineLength">
    /// The ideal maximum line length.  Must be greater than 0.
    /// </param>
    ///
    /// <returns>
    /// <paramref name="stringToBreak" /> broken into multiple lines, if
    /// possible.  Each sequence of whitespace is replaced with a single space,
    /// and the multiple lines are separated by "\n".
    /// </returns>
    ///
    /// <remarks>
    /// This method attempts to break <paramref name="stringToBreak" /> into
    /// multiple lines so that each line is no longer than <paramref
    /// name="lineLength" /> characters.  The breaks are made only at
    /// whitespaces, so the ideal line length is NOT guaranteed.
    /// </remarks>
    //*************************************************************************

    public static String
    BreakIntoLines
    (
        String stringToBreak,
        Int32 lineLength
    )
    {
        Debug.Assert(stringToBreak != null);
        Debug.Assert(lineLength > 0);

        String [] asWords = stringToBreak.Split(
            new char[] {' ', '\r', '\n', '\t'},
            StringSplitOptions.RemoveEmptyEntries);

        Int32 iWords = asWords.Length;
        StringBuilder oStringBuilder = new StringBuilder();
        Int32 iLengthOfThisLine = 0;

        for (Int32 i = 0; i < iWords; i++)
        {
            String sWord = asWords[i];
            oStringBuilder.Append(sWord);
            iLengthOfThisLine += sWord.Length;

            if (i != iWords - 1)
            {
                if (iLengthOfThisLine >= lineLength)
                {
                    oStringBuilder.Append("\n");
                    iLengthOfThisLine = 0;
                }
                else
                {
                    oStringBuilder.Append(' ');
                    iLengthOfThisLine += 1;
                }
            }
        }

        return ( oStringBuilder.ToString() );
    }

    //*************************************************************************
    //  Method: ReplaceControlCharacters()
    //
    /// <summary>
    /// Replaces Unicode control characters with a specified character.
    /// </summary>
    ///
    /// <param name="sString">
    /// String that may include control characters.  Can't be null.
    /// </param>
    ///
    /// <param name="cReplacementCharacter">
    /// Character to replace them with.
    /// </param>
    ///
    /// <returns>
    /// <paramref name="sString" /> with control characters replaced with
    /// <paramref name="cReplacementCharacter" />.
    /// </returns>
    //*************************************************************************

    public static String
    ReplaceControlCharacters
    (
        String sString,
        Char cReplacementCharacter
    )
    {
        Debug.Assert(sString != null);

        Int32 iLength = sString.Length;

        StringBuilder oStringBuilder = new StringBuilder(iLength);

        for (Int32 i = 0; i < iLength; i++)
        {
            Char cChar = sString[i];

            oStringBuilder.Append(Char.IsControl(cChar) ?
                cReplacementCharacter : cChar);
        }

        return ( oStringBuilder.ToString() );
    }
}

}
