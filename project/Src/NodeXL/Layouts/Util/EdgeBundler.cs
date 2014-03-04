﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Smrf.NodeXL.Core;
using System.Drawing;
using System.Threading;
//using System.Diagnostics;

namespace Smrf.NodeXL.Layouts
{
    /// <summary>
    /// EdgeBundler class is intended to be used for bundling and straightening of the edges of the graph.
    /// The goal is to get the layout that is less clutered and more suitable for analiyzing.
    /// 
    /// This class is based on the paper "Force-Directed Edge Bundling for Graph Visualization"
    /// by Danny Holten and Jarke J. van Wijk.
    /// http://www.win.tue.nl/~dholten/papers/forcebundles_eurovis.pdf
    /// 
    /// It was implemented and modified by Luka Potkonjak.
    /// </summary>
    public class EdgeBundler
    {
        /// <summary>
        /// Bundles edges of the graph.
        /// </summary>
        /// 
        /// <param name="graph">
        /// Graph whose edges should be bundled
        /// </param>
        /// 
        /// <param name="rectangle">
        /// Rectangle in which the graph is laid out.
        /// Control points of bundled edges should not fall outside of this rectangle.
        /// </param>
        public void BundleAllEdges(IGraph graph, Rectangle rectangle)
        {
            this.rectangle = rectangle;

            if (graph.Directedness != GraphDirectedness.Undirected)
                directed = true;
            else directed = false;

            AddDataForAllEdges(graph.Edges);

            //Stopwatch sw = new Stopwatch();
            //sw.Start();

            FindCompatibleEdges(edgeGroupData);

            //sw.Stop();


            DivideAllEdges(subdivisionPoints);

            //sw = new Stopwatch();
            //sw.Start();

            for (int i = 0; i < iterations; i++)
                MoveControlPoints(edgeGroupData);

            //prevents oscillating movements
            for (int i = 0; i < 5; i++)
            {
                cooldown *= 0.5f;
                MoveControlPoints(edgeGroupData);
            }

            //sw.Stop();

            cooldown = 1f;

            if (straightening > 0)
                StraightenEdgesInternally(edgeGroupData, straightening);

            foreach (IEdge e in graph.Edges)
            {
                if (!e.IsSelfLoop)
                {
                    KeyPair key = new KeyPair(e.Vertices[0].ID, e.Vertices[1].ID);
                    e.SetValue(ReservedMetadataKeys.PerEdgeIntermediateCurvePoints, edgeGroupData[key].controlPoints);
                }
            }
        }

        /// <summary>
        /// Bundles specified edges. Shapes of all the other edges remain the same,
        /// so this method is faster than the one for bundling all edges, but also produces less optimal layout.
        /// </summary>
        /// 
        /// <param name="graph">
        /// Parent graph of the edge set
        /// </param>
        /// 
        /// <param name="edges">
        /// Edges that should be bundled
        /// </param>
        /// 
        /// <param name="rectangle">
        /// Rectangle in which the graph is laid out.
        /// Control points of bundled edges should not fall outside of this rectangle.
        /// </param>
        public void BundleEdges(IGraph graph, IEnumerable<IEdge> edges, Rectangle rectangle)
        {
            this.rectangle = rectangle;
            if (graph.Directedness != GraphDirectedness.Undirected)
                directed = true;
            else directed = false;
            AddAllExistingData(graph.Edges);
            AddEdgeDataForMovedEdges(edges);
            FindCompatibleEdges(movedEdgeGroupData);
            ResetMovedEdges();

            for (int i = 0; i < iterations; i++)
                MoveControlPoints(movedEdgeGroupData);

            for (int i = 0; i < 5; i++)
            {
                cooldown *= 0.5f;
                MoveControlPoints(movedEdgeGroupData);
            }

            cooldown = 1f;

            if (straightening > 0)
                StraightenEdgesInternally(movedEdgeGroupData, straightening);

            foreach (IEdge e in edges)
            {
                EdgeGroupData ed;
                KeyPair key = new KeyPair(e.Vertices[0].ID, e.Vertices[1].ID);
                movedEdgeGroupData.TryGetValue(key, out ed);
                if (ed != null)
                    e.SetValue(ReservedMetadataKeys.PerEdgeIntermediateCurvePoints, ed.controlPoints);
            }
        }

