﻿
using System;
using System.Xml;
using System.Globalization;
using System.Diagnostics;
using Smrf.XmlLib;

namespace Smrf.NodeXL.Adapters
{
//*****************************************************************************
//  Class: GraphMLAttribute
//
/// <summary>
/// Represents a GraphML vertex or edge attribute parsed from a GraphML file.
/// </summary>
///
/// <remarks>
/// This class is used by <see cref="GraphMLGraphAdapter" /> while it is
/// parsing a GraphML file.
///
/// <para>
/// In GraphML, a "key" XML node defines an edge or vertex attribute, which
/// GraphML calls a "Graph-ML attribute," and a "data" XML node specifies the
/// GraphML-attribute's value for a specific vertex or edge.  The <see
/// cref="GraphMLAttribute" /> constructor parses the "key" XML node, and the
/// <see cref="GetAttributeValue" /> parses a "data" XML node.  <see
/// cref="TryGetDefaultAttributeValue" /> provides a default value for the
/// GraphML-attribute, if one was specified.
/// </para>
///
/// <para>
/// NodeXL doesn't support the for="graph" or for="all" attribute values
/// allowed by the GraphML specification.  The caller should filter out such
/// "key" XML nodes before using this class to parse them.
/// </para>
///
/// </remarks>
//*****************************************************************************

public class GraphMLAttribute : Object
{
    //*************************************************************************
    //  Constructor: GraphMLAttribute()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="GraphMLAttribute" />
    /// class.
    /// </summary>
    ///
    /// <param name="keyXmlNode">
    /// A "key" XML node representing a Graph-ML attribute.
    /// </param>
    ///
    /// <param name="xmlNamespaceManager">
    /// XML namespace manager.
    /// </param>
    ///
    /// <param name="graphMLPrefix">
    /// The prefix specified for the GraphML namespace when <paramref
    /// name="xmlNamespaceManager" /> was created.
    /// </param>
    //*************************************************************************

    public GraphMLAttribute
    (
        XmlNode keyXmlNode,
        XmlNamespaceManager xmlNamespaceManager,
        String graphMLPrefix
    )
    {
        Debug.Assert(keyXmlNode != null);
        Debug.Assert(keyXmlNode.Name == "key");
        Debug.Assert(xmlNamespaceManager != null);
        Debug.Assert( !String.IsNullOrEmpty(graphMLPrefix) );

        ParseKeyXmlNode(keyXmlNode, xmlNamespaceManager, graphMLPrefix);

        AssertValid();
    }

    //*************************************************************************
    //  Property: ID
    //
    /// <summary>
    /// Gets the ID of the GraphMLAttribute.
    /// </summary>
    ///
    /// <value>
    /// The ID of the GraphMLAttribute, as a String.
    /// </value>
    ///
    /// <remarks>
    /// The ID is the value of the "id" attribute on the "key" XML node.
    /// </remarks>
    //*************************************************************************

    public String
    ID
    {
        get
        {
            AssertValid();

            return (m_sID);
        }
    }

    //*************************************************************************
    //  Property: Name
    //
    /// <summary>
    /// Gets the name of the GraphMLAttribute.
    /// </summary>
    ///
    /// <value>
    /// The name of the GraphMLAttribute, as a String.
    /// </value>
    ///
    /// <remarks>
    /// The name is the value of the "attr.name" attribute on the "key" XML
    /// node, or the value of the "id" attribute if the "attr.name" attribute
    /// is missing.
    /// </remarks>
    //*************************************************************************

    public String
    Name
    {
        get
        {
            AssertValid();

            return (m_sName);
        }
    }

    //*************************************************************************
    //  Property: IsForVertex
    //
    /// <summary>
    /// Gets a flag indicating whether the GraphML-attribute is for a vertex or
    /// for an edge.
    /// </summary>
    ///
    /// <value>
    /// true if the GraphML-attribute is for a vertex, false if it is for an
    /// edge.
    /// </value>
    //*************************************************************************

    public Boolean
    IsForVertex
    {
        get
        {
            AssertValid();

            return (m_bIsForVertex);
        }
    }

