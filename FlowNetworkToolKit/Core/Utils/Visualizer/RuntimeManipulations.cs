using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FlowNetworkToolKit.Core.Base.Network;
using FlowNetworkToolKit.Core.Utils.Logger;

namespace FlowNetworkToolKit.Core.Utils.Visualizer
{
    
    public static class RuntimeManipulations
    {
        public static FlowNode ActiveNode;
        public static FlowEdge ActiveEdge;

        public static int DraggedNode = -1;

        public static Point LastPanPosition = new Point();
        public static Point CanvasCursorPosition = new Point();


        public static FlowNode GetHoverNode(MouseEventArgs e)
        {

            int nodeRadius = Visualizer.NodeDiameter / 2;

            foreach (var node in Runtime.currentGraph.Nodes.Values)
            {
                Point np = Visualizer.TranslateAbsoluteToScreenPoint(node.Position);
                //(a - x) ^ 2 + (b - y) ^ 2 < R ^ 2

                if (Math.Pow(np.X - e.X, 2) + Math.Pow(np.Y - e.Y, 2) <= Math.Pow(nodeRadius, 2))
                {
                    return node;
                }
            }
            return null;
        }

        public static FlowEdge GetHoverEdge(MouseEventArgs e)
        {
            int allowedDistance = 5;
            foreach (var edge in Runtime.currentGraph.Edges)
            {
                var from = Runtime.currentGraph.Nodes[edge.From];
                var to = Runtime.currentGraph.Nodes[edge.To];
                Point pf = Visualizer.TranslateAbsoluteToScreenPoint(from.Position);
                Point pt = Visualizer.TranslateAbsoluteToScreenPoint(to.Position);


                if (e.X > Math.Min(pf.X, pt.X) - allowedDistance && e.X < Math.Max(pf.X, pt.X) + allowedDistance && e.Y > Math.Min(pf.Y, pt.Y) - allowedDistance &&
                    e.Y < Math.Max(pf.Y, pt.Y) + allowedDistance)
                {
                    var dist = Math.Abs((e.X - pf.X) * (pt.Y - pf.Y) - (e.Y - pf.Y) * (pt.X - pf.X)) /
                               Math.Sqrt(Math.Pow((pt.X - pf.X), 2) + Math.Pow(pt.Y - pf.Y, 2));
                    if (dist < allowedDistance)
                    {
                        return edge;
                    }
                }
            }
            return null;
        }


        public static bool SetActiveNode(FlowNode node)
        {
            bool ret = !ActiveNode?.Equals(node) ?? node != null;
            ActiveNode = node;
            return ret;

        }

        public static bool SetActiveEdge(FlowEdge edge)
        {
            bool ret = !ActiveEdge?.Equals(edge) ?? edge != null;
            ActiveEdge = edge;
            return ret;

        }
    }
}
