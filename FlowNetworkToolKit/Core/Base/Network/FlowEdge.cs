using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using FlowNetworkToolKit.Core.Base.Exceptions;

namespace FlowNetworkToolKit.Core.Base.Network
{
    public class FlowEdge : IEquatable<FlowEdge>, ISerializable
    {
        #region Events

        public delegate void FlowChanged(FlowEdge sender, double capacity, double flow);
        public delegate void LengthChanged(FlowEdge sender, int length);

        public event FlowChanged OnFlowChanged;
        public event LengthChanged OnLengthChanged;

        #endregion
        public int From;
        public int To;
        public double Capacity;
        public double ResidualCapacity => Capacity - Flow;
        public int Length { protected set; get; } = 0;
        public double Flow = 0;


        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
        }
        public FlowEdge(int from, int to, double capacity) : base()
        {
            From = from;
            To = to;
            Capacity = capacity;
        }

        public FlowEdge(int from, int to, double capacity, double flow) : base()
        {
            From = from;
            To = to;
            Capacity = capacity;
            Flow = flow;
        }

        public FlowEdge(FlowEdge e) : base()
        {
            From = e.From;
            To = e.To;
            Capacity = e.Capacity;
            Flow = e.Flow;
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

            Flow += deltaFLow;
        }

        public override string ToString()
        {
            return $"{From} > {To} ({Capacity})";
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