    //*************************************************************************
    //  Method: GetAttributeValue()
    //
    /// <summary>
    /// Gets a GraphML-attribute value for a vertex or edge by parsing a "data"
    /// XML node.
    /// </summary>
    ///
    /// <param name="dataXmlNode">
    /// A "data" XML node for a vertex or edge.
    /// </param>
    ///
    /// <returns>
    /// The attribute's value.
    /// </returns>
    ///
    /// <remarks>
    /// The value of the key attribute of the <paramref name="dataXmlNode" />
    /// XML node must be the value of the <see cref="ID" /> property.  In other
    /// words, call this method only on the <see cref="GraphMLAttribute" />
    /// object that corresponds to the key value.
    /// </remarks>
    //*************************************************************************

    public Object
    GetAttributeValue
    (
        XmlNode dataXmlNode
    )
    {
        Debug.Assert(dataXmlNode != null);
        Debug.Assert(dataXmlNode.Name == "data");
        AssertValid();

        String sKey = XmlUtil2.SelectRequiredSingleNodeAsString(dataXmlNode,
            "@key", null);

        Debug.Assert(sKey == m_sID);

        String sAttributeValue;
        
        if ( !XmlUtil2.TrySelectSingleNodeAsString(dataXmlNode, "text()",
            null, out sAttributeValue) )
        {
            // Allow missing inner text for GraphML-attributes of type string.
            // This was found in a GraphML file created by the yED program.

            sAttributeValue = String.Empty;
        }

        try
        {
            return ( ConvertAttributeValue(sAttributeValue) );
        }
        catch (FormatException)
        {
            throw new XmlException(
                "The GraphML-attribute value specified for a \"data\" XML node"
                + " with the key \"" + sKey + "\" is not of the specified"
                + " type." 
                );
        }
    }

    //*************************************************************************
    //  Method: TryGetDefaultAttributeValue()
    //
    /// <summary>
    /// Attempts to get the default value for this GraphML-attribute.
    /// </summary>
    ///
    /// <param name="defaultAttributeValue">
    /// Where the default value gets stored if true is returned.
    /// </param>
    ///
    /// <returns>
    /// true if this GraphML-attribute has a default value.
    /// </returns>
    //*************************************************************************

    public Boolean
    TryGetDefaultAttributeValue
    (
        out Object defaultAttributeValue
    )
    {
        AssertValid();

        defaultAttributeValue = m_oDefaultAttributeValue;

        return (m_oDefaultAttributeValue != null);
    }

    //*************************************************************************
    //  Method: ParseKeyXmlNode()
    //
    /// <summary>
    /// Parses the "key" XML node representing a Graph-ML attribute.
    /// class.
    /// </summary>
    ///
    /// <param name="oKeyXmlNode">
    /// A "key" XML node.
    /// </param>
    ///
    /// <param name="oXmlNamespaceManager">
    /// XML namespace manager.
    /// </param>
    ///
    /// <param name="sGraphMLPrefix">
    /// The prefix specified for the GraphML namespace when <paramref
    /// name="oXmlNamespaceManager" /> was created.
    /// </param>
    //*************************************************************************

    protected void
    ParseKeyXmlNode
    (
        XmlNode oKeyXmlNode,
        XmlNamespaceManager oXmlNamespaceManager,
        String sGraphMLPrefix
    )
    {
        Debug.Assert(oKeyXmlNode != null);
        Debug.Assert(oXmlNamespaceManager != null);
        Debug.Assert( !String.IsNullOrEmpty(sGraphMLPrefix) );

        m_sID = XmlUtil2.SelectRequiredSingleNodeAsString(oKeyXmlNode,
            "@id", null);

        String sFor = XmlUtil2.SelectRequiredSingleNodeAsString(oKeyXmlNode,
            "@for", null);

        // Note that the @attr.name and @attr.type attributes are optional.
        // Default to sensible values if they are missing.

        if ( !XmlUtil2.TrySelectSingleNodeAsString(oKeyXmlNode,
            "@attr.name", null, out m_sName) )
        {
            m_sName = m_sID;
        }

        String sType;

        if ( !XmlUtil2.TrySelectSingleNodeAsString(oKeyXmlNode, "@attr.type",
            null, out sType) )
        {
            sType = "string";
        }

        switch (sFor)
        {
            case "node":

                m_bIsForVertex = true;
                break;
            
            case "edge":

                m_bIsForVertex = false;
                break;
            
            // NodeXL doesn't support the "graph" or "all" values allowed by
            // the GraphML specification.  The caller should filter out such
            // "key" XML nodes before using this class.

            default:

                throw new XmlException(
                    "The \"for\" attribute on the \"key\" XML node with the"
                    + " id \"" + m_sID + "\" must be either \"node\" or"
                    + " \"edge\"."
                    );
        }

        switch (sType)
        {
            case "boolean":

                m_eType = AttributeType.Boolean;
                break;
            
            case "int":

                m_eType = AttributeType.Int;
                break;
            
            case "long":

                m_eType = AttributeType.Long;
                break;
            
            case "float":

                m_eType = AttributeType.Float;
                break;
            
            case "double":

                m_eType = AttributeType.Double;
                break;
            
            case "string":

                m_eType = AttributeType.String;
                break;
            
            default:

                throw new XmlException(
                    "The \"attr.type\" attribute on the \"key\" XML node with"
                    + " id \"" + m_sID + "\" does not have a valid value."
                    );
        }

        m_oDefaultAttributeValue = null;

        String sDefaultAttributeValue;

        XmlNode oDefaultXmlNode = oKeyXmlNode.SelectSingleNode(
            sGraphMLPrefix + ":default", oXmlNamespaceManager);

        if (
            oDefaultXmlNode != null
            &&
            XmlUtil2.TrySelectSingleNodeAsString(oDefaultXmlNode, "text()",
                null, out sDefaultAttributeValue)
            )
        {
            try
            {
                m_oDefaultAttributeValue =
                    ConvertAttributeValue(sDefaultAttributeValue);
            }
            catch (FormatException)
            {
                throw new XmlException(
                    "The default value specified for the \"key\" XML node with"
                    + " the id \"" + m_sID + "\" is not of the specified type." 
                    );
            }
        }

        AssertValid();
    }

