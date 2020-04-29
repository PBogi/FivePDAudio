using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using Newtonsoft.Json;


namespace fivepdaudio
{
    class AudioHandler
    {
        public static float soundVolume = 0.75f;
        public static bool isPlaying;
        
        public async Task Play()
        {
            if (Dispatch.dispatchQueue.Count > 0 && isPlaying != true) {
                string[] soundArray = Dispatch.dispatchQueue[0];

                Dispatch.dispatchQueue.Remove(Dispatch.dispatchQueue[0]);

                var soundData = new
                {
                    Action = "play",
                    Files = soundArray,
                    Volume = soundVolume
                };

                SendNuiMessage(JsonConvert.SerializeObject(soundData));

                isPlaying = true;

                int i = 0;
                while(isPlaying == true)
                {
                    i++;
                    if (i > 10)
                    {
                        // Force Stop
                        await Stop();
                        isPlaying = false;
                    }
                    await BaseScript.Delay(1000);
                }
                await BaseScript.Delay(1000);
            }
        }

        public async Task PlayCode99(string[] soundArray)
        {
            isPlaying = true;
            await Stop();
            await BaseScript.Delay(4250);

            var soundData = new
            {
                Action = "play",
                Files = soundArray,
                Volume = soundVolume
            };

            SendNuiMessage(JsonConvert.SerializeObject(soundData));
            Dispatch.dispatchQueue.Clear();
            int i = 0;
            while (isPlaying == true)
            {
                i++;
                if (i > 10)
                {
                    // Force Stop
                    await Stop();
                    isPlaying = false;
                }
                await BaseScript.Delay(1000);
            }
            await BaseScript.Delay(2500);
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task Stop()
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            if (isPlaying == true)
            {
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
