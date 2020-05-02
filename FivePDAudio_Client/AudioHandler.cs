using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using Newtonsoft.Json;


namespace fivepdaudio
{
    class AudioHandler
    {
        public static bool isPlaying;
        public static bool isCode99 = false;
        
        public static async Task Play()
        {
            if (Dispatch.dispatchQueue.Count > 0 && isPlaying != true && isCode99 != true) {
                Common.DebugMessage("Playing audio");
                string[] soundArray = Dispatch.dispatchQueue[0];

                var soundData = new
                {
                    Action = "play",
                    Files = soundArray,
                    Volume = Settings.SoundVolume
                };

                SendNuiMessage(JsonConvert.SerializeObject(soundData));

                isPlaying = true;

                int i = 0;
                while(isPlaying == true)
                {
                    i++;
                    if (i > 10 && isCode99 == false)
                    {
                        // Force Stop
                        Stop();
                    }
                    await BaseScript.Delay(1000);
                }
                Dispatch.dispatchQueue.Remove(Dispatch.dispatchQueue[0]);
                isPlaying = false;
                Common.DebugMessage("Stopped playing audio");
                await BaseScript.Delay(1000);
            }
        }

        public static async Task PlayCode99(string[] soundArray)
        {
            isCode99 = true;
            isPlaying = true;
            Stop();
            await BaseScript.Delay(4250);

            Common.DebugMessage("Playing Code 99 Audio");

            var soundData = new
            {
                Action = "play",
                Files = soundArray,
                Volume = Settings.SoundVolume
            };

            SendNuiMessage(JsonConvert.SerializeObject(soundData));
            Dispatch.dispatchQueue.Clear();
            int i = 0;
            while (isPlaying == true)
            {
                i++;
                if (i > 15)
                {
                    // Force Stop
                    Stop();
                }
                await BaseScript.Delay(1000);
            }
            Common.DebugMessage("Stopped playing Code 99");
            await BaseScript.Delay(5000);
            isPlaying = false;
            isCode99 = false;
        }

        public static void Stop()
        {
            if (isPlaying == true)
            {
                Common.DebugMessage("Force stopping playback");
                SendNuiMessage(JsonConvert.SerializeObject(new
                {
                    Action = "stop"
                }));
            }
        }

        public static void FinishedPlaying()
        {
            isPlaying = false;
        }
    }
}