        /// <summary>
        /// Collects edge data from the specified edges
        /// </summary>
        /// 
        /// <param name="edges">
        /// Edges whose data should be added to the collection
        /// </param>
        private void AddEdgeDataForMovedEdges(IEnumerable<IEdge> edges)
        {
            foreach (IEdge e in edges)
                if (!e.IsSelfLoop)
                {
                    KeyPair key = new KeyPair(e.Vertices[0].ID, e.Vertices[1].ID);
                    if (!movedEdgeGroupData.ContainsKey(key))
                        movedEdgeGroupData.Add(key, edgeGroupData[key]);
                }
        }

        /// <summary>
        /// Collects edge data from all edges in the specified collection.
        /// Used by the <see cref="BundleAllEdges"/> method.
        /// </summary>
        /// 
        /// <param name="edges">
        /// Collection of edges whose data should be collected
        /// </param>
        private void AddDataForAllEdges(IEdgeCollection edges)
        {
            foreach (IEdge e in edges)
                if (!e.IsSelfLoop)
                    AddEdgeData(e);
        }

        /// <summary>
        /// Collects data from the specified edge
        /// </summary>
        /// 
        /// <param name="e">
        /// Edge to collect data from
        /// </param>
        private void AddEdgeData(IEdge e)
        {
            EdgeGroupData ed;
            KeyPair key = new KeyPair(e.Vertices[0].ID, e.Vertices[1].ID);

            edgeGroupData.TryGetValue(key, out ed);

            if (ed == null)
            {
                PointF p1 = e.Vertices[0].Location;
                PointF p2 = e.Vertices[1].Location;
                ed = new EdgeGroupData();
                ed.v1 = p1;
                ed.v2 = p2;
                ed.id = key;
                PointF mid = VectorTools.MidPoint(p1, p2);
                ed.middle = mid;
                ed.length = VectorTools.Distance(p1, p2);
                ed.compatibleGroups = new Dictionary<KeyPair, GroupPairData>();
                //ed.edges = new HashSet<int>();
                ed.edgeCount = 0;
                edgeGroupData.Add(key, ed);
            }
            //ed.edges.Add(e.ID);
            ed.edgeCount++;
        }

        /// <summary>
        /// Collects edge data from all edges in the specified collection.
        /// Used for edges that already have control points metadata.
        /// </summary>
        /// 
        /// <param name="edges">
        /// Collection of edges whose data should be collected
        /// </param>
        private void AddAllExistingData(IEdgeCollection edges)
        {
            subdivisionPoints = 0;
            foreach (IEdge e in edges)
                if (!e.IsSelfLoop)
                    AddExistingData(e);
        }

        /// <summary>
        /// Collects data from the specified edge.
        /// Used for edges that already have control points metadata.
        /// </summary>
        /// 
        /// <param name="e">
        /// Edge to collect data from
        /// </param>
        private void AddExistingData(IEdge e)
        {
            EdgeGroupData ed;
            KeyPair key = new KeyPair(e.Vertices[0].ID, e.Vertices[1].ID);

            edgeGroupData.TryGetValue(key, out ed);

            if (ed == null)
            {
                PointF p1 = e.Vertices[0].Location;
                PointF p2 = e.Vertices[1].Location;
                ed = new EdgeGroupData();
                ed.v1 = p1;
                ed.v2 = p2;
                ed.id = key;
                PointF mid = VectorTools.MidPoint(p1, p2);
                ed.middle = mid;
                ed.length = VectorTools.Distance(p1, p2);

                ed.controlPoints = (PointF[])e.GetValue(ReservedMetadataKeys.PerEdgeIntermediateCurvePoints);

                if (subdivisionPoints == 0) subdivisionPoints = ed.controlPoints.Length;
                ed.newControlPoints = new PointF[subdivisionPoints];
                ed.k = springConstant * (subdivisionPoints + 1) / ed.length;
                if (ed.k > 0.5f) ed.k = 0.5f;
                //ed.edges = new HashSet<int>();
                ed.edgeCount = 0;
                ed.compatibleGroups = new Dictionary<KeyPair, GroupPairData>();
                edgeGroupData.Add(key, ed);
            }
            //ed.edges.Add(e.ID);
            ed.edgeCount++;
        }

