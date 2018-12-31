using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeywordCollector.Collector
{
    public class ResultItem
    {
        public int Index { get; set; }
        public string OriginUrl { get; set; }
        public SearchEngineType SearchType { get; set; }
    }
}
