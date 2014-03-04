
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Smrf.AppLib;

namespace Smrf.Common.UnitTests
{
//*****************************************************************************
//  Class: WordPairCounterTest
//
/// <summary>
/// This is a Visual Studio test fixture for the <see cref="WordPairCounter" />
/// class.
/// </summary>
//*****************************************************************************

[TestClassAttribute]

public class WordPairCounterTest : Object
{
    //*************************************************************************
    //  Constructor: WordPairCounterTest()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="WordPairCounterTest" />
    /// class.
    /// </summary>
    //*************************************************************************

    public WordPairCounterTest()
    {
        m_oWordPairCounter = null;
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
        m_oWordPairCounter = new WordPairCounter(WordsToSkip);
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
        m_oWordPairCounter = null;
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
        Assert.AreEqual(0, m_oWordPairCounter.TotalDocuments);
        Assert.AreEqual(0, m_oWordPairCounter.TotalWordsInDocuments);
        Assert.AreEqual( 0, m_oWordPairCounter.CountedTerms.Count() );
    }

    //*************************************************************************
    //  Method: TestCountTermsInDocument()
    //
    /// <summary>
    /// Tests CountTermsInDocument() and CountedTerms.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestCountTermsInDocument()
    {
        // Basic test.

        m_oWordPairCounter.CountTermsInDocument(
            "This is a test the a this Words");

        m_oWordPairCounter.CountTermsInDocument(
            "words this Test hello a there ");

        /*
        this test this words
        words this test hello there

        this test 2
        test this 1
        this words 1
        words this 1
        test hello 1
        hello there 1
        */

        Assert.AreEqual(2, m_oWordPairCounter.TotalDocuments);
        Assert.AreEqual(9, m_oWordPairCounter.TotalWordsInDocuments);

        IEnumerable<CountedWordPair> oCountedWordPairs =
            m_oWordPairCounter.CountedTerms;

        Assert.AreEqual( 6, oCountedWordPairs.Count() );

        oCountedWordPairs.Single(
            oCountedWordPair => (
                oCountedWordPair.Word1 == "this"
                &&
                oCountedWordPair.Word2 == "test"
                &&
                oCountedWordPair.Count == 2
                &&
                oCountedWordPair.DocumentsInWhichTermWasCounted == 2
            ) );

        oCountedWordPairs.Single(
            oCountedWordPair => (
                oCountedWordPair.Word1 == "test"
                &&
                oCountedWordPair.Word2 == "this"
                &&
                oCountedWordPair.Count == 1
                &&
                oCountedWordPair.DocumentsInWhichTermWasCounted == 1
            ) );

        oCountedWordPairs.Single(
            oCountedWordPair => (
                oCountedWordPair.Word1 == "this"
                &&
                oCountedWordPair.Word2 == "words"
                &&
                oCountedWordPair.Count == 1
                &&
                oCountedWordPair.DocumentsInWhichTermWasCounted == 1
            ) );

        oCountedWordPairs.Single(
            oCountedWordPair => (
                oCountedWordPair.Word1 == "words"
                &&
                oCountedWordPair.Word2 == "this"
                &&
                oCountedWordPair.Count == 1
                &&
                oCountedWordPair.DocumentsInWhichTermWasCounted == 1
            ) );

        oCountedWordPairs.Single(
            oCountedWordPair => (
                oCountedWordPair.Word1 == "test"
                &&
                oCountedWordPair.Word2 == "hello"
                &&
                oCountedWordPair.Count == 1
                &&
                oCountedWordPair.DocumentsInWhichTermWasCounted == 1
            ) );

        oCountedWordPairs.Single(
            oCountedWordPair => (
                oCountedWordPair.Word1 == "hello"
                &&
                oCountedWordPair.Word2 == "there"
                &&
                oCountedWordPair.Count == 1
                &&
                oCountedWordPair.DocumentsInWhichTermWasCounted == 1
            ) );
    }

