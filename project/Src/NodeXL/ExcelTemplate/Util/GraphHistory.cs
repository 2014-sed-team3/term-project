
using System;
using System.Diagnostics;
using Smrf.AppLib;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: GraphHistory
//
/// <summary>
/// Stores attributes that describe how a graph was created.
/// </summary>
///
/// <remarks>
/// The attributes are stored as key/value pairs in a dictionary, where the
/// keys and values are strings.  All the key/value pairs are optional and may
/// be missing from the dictionary.
///
/// <para>
/// The entire collection of attributes can be saved to a single delimited
/// string using <see cref="PersistableStringDictionary.ToString" />, and
/// the collection can be restored from the delimited string using <see
/// cref="FromString" />.
/// </para>
///
/// </remarks>
//*****************************************************************************

public class GraphHistory : PersistableStringDictionary
{
    //*************************************************************************
    //  Constructor: GraphHistory()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="GraphHistory" /> class.
    /// </summary>
    //*************************************************************************

    public GraphHistory()
    {
        // (Do nothing.)

        AssertValid();
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

    public static new GraphHistory
    FromString
    (
        String attributes
    )
    {
        Debug.Assert(attributes != null);

        GraphHistory oGraphHistory = new GraphHistory();

        PopulateDictionary(oGraphHistory, attributes);

        return (oGraphHistory);
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
//  Class: GraphHistoryKeys
//
/// <summary>
/// Names of the keys stored in a <see cref="GraphHistory" /> object.
/// </summary>
///
/// <remarks>
/// For consistency, classes that store or retrieve key/value pairs from a <see
/// cref="GraphHistory" /> object should use the keys that are specified by
/// this class.
/// </remarks>
//*****************************************************************************

public static class GraphHistoryKeys
{
    /// <summary>
    /// A detailed description of how the graph was imported into the workbook.
    /// </summary>

    public const String ImportDescription = "ImportDescription";

    /// <summary>
    /// If the graph was imported into the workbook, this is a suggested file
    /// name for the workbook, without a path or extension.
    /// </summary>

    public const String ImportSuggestedFileNameNoExtension =
        "ImportSuggestedFileNameNoExtension";

    /// <summary>
    /// The technique that was used to group the graph's vertices.
    /// </summary>

    public const String GroupingDescription = "GroupingDescription";

    /// <summary>
    /// The graph's directedness.
    /// </summary>

    public const String GraphDirectedness = "GraphDirectedness";

    /// <summary>
    /// The algorithm that was used to lay out the graph.
    /// </summary>

    public const String LayoutAlgorithm = "LayoutAlgorithm";

    /// <summary>
    /// Comments provided by the user.
    /// </summary>

    public const String Comments = "Comments";
}

}
