
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Smrf.SocialNetworkLib.Twitter;

namespace Smrf.Common.UnitTests
{
//*****************************************************************************
//  Class: TwitterStatusTextParserTest
//
/// <summary>
/// This is a Visual Studio test fixture for the <see
/// cref="TwitterStatusTextParser" /> class.
/// </summary>
//*****************************************************************************

[TestClassAttribute]

public class TwitterStatusTextParserTest : Object
{
    //*************************************************************************
    //  Constructor: TwitterStatusTextParserTest()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="TwitterStatusTextParserTest" /> class.
    /// </summary>
    //*************************************************************************

    public TwitterStatusTextParserTest()
    {
        m_oTwitterStatusTextParser = null;
    }

    //*************************************************************************
    //  Method: SetUp()
    //
    /// <summary>
    /// Gets run before each test.
    /// </summary>
    //*************************************************************************

    [TestInitializeAttribute]

    public void
    SetUp()
    {
        m_oTwitterStatusTextParser = new TwitterStatusTextParser();
    }

    //*************************************************************************
    //  Method: TearDown()
    //
    /// <summary>
    /// Gets run after each test.
    /// </summary>
    //*************************************************************************

    [TestCleanupAttribute]

    public void
    TearDown()
    {
        m_oTwitterStatusTextParser = null;
    }

    //*************************************************************************
    //  Method: TestConstructor()
    //
    /// <summary>
    /// Tests the constructor.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestConstructor()
    {
        // (Do nothing.)
    }

    //*************************************************************************
    //  Method: TestGetScreenNames()
    //
    /// <summary>
    /// Tests the GetScreenNames() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGetScreenNames()
    {
        // Empty tweet.

        String sRepliedToScreenName;
        String [] asUniqueMentionedScreenNames;

        m_oTwitterStatusTextParser.GetScreenNames(String.Empty,
            out sRepliedToScreenName, out asUniqueMentionedScreenNames);

        Assert.IsNull(sRepliedToScreenName);
        Assert.AreEqual(0, asUniqueMentionedScreenNames.Length);
    }

    //*************************************************************************
    //  Method: TestGetScreenNames2()
    //
    /// <summary>
    /// Tests the GetScreenNames() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGetScreenNames2()
    {
        // No reply-to, no mentions.

        String sRepliedToScreenName;
        String [] asUniqueMentionedScreenNames;

        m_oTwitterStatusTextParser.GetScreenNames("the tweet",
            out sRepliedToScreenName, out asUniqueMentionedScreenNames);

        Assert.IsNull(sRepliedToScreenName);
        Assert.AreEqual(0, asUniqueMentionedScreenNames.Length);
    }

    //*************************************************************************
    //  Method: TestGetScreenNames3()
    //
    /// <summary>
    /// Tests the GetScreenNames() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGetScreenNames3()
    {
        // Reply-to, no mentions.

        String sRepliedToScreenName;
        String [] asUniqueMentionedScreenNames;

        m_oTwitterStatusTextParser.GetScreenNames("@John the tweet",
            out sRepliedToScreenName, out asUniqueMentionedScreenNames);

        Assert.AreEqual("john", sRepliedToScreenName);
        Assert.AreEqual(0, asUniqueMentionedScreenNames.Length);
    }

    //*************************************************************************
    //  Method: TestGetScreenNames4()
    //
    /// <summary>
    /// Tests the GetScreenNames() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGetScreenNames4()
    {
        // No reply-to, mentions.

        String sRepliedToScreenName;
        String [] asUniqueMentionedScreenNames;

        m_oTwitterStatusTextParser.GetScreenNames(
            "Hello the tweet @jack @jill @john",
            out sRepliedToScreenName, out asUniqueMentionedScreenNames);

        Assert.IsNull(sRepliedToScreenName);
        Assert.AreEqual(3, asUniqueMentionedScreenNames.Length);
        Assert.IsTrue( asUniqueMentionedScreenNames.Contains("jack") );
        Assert.IsTrue( asUniqueMentionedScreenNames.Contains("jill") );
        Assert.IsTrue( asUniqueMentionedScreenNames.Contains("john") );
    }