        /// <summary>
        /// Calculates angle compatibility of the two edges
        /// </summary>
        /// 
        /// <param name="ed1">
        /// First edge to be used in calculation
        /// </param>
        /// 
        /// <param name="ed2">
        /// Second edge to be used in calculation
        /// </param>
        /// 
        /// <returns>
        /// Angle compatibility coefficient ranging from 0 to 1
        /// </returns>
        private float AngleCompatibility(EdgeGroupData ed1, EdgeGroupData ed2)
        {
            float a = VectorTools.Angle(ed1.v1, ed1.v2, ed2.v1, ed2.v2);
            return (float)Math.Abs(Math.Cos(a));
        }

        /// <summary>
        /// Calculates position compatibility of the two edges
        /// </summary>
        /// 
        /// <param name="ed1">
        /// First edge to be used in calculation
        /// </param>
        /// 
        /// <param name="ed2">
        /// Second edge to be used in calculation
        /// </param>
        /// 
        /// <returns>
        /// Position compatibility coefficient ranging from 0 to 1
        /// </returns>
        private float PositionCompatibility(EdgeGroupData ed1, EdgeGroupData ed2)
        {
            float avg = (ed1.length + ed2.length) / 2;
            float dis = VectorTools.Distance(ed1.middle, ed2.middle);
            if ((avg + dis) == 0) return 0;
            return (avg / (avg + dis));
        }

        /// <summary>
        /// Calculates scale compatibility of the two edges
        /// </summary>
        /// 
        /// <param name="ed1">
        /// First edge to be used in calculation
        /// </param>
        /// 
        /// <param name="ed2">
        /// Second edge to be used in calculation
        /// </param>
        /// 
        /// <returns>
        /// Scale compatibility coefficient ranging from 0 to 1
        /// </returns>
        private float ScaleCompatibility(EdgeGroupData ed1, EdgeGroupData ed2)
        {
            float l1 = ed1.length;
            float l2 = ed2.length;
            float l = l1 + l2;
            if (l == 0)
                return 0;
            else
            {
                float ret = 4 * l1 * l2 / (l * l);
                return ret * ret;
            }
        }

        /// <summary>
        /// Calculates compatibility of the two edges.
        /// Combines angle, position, scale, and visibility compatibility coefficient.
        /// </summary>
        /// 
        /// <param name="ed1">
        /// First edge to be used in calculation
        /// </param>
        /// 
        /// <param name="ed2">
        /// Second edge to be used in calculation
        /// </param>
        /// 
        /// <returns>
        /// Compatibility coefficient ranging from 0 to 1
        /// </returns>
        private float CalculateCompatibility(EdgeGroupData ed1, EdgeGroupData ed2)
        {
            float c = PositionCompatibility(ed1, ed2);
            if (c > threshold) c *= ScaleCompatibility(ed1, ed2);
            else return 0;
            if (c > threshold) c *= AngleCompatibility(ed1, ed2);
            else return 0;
            if (c > threshold) c *= VisibilityCompatibility(ed1, ed2);
            else return 0;
            if (c > threshold)
                return c;
            else return 0;
        }

        /// <summary>
        /// Calculates visibility compatibility of the two edges.
        /// Uses lower of the two calculated visibility coefficients.
        /// </summary>
        /// 
        /// <param name="ed1">
        /// First edge to be used in calculation
        /// </param>
        /// 
        /// <param name="ed2">
        /// Second edge to be used in calculation
        /// </param>
        /// 
        /// <returns>
        /// Visibility compatibility coefficient ranging from 0 to 1
        /// </returns>
        private float VisibilityCompatibility(EdgeGroupData ed1, EdgeGroupData ed2)
        {
            float c1, c2;

            c1 = VisibilityCoefficient(ed1, ed2);
            if (c1 == 0)
                return 0;
            c2 = VisibilityCoefficient(ed2, ed1);

            return Math.Min(c1, c2);
        }

