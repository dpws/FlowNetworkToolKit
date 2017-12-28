using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FlowNetworkToolKit.Core;

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
    }
}