    //*************************************************************************
    //  Method: TestGetScreenNames5()
    //
    /// <summary>
    /// Tests the GetScreenNames() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGetScreenNames5()
    {
        // Reply-to, mentions.

        String sRepliedToScreenName;
        String [] asUniqueMentionedScreenNames;

        m_oTwitterStatusTextParser.GetScreenNames(
            "@John the tweet @jack @jill @john",
            out sRepliedToScreenName, out asUniqueMentionedScreenNames);

        Assert.AreEqual("john", sRepliedToScreenName);
        Assert.AreEqual(2, asUniqueMentionedScreenNames.Length);
        Assert.IsTrue( asUniqueMentionedScreenNames.Contains("jack") );
        Assert.IsTrue( asUniqueMentionedScreenNames.Contains("jill") );
    }

    //*************************************************************************
    //  Method: TestGetScreenNames6()
    //
    /// <summary>
    /// Tests the GetScreenNames() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGetScreenNames6()
    {
        // Reply-to, mentions, with commas and period.

        String sRepliedToScreenName;
        String [] asUniqueMentionedScreenNames;

        m_oTwitterStatusTextParser.GetScreenNames(
            "@John, the tweet @jack, @jill, and @john.",
            out sRepliedToScreenName, out asUniqueMentionedScreenNames);

        Assert.AreEqual("john", sRepliedToScreenName);
        Assert.AreEqual(2, asUniqueMentionedScreenNames.Length);
        Assert.IsTrue( asUniqueMentionedScreenNames.Contains("jack") );
        Assert.IsTrue( asUniqueMentionedScreenNames.Contains("jill") );
    }

    //*************************************************************************
    //  Method: TestGetScreenNames7()
    //
    /// <summary>
    /// Tests the GetScreenNames() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGetScreenNames7()
    {
        // Reply-to, mentions, with mixed-case duplicates.

        String sRepliedToScreenName;
        String [] asUniqueMentionedScreenNames;

        m_oTwitterStatusTextParser.GetScreenNames(
            "@John, the tweet @jack, @jill, @JaCk @JIll and @john.",
            out sRepliedToScreenName, out asUniqueMentionedScreenNames);

        Assert.AreEqual("john", sRepliedToScreenName);
        Assert.AreEqual(2, asUniqueMentionedScreenNames.Length);
        Assert.IsTrue( asUniqueMentionedScreenNames.Contains("jack") );
        Assert.IsTrue( asUniqueMentionedScreenNames.Contains("jill") );
    }

    //*************************************************************************
    //  Method: TestGetScreenNames8()
    //
    /// <summary>
    /// Tests the GetScreenNames() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGetScreenNames8()
    {
        // Multiline.

        String sRepliedToScreenName;
        String [] asUniqueMentionedScreenNames;

        m_oTwitterStatusTextParser.GetScreenNames(
            "@john, the tweet @jack\r\n @bill, @sally \r\n@joe\r\n",
            out sRepliedToScreenName, out asUniqueMentionedScreenNames);

        Assert.AreEqual("john", sRepliedToScreenName);
        Assert.AreEqual(4, asUniqueMentionedScreenNames.Length);
        Assert.IsTrue( asUniqueMentionedScreenNames.Contains("jack") );
        Assert.IsTrue( asUniqueMentionedScreenNames.Contains("bill") );
        Assert.IsTrue( asUniqueMentionedScreenNames.Contains("sally") );
        Assert.IsTrue( asUniqueMentionedScreenNames.Contains("joe") );
    }

    //*************************************************************************
    //  Method: TestGetScreenNames9()
    //
    /// <summary>
    /// Tests the GetScreenNames() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGetScreenNames9()
    {
        // Reply-to, mentions, with colons.

        String sRepliedToScreenName;
        String [] asUniqueMentionedScreenNames;

        m_oTwitterStatusTextParser.GetScreenNames(
            "@John: the tweet @jack: @jill: @john",
            out sRepliedToScreenName, out asUniqueMentionedScreenNames);

        Assert.AreEqual("john", sRepliedToScreenName);
        Assert.AreEqual(2, asUniqueMentionedScreenNames.Length);
        Assert.IsTrue( asUniqueMentionedScreenNames.Contains("jack") );
        Assert.IsTrue( asUniqueMentionedScreenNames.Contains("jill") );
    }

