using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ClosersGraphicMacro.src
{
    class SweetFxDownloader
    {
        private const string URL = "http://sfx.thelazy.net/static/media/downloads/SweetFX_1_5-23364.7z";
        private readonly string targetProgramPath;
        private readonly Action<string> logFunc;
        public SweetFxDownloader(string targetPathRegistry, Action<string> logFunc)
        {
            this.logFunc = logFunc;
            var reg = Microsoft.Win32.Registry.LocalMachine;
            var path = reg.OpenSubKey(targetPathRegistry);
            if (path != null) targetProgramPath = path.ToString();
            else log($"경로 획득 성공 : {path}");
        }

        public async void downloadStart()
        {
            log("SweetFx 파일을 다운로드중입니다...");
            var client = new WebClient();
            await client.DownloadFileTaskAsync(new Uri(URL), "test/sweetFx.7z");
            log("SweetFx 파일을 다운로드했습니다");
            client.Dispose();
        }

        private void log(string text)
        {
            logFunc.Invoke(text);
        }
    }
}
