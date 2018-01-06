using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlowNetworkToolKit.Forms;

namespace FlowNetworkToolKit.Core.Utils.Logger
{
    public struct LogItem
    {
        public DateTime Time { private set; get; }
        public int Level { private set; get; }
        public string Message { private set; get; }

        public LogItem(DateTime time, int level, string message)
        {
            Time = time;
            Level = level;
            Message = message;
            if (level == Log.ERROR)
            {
                Runtime.ShowError(message);
            }
        }

        public Color GetColor()
        {
            var color = Color.Black;
            switch (Level)
            {
                case Log.INFO:
                    color = Color.MidnightBlue;
                    break;
                case Log.WARNING:
                    color = Color.Orange;
                    break;
                case Log.ERROR:
                    color = Color.DarkRed;
                    break;
            }
            return color;
        }

        public override string ToString()
        {
            var levelSign = "";
            switch (Level)
            {
                case Log.INFO:
                    levelSign = "";
                    break;
                case Log.WARNING:
                    levelSign = "W";
                    break;
                case Log.ERROR:
                    levelSign = "E";
                    break;
            }
            return $"{levelSign} [{Time.ToLocalTime()}] {Message} {Environment.NewLine}";
        }
    }
}