        /// <summary>
        /// Calculates visibility coefficient of the two edges.
        /// </summary>
        /// 
        /// <param name="ed1">
        /// First edge to be used in calculation
        /// </param>
        /// 
        /// <param name="ed2">
        /// Second edge to be used in calculation
        /// </param>
        /// 
        /// <returns>
        /// Compatibility coefficient ranging from 0 to 1
        /// </returns>
        private float VisibilityCoefficient(EdgeGroupData ed1, EdgeGroupData ed2)
        {
            float c;
            PointF p1 = ed1.v1;
            PointF p2 = ed1.v2;
            PointF q1 = ed2.v1;
            PointF q2 = ed2.v2;

            PointF pn = new PointF();
            pn.X = p1.Y - p2.Y;
            pn.Y = p2.X - p1.X;

            PointF pn1 = pn + new SizeF(p1);
            PointF pn2 = pn + new SizeF(p2);

            PointF i1 = new PointF();
            PointF i2 = new PointF();

            float r1 = 0, r2 = 0;

            if (!Intersects(q1, q2, p1, pn1, ref i1, ref r1)) return 0;
            Intersects(q1, q2, p2, pn2, ref i2, ref r2);

            if ((r1 < 0 && r2 < 0) || (r1 > 1 && r2 > 1)) return 0;

            PointF im = VectorTools.MidPoint(i1, i2);

            PointF qm = ed2.middle;

            float i = VectorTools.Distance(i1, i2);
            float m = VectorTools.Distance(qm, im);

            if (i == 0) return 0;

            c = 1f - 2f * m / i;

            if (c < 0)
                return 0;
            else
                return c;
        }

        /// <summary>
        /// Calculates directedness of the two edges.
        /// </summary>
        /// 
        /// <param name="ed1">
        /// First edge to be used in calculation
        /// </param>
        /// 
        /// <param name="ed2">
        /// Second edge to be used in calculation
        /// </param>
        /// 
        /// <returns>
        /// True if edges have roughly the same direction, false otherwise
        /// </returns>
        private bool CalculateDirectedness(EdgeGroupData ed1, EdgeGroupData ed2)
        {
            if ((VectorTools.Distance(ed1.v1, ed2.v1) + VectorTools.Distance(ed1.v2, ed2.v2)) <
                (VectorTools.Distance(ed1.v1, ed2.v2) + VectorTools.Distance(ed1.v2, ed2.v1)))
                return true;
            else return false;
        }

        /// <summary>
        /// Finds an intersection point of the two lines
        /// </summary>
        /// 
        /// <param name="p1">
        /// First point of the first line
        /// </param>
        /// 
        /// <param name="p2">
        /// Second point of the first line
        /// </param>
        /// 
        /// <param name="q1">
        /// First point of the second line
        /// </param>
        /// 
        /// <param name="q2">
        /// Second point of the second line
        /// </param>
        /// 
        /// <param name="intersection">
        /// Point of intersection
        /// </param>
        /// 
        /// <param name="rp">
        /// Parameter used for determining on which segment the intersection point lies
        /// </param>
        /// 
        /// <returns>
        /// True if lines are not parallel, false otherwise
        /// </returns>
        private bool Intersects(PointF p1, PointF p2, PointF q1, PointF q2, ref PointF intersection, ref float rp)
        {
            float q = (p1.Y - q1.Y) * (q2.X - q1.X) - (p1.X - q1.X) * (q2.Y - q1.Y);
            float d = (p2.X - p1.X) * (q2.Y - q1.Y) - (p2.Y - p1.Y) * (q2.X - q1.X);

            if (d == 0) // parallel lines
            {
                return false;
            }

            float r = q / d;

            q = (p1.Y - q1.Y) * (p2.X - p1.X) - (p1.X - q1.X) * (p2.Y - p1.Y);
            float s = q / d;

            intersection = p1 + new SizeF(VectorTools.Multiply(p2 - new SizeF(p1), r));

            return true;
        }

