
using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Smrf.NodeXL.Core;

namespace Smrf.NodeXL.UnitTests
{
//*****************************************************************************
//  Class: GroupUtilTest
//
/// <summary>
/// This is a Visual Studio test fixture for the <see cref="GroupUtil" />
/// class.
/// </summary>
//*****************************************************************************

[TestClassAttribute]

public class GroupUtilTest : Object
{
    //*************************************************************************
    //  Constructor: GroupUtilTest()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="GroupUtilTest" /> class.
    /// </summary>
    //*************************************************************************

    public GroupUtilTest()
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
    //  Method: TestRemoveIsolatesFromGroups()
    //
    /// <summary>
    /// Tests the RemoveIsolatesFromGroups() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestRemoveIsolatesFromGroups()
    {
        IGraph oGraph = new Graph();
        IEdgeCollection oEdges = oGraph.Edges;
        IVertexCollection oVertices = oGraph.Vertices;

        // Non-isolates.

        IVertex oVertexA = oVertices.Add();
        IVertex oVertexB = oVertices.Add();
        IVertex oVertexC = oVertices.Add();
        IVertex oVertexD = oVertices.Add();

        // Isolates.

        IVertex oVertexE = oVertices.Add();
        IVertex oVertexF = oVertices.Add();
        IVertex oVertexG = oVertices.Add();

        oEdges.Add(oVertexA, oVertexB);
        oEdges.Add(oVertexB, oVertexC);
        oEdges.Add(oVertexD, oVertexA);

        // Group1 contains all non-isolate vertices.

        GroupInfo oGroup1 = new GroupInfo();
        oGroup1.Vertices.AddLast(oVertexA);
        oGroup1.Vertices.AddLast(oVertexB);
        oGroup1.Vertices.AddLast(oVertexC);

        // Group2 contains a mix of isolate and non-isolate vertices.

        GroupInfo oGroup2 = new GroupInfo();
        oGroup2.Vertices.AddLast(oVertexD);
        oGroup2.Vertices.AddLast(oVertexE);

        // Group3 contains all isolate vertices.

        GroupInfo oGroup3 = new GroupInfo();
        oGroup3.Vertices.AddLast(oVertexF);
        oGroup3.Vertices.AddLast(oVertexG);

        // Group4 is empty.

        GroupInfo oGroup4 = new GroupInfo();

        List<GroupInfo> oGroups = new List<GroupInfo>();
        oGroups.Add(oGroup1);
        oGroups.Add(oGroup2);
        oGroups.Add(oGroup3);
        oGroups.Add(oGroup4);

        GroupUtil.RemoveIsolatesFromGroups(oGroups);

        Assert.AreEqual(2, oGroups.Count);

        Assert.AreEqual(3, oGroups[0].Vertices.Count);
        Assert.IsNotNull( oGroups[0].Vertices.Find(oVertexA) );
        Assert.IsNotNull( oGroups[0].Vertices.Find(oVertexB) );
        Assert.IsNotNull( oGroups[0].Vertices.Find(oVertexC) );

        Assert.AreEqual(1, oGroups[1].Vertices.Count);
        Assert.IsNotNull( oGroups[1].Vertices.Find(oVertexD) );
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}

}
