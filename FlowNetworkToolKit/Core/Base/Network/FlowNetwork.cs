using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlowNetworkToolKit.Core.Base.Exceptions;

namespace FlowNetworkToolKit.Core.Base.Network
{
    

    public class FlowNetwork
    {

        #region Events

        public delegate void FlowNetworkCreated(FlowNetwork sender);
        public delegate void EdgeAdded(FlowNetwork sender, FlowEdge edge);
        public delegate void EdgeFlowChanged(FlowNetwork sender, FlowEdge edge);
        public delegate void EdgeLengthChanged(FlowNetwork sender, FlowEdge edge);

        public event FlowNetworkCreated OnCreate;
        public event EdgeAdded OnAddEdge;
        public event EdgeFlowChanged OnEdgeFlowChanged;
        public event EdgeLengthChanged OnEdgeLengthChanged;

        #endregion

        private int _source = -1;
        private int _target = -1;

        public List<FlowEdge> Edges { get; protected set; } = new List<FlowEdge>();
        public Dictionary<int, FlowNode> Nodes { get; protected set; } = new Dictionary<int, FlowNode>();

        public new int NodeCount
        {
            get { return Nodes.Count; }
        }

        public new int EdgeCount
        {
            get { return Edges.Count; }
        }

        public int Source
        {
            get { return _source; }
            set
            {
                if (Target != -1 && Target == value)
                    throw new InvalidConfigurationException("Source can't equals target");
                if (!Nodes.ContainsKey(value))
                    throw new InvalidConfigurationException("Source points to undefined node");
                _source = value;
            }
        }

        public int Target
        {
            get { return _target; }
            set
            {
                if (Source != -1 && Source == value)
                    throw new InvalidConfigurationException("Target can't equals source");
                if (!Nodes.ContainsKey(value))
                    throw new InvalidConfigurationException("Target points to undefined node");
                _target = value;
            }
        }

        public FlowNetwork()
        {

        }

        public FlowNetwork(List<FlowEdge> edges)
        {
            foreach (var edge in edges)
                AddEdge(edge.From, edge.To, edge.Capacity, edge.Flow);
        }

        public FlowNetwork(FlowNetwork graph): this(graph.Edges)
        {
            Source = graph.Source;
            Target = graph.Target;
        }

        public void AddEdge(int from, int to, double capacity, double f = -1)
        {
            if (from == to)
                throw new InvalidConfigurationException("'from' cant equals 'to'");

            //if (capacity < Double.Epsilon)
              //  throw new InvalidConfigurationException($"Capacity can't be less than {Double.Epsilon}");
           
            var edge = f == -1? new FlowEdge(from, to, capacity) : new FlowEdge(from, to, capacity, f);
            edge.OnFlowChanged += (sender, cap, flow) => OnEdgeFlowChanged?.Invoke(this, sender);
            edge.OnLengthChanged += (sender, length) => OnEdgeLengthChanged?.Invoke(this, sender);

            Edges.Add(edge);
            if (!Nodes.ContainsKey(edge.From))
                Nodes.Add(edge.From, new FlowNode(edge.From));

            if (!Nodes.ContainsKey(edge.To))
                Nodes.Add(edge.To, new FlowNode(edge.To));

            Nodes[edge.From].AddEdge(ref edge);
            Nodes[edge.To].AddEdge(ref edge);

        }

        public List<string> Validate()
        {
            List<string> errors = new List<string>();

            if (Source < 0)
                errors.Add("Source isn't set");
            if (Target < 0)
                errors.Add("Target isn't set");

            return errors;
        }

        public override string ToString()
        {
            return $"Edges: {EdgeCount} Nodes: {NodeCount}";
        }

        public void PushFlow(int from, int to, double flow)
        {

            var ind = Edges.IndexOf(new FlowEdge(from, to, 0));
            if (ind == -1) ind = Edges.IndexOf(new FlowEdge(to, from, 0));
            Edges[ind].AddFlow(flow, to);
        }
    }
}
