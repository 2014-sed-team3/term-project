
using System;
using System.Globalization;
using System.Diagnostics;
using Smrf.AppLib;

namespace Smrf.NodeXL.Core
{
//*****************************************************************************
//  Class: CollapsedGroupAttributes
//
/// <summary>
/// Stores attributes that describe how a collapsed group should be displayed.
/// </summary>
///
/// <remarks>
/// The attributes are stored as key/value pairs in a dictionary, where the
/// keys and values are strings.
///
/// <para>
/// The entire collection of attributes can be saved to a single delimited
/// string using PersistableStringDictionary.<see
/// cref="PersistableStringDictionary.ToString" />, and the collection can be
/// restored from the delimited string using <see cref="FromString" />.
/// </para>
///
/// </remarks>
//*****************************************************************************

public class CollapsedGroupAttributes : PersistableStringDictionary
{
    //*************************************************************************
    //  Constructor: CollapsedGroupAttributes()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="CollapsedGroupAttributes" /> class.
    /// </summary>
    //*************************************************************************

    public CollapsedGroupAttributes()
    {
        // (Do nothing.)

        AssertValid();
    }

    //*************************************************************************
    //  Method: GetGroupType()
    //
    /// <summary>
    /// Gets the type of the group, if it's available.
    /// </summary>
    ///
    /// <returns>
    /// The type of the group, or String.Empty if a type isn't available.
    /// </returns>
    ///
    /// <remarks>
    /// This returns the value of the <see
    /// cref="CollapsedGroupAttributeKeys.Type" /> key, if it's available.
    /// </remarks>
    //*************************************************************************

    public String
    GetGroupType()
    {
        AssertValid();

        String sType;

        if (
            TryGetValue(CollapsedGroupAttributeKeys.Type, out sType)
            &&
            !String.IsNullOrEmpty(sType)
            )
        {
            return (sType);
        }

        return (String.Empty);
    }

    //*************************************************************************
    //  Method: FromString()
    //
    /// <summary>
    /// Loads a new collection of key/value pairs from a string.
    /// </summary>
    ///
    /// <param name="attributes">
    /// The collection as a string.  Can be empty but not null.
    /// </param>
    ///
    /// <returns>
    /// A new collection.
    /// </returns>
    ///
    /// <remarks>
    /// <paramref name="attributes" /> should be a string that was returned
    /// from <see cref="PersistableStringDictionary.ToString" />.
    /// </remarks>
    //*************************************************************************

    public static new CollapsedGroupAttributes
    FromString
    (
        String attributes
    )
    {
        Debug.Assert(attributes != null);

        CollapsedGroupAttributes oCollapsedGroupAttributes =
            new CollapsedGroupAttributes();

        PopulateDictionary(oCollapsedGroupAttributes, attributes);

        return (oCollapsedGroupAttributes);
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

        // (Do nothing else.)
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}


//*****************************************************************************
//  Class: CollapsedGroupAttributeKeys
//
/// <summary>
/// Names of the keys stored in a <see cref="CollapsedGroupAttributes" />
/// object.
/// </summary>
///
/// <remarks>
/// For consistency, classes that store or retrieve key/value pairs from a <see
/// cref="CollapsedGroupAttributes" /> object should use the keys that are
/// specified by this class.
/// </remarks>
//*****************************************************************************

public static class CollapsedGroupAttributeKeys
{
    //*************************************************************************
    //  Method: GetAnchorVertexNameKey()
    //
    /// <summary>
    /// Gets the key for the name of a specified anchor vertex in a D-connector
    /// motif.
    /// </summary>
    ///
    /// <param name="anchorVertexIndex">
    /// Zero-based index of the anchor vertex.
    /// </param>
    ///
    /// <returns>
    /// The key for the name of the specified anchor vertex.
    /// </returns>
    //*************************************************************************

    public static String
    GetAnchorVertexNameKey
    (
        Int32 anchorVertexIndex
    )
    {
        Debug.Assert(anchorVertexIndex >= 0);

        return ( GetAnchorVertexKey(AnchorVertexNameBase, anchorVertexIndex) );
    }

    //*************************************************************************
    //  Method: GetAnchorVertexEdgeColorKey()
    //
    /// <summary>
    /// Gets the key for the color of the edges connecting the vertex used to
    /// represent a collapsed D-connector motif with one of its anchor vertices.
    /// </summary>
    ///
    /// <param name="anchorVertexIndex">
    /// Zero-based index of the anchor vertex.
    /// </param>
    ///
    /// <returns>
    /// The key for the edge color for the specified anchor vertex.
    /// </returns>
    //*************************************************************************

    public static String
    GetAnchorVertexEdgeColorKey
    (
        Int32 anchorVertexIndex
    )
    {
        Debug.Assert(anchorVertexIndex >= 0);

        return ( GetAnchorVertexKey(AnchorVertexEdgeColorBase,
            anchorVertexIndex) );
    }

    //*************************************************************************
    //  Method: GetAnchorVertexEdgeWidthKey()
    //
    /// <summary>
    /// Gets the key for the width of the edges connecting the vertex used to
    /// represent a collapsed D-connector motif with one of its anchor vertices.
    /// </summary>
    ///
    /// <param name="anchorVertexIndex">
    /// Zero-based index of the anchor vertex.
    /// </param>
    ///
    /// <returns>
    /// The key for the edge width for the specified anchor vertex.
    /// </returns>
    //*************************************************************************

