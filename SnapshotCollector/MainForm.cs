using CollectorCore.Collector;
using CollectorCore.Entites;
using CollectorCore.Log;
using CollectorCore.Model;
using CollectorCore.Snapshot;
using CollectorCore.Utils;
using CollectorCore.WebDriver;
using System;
using System.Collections.Concurrent;
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

namespace SnapshotCollector
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            WebDriverManager.IsHide = this.cbHideChrome.Checked;
            WebDriverManager.IsHideCmd = this.cbHideCmd.Checked;
            WinformLog.SetUp(this.txtLog);
        }

        private void cbHideChrome_CheckedChanged(object sender, EventArgs e)
        {
            WebDriverManager.IsHide = this.cbHideChrome.Checked;
        }

        private void cbHideCmd_CheckedChanged(object sender, EventArgs e)
        {
            WebDriverManager.IsHideCmd = this.cbHideCmd.Checked;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            Start();
        }

        private async void Start()
        {
            if (SnapshotManager.IsStarted)
                return;

            btnStart.Enabled = false;

            WinformLog.ShowLog("开始采集");           

            Order = 0;
            var items = await GetCollectInfoModel();
            LastIndex = Order;
            Order = 0;

            int.TryParse(this.txtStartThreads.Text, out int threadCount);
            SnapshotManager.MaxThread = threadCount <= 0 ? 1 : threadCount;

            if (this.cbRedirect.Checked)
            {
                WinformLog.ShowLog("开始Url重定向到host.");

                RedirectItems.Clear();
                //20条url一个协程
                var taskCount = (int)Math.Ceiling(items.Count / 20m);
                for(var i = 0; i < taskCount; i++)
                {
                    SaveRedirectItems(items.Skip(i * 20).Take(20));
                }
            }
            else
            {
                SnapshotManager.SaveAsSnapshot(items);
                SnapshotManager.Start();
                await CloseSnapshotGenerator();
            }
        }

        private int Order = 0;
        private int LastIndex = 0;
        private ConcurrentDictionary<string, KeywordCollectModel> RedirectItems = new ConcurrentDictionary<string, KeywordCollectModel>();
        private async void SaveRedirectItems(IEnumerable<KeywordCollectModel> items)
        {
            var orderItems = new List<KeywordCollectModel>();
            foreach (var item in items)
            {
                var host = await GetRedirectHost(item);
                if (!string.IsNullOrEmpty(host))
                {
                    if (!RedirectItems.ContainsKey(host))
                    {
                        Interlocked.Increment(ref Order);
                        var itm = new KeywordCollectModel
                        {
                            Index = Order,
                            OriginUrl = host
                        };
                        orderItems.Add(itm);
                        RedirectItems.TryAdd(host, itm);
                        WinformLog.ShowLog($"{host}");
                    }
                }
            }
            SnapshotManager.SaveAsSnapshot(orderItems);
            if(items.Last().Index == this.LastIndex)
            {
                SnapshotManager.Start();
                await CloseSnapshotGenerator();
            }
        }

        private async Task CloseSnapshotGenerator()
        {
            if (await SnapshotManager.GetCompleteAsync())
            {
                SnapshotManager.Close();
                btnStart.Enabled = true;
                WinformLog.ShowLog("采集结束.");
            }
        }

        private async Task<string> GetRedirectHost(KeywordCollectModel item)
        {
            item.OriginUrl = await HttpUtils.RedirectPathAsync(item.OriginUrl);
            var host = HttpUtils.GetHostByUrl(item.OriginUrl);
            return host;
        }

        private async Task<List<KeywordCollectModel>> GetCollectInfoModel()
        {
            var urlFiles = JsonFileUtils.GetTxtFileNames();
            var collects = new Dictionary<string, SnapshotModel>();
            foreach (var fileName in urlFiles)
            {
                var urls = await JsonFileUtils.GetUrlsAsync(fileName);
                foreach (var url in urls)
                {
                    if (collects.ContainsKey(url))
                        continue;

                    Order++;
                    collects.Add(url, new SnapshotModel
                    {
                        Order = Order,
                        Url = url,
                    });   
                }
            }

            var items = collects.Values.OrderBy(a => a.Order).Select(a => new KeywordCollectModel
            {
                Index = a.Order,
                OriginUrl = a.Url,
                Keyword = "UnKnow"
            }).ToList();

            return items;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            SnapshotManager.Close();
        }

        private void txtLog_TextChanged(object sender, EventArgs e)
        {
            this.txtLog.SelectionStart = this.txtLog.TextLength;
            this.txtLog.ScrollToCaret();
        }
    }
}
