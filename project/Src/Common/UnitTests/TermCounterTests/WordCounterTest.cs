
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Smrf.AppLib;

namespace Smrf.Common.UnitTests
{
//*****************************************************************************
//  Class: WordCounterTest
//
/// <summary>
/// This is a Visual Studio test fixture for the <see cref="WordCounter" />
/// class.
/// </summary>
//*****************************************************************************

[TestClassAttribute]

public class WordCounterTest : Object
{
    //*************************************************************************
    //  Constructor: WordCounterTest()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="WordCounterTest" />
    /// class.
    /// </summary>
    //*************************************************************************

    public WordCounterTest()
    {
        m_oWordCounter = null;
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
        m_oWordCounter = new WordCounter(WordsToSkip);
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
        m_oWordCounter = null;
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
        Assert.AreEqual(0, m_oWordCounter.TotalDocuments);
        Assert.AreEqual(0, m_oWordCounter.TotalWordsInDocuments);
        Assert.AreEqual( 0, m_oWordCounter.CountedTerms.Count() );
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

        m_oWordCounter.CountTermsInDocument(
            "This is a test the a this Words");

        m_oWordCounter.CountTermsInDocument(
            "words this Test hello a there ");

        /*
        this test this words
        words this test hello there

        this 3
        test 2
        words 2
        hello 1
        there 1
        */

        Assert.AreEqual(2, m_oWordCounter.TotalDocuments);
        Assert.AreEqual(9, m_oWordCounter.TotalWordsInDocuments);

        IEnumerable<CountedWord> oCountedWords = m_oWordCounter.CountedTerms;

        Assert.AreEqual( 5, oCountedWords.Count() );

        oCountedWords.Single(
            oCountedWord => (
                oCountedWord.Word == "this"
                &&
                oCountedWord.Count == 3
                &&
                oCountedWord.DocumentsInWhichTermWasCounted == 2
            ) );

        oCountedWords.Single(
            oCountedWord => (
                oCountedWord.Word == "test"
                &&
                oCountedWord.Count == 2
                &&
                oCountedWord.DocumentsInWhichTermWasCounted == 2
            ) );

        oCountedWords.Single(
            oCountedWord => (
                oCountedWord.Word == "words"
                &&
                oCountedWord.Count == 2
                &&
                oCountedWord.DocumentsInWhichTermWasCounted == 2
            ) );

        oCountedWords.Single(
            oCountedWord => (
                oCountedWord.Word == "hello"
                &&
                oCountedWord.Count == 1
                &&
                oCountedWord.DocumentsInWhichTermWasCounted == 1
            ) );

        oCountedWords.Single(
            oCountedWord => (
                oCountedWord.Word == "there"
                &&
                oCountedWord.Count == 1
                &&
                oCountedWord.DocumentsInWhichTermWasCounted == 1
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

        m_oWordCounter.CountTermsInDocument(String.Empty);

        Assert.AreEqual(1, m_oWordCounter.TotalDocuments);
        Assert.AreEqual(0, m_oWordCounter.TotalWordsInDocuments);

        IEnumerable<CountedWord> oCountedWords = m_oWordCounter.CountedTerms;

        Assert.AreEqual( 0, oCountedWords.Count() );
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

        m_oWordCounter.CountTermsInDocument(
            "\r\nThis is\r\n a test the a this\r\nWords\r\n"
            );

        m_oWordCounter.CountTermsInDocument(
            "words this Test hello a there ");

        m_oWordCounter.CountTermsInDocument("\r\na the\r\n");

        Assert.AreEqual(3, m_oWordCounter.TotalDocuments);
        Assert.AreEqual(9, m_oWordCounter.TotalWordsInDocuments);

        IEnumerable<CountedWord> oCountedWords = m_oWordCounter.CountedTerms;

        Assert.AreEqual( 5, oCountedWords.Count() );

        oCountedWords.Single(
            oCountedWord => (
                oCountedWord.Word == "this"
                &&
                oCountedWord.Count == 3
                &&
                oCountedWord.DocumentsInWhichTermWasCounted == 2
            ) );

        oCountedWords.Single(
            oCountedWord => (
                oCountedWord.Word == "test"
                &&
                oCountedWord.Count == 2
                &&
                oCountedWord.DocumentsInWhichTermWasCounted == 2
            ) );

        oCountedWords.Single(
            oCountedWord => (
                oCountedWord.Word == "words"
                &&
                oCountedWord.Count == 2
                &&
                oCountedWord.DocumentsInWhichTermWasCounted == 2
            ) );

        oCountedWords.Single(
            oCountedWord => (
                oCountedWord.Word == "hello"
                &&
                oCountedWord.Count == 1
                &&
                oCountedWord.DocumentsInWhichTermWasCounted == 1
            ) );

        oCountedWords.Single(
            oCountedWord => (
                oCountedWord.Word == "there"
                &&
                oCountedWord.Count == 1
                &&
                oCountedWord.DocumentsInWhichTermWasCounted == 1
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
        // Vary case and order.

        m_oWordCounter.CountTermsInDocument("brown jumping fox");
        m_oWordCounter.CountTermsInDocument("FOX JUMPING BROWN");

        Assert.AreEqual(2, m_oWordCounter.TotalDocuments);
        Assert.AreEqual(6, m_oWordCounter.TotalWordsInDocuments);

        IEnumerable<CountedWord> oCountedWords =
            m_oWordCounter.CountedTerms;

        Assert.AreEqual( 3, oCountedWords.Count() );

        oCountedWords.Single(
            oCountedWord => (
                oCountedWord.Word == "brown"
                &&
                oCountedWord.Count == 2
                &&
                oCountedWord.DocumentsInWhichTermWasCounted == 2
            ) );

        oCountedWords.Single(
            oCountedWord => (
                oCountedWord.Word == "jumping"
                &&
                oCountedWord.Count == 2
                &&
                oCountedWord.DocumentsInWhichTermWasCounted == 2
            ) );

        oCountedWords.Single(
            oCountedWord => (
                oCountedWord.Word == "fox"
                &&
                oCountedWord.Count == 2
                &&
                oCountedWord.DocumentsInWhichTermWasCounted == 2
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

        m_oWordCounter.CountTermsInDocument(
            "http://abc.com?a=b brown jumping fox https://2.com\r\n"
            + "Fox Jumping http://3.net a the"
            );

        Assert.AreEqual(1, m_oWordCounter.TotalDocuments);
        Assert.AreEqual(5, m_oWordCounter.TotalWordsInDocuments);

        IEnumerable<CountedWord> oCountedWords = m_oWordCounter.CountedTerms;

        Assert.AreEqual( 3, oCountedWords.Count() );

        oCountedWords.Single(
            oCountedWord => (
                oCountedWord.Word == "brown"
                &&
                oCountedWord.Count == 1
                &&
                oCountedWord.DocumentsInWhichTermWasCounted == 1
            ) );

        oCountedWords.Single(
            oCountedWord => (
                oCountedWord.Word == "jumping"
                &&
                oCountedWord.Count == 2
                &&
                oCountedWord.DocumentsInWhichTermWasCounted == 1
            ) );

        oCountedWords.Single(
            oCountedWord => (
                oCountedWord.Word == "fox"
                &&
                oCountedWord.Count == 2
                &&
                oCountedWord.DocumentsInWhichTermWasCounted == 1
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

        m_oWordCounter.CountTermsInDocument(
            "brown jumping fox"
            + " Fox Jumping"
            + "! @ # $ % ^ & * ( ) - = + \\ | ~ [ { ] } ; : \" , < . > / ?"
            );

        Assert.AreEqual(1, m_oWordCounter.TotalDocuments);
        Assert.AreEqual(5, m_oWordCounter.TotalWordsInDocuments);

        IEnumerable<CountedWord> oCountedWords = m_oWordCounter.CountedTerms;

        Assert.AreEqual( 3, oCountedWords.Count() );

        oCountedWords.Single(
            oCountedWord => (
                oCountedWord.Word == "brown"
                &&
                oCountedWord.Count == 1
                &&
                oCountedWord.DocumentsInWhichTermWasCounted == 1
            ) );

        oCountedWords.Single(
            oCountedWord => (
                oCountedWord.Word == "jumping"
                &&
                oCountedWord.Count == 2
                &&
                oCountedWord.DocumentsInWhichTermWasCounted == 1
            ) );

        oCountedWords.Single(
            oCountedWord => (
                oCountedWord.Word == "fox"
                &&
                oCountedWord.Count == 2
                &&
                oCountedWord.DocumentsInWhichTermWasCounted == 1
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

        m_oWordCounter.CountTermsInDocument(
            "a'b_ c_d'"
            );

        Assert.AreEqual(1, m_oWordCounter.TotalDocuments);
        Assert.AreEqual(2, m_oWordCounter.TotalWordsInDocuments);

        IEnumerable<CountedWord> oCountedWords =
            m_oWordCounter.CountedTerms;

        Assert.AreEqual( 2, oCountedWords.Count() );

        oCountedWords.Single(
            oCountedWord => (
                oCountedWord.Word == "a'b_"
                &&
                oCountedWord.Count == 1
                &&
                oCountedWord.DocumentsInWhichTermWasCounted == 1
            ) );

        oCountedWords.Single(
            oCountedWord => (
                oCountedWord.Word == "c_d'"
                &&
                oCountedWord.Count == 1
                &&
                oCountedWord.DocumentsInWhichTermWasCounted == 1
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

        m_oWordCounter.CountTermsInDocument(
            "#chi2012 123 456.789 1"
            );

        Assert.AreEqual(1, m_oWordCounter.TotalDocuments);
        Assert.AreEqual(5, m_oWordCounter.TotalWordsInDocuments);

        IEnumerable<CountedWord> oCountedWords = m_oWordCounter.CountedTerms;

        Assert.AreEqual( 5, oCountedWords.Count() );

        oCountedWords.Single(
            oCountedWord => (
                oCountedWord.Word == "chi2012"
                &&
                oCountedWord.Count == 1
                &&
                oCountedWord.DocumentsInWhichTermWasCounted == 1
            ) );

        oCountedWords.Single(
            oCountedWord => (
                oCountedWord.Word == "123"
                &&
                oCountedWord.Count == 1
                &&
                oCountedWord.DocumentsInWhichTermWasCounted == 1
            ) );

        oCountedWords.Single(
            oCountedWord => (
                oCountedWord.Word == "456"
                &&
                oCountedWord.Count == 1
                &&
                oCountedWord.DocumentsInWhichTermWasCounted == 1
            ) );

        oCountedWords.Single(
            oCountedWord => (
                oCountedWord.Word == "789"
                &&
                oCountedWord.Count == 1
                &&
                oCountedWord.DocumentsInWhichTermWasCounted == 1
            ) );

        oCountedWords.Single(
            oCountedWord => (
                oCountedWord.Word == "1"
                &&
                oCountedWord.Count == 1
                &&
                oCountedWord.DocumentsInWhichTermWasCounted == 1
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

        m_oWordCounter.CountTermsInDocument(
            "hello 各ブースで見かけた美女を特集します"
            );

        Assert.AreEqual(1, m_oWordCounter.TotalDocuments);
        Assert.AreEqual(2, m_oWordCounter.TotalWordsInDocuments);

        IEnumerable<CountedWord> oCountedWords = m_oWordCounter.CountedTerms;

        Assert.AreEqual( 2, oCountedWords.Count() );

        oCountedWords.Single(
            oCountedWord => (
                oCountedWord.Word == "hello"
                &&
                oCountedWord.Count == 1
                &&
                oCountedWord.DocumentsInWhichTermWasCounted == 1
            ) );

        oCountedWords.Single(
            oCountedWord => (
                oCountedWord.Word == "各ブースで見かけた美女を特集します"
                &&
                oCountedWord.Count == 1
                &&
                oCountedWord.DocumentsInWhichTermWasCounted == 1
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

        m_oWordCounter.CountTermsInDocument("b c d");
        m_oWordCounter.CountTermsInDocument("e f g");
        m_oWordCounter.CountTermsInDocument("c b g f b c");

        Assert.AreEqual(3, m_oWordCounter.TotalDocuments);
        Assert.AreEqual(12, m_oWordCounter.TotalWordsInDocuments);

        IEnumerable<CountedWord> oCountedWords = m_oWordCounter.CountedTerms;

        // b 3 2
        // c 3 2
        // d 1 1
        // e 1 1
        // f 2 2
        // g 2 2

        Assert.AreEqual( 6, oCountedWords.Count() );

        oCountedWords.Single(
            oCountedWord => (
                oCountedWord.Word == "b"
                &&
                oCountedWord.Count == 3
                &&
                oCountedWord.DocumentsInWhichTermWasCounted == 2
            ) );

        oCountedWords.Single(
            oCountedWord => (
                oCountedWord.Word == "c"
                &&
                oCountedWord.Count == 3
                &&
                oCountedWord.DocumentsInWhichTermWasCounted == 2
            ) );

        oCountedWords.Single(
            oCountedWord => (
                oCountedWord.Word == "d"
                &&
                oCountedWord.Count == 1
                &&
                oCountedWord.DocumentsInWhichTermWasCounted == 1
            ) );

        oCountedWords.Single(
            oCountedWord => (
                oCountedWord.Word == "e"
                &&
                oCountedWord.Count == 1
                &&
                oCountedWord.DocumentsInWhichTermWasCounted == 1
            ) );

        oCountedWords.Single(
            oCountedWord => (
                oCountedWord.Word == "f"
                &&
                oCountedWord.Count == 2
                &&
                oCountedWord.DocumentsInWhichTermWasCounted == 2
            ) );

        oCountedWords.Single(
            oCountedWord => (
                oCountedWord.Word == "g"
                &&
                oCountedWord.Count == 2
                &&
                oCountedWord.DocumentsInWhichTermWasCounted == 2
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
        // Test Jana Diesner's example, which doesn't skip words.

        WordCounter oWordCounter = new WordCounter( new String[] {} );

        oWordCounter.CountTermsInDocument("Ben and Bob went out for dinner.");
        oWordCounter.CountTermsInDocument("Ben and Mary share a driveway.");
        oWordCounter.CountTermsInDocument("Brent and Bob went out for lunch."); 
        oWordCounter.CalculateSalienceOfCountedTerms();

        Assert.AreEqual(3, oWordCounter.TotalDocuments);
        Assert.AreEqual(20, oWordCounter.TotalWordsInDocuments);

        IEnumerable<CountedWord> oCountedWords = oWordCounter.CountedTerms;

        Assert.AreEqual( 13, oCountedWords.Count() );

        CountedWord oTheCountedWord = oCountedWords.Single(
            oCountedWord => (
                oCountedWord.Word == "ben"
                &&
                oCountedWord.Count == 2
                &&
                oCountedWord.DocumentsInWhichTermWasCounted == 2
            ) );

        Assert.AreEqual(0.018, oTheCountedWord.Salience, 0.001);

        oTheCountedWord = oCountedWords.Single(
            oCountedWord => (
                oCountedWord.Word == "and"
                &&
                oCountedWord.Count == 3
                &&
                oCountedWord.DocumentsInWhichTermWasCounted == 3
            ) );

        Assert.AreEqual(0.0, oTheCountedWord.Salience, 0.001);

        oTheCountedWord = oCountedWords.Single(
            oCountedWord => (
                oCountedWord.Word == "bob"
                &&
                oCountedWord.Count == 2
                &&
                oCountedWord.DocumentsInWhichTermWasCounted == 2
            ) );

        Assert.AreEqual(0.018, oTheCountedWord.Salience, 0.001);

        oTheCountedWord = oCountedWords.Single(
            oCountedWord => (
                oCountedWord.Word == "went"
                &&
                oCountedWord.Count == 2
                &&
                oCountedWord.DocumentsInWhichTermWasCounted == 2
            ) );

        Assert.AreEqual(0.018, oTheCountedWord.Salience, 0.001);

        oTheCountedWord = oCountedWords.Single(
            oCountedWord => (
                oCountedWord.Word == "out"
                &&
                oCountedWord.Count == 2
                &&
                oCountedWord.DocumentsInWhichTermWasCounted == 2
            ) );

        Assert.AreEqual(0.018, oTheCountedWord.Salience, 0.001);

        oTheCountedWord = oCountedWords.Single(
            oCountedWord => (
                oCountedWord.Word == "for"
                &&
                oCountedWord.Count == 2
                &&
                oCountedWord.DocumentsInWhichTermWasCounted == 2
            ) );

        Assert.AreEqual(0.018, oTheCountedWord.Salience, 0.001);

        oTheCountedWord = oCountedWords.Single(
            oCountedWord => (
                oCountedWord.Word == "dinner"
                &&
                oCountedWord.Count == 1
                &&
                oCountedWord.DocumentsInWhichTermWasCounted == 1
            ) );

        Assert.AreEqual(0.024, oTheCountedWord.Salience, 0.001);

        oTheCountedWord = oCountedWords.Single(
            oCountedWord => (
                oCountedWord.Word == "mary"
                &&
                oCountedWord.Count == 1
                &&
                oCountedWord.DocumentsInWhichTermWasCounted == 1
            ) );

        Assert.AreEqual(0.024, oTheCountedWord.Salience, 0.001);

        oTheCountedWord = oCountedWords.Single(
            oCountedWord => (
                oCountedWord.Word == "share"
                &&
                oCountedWord.Count == 1
                &&
                oCountedWord.DocumentsInWhichTermWasCounted == 1
            ) );

        Assert.AreEqual(0.024, oTheCountedWord.Salience, 0.001);

        oTheCountedWord = oCountedWords.Single(
            oCountedWord => (
                oCountedWord.Word == "a"
                &&
                oCountedWord.Count == 1
                &&
                oCountedWord.DocumentsInWhichTermWasCounted == 1
            ) );

        Assert.AreEqual(0.024, oTheCountedWord.Salience, 0.001);

        oTheCountedWord = oCountedWords.Single(
            oCountedWord => (
                oCountedWord.Word == "driveway"
                &&
                oCountedWord.Count == 1
                &&
                oCountedWord.DocumentsInWhichTermWasCounted == 1
            ) );

        Assert.AreEqual(0.024, oTheCountedWord.Salience, 0.001);

        oTheCountedWord = oCountedWords.Single(
            oCountedWord => (
                oCountedWord.Word == "brent"
                &&
                oCountedWord.Count == 1
                &&
                oCountedWord.DocumentsInWhichTermWasCounted == 1
            ) );

        Assert.AreEqual(0.024, oTheCountedWord.Salience, 0.001);

        oTheCountedWord = oCountedWords.Single(
            oCountedWord => (
                oCountedWord.Word == "lunch"
                &&
                oCountedWord.Count == 1
                &&
                oCountedWord.DocumentsInWhichTermWasCounted == 1
            ) );

        Assert.AreEqual(0.024, oTheCountedWord.Salience, 0.001);
    }

    //*************************************************************************
    //  Method: TestCountTermsInDocument12()
    //
    /// <summary>
    /// Tests CountTermsInDocument() and CountedTerms.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestCountTermsInDocument12()
    {
        // Make sure URLs are not removed.

        m_oWordCounter.SkipUrlsAndPunctuation = false;

        m_oWordCounter.CountTermsInDocument(
            "http://abc.com?a=b https://2.com brown fox\r\n"
            + "http://3.net http://3.net  brown  fox"
            );

        Assert.AreEqual(1, m_oWordCounter.TotalDocuments);
        Assert.AreEqual(8, m_oWordCounter.TotalWordsInDocuments);

        IEnumerable<CountedWord> oCountedWords = m_oWordCounter.CountedTerms;

        Assert.AreEqual( 5, oCountedWords.Count() );

        oCountedWords.Single(
            oCountedWord => (
                oCountedWord.Word == "http://abc.com?a=b"
                &&
                oCountedWord.Count == 1
                &&
                oCountedWord.DocumentsInWhichTermWasCounted == 1
            ) );

        oCountedWords.Single(
            oCountedWord => (
                oCountedWord.Word == "https://2.com"
                &&
                oCountedWord.Count == 1
                &&
                oCountedWord.DocumentsInWhichTermWasCounted == 1
            ) );


        oCountedWords.Single(
            oCountedWord => (
                oCountedWord.Word == "brown"
                &&
                oCountedWord.Count == 2
                &&
                oCountedWord.DocumentsInWhichTermWasCounted == 1
            ) );

        oCountedWords.Single(
            oCountedWord => (
                oCountedWord.Word == "fox"
                &&
                oCountedWord.Count == 2
                &&
                oCountedWord.DocumentsInWhichTermWasCounted == 1
            ) );

        oCountedWords.Single(
            oCountedWord => (
                oCountedWord.Word == "http://3.net"
                &&
                oCountedWord.Count == 2
                &&
                oCountedWord.DocumentsInWhichTermWasCounted == 1
            ) );

    }

    //*************************************************************************
    //  Method: TestCountTermsInDocument13()
    //
    /// <summary>
    /// Tests CountTermsInDocument() and CountedTerms.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestCountTermsInDocument13()
    {
        // Don't convert to lower case.

        WordCounter oWordCounterForThisTest =
            new WordCounter(false, WordsToSkip);

        oWordCounterForThisTest.CountTermsInDocument("the brown jumping fox");
        oWordCounterForThisTest.CountTermsInDocument("FOX JUMPING BROWN THE");

        Assert.AreEqual(2, oWordCounterForThisTest.TotalDocuments);
        Assert.AreEqual(7, oWordCounterForThisTest.TotalWordsInDocuments);

        IEnumerable<CountedWord> oCountedWords =
            oWordCounterForThisTest.CountedTerms;

        Assert.AreEqual( 7, oCountedWords.Count() );

        oCountedWords.Single(
            oCountedWord => (
                oCountedWord.Word == "brown"
                &&
                oCountedWord.Count == 1
                &&
                oCountedWord.DocumentsInWhichTermWasCounted == 1
            ) );

        oCountedWords.Single(
            oCountedWord => (
                oCountedWord.Word == "jumping"
                &&
                oCountedWord.Count == 1
                &&
                oCountedWord.DocumentsInWhichTermWasCounted == 1
            ) );

        oCountedWords.Single(
            oCountedWord => (
                oCountedWord.Word == "fox"
                &&
                oCountedWord.Count == 1
                &&
                oCountedWord.DocumentsInWhichTermWasCounted == 1
            ) );

        oCountedWords.Single(
            oCountedWord => (
                oCountedWord.Word == "BROWN"
                &&
                oCountedWord.Count == 1
                &&
                oCountedWord.DocumentsInWhichTermWasCounted == 1
            ) );

        oCountedWords.Single(
            oCountedWord => (
                oCountedWord.Word == "JUMPING"
                &&
                oCountedWord.Count == 1
                &&
                oCountedWord.DocumentsInWhichTermWasCounted == 1
            ) );

        oCountedWords.Single(
            oCountedWord => (
                oCountedWord.Word == "FOX"
                &&
                oCountedWord.Count == 1
                &&
                oCountedWord.DocumentsInWhichTermWasCounted == 1
            ) );

        oCountedWords.Single(
            oCountedWord => (
                oCountedWord.Word == "THE"
                &&
                oCountedWord.Count == 1
                &&
                oCountedWord.DocumentsInWhichTermWasCounted == 1
            ) );
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
        m_oWordCounter.CountTermsInDocument("this is a test");
        m_oWordCounter.Clear();

        Assert.AreEqual( 0, m_oWordCounter.CountedTerms.Count() );
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

    protected WordCounter m_oWordCounter;
}

}
