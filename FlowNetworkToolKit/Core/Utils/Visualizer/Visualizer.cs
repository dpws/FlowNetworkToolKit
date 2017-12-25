using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowNetworkToolKit.Core.Utils.Visualizer
{
    public static class Visualizer
    {
        public static int CellSize = 15;
        public static double Scale { private set; get; } = 1;

        public static bool SetScale(double scale)
        {
            var newScale = Math.Round(scale,2);   
            if (newScale < 0.3f) newScale = 0.3f;
            if (newScale > 3f) newScale = 3f;
            var delta = newScale - Scale;
            Scale = newScale;
            if (delta != 0) return true;

            return false;
        }

        public static void drawGrid(Graphics g, Rectangle ClientRectangle)
        {
            var cellDim = (int)Math.Round(CellSize * Scale);
            g.SmoothingMode = SmoothingMode.HighQuality;
            var r = ClientRectangle;
            using (Pen pen = new Pen(Color.FromArgb(210, 210, 255), 1))
            {
                for (int i = 1; i < r.Bottom; i += cellDim)
                    g.DrawLine(pen, r.X, r.Y + i, r.Right, r.Y + i);
                for (int i = 1; i < r.Right; i += cellDim)
                    g.DrawLine(pen, r.X + i, r.Y, r.X + i, r.Bottom);
            }
        }

    }
}
