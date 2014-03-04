
using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Diagnostics;
using Smrf.NodeXL.Core;
using Smrf.NodeXL.Adapters;
using Microsoft.NodeXL.ExcelTemplatePlugIns;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: PlugInManager
//
/// <summary>
/// Provides access to plug-in classes.
/// </summary>
///
/// <remarks>
/// Call <see cref="GetGraphDataProviders" /> to get an array plug-in classes
/// that implement <see cref="IGraphDataProvider" />.
///
/// <para>
/// All methods are static.
/// </para>
///
/// </remarks>
//*****************************************************************************

public static class PlugInManager
{
    //*************************************************************************
    //  Method: GetGraphDataProviders()
    //
    /// <summary>
    /// Gets an array of plug-in classes that implement either the newer <see
    /// cref="IGraphDataProvider2" /> interface or the older <see
    /// cref="IGraphDataProvider" /> interface.
    /// </summary>
    ///
    /// <returns>
    /// An array of zero or more <see cref="IGraphDataProvider2" /> or <see
    /// cref="IGraphDataProvider" /> implementations.
    /// </returns>
    ///
    /// <remarks>
    /// The <see cref="IGraphDataProvider2" /> and <see
    /// cref="IGraphDataProvider" /> interfaces allow developers to
    /// create plug-in classes that import graph data into the NodeXL Excel
    /// Template without having to modify the ExcelTemplate's source code.  See
    /// <see cref="IGraphDataProvider2" /> for more information.
    /// </remarks>
    //*************************************************************************

    public static Object []
    GetGraphDataProviders()
    {
        List<Object> oGraphDataProviders = new List<Object>();

        AddBuiltInGraphDataProviders(oGraphDataProviders);
        AddPlugInFolderGraphDataProviders(oGraphDataProviders);

        SortGraphDataProviders(oGraphDataProviders);

        return ( oGraphDataProviders.ToArray() );
    }

    //*************************************************************************
    //  Method: GetGraphDataProviderName()
    //
    /// <summary>
    /// Gets the name of a graph data provider.
    /// </summary>
    ///
    /// <param name="graphDataProvider">
    /// An object that implements either <see cref="IGraphDataProvider2" /> or
    /// <see cref="IGraphDataProvider" />.
    /// </param>
    ///
    /// <returns>
    /// The name of the graph data provider.
    /// </returns>
    //*************************************************************************

    public static String
    GetGraphDataProviderName
    (
        Object graphDataProvider
    )
    {
        Debug.Assert(graphDataProvider != null);

        Debug.Assert(graphDataProvider is IGraphDataProvider2 ||
            graphDataProvider is IGraphDataProvider);

        return ( (graphDataProvider is IGraphDataProvider2) ?
            ( (IGraphDataProvider2)graphDataProvider ).Name
            :
            ( (IGraphDataProvider)graphDataProvider ).Name
            );
    }

    //*************************************************************************
    //  Method: GetGraphDataProviderDescription()
    //
    /// <summary>
    /// Gets the description of a graph data provider.
    /// </summary>
    ///
    /// <param name="graphDataProvider">
    /// An object that implements either <see cref="IGraphDataProvider2" /> or
    /// <see cref="IGraphDataProvider" />.
    /// </param>
    ///
    /// <returns>
    /// The description of the graph data provider.
    /// </returns>
    //*************************************************************************

    public static String
    GetGraphDataProviderDescription
    (
        Object graphDataProvider
    )
    {
        Debug.Assert(graphDataProvider != null);

        Debug.Assert(graphDataProvider is IGraphDataProvider2 ||
            graphDataProvider is IGraphDataProvider);

        return ( (graphDataProvider is IGraphDataProvider2) ?
            ( (IGraphDataProvider2)graphDataProvider ).Description
            :
            ( (IGraphDataProvider)graphDataProvider ).Description
            );
    }

    //*************************************************************************
    //  Method: TryGetGraphFromGraphDataProvider()
    //
    /// <summary>
    /// Attempts to get a graph from a graph data provider.
    /// </summary>
    ///
    /// <param name="graphDataProvider">
    /// An object that implements either <see cref="IGraphDataProvider2" /> or
    /// <see cref="IGraphDataProvider" />.
    /// </param>
    ///
    /// <param name="graph">
    /// Where the graph gets stored if true is returned.
    /// </param>
    ///
    /// <returns>
    /// true if the graph was obtained.
    /// </returns>
    //*************************************************************************

