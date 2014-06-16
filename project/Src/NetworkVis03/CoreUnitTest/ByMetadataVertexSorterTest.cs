
using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Smrf.NodeXL.Core;
using Smrf.NodeXL.Common;

namespace Smrf.NodeXL.UnitTests
{
//*****************************************************************************
//  Class: ByMetadataVertexSorterTest
//
/// <summary>
/// This is a Visual Studio test fixture for the <see
/// cref="ByMetadataVertexSorter" /> class.
/// </summary>
//*****************************************************************************

[TestClassAttribute]

public class ByMetadataVertexSorterTest : Object
{
    //*************************************************************************
    //  Constructor: ByMetadataVertexSorterTest()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="ByMetadataVertexSorterTest" /> class.
    /// </summary>
    //*************************************************************************

    public ByMetadataVertexSorterTest()
    {
        m_oByMetadataVertexSorter = null;
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
        m_oByMetadataVertexSorter = new ByMetadataVertexSorter<Int32>(SortKey);
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
        m_oByMetadataVertexSorter = null;
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
        Assert.AreEqual(SortKey, m_oByMetadataVertexSorter.SortKey);
        Assert.IsTrue(m_oByMetadataVertexSorter.SortAscending);
    }

   

    //*************************************************************************
    //  Method: TestSortKey()
    //
    /// <summary>
    /// Tests the SortKey property.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestSortKey()
    {
        const String SortKey2 = "jkrejkre";

        m_oByMetadataVertexSorter.SortKey = SortKey2;

        Assert.AreEqual(SortKey2, m_oByMetadataVertexSorter.SortKey);
    }

    

    //*************************************************************************
    //  Method: TestSortAscending()
    //
    /// <summary>
    /// Tests the SortAscending property.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestSortAscending()
    {
        m_oByMetadataVertexSorter.SortAscending = false;

        Assert.IsFalse(m_oByMetadataVertexSorter.SortAscending);

        m_oByMetadataVertexSorter.SortAscending = true;

        Assert.IsTrue(m_oByMetadataVertexSorter.SortAscending);
    }

    //*************************************************************************
    //  Method: TestSort()
    //
    /// <summary>
    /// Tests the Sort() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestSort()
    {
        // Ascending sort on Int32.

        const Int32 Vertices = 100;

        IGraph oGraph = new Graph();

        IVertex [] aoUnsortedVertices =
            TestGraphUtil.AddVertices(oGraph, Vertices);

        IVertexCollection oVertexCollection = oGraph.Vertices;

        Int32 i;

        for (i = 0; i < Vertices; i++)
        {
            aoUnsortedVertices[i].SetValue(SortKey, Vertices - i);
        }

        ICollection<IVertex> oSortedVertices =
            m_oByMetadataVertexSorter.Sort(oVertexCollection);

        Assert.AreEqual(Vertices, oSortedVertices.Count);

        i = 0;

        foreach (IVertex oSortedVertex in oSortedVertices)
        {
            Assert.AreEqual(
                i + 1,
                (Int32)oSortedVertex.GetRequiredValue(SortKey, typeof(Int32) )
                );

            i++;
        }
    }

    //*************************************************************************
    //  Method: TestSort2()
    //
    /// <summary>
    /// Tests the Sort() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestSort2()
    {
        // Descending sort on Int32.

        const Int32 Vertices = 100;

        IGraph oGraph = new Graph();

        IVertex [] aoUnsortedVertices =
            TestGraphUtil.AddVertices(oGraph, Vertices);

        IVertexCollection oVertexCollection = oGraph.Vertices;

        Int32 i;

        for (i = 0; i < Vertices; i++)
        {
            aoUnsortedVertices[i].SetValue(SortKey, Vertices - i);
        }

        m_oByMetadataVertexSorter.SortAscending = false;

        ICollection<IVertex> oSortedVertices =
            m_oByMetadataVertexSorter.Sort(oVertexCollection);

        Assert.AreEqual(Vertices, oSortedVertices.Count);

        i = 0;

        foreach (IVertex oSortedVertex in oSortedVertices)
        {
            Assert.AreEqual(
                Vertices - i,
                (Int32)oSortedVertex.GetRequiredValue( SortKey, typeof(Int32) )
                );

            i++;
        }
    }

    //*************************************************************************
    //  Method: TestSort3()
    //
    /// <summary>
    /// Tests the Sort() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestSort3()
    {
        // Ascending sort on Double.

        ByMetadataVertexSorter<Double> oByMetadataVertexSorter =
            new ByMetadataVertexSorter<Double>(SortKey);

        const Int32 Vertices = 100;

        IGraph oGraph = new Graph();

        IVertex [] aoUnsortedVertices =
            TestGraphUtil.AddVertices(oGraph, Vertices);

        IVertexCollection oVertexCollection = oGraph.Vertices;

        Int32 i;

        for (i = 0; i < Vertices; i++)
        {
            aoUnsortedVertices[i].SetValue( SortKey, (Double)(Vertices - i) );
        }

        ICollection<IVertex> oSortedVertices =
            oByMetadataVertexSorter.Sort(oVertexCollection);

        Assert.AreEqual(Vertices, oSortedVertices.Count);

        i = 0;

        foreach (IVertex oSortedVertex in oSortedVertices)
        {
            Assert.AreEqual(
                (Double)i + 1,

                (Double)oSortedVertex.GetRequiredValue(
                    SortKey, typeof(Double) )
                );

            i++;
        }
    }

    //*************************************************************************
    //  Method: TestSort4()
    //
    /// <summary>
    /// Tests the Sort() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestSort4()
    {
        // Descending sort on Double.

        ByMetadataVertexSorter<Double> oByMetadataVertexSorter =
            new ByMetadataVertexSorter<Double>(SortKey);

        const Int32 Vertices = 100;

        IGraph oGraph = new Graph();

        IVertex [] aoUnsortedVertices =
            TestGraphUtil.AddVertices(oGraph, Vertices);

        IVertexCollection oVertexCollection = oGraph.Vertices;

        Int32 i;

        for (i = 0; i < Vertices; i++)
        {
            aoUnsortedVertices[i].SetValue( SortKey, (Double)(Vertices - i) );
        }

        oByMetadataVertexSorter.SortAscending = false;

        for (i = 0; i < Vertices; i++)
        {
            aoUnsortedVertices[i].SetValue( SortKey, (Double)(Vertices - i) );
        }

        ICollection<IVertex> oSortedVertices =
            oByMetadataVertexSorter.Sort(oVertexCollection);

        Assert.AreEqual(Vertices, oSortedVertices.Count);

        i = 0;

        foreach (IVertex oSortedVertex in oSortedVertices)
        {
            Assert.AreEqual(

                (Double)(Vertices - i),

                (Double)oSortedVertex.GetRequiredValue(
                    SortKey, typeof(Double) )
                );

            i++;
        }
    }

   

    

    //*************************************************************************
    //  Protected constants
    //*************************************************************************

    protected const String SortKey = "abcdefg";


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // Object being tested.

    protected ByMetadataVertexSorter<Int32> m_oByMetadataVertexSorter;
}

}
