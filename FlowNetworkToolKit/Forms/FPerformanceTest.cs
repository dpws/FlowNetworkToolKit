using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using FlowNetworkToolKit.Core;
using FlowNetworkToolKit.Core.Base.Algorithm;
using FlowNetworkToolKit.Core.Utils;
using FlowNetworkToolKit.Core.Utils.Importer;
using FlowNetworkToolKit.Core.Utils.Logger;

namespace FlowNetworkToolKit.Forms
{
    public partial class FPerformanceTest : Form
    {
        public FPerformanceTest()
        {
            InitializeComponent();
            cblAlgorithms.Items.Clear();
            foreach (var algo in Runtime.loadedAlghoritms)
            {
                cblAlgorithms.Items.Add(algo.Name, true);
            }
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            var perfTest = new PerformanceCounter();
            if (cblAlgorithms.CheckedIndices != null)
                foreach (var item in cblAlgorithms.CheckedIndices)
                {
                    BaseMaxFlowAlgorithm algo = Runtime.loadedAlghoritms[(int)item].Instance;
                    for (int i = 0; i < udRunsCount.Value; i++)
                    {
                        BaseMaxFlowAlgorithm a = algo.Clone() as BaseMaxFlowAlgorithm;

                        a.OnFinish += (s) =>
                        {
                               perfTest.RegisterRun(s.GetName(), s.Elapsed, s.MaxFlow);
                                
                        };
                        a.SetGraph(Runtime.currentGraph);
                        a.Run();
                    }
                    updateDataGrid(perfTest);

                }
        }

        public void updateDataGrid(PerformanceCounter tester)
        {
            dgTestResults.Rows.Clear();
            if (dgTestResults.InvokeRequired)
            {
                dgTestResults.Invoke(new MethodInvoker(() => { updateDataGrid(tester); }));
            }
            else
            {
                foreach (var item in tester.items)
                {
                    dgTestResults.Rows.Add(item.Value.Name, item.Value.MaxFlow, item.Value.Runs, item.Value.Min.TotalSeconds,
                        item.Value.Max.TotalSeconds, item.Value.Avg.TotalSeconds);
                }
            }
        }

        private void btnGetStats_Click(object sender, EventArgs e)
        {
            var thread = new Thread(RunMultipleTests);
            thread.Start();
            //RunMultipleTests();
        }

        private void RunMultipleTests()
        {
            var importer = new Importer();
            var rootDir = new DirectoryInfo("E:\\wash\\test_sets");
            foreach (var workDir in rootDir.GetDirectories())
            {
                Log.Write($"Processing {workDir.Name}");
                foreach (var setDir in workDir.GetDirectories())
                {
                    Log.Write($"    Processing {setDir.Name}");
                    var statFile = new FileInfo(Path.Combine(setDir.FullName, "stats.csv"));
                    var counters = new Dictionary<int, PerformanceCounter>();
                    foreach (var networkFile in setDir.GetFiles())
                    {
                        if (networkFile.Extension.ToLower() != ".dimacs")
                        {
                            Log.Write($"     Skiping: {networkFile.Name}");
                            continue;
                        }
                        var name = networkFile.Name;
                        var count = int.Parse(name.Split('n')[0]);
                        if (!counters.ContainsKey(count))
                            counters[count] = new PerformanceCounter();

                        
                        var fn = importer.Import(networkFile);
                        if (fn != null)
                        {

                            Runtime.currentGraph = fn;
                            var pc = counters[count];
                            Log.Write($"        Processing {name} ({count})");
                            if (cblAlgorithms.CheckedIndices != null)
                                foreach (var item in cblAlgorithms.CheckedIndices)
                                {
                                
                                    BaseMaxFlowAlgorithm algo = Runtime.loadedAlghoritms[(int)item].Instance;
                                    if (count >= 500 && algo.GetName() == "Dinic") continue;
                                    for (int i = 0; i < udRunsCount.Value; i++)
                                    {
                                        BaseMaxFlowAlgorithm a = algo.Clone() as BaseMaxFlowAlgorithm;

                                        a.OnFinish += (s) =>
                                        {
                                            pc.RegisterRun(s.GetName(), s.Elapsed, s.MaxFlow);
                                        };

                                        a.SetGraph(Runtime.currentGraph);
                                        a.Run();
                                    }
                                }
                        }
                        else
                        {
                            Log.Write($"        Fail to load flow network from {networkFile.FullName}", Log.ERROR);
                        }
                        
                        
                      
                    }
                    string header = "";
                    var lines = new Dictionary<int, string>();
                    var headerSetted = false;
                    foreach (var counter in counters)
                    {

                        if (header != "") headerSetted = true;
                        foreach (var item in counter.Value.items)
                        {
                            if (!headerSetted)
                                header += $"узлы;{item.Value.Name}_Min;{item.Value.Name}_Max;{item.Value.Name}_Avg;;";

                            if (!lines.ContainsKey(counter.Key))
                                lines[counter.Key] = "";
                            lines[counter.Key] += $"{counter.Key};{item.Value.Min.TotalSeconds};{item.Value.Max.TotalSeconds};{item.Value.Avg.TotalSeconds};;";
                        }
                    }
                    using (var sw = new StreamWriter(statFile.FullName, false, Encoding.Default))
                    {
                        sw.WriteLine(header);
                        foreach (var line in lines)
                        {
                            sw.WriteLine(line.Value);
                        }
                    }
                    Log.Write($"        Saved: {statFile.FullName}");
                }
            }
            MessageBox.Show("Done");
        }
    }
}
