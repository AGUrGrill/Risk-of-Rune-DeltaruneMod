using R2API;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using static DeltaruneMod.DeltarunePlugin;

namespace DeltaruneMod.Items.Spamton
{
    public class CommRing : ItemBase<CommRing>
    {
        public override string ItemName => "Commemorative Ring";

        public override string ItemLangTokenName => "COMM_RING";

        public override string ItemPickupDesc => "10% increased luck at Suspicious Exchange.";

        public override string ItemFullDescription => "Gain a <style=cIsUtility>10%</style> higher roll chance at Suspicious Exchange.";

        public override string ItemLore => "WHEN KIDS LIKE YOU ARE <style=cEvent>[Beating People Up]</style>," +
            "\n[Spitting] IN THEIR EYES, THROWING SAND IN THEIR <style=cEvent>[Face]</style>," +
            "\n[Stomping] ON THEIR TOES, YANKING THEIR <style=cEvent>[Noses]</style>," +
            "\nAND NOT EVEN GIVING THEM A SINGLE CENT FOR IT!?" +
            "\nYOU SHOULD HAVE DONE ALL THAT EARLIER!" +
            "\nAND BEEN THE FIRST TO OWN MY <style=cEvent>[Commemorative Ring]</style>" +
            "\nTOO BAD! SEE YOU KID!";

        public override ItemTier Tier => ItemTier.Tier2;

        public override GameObject ItemModel => MainAssets.LoadAsset<GameObject>("comm_ring.prefab");

        public override Sprite ItemIcon => MainAssets.LoadAsset<Sprite>("comm_ring_icon.png");

        public override ItemTag[] ItemTags => new ItemTag[] { ItemTag.Utility };

        public override bool isChapter1 => false;

        public override bool isChapter2 => true;

        public override bool isChapter3 => false;

        public override bool isChapter4 => false;

        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            ItemDisplayRuleDict rules = new ItemDisplayRuleDict();
            rules.Add("mdlCommandoDualies", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "HandL",
                    localPos = new Vector3(0.04363F, 0.15085F, -0.01218F),
                    localAngles = new Vector3(291.7313F, 35.89888F, 149.2341F),
                    localScale = new Vector3(3.56431F, 3.56431F, 3.56431F)

                }
            });
            rules.Add("mdlHuntress", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "HandR",
                    localPos = new Vector3(-0.05961F, 0.12799F, -0.08759F),
                    localAngles = new Vector3(345.1665F, 257.8601F, 270.5199F),
                    localScale = new Vector3(3.15649F, 3.23952F, 3.15649F)

                }
            });
            rules.Add("mdlToolbot", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    childName = "HandR",
                    localPos = new Vector3(0.08838F, 1.41659F, -0.29016F),
                    localAngles = new Vector3(330.1074F, 280.9893F, 352.4626F),
                    localScale = new Vector3(44.27621F, 47.79814F, 54.67879F)
                }
            });
            rules.Add("mdlEngi", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "HandR",
                    localPos = new Vector3(-0.00876F, 0.18284F, -0.0207F),
                    localAngles = new Vector3(345.7541F, 323.9921F, 201.2191F),
                    localScale = new Vector3(4.03277F, 4.03277F, 3.98001F)
                }
            });
            rules.Add("mdlMage", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "HandR",
                    localPos = new Vector3(0.00375F, 0.18175F, 0.02323F),
                    localAngles = new Vector3(331.9111F, 208.5898F, 186.1979F),
                    localScale = new Vector3(1.85204F, 1.85204F, 1.85204F)
                }
            });
            rules.Add("mdlMerc", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Finger22R",
                    localPos = new Vector3(-0.03671F, 0.06605F, -0.01452F),
                    localAngles = new Vector3(5.30467F, 205.4741F, 164.3929F),
                    localScale = new Vector3(3.2169F, 3.2169F, 3.2169F)
                }
            });
            rules.Add("mdlTreebot", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "FootFrontR",
                    localPos = new Vector3(-0.00529F, 1.19196F, -0.00011F),
                    localAngles = new Vector3(4.4797F, 193.5389F, 177.6235F),
                    localScale = new Vector3(23.56433F, 23.56433F, 23.56433F)
                }
            });
            rules.Add("mdlLoader", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "MechFinger23R",
                    localPos = new Vector3(0.00017F, 0.00674F, 0.05709F),
                    localAngles = new Vector3(78.41496F, 195.9934F, 13.50731F),
                    localScale = new Vector3(5.70147F, 5.70147F, 5.70147F)
                }
            });
            rules.Add("mdlCroco", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Finger11L",
                    localPos = new Vector3(-0.07039F, 0.71007F, 0.73747F),
                    localAngles = new Vector3(9.3553F, 270.8647F, 167.1162F),
                    localScale = new Vector3(57.55627F, 57.55627F, 57.55627F)
                }
            });
            rules.Add("mdlCaptain", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Finger22R",
                    localPos = new Vector3(0.0298F, 0.01229F, 0.00559F),
                    localAngles = new Vector3(349.821F, 190.0275F, 176.9839F),
                    localScale = new Vector3(3.98387F, 3.98387F, 3.98387F)
                }
            });
            rules.Add("mdlBandit2", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "HandL",
                    localPos = new Vector3(0.05029F, 0.13514F, -0.02388F),
                    localAngles = new Vector3(320.5668F, 121.0486F, 243.9749F),
                    localScale = new Vector3(2.58858F, 2.5314F, 2.56262F)
                }
            });
            rules.Add("mdlVoidSurvivor", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "RingFinger",
                    localPos = new Vector3(-0.00009F, 0.08173F, 0.00036F),
                    localAngles = new Vector3(346.3212F, 77.13857F, 175.9458F),
                    localScale = new Vector3(2.9766F, 3.46018F, 2.4805F)

                }
            });
            rules.Add("mdlChef", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "OvenDoor",
                    localPos = new Vector3(-0.25566F, 0.01406F, 0.01958F),
                    localAngles = new Vector3(356.8177F, 3.85665F, 358.8758F),
                    localScale = new Vector3(11.68215F, 11.63221F, 5.70363F)
                }
            });
            rules.Add("mdlSeeker", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "HandR",
                    localPos = new Vector3(-0.0104F, 0.12534F, -0.00118F),
                    localAngles = new Vector3(310.499F, 327.4647F, 205.2438F),
                    localScale = new Vector3(2.78946F, 2.47952F, 2.47952F)

                }
            });
            rules.Add("mdlFalseSon", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "HandR",
                    localPos = new Vector3(0.09605F, 0.26009F, 0.00275F),
                    localAngles = new Vector3(353.4551F, 18.60704F, 348.559F),
                    localScale = new Vector3(5.31966F, 5.31966F, 5.31966F)
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

            GameObject pickupModel = MainAssets.LoadAsset<GameObject>("comm_ring.prefab").InstantiateClone("LightBulbPickup", false);
            pickupModel.transform.localScale = new Vector3(1.3f, 1.3f, 1.3f);
            ItemDef.pickupModelPrefab = pickupModel;
        }
    }
}
