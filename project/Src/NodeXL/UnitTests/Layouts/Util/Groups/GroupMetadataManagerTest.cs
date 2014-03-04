
using System;
using System.Collections.Generic;
using System.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Smrf.NodeXL.Core;
using Smrf.NodeXL.Layouts;
using System.Linq;

namespace Smrf.NodeXL.UnitTests
{
//*****************************************************************************
//  Class: GroupMetadataManagerTest
//
/// <summary>
/// This is a Visual Studio test fixture for the <see
/// cref="GroupMetadataManager" /> class.
/// </summary>
//*****************************************************************************

[TestClassAttribute]

public class GroupMetadataManagerTest : Object
{
    //*************************************************************************
    //  Constructor: GroupMetadataManagerTest()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="GroupMetadataManagerTest" /> class.
    /// </summary>
    //*************************************************************************

    public GroupMetadataManagerTest()
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
    //  Method: TestOnLayoutBegin()
    //
    /// <summary>
    /// Tests the OnLayoutBegin() and OnLayoutUsingGroupsEnd() methods.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestOnLayoutBegin()
    {
        // Draw rectangles, hide intergroup edges.

        const Double GroupRectanglePenWidth = 1.23;

        const IntergroupEdgeStyle IntergroupEdgeStyle =
            IntergroupEdgeStyle.Hide;

        IGraph oGraph = new Graph(GraphDirectedness.Undirected);

        IVertex[] aoVertices = TestGraphUtil.AddVertices(oGraph, 4);

        IEdgeCollection oEdges = oGraph.Edges;
        IEdge oEdge1 = oEdges.Add(aoVertices[0], aoVertices[1], false);
        IEdge oEdge2 = oEdges.Add(aoVertices[0], aoVertices[2], false);
        IEdge oEdge3 = oEdges.Add(aoVertices[2], aoVertices[3], false);
        IEdge oEdge4 = oEdges.Add(aoVertices[1], aoVertices[3], false);

        // oEdge2 is intergroup.  Give it an initial visibility. 

        oEdge2.SetValue(ReservedMetadataKeys.Visibility,
            VisibilityKeyValue.Filtered);

        // (oEdge4 is intergroup.  Don't give it an initial visibility.)

        GroupInfo oGroup1 = new GroupInfo();
        oGroup1.Vertices.AddLast( aoVertices[0] );
        oGroup1.Vertices.AddLast( aoVertices[1] );
        oGroup1.Rectangle = Rectangle.FromLTRB(1, 2, 3, 4);

        GroupInfo oGroup2 = new GroupInfo();
        oGroup2.Vertices.AddLast( aoVertices[2] );
        oGroup2.Vertices.AddLast( aoVertices[3] );
        oGroup2.Rectangle = Rectangle.FromLTRB(5, 6, 7, 8);

        GroupMetadataManager.OnLayoutBegin(oGraph);

        GroupMetadataManager.OnLayoutUsingGroupsEnd(oGraph,
            new GroupInfo[] {oGroup1, oGroup2},
            GroupRectanglePenWidth, IntergroupEdgeStyle);

        GroupLayoutDrawingInfo oGroupLayoutDrawingInfo;

        Assert.IsTrue( GroupMetadataManager.TryGetGroupLayoutDrawingInfo(
            oGraph, out oGroupLayoutDrawingInfo) );

        Assert.AreEqual(GroupRectanglePenWidth,
            oGroupLayoutDrawingInfo.PenWidth);

        IList<GroupInfo> oGroupInfo = oGroupLayoutDrawingInfo.GroupsToDraw;

        Assert.AreEqual(2, oGroupInfo.Count);
        Assert.AreEqual( oGroup1, oGroupInfo[0] );
        Assert.AreEqual( oGroup2, oGroupInfo[1] );

        Assert.IsTrue( oGraph.ContainsKey(
            ReservedMetadataKeys.IntergroupEdgesHidden) );

        Assert.IsFalse( oEdge1.ContainsKey(
            ReservedMetadataKeys.SavedVisibility) );

        Assert.IsTrue( oEdge2.ContainsKey(
            ReservedMetadataKeys.SavedVisibility) );

        Assert.AreEqual( VisibilityKeyValue.Filtered,
            oEdge2.GetValue(ReservedMetadataKeys.SavedVisibility) );

        Assert.IsTrue( oEdge2.ContainsKey(
            ReservedMetadataKeys.Visibility) );

        Assert.AreEqual( VisibilityKeyValue.Hidden,
            oEdge2.GetValue(ReservedMetadataKeys.Visibility) );

        Assert.IsFalse( oEdge3.ContainsKey(
            ReservedMetadataKeys.SavedVisibility) );

        Assert.IsTrue( oEdge4.ContainsKey(
            ReservedMetadataKeys.Visibility) );

        Assert.AreEqual( VisibilityKeyValue.Hidden,
            oEdge4.GetValue(ReservedMetadataKeys.Visibility) );

        // Verify that OnLayoutBegin() reverses the metadata changes.

        GroupMetadataManager.OnLayoutBegin(oGraph);

        VerifyOriginalMetadata(oGraph, oEdge1, oEdge2, oEdge3, oEdge4);
    }