    //*************************************************************************
    //  Method: ConvertAttributeValue()
    //
    /// <summary>
    /// Converts an attribute value from a string to the attribute's type.
    /// </summary>
    ///
    /// <param name="sAttributeValue">
    /// The value to convert.  Can't be null.
    /// </param>
    //*************************************************************************

    protected Object
    ConvertAttributeValue
    (
        String sAttributeValue
    )
    {
        Debug.Assert(sAttributeValue != null);

        Debug.Assert(m_eType == AttributeType.String ||
            sAttributeValue.Length > 0);

        IFormatProvider oFormatProvider = CultureInfo.InvariantCulture;

        switch (m_eType)
        {
            case AttributeType.Boolean:

                return ( Convert.ToBoolean(sAttributeValue, oFormatProvider) );
            
            case AttributeType.Int:

                return ( Convert.ToInt32(sAttributeValue, oFormatProvider) );
            
            case AttributeType.Long:

                return ( Convert.ToInt64(sAttributeValue, oFormatProvider) );
            
            case AttributeType.Float:

                return ( Convert.ToSingle(sAttributeValue, oFormatProvider) );
            
            case AttributeType.Double:

                return ( Convert.ToDouble(sAttributeValue, oFormatProvider) );
            
            case AttributeType.String:

                return (sAttributeValue);
            
            default:

                Debug.Assert(false);
                return (null);
        }
    }

    //*************************************************************************
    //  Enum: AttributeType
    //
    /// <summary>
    /// Specifies the type of a GraphML-attribute.
    /// </summary>
    ///
    /// <remarks>
    /// The enum names are the possible values of the attr.type attribute on
    /// a key node.
    /// </remarks>
    //*************************************************************************

    protected enum
    AttributeType
    {
        /// <summary>
        /// Boolean.
        /// </summary>

        Boolean,

        /// <summary>
        /// Int32.
        /// </summary>

        Int,

        /// <summary>
        /// Int64.
        /// </summary>

        Long,

        /// <summary>
        /// Single.
        /// </summary>

        Float,

        /// <summary>
        /// Double.
        /// </summary>

        Double,

        /// <summary>
        /// String.
        /// </summary>

        String,
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
        Debug.Assert( !String.IsNullOrEmpty(m_sID) );
        Debug.Assert( !String.IsNullOrEmpty(m_sName) );
        // m_bIsForVertex
        // m_eType
        // m_oDefaultAttributeValue
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Value of the key node's ID attribute.

    protected String m_sID;

    /// Value of the key node's attr.name attribute.

    protected String m_sName;

    /// true if the GraphML-attribute is for a vertex, false if it is for an
    /// edge.

    protected Boolean m_bIsForVertex;

    /// Value of the key node's attr.type attribute.

    protected AttributeType m_eType;

    /// Default attribute value, or null if there is no default.

    protected Object m_oDefaultAttributeValue;
}

}
