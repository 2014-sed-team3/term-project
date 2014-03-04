
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
    /// <param name="noun">
    /// Noun to make plural if necessary.  Sample: "orange".
    /// </param>
    ///
    /// <param name="count">
    /// Number of things that <paramref name="noun" /> describes.  Sample: 2.
    /// </param>
    ///
    /// <returns>
    /// <paramref name="noun" /> with "s" appended to it if necessary.
    /// Sample: "oranges".
    /// </returns>
    //*************************************************************************

    public static String
    MakePlural
    (
        String noun,
        Int32 count
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(noun) );
        Debug.Assert(count >= 0);

        return ( (count == 1) ? noun : noun + "s" );
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
    //  Method: SplitOnCommonDelimiters()
    //
    /// <summary>
    /// Splits a string on a set of common delimiters.
    /// </summary>
    ///
    /// <param name="stringToSplit">
    /// The string to split.  Can't be null.
    /// </param>
    ///
    /// <returns>
    /// An array of zero or more non-empty strings.
    /// </returns>
    ///
    /// <remarks>
    /// The common delimiters are space, comma, \r and \n.
    /// </remarks>
    //*************************************************************************

    public static String []
    SplitOnCommonDelimiters
    (
        String stringToSplit
    )
    {
        Debug.Assert(stringToSplit != null);

        return ( stringToSplit.Split(new Char[]{' ', ',', '\r', '\n'},
            StringSplitOptions.RemoveEmptyEntries) );
    }

    //*************************************************************************
    //  Method: SplitOnSpaces()
    //
    /// <summary>
    /// Splits a string on space delimiters.
    /// </summary>
    ///
    /// <param name="stringToSplit">
    /// The string to split.  Can't be null.
    /// </param>
    ///
    /// <returns>
    /// An array of zero or more non-empty strings.
    /// </returns>
    //*************************************************************************

    public static String []
    SplitOnSpaces
    (
        String stringToSplit
    )
    {
        Debug.Assert(stringToSplit != null);

        return ( stringToSplit.Split(new Char[]{' '},
            StringSplitOptions.RemoveEmptyEntries) );
    }

    //*************************************************************************
    //  Method: TruncateWithEllipses()
    //
    /// <summary>
    /// Truncates a string and adds ellipses if a string is too long.
    /// an Excel cell.
    /// </summary>
    ///
    /// <param name="stringToTruncate">
    /// The string to truncate.  Can't be null.
    /// </param>
    ///
    /// <param name="maximumLength">
    /// The maximum length of the string, including the 3 characters that this
    /// method may add to the string after truncating it.  Must be 3 or
    /// greater.
    /// </param>
    ///
    /// <returns>
    /// The string, truncated if necessary.
    /// </returns>
    ///
    /// <remarks>
    /// If the length of <paramref name="stringToTruncate" /> is greater than
    /// <paramref name="maximumLength" />, this method truncates the string and
    /// adds ellipses.  If <paramref name="stringToTruncate" /> is "The brown
    /// fox" and <paramref name="maximumLength" /> is 9, for example, then "The
    /// br..." is returned.
    /// </remarks>
    //*************************************************************************

    public static String
    TruncateWithEllipses
    (
        String stringToTruncate,
        Int32 maximumLength
    )
    {
        Debug.Assert(stringToTruncate != null);
        Debug.Assert(maximumLength >= 3);

        if (stringToTruncate.Length > maximumLength)
        {
            stringToTruncate = stringToTruncate.Substring(0, maximumLength - 3)
                + "...";
        }

        return (stringToTruncate);
    }

    //*************************************************************************
    //  Method: ReplaceControlCharacters()
    //
    /// <summary>
    /// Replaces Unicode control characters with a specified character.
    /// </summary>
    ///
    /// <param name="stringToReplace">
    /// String that may include control characters.  Can't be null.
    /// </param>
    ///
    /// <param name="replacementCharacter">
    /// Character to replace them with.
    /// </param>
    ///
    /// <returns>
    /// <paramref name="stringToReplace" /> with control characters replaced
    /// with <paramref name="replacementCharacter" />.
    /// </returns>
    //*************************************************************************

    public static String
    ReplaceControlCharacters
    (
        String stringToReplace,
        Char replacementCharacter
    )
    {
        Debug.Assert(stringToReplace != null);

        Int32 iLength = stringToReplace.Length;

        StringBuilder oStringBuilder = new StringBuilder(iLength);

        for (Int32 i = 0; i < iLength; i++)
        {
            Char cChar = stringToReplace[i];

            oStringBuilder.Append(Char.IsControl(cChar) ?
                replacementCharacter : cChar);
        }

        return ( oStringBuilder.ToString() );
    }

    //*************************************************************************
    //  Method: IsAscii()
    //
    /// <summary>
    /// Determines whether all characters in a string are ASCII.
    /// </summary>
    ///
    /// <param name="stringToTest">
    /// String to test.  Can't be null.
    /// </param>
    ///
    /// <returns>
    /// true if <paramref name="stringToTest" /> contains all ASCII characters.
    /// </returns>
    //*************************************************************************

    public static Boolean
    IsAscii
    (
        String stringToTest
    )
    {
        Debug.Assert(stringToTest != null);

        // Encoding.ASCII.GetBytes() returns 63 decimal (a question mark) for a
        // Unicode character, so a round-trip will change the string.

        return (

            stringToTest ==

            Encoding.ASCII.GetString(
                Encoding.ASCII.GetBytes(stringToTest) )
            );
    }

    //*************************************************************************
    //  Method: AppendAfterEmptyLine()
    //
    /// <summary>
    /// Appends text to a StringBuilder, preceding it with an empty line if
    /// appropriate.
    /// </summary>
    ///
    /// <param name="stringBuilder">
    /// Object used to build a string.
    /// </param>
    ///
    /// <param name="text">
    /// Text to append.  Can be null or empty.
    /// </param>
    ///
    /// <returns>
    /// true if the text was appended.
    /// </returns>
    ///
    /// <remarks>
    /// If <paramref name="text" /> is null or empty, this method does nothing.
    /// Otherwise, it appends a line terminator (if <paramref
    /// name="stringBuilder" /> isn't empty) and <paramref name="text" /> to
    /// <paramref name="stringBuilder" />.
    /// </remarks>
    //*************************************************************************

    public static Boolean
    AppendAfterEmptyLine
    (
        StringBuilder stringBuilder,
        String text
    )
    {
        Debug.Assert(stringBuilder != null);

        if ( !String.IsNullOrEmpty(text) )
        {
            if (stringBuilder.Length > 0)
            {
                stringBuilder.AppendLine();
            }

            stringBuilder.Append(text);
            return (true);
        }

        return (false);
    }

    //*************************************************************************
    //  Method: AppendLineAfterEmptyLine()
    //
    /// <summary>
    /// Appends text and a line terminator to a StringBuilder, preceding it
    /// with an empty line if appropriate.
    /// </summary>
    ///
    /// <param name="stringBuilder">
    /// Object used to build a string.
    /// </param>
    ///
    /// <param name="text">
    /// Text to append.  Can be null or empty.
    /// </param>
    ///
    /// <returns>
    /// true if the text was appended.
    /// </returns>
    ///
    /// <remarks>
    /// If <paramref name="text" /> is null or empty, this method does nothing.
    /// Otherwise, it appends a line terminator (if <paramref
    /// name="stringBuilder" /> isn't empty), <paramref name="text" /> and
    /// another line terminator to <paramref name="stringBuilder" />.
    /// </remarks>
    //*************************************************************************

    public static Boolean
    AppendLineAfterEmptyLine
    (
        StringBuilder stringBuilder,
        String text
    )
    {
        Debug.Assert(stringBuilder != null);

        if ( AppendAfterEmptyLine(stringBuilder, text) )
        {
            stringBuilder.AppendLine();
            return (true);
        }

        return (false);
    }

    //*************************************************************************
    //  Method: AppendSectionSeparator()
    //
    /// <summary>
    /// Appends a separator between sections of text in a StringBuilder.
    /// </summary>
    ///
    /// <param name="stringBuilder">
    /// Object used to build a string.
    /// </param>
    ///
    /// <remarks>
    /// If <paramref name="stringBuilder" /> isn't empty, this method appends
    /// two empty lines to it to provide a separator between the sections of
    /// text that the caller is building.
    /// </remarks>
    //*************************************************************************

    public static void
    AppendSectionSeparator
    (
        StringBuilder stringBuilder
    )
    {
        Debug.Assert(stringBuilder != null);

        if (stringBuilder.Length > 0)
        {
            stringBuilder.AppendLine();
            stringBuilder.AppendLine();
        }
    }


    //*************************************************************************
    //  Public constants
    //*************************************************************************

    /// Field separator that can be used to store joined strings.  This is the
    /// Unicode "dark shade" character, which is unlikely to be used within the
    /// strings that are joined.  This is defined in both character array and
    /// string versions to accommodate String.Split() and String.Join().

    public static readonly Char [] FieldSeparator = new Char[] {'\u2593'};
    ///
    public const String FieldSeparatorString = "\u2593";

    /// Sub-field separator.  This is the Unicode "light shade" character,

    public static readonly Char [] SubFieldSeparator = new Char[] {'\u2591'};
    ///
    public const String SubFieldSeparatorString = "\u2591";
}

}
