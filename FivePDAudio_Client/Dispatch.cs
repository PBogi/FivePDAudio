using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;

namespace fivepdaudio
{
    class Dispatch
    {
        static Random random = new Random();

        public static List<string[]> dispatchQueue = new List<string[]>();

        // New callout
        public static void ReceiveCalloutInformation(string ShortName, string Address, int ResponseCode, string Description, string Identifier)
        {
            Common.DebugMessage("Received callout information");
            List<string> soundFiles = new List<string>();
            List<string> SearchFiles = new List<string>();

            // Dispatch Intro
            /*soundFiles.Add(@"EFFECTS/INTRO_01.ogg");
            List<string> SearchFiles = AudioLibrary.availableAudio.Where(x => x.StartsWith(@"ATTENTION_ALL_UNITS_GEN/ATTENTION_ALL_UNITS_GENERIC_")).ToList();
            soundFiles.Add(SearchFiles[random.Next(0, SearchFiles.Count)]);*/

            soundFiles = soundFiles.Concat(DispatchIntro(ResponseCode)).ToList();

            // Search for registered callouts first, then check callouts.json
            if (AudioLibrary.registeredCrimeAudio.ContainsKey(ShortName))
            {
                SearchFiles = AudioLibrary.availableAudio.Where(x => x.StartsWith(@"WE_HAVE/CITIZENS")).ToList();
                SearchFiles.AddRange(AudioLibrary.availableAudio.Where(x => x.StartsWith(@"WE_HAVE/WE")));
                soundFiles.Add(SearchFiles[random.Next(0, SearchFiles.Count)]);

                SearchFiles = AudioLibrary.availableAudio.Where(x => x.StartsWith(AudioLibrary.registeredCrimeAudio[ShortName], StringComparison.OrdinalIgnoreCase)).ToList();
                soundFiles.Add(SearchFiles[random.Next(0, SearchFiles.Count)]);
            }
            else if (AudioLibrary.configuredCallouts.ContainsKey(ShortName))
            {
                SearchFiles = AudioLibrary.availableAudio.Where(x => x.StartsWith(@"WE_HAVE/CITIZENS")).ToList();
                SearchFiles.AddRange(AudioLibrary.availableAudio.Where(x => x.StartsWith(@"WE_HAVE/WE")));
                soundFiles.Add(SearchFiles[random.Next(0, SearchFiles.Count)]);

                SearchFiles = AudioLibrary.availableAudio.Where(x => x.StartsWith((string)AudioLibrary.configuredCallouts[ShortName], StringComparison.OrdinalIgnoreCase)).ToList();
                soundFiles.Add(SearchFiles[random.Next(0, SearchFiles.Count)]);
            }

            // Only play if Response Code is greater than 1, as there is no audio for that
            if (ResponseCode > 1)
            {
                soundFiles.Add(@"DISPATCH_RESPOND_CODE/RESPOND_CODE_" + ResponseCode.ToString() + ".ogg");
            }
            soundFiles.Add(@"EFFECTS/OUTRO_01.ogg");

            // Done, let's add it the the queue
            Common.DebugMessage("Finished creating playlist, adding it to dispatch queue");
            dispatchQueue.Add(soundFiles.ToArray());
        }

        // Callout ended (currently unused)
        public static void CalloutEnded(string Identifier)
        {
            Common.DebugMessage("Received code 4, adding it to dispatch queue");
            dispatchQueue.Add(new string[] { @"STAND_DOWN/ALL_UNITS_CODE_4.ogg" });
        }

        // Backup requests
        public static async void ReceiveBackupRequest(string CallSign, int departmentID, int playerNetworkID, int ResponseCode)
        {
            Common.DebugMessage("Received backup information");
            List<string> soundFiles = new List<string>();
            soundFiles.Add(@"EFFECTS/INTRO_01.ogg");
            List<string> SearchFiles = AudioLibrary.availableAudio.Where(x => x.StartsWith(@"ATTENTION_ALL_UNITS_GEN/ATTENTION_ALL_UNITS_GENERIC_")).ToList();
            soundFiles.Add(SearchFiles[random.Next(0, SearchFiles.Count)]);

            if (ResponseCode == 99)
            {
                Common.DebugMessage("Code 99!");
                SearchFiles = AudioLibrary.availableAudio.Where(x => x.StartsWith(@"OFFICER_REQUESTS_BACKUP/CODE99")).ToList();
                soundFiles.Add(SearchFiles[random.Next(0, SearchFiles.Count)]);
                soundFiles.Add(@"EFFECTS/OUTRO_01.ogg");

                await AudioHandler.PlayCode99(soundFiles.ToArray());
            }
            else
            {
                soundFiles.Add(@"OFFICER_REQUESTS_BACKUP/OFFICER_REQUESTING_BACKUP.ogg");
                if (ResponseCode > 1)
                {
                    soundFiles.Add(@"DISPATCH_RESPOND_CODE/RESPOND_CODE_" + ResponseCode.ToString() + ".ogg");
                }
                soundFiles.Add(@"EFFECTS/OUTRO_01.ogg");
                Common.DebugMessage("Finished creating playlist, adding it to dispatch queue");
                if (dispatchQueue.Count <= Settings.MaxDispatchQueue)
                {
                    dispatchQueue.Add(soundFiles.ToArray());
                }
            } 

        }
        public static void ReceiveBackupRequestCallout(string CallSign, int departmentID, string ShortName, int playerNetworkID, int ResponseCode)
        {
            ReceiveBackupRequest(CallSign, departmentID, playerNetworkID, ResponseCode);
        }

