
using System;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Smrf.AppLib;

namespace Smrf.Common.UnitTests
{
//*****************************************************************************
//  Class: StringUtilTest
//
/// <summary>
/// This is a test fixture for the StringUtil class.
/// </summary>
//*****************************************************************************

[TestClassAttribute]

public class StringUtilTest : Object
{
    //*************************************************************************
    //  Constructor: StringUtilTest()
    //
    /// <summary>
    /// Initializes a new instance of the StringUtilTest class.
    /// </summary>
    //*************************************************************************

    public StringUtilTest()
    {
        // (Do nothing.)
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
        // (Do nothing.)
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
        // (Do nothing.)
    }

    //*************************************************************************
    //  Method: TestBreakIntoLines()
    //
    /// <summary>
    /// Tests the BreakIntoLines() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestBreakIntoLines()
    {
        // Empty string.

        Assert.AreEqual( String.Empty,
            StringUtil.BreakIntoLines(String.Empty, 10) );
    }

    //*************************************************************************
    //  Method: TestBreakIntoLines2()
    //
    /// <summary>
    /// Tests the BreakIntoLines() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestBreakIntoLines2()
    {
        // Typical case.

        const String StringToBreak =
            "Flooding will force the evacuation of one-quarter of North Dakota's fourth largest city and levee breaches forced 300 residents from a Missouri town as flooding worsened on Tuesday, officials said."
            ;

        const String BrokenString =
            "Flooding will\n"
            + "force the evacuation\n"
            + "of one-quarter\n"
            + "of North Dakota's\n"
            + "fourth largest\n"
            + "city and levee\n"
            + "breaches forced\n"
            + "300 residents\n"
            + "from a Missouri\n"
            + "town as flooding\n"
            + "worsened on\n"
            + "Tuesday, officials\n"
            + "said."
            ;

        Assert.AreEqual( BrokenString,
            StringUtil.BreakIntoLines(StringToBreak, 10) );
    }

    //*************************************************************************
    //  Method: TestTruncateWithEllipses()
    //
    /// <summary>
    /// Tests the TruncateWithEllipses() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestTruncateWithEllipses()
    {
        Assert.AreEqual( "abc",
            StringUtil.TruncateWithEllipses("abc", 3) );
    }

    //*************************************************************************
    //  Method: TestTruncateWithEllipses2()
    //
    /// <summary>
    /// Tests the TruncateWithEllipses() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestTruncateWithEllipses2()
    {
        Assert.AreEqual( "...",
            StringUtil.TruncateWithEllipses("abcd", 3) );
    }

    //*************************************************************************
    //  Method: TestTruncateWithEllipses3()
    //
    /// <summary>
    /// Tests the TruncateWithEllipses() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestTruncateWithEllipses3()
    {
        Assert.AreEqual( "",
            StringUtil.TruncateWithEllipses("", 4) );
    }

    //*************************************************************************
    //  Method: TestTruncateWithEllipses4()
    //
    /// <summary>
    /// Tests the TruncateWithEllipses() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestTruncateWithEllipses4()
    {
        Assert.AreEqual( "a",
            StringUtil.TruncateWithEllipses("a", 4) );
    }

    //*************************************************************************
    //  Method: TestTruncateWithEllipses5()
    //
    /// <summary>
    /// Tests the TruncateWithEllipses() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestTruncateWithEllipses5()
    {
        Assert.AreEqual( "abcd",
            StringUtil.TruncateWithEllipses("abcd", 4) );
    }

    //*************************************************************************
    //  Method: TestTruncateWithEllipses6()
    //
    /// <summary>
    /// Tests the TruncateWithEllipses() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestTruncateWithEllipses6()
    {
        Assert.AreEqual( "a...",
            StringUtil.TruncateWithEllipses("abcde", 4) );
    }

    //*************************************************************************
    //  Method: TestAppendAfterEmptyLine()
    //
    /// <summary>
    /// Tests the AppendAfterEmptyLine() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestAppendAfterEmptyLine()
    {
        // Empty StringBuilder, null string.

        StringBuilder oStringBuilder = new StringBuilder();

        Assert.IsFalse( StringUtil.AppendAfterEmptyLine(
            oStringBuilder, null) );

        Assert.AreEqual(0, oStringBuilder.Length);
    }

