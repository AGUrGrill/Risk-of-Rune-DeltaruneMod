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
    public class NeoMithrixRightWing : ItemBase<NeoMithrixRightWing>
    {
        public override string ItemName => "NEO_MITHRIX_RIGHT_WING_DISPLAY_ITEM";

        public override string ItemLangTokenName => "NEO_MITHRIX_RIGHT_WING_DISPLAY_ITEM";

        public override string ItemPickupDesc => "";

        public override string ItemFullDescription => "";

        public override string ItemLore => "";

        public override ItemTier Tier => ItemTier.AssignedAtRuntime;

        public override GameObject ItemModel => MainAssets.LoadAsset<GameObject>("neo_right_wing.prefab");

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
                    localPos = new Vector3(0.34145F, 0.30649F, -0.20338F),
                    localAngles = new Vector3(349.2747F, 212.71F, 321.6776F),
                    localScale = new Vector3(40.64867F, 23.74224F, 9.22609F)
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
