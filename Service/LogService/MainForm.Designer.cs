namespace LogService
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.textBoxLogWriter1 = new System.TextBoxLogWriter();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.grid = new System.Windows.Forms.DataGridView();
            this.colGameID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colKey = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colActive = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colLoaded = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colCurrentTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLock = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMinLen = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMaxLen = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colReserved = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colState = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.btnInstallService = new System.Windows.Forms.Button();
            this.btnUninstallService = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.cmdMsg = new System.Windows.Forms.ToolStripButton();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
            this.tabPage2.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
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
            this.textBoxLogWriter1.Size = new System.Drawing.Size(784, 275);
            this.textBoxLogWriter1.TabIndex = 2;
            this.textBoxLogWriter1.WordWrap = false;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(784, 261);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.grid);
            this.tabPage1.Controls.Add(this.propertyGrid1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(776, 235);
            this.tabPage1.TabIndex = 2;
            this.tabPage1.Text = "Working Items";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // grid
            // 
            this.grid.AllowUserToAddRows = false;
            this.grid.AllowUserToDeleteRows = false;
            this.grid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.grid.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colGameID,
            this.colKey,
            this.colActive,
            this.colLoaded,
            this.colCurrentTime,
            this.colLock,
            this.colTime,
            this.colMinLen,
            this.colMaxLen,
            this.colReserved,
            this.colState});
            this.grid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grid.Location = new System.Drawing.Point(3, 3);
            this.grid.Name = "grid";
            this.grid.RowTemplate.Height = 24;
            this.grid.Size = new System.Drawing.Size(770, 229);
            this.grid.TabIndex = 1;
            this.grid.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grid_CellContentClick);
            this.grid.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.grid_CellEndEdit);
            this.grid.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.grid_CellValueChanged);
            // 
            // colGameID
            // 
            this.colGameID.HeaderText = "GameID";
            this.colGameID.Name = "colGameID";
            this.colGameID.ReadOnly = true;
            this.colGameID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colGameID.Width = 47;
            // 
            // colKey
            // 
            this.colKey.HeaderText = "Key";
            this.colKey.Name = "colKey";
            this.colKey.ReadOnly = true;
            this.colKey.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colKey.Width = 29;
            // 
            // colActive
            // 
            this.colActive.HeaderText = "Active";
            this.colActive.Name = "colActive";
            this.colActive.Width = 47;
            // 
            // colLoaded
            // 
            this.colLoaded.HeaderText = "load";
            this.colLoaded.Name = "colLoaded";
            this.colLoaded.ReadOnly = true;
            this.colLoaded.Width = 35;
            // 
            // colCurrentTime
            // 
            dataGridViewCellStyle1.Format = "yyyy/MM/dd HH:mm:ss";
            this.colCurrentTime.DefaultCellStyle = dataGridViewCellStyle1;
            this.colCurrentTime.HeaderText = "CurrentTime";
            this.colCurrentTime.Name = "colCurrentTime";
            this.colCurrentTime.ReadOnly = true;
            this.colCurrentTime.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colCurrentTime.Visible = false;
            this.colCurrentTime.Width = 77;
            // 
            // colLock
            // 
            this.colLock.HeaderText = "lock";
            this.colLock.Name = "colLock";
            this.colLock.ReadOnly = true;
            this.colLock.Width = 35;
            // 
            // colTime
            // 
            this.colTime.HeaderText = "Time";
            this.colTime.Name = "colTime";
            this.colTime.ReadOnly = true;
            this.colTime.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colTime.Width = 35;
            // 
            // colMinLen
            // 
            this.colMinLen.HeaderText = "MinLen";
            this.colMinLen.Name = "colMinLen";
            this.colMinLen.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colMinLen.Width = 47;
            // 
            // colMaxLen
            // 
            this.colMaxLen.HeaderText = "MaxLen";
            this.colMaxLen.Name = "colMaxLen";
            this.colMaxLen.Width = 66;
            // 
            // colReserved
            // 
            this.colReserved.HeaderText = "Reserved";
            this.colReserved.Name = "colReserved";
            this.colReserved.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colReserved.Width = 59;
            // 
            // colState
            // 
            this.colState.HeaderText = "State";
            this.colState.Name = "colState";
            this.colState.ReadOnly = true;
            this.colState.Width = 60;
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.HelpVisible = false;
            this.propertyGrid1.Location = new System.Drawing.Point(445, 111);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.PropertySort = System.Windows.Forms.PropertySort.NoSort;
            this.propertyGrid1.Size = new System.Drawing.Size(328, 141);
            this.propertyGrid1.TabIndex = 0;
            this.propertyGrid1.ToolbarVisible = false;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.btnInstallService);
            this.tabPage2.Controls.Add(this.btnUninstallService);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(664, 185);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Config";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // btnInstallService
            // 
            this.btnInstallService.Location = new System.Drawing.Point(8, 6);
            this.btnInstallService.Name = "btnInstallService";
            this.btnInstallService.Size = new System.Drawing.Size(150, 23);
            this.btnInstallService.TabIndex = 0;
            this.btnInstallService.Text = "Install Service";
            this.btnInstallService.UseVisualStyleBackColor = true;
            this.btnInstallService.Click += new System.EventHandler(this.InstallService);
            // 
            // btnUninstallService
            // 
            this.btnUninstallService.Location = new System.Drawing.Point(8, 35);
            this.btnUninstallService.Name = "btnUninstallService";
            this.btnUninstallService.Size = new System.Drawing.Size(150, 23);
            this.btnUninstallService.TabIndex = 1;
            this.btnUninstallService.Text = "Uninstall Service";
            this.btnUninstallService.UseVisualStyleBackColor = true;
            this.btnUninstallService.Click += new System.EventHandler(this.InstallService);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.Text = "notifyIcon1";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.Click += new System.EventHandler(this.notifyIcon1_Click);
            this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel2,
            this.cmdMsg});
            this.statusStrip1.Location = new System.Drawing.Point(0, 540);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(784, 22);
            this.statusStrip1.TabIndex = 4;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(672, 17);
            this.toolStripStatusLabel2.Spring = true;
            // 
            // cmdMsg
            // 
            this.cmdMsg.Checked = true;
            this.cmdMsg.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cmdMsg.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.cmdMsg.Name = "cmdMsg";
            this.cmdMsg.Size = new System.Drawing.Size(97, 20);
            this.cmdMsg.Text = "Message Panel";
            this.cmdMsg.Click += new System.EventHandler(this.cmdMsg_Click);
            // 
            // splitContainer2
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer2";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tabControl1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.textBoxLogWriter1);
            this.splitContainer1.Size = new System.Drawing.Size(784, 540);
            this.splitContainer1.SplitterDistance = 261;
            this.splitContainer1.TabIndex = 5;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 562);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.statusStrip1);
            this.Font = new System.Drawing.Font("細明體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "LogService";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.TextBoxLogWriter textBoxLogWriter1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button btnInstallService;
        private System.Windows.Forms.Button btnUninstallService;
        private System.Windows.Forms.PropertyGrid propertyGrid1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.DataGridView grid;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripButton cmdMsg;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGameID;
        private System.Windows.Forms.DataGridViewTextBoxColumn colKey;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colActive;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colLoaded;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCurrentTime;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colLock;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMinLen;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMaxLen;
        private System.Windows.Forms.DataGridViewTextBoxColumn colReserved;
        private System.Windows.Forms.DataGridViewTextBoxColumn colState;
        private System.Windows.Forms.SplitContainer splitContainer1;
    }
}

