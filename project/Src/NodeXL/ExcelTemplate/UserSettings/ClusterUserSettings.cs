
using System;
using System.Configuration;
using System.Diagnostics;
using Smrf.NodeXL.Algorithms;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: ClusterUserSettings
//
/// <summary>
/// Stores the user's settings for partitioning the graph into clusters.
/// </summary>
//*****************************************************************************

[ SettingsGroupNameAttribute("ClusterUserSettings") ]

public class ClusterUserSettings : NodeXLApplicationSettingsBase
{
    //*************************************************************************
    //  Constructor: ClusterUserSettings()
    //
    /// <summary>
    /// Initializes a new instance of the ClusterUserSettings class.
    /// </summary>
    //*************************************************************************

    public ClusterUserSettings()
    {
        // (Do nothing.)

        AssertValid();
    }

    //*************************************************************************
    //  Property: ClusterAlgorithm
    //
    /// <summary>
    /// Gets or sets the algorithm used to partition a graph into clusters.
    /// </summary>
    ///
    /// <value>
    /// The algorithm used to partition a graph into clusters.  The default
    /// value is ClusterAlgorithm.ClausetNewmanMoore.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("ClausetNewmanMoore") ]

    public ClusterAlgorithm
    ClusterAlgorithm
    {
        get
        {
            AssertValid();

            return ( (ClusterAlgorithm)this[ClusterAlgorithmKey] );
        }

        set
        {
            this[ClusterAlgorithmKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: PutNeighborlessVerticesInOneCluster
    //
    /// <summary>
    /// Gets or sets a flag indicating whether neighborless vertices should be
    /// put into one cluster.
    /// </summary>
    ///
    /// <value>
    /// true to put the neighborless vertices into one cluster, false to let
    /// the cluster algorithm deal with them.  The default is false.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("false") ]

    public Boolean
    PutNeighborlessVerticesInOneCluster
    {
        get
        {
            AssertValid();

            return ( (Boolean)this[PutNeighborlessVerticesInOneClusterKey] );
        }

        set
        {
            this[PutNeighborlessVerticesInOneClusterKey] = value;

            AssertValid();
        }
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
    //  Protected constants
    //*************************************************************************

    /// Name of the settings key for the ClusterAlgorithm property.

    protected const String ClusterAlgorithmKey =
        "ClusterAlgorithm";

    /// Name of the settings key for the PutNeighborlessVerticesInOneCluster
    /// property.

    protected const String PutNeighborlessVerticesInOneClusterKey =
        "PutNeighborlessVerticesInOneCluster";


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}

}
