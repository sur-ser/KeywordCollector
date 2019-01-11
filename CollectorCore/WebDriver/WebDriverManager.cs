using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollectorCore.WebDriver
{
    public class WebDriverManager
    {
        public static bool IsHide { get; set; }
        public static bool IsHideCmd { get; set; }

        public static ChromeOptions GetOptions()
        {
            var options = new ChromeOptions();
            options.AddArgument("--headless");
            options.AddArgument("--window-size=1440,900");
            options.AddUserProfilePreference("profile", new { default_content_setting_values = new { images = 2, javascript = 2 } });

            return options;
        }
    }
}
