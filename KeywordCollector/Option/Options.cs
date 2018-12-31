using KeywordCollector.Collector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeywordCollector.Option
{
    public class Options
    {
        public string MailUrl { get; set; }
        public string Keyword { get; set; }
        public int MaxCollect { get; set; }
        public SearchEngineType SearchType { get; set; }
    }
}