    //*************************************************************************
    //  Method: TestOnLayoutBegin2()
    //
    /// <summary>
    /// Tests the OnLayoutBegin() and OnLayoutUsingGroupsEnd() methods.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestOnLayoutBegin2()
    {
        // Don't draw rectangles, show intergroup edges.

        const Double GroupRectanglePenWidth = 0;

        const IntergroupEdgeStyle IntergroupEdgeStyle =
            IntergroupEdgeStyle.Show;

        IGraph oGraph = new Graph(GraphDirectedness.Undirected);

        IVertex[] aoVertices = TestGraphUtil.AddVertices(oGraph, 4);

        IEdgeCollection oEdges = oGraph.Edges;
        IEdge oEdge1 = oEdges.Add(aoVertices[0], aoVertices[1], false);
        IEdge oEdge2 = oEdges.Add(aoVertices[0], aoVertices[2], false);
        IEdge oEdge3 = oEdges.Add(aoVertices[2], aoVertices[3], false);
        IEdge oEdge4 = oEdges.Add(aoVertices[1], aoVertices[3], false);

        // oEdge2 is intergroup.  Give it an initial visibility. 

        oEdge2.SetValue(ReservedMetadataKeys.Visibility,
            VisibilityKeyValue.Filtered);

        // (oEdge4 is intergroup.  Don't give it an initial visibility.)

        GroupInfo oGroup1 = new GroupInfo();
        oGroup1.Vertices.AddLast( aoVertices[0] );
        oGroup1.Vertices.AddLast( aoVertices[1] );
        oGroup1.Rectangle = Rectangle.FromLTRB(1, 2, 3, 4);

        GroupInfo oGroup2 = new GroupInfo();
        oGroup2.Vertices.AddLast( aoVertices[2] );
        oGroup2.Vertices.AddLast( aoVertices[3] );
        oGroup2.Rectangle = Rectangle.FromLTRB(5, 6, 7, 8);

        GroupMetadataManager.OnLayoutBegin(oGraph);

        GroupMetadataManager.OnLayoutUsingGroupsEnd(oGraph,
            new GroupInfo[] {oGroup1, oGroup2},
            GroupRectanglePenWidth, IntergroupEdgeStyle);

        GroupLayoutDrawingInfo oGroupLayoutDrawingInfo;

        Assert.IsTrue( GroupMetadataManager.TryGetGroupLayoutDrawingInfo(
            oGraph, out oGroupLayoutDrawingInfo) );

        Assert.AreEqual(GroupRectanglePenWidth,
            oGroupLayoutDrawingInfo.PenWidth);

        IList<GroupInfo> oGroupInfo = oGroupLayoutDrawingInfo.GroupsToDraw;

        Assert.AreEqual(2, oGroupInfo.Count);
        Assert.AreEqual( oGroup1, oGroupInfo[0] );
        Assert.AreEqual( oGroup2, oGroupInfo[1] );

        VerifyOriginalEdgeMetadata(oEdge1, oEdge2, oEdge3, oEdge4);

        // Verify that OnLayoutBegin() reverses the metadata changes.

        GroupMetadataManager.OnLayoutBegin(oGraph);

        VerifyOriginalMetadata(oGraph, oEdge1, oEdge2, oEdge3, oEdge4);
    }

