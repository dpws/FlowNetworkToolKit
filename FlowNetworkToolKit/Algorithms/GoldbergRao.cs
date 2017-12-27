using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlowNetworkToolKit.Core.Base.Algorithm;
using FlowNetworkToolKit.Core.Base.Network;

namespace FlowNetworkToolKit.Algorithms
{
    class GoldbergRao : BaseMaxFlowAlgorithm
    {
        private int[] dist;
        private double delta;
        private Dictionary<FlowEdge, int> length;

        public GoldbergRao()
        {
            Name = "GoldbergRao";
            Url = "";
            Description =
                @"";
        }

        protected override void Init()
        {
            dist = new int[graph.NodeCount];
            length = new Dictionary<FlowEdge, int>();
            foreach (var edge in graph.Edges)
                length.Add(edge, 1);
        }

        protected override void Logic()
        {
            SearchMaxFlow();
        }

        public void SearchMaxFlow()
        {
            double flow = 0;
            double F = 0;
            foreach (var edge in graph.Nodes[graph.Source].AllEdges)
                F += edge.ResidualCapacityTo(edge.To);
            var lambda = Math.Min(Math.Pow(graph.NodeCount, 0.66), Math.Pow(graph.EdgeCount, 0.5));
            delta = Math.Ceiling(F / lambda);
            ComputeLengths();
            while (ComputeDistances(graph.Target, graph.Source))
            {
                var path = new List<FlowEdge>();
                int[] ptr = new int[graph.Nodes.Count];
                var scc = new List<List<int>>();
                var contracted_graph = ContractZeroLengthSCC(out scc);
                double df = Math.Min(DFS(ref contracted_graph, ref path, ptr, graph.Target, graph.Source, Double.MaxValue), delta);
                ExtendFlow(ref contracted_graph, scc, df, path);
                var volfl = GetVolFL();
                System.Console.WriteLine("volfl " + volfl);
                if (volfl / dist[graph.Source] < F / 2)
                {
                    F = volfl / dist[graph.Source];
                    delta = Math.Ceiling(F / lambda);
                }
                flow += df;
                ComputeLengths();
            }
            MaxFlow = flow;
        }

        private double DFS(ref FlowNetwork graph, ref List<FlowEdge> path, int[] ptr, int dest, int u, double f)
        {
            if (u == dest)
                return f;
            for (; ptr[u] < graph.Nodes[u].OutcomingEdges.Count(); ++ptr[u])
            {
                FlowEdge e = graph.Nodes[u].OutcomingEdges[ptr[u]];
                if (e.Flow < e.Capacity)
                {
                    System.Console.WriteLine(e.From + " " + e.To);
                    double df = DFS(ref graph, ref path, ptr, dest, e.To, Math.Min(f, e.Capacity - e.Flow));
                    if (df > 0)
                    {
                        System.Console.WriteLine(e.From + " " + e.To + " --> " + df);
                        path.Add(e);
                        return df;
                    }
                }
            }
            return 0;
        }

        private void ComputeLengths()
        {
            foreach (var edge in graph.Edges)
            {
                if (edge.ResidualCapacityTo(edge.To) >= delta * 3)
                    length[edge] = 0;
                else
                    length[edge] = 1;
            }
        }

        private bool ComputeDistances(int t, int s) //BFS - finding nodes distances from target
        {
            for (int i = 0; i < dist.Length; i++)
            {
                dist[i] = -1;
            }
            dist[t] = 0; //tagret layer (lvl) is 0

            var queue = new Queue<int>();
            queue.Enqueue(t);
            while (queue.Count > 0)
            {
                var v = queue.Dequeue();

                foreach (var e in graph.Nodes[v].AllEdges)
                {
                    var w = e.Other(v);
                    if (e.ResidualCapacityTo(v) > 0 && dist[w] < 0)
                    {
                        dist[w] = dist[v] + BinaryLength(e);
                        queue.Enqueue(w);
                    }
                }
            }
            return dist[s] > 0;
        }

        private int BinaryLength(FlowEdge e)
        {
            if (IsSpecialEdge(e) || e.ResidualCapacityTo(e.To) >= (3 * delta))
                return 0;
            else
                return 1;
        }

        private bool IsSpecialEdge(FlowEdge e)
        {
            if (2 * delta <= e.ResidualCapacityTo(e.To) && e.ResidualCapacityTo(e.To) < 3 * delta && dist[e.To] == dist[e.From] && e.ResidualCapacityTo(e.From) >= 3 * delta)
                return true;
            else
                return false;
        }

