﻿
using System;
using System.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Smrf.NodeXL.Core;
using Smrf.NodeXL.Layouts;

namespace Smrf.NodeXL.UnitTests
{
//*****************************************************************************
//  Class: LayOutGraphAsyncArgumentsTest
//
/// <summary>
/// This is a Visual Studio test fixture for the <see
/// cref="LayOutGraphAsyncArguments" /> class.
/// </summary>
//*****************************************************************************

[TestClassAttribute]

public class LayOutGraphAsyncArgumentsTest : Object
{
    //*************************************************************************
    //  Constructor: LayOutGraphAsyncArgumentsTest()
    //
    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="LayOutGraphAsyncArgumentsTest" /> class.
    /// </summary>
    //*************************************************************************

    public LayOutGraphAsyncArgumentsTest()
    {
        m_oLayOutGraphAsyncArguments = null;
        m_oGraph = null;
        m_oLayoutContext = null;
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
        m_oGraph = new Graph();

        Rectangle oRectangle = new Rectangle(21, 34, 56, 78);

        m_oLayoutContext = new LayoutContext(oRectangle);

        m_oLayOutGraphAsyncArguments =
            new LayOutGraphAsyncArguments(m_oGraph, m_oLayoutContext);
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
        m_oLayOutGraphAsyncArguments = null;
        m_oGraph = null;
        m_oLayoutContext = null;
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
        Assert.AreSame(m_oGraph, m_oLayOutGraphAsyncArguments.Graph);

        Assert.AreEqual(
            m_oLayoutContext, m_oLayOutGraphAsyncArguments.LayoutContext);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Object to test.

    protected LayOutGraphAsyncArguments m_oLayOutGraphAsyncArguments;

    /// Graph contained in the object.

    protected IGraph m_oGraph;

    /// LayoutContext contained in the object.

    protected LayoutContext m_oLayoutContext;
}

}
