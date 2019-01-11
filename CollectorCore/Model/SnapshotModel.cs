using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollectorCore.Model
{
    public class SnapshotModel
    {
        public string Url { get; set; }
        public string FileName { get; set; }
        public bool Checked { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        public int Order { get; set; }
        public string GetFullName()
        {
            return $"{Directory.GetCurrentDirectory()}\\images\\{this.FileName}";
        }
    }
}