    //*************************************************************************
    //  Method: TestOnLayoutBegin3()
    //
    /// <summary>
    /// Tests the OnLayoutBegin() and OnLayoutUsingGroupsEnd() methods.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestOnLayoutBegin3()
    {
        // Don't draw rectangles, hide intergroup edges.

        const Double GroupRectanglePenWidth = 0;

        const IntergroupEdgeStyle IntergroupEdgeStyle =
            IntergroupEdgeStyle.Hide;

        IGraph oGraph = new Graph(GraphDirectedness.Undirected);

        IVertex[] aoVertices = TestGraphUtil.AddVertices(oGraph, 4);

        IEdgeCollection oEdges = oGraph.Edges;
        IEdge oEdge1 = oEdges.Add(aoVertices[0], aoVertices[1], false);
        IEdge oEdge2 = oEdges.Add(aoVertices[0], aoVertices[2], false);
        IEdge oEdge3 = oEdges.Add(aoVertices[2], aoVertices[3], false);
        IEdge oEdge4 = oEdges.Add(aoVertices[1], aoVertices[3], false);

        // oEdge2 is intergroup.  Give it an initial visibility. 

        oEdge2.SetValue(ReservedMetadataKeys.Visibility,
            VisibilityKeyValue.Filtered);

        // (oEdge4 is intergroup.  Don't give it an initial visibility.)

        GroupInfo oGroup1 = new GroupInfo();
        oGroup1.Vertices.AddLast( aoVertices[0] );
        oGroup1.Vertices.AddLast( aoVertices[1] );
        oGroup1.Rectangle = Rectangle.FromLTRB(1, 2, 3, 4);

        GroupInfo oGroup2 = new GroupInfo();
        oGroup2.Vertices.AddLast( aoVertices[2] );
        oGroup2.Vertices.AddLast( aoVertices[3] );
        oGroup2.Rectangle = Rectangle.FromLTRB(5, 6, 7, 8);

        GroupMetadataManager.OnLayoutBegin(oGraph);

        GroupMetadataManager.OnLayoutUsingGroupsEnd(oGraph,
            new GroupInfo[] {oGroup1, oGroup2},
            GroupRectanglePenWidth, IntergroupEdgeStyle);

        GroupLayoutDrawingInfo oGroupLayoutDrawingInfo;

        Assert.IsTrue( GroupMetadataManager.TryGetGroupLayoutDrawingInfo(
            oGraph, out oGroupLayoutDrawingInfo) );

        Assert.AreEqual(GroupRectanglePenWidth,
            oGroupLayoutDrawingInfo.PenWidth);

        IList<GroupInfo> oGroupInfo = oGroupLayoutDrawingInfo.GroupsToDraw;

        Assert.AreEqual(2, oGroupInfo.Count);
        Assert.AreEqual( oGroup1, oGroupInfo[0] );
        Assert.AreEqual( oGroup2, oGroupInfo[1] );

        Assert.IsTrue( oGraph.ContainsKey(
            ReservedMetadataKeys.IntergroupEdgesHidden) );

        Assert.IsFalse( oEdge1.ContainsKey(
            ReservedMetadataKeys.SavedVisibility) );

        Assert.IsTrue( oEdge2.ContainsKey(
            ReservedMetadataKeys.SavedVisibility) );

        Assert.AreEqual( VisibilityKeyValue.Filtered,
            oEdge2.GetValue(ReservedMetadataKeys.SavedVisibility) );

        Assert.IsTrue( oEdge2.ContainsKey(
            ReservedMetadataKeys.Visibility) );

        Assert.AreEqual( VisibilityKeyValue.Hidden,
            oEdge2.GetValue(ReservedMetadataKeys.Visibility) );

        Assert.IsFalse( oEdge3.ContainsKey(
            ReservedMetadataKeys.SavedVisibility) );

        // Verify that OnLayoutBegin() reverses the metadata changes.

        GroupMetadataManager.OnLayoutBegin(oGraph);

        VerifyOriginalMetadata(oGraph, oEdge1, oEdge2, oEdge3, oEdge4);
    }

