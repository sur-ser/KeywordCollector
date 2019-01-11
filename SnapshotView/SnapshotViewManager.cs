using CollectorCore.Log;
using CollectorCore.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SnapshotView
{
    public class SnapshotViewManager
    {
        public const int PageSize = 4;
        private static Dictionary<string, SnapshotModel> UnSaveds { get; } = new Dictionary<string, SnapshotModel>();
        private static Dictionary<string, SnapshotModel> Saveds { get; } = new Dictionary<string, SnapshotModel>();
        private static List<SnapshotModel> AllSnapshots { get; } = new List<SnapshotModel>();

        public static List<string> SavedUrls
        {
            get
            {
                return Saveds.OrderBy(a=>a.Value.Order).Select(a => a.Value.Url).ToList();
            }
        }

        public static void ClearSaveds()
        {
            var fileName = FileName;
            if (File.Exists(fileName))
            {
                var file = new FileInfo(fileName);
                var newName = $"{fileName.TrimEnd(".txt".ToArray())}{DateTime.Now.ToString("yyyyMMddHHmmss")}.bak";
                file.MoveTo(newName);
            }

            foreach(var model in Saveds.Values)
            {
                AllSnapshots.Remove(model);
            }

            Saveds.Clear();
        }

        private static string BakFileName
        {
            get
            {
                var path = $"{Directory.GetCurrentDirectory()}\\data\\";
                var bakName = $"{path}VerifyUrls.bak";
                return bakName;
            }
        }

        private static string FileName
        {
            get
            {
                var path = $"{Directory.GetCurrentDirectory()}\\data\\";
                var fileName = $"{path}VerifyUrls.txt";
                return fileName;
            }
        }

        public static async Task AddToSaved(SnapshotModel model)
        {
            Saveds[model.Url] = model;
            UnSaveds.Remove(model.Url);
            await SaveAsync(model);
        }

        public static async Task AddToSaveds(IEnumerable<SnapshotModel> models)
        {
            var builder = new StringBuilder();
            foreach (var model in models)
            {
                try
                {
                    Saveds[model.Url] = model;
                    UnSaveds.Remove(model.Url);
                    var json = JsonConvert.SerializeObject(model, Formatting.None);
                    builder.AppendLine(json);
                }
                catch(Exception e)
                {
                    WinformLog.ShowLog(e.ToString());
                }
            }
            await SaveAsync(builder.ToString());
        }

        private enum SavedState
        {
            None,
            Saved,
            UnSaved,
        }

        public static async Task LoadAsync(List<string> fileNames)
        {
            if (fileNames == null)
                return;

            foreach (var file in fileNames)
            {
                await LoadFileAsync(file, SavedState.UnSaved);
            }
        }

        public static async Task LaodSavedFile()
        {
            //加载之前保存审核文件 
            await LoadFileAsync(FileName, SavedState.Saved);
        }

        public static List<SnapshotModel> GetPageList(int pageSize, int pageIndex)
        {
            if (pageIndex <= 0)
                pageIndex = 1;

            var skip = (pageIndex - 1) * pageSize;
            if (skip > AllSnapshots.Count)
                return new List<SnapshotModel>();

            var result = AllSnapshots.Skip(skip).Take(pageSize).ToList();
            return result;
        }

        public static int GetTotal()
        {
            return AllSnapshots.Count;
        }
        
        public static int GetLastChecked()
        {
            return AllSnapshots.FindLastIndex(a=>a.Checked);
        }

        public static int GetUnChecked()
        {
            return AllSnapshots.Where(a=>!a.Checked).Count();
        }

        public static int GetChecked()
        {
            return AllSnapshots.Where(a => a.Checked).Count();
        }

        public static bool GetSavedStatus(IEnumerable<SnapshotModel> models)
        {
            foreach(var model in models)
            {
                if (Saveds.ContainsKey(model.Url))
                    return true;
            }
            return false;
        }

        private static int Order;
        private static async Task LoadFileAsync(string fileName, SavedState state)
        {
            if (state == SavedState.None)
                return;

            if (!File.Exists(fileName))
                return;

            var orders = new Dictionary<string, SnapshotModel>();
            using (var sm = File.OpenRead(fileName))
            {
                sm.Position = 0;
                using (var sr = new StreamReader(sm))
                {
                    var str = await sr.ReadToEndAsync();
                    var datas = str.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var d in datas)
                    {
                        var model = JsonConvert.DeserializeObject<SnapshotModel>(d);

                        if (state == SavedState.Saved)
                        {
                            if (!Saveds.ContainsKey(model.Url))
                            {
                                Interlocked.Increment(ref Order);
                                model.Order = Order;
                            }

                            Saveds[model.Url] = model;
                            orders[model.Url] = model;
                        }
                        else if (state == SavedState.UnSaved)
                        {
                            if (Saveds.ContainsKey(model.Url))
                                continue;

                            if (UnSaveds.ContainsKey(model.Url))
                                continue;

                            UnSaveds[model.Url] = model;
                            orders[model.Url] = model;
                        }
                    }
                }
                AllSnapshots.AddRange(orders.Values.OrderBy(a=>a.Order));
            }
        }

        public static async Task SaveAsync(string data)
        {
            if (string.IsNullOrWhiteSpace(data))
                return;

            var path = $"{Directory.GetCurrentDirectory()}\\data\\";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            if (!await SaveTo(data, FileName))
                return;

            await SaveTo(data, BakFileName);
        }
        
        private static async Task<bool> SaveTo(string data, string fileName)
        {
            using (var sm = File.Open(fileName, FileMode.OpenOrCreate))
            {
                sm.Position = sm.Length;
                using (var sw = new StreamWriter(sm))
                {
                    try
                    {
                        await sw.WriteAsync(data);
                        await sw.FlushAsync();
                    }
                    catch
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public static async Task<bool> SaveAsync(SnapshotModel model)
        {
            var path = $"{Directory.GetCurrentDirectory()}\\data\\";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            if (!await SaveTo(model, FileName))
                return false;

            return await SaveTo(model, BakFileName);
        }

        private static async Task<bool> SaveTo(SnapshotModel model, string fileName)
        {
            using (var sm = File.Open(fileName, FileMode.OpenOrCreate))
            {
                sm.Position = sm.Length;
                using (var sw = new StreamWriter(sm))
                {
                    try
                    {
                        var json = JsonConvert.SerializeObject(model, Formatting.None);
                        await sw.WriteLineAsync(json);
                        await sw.FlushAsync();
                    }
                    catch
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
