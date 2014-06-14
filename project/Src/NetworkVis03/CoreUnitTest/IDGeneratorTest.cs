﻿
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Smrf.NodeXL.Core;

namespace Smrf.NodeXL.UnitTests
{
//*****************************************************************************
//  Class: IDGeneratorTest
//
/// <summary>
/// This is Visual Studio test fixture for the <see cref="IDGenerator" />
/// class.
/// </summary>
//*****************************************************************************

[TestClassAttribute]

public class IDGeneratorTest : Object
{
    //*************************************************************************
    //  Constructor: IDGeneratorTest()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="IDGeneratorTest" /> class.
    /// </summary>
    //*************************************************************************

    public IDGeneratorTest()
    {
        m_oIDGenerator = null;
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
        m_oIDGenerator = new IDGenerator();
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
        m_oIDGenerator = null;
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
        // (Do nothing else.)
    }

    //*************************************************************************
    //  Method: TestGetNextID()
    //
    /// <summary>
    /// Tests the GetNextID method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGetNextID()
    {
        for (Int32 i = 1; i < 100; i++)
        {
            Assert.AreEqual( i, m_oIDGenerator.GetNextID() );
        }
    }

    //*************************************************************************
    //  Method: TestGetNextID2()
    //
    /// <summary>
    /// Tests the GetNextID method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGetNextID2()
    {
        // Use the second constructor.

        const Int32 FirstID = Int32.MinValue;

        m_oIDGenerator = new IDGenerator(FirstID);

        for (Int32 i = FirstID; i < FirstID + 100; i++)
        {
            Assert.AreEqual( i, m_oIDGenerator.GetNextID() );
        }
    }

    //*************************************************************************
    //  Method: TestGetNextID3()
    //
    /// <summary>
    /// Tests the GetNextID method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGetNextID3()
    {
        // Use the second constructor.

        const Int32 FirstID = 0;

        m_oIDGenerator = new IDGenerator(FirstID);

        for (Int32 i = FirstID; i < FirstID + 100; i++)
        {
            Assert.AreEqual( i, m_oIDGenerator.GetNextID() );
        }
    }

    //*************************************************************************
    //  Method: TestGetNextID4()
    //
    /// <summary>
    /// Tests the GetNextID method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGetNextID4()
    {
        // Use the second constructor.

        const Int32 FirstID = 100000;

        m_oIDGenerator = new IDGenerator(FirstID);

        for (Int32 i = FirstID; i < FirstID + 100; i++)
        {
            Assert.AreEqual( i, m_oIDGenerator.GetNextID() );
        }
    }

    


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Object to test.

    protected IDGenerator m_oIDGenerator;
}

}
