using System;
using System.Collections.Generic;
using FlowNetworkToolKit.Core.Base.Algorithm;

namespace FlowNetworkToolKit.Algorithms
{
    class Dinic : BaseMaxFlowAlgorithm
    {
        private int[] dist;

        public Dinic()
        {
            Name = "Dinic";
            Url = "...";
            Description = @"...";
        }

        protected override void Init()
        {
            dist = new int[graph.NodeCount];
        }

        protected override void Logic()
        {
            SearchMaxFlow();
        }

        public void SearchMaxFlow()
        {
            double flow = 0;
            //пока допустимо построение новой слоистой сети
            while (BFS(graph.Source, graph.Target))
            {
                while (true)
                {
                    //поиск увеличивающего пути в слоистой сети, увеличение потока вдоль сети
                    double aug_flow = DFS(graph.Target, graph.Source, Double.MaxValue);
                    //если увеличивающий путь не найден, выходим из цикла и перестраиваем слоистую сеть
                    if (aug_flow == 0)
                        break;
                    //увеличиваем значение максимального потока
                    flow += aug_flow;
                }
            }
            MaxFlow = flow;
        }

        private bool BFS(int s, int t) 
        {
            for (int i = 0; i < dist.Length; i++)
            {
                dist[i] = -1;
            }
            //источник находится в слое 0
            dist[s] = 0;
            //создаем очередь обрабатываемых узлов
            var queue = new Queue<int>();
            //начинаем работу с узла-источника
            queue.Enqueue(s);
            while (queue.Count > 0)
            {                
                //достаем из очереди следующий узел для обработки
                var u = queue.Dequeue();
                //просматриаем все инцидентные узлу ребра
                foreach (var e in graph.Nodes[u].AllEdges)
                {
                    var v = e.Other(u);
                    //проверяем, что ребро является допустимым и узел v не приписан ни одному слою
                    if (e.ResidualCapacityTo(v) > 0 && dist[v] < 0)
                    {
                        e.Mark();
                        //проставляем узлу v метку расстояния
                        //Console.WriteLine("touch " + e.ToString());
                        
                        dist[v] = dist[u] + 1;
                        //Console.WriteLine("dist " + v + " = " + dist[v]);
                        //добавляем узел в очередь обрабатываемых узлов
                        queue.Enqueue(v);
                        e.Unmark();
                    }
                }
            }
            //если узел-сток имеет метку расстояния - увеличивающий путь был найден
            return dist[t] > 0;
        }

        private double DFS(int dest, int u, double f)
        {
            //если достигнут сток возвращаем значение найденного блокирующего потока
            if (u == dest)
                return f;
            //просматриаем все инцидентные узлу ребра
            foreach (var e in graph.Nodes[u].AllEdges)
            {
                //если ребро является допустимым (входит в остаточную сеть)
                if (dist[e.Other(u)] == dist[u] + 1 && e.ResidualCapacityTo(e.Other(u)) > 0)
                {
                    //вызов события отрисовки OnEdgeMarked
                    e.Mark();
                    //Console.WriteLine("touch " + e.ToString());

                    //рекурсивно ищем путь до стока, параллельно рассчитывая пропускную способность пути delta
                    double delta = DFS(dest, e.Other(u), Math.Min(f, e.ResidualCapacityTo(e.Other(u))));
                    //вызов события отрисовки OnEdgeUnmarked
                    e.Unmark();
                    //если найден путь, по которому можно пропустить положительный поток
                    if (delta > 0)
                    {
                        //увеличиваем поток в ребре 
                        e.AddFlow(delta, e.Other(u));
                       // Console.WriteLine("pushflow " + e.ToString()+ " "+ delta);
                        //рекурсивно возвращаем пропускную способность пути
                        return delta;
                    }
                }
            }
            //если увеличивающий путь не был найден возвращаем 0
            return 0;
        }
    }
}


