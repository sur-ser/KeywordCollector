using CollectorCore.Collector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollectorCore.Entites
{
    public class UrlEntity : IEntity
    {
        public SearchEngineType engineType { get; set; }
        public int CollectIndex { get; set; }
        public string Url { get; set; }
        public string ThumbnailFileName { get; set; }
        public bool Selected { get; set; }
    }
}
