using KeywordCollector.Collector;
using KeywordCollector.Entites;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KeywordCollector.Thumbnail
{
    public class ThumbnailFileManager
    {
        public static bool Exit = false;

        //根据网速与电脑配置设置，配置高与网速快可以配多点drever实例
        public static int DriversOfStart = 3;

        private static List<ChromeDriver> ChromeDrivers { get; } = new List<ChromeDriver>();
        private static ConcurrentQueue<ResultItem> ItemQueue { get; } = new ConcurrentQueue<ResultItem>();
        public static ConcurrentDictionary<ResultItem, UrlEntity> UrlEntitys { get; } = new ConcurrentDictionary<ResultItem, UrlEntity>();
        static ThumbnailFileManager()
        {
            for(var i = 0; i < DriversOfStart; i++)
            {
                var driver = new ChromeDriver();
                ChromeDrivers.Add(driver);
                Working(driver);
            }
        }

        public static void SaveAsThumbnail(ResultItem item)
        {
            ItemQueue.Enqueue(item);
        }
               
        private static async void Working(ChromeDriver driver)
        {
            //设置5秒钟超时
            driver.Manage().Timeouts().PageLoad = new TimeSpan(0,0,0,0,5000);
            await Task.Run(() =>
            {
                while (!Exit)
                {
                    if (!ItemQueue.TryDequeue(out ResultItem item))
                    {
                        Thread.Sleep(100);
                        continue;
                    }
                    try
                    {
                        try
                        {
                            driver.Navigate().GoToUrl(item.OriginUrl);
                        }
                        catch { }
    
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
                        driver.GetScreenshot().SaveAsFile(fullName, OpenQA.Selenium.ScreenshotImageFormat.Png);

                        UrlEntitys.TryAdd(item, new UrlEntity
                        {
                            engineType = item.SearchType,
                            CollectIndex = item.Index,
                            Url = driver.Url,
                            ThumbnailFileName = fileName,
                        });
                    }
                    catch{ }
                }
            });
        }

        public static void Close()
        {
            Exit = true;
            foreach (var dv in ChromeDrivers)
            {
                try
                {
                    dv.Close();
                    dv.Quit();
                }
                catch { }
            }
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
