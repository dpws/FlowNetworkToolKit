using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowNetworkToolKit.Core.Base.Network
{
    public class FlowNode
    {

        public int Index { protected set; get; }
        public List<FlowEdge> IncomingEdges { protected set; get; } = new List<FlowEdge>();
        public List<FlowEdge> OutcomingEdges { protected set; get; } = new List<FlowEdge>();
        public List<FlowEdge> AllEdges
        {
            get
            {
                var list = new List<FlowEdge>();
                list.AddRange(IncomingEdges);
                list.AddRange(OutcomingEdges);
                return list;
            }
        }


        public FlowNode(int index)
        {
            Index = index;
        }

        public void AddEdge(ref FlowEdge edge)
        {
            if (Index == edge.From)
                OutcomingEdges.Add(edge);

            if (Index == edge.To)
                IncomingEdges.Add(edge);
        }
    }
}
