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
        private const string DOWNLOAD_PATH = "test/sweetFx.7z";
        private readonly string targetProgramPath;
        private readonly Action<string> logFunc;
        public SweetFxDownloader(string targetPathRegistry, Action<string> logFunc)
        {
            this.logFunc = logFunc;
            var reg = Microsoft.Win32.Registry.LocalMachine;
            var pathList = targetPathRegistry.Split('/');
            pathList.Take(pathList.Length - 1).ToList().ForEach(p =>
            {
                reg = reg.OpenSubKey(p);
            });

            var path = reg.GetValue(pathList.Last());
            if (path != null && path.ToString().Length > 0)
            {
                targetProgramPath = path.ToString();
                log($"경로 획득 성공 : {targetProgramPath}");
            }
            else log($"경로 획득 실패");
        }

        public async void downloadStart()
        {
            log("SweetFx 파일을 다운로드중입니다...");
            var client = new WebClient();
            await client.DownloadFileTaskAsync(new Uri(URL), DOWNLOAD_PATH);
            log("SweetFx 파일을 다운로드했습니다");
            log("SweetFx 압축을 해제중입니다...");
            new SevenZipNET.SevenZipExtractor(DOWNLOAD_PATH).ExtractAll(targetProgramPath, true);
            log("SweetFx  압축을 해제했습니다.");
            client.Dispose();
        }

        private void log(string text)
        {
            logFunc.Invoke(text);
        }
    }
}