    //*************************************************************************
    //  Method: TestCountTermsInDocument2()
    //
    /// <summary>
    /// Tests CountTermsInDocument() and CountedTerms.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestCountTermsInDocument2()
    {
        // Empty text.

        m_oWordPairCounter.CountTermsInDocument(String.Empty);

        Assert.AreEqual(1, m_oWordPairCounter.TotalDocuments);
        Assert.AreEqual(0, m_oWordPairCounter.TotalWordsInDocuments);

        IEnumerable<CountedWordPair> oCountedWordPairs =
            m_oWordPairCounter.CountedTerms;

        Assert.AreEqual( 0, oCountedWordPairs.Count() );
    }

    //*************************************************************************
    //  Method: TestCountTermsInDocument3()
    //
    /// <summary>
    /// Tests CountTermsInDocument() and CountedTerms.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestCountTermsInDocument3()
    {
        // Multiline.

        m_oWordPairCounter.CountTermsInDocument(
            "\r\nThis is\r\n a test the a this\r\nWords\r\n"
            );

        m_oWordPairCounter.CountTermsInDocument(
            "words this Test hello a there ");

        m_oWordPairCounter.CountTermsInDocument("\r\na the\r\n");

        Assert.AreEqual(3, m_oWordPairCounter.TotalDocuments);
        Assert.AreEqual(9, m_oWordPairCounter.TotalWordsInDocuments);

        IEnumerable<CountedWordPair> oCountedWordPairs =
            m_oWordPairCounter.CountedTerms;

        Assert.AreEqual( 6, oCountedWordPairs.Count() );

        oCountedWordPairs.Single(
            oCountedWordPair => (
                oCountedWordPair.Word1 == "this"
                &&
                oCountedWordPair.Word2 == "test"
                &&
                oCountedWordPair.Count == 2
                &&
                oCountedWordPair.DocumentsInWhichTermWasCounted == 2
            ) );

        oCountedWordPairs.Single(
            oCountedWordPair => (
                oCountedWordPair.Word1 == "test"
                &&
                oCountedWordPair.Word2 == "this"
                &&
                oCountedWordPair.Count == 1
                &&
                oCountedWordPair.DocumentsInWhichTermWasCounted == 1
            ) );

        oCountedWordPairs.Single(
            oCountedWordPair => (
                oCountedWordPair.Word1 == "this"
                &&
                oCountedWordPair.Word2 == "words"
                &&
                oCountedWordPair.Count == 1
                &&
                oCountedWordPair.DocumentsInWhichTermWasCounted == 1
            ) );

        oCountedWordPairs.Single(
            oCountedWordPair => (
                oCountedWordPair.Word1 == "words"
                &&
                oCountedWordPair.Word2 == "this"
                &&
                oCountedWordPair.Count == 1
                &&
                oCountedWordPair.DocumentsInWhichTermWasCounted == 1
            ) );

        oCountedWordPairs.Single(
            oCountedWordPair => (
                oCountedWordPair.Word1 == "test"
                &&
                oCountedWordPair.Word2 == "hello"
                &&
                oCountedWordPair.Count == 1
                &&
                oCountedWordPair.DocumentsInWhichTermWasCounted == 1
            ) );

        oCountedWordPairs.Single(
            oCountedWordPair => (
                oCountedWordPair.Word1 == "hello"
                &&
                oCountedWordPair.Word2 == "there"
                &&
                oCountedWordPair.Count == 1
                &&
                oCountedWordPair.DocumentsInWhichTermWasCounted == 1
            ) );
    }

