using DeltaruneMod.Items;
using R2API;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using static DeltaruneMod.DeltarunePlugin;

namespace DeltaruneMod.NeoMithrix
{
    public class NeoMithrixLeftWing : ItemBase<NeoMithrixLeftWing>
    {
        public override string ItemName => "NEO_MITHRIX_LEFT_WING_DISPLAY_ITEM";

        public override string ItemLangTokenName => "NEO_MITHRIX_LEFT_WING_DISPLAY_ITEM";

        public override string ItemPickupDesc => "";

        public override string ItemFullDescription => "";

        public override string ItemLore => "";

        public override ItemTier Tier => ItemTier.AssignedAtRuntime;

        public override GameObject ItemModel => MainAssets.LoadAsset<GameObject>("neo_left_wing.prefab");

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
                    localPos = new Vector3(-0.24608F, 0.298F, -0.2235F),
                    localAngles = new Vector3(22.15644F, 326.9847F, 325.871F),
                    localScale = new Vector3(32.06147F, 23.74224F, 9.22609F)

                }
            });
            return rules;
        }

        public override void Hooks()
        {
        }

        public override void Init()
        {
            CreateItem();
            CreateLang();
        }
    }
}