        /// <summary>
        /// Finds compatible edges for the specified set of edges
        /// </summary>
        /// 
        /// <param name="edgeSet">
        /// Edges for which we should find compatible edges
        /// </param>
        private void FindCompatibleEdges(Dictionary<KeyPair, EdgeGroupData> edgeSet)
        {
            foreach (KeyValuePair<KeyPair, EdgeGroupData> p1 in edgeSet)
            {
                if (p1.Value.length < 50) continue;//??????
                foreach (KeyValuePair<KeyPair, EdgeGroupData> p2 in edgeGroupData)
                {
                    if (p2.Value.length < 50) continue;//??????
                    if (((p1.Key.k1 == p2.Key.k1) && (p1.Key.k2 == p2.Key.k2))
                        || (p1.Value.compatibleGroups.ContainsKey(p2.Key)))
                        continue;
                    //if ((((p1.Value.v1 == p2.Value.v1) && (p1.Value.v1.Y == p2.Value.v1.Y)) && (p1.Value.v2.X == p2.Value.v2.X) && (p1.Value.v2.Y == p2.Value.v2.Y)) ||
                    //(((p1.Value.v1.X == p2.Value.v2.X) && (p1.Value.v1.Y == p2.Value.v2.Y)) && (p1.Value.v2.X == p2.Value.v1.X) && (p1.Value.v2.Y == p2.Value.v1.Y)))
                    //    continue;
                    float c = CalculateCompatibility(p1.Value, p2.Value);
                    if (c == 0) continue;
                    bool d = CalculateDirectedness(p1.Value, p2.Value);
                    GroupPairData epd = new GroupPairData(c, p1.Value, p2.Value, d);
                    p1.Value.compatibleGroups.Add(p2.Key, epd);
                    p2.Value.compatibleGroups.Add(p1.Key, epd);
                }
            }
        }

        /// <summary>
        /// Divides edges into segments by adding subdivision points to them
        /// </summary>
        /// 
        /// <param name="subdivisionPointsNum">
        /// Number of subdivision points that should be created
        /// </param>
        private void DivideAllEdges(int subdivisionPointsNum)
        {
            if (subdivisionPointsNum < 1) return;

            foreach (EdgeGroupData ed in edgeGroupData.Values)
                DivideEdge(ed, subdivisionPointsNum);

            subdivisionPoints = subdivisionPointsNum;
        }


        /// <summary>
        /// Straightens moved edges.
        /// </summary>
        private void ResetMovedEdges()
        {
            foreach (EdgeGroupData ed in movedEdgeGroupData.Values)
                DivideEdge(ed, subdivisionPoints);
        }

        /// <summary>
        /// Divides an edge into segments by adding subdivision points to it
        /// </summary>
        /// 
        /// <param name="ed">
        /// Edge data that is used for creating new subdivision points
        /// </param>
        /// 
        /// <param name="subdivisionPointsNum">
        /// Number of subdivision points that should be created
        /// </param>
        private void DivideEdge(EdgeGroupData ed, int subdivisionPointsNum)
        {
            float r = ed.length / (subdivisionPointsNum + 1);
            PointF[] sPoints = new PointF[subdivisionPointsNum];
            ed.newControlPoints = new PointF[subdivisionPointsNum];
            PointF move;
            if (ed.length == 0)
                move = new PointF(0, 0);
            else
                move = VectorTools.Multiply(ed.v2 - new SizeF(ed.v1), 1f / ed.length);
            for (int i = 0; i < subdivisionPointsNum; i++)
                sPoints[i] = ed.v1 + new SizeF(VectorTools.Multiply(move, r * (i + 1)));
            ed.controlPoints = sPoints;
            ed.k = springConstant * (subdivisionPointsNum + 1) / ed.length;
            if (ed.k > 0.5f) ed.k = 0.5f;
        }

        /// <summary>
        /// Doubles subdivision points for an edge by adding one new subdivision point between each two
        /// </summary>
        /// 
        /// <param name="ed">
        /// Edge data that contains subdivision points to be doubled
        /// </param>
        private void DoubleSubdivisionPoints(EdgeGroupData ed)
        {
            if (subdivisionPoints == 0) //make one subdivision point
            {
                ed.k = springConstant * 2 / ed.length;
                if (ed.k > 0.5f) ed.k = 0.5f;
                ed.controlPoints = new PointF[1];
                ed.newControlPoints = new PointF[1];
                ed.controlPoints[0] = ed.middle;

                return;
            }

            PointF[] sPoints = ed.controlPoints;
            PointF[] sPointsDoubled = new PointF[subdivisionPoints * 2 + 1];
            ed.newControlPoints = new PointF[subdivisionPoints * 2 + 1];
            for (int i = 0; i < subdivisionPoints; i++)
                sPointsDoubled[i * 2 + 1] = sPoints[i];


            for (int i = 0; i < subdivisionPoints - 1; i++)
                sPointsDoubled[i * 2 + 2] = VectorTools.MidPoint(sPoints[i], sPoints[i + 1]);


            sPointsDoubled[0] = VectorTools.MidPoint(ed.v1, sPoints[0]);
            sPointsDoubled[subdivisionPoints * 2] = VectorTools.MidPoint(sPoints[subdivisionPoints - 1], ed.v2);
            //ed.K = springConstant * (subdivisionPoints * 2 + 2) / ed.Length;
            ed.k *= 2f;
            if (ed.k > 0.5f) ed.k = 0.5f;
            ed.controlPoints = sPointsDoubled;
        }

