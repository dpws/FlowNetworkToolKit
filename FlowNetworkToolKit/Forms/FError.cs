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
    public partial class FError : Form
    {
        bool stay;

        public FError(string message, int duration = 5000) : this()
        {
            tTimer.Interval = duration;
            lbErrorText.Text = message;
            
            tTimer.Start();
        }

        private FError()
        {
            InitializeComponent();
            Show();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Dispose();
        }

        private void tTimer_Tick(object sender, EventArgs e)
        {
            Dispose();
        }

        private void FError_FormClosed(object sender, FormClosedEventArgs e)
        {
            
        }
    }
}
