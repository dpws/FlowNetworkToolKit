using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FlowNetworkToolKit.Core.Base.Network;
using FlowNetworkToolKit.Core.Utils.Loader;
using FlowNetworkToolKit.Forms;

namespace FlowNetworkToolKit.Core
{
    public static class Runtime
    {
        public static List<AlgorithmInfo> loadedAlghoritms = new List<AlgorithmInfo>();
        public static AlgorithmInfo currentAlghoritm = null;

        public static FlowNetwork currentGraph = null;

        public static bool VisualisationEnabled = true;
        public static bool EditionEnabled = true;
        public static bool CreationEnabled = false;

        private static List<FError> MessageForms = new List<FError>();

        public static void ShowError(string message)
        {
            var fError = new FError(message);
            MessageForms.Add(fError);
            fError.Disposed += (sender, args) =>
            {
                MessageForms.RemoveAt(MessageForms.IndexOf((FError) sender));
                RearrangeMessageForms();
            };
            var fMain = Application.OpenForms[0];
            fError.Left = fMain.Left + fMain.Width - fError.Width - 10;
            fError.Top = fMain.Top + fMain.Height - ((fError.Height + 10) * MessageForms.Count) - 20;
        }

        public static void RearrangeMessageForms()
        {
            if (Application.OpenForms.Count > 0)
            {
                var fMain = Application.OpenForms[0];
                int i = 1;
                foreach (var fError in MessageForms)
                {
                    fError.Top = fMain.Top + fMain.Height - ((fError.Height + 10) * i) - 20;
                    i++;
                }
            }
        }
    }
}
