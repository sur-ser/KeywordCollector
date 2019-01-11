using CollectorCore.Collector;
using CollectorCore.Entites;
using CollectorCore.Log;
using CollectorCore.Model;
using CollectorCore.Utils;
using CollectorCore.WebDriver;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CollectorCore.Snapshot
{
    public class SnapshotManager
    {
        public static List<int> OldChromeDriverProcessId { get; set; }
        public static List<int> OldChromeProcessId { get; set; }
        public static bool IsStarted { get; private set; }
        public static HashSet<string> AllUrls { get; } = new HashSet<string>();

        public static int MaxThread { get; set; }
        public static int CloseOfTasks = 200;
        private static int CurrentTasks;
        private static bool Exit = false;

        private static List<SnapshotGennerator> SnapshotGennerators { get; } = new List<SnapshotGennerator>();
        private static ConcurrentQueue<KeywordCollectModel> keywordCollectInfos { get; } = new ConcurrentQueue<KeywordCollectModel>();
        private static ConcurrentQueue<UrlEntity> UrlEntities { get; } = new ConcurrentQueue<UrlEntity>();

        public static void SaveAsSnapshot(List<KeywordCollectModel> items)
        {
            foreach (var item in items)
            {
                if (AllUrls.Add(item.OriginUrl))
                {
                    keywordCollectInfos.Enqueue(item);
                }
            }
        }

        public static async Task<bool> GetCompleteAsync()
        {
            var result = false;
            await Task.Run(() =>
            {
                while (true)
                {
                    Thread.Sleep(1);

                    if (keywordCollectInfos.Any())
                        continue;

                    if (SnapshotGennerators.Where(a => !a.Complete).Any())
                        continue;

                    result = true;
                    return;
                }
            });
            return result;
        }

        public static void Start()
        {
            if (IsStarted)
                return;

            IsStarted = true;

            OldChromeDriverProcessId = ProcessUtils.GetChromeDriverProcessId();
            OldChromeProcessId = ProcessUtils.GetChromeProcessId();

            for (var i = 0; i < MaxThread; i++)
            {
                var downloader = new SnapshotGennerator();
                SnapshotGennerators.Add(downloader);
                GennerateSnapshot(downloader);
            }

            WriteEntites();
        }

        private static int Index = 0;
        private static void GennerateSnapshot(SnapshotGennerator gennerator)
        {
            Task.Run(() =>
            {
                while (true)
                {
                    Thread.Sleep(1);

                    if (Exit)
                        return;

                    if (!keywordCollectInfos.TryDequeue(out KeywordCollectModel model))
                        continue;

                    Interlocked.Increment(ref CurrentTasks);
                    if (CurrentTasks > CloseOfTasks)
                    {
                        Interlocked.Exchange(ref CurrentTasks, 0);
                        KillWorkChromeProcess();
                        WinformLog.ShowLog("释放Chrome占用内存.");
                    }
                    gennerator.Complete = false;
                    var entity = gennerator.SaveSnapshotAndGennerateUrlEntity(model);
                    gennerator.Complete = true;
                    Interlocked.Increment(ref Index);
                    if (entity != null)
                    {
                        WinformLog.ShowLog($"第:{Index}条{model.OriginUrl}快照成功.");
                        UrlEntities.Enqueue(entity);
                    }
                    else
                    {
                        WinformLog.ShowLog($"第:{Index}条{model.OriginUrl}快照失败.");
                    }                    
                }
            });
        }

        private static void WriteEntites()
        {
            var list = new List<UrlEntity>();
            Task.Run(async()=>
            {
                while (true)
                {
                    Thread.Sleep(1);
                    if (Exit)
                        return;
                                        
                    while (UrlEntities.TryDequeue(out UrlEntity entity))
                    {
                        list.Add(entity);
                    }
                    await SnapshotWriter.WriteEntityAsync(list);
                    list.Clear();
                }
            });
        }

        private static void CloseAllDriverProcess()
        {
            try
            {
                var processIdAry = Process.GetProcessesByName("chromedriver");
                if (processIdAry.Count() > 0)
                {
                    for (int i = 0; i < processIdAry.Count(); i++)
                    {
                        var process = processIdAry[i];
                        if (!process.CloseMainWindow())
                        {
                            try
                            {
                                process.Kill();
                            }
                            catch { }
                        }
                    }
                }

                processIdAry = Process.GetProcessesByName("chrome");
                if (processIdAry.Count() > 0)
                {
                    for (int i = 0; i < processIdAry.Count(); i++)
                    {
                        var process = processIdAry[i];
                        if (!process.CloseMainWindow())
                        {
                            try
                            {
                                process.Kill();
                            }
                            catch { }
                        }
                    }
                }
            }
            catch { }
        }

        public static void Close()
        {
            foreach(var downloader in SnapshotGennerators)
            {
                downloader.Dispose();
            };
            CloseAllDriverProcess();
        }

        private static void KillWorkChromeProcess()
        {
            var processIdAry = Process.GetProcessesByName("chrome");
            if (processIdAry.Count() > 0)
            {
                for (int i = 0; i < processIdAry.Count(); i++)
                {
                    var process = processIdAry[i];
                    if (OldChromeProcessId.Contains(process.Id))
                        continue;

                    if (!process.CloseMainWindow())
                    {
                        try
                        {
                            process.Kill();
                        }
                        catch { }
                    }
                }
            }
        }
    }
}
