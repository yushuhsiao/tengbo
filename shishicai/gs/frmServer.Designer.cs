namespace shishicai
{
    partial class frmServer
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
            this.textBoxLogWriter1 = new System.TextBoxLogWriter();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.textBoxLogWriter1);
            this.splitContainer1.Size = new System.Drawing.Size(784, 562);
            this.splitContainer1.SplitterDistance = 261;
            this.splitContainer1.TabIndex = 0;
            // 
            // textBoxLogWriter1
            // 
            this.textBoxLogWriter1.All = false;
            this.textBoxLogWriter1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxLogWriter1.Groups = new int[0];
            this.textBoxLogWriter1.Interval = 100;
            this.textBoxLogWriter1.Location = new System.Drawing.Point(0, 0);
            this.textBoxLogWriter1.Multiline = true;
            this.textBoxLogWriter1.Name = "textBoxLogWriter1";
            this.textBoxLogWriter1.ReadOnly = true;
            this.textBoxLogWriter1.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxLogWriter1.Size = new System.Drawing.Size(784, 297);
            this.textBoxLogWriter1.TabIndex = 0;
            this.textBoxLogWriter1.WordWrap = false;
            // 
            // frmServer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 562);
            this.Controls.Add(this.splitContainer1);
            this.Name = "frmServer";
            this.Text = "frmServer";
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.TextBoxLogWriter textBoxLogWriter1;
    }
}