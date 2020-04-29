using System;
using System.Collections.Generic;
using System.Linq;
using Debug = CitizenFX.Core.Debug;

namespace fivepdaudio
{
    class Dispatch
    {
        static Random random = new Random();

        public static List<string[]> dispatchQueue = new List<string[]>();

        // New callout
        static public void ReceiveCalloutInformation(string ShortName, string Address, int ResponseCode, string Description, string Identifier)
        {
            List<string> soundFiles = new List<string>();

            List<string> SearchFiles = AudioLibrary.availableAudio.Where(x => x.StartsWith(@"ATTENTION_ALL_UNITS_GEN/ATTENTION_ALL_UNITS_GENERIC_")).ToList();
            soundFiles.Add(SearchFiles[random.Next(0, SearchFiles.Count)]);

            List<string> crimeSounds = new List<string>();

            // Search for registered callouts first
            if(AudioLibrary.registeredCrimeAudio.ContainsKey(ShortName))
            {
                crimeSounds.Add(AudioLibrary.registeredCrimeAudio[ShortName]);
            }
            else
            {
                foreach (var element in AudioLibrary.availableCrimeAudio)
                {
                    if (element.Key.Contains(ShortName.ToLower()))
                    {
                        crimeSounds.Add(element.Value);
                    }
                }
            }


            if (crimeSounds.Count > 0) {
                SearchFiles = AudioLibrary.availableAudio.Where(x => x.StartsWith(@"WE_HAVE/CITIZENS")).ToList();
                SearchFiles.AddRange(AudioLibrary.availableAudio.Where(x => x.StartsWith(@"WE_HAVE/WE")));

                soundFiles.Add(SearchFiles[random.Next(0, SearchFiles.Count)]);


                int index = random.Next(crimeSounds.Count);
                soundFiles.Add(crimeSounds[index]);

                soundFiles.Add(@"DISPATCH_RESPOND_CODE/RESPOND_CODE_" + ResponseCode.ToString() + ".ogg");
            }
            else
            {
                soundFiles.Add(@"DISPATCH_RESPOND_CODE/RESPOND_CODE_" + ResponseCode.ToString() + ".ogg");
            }

            dispatchQueue.Add(soundFiles.ToArray());
        }

        // Callout ended (currently unused)
        static public void CalloutEnded(string Identifier)
        {
            dispatchQueue.Add(new string[] { @"STAND_DOWN/ALL_UNITS_CODE_4.ogg" });
        }

        // Backup requests
        static public void ReceiveBackupRequest(string CallSign, int departmentID, int playerNetworkID, int ResponseCode)
        {
            List<string> soundFiles = new List<string>();
            List<string> SearchFiles = AudioLibrary.availableAudio.Where(x => x.StartsWith(@"ATTENTION_ALL_UNITS_GEN/ATTENTION_ALL_UNITS_GENERIC_")).ToList();
            soundFiles.Add(SearchFiles[random.Next(0, SearchFiles.Count)]);

            if (ResponseCode == 99)
            {
                SearchFiles = AudioLibrary.availableAudio.Where(x => x.StartsWith(@"OFFICER_REQUESTS_BACKUP/CODE99")).ToList();
                soundFiles.Add(SearchFiles[random.Next(0, SearchFiles.Count)]);

                AudioHandler soundHandler = new AudioHandler();
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                soundHandler.PlayCode99(soundFiles.ToArray());
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            }
            else
            {
                soundFiles.Add(@"OFFICER_REQUESTS_BACKUP/OFFICER_REQUESTING_BACKUP.ogg");
                soundFiles.Add(@"DISPATCH_RESPOND_CODE/RESPOND_CODE_" + ResponseCode.ToString() + ".ogg");
                dispatchQueue.Add(soundFiles.ToArray());
            }            
        }
        static public void ReceiveBackupRequestCallout(string CallSign, int departmentID, string ShortName, int playerNetworkID, int ResponseCode)
        {
            ReceiveBackupRequest(CallSign, departmentID, playerNetworkID, ResponseCode);
        }

        // End Backup
        static public void EndBackupRequest(string CallSign,int networkID)
        {
            dispatchQueue.Add(new string[] { @"STAND_DOWN/ALL_UNITS_CODE_4.ogg" });
        }


        // API Call, add to dispatch queue
        public static void AddToDispatchQueue(string audioList)
        {
            if (dispatchQueue.Count <= 3)
            {
                dispatchQueue.Add(audioList.Split(','));
            }
        }
    }
}
