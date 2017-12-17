using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlowNetworkToolKit.Core.Base.Algorithm;
using FlowNetworkToolKit.Core.Base.Network;

namespace FlowNetworkToolKit.Algorithms
{
    class PushRelabel : BaseMaxFlowAlgorithm
    {
        private int[] height;
        private double[] excess;

        public PushRelabel()
        {
            Name = "PushRelabel";
            Url = "";
            Description =
                @"";


        }

        protected override void Init()
        {
            height = new int[graph.NodeCount];
            for (int i = 0; i < height.Length; i++)
                height[i] = 0;
            excess = new double[graph.NodeCount];
            for (int i = 0; i < excess.Length; i++)
                excess[i] = 0;
        }

        protected override void Logic()
        {
            SearchMaxFlow();
        }

        private void Preflow(int s)
        {
            // Making h of source Vertex equal to no. of vertices
            // Height of other vertices is 0.
            height[graph.Source] = graph.NodeCount;

            // If current edge goes from source
            foreach (var edge in graph.Nodes[graph.Source].OutcomingEdges)
            {
                // Flow is equal to capacity
                edge.AddFlow(edge.ResidualCapacityTo(edge.To), edge.To);
                // Initialize excess flow for adjacent v
                excess[edge.To] += edge.Flow;
            }
        }

        // returns index of overflowing Vertex (== hasActive)
        private int OverFlow()
        {
            for (int i = 1; i < excess.Count() - 1; i++)
                if (excess[i] > 0)
                    return i;

            // -1 if no overflowing Vertex
            return -1;
        }

        public void SearchMaxFlow()
        {
            Preflow(graph.Source);

            // loop untill none of the Vertex is in overflow
            while (OverFlow() != -1)
            {
                int v = OverFlow();
                if (!Push(v))
                    Relabel(v);
            }

            // ver.back() returns last Vertex, whose
            // e_flow will be final maximum flow
            MaxFlow = excess[graph.Target];
        }

        private bool Push(int v)
        {
            // Traverse through all edges to find an adjacent (of u)
            // to which flow can be pushed
            foreach (var edge in graph.Nodes[v].AllEdges)
            {
                var other = edge.From == v ? edge.To : edge.From;
                // Checks u of current edge is same as given
                // overflowing vertex

                // if flow is equal to capacity then no push
                // is possible
                //if (edge.Flow == edge.Capacity)//backw flow
                if (!(edge.ResidualCapacityTo(other) > 0))
                    continue;

                // Push is only possible if height of adjacent
                // is smaller than height of overflowing vertex
                if (height[v] > height[other])
                {
                    // Flow to be pushed is equal to minimum of
                    // remaining flow on edge and excess flow.
                    double flow = Math.Min(edge.ResidualCapacityTo(other), excess[v]);

                    // Reduce excess flow for overflowing vertex
                    excess[v] -= flow;

                    // Increase excess flow for adjacent
                    excess[other] += flow;

                    // Add residual flow (With capacity 0 and negative
                    // flow)
                    edge.AddFlow(flow, other);
                    //edge[i].flow += flow;

                    //updateReverseEdgeFlow(i, flow);

                    return true;
                }
            }
            return false;
        }



        private void Relabel(int v)
        {
            // Initialize minimum height of an adjacent
            int min_height = Int32.MaxValue;
            // Find the adjacent with minimum height
            foreach (var edge in graph.Nodes[v].AllEdges)
            {
                var other = edge.From == v ? edge.To : edge.From;
                // if flow is equal to capacity then no
                // relabeling
                //if (edge.Flow == edge.Capacity)
                if (!(edge.ResidualCapacityTo(other) > 0))
                    continue;

                // Update minimum height
                if (height[other] < min_height)
                {
                    min_height = height[other];
                    // updating height of u
                    height[v] = min_height + 1;
                }
            }

        }
    }
}
