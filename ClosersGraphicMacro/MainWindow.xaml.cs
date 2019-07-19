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

        private async void startProcess(object sender, RoutedEventArgs e)
        {
            log("클릭!");
            using (var api = new NvApiConnect("Feng Yin Zhe / Closers", this.log))
            {
                if (api.enabled) api.antiAliasSet();
                else return;
            };
            var downloader = new SweetFxDownloader("SOFTWARE/WOW6432Node/Nexon/Closers/RootPath", this.log);
            if (downloader.enabled) await downloader.downloadStart();
            else return;
            downloader.setSweetFxSetting("SweetFX_settings.txt");

        }

        private void log(String text)
        {
            text_log.Dispatcher.Invoke(() =>
            {
                logText += $"{text}\n";
                text_log.Text = logText;
            });
        }

        private void showLicense(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("SweetFx 기본 설정 원본 : https://gall.dcinside.com/board/view?id=closers&no=2835729", "SweetFx Setting", MessageBoxButton.OK, MessageBoxImage.Information);

            System.Diagnostics.Process.Start("https://github.com/heukhyeon/ClosersGraphicMacro/blob/master/LICENSE.txt");

        }
    }
}
