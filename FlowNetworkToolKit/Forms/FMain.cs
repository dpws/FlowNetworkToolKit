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
using System.Xml;
using System.Xml.Serialization;
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
            Log.OnMessageAdded += item =>
            {
                tsInfoCount.Text = Log.InfoCount.ToString();
                tsWarningCount.Text = Log.WarningCount.ToString();
                tsErrorCount.Text = Log.ErrorCount.ToString();
            }; 
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

        private void mnRunVisualization_Click(object sender, EventArgs e)
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
                algorithm.OnEdgeFlowChanged += (flowAlgorithm, network, edge) => Log.Write($"{edge} flow changed"); 
                algorithm.OnFinish += OnAlgorithmFinished;
                algorithm.SetGraph(Runtime.currentGraph);
                algorithm.RunAsync();

            }
        }


        private void canvas_MouseWheel(object sender, MouseEventArgs e)
        {
            if (!Runtime.VisualisationEnabled) return;
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
                    Visualizer.ZoomFit(pbDraw.ClientRectangle);
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
            checkControlsAccesible();
            base.Invalidate();
        }

        private void checkControlsAccesible()
        {
            mnEditionEnabled.Enabled = mnCreationEnabled.Enabled = Runtime.VisualisationEnabled;
            mnEditionEnabled.Checked = Runtime.EditionEnabled;
            mnCreationEnabled.Checked = Runtime.CreationEnabled;

            if (Runtime.currentGraph == null)
            {
                pnPlaceHolder.Visible = true;
                mnZoomAll.Visible = false;
                tsVisStatus.Visible = false;
                mnArrangement.Enabled = false;
                slGraphInfo.Text = $"No flow network is loaded";
            }
            else
            {
                pnPlaceHolder.Visible = false;
                mnZoomAll.Visible = true;
                tsVisStatus.Visible = true;
                mnArrangement.Enabled = true;
                slGraphInfo.Text = $"Nodes: {Runtime.currentGraph.NodeCount} Edges: {Runtime.currentGraph.EdgeCount}";
                
            }

            if (Runtime.currentAlghoritm != null)
            {
                mnAlgorithmInfo.Visible = true;
                if (Runtime.currentGraph != null && Runtime.currentGraph.Source != -1 && Runtime.currentGraph.Target != -1)
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
            if (Runtime.VisualisationEnabled)
            {
                tsVisStatus.Text = $"Scale: {Visualizer.Scale} Offset: {Visualizer.Offset.X}, {Visualizer.Offset.Y}";
            }
            else
            {
                tsVisStatus.Text = "Visualization disabled";
            }
            if (Runtime.currentGraph != null && Runtime.VisualisationEnabled)
            {
                Visualizer.Visualise(e.Graphics, pbDraw.ClientRectangle);
            }


        }

        private void FMain_Paint(object sender, PaintEventArgs e)
        {
        }

        private void pbDraw_MouseMove(object sender, MouseEventArgs e)
        {
            bool needInvalidate = false;
            if (RuntimeManipulations.CreatingEdgeFromNode != -1)
                needInvalidate = true;
            RuntimeManipulations.CanvasCursorPosition = e.Location;
            #region Pan
            if (e.Button == MouseButtons.Middle)
            {
                changeCanvasOffset(e);
                return;
            }
            RuntimeManipulations.LastPanPosition = new Point();

            #endregion

            #region Hover
            
            if (Runtime.EditionEnabled)
            {
                FlowNode hoverNode = null;
                FlowEdge hoverEdge = null;
                if (e.Button == MouseButtons.Left && RuntimeManipulations.ActiveNode != null)
                {
                    RuntimeManipulations.ActiveNode.Position =
                        Visualizer.TranslateScreenToAbsolutePoint(e.X, e.Y);
                    needInvalidate = true;
                }
                else
                {
                    hoverNode = RuntimeManipulations.GetHoverNode(e);
                    if (hoverNode != null)
                    {
                        Cursor = Cursors.Hand;
                    }
                    else
                    {
                        
                        hoverEdge = RuntimeManipulations.GetHoverEdge(e);
                        if (hoverEdge != null)
                        {
                            Cursor = Cursors.Hand;
                        }
                        else
                        {
                            Cursor = Cursors.Default;
                        }

                    }
                    if (RuntimeManipulations.SetActiveNode(hoverNode))
                    {
                        needInvalidate = true;
                    }
                    if (RuntimeManipulations.SetActiveEdge(hoverEdge))
                    {
                        needInvalidate = true;
                    }
                }
            }

            #endregion

            if(needInvalidate)
                pbDraw.Invalidate();
        }

        private void changeCanvasOffset(MouseEventArgs e)
        {
            if (RuntimeManipulations.LastPanPosition != new Point())
            {
                var newX = Visualizer.Offset.X + (e.Location.X + RuntimeManipulations.LastPanPosition.X);
                var newY = Visualizer.Offset.Y + (e.Location.Y + RuntimeManipulations.LastPanPosition.Y);
                Visualizer.SetOffset(new Point(newX, newY));
                pbDraw.Invalidate();
            }
            RuntimeManipulations.LastPanPosition = e.Location;
        }

        private void mnZoomAll_Click(object sender, EventArgs e)
        {
            Visualizer.ZoomFit(pbDraw.ClientRectangle);
            pbDraw.Invalidate();
        }

        private void mnSave_Click(object sender, EventArgs e)
        {
            if (Runtime.currentGraph == null) return;
            if (dlgSaveFile.ShowDialog() == DialogResult.OK)
            {
                using (FileStream fs = new FileStream(dlgSaveFile.FileName, FileMode.OpenOrCreate))
                {
                    var xml = Runtime.currentGraph.GenerateXml();
                    xml.Save(fs);
                }
            }
        }


        private void mnOpen_Click(object sender, EventArgs e)
        {
            if (dlgOpenFile.ShowDialog() == DialogResult.OK)
            {
                using (FileStream fs = new FileStream(dlgOpenFile.FileName, FileMode.Open))
                {
                    XmlDocument xml = new XmlDocument();
                    try
                    {
                        xml.Load(fs);
                        FlowNetwork g = new FlowNetwork();
                        g.ParseXml(xml);
                        Runtime.currentGraph = g;
                        Visualizer.ZoomFit(pbDraw.ClientRectangle);
                        Invalidate();
                        pbDraw.Invalidate();

                    }
                    catch (Exception ex)
                    {
                        Log.Write("Can't load FlowNetwork from file.", Log.ERROR);
                        Log.Write(ex.Message, Log.ERROR);
                    }
                    
                }
            }
        }

        private void byDistanceFromSourceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Visualizer.arrangeNodesByDistance(ClientRectangle);
            Visualizer.ZoomFit(ClientRectangle);
            pbDraw.Invalidate();
        }

        private void byCircleFormToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Visualizer.arrangeNodesByCircle(ClientRectangle);
            Visualizer.ZoomFit(ClientRectangle);
            pbDraw.Invalidate();
        }

        private void mnVisualisationEnabled_CheckStateChanged(object sender, EventArgs e)
        {
            Runtime.VisualisationEnabled = mnVisualisationEnabled.Checked;
            Invalidate();
            pbDraw.Invalidate();
        }

        private void mnCreationEnabled_CheckStateChanged(object sender, EventArgs e)
        {
            Runtime.CreationEnabled = mnCreationEnabled.Checked;
            Invalidate();
        }

        private void mnEditionEnabled_CheckStateChanged(object sender, EventArgs e)
        {
            Runtime.EditionEnabled = mnEditionEnabled.Checked;
            Invalidate();
        }

        private void pbDraw_MouseClick(object sender, MouseEventArgs e)
        {
            if (RuntimeManipulations.ActiveNode != null && e.Button == MouseButtons.Right)
            {
                cmNodeId.Text = $"NODE: {RuntimeManipulations.ActiveNode.Index}";
                cmNodeInfo.Text =
                    $"IN: {RuntimeManipulations.ActiveNode.InCapacity} OUT: {RuntimeManipulations.ActiveNode.OutCapacity}";
                cmNodeSetTarget.Enabled = cmNodeSetSource.Enabled = RuntimeManipulations.ActiveNode.Index != Runtime.currentGraph.Source && RuntimeManipulations.ActiveNode.Index != Runtime.currentGraph.Target;
                cmNodeAddEdge.Enabled = Runtime.CreationEnabled;
                cmNode.Show(Cursor.Position);
            }
            if (RuntimeManipulations.ActiveEdge != null && e.Button == MouseButtons.Right)
            {
                mnEdgeCapacitySave.Enabled = false;
                cmEdgeInfo.Text = $"{RuntimeManipulations.ActiveEdge.From} -> {RuntimeManipulations.ActiveEdge.To}";
                cmEdgeCapacityTextBox.Text = RuntimeManipulations.ActiveEdge.Capacity.ToString();
                cmEdge.Show(Cursor.Position);
            }

            if (e.Button == MouseButtons.Left)
            {
                if (Runtime.CreationEnabled)
                {
                    if (RuntimeManipulations.ActiveNode == null)
                    {
                        if (RuntimeManipulations.CreatingEdgeFromNode == -1)
                        {
                            Runtime.currentGraph.AddNodeAtMouse(e);
                            Invalidate();
                            pbDraw.Invalidate();
                        }
                        RuntimeManipulations.CreatingEdgeFromNode = -1;
                    }
                    else
                    {
                        if (RuntimeManipulations.CreatingEdgeFromNode != -1)
                        {
                            try
                            {
                                Runtime.currentGraph.AddEdge(RuntimeManipulations.CreatingEdgeFromNode,
                                    RuntimeManipulations.ActiveNode.Index, 1);
                                RuntimeManipulations.CreatingEdgeFromNode = -1;
                                pbDraw.Invalidate();
                            }
                            catch (Exception ex)
                            {
                                Log.Write(ex.Message, Log.ERROR);
                            }
                        }
                    }
                }

                
            }
        }

        private void cmNodeDelete_Click(object sender, EventArgs e)
        {
            Runtime.currentGraph.DeleteNode(RuntimeManipulations.ActiveNode.Index);
            Invalidate();
            pbDraw.Invalidate();
        }

        private void cmNodeSetSource_Click(object sender, EventArgs e)
        {
            Runtime.currentGraph.Source = RuntimeManipulations.ActiveNode.Index;
            Invalidate();
        }

        private void cmNodeSetTarget_Click(object sender, EventArgs e)
        {
            Runtime.currentGraph.Target = RuntimeManipulations.ActiveNode.Index;
            Invalidate();
        }

        private void cmEdgeDelete_Click(object sender, EventArgs e)
        {
            Runtime.currentGraph.DeleteEdge(RuntimeManipulations.ActiveEdge.From, RuntimeManipulations.ActiveEdge.To);
            Invalidate();
            pbDraw.Invalidate();
        }

        private void mnEdgeCapacitySave_Click(object sender, EventArgs e)
        {
            int capacity = 0;
            if (!int.TryParse(cmEdgeCapacityTextBox.Text, out capacity))
            {
                cmEdgeCapacityTextBox.ForeColor = Color.LightPink;
                return;
            }
            RuntimeManipulations.ActiveEdge.Capacity = capacity;
            Invalidate();
            pbDraw.Invalidate();
        }

        private void cmEdgeCapacityTextBox_TextChanged(object sender, EventArgs e)
        {
            double capacity = -1;
            if (!double.TryParse(cmEdgeCapacityTextBox.Text, out capacity) || capacity < 0)
            {
                cmEdgeCapacityTextBox.BackColor = Color.LightPink;
                mnEdgeCapacitySave.Enabled = false;
            }
            else
            {
                cmEdgeCapacityTextBox.BackColor = Color.White;
                if(capacity != RuntimeManipulations.ActiveEdge.Capacity)
                    mnEdgeCapacitySave.Enabled = true;
                else
                    mnEdgeCapacitySave.Enabled = false;
            }
        }

        private void cmNodeAddEdge_Click(object sender, EventArgs e)
        {
            RuntimeManipulations.CreatingEdgeFromNode = RuntimeManipulations.ActiveNode.Index;
        }

        private void cmEdgeInverse_Click(object sender, EventArgs e)
        {
            RuntimeManipulations.ActiveEdge.SwitchFromTo();
            pbDraw.Invalidate();
        }

        private void mnCreate_Click(object sender, EventArgs e)
        {
            if (Runtime.currentGraph != null)
            {
                if (MessageBox.Show("All unsaved changes in the current flow network will be lost", "Are you sure?",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    return;
            }
            Runtime.currentGraph = new FlowNetwork();
            Runtime.CreationEnabled = true;
            Runtime.EditionEnabled = true;
            Runtime.VisualisationEnabled = true;
            Invalidate();
            pbDraw.Invalidate();
        }

        
    }
}