    //*************************************************************************
    //  Method: TestOnLayoutBegin4()
    //
    /// <summary>
    /// Tests the OnLayoutBegin() and OnLayoutUsingGroupsEnd() methods.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestOnLayoutBegin4()
    {
        // Draw rectangles, show intergroup edges.

        const Double GroupRectanglePenWidth = 4.567;

        const IntergroupEdgeStyle IntergroupEdgeStyle =
            IntergroupEdgeStyle.Show;

        IGraph oGraph = new Graph(GraphDirectedness.Undirected);

        IVertex[] aoVertices = TestGraphUtil.AddVertices(oGraph, 4);

        IEdgeCollection oEdges = oGraph.Edges;
        IEdge oEdge1 = oEdges.Add(aoVertices[0], aoVertices[1], false);
        IEdge oEdge2 = oEdges.Add(aoVertices[0], aoVertices[2], false);
        IEdge oEdge3 = oEdges.Add(aoVertices[2], aoVertices[3], false);
        IEdge oEdge4 = oEdges.Add(aoVertices[1], aoVertices[3], false);

        // oEdge2 is intergroup.  Give it an initial visibility. 

        oEdge2.SetValue(ReservedMetadataKeys.Visibility,
            VisibilityKeyValue.Filtered);

        // (oEdge4 is intergroup.  Don't give it an initial visibility.)

        GroupInfo oGroup1 = new GroupInfo();
        oGroup1.Vertices.AddLast( aoVertices[0] );
        oGroup1.Vertices.AddLast( aoVertices[1] );
        oGroup1.Rectangle = Rectangle.FromLTRB(1, 2, 3, 4);

        GroupInfo oGroup2 = new GroupInfo();
        oGroup2.Vertices.AddLast( aoVertices[2] );
        oGroup2.Vertices.AddLast( aoVertices[3] );
        oGroup2.Rectangle = Rectangle.FromLTRB(5, 6, 7, 8);

        GroupMetadataManager.OnLayoutBegin(oGraph);

        GroupMetadataManager.OnLayoutUsingGroupsEnd(oGraph,
            new GroupInfo[] {oGroup1, oGroup2},
            GroupRectanglePenWidth, IntergroupEdgeStyle);

        GroupLayoutDrawingInfo oGroupLayoutDrawingInfo;

        Assert.IsTrue( GroupMetadataManager.TryGetGroupLayoutDrawingInfo(
            oGraph, out oGroupLayoutDrawingInfo) );

        Assert.AreEqual(GroupRectanglePenWidth,
            oGroupLayoutDrawingInfo.PenWidth);

        IList<GroupInfo> oGroupInfo = oGroupLayoutDrawingInfo.GroupsToDraw;

        Assert.AreEqual(2, oGroupInfo.Count);
        Assert.AreEqual( oGroup1, oGroupInfo[0] );
        Assert.AreEqual( oGroup2, oGroupInfo[1] );

        Assert.IsFalse( oGraph.ContainsKey(
            ReservedMetadataKeys.IntergroupEdgesHidden) );

        Assert.IsFalse( oEdge1.ContainsKey(
            ReservedMetadataKeys.SavedVisibility) );

        Assert.IsFalse( oEdge2.ContainsKey(
            ReservedMetadataKeys.SavedVisibility) );

        Assert.IsTrue( oEdge2.ContainsKey(
            ReservedMetadataKeys.Visibility) );

        Assert.AreEqual( VisibilityKeyValue.Filtered,
            oEdge2.GetValue(ReservedMetadataKeys.Visibility) );

        Assert.IsFalse( oEdge3.ContainsKey(
            ReservedMetadataKeys.SavedVisibility) );

        // Verify that OnLayoutBegin() reverses the metadata changes.

        GroupMetadataManager.OnLayoutBegin(oGraph);

        VerifyOriginalMetadata(oGraph, oEdge1, oEdge2, oEdge3, oEdge4);
    }

