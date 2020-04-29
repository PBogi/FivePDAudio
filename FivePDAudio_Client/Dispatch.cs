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
        public static void ReceiveCalloutInformation(string ShortName, string Address, int ResponseCode, string Description, string Identifier)
        {
            Debug.WriteLine("Received callout information");
            List<string> soundFiles = new List<string>();

            soundFiles.Add(@"EFFECTS/INTRO_01.ogg");
            List<string> SearchFiles = AudioLibrary.availableAudio.Where(x => x.StartsWith(@"ATTENTION_ALL_UNITS_GEN/ATTENTION_ALL_UNITS_GENERIC_")).ToList();
            soundFiles.Add(SearchFiles[random.Next(0, SearchFiles.Count)]);


            // Search for registered callouts first, then check callouts.json
            if (AudioLibrary.registeredCrimeAudio.ContainsKey(ShortName))
            {
                SearchFiles = AudioLibrary.availableAudio.Where(x => x.StartsWith(@"WE_HAVE/CITIZENS")).ToList();
                SearchFiles.AddRange(AudioLibrary.availableAudio.Where(x => x.StartsWith(@"WE_HAVE/WE")));
                soundFiles.Add(SearchFiles[random.Next(0, SearchFiles.Count)]);

                SearchFiles = AudioLibrary.availableAudio.Where(x => x.StartsWith(AudioLibrary.registeredCrimeAudio[ShortName])).ToList();
                soundFiles.Add(SearchFiles[random.Next(0, SearchFiles.Count)]);
            }
            else if (AudioLibrary.configuredCallouts.ContainsKey(ShortName))
            {
                SearchFiles = AudioLibrary.availableAudio.Where(x => x.StartsWith(@"WE_HAVE/CITIZENS")).ToList();
                SearchFiles.AddRange(AudioLibrary.availableAudio.Where(x => x.StartsWith(@"WE_HAVE/WE")));
                soundFiles.Add(SearchFiles[random.Next(0, SearchFiles.Count)]);

                SearchFiles = AudioLibrary.availableAudio.Where(x => x.StartsWith((string)AudioLibrary.configuredCallouts[ShortName])).ToList();
                soundFiles.Add(SearchFiles[random.Next(0, SearchFiles.Count)]);
            }

            if (ResponseCode > 1)
            {
                soundFiles.Add(@"DISPATCH_RESPOND_CODE/RESPOND_CODE_" + ResponseCode.ToString() + ".ogg");
            }
            soundFiles.Add(@"EFFECTS/OUTRO_01.ogg");

            Debug.WriteLine("Finished creating playlist, adding it to dispatch queue");
            dispatchQueue.Add(soundFiles.ToArray());
        }

        // Callout ended (currently unused)
        public static void CalloutEnded(string Identifier)
        {
            Debug.WriteLine("Received code 4, adding it to dispatch queue");
            dispatchQueue.Add(new string[] { @"STAND_DOWN/ALL_UNITS_CODE_4.ogg" });
        }

        // Backup requests
        public static void ReceiveBackupRequest(string CallSign, int departmentID, int playerNetworkID, int ResponseCode)
        {
            Debug.WriteLine("Received backup information");
            List<string> soundFiles = new List<string>();
            List<string> SearchFiles = AudioLibrary.availableAudio.Where(x => x.StartsWith(@"ATTENTION_ALL_UNITS_GEN/ATTENTION_ALL_UNITS_GENERIC_")).ToList();
            soundFiles.Add(SearchFiles[random.Next(0, SearchFiles.Count)]);

            if (ResponseCode == 99)
            {
                Debug.WriteLine("Code 99!");
                SearchFiles = AudioLibrary.availableAudio.Where(x => x.StartsWith(@"OFFICER_REQUESTS_BACKUP/CODE99")).ToList();
                soundFiles.Add(SearchFiles[random.Next(0, SearchFiles.Count)]);

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                AudioHandler.PlayCode99(soundFiles.ToArray());
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            }
            else
            {
                soundFiles.Add(@"OFFICER_REQUESTS_BACKUP/OFFICER_REQUESTING_BACKUP.ogg");
                if (ResponseCode > 1)
                {
                    soundFiles.Add(@"DISPATCH_RESPOND_CODE/RESPOND_CODE_" + ResponseCode.ToString() + ".ogg");
                }

                Debug.WriteLine("Finished creating playlist, adding it to dispatch queue");
                dispatchQueue.Add(soundFiles.ToArray());
            } 

        }
        public static void ReceiveBackupRequestCallout(string CallSign, int departmentID, string ShortName, int playerNetworkID, int ResponseCode)
        {
            ReceiveBackupRequest(CallSign, departmentID, playerNetworkID, ResponseCode);
        }

        // End Backup
        public static void EndBackupRequest(string CallSign,int networkID)
        {
            Debug.WriteLine("Received code 4, adding it to dispatch queue");
            dispatchQueue.Add(new string[] { @"STAND_DOWN/ALL_UNITS_CODE_4.ogg" });
        }


        // API Call, add to dispatch queue
        public static void AddToDispatchQueue(string audioList)
        {
            Debug.WriteLine("Dispatch playlist received via event from 3rd party, adding it to queue");
            if (dispatchQueue.Count <= 3)
            {
                dispatchQueue.Add(audioList.Split(','));
            }
        }
    }
}
