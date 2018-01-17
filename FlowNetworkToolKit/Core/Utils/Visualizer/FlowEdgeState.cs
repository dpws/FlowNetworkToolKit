using System;

namespace FlowNetworkToolKit.Core.Utils.Visualizer
{
    public class FlowEdgeState
    {
        public bool FlowChanged = false;

        private double _flow = 0;
        public double Flow
        {
            get => _flow;
            set
            {
                if(value - _flow != 0)
                    DeltaFlow = value - _flow;
                _flow = value;
            }
        }

        private double _deltaFlow = 0;

        public double DeltaFlow
        {
            get => _deltaFlow;
            private set
            {
                _deltaFlow = value;
                Updated = DateTime.Now;
            }
        }


        public bool Marked = false;

        public DateTime Updated;

    }
}