    //*************************************************************************
    //  Method: TestOnLayoutBegin5()
    //
    /// <summary>
    /// Tests the OnLayoutBegin() and OnLayoutUsingGroupsEnd() methods.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestOnLayoutBegin5()
    {
        // Request that intergroup edges be combined, although there are none.

        const Double GroupRectanglePenWidth = 4.567;

        const IntergroupEdgeStyle IntergroupEdgeStyle =
            IntergroupEdgeStyle.Combine;

        IGraph oGraph = new Graph(GraphDirectedness.Undirected);

        IVertex[] aoVertices = TestGraphUtil.AddVertices(oGraph, 5);

        IEdgeCollection oEdges = oGraph.Edges;
        IEdge oEdge1 = oEdges.Add(aoVertices[0], aoVertices[1], false);
        IEdge oEdge2 = oEdges.Add(aoVertices[2], aoVertices[3], false);

        GroupInfo oGroup1 = new GroupInfo();
        oGroup1.Vertices.AddLast( aoVertices[0] );
        oGroup1.Vertices.AddLast( aoVertices[1] );
        oGroup1.Rectangle = Rectangle.FromLTRB(1, 2, 3, 4);

        GroupInfo oGroup2 = new GroupInfo();
        oGroup2.Vertices.AddLast( aoVertices[2] );
        oGroup2.Vertices.AddLast( aoVertices[3] );
        oGroup2.Rectangle = Rectangle.FromLTRB(5, 6, 7, 8);

        GroupInfo oGroup3 = new GroupInfo();
        oGroup3.Vertices.AddLast( aoVertices[4] );
        oGroup3.Rectangle = Rectangle.FromLTRB(9, 10, 11, 12);

        GroupMetadataManager.OnLayoutBegin(oGraph);

        GroupMetadataManager.OnLayoutUsingGroupsEnd(oGraph,
            new GroupInfo[] {oGroup1, oGroup2, oGroup3},
            GroupRectanglePenWidth, IntergroupEdgeStyle);

        GroupLayoutDrawingInfo oGroupLayoutDrawingInfo;

        Assert.IsTrue( GroupMetadataManager.TryGetGroupLayoutDrawingInfo(
            oGraph, out oGroupLayoutDrawingInfo) );

        Assert.AreEqual(GroupRectanglePenWidth,
            oGroupLayoutDrawingInfo.PenWidth);

        IList<GroupInfo> oGroupInfo = oGroupLayoutDrawingInfo.GroupsToDraw;

        Assert.AreEqual(3, oGroupInfo.Count);
        Assert.AreEqual( oGroup1, oGroupInfo[0] );
        Assert.AreEqual( oGroup2, oGroupInfo[1] );
        Assert.AreEqual( oGroup3, oGroupInfo[2] );

        Assert.IsFalse( oGraph.ContainsKey(
            ReservedMetadataKeys.IntergroupEdgesHidden) );

        Assert.IsFalse( oEdge1.ContainsKey(
            ReservedMetadataKeys.SavedVisibility) );

        Assert.IsFalse( oEdge2.ContainsKey(
            ReservedMetadataKeys.SavedVisibility) );

        Assert.AreEqual( 0,
            oGroupLayoutDrawingInfo.CombinedIntergroupEdges.Count() );
    }

