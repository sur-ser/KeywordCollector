using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KeywordCollector.Log
{
    public class WinformLog
    {
        private static TextBox LogText { get; set; }
        private static string Text
        {
            get
            {
                return LogText.Text;
            }
            set
            {
                LogText.Text = value;
            }
        }

        public static void SetUp(TextBox textBox)
        {
            LogText = textBox;
        }


        public static void ShowLog(string logContent)
        {
            Text += logContent;
            Text += Environment.NewLine;
        }

        public static void Clear()
        {
            Text = string.Empty;
        }
    }
}
