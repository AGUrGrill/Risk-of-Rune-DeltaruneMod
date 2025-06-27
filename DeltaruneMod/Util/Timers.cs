using DeltaruneMod.Items;
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
                Log.Debug("BigShot Timer");
                BigShot.instance.BigShotEffect();
                Log.Debug("Big Shot Timer Tick");
                yield return new WaitForSeconds(10.5f);
            }
        }
    }
}