    //*************************************************************************
    //  Method: TestCountTermsInDocument4()
    //
    /// <summary>
    /// Tests CountTermsInDocument() and CountedTerms.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestCountTermsInDocument4()
    {
        // Make sure word order is preserved.

        m_oWordPairCounter.CountTermsInDocument("brown jumping fox");
        m_oWordPairCounter.CountTermsInDocument("FOX JUMPING BROWN");

        Assert.AreEqual(2, m_oWordPairCounter.TotalDocuments);
        Assert.AreEqual(6, m_oWordPairCounter.TotalWordsInDocuments);

        IEnumerable<CountedWordPair> oCountedWordPairs =
            m_oWordPairCounter.CountedTerms;

        Assert.AreEqual( 4, oCountedWordPairs.Count() );

        oCountedWordPairs.Single(
            oCountedWordPair => (
                oCountedWordPair.Word1 == "brown"
                &&
                oCountedWordPair.Word2 == "jumping"
                &&
                oCountedWordPair.Count == 1
                &&
                oCountedWordPair.DocumentsInWhichTermWasCounted == 1
            ) );

        oCountedWordPairs.Single(
            oCountedWordPair => (
                oCountedWordPair.Word1 == "jumping"
                &&
                oCountedWordPair.Word2 == "fox"
                &&
                oCountedWordPair.Count == 1
                &&
                oCountedWordPair.DocumentsInWhichTermWasCounted == 1
            ) );

        oCountedWordPairs.Single(
            oCountedWordPair => (
                oCountedWordPair.Word1 == "fox"
                &&
                oCountedWordPair.Word2 == "jumping"
                &&
                oCountedWordPair.Count == 1
                &&
                oCountedWordPair.DocumentsInWhichTermWasCounted == 1
            ) );

        oCountedWordPairs.Single(
            oCountedWordPair => (
                oCountedWordPair.Word1 == "jumping"
                &&
                oCountedWordPair.Word2 == "brown"
                &&
                oCountedWordPair.Count == 1
                &&
                oCountedWordPair.DocumentsInWhichTermWasCounted == 1
            ) );
    }

    //*************************************************************************
    //  Method: TestCountTermsInDocument5()
    //
    /// <summary>
    /// Tests CountTermsInDocument() and CountedTerms.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestCountTermsInDocument5()
    {
        // Make sure URLs are removed.

        m_oWordPairCounter.CountTermsInDocument(
            "http://abc.com?a=b brown jumping fox https://2.com\r\n"
            + "Fox Jumping http://3.net a the"
            );

        Assert.AreEqual(1, m_oWordPairCounter.TotalDocuments);
        Assert.AreEqual(5, m_oWordPairCounter.TotalWordsInDocuments);

        IEnumerable<CountedWordPair> oCountedWordPairs =
            m_oWordPairCounter.CountedTerms;

        Assert.AreEqual( 4, oCountedWordPairs.Count() );

        oCountedWordPairs.Single(
            oCountedWordPair => (
                oCountedWordPair.Word1 == "brown"
                &&
                oCountedWordPair.Word2 == "jumping"
                &&
                oCountedWordPair.Count == 1
                &&
                oCountedWordPair.DocumentsInWhichTermWasCounted == 1
            ) );

        oCountedWordPairs.Single(
            oCountedWordPair => (
                oCountedWordPair.Word1 == "jumping"
                &&
                oCountedWordPair.Word2 == "fox"
                &&
                oCountedWordPair.Count == 1
                &&
                oCountedWordPair.DocumentsInWhichTermWasCounted == 1
            ) );

        oCountedWordPairs.Single(
            oCountedWordPair => (
                oCountedWordPair.Word1 == "fox"
                &&
                oCountedWordPair.Word2 == "fox"
                &&
                oCountedWordPair.Count == 1
                &&
                oCountedWordPair.DocumentsInWhichTermWasCounted == 1
            ) );

        oCountedWordPairs.Single(
            oCountedWordPair => (
                oCountedWordPair.Word1 == "fox"
                &&
                oCountedWordPair.Word2 == "jumping"
                &&
                oCountedWordPair.Count == 1
                &&
                oCountedWordPair.DocumentsInWhichTermWasCounted == 1
            ) );
    }

