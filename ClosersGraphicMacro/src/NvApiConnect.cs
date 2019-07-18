using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NvAPIWrapper;
using NvAPIWrapper.DRS;

namespace ClosersGraphicMacro.src
{
    class NvApiConnect : IDisposable
    {
        private readonly string name;
        private readonly Action<string> logFunc;
        private readonly DriverSettingsProfile profile;
        private readonly DriverSettingsSession session = DriverSettingsSession.CreateAndLoad();
        public readonly bool enabled;
        private const uint ID_ANTIALIAS_MODE = 276757595;
        private const uint ID_ANTIALIAS_SETTING = 282555346;
        private const uint ID_ATOMIC_FILTERING = 270426537;
        private const uint ID_ATOMIC_FILTERING_MODE = 282245910;
        private const uint ID_TEXTURE_QUALITY = 13510289;

        private const uint ANTIALIAS_MODE_MAX = 2;
        private const uint ANTIALIAS_SETTING_MAX = 37;
        private const uint ATOMIC_FILTERING_MAX = 16;
        private const uint ATOMIC_FILTERING_MODE = 1;
        private const uint TEXTURE_QUALITY_MAX = 4294967286;

        public NvApiConnect(string name, Action<string> logFunc)
        {
            this.name = name;
            this.logFunc = logFunc;
            try
            {
                this.profile = session.FindProfileByName(name);
                log($"프로세스 감지 완료 : {name}");
                var gpuList = NvAPIWrapper.GPU.PhysicalGPU.GetPhysicalGPUs();
                enabled = gpuList.Length > 0;
                if (gpuList.Length == 0)
                {
                    log("Nvidia 그래픽카드가 감지되지 않습니다");
                }
                else gpuList.ToList().ForEach(gpu =>
                {
                    log($"Nvidia 그래픽카드 감지 : {gpu.FullName}");
                });
            }
            catch(Exception e)
            {
                log("Nvidia 그래픽카드 설정 초기화중 에러 발생");
                log(e.StackTrace);
                enabled = false;
            }
           
        }

        public void antiAliasGet()
        {
            profile.Settings.Where(item => item.SettingInfo.Name?.Length > 0).ToList().ForEach(item =>
              {
                  log($"Name : {item.SettingInfo.Name}");
                  log($"Id : {item.SettingId}");
                  log($"Value : {item.CurrentValue}");
                  log("");
              });
            log("----------------------------------------------------");
        }

        public void antiAliasSet()
        {
            setSetting(ID_ANTIALIAS_MODE, ANTIALIAS_MODE_MAX, "안티앨리어스 모드");
            setSetting(ID_ANTIALIAS_SETTING, ANTIALIAS_SETTING_MAX, "안티앨리어스 설정");
            setSetting(ID_ATOMIC_FILTERING, ATOMIC_FILTERING_MAX, "이방성 필터링");
            setSetting(ID_ATOMIC_FILTERING_MODE, ATOMIC_FILTERING_MODE, "이방성 필터링 설정");
            setSetting(ID_TEXTURE_QUALITY, TEXTURE_QUALITY_MAX, "텍스처 필터링 - 품질");
        }

        private void log(string text)
        {
            logFunc.Invoke(text);
        }

        private ProfileSetting getSetting(int id)
        {
            return profile.GetSetting((uint)id);
        }

        private void setSetting(uint id, uint value, string titleName)
        {
        
            try
            {
                profile.SetSetting(id, value);
                log($"{titleName} 값 지정 성공");
            }
            catch(Exception e)
            {
                log($"{titleName} 값 지정 실패");
                log(e.StackTrace);
            }
        }

        public void Dispose()
        {
            session.Save();
            session.Dispose();
        }
    }
}
