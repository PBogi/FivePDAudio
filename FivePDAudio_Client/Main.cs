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

            RegisterCommand("audio", new Action<int, List<object>, string>(CommandHandler),false);
 

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

        void CommandHandler(int source, List<object> args, string raw)
        {
            if (args.Count >= 2)
            {
                switch (args[0].ToString().ToLower())
                {
                    case "debug":
                        try
                        {
                            Settings.Debug = Convert.ToBoolean(args[1]);
                            OutputChat(new[] { 255, 255, 255 }, new[] { "[FivePDAudio] Set debug to " + args[1].ToString() + "; Debug messages should appear in the client console (F8)"});
                        }
                        catch
                        {
                            OutputChat(new[] { 255, 0, 0 }, new[] { "[FivePDAudio] Invalid value", "Needs to be true or false"});
                        }
                        break;

                    default:
                            OutputChat(new[] { 255, 0, 0 }, new[] { "[FivePDAudio] Invalid argument","Type /audio to see available commands" });
                        break;
                }
            }
            else
            {
                OutputChat(new[] { 255, 0, 0 }, new[] { "[FivePDAudio] Invalid argument count" });
                OutputChat(new[] { 255, 255, 255 }, new[] { "Available arguments:"});
                OutputChat(new[] { 255, 255, 255 }, new[] { "debug true/false"});
            }
        }

        void OutputChat(int[] messagecolor, string[] message)
        {
            BaseScript.TriggerEvent("chat:addMessage", new
            {
                color = messagecolor, //new[] { 255, 0, 0 },
                args = message //new[] { "Invalid value" }
            });
        }

        public static void OutputDebug(string message)
        {
            if(Settings.Debug == true)
            {
                Debug.WriteLine(message);
            }            
        }
    }
}
