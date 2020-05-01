using System;
using System.Collections.Generic;
using CitizenFX.Core;

namespace fivepdaudio
{
    class Common
    {
        public static void CommandHandler(int source, List<object> args, string raw)
        {
            if (args.Count >= 2)
            {
                switch (args[0].ToString().ToLower())
                {
                    case "debug":
                        try
                        {
                            Settings.Debug = Convert.ToBoolean(args[1]);
                            ChatMessage(new[] { 255, 255, 255 }, new[] { "[FivePDAudio] Set debug to " + args[1].ToString() + "; Debug messages should appear in the client console (F8)" });
                        }
                        catch
                        {
                            ChatMessage(new[] { 255, 0, 0 }, new[] { "[FivePDAudio] Invalid value", "Needs to be true or false" });
                        }
                        break;

                    default:
                        ChatMessage(new[] { 255, 0, 0 }, new[] { "[FivePDAudio] Invalid argument", "Type /audio to see available commands" });
                        break;
                }
            }
            else
            {
                ChatMessage(new[] { 255, 0, 0 }, new[] { "[FivePDAudio] Invalid argument count" });
                ChatMessage(new[] { 255, 255, 255 }, new[] { "Available arguments:" });
                ChatMessage(new[] { 255, 255, 255 }, new[] { "debug true/false" });
            }
        }

        public static void ChatMessage(int[] messagecolor, string[] message)
        {
            BaseScript.TriggerEvent("chat:addMessage", new
            {
                color = messagecolor, //new[] { 255, 0, 0 },
                args = message //new[] { "Invalid value" }
            });
        }

        public static void DebugMessage(string message)
        {
            if (Settings.Debug == true)
            {
                Debug.WriteLine(message);
            }
        }
    }
}
