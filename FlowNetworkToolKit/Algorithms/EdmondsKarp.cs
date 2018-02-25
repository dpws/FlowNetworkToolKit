using System;
using System.Collections.Generic;
using FlowNetworkToolKit.Core.Base.Algorithm;
using FlowNetworkToolKit.Core.Base.Network;

namespace FlowNetworkToolKit.Algorithms
{
    class EdmondsKarp : BaseMaxFlowAlgorithm
    {
        private bool[] marked;
        private FlowEdge[] edgeTo;

        public EdmondsKarp()
        {
            Name = "*EdmondsKarp";
            Url = "https://ru.wikipedia.org/wiki/%D0%90%D0%BB%D0%B3%D0%BE%D1%80%D0%B8%D1%82%D0%BC_%D0%AD%D0%B4%D0%BC%D0%BE%D0%BD%D0%B4%D1%81%D0%B0_%E2%80%94_%D0%9A%D0%B0%D1%80%D0%BF%D0%B0";
            Description =
                @"Алгоритм Эдмондса—Карпа — это вариант алгоритма Форда—Фалкерсона, при котором на каждом шаге выбирают кратчайший дополняющий путь из источника в сток в остаточной сети (полагая, что каждое ребро имеет единичную длину). Кратчайший путь находится поиском в ширину.";
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
            //пока существует увеличивающий путь из источника в сток
            while (HasAugmentingPath(graph.Source, graph.Target))
            {

                Tick();

                var bottle = Double.PositiveInfinity; // пропускная способность пути

                // вычисление пропускной способности пути
                for (var u = graph.Target; u != graph.Source; u = edgeTo[u].Other(u))
                {
                    bottle = Math.Min(bottle, edgeTo[u].ResidualCapacityTo(u));
                }

                // увеличение потока на кратчайшем пути из источника в сток
                for (var u = graph.Target; u != graph.Source; u = edgeTo[u].Other(u))
                {
                    edgeTo[u].AddFlow(bottle, u);
                    //Console.WriteLine("pushflow " + edgeTo[u].ToString()+" flow "+ bottle);
                }

                MaxFlow += bottle;
            }

        }

        private bool HasAugmentingPath(int s, int t)
        {
            edgeTo = new FlowEdge[graph.NodeCount];
            marked = new bool[graph.NodeCount];
            //создаем очередь обрабатываемых узлов
            var queue = new Queue<int>();
            //начинаем работу с узла-источника
            queue.Enqueue(s);
            marked[s] = true;
            while (queue.Count > 0 && !marked[t])
            {
                //достаем из очереди следующий узел для обработки
                var u = queue.Dequeue();
                //просматриаем все инцидентные узлу ребра
                foreach (var e in graph.Nodes[u].AllEdges)
                {
                    var v = e.Other(u);
                    //проверяем, что ребро является допустимым и узел w не был посещен ранее
                    if (e.ResidualCapacityTo(v) > 0 && !marked[v])
                    {
                       // Console.WriteLine("touch " + e.ToString());
                        //Console.WriteLine("mark " + v);
                        //сохраняем информацию, о том через какое ребро был посещен узел v
                        edgeTo[v] = e;
                        //помечаем узел, как почещенный
                        marked[v] = true;
                        //добавляем узел в очередь обрабатываемых узлов
                        queue.Enqueue(v);
                       
                    }
                }
            }
            //если узел-сток был посещен - увеличивающий был найден
            return marked[t];
        }

    }
}