        /// <summary>
        /// Doubles subdivision points for all edges
        /// </summary>
        private void DoubleSubdivisionPointsForAllEdges()
        {
            foreach (EdgeGroupData ed in edgeGroupData.Values) DoubleSubdivisionPoints(ed);
            subdivisionPoints = subdivisionPoints * 2 + 1;
        }

        /// <summary>
        /// Calculates new positions for the control points of an edge by applying elastic and electrostatic forces to them
        /// </summary>
        /// 
        /// <param name="o">
        /// Edge data that contains subdivision points to be moved
        /// </param>
        private void CalculateNewControlPoints(Object o)
        {
            EdgeGroupData ed = (EdgeGroupData)o;
            for (int i = 0; i < subdivisionPoints; i++)
            {
                PointF p = ed.controlPoints[i];
                PointF p1, p2;
                if (i == 0) p1 = ed.v1; else p1 = ed.controlPoints[i - 1];
                if (i == (subdivisionPoints - 1)) p2 = ed.v2; else p2 = ed.controlPoints[i + 1];
                SizeF sp = new SizeF(p);
                PointF f = VectorTools.Multiply((p1 - sp) + new SizeF((p2 - sp)), ed.k);
                PointF r = new PointF(0, 0);
                foreach (GroupPairData epd in ed.compatibleGroups.Values)
                {
                    PointF q;
                    float j = 1f;
                    EdgeGroupData ed2;
                    if ((epd.ed1.id.k1 == ed.id.k1) && (epd.ed1.id.k2 == ed.id.k2))
                        ed2 = epd.ed2;
                    else
                        ed2 = epd.ed1;

                    if (epd.d)
                        q = ed2.controlPoints[i];
                    else
                    {
                        q = ed2.controlPoints[subdivisionPoints - i - 1];
                        if (directed && repulseOpposite) j = repulsionCoefficient;
                    }
                    PointF fs = q - sp;
                    //PointF fs = new PointF(q.X - p.X, q.Y - p.Y);

                    float l = VectorTools.Length(fs);
                    if (l > 0)//???
                    {
                        fs = VectorTools.Multiply(fs, epd.c / (l));

                        //fs = VectorTools.Multiply(fs, VectorTools.Length(fs) * ed2.edges.Count);
                        fs = VectorTools.Multiply(fs, VectorTools.Length(fs) * ed2.edgeCount);

                        r.X += (j * fs.X);
                        r.Y += (j * fs.Y);
                    }
                }

                float rl = VectorTools.Length(r);
                if (rl>0)
                    r = VectorTools.Multiply(r, (float)(1.0/Math.Sqrt(rl)));

                PointF move = new PointF(f.X + r.X, f.Y + r.Y);
                float moveL = VectorTools.Length(move);

                //float len = ed.Length / (subdivisionPoints + 1);
                //if (moveL > (len)) move = VectorTools.Multiply(move, len*cooldown / moveL);
                //if (moveL != 0) move = VectorTools.Multiply(move, cooldown / moveL);
                move = VectorTools.Multiply(move, cooldown*0.5f);
                ed.newControlPoints[i] = move + sp;

                if (ed.newControlPoints[i].X < rectangle.Left)
                    ed.newControlPoints[i].X = rectangle.Left;
                else
                    if (ed.newControlPoints[i].X > rectangle.Right)
                        ed.newControlPoints[i].X = rectangle.Right;

                if (ed.newControlPoints[i].Y < rectangle.Top)
                    ed.newControlPoints[i].Y = rectangle.Top;
                else
                    if (ed.newControlPoints[i].Y > rectangle.Bottom)
                        ed.newControlPoints[i].Y = rectangle.Bottom;
            }
            if (useThreading) sem.Release();
        }

