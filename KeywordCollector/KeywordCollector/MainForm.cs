using KeywordCollector.Collector;
using KeywordCollector.Entites;
using KeywordCollector.JsonFile;
using KeywordCollector.Log;
using KeywordCollector.Option;
using KeywordCollector.Thumbnail;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
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
            WinformLog.SetUp(this.txtLog);
            this.cbBaidu.Checked = true;
            SetFileCombobox();
            this.timer1.Enabled = true;
            this.txtCurrent.Text = "1";
        }

        private List<ICollector> Collectors { get; set; }

        private void btnStart_Click(object sender, EventArgs e)
        {
            List<ICollector> collectors = new List<ICollector>();
            this.Collectors = collectors;
            if (cbBaidu.Checked)
            {
                var options = new Options
                {
                    MailUrl = "https://www.baidu.com",
                    Keyword = this.txtKeyword.Text,
                    MaxCollect = 100000,
                    SearchType = SearchEngineType.Baidu,
                };
                var collector = new BaiduCollector(options);
                collectors.Add(collector);
            }
            if (cbGoogle.Checked)
            {
                var options = new Options
                {
                    MailUrl = "https://www.google.com",
                    Keyword = this.txtKeyword.Text,
                    MaxCollect = 100000,
                    SearchType = SearchEngineType.Google,
                };
                var collector = new GoogleCollector(options);
                collectors.Add(collector);
            }
            if (cbSougou.Checked)
            {
                var options = new Options
                {
                    MailUrl = "https://www.sougou.com",
                    Keyword = this.txtKeyword.Text,
                    MaxCollect = 100000,
                    SearchType = SearchEngineType.Sougou,
                };
                var collector = new SougouCollector(options);
                collectors.Add(collector);
            }

            foreach (var collector in collectors)
            {
                HandleStart(collector);
            }
        }

        private async void HandleStart(ICollector collector)
        {
            WinformLog.ShowLog($"Start collect:{collector.Options.SearchType}");
            List<ResultItem> result = null;
            await Task.Run(() =>
            {
                result = collector.FecthUrls();
            });

            WinformLog.ShowLog($"Handle {collector.Options.SearchType} complete!");
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
            ThumbnailFileManager.Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            var deletes = new List<ResultItem>();
            var kvs = ThumbnailFileManager.UrlEntitys;
            foreach (var kv in kvs)
            {
                var fileName = $"{this.txtKeyword.Text}{kv.Value.engineType}";
                var path = $"{Directory.GetCurrentDirectory()}\\files\\";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                var fullName = $"{path}{fileName}.txt";
                JsonFileClass.Insert(fullName, kv.Value);
                deletes.Add(kv.Key);
            }

            foreach (var delete in deletes)
            {
                kvs.TryRemove(delete, out UrlEntity value);
            }
        }

        private void SetFileCombobox()
        {
            var files = JsonFileClass.GetFileNames();
            this.cbbFiles.Items.Clear();
            foreach (var file in files)
            {
                this.cbbFiles.Items.Add(file);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            SetFileCombobox();
        }

        private void ShowThumbnail(string fileName)
        {
            var img = Image.FromFile(fileName);
            this.pbSnapshot.Image = ThumbnailFileManager.ZoomPicture(img, this.pbSnapshot.Width, this.pbSnapshot.Height);
        }      


        private Dictionary<string, UrlEntity> UrlEntites { get; set; }
        private async void cbbFiles_TextChanged(object sender, EventArgs e)
        {
            var fileName = this.cbbFiles.Text;
            this.UrlEntites = await JsonFileClass.GetEntites(fileName);

            this.txtCurrent.Text = "1";
            this.lbTotal.Text = this.UrlEntites.Count.ToString();
            var pageList = GetPageList(this.UrlEntites.Values, 1, 20);

            SetSelectList(pageList);
        }

        private List<UrlEntity> GetPageList(ICollection<UrlEntity> entities, int index, int pageSize)
        {
            if (entities.Count < pageSize)
                pageSize = entities.Count;

            if (entities.Count < index)
                return new List<UrlEntity>();

            var skip = (index - 1) * pageSize;
            if (entities.Count < skip)
            {
                return new List<UrlEntity>();
            }

            pageSize = entities.Count - skip < pageSize ? entities.Count - skip : pageSize;
            return entities.Skip(skip).Take(pageSize).ToList();
        }

        private void SetSelectList(List<UrlEntity> entities)
        {
            lbSelect.Items.Clear();

            if (!entities.Any())
                return;

            foreach (var entity in entities)
            {
               var index = lbSelect.Items.Add(entity.Url);
                lbSelect.SetItemChecked(index, entity.Selected);
            }
        }

        private void lbSelect_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < lbSelect.Items.Count; i++)
            {
                var select = lbSelect.GetSelected(i);
                if (select)
                {
                    var curChecked = lbSelect.GetItemChecked(i);
                    if (curChecked)
                    {
                        lbSelect.SetSelected(i, false);
                    }
                    lbSelect.SetItemChecked(i, !curChecked);

                    var key = lbSelect.Items[i].ToString();
                    //修改选择状态
                    this.UrlEntites[key].Selected = !curChecked;

                    this.lbCurrentSelectUrl.Links.Clear();
                    this.lbCurrentSelectUrl.Links.Add(0, key.Length, key);
                    this.lbCurrentSelectUrl.Text = key;

                    var path = $"{Directory.GetCurrentDirectory()}\\images\\";
                    var fullName = $"{path}{this.UrlEntites[key].ThumbnailFileName}";
                    ShowThumbnail(fullName);

                    break;
                }
            }
        }

        private void lbSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < lbSelect.Items.Count; i++)
            {
                var select = lbSelect.GetSelected(i);
                if (select)
                {
                    var curChecked = lbSelect.GetItemChecked(i);
                    var key = lbSelect.Items[i].ToString();

                    //当前选中Url
                    this.lbCurrentSelectUrl.Links.Clear();
                    this.lbCurrentSelectUrl.Links.Add(0, key.Length, key);
                    this.lbCurrentSelectUrl.Text = key;

                    var path = $"{Directory.GetCurrentDirectory()}\\images\\";
                    var fullName = $"{path}{this.UrlEntites[key].ThumbnailFileName}";
                    ShowThumbnail(fullName);

                    break;
                }
            }
        }

        private void lbSelect_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Space)
            {
                for (int i = 0; i < lbSelect.Items.Count; i++)
                {
                    var select = lbSelect.GetSelected(i);
                    if (select)
                    {
                        var curChecked = lbSelect.GetItemChecked(i);

                        var key = lbSelect.Items[i].ToString();
                        //修改选择状态
                        this.UrlEntites[key].Selected = !curChecked;
                        break;
                    }
                }
            }
        }

        private void lbCurrentSelectUrl_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string targetUrl = e.Link.LinkData as string;
            if (string.IsNullOrEmpty(targetUrl))
                MessageBox.Show("没有链接地址！");
            else
                System.Diagnostics.Process.Start(targetUrl);
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            var index = int.Parse(this.txtCurrent.Text);
            if (index == 1)
            {
                index = 1;
            }
            else
            {
                index--;
            }
            this.txtCurrent.Text = index.ToString();
            var pageList = GetPageList(this.UrlEntites.Values, index, 20);
            SetSelectList(pageList);
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            var index = int.Parse(this.txtCurrent.Text);

            index++;
            this.txtCurrent.Text = index.ToString();

            var pageList = GetPageList(this.UrlEntites.Values, index, 20);
            SetSelectList(pageList);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var index = int.Parse(this.txtCurrent.Text);
            var pageList = GetPageList(this.UrlEntites.Values, index, 20);
            SetSelectList(pageList);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();

            sfd.Filter = "网址文件（*.txt）|*.txt";
            sfd.FilterIndex = 1;
            sfd.RestoreDirectory = true;
            sfd.DefaultExt = "txt";

            //点了保存按钮进入 
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                string fullName = sfd.FileName.ToString();
                if (File.Exists(fullName))
                {
                    File.Delete(fullName);
                }
                var list = new List<string>();
                foreach(var kv in this.UrlEntites)
                {
                    if (kv.Value.Selected)
                    {
                        list.Add(kv.Value.Url);
                    }
                }
                using (var sm = File.Open(fullName, FileMode.OpenOrCreate))
                {
                    sm.Position = sm.Length;
                    using (var sw = new StreamWriter(sm))
                    {
                        foreach(var ls in list)
                        {
                            sw.WriteLine(ls);
                        }
                        sw.Flush();
                    }
                }
            }
        }
    }
}
