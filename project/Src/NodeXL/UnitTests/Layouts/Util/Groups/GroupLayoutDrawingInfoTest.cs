
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Smrf.NodeXL.Core;
using Smrf.NodeXL.Layouts;

namespace Smrf.NodeXL.UnitTests
{
//*****************************************************************************
//  Class: GroupLayoutDrawingInfoTest
//
/// <summary>
/// This is a Visual Studio test fixture for the <see
/// cref="GroupLayoutDrawingInfo" /> class.
/// </summary>
//*****************************************************************************

[TestClassAttribute]

public class GroupLayoutDrawingInfoTest : Object
{
    //*************************************************************************
    //  Constructor: GroupLayoutDrawingInfoTest()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="GroupLayoutDrawingInfoTest" />
    /// class.
    /// </summary>
    //*************************************************************************

    public GroupLayoutDrawingInfoTest()
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
        // Has CombinedIntergroupEdges.

        GroupInfo [] aoGroupInfo = new GroupInfo [] {
            new GroupInfo(),
            new GroupInfo(),
            new GroupInfo()
            };

        const Double PenWidth = 1.234;

        IntergroupEdgeInfo[] aoCombinedIntergroupEdges =
            new IntergroupEdgeInfo[5];

        GroupLayoutDrawingInfo oGroupLayoutDrawingInfo =
            new GroupLayoutDrawingInfo(aoGroupInfo, PenWidth,
                aoCombinedIntergroupEdges);

        Assert.AreEqual(aoGroupInfo, oGroupLayoutDrawingInfo.GroupsToDraw);
        Assert.AreEqual(PenWidth, oGroupLayoutDrawingInfo.PenWidth);

        Assert.AreEqual(aoCombinedIntergroupEdges,
            oGroupLayoutDrawingInfo.CombinedIntergroupEdges);
    }

    //*************************************************************************
    //  Method: TestConstructor2()
    //
    /// <summary>
    /// Tests the constructor.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestConstructor2()
    {
        // Null CombinedIntergroupEdges.

        GroupInfo [] aoGroupInfo = new GroupInfo [] {
            new GroupInfo(),
            new GroupInfo(),
            new GroupInfo()
            };

        const Double PenWidth = 1.234;

        GroupLayoutDrawingInfo oGroupLayoutDrawingInfo =
            new GroupLayoutDrawingInfo(aoGroupInfo, PenWidth, null);

        Assert.AreEqual(aoGroupInfo, oGroupLayoutDrawingInfo.GroupsToDraw);
        Assert.AreEqual(PenWidth, oGroupLayoutDrawingInfo.PenWidth);
        Assert.IsNull(oGroupLayoutDrawingInfo.CombinedIntergroupEdges);
    }

    //*************************************************************************
    //  Method: TestConstructor3()
    //
    /// <summary>
    /// Tests the constructor.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestConstructor3()
    {
        // Zero pen width.

        GroupInfo [] aoGroupInfo = new GroupInfo [] {
            new GroupInfo(),
            new GroupInfo(),
            new GroupInfo()
            };

        const Double PenWidth = 0;

        IntergroupEdgeInfo[] aoCombinedIntergroupEdges =
            new IntergroupEdgeInfo[5];

        GroupLayoutDrawingInfo oGroupLayoutDrawingInfo =
            new GroupLayoutDrawingInfo(aoGroupInfo, PenWidth,
                aoCombinedIntergroupEdges);

        Assert.AreEqual(aoGroupInfo, oGroupLayoutDrawingInfo.GroupsToDraw);
        Assert.AreEqual(PenWidth, oGroupLayoutDrawingInfo.PenWidth);

        Assert.AreEqual(aoCombinedIntergroupEdges,
            oGroupLayoutDrawingInfo.CombinedIntergroupEdges);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}

}
