
using System;
using System.Drawing;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using Smrf.NodeXL.Core;
using Smrf.GraphicsLib;

namespace Smrf.NodeXL.Layouts
{

//*****************************************************************************
//  Class: PackedGroupRectangleCalculator
//
/// <summary>
/// Calculates a rectangle for each group of vertices.
/// </summary>
///
/// <remarks>
/// This class is used when a BoxLayoutAlgorithm of <see
/// cref="BoxLayoutAlgorithm.PackedRectangles" /> is specified.
///
/// <para>
/// All methods are static.
/// </para>
///
/// </remarks>
//*****************************************************************************

public static class PackedGroupRectangleCalculator
{
    static LinkedList<FreeSpaceBox> freeSpaceBoxList;
    static RectangleF[] aoGroupRectangles;
    //*************************************************************************
    //  Method: CalculateGroupRectangles()
    //
    /// <summary>
    /// Calculates a rectangle for each group of vertices.
    /// </summary>
    ///
    /// <param name="graphRectangle">
    /// The <see cref="Rectangle" /> the graph is being laid out within.
    /// </param>
    ///
    /// <param name="sortedGroups">
    /// List of <see cref="GroupInfo" /> objects, one for each group of
    /// vertices.  Must be sorted in descending order of the number of vertices
    /// in each group.  There must be at least one vertex in each group.
    /// </param>
    /// 
    /// <param name="alpha">
    /// The factor by which the center group's size should be reduced if all groups
    /// do not fit in the screen space
    /// </param>
    //*************************************************************************

    public static void
    CalculateGroupRectangles
    (
        Rectangle graphRectangle,
        IList<GroupInfo> sortedGroups,
        double alpha
    )
    {
        Debug.Assert(sortedGroups != null);

        #if DEBUG
        {
            Int32 iPreviousGroupVertices = Int32.MaxValue;

            foreach (GroupInfo oGroup in sortedGroups)
            {
                Int32 iVertices = oGroup.Vertices.Count;
                Debug.Assert(iVertices > 0);
                //Debug.Assert(iVertices <= iPreviousGroupVertices);
                iPreviousGroupVertices = iVertices;
            }
        }
        #endif

        double ARs = (double)graphRectangle.Height / graphRectangle.Width; // Aspect Ratio of the screen
        double gamma = 2.0;

        Int32 iSortedGroupCount = sortedGroups.Count;

        // Count the total number of vertices in the graph
        Double dTotalNumberOfVertices = 0;
        for (int i = 0; i < iSortedGroupCount; i++)
        {
            dTotalNumberOfVertices += sortedGroups[i].Vertices.Count;
        }
        Debug.Assert(dTotalNumberOfVertices > 0);

        // Get what percent of the total number of vertices is the first two groups fraction.
        double firstTwoGroupFraction = ((double)sortedGroups[0].Vertices.Count + (double)sortedGroups[1].Vertices.Count) * 100 / (double)dTotalNumberOfVertices;

        if (iSortedGroupCount<=3 || firstTwoGroupFraction <=10)
        {   //Layout in group in a box
            GroupRectangleCalculator.CalculateGroupRectangles(graphRectangle, sortedGroups);
        }
        else if (firstTwoGroupFraction >= 10 && firstTwoGroupFraction < 45)
        {   // Layout in donut shape
            LayoutAllGroupsInDonut(graphRectangle, sortedGroups, alpha, gamma, ARs, iSortedGroupCount, dTotalNumberOfVertices);
        }
        else
        {   // Layout in U shape
            LayoutAllGroupsInU(graphRectangle, sortedGroups, alpha, gamma, ARs, iSortedGroupCount, dTotalNumberOfVertices);
        }
    }

