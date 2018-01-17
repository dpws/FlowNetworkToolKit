using System;
using System.Collections.Generic;
using System.Windows.Forms;
using FlowNetworkToolKit.Forms;

namespace FlowNetworkToolKit.Core.Utils.Logger
{
    class Log
    {

        #region Events

        public delegate void MessageAdded(LogItem item);

        public static event MessageAdded OnMessageAdded;

        #endregion

        public const int INFO = 0;
        public const int WARNING = 1;
        public const int ERROR = 2;

        private static FLog Form;
        private static List<LogItem> log = new List<LogItem>();


        public static int InfoCount = 0;
        public static int WarningCount = 0;
        public static int ErrorCount = 0;

        public static void Init()
        {
            Form = new FLog();
            var hWnd = Form.Handle;
            Form.FormClosing += delegate (object sender, FormClosingEventArgs e) {
                e.Cancel = true;
                ((FLog)sender).Hide();
            };
        }


        public static void Write(string message, int level = INFO)
        {
            if (Form.logVisualizer.InvokeRequired)
            {
                if (!Form.IsHandleCreated) return;
                 Form.Invoke(new MethodInvoker(() => { Write(message, level); }));
            }
            else
            {
                switch (level)
                {
                    case INFO: InfoCount++;
                        break;
                    case WARNING: WarningCount++;
                        break;
                    case ERROR: ErrorCount++;
                        break;
                }
                var item = new LogItem(DateTime.Now, level, message);
                log.Add(item);
                var text = item.ToString();
                Form.logVisualizer.AppendText(text);
                Form.logVisualizer.SelectionStart = Form.logVisualizer.TextLength - text.Length + 1;
                Form.logVisualizer.SelectionLength = text.Length;
                Form.logVisualizer.SelectionColor = item.GetColor();
                Form.logVisualizer.HideSelection = true;
                OnMessageAdded?.Invoke(item);
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
