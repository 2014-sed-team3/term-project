﻿using System;
using System.Xml;
using System.Globalization;
using System.Diagnostics;

namespace Smrf.XmlLib
{
//*****************************************************************************
//  Class: GraphMLXmlDocument
//
/// <summary>
/// Represents an XML document containing GraphML that represents a graph.
/// </summary>
///
/// <remarks>
/// See the "GraphML Primer" for details on the GraphML XML schema:
///
/// <para>
/// http://graphml.graphdrawing.org/primer/graphml-primer.html
/// </para>
///
/// <para>
/// Creating a <see cref="GraphMLXmlDocument" /> automatically creates an XML
/// declaration, a root "graphml" XML node, and a "graph" child XML node.  Use
/// <see cref="DefineGraphMLAttribute" />, <see cref="AppendVertexXmlNode" />,
/// <see cref="AppendEdgeXmlNode" />, and <see
/// cref="AppendGraphMLAttributeValue(XmlNode, String, String)" /> to populate
/// the document with vertices, edges, and vertex/edge attributes.
/// </para>
///
/// </remarks>
//*****************************************************************************

public class GraphMLXmlDocument : XmlDocument
{
   //*************************************************************************
    //  Constructor: GraphMLXmlDocument()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="GraphMLXmlDocument" />
    /// class.
    /// </summary>
    ///
    /// <param name="directed">
    /// true if the graph is directed, false if it is undirected.
    /// </param>
    //*************************************************************************

    public GraphMLXmlDocument
    (
        Boolean directed
    )
    {
        this.AppendChild( this.CreateXmlDeclaration("1.0", "UTF-8", null) );

        // (The XML Schema reference presented in the Primer is optional and is
        // skipped here.)

        // Add the root XML node.

        m_oGraphMLXmlNode = AppendXmlNode(this, "graphml");

        // Add the graph node and its required directedness value.

        m_oGraphXmlNode = AppendXmlNode(m_oGraphMLXmlNode, "graph");

        SetXmlNodeAttributes(m_oGraphXmlNode, "edgedefault",
            directed ? "directed" : "undirected");

        m_iVertexXmlNodes = 0;

        AssertValid();
    }

    //*************************************************************************
    //  Property: GraphXmlNode
    //
    /// <summary>
    /// Gets the "graph" XML node.
    /// </summary>
    ///
    /// <value>
    /// The XML node for the "graph" GraphML element.
    /// </value>
    //*************************************************************************

    public XmlNode
    GraphXmlNode
    {
        get
        {
            AssertValid();

            return (m_oGraphXmlNode);
        }
    }

    //*************************************************************************
    //  Property: HasVertexXmlNode
    //
    /// <summary>
    /// Gets a flag indicating whether the document has at least one XML node
    /// that represents a vertex.
    /// </summary>
    ///
    /// <value>
    /// true if the document has at least one XML node that represents a
    /// vertex, false if there are no such XML nodes.
    /// </value>
    //*************************************************************************

    public Boolean
    HasVertexXmlNode
    {
        get
        {
            AssertValid();

            return ( GetHasVertexXmlNode(this) );
        }
    }

    //*************************************************************************
    //  Property: VertexXmlNodes
    //
    /// <summary>
    /// Gets the number of XML nodes that represent a vertex.
    /// </summary>
    ///
    /// <value>
    /// The number of XML nodes that represent a vertex.
    /// </value>
    //*************************************************************************

    public Int32
    VertexXmlNodes
    {
        get
        {
            AssertValid();

            return (m_iVertexXmlNodes);
        }
    }

    //*************************************************************************
    //  Method: DefineGraphMLAttribute()
    //
    /// <summary>
    /// Defines a GraphML-Attribute.
    /// </summary>
    ///
    /// <param name="forEdge">
    /// true if the attribute is for edges, false if it is for vertices.
    /// </param>
    /// 
    /// <param name="attributeID">
    /// The attribute's ID.
    /// </param>
    /// 
    /// <param name="attributeName">
    /// The attribute's name
    /// </param>
    /// 
    /// <param name="attributeType">
    /// The attribute's type  Must be "boolean," "int," "long," "float,"
    /// "double," or "string."
    /// </param>
    /// 
    /// <param name="defaultAttributeValue">
    /// Default attribute value, or null there is no default.
    /// </param>
    //*************************************************************************

