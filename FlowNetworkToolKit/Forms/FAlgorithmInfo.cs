using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FlowNetworkToolKit.Core.Utils.Loader;

namespace FlowNetworkToolKit.Forms
{
    public partial class FAlgorithmInfo : Form
    {
        public FAlgorithmInfo(AlgorithmInfo algorithm)
        {
            InitializeComponent();
            llAlgorithmUrl.Text = algorithm.Url;
            lAlgorithmName.Text = algorithm.Name;
            rtAlgorithmDescription.Text = algorithm.Description;
        }

        private void llAlgorithmUrl_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(llAlgorithmUrl.Text);
        }
    }
}
