
using System;
using System.Xml;
using System.Diagnostics;
using Smrf.XmlLib;

namespace Smrf.NodeXL.GraphMLLib
{
//*****************************************************************************
//  Class: NodeXLGraphMLUtil
//
/// <summary>
/// Utility methods for creating GraphML XML documents for use with the NodeXL
/// Excel Template.
/// </summary>
//*****************************************************************************

public static class NodeXLGraphMLUtil : Object
{
    //*************************************************************************
    //  Method: DefineEdgeRelationshipGraphMLAttribute()
    //
    /// <summary>
    /// Defines a GraphML-Attribute for edge relationships.
    /// </summary>
    ///
    /// <param name="graphMLXmlDocument">
    /// GraphMLXmlDocument being populated.
    /// </param>
    //*************************************************************************

    public static void
    DefineEdgeRelationshipGraphMLAttribute
    (
        GraphMLXmlDocument graphMLXmlDocument
    )
    {
        Debug.Assert(graphMLXmlDocument != null);

        graphMLXmlDocument.DefineEdgeStringGraphMLAttributes(
            EdgeRelationshipID, "Relationship");
    }

    //*************************************************************************
    //  Method: DefineVertexImageFileGraphMLAttribute()
    //
    /// <summary>
    /// Defines a GraphML-Attribute for vertex image files.
    /// </summary>
    ///
    /// <param name="graphMLXmlDocument">
    /// GraphMLXmlDocument being populated.
    /// </param>
    //*************************************************************************

    public static void
    DefineVertexImageFileGraphMLAttribute
    (
        GraphMLXmlDocument graphMLXmlDocument
    )
    {
        Debug.Assert(graphMLXmlDocument != null);

        graphMLXmlDocument.DefineVertexStringGraphMLAttributes(
            VertexImageFileID, VertexImageFileColumnName);
    }

    //*************************************************************************
    //  Method: DefineVertexLabelGraphMLAttribute()
    //
    /// <summary>
    /// Defines a GraphML-Attribute for vertex labels.
    /// </summary>
    ///
    /// <param name="graphMLXmlDocument">
    /// GraphMLXmlDocument being populated.
    /// </param>
    //*************************************************************************

    public static void
    DefineVertexLabelGraphMLAttribute
    (
        GraphMLXmlDocument graphMLXmlDocument
    )
    {
        Debug.Assert(graphMLXmlDocument != null);

        graphMLXmlDocument.DefineVertexStringGraphMLAttributes(
            VertexLabelID, VertexLabelColumnName);
    }

    //*************************************************************************
    //  Method: DefineVertexCustomMenuGraphMLAttributes()
    //
    /// <summary>
    /// Defines the GraphML-Attributes for vertex custom menu items.
    /// </summary>
    ///
    /// <param name="graphMLXmlDocument">
    /// GraphMLXmlDocument being populated.
    /// </param>
    //*************************************************************************

    public static void
    DefineVertexCustomMenuGraphMLAttributes
    (
        GraphMLXmlDocument graphMLXmlDocument
    )
    {
        Debug.Assert(graphMLXmlDocument != null);

        graphMLXmlDocument.DefineVertexStringGraphMLAttributes(
            VertexMenuTextID, VertexMenuTextColumnName, 
            VertexMenuActionID, VertexMenuActionColumnName
            );
    }

    //*************************************************************************
    //  Method: DefineImportedIDGraphMLAttribute()
    //
    /// <summary>
    /// Defines a GraphML-Attribute for imported IDs.
    /// </summary>
    ///
    /// <param name="graphMLXmlDocument">
    /// GraphMLXmlDocument being populated.
    /// </param>
    ///
    /// <param name="forEdges">
    /// true if the attribute is for edges, false if it is for vertices.
    /// </param>
    ///
    /// <remarks>
    /// See the definition of ImportedIDID for more details.
    /// </remarks>
    //*************************************************************************

    public static void
    DefineImportedIDGraphMLAttribute
    (
        GraphMLXmlDocument graphMLXmlDocument,
        Boolean forEdges
    )
    {
        Debug.Assert(graphMLXmlDocument != null);

        graphMLXmlDocument.DefineStringGraphMLAttributes(forEdges,
            ImportedIDID, "Imported ID");
    }

    //*************************************************************************
    //  Method: AppendEdgeXmlNode()
    //
    /// <summary>
    /// Appends an edge XML node to a GraphML document.
    /// </summary>
    ///
    /// <param name="graphMLXmlDocument">
    /// GraphMLXmlDocument being populated.
    /// </param>
    ///
    /// <param name="vertex1ID">
    /// ID of the edge's first vertex.
    /// </param>
    ///
    /// <param name="vertex2ID">
    /// ID of the edge's second vertex.
    /// </param>
    ///
    /// <param name="relationship">
    /// The relationship between the vertices.
    /// </param>
    ///
    /// <returns>
    /// The new edge XML node.
    /// </returns>
    //*************************************************************************

    public static XmlNode
    AppendEdgeXmlNode
    (
        GraphMLXmlDocument graphMLXmlDocument,
        String vertex1ID,
        String vertex2ID,
        String relationship
    )
    {
        Debug.Assert(graphMLXmlDocument != null);
        Debug.Assert( !String.IsNullOrEmpty(vertex1ID) );
        Debug.Assert( !String.IsNullOrEmpty(vertex2ID) );
        Debug.Assert( !String.IsNullOrEmpty(relationship) );

        XmlNode edgeXmlNode = graphMLXmlDocument.AppendEdgeXmlNode(
            vertex1ID, vertex2ID);

        graphMLXmlDocument.AppendGraphMLAttributeValue(edgeXmlNode,
            EdgeRelationshipID, relationship);

        return (edgeXmlNode);
    }


    //*************************************************************************
    //  Public GraphML-attribute IDs for edges
    //*************************************************************************

    ///
    public const String EdgeRelationshipID = "Relationship";


    //*************************************************************************
    //  Public GraphML-attribute IDs for vertices
    //*************************************************************************

    ///
    public const String VertexImageFileID = "Image";
    ///
    public const String VertexLabelID = "Label";
    ///
    public const String VertexMenuTextID = "MenuText";
    ///
    public const String VertexMenuActionID = "MenuAction";


    //*************************************************************************
    //  Public GraphML-attribute IDs for edges or vertices
    //*************************************************************************

    /// This is a special GraphML-attribute that specifies the unique imported
    /// ID for an imported edge or vertex.  It is used by the Excel Template to
    /// skip over multiple rows that represent the same edge or vertex.
    ///
    /// For example, the Twitter search network can create multiple edges for
    /// the same tweet.  It sets the ImportedIDID GraphML-attribute on the
    /// edges to a tweet ID, so the Excel Template can determine which edges
    /// represent the same tweet.

    public const String ImportedIDID = "ImportedID";


    //*************************************************************************
    //  Private names of columns that are built into the NodeXL Excel template
    //*************************************************************************

    private const String VertexImageFileColumnName = "Image File";

    private const String VertexLabelColumnName = "Label";

    private const String VertexMenuTextColumnName = "Custom Menu Item Text";

    private const String VertexMenuActionColumnName =
        "Custom Menu Item Action";
}

}
