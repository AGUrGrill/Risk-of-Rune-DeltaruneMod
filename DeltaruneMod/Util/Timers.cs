using DeltaruneMod.Items.Lunar;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using static DeltaruneMod.DeltarunePlugin;

namespace DeltaruneMod.Util
{
    public class Timers : MonoBehaviour
    {
        public void Start()
        {
            Log.Debug("Timers started.");
            StartCoroutine(BigShotTimer());
        }

        private IEnumerator BigShotTimer()
        {
            while (true)
            {
                //Log.Debug("Devil Timer");
                DevilsKnife.instance.DevilsKnifeEffect();
                //Log.Debug("Devil Timer Tick");
                yield return new WaitForSeconds(10.5f);
            }
        }
    }
}
