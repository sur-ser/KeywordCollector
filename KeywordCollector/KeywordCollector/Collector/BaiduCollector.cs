using KeywordCollector.Option;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeywordCollector.Collector
{
    public class BaiduCollector : ACollector
    {
        public BaiduCollector(Options options) : base(options) { }

        protected override async Task<List<ResultItem>> ImplementFecthUrls(string keyword)
        {
            await this.LoadMainPage();
            var result = new List<ResultItem>();
            while (!this.Complete)
            {
                var items = GetResultItems();
                result.AddRange(items);
                await LoadNextPage();
            }
            return result;
        }

        protected override List<ResultItem> GetResultItems()
        {
            var selector = "#\\31 > h3 > a";
            this.WebDriver.FindElementByCssSelector(selector);

            var result = new List<ResultItem>();
            return result;
        }

        protected override Task LoadNextPage()
        {
            return Task.CompletedTask;
        }
    }
}
