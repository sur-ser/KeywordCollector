namespace SnapshotView
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.panel4 = new System.Windows.Forms.Panel();
            this.btnDownPage = new System.Windows.Forms.Button();
            this.btnUpPage = new System.Windows.Forms.Button();
            this.lbLinkUrl = new System.Windows.Forms.LinkLabel();
            this.label6 = new System.Windows.Forms.Label();
            this.btnGoCurrentPage = new System.Windows.Forms.Button();
            this.txtCurrentPageIndex = new System.Windows.Forms.TextBox();
            this.lbUnHandleCount = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.lbHandleCount = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lbTotal = new System.Windows.Forms.Label();
            this.lbTotalPage = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel6 = new System.Windows.Forms.Panel();
            this.panel9 = new System.Windows.Forms.Panel();
            this.btnSelectAll = new System.Windows.Forms.Button();
            this.panCenter = new System.Windows.Forms.Panel();
            this.panel4.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel9.SuspendLayout();
            this.panCenter.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.panCenter);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(1254, 844);
            this.panel4.TabIndex = 3;
            // 
            // btnDownPage
            // 
            this.btnDownPage.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnDownPage.Location = new System.Drawing.Point(234, 0);
            this.btnDownPage.Name = "btnDownPage";
            this.btnDownPage.Size = new System.Drawing.Size(120, 73);
            this.btnDownPage.TabIndex = 1;
            this.btnDownPage.Text = "下一页";
            this.btnDownPage.UseVisualStyleBackColor = true;
            this.btnDownPage.Click += new System.EventHandler(this.btnDownPage_Click);
            // 
            // btnUpPage
            // 
            this.btnUpPage.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnUpPage.Location = new System.Drawing.Point(0, 0);
            this.btnUpPage.Name = "btnUpPage";
            this.btnUpPage.Size = new System.Drawing.Size(121, 73);
            this.btnUpPage.TabIndex = 0;
            this.btnUpPage.Text = "上一页";
            this.btnUpPage.UseVisualStyleBackColor = true;
            this.btnUpPage.Click += new System.EventHandler(this.btnUpPage_Click);
            // 
            // lbLinkUrl
            // 
            this.lbLinkUrl.AutoSize = true;
            this.lbLinkUrl.Location = new System.Drawing.Point(61, 26);
            this.lbLinkUrl.Name = "lbLinkUrl";
            this.lbLinkUrl.Size = new System.Drawing.Size(35, 18);
            this.lbLinkUrl.TabIndex = 1;
            this.lbLinkUrl.TabStop = true;
            this.lbLinkUrl.Text = "url";
            this.lbLinkUrl.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lbLinkUrl_LinkClicked);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(9, 26);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(44, 18);
            this.label6.TabIndex = 0;
            this.label6.Text = "网址";
            // 
            // btnGoCurrentPage
            // 
            this.btnGoCurrentPage.Location = new System.Drawing.Point(199, 145);
            this.btnGoCurrentPage.Name = "btnGoCurrentPage";
            this.btnGoCurrentPage.Size = new System.Drawing.Size(96, 35);
            this.btnGoCurrentPage.TabIndex = 15;
            this.btnGoCurrentPage.Text = "Go";
            this.btnGoCurrentPage.UseVisualStyleBackColor = true;
            this.btnGoCurrentPage.Click += new System.EventHandler(this.btnGoCurrentPage_Click);
            // 
            // txtCurrentPageIndex
            // 
            this.txtCurrentPageIndex.Location = new System.Drawing.Point(64, 148);
            this.txtCurrentPageIndex.Name = "txtCurrentPageIndex";
            this.txtCurrentPageIndex.Size = new System.Drawing.Size(94, 28);
            this.txtCurrentPageIndex.TabIndex = 14;
            // 
            // lbUnHandleCount
            // 
            this.lbUnHandleCount.AutoSize = true;
            this.lbUnHandleCount.Location = new System.Drawing.Point(59, 86);
            this.lbUnHandleCount.Name = "lbUnHandleCount";
            this.lbUnHandleCount.Size = new System.Drawing.Size(62, 18);
            this.lbUnHandleCount.TabIndex = 12;
            this.lbUnHandleCount.Text = "100000";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(9, 86);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(44, 18);
            this.label11.TabIndex = 11;
            this.label11.Text = "未选";
            // 
            // lbHandleCount
            // 
            this.lbHandleCount.AutoSize = true;
            this.lbHandleCount.Location = new System.Drawing.Point(59, 56);
            this.lbHandleCount.Name = "lbHandleCount";
            this.lbHandleCount.Size = new System.Drawing.Size(62, 18);
            this.lbHandleCount.TabIndex = 9;
            this.lbHandleCount.Text = "100000";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(9, 56);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(44, 18);
            this.label7.TabIndex = 8;
            this.label7.Text = "已选";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(166, 153);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(26, 18);
            this.label5.TabIndex = 7;
            this.label5.Text = "页";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(11, 153);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(44, 18);
            this.label4.TabIndex = 5;
            this.label4.Text = "当前";
            // 
            // lbTotal
            // 
            this.lbTotal.AutoSize = true;
            this.lbTotal.Location = new System.Drawing.Point(158, 118);
            this.lbTotal.Name = "lbTotal";
            this.lbTotal.Size = new System.Drawing.Size(62, 18);
            this.lbTotal.TabIndex = 3;
            this.lbTotal.Text = "100000";
            // 
            // lbTotalPage
            // 
            this.lbTotalPage.AutoSize = true;
            this.lbTotalPage.Location = new System.Drawing.Point(60, 118);
            this.lbTotalPage.Name = "lbTotalPage";
            this.lbTotalPage.Size = new System.Drawing.Size(62, 18);
            this.lbTotalPage.TabIndex = 1;
            this.lbTotalPage.Text = "100000";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 118);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "总数";
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.btnSelectAll);
            this.panel6.Controls.Add(this.btnDownPage);
            this.panel6.Controls.Add(this.btnUpPage);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel6.Location = new System.Drawing.Point(0, 205);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(354, 73);
            this.panel6.TabIndex = 0;
            // 
            // panel9
            // 
            this.panel9.Controls.Add(this.lbUnHandleCount);
            this.panel9.Controls.Add(this.lbLinkUrl);
            this.panel9.Controls.Add(this.label11);
            this.panel9.Controls.Add(this.btnGoCurrentPage);
            this.panel9.Controls.Add(this.lbTotal);
            this.panel9.Controls.Add(this.label6);
            this.panel9.Controls.Add(this.lbHandleCount);
            this.panel9.Controls.Add(this.txtCurrentPageIndex);
            this.panel9.Controls.Add(this.label1);
            this.panel9.Controls.Add(this.label7);
            this.panel9.Controls.Add(this.label4);
            this.panel9.Controls.Add(this.lbTotalPage);
            this.panel9.Controls.Add(this.label5);
            this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel9.Location = new System.Drawing.Point(0, 0);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(354, 205);
            this.panel9.TabIndex = 1;
            // 
            // btnSelectAll
            // 
            this.btnSelectAll.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSelectAll.Location = new System.Drawing.Point(121, 0);
            this.btnSelectAll.Name = "btnSelectAll";
            this.btnSelectAll.Size = new System.Drawing.Size(113, 73);
            this.btnSelectAll.TabIndex = 2;
            this.btnSelectAll.Text = "全选";
            this.btnSelectAll.UseVisualStyleBackColor = true;
            this.btnSelectAll.Click += new System.EventHandler(this.btnSelectAll_Click);
            // 
            // panCenter
            // 
            this.panCenter.Controls.Add(this.panel6);
            this.panCenter.Controls.Add(this.panel9);
            this.panCenter.Location = new System.Drawing.Point(564, 228);
            this.panCenter.Name = "panCenter";
            this.panCenter.Size = new System.Drawing.Size(354, 278);
            this.panCenter.TabIndex = 2;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1254, 844);
            this.Controls.Add(this.panel4);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "快照审核工具";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.panel4.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.panel9.ResumeLayout(false);
            this.panel9.PerformLayout();
            this.panCenter.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Button btnDownPage;
        private System.Windows.Forms.Button btnUpPage;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lbTotal;
        private System.Windows.Forms.Label lbTotalPage;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.LinkLabel lbLinkUrl;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lbHandleCount;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lbUnHandleCount;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button btnGoCurrentPage;
        private System.Windows.Forms.TextBox txtCurrentPageIndex;
        private System.Windows.Forms.Panel panel9;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Button btnSelectAll;
        private System.Windows.Forms.Panel panCenter;
    }
}

