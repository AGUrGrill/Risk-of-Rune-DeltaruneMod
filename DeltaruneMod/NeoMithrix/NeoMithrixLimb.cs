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
    public class NeoMithrixLimb : ItemBase<NeoMithrixLimb>
    {
        public override string ItemName => "NEO_MITHRIX_LIMB_DISPLAY_ITEM";

        public override string ItemLangTokenName => "NEO_MITHRIX_LIMB_DISPLAY_ITEM";

        public override string ItemPickupDesc => "";

        public override string ItemFullDescription => "";

        public override string ItemLore => "";

        public override ItemTier Tier => ItemTier.AssignedAtRuntime;

        public override GameObject ItemModel => MainAssets.LoadAsset<GameObject>("neo_limb.prefab");

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
                    childName = "LowerArmR",
                    localPos = new Vector3(2.21972F, -0.3514F, 0.97877F),
                    localAngles = new Vector3(359.5706F, 336.0472F, 355.0849F),
                    localScale = new Vector3(50.67373F, 10.90218F, 37.61541F)
                },
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "CalfL",
                    localPos = new Vector3(1.69327F, -0.87295F, -2.68867F),
                    localAngles = new Vector3(357.1952F, 58.53083F, 350.397F),
                    localScale = new Vector3(68.1146F, 14.93527F, 46.738F)
                },
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "CalfR",
                    localPos = new Vector3(2.03953F, 1.32916F, 2.46837F),
                    localAngles = new Vector3(358.2022F, 130.2431F, 171.4975F),
                    localScale = new Vector3(68.1146F, 14.93527F, 46.738F)
                },
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "LowerArmL",
                    localPos = new Vector3(2.66172F, 0.04324F, -1.36845F),
                    localAngles = new Vector3(8.13478F, 26.6752F, 5.05343F),
                    localScale = new Vector3(60.19924F, 12.65326F, 39.0168F)
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
