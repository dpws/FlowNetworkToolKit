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
        EdgeFlowChanged,
        EdgeMarked,
        EdgeUnmarked
    }

    public class Animation
    {
        public FlowEdge Edge;
        public AnimationType Type;

        public Animation(object obj, AnimationType type)
        {
            Type = type;
            if (Type == AnimationType.EdgeFlowChanged || Type == AnimationType.EdgeMarked || Type == AnimationType.EdgeUnmarked)
            {
                if (!(obj is FlowEdge))
                    throw new InvalidConfigurationException("This types requires FLowEdge object");
                Edge = (FlowEdge) obj;
            }
        }

    }
}