        // End Backup
        public static void EndBackupRequest(string CallSign,int networkID)
        {
            Common.DebugMessage("Received code 4, adding it to dispatch queue");
            if (dispatchQueue.Count <= Settings.MaxDispatchQueue)
            {
                dispatchQueue.Add(new string[] { @"STAND_DOWN/ALL_UNITS_CODE_4.ogg" });
            }
        }


        // API Call, add to dispatch queue
        public static void AddToDispatchQueue(string audioList)
        {
            Common.DebugMessage("Dispatch playlist received via event from 3rd party, adding it to queue");
            if (dispatchQueue.Count <= Settings.MaxDispatchQueue)
            {
                dispatchQueue.Add(audioList.Split(','));
            }
        }

        static List<string> DispatchIntro(int ResponseCode)
        {
            // Update playerdata object
            Settings.GetPlayerData();

            List<string> soundFiles = new List<string>();
            soundFiles.Add(@"EFFECTS/INTRO_01.ogg");

            //int chance = random.Next(0, 100);
            int chance = 1;

            if ((string)Settings.playerData.Callsign != null
                &&  (string)Settings.playerData.Callsign != ""
                && ResponseCode != 99
                && (
                    (ResponseCode == 3 && chance < 50 )
                    || (ResponseCode == 2 && chance < 75)
                    || ResponseCode == 1
                )
            )
            {
                // Add "greeting"
                soundFiles = soundFiles.Concat(GetCallsignAudio((string)Settings.playerData.Callsign)).ToList();
            }
            else {
                List<string> SearchFiles = AudioLibrary.availableAudio.Where(x => x.StartsWith(@"ATTENTION_ALL_UNITS_GEN/ATTENTION_ALL_UNITS_GENERIC_")).ToList();
                soundFiles.Add(SearchFiles[random.Next(0, SearchFiles.Count)]);
            }


            return soundFiles;
        }

        public static List<string> GetCallsignAudio(string callsign)
        {
            List<string> callsignAudio = new List<string>();

            if (AudioLibrary.callsignAudio.ContainsKey(callsign))
            {
                callsignAudio = AudioLibrary.callsignAudio[callsign];
            }
            else
            {
                // Regex (not working with FiveM): [a-zA-Z]+|[0-9]+(?:\.[0-9]+|)
                string callsignQuery = callsign.ToLower() + "_____";
                
                //TODO: first digit not taken from "CAR_CODE_DIVISION"
                for(int i=0;i < callsignQuery.Length; i++)
                {
                    // letter
                    if (char.IsLetter(callsignQuery[i]))
                    {
                        callsignAudio.Add(@"CAR_CODE_UNIT_TYPE/" + callsignQuery[i] + ".ogg");
                    }
                    // 3 digit number
                    if (char.IsDigit(callsignQuery[i]) && int.TryParse(String.Concat(callsignQuery[i], callsignQuery[i + 1], callsign[i + 2]), out int number3) && (number3 % 100) <= 24)
                    {
                        callsignAudio.Add(@"CAR_CODE_DIVISION/" + callsignQuery[i] + ".ogg");
                        callsignAudio.Add(@"CAR_CODE_BEAT/" + (number3 % 100) + ".ogg");
                        i += 2;
                    }
                    // 2 digit number
                    else if (char.IsDigit(callsignQuery[i]) && int.TryParse(String.Concat(callsignQuery[i], callsignQuery[i + 1]), out int number2) && number2 <= 24)
                    {
                        if (i == 1 && number2 == 10)
                        {
                            callsignAudio.Add(@"CAR_CODE_DIVISION/" + number2 + ".ogg");
                        }
                        else
                        {
                            callsignAudio.Add(@"CAR_CODE_BEAT/" + number2 + ".ogg");
                        }
                        i++;
                    }
                    // 1 digit number at beginning
                    else if(i==1 && char.IsDigit(callsignQuery[i]))
                    {
                        callsignAudio.Add(@"CAR_CODE_DIVISION/" + callsignQuery[i] + ".ogg");
                    }
                    // 1 digit number in between
                    else if(char.IsDigit(callsignQuery[i]))
                    {
                        callsignAudio.Add(@"CAR_CODE_BEAT/" + callsignQuery[i] + ".ogg");
                    }
                }

                foreach(string test in callsignAudio)
                {
                    Common.ChatMessage(new[] { 255, 255, 255 }, new[] { test });
                }

                // Save list to dictionary
                AudioLibrary.callsignAudio[callsign] = callsignAudio;
            }

            // Return list to caller
            return callsignAudio;
        }

    }
}
