using System;

namespace FlowNetworkToolKit.Core.Base.Network
{
    public class FlowEdge : IEquatable<FlowEdge>
    {
        #region Events

        public delegate void FlowChanged(FlowEdge sender);
        public delegate void LengthChanged(FlowEdge sender, int length);
        public delegate void EdgeMarked(FlowEdge sender);
        public delegate void EdgeUnmarked(FlowEdge sender);

        public event FlowChanged OnFlowChanged;
        public event LengthChanged OnLengthChanged;
        public event EdgeMarked OnEdgeMarked;
        public event EdgeUnmarked OnEdgeUnmarked;

        #endregion
        public int From;
        public int To;
        public double Capacity;
        public double ResidualCapacity => Capacity - Flow;
        public int Length { protected set; get; } = 0;

        public double Flow { get; private set; } = 0;

        public FlowEdge(int from, int to, double capacity) 
        {
            From = from;
            To = to;
            Capacity = capacity;
            Flow = 0;
        }

        public FlowEdge(int from, int to, double capacity, double flow)
        {
            From = from;
            To = to;
            Capacity = capacity;
            Flow = flow;
        }

        public FlowEdge(FlowEdge e) 
        {
            From = e.From;
            To = e.To;
            Capacity = e.Capacity;
            Flow = e.Flow;
        }

        public void Mark()
        {
            OnEdgeMarked?.Invoke(this);
        }

        public void Unmark()
        {
            OnEdgeUnmarked?.Invoke(this);
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
            double deltaFLow = 0;
            if (to == From) deltaFLow = -flow;
            if (to == To || to == null) deltaFLow = flow;

            //if (Flow + deltaFLow > Capacity)
            //    throw new InvalidFlowException($"New flow ({Flow + flow}) exceed capacity ({Capacity})");
            //if (Flow + deltaFLow < Double.Epsilon)
            //    throw new InvalidFlowException($"New flow ({Flow + flow}) less than ({Double.Epsilon})");
            if(deltaFLow != 0) 
                OnFlowChanged?.Invoke(this);
            Flow += deltaFLow;
        }

        public void SetFlow(double newFlow)
        {
            Flow = newFlow;
        }

        public override string ToString()
        {
            return $"{From} -> {To} ({Capacity})";
        }

        public string ToShortString()
        {
            return $"{From} -> {To}";
        }

        public void SwitchFromTo()
        {
            var tmp = To;
            To = From;
            From = tmp;
        }

        public bool Equals(FlowEdge other)
        {
            if (other == null) return false;
            return this.From == other.From && this.To == other.To;
        }

        public int GetHashCode(FlowEdge obj)
        {
            return obj.From * 31 + obj.To;
        }
    }
}
