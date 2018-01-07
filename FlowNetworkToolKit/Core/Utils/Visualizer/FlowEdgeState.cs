using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowNetworkToolKit.Core.Utils.Visualizer
{
    public class FlowEdgeState
    {
        public bool FlowChanged;
        public double Flow;
        public bool Marked;

        public FlowEdgeState(bool flowChanged = false, bool marked = false, double flow = 0f)
        {
            FlowChanged = flowChanged;
            Flow = flow;
            Marked = marked;
        }
    }
}