    //*************************************************************************
    //  Method: TestCountTermsInDocument6()
    //
    /// <summary>
    /// Tests CountTermsInDocument() and CountedTerms.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestCountTermsInDocument6()
    {
        // Make sure most puncutation is removed.

        m_oWordPairCounter.CountTermsInDocument(
            "brown jumping fox"
            + " Fox Jumping"
            + "! @ # $ % ^ & * ( ) - = + \\ | ~ [ { ] } ; : \" , < . > / ?"
            );

        Assert.AreEqual(1, m_oWordPairCounter.TotalDocuments);
        Assert.AreEqual(5, m_oWordPairCounter.TotalWordsInDocuments);

        IEnumerable<CountedWordPair> oCountedWordPairs =
            m_oWordPairCounter.CountedTerms;

        Assert.AreEqual( 4, oCountedWordPairs.Count() );

        oCountedWordPairs.Single(
            oCountedWordPair => (
                oCountedWordPair.Word1 == "brown"
                &&
                oCountedWordPair.Word2 == "jumping"
                &&
                oCountedWordPair.Count == 1
                &&
                oCountedWordPair.DocumentsInWhichTermWasCounted == 1
            ) );

        oCountedWordPairs.Single(
            oCountedWordPair => (
                oCountedWordPair.Word1 == "jumping"
                &&
                oCountedWordPair.Word2 == "fox"
                &&
                oCountedWordPair.Count == 1
                &&
                oCountedWordPair.DocumentsInWhichTermWasCounted == 1
            ) );

        oCountedWordPairs.Single(
            oCountedWordPair => (
                oCountedWordPair.Word1 == "fox"
                &&
                oCountedWordPair.Word2 == "fox"
                &&
                oCountedWordPair.Count == 1
                &&
                oCountedWordPair.DocumentsInWhichTermWasCounted == 1
            ) );

        oCountedWordPairs.Single(
            oCountedWordPair => (
                oCountedWordPair.Word1 == "fox"
                &&
                oCountedWordPair.Word2 == "jumping"
                &&
                oCountedWordPair.Count == 1
                &&
                oCountedWordPair.DocumentsInWhichTermWasCounted == 1
            ) );
    }

    //*************************************************************************
    //  Method: TestCountTermsInDocument7()
    //
    /// <summary>
    /// Tests CountTermsInDocument() and CountedTerms.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestCountTermsInDocument7()
    {
        // Single quotes and underscores should not be removed.

        m_oWordPairCounter.CountTermsInDocument(
            "a'b_ c_d'"
            );

        Assert.AreEqual(1, m_oWordPairCounter.TotalDocuments);
        Assert.AreEqual(2, m_oWordPairCounter.TotalWordsInDocuments);

        IEnumerable<CountedWordPair> oCountedWordPairs =
            m_oWordPairCounter.CountedTerms;

        Assert.AreEqual( 1, oCountedWordPairs.Count() );

        oCountedWordPairs.Single(
            oCountedWordPair => (
                oCountedWordPair.Word1 == "a'b_"
                &&
                oCountedWordPair.Word2 == "c_d'"
                &&
                oCountedWordPair.Count == 1
                &&
                oCountedWordPair.DocumentsInWhichTermWasCounted == 1
            ) );
    }

    //*************************************************************************
    //  Method: TestCountTermsInDocument8()
    //
    /// <summary>
    /// Tests CountTermsInDocument() and CountedTerms.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestCountTermsInDocument8()
    {
        // Make sure numbers aren't removed.

        m_oWordPairCounter.CountTermsInDocument(
            "#chi2012 123 456.789 1"
            );

        Assert.AreEqual(1, m_oWordPairCounter.TotalDocuments);
        Assert.AreEqual(5, m_oWordPairCounter.TotalWordsInDocuments);

        IEnumerable<CountedWordPair> oCountedWordPairs =
            m_oWordPairCounter.CountedTerms;

        Assert.AreEqual( 4, oCountedWordPairs.Count() );

        oCountedWordPairs.Single(
            oCountedWordPair => (
                oCountedWordPair.Word1 == "chi2012"
                &&
                oCountedWordPair.Word2 == "123"
                &&
                oCountedWordPair.Count == 1
                &&
                oCountedWordPair.DocumentsInWhichTermWasCounted == 1
            ) );

        oCountedWordPairs.Single(
            oCountedWordPair => (
                oCountedWordPair.Word1 == "123"
                &&
                oCountedWordPair.Word2 == "456"
                &&
                oCountedWordPair.Count == 1
                &&
                oCountedWordPair.DocumentsInWhichTermWasCounted == 1
            ) );

        oCountedWordPairs.Single(
            oCountedWordPair => (
                oCountedWordPair.Word1 == "456"
                &&
                oCountedWordPair.Word2 == "789"
                &&
                oCountedWordPair.Count == 1
                &&
                oCountedWordPair.DocumentsInWhichTermWasCounted == 1
            ) );

        oCountedWordPairs.Single(
            oCountedWordPair => (
                oCountedWordPair.Word1 == "789"
                &&
                oCountedWordPair.Word2 == "1"
                &&
                oCountedWordPair.Count == 1
                &&
                oCountedWordPair.DocumentsInWhichTermWasCounted == 1
            ) );
    }