    //*************************************************************************
    //  Method: TestAppendAfterEmptyLine2()
    //
    /// <summary>
    /// Tests the AppendAfterEmptyLine() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestAppendAfterEmptyLine2()
    {
        // Empty StringBuilder, empty string.

        StringBuilder oStringBuilder = new StringBuilder();

        Assert.IsFalse( StringUtil.AppendAfterEmptyLine(
            oStringBuilder, String.Empty) );

        Assert.AreEqual(0, oStringBuilder.Length);
    }

    //*************************************************************************
    //  Method: TestAppendAfterEmptyLine3()
    //
    /// <summary>
    /// Tests the AppendAfterEmptyLine() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestAppendAfterEmptyLine3()
    {
        // Empty StringBuilder, non-empty string.

        StringBuilder oStringBuilder = new StringBuilder();

        Assert.IsTrue( StringUtil.AppendAfterEmptyLine(
            oStringBuilder, "abc") );

        Assert.AreEqual( "abc", oStringBuilder.ToString() );
    }

    //*************************************************************************
    //  Method: TestAppendAfterEmptyLine4()
    //
    /// <summary>
    /// Tests the AppendAfterEmptyLine() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestAppendAfterEmptyLine4()
    {
        // Non-empty StringBuilder, null string.

        StringBuilder oStringBuilder = new StringBuilder();
        oStringBuilder.Append("123");

        Assert.IsFalse( StringUtil.AppendAfterEmptyLine(
            oStringBuilder, null) );

        Assert.AreEqual( "123", oStringBuilder.ToString() );
    }

    //*************************************************************************
    //  Method: TestAppendAfterEmptyLine5()
    //
    /// <summary>
    /// Tests the AppendAfterEmptyLine() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestAppendAfterEmptyLine5()
    {
        // Non-empty StringBuilder, empty string.

        StringBuilder oStringBuilder = new StringBuilder();
        oStringBuilder.Append("123");

        Assert.IsFalse( StringUtil.AppendAfterEmptyLine(
            oStringBuilder, String.Empty) );

        Assert.AreEqual( "123", oStringBuilder.ToString() );
    }

    //*************************************************************************
    //  Method: TestAppendAfterEmptyLine6()
    //
    /// <summary>
    /// Tests the AppendAfterEmptyLine() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestAppendAfterEmptyLine6()
    {
        // Non-empty StringBuilder, non-empty string.

        StringBuilder oStringBuilder = new StringBuilder();
        oStringBuilder.Append("123");

        Assert.IsTrue( StringUtil.AppendAfterEmptyLine(
            oStringBuilder, "abc") );

        Assert.AreEqual( "123\r\nabc", oStringBuilder.ToString() );
    }

    //*************************************************************************
    //  Method: TestAppendLineAfterEmptyLine()
    //
    /// <summary>
    /// Tests the AppendLineAfterEmptyLine() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestAppendLineAfterEmptyLine()
    {
        // Empty StringBuilder, null string.

        StringBuilder oStringBuilder = new StringBuilder();

        Assert.IsFalse( StringUtil.AppendLineAfterEmptyLine(
            oStringBuilder, null) );

        Assert.AreEqual(0, oStringBuilder.Length);
    }

    //*************************************************************************
    //  Method: TestAppendLineAfterEmptyLine2()
    //
    /// <summary>
    /// Tests the AppendLineAfterEmptyLine() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestAppendLineAfterEmptyLine2()
    {
        // Empty StringBuilder, empty string.

        StringBuilder oStringBuilder = new StringBuilder();

        Assert.IsFalse( StringUtil.AppendLineAfterEmptyLine(
            oStringBuilder, String.Empty) );

        Assert.AreEqual(0, oStringBuilder.Length);
    }

    //*************************************************************************
    //  Method: TestAppendLineAfterEmptyLine3()
    //
    /// <summary>
    /// Tests the AppendLineAfterEmptyLine() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestAppendLineAfterEmptyLine3()
    {
        // Empty StringBuilder, non-empty string.

        StringBuilder oStringBuilder = new StringBuilder();

        Assert.IsTrue( StringUtil.AppendLineAfterEmptyLine(
            oStringBuilder, "abc") );

        Assert.AreEqual( "abc\r\n", oStringBuilder.ToString() );
    }

