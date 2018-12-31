using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeywordCollector.Thumbnail
{
    public class ProcessUtils
    {
        public static List<int> GetChromeDriverProcessId()
        {
            Process[] processIdAry = Process.GetProcessesByName("chromedriver");
            return processIdAry.Select(a => a.Id).ToList();
        }
    }
}
