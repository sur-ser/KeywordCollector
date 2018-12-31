using KeywordCollector.Collector;
using KeywordCollector.Entites;
using KeywordCollector.WebDriver;
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

namespace KeywordCollector.Thumbnail
{
    public class SnapshotManager
    {
        public static bool Exit = false;
        public static List<int> OldChromeDriverProcessId { get; set; }
        private static ConcurrentQueue<ResultItem> ItemQueue { get; } = new ConcurrentQueue<ResultItem>();
        public static ConcurrentDictionary<ResultItem, UrlEntity> UrlEntitys { get; } = new ConcurrentDictionary<ResultItem, UrlEntity>();
        static SnapshotManager()
        {
            Working();
        }

        public static void SaveAsThumbnail(ResultItem item)
        {
            ItemQueue.Enqueue(item);
        }

        private class ChromeTaskItem
        {
            public ChromeDriver ChromeDriver { get; set; }
            public IWebDriver CurrentWebDriver { get; set; }
            public string CurrentHandler { get; set; }
            public ResultItem ResultItem { get; set; }
            public string FullName { get; set; }
            public string FileName { get; set; }
            public bool IsOk { get; set; }
            public string CurrentUrl { get; set; }
            public DateTime HandleTime { get; set; }
            public string DefHandler { get; set; }
            public bool IsCancel { get; set; }
            public bool SaveFile()
            {
                var useTime = (DateTime.Now - this.HandleTime).TotalMilliseconds;
                if (useTime < 15000)
                    return false;

                try
                {
                    this.CurrentWebDriver = GetWebDriver();

                    if (this.IsCancel)
                        return false;

                    Save();

                    if (this.IsCancel)
                        return false;

                    this.CurrentUrl = this.ChromeDriver.Url;
                    this.IsOk = true;
                }
                catch(Exception e)
                {
                    if (e is NoSuchWindowException)
                    {
                        SwitchToDef();
                        return false;
                    }
                }
                return true;
            }

            private void SwitchToDef()
            {
                var task = Task.Run(() =>
                {
                    try
                    {
                        ChromeDriver.SwitchTo().Window(this.DefHandler);
                    }
                    catch { }

                });
                if (!task.Wait(2000))
                {
                    this.IsCancel = true;
                }
            }

            private IWebDriver GetWebDriver()
            {
                IWebDriver result = null;
                var task = Task.Run(() =>
                {
                    try
                    {
                        result = this.ChromeDriver.SwitchTo().Window(this.CurrentHandler);
                    }
                    catch { }
                });
                if (!task.Wait(2000))
                {
                    this.IsCancel = true;
                }
                return result;
            }

            private void Save()
            {
                var task = Task.Run(() =>
                {
                    try
                    {
                        this.ChromeDriver.GetScreenshot().SaveAsFile(this.FullName, ScreenshotImageFormat.Png);
                    }
                    catch { }
                });
                if (!task.Wait(8000))
                {
                    this.IsCancel = true;
                }
            }

        }

        private class TaskClass : IDisposable
        {
            public int MaxTask { get; set; }
            public int CompletedTasks{get;set;}
            public int CurrentTasks { get; set; }
            public bool IsCompleted { get; set; }
            public bool IsStart { get; set; }
            private ConcurrentDictionary<string, ChromeTaskItem> Tasks { get; } = new ConcurrentDictionary<string, ChromeTaskItem>();
            private ChromeDriver ChromeDriver { get; set; }
            private string DefHandler { get; set; }

            public TaskClass()
            {
                OldChromeDriverProcessId = ProcessUtils.GetChromeDriverProcessId();

                InitWebDriver();
            }

            private void InitWebDriver()
            {
                if (WebDriverManager.IsHide)
                {
                    var options = WebDriverManager.GetOptions();
                    this.ChromeDriver = new ChromeDriver(options);
                }
                else
                {
                    this.ChromeDriver = new ChromeDriver();
                }
                this.DefHandler = this.ChromeDriver.WindowHandles[0];
            }

            public void Dispose()
            {
                try
                {
                    if (!Quit())
                    {
                        try
                        {
                            var processIdAry = Process.GetProcessesByName("chromedriver");
                            if (processIdAry.Count() > 0)
                            {
                                for (int i = 0; i < processIdAry.Count(); i++)
                                {
                                    var process = processIdAry[i];
                                    if (OldChromeDriverProcessId.Contains(process.Id))
                                        continue;

                                    process.CloseMainWindow();
                                }
                            }
                        }
                        catch{}
                    }
                }
                catch { }
            }

            private bool Quit()
            {
                bool result = false;
                var task = Task.Run(() =>
                {
                    try
                    {
                        ChromeDriver.Quit();
                        result = true;
                    }
                    catch { }
                });
                if (!task.Wait(3000))
                {
                    result = false;
                }
                return result;
            }

            private void Reload()
            {
                Dispose();

                InitWebDriver();

                var list = this.Tasks.Values.ToList();
                this.Tasks.Clear();
                foreach (var t in list)
                {
                    Thread.Sleep(100);
                    AddTask(t, 0);
                }
                this.CurrentTasks = this.MaxTask;
            }

