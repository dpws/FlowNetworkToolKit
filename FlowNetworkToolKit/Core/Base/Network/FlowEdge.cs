using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlowNetworkToolKit.Core.Base.Exceptions;

namespace FlowNetworkToolKit.Core.Base.Network
{
    public class FlowEdge
    {
        #region Events

        public delegate void FlowChanged(FlowEdge sender, double capacity, double flow);
        public delegate void LengthChanged(FlowEdge sender, int length);

        public event FlowChanged OnFlowChanged;
        public event LengthChanged OnLengthChanged;

        #endregion

        public readonly int From;
        public readonly int To;
        public readonly double Capacity;

        public double ResidualCapacity => Capacity - Flow;

        public int Length { protected set; get; } = 0;
        public double Flow = 0;


        public FlowEdge(int from, int to, double capacity) : base()
        {
            From = from;
            To = to;
            Capacity = capacity;
        }

        public int Other(int node)
        {
            if (From != node && To != node)
                throw new ArgumentException($"Node {node} is outside this edge.");

            return node == From ? To : From;
        }

        public double ResidualCapacityTo(int node)
        {
            if (From != node && To != node)
                throw new ArgumentException($"Node {node} is outside this edge.");

            return node == From ? Flow : ResidualCapacity;
        }

        public void SetLength(int length)
        {
            Length = length;
            OnLengthChanged?.Invoke(this, Length);
        }

        public void AddFlow(double flow, int? to = null)
        {
            if (to != null && From != to && To != to)
                throw new ArgumentException($"Node {to} is outside this edge.");

            if (Flow + flow > Capacity)
                throw new InvalidFlowException($"New flow ({Flow + flow}) exceed capacity ({Capacity})");
            if (Flow + flow < Double.Epsilon)
                throw new InvalidFlowException($"New flow ({Flow + flow}) less than ({Double.Epsilon})");

            if (to == From) Flow -= flow;
            if (to == To) Flow += flow;
            if (to == null) Flow += flow;
        }

        public override string ToString()
        {
            return $"{From} > {To} ({Capacity})";
        }
    }
}
