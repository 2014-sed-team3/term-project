using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Smrf.DateTimeLib;

namespace Smrf.Common.UnitTests
{
//*****************************************************************************
//  Class: DateTimeUtil2Test
//
/// <summary>
/// This is a test fixture for the DateTimeUtil2 class.
/// </summary>
//*****************************************************************************

[TestClassAttribute]

public class DateTimeUtil2Test : Object
{
    //*************************************************************************
    //  Constructor: DateTimeUtil2Test()
    //
    /// <summary>
    /// Initializes a new instance of the DateTimeUtil2Test class.
    /// </summary>
    //*************************************************************************

    public DateTimeUtil2Test()
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
    //  Method: TestFormatDuration()
    //
    /// <summary>
    /// Tests the FormatDuration method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestFormatDuration()
    {
        TimeSpan oDuration = new TimeSpan(0);

        Assert.AreEqual("0-minute",
            DateTimeUtil2.FormatDuration(oDuration) );
    }

    //*************************************************************************
    //  Method: TestFormatDuration2()
    //
    /// <summary>
    /// Tests the FormatDuration method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestFormatDuration2()
    {
        // Hours, minutes, seconds.
        TimeSpan oDuration = new TimeSpan(0, 0, 1);

        Assert.AreEqual("0-minute",
            DateTimeUtil2.FormatDuration(oDuration) );
    }

    //*************************************************************************
    //  Method: TestFormatDuration3()
    //
    /// <summary>
    /// Tests the FormatDuration method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestFormatDuration3()
    {
        // Hours, minutes, seconds.
        TimeSpan oDuration = new TimeSpan(0, 2, 0);

        Assert.AreEqual("2-minute",
            DateTimeUtil2.FormatDuration(oDuration) );
    }

    //*************************************************************************
    //  Method: TestFormatDuration4()
    //
    /// <summary>
    /// Tests the FormatDuration method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestFormatDuration4()
    {
        // Hours, minutes, seconds.
        TimeSpan oDuration = new TimeSpan(0, 59, 0);

        Assert.AreEqual("59-minute",
            DateTimeUtil2.FormatDuration(oDuration) );
    }

    //*************************************************************************
    //  Method: TestFormatDuration5()
    //
    /// <summary>
    /// Tests the FormatDuration method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestFormatDuration5()
    {
        // Hours, minutes, seconds.
        TimeSpan oDuration = new TimeSpan(1, 0, 0);

        Assert.AreEqual("1-hour, 0-minute",
            DateTimeUtil2.FormatDuration(oDuration) );
    }

    //*************************************************************************
    //  Method: TestFormatDuration6()
    //
    /// <summary>
    /// Tests the FormatDuration method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestFormatDuration6()
    {
        // Hours, minutes, seconds.
        TimeSpan oDuration = new TimeSpan(23, 43, 59);

        Assert.AreEqual("23-hour, 43-minute",
            DateTimeUtil2.FormatDuration(oDuration) );
    }

    //*************************************************************************
    //  Method: TestFormatDuration7()
    //
    /// <summary>
    /// Tests the FormatDuration method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestFormatDuration7()
    {
        // Hours, minutes, seconds.
        TimeSpan oDuration = new TimeSpan(24, 0, 0);

        Assert.AreEqual("1-day, 0-hour, 0-minute",
            DateTimeUtil2.FormatDuration(oDuration) );
    }

    //*************************************************************************
    //  Method: TestFormatDuration8()
    //
    /// <summary>
    /// Tests the FormatDuration method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestFormatDuration8()
    {
        // Hours, minutes, seconds.
        TimeSpan oDuration = new TimeSpan(24, 12, 59);

        Assert.AreEqual("1-day, 0-hour, 12-minute",
            DateTimeUtil2.FormatDuration(oDuration) );
    }

    //*************************************************************************
    //  Method: TestFormatDuration9()
    //
    /// <summary>
    /// Tests the FormatDuration method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestFormatDuration9()
    {
        // Hours, minutes, seconds.
        TimeSpan oDuration = new TimeSpan(24, 59, 59);

        Assert.AreEqual("1-day, 0-hour, 59-minute",
            DateTimeUtil2.FormatDuration(oDuration) );
    }

    //*************************************************************************
    //  Method: TestFormatDuration10()
    //
    /// <summary>
    /// Tests the FormatDuration method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestFormatDuration10()
    {
        // Hours, minutes, seconds.
        TimeSpan oDuration = new TimeSpan(25, 0, 0);

        Assert.AreEqual("1-day, 1-hour, 0-minute",
            DateTimeUtil2.FormatDuration(oDuration) );
    }

    //*************************************************************************
    //  Method: TestFormatDuration11()
    //
    /// <summary>
    /// Tests the FormatDuration method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestFormatDuration11()
    {
        // Hours, minutes, seconds.
        TimeSpan oDuration = new TimeSpan(25, 59, 48);

        Assert.AreEqual("1-day, 1-hour, 59-minute",
            DateTimeUtil2.FormatDuration(oDuration) );
    }

    //*************************************************************************
    //  Method: TestFormatDuration12()
    //
    /// <summary>
    /// Tests the FormatDuration method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestFormatDuration12()
    {
        // Hours, minutes, seconds.
        TimeSpan oDuration = new TimeSpan(26, 0, 0);

        Assert.AreEqual("1-day, 2-hour, 0-minute",
            DateTimeUtil2.FormatDuration(oDuration) );
    }

    //*************************************************************************
    //  Method: TestFormatDuration13()
    //
    /// <summary>
    /// Tests the FormatDuration method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestFormatDuration13()
    {
        // Hours, minutes, seconds.
        TimeSpan oDuration = new TimeSpan(26, 12, 14);

        Assert.AreEqual("1-day, 2-hour, 12-minute",
            DateTimeUtil2.FormatDuration(oDuration) );
    }

    //*************************************************************************
    //  Method: TestFormatDuration14()
    //
    /// <summary>
    /// Tests the FormatDuration method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestFormatDuration14()
    {
        // Hours, minutes, seconds.
        TimeSpan oDuration = new TimeSpan(123, 19, 44);

        Assert.AreEqual("5-day, 3-hour, 19-minute",
            DateTimeUtil2.FormatDuration(oDuration) );
    }

    //*************************************************************************
    //  Method: TestFormatDurationBad()
    //
    /// <summary>
    /// Tests the FormatDuration method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ ExpectedException( typeof(ArgumentException) ) ]

    public void
    TestFormatDurationBad()
    {
        // Negative duration.

        // Hours, minutes, seconds.
        TimeSpan oDuration = new TimeSpan(-123, -19, -44);

        DateTimeUtil2.FormatDuration(oDuration);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}

}
