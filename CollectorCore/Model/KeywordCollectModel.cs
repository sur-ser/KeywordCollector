using CollectorCore.Collector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollectorCore.Model
{
    public class KeywordCollectModel
    {
        public int Index { get; set; }
        public string OriginUrl { get; set; }
        public string Keyword { get; set; }
        public SearchEngineType SearchType { get; set; }
    }
}