        /// <summary>
        /// Moves control points for the specified edges
        /// </summary>
        /// 
        /// <param name="groupsToMove">
        /// Edges that should be moved
        /// </param>
        private void MoveControlPoints(Dictionary<KeyPair, EdgeGroupData> groupsToMove)
        {
            if (useThreading)
            {
                foreach (EdgeGroupData ed in groupsToMove.Values)
                    ThreadPool.QueueUserWorkItem(new WaitCallback(this.CalculateNewControlPoints), ed);
                for (int i = 0; i < groupsToMove.Values.Count; i++)
                    sem.WaitOne();
            }
            else
            {
                foreach (EdgeGroupData ed in groupsToMove.Values)
                    CalculateNewControlPoints(ed);
            }


            foreach (EdgeGroupData ed in groupsToMove.Values)
            {
                ed.controlPoints = ed.newControlPoints;
                ed.newControlPoints = new PointF[subdivisionPoints];
            }

            //if (cooldown > 0.05) cooldown *= 0.95f; else cooldown = 0;
            //cooldown *= 0.99f;
        }

        /// <summary>
        /// Straightens the edges using internal data sturctures
        /// </summary>
        /// 
        /// <param name="groupsToStraighten">
        /// Groups of edges that should be straightened
        /// </param>
        /// 
        /// <param name="s">
        /// Specifies the amount of straightening, from 0 to 1
        /// </param>
        private void StraightenEdgesInternally(Dictionary<KeyPair, EdgeGroupData> groupsToStraighten, float s)
        {
            foreach (EdgeGroupData ed in groupsToStraighten.Values)
            {
                for (int i = 0; i < subdivisionPoints; i++)
                {
                    PointF p = ed.controlPoints[i];
                    p = VectorTools.Multiply(p, 1 - s) +
                        new SizeF(VectorTools.Multiply(ed.v1 +
                            new SizeF(VectorTools.Multiply(ed.v2 -
                                new SizeF(ed.v1), 1.0f * (i + 1) / (subdivisionPoints + 1))), s));
                    ed.controlPoints[i].X = p.X;
                    ed.controlPoints[i].Y = p.Y;
                }
            }
        }

        /// <summary>
        /// Moves the control points of all the edges of the graph closer to their original position on the straight edge
        /// </summary>
        /// 
        /// <param name="graph">
        /// Graph whose edges should be straightened
        /// </param>
        /// 
        /// <param name="s">
        /// Specifies the amount of straightening, from 0 to 1
        /// </param>
        public void StraightenEdges(IGraph graph, float s)
        {
            foreach (IEdge e in graph.Edges)
            {
                if (e.IsSelfLoop) continue;
                PointF[] controlPoints = (PointF[])e.GetValue(ReservedMetadataKeys.PerEdgeIntermediateCurvePoints);
                PointF[] newControlPoints = new PointF[controlPoints.Length];
                for (int i = 0; i < controlPoints.Length; i++)
                {
                    PointF p = controlPoints[i];
                    p = VectorTools.Multiply(p, 1 - s) +
                        new SizeF(VectorTools.Multiply(e.Vertices[0].Location +
                            new SizeF(VectorTools.Multiply(e.Vertices[1].Location -
                                new SizeF(e.Vertices[0].Location), 1.0f * (i + 1) / (controlPoints.Length + 1))), s));
                    newControlPoints[i].X = p.X;
                    newControlPoints[i].Y = p.Y;
                }
                e.SetValue(ReservedMetadataKeys.PerEdgeIntermediateCurvePoints, newControlPoints);
            }
        }

        private Dictionary<KeyPair, EdgeGroupData> edgeGroupData = new Dictionary<KeyPair, EdgeGroupData>();

        private Dictionary<KeyPair, EdgeGroupData> movedEdgeGroupData = new Dictionary<KeyPair, EdgeGroupData>();

        private Rectangle rectangle;

        private int subdivisionPoints = 15;

        private int iterations = 50;

        private bool repulseOpposite = false;

        private bool directed = true;

        private bool useThreading = true;

        private float springConstant = 10f;

        private float threshold = 0.2f;

        private float cooldown = 1f;

        private float repulsionCoefficient = -0.1f;

        private float straightening = 0.15f;

        private Semaphore sem = new Semaphore(0, Int32.MaxValue);
        
        /// <summary>
        /// Gets or sets the number of subdivision points each edge should have.
        /// Default value is 15.
        /// </summary>
        public int SubdivisionPoints
        {
            get { return subdivisionPoints; }
            set { subdivisionPoints = value; }
        }

