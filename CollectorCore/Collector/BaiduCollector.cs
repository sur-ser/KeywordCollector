using CollectorCore.Log;
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
    public class BaiduCollector : ACollector
    {
        public BaiduCollector(Options options) : base(options) { }

        private int XPathIndex { get; set; }

        protected override List<KeywordCollectModel> GetKeywordCollectInfo()
        {
            var result = new List<KeywordCollectModel>();

            var items = CollectFromXPathIndex();
            result.AddRange(items);
            result.AddRange(CollectFromAd());

            if (this.Index >= this.Options.MaxCollect)
            {
                this.Complete = true;
            }

            return result;
        }

        private int RegulateXpathIndex()
        {
            WinformLog.ShowLog("校正XPathIndex");
            for(int i = 1; i < 10000; i++)
            {
                try
                {
                    var element = this.WebDriver.FindElementByXPath($"//*[@id=\"{i}\"]/h3/a");
                    WinformLog.ShowLog("校正XPathIndex成功!");
                    return i;
                }
                catch { }
            }
            WinformLog.ShowLog("校正XPathIndex失败!");
            return 0;
        }


        private List<KeywordCollectModel> CollectFromXPathIndex()
        {
            var result = new List<KeywordCollectModel>();

            var start = this.XPathIndex + 1;
            var end = this.XPathIndex + 15;
            for (var i = start; i <= end; i++)
            {
                string url = null;
                int retry = 0;
                while (true)
                {
                    try
                    {
                        var element = this.WebDriver.FindElementByXPath($"//*[@id=\"{i}\"]/h3/a");
                        url = element.GetAttribute("href");
                        XPathIndex++;
                        break;
                    }
                    catch(Exception e)
                    {
                        if(i == start)
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

                if (string.IsNullOrEmpty(url))
                {
                    if(i == start)
                    {
                        var rlIndex = RegulateXpathIndex();
                        if (rlIndex == 0)
                        {
                            this.Complete = true;
                            return result;
                        }

                        this.XPathIndex = rlIndex;
                        return CollectFromXPathIndex();
                    }
                    break;
                }

                if (CollectUrls.Add(url))
                {
                    this.Index++;
                    result.Add(new KeywordCollectModel
                    {
                        Index = this.Index,
                        OriginUrl = url,
                        Keyword = this.Options.Keyword,
                        SearchType = this.Options.SearchType,
                    });
                }
            }

            return result;
        }

        /// <summary>
        /// /抓取百度推广的连接
        /// </summary>
        /// <returns></returns>
        private List<KeywordCollectModel> CollectFromAd()
        {
            var result = new List<KeywordCollectModel>();
            for(int i = 3; i < 6; i++)
            {
                for(int j = 1; j < 4; j++)
                {
                    IWebElement element = null;
                    try
                    {
                        element = this.WebDriver.FindElementByXPath($"//*[@id=\"{i}00{j}\"]/div[1]/h3/a[1]");
                    }
                    catch { }

                    if(element == null)
                    {
                        try
                        {
                            element = this.WebDriver.FindElementByXPath($"//*[@id=\"{i}00{j}\"]/div[1]/h3/a");
                        }
                        catch { }
                    }

                    if(element != null)
                    {
                        var url = element.GetAttribute("href");
                        if (CollectUrls.Add(url))
                        {
                            this.Index++;
                            result.Add(new KeywordCollectModel
                            {
                                Index = this.Index,
                                OriginUrl = url,
                                Keyword = this.Options.Keyword,
                                SearchType = this.Options.SearchType,
                            });
                        }
                    }
                }
            }
            return result;
        }

        public override void LoadNextPage()
        {
            if(this.NextPages.Count == 0)
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
                            if(i == 8)
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
                            if(i == 2)
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

        protected override void LoadMainPage()
        {
            try
            {
                this.LoadUrl(this.CurrentUrl);

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