    //*************************************************************************
    //  Property: LayoutAllGroupsInU
    //
    /// <summary>
    /// Determines a rectangle for every group. Places one group at top center and arranges the rest around it in a U
    /// </summary>
    ///
    /// <param name="graphRectangle">
    /// The <see cref="Rectangle" /> the graph is being laid out within.
    /// </param>
    ///
    /// <param name="sortedGroups">
    /// List of <see cref="GroupInfo" /> objects, one for each group of
    /// vertices.  Must be sorted in descending order of the connectivity
    /// There must be at least one vertex in each group.
    /// </param>
    ///
    /// <param name="alpha">
    /// The factor by which gamma is to be reduced if all groups do not fit in the graph rectangle
    /// </param>
    /// 
    /// <param name="gamma">
    /// The fraction of space occupied by the first (center) group
    /// </param>
    ///
    /// <param name="ARs">
    /// Aspect ratio of graphRectangle
    /// </param>
    ///
    /// <param name="iSortedGroupCount">
    /// Total number of groups to be placed (including the first/center one)
    /// </param>
    ///
    /// <param name="dTotalNumberOfVertices">
    /// Total number of vertices in the graph
    /// </param>
    //*************************************************************************

    private static void
    LayoutAllGroupsInU
    (
        Rectangle graphRectangle,
        IList<GroupInfo> sortedGroups,
        double alpha,
        double gamma,
        double ARs,
        Int32 iSortedGroupCount,
        Double dTotalNumberOfVertices
    )
    {
        aoGroupRectangles = new RectangleF[iSortedGroupCount]; 
        Boolean done = false;
        while (!done)
        {
        redoU:
            // Place the biggest group at the top center
            double ratioFirstGroup = gamma * ((double)sortedGroups[0].Vertices.Count / dTotalNumberOfVertices);

            Debug.Assert(ratioFirstGroup > 0);

            double AreaFirstGroup = ratioFirstGroup * graphRectangle.Width * graphRectangle.Height;
            double firstGroupWidth = Math.Sqrt(ratioFirstGroup * graphRectangle.Width * graphRectangle.Height / ARs);
            double firstGroupHeight = Math.Sqrt(ratioFirstGroup * graphRectangle.Width * graphRectangle.Height * ARs);
            double dGroupTop = (double)graphRectangle.Top;
            double dGroupBottom = dGroupTop + firstGroupHeight;
            double dGroupLeft = (double)graphRectangle.Left + ((double)(graphRectangle.Width - firstGroupWidth) / 2.0);
            double dGroupRight = dGroupLeft + firstGroupWidth;

            // Assign the boundaries to center(first) group's rectangle
            aoGroupRectangles[0] = RectangleF.FromLTRB((float)dGroupLeft,
            (float)dGroupTop, (float)dGroupRight, (float)dGroupBottom);

            // Calculate the free spaces. Constructor follows the Left, Top, Right, Bottom order
            FreeSpaceBox B2 = new FreeSpaceBox(graphRectangle.Left, (float)dGroupTop, (float)dGroupLeft, graphRectangle.Bottom, FreeSpaceBox.Orientation.Vert);
            FreeSpaceBox B3 = new FreeSpaceBox((float)dGroupLeft, (float)dGroupBottom, graphRectangle.Right, graphRectangle.Bottom, FreeSpaceBox.Orientation.Horiz);
            FreeSpaceBox B4 = new FreeSpaceBox((float)dGroupRight, (float)dGroupTop, graphRectangle.Right, (float)dGroupBottom, FreeSpaceBox.Orientation.Vert);

            // Place the boxes in the linked list
            freeSpaceBoxList = new LinkedList<FreeSpaceBox>();
            freeSpaceBoxList.AddFirst(B3);
            freeSpaceBoxList.AddLast(B2);
            freeSpaceBoxList.AddLast(B4);

            done = ArrangeRemainingGroupsInFreeSpaces(graphRectangle, sortedGroups, gamma, iSortedGroupCount, dTotalNumberOfVertices);
            if (!done)
            {
                gamma = gamma * alpha; // alpha has been set in Layouts->Layouts->LayoutBase.cs
                Debug.Assert(gamma > 0);
                for (int j = 0; j < iSortedGroupCount; j++)
                {
                    aoGroupRectangles[j] = RectangleF.Empty;
                    freeSpaceBoxList = new LinkedList<FreeSpaceBox>();
                }
                goto redoU;
            }
        }
        for (Int32 i = 0; i < iSortedGroupCount; i++)
        {
            sortedGroups[i].Rectangle = GraphicsUtil.RectangleFToRectangle(aoGroupRectangles[i], 1);
        }

        // Calculate the screen space wasted
        double totalFreeAreaLeft = 0;
        LinkedListNode<FreeSpaceBox> next = freeSpaceBoxList.First;
        while (next != null)
        {
            totalFreeAreaLeft = totalFreeAreaLeft + next.Value.GetArea();
            next = next.Next;
        }
    }
    
    
    //*************************************************************************
    //  Property: LayoutAllGroupsInDonut
    //
    /// <summary>
    /// Determines a rectangle for every group. Places one group at center and arranges the rest around it
    /// </summary>
    ///
    /// <param name="graphRectangle">
    /// The <see cref="Rectangle" /> the graph is being laid out within.
    /// </param>
    ///
    /// <param name="sortedGroups">
    /// List of <see cref="GroupInfo" /> objects, one for each group of
    /// vertices.  Must be sorted in descending order of the connectivity
    /// There must be at least one vertex in each group.
    /// </param>
    ///
    /// <param name="alpha">
    /// The factor by which gamma is to be reduced if all groups do not fit in the graph rectangle
    /// </param>
    /// 
    /// <param name="gamma">
    /// The fraction of space occupied by the first (center) group
    /// </param>
    ///
    /// <param name="ARs">
    /// Aspect ratio of graphRectangle
    /// </param>
    ///
    /// <param name="iSortedGroupCount">
    /// Total number of groups to be placed (including the first/center one)
    /// </param>
    ///
    /// <param name="dTotalNumberOfVertices">
    /// Total number of vertices in the graph
    /// </param>
    //*************************************************************************

