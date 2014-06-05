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
    class GroupMetricCalculator : IAnalyzer
    {
        private BackgroundWorker m_obackgroundWorker;


        public GroupMetricCalculator() { }

        public AnalyzeResultBase analyze(IGraph graph)
        {
            Dictionary<int, OverallMetrics> groupsmetric;
            TryCalculateGraphMetrics(graph, out groupsmetric);
            GroupsMetric result = new GroupsMetric(groupsmetric.Count);
            foreach (KeyValuePair<int, OverallMetrics> p in groupsmetric)
                result.Add(p.Key, p.Value);

            return result;

        }

        public void setBackgroundWorker(BackgroundWorker b)
        {
            m_obackgroundWorker = b;
        }

        public string GraphMetricDescription
        {
            get { return "~~~"; }
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

    //*************************************************************************
    //  Method: CreateGraphMetricColumnWithID()
    //
    /// <summary>
    /// Creates a <see cref="GraphMetricColumnWithID" /> for one column of the
    /// group table.
    /// </summary>
    ///
    /// <param name="sColumnName">
    /// Name of the column.
    /// </param>
    ///
    /// <param name="sNumberFormat">
    /// Number format of the column, or null if the column is not numeric.
    /// Sample: "0.00".
    /// </param>
    ///
    /// <param name="oGraphMetricValues">
    /// Graph metric values to include in the column.
    /// </param>
    //*************************************************************************

    

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
