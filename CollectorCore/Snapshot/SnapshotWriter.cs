using CollectorCore.Entites;
using CollectorCore.Model;
using CollectorCore.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollectorCore.Snapshot
{
    public class SnapshotWriter
    {
        public static async Task WriteEntityAsync(IEnumerable<UrlEntity> entities)
        {
            if (entities == null)
                return;

            var fileName = JsonFileUtils.SnapshotDataName;
            var path = $"{Directory.GetCurrentDirectory()}\\jdatas\\";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            var fullName = $"{path}{fileName}";

            var builder = new StringBuilder();
            foreach(var entity in entities)
            {
                var model = new SnapshotModel
                {
                    Url = entity.Url,
                    FileName = entity.ThumbnailFileName,
                };
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(model, Newtonsoft.Json.Formatting.None);
                builder.AppendLine(json);
            }
            await JsonFileUtils.InsertAsync(fullName, builder.ToString()); 
        }
    }
}
