using CollectorCore.Utils;
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

namespace SnapshotView
{
    public partial class MainForm : Form
    {
        private PicturePanelListBox PPList { get; } = new PicturePanelListBox();
        private int PageIndex
        {
            get
            {
                int.TryParse(this.txtCurrentPageIndex.Text, out int val);
                val = val == 0 ? 1 : val;
                return val;
            }
            set
            {
                this.txtCurrentPageIndex.Text = value.ToString();
            }
        }

        public MainForm()
        {
            InitializeComponent();

        }

        private async void MainForm_Load(object sender, EventArgs e)
        {
            Settings();

            await SnapshotViewManager.LaodSavedFile();
            await LoadSnapshotJsonData();

            SetConsoleControlToCenter();
        }

        private void SetConsoleControlToCenter()
        {
            this.panCenter.Location = new Point(this.Width / 2 - this.panCenter.Width /2, this.Height - this.panCenter.Height - 50);
        }

        private void Settings()
        {
            SetPicturePanelListBox();
        }

        private void SetPicturePanelListBox()
        {
            this.panel4.Controls.Add(this.PPList);
            this.PPList.CreateItems(SnapshotViewManager.PageSize);
            this.PPList.OnItemClick += async (p, m) => 
            {
                if(m != null)
                {
                    await SnapshotViewManager.AddToSaved(m);
                    SetStatusBar(PageIndex);
                }
            };

            this.PPList.OnItemMouseHover += p =>
            {
                //当前选中Url
                this.lbLinkUrl.Links.Clear();
                this.lbLinkUrl.Text = p.ShowUrl;
                if (!string.IsNullOrEmpty(p.ShowUrl))
                {
                    this.lbLinkUrl.Links.Add(0, p.ShowUrl.Length, p.ShowUrl);
                }
            };
        }

        private void SetStatusBar(int pageIndex)
        {
            this.lbTotalPage.Text = $"{Math.Ceiling(SnapshotViewManager.GetTotal() / (decimal)SnapshotViewManager.PageSize)} 页";
            this.lbTotal.Text = $"{SnapshotViewManager.GetTotal()} 条";
            this.lbHandleCount.Text = $"{SnapshotViewManager.GetChecked()} 条";
            this.lbUnHandleCount.Text = $"{SnapshotViewManager.GetUnChecked()} 条";
            this.txtCurrentPageIndex.Text = $"{pageIndex}";
        }

        private void btnUpPage_Click(object sender, EventArgs e)
        {
            if (PageIndex == 1)
                return;

            PageIndex--;
            var models = SnapshotViewManager.GetPageList(SnapshotViewManager.PageSize, PageIndex);
            this.PPList.UpdateItems(models);
        }

        private void btnDownPage_Click(object sender, EventArgs e)
        {
            PageIndex++;
            var models = SnapshotViewManager.GetPageList(SnapshotViewManager.PageSize, PageIndex);

            if (!models.Any())
            {
                PageIndex--;
                return;
            }

            this.PPList.UpdateItems(models);
        }

        private void btnGoCurrentPage_Click(object sender, EventArgs e)
        {
            var modes = SnapshotViewManager.GetPageList(SnapshotViewManager.PageSize, PageIndex);
            this.PPList.UpdateItems(modes);
        }

        private async void tsmOpen_Click(object sender, EventArgs e)
        {
            await LoadSnapshotJsonData();
        }

        private async Task LoadSnapshotJsonData()
        {
            var files = JsonFileUtils.GetJsonFileNames().Where(a => a.EndsWith(JsonFileUtils.SnapshotDataName)).ToList();
            await SnapshotViewManager.LoadAsync(files);

            PageIndex = (int)Math.Ceiling((decimal)SnapshotViewManager.GetLastChecked() / SnapshotViewManager.PageSize);
            var modes = SnapshotViewManager.GetPageList(SnapshotViewManager.PageSize, PageIndex);
            SetStatusBar(PageIndex);
            this.PPList.UpdateItems(modes);
        }

        private void tsmSaveAs_Click(object sender, EventArgs e)
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

                using (var sm = File.Open(fullName, FileMode.OpenOrCreate))
                {
                    sm.Position = sm.Length;
                    using (var sw = new StreamWriter(sm))
                    {
                        foreach (var ls in SnapshotViewManager.SavedUrls)
                        {
                            sw.WriteLine(ls);
                        }
                        sw.Flush();
                    }
                }

                MessageBox.Show("保存成功！", "提示！");
            }
        }

        private async void CheckAll()
        {
            var items = this.PPList.Items.Where(a => a.Model != null && !a.Checked).ToList();
            items.ForEach(a => a.Checked = true);
            var models = items.Select(a => a.Model);
            await SnapshotViewManager.AddToSaveds(models);
            SetStatusBar(PageIndex);
        }

        private void tsmDeleteSaveds_Click(object sender, EventArgs e)
        {
            SnapshotViewManager.ClearSaveds();
            SetStatusBar(this.PageIndex);
        }

        private void lbLinkUrl_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string targetUrl = e.Link.LinkData as string;
            if (string.IsNullOrEmpty(targetUrl))
                MessageBox.Show("没有链接地址！");
            else
                System.Diagnostics.Process.Start(targetUrl);
        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            this.CheckAll();
        }
    }
}
