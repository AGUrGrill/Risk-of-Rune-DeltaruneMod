using R2API;
using RoR2;
using RoR2.Stats;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Timers;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;
using static DeltaruneMod.DeltarunePlugin;

namespace DeltaruneMod.Items
{
    public class TennaBuckle : ItemBase<TennaBuckle>
    {
        public override string ItemName => "Showrunner's Buckle";
        public override string ItemLangTokenName => "TENNABUCKLE";
        public override string ItemPickupDesc => "10% more gold gain.";
        public override string ItemFullDescription => "10% more gold gain (+5% per item)";
        public override string ItemLore => "Mr. Tenna has been looking for this for weeks!\nI should give it back... but its so shinyyyy...";
        public override ItemTier Tier => ItemTier.Tier1;
        public override GameObject ItemModel => MainAssets.LoadAsset<GameObject>("tenna_buckle.prefab");
        public override Sprite ItemIcon => MainAssets.LoadAsset<Sprite>("tenna_buckle_icon.png");
        
        public override void Init()
        {
            CreateItem();
            CreateLang();
            Hooks();
        }

        public override void Hooks()
        {
            On.RoR2.CharacterMaster.GiveMoney += TennaBuckleEffect;
        }

        public void TennaBuckleEffect(On.RoR2.CharacterMaster.orig_GiveMoney orig, CharacterMaster self, uint amount)
        {
            if (!NetworkServer.active || !self.GetBody()) return;

            var body = self.GetBody();
            var itemCount = GetCount(body);

            if (body.inventory && itemCount > 0)
            {
                Debug.Log($"Amount | " + amount);
                uint bonus = (uint)Mathf.CeilToInt(amount * (0.1f + (0.05f * (itemCount-1))));
                amount += bonus;
                Debug.Log($"Adjusted Amount | " + amount);
                
            }
            orig(self, amount);
        }

        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            ItemDisplayRuleDict rules = new ItemDisplayRuleDict();
            rules.Add("mdlCommandoDualies", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Pelvis",
                    localPos = new Vector3(-0.00373F, -0.06952F, -0.15043F),
                    localAngles = new Vector3(5.39609F, 183.0791F, 178.6603F),
                    localScale = new Vector3(3F, 3F, 3F)

                }
            });
            rules.Add("mdlHuntress", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Pelvis",
                    localPos = new Vector3(-0.0025F, -0.06824F, -0.13148F),
                    localAngles = new Vector3(4.29842F, 167.8138F, 174.8732F),
                    localScale = new Vector3(3F, 3F, 3F)
                }
            });
            rules.Add("mdlToolbot", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Pelvis",
                    localPos = new Vector3(1.68019F, 2.73918F, -0.00804F),
                    localAngles = new Vector3(2.27102F, 267.7722F, 358.5837F),
                    localScale = new Vector3(30F, 30F, 30F)
                }
            });
            rules.Add("mdlEngi", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Pelvis",
                    localPos = new Vector3(-0.00386F, -0.01181F, -0.22054F),
                    localAngles = new Vector3(8.86381F, 178.4863F, 179.1359F),
                    localScale = new Vector3(3F, 3F, 3F)
                }
            });
            rules.Add("mdlMage", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "ThighL",
                    localPos = new Vector3(0.07764F, 0.19436F, 0.14714F),
                    localAngles = new Vector3(349.3714F, 228.8959F, 189.4519F),
                    localScale = new Vector3(2F, 2F, 2F)
                }
            });
            rules.Add("mdlMerc", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Pelvis",
                    localPos = new Vector3(0.03208F, 0.04264F, -0.19609F),
                    localAngles = new Vector3(4.07449F, 172.4652F, 178.222F),
                    localScale = new Vector3(2.5F, 2.5F, 2.5F)
                }
            });
            rules.Add("mdlTreebot", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "FlowerBase",
                    localPos = new Vector3(0.06715F, -0.05151F, 0.72664F),
                    localAngles = new Vector3(3.2722F, 195.2976F, 1.56459F),
                    localScale = new Vector3(5F, 5F, 5F)
                }
            });
            rules.Add("mdlLoader", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "ThighR",
                    localPos = new Vector3(-0.06266F, 0.40189F, 0.1992F),
                    localAngles = new Vector3(336.7605F, 335.6627F, 138.7854F),
                    localScale = new Vector3(3F, 3F, 3F)
                }
            });
            rules.Add("mdlCroco", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Head",
                    localPos = new Vector3(0.30336F, 4.76385F, -0.07585F),
                    localAngles = new Vector3(325.9449F, 21.35626F, 353.344F),
                    localScale = new Vector3(25F, 25F, 25F)
                }
            });
            rules.Add("mdlCaptain", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Pelvis",
                    localPos = new Vector3(-0.02665F, -0.1417F, -0.16756F),
                    localAngles = new Vector3(357.7801F, 188.4642F, 177.4114F),
                    localScale = new Vector3(4.2F, 4.2F, 4.2F)
                }
            });
            rules.Add("mdlBandit2", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Pelvis",
                    localPos = new Vector3(0.02897F, -0.00556F, -0.18436F),
                    localAngles = new Vector3(7.5524F, 170.7124F, 182.7698F),
                    localScale = new Vector3(4F, 3F, 3F)
                }
            });
            rules.Add("mdlHeretic", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Head",
                    localPos = new Vector3(0.02105F, -0.87071F, 0.01385F),
                    localAngles = new Vector3(355.2848F, 47.55381F, 355.0908F),
                    localScale = new Vector3(0.20392F, 0.20392F, 0.20392F)

                }
            });
            rules.Add("mdlRailGunner", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Pelvis",
                    localPos = new Vector3(-0.00063F, 0.12378F, -0.10718F),
                    localAngles = new Vector3(337.2103F, 179.3639F, 180.8889F),
                    localScale = new Vector3(2F, 2F, 2F)
                }
            });
            rules.Add("mdlVoidSurvivor", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Pelvis",
                    localPos = new Vector3(0.13922F, 0.05034F, 0.03156F),
                    localAngles = new Vector3(8.40652F, 71.7737F, 179.5784F),
                    localScale = new Vector3(3F, 3F, 3F)
                }
            });
            rules.Add("mdlChef", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Pelvis",
                    localPos = new Vector3(0.09356F, 0.14264F, -0.01847F),
                    localAngles = new Vector3(280.7382F, 126.7378F, 319.4977F),
                    localScale = new Vector3(4F, 4F, 4F)
                }
            });
            rules.Add("mdlSeeker", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Chest",
                    localPos = new Vector3(-0.07135F, 0.06F, 0.10751F),
                    localAngles = new Vector3(358.8024F, 352.8521F, 354.5065F),
                    localScale = new Vector3(3F, 3F, 3F)
                }
            });
            rules.Add("mdlFalseSon", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Pelvis",
                    localPos = new Vector3(-0.00868F, -0.02633F, 0.19566F),
                    localAngles = new Vector3(358.0899F, 0.41444F, 357.5107F),
                    localScale = new Vector3(6F, 6F, 6F)
                }
            });
            return rules;
        }

        
    }
}
