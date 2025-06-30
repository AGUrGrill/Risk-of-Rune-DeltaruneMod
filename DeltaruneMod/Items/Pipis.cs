using BepInEx.Configuration;
using R2API;
using RoR2;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;
using static DeltaruneMod.DeltarunePlugin;

namespace DeltaruneMod.Items
{
    public class Pipis : ItemBase<Pipis>
    {
        public override string ItemName => "Pipis";

        public override string ItemLangTokenName => "PIPIS";

        public override string ItemPickupDesc => "Wow, pipis!";

        public override string ItemFullDescription => "Provides a minor bonus to all stats! Pearl be damned my boy ballin'.";

        public override string ItemLore => "It's pipis.";

        public override ItemTier Tier => ItemTier.Boss;

        public override GameObject ItemModel => MainAssets.LoadAsset<GameObject>("lancer_card.prefab");

        public override Sprite ItemIcon => MainAssets.LoadAsset<Sprite>("lancer_card_icon.png");

        public override void Init()
        {
            CreateLang();
            CreateItem();
            Hooks();
        }

        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            return null;
        }

        public override void Hooks()
        {
            RecalculateStatsAPI.GetStatCoefficients += PipisEffect;
        }

        private void PipisEffect(CharacterBody sender, RecalculateStatsAPI.StatHookEventArgs args)
        {
            if (!NetworkServer.active || !sender) return;

            var existing = sender.GetComponent<PipisTracker>();
            if (sender.inventory && GetCount(sender) > 0)
            {
                if (!existing)
                {
                    existing = sender.gameObject.AddComponent<PipisTracker>();
                    existing.body = sender;
                    existing.currNumOfPipis = GetCount(sender);
                }
                else if (existing && GetCount(sender) <= 0) existing.enabled = false;
                else if (existing && GetCount(sender) > 0 && !existing.enabled) existing.enabled = true;
                if (existing) existing.currNumOfPipis = GetCount(sender);
            }
        }
    }

    public class PipisTracker : CharacterBody.ItemBehavior
    {
        public int prevNumOfPipis = 0;
        public int currNumOfPipis = 0;
        public CharacterBody body;
        private float pipisMult = 0.05F;

        private void FixedUpdate()
        {
            if (currNumOfPipis > prevNumOfPipis)
            {
                body.baseArmor *= pipisMult;
                body.baseMaxHealth *= pipisMult;
                body.baseMoveSpeed *= pipisMult;
                body.baseRegen *= pipisMult;
                body.baseAttackSpeed *= pipisMult;
                body.baseDamage *= pipisMult;
                body.baseCrit *= pipisMult;
                prevNumOfPipis = currNumOfPipis;
            }
        }
    }
}
