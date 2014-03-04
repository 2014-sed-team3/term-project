
using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Smrf.NodeXL.Core;
using Smrf.NodeXL.Adapters;
using Smrf.AppLib;

namespace Smrf.NodeXL.UnitTests
{
//*****************************************************************************
//  Class: UcinetGraphAdapterTest
//
/// <summary>
/// This is a Visual Studio test fixture for the <see
/// cref="UcinetGraphAdapter" /> class.
/// </summary>
//*****************************************************************************

[TestClassAttribute]

public class UcinetGraphAdapterTest : Object
{
    //*************************************************************************
    //  Constructor: UcinetGraphAdapterTest()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="UcinetGraphAdapterTest" />
    /// class.
    /// </summary>
    //*************************************************************************

    public UcinetGraphAdapterTest()
    {
        m_oGraphAdapter = null;
        m_sTempFileName = null;
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
        m_oGraphAdapter = new UcinetGraphAdapter();

        m_sTempFileName = Path.GetTempFileName();
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
        m_oGraphAdapter = null;

        if ( File.Exists(m_sTempFileName) )
        {
            File.Delete(m_sTempFileName);
        }
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
        // (Do nothing.)
    }

    //*************************************************************************
    //  Method: TestLoadGraph()
    //
    /// <summary>
    /// Tests the LoadGraph(filename, fileDirectedness) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestLoadGraph()
    {
        // Directed file.

        const String FileContents =
            "DL\r\n"
            + "N=4\r\n"
            + "FORMAT = FULLMATRIX DIAGONAL PRESENT\r\n"
            + "ROW LABELS:\r\n"
            + "\"a\"\r\n"
            + "\"b\"\r\n"
            + "\"c\"\r\n"
            + "\"d\"\r\n"
            + "COLUMN LABELS:\r\n"
            + "\"a\"\r\n"
            + "\"b\"\r\n"
            + "\"c\"\r\n"
            + "\"d\"\r\n"
            + "DATA:\r\n"
            + "0 0 0 0\r\n"
            + "0 0 3 1\r\n"
            + "0 0 4.12 0\r\n"
            + "0 2 0 0\r\n"
            ;

        using ( StreamWriter oStreamWriter = new StreamWriter(m_sTempFileName) )
        {
            oStreamWriter.Write(FileContents);
        }

        IGraph oGraph = m_oGraphAdapter.LoadGraph(m_sTempFileName,
            GraphDirectedness.Directed);

        Assert.IsInstanceOfType( oGraph, typeof(Graph) );

        IVertexCollection oVertices = oGraph.Vertices;

        Assert.AreEqual(4, oVertices.Count);

        Assert.IsTrue( oVertices.Contains("a") );
        Assert.IsTrue( oVertices.Contains("b") );
        Assert.IsTrue( oVertices.Contains("c") );
        Assert.IsTrue( oVertices.Contains("d") );

        IEdgeCollection oEdges = oGraph.Edges;

        Assert.AreEqual(4, oEdges.Count);

        // The key is a concatenation of vertex names and the value is the
        // edge.

        Dictionary<String, IEdge> oEdgeDictionary =
            new Dictionary<String, IEdge>(4);

        foreach (IEdge oEdge in oEdges)
        {
            oEdgeDictionary.Add(
                oEdge.Vertices[0].Name + oEdge.Vertices[1].Name,
                oEdge);
        }

        Assert.AreEqual(4, oEdgeDictionary.Count);

        IEdge oFoundEdge;

        Assert.IsTrue( oEdgeDictionary.TryGetValue("db", out oFoundEdge) );

        Assert.AreEqual( 2.0, oFoundEdge.GetRequiredValue(
            ReservedMetadataKeys.EdgeWeight, typeof(Double) ) );

        Assert.IsTrue( oEdgeDictionary.TryGetValue("bd", out oFoundEdge) );

        Assert.AreEqual( 1.0, oFoundEdge.GetRequiredValue(
            ReservedMetadataKeys.EdgeWeight, typeof(Double) ) );

        Assert.IsTrue( oEdgeDictionary.TryGetValue("bc", out oFoundEdge) );

        Assert.AreEqual( 3.0, oFoundEdge.GetRequiredValue(
            ReservedMetadataKeys.EdgeWeight, typeof(Double) ) );

        Assert.IsTrue( oEdgeDictionary.TryGetValue("cc", out oFoundEdge) );

        Assert.AreEqual( 4.12, oFoundEdge.GetRequiredValue(
            ReservedMetadataKeys.EdgeWeight, typeof(Double) ) );
    }