            public void AddTask(ChromeTaskItem task, int retry)
            {
                try
                {
                    this.ChromeDriver.ExecuteScript($"window.open('{task.ResultItem.OriginUrl}','_blank');");
                }
                catch (Exception e)
                {
                    if (e is NoSuchWindowException)
                    {
                        ChromeDriver.SwitchTo().Window(this.DefHandler);
                        retry++;
                        if (retry > 3)
                            return;

                        AddTask(task, retry);
                    }
                }

                var handle = ChromeDriver.WindowHandles[ChromeDriver.WindowHandles.Count - 1];
                task.HandleTime = DateTime.Now;
                task.ChromeDriver = this.ChromeDriver;
                task.CurrentHandler = handle;
                task.DefHandler = this.DefHandler;
                Tasks[handle] = task;
                CurrentTasks++;
            }

            public void Update()
            {
                this.IsStart = true;

                bool reload = false;
                foreach (var kv in this.Tasks)
                {
                    var value = kv.Value;
                    var key = kv.Key;
                    if (value.SaveFile())
                    {
                        if (value.IsOk)
                        {
                            UrlEntitys.TryAdd(value.ResultItem, new UrlEntity
                            {
                                engineType = value.ResultItem.SearchType,
                                CollectIndex = value.ResultItem.Index,
                                Url = value.CurrentUrl,
                                ThumbnailFileName = value.FileName,
                            });
                        }

                        try
                        {
                            value.CurrentWebDriver.Close();
                        }
                        catch { }

                        Tasks.TryRemove(kv.Key, out ChromeTaskItem val);
                        CompletedTasks++;

                        continue;
                    }

                    if (kv.Value.IsCancel)
                    {
                        Tasks.TryRemove(kv.Key, out ChromeTaskItem val);
                        CompletedTasks++;
                        reload = true;
                        break;
                    }
                }

                if (reload)
                {
                    this.Reload();
                    return;
                }

                if(this.Tasks.Count == 0)
                {
                    this.IsCompleted = true;
                    this.Dispose();
                }
            }
        }

        private static void Working()
        {
            TaskClass task = null;
            Task.Run(() =>
            {
                while (!Exit)
                {
                    Thread.Sleep(100);
                    if (!ItemQueue.TryPeek(out ResultItem item))
                    {
                        continue;
                    }

                    var maxTask = ItemQueue.Count > 15 ? 15 : ItemQueue.Count;

                    task = task ?? new TaskClass() { MaxTask = maxTask };
                    if(task.CurrentTasks == task.MaxTask)
                    {
                        task.Update();

                        if (!task.IsCompleted)
                            continue;
                    }

                    if (task.IsCompleted)
                    {
                        task = new TaskClass() { MaxTask = maxTask };
                        continue;
                    }

                    if (task.IsStart)
                    {
                        continue;
                    }

                    try
                    {
                        var fileName = $"{item.OriginUrl.GetHashCode()}{item.Index}{item.SearchType}.png";
                        var path = $"{Directory.GetCurrentDirectory()}\\images\\";
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        var fullName = $"{path}{fileName}";
                        if (File.Exists(fullName))
                        {
                            File.Delete(fullName);
                        }

                        var chromeItem = new ChromeTaskItem
                        {
                            ResultItem = item,
                            FullName = fullName,
                            FileName = fileName,
                        };
                        task.AddTask(chromeItem, 0);
                        ItemQueue.TryDequeue(out item);
                    }
                    catch{}

                }
            });
        }


        public static void Close()
        {
            Exit = true;
        }

        // 按比例缩放图片
        public static Image ZoomPicture(Image SourceImage, int TargetWidth, int TargetHeight)
        {

            //新的图片宽
            int IntWidth;
            //新的图片高
            int IntHeight;
            try
            {
                System.Drawing.Imaging.ImageFormat format = SourceImage.RawFormat;
                System.Drawing.Bitmap SaveImage = new System.Drawing.Bitmap(TargetWidth, TargetHeight);
                Graphics g = Graphics.FromImage(SaveImage);
                g.Clear(Color.White);


                //宽度比目的图片宽度大，长度比目的图片长度小
                if (SourceImage.Width > TargetWidth && SourceImage.Height <= TargetHeight)
                {
                    IntWidth = TargetWidth;
                    IntHeight = (IntWidth * SourceImage.Height) / SourceImage.Width;
                }
                //宽度比目的图片宽度小，长度比目的图片长度大
                else if (SourceImage.Width <= TargetWidth && SourceImage.Height > TargetHeight)
                {
                    IntHeight = TargetHeight;
                    IntWidth = (IntHeight * SourceImage.Width) / SourceImage.Height;
                }
                //长宽比目的图片长宽都小
                else if (SourceImage.Width <= TargetWidth && SourceImage.Height <= TargetHeight)
                {
                    IntHeight = SourceImage.Width;
                    IntWidth = SourceImage.Height;
                }
                //长宽比目的图片的长宽都大
                else
                {
                    IntWidth = TargetWidth;
                    IntHeight = (IntWidth * SourceImage.Height) / SourceImage.Width;
                    if (IntHeight > TargetHeight)
                    {
                        IntHeight = TargetHeight;
                        IntWidth = (IntHeight * SourceImage.Width) / SourceImage.Height;
                    }
                }

                g.DrawImage(SourceImage, (TargetWidth - IntWidth) / 2, (TargetHeight - IntHeight) / 2, IntWidth, IntHeight);
                SourceImage.Dispose();

                return SaveImage;
            }
            catch { }

            return null;
        }
    }
}