        private double GetVolFL()
        {
            double volFL = 0;
            foreach (var edge in graph.Edges)
                volFL += edge.ResidualCapacityTo(edge.To) * length[edge];
            return volFL;
        }

        private FlowNetwork ContractZeroLengthSCC(out List<List<int>> scc)
        {
            int[] ids = new int[graph.NodeCount];
            int[] lowLinks = new int[graph.NodeCount];

            var admissible_edges = new List<FlowEdge>();
            foreach (var edge in graph.Edges)
                if (dist[edge.From] == dist[edge.To] + BinaryLength(edge))//length[edge]) //admissible edge
                    admissible_edges.Add(edge);

            scc = new List<List<int>>();
            var stack = new Stack<int>();
            var sorted_nodes = TopologicalSort(admissible_edges);

            // поиск сильно связанных компонентов
            int id = 0;
            foreach (var node in sorted_nodes)
            {
                if (ids[node] == 0)
                    SCC(node, id, ref stack, ref scc, ref ids, ref lowLinks);
            }

            // устанавливаем пропускные способности ребер связанных компонентов
            var contracted_admissible_edges = new List<FlowEdge>();

            foreach (var edge in admissible_edges)
            {
                string res = "00";
                foreach (var contracted in scc.Where((list) => list.Count() > 1))
                {
                    res = $"{(contracted.Contains(edge.From) ? 1 : 0)}{(contracted.Contains(edge.To) ? 1 : 0)}";
                    switch (res)
                    {
                        //case "00": contracted_admissible_edges.Add(edge); break;
                        case "01":
                            var e = new FlowEdge(edge.From, contracted.First(), edge.ResidualCapacityTo(edge.To));
                            var idx = contracted_admissible_edges.IndexOf(e);
                            if (idx == -1)
                                contracted_admissible_edges.Add(e);
                            else
                                contracted_admissible_edges[idx].Capacity += edge.ResidualCapacityTo(edge.To);
                            break;
                        case "10":
                            e = new FlowEdge(contracted.First(), edge.To, edge.ResidualCapacityTo(edge.To));
                            idx = contracted_admissible_edges.IndexOf(e);
                            if (idx == -1)
                                contracted_admissible_edges.Add(e);
                            else
                                contracted_admissible_edges[idx].Capacity += edge.ResidualCapacityTo(edge.To);
                            break;
                    }
                }
                if (String.Equals(res, "00"))
                    contracted_admissible_edges.Add(new FlowEdge(edge));
            }

            // создаем граф
            var contracted_graph = new FlowNetwork(contracted_admissible_edges);
            contracted_graph.Source = graph.Source;
            contracted_graph.Target = graph.Target;
            return contracted_graph;
        }

        private void SCC(int node, int id, ref Stack<int> stack, ref List<List<int>> scc, ref int[] ids, ref int[] lowLinks)
        {
            ids[node] = id;
            lowLinks[node] = id;
            id++;
            stack.Push(node);//todo
            foreach (var edge in graph.Nodes[node].OutcomingEdges)
            {
                if (edge.ResidualCapacityTo(edge.To) > 0)
                {
                    int neighbour_node = edge.To;
                    if (stack.Contains(neighbour_node) && length[edge] == 0)
                    {
                        lowLinks[node] = Math.Min(lowLinks[node], ids[neighbour_node]);
                    }
                    if (ids[neighbour_node] == 0)
                    {
                        SCC(neighbour_node, id, ref stack, ref scc, ref ids, ref lowLinks);
                        lowLinks[node] = Math.Min(lowLinks[node], lowLinks[neighbour_node]);
                    }
                }
            }
            if (lowLinks[node] == ids[node])
            {
                var subList = new List<int>();

                int n = stack.Pop();
                subList.Add(n);
                while (n != node)
                {
                    n = stack.Pop();
                    subList.Add(n);
                }
                scc.Add(subList);
            }
        }

        private List<int> TopologicalSort(List<FlowEdge> admissible_edges)
        {
            var marked_nodes = new Dictionary<int, bool>();
            for (int i = 0; i < graph.NodeCount; i++)
                marked_nodes.Add(i, false);
            var sortedList = new List<int>();
            for (int i = 0; i < graph.NodeCount; i++)
                if (marked_nodes[i] == false)
                    DFSTopSort(ref sortedList, ref marked_nodes, i);
            sortedList.Reverse();
            return sortedList;
        }

