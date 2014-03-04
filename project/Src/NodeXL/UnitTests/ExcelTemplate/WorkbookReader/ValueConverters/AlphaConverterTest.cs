﻿
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Smrf.NodeXL.ExcelTemplate;

namespace Smrf.NodeXL.UnitTests
{
//*****************************************************************************
//  Class: AlphaConverterTest
//
/// <summary>
/// This is a Visual Studio test fixture for the <see cref="AlphaConverter" />
/// class.
/// </summary>
//*****************************************************************************

[TestClassAttribute]

public class AlphaConverterTest : Object
{
    //*************************************************************************
    //  Constructor: AlphaConverterTest()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="AlphaConverterTest" /> class.
    /// </summary>
    //*************************************************************************

    public AlphaConverterTest()
    {
        m_oAlphaConverter = null;
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
        m_oAlphaConverter = new AlphaConverter();
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
        m_oAlphaConverter = null;
    }

    //*************************************************************************
    //  Method: TestWorkbookToGraphAsByte()
    //
    /// <summary>
    /// Tests the WorkbookToGraphAsByte() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestWorkbookToGraphAsByte()
    {
        // Minimum.

        Single fAlphaWorkbook = AlphaConverter.MinimumAlphaWorkbook;

        Byte btExpectedAlphaGraph = 0;

        Byte btAlphaGraph =
            m_oAlphaConverter.WorkbookToGraphAsByte(fAlphaWorkbook);

        Assert.AreEqual(btExpectedAlphaGraph, btAlphaGraph);
    }

    //*************************************************************************
    //  Method: TestWorkbookToGraphAsByte2()
    //
    /// <summary>
    /// Tests the WorkbookToGraphAsByte() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestWorkbookToGraphAsByte2()
    {
        // Maximum.

        Single fAlphaWorkbook = AlphaConverter.MaximumAlphaWorkbook;

        Byte btExpectedAlphaGraph = 255;

        Byte btAlphaGraph =
            m_oAlphaConverter.WorkbookToGraphAsByte(fAlphaWorkbook);

        Assert.AreEqual(btExpectedAlphaGraph, btAlphaGraph);
    }

    //*************************************************************************
    //  Method: TestWorkbookToGraphAsByte3()
    //
    /// <summary>
    /// Tests the WorkbookToGraphAsByte() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestWorkbookToGraphAsByte3()
    {
        // Midpoint.

        Single fAlphaWorkbook = AlphaConverter.MinimumAlphaWorkbook
            + (AlphaConverter.MaximumAlphaWorkbook -
                AlphaConverter.MinimumAlphaWorkbook) / 2;

        Byte btExpectedAlphaGraph = (Byte)(0 + (255 - 0) / 2);

        Byte btAlphaGraph =
            m_oAlphaConverter.WorkbookToGraphAsByte(fAlphaWorkbook);

        Assert.AreEqual(btExpectedAlphaGraph, btAlphaGraph);
    }

    //*************************************************************************
    //  Method: TestWorkbookToGraphAsByte4()
    //
    /// <summary>
    /// Tests the WorkbookToGraphAsByte() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestWorkbookToGraphAsByte4()
    {
        // Below minimum.

        Single fAlphaWorkbook = AlphaConverter.MinimumAlphaWorkbook - 1;

        Byte btExpectedAlphaGraph = 0;

        Byte btAlphaGraph =
            m_oAlphaConverter.WorkbookToGraphAsByte(fAlphaWorkbook);

        Assert.AreEqual(btExpectedAlphaGraph, btAlphaGraph);
    }

    //*************************************************************************
    //  Method: TestWorkbookToGraphAsByte5()
    //
    /// <summary>
    /// Tests the WorkbookToGraphAsByte() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestWorkbookToGraphAsByte5()
    {
        // Above maximum.

        Single fAlphaWorkbook = AlphaConverter.MaximumAlphaWorkbook + 1;

        Byte btExpectedAlphaGraph = 255;

        Byte btAlphaGraph =
            m_oAlphaConverter.WorkbookToGraphAsByte(fAlphaWorkbook);

        Assert.AreEqual(btExpectedAlphaGraph, btAlphaGraph);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Object to test.

    protected AlphaConverter m_oAlphaConverter;
}

}
