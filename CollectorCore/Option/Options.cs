using CollectorCore.Collector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollectorCore.Option
{
    public class Options
    {
        public string MainUrl { get; set; }
        public string Keyword { get; set; }
        public int MaxCollect { get; set; }
        public SearchEngineType SearchType { get; set; }
    }
}