    //*************************************************************************
    //  Method: TestCountTermsInDocument9()
    //
    /// <summary>
    /// Tests CountTermsInDocument() and CountedTerms.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestCountTermsInDocument9()
    {
        // Make sure foreign characters aren't removed.

        m_oWordPairCounter.CountTermsInDocument(
            "hello 各ブースで見かけた美女を特集します"
            );

        Assert.AreEqual(1, m_oWordPairCounter.TotalDocuments);
        Assert.AreEqual(2, m_oWordPairCounter.TotalWordsInDocuments);

        IEnumerable<CountedWordPair> oCountedWordPairs =
            m_oWordPairCounter.CountedTerms;

        Assert.AreEqual( 1, oCountedWordPairs.Count() );

        oCountedWordPairs.Single(
            oCountedWordPair => (
                oCountedWordPair.Word1 == "hello"
                &&
                oCountedWordPair.Word2 == "各ブースで見かけた美女を特集します"
                &&
                oCountedWordPair.Count == 1
                &&
                oCountedWordPair.DocumentsInWhichTermWasCounted == 1
            ) );
    }

    //*************************************************************************
    //  Method: TestCountTermsInDocument10()
    //
    /// <summary>
    /// Tests CountTermsInDocument() and CountedTerms.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestCountTermsInDocument10()
    {
        // Exercise DocumentsInWhichTermWasCounted.

        m_oWordPairCounter.CountTermsInDocument("b c d");
        m_oWordPairCounter.CountTermsInDocument("e f g");
        m_oWordPairCounter.CountTermsInDocument("b c f g b c");

        /*
        b c 3
        c d 1
        e f 1
        f g 2
        c f 1
        g b 1
        */

        Assert.AreEqual(3, m_oWordPairCounter.TotalDocuments);
        Assert.AreEqual(12, m_oWordPairCounter.TotalWordsInDocuments);

        IEnumerable<CountedWordPair> oCountedWordPairs =
            m_oWordPairCounter.CountedTerms;

        Assert.AreEqual( 6, oCountedWordPairs.Count() );

        oCountedWordPairs.Single(
            oCountedWordPair => (
                oCountedWordPair.Word1 == "b"
                &&
                oCountedWordPair.Word2 == "c"
                &&
                oCountedWordPair.Count == 3
                &&
                oCountedWordPair.DocumentsInWhichTermWasCounted == 2
            ) );

        oCountedWordPairs.Single(
            oCountedWordPair => (
                oCountedWordPair.Word1 == "c"
                &&
                oCountedWordPair.Word2 == "d"
                &&
                oCountedWordPair.Count == 1
                &&
                oCountedWordPair.DocumentsInWhichTermWasCounted == 1
            ) );

        oCountedWordPairs.Single(
            oCountedWordPair => (
                oCountedWordPair.Word1 == "e"
                &&
                oCountedWordPair.Word2 == "f"
                &&
                oCountedWordPair.Count == 1
                &&
                oCountedWordPair.DocumentsInWhichTermWasCounted == 1
            ) );

        oCountedWordPairs.Single(
            oCountedWordPair => (
                oCountedWordPair.Word1 == "f"
                &&
                oCountedWordPair.Word2 == "g"
                &&
                oCountedWordPair.Count == 2
                &&
                oCountedWordPair.DocumentsInWhichTermWasCounted == 2
            ) );

        oCountedWordPairs.Single(
            oCountedWordPair => (
                oCountedWordPair.Word1 == "c"
                &&
                oCountedWordPair.Word2 == "f"
                &&
                oCountedWordPair.Count == 1
                &&
                oCountedWordPair.DocumentsInWhichTermWasCounted == 1
            ) );

        oCountedWordPairs.Single(
            oCountedWordPair => (
                oCountedWordPair.Word1 == "g"
                &&
                oCountedWordPair.Word2 == "b"
                &&
                oCountedWordPair.Count == 1
                &&
                oCountedWordPair.DocumentsInWhichTermWasCounted == 1
            ) );
    }

