
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
//  Class: GroupRectangleCalculator
//
/// <summary>
/// Calculates a rectangle for each group of vertices.
/// </summary>
///
/// <remarks>
/// This class is used when a LayoutStyle of <see
/// cref="LayoutStyle.UseGroups" /> is specified.
///
/// <para>
/// All methods are static.
/// </para>
///
/// </remarks>
//*****************************************************************************

public static class GroupRectangleCalculator
{
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
    /// <remarks>
    /// This method sets the <see cref="GroupInfo.Rectangle" /> property for
    /// each group in <paramref name="sortedGroups" />.  Some of the rectangles
    /// may be empty.
    /// </remarks>
    //*************************************************************************

    public static void
    CalculateGroupRectangles
    (
        Rectangle graphRectangle,
        IList<GroupInfo> sortedGroups
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
                Debug.Assert(iVertices <= iPreviousGroupVertices);
                iPreviousGroupVertices = iVertices;
            }
        }
        #endif

        Int32 iGroups = sortedGroups.Count;
        RectangleF [] aoGroupRectangles = new RectangleF[iGroups];
        Double dAreaPerVertex = GetAreaPerVertex(graphRectangle, sortedGroups);
        RectangleF oParentRectangle = graphRectangle;

        // Indexes of groups that have already been inserted into
        // parentRectangle, or -1 if none have been inserted yet.

        Int32 iIndexOfFirstInsertedGroup = -1;
        Int32 iIndexOfLastInsertedGroup = -1;

        // Index of the next group to insert.

        Int32 iIndexOfGroupToInsert = 0;

        // Sum of the vertex counts for the groups that have already been
        // inserted and the next group to insert.

        Double dVerticesSum = 0;

        // Worst aspect ratio of the groups that have already been inserted
        // into parentRectangle.  Aspect ratios are 1.0 or greater.  1.0 is
        // ideal.  Initialize this to a large value so that any other value
        // will be better.

        Double dOldWorstAspectRatio = Double.MaxValue;

        RectangleF [] aoSavedGroupRectangles = new RectangleF[iGroups];

        // Loop through all groups, inserting them one-by-one into
        // oParentRectangle.  Note that oParentRectangle gets shrunk at the end
        // of the loop whenever part of the rectangle gets filled with an
        // optimal arrangement of groups.  The loop then starts filling the
        // shrunken rectangle with the remaining groups.

        while (iIndexOfGroupToInsert < iGroups)
        {
            // Note that RectangleF.IsEmpty() returns true if either the width
            // or height is non-positive, as expected.

            if (oParentRectangle.IsEmpty)
            {
                // This shouldn't happen, but because of rounding errors,
                // conversions from Doubles to Singles, and use of the
                // Rectangle.FromLTRB() method (which has to convert its
                // arguments to an internal Point and Size), it occasionally
                // does.  Handle it by setting to empty the rectangles for all
                // groups that haven't been inserted yet.

                for (Int32 i = iIndexOfGroupToInsert; i < iGroups; i++)
                {
                    aoGroupRectangles[i] = RectangleF.Empty;
                }

                break;
            }

            GroupInfo oGroupToInsert = sortedGroups[iIndexOfGroupToInsert];
            Double dVerticesInGroupToInsert = oGroupToInsert.Vertices.Count;

            Debug.Assert(dVerticesInGroupToInsert > 0);

            Boolean bGroupWasInsertedBefore =
                (iIndexOfFirstInsertedGroup != -1);

            if (bGroupWasInsertedBefore)
            {
                // Save the current group rectangles.  Inserting a new group
                // causes the rectangles for the already-inserted groups to be
                // recalculated, and if the new rectangles are worse than the
                // old ones, the old ones will have to be restored.

                for (Int32 i = 0; i < iGroups; i++)
                {
                    aoSavedGroupRectangles[i] = aoGroupRectangles[i];
                }
            }

            dVerticesSum += dVerticesInGroupToInsert;

            InsertGroupsIntoRectangle(sortedGroups, aoGroupRectangles,
                oParentRectangle,

                (bGroupWasInsertedBefore ?
                    iIndexOfFirstInsertedGroup : iIndexOfGroupToInsert),

                iIndexOfGroupToInsert, dVerticesSum, dAreaPerVertex);

            // Compute the new worst aspect ratio.  Since the groups are sorted
            // by descending vertex count, the newly inserted group has the
            // worst one.

            Double dNewWorstAspectRatio = CalculateAspectRatio(
                aoGroupRectangles[iIndexOfGroupToInsert] );

            // Has it improved or at least stayed the same?

            if (dNewWorstAspectRatio <= dOldWorstAspectRatio)
            {
                // Yes, so leave the newly inserted group where it is and move
                // on to the next group.

                if (bGroupWasInsertedBefore)
                {
                    iIndexOfLastInsertedGroup++;
                }
                else
                {
                    iIndexOfFirstInsertedGroup = iIndexOfLastInsertedGroup =
                        iIndexOfGroupToInsert;
                }

                iIndexOfGroupToInsert++;
                dOldWorstAspectRatio = dNewWorstAspectRatio;
            }
            else
            {
                // The insertion of the new group made the aspect ratio worse,
                // so cancel what we just did.
            
                if (bGroupWasInsertedBefore)
                {
                    for (Int32 i = 0; i < iGroups; i++)
                    {
                        // Restore the previous rectangles.

                        aoGroupRectangles[i] = aoSavedGroupRectangles[i];
                    }
                }
            
                // Leave the previously inserted groups where they are, and get
                // a rectangle for the remaining empty space.  The next pass
                // through the loop will take the group that just failed and
                // insert it into the empty space.

                oParentRectangle = GetRemainingEmptySpace(sortedGroups,
                    aoGroupRectangles, oParentRectangle,
                    iIndexOfFirstInsertedGroup, iIndexOfLastInsertedGroup);

                iIndexOfFirstInsertedGroup = -1;
                iIndexOfLastInsertedGroup = -1;
                dVerticesSum = 0;
                dOldWorstAspectRatio = Double.MaxValue;
            }
        }

        for (Int32 i = 0; i < iGroups; i++)
        {
            sortedGroups[i].Rectangle =
                GraphicsUtil.RectangleFToRectangle(aoGroupRectangles[i], 1);
        }
    }

    //*************************************************************************
    //  Method: GetAreaPerVertex()
    //
    /// <summary>
    /// Returns the area within the graph rectangle that will be taken up by
    /// each vertex.
    /// </summary>
    ///
    /// <param name="oGraphRectangle">
    /// The <see cref="Rectangle" /> the graph is being laid out within.
    /// </param>
    ///
    /// <param name="oGroups">
    /// GroupInfo objects.
    /// </param>
    /// 
    /// <returns>
    /// Area within oGroupRectangle to allocate to each vertex.
    /// </returns>
    //*************************************************************************

    public static Double
    GetAreaPerVertex
    (
        Rectangle oGraphRectangle,
        IEnumerable<GroupInfo> oGroups
    )
    {
        Debug.Assert(oGraphRectangle.Width > 0);
        Debug.Assert(oGraphRectangle.Height > 0);
        Debug.Assert(oGroups != null);

        Int32 iVertices = oGroups.Sum(oGroupInfo => oGroupInfo.Vertices.Count);
    
        Debug.Assert(iVertices != 0);

        return ( (Double)(oGraphRectangle.Width * oGraphRectangle.Height) /
            (Double)iVertices);
    }

    //*************************************************************************
    //  Method: InsertGroupsIntoRectangle()
    //
    /// <summary>
    /// Inserts a set of groups into a parent rectangle.
    /// </summary>
    ///
    /// <param name="oSortedGroups">
    /// List of <see cref="GroupInfo" /> objects, one for each group of
    /// vertices.  Must be sorted in descending order of the number of vertices
    /// in each group.  There must be at least one vertex in each group.
    /// </param>
    ///
    /// <param name="aoGroupRectangles">
    /// Array of rectangles, one for each group in <paramref
    /// name="oSortedGroups" />.
    /// </param>
    ///
    /// <param name="oParentRectangle">
    /// Parent rectangle the groups should be laid out within.  Can't be empty
    /// -- must have positive width and height.
    /// </param>
    /// 
    /// <param name="iIndexOfFirstGroupToInsert">
    /// Zero-based index of the first group in <paramref
    /// name="oSortedGroups" /> to insert.
    /// </param>
    ///
    /// <param name="iIndexOfLastGroupToInsert">
    /// Zero-based index of the last group in <paramref
    /// name="oSortedGroups" /> to insert.
    /// </param>
    ///
    /// <param name="dVerticesSum">
    /// Sum of the vertex counts for the groups to be inserted.  Must be
    /// greater than zero.
    /// </param>
    ///
    /// <param name="dAreaPerVertex">
    /// The area within the graph rectangle that will be taken up by each
    /// vertex.
    /// </param>
    /// 
    /// <remarks>
    /// This method inserts the specified groups in <paramref
    /// name="oSortedGroups" /> into <paramref name="oParentRectangle" />.
    ///
    /// <para>
    /// If the rectangle is wider than it is tall, the groups are inserted on
    /// top of each other so that they fill the left part of the rectangle
    /// from top to bottom.  That probably leaves some space on the right side
    /// of the rectangle.
    /// </para>
    ///
    /// <para>
    /// If the rectangle is taller than it is wide, the groups are inserted to
    /// the right of each other so that they fill the or top of the rectangle,
    /// from left to right.  That probably leaves some space at the top or
    /// bottom of the rectangle.
    /// </para>
    ///
    /// <para>
    /// The algorithm presented in "Squarified Treemaps," by Mark Bruls, Kees
    /// Huizing, and Jarke J. van Wijk, always inserts larger groups at the
    /// lower left corner of the parent rectangle.  This method modifies the
    /// algorithm to insert larger groups at the upper left corner instead.
    /// </para>
    ///
    /// <para>
    /// Note that the results of this method call may not be optimal.  It's up
    /// to the caller to determine whether the results are good and to take
    /// corrective action if they are not.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    private static void
    InsertGroupsIntoRectangle
    (
        IList<GroupInfo> oSortedGroups,
        RectangleF [] aoGroupRectangles,
        RectangleF oParentRectangle,
        Int32 iIndexOfFirstGroupToInsert,
        Int32 iIndexOfLastGroupToInsert,
        Double dVerticesSum,
        Double dAreaPerVertex
    )
    {
        Debug.Assert(oSortedGroups != null);
        Debug.Assert(aoGroupRectangles != null);
        Debug.Assert(oSortedGroups.Count == aoGroupRectangles.Length);
        Debug.Assert(oParentRectangle.Width > 0);
        Debug.Assert(oParentRectangle.Height > 0);
        Debug.Assert(iIndexOfFirstGroupToInsert >= 0);
        Debug.Assert(iIndexOfLastGroupToInsert >= 0);
        Debug.Assert(iIndexOfLastGroupToInsert >= iIndexOfFirstGroupToInsert);
        Debug.Assert(iIndexOfLastGroupToInsert < oSortedGroups.Count);
        Debug.Assert(dVerticesSum > 0);
        Debug.Assert(dAreaPerVertex >= 0);

        Int32 i;

        // If the rectangle is wider than it is tall, the groups will get
        // inserted from bottom to top or top to bottom.  Otherwise, they will
        // get inserted from left to right.

        Boolean bInsertVertically =
            (oParentRectangle.Width >= oParentRectangle.Height);

        // Get the rectangle's height or width, whichever is shorter.

        Double dParentHeightOrWidth = bInsertVertically ?
            oParentRectangle.Height : oParentRectangle.Width;

        Debug.Assert(dParentHeightOrWidth != 0);

        // Compute the area that will be taken up by the groups.

        Double dGroupsArea = dAreaPerVertex * dVerticesSum;

        // Compute the width or height of the groups.  (They all have the same
        // width or height.)

        Double dGroupWidthOrHeight = dGroupsArea / dParentHeightOrWidth;

        Double dGroupLeft = 0;
        Double dGroupRight = 0;
        Double dGroupTop = 0;
        Double dGroupBottom = 0;

        if (bInsertVertically)
        {
            // Compute the left and right edges of the groups.  These don't
            // change.

            dGroupLeft = oParentRectangle.Left;
            dGroupRight = dGroupLeft + dGroupWidthOrHeight;

            // The top and bottom edges will be recalculated for each group.

            dGroupTop = dGroupBottom = oParentRectangle.Top;
        }
        else
        {
            // Compute the top and bottom edges of the groups.  These don't
            // change.

            dGroupTop = oParentRectangle.Top;
            dGroupBottom = dGroupTop + dGroupWidthOrHeight;

            // The left and right edges will be recalculated for each group.

            dGroupLeft = dGroupRight = oParentRectangle.Left;
        }

        for (i = iIndexOfFirstGroupToInsert;
            i <= iIndexOfLastGroupToInsert;
            i++)
        {
            GroupInfo oGroup = oSortedGroups[i];

            // The group's height or width is a fraction of the rectangle's
            // height or width, where the fraction is determined by the ratio
            // of the group's vertex count to the sum of values for all the
            // inserted groups.

            Debug.Assert(dVerticesSum != 0);

            Double dGroupHeightOrWidth = dParentHeightOrWidth
                * (oGroup.Vertices.Count / dVerticesSum);

            if (bInsertVertically)
            {
                dGroupBottom = dGroupTop + dGroupHeightOrWidth;
            }
            else
            {
                dGroupRight = dGroupLeft + dGroupHeightOrWidth;
            }

            aoGroupRectangles[i] = RectangleF.FromLTRB( (float)dGroupLeft,
                (float)dGroupTop, (float)dGroupRight, (float)dGroupBottom);

            Debug.Assert(aoGroupRectangles[i].Width >= 0);
            Debug.Assert(aoGroupRectangles[i].Height >= 0);

            if (bInsertVertically)
            {
                // The top of the next group should be at the bottom of this
                // group.

                dGroupTop = dGroupBottom;
            }
            else
            {
                // The left side of the next group should be to the right of
                // this group.

                dGroupLeft = dGroupRight;

            }
        }
    }

    //*************************************************************************
    //  Method: GetRemainingEmptySpace()
    //
    /// <summary>
    /// Returns a rectangle that contains the empty space not already filled by
    /// a set of inserted groups.
    /// </summary>
    ///
    /// <param name="oSortedGroups">
    /// List of <see cref="GroupInfo" /> objects, one for each group of
    /// vertices.  Must be sorted in descending order of the number of vertices
    /// in each group.  There must be at least one vertex in each group.
    /// </param>
    ///
    /// <param name="aoGroupRectangles">
    /// Array of rectangles, one for each group in <paramref
    /// name="oSortedGroups" />.
    /// </param>
    ///
    /// <param name="oParentRectangle">
    /// Parent rectangle the groups should be laid out within.  Can't be empty
    /// -- must have positive width and height.
    /// </param>
    /// 
    /// <param name="iIndexOfFirstInsertedGroup">
    /// Zero-based index of the first group that has already been inserted into
    /// <paramref name="oParentRectangle" />.
    /// </param>
    /// 
    /// <param name="iIndexOfLastInsertedGroup">
    /// Zero-based index of the last group that has already been inserted into
    /// <paramref name="oParentRectangle" />.
    /// </param>
    /// 
    /// <returns>
    /// New rectangle that contains the empty space.  May be empty.
    /// </returns>
    /// 
    /// <remarks>
    /// If there is no empty space left, an empty rectangle is returned.
    /// </remarks>
    //*************************************************************************

    private static RectangleF
    GetRemainingEmptySpace
    (
        IList<GroupInfo> oSortedGroups,
        RectangleF [] aoGroupRectangles,
        RectangleF oParentRectangle,
        Int32 iIndexOfFirstInsertedGroup,
        Int32 iIndexOfLastInsertedGroup
    )
    {
        Debug.Assert(oSortedGroups != null);
        Debug.Assert(aoGroupRectangles != null);
        Debug.Assert(oSortedGroups.Count == aoGroupRectangles.Length);
        Debug.Assert(oParentRectangle.Width > 0);
        Debug.Assert(oParentRectangle.Height > 0);
        Debug.Assert(iIndexOfFirstInsertedGroup >= 0);
        Debug.Assert(iIndexOfLastInsertedGroup >= 0);
        Debug.Assert(iIndexOfLastInsertedGroup >= iIndexOfFirstInsertedGroup);
        Debug.Assert(iIndexOfLastInsertedGroup < oSortedGroups.Count);
        
        RectangleF oRemainingEmptySpace;

        // Get the most recently inserted rectangle.

        RectangleF oLastInsertedRectangle =
            aoGroupRectangles[iIndexOfLastInsertedGroup];

        if (oParentRectangle.Width >= oParentRectangle.Height)
        {
            // Groups were added from top to bottom.  Move the left edge of the
            // rectangle to the right of the inserted groups.

            oRemainingEmptySpace = RectangleF.FromLTRB(
                oLastInsertedRectangle.Right,
                oParentRectangle.Top,
                oParentRectangle.Right,
                oParentRectangle.Bottom);
        }
        else
        {
            // Groups were added from left to right.  Move the top edge of the
            // rectangle to the bottom of the inserted groups.

            oRemainingEmptySpace = RectangleF.FromLTRB(
                oParentRectangle.Left,
                oLastInsertedRectangle.Bottom,
                oParentRectangle.Right,
                oParentRectangle.Bottom);
        }

        // If the rectangle's height or width is <= 0, return an empty
        // rectangle.

        if (oRemainingEmptySpace.IsEmpty)
        {
            oRemainingEmptySpace = RectangleF.Empty;
        }

        return (oRemainingEmptySpace);
    }

    //*************************************************************************
    //  Property: CalculateAspectRatio
    //
    /// <summary>
    /// Calculates the aspect ratio of a rectangle.
    /// </summary>
    ///
    /// <param name="oRectangle">
    /// The rectangle to calculate the aspect ratio for.
    /// </param>
    ///
    /// <returns>
    /// Aspect ratio of the rectangle.
    /// </returns>
    ///
    /// <remarks>
    /// The aspect ratio is the ratio of the rectangle's longer dimension to
    /// its shorter dimension.  If the shorter dimension has a length of zero,
    /// Double.MaxValue is returned.
    /// </remarks>
    //*************************************************************************

    private static Double
    CalculateAspectRatio
    (
        RectangleF oRectangle
    )
    {
        Single fWidth = oRectangle.Width;
        Single fHeight = oRectangle.Height;
        Single fLargerDimension, fSmallerDimension;

        if (fWidth > fHeight)
        {
            fLargerDimension = fWidth;
            fSmallerDimension = fHeight;
        }
        else
        {
            fLargerDimension = fHeight;
            fSmallerDimension = fWidth;
        }

        if (fSmallerDimension == 0)
        {
            return (Double.MaxValue);
        }

        return (fLargerDimension / fSmallerDimension);
    }
}

}
