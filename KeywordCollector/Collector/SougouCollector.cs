using KeywordCollector.Option;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeywordCollector.Collector
{
    public class SougouCollector : ACollector
    {
        public SougouCollector(Options options) : base(options) { }

        protected override List<ResultItem> GetResultItems()
        {
            throw new NotImplementedException();
        }

        protected override List<ResultItem> ImplementFecthUrls(string keyword)
        {
            throw new NotImplementedException();
        }

        protected override void LoadMainPage()
        {
            throw new NotImplementedException();
        }

        protected override void LoadNextPage()
        {
            throw new NotImplementedException();
        }
    }
}
