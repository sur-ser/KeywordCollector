using KeywordCollector.Option;
using KeywordCollector.Thumbnail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KeywordCollector.Collector
{
    public class BaiduCollector : ACollector
    {
        public BaiduCollector(Options options) : base(options) { }

        protected override List<ResultItem> ImplementFecthUrls(string keyword)
        {
            this.Index = 0;
            this.PageIndex = 1;
            this.LoadMainPage();
            var result = new List<ResultItem>();
            while (!this.Complete)
            {
                var items = GetResultItems();
                result.AddRange(items);
                foreach (var item in items)
                    SnapshotManager.SaveAsThumbnail(item);

                LoadNextPage();
                this.PageIndex++;
            }
            return result;
        }

        protected override List<ResultItem> GetResultItems()
        {
            var result = new List<ResultItem>();
            var start = this.Index + 1;
            var end = this.Index + 10;
            for (var i = start; i <= end; i++)
            {
                string url = null;
                int retry = 0;
                while(true)
                {
                    try
                    {
                        var element = this.WebDriver.FindElementByXPath($"//*[@id=\"{i}\"]/h3/a");
                        url = element.GetAttribute("href");
                        break;
                    }
                    catch
                    {
                        url = null;
                        retry++;
                        if (retry > 30)
                            break;

                        //需要等待WebDriver加载完整DOM树，在本地上测试2秒钟合适，这个得根据实际情况调整。
                        Thread.Sleep(100);
                    }
                }

                this.Index++;
                if (string.IsNullOrEmpty(url))
                {
                    continue;
                }

                result.Add(new ResultItem
                {
                    Index = this.Index,
                    OriginUrl = url,
                    SearchType = this.Options.SearchType,
                });
            }

            if (this.Index >= this.Options.MaxCollect)
            {
                this.Complete = true;
            }

            return result;
        }

        protected override void LoadNextPage()
        {
            if(this.NextPages.Count == 0)
            {
                SetNextPages();
            }

            var url = this.NextPages.First();
            if (!string.IsNullOrEmpty(url))
            {
                try
                {
                    this.WebDriver.Navigate().GoToUrl(url);
                    this.CurrentUrl = this.WebDriver.Url;
                }
                catch { }
            }
            this.NextPages.RemoveFirst();
        }

        private void SetNextPages()
        {
            if (this.PageIndex > 5)
            {
                try
                {
                    var lastElement = this.WebDriver.FindElementByCssSelector("#page > a:nth-child(11) > span.fk.fkd");
                    if (lastElement == null)
                    {
                        this.Complete = true;
                        return;
                    }
                }
                catch
                {
                    this.Complete = true;
                    return;
                }

                for (var i = 8; i <= 11; i++)
                {                   
                    string url = null;
                    int retry = 0;
                    while(true)
                    {
                        try
                        {
                            var element = this.WebDriver.FindElementByCssSelector($"#page > a:nth-child({i})");
                            url = element.GetAttribute("href");
                            break;
                        }
                        catch
                        {
                            url = null;
                            retry++;
                            if (retry > 30)
                                break;

                            //需要等待WebDriver加载完整DOM树，在本地上测试2秒钟合适，这个得根据实际情况调整。
                            Thread.Sleep(100);
                        }
                    }
                    this.NextPages.AddLast(url);
                }
            }
            else
            {
                for (var i = 2; i < 11; i++)
                {
                    string url = null;
                    int retry = 0;
                    while (true)
                    {
                        try
                        {
                            var element = this.WebDriver.FindElementByCssSelector($"#page > a:nth-child({i})");
                            url = element.GetAttribute("href");
                            break;
                        }
                        catch
                        {
                            url = null;
                            retry++;
                            if (retry > 30)
                                break;

                            //需要等待WebDriver加载完整DOM树，在本地上测试2秒钟合适，这个得根据实际情况调整。
                            Thread.Sleep(100);
                        }
                    }
                    this.NextPages.AddLast(url);
                }
            }
        }

        protected override void LoadMainPage()
        {
            try
            {
                try
                {
                    WebDriver.Navigate().GoToUrl(this.CurrentUrl);

                }
                catch { }

                var searchTxtElement = this.WebDriver.FindElementByCssSelector("#kw");
                searchTxtElement.SendKeys(this.Options.Keyword);

                var searchBtnElement = this.WebDriver.FindElementByCssSelector("#su");
                searchBtnElement.Click();
                this.CurrentUrl = this.WebDriver.Url;
            }
            catch { }
        }
    }
}