    private static void
    LayoutAllGroupsInDonut
    (
        Rectangle graphRectangle,
        IList<GroupInfo> sortedGroups,
        double alpha,
        double gamma,
        double ARs,
        Int32 iSortedGroupCount,
        Double dTotalNumberOfVertices
    )
    {
        aoGroupRectangles = new RectangleF[iSortedGroupCount]; 
        Boolean done = false;
        while (!done)
        {
        redoDonut:
            // Place the biggest group at the center
            double ratioCenterGroup = gamma * ((double)sortedGroups[0].Vertices.Count / dTotalNumberOfVertices);
            Debug.Assert(ratioCenterGroup > 0);
            double AreaCenterGroup = ratioCenterGroup * graphRectangle.Width * graphRectangle.Height;
            double centerGroupWidth = Math.Sqrt(ratioCenterGroup * graphRectangle.Width * graphRectangle.Height / ARs);
            double centerGroupHeight = Math.Sqrt(ratioCenterGroup * graphRectangle.Width * graphRectangle.Height * ARs);
            double centerX = (graphRectangle.Left + graphRectangle.Right) / 2;
            double centerY = (graphRectangle.Top + graphRectangle.Bottom) / 2;
            Debug.Assert(centerGroupHeight > 0);
            Debug.Assert(centerGroupWidth > 0);

            double dGroupTop = centerY - (centerGroupHeight / 2);
            double dGroupBottom = centerY + (centerGroupHeight / 2);
            double dGroupLeft = centerX - (centerGroupWidth / 2);
            double dGroupRight = centerX + (centerGroupWidth / 2);

            // Assign the boundaries to center group's rectangle
            aoGroupRectangles[0] = RectangleF.FromLTRB((float)dGroupLeft,
            (float)dGroupTop, (float)dGroupRight, (float)dGroupBottom);

            // Calculate the free spaces. Constructor follows the Left, Top, Right, Bottom order
            FreeSpaceBox B1 = new FreeSpaceBox(graphRectangle.Left, graphRectangle.Top, graphRectangle.Right, (float)dGroupTop, FreeSpaceBox.Orientation.Horiz);
            FreeSpaceBox B2 = new FreeSpaceBox(graphRectangle.Left, (float)dGroupTop, (float)dGroupLeft, graphRectangle.Bottom, FreeSpaceBox.Orientation.Vert);
            FreeSpaceBox B3 = new FreeSpaceBox((float)dGroupLeft, (float)dGroupBottom, graphRectangle.Right, graphRectangle.Bottom, FreeSpaceBox.Orientation.Horiz);
            FreeSpaceBox B4 = new FreeSpaceBox((float)dGroupRight, (float)dGroupTop, graphRectangle.Right, (float)dGroupBottom, FreeSpaceBox.Orientation.Vert);

            // Place the boxes in the linked list
            freeSpaceBoxList = new LinkedList<FreeSpaceBox>();
            freeSpaceBoxList.AddFirst(B1);
            freeSpaceBoxList.AddLast(B3);
            freeSpaceBoxList.AddLast(B4);
            freeSpaceBoxList.AddLast(B2);
            
            done = ArrangeRemainingGroupsInFreeSpaces(graphRectangle, sortedGroups, gamma, iSortedGroupCount, dTotalNumberOfVertices);
            if (!done)
            {
                gamma = gamma * alpha; // ETM: alpha has been set in Layouts->Layouts->LayoutBase.cs
                Debug.Assert(gamma > 0);
                for (int j = 0; j < iSortedGroupCount; j++)
                {
                    aoGroupRectangles[j] = RectangleF.Empty;
                    freeSpaceBoxList = new LinkedList<FreeSpaceBox>();
                }
                goto redoDonut;
            }
        }
        for (Int32 i = 0; i < iSortedGroupCount; i++)
        {
            sortedGroups[i].Rectangle = GraphicsUtil.RectangleFToRectangle(aoGroupRectangles[i], 1);
        }
    }