    public static Boolean
    TryGetGraphFromGraphDataProvider
    (
        Object graphDataProvider,
        out IGraph graph
    )
    {
        Debug.Assert(graphDataProvider != null);

        Debug.Assert(graphDataProvider is IGraphDataProvider2 ||
            graphDataProvider is IGraphDataProvider);

        graph = null;
        GraphMLGraphAdapter oGraphMLGraphAdapter = new GraphMLGraphAdapter();

        if (graphDataProvider is IGraphDataProvider2)
        {
            String sPathToTemporaryFile = null;

            if ( !( (IGraphDataProvider2)graphDataProvider )
                .TryGetGraphDataAsTemporaryFile(out sPathToTemporaryFile) )
            {
                return (false);
            }

            try
            {
                graph = oGraphMLGraphAdapter.LoadGraphFromFile(
                    sPathToTemporaryFile);
            }
            finally
            {
                File.Delete(sPathToTemporaryFile);
            }
        }
        else
        {
            String sGraphDataAsGraphML;

            if ( !( (IGraphDataProvider)graphDataProvider ).TryGetGraphData(
                out sGraphDataAsGraphML) )
            {
                return (false);
            }

            graph = oGraphMLGraphAdapter.LoadGraphFromString(
                sGraphDataAsGraphML);
        }

        return (true);
    }

    //*************************************************************************
    //  Method: AddPlugInFolderGraphDataProviders()
    //
    /// <summary>
    /// Adds types in the plug-in folder that implement either the newer <see
    /// cref="IGraphDataProvider2" /> interface or the older <see
    /// cref="IGraphDataProvider" /> interface.
    /// </summary>
    ///
    /// <param name="oGraphDataProviders">
    /// Collection to add the types to if they implement one of the interfaces.
    /// </param>
    //*************************************************************************

    private static void
    AddPlugInFolderGraphDataProviders
    (
        List<Object> oGraphDataProviders
    )
    {
        Debug.Assert(oGraphDataProviders != null);

        IEnumerable<String> oFilePaths;

        if ( TryGetFilesInPlugInFolder(out oFilePaths) )
        {
            foreach (String sFilePath in oFilePaths)
            {
                AddGraphDataProvidersFromFile(sFilePath, oGraphDataProviders);
            }
        }
    }

    //*************************************************************************
    //  Method: AddBuiltInGraphDataProviders()
    //
    /// <summary>
    /// Adds built-in types that implement either the newer <see
    /// cref="IGraphDataProvider2" /> interface or the older <see
    /// cref="IGraphDataProvider" /> interface.
    /// </summary>
    ///
    /// <param name="oGraphDataProviders">
    /// Collection to add the types to if they implement one of the interfaces.
    /// </param>
    //*************************************************************************

    private static void
    AddBuiltInGraphDataProviders
    (
        List<Object> oGraphDataProviders
    )
    {
        Debug.Assert(oGraphDataProviders != null);

        String sGraphDataProvidersFilePath = Path.Combine(
            ApplicationUtil.GetApplicationFolder(), GraphDataProvidersFileName);

        if ( File.Exists(sGraphDataProvidersFilePath) )
        {
            AddGraphDataProvidersFromFile(
                sGraphDataProvidersFilePath, oGraphDataProviders);
        }
    }

    //*************************************************************************
    //  Method: TryGetFilesInPlugInFolder()
    //
    /// <summary>
    /// Attempts to get the full paths to the files in the folder where the
    /// user can place plug-in assemblies.
    /// </summary>
    ///
    /// <param name="oFilePaths">
    /// Where the full paths get stored if true is returned.
    /// </param>
    ///
    /// <returns>
    /// true if the full paths were obtained.
    /// </returns>
    //*************************************************************************

    private static Boolean
    TryGetFilesInPlugInFolder
    (
        out IEnumerable<String> oFilePaths
    )
    {
        oFilePaths = null;

        String sPlugInFolderPath =
            ( new PlugInUserSettings() ).PlugInFolderPath;

        if (
            String.IsNullOrEmpty(sPlugInFolderPath)
            ||
            !Directory.Exists(sPlugInFolderPath)
            )
        {
            return (false);
        }

        List<String> oFilePathList = new List<String>();

        foreach ( String sSearchPattern in new String[] {"*.dll", "*.exe"} )
        {
            oFilePathList.AddRange( Directory.GetFiles(
                sPlugInFolderPath, sSearchPattern) );
        }

        oFilePaths = oFilePathList;

        return (true);
    }

