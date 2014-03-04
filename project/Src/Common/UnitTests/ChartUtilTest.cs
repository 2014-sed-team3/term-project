using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Smrf.ChartLib;

namespace Smrf.Common.UnitTests
{
//*****************************************************************************
//  Class: ChartUtilTest
//
/// <summary>
/// This is a test fixture for the ChartUtil class.
/// </summary>
//*****************************************************************************

[TestClassAttribute]

public class ChartUtilTest : Object
{
    //*************************************************************************
    //  Constructor: ChartUtilTest()
    //
    /// <summary>
    /// Initializes a new instance of the ChartUtilTest class.
    /// </summary>
    //*************************************************************************

    public ChartUtilTest()
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
    //  Method: TestGetLogAxisGridlineValues()
    //
    /// <summary>
    /// Tests the GetLogAxisGridlineValues method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGetLogAxisGridlineValues()
    {
        Double [] adGridlineValues =
            ChartUtil.GetLogAxisGridlineValues(0.3, 103.0);

        Assert.AreEqual(5, adGridlineValues.Length);
        Assert.AreEqual( 0.1, adGridlineValues[0] );
        Assert.AreEqual( 1.0, adGridlineValues[1] );
        Assert.AreEqual( 10.0, adGridlineValues[2] );
        Assert.AreEqual( 100.0, adGridlineValues[3] );
        Assert.AreEqual( 1000.0, adGridlineValues[4] );
    }

    //*************************************************************************
    //  Method: TestGetLogAxisGridlineValues2()
    //
    /// <summary>
    /// Tests the GetLogAxisGridlineValues method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGetLogAxisGridlineValues2()
    {
        Double [] adGridlineValues =
            ChartUtil.GetLogAxisGridlineValues(1.0, 1.0);

        Assert.AreEqual(2, adGridlineValues.Length);
        Assert.AreEqual( 1.0, adGridlineValues[0] );
        Assert.AreEqual( 10.0, adGridlineValues[1] );
    }

    //*************************************************************************
    //  Method: TestGetLogAxisGridlineValues3()
    //
    /// <summary>
    /// Tests the GetLogAxisGridlineValues method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGetLogAxisGridlineValues3()
    {
        Double [] adGridlineValues =
            ChartUtil.GetLogAxisGridlineValues(0.1, 0.1);

        Assert.AreEqual(2, adGridlineValues.Length);
        Assert.AreEqual( 0.1, adGridlineValues[0] );
        Assert.AreEqual( 1.0, adGridlineValues[1] );
    }

    //*************************************************************************
    //  Method: TestGetLogAxisGridlineValues4()
    //
    /// <summary>
    /// Tests the GetLogAxisGridlineValues method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGetLogAxisGridlineValues4()
    {
        Double [] adGridlineValues =
            ChartUtil.GetLogAxisGridlineValues(0.3, 0.3);

        Assert.AreEqual(2, adGridlineValues.Length);
        Assert.AreEqual( 0.1, adGridlineValues[0] );
        Assert.AreEqual( 1.0, adGridlineValues[1] );
    }

    //*************************************************************************
    //  Method: TestGetLogAxisGridlineValues5()
    //
    /// <summary>
    /// Tests the GetLogAxisGridlineValues method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGetLogAxisGridlineValues5()
    {
        Double [] adGridlineValues =
            ChartUtil.GetLogAxisGridlineValues(0.119, 138495.24);

        Assert.AreEqual(8, adGridlineValues.Length);
        Assert.AreEqual( 0.1, adGridlineValues[0] );
        Assert.AreEqual( 1.0, adGridlineValues[1] );
        Assert.AreEqual( 10.0, adGridlineValues[2] );
        Assert.AreEqual( 100.0, adGridlineValues[3] );
        Assert.AreEqual( 1000.0, adGridlineValues[4] );
        Assert.AreEqual( 10000.0, adGridlineValues[5] );
        Assert.AreEqual( 100000.0, adGridlineValues[6] );
        Assert.AreEqual( 1000000.0, adGridlineValues[7] );
    }

    //*************************************************************************
    //  Method: TestGetLogAxisGridlineValues6()
    //
    /// <summary>
    /// Tests the GetLogAxisGridlineValues method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGetLogAxisGridlineValues6()
    {
        // Backward values.

        Double [] adGridlineValues =
            ChartUtil.GetLogAxisGridlineValues(103.0, 0.3);

        Assert.AreEqual(5, adGridlineValues.Length);
        Assert.AreEqual( 0.1, adGridlineValues[0] );
        Assert.AreEqual( 1.0, adGridlineValues[1] );
        Assert.AreEqual( 10.0, adGridlineValues[2] );
        Assert.AreEqual( 100.0, adGridlineValues[3] );
        Assert.AreEqual( 1000.0, adGridlineValues[4] );
    }

    //*************************************************************************
    //  Method: TestGetLogAxisGridlineValuesBad()
    //
    /// <summary>
    /// Tests the constructor.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ExpectedException(typeof(InvalidOperationException))]

    public void
    TestGetLogAxisGridlineValuesBad()
    {
        try
        {
            Double [] adGridlineValues =
                ChartUtil.GetLogAxisGridlineValues(0.1, -1.0);
        }
        catch (InvalidOperationException oInvalidOperationException)
        {
            Assert.AreEqual(

                "ChartUtil.GetLogAxisGridlineValues: limit2 value must be"
                + " > 0 when using log scaling."
                ,
                oInvalidOperationException.Message
                );

            throw;
        }
    }

    //*************************************************************************
    //  Method: TestGetLogAxisGridlineValuesBad2()
    //
    /// <summary>
    /// Tests the constructor.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ExpectedException(typeof(InvalidOperationException))]

    public void
    TestGetLogAxisGridlineValuesBad2()
    {
        try
        {
            Double [] adGridlineValues =
                ChartUtil.GetLogAxisGridlineValues(0.1, 0.0);
        }
        catch (InvalidOperationException oInvalidOperationException)
        {
            Assert.AreEqual(

                "ChartUtil.GetLogAxisGridlineValues: limit2 value must be"
                + " > 0 when using log scaling."
                ,
                oInvalidOperationException.Message
                );

            throw;
        }
    }

    //*************************************************************************
    //  Method: TestGetLogAxisGridlineValuesBad3()
    //
    /// <summary>
    /// Tests the constructor.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ExpectedException(typeof(InvalidOperationException))]

    public void
    TestGetLogAxisGridlineValuesBad3()
    {
        try
        {
            Double[] adGridlineValues =
                ChartUtil.GetLogAxisGridlineValues(-0.1, 1.0);
        }
        catch (InvalidOperationException oInvalidOperationException)
        {
            Assert.AreEqual(

                "ChartUtil.GetLogAxisGridlineValues: limit1 value must be"
                + " > 0 when using log scaling."
                ,
                oInvalidOperationException.Message
                );

            throw;
        }
    }

    //*************************************************************************
    //  Method: TestGetLogAxisGridlineValuesBad4()
    //
    /// <summary>
    /// Tests the constructor.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]
    [ExpectedException(typeof(InvalidOperationException))]

    public void
    TestGetLogAxisGridlineValuesBad4()
    {
        try
        {
            Double [] adGridlineValues =
                ChartUtil.GetLogAxisGridlineValues(0, 1.0);
        }
        catch (InvalidOperationException oInvalidOperationException)
        {
            Assert.AreEqual(

                "ChartUtil.GetLogAxisGridlineValues: limit1 value must be"
                + " > 0 when using log scaling."
                ,
                oInvalidOperationException.Message
                );

            throw;
        }
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}

}