    public void
    DefineGraphMLAttribute
    (
        Boolean forEdge,
        String attributeID,
        String attributeName,
        String attributeType,
        String defaultAttributeValue
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(attributeID) );
        Debug.Assert( !String.IsNullOrEmpty(attributeName) );
        Debug.Assert( !String.IsNullOrEmpty(attributeType) );
        AssertValid();

        XmlNode oGraphMLAttributeXmlNode =
            AppendXmlNode(m_oGraphMLXmlNode, "key");

        SetXmlNodeAttributes(oGraphMLAttributeXmlNode, 
            "id", attributeID,
            "for", forEdge ? "edge" : "node",
            "attr.name", attributeName,
            "attr.type", attributeType
            );

        if ( !String.IsNullOrEmpty(defaultAttributeValue) )
        {
            AppendXmlNode(oGraphMLAttributeXmlNode, "default",
                defaultAttributeValue);
        }

        // Make all "key" nodes precede the "graph" node.

        m_oGraphMLXmlNode.InsertBefore(oGraphMLAttributeXmlNode,
            m_oGraphXmlNode);
    }

    //*************************************************************************
    //  Method: DefineEdgeStringGraphMLAttributes()
    //
    /// <summary>
    /// Defines one or more GraphML-Attributes of type "string" for edges.
    /// </summary>
    ///
    /// <param name="attributeIDNamePairs">
    /// Array of zero or more pairs of strings.  The first string in the pair
    /// is an attribute ID and the second is the corresponding attribute name.
    /// </param>
    /// 
    /// <remarks>
    /// THe GraphML-Attributes cannot have default attribute values.
    /// </remarks>
    //*************************************************************************

    public void
    DefineEdgeStringGraphMLAttributes
    (
        params String[] attributeIDNamePairs
    )
    {
        Debug.Assert(attributeIDNamePairs != null);
        AssertValid();

        DefineStringGraphMLAttributes(true, attributeIDNamePairs);
    }

    //*************************************************************************
    //  Method: DefineVertexStringGraphMLAttributes()
    //
    /// <summary>
    /// Defines one or more GraphML-Attributes of type "string" for vertices.
    /// </summary>
    ///
    /// <param name="attributeIDNamePairs">
    /// Array of zero or more pairs of strings.  The first string in the pair
    /// is an attribute ID and the second is the corresponding attribute name.
    /// </param>
    /// 
    /// <remarks>
    /// THe GraphML-Attributes cannot have default attribute values.
    /// </remarks>
    //*************************************************************************

    public void
    DefineVertexStringGraphMLAttributes
    (
        params String[] attributeIDNamePairs
    )
    {
        Debug.Assert(attributeIDNamePairs != null);
        AssertValid();

        DefineStringGraphMLAttributes(false, attributeIDNamePairs);
    }

    //*************************************************************************
    //  Method: DefineStringGraphMLAttributes()
    //
    /// <summary>
    /// Defines one or more GraphML-Attributes of type "string" for edges or
    /// vertices.
    /// </summary>
    ///
    /// <param name="forEdges">
    /// true if the attributes are for edges, false if they are for vertices.
    /// </param>
    ///
    /// <param name="attributeIDNamePairs">
    /// Array of zero or more pairs of strings.  The first string in the pair
    /// is an attribute ID and the second is the corresponding attribute name.
    /// </param>
    /// 
    /// <remarks>
    /// THe GraphML-Attributes cannot have default attribute values.
    /// </remarks>
    //*************************************************************************

    public void
    DefineStringGraphMLAttributes
    (
        Boolean forEdges,
        params String[] attributeIDNamePairs
    )
    {
        Debug.Assert(attributeIDNamePairs != null);
        AssertValid();

        DefineGraphMLAttributes(forEdges, "string", attributeIDNamePairs);
    }

    //*************************************************************************
    //  Method: DefineGraphMLAttributes()
    //
    /// <summary>
    /// Defines one or more GraphML-Attributes for edges or vertices.
    /// </summary>
    ///
    /// <param name="forEdges">
    /// true if the attributes are for edges, false if they are for vertices.
    /// </param>
    ///
    /// <param name="attributeType">
    /// The attribute's type  Must be "boolean," "int," "long," "float,"
    /// "double," or "string."
    /// </param>
    ///
    /// <param name="attributeIDNamePairs">
    /// Array of zero or more pairs of strings.  The first string in the pair
    /// is an attribute ID and the second is the corresponding attribute name.
    /// </param>
    /// 
    /// <remarks>
    /// THe GraphML-Attributes cannot have default attribute values.
    /// </remarks>
    //*************************************************************************

