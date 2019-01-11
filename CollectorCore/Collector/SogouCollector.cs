using CollectorCore.Model;
using CollectorCore.Option;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CollectorCore.Collector
{
    public class SogouCollector : ACollector
    {
        public SogouCollector(Options options) : base(options) { }

        protected override List<KeywordCollectModel> GetKeywordCollectInfo()
        {
            var result = new List<KeywordCollectModel>();

            var items = CollectFromXPathIndex();
            result.AddRange(items);

            if (this.Index >= this.Options.MaxCollect)
            {
                this.Complete = true;
            }

            return result;
        }

        private List<KeywordCollectModel> CollectFromXPathIndex()
        {
            var result = new List<KeywordCollectModel>();
            for (var i = 0; i <= 10; i++)
            {
                var xpath = $"//*[@id=\"main\"]/div[2]/div/div[{i}]/h3/a";
                var url = GetXpathResult(xpath, i);

                if (url != null)
                    AddResultItem(result, url);


                xpath = $"//*[@id=\"main\"]/div[3]/div/div[{i}]/h3/a";
                url = GetXpathResult(xpath, i);

                if (url != null)
                    AddResultItem(result, url);

                xpath = $"//*[@id=\"uigs__{i}\"]";
                url = GetXpathResult(xpath, i);

                if (url != null)
                    AddResultItem(result, url);

            }
            return result;
        }

        private void AddResultItem(List<KeywordCollectModel> results, string url)
        {
            if (CollectUrls.Add(url))
            {
                this.Index++;
                results.Add(new KeywordCollectModel
                {
                    Index = this.Index,
                    OriginUrl = url,
                    Keyword = this.Options.Keyword,
                    SearchType = this.Options.SearchType,
                });
            }
        }

        private string GetXpathResult(string xpath, int index)
        {
            string url = null;
            int retry = 0;
            while (true)
            {
                try
                {
                    var element = this.WebDriver.FindElementByXPath(xpath);
                    url = element.GetAttribute("href");
                    break;
                }
                catch (Exception e)
                {
                    if (index == 0)
                    {
                        if (e is NoSuchElementException)
                        {
                            retry++;
                            if (retry > 30)
                                break;

                            Thread.Sleep(100);
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }

            return url;
        }


        protected override void LoadMainPage()
        {
            try
            {
                this.LoadUrl(this.CurrentUrl);

                var searchTxtElement = this.WebDriver.FindElementByCssSelector("#query");
                searchTxtElement.SendKeys(this.Options.Keyword);

                var searchBtnElement = this.WebDriver.FindElementByCssSelector("#stb");
                searchBtnElement.Click();
                this.CurrentUrl = this.WebDriver.Url;
            }
            catch { }
        }

        public override void LoadNextPage()
        {
            if (this.NextPages.Count == 0)
            {
                SetNextPages();
            }

            if (!this.NextPages.Any())
            {
                this.Complete = true;
                return;
            }

            var url = this.NextPages.First();

            this.LoadUrl(url);

            this.NextPages.RemoveFirst();
        }

        private void SetNextPages()
        {
            if (this.PageIndex > 5)
            {
                try
                {
                    var lastElement = this.WebDriver.FindElementByXPath("//*[@id=\"sogou_next\"]");
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

                for (var i = this.PageIndex + 1; i <= this.PageIndex + 4; i++)
                {
                    string url = null;
                    int retry = 0;
                    while (true)
                    {
                        try
                        {
                            var element = this.WebDriver.FindElementByXPath($"//*[@id=\"sogou_page_{i}\"]");
                            url = element.GetAttribute("href");
                            break;
                        }
                        catch
                        {
                            if (i == 8)
                            {
                                retry++;
                                if (retry > 30)
                                    break;

                                //需要等待WebDriver加载完整DOM树，在本地上测试2秒钟合适，这个得根据实际情况调整。
                                Thread.Sleep(100);
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(url))
                    {
                        this.NextPages.AddLast(url);
                    }
                }
            }
            else
            {
                for (var i = this.PageIndex + 2; i <= 10; i++)
                {
                    string url = null;
                    int retry = 0;
                    while (true)
                    {
                        try
                        {
                            var element = this.WebDriver.FindElementByXPath($"//*[@id=\"sogou_page_{i}\"]");
                            url = element.GetAttribute("href");
                            break;
                        }
                        catch
                        {
                            if (i == 2)
                            {
                                retry++;
                                if (retry > 30)
                                    break;

                                //需要等待WebDriver加载完整DOM树，在本地上测试2秒钟合适，这个得根据实际情况调整。
                                Thread.Sleep(100);
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(url))
                    {
                        this.NextPages.AddLast(url);
                    }
                }
            }
        }
    }
}
