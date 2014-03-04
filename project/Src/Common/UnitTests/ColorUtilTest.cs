
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;
using Smrf.GraphicsLib;

namespace Smrf.Common.UnitTests
{
//*****************************************************************************
//  Class: ColorUtilTest
//
/// <summary>
/// This is a test fixture for the ColorUtil class.
/// </summary>
//*****************************************************************************

[TestClassAttribute]

public class ColorUtilTest : Object
{
    //*************************************************************************
    //  Constructor: ColorUtilTest()
    //
    /// <summary>
    /// Initializes a new instance of the ColorUtilTest class.
    /// </summary>
    //*************************************************************************

    public ColorUtilTest()
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
    //  Method: TestTryConvertFromInvariantString()
    //
    /// <summary>
    /// Tests the TryConvertFromInvariantString() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestTryConvertFromInvariantString()
    {
        Color color;

        foreach (String sColor in new String [] {
            "alice blue",
            "aliceblue",
            "aLice blUe",
            "ALICEBLUE",
            } )
        {
            Assert.IsTrue( ColorUtil.TryConvertFromInvariantString(
                sColor, new ColorConverter(), out color) );

            Assert.AreEqual(Color.AliceBlue, color);
        }
    }

    //*************************************************************************
    //  Method: TestTryConvertFromInvariantString2()
    //
    /// <summary>
    /// Tests the TryConvertFromInvariantString() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestTryConvertFromInvariantString2()
    {
        Color color;

        foreach (String sColor in new String [] {
            "blue",
            "blue",
            "blUe",
            "BLUE",
            } )
        {
            Assert.IsTrue( ColorUtil.TryConvertFromInvariantString(
                sColor, new ColorConverter(), out color) );

            Assert.AreEqual(Color.Blue, color);
        }
    }

    //*************************************************************************
    //  Method: TestTryConvertFromInvariantString3()
    //
    /// <summary>
    /// Tests the TryConvertFromInvariantString() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestTryConvertFromInvariantString3()
    {
        String sColor = "0, 100, 255";
        Color color;

        Assert.IsTrue( ColorUtil.TryConvertFromInvariantString(
            sColor, new ColorConverter(), out color) );

        Assert.AreEqual(Color.FromArgb(0, 100, 255), color);
    }

    //*************************************************************************
    //  Method: TestTryConvertFromInvariantString4()
    //
    /// <summary>
    /// Tests the TryConvertFromInvariantString() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestTryConvertFromInvariantString4()
    {
        String sColor = "0,100,255";
        Color color;

        Assert.IsTrue( ColorUtil.TryConvertFromInvariantString(
            sColor, new ColorConverter(), out color) );

        Assert.AreEqual(Color.FromArgb(0, 100, 255), color);
    }

    //*************************************************************************
    //  Method: TestTryConvertFromInvariantString5()
    //
    /// <summary>
    /// Tests the TryConvertFromInvariantString() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestTryConvertFromInvariantString5()
    {
        Color color;

        foreach (String sColor in new String [] {
            "",
            "Xred",
            "x",
            "greenX",
            } )
        {
            Assert.IsFalse( ColorUtil.TryConvertFromInvariantString(
                sColor, new ColorConverter(), out color) );
        }
    }

    //*************************************************************************
    //  Method: TestToHtmlString()
    //
    /// <summary>
    /// Tests the ToHtmlString method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestToHtmlString()
    {
        Assert.AreEqual("#0064ff",
            ColorUtil.ToHtmlString( Color.FromArgb(0, 100, 255) ) );
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}

}
