﻿using System;
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
using FlowNetworkToolKit.Core.Base.Network;
using FlowNetworkToolKit.Core.Utils.Loader;
using FlowNetworkToolKit.Core.Utils.Logger;

namespace FlowNetworkToolKit.Forms
{
    public partial class FMain : Form
    {
        public FMain()
        {
            InitializeComponent();
            loadAlgorithms();
        }


        private void mnAbout_Click(object sender, EventArgs e)
        {
            new FAbout().ShowDialog();
        }

        private void mnExit_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you shure want to exit?", "Confirm exit", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.OK)
                Application.Exit();
        }

        private void mnReloadAlgorithms_Click(object sender, EventArgs e)
        {
            loadAlgorithms();
        }

        private void loadAlgorithms()
        {
            var dir = AppDomain.CurrentDomain.BaseDirectory;
            var loader = new AlgorithmLoader();
            var list = loader.LoadFromNamespace("FlowNetworkToolKit.Algorithms");
            mnAlghoritmList.Items.Clear();
            for (int i = 0; i < list.Count; i++)
            {
                AlgorithmInfo alghoritm = list[i];
                alghoritm.Index = mnAlghoritmList.Items.Add(alghoritm.Name);
                list[i] = alghoritm;
            }
            Runtime.loadedAlghoritms = list;
            mnAlghoritmList.SelectedIndex = 0;
        }

        private void mnToggleLogWindow_Click(object sender, EventArgs e)
        {
            Log.toggleForm();
        }

        private void mnAlghoritmList_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                AlgorithmInfo algo = Runtime.loadedAlghoritms.First(t => t.Index == mnAlghoritmList.SelectedIndex);
                Log.Write($"Selected alghoritm: {algo.Name} ({algo.ClassName})");
                Runtime.currentAlghoritm = algo;
            }
            catch (InvalidOperationException except)
            {
                Log.Write(except.Message, Log.ERROR);
                return;
            }

            Refresh();
        }

        private void mnAlgorithmInfo_Click(object sender, EventArgs e)
        {
            if (Runtime.currentAlghoritm == null)
            {
                Log.Write("Can't show algorithm, info. Can't determine selected algorithm.", Log.WARNING);
                return;
            }
            var aboutAlgorithmForm = new FAlgorithmInfo(Runtime.currentAlghoritm);
            aboutAlgorithmForm.ShowDialog();
        }

        private void FMain_Paint(object sender, PaintEventArgs e)
        {
            if (Runtime.currentGraph == null)
            {
                pnPlaceHolder.Visible = true;
                slGraphInfo.Text = $"-";
            }
            else
            {
                pnPlaceHolder.Visible = false;
                slGraphInfo.Text = $"Nodes: {Runtime.currentGraph.NodeCount} Edges: {Runtime.currentGraph.EdgeCount}";
            }

            if (Runtime.currentAlghoritm != null)
            {
                mnAlgorithmInfo.Visible = true;
                if (Runtime.currentGraph != null)
                    mnRunAlghoritm.Visible = true;
                else
                    mnRunAlghoritm.Visible = false;
            }
            else
            {
                mnAlgorithmInfo.Visible = false;
                mnRunAlghoritm.Visible = false;
            }

        }

        private void visualizationToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        private void mnGenerate_Click(object sender, EventArgs e)
        {
            FlowNetwork g = new FlowNetwork();
            g.AddEdge(0, 1, 10);
            g.AddEdge(0, 2, 10);
            g.AddEdge(1, 2, 2);
            g.AddEdge(1, 3, 4);
            g.AddEdge(1, 4, 8);
            g.AddEdge(2, 4, 9);
            g.AddEdge(4, 5, 10);
            g.AddEdge(4, 3, 6);
            g.AddEdge(3, 5, 10);
            g.Source = 0;
            g.Target = 5;
            Runtime.currentGraph = g;

            Refresh();
        }

        private void runWithoutVisualizationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Runtime.currentAlghoritm == null)
            {
                Log.Write("Can't determine selected algorithm.");
                return;
            }
            if (Runtime.currentAlghoritm.Instance is BaseMaxFlowAlgorithm)
            {
                BaseMaxFlowAlgorithm algorithm = Runtime.currentAlghoritm.Instance;
                algorithm.SetGraph(Runtime.currentGraph);
                algorithm.Run();
                MessageBox.Show($"Max flow: {algorithm.MaxFlow}");
            }

        }
    }
}
