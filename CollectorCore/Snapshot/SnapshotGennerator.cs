using CollectorCore.Collector;
using CollectorCore.Entites;
using CollectorCore.Log;
using CollectorCore.Model;
using CollectorCore.WebDriver;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CollectorCore.Snapshot
{
    public class SnapshotGennerator :IDisposable
    {
        private ChromeDriver WebDriver { get; set; }
        private ChromeDriverService Services { get; set; }
        private bool Exit { get; set; }
        public bool Complete { get; set; }

        private void CreateWebDriver()
        {
            try
            {
                if (WebDriverManager.IsHide)
                {
                    var options = WebDriverManager.GetOptions();
                    if (this.Services != null)
                    {
                        this.Services.Dispose();
                    }
                    if (WebDriverManager.IsHideCmd)
                    {
                        this.Services = ChromeDriverService.CreateDefaultService();
                        this.Services.HideCommandPromptWindow = WebDriverManager.IsHideCmd;
                        this.WebDriver = new ChromeDriver(this.Services, options);
                    }
                    else
                    {
                        this.WebDriver = new ChromeDriver(options);
                    }
                }
                else
                {
                    this.WebDriver = new ChromeDriver();
                }

                this.WebDriver.Manage().Timeouts().PageLoad = TimeSpan.FromMinutes(6);
            }
            catch { }
        }

        public UrlEntity SaveSnapshotAndGennerateUrlEntity(KeywordCollectModel collectModel)
        {
            if (this.WebDriver == null)
                this.CreateWebDriver();

            UrlEntity result = null;
            int retry = 0;
            while (true)
            {
                try
                {
                    if (Exit)
                        break;

                    this.WebDriver.Navigate().GoToUrl(collectModel.OriginUrl);

                    var fileName = $"{collectModel.OriginUrl.GetHashCode()}{collectModel.Index}{collectModel.SearchType}.png";

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
                    this.WebDriver.GetScreenshot().SaveAsFile(fullName, ScreenshotImageFormat.Png);

                    result = new UrlEntity
                    {
                        engineType = collectModel.SearchType,
                        CollectIndex = collectModel.Index,
                        Url = this.WebDriver.Url,
                        ThumbnailFileName = fileName,
                    };
                    break;
                }
                catch (Exception e)
                {
                    try
                    {
                        this.WebDriver.Close();
                    }
                    catch { }
                    this.CreateWebDriver();
                    retry++;
                    if (retry >= 2)
                    {
                        break;
                    }
                }
            }
            return result;
        }

        public void Dispose()
        {
            try
            {
                Exit = true;
            }
            catch { }
        }
    }
}
