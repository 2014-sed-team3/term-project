
using System;
using System.Diagnostics;
using Smrf.NodeXL.Core;

namespace Smrf.NodeXL.Algorithms
{
//*****************************************************************************
//  Class: FanMotif
//
/// <summary>
/// Represents a fan motif.
/// </summary>
///
/// <remarks>
/// See the <see cref="Motifs" /> enumeration for the definition of a fan
/// motif.  Note that a fan motif includes both its <see cref="HeadVertex" />
/// and its <see cref="LeafVertices" />.
///
/// <para>
/// This class is designed in a way that guarantees that an instance of the
/// class is a valid fan motif.
/// </para>
///
/// </remarks>
//*****************************************************************************

public class FanMotif : Motif
{
    //*************************************************************************
    //  Constructor: FanMotif()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="FanMotif" /> class.
    /// </summary>
    ///
    /// <param name="headVertex">
    /// The fan's head vertex.
    /// </param>
    ///
    /// <param name="leafVertices">
    /// An array of two or more leaf vertices.
    /// </param>
    //*************************************************************************

    public FanMotif
    (
        IVertex headVertex,
        IVertex [] leafVertices
    )
    {
        m_oHeadVertex = headVertex;
        m_aoLeafVertices = leafVertices;
        m_dArcScale = 1.0;

        AssertValid();
    }

    //*************************************************************************
    //  Property: HeadVertex
    //
    /// <summary>
    /// Gets the fan's head vertex.
    /// </summary>
    ///
    /// <value>
    /// The fan's head vertex, as an <see cref="IVertex" />.
    /// </value>
    //*************************************************************************

    public IVertex
    HeadVertex
    {
        get
        {
            AssertValid();

            return (m_oHeadVertex);
        }
    }

    //*************************************************************************
    //  Property: LeafVertices
    //
    /// <summary>
    /// Gets the fan's leaf vertices.
    /// </summary>
    ///
    /// <value>
    /// The fan's leaf vertices, as an array of at least two <see
    /// cref="IVertex" /> objects.
    /// </value>
    //*************************************************************************

    public IVertex[]
    LeafVertices
    {
        get
        {
            AssertValid();

            return (m_aoLeafVertices);
        }
    }

    //*************************************************************************
    //  Property: ArcScale
    //
    /// <summary>
    /// Gets or sets the scale factor to use when determining the arc of the
    /// vertex that represents the collapsed motif.
    /// </summary>
    ///
    /// <value>
    /// An arc scale factor between 0 and 1.0.  The code that draws the
    /// collapsed motif should draw a minimum arc when the scale factor is 0,
    /// and a maximum arc when the scale factor is 1.0.  The default is 1.0.
    /// </value>
    //*************************************************************************

    public Double
    ArcScale
    {
        get
        {
            AssertValid();

            return (m_dArcScale);
        }

        set
        {
            m_dArcScale = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: VerticesInMotif
    //
    /// <summary>
    /// Gets the motif's vertices.
    /// </summary>
    ///
    /// <value>
    /// The motif's vertices, as an array of <see cref="IVertex" /> objects.
    /// </value>
    //*************************************************************************

    public override IVertex[]
    VerticesInMotif
    {
        get
        {
            AssertValid();

            return m_aoLeafVertices;
        }
    }

    //*************************************************************************
    //  Property: CollapsedAttributes
    //
    /// <summary>
    /// Gets a string that describes what the motif should look like when it is
    /// collapsed.
    /// </summary>
    ///
    /// <value>
    /// A delimited set of key/value pairs.  Sample: "Key1=Value1|Key2=Value2".
    /// </value>
    ///
    /// <remarks>
    /// The returned string is created with the <see
    /// cref="CollapsedGroupAttributes" /> class.  The same class can be used
    /// later to parse the string.
    /// </remarks>
    //*************************************************************************

    public override String
    CollapsedAttributes
    {
        get
        {
            AssertValid();

            CollapsedGroupAttributes oCollapsedGroupAttributes =
                new CollapsedGroupAttributes();

            oCollapsedGroupAttributes.Add(CollapsedGroupAttributeKeys.Type,
                CollapsedGroupAttributeValues.FanMotifType);

            oCollapsedGroupAttributes.Add(
                CollapsedGroupAttributeKeys.HeadVertexName,
                m_oHeadVertex.Name);

            oCollapsedGroupAttributes.Add(
                CollapsedGroupAttributeKeys.LeafVertices,
                m_aoLeafVertices.Length);

            oCollapsedGroupAttributes.Add(
                CollapsedGroupAttributeKeys.ArcScale,
                m_dArcScale);

            return ( oCollapsedGroupAttributes.ToString() );
        }
    }


    //*************************************************************************
    //  Method: AssertValid()
    //
    /// <summary>
    /// Asserts if the object is in an invalid state.  Debug-only.
    /// </summary>
    //*************************************************************************

    // [Conditional("DEBUG")]

    public override void
    AssertValid()
    {
        base.AssertValid();

        Debug.Assert(m_oHeadVertex != null);
        Debug.Assert(m_aoLeafVertices != null);
        Debug.Assert(m_aoLeafVertices.Length >= 2);
        Debug.Assert(m_dArcScale >= 0);
        Debug.Assert(m_dArcScale <= 1.0);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// The fan's head vertex.

    protected IVertex m_oHeadVertex;

    /// An array of two or more leaf vertices.

    protected IVertex [] m_aoLeafVertices;

    /// An arc scale factor between 0 and 1.0.

    protected Double m_dArcScale;
}

}
