
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Smrf.NodeXL.Core;

namespace Smrf.NodeXL.Algorithms
{
//*****************************************************************************
//  Class: CliqueMotif
//
/// <summary>
/// Represents a clique motif.
/// </summary>
///
/// <remarks>
/// See the <see cref="Motifs" /> enumeration for the definition of a clique
/// motif.  Note that a clique motif includes all its <see cref="MemberVertices" />.
///
/// </remarks>
//*****************************************************************************

public class CliqueMotif : Motif
{
    //*************************************************************************
    //  Constructor: CliqueMotif()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="CliqueMotif" /> class.
    /// </summary>
    ///
    /// <param name="memberVertices">
    /// The vertices that are members of this clique.
    /// </param>
    //*************************************************************************

    public CliqueMotif
    (
        List<IVertex> memberVertices
    )
    {
        m_oMemberVertices = memberVertices;
        m_dCliqueScale = 1.0;

        AssertValid();
    }

    //*************************************************************************
    //  Property: MemberVertices
    //
    /// <summary>
    /// Gets the motif's member vertices.
    /// </summary>
    ///
    /// <value>
    /// The motif's member vertices, as a List of <see cref="IVertex" />
    /// objects.
    /// </value>
    //*************************************************************************

    public List<IVertex>
    MemberVertices
    {
        get
        {
            AssertValid();

            return (m_oMemberVertices);
        }
    }

    //*************************************************************************
    //  Property: CliqueScale
    //
    /// <summary>
    /// Gets or sets the scale factor to use when determining the size of the
    /// vertex that represents the collapsed motif.
    /// </summary>
    ///
    /// <value>
    /// A clique scale factor between 0 and 1.0.  The code that draws the
    /// collapsed motif should use a minimum size when the scale factor is 0,
    /// and a maximum size when the scale factor is 1.0.  The default is 1.0.
    /// </value>
    //*************************************************************************

    public Double
    CliqueScale
    {
        get
        {
            AssertValid();

            return (m_dCliqueScale);
        }

        set
        {
            m_dCliqueScale = value;

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

            // Only the span vertices are part of the motif.

            return (m_oMemberVertices.ToArray());
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
                CollapsedGroupAttributeValues.CliqueMotifType);

            oCollapsedGroupAttributes.Add(
                CollapsedGroupAttributeKeys.CliqueVertices,
                m_oMemberVertices.Count);

            oCollapsedGroupAttributes.Add(
                CollapsedGroupAttributeKeys.CliqueScale,
                m_dCliqueScale);

            return (oCollapsedGroupAttributes.ToString());
        }
    }

    private string VertexCollToNames
    (
        ICollection<IVertex> collection
    )
    {
        string ret = "";
        bool first = false;
        foreach (IVertex v in collection)
        {
            ret += (first ? "" : ", ") + v.Name;
            first = false;
        }
        return ret;
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

        Debug.Assert(m_oMemberVertices != null);
        Debug.Assert(m_oMemberVertices.Count >= 4);
        //Verify members are all connected to each other
        foreach (Vertex src in m_oMemberVertices)
        {
            foreach (Vertex tar in m_oMemberVertices)
            {
                if (src != tar)
                {
                    // Checking either direction (undirected clique)
                    Debug.Assert(
                        src.AdjacentVertices.Contains(tar) == true
                        ||
                        tar.AdjacentVertices.Contains(src) == true
                    );
                }
            }
        }
        Debug.Assert(m_dCliqueScale >= 0);
        Debug.Assert(m_dCliqueScale <= 1.0);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// The motif's member vertices.

    protected List<IVertex> m_oMemberVertices;

    /// A clique glyph scale factor between 0 and 1.0.

    protected Double m_dCliqueScale;
}

}
