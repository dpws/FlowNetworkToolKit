namespace FlowNetworkToolKit.Forms
{
    partial class FLog
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
            this.pnControls = new System.Windows.Forms.Panel();
            this.logVisualizer = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // pnControls
            // 
            this.pnControls.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnControls.Location = new System.Drawing.Point(0, 387);
            this.pnControls.Name = "pnControls";
            this.pnControls.Size = new System.Drawing.Size(871, 57);
            this.pnControls.TabIndex = 1;
            // 
            // logVisualizer
            // 
            this.logVisualizer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logVisualizer.Location = new System.Drawing.Point(0, 0);
            this.logVisualizer.Name = "logVisualizer";
            this.logVisualizer.Size = new System.Drawing.Size(871, 387);
            this.logVisualizer.TabIndex = 2;
            this.logVisualizer.Text = "";
            // 
            // FLog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(871, 444);
            this.Controls.Add(this.logVisualizer);
            this.Controls.Add(this.pnControls);
            this.Name = "FLog";
            this.Text = "Application log";
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel pnControls;
        public System.Windows.Forms.RichTextBox logVisualizer;
    }
}