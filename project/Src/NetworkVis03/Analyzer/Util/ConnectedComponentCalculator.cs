using Smrf.NodeXL.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyzer
{
    public class ConnectedComponentCalculator
    {
        private bool m_sortAscending;
        public ConnectedComponentCalculator(bool sortAscending)
        {
        }
        public IList<LinkedList<IVertex>>
        CalculateStronglyConnectedComponents
        (
            IGraph graph,
            Boolean sortAscending
        )
        {
            Debug.Assert(graph != null);

            return (CalculateStronglyConnectedComponents(graph.Vertices, graph,
                sortAscending));
        }

        public IList<LinkedList<IVertex>>
        CalculateStronglyConnectedComponents
        (
            ICollection<IVertex> vertices,
            IGraph graph,
            Boolean sortAscending
        )
        {
            Debug.Assert(vertices != null);
            Debug.Assert(graph != null);

            Int32 iNextIndex = 0;
            Stack<IVertex> oStack = new Stack<IVertex>();

            // Note: A List is used instead of a LinkedList for the strongly
            // connected components only because LinkedList does not have a Sort()
            // method.

            List<LinkedList<IVertex>> oStronglyConnectedComponents =
                new List<LinkedList<IVertex>>();

            foreach (IVertex oVertex in vertices)
            {
                if (!oVertex.ContainsKey(
                    ReservedMetadataKeys.ConnectedComponentCalculatorIndex))
                {
                    RunTarjanAlgorithm(oVertex, oStack,
                        oStronglyConnectedComponents, ref iNextIndex);
                }
            }

            SortStronglyConnectedComponents(oStronglyConnectedComponents, graph,
                sortAscending);

            // Remove the metadata that was added to each vertex.

            foreach (IVertex oVertex in vertices)
            {
                oVertex.RemoveKey(
                    ReservedMetadataKeys.ConnectedComponentCalculatorIndex);

                oVertex.RemoveKey(
                    ReservedMetadataKeys.ConnectedComponentCalculatorLowLink);
            }

            return (oStronglyConnectedComponents);
        }


        protected void
        RunTarjanAlgorithm
        (
            IVertex oVertex,
            Stack<IVertex> oStack,
            List<LinkedList<IVertex>> oStronglyConnectedComponents,
            ref Int32 iNextIndex
        )
        {
            Debug.Assert(oVertex != null);

            Debug.Assert(!oVertex.ContainsKey(
                ReservedMetadataKeys.ConnectedComponentCalculatorIndex));

            Debug.Assert(oStack != null);
            Debug.Assert(oStronglyConnectedComponents != null);
            Debug.Assert(iNextIndex >= 0);

            SetIndex(oVertex, iNextIndex);
            SetLowLink(oVertex, iNextIndex);
            iNextIndex++;

            oStack.Push(oVertex);

            foreach (IEdge oIncidentEdge in oVertex.IncidentEdges)
            {
                IVertex oAdjacentVertex = oIncidentEdge.GetAdjacentVertex(oVertex);

                if (!oAdjacentVertex.ContainsKey(
                    ReservedMetadataKeys.ConnectedComponentCalculatorIndex))
                {
                    RunTarjanAlgorithm(oAdjacentVertex, oStack,
                        oStronglyConnectedComponents, ref iNextIndex);

                    SetLowLink(oVertex,
                        Math.Min(GetLowLink(oVertex), GetLowLink(oAdjacentVertex))
                        );
                }
                else if (oStack.Contains(oAdjacentVertex))
                {
                    SetLowLink(oVertex,
                        Math.Min(GetLowLink(oVertex), GetIndex(oAdjacentVertex))
                        );
                }
            }

            if (GetLowLink(oVertex) == GetIndex(oVertex))
            {
                LinkedList<IVertex> oStronglyConnectedComponent =
                    new LinkedList<IVertex>();

                IVertex oVertexInComponent;

                do
                {
                    oVertexInComponent = oStack.Pop();
                    oStronglyConnectedComponent.AddLast(oVertexInComponent);

                } while (oVertexInComponent.ID != oVertex.ID);

                oStronglyConnectedComponents.Add(oStronglyConnectedComponent);
            }
        }


        protected void
        SetIndex
        (
            IVertex oVertex,
            Int32 iIndex
        )
        {
            Debug.Assert(oVertex != null);

            oVertex.SetValue(
                ReservedMetadataKeys.ConnectedComponentCalculatorIndex,
                iIndex);
        }

        

        protected Int32
        GetIndex
        (
            IVertex oVertex
        )
        {
            Debug.Assert(oVertex != null);

            return ((Int32)oVertex.GetRequiredValue(
                ReservedMetadataKeys.ConnectedComponentCalculatorIndex,
                typeof(Int32)));
        }

        protected void
        SetLowLink
        (
            IVertex oVertex,
            Int32 iLowLink
        )
        {
            Debug.Assert(oVertex != null);

            oVertex.SetValue(
                ReservedMetadataKeys.ConnectedComponentCalculatorLowLink,
                iLowLink);
        }

        protected Int32
        GetLowLink
        (
            IVertex oVertex
        )
        {
            Debug.Assert(oVertex != null);

            return ((Int32)oVertex.GetRequiredValue(
                ReservedMetadataKeys.ConnectedComponentCalculatorLowLink,
                typeof(Int32)));
        }

        protected void
        SortStronglyConnectedComponents
        (
            List<LinkedList<IVertex>> oStronglyConnectedComponents,
            IGraph oGraph,
            Boolean bSortAscending
        )
        {
            Debug.Assert(oStronglyConnectedComponents != null);
            Debug.Assert(oGraph != null);

            // The key is a strongly connected component and the value is the
            // smallest vertex layout sort order within the component.

            Dictionary<LinkedList<IVertex>, Single>
                oSmallestSortableLayoutAndZOrder = null;

            if (oGraph.ContainsKey(
                ReservedMetadataKeys.SortableLayoutAndZOrderSet))
            {
                // The vertex layout sort orders have been set on the vertices.
                // Populate the dictionary.

                oSmallestSortableLayoutAndZOrder =
                    new Dictionary<LinkedList<IVertex>, Single>();

                foreach (LinkedList<IVertex> oStronglyConnectedComponent in
                    oStronglyConnectedComponents)
                {
                    oSmallestSortableLayoutAndZOrder.Add(
                        oStronglyConnectedComponent,
                        GetSmallestSortableLayoutAndZOrder(
                            oStronglyConnectedComponent)
                        );
                }
            }

            oStronglyConnectedComponents.Sort(
                delegate
                (
                    LinkedList<IVertex> oStronglyConnectedComponent1,
                    LinkedList<IVertex> oStronglyConnectedComponent2
                )
                {
                    // Sort the components first by increasing vertex count.

                    Int32 iCompareTo =
                        oStronglyConnectedComponent1.Count.CompareTo(
                            oStronglyConnectedComponent2.Count);

                    if (!bSortAscending)
                    {
                        iCompareTo *= -1;
                    }

                    if (iCompareTo == 0 &&
                        oSmallestSortableLayoutAndZOrder != null)
                    {
                        // Sub-sort components with the same vertex count by the
                        // smallest layout and z-order within the component.

                        iCompareTo = oSmallestSortableLayoutAndZOrder[
                            oStronglyConnectedComponent1].CompareTo(
                                oSmallestSortableLayoutAndZOrder[
                                    oStronglyConnectedComponent2]);
                    }

                    return (iCompareTo);
                }
                );
        }


        protected Single
        GetSmallestSortableLayoutAndZOrder
        (
            LinkedList<IVertex> oStronglyConnectedComponent
        )
        {
            Debug.Assert(oStronglyConnectedComponent != null);

            Single fSmallestSortableLayoutAndZOrder = Single.MaxValue;

            foreach (IVertex oVertex in oStronglyConnectedComponent)
            {
                Object oSortableLayoutAndZOrder;

                if (oVertex.TryGetValue(
                    ReservedMetadataKeys.SortableLayoutAndZOrder, typeof(Single),
                    out oSortableLayoutAndZOrder))
                {
                    fSmallestSortableLayoutAndZOrder = Math.Min(
                        fSmallestSortableLayoutAndZOrder,
                        (Single)oSortableLayoutAndZOrder);
                }
            }

            return ((fSmallestSortableLayoutAndZOrder == Single.MaxValue) ?
                0 : fSmallestSortableLayoutAndZOrder);
        }

        public LinkedList<IVertex>
        getMostConnectedComponent
        (
            IList<LinkedList<IVertex>> oStronglyConnectedComponents,
            IGraph oGraph
        )
        {
            Debug.Assert(oStronglyConnectedComponents != null);
            Debug.Assert(oGraph != null);

            int count = 0;
            LinkedList<IVertex> biggest = null;
            foreach (LinkedList<IVertex> oStronglyConnectedComponent in
                oStronglyConnectedComponents)
            {
                int temp = oStronglyConnectedComponent.Count;
                if (temp > count)
                {

                    count = temp;
                    biggest = oStronglyConnectedComponent;
                }
            }
            return biggest;
        }
    }
}