    //*************************************************************************
    //  Property: ArrangeRemainingGroupsInFreeSpaces
    //
    /// <summary>
    /// Makes rectangle assignments to the groups left after placing the center/first group from the free space list
    /// </summary>
    ///
    /// <param name="graphRectangle">
    /// The <see cref="Rectangle" /> the graph is being laid out within.
    /// </param>
    ///
    /// <param name="sortedGroups">
    /// List of <see cref="GroupInfo" /> objects, one for each group of
    /// vertices.  Must be sorted in descending order of the connectivity
    /// There must be at least one vertex in each group.
    /// </param>
    ///
    /// <param name="gamma">
    /// The fraction of space occupied by the first (center) group
    /// </param>
    ///
    /// <param name="iSortedGroupCount">
    /// Total number of groups to be placed (including the first/center one)
    /// </param>
    ///
    /// <param name="dTotalNumberOfVertices">
    /// Total number of vertices in the graph
    /// </param>
    //*************************************************************************

    private static Boolean
    ArrangeRemainingGroupsInFreeSpaces
    (
        Rectangle graphRectangle,
        IList<GroupInfo> sortedGroups,
        double gamma,
        Int32 iSortedGroupCount,
        Double dTotalNumberOfVertices
    )
    {
        for (int i = 1; i < iSortedGroupCount; i++)
        {
            Boolean groupPlaced = false;
            double groupArea = gamma * ((double)sortedGroups[i].Vertices.Count / dTotalNumberOfVertices) * (graphRectangle.Width * graphRectangle.Height);

            if (freeSpaceBoxList == null || freeSpaceBoxList.Count <= 0)
            {
                return false;
            }

            // Spiral layout: Traverse the linked list and find the next free box that can accomodate this group
            LinkedListNode<FreeSpaceBox> node = freeSpaceBoxList.First;
            while (node != null)
            {
                if (node.Value.GetArea() >= groupArea)
                {                   
                    // Place the group in this node and recalculate free space
                    aoGroupRectangles[i] = PlaceGroupInFreeBox(sortedGroups[i], groupArea, node.Value);
                    RecalculateFreeSpace(node, aoGroupRectangles[i]);
                    groupPlaced = true;
                    break;
                }
                else
                {
                    node = node.Next;
                }
            }
            if (!groupPlaced)
                return false;
        }

        return true;
    }

