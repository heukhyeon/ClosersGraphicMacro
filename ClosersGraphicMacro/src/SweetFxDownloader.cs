using System;
using System.Collections.Generic;
using System.IO;
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
        private string targetSweetFxPath
        {
            get
            {
                return $"{targetProgramPath}/SweetFX_settings.txt";
            }
            
        }
        public readonly bool enabled;

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
            enabled = path != null && path.ToString().Length > 0;
            if (enabled)
            {
                targetProgramPath = path.ToString();
                log($"경로 획득 성공 : {targetProgramPath}");
            }
            else log($"경로 획득 실패");
            reg.Dispose();
        }

        public async Task downloadStart()
        {
            log("SweetFx 파일을 다운로드중입니다...");
            var client = new WebClient();
            await client.DownloadFileTaskAsync(new Uri(URL), DOWNLOAD_PATH);
            log("SweetFx 파일을 다운로드했습니다");
            log("SweetFx 압축을 해제중입니다...");
            new SevenZipNET.SevenZipExtractor(DOWNLOAD_PATH).ExtractAll(targetProgramPath, true);
            log("SweetFx 압축을 해제했습니다.");
            client.Dispose();
        }

        public void setSweetFxSetting(string predefinedFilePath)
        {
            log("SweetFx 설정을 갱신하는 중입니다...");
            if (!File.Exists(predefinedFilePath))
            {
                log("참조할 SweetFx 설정파일이 없습니다. 실행파일과 동일한 위치에 다음 파일을 추가해주세요 : SweetFX_settings.txt");
                return;
            }
            var file = File.OpenText(predefinedFilePath);
            var setting = file.ReadToEnd();
            file.Dispose();
            File.WriteAllText(targetSweetFxPath, setting);
            log("SweetFx 설정을 갱신했습니다");
        }

       
        private void log(string text)
        {
            logFunc.Invoke(text);
        }
    }
}
