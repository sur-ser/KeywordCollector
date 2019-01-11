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
    public class GoogleCollector : ACollector
    {
        public GoogleCollector(Options options) : base(options) { }
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
            for (var i = 1; i <= 10; i++)
            {
                string url = null;
                int retry = 0;
                while (true)
                {
                    try
                    {
                        var element = this.WebDriver.FindElementByXPath($"//*[@id=\"rso\"]/div/div/div[{i}]/div/div/div[1]/a");
                        url = element.GetAttribute("href");
                        break;
                    }
                    catch (Exception e)
                    {
                        if (i == 1)
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
                    break;

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

        protected override void LoadMainPage()
        {
            try
            {
                this.LoadUrl(this.CurrentUrl);

                var searchTxtElement = this.WebDriver.FindElementByCssSelector("#tsf > div:nth-child(2) > div > div.RNNXgb > div > div.a4bIc > input");
                searchTxtElement.SendKeys(this.Options.Keyword);

                var searchBtnElement = this.WebDriver.FindElementByCssSelector("#tsf > div:nth-child(2) > div > div.FPdoLc.VlcLAe > center > input[type=\"submit\"]:nth-child(1)");
                searchBtnElement.Click();
                this.CurrentUrl = this.WebDriver.Url;
            }
            catch { }

            GoToSearchPage();
        }


        //谷歌默认推荐的搜索页面最多只有15页，需要跳转到真正的搜索页面。
        private void GoToSearchPage()
        {
            while (true)
            {
                try
                {
                    var searchBtnElement = this.WebDriver.FindElementByXPath("//*[@id=\"nav\"]/tbody/tr/td[11]/a");
                    searchBtnElement.Click();

                    if (this.CurrentUrl == this.WebDriver.Url)
                        break;

                    this.CurrentUrl = this.WebDriver.Url;
                }
                catch
                {
                    break;
                }
            }

            try
            {
                //这里才是跳转到真正搜索的页面
                var searchBtnElement = this.WebDriver.FindElementByCssSelector("#ofr > i > a");
                searchBtnElement.Click();
                this.CurrentUrl = this.WebDriver.Url;
            }
            catch
            {
                WinformLog.ShowLog("查找谷歌搜索主页面失败.");
                this.Complete = true;
            }
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
                    var lastElement = this.WebDriver.FindElementByXPath("//*[@id=\"nav\"]/tbody/tr/td[11]/a");
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
                    while (true)
                    {
                        try
                        {
                            var element = this.WebDriver.FindElementByXPath($"//*[@id=\"nav\"]/tbody/tr/td[{i}]/a");
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
                for (var i = 3; i <= 11; i++)
                {
                    string url = null;
                    int retry = 0;
                    while (true)
                    {
                        try
                        {
                            var element = this.WebDriver.FindElementByXPath($"//*[@id=\"nav\"]/tbody/tr/td[{i}]/a");
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