    //*************************************************************************
    //  Property: PlaceGroupInFreeBox
    //
    /// <summary>
    /// Places the group in the free box provided as a parameter
    /// </summary>
    ///
    /// <param name="groupToBePlaced">
    ///The group to be placed
    /// </param>
    ///
    /// <param name="groupArea">
    ///Area of the group to be placed. It is passed as a parameter to reduce computation
    /// </param>
    ///
    /// <param name="freeBox">
    ///The free space box in which group is to be placed
    /// </param>
    //*************************************************************************

    private static RectangleF
    PlaceGroupInFreeBox
    (
        GroupInfo groupToBePlaced,
        double groupArea,
        FreeSpaceBox freeBox
    )
    {   double gLeft, gRight, gTop, gBottom, gHeight, gWidth;
        
        if (freeBox.orientation == FreeSpaceBox.Orientation.Vert)
        {
            gLeft = freeBox.boxRectangle.Left;
            gRight = freeBox.boxRectangle.Right;
            gTop = freeBox.boxRectangle.Top;
            gHeight = groupArea / ((double)(gRight - gLeft));
            gBottom = gTop + gHeight;
        }
        else
        {
            gLeft = freeBox.boxRectangle.Left;
            gTop = freeBox.boxRectangle.Top;
            gBottom = freeBox.boxRectangle.Bottom;
            gWidth = groupArea / ((double)Math.Abs(gTop - gBottom));
            gRight = gLeft + gWidth;
        }
        return RectangleF.FromLTRB((float)gLeft, (float)gTop, (float)gRight, (float)gBottom);
    }

    //*************************************************************************
    //  Method: RecalculateFreeSpace
    //
    /// <summary>
    /// Recalculates the free space
    /// </summary>
    ///
    /// <param name="freeSpaceBoxNode">
    ///The free space box in which group is was placed (this box is not in linked list anymore
    /// </param>
    /// 
    /// <param name="boxPlaced">
    ///The group rectangle that was placed in the above free space box
    /// </param>
    /// 
    /// <remarks>
    /// This  is for spiral layout
    /// </remarks>
    //*************************************************************************

    private static void
    RecalculateFreeSpace
    (
        LinkedListNode<FreeSpaceBox> freeSpaceBoxNode,
        RectangleF boxPlaced
    )
    {
        FreeSpaceBox freeSpaceBox = freeSpaceBoxNode.Value;

        float top, bottom, right, left;
        if (freeSpaceBox.orientation == FreeSpaceBox.Orientation.Horiz)
        {
            top = freeSpaceBox.boxRectangle.Top;
            left = boxPlaced.Right;
            right = freeSpaceBox.boxRectangle.Right;
            bottom = freeSpaceBox.boxRectangle.Bottom;
        }
        else
        {
            top = boxPlaced.Bottom;
            left = freeSpaceBox.boxRectangle.Left;
            right = freeSpaceBox.boxRectangle.Right;
            bottom = freeSpaceBox.boxRectangle.Bottom;
        }

        FreeSpaceBox newFreeSpaceBox = new FreeSpaceBox(left, top, right, bottom, freeSpaceBox.orientation);
        double newFreeSpaceBoxArea = newFreeSpaceBox.GetArea();

        freeSpaceBoxList.AddLast(newFreeSpaceBox);
        freeSpaceBoxList.Remove(freeSpaceBoxNode);
    }
}
}
