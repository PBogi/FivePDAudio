using Newtonsoft.Json.Linq;
using static CitizenFX.Core.Native.API;

namespace fivepdaudio
{
    class Settings
    {
        public static bool Debug = false;
        public static float SoundVolume = 0.5f;

        // Read settings from json and playerprofile
        public static void GetSettings()
        {
            SetVolume(GetResourceKvpInt("SoundVolume")); 

            //Load callouts into AudioLibrary
            AudioLibrary.configuredCallouts = JObject.Parse(LoadResourceFile("fivepdaudio", "callouts.json"));
            Common.DebugMessage("Loaded callouts.json");
        }

        public static void SetVolume(int newVolume)
        {
            // Save new setting
            SetResourceKvpInt("SoundVolume", newVolume);

            // Calculate actual volume
            float VolumeSetting = 50;
            if (newVolume > 0)
            {
                VolumeSetting = newVolume;
            }
            float ProfileVolume = GetProfileSetting(300); // 0? - 10 <stat Name="_PROFILE_SETTING_300" Type="profilesetting" profile="true" FlushPriority="15" ProfileSettingId="300" Comment="AUDIO_SFX_LEVEL - 300" />

            SoundVolume = (ProfileVolume / 10) * (VolumeSetting / 100) * 0.75f;

            Common.ChatMessage(new[] { 255, 255, 255 }, new[] { "[FivePDAudio] Volume set to " + VolumeSetting + "%" });
           
        }
    }
}
