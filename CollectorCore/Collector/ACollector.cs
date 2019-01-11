using CollectorCore.Log;
using CollectorCore.Model;
using CollectorCore.Option;
using CollectorCore.WebDriver;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollectorCore.Collector
{
    public abstract class ACollector : ICollector
    {
        public string CurrentUrl { get; set; }
        public bool Complete { get; set; }
        public Options Options { get;}
        protected ChromeDriver WebDriver { get; set; }
        public int Index { get; set; }
        public int PageIndex { get; set; }
        protected LinkedList<string> NextPages { get; }
        protected HashSet<string> CollectUrls { get; } = new HashSet<string>();

        public ACollector(Options options)
        {
            this.Options = options;
            this.CurrentUrl = options.MainUrl;
            this.NextPages = new LinkedList<string>();

            InitWebDriver();
        }

        public abstract void LoadNextPage();
        protected abstract List<KeywordCollectModel> GetKeywordCollectInfo();

        protected void ReloadWebDriver()
        {
            try
            {
                this.WebDriver.Close();
            }
            catch { }

            InitWebDriver();
        }

        protected void LoadUrl(string url)
        {
            if (!string.IsNullOrEmpty(url))
            {
                int retry = 0;
                while (true)
                {
                    try
                    {
                        this.WebDriver.Navigate().GoToUrl(url);
                        this.CurrentUrl = this.WebDriver.Url;
                        break;
                    }
                    catch
                    {
                        retry++;
                        if (retry >= 2)
                        {
                            WinformLog.ShowLog("加载搜搜引擎页面失败.");
                            this.Complete = true;
                            return;
                        }
                        this.ReloadWebDriver();
                    }
                }

            }
        }

        private void InitWebDriver()
        {
            try
            {
                if (WebDriverManager.IsHide)
                {
                    var options = WebDriverManager.GetOptions();

                    if (WebDriverManager.IsHideCmd)
                    {
                        var services = ChromeDriverService.CreateDefaultService();
                        services.HideCommandPromptWindow = WebDriverManager.IsHideCmd;
                        this.WebDriver = new ChromeDriver(services, options);
                    }
                    else
                    {
                        this.WebDriver = new ChromeDriver(options);
                    }
                }
                else
                {
                    this.WebDriver = new ChromeDriver();
                }
            }
            catch
            {
                this.Complete = true;
                WinformLog.ShowLog($"{this.Options.SearchType} 创建WebDriver实例失败.");
            }
        }

        protected List<KeywordCollectModel> ImplementFecthUrls(string keyword)
        {
            if (this.Index == 0)
            {
                this.Index = 0;
                this.PageIndex = 0;
                this.LoadMainPage();
            }
            else
            {
                this.LoadNextPage();

                if (this.Complete)
                    return new List<KeywordCollectModel>();
            }

            var result = GetKeywordCollectInfo();
            if (!result.Any())
            {
                this.Complete = true;
            }
            this.PageIndex++;
            return result;
        }

        public List<KeywordCollectModel> FecthUrls()
        {
            return ImplementFecthUrls(this.Options.Keyword);
        }

        protected abstract void LoadMainPage();

        public void Close()
        {
            try
            {
                this.WebDriver.Close();
            }
            catch { }
        }
    }
}