    public static String
    GetAnchorVertexEdgeWidthKey
    (
        Int32 anchorVertexIndex
    )
    {
        Debug.Assert(anchorVertexIndex >= 0);

        return ( GetAnchorVertexKey(AnchorVertexEdgeWidthBase,
            anchorVertexIndex) );
    }

    //*************************************************************************
    //  Method: GetAnchorVertexKey()
    //
    /// <summary>
    /// Gets a key for a specified anchor vertex in a D-connector motif.
    /// </summary>
    ///
    /// <param name="sKeyBase">
    /// The base of the key.
    /// </param>
    ///
    /// <param name="iAnchorVertexIndex">
    /// Zero-based index of the anchor vertex.
    /// </param>
    ///
    /// <returns>
    /// A key for the specified anchor vertex.
    /// </returns>
    //*************************************************************************

    private static String
    GetAnchorVertexKey
    (
        String sKeyBase,
        Int32 iAnchorVertexIndex
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sKeyBase) );
        Debug.Assert(iAnchorVertexIndex >= 0);

        return ( sKeyBase
            + iAnchorVertexIndex.ToString(CultureInfo.InvariantCulture) );
    }


    //*************************************************************************
    //  Public constants
    //*************************************************************************

    /// <summary>
    /// The collapsed group's type.
    /// </summary>

    public const String Type = "Type";

    /// <summary>
    /// The color of the vertex used to represent a collapsed fan, D-connector,
    /// or clique motif.
    /// </summary>

    public const String VertexColor = "VertexColor";

    /// <summary>
    /// The name of the head vertex in a fan motif.
    /// </summary>

    public const String HeadVertexName = "HeadVertexName";

    /// <summary>
    /// The number of leaf vertices in a fan motif.
    /// </summary>

    public const String LeafVertices = "LeafVertices";

    /// <summary>
    /// The arc scale in a collapsed fan motif.
    /// </summary>

    public const String ArcScale = "ArcScale";

    /// <summary>
    /// The number of anchor vertices in a D-connector motif.
    /// </summary>

    public const String AnchorVertices = "AnchorVertices";

    /// <summary>
    /// The number of span vertices in a D-connector motif.
    /// </summary>

    public const String SpanVertices = "SpanVertices";

    /// <summary>
    /// The span scale in a collapsed D-connector motif.
    /// </summary>

    public const String SpanScale = "SpanScale";

    /// <summary>
    /// The color of the edges connecting the vertex used to represent a
    /// collapsed D-connector motif with its first anchor vertex.
    /// </summary>

    public const String AnchorVertex1EdgeColor = "AnchorVertex1EdgeColor";

    /// <summary>
    /// The color of the edges connecting the vertex used to represent a
    /// collapsed D-connector motif with its second anchor vertex.
    /// </summary>

    public const String AnchorVertex2EdgeColor = "AnchorVertex2EdgeColor";

    /// <summary>
    /// The width of the edges connecting the vertex used to represent a
    /// collapsed D-connector motif with its first anchor vertex.
    /// </summary>

    public const String AnchorVertex1EdgeWidth = "AnchorVertex1EdgeWidth";

    /// <summary>
    /// The width of the edges connecting the vertex used to represent a
    /// collapsed D-connector motif with its second anchor vertex.
    /// </summary>

    public const String AnchorVertex2EdgeWidth = "AnchorVertex2EdgeWidth";

    /// <summary>
    /// The number of member vertices in a clique motif.
    /// </summary>

    public const String CliqueVertices = "CliqueVertices";

    /// <summary>
    /// The clique scale in a collapsed clique motif.
    /// </summary>

    public const String CliqueScale = "CliqueScale";


    //*************************************************************************
    //  Private constants
    //*************************************************************************

    /// <summary>
    /// The base used for the name of an anchor vertex in a D-connector motif.
    /// The full key is AnchorVertexNameBase with an appended zero-based anchor
    /// vertex index: "AnchorVertexName0" and "AnchorVertexName1", for example.
    /// </summary>

    private const String AnchorVertexNameBase = "AnchorVertexName";

    /// <summary>
    /// The base used for the color of the edges connecting the vertex used to
    /// represent a collapsed D-connector motif with one of its anchor vertices.
    /// </summary>

    private const String AnchorVertexEdgeColorBase = "AnchorVertexEdgeColor";

    /// <summary>
    /// The base used for the width of the edges connecting the vertex used to
    /// represent a collapsed D-connector motif with one of its anchor vertices.
    /// </summary>

    private const String AnchorVertexEdgeWidthBase = "AnchorVertexEdgeWidth";
}


//*****************************************************************************
//  Class: CollapsedGroupAttributeValues
//
/// <summary>
/// Values stored in a <see cref="CollapsedGroupAttributes" /> object.
/// </summary>
///
/// <remarks>
/// For consistency, classes that store or retrieve key/value pairs from a <see
/// cref="CollapsedGroupAttributes" /> object should use the values that are
/// specified by this class.
/// </remarks>
//*****************************************************************************

public static class CollapsedGroupAttributeValues
{
    /// <summary>
    /// Value of the <see cref="CollapsedGroupAttributeKeys.Type" /> key for a
    /// collapsed fan motif.
    /// </summary>

    public const String FanMotifType = "FanMotif";

    /// <summary>
    /// Value of the <see cref="CollapsedGroupAttributeKeys.Type" /> key for a
    /// collapsed D-connector motif.
    /// </summary>

    public const String DConnectorMotifType = "DConnectorMotif";

    /// <summary>
    /// Value of the <see cref="CollapsedGroupAttributeKeys.Type" /> key for a
    /// collapsed clique motif.
    /// </summary>

    public const String CliqueMotifType = "CliqueMotif";
}

}