    //*************************************************************************
    //  Method: TestOnLayoutBegin6()
    //
    /// <summary>
    /// Tests the OnLayoutBegin() and OnLayoutUsingGroupsEnd() methods.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestOnLayoutBegin6()
    {
        // Request that intergroup edges be combined, and there are some.

        const Double GroupRectanglePenWidth = 4.567;

        const IntergroupEdgeStyle IntergroupEdgeStyle =
            IntergroupEdgeStyle.Combine;

        IGraph oGraph = new Graph(GraphDirectedness.Undirected);

        IVertex[] aoVertices = TestGraphUtil.AddVertices(oGraph, 5);

        IEdgeCollection oEdges = oGraph.Edges;

        // Not intergroup.

        IEdge oEdge1 = oEdges.Add(aoVertices[0], aoVertices[1], false);
        IEdge oEdge2 = oEdges.Add(aoVertices[2], aoVertices[3], false);

        // Intergroup, between groups 1 and 2.

        IEdge oEdge3 = oEdges.Add(aoVertices[0], aoVertices[2], false);
        IEdge oEdge4 = oEdges.Add(aoVertices[1], aoVertices[2], false);
        IEdge oEdge5 = oEdges.Add(aoVertices[1], aoVertices[3], false);

        // Intergroup, between groups 2 and 3.

        IEdge oEdge6 = oEdges.Add(aoVertices[2], aoVertices[4], false);
        IEdge oEdge7 = oEdges.Add(aoVertices[3], aoVertices[4], false);

        // Intergroup, between groups 1 and 3.

        IEdge oEdge8 = oEdges.Add(aoVertices[1], aoVertices[4], false);

        GroupInfo oGroup1 = new GroupInfo();
        oGroup1.Vertices.AddLast( aoVertices[0] );
        oGroup1.Vertices.AddLast( aoVertices[1] );
        oGroup1.Rectangle = Rectangle.FromLTRB(1, 2, 3, 4);

        GroupInfo oGroup2 = new GroupInfo();
        oGroup2.Vertices.AddLast( aoVertices[2] );
        oGroup2.Vertices.AddLast( aoVertices[3] );
        oGroup2.Rectangle = Rectangle.FromLTRB(5, 6, 7, 8);

        GroupInfo oGroup3 = new GroupInfo();
        oGroup3.Vertices.AddLast( aoVertices[4] );
        oGroup3.Rectangle = Rectangle.FromLTRB(9, 10, 11, 12);

        GroupMetadataManager.OnLayoutBegin(oGraph);

        GroupMetadataManager.OnLayoutUsingGroupsEnd(oGraph,
            new GroupInfo[] {oGroup1, oGroup2, oGroup3},
            GroupRectanglePenWidth, IntergroupEdgeStyle);

        GroupLayoutDrawingInfo oGroupLayoutDrawingInfo;

        Assert.IsTrue( GroupMetadataManager.TryGetGroupLayoutDrawingInfo(
            oGraph, out oGroupLayoutDrawingInfo) );

        Assert.AreEqual(GroupRectanglePenWidth,
            oGroupLayoutDrawingInfo.PenWidth);

        IList<GroupInfo> oGroupInfo = oGroupLayoutDrawingInfo.GroupsToDraw;

        Assert.AreEqual(3, oGroupInfo.Count);
        Assert.AreEqual( oGroup1, oGroupInfo[0] );
        Assert.AreEqual( oGroup2, oGroupInfo[1] );
        Assert.AreEqual( oGroup3, oGroupInfo[2] );

        Assert.IsTrue( oGraph.ContainsKey(
            ReservedMetadataKeys.IntergroupEdgesHidden) );

        Assert.IsFalse( oEdge1.ContainsKey(
            ReservedMetadataKeys.SavedVisibility) );

        Assert.IsFalse( oEdge2.ContainsKey(
            ReservedMetadataKeys.SavedVisibility) );

        Assert.IsTrue( oEdge3.ContainsKey(
            ReservedMetadataKeys.SavedVisibility) );

        Assert.IsTrue( oEdge4.ContainsKey(
            ReservedMetadataKeys.SavedVisibility) );

        Assert.IsTrue( oEdge5.ContainsKey(
            ReservedMetadataKeys.SavedVisibility) );

        Assert.IsTrue( oEdge6.ContainsKey(
            ReservedMetadataKeys.SavedVisibility) );

        Assert.IsTrue( oEdge7.ContainsKey(
            ReservedMetadataKeys.SavedVisibility) );

        Assert.IsTrue( oEdge8.ContainsKey(
            ReservedMetadataKeys.SavedVisibility) );

        Assert.AreEqual( 3,
            oGroupLayoutDrawingInfo.CombinedIntergroupEdges.Count() );

        IntergroupEdgeInfo oCombinedIntergroupEdge;

        oCombinedIntergroupEdge =
            oGroupLayoutDrawingInfo.CombinedIntergroupEdges.Single(
                intergroupEdgeInfo =>
                intergroupEdgeInfo.Group1Index == 0
                &&
                intergroupEdgeInfo.Group2Index == 1
                );

        Assert.AreEqual(0, oCombinedIntergroupEdge.Group1Index);
        Assert.AreEqual(1, oCombinedIntergroupEdge.Group2Index);
        Assert.AreEqual(3, oCombinedIntergroupEdge.Edges);

        oCombinedIntergroupEdge =
            oGroupLayoutDrawingInfo.CombinedIntergroupEdges.Single(
                intergroupEdgeInfo =>
                intergroupEdgeInfo.Group1Index == 0
                &&
                intergroupEdgeInfo.Group2Index == 2
                );

        Assert.AreEqual(0, oCombinedIntergroupEdge.Group1Index);
        Assert.AreEqual(2, oCombinedIntergroupEdge.Group2Index);
        Assert.AreEqual(1, oCombinedIntergroupEdge.Edges);

        oCombinedIntergroupEdge =
            oGroupLayoutDrawingInfo.CombinedIntergroupEdges.Single(
                intergroupEdgeInfo =>
                intergroupEdgeInfo.Group1Index == 1
                &&
                intergroupEdgeInfo.Group2Index == 2
                );

        Assert.AreEqual(1, oCombinedIntergroupEdge.Group1Index);
        Assert.AreEqual(2, oCombinedIntergroupEdge.Group2Index);
        Assert.AreEqual(2, oCombinedIntergroupEdge.Edges);
    }

