
using System;
using System.Diagnostics;

namespace Smrf.NodeXL.GraphDataProviders.GraphServer
{
//*****************************************************************************
//  Class: GraphServerNetworkGraphDataProvider
//
/// <summary>
/// Uses the NodeXL Graph Server to get a network of people who have tweeted
/// a specified search term.
/// </summary>
///
/// <remarks>
/// Call <see cref="GraphDataProviderBase.TryGetGraphDataAsTemporaryFile" /> to
/// get the network as a temporary GraphML file.
/// </remarks>
//*****************************************************************************

public class GraphServerNetworkGraphDataProvider : GraphDataProviderBase
{
   //*************************************************************************
    //  Constructor: GraphServerNetworkGraphDataProvider()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="GraphServerNetworkGraphDataProvider" /> class.
    /// </summary>
    //*************************************************************************

    public GraphServerNetworkGraphDataProvider()
    :
    base(GraphDataProviderName,
        "get the network of people whose tweets contain a specified word."
        + "  The network is obtained from the NodeXL Graph Server, not"
        + " directly from Twitter"
        )
    {
        // (Do nothing.)

        AssertValid();
    }

    //*************************************************************************
    //  Method: CreateDialog()
    //
    /// <summary>
    /// Creates a dialog for getting graph data.
    /// </summary>
    ///
    /// <returns>
    /// A dialog derived from GraphDataProviderDialogBase.
    /// </returns>
    //*************************************************************************

    protected override GraphDataProviderDialogBase
    CreateDialog()
    {
        AssertValid();

        return ( new GraphServerGetNetworkDialog() );
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
    //  Public constants
    //*************************************************************************

    /// Value of the Name property.

    public const String GraphDataProviderName =
        "NodeXL Graph Server";


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}

}
