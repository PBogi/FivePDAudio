using System;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using System.Collections.Generic;

namespace fivepdaudio
{
    public class Main : BaseScript
    {
        public Main()
        {
            // Read settings from json and playerprofile
            Settings.GetSettings();
            //Speech.SetVoice();

            /*RegisterCommand("setvoice", new Action<int, List<object>, string>((source, args, raw) =>
            {
                // Parameter: Gender, Type (cop,hwaycop,sheriff), color, variant
                //Speech.ChangeVoice(args);
            }), false);*/

            RegisterCommand("audio", new Action<int, List<object>, string>(Common.CommandHandler),false);
 

            // Register all Event handlers
            RegisterEventHandlers();

            Tick += OnTick;
        }

        private static async Task OnTick()
        {
            await AudioHandler.Play();
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
    }
}
