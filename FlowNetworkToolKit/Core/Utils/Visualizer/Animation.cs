using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlowNetworkToolKit.Core.Base.Exceptions;
using FlowNetworkToolKit.Core.Base.Network;

namespace FlowNetworkToolKit.Core.Utils.Visualizer
{
    public enum AnimationType
    {
        EdgeFlowChanged, //Изменение потока ребра
        EdgeMarked, //Пометка ребра
        EdgeUnmarked //Снятие пометки с ребра
    }

    public class Animation
    {
        public FlowEdge Edge; //Анимируемое ребро
        public AnimationType Type; //Тип анимации

        //Тип object используется для возможности анимации не только рёбер, но и вершин
        public Animation(object obj, AnimationType type)
        {
            Type = type;
            if (Type == AnimationType.EdgeFlowChanged || Type == AnimationType.EdgeMarked || Type == AnimationType.EdgeUnmarked)
            {
                //Проверка соответствия типа анимации классу переданого объекта
                if (!(obj is FlowEdge))
                    throw new InvalidConfigurationException("This types requires FLowEdge object");
                //Edge = new FlowEdge((FlowEdge) obj);
                Edge = (FlowEdge) obj;
            }
        }

    }
}
