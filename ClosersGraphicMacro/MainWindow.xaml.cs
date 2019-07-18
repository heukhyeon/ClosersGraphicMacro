using ClosersGraphicMacro.src;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ClosersGraphicMacro
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        private String logText = "대기중";

        public MainWindow()
        {
            InitializeComponent();
            NvAPIWrapper.NVIDIA.Initialize();
            log("");
        }

        private void startButtonClick(object sender, RoutedEventArgs e)
        {
            log("클릭!");
            var api = new NvApiConnect("Feng Yin Zhe / Closers", this.log);
            //api.antiAliasGet();
            api.antiAliasSet();
            api.Dispose();
            new SweetFxDownloader("SOFTWARE/WOW6432Node/Nexon/Closers/RootPath", this.log);
        }

        private void log(String text)
        {
            text_log.Dispatcher.Invoke(()=>{
                logText += $"{text}\n";
                text_log.Text = logText;
            });
        }
    }
}
