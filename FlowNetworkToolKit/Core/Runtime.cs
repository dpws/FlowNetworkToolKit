using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlowNetworkToolKit.Core.Base.Network;
using FlowNetworkToolKit.Core.Utils.Loader;

namespace FlowNetworkToolKit.Core
{
    public static class Runtime
    {
        public static List<AlgorithmInfo> loadedAlghoritms = new List<AlgorithmInfo>();
        public static AlgorithmInfo currentAlghoritm = null;
        public static FlowNetwork currentGraph = null;
    }
}
