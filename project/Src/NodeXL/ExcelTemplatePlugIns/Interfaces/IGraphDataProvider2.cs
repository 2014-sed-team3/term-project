
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
//  Interface: IGraphDataProvider2
//
/// <summary>
/// Represents an object that can provide graph data to the NodeXL Excel
/// Template.
/// </summary>
///
/// <remarks>
/// Implement this interface when you want to import graph data into the NodeXL
/// Excel Template and the data is not in a format directly importable by the
/// Template.
///
/// <para>
/// The NodeXL Excel Template (implemented by the ExcelTemplate project) can 
/// directly import graph data from a variety of sources, including other Excel
/// workbooks, UCINET files, Pajek files, and GraphML files.  It also has a
/// plug-in architecture that can be used to import graph data in other
/// unsupported formats.  If you are a .NET programmer, you can create a custom
/// <i>graph data provider DLL</i> that can be integrated into the NodeXL Excel
/// Template by simply copying it into a designated plug-ins folder.  You do
/// not need to build NodeXL to do this, and in fact you do not need the NodeXL
/// source code at all.
/// </para>
///
/// <para>
/// To create a custom graph data provider DLL, do the following:
/// </para>
///
/// <list type="number">
///
/// <item><description>
/// Create a new Visual Studio solution and a Class Libraries project.  The
/// Class Libraries project will build your custom graph data provider DLL.
/// You can use either Visual Studio 2008 or 2010.
/// </description></item>
///
/// <item><description>
/// Target the Class Libraries project to the .NET Framework 3.5.
/// </description></item>
///
/// <item><description>
/// Add a reference to Microsoft.NodeXL.ExcelTemplatePlugIns.dll.  This DLL is
/// available within the latest NodeXL Class Libraries download on the CodePlex
/// website, at http://nodexl.codeplex.com/releases.  It is the only NodeXL
/// file you need from the download, apart from the NodeXLApi.chm help file
/// that documents the <see cref="IGraphDataProvider2" /> interface.
/// </description></item>
///
/// <item><description>
/// Add a class to the assembly that implements the <see
/// cref="IGraphDataProvider2" /> interface.
/// </description></item>
///
/// <item><description>
/// In the same solution, create a Windows Forms project (or some other type of
/// project) to test your <see cref="IGraphDataProvider2" /> implementation.
/// The nature of the test project is up to you, but it should call your <see
/// cref="IGraphDataProvider2.TryGetGraphDataAsTemporaryFile" /> method and
/// verify the returned graph data.  The test project allows you to easily step
/// through your code.  <i>You do not need Excel, the NodeXL Excel Template, or
/// any NodeXL source code to develop and test your graph data provider.</i>
/// </description></item>
///
/// <item><description>
/// When your DLL is implemented and tested, perform final testing within
/// Excel.  Install NodeXL on some computer using the standard NodeXL Excel
/// Template setup program, available on the CodePlex site, and manually copy
/// your graph data provider DLL into NodeXL's PlugIns folder.  This is 
/// "C:\Program Files\Social Media Research Foundation\NodeXL Excel Template\
/// PlugIns" on 32-bit English computers.
/// </description></item>
///
/// </list>
///
/// <para>
/// When you run the Excel Template and open its Import menu, the Excel
/// Template uses .NET reflection to find all the classes in its PlugIns folder
/// that implement <see cref="IGraphDataProvider2" />, including yours.  For
/// each such class, it adds a child item to the Import menu using the strings
/// returned by <see cref="Name" /> and <see cref="Description" />.  When you
/// select one of the child menu items, the <see
/// cref="TryGetGraphDataAsTemporaryFile" /> method on the corresponding class
/// is called, and the graph data returned by that method is used to populate
/// the NodeXL workbook.
/// </para>
///
/// <para>
/// If you would like to see a sample implementation of <see
/// cref="IGraphDataProvider2" />, one is provided within the NodeXL Class
/// Libraries download.  It's called SampleGraphDataProvider.cs.
/// </para>
///
/// </remarks>
//*****************************************************************************

public interface IGraphDataProvider2
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
    //  Method: TryGetGraphDataAsTemporaryFile()
    //
    /// <summary>
    /// Attempts to get graph data to import into the NodeXL Excel Template.
    /// </summary>
    ///
    /// <param name="pathToTemporaryFile">
    /// If true is returned, this gets set to the full path of a temporary file
    /// created by this method.  The method's caller will read the temporary
    /// file and then delete it.
    /// </param>
    ///
    /// <returns>
    /// true if the graph data was obtained and stored in a temporary file
    /// created by this method, false if the graph data was not obtained.
    /// </returns>
    ///
    /// <remarks>
    /// If the graph data is obtained, this method does the following:
    ///
    /// <list type="number">
    ///
    /// <item><description>
    /// Creates a temporary file.
    /// </description></item>
    ///
    /// <item><description>
    /// Stores the graph data in the temporary file as a GraphML XML string.
    /// </description></item>
    ///
    /// <item><description>
    /// Sets <paramref name="pathToTemporaryFile" /> to the full path of the
    /// temporary file.
    /// </description></item>
    ///
    /// <item><description>
    /// Returns true.
    /// </description></item>
    ///
    /// </list>
    ///
    /// <para>
    /// If the graph data is not obtained, this method returns false.
    /// </para>
    ///
    /// <para>
    /// The graph data must be a string containing XML that uses the GraphML
    /// schema.  A good introduction to GraphML can be found in the GraphML
    /// Primer:
    /// </para>
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
    /// <para>
    /// If there is an optional "description" attribute on the "graph" XML
    /// node, NodeXL will use the description for its "graph summary" feature,
    /// which describes how the graph was created.
    /// </para>
    ///
    /// <para>
    /// If there is an optional "suggestedFileNameNoExtension" attribute on the
    /// "graph" XML node, NodeXL will use the suggested file name when the user
    /// automates the graph and specifies that he wants the workbook saved
    /// during the automation.  The suggested file name should not include a
    /// path or extension, and NodeXL will substitute spaces for any characters
    /// that are not valid in a file name.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    Boolean
    TryGetGraphDataAsTemporaryFile
    (
        out String pathToTemporaryFile
    );
}

}
