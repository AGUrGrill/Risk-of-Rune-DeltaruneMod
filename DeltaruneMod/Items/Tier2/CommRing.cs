using R2API;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using static DeltaruneMod.DeltarunePlugin;

namespace DeltaruneMod.Items.Tier2
{
    public class CommRing : ItemBase<CommRing>
    {
        public override string ItemName => "Commemorative Ring";

        public override string ItemLangTokenName => "COMM_RING";

        public override string ItemPickupDesc => "Gain a 10% increase in roll chance at the Suspicious Exchange.";

        public override string ItemFullDescription => "On hit, heal <style=cIsHealing>1 hp</style> and gain <style=cIsHealing>1 temporary barrier</style>. <style=cStack>(+1 hp, +1 barrier, per stack)</style>." +
            "\nYou can heal a maximum of <style=cIsHealing>50% hp</style> and <style=cIsHealing>20% of hp as barrier</style>.";

        public override string ItemLore => "WHEN KIDS LIKE YOU ARE <style=cEvent>[Beating People Up]</style>," +
            "\n[Spitting] IN THEIR EYES, THROWING SAND IN THEIR <style=cEvent>[Face]</style>," +
            "\n[Stomping] ON THEIR TOES, YANKING THEIR <style=cEvent>[Noses]</style>," +
            "\nAND NOT EVEN GIVING THEM A SINGLE CENT FOR IT!?" +
            "\nYOU SHOULD HAVE DONE ALL THAT EARLIER!" +
            "\nAND BEEN THE FIRST TO OWN MY <style=cEvent>[Commemorative Ring]</style>" +
            "\nTOO BAD! SEE YOU KID!";

        public override ItemTier Tier => ItemTier.NoTier;

        public override GameObject ItemModel => MainAssets.LoadAsset<GameObject>("thorn_ring.prefab");

        public override Sprite ItemIcon => MainAssets.LoadAsset<Sprite>("comm_ring_icon.png");

        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            return null;
        }

        public override void Hooks()
        {
            //On.RoR2.GlobalEventManager.OnHitEnemy += CommRingEffect;
            RecalculateStatsAPI.GetStatCoefficients += RecalculateStatsAPI_GetStatCoefficients;
        }

        private void RecalculateStatsAPI_GetStatCoefficients(CharacterBody sender, RecalculateStatsAPI.StatHookEventArgs args)
        {
            
        }

        private void CommRingEffect(On.RoR2.GlobalEventManager.orig_OnHitEnemy orig, RoR2.GlobalEventManager self, RoR2.DamageInfo damageInfo, GameObject victim)
        {
            if (!NetworkServer.active) return;

            var body = damageInfo.attacker.GetComponent<CharacterBody>();
            var enemyBody = victim.GetComponent<CharacterBody>();
            var itemCount = GetCount(body);

            if (body.inventory && itemCount > 0)
            {
                //var healing = itemCount * 1;
                //if (!(body.healthComponent.health + healing > body.maxHealth * 0.5f)) body.healthComponent.health += healing;
                //if (!(body.healthComponent.barrier + healing > body.maxHealth * 0.2f)) body.healthComponent.barrier += healing;
            }

            orig(self, damageInfo, victim);
        }

        public override void Init()
        {
            CreateItem();
            CreateLang();
            Hooks();
        }
    }
}
