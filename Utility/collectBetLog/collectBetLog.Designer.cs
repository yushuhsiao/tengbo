namespace collecReportLog
{
    partial class collectReportLog
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(collectReportLog));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsbCollect = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbDataToServer = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbRefresh = new System.Windows.Forms.ToolStripButton();
            this.tsbExit = new System.Windows.Forms.ToolStripButton();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsp_WebProcress = new System.Windows.Forms.ToolStripProgressBar();
            this.tabReportPage = new System.Windows.Forms.TabControl();
            this.tabPage_BBIN = new System.Windows.Forms.TabPage();
            this.tabPage_AG = new System.Windows.Forms.TabPage();
            this.tabPage_HG = new System.Windows.Forms.TabPage();
            this.tabPage_SunCity = new System.Windows.Forms.TabPage();
            this.tabPage_Upload = new System.Windows.Forms.TabPage();
            this.dgvBetAmtDG = new System.Windows.Forms.DataGridView();
            this.ACTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ACNT = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.GameID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.GameType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BetAmount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BetAmountAct = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Payout = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.isUpdateLoad = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.json = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tab_SetInfo = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.configGrid = new System.Windows.Forms.DataGridView();
            this.colConfigKey = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colConfigValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnConfigSave = new System.Windows.Forms.Button();
            this.btnConfigReload = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.tsbClear = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.collect_BBIN = new collecReportLog.collect_BBIN();
            this.collect_AG = new collecReportLog.collect_AG();
            this.collect_HG = new collecReportLog.collect_HG();
            this.collect_SunCity = new collecReportLog.collect_SunCity();
            this.toolStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.tabReportPage.SuspendLayout();
            this.tabPage_BBIN.SuspendLayout();
            this.tabPage_AG.SuspendLayout();
            this.tabPage_HG.SuspendLayout();
            this.tabPage_SunCity.SuspendLayout();
            this.tabPage_Upload.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBetAmtDG)).BeginInit();
            this.tab_SetInfo.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.configGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbCollect,
            this.toolStripSeparator1,
            this.tsbDataToServer,
            this.toolStripSeparator2,
            this.tsbRefresh,
            this.toolStripSeparator3,
            this.tsbClear,
            this.toolStripSeparator4,
            this.tsbExit});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip1.Size = new System.Drawing.Size(985, 25);
            this.toolStrip1.TabIndex = 16;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tsbCollect
            // 
            this.tsbCollect.Image = ((System.Drawing.Image)(resources.GetObject("tsbCollect.Image")));
            this.tsbCollect.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbCollect.Name = "tsbCollect";
            this.tsbCollect.Size = new System.Drawing.Size(60, 22);
            this.tsbCollect.Text = "采  集";
            this.tsbCollect.Click += new System.EventHandler(this.btn_Collect_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbDataToServer
            // 
            this.tsbDataToServer.Image = ((System.Drawing.Image)(resources.GetObject("tsbDataToServer.Image")));
            this.tsbDataToServer.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbDataToServer.Name = "tsbDataToServer";
            this.tsbDataToServer.Size = new System.Drawing.Size(88, 22);
            this.tsbDataToServer.Text = "上传服务器";
            this.tsbDataToServer.Click += new System.EventHandler(this.tsbDataToServer_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbRefresh
            // 
            this.tsbRefresh.Image = ((System.Drawing.Image)(resources.GetObject("tsbRefresh.Image")));
            this.tsbRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbRefresh.Name = "tsbRefresh";
            this.tsbRefresh.Size = new System.Drawing.Size(52, 22);
            this.tsbRefresh.Text = "刷新";
            this.tsbRefresh.Click += new System.EventHandler(this.tsbRefresh_Click);
            // 
            // tsbExit
            // 
            this.tsbExit.Image = ((System.Drawing.Image)(resources.GetObject("tsbExit.Image")));
            this.tsbExit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbExit.Name = "tsbExit";
            this.tsbExit.Size = new System.Drawing.Size(52, 22);
            this.tsbExit.Text = "退出";
            this.tsbExit.Click += new System.EventHandler(this.tsbExit_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.tsp_WebProcress});
            this.statusStrip1.Location = new System.Drawing.Point(0, 560);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(985, 22);
            this.statusStrip1.TabIndex = 17;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 17);
            // 
            // tsp_WebProcress
            // 
            this.tsp_WebProcress.Name = "tsp_WebProcress";
            this.tsp_WebProcress.Size = new System.Drawing.Size(300, 16);
            // 
            // tabReportPage
            // 
            this.tabReportPage.Controls.Add(this.tabPage_BBIN);
            this.tabReportPage.Controls.Add(this.tabPage_AG);
            this.tabReportPage.Controls.Add(this.tabPage_HG);
            this.tabReportPage.Controls.Add(this.tabPage_SunCity);
            this.tabReportPage.Controls.Add(this.tabPage_Upload);
            this.tabReportPage.Controls.Add(this.tab_SetInfo);
            this.tabReportPage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabReportPage.Location = new System.Drawing.Point(0, 25);
            this.tabReportPage.Name = "tabReportPage";
            this.tabReportPage.SelectedIndex = 0;
            this.tabReportPage.Size = new System.Drawing.Size(985, 535);
            this.tabReportPage.TabIndex = 18;
            this.tabReportPage.Selected += new System.Windows.Forms.TabControlEventHandler(this.tabReportPage_Selected);
            // 
            // tabPage_BBIN
            // 
            this.tabPage_BBIN.Controls.Add(this.collect_BBIN);
            this.tabPage_BBIN.Location = new System.Drawing.Point(4, 22);
            this.tabPage_BBIN.Name = "tabPage_BBIN";
            this.tabPage_BBIN.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_BBIN.Size = new System.Drawing.Size(977, 509);
            this.tabPage_BBIN.TabIndex = 3;
            this.tabPage_BBIN.Text = "BBIN";
            this.tabPage_BBIN.UseVisualStyleBackColor = true;
            // 
            // tabPage_AG
            // 
            this.tabPage_AG.Controls.Add(this.collect_AG);
            this.tabPage_AG.Location = new System.Drawing.Point(4, 22);
            this.tabPage_AG.Name = "tabPage_AG";
            this.tabPage_AG.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_AG.Size = new System.Drawing.Size(977, 509);
            this.tabPage_AG.TabIndex = 2;
            this.tabPage_AG.Text = "AG";
            this.tabPage_AG.UseVisualStyleBackColor = true;
            // 
            // tabPage_HG
            // 
            this.tabPage_HG.Controls.Add(this.collect_HG);
            this.tabPage_HG.Location = new System.Drawing.Point(4, 22);
            this.tabPage_HG.Name = "tabPage_HG";
            this.tabPage_HG.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_HG.Size = new System.Drawing.Size(977, 509);
            this.tabPage_HG.TabIndex = 4;
            this.tabPage_HG.Text = "HG";
            this.tabPage_HG.UseVisualStyleBackColor = true;
            // 
            // tabPage_SunCity
            // 
            this.tabPage_SunCity.Controls.Add(this.collect_SunCity);
            this.tabPage_SunCity.Location = new System.Drawing.Point(4, 22);
            this.tabPage_SunCity.Name = "tabPage_SunCity";
            this.tabPage_SunCity.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_SunCity.Size = new System.Drawing.Size(977, 509);
            this.tabPage_SunCity.TabIndex = 5;
            this.tabPage_SunCity.Text = "SunCity";
            this.tabPage_SunCity.UseVisualStyleBackColor = true;
            // 
            // tabPage_Upload
            // 
            this.tabPage_Upload.Controls.Add(this.dgvBetAmtDG);
            this.tabPage_Upload.Location = new System.Drawing.Point(4, 22);
            this.tabPage_Upload.Name = "tabPage_Upload";
            this.tabPage_Upload.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_Upload.Size = new System.Drawing.Size(977, 509);
            this.tabPage_Upload.TabIndex = 1;
            this.tabPage_Upload.Text = "待上传数据";
            this.tabPage_Upload.UseVisualStyleBackColor = true;
            // 
            // dgvBetAmtDG
            // 
            this.dgvBetAmtDG.AllowUserToAddRows = false;
            this.dgvBetAmtDG.AllowUserToDeleteRows = false;
            this.dgvBetAmtDG.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvBetAmtDG.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgvBetAmtDG.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvBetAmtDG.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ACTime,
            this.ACNT,
            this.GameID,
            this.GameType,
            this.BetAmount,
            this.BetAmountAct,
            this.Payout,
            this.isUpdateLoad,
            this.colStatus,
            this.json});
            this.dgvBetAmtDG.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvBetAmtDG.Location = new System.Drawing.Point(3, 3);
            this.dgvBetAmtDG.Name = "dgvBetAmtDG";
            this.dgvBetAmtDG.ReadOnly = true;
            this.dgvBetAmtDG.RowTemplate.Height = 23;
            this.dgvBetAmtDG.Size = new System.Drawing.Size(971, 503);
            this.dgvBetAmtDG.TabIndex = 0;
            this.dgvBetAmtDG.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvBetAmtDG_DataError);
            // 
            // ACTime
            // 
            this.ACTime.DataPropertyName = "_ACTime";
            dataGridViewCellStyle1.Format = "yyyy/MM/dd";
            dataGridViewCellStyle1.NullValue = null;
            this.ACTime.DefaultCellStyle = dataGridViewCellStyle1;
            this.ACTime.HeaderText = "帐务日";
            this.ACTime.Name = "ACTime";
            this.ACTime.ReadOnly = true;
            this.ACTime.Width = 66;
            // 
            // ACNT
            // 
            this.ACNT.DataPropertyName = "ACNT";
            this.ACNT.HeaderText = "子帐号";
            this.ACNT.Name = "ACNT";
            this.ACNT.ReadOnly = true;
            this.ACNT.Width = 66;
            // 
            // GameID
            // 
            this.GameID.DataPropertyName = "GameID";
            this.GameID.HeaderText = "游戏平台";
            this.GameID.Name = "GameID";
            this.GameID.ReadOnly = true;
            this.GameID.Width = 78;
            // 
            // GameType
            // 
            this.GameType.DataPropertyName = "GameType";
            this.GameType.HeaderText = "游戏类型";
            this.GameType.Name = "GameType";
            this.GameType.ReadOnly = true;
            this.GameType.Width = 78;
            // 
            // BetAmount
            // 
            this.BetAmount.DataPropertyName = "BetAmount";
            this.BetAmount.HeaderText = "投注额";
            this.BetAmount.Name = "BetAmount";
            this.BetAmount.ReadOnly = true;
            this.BetAmount.Width = 66;
            // 
            // BetAmountAct
            // 
            this.BetAmountAct.DataPropertyName = "BetAmountAct";
            this.BetAmountAct.HeaderText = "有效投注额";
            this.BetAmountAct.Name = "BetAmountAct";
            this.BetAmountAct.ReadOnly = true;
            this.BetAmountAct.Width = 90;
            // 
            // Payout
            // 
            this.Payout.DataPropertyName = "Payout";
            this.Payout.HeaderText = "输赢额度";
            this.Payout.Name = "Payout";
            this.Payout.ReadOnly = true;
            this.Payout.Width = 78;
            // 
            // isUpdateLoad
            // 
            this.isUpdateLoad.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
            this.isUpdateLoad.DataPropertyName = "IsUploaded";
            this.isUpdateLoad.HeaderText = "UpdateLoad";
            this.isUpdateLoad.Name = "isUpdateLoad";
            this.isUpdateLoad.ReadOnly = true;
            this.isUpdateLoad.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.isUpdateLoad.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.isUpdateLoad.Width = 5;
            // 
            // colStatus
            // 
            this.colStatus.DataPropertyName = "Status";
            this.colStatus.HeaderText = "Status";
            this.colStatus.Name = "colStatus";
            this.colStatus.ReadOnly = true;
            this.colStatus.Width = 66;
            // 
            // json
            // 
            this.json.DataPropertyName = "json";
            this.json.HeaderText = "json";
            this.json.Name = "json";
            this.json.ReadOnly = true;
            this.json.Width = 54;
            // 
            // tab_SetInfo
            // 
            this.tab_SetInfo.Controls.Add(this.splitContainer1);
            this.tab_SetInfo.Location = new System.Drawing.Point(4, 22);
            this.tab_SetInfo.Name = "tab_SetInfo";
            this.tab_SetInfo.Padding = new System.Windows.Forms.Padding(3);
            this.tab_SetInfo.Size = new System.Drawing.Size(977, 509);
            this.tab_SetInfo.TabIndex = 6;
            this.tab_SetInfo.Text = "配置信息";
            this.tab_SetInfo.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 3);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.configGrid);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.btnConfigSave);
            this.splitContainer1.Panel2.Controls.Add(this.btnConfigReload);
            this.splitContainer1.Size = new System.Drawing.Size(971, 503);
            this.splitContainer1.SplitterDistance = 462;
            this.splitContainer1.TabIndex = 1;
            // 
            // configGrid
            // 
            this.configGrid.AllowUserToAddRows = false;
            this.configGrid.AllowUserToDeleteRows = false;
            this.configGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.configGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colConfigKey,
            this.colConfigValue});
            this.configGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.configGrid.Location = new System.Drawing.Point(0, 0);
            this.configGrid.Name = "configGrid";
            this.configGrid.RowTemplate.Height = 23;
            this.configGrid.Size = new System.Drawing.Size(971, 462);
            this.configGrid.TabIndex = 0;
            this.configGrid.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellEndEdit);
            // 
            // colConfigKey
            // 
            this.colConfigKey.HeaderText = "Key";
            this.colConfigKey.Name = "colConfigKey";
            this.colConfigKey.ReadOnly = true;
            this.colConfigKey.Width = 200;
            // 
            // colConfigValue
            // 
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.colConfigValue.DefaultCellStyle = dataGridViewCellStyle2;
            this.colConfigValue.HeaderText = "Value";
            this.colConfigValue.Name = "colConfigValue";
            this.colConfigValue.Width = 500;
            // 
            // btnConfigSave
            // 
            this.btnConfigSave.Location = new System.Drawing.Point(87, 7);
            this.btnConfigSave.Name = "btnConfigSave";
            this.btnConfigSave.Size = new System.Drawing.Size(75, 23);
            this.btnConfigSave.TabIndex = 1;
            this.btnConfigSave.Text = "保存配置";
            this.btnConfigSave.UseVisualStyleBackColor = true;
            this.btnConfigSave.Click += new System.EventHandler(this.btnConfigSave_Click);
            // 
            // btnConfigReload
            // 
            this.btnConfigReload.Location = new System.Drawing.Point(6, 7);
            this.btnConfigReload.Name = "btnConfigReload";
            this.btnConfigReload.Size = new System.Drawing.Size(75, 23);
            this.btnConfigReload.TabIndex = 0;
            this.btnConfigReload.Text = "刷 新";
            this.btnConfigReload.UseVisualStyleBackColor = true;
            this.btnConfigReload.Click += new System.EventHandler(this.btnConfigReload_Click);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 300;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // tsbClear
            // 
            this.tsbClear.Image = ((System.Drawing.Image)(resources.GetObject("tsbClear.Image")));
            this.tsbClear.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbClear.Name = "tsbClear";
            this.tsbClear.Size = new System.Drawing.Size(76, 22);
            this.tsbClear.Text = "清除数据";
            this.tsbClear.Click += new System.EventHandler(this.tsbClear_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // collect_BBIN
            // 
            this.collect_BBIN.Dock = System.Windows.Forms.DockStyle.Fill;
            this.collect_BBIN.Location = new System.Drawing.Point(3, 3);
            this.collect_BBIN.MinimumSize = new System.Drawing.Size(20, 20);
            this.collect_BBIN.Name = "collect_BBIN";
            this.collect_BBIN.Size = new System.Drawing.Size(971, 503);
            this.collect_BBIN.TabIndex = 0;
            // 
            // collect_AG
            // 
            this.collect_AG.Dock = System.Windows.Forms.DockStyle.Fill;
            this.collect_AG.Location = new System.Drawing.Point(3, 3);
            this.collect_AG.MinimumSize = new System.Drawing.Size(20, 20);
            this.collect_AG.Name = "collect_AG";
            this.collect_AG.Size = new System.Drawing.Size(971, 503);
            this.collect_AG.TabIndex = 0;
            // 
            // collect_HG
            // 
            this.collect_HG.Dock = System.Windows.Forms.DockStyle.Fill;
            this.collect_HG.Location = new System.Drawing.Point(3, 3);
            this.collect_HG.MinimumSize = new System.Drawing.Size(20, 20);
            this.collect_HG.Name = "collect_HG";
            this.collect_HG.Size = new System.Drawing.Size(971, 503);
            this.collect_HG.TabIndex = 0;
            // 
            // collect_SunCity
            // 
            this.collect_SunCity.Dock = System.Windows.Forms.DockStyle.Fill;
            this.collect_SunCity.Location = new System.Drawing.Point(3, 3);
            this.collect_SunCity.MinimumSize = new System.Drawing.Size(20, 20);
            this.collect_SunCity.Name = "collect_SunCity";
            this.collect_SunCity.Size = new System.Drawing.Size(971, 503);
            this.collect_SunCity.TabIndex = 0;
            // 
            // collectReportLog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(985, 582);
            this.Controls.Add(this.tabReportPage);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.statusStrip1);
            this.Name = "collectReportLog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "平台投注报表采集系统";
            this.Load += new System.EventHandler(this.collectReportLog_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.tabReportPage.ResumeLayout(false);
            this.tabPage_BBIN.ResumeLayout(false);
            this.tabPage_AG.ResumeLayout(false);
            this.tabPage_HG.ResumeLayout(false);
            this.tabPage_SunCity.ResumeLayout(false);
            this.tabPage_Upload.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvBetAmtDG)).EndInit();
            this.tab_SetInfo.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.configGrid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripButton tsbCollect;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        public System.Windows.Forms.ToolStripProgressBar tsp_WebProcress;
        private System.Windows.Forms.TabControl tabReportPage;
        private System.Windows.Forms.TabPage tabPage_Upload;
        private System.Windows.Forms.DataGridView dgvBetAmtDG;
        private System.Windows.Forms.TabPage tabPage_AG;
        private collect_AG collect_AG;
        private System.Windows.Forms.TabPage tabPage_BBIN;
        private collect_BBIN collect_BBIN;
        private System.Windows.Forms.TabPage tabPage_HG;
        private collect_HG collect_HG;
        private System.Windows.Forms.TabPage tabPage_SunCity;
        private collect_SunCity collect_SunCity;
        private System.Windows.Forms.ToolStripButton tsbRefresh;
        private System.Windows.Forms.ToolStripButton tsbDataToServer;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton tsbExit;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.DataGridViewTextBoxColumn ACTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn ACNT;
        private System.Windows.Forms.DataGridViewTextBoxColumn GameID;
        private System.Windows.Forms.DataGridViewTextBoxColumn GameType;
        private System.Windows.Forms.DataGridViewTextBoxColumn BetAmount;
        private System.Windows.Forms.DataGridViewTextBoxColumn BetAmountAct;
        private System.Windows.Forms.DataGridViewTextBoxColumn Payout;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isUpdateLoad;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn json;
        private System.Windows.Forms.TabPage tab_SetInfo;
        private System.Windows.Forms.DataGridView configGrid;
        private System.Windows.Forms.DataGridViewTextBoxColumn colConfigKey;
        private System.Windows.Forms.DataGridViewTextBoxColumn colConfigValue;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button btnConfigSave;
        private System.Windows.Forms.Button btnConfigReload;
        private System.Windows.Forms.ToolStripButton tsbClear;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
    }
}