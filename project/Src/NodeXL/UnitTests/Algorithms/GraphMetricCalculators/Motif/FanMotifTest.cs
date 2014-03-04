
using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Smrf.NodeXL.Core;
using Smrf.NodeXL.Algorithms;

namespace Smrf.NodeXL.UnitTests
{
//*****************************************************************************
//  Class: FanMotifTest
//
/// <summary>
/// This is a Visual Studio test fixture for the <see cref="FanMotif" /> class.
/// </summary>
//*****************************************************************************

[TestClassAttribute]

public class FanMotifTest : Object
{
    //*************************************************************************
    //  Constructor: FanMotifTest()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="FanMotifTest" /> class.
    /// </summary>
    //*************************************************************************

    public FanMotifTest()
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
        IVertex oHeadVertex = new Vertex();
        IVertex oLeafVertex1 = new Vertex();
        IVertex oLeafVertex2 = new Vertex();

        FanMotif oFanMotif = new FanMotif( oHeadVertex,
            new IVertex[] {oLeafVertex1, oLeafVertex2} );

        Assert.AreEqual(oHeadVertex, oFanMotif.HeadVertex);
        Assert.AreEqual(2, oFanMotif.LeafVertices.Length);
        Assert.AreEqual( oLeafVertex1, oFanMotif.LeafVertices[0] );
        Assert.AreEqual( oLeafVertex2, oFanMotif.LeafVertices[1] );
        Assert.AreEqual(1.0, oFanMotif.ArcScale);
    }

    //*************************************************************************
    //  Method: TestAllVertices()
    //
    /// <summary>
    /// Tests the AllVertices property.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestAllVertices()
    {
        IVertex oHeadVertex = new Vertex();
        IVertex oLeafVertex1 = new Vertex();
        IVertex oLeafVertex2 = new Vertex();

        FanMotif oFanMotif = new FanMotif( oHeadVertex,
            new IVertex[] {oLeafVertex1, oLeafVertex2} );

        IVertex[] aoVerticesInMotif = oFanMotif.VerticesInMotif;

        Assert.AreEqual(2, aoVerticesInMotif.Length);

        Assert.AreEqual( oHeadVertex, oFanMotif.HeadVertex);

        Assert.AreEqual( oLeafVertex1, aoVerticesInMotif.Single(
            oVertex => oVertex == oLeafVertex1) );

        Assert.AreEqual( oLeafVertex2, aoVerticesInMotif.Single(
            oVertex => oVertex == oLeafVertex2) );
    }

    //*************************************************************************
    //  Method: TestCollapsedAttributes()
    //
    /// <summary>
    /// Tests the CollapsedAttributes property.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestCollapsedAttributes()
    {
        // With head vertex name.

        IVertex oHeadVertex = new Vertex();
        oHeadVertex.Name = "Head";

        IVertex oLeafVertex1 = new Vertex();
        IVertex oLeafVertex2 = new Vertex();

        FanMotif oFanMotif = new FanMotif( oHeadVertex,
            new IVertex[] {oLeafVertex1, oLeafVertex2} );

        String sCollapsedAttributes = oFanMotif.CollapsedAttributes;

        CollapsedGroupAttributes oCollapsedGroupAttributes =
            CollapsedGroupAttributes.FromString(sCollapsedAttributes);

        Assert.AreEqual( CollapsedGroupAttributeValues.FanMotifType,
            oCollapsedGroupAttributes[CollapsedGroupAttributeKeys.Type] );

        Assert.IsTrue(
            oCollapsedGroupAttributes.ContainsKey(
                CollapsedGroupAttributeKeys.HeadVertexName) );

        Assert.AreEqual( "Head",
            oCollapsedGroupAttributes[
                CollapsedGroupAttributeKeys.HeadVertexName] );

        Assert.AreEqual( "2",
            oCollapsedGroupAttributes[
                CollapsedGroupAttributeKeys.LeafVertices] );

        Assert.IsTrue(
            oCollapsedGroupAttributes.ContainsKey(
                CollapsedGroupAttributeKeys.ArcScale) );

        Assert.AreEqual( "1",
            oCollapsedGroupAttributes[CollapsedGroupAttributeKeys.ArcScale] );
    }

    //*************************************************************************
    //  Method: TestCollapsedAttributes2()
    //
    /// <summary>
    /// Tests the CollapsedAttributes property.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestCollapsedAttributes2()
    {
        // Without head vertex name, and with ArcScale set to a non-default
        // value.

        IVertex oHeadVertex = new Vertex();

        IVertex oLeafVertex1 = new Vertex();
        IVertex oLeafVertex2 = new Vertex();

        FanMotif oFanMotif = new FanMotif( oHeadVertex,
            new IVertex[] {oLeafVertex1, oLeafVertex2} );

        oFanMotif.ArcScale = 0.5;

        String sCollapsedAttributes = oFanMotif.CollapsedAttributes;

        CollapsedGroupAttributes oCollapsedGroupAttributes =
            CollapsedGroupAttributes.FromString(sCollapsedAttributes);

        Assert.AreEqual( CollapsedGroupAttributeValues.FanMotifType,
            oCollapsedGroupAttributes[CollapsedGroupAttributeKeys.Type] );

        Assert.IsFalse(
            oCollapsedGroupAttributes.ContainsKey(
                CollapsedGroupAttributeKeys.HeadVertexName) );

        Assert.AreEqual( "2",
            oCollapsedGroupAttributes[
                CollapsedGroupAttributeKeys.LeafVertices] );

        Assert.IsTrue(
            oCollapsedGroupAttributes.ContainsKey(
                CollapsedGroupAttributeKeys.ArcScale) );

        Assert.AreEqual( "0.5",
            oCollapsedGroupAttributes[CollapsedGroupAttributeKeys.ArcScale] );
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}

}
