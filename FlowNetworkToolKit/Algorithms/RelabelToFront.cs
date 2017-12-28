using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlowNetworkToolKit.Core.Base.Algorithm;
using FlowNetworkToolKit.Core.Base.Network;
using FlowNetworkToolKit.Core.Utils.ListExtensions;

namespace FlowNetworkToolKit.Algorithms
{
    class RelabelToFront : BaseMaxFlowAlgorithm
    {

        //The algorithm scans the list from front to back and performs a discharge operation on the current node if it is active. 
        //If the node is relabeled, it is moved to the front of the list, and the scan is restarted from the front.
        private int[] height;
        private double[] excess;
        List<int> nodes;

        public RelabelToFront()
        {
            Name = "RelabelToFront";
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
            nodes = new List<int>(graph.Nodes.Keys);
            nodes.Remove(graph.Source);
            nodes.Remove(graph.Target);
    }

        protected override void Logic()
        {
            SearchMaxFlow();
        }

        private void Preflow(int s)
        {
            height[graph.Source] = graph.NodeCount;

            foreach (var edge in graph.Nodes[graph.Source].OutcomingEdges)
            {
                edge.AddFlow(edge.ResidualCapacityTo(edge.To), edge.To);
                excess[edge.To] += edge.Flow;
            }
        }

        private int OverFlow()
        {
            for (int i = 1; i < excess.Count() - 1; i++) //except target node
                if (excess[i] > 0)
                 return i; //index of active vertex
            return -1;
        }

        public void SearchMaxFlow()
        {
            Preflow(graph.Source);
            var ind = 0;
            while(ind < nodes.Count)
            {
                int v = nodes[ind];
                var prev_height = height[v];
                Discharge(v);
                if (height[v] > prev_height)
                {
                    nodes.MoveItemAtIndexToFront(ind);
                    ind = 0;
                }
                else
                    ind++;
            }
            MaxFlow = excess[graph.Target];
        }

        private void Push(FlowEdge edge, int v, int other)
        {
            double flow = Math.Min(edge.ResidualCapacityTo(other), excess[v]);
            excess[v] -= flow;
            excess[other] += flow;
            edge.AddFlow(flow, other);
        }

        private void Relabel(int v)
        {
            int min_height = Int32.MaxValue;
            foreach (var edge in graph.Nodes[v].AllEdges) // Find the adjacent with minimum height
            {
                var other = edge.Other(v);
                if (edge.ResidualCapacityTo(other) > 0 && height[other] < min_height)
                {
                    min_height = height[other]; // updating heights
                    height[v] = min_height + 1;
                }
            }
        }

        private void Discharge(int v)
        {
            var adj = graph.Nodes[v].AllEdges.Count; //all adjacent edges
            var ptr = 0; //visited adjacent edges
            while (excess[v] > 0)
            {
                if (ptr >= adj) 
                {
                    Relabel(v);
                    ptr = 0;
                    continue;
                }
                var edge = graph.Nodes[v].AllEdges[ptr];
                var other = edge.Other(v);
                if (edge.ResidualCapacityTo(other) > 0 && height[v] > height[other])
                    Push(edge, v, other);
                else
                    ptr += 1;

            }
        }
    }
}
