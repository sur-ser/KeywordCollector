using KeywordCollector.Option;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeywordCollector.Collector
{
    public interface ICollector
    {
        Options Options { get;}
        string CurrentUrl { get; set; }
        List<ResultItem> FecthUrls();
        void Close();
    }
}
