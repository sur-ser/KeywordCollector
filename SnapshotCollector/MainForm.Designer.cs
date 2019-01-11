namespace SnapshotCollector
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.cbHideCmd = new System.Windows.Forms.CheckBox();
            this.cbHideChrome = new System.Windows.Forms.CheckBox();
            this.txtStartThreads = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.cbRedirect = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cbRedirect);
            this.groupBox1.Controls.Add(this.btnStart);
            this.groupBox1.Controls.Add(this.cbHideCmd);
            this.groupBox1.Controls.Add(this.cbHideChrome);
            this.groupBox1.Controls.Add(this.txtStartThreads);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1176, 144);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "设置";
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(424, 32);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(118, 39);
            this.btnStart.TabIndex = 6;
            this.btnStart.Text = "开始采集";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // cbHideCmd
            // 
            this.cbHideCmd.AutoSize = true;
            this.cbHideCmd.Checked = true;
            this.cbHideCmd.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbHideCmd.Location = new System.Drawing.Point(222, 68);
            this.cbHideCmd.Name = "cbHideCmd";
            this.cbHideCmd.Size = new System.Drawing.Size(124, 22);
            this.cbHideCmd.TabIndex = 5;
            this.cbHideCmd.Text = "隐藏命令行";
            this.cbHideCmd.UseVisualStyleBackColor = true;
            this.cbHideCmd.CheckedChanged += new System.EventHandler(this.cbHideCmd_CheckedChanged);
            // 
            // cbHideChrome
            // 
            this.cbHideChrome.AutoSize = true;
            this.cbHideChrome.Checked = true;
            this.cbHideChrome.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbHideChrome.Location = new System.Drawing.Point(222, 32);
            this.cbHideChrome.Name = "cbHideChrome";
            this.cbHideChrome.Size = new System.Drawing.Size(124, 22);
            this.cbHideChrome.TabIndex = 4;
            this.cbHideChrome.Text = "隐藏浏览器";
            this.cbHideChrome.UseVisualStyleBackColor = true;
            this.cbHideChrome.CheckedChanged += new System.EventHandler(this.cbHideChrome_CheckedChanged);
            // 
            // txtStartThreads
            // 
            this.txtStartThreads.Location = new System.Drawing.Point(91, 33);
            this.txtStartThreads.Name = "txtStartThreads";
            this.txtStartThreads.Size = new System.Drawing.Size(100, 28);
            this.txtStartThreads.TabIndex = 3;
            this.txtStartThreads.Text = "15";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "线程数";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtLog);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 144);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1176, 521);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "日志";
            // 
            // txtLog
            // 
            this.txtLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtLog.Location = new System.Drawing.Point(3, 24);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLog.Size = new System.Drawing.Size(1170, 494);
            this.txtLog.TabIndex = 0;
            this.txtLog.TextChanged += new System.EventHandler(this.txtLog_TextChanged);
            // 
            // cbRedirect
            // 
            this.cbRedirect.AutoSize = true;
            this.cbRedirect.Checked = true;
            this.cbRedirect.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbRedirect.Location = new System.Drawing.Point(222, 105);
            this.cbRedirect.Name = "cbRedirect";
            this.cbRedirect.Size = new System.Drawing.Size(169, 22);
            this.cbRedirect.TabIndex = 7;
            this.cbRedirect.Text = "Url重定向主域名";
            this.cbRedirect.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1176, 665);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "快照采集工具";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox cbHideCmd;
        private System.Windows.Forms.CheckBox cbHideChrome;
        private System.Windows.Forms.TextBox txtStartThreads;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.CheckBox cbRedirect;
    }
}

