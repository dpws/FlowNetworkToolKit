﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using FlowNetworkToolKit.Core.Utils.Logger;

namespace FlowNetworkToolKit.Core.Utils.Visualizer
{
    public static class Visualizer
    {

        public delegate void RedrawRequired();

        public static event RedrawRequired OnRedrawRequired;

        public static int CellSize = 15;
        public static int NodeDiameter = 24;
        public static int NodeMargin = 30;
        public static double Scale { private set; get; } = 1;

        public static Point Offset { private set; get; } = new Point(0, 0);

        public static Color BaseColor = Color.DarkCyan;

        public static Color HoverNodeColor = Color.DarkOrange;
        public static Color HoverNodeIncomingEdgeColor = Color.DarkRed;
        public static Color HoverNodeOutcomingEdgeColor = Color.Green;

        public static Color HoverEdgeColor = Color.DarkOrange;
        public static Color HoverEdgeFromNodeColor = Color.DarkRed;
        public static Color HoverEdgeToNodeColor = Color.Green;


        public static Color EdgeMarkedColor = Color.BlueViolet;
        public static Color EdgeActiveChangedColor = Color.DarkRed;
        public static Color EdgeChangedColor = Color.IndianRed;

        public static Brush SourceBrush = Brushes.LightSteelBlue;
        public static Brush TargetBrush = Brushes.DarkSeaGreen;

        public static Brush FlowBrush = Brushes.Black;
        public static Brush FlowBackgroundBrush = Brushes.White;

        public static Brush ChangedFlowBrush = Brushes.Red;
        public static Brush ChangedFlowBackgroundBrush = Brushes.White;

        public static Brush ChangedActiveFlowBrush = Brushes.Red;
        public static Brush ChangedActiveFlowBackgroundBrush = Brushes.Pink;

        public static TimeSpan ActiveChangeTime = TimeSpan.FromSeconds(1.5f);

        private static double maxScale = 10f, minScale = .3f;

        private static Dictionary<string, FlowEdgeState> EdgeStates = new Dictionary<string, FlowEdgeState>();

        #region Setters

        public static bool SetScale(double scale)
        {
            var newScale = Math.Round(scale, 2);
            if (newScale < minScale) newScale = minScale;
            if (newScale > maxScale) newScale = maxScale;
            var delta = newScale - Scale;
            Scale = newScale;
            if (delta != 0) return true;

            return false;
        }

        public static void SetOffset(Point offset)
        {
            Offset = offset;
        }

        #endregion

        public static void Visualise(Graphics g, Rectangle ClientRectangle)
        {
            drawGrid(g, ClientRectangle);
            drawEdges(g, ClientRectangle);
            drawNodes(g, ClientRectangle);
        }

        

        public static void ZoomFit(Rectangle ClientRectangle)
        {
            int padding = NodeDiameter*2;

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
            var scaleX = (ClientRectangle.Width) / (maxX - Offset.X + padding * 2);
            var scaleY = (ClientRectangle.Height) / (maxY - Offset.Y + padding * 2);

            SetScale(scaleX < scaleY ? scaleX : scaleY);

        }

        public static void Reset()
        {
            EdgeStates = new Dictionary<string, FlowEdgeState>();
            OnRedrawRequired?.Invoke();
        }

        public static void GetAnimation(Animator animator)
        {
            var animation = animator.GetAnimation();
            if (animation == null) return;
            switch (animation.Type)
            {
                case AnimationType.EdgeFlowChanged:
                    if(!EdgeStates.ContainsKey(animation.Edge.ToShortString()))
                        EdgeStates[animation.Edge.ToShortString()] = new FlowEdgeState();
                    EdgeStates[animation.Edge.ToShortString()].FlowChanged = true;
                    EdgeStates[animation.Edge.ToShortString()].Flow = animation.Edge.Flow;
                    break;
                case AnimationType.EdgeMarked:
                    if (!EdgeStates.ContainsKey(animation.Edge.ToShortString()))
                        EdgeStates[animation.Edge.ToShortString()] = new FlowEdgeState();
                    EdgeStates[animation.Edge.ToShortString()].Marked = true;
                    break;
                case AnimationType.EdgeUnmarked:
                    if (!EdgeStates.ContainsKey(animation.Edge.ToShortString()))
                        EdgeStates[animation.Edge.ToShortString()] = new FlowEdgeState();
                    EdgeStates[animation.Edge.ToShortString()].Marked = false;
                    break;
            }

            OnRedrawRequired?.Invoke();
        }

        #region Drawing

