using DeltaruneMod.Items;
using DeltaruneMod.Items.Lunar;
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
    public class NeoMithrixItem : ItemBase<NeoMithrixItem>
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

            // Provide mithrix with buffs
            if (sender.inventory && GetCount(sender) > 0)
            {
                // Remove frostbite when needed
                if (sender.GetBuffCount(ThornRing.frostbite) > 0)
                {
                    for (int i = 0; i < sender.GetBuffCount(ThornRing.frostbite); i++)
                    {
                        sender.RemoveBuff(ThornRing.frostbite);
                        Debug.Log("Removing fortbite stacks!");
                    }
                    
                }
                
                args.healthMultAdd += 4;
                args.armorTotalMult += 2;
                args.attackSpeedMultAdd += 2f;
                args.moveSpeedMultAdd += 10f;
                args.critDamageMultAdd += 2f;
                args.critAdd += 1.25f;
                args.regenMultAdd += 2;
                args.damageMultAdd += 2;
            }
        }

        public override void Init()
        {
            CreateItem();
            CreateLang();
            Hooks();
        }
    }
}
