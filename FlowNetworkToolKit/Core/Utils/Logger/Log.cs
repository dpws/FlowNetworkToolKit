using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FlowNetworkToolKit.Forms;

namespace FlowNetworkToolKit.Core.Utils.Logger
{
    class Log
    {
        public const int INFO = 0;
        public const int WARNING = 1;
        public const int ERROR = 2;

        private static FLog Form;

        private static List<LogItem> log = new List<LogItem>();

        public static void Init()
        {
            Form = new FLog();
            Form.FormClosing += delegate (object sender, FormClosingEventArgs e) {
                e.Cancel = true;
                ((FLog)sender).Hide();
            };
        }


        public static void Write(string message, int level = INFO)
        {
            var item = new LogItem(DateTime.Now, level, message);
            log.Add(item);
            if (Form.logVisualizer.InvokeRequired)
            {
                Form.Invoke(new MethodInvoker(() => { Write(message, level); }));
            }
            else
            {
                var text = item.ToString();
                Form.logVisualizer.AppendText(text);
                Form.logVisualizer.SelectionStart = Form.logVisualizer.TextLength - text.Length + 1;
                Form.logVisualizer.SelectionLength = text.Length;
                Form.logVisualizer.SelectionColor = item.GetColor();
                Form.logVisualizer.HideSelection = true;
            }

        }

        public static void ShowForm()
        {
            Form.Show();
        }

        public static void HideForm()
        {
            Form.Hide();
        }

        public static void toggleForm()
        {
            if (Form.Visible)
            {
                HideForm();
            }
            else
            {
                ShowForm();
            }
        }
    }
}
