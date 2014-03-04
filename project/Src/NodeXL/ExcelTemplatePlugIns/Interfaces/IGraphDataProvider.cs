
using System;
using System.Diagnostics;

// Note that the following namespace was not changed to
// Smrf.NodeXL.ExcelTemplatePlugIns when the product's company was changed.
// This is to retain backward compatibility with existing graph data providers,
// which know only about the old namespace.
//
// The old name of the DLL, Microsoft.NodeXL.ExcelTemplatePlugIns, was retained
// for the same reason.

namespace Microsoft.NodeXL.ExcelTemplatePlugIns
{
//*****************************************************************************
//  Interface: IGraphDataProvider
//
/// <summary>
/// <b>Obsolete.</b>  Represents an object that can provide graph data to the
/// NodeXL Excel Template.
/// </summary>
///
/// <remarks>
/// This interface has been replaced by <see cref="IGraphDataProvider2" />.
/// New graph data providers should implement <see
/// cref="IGraphDataProvider2" />, not <see cref="IGraphDataProvider" />,
/// although the NodeXL Excel Template will continue to support older graph
/// data providers that implement <see cref="IGraphDataProvider" />.
/// </remarks>
//*****************************************************************************

public interface IGraphDataProvider
{
    //*************************************************************************
    //  Property: Name
    //
    /// <summary>
    /// Gets the name of the data provider.
    /// </summary>
    ///
    /// <value>
    /// The name of the data provider, as a String.  Sample: "Sample Network".
    /// </value>
    ///
    /// <remarks>
    /// The NodeXL Excel Template adds a child item to its Import menu using
    /// the menu text "From [Name]...".  If the property value is "Sample
    /// Network", for example, then the menu text will be "From Sample
    /// Network...".
    /// </remarks>
    //*************************************************************************

    String
    Name
    {
        get;
    }

    //*************************************************************************
    //  Property: Description
    //
    /// <summary>
    /// Gets a description of the data provider.
    /// </summary>
    ///
    /// <value>
    /// A description of the data provider, as a String.  Sample: "show the
    /// friends of a user".
    /// </value>
    ///
    /// <remarks>
    /// The NodeXL Excel Template uses the description as part of the tooltip
    /// for the child item it adds to its Import menu.  The tooltip text is
    /// "Optionally clear the NodeXL workbook, then [Description]."  If the
    /// property value is "show the friends of a user", for example, then the
    /// child menu item's tooltip is "Optionally clear the NodeXL workbook,
    /// then show the friends of a user."
    /// </remarks>
    //*************************************************************************

    String
    Description
    {
        get;
    }

    //*************************************************************************
    //  Method: TryGetGraphData()
    //
    /// <summary>
    /// Attempts to get graph data to import into the NodeXL Excel Template.
    /// </summary>
    ///
    /// <param name="graphDataAsGraphML">
    /// Where the graph data gets stored as a GraphML XML string, if true is
    /// returned.
    /// </param>
    ///
    /// <returns>
    /// true if the graph data was obtained, false if not.
    /// </returns>
    ///
    /// <remarks>
    /// The graph data must be a string containing XML that uses the GraphML
    /// schema.  A good introduction to GraphML can be found in the GraphML
    /// Primer:
    ///
    /// <para>
    /// http://graphml.graphdrawing.org/primer/graphml-primer.html
    /// </para>
    ///
    /// <para>
    /// Here is a GraphML XML string that represents a graph with five vertices
    /// and two edges.
    /// </para>
    ///
    /// <code>
    /// &lt;?xml version="1.0" encoding="UTF-8"?&gt;
    /// &lt;graphml xmlns="http://graphml.graphdrawing.org/xmlns"&gt;
    ///
    ///     &lt;key id="VertexColor" for="node" attr.name="Color" attr.type="string" /&gt;
    ///     &lt;key id="VertexLatestPostDate" for="node" attr.name="Latest Post Date"
    ///         attr.type="string" /&gt;
    ///     &lt;key id="EdgeWidth" for="edge" attr.name="Width" attr.type="double"&gt;
    ///         &lt;default&gt;1.5&lt;/default&gt;
    ///     &lt;/key&gt;
    ///
    ///     &lt;graph edgedefault="undirected"&gt;
    ///         &lt;node id="V1"&gt;
    ///             &lt;data key="VertexColor"&gt;red&lt;/data&gt;
    ///             &lt;data key="VertexLatestPostDate"&gt;2009/07/05&lt;/data&gt;
    ///         &lt;/node&gt;
    ///         &lt;node id="V2"&gt;
    ///             &lt;data key="VertexColor"&gt;orange&lt;/data&gt;
    ///             &lt;data key="VertexLatestPostDate"&gt;2009/07/12&lt;/data&gt;
    ///         &lt;/node&gt;
    ///         &lt;node id="V3"&gt;
    ///             &lt;data key="VertexColor"&gt;blue&lt;/data&gt;
    ///         &lt;/node&gt;
    ///         &lt;node id="V4"&gt;
    ///             &lt;data key="VertexColor"&gt;128,0,128&lt;/data&gt;
    ///         &lt;/node&gt;
    ///         &lt;node id="V5" /&gt;
    ///
    ///         &lt;edge source="V1" target="V2" /&gt;
    ///         &lt;edge source="V3" target="V2"&gt;
    ///             &lt;data key="EdgeWidth"&gt;2.5&lt;/data&gt;
    ///         &lt;/edge&gt;
    ///     &lt;/graph&gt;
    /// &lt;/graphml&gt;
    /// </code>
    ///
    /// <para>
    /// The Excel Template imports edge and vertex attributes, which GraphML
    /// calls "GraphML-attributes.  If the "attr.name" of the GraphML-attribute
    /// is the name of an existing column in the Edges or Vertices worksheet
    /// ("Width" and "Color" in this example), the specified edge or vertex
    /// attribute values get imported to those columns when the graph data is
    /// imported.  If the attr.name is not the name of an existing column
    /// ("Latest Post Date" in this example), a new column is added to the
    /// worksheet and the specified attribute values get imported to the new
    /// column.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    Boolean
    TryGetGraphData
    (
        out String graphDataAsGraphML
    );
}

}
