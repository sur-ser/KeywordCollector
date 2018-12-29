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
        public Options Options { get;}
        protected ChromeDriver WebDriver { get; set; }
        protected int Index { get; set; }
        protected int PageIndex { get; set; }
        protected LinkedList<string> NextPages { get; }

        public ACollector(Options options)
        {
            this.Options = options;
            this.CurrentUrl = options.MailUrl;
            this.NextPages = new LinkedList<string>();
            this.WebDriver = new ChromeDriver();
        }

        protected abstract List<ResultItem> ImplementFecthUrls(string keyword);
        protected abstract void LoadNextPage();
        protected abstract List<ResultItem> GetResultItems();

        public List<ResultItem> FecthUrls()
        {
            return ImplementFecthUrls(this.Options.Keyword);
        }

        protected abstract void LoadMainPage();

        public void Close()
        {
            try
            {
                this.WebDriver.Close();
                this.WebDriver.Quit();
            }
            catch { }
        }
    }
}