    //*************************************************************************
    //  Method: TestGetScreenNames10()
    //
    /// <summary>
    /// Tests the GetScreenNames() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGetScreenNames10()
    {
        // Reply-to, mentions, with email addresses that shouldn't be counted
        // as mentions.

        String sRepliedToScreenName;
        String [] asUniqueMentionedScreenNames;

        m_oTwitterStatusTextParser.GetScreenNames(

            "@John joe@a.com the bill99@nowhere.com tweet @jack @jill @john"
            + " sally@yahoo.com",

            out sRepliedToScreenName, out asUniqueMentionedScreenNames);

        Assert.AreEqual("john", sRepliedToScreenName);
        Assert.AreEqual(2, asUniqueMentionedScreenNames.Length);
        Assert.IsTrue( asUniqueMentionedScreenNames.Contains("jack") );
        Assert.IsTrue( asUniqueMentionedScreenNames.Contains("jill") );
    }

    //*************************************************************************
    //  Method: TestGetScreenNames11()
    //
    /// <summary>
    /// Tests the GetScreenNames() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGetScreenNames11()
    {
        // Mentions, with email addresses that shouldn't be counted as
        // mentions.

        String sRepliedToScreenName;
        String [] asUniqueMentionedScreenNames;

        m_oTwitterStatusTextParser.GetScreenNames(

            "joe@a.com the bill99@nowhere.com tweet @jack @jill @john"
            + " sally@yahoo.com",

            out sRepliedToScreenName, out asUniqueMentionedScreenNames);

        Assert.IsNull(sRepliedToScreenName);
        Assert.AreEqual(3, asUniqueMentionedScreenNames.Length);
        Assert.IsTrue( asUniqueMentionedScreenNames.Contains("jack") );
        Assert.IsTrue( asUniqueMentionedScreenNames.Contains("jill") );
        Assert.IsTrue( asUniqueMentionedScreenNames.Contains("john") );
    }

    //*************************************************************************
    //  Method: TestGetScreenNames12()
    //
    /// <summary>
    /// Tests the GetScreenNames() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGetScreenNames12()
    {
        // User names preceeded by alpha characters and numbers.

        String sRepliedToScreenName;
        String [] asUniqueMentionedScreenNames;

        m_oTwitterStatusTextParser.GetScreenNames(
            "x@John the tweet y@jack b@jill 3@john",
            out sRepliedToScreenName, out asUniqueMentionedScreenNames);

        Assert.IsNull(sRepliedToScreenName);
        Assert.AreEqual(0, asUniqueMentionedScreenNames.Length);
    }

    //*************************************************************************
    //  Method: TestGetScreenNames13()
    //
    /// <summary>
    /// Tests the GetScreenNames() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGetScreenNames13()
    {
        // Reply-to, mentions, with punctuation preceeding the @ character in
        // mentions.  The mentions are separated by spaces.

        String [] asScreenNamesPreceededByPunctuation =
            GetScreenNamesPreceededByPunctuation();

        Int32 iScreenNamesPreceededByPunctuation =
            asScreenNamesPreceededByPunctuation.Length;

        String sRepliedToScreenName;
        String [] asUniqueMentionedScreenNames;

        m_oTwitterStatusTextParser.GetScreenNames(

            "@John"
            + " "
            + String.Join(" ", asScreenNamesPreceededByPunctuation)
            ,

            out sRepliedToScreenName, out asUniqueMentionedScreenNames);

        Assert.AreEqual("john", sRepliedToScreenName);

        Assert.AreEqual(iScreenNamesPreceededByPunctuation,
            asUniqueMentionedScreenNames.Length);

        for (Int32 i = 0; i < iScreenNamesPreceededByPunctuation; i++)
        {
            Assert.IsTrue( asUniqueMentionedScreenNames.Contains(
                "screenname" + i) );
        }
    }