    //*************************************************************************
    //  Method: AddGraphDataProvidersFromFile()
    //
    /// <summary>
    /// Adds types that implement either the newer <see
    /// cref="IGraphDataProvider2" /> interface or the older <see
    /// cref="IGraphDataProvider" /> interface.
    /// </summary>
    ///
    /// <param name="sFilePath">
    /// Full path to a file that might contains types that implement one of the
    /// interfaces.
    /// </param>
    ///
    /// <param name="oGraphDataProviders">
    /// Collection to add the type to if they implement one of the interfaces.
    /// </param>
    //*************************************************************************

    private static void
    AddGraphDataProvidersFromFile
    (
        String sFilePath,
        List<Object> oGraphDataProviders
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sFilePath) ); 
        Debug.Assert(oGraphDataProviders != null);

        Type [] aoTypes;

        if ( TryGetTypesFromFile(sFilePath, out aoTypes) )
        {
            foreach (Type oType in aoTypes)
            {
                AddGraphDataProvidersFromType(oType, oGraphDataProviders);
            }
        }
    }

    //*************************************************************************
    //  Method: TryGetTypesFromFile()
    //
    /// <summary>
    /// Attempts to get an array of types implemented by an assembly.
    /// </summary>
    ///
    /// <param name="sFilePath">
    /// Full path to a file that might be an assembly.
    /// </param>
    ///
    /// <param name="aoTypes">
    /// Where the array of types implemented by the assembly gets stored if
    /// true is returned.
    /// </param>
    ///
    /// <returns>
    /// true if the types were obtained.
    /// </returns>
    //*************************************************************************

    private static Boolean
    TryGetTypesFromFile
    (
        String sFilePath,
        out Type [] aoTypes
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sFilePath) );

        aoTypes = null;

        Assembly oAssembly;

        try
        {
            oAssembly = Assembly.LoadFrom(sFilePath);
        }
        catch (FileLoadException)
        {
            return (false);
        }
        catch (BadImageFormatException)
        {
            return (false);
        }

        try
        {
            aoTypes = oAssembly.GetTypes();
        }
        catch (ReflectionTypeLoadException)
        {
            // This occurs when the loaded assembly has dependencies in an
            // assembly that hasn't been loaded.

            return (false);
        }

        return (true);
    }

    //*************************************************************************
    //  Method: AddGraphDataProvidersFromType()
    //
    /// <summary>
    /// Adds a type that implements either the newer <see
    /// cref="IGraphDataProvider2" /> interface or the older <see
    /// cref="IGraphDataProvider" /> interface.
    /// </summary>
    ///
    /// <param name="oType">
    /// The type that might implement one of the interfaces.
    /// </param>
    ///
    /// <param name="oGraphDataProviders">
    /// Collection to add the type to if it implements one of the interfaces.
    /// </param>
    //*************************************************************************

    private static void
    AddGraphDataProvidersFromType
    (
        Type oType,
        List<Object> oGraphDataProviders
    )
    {
        Debug.Assert(oType != null);
        Debug.Assert(oGraphDataProviders != null);

        // The techniques for checking types for a specified interface and
        // instantiating instances of those types are from the article "Let
        // Users Add Functionality to Your .NET Applications with Macros and
        // Plug-Ins" in the October 2003 issue of MSDN Magazine.

        if (!oType.IsAbstract)
        {
            if (
                typeof(IGraphDataProvider2).IsAssignableFrom(oType)
                ||
                typeof(IGraphDataProvider).IsAssignableFrom(oType)
                )
            {
                oGraphDataProviders.Add( Activator.CreateInstance(oType) );
            }
        }
    }

    //*************************************************************************
    //  Method: SortGraphDataProviders()
    //
    /// <summary>
    /// Sorts a list of graph data providers.
    /// </summary>
    ///
    /// <param name="oGraphDataProviders">
    /// The list to sort.
    /// </param>
    //*************************************************************************

    private static void
    SortGraphDataProviders
    (
        List<Object> oGraphDataProviders
    )
    {
        Debug.Assert(oGraphDataProviders != null);

        oGraphDataProviders.Sort(
            delegate
            (
                Object oGraphDataProviderA,
                Object oGraphDataProviderB
            )
            {
                Debug.Assert(oGraphDataProviderA != null);
                Debug.Assert(oGraphDataProviderB != null);

                return ( GetGraphDataProviderName(oGraphDataProviderA).
                    CompareTo( GetGraphDataProviderName(oGraphDataProviderB) )
                    );
            }
            );
    }


    //*************************************************************************
    //  Private members
    //*************************************************************************

    /// File name of the NodeXL assembly that implements built-in graph data
    /// providers, without a path.

    private const String GraphDataProvidersFileName =
        "Smrf.NodeXL.GraphDataProviders.dll";
}
}
