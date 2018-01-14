namespace FlowNetworkToolKit.Forms
{
    partial class FPerformanceTest
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.cblAlgorithms = new System.Windows.Forms.CheckedListBox();
            this.dgTestResults = new System.Windows.Forms.DataGridView();
            this.algorithm = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.maxflow = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.runs = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.mintime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.maxtime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.avgtime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnRun = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.udRunsCount = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgTestResults)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.udRunsCount)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitContainer1.Location = new System.Drawing.Point(0, 53);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.cblAlgorithms);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.dgTestResults);
            this.splitContainer1.Size = new System.Drawing.Size(989, 435);
            this.splitContainer1.SplitterDistance = 172;
            this.splitContainer1.TabIndex = 99;
            // 
            // cblAlgorithms
            // 
            this.cblAlgorithms.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cblAlgorithms.FormattingEnabled = true;
            this.cblAlgorithms.Location = new System.Drawing.Point(0, 0);
            this.cblAlgorithms.Name = "cblAlgorithms";
            this.cblAlgorithms.Size = new System.Drawing.Size(172, 435);
            this.cblAlgorithms.TabIndex = 0;
            // 
            // dgTestResults
            // 
            this.dgTestResults.AllowUserToAddRows = false;
            this.dgTestResults.AllowUserToDeleteRows = false;
            this.dgTestResults.AllowUserToOrderColumns = true;
            this.dgTestResults.AllowUserToResizeRows = false;
            this.dgTestResults.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgTestResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgTestResults.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.algorithm,
            this.maxflow,
            this.runs,
            this.mintime,
            this.maxtime,
            this.avgtime});
            this.dgTestResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgTestResults.Location = new System.Drawing.Point(0, 0);
            this.dgTestResults.Name = "dgTestResults";
            this.dgTestResults.ReadOnly = true;
            this.dgTestResults.RowHeadersVisible = false;
            this.dgTestResults.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgTestResults.Size = new System.Drawing.Size(813, 435);
            this.dgTestResults.TabIndex = 0;
            // 
            // algorithm
            // 
            this.algorithm.HeaderText = "Algorithm";
            this.algorithm.Name = "algorithm";
            this.algorithm.ReadOnly = true;
            // 
            // maxflow
            // 
            this.maxflow.HeaderText = "Max Flow";
            this.maxflow.Name = "maxflow";
            this.maxflow.ReadOnly = true;
            // 
            // runs
            // 
            this.runs.HeaderText = "Runs";
            this.runs.Name = "runs";
            this.runs.ReadOnly = true;
            // 
            // mintime
            // 
            this.mintime.HeaderText = "Minimum time";
            this.mintime.Name = "mintime";
            this.mintime.ReadOnly = true;
            // 
            // maxtime
            // 
            this.maxtime.HeaderText = "Maximum time";
            this.maxtime.Name = "maxtime";
            this.maxtime.ReadOnly = true;
            // 
            // avgtime
            // 
            this.avgtime.HeaderText = "Average time";
            this.avgtime.Name = "avgtime";
            this.avgtime.ReadOnly = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnRun);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.udRunsCount);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(989, 47);
            this.panel1.TabIndex = 7;
            // 
            // btnRun
            // 
            this.btnRun.Image = global::FlowNetworkToolKit.Properties.Resources.control_skip;
            this.btnRun.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRun.Location = new System.Drawing.Point(864, 12);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(113, 23);
            this.btnRun.TabIndex = 9;
            this.btnRun.Text = "Run tests";
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Runs count";
            // 
            // udRunsCount
            // 
            this.udRunsCount.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.udRunsCount.Location = new System.Drawing.Point(80, 15);
            this.udRunsCount.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.udRunsCount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.udRunsCount.Name = "udRunsCount";
            this.udRunsCount.Size = new System.Drawing.Size(119, 20);
            this.udRunsCount.TabIndex = 7;
            this.udRunsCount.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // FPerformanceTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(989, 488);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.splitContainer1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FPerformanceTest";
            this.Text = "PerformanceTest";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgTestResults)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.udRunsCount)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.CheckedListBox cblAlgorithms;
        private System.Windows.Forms.DataGridView dgTestResults;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown udRunsCount;
        private System.Windows.Forms.DataGridViewTextBoxColumn algorithm;
        private System.Windows.Forms.DataGridViewTextBoxColumn maxflow;
        private System.Windows.Forms.DataGridViewTextBoxColumn runs;
        private System.Windows.Forms.DataGridViewTextBoxColumn mintime;
        private System.Windows.Forms.DataGridViewTextBoxColumn maxtime;
        private System.Windows.Forms.DataGridViewTextBoxColumn avgtime;
    }
}