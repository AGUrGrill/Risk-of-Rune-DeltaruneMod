using DeltaruneMod.Items;
using R2API;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using static DeltaruneMod.DeltarunePlugin;

namespace DeltaruneMod.Neo.NeoMithrix
{
    public class NeoMithrixMain : ItemBase<NeoMithrixMain>
    {
        public override string ItemName => "NEO_MITHRIX_BASE_ITEM";

        public override string ItemLangTokenName => "NEO_MITHRIX_BASE";

        public override string ItemPickupDesc => "";

        public override string ItemFullDescription => "";

        public override string ItemLore => "";

        public override ItemTier Tier => ItemTier.AssignedAtRuntime;

        public override GameObject ItemModel => MainAssets.LoadAsset<GameObject>("combined_neo.prefab");

        public override Sprite ItemIcon => null;

        public override bool isChapter1 => false;

        public override bool isChapter2 => true;

        public override bool isChapter3 => false;

        public override bool isChapter4 => false;

        private bool StatsUpgraded = false;

        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            return null;
        }

        public override void Hooks()
        { 
            RecalculateStatsAPI.GetStatCoefficients += RecalculateStatsAPI_GetStatCoefficients;
        }

        private void RecalculateStatsAPI_GetStatCoefficients(CharacterBody sender, RecalculateStatsAPI.StatHookEventArgs args)
        {
            if (!NetworkServer.active) return;

            if (sender.inventory && GetCount(sender) > 0 && !StatsUpgraded)
            {
                sender.baseMaxHealth *= 4;
                sender.baseDamage *= 3;
                sender.baseRegen *= 2;
                sender.baseMoveSpeed *= 1.5f;
                StatsUpgraded = true;
            }

            // Give other display items
        }

        public override void Init()
        {
            //CreateItem();
            //CreateLang();
            //Hooks();
        }
    }
}
