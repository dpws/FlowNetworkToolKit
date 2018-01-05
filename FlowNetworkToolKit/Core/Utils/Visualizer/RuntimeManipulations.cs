using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FlowNetworkToolKit.Core.Utils.Logger;

namespace FlowNetworkToolKit.Core.Utils.Visualizer
{
    public static class RuntimeManipulations
    {
        public static int ActiveNode = -1;
        public static int ActiveEdge = -1;

        public static int DraggedNode = -1;

        public static Point LastPanPosition = new Point();
        public static Point CanvasCursorPosition = new Point();


        public static int GetHoverNode(MouseEventArgs e)
        {
            var real = Visualizer.TranslateScreenToAbsolutePoint(e.X, e.Y);

            int nodeRadius = Visualizer.NodeDiameter / 2;

            foreach (var node in Runtime.currentGraph.Nodes.Values)
            {
                Point np = Visualizer.TranslateAbsoluteToScreenPoint(node.Position);
                //(a - x) ^ 2 + (b - y) ^ 2 < R ^ 2

                if (Math.Pow(np.X - e.X, 2) + Math.Pow(np.Y - e.Y, 2) <= Math.Pow(nodeRadius, 2))
                {
                    return node.Index;
                }
            }
            return -1;
        }

        public static bool SetActiveNode(int node)
        {
            bool ret = ActiveNode != node;
            ActiveNode = node;
            return ret;

        }
    }
}