        /// <summary>
        /// Gets or sets the number of iterations for moving the control points.
        /// Default value is 50.
        /// </summary>
        public int Iterations
        {
            get { return iterations; }
            set { iterations = value; }
        }

        /// <summary>
        /// Gets or sets the value indicating whether opposite edges should attracts or repulse each other.
        /// Default value is false.
        /// </summary>
        public bool RepulseOpposite
        {
            get { return repulseOpposite; }
            set { repulseOpposite = value; }
        }

        /// <summary>
        /// Gets or sets the the value that determines if multiple threads should be used for the calculations.
        /// Default value is true.
        /// </summary>
        public bool UseThreading
        {
            get { return useThreading; }
            set { useThreading = value; }
        }

        /// <summary>
        /// Gets or sets the value for the spring constant.
        /// Edges are more easely bent if the value is lower.
        /// Default value is 10.
        /// </summary>
        public float SpringConstant
        {
            get { return springConstant; }
            set { springConstant = value; }
        }

        /// <summary>
        /// Gets or sets the treshold for the edge compatibility.
        /// Every pair of edges has the compatibility coefficient assigned to it.
        /// Range of the coefficient is from 0 to 1.
        /// Edges that have coefficient lower than the treshold between them are not considered for interaction.
        /// Default value is 0.2.
        /// </summary>
        public float Threshold
        {
            get { return threshold; }
            set { threshold = value; }
        }

        /// <summary>
        /// If repulseOpposite is true, this determines how much will opposite edges repulse eachother.
        /// From -1 to 0.
        /// Default is -0.1
        /// </summary>
        public float RepulsionCoefficient
        {
            get { return repulsionCoefficient; }
            set { repulsionCoefficient = value; }
        }

        /// <summary>
        /// Gets or sets the amount of straightening that will be applied after every bundling.
        /// This can produce better-looking graphs.
        /// Default value is 0.15, range is from 0 to 1.
        /// </summary>
        public float Straightening
        {
            get { return straightening; }
            set { straightening = value; }
        }

        struct KeyPair
        {
            public KeyPair(int n1, int n2)
            {
                k1 = n1;
                k2 = n2;
            }

            public int k1;
            
            public int k2;
        }

        /// <summary>
        /// Class used for storing the needed edge metadata
        /// </summary>
        class EdgeGroupData
        {
            public PointF v1;

            public PointF v2;

            public PointF middle;

            public PointF[] controlPoints;

            public PointF[] newControlPoints;

            public Dictionary<KeyPair, GroupPairData> compatibleGroups;

            //public HashSet<Int32> edges;

            public int edgeCount;

            public float length;

            public float k;

            public KeyPair id;
        }

        /// <summary>
        /// Class used for storing data for a pair of groups of edges (direction and compatibility coefficient)
        /// </summary>
        class GroupPairData
        {
            public GroupPairData(float cc, EdgeGroupData e1, EdgeGroupData e2, bool dd)
            {
                c = cc;
                ed1 = e1;
                ed2 = e2;
                d = dd;
            }

            public float c;

            public EdgeGroupData ed1;

            public EdgeGroupData ed2;

            public bool d;
        }
    }

    /// <summary>
    /// Used for vector calculations
    /// </summary>
    static class VectorTools
    {
        public static PointF Multiply(PointF p, float f)
        {
            return new PointF(p.X * f, p.Y * f);
        }

        public static PointF MidPoint(PointF p1, PointF p2)
        {
            return Multiply((p1 + new SizeF(p2)), 0.5f);
        }

        public static float Length(PointF p)
        {
            return (float)Math.Sqrt(p.X * p.X + p.Y * p.Y);
        }

        public static PointF Normalize(PointF p)
        {
            float l = Length(p);
            if (l == 0)
                return p;
            else
                return Multiply(p, 1f / l);
        }

        public static float Angle(PointF p1, PointF p2, PointF q1, PointF q2)
        {
            return (float)(Math.Atan2(p2.Y - p1.Y, p2.X - p1.X) - Math.Atan2(q2.Y - q1.Y, q2.X - q1.X));
        }

        public static float Distance(PointF p, PointF q)
        {
            return Length(q - new SizeF(p));
        }
    }
}
