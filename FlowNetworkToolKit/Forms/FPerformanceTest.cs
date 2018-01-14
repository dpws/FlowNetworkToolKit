using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FlowNetworkToolKit.Core;
using FlowNetworkToolKit.Core.Base.Algorithm;
using FlowNetworkToolKit.Core.Utils;
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
                    dgTestResults.Rows.Add(item.Value.Name, item.Value.MaxFlow, item.Value.Runs, item.Value.Min,
                        item.Value.Max, item.Value.Avg);
                }
            }
        }

        private void btnGetStats_Click(object sender, EventArgs e)
        {
            var rootDir = new DirectoryInfo("E:\\wash\\test_sets");
            foreach (var workDir in rootDir.GetDirectories())
            {
                Log.Write($"Processing {workDir.Name}");
                foreach (var setDir in workDir.GetDirectories())
                {
                    Log.Write($"    Processing {setDir.Name}");
                    foreach (var networkFile in setDir.GetFiles())
                    {
                        Log.Write($"        Processing {networkFile.Name}");
                    }
                }
            }

        }
    }
}
