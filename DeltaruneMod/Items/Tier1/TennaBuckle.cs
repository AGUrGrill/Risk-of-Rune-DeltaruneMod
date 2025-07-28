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

namespace DeltaruneMod.Items.Tier1
{
    public class TennaBuckle : ItemBase<TennaBuckle>
    {
        public override string ItemName => "Showrunner's Buckle";
        public override string ItemLangTokenName => "TENNABUCKLE";
        public override string ItemPickupDesc => "10% more gold gain.";
        public override string ItemFullDescription => "<style=cIsUtility>10%</style> more gold gain <style=cStack>(+5% per stack)</style>";
        public override string ItemLore => "Mr. Tenna has been looking for this for weeks!\nI should give it back... but its so shinyyyy...";
        public override ItemTier Tier => ItemTier.Tier1;
        public override GameObject ItemModel => MainAssets.LoadAsset<GameObject>("tenna_buckle.prefab");
        public override Sprite ItemIcon => MainAssets.LoadAsset<Sprite>("tenna_buckle_icon.png");

        public override ItemTag[] ItemTags => new ItemTag[] { ItemTag.Utility };

        public override bool isChapter1 => false;

        public override bool isChapter2 => false;

        public override bool isChapter3 => true;

        public override bool isChapter4 => false;

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
                //Debug.Log($"Amount | " + amount);
                uint bonus = (uint)Mathf.CeilToInt(amount * (0.1f + 0.05f * (itemCount-1)));
                amount += bonus;
                //Debug.Log($"Adjusted Tennabuckle Amount | " + amount);
                
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
                    childName = "Stomach",
                    localPos = new Vector3(-0.0049F, 0.04183F, 0.15285F),
                    localAngles = new Vector3(3.26827F, 0.79509F, 357.778F),
                    localScale = new Vector3(5F, 5F, 5F)


                }
            });
            rules.Add("mdlHuntress", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Pelvis",
                    localPos = new Vector3(-0.00059F, -0.05429F, -0.13213F),
                    localAngles = new Vector3(4.33248F, 172.0518F, 175.0657F),
                    localScale = new Vector3(4F, 4F, 4F)

                }
            });
            rules.Add("mdlToolbot", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Hip",
                    localPos = new Vector3(1.68019F, 2.66642F, -0.00804F),
                    localAngles = new Vector3(2.27094F, 90.44276F, 358.5837F),
                    localScale = new Vector3(40F, 40F, 40F)

                }
            });
            rules.Add("mdlEngi", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Pelvis",
                    localPos = new Vector3(-0.00244F, 0.01116F, -0.22892F),
                    localAngles = new Vector3(8.86381F, 178.4863F, 179.1359F),
                    localScale = new Vector3(5F, 5F, 5F)

                }
            });
            rules.Add("mdlMage", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "ThighL",
                    localPos = new Vector3(0.06536F, 0.21384F, 0.15102F),
                    localAngles = new Vector3(7.82877F, 39.70278F, 166.4932F),
                    localScale = new Vector3(4F, 4F, 4F)

                }
            });
            rules.Add("mdlMerc", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "ThighR",
                    localPos = new Vector3(0.00605F, 0.12349F, 0.14855F),
                    localAngles = new Vector3(8.56603F, 341.2049F, 178.9855F),
                    localScale = new Vector3(4F, 4F, 4F)

                }
            });
            rules.Add("mdlTreebot", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "FlowerBase",
                    localPos = new Vector3(0.07716F, -0.07133F, 0.72874F),
                    localAngles = new Vector3(6.35499F, 10.55185F, 357.6271F),
                    localScale = new Vector3(8F, 8F, 8F)

                }
            });
            rules.Add("mdlLoader", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "ThighR",
                    localPos = new Vector3(-0.06006F, 0.42304F, 0.19245F),
                    localAngles = new Vector3(336.7605F, 335.6627F, 138.7854F),
                    localScale = new Vector3(5F, 5F, 5F)

                }
            });
            rules.Add("mdlCroco", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Head",
                    localPos = new Vector3(0.18475F, 4.63483F, -0.05588F),
                    localAngles = new Vector3(325.9449F, 21.35626F, 353.344F),
                    localScale = new Vector3(40F, 40F, 40F)

                }
            });
            rules.Add("mdlCaptain", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Stomach",
                    localPos = new Vector3(0.00148F, 0.11693F, 0.1787F),
                    localAngles = new Vector3(3.00358F, 359.9656F, 357.4811F),
                    localScale = new Vector3(6F, 6F, 6F)

                }
            });
            rules.Add("mdlBandit2", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Stomach",
                    localPos = new Vector3(-0.00105F, 0.01815F, 0.18323F),
                    localAngles = new Vector3(356.1403F, 359.3785F, 359.7015F),
                    localScale = new Vector3(6F, 5F, 5F)

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
                    childName = "CalfL",
                    localPos = new Vector3(0.11341F, 0.09869F, 0.03613F),
                    localAngles = new Vector3(3.19188F, 80.9049F, 190.4413F),
                    localScale = new Vector3(4F, 4F, 4F)

                }
            });
            rules.Add("mdlVoidSurvivor", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Pelvis",
                    localPos = new Vector3(0.13101F, 0.05237F, 0.03156F),
                    localAngles = new Vector3(2.45553F, 71.95915F, 181.5351F),
                    localScale = new Vector3(5F, 5F, 5F)

                }
            });
            rules.Add("mdlChef", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Pelvis",
                    localPos = new Vector3(0.10473F, 0.13733F, -0.01701F),
                    localAngles = new Vector3(287.2553F, 149.4112F, 296.2607F),
                    localScale = new Vector3(6F, 6F, 6F)

                }
            });
            rules.Add("mdlSeeker", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Chest",
                    localPos = new Vector3(-0.10687F, 0.06086F, 0.10486F),
                    localAngles = new Vector3(345.366F, 347.541F, 357.0169F),
                    localScale = new Vector3(5F, 5F, 5F)

                }
            });
            rules.Add("mdlFalseSon", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Pelvis",
                    localPos = new Vector3(-0.01761F, -0.02624F, 0.19554F),
                    localAngles = new Vector3(358.0899F, 0.41444F, 357.5107F),
                    localScale = new Vector3(9F, 9F, 9F)

                }
            });
            rules.Add("mdlRalsei", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Hips",
                    localPos = new Vector3(0.00042F, 0.00033F, 0.00586F),
                    localAngles = new Vector3(348.6603F, 4.18738F, 359.9568F),
                    localScale = new Vector3(0.1158F, 0.1158F, 0.05478F)

                }
            });
            return rules;
        }

        
    }
}
