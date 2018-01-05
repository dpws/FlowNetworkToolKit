using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Numerics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using FlowNetworkToolKit.Core.Base.Network;

namespace FlowNetworkToolKit.Core.Utils.Visualizer
{
    public static class Visualizer
    {
        public static int CellSize = 15;
        public static int NodeDiameter = 24;
        public static int NodeMargin = 30;
        public static double Scale { private set; get; } = 1;

        public static Point Offset { private set; get; } = new Point(0, 0);

        public static Color BaseColor = Color.DarkCyan;
        public static Color SelectedColor = Color.DarkCyan;
        public static Brush SourceBrush = Brushes.LightSteelBlue;
        public static Brush TargetBrush = Brushes.LightPink;

        private static double maxScale = 6f, minScale = 0.3f;

        public static bool SetScale(double scale)
        {
            var newScale = Math.Round(scale,2);   
            if (newScale < minScale) newScale = minScale;
            if (newScale > maxScale) newScale = maxScale;
            var delta = newScale - Scale;
            Scale = newScale;
            if (delta != 0) return true;

            return false;
        }

        public static bool SetOffset(Point offset)
        {
            Offset = offset;
            return false;
        }

        public static void Visualise(Graphics g, Rectangle ClientRectangle)
        {
            drawGrid(g, ClientRectangle);
            drawEdges(g, ClientRectangle);
            drawNodes(g, ClientRectangle);
            
        }

        public static void ZoomAll(Rectangle ClientRectangle)
        {
            int padding = NodeDiameter;

            double minX, minY, maxX, maxY;

            minX = minY = double.MaxValue;
            maxX = maxY = double.MinValue;

            foreach (var node in Runtime.currentGraph.Nodes)
            {
                var pos = node.Value.Position;
                if (pos.X > maxX) maxX = pos.X;
                if (pos.Y > maxY) maxY = pos.Y;
                if (pos.X < minX) minX = pos.X;
                if (pos.Y < minY) minY = pos.Y;
            }

            SetOffset(new Point((int) minX - padding, (int) minY - padding));
            var scaleX = (ClientRectangle.Width) / (maxX - Offset.X + padding *2);
            var scaleY = (ClientRectangle.Height) / (maxY - Offset.Y + padding * 2);

            SetScale(scaleX < scaleY ? scaleX : scaleY);

        }

        public static void drawGrid(Graphics g, Rectangle ClientRectangle)
        {
            var cellDim = (int)Math.Round(CellSize * Scale);
            var cellOffset = new Point(Offset.X % cellDim, Offset.Y % cellDim);
            g.SmoothingMode = SmoothingMode.HighQuality;
            var r = ClientRectangle;
            using (Pen pen = new Pen(Color.FromArgb(210, 210, 255), 1))
            {
                for (int i = 1; i < r.Bottom; i += cellDim)
                    g.DrawLine(pen, r.X, r.Y + i - cellOffset.Y, r.Right, r.Y + i - cellOffset.Y);
                for (int i = 1; i < r.Right; i += cellDim)
                    g.DrawLine(pen, r.X + i - cellOffset.X, r.Y, r.X + i - cellOffset.X, r.Bottom);
            }
        }

        public static void arrangeNodesByCircle(Rectangle ClientRectangle)
        {
            var fn = Runtime.currentGraph;
            var c = new Point(ClientRectangle.Width / 2, ClientRectangle.Height / 2);
            int rx = (int)(ClientRectangle.Width / 2.5), ry = (int)(ClientRectangle.Height / 2.5);
            var factor = Math.PI * 2 / fn.NodeCount;
            foreach (var node in fn.Nodes)
            {
                var a = factor * node.Key;
                node.Value.Position = new Point(c.X + (int)(rx * Math.Sin(a)), c.Y + (int)(ry * Math.Cos(a)));
                
            }
        }

        public static void arrangeNodesByDistance(Rectangle ClientRectangle)
        {
            var spacing = NodeDiameter * 6;

            var fn = Runtime.currentGraph;
            foreach (var node in fn.Nodes)
            {
                

            }
        }

