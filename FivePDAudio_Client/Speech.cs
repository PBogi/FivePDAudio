using System;
using System.Collections.Generic;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;

namespace fivepdaudio
{
    class Speech
    {
        public static int voiceId = 0;
        public static string playerVoice;
        // PlayAmbientSpeech2(PedId, "GENERIC_CURSE_HIGH", "SPEECH_PARAMS_SHOUTED");
        // https://runtime.fivem.net/doc/natives/?_0xC6941B4A3A8FBBB9
        // https://pastebin.com/1GZS5dCL
        // https://github.com/Albo1125/LSPDFRPlus/blob/master/LSPDFR%2B/EnhancedTrafficStop.cs
        // https://runtime.fivem.net/doc/natives/?_0x3523634255FC3318
        // Voice an Hand von player ped model zuweisen
        //PlayAmbientSpeechWithVoice(PlayerPedId(), "DRAW_GUN_03", "s_m_y_cop_01_white_full_01", "SPEECH_PARAMS_FORCE_SHOUTED", false);
        //using CitizenFX.Core.Native; Function.Call<string>(Hash.LOAD_RESOURCE_FILE, "lspdfrm", "callouts.json");

        public static string ownVoice;

        public static void ChangeVoice<T>(List<T> newVoice)
        {
            ownVoice = "test";

        }

        public static void SetVoice()
        {
            if(voiceId == 0)
            {
                Random random = new Random();
                voiceId = random.Next(1, 3);
            }
            bool isMale = IsPedMale(GetPlayerPed(-1));

            if(isMale == true) {
                playerVoice = "s_m_y_cop_01_white_full_0" + voiceId;
            }
            else
            {
                playerVoice = "s_f_y_cop_01_white_full_0" + voiceId;
            }

        }

        public static void StopPed()
        {
            int playerId = GetPlayerPed(-1);
            Vector3 coords = GetEntityCoords(playerId, true);


            if (GetPedConfigFlag(playerId, 78, true))
            {
                Debug.WriteLine("aiming");
                PlayAmbientSpeechAtCoords("DRAW_GUN", playerVoice, coords.X, coords.Y, coords.Z, "SPEECH_PARAMS_ALLOW_REPEAT");
            }
            else if (IsPedSprinting(playerId))
            {
                Debug.WriteLine("sprinting");
                PlayAmbientSpeechAtCoords("FOOT_CHASE", playerVoice, coords.X, coords.Y, coords.Z, "SPEECH_PARAMS_ALLOW_REPEAT");
            }
            else
            {
                Debug.WriteLine("normal");
                PlayAmbientSpeechAtCoords("GENERIC_HI", playerVoice, coords.X, coords.Y, coords.Z, "SPEECH_PARAMS_ALLOW_REPEAT");
            }
        }

    }
}
