using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using FlowNetworkToolKit.Core.Base.Exceptions;
using FlowNetworkToolKit.Core.Utils.Logger;
using FlowNetworkToolKit.Core.Utils.Visualizer;

namespace FlowNetworkToolKit.Core.Base.Network
{
    public class FlowNetwork
    {

        #region Events

        public delegate void FlowNetworkCreated(FlowNetwork sender);
        public delegate void EdgeAdded(FlowNetwork sender, FlowEdge edge);
        public delegate void EdgeFlowChanged(FlowNetwork sender, FlowEdge edge);
        public delegate void EdgeLengthChanged(FlowNetwork sender, FlowEdge edge);
        public delegate void EdgeMarked(FlowNetwork sender, FlowEdge edge);
        public delegate void EdgeUnmarked(FlowNetwork sender, FlowEdge edge);

        public event FlowNetworkCreated OnCreate;
        public event EdgeAdded OnAddEdge;
        public event EdgeFlowChanged OnEdgeFlowChanged;
        public event EdgeLengthChanged OnEdgeLengthChanged;
        public event EdgeMarked OnEdgeMarked;
        public event EdgeUnmarked OnEdgeUnmarked;

        #endregion
        private int _source = -1;
        private int _target = -1;

        public List<FlowEdge> Edges { get; protected set; } = new List<FlowEdge>();

        public Dictionary<int, FlowNode> Nodes { get; protected set; } = new Dictionary<int, FlowNode>();

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

        public FlowNetwork(List<FlowEdge> edges, Dictionary<int, FlowNode> nodes) : this()
        {
            foreach (var node in nodes.Values)
                Nodes.Add(node.Index, new FlowNode(node));
            foreach (var edge in edges)
                AddEdge(edge.From, edge.To, edge.Capacity, edge.Flow);
        }

        public FlowNetwork(FlowNetwork graph) : this(graph.Edges, graph.Nodes)
        {
            Source = graph.Source;
            Target = graph.Target;
        }

        public void AddNodeAtMouse(MouseEventArgs e) 
        {
            var nextIndex = Nodes.Count > 0 ? Nodes.Keys.Max() + 1 : 0;
            var node = new FlowNode(nextIndex);
            node.Position = Visualizer.TranslateScreenToAbsolutePoint(e.Location);
            Nodes.Add(node.Index, node);
        }

        public void AddEdge(int from, int to, double capacity, double f = -1)
        {
            if (from == to)
                throw new InvalidConfigurationException("'from' cant equals 'to'");

            if (capacity < 0)
              throw new InvalidConfigurationException($"Capacity can't be less than 0");

            FlowEdge sameEdge = null;
            try
            {
                 sameEdge = Edges.Find((e) => (e.From == from && e.To == to) || (e.From == to && e.To == from));
            } catch(Exception e) { }

            if (sameEdge != null)
                throw new InvalidConfigurationException($"Edge between {from} and {to} already exists");

            var edge = f == -1 ? new FlowEdge(from, to, capacity) : new FlowEdge(from, to, capacity, f);
            edge.OnFlowChanged += (sender) => OnEdgeFlowChanged?.Invoke(this, sender);
            edge.OnLengthChanged += (sender, length) => OnEdgeLengthChanged?.Invoke(this, sender);
            edge.OnEdgeMarked += (sender) => OnEdgeMarked?.Invoke(this, sender);
            edge.OnEdgeUnmarked += (sender) => OnEdgeUnmarked?.Invoke(this, sender);

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

            var distances = ComputeDistances();
            if(distances[Target] == -1)
                errors.Add("Target isn't reachable");

            return errors;
        }

        public override string ToString()
        {
            return $"Edges: {EdgeCount} Nodes: {NodeCount}";
        }


        public void DeleteNode(int node)
        {
            Nodes.Remove(node);
            Edges.RemoveAll(e => e.From == node || e.To == node);
            if (_source == node) _source = Nodes.First().Value.Index;
            if (_target == node) _target = Nodes.Last().Value.Index;
        }

        public void DeleteEdge(int from, int to)
        {
            Edges.RemoveAll(e => e.From == from && e.To == to);
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

        #region Custom serialization

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
                Nodes[id].Position = new Point(x,y);
            }
        }

        #endregion
    }
}