        public static void drawNodes(Graphics g, Rectangle ClientRectangle)
        {
            var fn = Runtime.currentGraph;

            

            foreach (var node in fn.Nodes)
            {
                
                var newX = (int)(node.Value.Position.X * Scale) - Offset.X;
                var newY = (int)(node.Value.Position.Y * Scale) - Offset.Y;
                Point pos = new Point(newX, newY);
                var bgbrush = Brushes.White;
                var ntext = "";
                if (node.Value.Index == Runtime.currentGraph.Source)
                {
                    bgbrush = SourceBrush;
                    ntext = "Source";
                } else if (node.Value.Index == Runtime.currentGraph.Target)
                {
                    bgbrush = TargetBrush;
                    ntext = "Target";
                }
                g.FillEllipse(bgbrush, pos.X, pos.Y, NodeDiameter, NodeDiameter);
                g.DrawEllipse(new Pen(BaseColor, 3), pos.X, pos.Y, NodeDiameter, NodeDiameter);
                var s = $"{node.Value.Index}";
                int
                    len = s.Length,
                    dx = len > 3 ? -1 : len > 2 ? 1 : len > 1 ? 2 : 6,
                    dy = 6,
                    sz = len > 2 ? 6 : 8,
                    x = pos.X + dx, y = pos.Y + dy;
                using (var font = new Font(FontFamily.GenericSansSerif, sz))
                {
                    g.DrawString(s, font, Brushes.Black, x, y);
                    if(ntext.Length > 0)
                        g.DrawString(ntext, font, Brushes.Black, x + NodeDiameter, y);
                }
                    
            }

        }

        public static void drawEdges(Graphics g, Rectangle ClientRectangle)
        {
            var fn = Runtime.currentGraph;
            var pen = new Pen(BaseColor, 2);
            foreach (var edge in fn.Edges)
            {
                var posFrom = new Point((int) (fn.Nodes[edge.From].Position.X * Scale + NodeDiameter / 2 - Offset.X),
                    (int) (fn.Nodes[edge.From].Position.Y * Scale + NodeDiameter / 2 - Offset.Y));
                var posTo = new Point((int) (fn.Nodes[edge.To].Position.X * Scale + NodeDiameter / 2 - Offset.X),
                    (int) (fn.Nodes[edge.To].Position.Y * Scale + NodeDiameter / 2 - Offset.Y));
                g.DrawLine(pen, posFrom.X, posFrom.Y, posTo.X, posTo.Y);
                var angle = Math.Atan2(posFrom.X - posTo.X, posFrom.Y - posTo.Y);

                int
                    arrowAngle = 30,
                    arrowSize = 12,
                    arrowShift = 15;

                var arrowAngleRad = ((float) arrowAngle / 180) * Math.PI;

                var arrowPivot = new Point((int) Math.Round(posTo.X + arrowShift * Math.Sin(angle)),
                    (int) Math.Round(posTo.Y + arrowShift * Math.Cos(angle)));

                g.DrawLine(pen, arrowPivot.X, arrowPivot.Y,
                    (int) Math.Round(arrowPivot.X + arrowSize * Math.Sin(angle + arrowAngleRad)),
                    (int) Math.Round(arrowPivot.Y + arrowSize * Math.Cos(angle + arrowAngleRad)));
                g.DrawLine(pen, arrowPivot.X, arrowPivot.Y,
                    (int) Math.Round(arrowPivot.X + arrowSize * Math.Sin(angle - arrowAngleRad)),
                    (int) Math.Round(arrowPivot.Y + arrowSize * Math.Cos(angle - arrowAngleRad)));

            }
            foreach (var edge in fn.Edges)
                {
                    var posFrom = new Point((int)(fn.Nodes[edge.From].Position.X * Scale + NodeDiameter / 2 - Offset.X), (int)(fn.Nodes[edge.From].Position.Y * Scale + NodeDiameter / 2 - Offset.Y));
                    var posTo = new Point((int)(fn.Nodes[edge.To].Position.X * Scale + NodeDiameter / 2 - Offset.X), (int)(fn.Nodes[edge.To].Position.Y * Scale + NodeDiameter / 2 - Offset.Y));
                var w = new Point((int)((posFrom.X + posTo.X) / 2f), (int)((posFrom.Y + posTo.Y) / 2f));

                using (var font = new Font(FontFamily.GenericSansSerif, 10))
                {
                    var weight = edge.Flow + "/" +(edge.Capacity - edge.Flow);
                    var size = g.MeasureString(weight, font);
                    w.X -=(int)(size.Width / 2f);
                    w.Y -= (int)(size.Height / 2f);
                    g.FillRectangle(Brushes.White, new RectangleF(w.X, w.Y, size.Width, size.Height));
                    g.DrawString(weight, font, Brushes.Black, w.X, w.Y);
                }
            
            
            }
            pen.Dispose();
        }
    }
}