    public void
    DefineGraphMLAttributes
    (
        Boolean forEdges,
        String attributeType,
        params String[] attributeIDNamePairs
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(attributeType) );
        Debug.Assert(attributeIDNamePairs != null);
        AssertValid();

        Int32 iStrings = attributeIDNamePairs.Length;
        Debug.Assert(iStrings % 2 == 0);

        for (Int32 i = 0; i < iStrings; i += 2)
        {
            DefineGraphMLAttribute(forEdges,
                attributeIDNamePairs[i + 0],
                attributeIDNamePairs[i + 1],
                attributeType, null);
        }
    }

    //*************************************************************************
    //  Method: AppendVertexXmlNode()
    //
    /// <summary>
    /// Creates a new XML node representing a vertex and appends it to the
    /// "graph" XML node.
    /// </summary>
    ///
    /// <param name="vertexID">
    /// Vertex ID.
    /// </param>
    ///
    /// <returns>
    /// The new node.
    /// </returns>
    //*************************************************************************

    public XmlNode
    AppendVertexXmlNode
    (
        String vertexID
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(vertexID) );
        AssertValid();

        // The "id" attribute is required.

        XmlNode oVertexXmlNode = AppendXmlNode(m_oGraphXmlNode, "node");
        SetXmlNodeAttributes(oVertexXmlNode, "id", vertexID);

        m_iVertexXmlNodes++;

        return (oVertexXmlNode);
    }

    //*************************************************************************
    //  Method: AppendEdgeXmlNode()
    //
    /// <summary>
    /// Creates a new XML node representing an edge and appends it to the
    /// "graph" XML node.
    /// </summary>
    ///
    /// <param name="vertex1ID">
    /// ID of the edge's first vertex.
    /// </param>
    ///
    /// <param name="vertex2ID">
    /// ID of the edge's second vertex.
    /// </param>
    ///
    /// <returns>
    /// The new node.
    /// </returns>
    //*************************************************************************

    public XmlNode
    AppendEdgeXmlNode
    (
        String vertex1ID,
        String vertex2ID
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(vertex1ID) );
        Debug.Assert( !String.IsNullOrEmpty(vertex2ID) );
        AssertValid();

        XmlNode oEdgeXmlNode = AppendXmlNode(m_oGraphXmlNode, "edge");

        // The "id" attribute is optional and is omitted here.

        SetXmlNodeAttributes(oEdgeXmlNode,
            "source", vertex1ID,
            "target", vertex2ID
            );

        return (oEdgeXmlNode);
    }

    //*************************************************************************
    //  Method: AppendGraphMLAttributeValue()
    //
    /// <overloads>
    /// Appends a GraphML-Attribute value to an edge or vertex XML node. 
    /// </overloads>
    ///
    /// <summary>
    /// Appends a GraphML-Attribute value of type "string" to an edge or vertex
    /// XML node. 
    /// </summary>
    ///
    /// <param name="edgeOrVertexXmlNode">
    /// The edge or vertex XML node to add the attribute value to.
    /// </param>
    /// 
    /// <param name="attributeID">
    /// The attribute's ID.
    /// </param>
    /// 
    /// <param name="attributeValue">
    /// The attribute's value.  Can be an empty string but not null.
    /// </param>
    //*************************************************************************

    public void
    AppendGraphMLAttributeValue
    (
        XmlNode edgeOrVertexXmlNode,
        String attributeID,
        String attributeValue
    )
    {
        Debug.Assert(edgeOrVertexXmlNode != null);
        Debug.Assert( !String.IsNullOrEmpty(attributeID) );
        Debug.Assert(attributeValue != null);
        AssertValid();

        XmlNode oGraphMLAttributeValueXmlNode = AppendXmlNode(
            edgeOrVertexXmlNode, "data", attributeValue);

        SetXmlNodeAttributes(oGraphMLAttributeValueXmlNode, "key",
            attributeID);
    }

    //*************************************************************************
    //  Method: AppendGraphMLAttributeValue()
    //
    /// <summary>
    /// Appends a GraphML-Attribute value of type "int" to an edge or vertex
    /// XML node. 
    /// </summary>
    ///
    /// <param name="edgeOrVertexXmlNode">
    /// The edge or vertex XML node to add the attribute value to.
    /// </param>
    /// 
    /// <param name="attributeID">
    /// The attribute's ID.
    /// </param>
    /// 
    /// <param name="attributeValue">
    /// The attribute's value.
    /// </param>
    //*************************************************************************

    public void
    AppendGraphMLAttributeValue
    (
        XmlNode edgeOrVertexXmlNode,
        String attributeID,
        Int32 attributeValue
    )
    {
        Debug.Assert(edgeOrVertexXmlNode != null);
        Debug.Assert( !String.IsNullOrEmpty(attributeID) );
        AssertValid();

        AppendGraphMLAttributeValue( edgeOrVertexXmlNode, attributeID,
            attributeValue.ToString(CultureInfo.InvariantCulture) );
    }

    //*************************************************************************
    //  Method: AppendGraphMLAttributeValue()
    //
    /// <summary>
    /// Appends a GraphML-Attribute value of type "double" to an edge or vertex
    /// XML node. 
    /// </summary>
    ///
    /// <param name="edgeOrVertexXmlNode">
    /// The edge or vertex XML node to add the attribute value to.
    /// </param>
    /// 
    /// <param name="attributeID">
    /// The attribute's ID.
    /// </param>
    /// 
    /// <param name="attributeValue">
    /// The attribute's value.
    /// </param>
    //*************************************************************************

    public void
    AppendGraphMLAttributeValue
    (
        XmlNode edgeOrVertexXmlNode,
        String attributeID,
        Double attributeValue
    )
    {
        Debug.Assert(edgeOrVertexXmlNode != null);
        Debug.Assert( !String.IsNullOrEmpty(attributeID) );
        AssertValid();

        AppendGraphMLAttributeValue( edgeOrVertexXmlNode, attributeID,
            attributeValue.ToString(CultureInfo.InvariantCulture) );
    }

    //*************************************************************************
    //  Method: GetHasVertexXmlNode()
    //
    /// <summary>
    /// Gets a flag indicating whether a document has at least one XML node
    /// that represents a vertex.
    /// </summary>
    ///
    /// <param name="graphMLXmlDocument">
    /// XML document containing GraphML that represents a graph.  This does not
    /// have to be a GraphMLXmlDocument.
    /// </param>
    ///
    /// <returns>
    /// true if the document has at least one XML node that represents a
    /// vertex, false if there are no such XML nodes.
    /// </returns>
    //*************************************************************************

    public static Boolean
    GetHasVertexXmlNode
    (
        XmlDocument graphMLXmlDocument
    )
    {
        Debug.Assert(graphMLXmlDocument != null);

        return (graphMLXmlDocument.SelectSingleNode("g:graphml/g:graph/g:node",
            CreateXmlNamespaceManager(graphMLXmlDocument, "g") ) != null);
    }

    //*************************************************************************
    //  Method: AppendXmlNode()
    //
    /// <overloads>
    /// Creates a new XML node and appends it to a parent node.
    /// </overloads>
    ///
    /// <summary>
    /// Creates a new XML node and appends it to a parent node.
    /// </summary>
    ///
    /// <param name="oParentXmlNode">
    /// Node to append the new node to.
    /// </param>
    /// 
    /// <param name="sChildName">
    /// Name of the new node.
    /// </param>
    ///
    /// <returns>
    /// The new node.
    /// </returns>
    //*************************************************************************

    protected XmlNode
    AppendXmlNode
    (
        XmlNode oParentXmlNode,
        String sChildName
    )
    {
        Debug.Assert(oParentXmlNode != null);
        Debug.Assert( !String.IsNullOrEmpty(sChildName) );
        // AssertValid();

        return ( XmlUtil2.AppendNewNode(oParentXmlNode, sChildName,
            GraphMLNamespaceUri) );
    }

    //*************************************************************************
    //  Method: AppendXmlNode()
    //
    /// <summary>
    /// Creates a new XML node, appends it to a parent node, and sets its inner
    /// text.
    /// </summary>
    ///
    /// <param name="oParentXmlNode">
    /// Node to append the new node to.
    /// </param>
    /// 
    /// <param name="sChildName">
    /// Name of the new node.
    /// </param>
    ///
    /// <param name="sInnerText">
    /// The new node's inner text.  Can be empty but not null.
    /// </param>
    ///
    /// <returns>
    /// The new node.
    /// </returns>
    //*************************************************************************

    protected XmlNode
    AppendXmlNode
    (
        XmlNode oParentXmlNode,
        String sChildName,
        String sInnerText
    )
    {
        Debug.Assert(oParentXmlNode != null);
        Debug.Assert( !String.IsNullOrEmpty(sChildName) );
        Debug.Assert(sInnerText != null);
        AssertValid();

        XmlNode oNewXmlNode = AppendXmlNode(oParentXmlNode, sChildName);
        oNewXmlNode.InnerText = sInnerText;

        return (oNewXmlNode);
    }

    //*************************************************************************
    //  Method: SetXmlNodeAttributes()
    //
    /// <summary>
    /// Sets multiple attributes on an XML node.
    /// </summary>
    ///
    /// <param name="oXmlNode">
    /// Node to set attributes on.
    /// </param>
    ///
    /// <param name="asNameValuePairs">
    /// One or more pairs of strings.  The first string in each pair is an
    /// attribute name and the second is the attribute value.
    /// </param>
    ///
    /// <remarks>
    /// This sets multiple attributes on an XML node in one call.  It's an
    /// alternative to calling <see
    /// cref="XmlElement.SetAttribute(String, String)" /> repeatedly.
    /// </remarks>
    //*************************************************************************

    protected void
    SetXmlNodeAttributes
    (
        XmlNode oXmlNode,
        params String[] asNameValuePairs
    )
    {
        Debug.Assert(oXmlNode != null);
        Debug.Assert(asNameValuePairs != null);
        AssertValid();

        XmlUtil2.SetAttributes(oXmlNode, asNameValuePairs);
    }

    //*************************************************************************
    //  Method: CreateXmlNamespaceManager()
    //
    /// <overloads>
    /// Creates an XmlNamespaceManager object to use with the document.
    /// </overloads>
    ///
    /// <summary>
    /// Creates an XmlNamespaceManager object to use with this document.
    /// </summary>
    ///
    /// <param name="prefix">
    /// The prefix to use for the default GraphML namespace.
    /// </param>
    ///
    /// <returns>
    /// An XmlNamespaceManager object to use with the document.
    /// </returns>
    ///
    /// <remarks>
    /// Any prefix will do, so long as it is also included in every XPath
    /// expression when the document is searched.  For example, if you want all
    /// "node" XML nodes in the document, this would work:
    ///
    /// <code>
    /// XmlNamespaceManager oXmlNamespaceManager =
    ///     oGraphMLXmlDocument.CreateXmlNamespaceManager("g");
    ///
    /// foreach ( XmlNode oVertexXmlNode in oGraphMLXmlDocument.SelectNodes(
    ///     "g:graphml/g:graph/g:node", oXmlNamespaceManager) )
    /// {
    /// ...
    /// }
    /// </code>
    ///
    /// </remarks>
    //*************************************************************************

    public XmlNamespaceManager
    CreateXmlNamespaceManager
    (
        String prefix
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(prefix) );

        return ( CreateXmlNamespaceManager(this, prefix) ); 
    }

    //*************************************************************************
    //  Method: CreateXmlNamespaceManager()
    //
    /// <summary>
    /// Creates an XmlNamespaceManager object to use with a specified document.
    /// </summary>
    ///
    /// <param name="graphMLXmlDocument">
    /// XML document containing GraphML that represents a graph.  This does not
    /// have to be a GraphMLXmlDocument.
    /// </param>
    ///
    /// <param name="prefix">
    /// The prefix to use for the default GraphML namespace.
    /// </param>
    ///
    /// <returns>
    /// An XmlNamespaceManager object to use with the document.
    /// </returns>
    ///
    /// <remarks>
    /// See the other overload for details on using this method.
    /// </remarks>
    //*************************************************************************

    public static XmlNamespaceManager
    CreateXmlNamespaceManager
    (
        XmlDocument graphMLXmlDocument,
        String prefix
    )
    {
        Debug.Assert(graphMLXmlDocument != null);
        Debug.Assert( !String.IsNullOrEmpty(prefix) );

        XmlNamespaceManager oXmlNamespaceManager = new XmlNamespaceManager(
            graphMLXmlDocument.NameTable);

        oXmlNamespaceManager.AddNamespace(prefix, GraphMLNamespaceUri);

        return (oXmlNamespaceManager);
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
        Debug.Assert(m_oGraphMLXmlNode != null);
        Debug.Assert(m_oGraphXmlNode != null);
        Debug.Assert(m_iVertexXmlNodes >= 0);
    }


    //*************************************************************************
    //  Public constants
    //*************************************************************************

    /// GraphML namespace URI.

    public const String GraphMLNamespaceUri =
        "http://graphml.graphdrawing.org/xmlns";


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Root GraphML XML node.

    protected XmlNode m_oGraphMLXmlNode;

    /// Graph XML node.

    protected XmlNode m_oGraphXmlNode;

    /// The number of XML nodes that represent a vertex.

    protected Int32 m_iVertexXmlNodes;
}

}
