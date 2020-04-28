using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;

namespace fivepdaudio
{
    public class Main : BaseScript
    {

        public Main()
        {
            // Read settings from json and playerprofile
            GetSettings();
            //Speech.SetVoice();

            /*RegisterCommand("setvoice", new Action<int, List<object>, string>((source, args, raw) =>
            {
                // Parameter: Gender, Type (cop,hwaycop,sheriff), color, variant
                //Speech.ChangeVoice(args);
            }), false);*/


            // Register all Event handlers
            RegisterEventHandlers();


            AudioHandler soundHandler = new AudioHandler();
            Tick += soundHandler.Play;
            Tick += onTick;
        }

        private static async Task onTick()
        {
            /*if (IsControlJustPressed(0, 38))
            {
                Speech.StopPed();
                // 38 	INPUT_PICKUP 	E 	LB
            }*/
        }

        void RegisterEventHandlers()
        {
            // Register Audio API Event Handlers
            EventHandlers["FivePDAudio::RegisterCallout"] += new Action<string, string>(AudioLibrary.RegisterCalloutAudio);
            EventHandlers["FivePDAudio::DispatchPlay"] += new Action<string>(Dispatch.AddToDispatchQueue);

            // Register NUI Callback Event handler
            RegisterNuiCallbackType("FinishedPlaying");
            EventHandlers["__cfx_nui:FinishedPlaying"] += new Action(AudioHandler.FinishedPlaying);

            /*****************\
            |  FivePD Events  |
            \* ***************/
            // Receive Callout
            EventHandlers["FIVEPD::CalloutManager::sendCalloutToComputer"] += new Action<string, string, int, string, string>(Dispatch.ReceiveCalloutInformation);
            // End Callout
            //EventHandlers["FIVEPD::CalloutManager::completeCallout"] += new Action<string>(Dispatch.CalloutEnded);
            // Receive Backup Request
            EventHandlers["nuiReceiveAssistanceRequiredCallout"] += new Action<string, int, string, int, int>(Dispatch.ReceiveBackupRequestCallout);
            EventHandlers["nuiReceiveAssistanceRequired"] += new Action<string, int, int, int>(Dispatch.ReceiveBackupRequest);
            // End Backup Request
            EventHandlers["nuiDeleteAssistanceRequired"] += new Action<string, int>(Dispatch.EndBackupRequest);

        }

        void GetSettings()
        {
            float ProfileVolume = GetProfileSetting(300) / 10; // 0? - 10 <stat Name="_PROFILE_SETTING_300"    Type="profilesetting"  profile="true"  FlushPriority="15"  ProfileSettingId="300"  Comment="AUDIO_SFX_LEVEL - 300" /><
            AudioHandler.soundVolume = ProfileVolume * 0.5f;
        }
    }
}
