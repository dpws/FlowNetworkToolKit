using FlowNetworkToolKit.Core.Base.Algorithm;
using FlowNetworkToolKit.Core.Base.Network;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FlowNetworkToolKit.Algorithms
{
    class HLPR : BaseMaxFlowAlgorithm
    {
        //The highest-label push–relabel algorithm[11] organizes all nodes into buckets indexed by their labels. 
        //The algorithm always selects an active node with the largest label to discharge.
        int n;
        List<double> excess = new List<double>();
        List<int> height = new List<int>();
        List<int> count = new List<int>();
        List<bool> active = new List<bool>();
        List<List<int>> B = new List<List<int>>(); //buckets
        int b;

        public HLPR()
        {
            Name = "HLPR*";
            Url = "";
            Description =
                @"";
        }

        protected override void Init()
        {

            excess = new List<double>();
            height = new List<int>();
            count = new List<int>();
            active = new List<bool>();
            B = new List<List<int>>();

            for (int i = 0; i < graph.NodeCount; i++)
            {
                height.Add(0);
                excess.Add(0);
                count.Add(0);
                active.Add(false);
                B.Add(new List<int>());
            }
            count.Add(0);
            b = 0;
            n = graph.NodeCount;
        }

        protected override void Logic()
        {
            SearchMaxFlow();
        }

        public void SearchMaxFlow()
        {
            foreach (var e in graph.Nodes[graph.Source].AllEdges) 
            {
                excess[graph.Source] += e.Capacity;
            }

            count[0] = n;
            Enq(graph.Source);
            active[graph.Target] = true;

            while (b >= 0)
            {
                if (B[b].Count > 0) 
                {
                    int v = B[b].Last();
                    B[b].Remove(B[b].Last());
                    active[v] = false;
                    Discharge(v);
                }
                else
                {
                    b--;
                }
            }
            MaxFlow = excess[graph.Target];
        }

        void Enq(int v)//enqueue
        {
            if (!active[v] && excess[v] > 0 && height[v] < n)
            {
                active[v] = true;
                B[height[v]].Add(v);
                b = Math.Max(b, height[v]);
            }
        }

        void Push(FlowEdge e, int from)
        {
            var f = Math.Min(excess[from], e.ResidualCapacityTo(e.Other(from))); 
            if (height[from] == height[e.Other(from)] + 1 && f > 0)
            {
                e.AddFlow(f, e.Other(from));
                excess[e.Other(from)] += f;
                excess[from] -= f;
                Enq(e.Other(from));
            }
        }

        //The gap heuristic detects gaps in the labeling function 
        //If there is a label 0 < 𝓁' < | V | for which there is no node u such that 𝓁(u) = 𝓁', 
        //then any node u with 𝓁' < 𝓁(u) < | V | has been disconnected from t and can be relabeled to (| V | + 1) immediately.
        void Gap(int k)
        {
            for (int v = 0; v < n; v++)
                if (height[v] >= k)
                {
                    count[height[v]]--;
                    height[v] = Math.Max(height[v], n);
                    count[height[v]]++;
                    Enq(v);
                }
        }

        void Relabel(int v)
        {
            count[height[v]]--;
            height[v] = n;
            foreach (var e in graph.Nodes[v].AllEdges)
            {
                if (e.ResidualCapacityTo(e.Other(v)) > 0)
                {
                    height[v] = Math.Min(height[v], height[e.Other(v)] + 1);
                }
            }
            count[height[v]]++;
            Enq(v);
        }

        void Discharge(int v)
        {
            foreach (var e in graph.Nodes[v].AllEdges) 
            {
                if (excess[v] > 0)
                {
                    Push(e, v);
                }
                else
                {
                    break;
                }
            }

            if (excess[v] > 0)
            {
                if (count[height[v]] == 1)
                {
                    Gap(height[v]);
                }
                else
                {
                    Relabel(v);
                }
            }
        }

    }
}
