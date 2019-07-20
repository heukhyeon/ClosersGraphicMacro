using Microsoft.VisualBasic.FileIO;
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
        private const string DOWNLOAD_DIR = "temp";
        private string DOWNLOAD_PATH = $"{DOWNLOAD_DIR}/sweetFx.7z";
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
            log("Reshade 설정을 추가중입니다...");
            new SevenZipNET.SevenZipExtractor("reshade.zip").ExtractAll(DOWNLOAD_DIR, true);
            FileSystem.CopyDirectory(DOWNLOAD_DIR, targetProgramPath, UIOption.OnlyErrorDialogs);
            Directory.Delete(DOWNLOAD_DIR, true);
            log("Reshade 설정을 추가했습니다");
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