    //*************************************************************************
    //  Method: TestTryGetGroupLayoutDrawingInfo()
    //
    /// <summary>
    /// Tests the TryGetGroupLayoutDrawingInfo() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestTryGetGroupLayoutDrawingInfo()
    {
        // Graph has group drawing information.

        const Double GroupRectanglePenWidth = 4.567;

        const IntergroupEdgeStyle IntergroupEdgeStyle =
            IntergroupEdgeStyle.Show;

        IGraph oGraph = new Graph();

        oGraph.SetValue(ReservedMetadataKeys.GroupLayoutDrawingInfo,
            new GroupLayoutDrawingInfo(
                new GroupInfo[] { new GroupInfo(), new GroupInfo() },
                GroupRectanglePenWidth, null
            ) );

        GroupLayoutDrawingInfo oGroupLayoutDrawingInfo;

        Assert.IsTrue( GroupMetadataManager.TryGetGroupLayoutDrawingInfo(
            oGraph, out oGroupLayoutDrawingInfo) );

        Assert.AreEqual(2, oGroupLayoutDrawingInfo.GroupsToDraw.Count);

        Assert.AreEqual(GroupRectanglePenWidth,
            oGroupLayoutDrawingInfo.PenWidth);

        Assert.IsNull(oGroupLayoutDrawingInfo.CombinedIntergroupEdges);
    }

    //*************************************************************************
    //  Method: TestTryGetGroupLayoutDrawingInfo2()
    //
    /// <summary>
    /// Tests the TryGetGroupLayoutDrawingInfo() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestTryGetGroupLayoutDrawingInfo2()
    {
        // Graph does not have group drawing information.

        IGraph oGraph = new Graph();

        GroupLayoutDrawingInfo oGroupLayoutDrawingInfo;

        Assert.IsFalse( GroupMetadataManager.TryGetGroupLayoutDrawingInfo(
            oGraph, out oGroupLayoutDrawingInfo) );
    }

    //*************************************************************************
    //  Method: TestTryTransformGroupRectangles()
    //
    /// <summary>
    /// Tests the TryTransformGroupRectangles() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestTryTransformGroupRectangles()
    {
        // Graph has group drawing information.

        const Double GroupRectanglePenWidth = 4.567;

        IGraph oGraph = new Graph();

        GroupInfo oGroupInfo1 = new GroupInfo();
        oGroupInfo1.Rectangle = Rectangle.FromLTRB(0, 0, 1, 2);

        GroupInfo oGroupInfo2 = new GroupInfo();
        oGroupInfo2.Rectangle = Rectangle.FromLTRB(0, 0, 3, 4);

        oGraph.SetValue(ReservedMetadataKeys.GroupLayoutDrawingInfo,
            new GroupLayoutDrawingInfo(
                new GroupInfo[] {oGroupInfo1, oGroupInfo2},
                GroupRectanglePenWidth, null
            ) );

        GroupMetadataManager.TransformGroupRectangles(oGraph,
            new LayoutContext( Rectangle.FromLTRB(0, 0, 10, 20) ),
            new LayoutContext( Rectangle.FromLTRB(0, 0, 20, 60) )
            );

        GroupLayoutDrawingInfo oGroupLayoutDrawingInfo;

        Assert.IsTrue( GroupMetadataManager.TryGetGroupLayoutDrawingInfo(
            oGraph, out oGroupLayoutDrawingInfo) );

        Assert.AreEqual(2, oGroupLayoutDrawingInfo.GroupsToDraw.Count);

        Assert.AreEqual(Rectangle.FromLTRB(0, 0, 2, 6),
            oGroupLayoutDrawingInfo.GroupsToDraw[0].Rectangle);

        Assert.AreEqual(Rectangle.FromLTRB(0, 0, 6, 12),
            oGroupLayoutDrawingInfo.GroupsToDraw[1].Rectangle);
    }

