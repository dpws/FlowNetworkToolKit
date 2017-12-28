using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlowNetworkToolKit.Core.Base.Algorithm;
using FlowNetworkToolKit.Core.Base.Network;

namespace FlowNetworkToolKit.Algorithms
{
    class Dinic : BaseMaxFlowAlgorithm
    {
        private int[] dist;
        public Dinic()
        {
            Name = "Dinic";
            Url = "";
            Description =
                @"";
        }

        public override string GetStats()
        {
            return "Some statisctics";
        }

        protected override void Init()
        {
           
        }

        protected override void Logic()
        {
            SearchMaxFlow();
        }

        public void SearchMaxFlow()
        {
            double flow = 0;
            dist = new int[graph.NodeCount];
            while (BFS(graph.Source, graph.Target))
            {
                int[] ptr = new int[graph.NodeCount];
                while (true)
                {
                    double df = DFS(ptr, graph.Target, graph.Source, Double.MaxValue);
                    if (df == 0)
                        break;
                    flow += df;
                }
            }
            MaxFlow = flow;
        }

        private bool BFS(int s, int t) //BFS - finding nodes distances from source
        {
            for (int i = 0; i < dist.Length; i++)
            {
                dist[i] = -1;
            }
            dist[s] = 0; //source layer (lvl) is 0

            var queue = new Queue<int>();
            queue.Enqueue(s);
            while (queue.Count > 0)
            {
                var v = queue.Dequeue();

                foreach (var e in graph.Nodes[v].AllEdges)
                {
                    var w = e.Other(v);
                    if (e.ResidualCapacityTo(w) > 0 && dist[w] < 0)
                    {
                        dist[w] = dist[v] + 1;
                        queue.Enqueue(w);
                    }
                }
            }
            return dist[t] > 0;
        }

        private double DFS(int[] ptr, int dest, int u, double f)
        {
            if (u == dest)
                return f;
            for (; ptr[u] < graph.Nodes[u].AllEdges.Count(); ++ptr[u])
            {
                FlowEdge e = graph.Nodes[u].AllEdges[ptr[u]];
                if (dist[e.Other(u)] == dist[u] + 1 && e.ResidualCapacityTo(e.Other(u)) > 0)
                {
                    double df = DFS(ptr, dest, e.Other(u), Math.Min(f, e.ResidualCapacityTo(e.Other(u))));
                    if (df > 0)
                    {
                        e.AddFlow(df, e.Other(u));
                        return df;
                    }
                }
            }
            return 0;
        }
    }
}
