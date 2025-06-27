using System;
using System.Collections.Generic;
using System.Text;
using DeltaruneMod.Items;
using R2API;
using RoR2;
using RoR2.Stats;
using RoR2BepInExPack.GameAssetPaths;
using UnityEngine;
using static DeltaruneMod.DeltarunePlugin;

namespace DeltaruneMod.Util
{
    public static class Events
    {
        public static void Init()
        {
            Run.onRunStartGlobal += (run) =>
            {
                GameObject timerHost = new GameObject("TimerHost");
                UnityEngine.Object.DontDestroyOnLoad(timerHost);
                timerHost.AddComponent<Timers>();
            };
        }
    }
}
