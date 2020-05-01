using Newtonsoft.Json.Linq;
using static CitizenFX.Core.Native.API;

namespace fivepdaudio
{
    class Settings
    {
        public static bool Debug = false;
        public static float SoundVolume = 0.75f;

        // Read settings from json and playerprofile
        public static void GetSettings()
        {
            float ProfileVolume = GetProfileSetting(300) / 10; // 0? - 10 <stat Name="_PROFILE_SETTING_300"    Type="profilesetting"  profile="true"  FlushPriority="15"  ProfileSettingId="300"  Comment="AUDIO_SFX_LEVEL - 300" /><
            SoundVolume = ProfileVolume * 0.5f;
            Main.OutputDebug("Audio Volume set to " + Settings.SoundVolume);

            //Load callouts into AudioLibrary
            AudioLibrary.configuredCallouts = JObject.Parse(LoadResourceFile("fivepdaudio", "callouts.json"));
            Main.OutputDebug("Loaded callouts.json");
        }
    }
}
