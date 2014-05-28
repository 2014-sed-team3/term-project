
using System;
using System.Diagnostics;
using Smrf.NodeXL.Core;

namespace Smrf.NodeXL.Algorithms
{
//*****************************************************************************
//  Class: Motif
//
/// <summary>
/// Base class for a family of classes that represent motifs.
/// </summary>
//*****************************************************************************

public abstract class Motif : Object
{
    //*************************************************************************
    //  Constructor: Motif()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="Motif" /> class.
    /// </summary>
    //*************************************************************************

    public Motif()
    {
        // (Do nothing.)

        // AssertValid();
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

    public abstract IVertex[]
    VerticesInMotif
    {
        get;
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

    public abstract String
    CollapsedAttributes
    {
        get;
    }


    //*************************************************************************
    //  Method: AssertValid()
    //
    /// <summary>
    /// Asserts if the object is in an invalid state.  Debug-only.
    /// </summary>
    //*************************************************************************

    [Conditional("DEBUG")]

    public virtual void
    AssertValid()
    {
        // (Do nothing.)
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}

}