    //*************************************************************************
    //  Method: TestCountTermsInDocument11()
    //
    /// <summary>
    /// Tests CountTermsInDocument() and CountedTerms.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestCountTermsInDocument11()
    {
        // Test Jana Diesner's example for mutual information, which doesn't
        // skip words.

        WordPairCounter oWordPairCounter = new WordPairCounter(
            new String[] {} );

        oWordPairCounter.CountTermsInDocument("Tim and Ben play soccer.");
        oWordPairCounter.CountTermsInDocument("Tim and Sue play soccer.");
        oWordPairCounter.CountTermsInDocument("Yes we can.");
        oWordPairCounter.CountTermsInDocument("epic fail");
        oWordPairCounter.CountTermsInDocument("Tim is available now.");

        oWordPairCounter.CalculateMutualInformationOfCountedTerms();

        Assert.AreEqual(5, oWordPairCounter.TotalDocuments);
        Assert.AreEqual(19, oWordPairCounter.TotalWordsInDocuments);

        IEnumerable<CountedWordPair> oCountedWordPairs =
            oWordPairCounter.CountedTerms;

        Assert.AreEqual( 12, oCountedWordPairs.Count() );

        CountedWordPair oTheCountedWordPair;

        oTheCountedWordPair = oCountedWordPairs.Single(
            oCountedWordPair => (
                oCountedWordPair.Word1 == "tim"
                &&
                oCountedWordPair.Word2 == "and"
                &&
                oCountedWordPair.Count == 2
                &&
                oCountedWordPair.DocumentsInWhichTermWasCounted == 2
            ) );

        Assert.AreEqual(0.669, oTheCountedWordPair.MutualInformation, 0.001);

        oTheCountedWordPair = oCountedWordPairs.Single(
            oCountedWordPair => (
                oCountedWordPair.Word1 == "and"
                &&
                oCountedWordPair.Word2 == "ben"
                &&
                oCountedWordPair.Count == 1
                &&
                oCountedWordPair.DocumentsInWhichTermWasCounted == 1
            ) );

        Assert.AreEqual(0.845, oTheCountedWordPair.MutualInformation, 0.001);

        oTheCountedWordPair = oCountedWordPairs.Single(
            oCountedWordPair => (
                oCountedWordPair.Word1 == "ben"
                &&
                oCountedWordPair.Word2 == "play"
                &&
                oCountedWordPair.Count == 1
                &&
                oCountedWordPair.DocumentsInWhichTermWasCounted == 1
            ) );

        Assert.AreEqual(0.845, oTheCountedWordPair.MutualInformation, 0.001);

        oTheCountedWordPair = oCountedWordPairs.Single(
            oCountedWordPair => (
                oCountedWordPair.Word1 == "and"
                &&
                oCountedWordPair.Word2 == "sue"
                &&
                oCountedWordPair.Count == 1
                &&
                oCountedWordPair.DocumentsInWhichTermWasCounted == 1
            ) );

        Assert.AreEqual(0.845, oTheCountedWordPair.MutualInformation, 0.001);

        oTheCountedWordPair = oCountedWordPairs.Single(
            oCountedWordPair => (
                oCountedWordPair.Word1 == "sue"
                &&
                oCountedWordPair.Word2 == "play"
                &&
                oCountedWordPair.Count == 1
                &&
                oCountedWordPair.DocumentsInWhichTermWasCounted == 1
            ) );

        Assert.AreEqual(0.845, oTheCountedWordPair.MutualInformation, 0.001);

        oTheCountedWordPair = oCountedWordPairs.Single(
            oCountedWordPair => (
                oCountedWordPair.Word1 == "play"
                &&
                oCountedWordPair.Word2 == "soccer"
                &&
                oCountedWordPair.Count == 2
                &&
                oCountedWordPair.DocumentsInWhichTermWasCounted == 2
            ) );

        Assert.AreEqual(0.845, oTheCountedWordPair.MutualInformation, 0.001);

        oTheCountedWordPair = oCountedWordPairs.Single(
            oCountedWordPair => (
                oCountedWordPair.Word1 == "yes"
                &&
                oCountedWordPair.Word2 == "we"
                &&
                oCountedWordPair.Count == 1
                &&
                oCountedWordPair.DocumentsInWhichTermWasCounted == 1
            ) );

        Assert.AreEqual(1.146, oTheCountedWordPair.MutualInformation, 0.001);

        oTheCountedWordPair = oCountedWordPairs.Single(
            oCountedWordPair => (
                oCountedWordPair.Word1 == "we"
                &&
                oCountedWordPair.Word2 == "can"
                &&
                oCountedWordPair.Count == 1
                &&
                oCountedWordPair.DocumentsInWhichTermWasCounted == 1
            ) );

        Assert.AreEqual(1.146, oTheCountedWordPair.MutualInformation, 0.001);

        oTheCountedWordPair = oCountedWordPairs.Single(
            oCountedWordPair => (
                oCountedWordPair.Word1 == "epic"
                &&
                oCountedWordPair.Word2 == "fail"
                &&
                oCountedWordPair.Count == 1
                &&
                oCountedWordPair.DocumentsInWhichTermWasCounted == 1
            ) );

        Assert.AreEqual(1.146, oTheCountedWordPair.MutualInformation, 0.001);

        oTheCountedWordPair = oCountedWordPairs.Single(
            oCountedWordPair => (
                oCountedWordPair.Word1 == "tim"
                &&
                oCountedWordPair.Word2 == "is"
                &&
                oCountedWordPair.Count == 1
                &&
                oCountedWordPair.DocumentsInWhichTermWasCounted == 1
            ) );

        Assert.AreEqual(0.669, oTheCountedWordPair.MutualInformation, 0.001);

        oTheCountedWordPair = oCountedWordPairs.Single(
            oCountedWordPair => (
                oCountedWordPair.Word1 == "is"
                &&
                oCountedWordPair.Word2 == "available"
                &&
                oCountedWordPair.Count == 1
                &&
                oCountedWordPair.DocumentsInWhichTermWasCounted == 1
            ) );

        Assert.AreEqual(1.146, oTheCountedWordPair.MutualInformation, 0.001);

        oTheCountedWordPair = oCountedWordPairs.Single(
            oCountedWordPair => (
                oCountedWordPair.Word1 == "available"
                &&
                oCountedWordPair.Word2 == "now"
                &&
                oCountedWordPair.Count == 1
                &&
                oCountedWordPair.DocumentsInWhichTermWasCounted == 1
            ) );

        Assert.AreEqual(1.146, oTheCountedWordPair.MutualInformation, 0.001);
    }

    //*************************************************************************
    //  Method: TestClear()
    //
    /// <summary>
    /// Tests the Clear() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestClear()
    {
        m_oWordPairCounter.CountTermsInDocument("this is a test");
        m_oWordPairCounter.Clear();

        Assert.AreEqual( 0, m_oWordPairCounter.CountedTerms.Count() );
    }


    //*************************************************************************
    //  Protected constants
    //*************************************************************************

    /// Words to skip.

    protected static readonly String [] WordsToSkip =
        {"the", "a", "is"};


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Object being tested.

    protected WordPairCounter m_oWordPairCounter;
}

}
