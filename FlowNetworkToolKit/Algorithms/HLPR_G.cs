using FlowNetworkToolKit.Core.Base.Algorithm;
using FlowNetworkToolKit.Core.Base.Network;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FlowNetworkToolKit.Algorithms
{
    class HLPR_G : BaseMaxFlowAlgorithm
    {
        //The highest-label push–relabel algorithm[11] organizes all nodes into buckets indexed by their labels. 
        //The algorithm always selects an active node with the largest label to discharge.
        double[] excess;
        int[] height;
        Dictionary<int,int> count = new Dictionary<int, int>();
        bool[] active;
        Dictionary<int,List<int>> Bucket = new Dictionary<int,List<int>>(); //buckets
        int b;
        public HLPR_G()
        {
            Name = "HLPR_G";
            Url = "";
            Description = @"";
        }
        protected override void Init()
        {
            excess = new double[graph.NodeCount];
            height = new int[graph.NodeCount+2];
            active = new bool[graph.NodeCount];
            Bucket = new Dictionary<int, List<int>>();
            count = new Dictionary<int, int>();
            for (int i = 0; i < graph.NodeCount; i++)
            {
                height[i] = 0;
                excess[i] = 0;
                
                active[i] = false;
                
            }
            height[graph.NodeCount] = 0;
            b = 0;
        }

        protected override void Logic()
        {
            SearchMaxFlow();
        }

        public void SearchMaxFlow()
        {
            //Bucket.Add(0, new List<int>());
            //увеличиваем избыток в источнике
            foreach (var e in graph.Nodes[graph.Source].AllEdges) 
            {
                // excess[graph.Source] += e.Capacity;
                excess[e.Other(graph.Source)] += e.Capacity;
                e.AddFlow(e.Capacity, e.Other(graph.Source));
                Enq(e.Other(graph.Source));
            }
            height[graph.Source] = graph.NodeCount;
            //if (!count.ContainsKey(graph.NodeCount))
                count.Add(graph.NodeCount, 1);

            //все узлы находятся на высоте 0
            count.Add(0, graph.NodeCount);
            //помечаем узел стока активным
            active[graph.Target] = true;
            //пока текущая рабочая высота неотрицательна (Bucket содержит активные узлы)
            while (b >= 0)
            {
                //если в Bucket есть активные узлы с высотой b
                if (Bucket.ContainsKey(b) && Bucket[b].Count > 0) 
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
            if (!active[v] && excess[v] > 0) //&& height[v] < graph.NodeCount)
            {
                //помечаем узел активным
                active[v] = true;
                //помещаем узел в Вucket
                if (!Bucket.ContainsKey(height[v]))
                    Bucket.Add(height[v], new List<int>());
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
                //обновляем избыток в узлах ребра
                excess[e.Other(from)] += f;
                excess[from] -= f;
                //добавляем в Bucket узел, в сторону которого протолкнули поток
                if(e.Other(from)!=graph.Source)
                    Enq(e.Other(from));
            }
        }

        //The gap heuristic detects gaps in the labeling function 
        //If there is a label 0 < 𝓁' < | V | for which there is no node u such that 𝓁(u) = 𝓁', 
        //then any node u with 𝓁' < 𝓁(u) < | V | has been disconnected from t and can be relabeled to (| V | + 1) immediately.
        private void Gap(int k)
        {
            //просматриваем все узлы
            for (int v = 1; v < graph.NodeCount; v++)
                //если узел выше промежутка 
                if (height[v] >= k)
                {
                    //уменьшаем количество узлов в старой высоте
                    count[height[v]]--;
                    //обновляем метку высоты
                    height[v] = Math.Max(height[v], graph.NodeCount+1);
                    //увеличиваем количество узлов в новой высоте
                    if (!count.ContainsKey(height[v]))
                        count.Add(height[v], 0);
                    count[height[v]]++;
                    //добавляем в Bucket узел
                    Enq(v);
                }
        }

        private void Relabel(int v)
        {
            var oldheight = height[v];
            //уменьшаем количество вершин в текущей высоте
            count[height[v]]--;
            //устанавливаем максимально возможную высоту
            height[v] = int.MaxValue;//graph.NodeCount;
            //просматриваем все инцидентные v ребра
            foreach (var e in graph.Nodes[v].AllEdges)
                //если ребро допустимо
                if (e.ResidualCapacityTo(e.Other(v)) > 0)
                    //обновляем метку высоты v
                    height[v] = Math.Min(height[v], height[e.Other(v)] + 1);
          //  Console.WriteLine("relabel " + v +" h=" + height[v]);
            //увеличиваем количество вершин в новой высоте
            if (!count.ContainsKey(height[v]))
                count.Add(height[v], 0);
            count[height[v]]++;
            //добавляем в Bucket поднятый узел
            Enq(v);
            if (count[oldheight] == 0)
                Gap(oldheight);
        }

        private void Discharge(int v)
        {
            //просматриваем все инцидентные узлу v ребра
            foreach (var e in graph.Nodes[v].AllEdges) 
            {
                //если избыток в узле положителен
                if (excess[v] > 0)
                    //проталкиваем поток
                    Push(e, v);
                else
                    break;
            }
            //если избыток в узле все еще положителен
            if (excess[v] > 0)
                Relabel(v);
        }

    }
}
