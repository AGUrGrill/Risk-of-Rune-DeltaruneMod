using R2API;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using static DeltaruneMod.DeltarunePlugin;

namespace DeltaruneMod.Items.Tier1.Gacha
{
    public class ExecBuffet : ItemBase<ExecBuffet>
    {
        public override string ItemName => "ExecBuffet";

        public override string ItemLangTokenName => "EXEC_BUFFET";

        public override string ItemPickupDesc => "Heal party +100 HP on low health.";

        public override string ItemFullDescription => "";

        public override string ItemLore => "";

        public override ItemTier Tier => ItemTier.NoTier;

        public override GameObject ItemModel => null;

        public override Sprite ItemIcon => MainAssets.LoadAsset<Sprite>("exec_buffet_icon.png");

        public override bool isChapter1 => false;

        public override bool isChapter2 => false;

        public override bool isChapter3 => true;

        public override bool isChapter4 => false;

        private readonly float healAmount = 100f;

        public override ItemTag[] ItemTags => new ItemTag[] { ItemTag.Healing, ItemTag.WorldUnique };

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

            #region Heal All Players at Low HP
            if (GetCount(self) <= 0) return;

            var holderHealth = self.healthComponent.health;
            var holderMaxHealth = self.maxHealth;
            var activationThreshold = holderMaxHealth * 0.4;

            if (holderHealth <= activationThreshold)
            {
                foreach (var characterBody in CharacterBody.readOnlyInstancesList)
                {
                    if (characterBody.isPlayerControlled)
                    {
                        var playerHealth = characterBody.healthComponent.health;
                        var playerMaxHealth = characterBody.maxHealth;
                        var healthCalculation = playerHealth + healAmount;
                        var wontOverheal = healthCalculation <= playerMaxHealth;
                        if (wontOverheal)
                        {
                            characterBody.healthComponent.health += healAmount;
                        }
                        else
                        {
                            characterBody.healthComponent.health = playerMaxHealth;
                        }
                        Debug.Log(characterBody.name + " healed!");
                    }
                    Debug.Log(characterBody + " found.");
                }
                self.inventory.RemoveItemPermanent(ExecBuffet.instance.ItemDef.itemIndex);
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