        public static void drawGrid(Graphics g, Rectangle ClientRectangle)
        {
            var cellDim = (int)Math.Round(CellSize * Scale);

            var cellOffset = new Point((int)Math.Round(Offset.X * Scale % cellDim), (int)Math.Round(Offset.Y * Scale % cellDim));

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



        public static void drawNodes(Graphics g, Rectangle ClientRectangle)
        {
            var fn = Runtime.currentGraph;

            foreach (var node in fn.Nodes.Values)
            {
                Point pos = TranslateAbsoluteToScreenPoint(node.Position.X, node.Position.Y);
                if (RuntimeManipulations.CreatingEdgeFromNode == node.Index)
                {
                    drawEgde(g, pos, RuntimeManipulations.CanvasCursorPosition, new Pen(BaseColor, 2), false);
                }
                pos.X -= NodeDiameter / 2;
                pos.Y -= NodeDiameter / 2;
                var bgbrush = Brushes.White;
                var ntext = "";
                if (node.Index == Runtime.currentGraph.Source)
                {
                    bgbrush = SourceBrush;
                    ntext = "Source";
                } else if (node.Index == Runtime.currentGraph.Target)
                {
                    bgbrush = TargetBrush;
                    ntext = "Target";
                }

                

                g.FillEllipse(bgbrush, pos.X, pos.Y, NodeDiameter, NodeDiameter);
                var nodeColor = BaseColor;
                if (RuntimeManipulations.ActiveNode != null && RuntimeManipulations.ActiveNode.Equals(node))
                    nodeColor = HoverNodeColor;

                if (node.Index == RuntimeManipulations.ActiveEdge?.From)
                    nodeColor = HoverEdgeFromNodeColor;
                if (node.Index == RuntimeManipulations.ActiveEdge?.To)
                    nodeColor = HoverEdgeToNodeColor;

                g.DrawEllipse(new Pen(nodeColor, 3), pos.X, pos.Y, NodeDiameter, NodeDiameter);
                var s = $"{node.Index}";
                int
                    len = s.Length,
                    dx = len > 3 ? -1 : len > 2 ? 1 : len > 1 ? 4 : 6,
                    dy = 6,
                    sz = 8,
                    x = pos.X + dx, y = pos.Y + dy;
                using (var font = new Font(FontFamily.GenericSansSerif, sz))
                {
                    g.DrawString(s, font, Brushes.Black, x, y);
                    if(ntext.Length > 0)
                        g.DrawString(ntext, font, Brushes.Black, x + NodeDiameter, y);
                }


            }

        }

        private static void drawEgde(Graphics g, Point from, Point to, Pen pen, bool excludeNodeDiameter = true)
        {
            int
                arrowAngle = 30,
                arrowSize = (int)Math.Round(12 * Scale),
                arrowShift = 15;

            var angle = Math.Atan2(from.X - to.X, from.Y - to.Y);
            var arrowAngleRad = ((float)arrowAngle / 180) * Math.PI;
            var arrowPivot = to;
            if (excludeNodeDiameter)
                arrowPivot = new Point((int)Math.Round(to.X + arrowShift * Math.Sin(angle)),
                (int)Math.Round(to.Y + arrowShift * Math.Cos(angle)));

            g.DrawLine(pen, from.X, from.Y, arrowPivot.X, arrowPivot.Y);

            g.DrawLine(pen, arrowPivot.X, arrowPivot.Y,
                (int)Math.Round(arrowPivot.X + arrowSize * Math.Sin(angle + arrowAngleRad)),
                (int)Math.Round(arrowPivot.Y + arrowSize * Math.Cos(angle + arrowAngleRad)));
            g.DrawLine(pen, arrowPivot.X, arrowPivot.Y,
                (int)Math.Round(arrowPivot.X + arrowSize * Math.Sin(angle - arrowAngleRad)),
                (int)Math.Round(arrowPivot.Y + arrowSize * Math.Cos(angle - arrowAngleRad)));
        }

        public static void drawEdges(Graphics g, Rectangle ClientRectangle)
        {
            var fn = Runtime.currentGraph;
            var pen = new Pen(BaseColor, 2);
            foreach (var edge in fn.Edges)
            {
                pen.Color = BaseColor;
                pen.Width = 2;
                if (edge.From == RuntimeManipulations.ActiveNode?.Index)
                {
                    pen.Color = HoverNodeOutcomingEdgeColor;
                }
                else if (edge.To == RuntimeManipulations.ActiveNode?.Index)
                {
                    pen.Color = HoverNodeIncomingEdgeColor;
                }

                if (EdgeStates.ContainsKey(edge.ToShortString()) && EdgeStates[edge.ToShortString()].FlowChanged)
                {
                    if (DateTime.Now - EdgeStates[edge.ToShortString()].Updated < ActiveChangeTime)
                    {
                        pen.Color = EdgeActiveChangedColor;
                        pen.Width = 4;
                    }
                    else
                    {
                        pen.Color = EdgeChangedColor;
                    }
                }

                if (RuntimeManipulations.ActiveEdge != null && RuntimeManipulations.ActiveEdge.Equals(edge))
                {
                    pen.Color = HoverEdgeColor;
                    
                }

                if (EdgeStates.ContainsKey(edge.ToShortString()) && EdgeStates[edge.ToShortString()].Marked)
                {
                    pen.Color = EdgeMarkedColor;
                    pen.Width = 4;
                }

                var posFrom = TranslateAbsoluteToScreenPoint(fn.Nodes[edge.From].Position.X,
                    fn.Nodes[edge.From].Position.Y);
                var posTo = TranslateAbsoluteToScreenPoint(fn.Nodes[edge.To].Position.X,
                    fn.Nodes[edge.To].Position.Y);

                drawEgde(g, posFrom, posTo, pen);

            }
            foreach (var edge in fn.Edges)
                {
                    var posFrom = TranslateAbsoluteToScreenPoint(fn.Nodes[edge.From].Position.X,
                        fn.Nodes[edge.From].Position.Y);
                    var posTo = TranslateAbsoluteToScreenPoint(fn.Nodes[edge.To].Position.X,
                        fn.Nodes[edge.To].Position.Y);

                var w = new Point((int)((posFrom.X + posTo.X) / 2f), (int)((posFrom.Y + posTo.Y) / 2f));

                using (var font = new Font(FontFamily.GenericSansSerif, 10))
                {
                    var weight = edge.Flow + "/" + edge.Capacity;

                    Brush textBrush = FlowBrush;Brush textBackgorundBrush = FlowBackgroundBrush;
                    if (EdgeStates.ContainsKey(edge.ToShortString()))
                    {
                        if (EdgeStates[edge.ToShortString()].FlowChanged)
                        {
                            weight = $"{EdgeStates[edge.ToShortString()].Flow}/{edge.Capacity}";
                            if (DateTime.Now - EdgeStates[edge.ToShortString()].Updated < ActiveChangeTime)
                            {
                                weight = $"{EdgeStates[edge.ToShortString()].Flow}/{edge.Capacity} ({(EdgeStates[edge.ToShortString()].DeltaFlow > 0 ? "+" : "")}{EdgeStates[edge.ToShortString()].DeltaFlow})";
                                textBrush = ChangedActiveFlowBrush;
                                textBackgorundBrush = ChangedActiveFlowBackgroundBrush;
                            }
                            else
                            {
                                textBrush = ChangedFlowBrush;
                                textBackgorundBrush = ChangedFlowBackgroundBrush;
                            }


                        }
                    }
                    var size = g.MeasureString(weight, font);
                    w.X -= (int)(size.Width / 2f);
                    w.Y -= (int)(size.Height / 2f);
                    g.FillRectangle(textBackgorundBrush, new RectangleF(w.X, w.Y, size.Width, size.Height));
                    g.DrawString(weight, font, textBrush, w.X, w.Y);
                }


            }
            pen.Dispose();
        }

        #endregion

        #region Arranges

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
            Log.Write("All nodes are arranged by circle");
        }

