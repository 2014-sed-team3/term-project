
using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace Smrf.SocialNetworkLib.Twitter
{
//*****************************************************************************
//  Class: TwitterStatusTextParser
//
/// <summary>
/// Parses the text of a Twitter status (tweet).
/// </summary>
///
/// <remarks>
/// A tweet is known as a "status" in the Twitter API, so "status" is the term
/// used in this class's methods.
/// </remarks>
//*****************************************************************************

public class TwitterStatusTextParser : Object
{
    //*************************************************************************
    //  Constructor: TwitterStatusTextParser()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="TwitterStatusTextParser" /> class.
    /// </summary>
    //*************************************************************************

    public TwitterStatusTextParser()
    {
        // "Starts with a screen name."

        m_oRepliedToRegex = new Regex(@"^@(?<ScreenName>\w+)");

        // "Contains a screen name."

        m_oMentionedRegex = new Regex(@"(^|\s)@(?<ScreenName>\w+)",
            RegexOptions.Multiline);

        AssertValid();
    }

    //*************************************************************************
    //  Method: GetScreenNames()
    //
    /// <summary>
    /// Extracts the screen names from a status.
    /// </summary>
    ///
    /// <param name="statusText">
    /// Status text that might contain screen names.  Can't be null.
    /// </param>
    ///
    /// <param name="repliedToScreenName">
    /// If the status is a reply-to, this gets set to the replied-to screen
    /// name, converted to lower case.  Otherwise, this gets set to null.
    /// </param>
    ///
    /// <param name="uniqueMentionedScreenNames">
    /// If the status mentions other screen names, they gets stored here,
    /// converted to lower case.  Otherwise, this gets set to an empty array.
    /// </param>
    ///
    /// <remarks>
    /// The <paramref name="repliedToScreenName" />, if there is one, is NOT
    /// also included in <paramref name="uniqueMentionedScreenNames" />.
    ///
    /// <para>
    /// If a screen name is mentioned more than once, the duplicates are
    /// ignored.  Screen names are not considered case-sensitive.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public void
    GetScreenNames
    (
        String statusText,
        out String repliedToScreenName,
        out String [] uniqueMentionedScreenNames
    )
    {
        Debug.Assert(statusText != null);
        AssertValid();

        statusText = ReplacePunctuationWithSpaces(statusText);

        repliedToScreenName = null;
        Match oRepliedToMatch = m_oRepliedToRegex.Match(statusText);

        if (oRepliedToMatch.Success)
        {
            repliedToScreenName =
                oRepliedToMatch.Groups["ScreenName"].Value.ToLower();
        }

        HashSet<String> oUniqueMentionedScreenNames = new HashSet<String>();
        Match oMentionedMatch = m_oMentionedRegex.Match(statusText);

        while (oMentionedMatch.Success)
        {
            String sMentionedScreenName =
                oMentionedMatch.Groups["ScreenName"].Value.ToLower();

            // A "reply-to is not also a "mentions."

            if (sMentionedScreenName != repliedToScreenName)
            {
                oUniqueMentionedScreenNames.Add(sMentionedScreenName);
            }

            oMentionedMatch = oMentionedMatch.NextMatch();
        }

        uniqueMentionedScreenNames = oUniqueMentionedScreenNames.ToArray();
    }

    //*************************************************************************
    //  Method: ReplacePunctuationWithSpaces()
    //
    /// <summary>
    /// Replaces most punctuation characters in status text with spaces.
    /// </summary>
    ///
    /// <param name="sStatusText">
    /// Status text that might contain screen names.  Can't be null.
    /// </param>
    ///
    /// <returns>
    /// <paramref name="sStatusText" /> with most punctuation replaced.
    /// </returns>
    //*************************************************************************

    protected String
    ReplacePunctuationWithSpaces
    (
        String sStatusText
    )
    {
        Debug.Assert(sStatusText != null);
        AssertValid();

        StringBuilder oStringBuilder = new StringBuilder();

        // It's more straightforward to do this with a StringBuilder than with
        // a regular expression.

        foreach (Char c in sStatusText)
        {
            oStringBuilder.Append(

                (
                    // Don't replace "@", which signals the start of a screen
                    // name.

                    c == '@'

                    // Don't replace "_", which is the only punctuation allowed
                    // in a twitter screen name.

                    ||
                    c == '_'

                    // Don't replace alphanumeric characters.

                    ||
                    Char.IsLetterOrDigit(c)
                )

                ? c : ' '
                );
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

    public void
    AssertValid()
    {
        Debug.Assert(m_oRepliedToRegex != null);
        Debug.Assert(m_oMentionedRegex != null);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Regex for finding a "reply-to."

    protected Regex m_oRepliedToRegex;

    /// Regex for finding a "mentions."

    protected Regex m_oMentionedRegex;
}

}
