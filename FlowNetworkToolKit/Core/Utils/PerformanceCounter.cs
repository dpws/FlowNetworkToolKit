using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlowNetworkToolKit.Core.Utils.Loader;

namespace FlowNetworkToolKit.Core.Utils
{
    public struct AlgorithmPerformance
    {
        public string Name, Stats;
        public TimeSpan Min, Max, Avg;
        public int Runs;
        public double MaxFlow;
        public List<TimeSpan> AllRunsTime;
    }

    public class PerformanceCounter
    {
        public Dictionary<string, AlgorithmPerformance> items = new Dictionary<string, AlgorithmPerformance>();

        public void RegisterRun(string name, TimeSpan duration, double maxFlow)
        {
            AlgorithmPerformance run;
            if (items.ContainsKey(name)) { 
                run = items[name];
            }
            else
            {
                run = new AlgorithmPerformance();
                run.Name = name;
                run.MaxFlow = -1;
                run.Stats = "";
                run.Runs = 0;
                run.Avg = TimeSpan.MinValue;
                run.Max = TimeSpan.MinValue;
                run.Min = TimeSpan.MaxValue;
                run.AllRunsTime = new List<TimeSpan>();
                items.Add(name, run);
            }
            

            run.Runs++;
            run.AllRunsTime.Add(duration);
            if (duration > run.Max) run.Max = duration;
            if (duration < run.Min) run.Min = duration;
            run.MaxFlow = maxFlow;
            TimeSpan all = TimeSpan.Zero;
            foreach (var r in run.AllRunsTime)
            {
                all += r;
            }
            run.Avg = new TimeSpan(all.Ticks / run.AllRunsTime.Count);

            items[run.Name] = run;
        }

    }
}
