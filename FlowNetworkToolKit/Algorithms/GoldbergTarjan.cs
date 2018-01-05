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
    class GoldbergTarjan : BaseMaxFlowAlgorithm
    {
        private Dictionary<int, bool> blocked;
        private Dictionary<int, double> excess;

        public GoldbergTarjan()
        {
            Name = "GoldbergTarjan (acyclic only)";
            Url = "";
            Description =
                @"";
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
            double f = 0;
            int srs = graph.Source;
            blocked = new Dictionary<int, bool>();
            excess = new Dictionary<int, double>();
            foreach (var node in graph.Nodes.Keys)
            {
                blocked.Add(node, false);
                excess.Add(node, 0);
            }

            List<int> L = graph.Nodes.Keys.ToList();
            foreach (var edge in graph.Nodes[srs].OutcomingEdges)
            {
                edge.AddFlow( edge.ResidualCapacityTo(edge.To), edge.To);
                excess[edge.To] += edge.Flow;
            }

            var fifo = new Queue<int>();
            foreach (var nodeId in graph.Nodes.Keys)
            {
                if (nodeId != graph.Target)
                    fifo.Enqueue(nodeId);
            }

            fifo.Dequeue();
            L.Remove(graph.Source);
            L.Remove(graph.Target);


            //discharge (until there are no active vertexes)

            while (L.Sum((id) => excess[id]) > 0)
            {
                int v = L.First((id) => excess[id] > 0); // первый активный узел в списке
                while (excess[v] > 0) // is active
                {
                    bool block_v = true;
                    bool pushed = false;

                    foreach (var edge in graph.Nodes[v].OutcomingEdges) //исходящие арки 
                    {
                        var other = edge.From == v ? edge.To : edge.From;

                        if (excess[v] > 0 && !blocked[other] && !blocked[v] && edge.ResidualCapacityTo(other) > 0)
                        {
                            f = Math.Min(excess[v], edge.ResidualCapacityTo(other));
                            edge.AddFlow(f, other); // push + pull (backward push)                                                                                                                      
                            excess[v] -= f;
                            excess[other] += f;
                            pushed = true;
                        }
                        if (!(blocked[other] || edge.ResidualCapacityTo(edge.To) == 0))
                            block_v = false;
                    }
                    foreach (var edge in graph.Nodes[v].IncomingEdges.Where((edge) => edge.From != srs)) //входящие арки,кроме источника
                    {
                        var other = edge.From == v ? edge.To : edge.From;

                        if (excess[v] > 0 && edge.ResidualCapacityTo(other) > 0)
                        {
                            f = Math.Min(excess[v], edge.ResidualCapacityTo(other));
                            edge.AddFlow(f, other); // push + pull (backward push)
                            excess[v] -= f;
                            excess[other] += f;
                        }
                    }

                    foreach (var edge in graph.Nodes[v].IncomingEdges.Where((edge) => edge.From == srs)) //входящие арки из источника
                    {
                        var other = edge.From == v ? edge.To : edge.From;

                        if (excess[v] > 0 && edge.ResidualCapacityTo(other) > 0) //&& !blocked[other] && !blocked[v]
                        {
                            f = Math.Min(excess[v], edge.ResidualCapacityTo(other));
                            edge.AddFlow(f, other); // push + pull (backward push)
                            excess[v] -= f;
                            excess[other] += f;
                        }
                    }

                    if (block_v && !blocked[v]) //&& pushed
                    {
                        blocked[v] = true; //block
                        L.MoveItemAtIndexToFront(L.FindIndex((vertex) => vertex == v)); //перемещаем узел в начало списка 
                    }
                }
            }
            MaxFlow =  excess[graph.Target];
        }
    }
}
