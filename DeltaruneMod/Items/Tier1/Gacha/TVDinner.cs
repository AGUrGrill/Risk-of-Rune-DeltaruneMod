using R2API;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using static DeltaruneMod.DeltarunePlugin;

namespace DeltaruneMod.Items.Tier1.Gacha
{
    public class TVDinner : ItemBase<TVDinner>
    {
        public override string ItemName => "TVDinner";

        public override string ItemLangTokenName => "TV_DINNER";

        public override string ItemPickupDesc => "Heal +100 HP on low health.";

        public override string ItemFullDescription => "";

        public override string ItemLore => "";

        public override ItemTier Tier => ItemTier.AssignedAtRuntime;

        public override GameObject ItemModel => null;

        public override Sprite ItemIcon => MainAssets.LoadAsset<Sprite>("tv_dinner_icon.png");

        public override bool isChapter1 => false;

        public override bool isChapter2 => false;

        public override bool isChapter3 => true;

        public override bool isChapter4 => false;

        private readonly float healAmount = 100f;

        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            return null;
        }

        public override void Hooks()
        {
            On.RoR2.CharacterBody.OnTakeDamageServer += CharacterBody_OnTakeDamageServer;
        }

        private void CharacterBody_OnTakeDamageServer(On.RoR2.CharacterBody.orig_OnTakeDamageServer orig, CharacterBody self, DamageReport damageReport)
        {
            orig(self, damageReport);

            #region Heal Holder at Low HP
            var playerHealth = self.healthComponent.health;
            var playerMaxHealth = self.maxHealth;
            var healthCalculation = playerHealth + healAmount;
            var wontOverheal = healthCalculation <= playerMaxHealth;
            var activationThreshold = playerMaxHealth * 0.4;
            if (GetCount(self) > 0 && playerHealth <= activationThreshold)
            {
                //Debug.Log("Stats:" +
                //    "\n" + playerHealth +
                //    "\n" + playerMaxHealth +
                //    "\n" + healthCalculation +
                //    "\n" + wontOverheal +
                //    "\n" + activationThreshold);
                if (wontOverheal)
                {
                    self.healthComponent.health += healAmount;
                }
                else
                {
                    self.healthComponent.health = playerMaxHealth;
                }
                self.inventory.RemoveItemTemp(TVDinner.instance.ItemDef.itemIndex);
                Debug.Log(self.name + " healed!");
            }
            #endregion
        }

        public override void Init()
        {
            CreateItem();
            CreateLang();
            Hooks();
        }
    }
}
