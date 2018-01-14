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
        double[] excess;
        int[] height;
        int[] count;
        bool[] active; 
        List<List<int>> Bucket = new List<List<int>>(); //buckets
        int b;

        public HLPR()
        {
            Name = "HLPR*";
            Url = "";
            Description = @"";
        }

        protected override void Init()
        {
            excess = new double[graph.NodeCount];
            height = new int[graph.NodeCount];
            count = new int[graph.NodeCount+1];
            active = new bool[graph.NodeCount];
            Bucket = new List<List<int>>();

            for (int i = 0; i < graph.NodeCount; i++)
            {
                height[i] = 0;
                excess[i] = 0;
                count[i] = 0;
                active[i] = false;
                Bucket.Add(new List<int>());
            }
            count[graph.NodeCount]=0;
            b = 0;
        }

        protected override void Logic()
        {
            SearchMaxFlow();
        }

        public void SearchMaxFlow()
        {
            //увеличиваем избыток в источнике
            foreach (var e in graph.Nodes[graph.Source].AllEdges) 
            {
                excess[graph.Source] += e.Capacity;
            }
            //все узлы находятся на высоте 0
            count[0] = graph.NodeCount;
            //добавляем источник в Bucket
            Enq(graph.Source);
            //помечаем узел стока активным
            active[graph.Target] = true;
            //пока текущая рабочая высота неотрицательна (Bucket содержит активные узлы)
            while (b >= 0)
            {
                //если в Bucket есть активные узлы с высотой b
                if (Bucket[b].Count > 0) 
                {
                    //получаем последний узел, расположенный на максимальной высоте из Bucket 
                    int v = Bucket[b].Last();
                    //удаляем его из Bucket
                    Bucket[b].Remove(Bucket[b].Last());
                    //помечаем узел неактивным
                    active[v] = false;
                    //разгружаем узел
                    Discharge(v);
                }
                else
                {
                    //переходим на более низкую высоту
                    b--;
                }
            }
            MaxFlow = excess[graph.Target];
        }

        private void Enq(int v)//enqueue
        {
            //если вершина не активна (не содежится в Bucket) и допустима
            if (!active[v] && excess[v] > 0 && height[v] < graph.NodeCount)
            {
                //помечаем узел активным
                active[v] = true;
                //помещаем узел в Вucket
                Bucket[height[v]].Add(v);
                //обновляем рабочую высоту
                b = Math.Max(b, height[v]);
            }
        }

        private void Push(FlowEdge e, int from)
        {
            //вычислляем максимальный объем потока, который можно протолкнуть через ребро
            var f = Math.Min(excess[from], e.ResidualCapacityTo(e.Other(from))); 
            //если ребро допустимо
            if (height[from] == height[e.Other(from)] + 1 && f > 0)
            {
                //проталкиваем поток в ребро
                e.AddFlow(f, e.Other(from));
                Console.WriteLine("pushflow " + e.ToString());
                Console.WriteLine("hf " + f);
                //обновляем избыток в узлах ребра
                excess[e.Other(from)] += f;
                excess[from] -= f;
                //добавляем в Bucket узел, в сторону которого протолкнули поток
                Enq(e.Other(from));
            }
        }

        //The gap heuristic detects gaps in the labeling function 
        //If there is a label 0 < 𝓁' < | V | for which there is no node u such that 𝓁(u) = 𝓁', 
        //then any node u with 𝓁' < 𝓁(u) < | V | has been disconnected from t and can be relabeled to (| V | + 1) immediately.
        private void Gap(int k)
        {
            //просматриваем все узлы
            for (int v = 0; v < graph.NodeCount; v++)
                //если узел выше промежутка 
                if (height[v] >= k)
                {
                    //уменьшаем количество узлов в старой высоте
                    count[height[v]]--;
                    //обновляем метку высоты
                    height[v] = Math.Max(height[v], graph.NodeCount);
                    //увеличиваем количество узлов в новой высоте
                    count[height[v]]++;
                    //добавляем в Bucket узел
                    Enq(v);
                }
        }

        private void Relabel(int v)
        {
            //уменьшаем количество вершин в текущей высоте
            count[height[v]]--;
            //устанавливаем максимально возможную высоту
            height[v] = graph.NodeCount;
            //просматриваем все инцидентные v ребра
            foreach (var e in graph.Nodes[v].AllEdges)
            {
                //если ребро допустимо
                if (e.ResidualCapacityTo(e.Other(v)) > 0)
                {
                    //обновляем метку высоты v
                    height[v] = Math.Min(height[v], height[e.Other(v)] + 1);

                }
            }
            Console.WriteLine("relabel " + v +" h=" + height[v]);
            //увеличиваем количество вершин в новой высоте
            count[height[v]]++;
            //добавляем в Bucket поднятый узел
            Enq(v);
        }

        private void Discharge(int v)
        {
            //просматриваем все инцидентные узлу v ребра
            foreach (var e in graph.Nodes[v].AllEdges) 
            {
                //если избыток в узле положителен
                if (excess[v] > 0)
                {
                    //проталкиваем поток
                    Push(e, v);
                }
                else
                {
                    break;
                }
            }
            //если избыток в узле все еще положителен
            if (excess[v] > 0)
            {
                //если текущий узел последний на данной высоте
                if (count[height[v]] == 1)
                {
                    //перестанавливаем метки узлов, расположенных выше 
                    //текущей высоты до максимального значения
                    Gap(height[v]);
                }
                else
                {
                    //поднимаем вершину
                    Relabel(v);
                }
            }
        }

    }
}
