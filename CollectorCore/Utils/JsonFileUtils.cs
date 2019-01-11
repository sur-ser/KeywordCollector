using CollectorCore.Entites;
using CollectorCore.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CollectorCore.Utils
{
    public class JsonFileUtils
    {
        public const string SnapshotDataName = "SnapshotDatas.json";

        public static void Insert(string fileName, SnapshotModel model)
        {
            using(var sm = File.Open(fileName, FileMode.OpenOrCreate))
            {
                sm.Position = sm.Length;
                using (var sw = new StreamWriter(sm))
                {
                    var json = Newtonsoft.Json.JsonConvert.SerializeObject(model, Newtonsoft.Json.Formatting.None);
                    sw.WriteLine(json);
                    sw.Flush();
                }
            }
        }

        public static async Task InsertAsync(string fileName, string url)
        {
            using (var sm = File.Open(fileName, FileMode.OpenOrCreate))
            {
                sm.Position = sm.Length;
                using (var sw = new StreamWriter(sm))
                {
                    await sw.WriteAsync(url);
                    await sw.FlushAsync();
                }
            }
        }

        public static async Task<Dictionary<string, SnapshotModel>> GetModelsAsync(string fileName)
        {
            var result = new Dictionary<string, SnapshotModel>();

            try
            {
                if (string.IsNullOrEmpty(fileName))
                    return result;

                using (var sm = File.Open(fileName, FileMode.OpenOrCreate))
                {
                    sm.Position = 0;
                    using (var sr = new StreamReader(sm))
                    {
                        var str = await sr.ReadToEndAsync();
                        var list = str.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (var json in list)
                        {
                            var entity = Newtonsoft.Json.JsonConvert.DeserializeObject<SnapshotModel>(json);
                            result[entity.Url] = entity;
                        }
                    }
                }
            }
            catch{}
            return result;
        }

        public static async Task<HashSet<string>> GetUrlsAsync(string fileName)
        {
            var result = new HashSet<string>();
            try
            {
                if (string.IsNullOrEmpty(fileName))
                    return result;

                using (var sm = File.Open(fileName, FileMode.OpenOrCreate))
                {
                    sm.Position = 0;
                    using (var sr = new StreamReader(sm))
                    {
                        var str = await sr.ReadToEndAsync();
                        var urls = str.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (var url in urls)
                        {
                            result.Add(url);
                        }
                    }
                }
            }
            catch { }
            return result;
        }

        public static List<string> GetJsonFileNames()
        {
            try
            {
                var path = $"{Directory.GetCurrentDirectory()}\\jdatas\\";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                var files = Directory.GetFiles(path);
                return files.Where(a=>a.EndsWith(".json")).ToList();
            }
            catch
            {
                return new List<string>();
            }
        }

        public static List<string> GetTxtFileNames()
        {
            try
            {
                var path = $"{Directory.GetCurrentDirectory()}\\jdatas\\";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                var files = Directory.GetFiles(path);
                return files.Where(a => a.EndsWith(".txt")).ToList();
            }
            catch
            {
                return new List<string>();
            }
        }
    }
}
