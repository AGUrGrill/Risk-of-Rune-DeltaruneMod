using R2API;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using UnityEngine;
using static DeltaruneMod.DeltarunePlugin;

namespace DeltaruneMod.Util
{
    public static class SoundBank
    {
        public static uint _soundBankId;
        //public const string soundBankFolder = "SoundBanks";
        public const string soundBankFileName = "DeltaruneSoundBank.bnk";
        public const string soundBankName = "DeltaruneSoundBank";
        public static string SoundBankDirectory => Path.Combine(Path.GetDirectoryName(DeltarunePlugin.Instance.Info.Location));
        public static void Init()
        {
            UnityEngine.Debug.Log(SoundBankDirectory);
            try
            {
                string fullBankPath = Path.Combine(SoundBankDirectory, soundBankFileName);
                UnityEngine.Debug.Log($"SoundBank size: {new FileInfo(fullBankPath).Length} bytes");

                UnityEngine.Debug.Log($"Attempting to load sound bank...");

                if (!File.Exists(fullBankPath))
                {
                    Log.Error($"Sound bank path does not exist!!");
                    return;
                }

                var result = AkSoundEngine.LoadBank(fullBankPath, out _soundBankId);

                if (result == AKRESULT.AK_Success)
                {
                    Log.Info($"SoundBank loaded successfully!");
                }
                else
                {
                    Log.Error($"SoundBank failed to load. {result}");
                }

                SoundAPI.SoundBanks.Add(fullBankPath);
            }
            catch ( Exception ex ) { UnityEngine.Debug.Log( ex ); }
            
            
            
        }
    }

}
