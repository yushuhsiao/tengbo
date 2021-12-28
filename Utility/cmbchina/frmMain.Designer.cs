namespace cmbchina
{
    partial class frmMain
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置 Managed 資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器
        /// 修改這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.wb = new cmbchina.frmMain.WebBrowser2();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.view = new System.Windows.Forms.DataGridView();
            this.col_index = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_Time = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_out = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_in = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_bal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_type = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_memo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_srcUrl = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_Status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnConfigReload = new System.Windows.Forms.Button();
            this.btnConfigSave = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.textBoxLogWriter1 = new System.TextBoxLogWriter();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.txtStatusText = new System.Windows.Forms.ToolStripStatusLabel();
            this.loading1 = new System.Windows.Forms.ToolStripProgressBar();
            this.loading2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.txtUpdateTime = new System.Windows.Forms.ToolStripStatusLabel();
            this.txtState = new System.Windows.Forms.ToolStripStatusLabel();
            this.cmdMsg = new System.Windows.Forms.ToolStripButton();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.txtEncryptionLevel = new System.Windows.Forms.ToolStripLabel();
            this.txtTickStatus = new System.Windows.Forms.ToolStripLabel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.view)).BeginInit();
            this.tabPage3.SuspendLayout();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            resources.ApplyResources(this.tabControl1, "tabControl1");
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.wb);
            resources.ApplyResources(this.tabPage1, "tabPage1");
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // wb
            // 
            resources.ApplyResources(this.wb, "wb");
            this.wb.MinimumSize = new System.Drawing.Size(20, 20);
            this.wb.Name = "wb";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.view);
            resources.ApplyResources(this.tabPage2, "tabPage2");
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // view
            // 
            this.view.AllowUserToAddRows = false;
            this.view.AllowUserToDeleteRows = false;
            this.view.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.view.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.view.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.view.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.col_index,
            this.col_Time,
            this.col_out,
            this.col_in,
            this.col_bal,
            this.col_type,
            this.col_memo,
            this.col_name,
            this.col_srcUrl,
            this.col_Status});
            resources.ApplyResources(this.view, "view");
            this.view.Name = "view";
            this.view.ReadOnly = true;
            this.view.RowHeadersVisible = false;
            this.view.RowTemplate.Height = 24;
            // 
            // col_index
            // 
            resources.ApplyResources(this.col_index, "col_index");
            this.col_index.Name = "col_index";
            this.col_index.ReadOnly = true;
            // 
            // col_Time
            // 
            dataGridViewCellStyle1.Format = "yyyy-MM-dd";
            this.col_Time.DefaultCellStyle = dataGridViewCellStyle1;
            resources.ApplyResources(this.col_Time, "col_Time");
            this.col_Time.Name = "col_Time";
            this.col_Time.ReadOnly = true;
            // 
            // col_out
            // 
            resources.ApplyResources(this.col_out, "col_out");
            this.col_out.Name = "col_out";
            this.col_out.ReadOnly = true;
            // 
            // col_in
            // 
            resources.ApplyResources(this.col_in, "col_in");
            this.col_in.Name = "col_in";
            this.col_in.ReadOnly = true;
            // 
            // col_bal
            // 
            resources.ApplyResources(this.col_bal, "col_bal");
            this.col_bal.Name = "col_bal";
            this.col_bal.ReadOnly = true;
            // 
            // col_type
            // 
            resources.ApplyResources(this.col_type, "col_type");
            this.col_type.Name = "col_type";
            this.col_type.ReadOnly = true;
            // 
            // col_memo
            // 
            resources.ApplyResources(this.col_memo, "col_memo");
            this.col_memo.Name = "col_memo";
            this.col_memo.ReadOnly = true;
            // 
            // col_name
            // 
            resources.ApplyResources(this.col_name, "col_name");
            this.col_name.Name = "col_name";
            this.col_name.ReadOnly = true;
            // 
            // col_srcUrl
            // 
            resources.ApplyResources(this.col_srcUrl, "col_srcUrl");
            this.col_srcUrl.Name = "col_srcUrl";
            this.col_srcUrl.ReadOnly = true;
            // 
            // col_Status
            // 
            resources.ApplyResources(this.col_Status, "col_Status");
            this.col_Status.Name = "col_Status";
            this.col_Status.ReadOnly = true;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.splitContainer2);
            resources.ApplyResources(this.tabPage3, "tabPage3");
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // splitContainer2
            // 
            resources.ApplyResources(this.splitContainer2, "splitContainer2");
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.propertyGrid1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.tableLayoutPanel1);
            // 
            // propertyGrid1
            // 
            resources.ApplyResources(this.propertyGrid1, "propertyGrid1");
            this.propertyGrid1.Name = "propertyGrid1";
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.btnConfigReload, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnConfigSave, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.checkBox1, 3, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // btnConfigReload
            // 
            resources.ApplyResources(this.btnConfigReload, "btnConfigReload");
            this.btnConfigReload.Name = "btnConfigReload";
            this.btnConfigReload.UseVisualStyleBackColor = true;
            this.btnConfigReload.Click += new System.EventHandler(this.btnConfigReload_Click);
            // 
            // btnConfigSave
            // 
            resources.ApplyResources(this.btnConfigSave, "btnConfigSave");
            this.btnConfigSave.Name = "btnConfigSave";
            this.btnConfigSave.UseVisualStyleBackColor = true;
            this.btnConfigSave.Click += new System.EventHandler(this.btnConfigSave_Click);
            // 
            // checkBox1
            // 
            resources.ApplyResources(this.checkBox1, "checkBox1");
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // textBoxLogWriter1
            // 
            this.textBoxLogWriter1.All = false;
            resources.ApplyResources(this.textBoxLogWriter1, "textBoxLogWriter1");
            this.textBoxLogWriter1.Groups = new int[0];
            this.textBoxLogWriter1.Interval = 100;
            this.textBoxLogWriter1.Name = "textBoxLogWriter1";
            this.textBoxLogWriter1.ReadOnly = true;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.txtStatusText,
            this.loading1,
            this.loading2,
            this.toolStripStatusLabel1,
            this.txtUpdateTime,
            this.txtState,
            this.cmdMsg});
            resources.ApplyResources(this.statusStrip1, "statusStrip1");
            this.statusStrip1.Name = "statusStrip1";
            // 
            // txtStatusText
            // 
            this.txtStatusText.Name = "txtStatusText";
            resources.ApplyResources(this.txtStatusText, "txtStatusText");
            // 
            // loading1
            // 
            this.loading1.Name = "loading1";
            resources.ApplyResources(this.loading1, "loading1");
            // 
            // loading2
            // 
            this.loading2.Name = "loading2";
            resources.ApplyResources(this.loading2, "loading2");
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            resources.ApplyResources(this.toolStripStatusLabel1, "toolStripStatusLabel1");
            this.toolStripStatusLabel1.Spring = true;
            // 
            // txtUpdateTime
            // 
            this.txtUpdateTime.Name = "txtUpdateTime";
            resources.ApplyResources(this.txtUpdateTime, "txtUpdateTime");
            // 
            // txtState
            // 
            this.txtState.Name = "txtState";
            resources.ApplyResources(this.txtState, "txtState");
            // 
            // cmdMsg
            // 
            this.cmdMsg.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            resources.ApplyResources(this.cmdMsg, "cmdMsg");
            this.cmdMsg.Name = "cmdMsg";
            this.cmdMsg.Click += new System.EventHandler(this.cmdMsg_Click);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.Timer_Tick);
            // 
            // notifyIcon1
            // 
            resources.ApplyResources(this.notifyIcon1, "notifyIcon1");
            this.notifyIcon1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseClick);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.txtEncryptionLevel,
            this.txtTickStatus});
            resources.ApplyResources(this.toolStrip1, "toolStrip1");
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            // 
            // txtEncryptionLevel
            // 
            this.txtEncryptionLevel.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.txtEncryptionLevel.Name = "txtEncryptionLevel";
            resources.ApplyResources(this.txtEncryptionLevel, "txtEncryptionLevel");
            // 
            // txtTickStatus
            // 
            this.txtTickStatus.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.txtTickStatus.Name = "txtTickStatus";
            resources.ApplyResources(this.txtTickStatus, "txtTickStatus");
            // 
            // splitContainer1
            // 
            resources.ApplyResources(this.splitContainer1, "splitContainer1");
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tabControl1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.textBoxLogWriter1);
            // 
            // frmMain
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.statusStrip1);
            this.Name = "frmMain";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.view)).EndInit();
            this.tabPage3.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripButton cmdMsg;
        private System.TextBoxLogWriter textBoxLogWriter1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private WebBrowser2 wb;
        private System.Windows.Forms.ToolStripProgressBar loading1;
        private System.Windows.Forms.ToolStripStatusLabel loading2;
        private System.Windows.Forms.ToolStripStatusLabel txtStatusText;
        private System.Windows.Forms.DataGridView view;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ToolStripStatusLabel txtUpdateTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_index;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_Time;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_out;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_in;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_bal;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_type;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_memo;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_name;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_srcUrl;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.PropertyGrid propertyGrid1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button btnConfigReload;
        private System.Windows.Forms.Button btnConfigSave;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_Status;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.ToolStripStatusLabel txtState;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ToolStripLabel txtEncryptionLevel;
        private System.Windows.Forms.ToolStripLabel txtTickStatus;

    }
}

