namespace KeywordCollector
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.cbCollectSnapshot = new System.Windows.Forms.CheckBox();
            this.txtKeyword = new System.Windows.Forms.TextBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cbBaidu = new System.Windows.Forms.CheckBox();
            this.txtThreadCount = new System.Windows.Forms.TextBox();
            this.cbGoogle = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cbSougou = new System.Windows.Forms.CheckBox();
            this.cbIsHideCmd = new System.Windows.Forms.CheckBox();
            this.cbHideBrowser = new System.Windows.Forms.CheckBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel6 = new System.Windows.Forms.Panel();
            this.panel9 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel9.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox3);
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1167, 116);
            this.panel1.TabIndex = 0;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.cbCollectSnapshot);
            this.groupBox3.Controls.Add(this.txtKeyword);
            this.groupBox3.Controls.Add(this.btnStart);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point(363, 0);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(804, 116);
            this.groupBox3.TabIndex = 15;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "关键词";
            // 
            // cbCollectSnapshot
            // 
            this.cbCollectSnapshot.AutoSize = true;
            this.cbCollectSnapshot.Location = new System.Drawing.Point(310, 31);
            this.cbCollectSnapshot.Name = "cbCollectSnapshot";
            this.cbCollectSnapshot.Size = new System.Drawing.Size(89, 19);
            this.cbCollectSnapshot.TabIndex = 13;
            this.cbCollectSnapshot.Text = "采集快照";
            this.cbCollectSnapshot.UseVisualStyleBackColor = true;
            // 
            // txtKeyword
            // 
            this.txtKeyword.Dock = System.Windows.Forms.DockStyle.Left;
            this.txtKeyword.Location = new System.Drawing.Point(3, 21);
            this.txtKeyword.Multiline = true;
            this.txtKeyword.Name = "txtKeyword";
            this.txtKeyword.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtKeyword.Size = new System.Drawing.Size(291, 92);
            this.txtKeyword.TabIndex = 4;
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(310, 67);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(92, 32);
            this.btnStart.TabIndex = 5;
            this.btnStart.Text = "开始采集";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cbBaidu);
            this.groupBox2.Controls.Add(this.txtThreadCount);
            this.groupBox2.Controls.Add(this.cbGoogle);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.cbSougou);
            this.groupBox2.Controls.Add(this.cbIsHideCmd);
            this.groupBox2.Controls.Add(this.cbHideBrowser);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(363, 116);
            this.groupBox2.TabIndex = 14;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "配置项";
            // 
            // cbBaidu
            // 
            this.cbBaidu.AutoSize = true;
            this.cbBaidu.Checked = true;
            this.cbBaidu.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbBaidu.Location = new System.Drawing.Point(34, 31);
            this.cbBaidu.Name = "cbBaidu";
            this.cbBaidu.Size = new System.Drawing.Size(59, 19);
            this.cbBaidu.TabIndex = 0;
            this.cbBaidu.Text = "百度";
            this.cbBaidu.UseVisualStyleBackColor = true;
            // 
            // txtThreadCount
            // 
            this.txtThreadCount.Location = new System.Drawing.Point(187, 25);
            this.txtThreadCount.Name = "txtThreadCount";
            this.txtThreadCount.Size = new System.Drawing.Size(151, 25);
            this.txtThreadCount.TabIndex = 12;
            this.txtThreadCount.Text = "15";
            // 
            // cbGoogle
            // 
            this.cbGoogle.AutoSize = true;
            this.cbGoogle.Location = new System.Drawing.Point(34, 56);
            this.cbGoogle.Name = "cbGoogle";
            this.cbGoogle.Size = new System.Drawing.Size(77, 19);
            this.cbGoogle.TabIndex = 1;
            this.cbGoogle.Text = "Google";
            this.cbGoogle.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(130, 30);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(52, 15);
            this.label5.TabIndex = 11;
            this.label5.Text = "线程数";
            // 
            // cbSougou
            // 
            this.cbSougou.AutoSize = true;
            this.cbSougou.Location = new System.Drawing.Point(34, 81);
            this.cbSougou.Name = "cbSougou";
            this.cbSougou.Size = new System.Drawing.Size(69, 19);
            this.cbSougou.TabIndex = 2;
            this.cbSougou.Text = "Sogou";
            this.cbSougou.UseVisualStyleBackColor = true;
            // 
            // cbIsHideCmd
            // 
            this.cbIsHideCmd.AutoSize = true;
            this.cbIsHideCmd.Checked = true;
            this.cbIsHideCmd.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbIsHideCmd.Location = new System.Drawing.Point(187, 81);
            this.cbIsHideCmd.Name = "cbIsHideCmd";
            this.cbIsHideCmd.Size = new System.Drawing.Size(134, 19);
            this.cbIsHideCmd.TabIndex = 7;
            this.cbIsHideCmd.Text = "隐藏命令行窗口";
            this.cbIsHideCmd.UseVisualStyleBackColor = true;
            this.cbIsHideCmd.CheckedChanged += new System.EventHandler(this.cbIsHideCmd_CheckedChanged);
            // 
            // cbHideBrowser
            // 
            this.cbHideBrowser.AutoSize = true;
            this.cbHideBrowser.Checked = true;
            this.cbHideBrowser.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbHideBrowser.Location = new System.Drawing.Point(187, 55);
            this.cbHideBrowser.Name = "cbHideBrowser";
            this.cbHideBrowser.Size = new System.Drawing.Size(104, 19);
            this.cbHideBrowser.TabIndex = 6;
            this.cbHideBrowser.Text = "隐藏浏览器";
            this.cbHideBrowser.UseVisualStyleBackColor = true;
            this.cbHideBrowser.CheckedChanged += new System.EventHandler(this.cbHideBrowser_CheckedChanged);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.txtLog);
            this.groupBox4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox4.Location = new System.Drawing.Point(0, 0);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox4.Size = new System.Drawing.Size(1167, 538);
            this.groupBox4.TabIndex = 2;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "日志";
            // 
            // txtLog
            // 
            this.txtLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtLog.Location = new System.Drawing.Point(3, 20);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLog.Size = new System.Drawing.Size(1161, 516);
            this.txtLog.TabIndex = 1;

            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.panel6);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(0, 116);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(1167, 538);
            this.panel4.TabIndex = 1;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.panel9);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel6.Location = new System.Drawing.Point(0, 0);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(1167, 538);
            this.panel6.TabIndex = 1;
            // 
            // panel9
            // 
            this.panel9.Controls.Add(this.groupBox4);
            this.panel9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel9.Location = new System.Drawing.Point(0, 0);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(1167, 538);
            this.panel9.TabIndex = 2;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1167, 654);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel1);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "关键字采集器";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.panel1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.panel9.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Panel panel9;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox txtKeyword;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox cbBaidu;
        private System.Windows.Forms.TextBox txtThreadCount;
        private System.Windows.Forms.CheckBox cbGoogle;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox cbSougou;
        private System.Windows.Forms.CheckBox cbIsHideCmd;
        private System.Windows.Forms.CheckBox cbHideBrowser;
        private System.Windows.Forms.CheckBox cbCollectSnapshot;
    }
}

