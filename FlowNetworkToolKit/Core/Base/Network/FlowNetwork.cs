using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using FlowNetworkToolKit.Core.Base.Exceptions;
using FlowNetworkToolKit.Core.Utils.Logger;

namespace FlowNetworkToolKit.Core.Base.Network
{
    public class FlowNetwork: ISerializable
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

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("source", _source);
            info.AddValue("target", _target);
        }

        public int NodeCount
        {
            get { return Nodes.Count; }
        }

        public int EdgeCount
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

        public FlowNetwork(List<FlowEdge> edges) : this()
        {
            foreach (var edge in edges)
                AddEdge(edge.From, edge.To, edge.Capacity, edge.Flow);
        }

        public FlowNetwork(FlowNetwork graph) : this(graph.Edges)
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

            var edge = f == -1 ? new FlowEdge(from, to, capacity) : new FlowEdge(from, to, capacity, f);
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

        public Dictionary<int, int> ComputeDistances() //для отрисовки (базовое расстояние от источника до узла)
        {
            var distances = new Dictionary<int, int>();
            foreach (var node in Nodes)
            {
                distances[node.Value.Index] = -1;
            }
            distances[Source] = 0;
            var queue = new Queue<int>();
            queue.Enqueue(Source);
            while (queue.Count > 0)
            {
                var v = queue.Dequeue();

                foreach (var e in Nodes[v].AllEdges)
                {
                    var w = e.Other(v);
                    if (e.ResidualCapacityTo(w) > 0 && distances[w] < 0)
                    {
                        distances[w] = distances[v] + 1;
                        queue.Enqueue(w);
                    }
                }
            }
            return distances;
        }

        public XmlDocument GenerateXml()
        {
            XmlDocument document = new XmlDocument();
            document.AppendChild(document.CreateXmlDeclaration("1.0", "utf-8", null));
            XmlNode root = document.CreateElement("flow-network");
            //XmlAttribute attribute = document.CreateAttribute("num");
            //attribute.Value = "5";
            document.AppendChild(root);
            XmlNode source = document.CreateElement("source");
            source.InnerText = _source.ToString();
            XmlNode target = document.CreateElement("target");
            target.InnerText = _target.ToString();
            root.AppendChild(source);
            root.AppendChild(target);
            XmlNode edges = document.CreateElement("edges");
            root.AppendChild(edges);
            foreach (var edge in Edges)
            {
                XmlNode ed = document.CreateElement("edge");
                XmlAttribute from = document.CreateAttribute("from");
                from.Value = edge.From.ToString();
                XmlAttribute to = document.CreateAttribute("to");
                to.Value = edge.To.ToString();
                XmlAttribute capacity = document.CreateAttribute("capacity");
                capacity.Value = edge.Capacity.ToString();
                ed.Attributes.Append(from);
                ed.Attributes.Append(to);
                ed.Attributes.Append(capacity);
                edges.AppendChild(ed);
            }

            XmlNode nodes = document.CreateElement("node-positions");
            root.AppendChild(nodes);
            foreach (var node in Nodes.Values)
            {
                XmlNode nd = document.CreateElement("node");
                XmlAttribute id = document.CreateAttribute("id");
                id.Value = node.Index.ToString();
                XmlAttribute x = document.CreateAttribute("x");
                x.Value = node.Position.X.ToString();
                XmlAttribute y = document.CreateAttribute("y");
                y.Value = node.Position.Y.ToString();
                nd.Attributes.Append(id);
                nd.Attributes.Append(x);
                nd.Attributes.Append(y);
                nodes.AppendChild(nd);
            }

            return document;
        }

        public void ParseXml(XmlDocument xml)
        {
            XmlNodeList edges = xml.SelectNodes("//flow-network/edges/edge");
            foreach (XmlNode xn in edges)
            {
                AddEdge(int.Parse(xn.Attributes["from"].Value), int.Parse(xn.Attributes["to"].Value), int.Parse(xn.Attributes["capacity"].Value));
            }
            XmlNode source = xml.SelectNodes("//flow-network/source")[0];
            XmlNode target = xml.SelectNodes("//flow-network/target")[0];
            _source = int.Parse(source.InnerText);
            _target = int.Parse(target.InnerText);
            XmlNodeList nodes = xml.SelectNodes("//flow-network/node-positions/node");
            foreach (XmlNode xn in nodes)
            {
                int id, x, y;
                id = int.Parse(xn.Attributes["id"].Value);
                x = int.Parse(xn.Attributes["x"].Value);
                y = int.Parse(xn.Attributes["y"].Value);
                Log.Write($"{id}: {x},{y}");
                Nodes[id].Position = new Point(x,y);
            }
        }
    }
}
