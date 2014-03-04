
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Smrf.NodeXL.Core;

namespace Smrf.NodeXL.UnitTests
{
//*****************************************************************************
//  Class: IntergroupEdgeInfoTest
//
/// <summary>
/// This is a Visual Studio test fixture for the <see
/// cref="IntergroupEdgeInfo" /> class.
/// </summary>
//*****************************************************************************

[TestClassAttribute]

public class IntergroupEdgeInfoTest : Object
{
    //*************************************************************************
    //  Constructor: IntergroupEdgeInfoTest()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="IntergroupEdgeInfoTest" />
    /// class.
    /// </summary>
    //*************************************************************************

    public IntergroupEdgeInfoTest()
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
        const Int32 Group1Index = 0;
        const Int32 Group2Index = 12;
        const Int32 Edges = 1;
        const Double EdgeWeightSum = 1.234;

        IntergroupEdgeInfo oIntergroupEdgeInfo =
            new IntergroupEdgeInfo(Group1Index, Group2Index, Edges,
                EdgeWeightSum);

        Assert.AreEqual(Group1Index, oIntergroupEdgeInfo.Group1Index);
        Assert.AreEqual(Group2Index, oIntergroupEdgeInfo.Group2Index);
        Assert.AreEqual(Edges, oIntergroupEdgeInfo.Edges);
        Assert.AreEqual(EdgeWeightSum, oIntergroupEdgeInfo.EdgeWeightSum);

        oIntergroupEdgeInfo.Edges++;
        Assert.AreEqual(Edges + 1, oIntergroupEdgeInfo.Edges);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}

}
