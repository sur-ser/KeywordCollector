using CollectorCore.Collector;
using CollectorCore.Entites;
using CollectorCore.Log;
using CollectorCore.Model;
using CollectorCore.Option;
using CollectorCore.Snapshot;
using CollectorCore.Utils;
using CollectorCore.WebDriver;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KeywordCollector
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            WebDriverManager.IsHide = this.cbHideBrowser.Checked;
            WebDriverManager.IsHideCmd = this.cbIsHideCmd.Checked;
            WinformLog.SetUp(this.txtLog);
            this.cbBaidu.Checked = true;
            this.txtKeyword.Text = "测试关键词1\r\n测试关键词2\r\n测试关键词3";
        }

        private List<ICollector> Collectors { get;} = new List<ICollector>();

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (SnapshotManager.IsStarted)
                return;

            var keyword = this.txtKeyword.Text;
            if (string.IsNullOrWhiteSpace(keyword))
                return;

            btnStart.Enabled = false;

            var keywords = keyword.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();
            AssignTasks(keywords);
        }

        private void AssignTasks(List<string> keywords)
        {
            if (cbBaidu.Checked)
            {
                StartCollect(keywords, SearchEngineType.Baidu);
            }
            if (cbGoogle.Checked)
            {
                StartCollect(keywords, SearchEngineType.Google);
            }
            if (cbSougou.Checked)
            {
                StartCollect(keywords, SearchEngineType.Sogou);
            }

            if (cbCollectSnapshot.Checked)
            {
                int.TryParse(this.txtThreadCount.Text, out int threadCount);
                SnapshotManager.MaxThread = threadCount <= 0 ? 1 : threadCount;
                SnapshotManager.Start();
            }
        }

        private async void StartCollect(List<string> keywords, SearchEngineType searchEngineType)
        {
            foreach(var keyword in keywords)
            {
                ICollector collector = null;

                if (searchEngineType == SearchEngineType.Baidu)
                {
                    var options = new Options
                    {
                        MainUrl = "https://www.baidu.com",
                        Keyword = keyword,
                        MaxCollect = 100000,
                        SearchType = SearchEngineType.Baidu,
                    };
                    collector = new BaiduCollector(options);
                }
                else if(searchEngineType == SearchEngineType.Google)
                {
                    var options = new Options
                    {
                        MainUrl = "https://www.google.com",
                        Keyword = keyword,
                        MaxCollect = 100000,
                        SearchType = SearchEngineType.Google,
                    };
                    collector = new GoogleCollector(options);
                }
                else if(searchEngineType == SearchEngineType.Sogou)
                {
                    var options = new Options
                    {
                        MainUrl = "https://www.sogou.com",
                        Keyword = keyword,
                        MaxCollect = 100000,
                        SearchType = SearchEngineType.Sogou,
                    };
                    collector = new SogouCollector(options);
                }

                if(collector != null)
                {
                    this.Collectors.Add(collector);
                    await StartOne(collector);
                    collector.Close();
                }
            }

            //采集状态机更新
            if (!this.Collectors.Where(a => !a.Complete).Any())
            {
                if (!cbCollectSnapshot.Checked)
                {
                    btnStart.Enabled = true;
                    return;
                }

                if (await SnapshotManager.GetCompleteAsync())
                {
                    btnStart.Enabled = true;
                    SnapshotManager.Close();
                }
            }
        }

        private async Task StartOne(ICollector collector)
        {
            WinformLog.ShowLog($"开始采集:{collector.Options.SearchType} {collector.Options.Keyword}");

            while (!collector.Complete)
            {
                var items = await StartCollect(collector);
                WinformLog.ShowLog($"{collector.Options.SearchType} {collector.Options.Keyword} 第:{collector.PageIndex}页抓取{items.Count}条。");

                if (cbCollectSnapshot.Checked)
                {
                    SnapshotManager.SaveAsSnapshot(items);
                    continue;
                }
                await SaveUrls(collector, items);
            }

            WinformLog.ShowLog($"{collector.Options.SearchType} {collector.Options.Keyword}:采集完成！");
        }

        private async Task SaveUrls(ICollector collector, List<KeywordCollectModel> items)
        {
            var builder = new StringBuilder();
            items.ForEach(a => builder.AppendLine(a.OriginUrl));
            var path = $"{Directory.GetCurrentDirectory()}\\jdatas\\";
            var fileName = $"{path}{collector.Options.Keyword}.txt";
            await JsonFileUtils.InsertAsync(fileName, builder.ToString());
        }

        private async Task<List<KeywordCollectModel>> StartCollect(ICollector collector)
        {
            List<KeywordCollectModel> result = null;
            await Task.Run(() =>
            {
                result = collector.FecthUrls();
            });
            return result;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();

            if (this.Collectors != null)
            {
                foreach (var collector in this.Collectors)
                {
                    collector.Close();
                }
            }
            SnapshotManager.Close();
        }

        private void cbHideBrowser_CheckedChanged(object sender, EventArgs e)
        {
            WebDriverManager.IsHide = this.cbHideBrowser.Checked;
        }

        private void cbIsHideCmd_CheckedChanged(object sender, EventArgs e)
        {
            WebDriverManager.IsHideCmd = this.cbIsHideCmd.Checked;
        }
    }
}