        private void DFSTopSort(ref List<int> sortedList, ref Dictionary<int, bool> marked_nodes, int node)
        {
            marked_nodes[node] = true;
            foreach (var edge in graph.Nodes[node].OutcomingEdges)
                if (marked_nodes[edge.To] == false)
                    DFSTopSort(ref sortedList, ref marked_nodes, edge.To);
            sortedList.Add(node);
        }

        private void ExtendFlow(ref FlowNetwork A, List<List<int>> scc, double flow, List<FlowEdge> path)
        {
            var supernodes = new List<int>();
            foreach (var contracted in scc.Where((list) => list.Count() > 1))
                supernodes.Add(contracted.First());

            var excess = new Dictionary<int, double>(); //flow balance
            foreach (var node in graph.Nodes.Keys)
            {
                excess.Add(node, 0);
            }
            excess[graph.Source] = flow; //should be 0 at the end of routing flow
            excess[graph.Target] = -flow; //should be 0 at the end of routing flow

            foreach (var edge in path)
            {
                if (!supernodes.Contains(edge.From) && !supernodes.Contains(edge.To)) 
                {
                    graph.PushFlow(edge.From, edge.To, flow);
                    excess[edge.From] -= flow;
                    excess[edge.To] += flow;

                }
            }
            //reroute flow in strongly connected components using excess lables
            //working with original graph
            foreach (var contracted in scc.Where((list) => list.Count() > 1))
            {
                foreach (var node in contracted) //routing edges in and out of supernodes
                {
                    foreach (var edge in graph.Nodes[node].IncomingEdges)
                    {

                        if (excess[edge.From] > 0 && !contracted.Contains(edge.From))
                        {
                            var f = Math.Min(edge.ResidualCapacityTo(node), excess[edge.From]);
                            edge.AddFlow(f, node);
                            excess[edge.From] -= f;
                            excess[node] += f;
                        }
                    }
                    foreach (var edge in graph.Nodes[node].OutcomingEdges)
                    {
                        if (excess[edge.To] < 0 && !contracted.Contains(edge.To))
                        {
                            var f = Math.Min(edge.ResidualCapacityTo(edge.To), -excess[edge.To]);
                            edge.AddFlow(f, edge.To);
                            excess[node] -= f;
                            excess[edge.To] += f;
                        }
                    }
                }
                while (contracted.Count((id) => excess[id] > 0) > 0)//multiple source
                {
                    int srs = contracted.First((id) => excess[id] > 0);
                    while (excess[srs] > 0 && contracted.Count((id) => excess[id] < 0) > 0) //multiple target       
                    {
                        int trg = contracted.First((id) => excess[id] < 0);
                        SimpleBlockingFlowInConnectedComponent(contracted, ref excess, srs, trg);
                    }
                }

            }
        }

        private void SimpleBlockingFlowInConnectedComponent(List<int> contracted, ref Dictionary<int, double> excess, int source, int target)
        {
            var visited = new Dictionary<int, bool>();
            var prev_node = new Dictionary<int, int>();
            var mincap = new Dictionary<int, double>();
            for (int i = 0; i < contracted.Count(); i++)
            {
                visited.Add(contracted[i], false);
                prev_node.Add(contracted[i], -1);
                mincap.Add(contracted[i], Int32.MaxValue);
            }
            mincap[source] = excess[source];
            var stack = new Stack<int>();
            stack.Push(source);

            while (stack.Count > 0)
            {
                var v = stack.Pop();
                visited[v] = true;
                if (v == target)
                    break;
                foreach (var edge in graph.Nodes[v].AllEdges)
                {
                    var other = edge.From == v ? edge.To : edge.From;
                    if (contracted.Contains(other))
                    {
                        if (!visited[other] && edge.ResidualCapacityTo(other) > 0)
                        {
                            stack.Push(other);
                            prev_node[other] = v;
                            mincap[other] = Math.Min(mincap[v], edge.ResidualCapacityTo(other));
                        }
                    }
                }

            }
            if (mincap[target] != int.MaxValue)
            {
                var v = target;
                while (prev_node[v] != -1)
                {
                    graph.PushFlow(prev_node[v], v, mincap[target]);
                    v = prev_node[v];
                }
            }
            excess[source] -= mincap[target];
            excess[target] += mincap[target];

        }

    }

}
