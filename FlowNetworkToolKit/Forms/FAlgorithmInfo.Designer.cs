namespace FlowNetworkToolKit.Forms
{
    partial class FAlgorithmInfo
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
            this.btnClose = new System.Windows.Forms.Button();
            this.lAlgorithmName = new System.Windows.Forms.Label();
            this.llAlgorithmUrl = new System.Windows.Forms.LinkLabel();
            this.rtAlgorithmDescription = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnClose.Location = new System.Drawing.Point(0, 132);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(449, 23);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // lAlgorithmName
            // 
            this.lAlgorithmName.Dock = System.Windows.Forms.DockStyle.Top;
            this.lAlgorithmName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lAlgorithmName.Location = new System.Drawing.Point(0, 0);
            this.lAlgorithmName.Name = "lAlgorithmName";
            this.lAlgorithmName.Size = new System.Drawing.Size(449, 27);
            this.lAlgorithmName.TabIndex = 4;
            this.lAlgorithmName.Text = "Algorithm name";
            this.lAlgorithmName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // llAlgorithmUrl
            // 
            this.llAlgorithmUrl.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.llAlgorithmUrl.Location = new System.Drawing.Point(0, 109);
            this.llAlgorithmUrl.Name = "llAlgorithmUrl";
            this.llAlgorithmUrl.Size = new System.Drawing.Size(449, 23);
            this.llAlgorithmUrl.TabIndex = 5;
            this.llAlgorithmUrl.TabStop = true;
            this.llAlgorithmUrl.Text = "Algorithm url";
            this.llAlgorithmUrl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.llAlgorithmUrl.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llAlgorithmUrl_LinkClicked);
            // 
            // rtAlgorithmDescription
            // 
            this.rtAlgorithmDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtAlgorithmDescription.Location = new System.Drawing.Point(0, 27);
            this.rtAlgorithmDescription.Name = "rtAlgorithmDescription";
            this.rtAlgorithmDescription.ReadOnly = true;
            this.rtAlgorithmDescription.Size = new System.Drawing.Size(449, 82);
            this.rtAlgorithmDescription.TabIndex = 6;
            this.rtAlgorithmDescription.Text = "";
            // 
            // FAlgorithmInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(449, 155);
            this.Controls.Add(this.rtAlgorithmDescription);
            this.Controls.Add(this.llAlgorithmUrl);
            this.Controls.Add(this.lAlgorithmName);
            this.Controls.Add(this.btnClose);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FAlgorithmInfo";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "About algorithm";
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lAlgorithmName;
        private System.Windows.Forms.LinkLabel llAlgorithmUrl;
        private System.Windows.Forms.RichTextBox rtAlgorithmDescription;
    }
}