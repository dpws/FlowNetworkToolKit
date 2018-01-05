using System.Windows.Forms;

namespace FlowNetworkToolKit.Forms
{
    partial class FMain
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.ssStatus = new System.Windows.Forms.StatusStrip();
            this.slGraphInfo = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsVisStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.mainMenu = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mnCreate = new System.Windows.Forms.ToolStripMenuItem();
            this.mnOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.importToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mnGenerate = new System.Windows.Forms.ToolStripMenuItem();
            this.mnSave = new System.Windows.Forms.ToolStripMenuItem();
            this.mnExit = new System.Windows.Forms.ToolStripMenuItem();
            this.mnReloadAlgorithms = new System.Windows.Forms.ToolStripMenuItem();
            this.visualizationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mnVisualisationEnabled = new System.Windows.Forms.ToolStripMenuItem();
            this.mnEditionEnabled = new System.Windows.Forms.ToolStripMenuItem();
            this.mnCreationEnabled = new System.Windows.Forms.ToolStripMenuItem();
            this.mnArrangement = new System.Windows.Forms.ToolStripMenuItem();
            this.byDistanceFromSourceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.byCircleFormToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mnAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.mnZoomAll = new System.Windows.Forms.ToolStripMenuItem();
            this.mnAlghoritmList = new System.Windows.Forms.ToolStripComboBox();
            this.mnToggleLogWindow = new System.Windows.Forms.ToolStripMenuItem();
            this.mnAlgorithmInfo = new System.Windows.Forms.ToolStripMenuItem();
            this.mnRunAlghoritm = new System.Windows.Forms.ToolStripMenuItem();
            this.mnRunVisualization = new System.Windows.Forms.ToolStripMenuItem();
            this.runWithoutVisualizationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.performanceTestToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dlgOpenFile = new System.Windows.Forms.OpenFileDialog();
            this.dlgImportFile = new System.Windows.Forms.OpenFileDialog();
            this.pnPlaceHolder = new System.Windows.Forms.Panel();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.btnImport = new System.Windows.Forms.Button();
            this.btnOpen = new System.Windows.Forms.Button();
            this.btnCreate = new System.Windows.Forms.Button();
            this.dlgSaveFile = new System.Windows.Forms.SaveFileDialog();
            this.pbDraw = new System.Windows.Forms.PictureBox();
            this.ssStatus.SuspendLayout();
            this.mainMenu.SuspendLayout();
            this.pnPlaceHolder.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbDraw)).BeginInit();
            this.SuspendLayout();
            // 
            // ssStatus
            // 
            this.ssStatus.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.slGraphInfo,
            this.tsVisStatus});
            this.ssStatus.Location = new System.Drawing.Point(0, 364);
            this.ssStatus.Name = "ssStatus";
            this.ssStatus.Size = new System.Drawing.Size(830, 22);
            this.ssStatus.TabIndex = 0;
            this.ssStatus.Text = "statusStrip1";
            // 
            // slGraphInfo
            // 
            this.slGraphInfo.Name = "slGraphInfo";
            this.slGraphInfo.Size = new System.Drawing.Size(60, 17);
            this.slGraphInfo.Text = "GraphInfo";
            // 
            // tsVisStatus
            // 
            this.tsVisStatus.Name = "tsVisStatus";
            this.tsVisStatus.Size = new System.Drawing.Size(63, 17);
            this.tsVisStatus.Text = "tsVisStatus";
            // 
            // mainMenu
            // 
            this.mainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.visualizationToolStripMenuItem,
            this.mnAbout,
            this.mnZoomAll,
            this.mnAlghoritmList,
            this.mnToggleLogWindow,
            this.mnAlgorithmInfo,
            this.mnRunAlghoritm});
            this.mainMenu.Location = new System.Drawing.Point(0, 0);
            this.mainMenu.Name = "mainMenu";
            this.mainMenu.Size = new System.Drawing.Size(830, 27);
            this.mainMenu.TabIndex = 1;
            this.mainMenu.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnCreate,
            this.mnOpen,
            this.importToolStripMenuItem,
            this.mnGenerate,
            this.mnSave,
            this.mnExit,
            this.mnReloadAlgorithms});
            this.fileToolStripMenuItem.Image = global::FlowNetworkToolKit.Properties.Resources.processor;
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F)));
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(53, 23);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // mnCreate
            // 
            this.mnCreate.Image = global::FlowNetworkToolKit.Properties.Resources.plus;
            this.mnCreate.Name = "mnCreate";
            this.mnCreate.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.mnCreate.Size = new System.Drawing.Size(210, 22);
            this.mnCreate.Text = "Create new graph";
            // 
            // mnOpen
            // 
            this.mnOpen.Image = global::FlowNetworkToolKit.Properties.Resources.folder_horizontal_open;
            this.mnOpen.Name = "mnOpen";
            this.mnOpen.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.mnOpen.Size = new System.Drawing.Size(210, 22);
            this.mnOpen.Text = "Open graph...";
            this.mnOpen.Click += new System.EventHandler(this.mnOpen_Click);
            // 
            // importToolStripMenuItem
            // 
            this.importToolStripMenuItem.Image = global::FlowNetworkToolKit.Properties.Resources.receipt_import;
            this.importToolStripMenuItem.Name = "importToolStripMenuItem";
            this.importToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.I)));
            this.importToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
            this.importToolStripMenuItem.Text = "Import";
            this.importToolStripMenuItem.Click += new System.EventHandler(this.importToolStripMenuItem_Click);
            // 
            // mnGenerate
            // 
            this.mnGenerate.Image = global::FlowNetworkToolKit.Properties.Resources.puzzle;
            this.mnGenerate.Name = "mnGenerate";
            this.mnGenerate.Size = new System.Drawing.Size(210, 22);
            this.mnGenerate.Text = "Generate random";
            this.mnGenerate.Click += new System.EventHandler(this.mnGenerate_Click);
            // 
            // mnSave
            // 
            this.mnSave.Image = global::FlowNetworkToolKit.Properties.Resources.disk_return_black;
            this.mnSave.Name = "mnSave";
            this.mnSave.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.mnSave.Size = new System.Drawing.Size(210, 22);
            this.mnSave.Text = "Save graph...";
            this.mnSave.Click += new System.EventHandler(this.mnSave_Click);
            // 
            // mnExit
            // 
            this.mnExit.Image = global::FlowNetworkToolKit.Properties.Resources.door_open_out;
            this.mnExit.Name = "mnExit";
            this.mnExit.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.mnExit.Size = new System.Drawing.Size(210, 22);
            this.mnExit.Text = "Exit";
            this.mnExit.Click += new System.EventHandler(this.mnExit_Click);
            // 
            // mnReloadAlgorithms
            // 
            this.mnReloadAlgorithms.Name = "mnReloadAlgorithms";
            this.mnReloadAlgorithms.Size = new System.Drawing.Size(210, 22);
            // 
            // visualizationToolStripMenuItem
            // 
            this.visualizationToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnVisualisationEnabled,
            this.mnEditionEnabled,
            this.mnCreationEnabled,
            this.mnArrangement});
            this.visualizationToolStripMenuItem.Image = global::FlowNetworkToolKit.Properties.Resources.eye;
            this.visualizationToolStripMenuItem.Name = "visualizationToolStripMenuItem";
            this.visualizationToolStripMenuItem.Size = new System.Drawing.Size(101, 23);
            this.visualizationToolStripMenuItem.Text = "Visualization";
            this.visualizationToolStripMenuItem.Click += new System.EventHandler(this.visualizationToolStripMenuItem_Click);
            // 
            // mnVisualisationEnabled
            // 
            this.mnVisualisationEnabled.Checked = true;
            this.mnVisualisationEnabled.CheckOnClick = true;
            this.mnVisualisationEnabled.CheckState = System.Windows.Forms.CheckState.Checked;
            this.mnVisualisationEnabled.Name = "mnVisualisationEnabled";
            this.mnVisualisationEnabled.Size = new System.Drawing.Size(152, 22);
            this.mnVisualisationEnabled.Text = "Visualization";
            this.mnVisualisationEnabled.CheckStateChanged += new System.EventHandler(this.mnVisualisationEnabled_CheckStateChanged);
            // 
            // mnEditionEnabled
            // 
            this.mnEditionEnabled.Checked = true;
            this.mnEditionEnabled.CheckOnClick = true;
            this.mnEditionEnabled.CheckState = System.Windows.Forms.CheckState.Checked;
            this.mnEditionEnabled.Name = "mnEditionEnabled";
            this.mnEditionEnabled.Size = new System.Drawing.Size(152, 22);
            this.mnEditionEnabled.Text = "Edition";
            this.mnEditionEnabled.CheckStateChanged += new System.EventHandler(this.mnEditionEnabled_CheckStateChanged);
            // 
            // mnCreationEnabled
            // 
            this.mnCreationEnabled.Checked = true;
            this.mnCreationEnabled.CheckOnClick = true;
            this.mnCreationEnabled.CheckState = System.Windows.Forms.CheckState.Checked;
            this.mnCreationEnabled.Name = "mnCreationEnabled";
            this.mnCreationEnabled.Size = new System.Drawing.Size(152, 22);
            this.mnCreationEnabled.Text = "Creation";
            this.mnCreationEnabled.CheckStateChanged += new System.EventHandler(this.mnCreationEnabled_CheckStateChanged);
            // 
            // mnArrangement
            // 
            this.mnArrangement.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.byDistanceFromSourceToolStripMenuItem,
            this.byCircleFormToolStripMenuItem});
            this.mnArrangement.Image = global::FlowNetworkToolKit.Properties.Resources.application_tile;
            this.mnArrangement.Name = "mnArrangement";
            this.mnArrangement.Size = new System.Drawing.Size(152, 22);
            this.mnArrangement.Text = "Arrangement";
            // 
            // byDistanceFromSourceToolStripMenuItem
            // 
            this.byDistanceFromSourceToolStripMenuItem.Image = global::FlowNetworkToolKit.Properties.Resources.ruler__arrow;
            this.byDistanceFromSourceToolStripMenuItem.Name = "byDistanceFromSourceToolStripMenuItem";
            this.byDistanceFromSourceToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.byDistanceFromSourceToolStripMenuItem.Text = "By distance from source";
            this.byDistanceFromSourceToolStripMenuItem.Click += new System.EventHandler(this.byDistanceFromSourceToolStripMenuItem_Click);
            // 
            // byCircleFormToolStripMenuItem
            // 
            this.byCircleFormToolStripMenuItem.Image = global::FlowNetworkToolKit.Properties.Resources.ring;
            this.byCircleFormToolStripMenuItem.Name = "byCircleFormToolStripMenuItem";
            this.byCircleFormToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.byCircleFormToolStripMenuItem.Text = "By circle form";
            this.byCircleFormToolStripMenuItem.Click += new System.EventHandler(this.byCircleFormToolStripMenuItem_Click);
            // 
            // mnAbout
            // 
            this.mnAbout.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.mnAbout.Image = global::FlowNetworkToolKit.Properties.Resources.question;
            this.mnAbout.Name = "mnAbout";
            this.mnAbout.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.A)));
            this.mnAbout.Size = new System.Drawing.Size(68, 23);
            this.mnAbout.Text = "About";
            this.mnAbout.ToolTipText = "About program";
            this.mnAbout.Click += new System.EventHandler(this.mnAbout_Click);
            // 
            // mnZoomAll
            // 
            this.mnZoomAll.AutoToolTip = true;
            this.mnZoomAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.mnZoomAll.Image = global::FlowNetworkToolKit.Properties.Resources.magnifier_zoom_fit;
            this.mnZoomAll.Name = "mnZoomAll";
            this.mnZoomAll.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z)));
            this.mnZoomAll.Size = new System.Drawing.Size(28, 23);
            this.mnZoomAll.Text = "zoomAll";
            this.mnZoomAll.ToolTipText = "Zoom fit";
            this.mnZoomAll.Click += new System.EventHandler(this.mnZoomAll_Click);
            // 
            // mnAlghoritmList
            // 
            this.mnAlghoritmList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.mnAlghoritmList.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.mnAlghoritmList.Name = "mnAlghoritmList";
            this.mnAlghoritmList.Size = new System.Drawing.Size(200, 23);
            this.mnAlghoritmList.SelectedIndexChanged += new System.EventHandler(this.mnAlghoritmList_SelectedIndexChanged);
            // 
            // mnToggleLogWindow
            // 
            this.mnToggleLogWindow.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.mnToggleLogWindow.Image = global::FlowNetworkToolKit.Properties.Resources.system_monitor;
            this.mnToggleLogWindow.Name = "mnToggleLogWindow";
            this.mnToggleLogWindow.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.L)));
            this.mnToggleLogWindow.Size = new System.Drawing.Size(55, 23);
            this.mnToggleLogWindow.Text = "Log";
            this.mnToggleLogWindow.ToolTipText = "Application log";
            this.mnToggleLogWindow.Click += new System.EventHandler(this.mnToggleLogWindow_Click);
            // 
            // mnAlgorithmInfo
            // 
            this.mnAlgorithmInfo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.mnAlgorithmInfo.Image = global::FlowNetworkToolKit.Properties.Resources.information;
            this.mnAlgorithmInfo.Name = "mnAlgorithmInfo";
            this.mnAlgorithmInfo.Size = new System.Drawing.Size(28, 23);
            this.mnAlgorithmInfo.Text = "About algorithm";
            this.mnAlgorithmInfo.ToolTipText = "About algorithm";
            this.mnAlgorithmInfo.Visible = false;
            this.mnAlgorithmInfo.Click += new System.EventHandler(this.mnAlgorithmInfo_Click);
            // 
            // mnRunAlghoritm
            // 
            this.mnRunAlghoritm.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.mnRunAlghoritm.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnRunVisualization,
            this.runWithoutVisualizationToolStripMenuItem,
            this.performanceTestToolStripMenuItem});
            this.mnRunAlghoritm.Image = global::FlowNetworkToolKit.Properties.Resources.control;
            this.mnRunAlghoritm.Name = "mnRunAlghoritm";
            this.mnRunAlghoritm.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
            this.mnRunAlghoritm.Size = new System.Drawing.Size(28, 23);
            this.mnRunAlghoritm.Text = "Run algorithm";
            this.mnRunAlghoritm.ToolTipText = "Run algorithm";
            this.mnRunAlghoritm.Visible = false;
            // 
            // mnRunVisualization
            // 
            this.mnRunVisualization.Image = global::FlowNetworkToolKit.Properties.Resources.eye;
            this.mnRunVisualization.Name = "mnRunVisualization";
            this.mnRunVisualization.Size = new System.Drawing.Size(284, 22);
            this.mnRunVisualization.Text = "Run and visualize";
            // 
            // runWithoutVisualizationToolStripMenuItem
            // 
            this.runWithoutVisualizationToolStripMenuItem.Image = global::FlowNetworkToolKit.Properties.Resources.eye_close;
            this.runWithoutVisualizationToolStripMenuItem.Name = "runWithoutVisualizationToolStripMenuItem";
            this.runWithoutVisualizationToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.W)));
            this.runWithoutVisualizationToolStripMenuItem.Size = new System.Drawing.Size(284, 22);
            this.runWithoutVisualizationToolStripMenuItem.Text = "Run without visualization";
            this.runWithoutVisualizationToolStripMenuItem.Click += new System.EventHandler(this.runWithoutVisualizationToolStripMenuItem_Click);
            // 
            // performanceTestToolStripMenuItem
            // 
            this.performanceTestToolStripMenuItem.Image = global::FlowNetworkToolKit.Properties.Resources.clock_select;
            this.performanceTestToolStripMenuItem.Name = "performanceTestToolStripMenuItem";
            this.performanceTestToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.P)));
            this.performanceTestToolStripMenuItem.Size = new System.Drawing.Size(284, 22);
            this.performanceTestToolStripMenuItem.Text = "Performance test";
            this.performanceTestToolStripMenuItem.Click += new System.EventHandler(this.performanceTestToolStripMenuItem_Click);
            // 
            // dlgOpenFile
            // 
            this.dlgOpenFile.Filter = "FlowNetwork xml|*.xml";
            // 
            // dlgImportFile
            // 
            this.dlgImportFile.Filter = "FlowNetwork|*.csv;*.fn;*.dimacs";
            this.dlgImportFile.FilterIndex = 2;
            // 
            // pnPlaceHolder
            // 
            this.pnPlaceHolder.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.pnPlaceHolder.Controls.Add(this.btnExit);
            this.pnPlaceHolder.Controls.Add(this.btnGenerate);
            this.pnPlaceHolder.Controls.Add(this.btnImport);
            this.pnPlaceHolder.Controls.Add(this.btnOpen);
            this.pnPlaceHolder.Controls.Add(this.btnCreate);
            this.pnPlaceHolder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnPlaceHolder.Location = new System.Drawing.Point(0, 27);
            this.pnPlaceHolder.Name = "pnPlaceHolder";
            this.pnPlaceHolder.Size = new System.Drawing.Size(830, 337);
            this.pnPlaceHolder.TabIndex = 2;
            // 
            // btnExit
            // 
            this.btnExit.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnExit.Image = global::FlowNetworkToolKit.Properties.Resources.door_open_out;
            this.btnExit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnExit.Location = new System.Drawing.Point(0, 92);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(830, 23);
            this.btnExit.TabIndex = 4;
            this.btnExit.Text = "Close Graph ToolKit";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.mnExit_Click);
            // 
            // btnGenerate
            // 
            this.btnGenerate.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnGenerate.Image = global::FlowNetworkToolKit.Properties.Resources.puzzle;
            this.btnGenerate.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnGenerate.Location = new System.Drawing.Point(0, 69);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(830, 23);
            this.btnGenerate.TabIndex = 3;
            this.btnGenerate.Text = "Generate random graph";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.mnGenerate_Click);
            // 
            // btnImport
            // 
            this.btnImport.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnImport.Image = global::FlowNetworkToolKit.Properties.Resources.receipt_import;
            this.btnImport.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnImport.Location = new System.Drawing.Point(0, 46);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(830, 23);
            this.btnImport.TabIndex = 2;
            this.btnImport.Text = "Import graph";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.importToolStripMenuItem_Click);
            // 
            // btnOpen
            // 
            this.btnOpen.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnOpen.Image = global::FlowNetworkToolKit.Properties.Resources.folder_horizontal_open;
            this.btnOpen.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOpen.Location = new System.Drawing.Point(0, 23);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(830, 23);
            this.btnOpen.TabIndex = 1;
            this.btnOpen.Text = "Open graph";
            this.btnOpen.UseVisualStyleBackColor = true;
            this.btnOpen.Click += new System.EventHandler(this.mnOpen_Click);
            // 
            // btnCreate
            // 
            this.btnCreate.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnCreate.Image = global::FlowNetworkToolKit.Properties.Resources.plus;
            this.btnCreate.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCreate.Location = new System.Drawing.Point(0, 0);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(830, 23);
            this.btnCreate.TabIndex = 0;
            this.btnCreate.Text = "Create new graph";
            this.btnCreate.UseVisualStyleBackColor = true;
            // 
            // dlgSaveFile
            // 
            this.dlgSaveFile.DefaultExt = "xml";
            this.dlgSaveFile.Filter = "FlowNetwork xml|*.xml";
            // 
            // pbDraw
            // 
            this.pbDraw.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbDraw.Location = new System.Drawing.Point(0, 27);
            this.pbDraw.Name = "pbDraw";
            this.pbDraw.Size = new System.Drawing.Size(830, 337);
            this.pbDraw.TabIndex = 3;
            this.pbDraw.TabStop = false;
            this.pbDraw.Paint += new System.Windows.Forms.PaintEventHandler(this.canvas_Paint);
            this.pbDraw.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pbDraw_MouseMove);
            // 
            // FMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(830, 386);
            this.Controls.Add(this.pnPlaceHolder);
            this.Controls.Add(this.pbDraw);
            this.Controls.Add(this.ssStatus);
            this.Controls.Add(this.mainMenu);
            this.DoubleBuffered = true;
            this.MainMenuStrip = this.mainMenu;
            this.Name = "FMain";
            this.Text = "Graph ToolKit";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.FMain_Paint);
            this.Resize += new System.EventHandler(this.FMain_Resize);
            this.ssStatus.ResumeLayout(false);
            this.ssStatus.PerformLayout();
            this.mainMenu.ResumeLayout(false);
            this.mainMenu.PerformLayout();
            this.pnPlaceHolder.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbDraw)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip ssStatus;
        private System.Windows.Forms.MenuStrip mainMenu;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem visualizationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mnAbout;
        private System.Windows.Forms.ToolStripMenuItem mnCreate;
        private System.Windows.Forms.ToolStripMenuItem mnOpen;
        private System.Windows.Forms.ToolStripMenuItem mnSave;
        private System.Windows.Forms.ToolStripMenuItem mnExit;
        private System.Windows.Forms.ToolStripMenuItem mnReloadAlgorithms;
        private System.Windows.Forms.ToolStripComboBox mnAlghoritmList;
        private System.Windows.Forms.ToolStripMenuItem mnToggleLogWindow;
        private System.Windows.Forms.ToolStripMenuItem mnAlgorithmInfo;
        private System.Windows.Forms.ToolStripMenuItem mnRunAlghoritm;
        private System.Windows.Forms.ToolStripMenuItem mnRunVisualization;
        private System.Windows.Forms.ToolStripMenuItem runWithoutVisualizationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem performanceTestToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mnGenerate;
        private System.Windows.Forms.ToolStripStatusLabel slGraphInfo;
        private System.Windows.Forms.OpenFileDialog dlgOpenFile;
        private System.Windows.Forms.OpenFileDialog dlgImportFile;
        private System.Windows.Forms.Button btnCreate;
        private System.Windows.Forms.Button btnOpen;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Panel pnPlaceHolder;
        private PictureBox pbDraw;
        private ToolStripMenuItem mnZoomAll;
        private ToolStripStatusLabel tsVisStatus;
        private SaveFileDialog dlgSaveFile;
        private ToolStripMenuItem mnVisualisationEnabled;
        private ToolStripMenuItem mnEditionEnabled;
        private ToolStripMenuItem mnCreationEnabled;
        private ToolStripMenuItem mnArrangement;
        private ToolStripMenuItem byDistanceFromSourceToolStripMenuItem;
        private ToolStripMenuItem byCircleFormToolStripMenuItem;
    }
}