        public static void arrangeNodesByDistance(Rectangle ClientRectangle)
        {
            var spacing = NodeDiameter * 5;
            Dictionary<int, int> itemsAtLevel = new Dictionary<int, int>();
            Dictionary<int, int> levelOffsets = new Dictionary<int, int>();
            Dictionary<int, int> levelCounter = new Dictionary<int, int>();
            var fn = Runtime.currentGraph;
            var distances = fn.ComputeDistances();

            foreach (var node in fn.Nodes.Values)
            {
                var d = distances[node.Index];
                if (!itemsAtLevel.ContainsKey(d))
                {
                    itemsAtLevel[d] = 0;
                }
                itemsAtLevel[d]++;
            }

            int maxCountAtLevel = itemsAtLevel.Values.Max();

            foreach (var level in itemsAtLevel)
            {
                levelOffsets[level.Key] = ((maxCountAtLevel - level.Value) * spacing) / 2;
            }

            foreach (var node in fn.Nodes.Values)
            {
                var d = distances[node.Index];
                if (!levelCounter.ContainsKey(d))
                {
                    levelCounter[d] = 0;
                }

                node.Position = new Point(d * spacing , levelCounter[d] * spacing + levelOffsets[d]);
                levelCounter[d]++;
            }
            Log.Write("All nodes are arranged by distance");
        }

        #endregion

        #region Translations

        public static Point TranslateScreenToAbsolutePoint(int x, int y)
        {
            var newX = (int)Math.Round(x / Scale + Offset.X);
            var newY = (int)Math.Round(y / Scale + Offset.Y);
            return new Point(newX, newY);
        }

        public static Point TranslateScreenToAbsolutePoint(Point coords)
        {
            return TranslateScreenToAbsolutePoint(coords.X, coords.Y);
        }

        public static Point TranslateAbsoluteToScreenPoint(int x, int y)
        {
            var newX = (int)Math.Round((x - Offset.X) * Scale);
            var newY = (int)Math.Round((y - Offset.Y) * Scale);            
            return new Point(newX, newY);
        }

        public static Point TranslateAbsoluteToScreenPoint(Point coords)
        {
            return TranslateAbsoluteToScreenPoint(coords.X, coords.Y);
        }

        

        #endregion


    }
}
