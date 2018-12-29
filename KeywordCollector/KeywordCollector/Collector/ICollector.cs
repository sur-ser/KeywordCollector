using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeywordCollector.Collector
{
    public interface ICollector
    {
        string CurrentUrl { get; set; }
        Task<List<ResultItem>> FecthUrls(string keyword);
    }
}
