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
using FlowNetworkToolKit.Core.Utils.Importer;
using FlowNetworkToolKit.Core.Utils.Loader;
using FlowNetworkToolKit.Core.Utils.Logger;
using FlowNetworkToolKit.Core.Utils.Visualizer;

namespace FlowNetworkToolKit.Forms
{
    public partial class FMain : Form
    {
        
        public FMain()
        {
            InitializeComponent();
            Log.Init();
            pbDraw.MouseWheel += canvas_MouseWheel;
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
            Invalidate();
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
            Invalidate();
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
            Visualizer.arrangeNodesByDistance(pbDraw.ClientRectangle);
            Visualizer.ZoomAll(pbDraw.ClientRectangle);
            Invalidate();
        }

        private void runWithoutVisualizationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Runtime.currentAlghoritm == null)
            {
                Log.Write("Can't determine selected algorithm.");
                return;
            }
            if (Runtime.currentGraph == null)
            {
                Log.Write("Please, load graph first.");
                return;
            }
            if (Runtime.currentAlghoritm.Instance is BaseMaxFlowAlgorithm)
            {
                BaseMaxFlowAlgorithm algorithm = Runtime.currentAlghoritm.Instance.Clone();
                algorithm.OnFinish += OnAlgorithmFinished;
                algorithm.SetGraph(Runtime.currentGraph);
                algorithm.RunAsync();
                
            }

        }

        private void mnOpen_Click(object sender, EventArgs e)
        {
            Invalidate();
        }



        private void canvas_MouseWheel(object sender, MouseEventArgs e)
        {
            if (Visualizer.SetScale(Math.Round(Visualizer.Scale + (e.Delta / 120 * 0.05), 2)))
            {
               pbDraw.Invalidate();
            }
            
        }


        private void importToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dlgImportFile.ShowDialog() == DialogResult.OK)
            {
                var file = new FileInfo(dlgImportFile.FileName);
                var importer = new Importer();
                var fn = importer.Import(file);
                if (fn != null)
                {
                    Runtime.currentGraph = fn;
                    Log.Write($"Loaded flow network from {file.FullName}");
                    Visualizer.arrangeNodesByDistance(pbDraw.ClientRectangle);
                    Visualizer.ZoomAll(pbDraw.ClientRectangle);
                    Invalidate();
                    pbDraw.Invalidate();
                }
                else
                {
                    Log.Write($"Fail to load flow network from {file.FullName}", Log.ERROR);
                }
            }

        }

        private void FMain_Resize(object sender, EventArgs e)
        {
            Invalidate();
            pbDraw.Invalidate();
        }

        private void OnAlgorithmFinished(BaseMaxFlowAlgorithm algorithm)
        {
                MessageBox.Show($"Max flow from {Runtime.currentGraph.Source} to {Runtime.currentGraph.Target}: {algorithm.MaxFlow}");
        }

        private void performanceTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Runtime.currentGraph == null) return;
            var Form = new FPerformanceTest();
            Form.ShowDialog();
        }

        public new void Invalidate()
        {
            checkPlaceHolder();
            base.Invalidate();
        }

        private void checkPlaceHolder()
        {
            if (Runtime.currentGraph == null)
            {
                pnPlaceHolder.Visible = true;
                mnZoomAll.Visible = false;
                tsVisStatus.Visible = false;
                slGraphInfo.Text = $"-";
            }
            else
            {
                pnPlaceHolder.Visible = false;
                mnZoomAll.Visible = true;
                tsVisStatus.Visible = true;
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

        private void canvas_Paint(object sender, PaintEventArgs e)
        {
            tsVisStatus.Text = $"Scale: {Visualizer.Scale} Offset: {Visualizer.Offset.X}, {Visualizer.Offset.Y}";
            if (Runtime.currentGraph != null)
            {
                Visualizer.Visualise(e.Graphics, pbDraw.ClientRectangle);
            }
        }

        private void FMain_Paint(object sender, PaintEventArgs e)
        {
        }

        private void pbDraw_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle)
            {
                changeCanvasOffset(e);
                return;
            }

            int hoverNode = RuntimeManipulations.GetHoverNode(e);
            if (hoverNode != -1)
            {
                Cursor = Cursors.Hand;
            }
            else
            {
                Cursor = Cursors.Default;
            }
            if (RuntimeManipulations.SetActiveNode(hoverNode))
            {
                pbDraw.Invalidate();
            }
           

            RuntimeManipulations.LastPanPosition = new Point();
        }

        private void changeCanvasOffset(MouseEventArgs e)
        {
            if (RuntimeManipulations.LastPanPosition != new Point())
            {
                var newX = Visualizer.Offset.X + (e.Location.X - RuntimeManipulations.LastPanPosition.X);
                var newY = Visualizer.Offset.Y + (e.Location.Y - RuntimeManipulations.LastPanPosition.Y);
                Visualizer.SetOffset(new Point(newX, newY));
                pbDraw.Invalidate();
            }
            RuntimeManipulations.LastPanPosition = e.Location;
        }

        private void mnZoomAll_Click(object sender, EventArgs e)
        {
            Visualizer.ZoomAll(pbDraw.ClientRectangle);
            pbDraw.Invalidate();
        }
    }
}
