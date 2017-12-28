using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlowNetworkToolKit.Core.Base.Algorithm;
using FlowNetworkToolKit.Core.Base.Network;

namespace FlowNetworkToolKit.Algorithms
{
    class EdmondsKarp : BaseMaxFlowAlgorithm
    {

        public EdmondsKarp()
        {
            Name = "EdmondsKarp*";
            Url = "https://ru.wikipedia.org/wiki/%D0%90%D0%BB%D0%B3%D0%BE%D1%80%D0%B8%D1%82%D0%BC_%D0%AD%D0%B4%D0%BC%D0%BE%D0%BD%D0%B4%D1%81%D0%B0_%E2%80%94_%D0%9A%D0%B0%D1%80%D0%BF%D0%B0";
            Description =
                @"Алгоритм Эдмондса—Карпа — это вариант алгоритма Форда—Фалкерсона, при котором на каждом шаге выбирают кратчайший дополняющий путь из источника в сток в остаточной сети (полагая, что каждое ребро имеет единичную длину). Кратчайший путь находится поиском в ширину.";
        }

        private bool[] _marked;

        private FlowEdge[] _edgeTo;


        protected override void Init()
        {
            //if (!IsReacheable(graph.Source, graph.Target)) throw new ArgumentException("Initial flow is infeasible");
        }

        protected override void Logic()
        {
            SearchMaxFlow();
        }

        public void SearchMaxFlow()
        {
            MaxFlow = Excess(graph.Target);
            while (HasAugmentingPath(graph.Source, graph.Target))
            {

                Tick();
 
                var bottle = Double.PositiveInfinity;

                // compute bottleneck capacity;
                for (var v = graph.Target; v != graph.Source; v = _edgeTo[v].Other(v))
                {
                    bottle = Math.Min(bottle, _edgeTo[v].ResidualCapacityTo(v));
                }

                // augment flow
                for (var v = graph.Target; v != graph.Source; v = _edgeTo[v].Other(v))
                {
                    _edgeTo[v].AddFlow(bottle, v);
                }

                MaxFlow += bottle;
            }

        }

        private double Excess(int v)
        {
            var excess = 0.0;
            foreach (var e in graph.Nodes[v].AllEdges)
            {
                if (v == e.From) excess -= e.Flow;
                else excess += e.Flow;
            }
            return excess;
        }

        private bool IsReacheable(int s, int t)
        {

            if (Math.Abs(MaxFlow + Excess(s)) > Double.Epsilon)
            {
                // System.err.println("Excess at source = " + Excess(G, s));
                // System.err.println("Mas flow = " + Value);
                return false;
            }

            if (Math.Abs(MaxFlow - Excess(t)) > Double.Epsilon)
            {
                //System.err.println("Excess at sink   = " + excess(G, t));
                //System.err.println("Max flow         = " + value);
                return false;
            }

            for (int v = 0; v < graph.NodeCount; v++)
            {
                if (v == s || v == t) continue;

                if (Math.Abs(Excess(v)) > Double.Epsilon)
                {
                    //System.err.println("Net flow out of " + v + " doesn't equal zero");
                    return false;
                }
            }

            return true;
        }

        public bool InCut(int v)
        {
            return _marked[v];
        }

        private bool HasAugmentingPath(int s, int t)
        {
            _edgeTo = new FlowEdge[graph.NodeCount];
            _marked = new bool[graph.NodeCount];

            var queue = new Queue<int>();
            queue.Enqueue(s);
            _marked[s] = true;
            while (queue.Count > 0 && !_marked[t])
            {
                var v = queue.Dequeue();

                foreach (var e in graph.Nodes[v].AllEdges)
                {
                    var w = e.Other(v);

                    if (e.ResidualCapacityTo(w) > 0)
                    {
                        if (!_marked[w])
                        {
                            _edgeTo[w] = e;
                            _marked[w] = true;
                            queue.Enqueue(w);
                        }
                    }
                }
            }

            return _marked[t];
        }


    }
}
