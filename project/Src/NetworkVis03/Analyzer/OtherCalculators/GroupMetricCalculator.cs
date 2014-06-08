using Smrf.NodeXL.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyzer
{
    class GroupMetricCalculator : AnalyzerBase
    {
        private BackgroundWorker m_obackgroundWorker;


        public GroupMetricCalculator() { }

        public override bool tryAnalyze(IGraph graph, BackgroundWorker bgw, out AnalyzeResultBase results)
        {
            Dictionary<int, OverallMetrics> groupsmetric;
            GroupsMetric oGroupsMetric;
            bool rv = TryCalculateGraphMetrics(graph, out groupsmetric);
            if (rv == true)
            {
                oGroupsMetric = new GroupsMetric(groupsmetric.Count);
                foreach (KeyValuePair<int, OverallMetrics> p in groupsmetric)
                    oGroupsMetric.Add(p.Key, p.Value);
            }
            else
                oGroupsMetric = new GroupsMetric(1);
             results = oGroupsMetric;
             return rv;
        }


        public override string AnalyzerDescription
        {
            get { return "Calculating groups metrics"; }
        }

        public Boolean TryCalculateGraphMetrics
            (IGraph graph, out Dictionary<int, OverallMetrics> groupsmetric)
        {
        Debug.Assert(graph != null);

        // Attempt to retrieve the group information the WorkbookReader object
        // may have stored as metadata on the graph.

        GroupInfo [] aoGroups;

        

        if (!GroupUtil.TryGetGroups(graph, out aoGroups))
        {
            groupsmetric = new Dictionary<int, OverallMetrics>(1);
            return (true);
        }

        groupsmetric = new Dictionary<int, OverallMetrics>(aoGroups.Length);
        
        int i =0;
        foreach (GroupInfo oGroupInfo in aoGroups)
        {
            ICollection<IVertex> oVertices = oGroupInfo.Vertices;

            if (oVertices == null || oVertices.Count == 0)
            {
                continue;
            }

            OverallMetrics oOverallMetrics;

            if ( !TryCalculateGraphMetricsForOneGroup(oGroupInfo, out oOverallMetrics) )
            {
                // The user cancelled.

                return (false);
            }
            groupsmetric.Add(i, oOverallMetrics);
            i++;
            
        }

        

        return (true);
    }

    protected Boolean TryCalculateGraphMetricsForOneGroup
        (GroupInfo oGroupInfo, out OverallMetrics oOverallMetrics)
    {
        Debug.Assert(oGroupInfo != null);
        Debug.Assert(oGroupInfo.Vertices != null);
        Debug.Assert(oGroupInfo.Vertices.Count > 0);
        
        

        oOverallMetrics = null;

        ICollection<IVertex> oVertices = oGroupInfo.Vertices;

        // Create a new graph from the vertices in the group and the edges that
        // connect them.

        IGraph oNewGraph = SubgraphCalculator.GetSubgraphAsNewGraph(oVertices);

        // Calculate the overall metrics for the new graph using the
        // OverallMetricCalculator class in the Algorithms namespace, which
        // knows nothing about Excel.

        return ( (new OverallMetricCalculator() ).
            TryCalculateGraphMetrics(oNewGraph,
                null,
                out oOverallMetrics) );
    }

    public Boolean
    HandlesDuplicateEdges
    {
        get
        {


            // Duplicate edges get counted by this class, so they should be
            // included in the graph.

            return (true);
        }
    }


    //*************************************************************************
    //  Protected constants
    //*************************************************************************

    /// Number format for Int32 columns.

    protected const String Int32NumericFormat = "0";

    /// Number format for Double columns.

    protected const String DoubleNumericFormat = "0.000";


    
    }
}
