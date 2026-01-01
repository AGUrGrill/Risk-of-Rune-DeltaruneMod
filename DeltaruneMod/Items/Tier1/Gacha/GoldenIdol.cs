using R2API;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using static DeltaruneMod.DeltarunePlugin;

namespace DeltaruneMod.Items.Tier1.Gacha
{
    public class GoldenIdol : ItemBase<GoldenIdol>
    {
        public override string ItemName => "Gold Tenna Statue";

        public override string ItemLangTokenName => "GOLDEN_IDOL";

        public override string ItemPickupDesc => "Gain 20% more experience.";

        public override string ItemFullDescription => "Gain 20% more experience. <style=cStack>(+20% per stack)</style>";

        public override string ItemLore => "";

        public override ItemTier Tier => ItemTier.NoTier;

        public override GameObject ItemModel => null;

        public override Sprite ItemIcon => MainAssets.LoadAsset<Sprite>("golden_idol_icon.png");

        public override bool isChapter1 => false;

        public override bool isChapter2 => false;

        public override bool isChapter3 => true;

        public override bool isChapter4 => false;

        private readonly float xpGainMult = 0.2f;

        public override ItemTag[] ItemTags => new ItemTag[] { ItemTag.Utility, ItemTag.WorldUnique };

        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            return null;
        }

        public override void Hooks()
        {
            On.RoR2.DeathRewards.OnKilledServer += DeathRewards_OnKilledServer;
        }

        private void DeathRewards_OnKilledServer(On.RoR2.DeathRewards.orig_OnKilledServer orig, DeathRewards self, DamageReport damageReport)
        {
            var player = damageReport.attackerBody;
            var xp = self.expReward;

            #region Give player increased xp
            if (GetCount(player) > 0)
            {
                Debug.Log("XP Bonus Added: " + xp);
                var mult = xpGainMult * GetCount(player);
                uint bonus = (uint)Mathf.CeilToInt(xp * mult);
                self.expReward += bonus;
                Debug.Log(" -> " + self.expReward);
            }
            #endregion

            orig(self, damageReport);
        }

        public override void Init()
        {
            CreateItem();
            CreateLang();
            Hooks();
        }
    }
}
