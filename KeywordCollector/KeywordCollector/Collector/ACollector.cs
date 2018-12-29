using KeywordCollector.Option;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeywordCollector.Collector
{
    public abstract class ACollector : ICollector
    {
        public string CurrentUrl { get; set; }
        public bool Complete { get; protected set; }
        private Options Options { get; set; }
        protected ChromeDriver WebDriver { get; set; }

        public ACollector(Options options)
        {
            this.Options = options;
            this.WebDriver = new ChromeDriver();
        }

        protected abstract Task<List<ResultItem>> ImplementFecthUrls(string keyword);
        protected abstract Task LoadNextPage();
        protected abstract List<ResultItem> GetResultItems();

        public Task<List<ResultItem>> FecthUrls(string keyword)
        {
            return ImplementFecthUrls(keyword);
        }

        protected async Task LoadMainPage()
        {
            await Task.Run(() =>
            {
                try
                {
                    WebDriver.Navigate().GoToUrl(this.CurrentUrl);
                }
                catch { }
            });
        }
    }
}
