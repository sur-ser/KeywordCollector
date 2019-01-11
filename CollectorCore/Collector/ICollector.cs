using CollectorCore.Model;
using CollectorCore.Option;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollectorCore.Collector
{
    public interface ICollector
    {
        Options Options { get;}
        bool Complete { get;}
        string CurrentUrl { get; set; }
        List<KeywordCollectModel> FecthUrls();
        int Index { get; }
        int PageIndex { get; }
        void Close();
    }
}
