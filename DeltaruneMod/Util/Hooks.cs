using DeltaruneMod.Elites;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace DeltaruneMod.Util
{
    public class Hooks
    {
        int numOfEnemiesSpawned = 0;
        public Hooks()
        {
            On.RoR2.CharacterMaster.OnBodyStart += CharacterMaster_OnBodyStart;
        }

        private void CharacterMaster_OnBodyStart(On.RoR2.CharacterMaster.orig_OnBodyStart orig, RoR2.CharacterMaster self, RoR2.CharacterBody body)
        {
            orig(self, body);

            // Simulate NEO Elite
            if (DeltaruneMod.DeltarunePlugin.eliteDisable.Value) return;
            if (body.isPlayerControlled || body.isRemoteOp || body.IsDrone) return;

            numOfEnemiesSpawned++;
            if (numOfEnemiesSpawned % 150 == 0)
            {
                UnityEngine.Debug.Log("Spawning NEO Elite.");
                body.inventory.SetEquipmentIndex(NeoElite.instance.EliteEquip.equipmentIndex, true);
            }
        }
    }
}