    //*************************************************************************
    //  Method: TestTryTransformGroupRectangles2()
    //
    /// <summary>
    /// Tests the TryTransformGroupRectangles() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestTryTransformGroupRectangles2()
    {
        // Graph does not have group drawing information.

        IGraph oGraph = new Graph();

        GroupMetadataManager.TransformGroupRectangles(oGraph,
            new LayoutContext( Rectangle.FromLTRB(0, 0, 10, 20) ),
            new LayoutContext( Rectangle.FromLTRB(0, 0, 20, 10) )
            );

        GroupLayoutDrawingInfo oGroupLayoutDrawingInfo;

        Assert.IsFalse( GroupMetadataManager.TryGetGroupLayoutDrawingInfo(
            oGraph, out oGroupLayoutDrawingInfo) );
    }

    //*************************************************************************
    //  Method: VerifyOriginalMetadata()
    //
    /// <summary>
    /// Verifies that the graph passed to the OnLayoutBegin() and
    /// OnLayoutUsingGroupsEnd() methods have their original metadata.
    /// </summary>
    ///
    /// <param name="oGraph">
    /// The graph passed to the OnLayoutBegin() and OnLayoutUsingGroupsEnd()
    /// methods.
    /// </param>
    ///
    /// <param name="oEdge1">
    /// The graph's first edge.
    /// </param>
    ///
    /// <param name="oEdge2">
    /// The graph's second edge, which is intergroup.
    /// </param>
    ///
    /// <param name="oEdge3">
    /// The graph's third edge.
    /// </param>
    ///
    /// <param name="oEdge4">
    /// The graph's fourth edge, which is intergroup.
    /// </param>
    //*************************************************************************

    protected void
    VerifyOriginalMetadata
    (
        IGraph oGraph,
        IEdge oEdge1,
        IEdge oEdge2,
        IEdge oEdge3,
        IEdge oEdge4
    )
    {
        GroupLayoutDrawingInfo oGroupLayoutDrawingInfo;

        Assert.IsFalse( GroupMetadataManager.TryGetGroupLayoutDrawingInfo(
            oGraph, out oGroupLayoutDrawingInfo) );

        Assert.IsFalse( oGraph.ContainsKey(
            ReservedMetadataKeys.IntergroupEdgesHidden) );

        VerifyOriginalEdgeMetadata(oEdge1, oEdge2, oEdge3, oEdge4);
    }

    //*************************************************************************
    //  Method: VerifyOriginalEdgeMetadata()
    //
    /// <summary>
    /// Verifies that the edges of the graph passed to the OnLayoutBegin() and
    /// OnLayoutUsingGroupsEnd() methods have their original metadata.
    /// </summary>
    ///
    /// <param name="oEdge1">
    /// The graph's first edge.
    /// </param>
    ///
    /// <param name="oEdge2">
    /// The graph's second edge, which is intergroup.
    /// </param>
    ///
    /// <param name="oEdge3">
    /// The graph's third edge.
    /// </param>
    ///
    /// <param name="oEdge4">
    /// The graph's fourth edge, which is intergroup.
    /// </param>
    //*************************************************************************

    protected void
    VerifyOriginalEdgeMetadata
    (
        IEdge oEdge1,
        IEdge oEdge2,
        IEdge oEdge3,
        IEdge oEdge4
    )
    {
        Assert.IsFalse( oEdge1.ContainsKey(
            ReservedMetadataKeys.SavedVisibility) );

        Assert.IsFalse( oEdge1.ContainsKey(
            ReservedMetadataKeys.Visibility) );

        Assert.IsFalse( oEdge2.ContainsKey(
            ReservedMetadataKeys.SavedVisibility) );

        Assert.IsTrue( oEdge2.ContainsKey(
            ReservedMetadataKeys.Visibility) );

        Assert.AreEqual( VisibilityKeyValue.Filtered,
            oEdge2.GetValue(ReservedMetadataKeys.Visibility) );

        Assert.IsFalse( oEdge3.ContainsKey(
            ReservedMetadataKeys.SavedVisibility) );

        Assert.IsFalse( oEdge3.ContainsKey(
            ReservedMetadataKeys.Visibility) );

        Assert.IsFalse( oEdge4.ContainsKey(
            ReservedMetadataKeys.SavedVisibility) );

        Assert.IsFalse( oEdge4.ContainsKey(
            ReservedMetadataKeys.Visibility) );
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}

}