    //*************************************************************************
    //  Method: TestAppendLineAfterEmptyLine4()
    //
    /// <summary>
    /// Tests the AppendLineAfterEmptyLine() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestAppendLineAfterEmptyLine4()
    {
        // Non-empty StringBuilder, null string.

        StringBuilder oStringBuilder = new StringBuilder();
        oStringBuilder.Append("123");

        Assert.IsFalse( StringUtil.AppendLineAfterEmptyLine(
            oStringBuilder, null) );

        Assert.AreEqual( "123", oStringBuilder.ToString() );
    }

    //*************************************************************************
    //  Method: TestAppendLineAfterEmptyLine5()
    //
    /// <summary>
    /// Tests the AppendLineAfterEmptyLine() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestAppendLineAfterEmptyLine5()
    {
        // Non-empty StringBuilder, empty string.

        StringBuilder oStringBuilder = new StringBuilder();
        oStringBuilder.Append("123");

        Assert.IsFalse( StringUtil.AppendLineAfterEmptyLine(
            oStringBuilder, String.Empty) );

        Assert.AreEqual( "123", oStringBuilder.ToString() );
    }

    //*************************************************************************
    //  Method: TestAppendLineAfterEmptyLine6()
    //
    /// <summary>
    /// Tests the AppendLineAfterEmptyLine() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestAppendLineAfterEmptyLine6()
    {
        // Non-empty StringBuilder, non-empty string.

        StringBuilder oStringBuilder = new StringBuilder();
        oStringBuilder.Append("123");

        Assert.IsTrue( StringUtil.AppendLineAfterEmptyLine(
            oStringBuilder, "abc") );

        Assert.AreEqual( "123\r\nabc\r\n", oStringBuilder.ToString() );
    }

    //*************************************************************************
    //  Method: TestAppendSectionSeparator()
    //
    /// <summary>
    /// Tests the AppendSectionSeparator() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestAppendSectionSeparator()
    {
        // Empty StringBuilder.

        StringBuilder oStringBuilder = new StringBuilder();
        StringUtil.AppendSectionSeparator(oStringBuilder);

        Assert.AreEqual(0, oStringBuilder.Length);
    }

    //*************************************************************************
    //  Method: TestAppendSectionSeparator2()
    //
    /// <summary>
    /// Tests the AppendSectionSeparator() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestAppendSectionSeparator2()
    {
        // Non-empty StringBuilder.

        StringBuilder oStringBuilder = new StringBuilder();
        oStringBuilder.Append("abc");
        StringUtil.AppendSectionSeparator(oStringBuilder);

        Assert.AreEqual( "abc\r\n\r\n", oStringBuilder.ToString() );
    }

    //*************************************************************************
    //  Method: TestIsAscii()
    //
    /// <summary>
    /// Tests the IsAscii() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestIsAscii()
    {
        // Empty string.

        Assert.IsTrue( StringUtil.IsAscii(String.Empty) );
    }

    //*************************************************************************
    //  Method: TestIsAscii2()
    //
    /// <summary>
    /// Tests the IsAscii() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestIsAscii2()
    {
        // All ASCII.

        const String StringToTest =
            "abcABC123!@#$%^&*()-_=+\\|`~[{]};:'\",<.>/?"
            ;

        Assert.IsTrue( StringUtil.IsAscii(StringToTest) );
    }

    //*************************************************************************
    //  Method: TestIsAscii3()
    //
    /// <summary>
    /// Tests the IsAscii() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestIsAscii3()
    {
        // All Korean.

        const String StringToTest =
            "자살"
            ;

        Assert.IsFalse( StringUtil.IsAscii(StringToTest) );
    }

    //*************************************************************************
    //  Method: TestIsAscii4()
    //
    /// <summary>
    /// Tests the IsAscii() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestIsAscii4()
    {
        // Mixed.

        const String StringToTest =
            "abc자살def"
            ;

        Assert.IsFalse( StringUtil.IsAscii(StringToTest) );
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}

}
