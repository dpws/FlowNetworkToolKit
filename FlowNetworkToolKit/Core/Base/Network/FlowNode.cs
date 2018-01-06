using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace FlowNetworkToolKit.Core.Base.Network
{

    public class FlowNode
    {

        public int Index { protected set; get; }
        public Point Position;
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

        public FlowNode()
        {
            
        }

        public FlowNode(FlowNode node): this()
        {
            Index = node.Index;
            Position = node.Position;
        }

        public double InCapacity
        {
            get { return IncomingEdges.Sum(e => e.Capacity); }
        }

        public double OutCapacity
        {
            get { return OutcomingEdges.Sum(e => e.Capacity); }
        }

        public FlowNode(int index)
        {
            Index = index;
        }

        public FlowNode(int index, int x, int y)
        {
            Index = index;
            Position = new Point(x,y);
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