    //*************************************************************************
    //  Method: TestGetScreenNames14()
    //
    /// <summary>
    /// Tests the GetScreenNames() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGetScreenNames14()
    {
        // Reply-to, mentions, with punctuation preceeding the @ character in
        // mentions.  The mentions are NOT separated by spaces.

        String [] asScreenNamesPreceededByPunctuation =
            GetScreenNamesPreceededByPunctuation();

        Int32 iScreenNamesPreceededByPunctuation =
            asScreenNamesPreceededByPunctuation.Length;

        String sRepliedToScreenName;
        String [] asUniqueMentionedScreenNames;

        m_oTwitterStatusTextParser.GetScreenNames(

            "@John"
            + String.Join(String.Empty, asScreenNamesPreceededByPunctuation)
            ,

            out sRepliedToScreenName, out asUniqueMentionedScreenNames);

        Assert.AreEqual("john", sRepliedToScreenName);

        Assert.AreEqual(iScreenNamesPreceededByPunctuation,
            asUniqueMentionedScreenNames.Length);

        for (Int32 i = 0; i < iScreenNamesPreceededByPunctuation; i++)
        {
            Assert.IsTrue( asUniqueMentionedScreenNames.Contains(
                "screenname" + i) );
        }
    }

    //*************************************************************************
    //  Method: TestGetScreenNames15()
    //
    /// <summary>
    /// Tests the GetScreenNames() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGetScreenNames15()
    {
        // Reply-to, mentions, with punctuation preceeding the @ character in
        // reply-to and mentions.

        String [] asScreenNamesPreceededByPunctuation =
            GetScreenNamesPreceededByPunctuation();

        Int32 iScreenNamesPreceededByPunctuation =
            asScreenNamesPreceededByPunctuation.Length;

        String sRepliedToScreenName;
        String [] asUniqueMentionedScreenNames;

        m_oTwitterStatusTextParser.GetScreenNames(

            // Note that the "!" here should get replaced with a space, which
            // should cause John to be counted as a mentions, not a reply-to.

            "!@John"
            + " "
            + String.Join(" ", asScreenNamesPreceededByPunctuation)
            ,

            out sRepliedToScreenName, out asUniqueMentionedScreenNames);

        Assert.IsNull(sRepliedToScreenName);

        Assert.AreEqual(iScreenNamesPreceededByPunctuation + 1,
            asUniqueMentionedScreenNames.Length);

        Assert.IsTrue( asUniqueMentionedScreenNames.Contains("john") );

        for (Int32 i = 0; i < iScreenNamesPreceededByPunctuation; i++)
        {
            Assert.IsTrue( asUniqueMentionedScreenNames.Contains(
                "screenname" + i) );
        }
    }

    //*************************************************************************
    //  Method: TestGetScreenNames16()
    //
    /// <summary>
    /// Tests the GetScreenNames() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGetScreenNames16()
    {
        // Reply-to, mentions, both with underscores.

        String sRepliedToScreenName;
        String [] asUniqueMentionedScreenNames;

        m_oTwitterStatusTextParser.GetScreenNames(
            "@John_smith the tweet @jack_jones @jill @john_smith",
            out sRepliedToScreenName, out asUniqueMentionedScreenNames);

        Assert.AreEqual("john_smith", sRepliedToScreenName);
        Assert.AreEqual(2, asUniqueMentionedScreenNames.Length);
        Assert.IsTrue( asUniqueMentionedScreenNames.Contains("jack_jones") );
        Assert.IsTrue( asUniqueMentionedScreenNames.Contains("jill") );
    }

    //*************************************************************************
    //  Method: GetScreenNamesPreceededByPunctuation()
    //
    /// <summary>
    /// Gets an array of screen names, each one preceeded by a different
    /// punctuation character.
    /// </summary>
    ///
    /// <returns>
    /// An array of screen names.  The screen names, without the punctuation
    /// character, have the format "@screennameN".
    /// </returns>
    //*************************************************************************

    protected String []
    GetScreenNamesPreceededByPunctuation()
    {
        List<String> oScreenNames = new List<String>();
        Int32 i = 0;

        // Note the absence of "@" and "_".

        foreach (Char cPunctuation in "!#$%^&*()-=+\\|`~[{]};:'\",<.>/?")
        {
            oScreenNames.Add(cPunctuation + "@screenname" + i);
            i++;
        }

        return ( oScreenNames.ToArray() );
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Object to test.

    protected TwitterStatusTextParser m_oTwitterStatusTextParser;
}

}
