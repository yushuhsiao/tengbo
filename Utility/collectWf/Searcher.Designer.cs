namespace collectWf
{
    partial class Searcher
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
            this.txt_KeyWords = new System.Windows.Forms.TextBox();
            this.btn_Search = new System.Windows.Forms.Button();
            this.dgv_List = new System.Windows.Forms.DataGridView();
            this.lab_Keywords = new System.Windows.Forms.Label();
            this.cbo_PageIndex = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.dgv_YList = new System.Windows.Forms.DataGridView();
            this.label4 = new System.Windows.Forms.Label();
            this.lab_Prompt = new System.Windows.Forms.Label();
            this.cbo_SearchUrl = new System.Windows.Forms.ComboBox();
            this.btn_ToExcel = new System.Windows.Forms.Button();
            this.Content = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Href = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DMain = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.KeyWords = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PageIndex = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Ranking = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SearchName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Rule = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.YContent = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Yhref = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.YDMain = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.YKeyWords = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.YPageIndex = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.YRanking = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.YSearchName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.YRule = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_List)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_YList)).BeginInit();
            this.SuspendLayout();
            // 
            // txt_KeyWords
            // 
            this.txt_KeyWords.Location = new System.Drawing.Point(63, 4);
            this.txt_KeyWords.Multiline = true;
            this.txt_KeyWords.Name = "txt_KeyWords";
            this.txt_KeyWords.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txt_KeyWords.Size = new System.Drawing.Size(318, 75);
            this.txt_KeyWords.TabIndex = 3;
            // 
            // btn_Search
            // 
            this.btn_Search.Location = new System.Drawing.Point(686, 24);
            this.btn_Search.Name = "btn_Search";
            this.btn_Search.Size = new System.Drawing.Size(75, 23);
            this.btn_Search.TabIndex = 4;
            this.btn_Search.Text = "采集";
            this.btn_Search.UseVisualStyleBackColor = true;
            this.btn_Search.Click += new System.EventHandler(this.btn_Search_Click);
            // 
            // dgv_List
            // 
            this.dgv_List.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_List.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Content,
            this.Href,
            this.DMain,
            this.KeyWords,
            this.PageIndex,
            this.Ranking,
            this.SearchName,
            this.Rule});
            this.dgv_List.Location = new System.Drawing.Point(4, 99);
            this.dgv_List.Name = "dgv_List";
            this.dgv_List.RowTemplate.Height = 23;
            this.dgv_List.Size = new System.Drawing.Size(1102, 221);
            this.dgv_List.TabIndex = 5;
            // 
            // lab_Keywords
            // 
            this.lab_Keywords.AutoSize = true;
            this.lab_Keywords.Location = new System.Drawing.Point(12, 33);
            this.lab_Keywords.Name = "lab_Keywords";
            this.lab_Keywords.Size = new System.Drawing.Size(47, 12);
            this.lab_Keywords.TabIndex = 6;
            this.lab_Keywords.Text = "关键字:";
            // 
            // cbo_PageIndex
            // 
            this.cbo_PageIndex.FormattingEnabled = true;
            this.cbo_PageIndex.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5"});
            this.cbo_PageIndex.Location = new System.Drawing.Point(441, 27);
            this.cbo_PageIndex.Name = "cbo_PageIndex";
            this.cbo_PageIndex.Size = new System.Drawing.Size(41, 20);
            this.cbo_PageIndex.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(417, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(17, 12);
            this.label1.TabIndex = 8;
            this.label1.Text = "前";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(497, 30);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(17, 12);
            this.label2.TabIndex = 9;
            this.label2.Text = "页";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 323);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 10;
            this.label3.Text = "可用域名";
            // 
            // dgv_YList
            // 
            this.dgv_YList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_YList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.YContent,
            this.Yhref,
            this.YDMain,
            this.YKeyWords,
            this.YPageIndex,
            this.YRanking,
            this.YSearchName,
            this.YRule});
            this.dgv_YList.Location = new System.Drawing.Point(4, 339);
            this.dgv_YList.Name = "dgv_YList";
            this.dgv_YList.RowTemplate.Height = 23;
            this.dgv_YList.Size = new System.Drawing.Size(1102, 190);
            this.dgv_YList.TabIndex = 11;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(14, 83);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 12;
            this.label4.Text = "所有域名";
            // 
            // lab_Prompt
            // 
            this.lab_Prompt.AutoSize = true;
            this.lab_Prompt.Location = new System.Drawing.Point(417, 67);
            this.lab_Prompt.Name = "lab_Prompt";
            this.lab_Prompt.Size = new System.Drawing.Size(401, 12);
            this.lab_Prompt.TabIndex = 13;
            this.lab_Prompt.Text = "注意：点击采集后，请不要进行任何点击操作，直至弹出采集完毕提示框！";
            // 
            // cbo_SearchUrl
            // 
            this.cbo_SearchUrl.FormattingEnabled = true;
            this.cbo_SearchUrl.Location = new System.Drawing.Point(535, 26);
            this.cbo_SearchUrl.Name = "cbo_SearchUrl";
            this.cbo_SearchUrl.Size = new System.Drawing.Size(121, 20);
            this.cbo_SearchUrl.TabIndex = 14;
            // 
            // btn_ToExcel
            // 
            this.btn_ToExcel.Location = new System.Drawing.Point(771, 25);
            this.btn_ToExcel.Name = "btn_ToExcel";
            this.btn_ToExcel.Size = new System.Drawing.Size(88, 23);
            this.btn_ToExcel.TabIndex = 15;
            this.btn_ToExcel.Text = "导出至Excel";
            this.btn_ToExcel.UseVisualStyleBackColor = true;
            this.btn_ToExcel.Click += new System.EventHandler(this.btn_ToExcel_Click);
            // 
            // Content
            // 
            this.Content.DataPropertyName = "Content";
            this.Content.HeaderText = "标题";
            this.Content.Name = "Content";
            // 
            // Href
            // 
            this.Href.DataPropertyName = "Href";
            this.Href.HeaderText = "链接";
            this.Href.Name = "Href";
            this.Href.Width = 220;
            // 
            // DMain
            // 
            this.DMain.DataPropertyName = "DMain";
            this.DMain.HeaderText = "域名";
            this.DMain.Name = "DMain";
            this.DMain.Width = 220;
            // 
            // KeyWords
            // 
            this.KeyWords.DataPropertyName = "KeyWords";
            this.KeyWords.HeaderText = "关键字";
            this.KeyWords.Name = "KeyWords";
            // 
            // PageIndex
            // 
            this.PageIndex.DataPropertyName = "PageIndex";
            this.PageIndex.HeaderText = "第几页";
            this.PageIndex.Name = "PageIndex";
            this.PageIndex.Width = 70;
            // 
            // Ranking
            // 
            this.Ranking.DataPropertyName = "Ranking";
            this.Ranking.HeaderText = "排名";
            this.Ranking.Name = "Ranking";
            // 
            // SearchName
            // 
            this.SearchName.DataPropertyName = "SearchName";
            this.SearchName.HeaderText = "搜索引擎";
            this.SearchName.Name = "SearchName";
            this.SearchName.Width = 80;
            // 
            // Rule
            // 
            this.Rule.DataPropertyName = "Rule";
            this.Rule.HeaderText = "采集规则";
            this.Rule.Name = "Rule";
            // 
            // YContent
            // 
            this.YContent.DataPropertyName = "Content";
            this.YContent.HeaderText = "标题";
            this.YContent.Name = "YContent";
            // 
            // Yhref
            // 
            this.Yhref.DataPropertyName = "href";
            this.Yhref.HeaderText = "链接";
            this.Yhref.Name = "Yhref";
            this.Yhref.Width = 220;
            // 
            // YDMain
            // 
            this.YDMain.DataPropertyName = "DMain";
            this.YDMain.HeaderText = "域名";
            this.YDMain.Name = "YDMain";
            this.YDMain.Width = 220;
            // 
            // YKeyWords
            // 
            this.YKeyWords.DataPropertyName = "KeyWords";
            this.YKeyWords.HeaderText = "关键字";
            this.YKeyWords.Name = "YKeyWords";
            // 
            // YPageIndex
            // 
            this.YPageIndex.DataPropertyName = "PageIndex";
            this.YPageIndex.HeaderText = "第几页";
            this.YPageIndex.Name = "YPageIndex";
            this.YPageIndex.Width = 70;
            // 
            // YRanking
            // 
            this.YRanking.DataPropertyName = "Ranking";
            this.YRanking.HeaderText = "排名";
            this.YRanking.Name = "YRanking";
            // 
            // YSearchName
            // 
            this.YSearchName.DataPropertyName = "SearchName";
            this.YSearchName.HeaderText = "搜索引擎";
            this.YSearchName.Name = "YSearchName";
            this.YSearchName.Width = 80;
            // 
            // YRule
            // 
            this.YRule.DataPropertyName = "Rule";
            this.YRule.HeaderText = "采集规则";
            this.YRule.Name = "YRule";
            // 
            // Searcher
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(1110, 537);
            this.Controls.Add(this.btn_ToExcel);
            this.Controls.Add(this.cbo_SearchUrl);
            this.Controls.Add(this.lab_Prompt);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.dgv_YList);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbo_PageIndex);
            this.Controls.Add(this.lab_Keywords);
            this.Controls.Add(this.dgv_List);
            this.Controls.Add(this.txt_KeyWords);
            this.Controls.Add(this.btn_Search);
            this.MaximizeBox = false;
            this.Name = "Searcher";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "搜索引擎域名采集器";
            this.Load += new System.EventHandler(this.Searcher_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_List)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_YList)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txt_KeyWords;
        private System.Windows.Forms.Button btn_Search;
        private System.Windows.Forms.DataGridView dgv_List;
        private System.Windows.Forms.Label lab_Keywords;
        private System.Windows.Forms.ComboBox cbo_PageIndex;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataGridView dgv_YList;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lab_Prompt;
        private System.Windows.Forms.ComboBox cbo_SearchUrl;
        private System.Windows.Forms.Button btn_ToExcel;
        private System.Windows.Forms.DataGridViewTextBoxColumn Content;
        private System.Windows.Forms.DataGridViewTextBoxColumn Href;
        private System.Windows.Forms.DataGridViewTextBoxColumn DMain;
        private System.Windows.Forms.DataGridViewTextBoxColumn KeyWords;
        private System.Windows.Forms.DataGridViewTextBoxColumn PageIndex;
        private System.Windows.Forms.DataGridViewTextBoxColumn Ranking;
        private System.Windows.Forms.DataGridViewTextBoxColumn SearchName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Rule;
        private System.Windows.Forms.DataGridViewTextBoxColumn YContent;
        private System.Windows.Forms.DataGridViewTextBoxColumn Yhref;
        private System.Windows.Forms.DataGridViewTextBoxColumn YDMain;
        private System.Windows.Forms.DataGridViewTextBoxColumn YKeyWords;
        private System.Windows.Forms.DataGridViewTextBoxColumn YPageIndex;
        private System.Windows.Forms.DataGridViewTextBoxColumn YRanking;
        private System.Windows.Forms.DataGridViewTextBoxColumn YSearchName;
        private System.Windows.Forms.DataGridViewTextBoxColumn YRule;
    }
}