    //*************************************************************************
    //  Method: TestLoadGraph2()
    //
    /// <summary>
    /// Tests the LoadGraph(filename, fileDirectedness) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestLoadGraph2()
    {
        // Undirected file.

        const String FileContents =
            "DL\r\n"
            + "N=4\r\n"
            + "FORMAT = FULLMATRIX DIAGONAL PRESENT\r\n"
            + "ROW LABELS:\r\n"
            + "\"a\"\r\n"
            + "\"b\"\r\n"
            + "\"c\"\r\n"
            + "\"d\"\r\n"
            + "COLUMN LABELS:\r\n"
            + "\"a\"\r\n"
            + "\"b\"\r\n"
            + "\"c\"\r\n"
            + "\"d\"\r\n"
            + "DATA:\r\n"
            + "0 0 0 0\r\n"
            + "0 0 3 1\r\n"
            + "0 3 4.12 0\r\n"
            + "0 1 0 0\r\n"
            ;

        using ( StreamWriter oStreamWriter = new StreamWriter(m_sTempFileName) )
        {
            oStreamWriter.Write(FileContents);
        }

        IGraph oGraph = m_oGraphAdapter.LoadGraph(m_sTempFileName,
            GraphDirectedness.Undirected);

        Assert.IsInstanceOfType( oGraph, typeof(Graph) );

        IVertexCollection oVertices = oGraph.Vertices;

        Assert.AreEqual(4, oVertices.Count);

        Assert.IsTrue( oVertices.Contains("a") );
        Assert.IsTrue( oVertices.Contains("b") );
        Assert.IsTrue( oVertices.Contains("c") );
        Assert.IsTrue( oVertices.Contains("d") );

        IEdgeCollection oEdges = oGraph.Edges;

        Assert.AreEqual(3, oEdges.Count);

        // The key is a concatenation of vertex names and the value is the
        // edge.

        Dictionary<String, IEdge> oEdgeDictionary =
            new Dictionary<String, IEdge>(3);

        foreach (IEdge oEdge in oEdges)
        {
            String sName0 = oEdge.Vertices[0].Name;
            String sName1 = oEdge.Vertices[1].Name;

            oEdgeDictionary.Add(

                sName0.CompareTo(sName1) > 0 ?
                    (sName1 + sName0) : (sName0 + sName1),

                oEdge);
        }

        Assert.AreEqual(3, oEdgeDictionary.Count);

        IEdge oFoundEdge;

        Assert.IsTrue( oEdgeDictionary.TryGetValue("bd", out oFoundEdge) );

        Assert.AreEqual( 1.0, oFoundEdge.GetRequiredValue(
            ReservedMetadataKeys.EdgeWeight, typeof(Double) ) );

        Assert.IsTrue( oEdgeDictionary.TryGetValue("bc", out oFoundEdge) );

        Assert.AreEqual( 3.0, oFoundEdge.GetRequiredValue(
            ReservedMetadataKeys.EdgeWeight, typeof(Double) ) );

        Assert.IsTrue( oEdgeDictionary.TryGetValue("cc", out oFoundEdge) );

        Assert.AreEqual( 4.12, oFoundEdge.GetRequiredValue(
            ReservedMetadataKeys.EdgeWeight, typeof(Double) ) );
    }

    //*************************************************************************
    //  Method: TestSaveGraph()
    //
    /// <summary>
    /// Tests the SaveGraph(IGraph, String) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestSaveGraph()
    {
        // Directed graph.

        IGraph oGraph = new Graph(GraphDirectedness.Directed);

        IVertexCollection oVertices = oGraph.Vertices;

        IEdgeCollection oEdges = oGraph.Edges;

        IVertex oVertexA = oVertices.Add();
        oVertexA.Name = "a";

        IVertex oVertexB = oVertices.Add();
        oVertexB.Name = "b";

        IVertex oVertexC = oVertices.Add();
        oVertexC.Name = "c";

        IVertex oVertexD = oVertices.Add();
        oVertexD.Name = "d";

        IEdge oEdge;

        oEdge = oEdges.Add(oVertexD, oVertexB, true);
        oEdge.SetValue(ReservedMetadataKeys.EdgeWeight, 2.0);

        oEdge = oEdges.Add(oVertexB, oVertexD, true);
        oEdge.SetValue(ReservedMetadataKeys.EdgeWeight, 1.0);

        oEdge = oEdges.Add(oVertexB, oVertexC, true);
        oEdge.SetValue(ReservedMetadataKeys.EdgeWeight, 3.0);

        oEdge = oEdges.Add(oVertexC, oVertexC, true);
        oEdge.SetValue(ReservedMetadataKeys.EdgeWeight, 4.12);

        m_oGraphAdapter.SaveGraph(oGraph, m_sTempFileName);

        String sFileContents = FileUtil.ReadTextFile(m_sTempFileName);

        const String ExpectedFileContents =
            "DL\r\n"
            + "N=4\r\n"
            + "FORMAT = FULLMATRIX DIAGONAL PRESENT\r\n"
            + "ROW LABELS:\r\n"
            + "\"a\"\r\n"
            + "\"b\"\r\n"
            + "\"c\"\r\n"
            + "\"d\"\r\n"
            + "COLUMN LABELS:\r\n"
            + "\"a\"\r\n"
            + "\"b\"\r\n"
            + "\"c\"\r\n"
            + "\"d\"\r\n"
            + "DATA:\r\n"
            + "0 0 0 0 \r\n"
            + "0 0 3 1 \r\n"
            + "0 0 4.12 0 \r\n"
            + "0 2 0 0 \r\n"
            ;

        Assert.AreEqual(ExpectedFileContents, sFileContents);
    }

