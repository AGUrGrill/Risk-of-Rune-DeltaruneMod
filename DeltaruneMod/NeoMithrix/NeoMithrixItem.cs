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

namespace DeltaruneMod.NeoMithrix
{
    public class NeoMithrixItem : ItemBase<NeoMithrixItem>
    {
        public override string ItemName => "NEO_MITHRIX_BASE_ITEM";

        public override string ItemLangTokenName => "NEO_MITHRIX_BASE";

        public override string ItemPickupDesc => "";

        public override string ItemFullDescription => "";

        public override string ItemLore => "";

        public override ItemTier Tier => ItemTier.AssignedAtRuntime;

        public override GameObject ItemModel => MainAssets.LoadAsset<GameObject>("neo_chestplate.prefab");

        public override Sprite ItemIcon => null;

        public override bool isChapter1 => false;

        public override bool isChapter2 => true;

        public override bool isChapter3 => false;

        public override bool isChapter4 => false;

        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            ItemDisplayRuleDict rules = new ItemDisplayRuleDict();
            rules.Add("mdlBrother", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "chest",
                    localPos = new Vector3(-0.02253F, -0.24776F, 0.26671F),
                    localAngles = new Vector3(340.4726F, 7.2708F, 350.1845F),
                    localScale = new Vector3(13.03703F, 14.24153F, 15.06375F)
                }
            });
            return rules;
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
                
                args.healthMultAdd += 8f;
                args.armorTotalMult += 0.5f;
                args.attackSpeedMultAdd += 2f;
                args.critDamageMultAdd += 2f;
                args.critAdd += 1.25f;
                args.regenMultAdd += 2f;
                args.damageMultAdd += 2f;
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
