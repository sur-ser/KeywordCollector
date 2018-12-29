using KeywordCollector.Entites;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeywordCollector.JsonFile
{
    public class JsonFileClass
    {
        public static async Task InsertAsync(string fileName, UrlEntity entity)
        {
            using(var sm = File.Open(fileName, FileMode.OpenOrCreate))
            {
                sm.Position = sm.Length;
                using (var sw = new StreamWriter(sm))
                {
                    var json = Newtonsoft.Json.JsonConvert.SerializeObject(entity, Newtonsoft.Json.Formatting.None);
                    await sw.WriteLineAsync(json);
                    await sw.FlushAsync();
                }
            }
        }

        public static async Task<Dictionary<string, UrlEntity>> GetEntites(string fileName)
        {
            var result = new Dictionary<string, UrlEntity>();
            if (string.IsNullOrEmpty(fileName))
                return result;

            using (var sm = File.Open(fileName, FileMode.OpenOrCreate))
            {
                sm.Position = 0;
                using (var sr = new StreamReader(sm))
                {
                    var str = await sr.ReadToEndAsync();
                    var list = str.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
                    foreach(var json in list)
                    {
                        var entity = Newtonsoft.Json.JsonConvert.DeserializeObject<UrlEntity>(json);
                        result[entity.Url] = entity;
                    }
                }
            }
            return result;
        }

        public static List<string> GetFileNames()
        {
            var path = $"{Directory.GetCurrentDirectory()}\\files\\";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            var files = Directory.GetFiles(path);
            return files.ToList();
        }
    }
}
