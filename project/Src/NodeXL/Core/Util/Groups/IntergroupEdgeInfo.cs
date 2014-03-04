
using System;
using System.Diagnostics;

namespace Smrf.NodeXL.Core
{
//*****************************************************************************
//  Class: IntergroupEdgeInfo
//
/// <summary>
/// Contains information about the edges that connect the vertices in one
/// vertex group to the vertices in another vertex group.
/// </summary>
///
/// <remarks>
/// The groups are assumed to be stored in some external indexed collection.
/// This class stores the indexes of two groups, along with metrics for the
/// edges that connect the groups.
/// </remarks>
//*****************************************************************************

public class IntergroupEdgeInfo : Object
{
    //*************************************************************************
    //  Constructor: IntergroupEdgeInfo()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="IntergroupEdgeInfo" />
    /// class.
    /// </summary>
    ///
    /// <param name="group1Index">
    /// The index of the first group.
    /// </param>
    ///
    /// <param name="group2Index">
    /// The index of the second group.  Note that this can be the same as
    /// <paramref name="group1Index" />, in which case the object contains
    /// information about the edges that connect the vertices within a single
    /// group.
    /// </param>
    ///
    /// <param name="edges">
    /// The number of edges that connect the vertices in the first group with
    /// the vertices in the second group.  Must be greater than zero.
    /// </param>
    ///
    /// <param name="edgeWeightSum">
    /// The sum of the edge weights of the edges that connect the vertices in
    /// the first group with the vertices in the second group.  Must be greater
    /// than zero.
    /// </param>
    //*************************************************************************

    public IntergroupEdgeInfo
    (
        Int32 group1Index,
        Int32 group2Index,
        Int32 edges,
        Double edgeWeightSum
    )
    {
        m_iGroup1Index = group1Index;
        m_iGroup2Index = group2Index;
        m_iEdges = edges;
        m_dEdgeWeightSum = edgeWeightSum;

        AssertValid();
    }

    //*************************************************************************
    //  Property: Group1Index
    //
    /// <summary>
    /// Gets the index of the first group.
    /// </summary>
    ///
    /// <value>
    /// A zero-based index into a collection of groups.
    /// </value>
    //*************************************************************************

    public Int32
    Group1Index
    {
        get
        {
            AssertValid();

            return (m_iGroup1Index);
        }
    }

    //*************************************************************************
    //  Property: Group2Index
    //
    /// <summary>
    /// Gets the index of the second group.
    /// </summary>
    ///
    /// <value>
    /// A zero-based index into a collection of groups.
    /// </value>
    //*************************************************************************

    public Int32
    Group2Index
    {
        get
        {
            AssertValid();

            return (m_iGroup2Index);
        }
    }

    //*************************************************************************
    //  Property: Edges
    //
    /// <summary>
    /// Gets or sets the number of edges that connect the two groups.
    /// </summary>
    ///
    /// <value>
    /// The number of edges that connect the vertices in the first group with
    /// the vertices in the second group.  Must be greater than zero.
    /// </value>
    //*************************************************************************

    public Int32
    Edges
    {
        get
        {
            AssertValid();

            return (m_iEdges);
        }

        set
        {
            m_iEdges = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: EdgeWeightSum
    //
    /// <summary>
    /// Gets or sets the sum of the edge weights of the edges that connect the
    /// vertices.
    /// </summary>
    ///
    /// <value>
    /// The sum of the edge weights of the edges that connect the vertices in
    /// the first group with the vertices in the second group.  Must be greater
    /// than zero.
    /// </value>
    //*************************************************************************

    public Double
    EdgeWeightSum
    {
        get
        {
            AssertValid();

            return (m_dEdgeWeightSum);
        }

        set
        {
            m_dEdgeWeightSum = value;

            AssertValid();
        }
    }


    //*************************************************************************
    //  Method: AssertValid()
    //
    /// <summary>
    /// Asserts if the object is in an invalid state.  Debug-only.
    /// </summary>
    //*************************************************************************

    [Conditional("DEBUG")]

    public void
    AssertValid()
    {
        Debug.Assert(m_iGroup1Index >= 0);
        Debug.Assert(m_iGroup2Index >= 0);
        Debug.Assert(m_iEdges > 0);
        Debug.Assert(m_dEdgeWeightSum > 0);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// The index of the first group.

    protected Int32 m_iGroup1Index;

    /// The index of the second group.

    protected Int32 m_iGroup2Index;

    /// The number of edges that connect the two groups.

    protected Int32 m_iEdges;

    /// The sum of the edge weights of the edges that connect the vertices in
    /// the first group with the vertices in the second group.

    protected Double m_dEdgeWeightSum;
}

}