    //*************************************************************************
    //  Method: TestSaveGraph2()
    //
    /// <summary>
    /// Tests the SaveGraph(IGraph, String) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestSaveGraph2()
    {
        // Undirected graph.

        IGraph oGraph = new Graph(GraphDirectedness.Undirected);

        IVertexCollection oVertices = oGraph.Vertices;

        IEdgeCollection oEdges = oGraph.Edges;

        IVertex oVertexA = oVertices.Add();
        oVertexA.Name = "a";

        IVertex oVertexB = oVertices.Add();
        oVertexB.Name = "b";

        IVertex oVertexC = oVertices.Add();
        oVertexC.Name = "c";

        IVertex oVertexD = oVertices.Add();
        oVertexD.Name = "d";

        IEdge oEdge;

        oEdge = oEdges.Add(oVertexD, oVertexB, false);
        oEdge.SetValue(ReservedMetadataKeys.EdgeWeight, 1.0);

        oEdge = oEdges.Add(oVertexB, oVertexC, false);
        oEdge.SetValue(ReservedMetadataKeys.EdgeWeight, 3.0);

        oEdge = oEdges.Add(oVertexC, oVertexC, false);
        oEdge.SetValue(ReservedMetadataKeys.EdgeWeight, 4.12);

        m_oGraphAdapter.SaveGraph(oGraph, m_sTempFileName);

        String sFileContents = FileUtil.ReadTextFile(m_sTempFileName);

        const String ExpectedFileContents =
            "DL\r\n"
            + "N=4\r\n"
            + "FORMAT = FULLMATRIX DIAGONAL PRESENT\r\n"
            + "ROW LABELS:\r\n"
            + "\"a\"\r\n"
            + "\"b\"\r\n"
            + "\"c\"\r\n"
            + "\"d\"\r\n"
            + "COLUMN LABELS:\r\n"
            + "\"a\"\r\n"
            + "\"b\"\r\n"
            + "\"c\"\r\n"
            + "\"d\"\r\n"
            + "DATA:\r\n"
            + "0 0 0 0 \r\n"
            + "0 0 3 1 \r\n"
            + "0 3 4.12 0 \r\n"
            + "0 1 0 0 \r\n"
            ;

        Assert.AreEqual(ExpectedFileContents, sFileContents);
    }

    //*************************************************************************
    //  Method: TestSupportsDirectedness()
    //
    /// <summary>
    /// Tests the SupportsDirectedness() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestSupportsDirectedness()
    {
        Assert.IsTrue( m_oGraphAdapter.SupportsDirectedness(
            GraphDirectedness.Undirected) );
    }

    //*************************************************************************
    //  Method: TestSupportsDirectedness2()
    //
    /// <summary>
    /// Tests the SupportsDirectedness() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestSupportsDirectedness2()
    {
        Assert.IsTrue( m_oGraphAdapter.SupportsDirectedness(
            GraphDirectedness.Directed) );
    }

    //*************************************************************************
    //  Method: TestSupportsDirectedness3()
    //
    /// <summary>
    /// Tests the SupportsDirectedness() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestSupportsDirectedness3()
    {
        Assert.IsFalse( m_oGraphAdapter.SupportsDirectedness(
            GraphDirectedness.Mixed) );
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Object to test.

    protected UcinetGraphAdapter m_oGraphAdapter;

    /// Name of the temporary file that may be created by the unit tests.

    protected String m_sTempFileName;
}

}
