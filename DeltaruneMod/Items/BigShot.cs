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
    public class BigShot : ItemBase<BigShot>
    {
        public override string ItemName => "[Big Shot]";
        public override string ItemLangTokenName => "BIGSHOT";
        public override string ItemPickupDesc => "Gain a random effect every 10 seconds for 10 seconds...";
        public override string ItemFullDescription => "Grants <style=cDeath>[Big Shot]</style> every 10 seconds.\n<style=cDeath>[Big Shot]</style>: Gain a random effect every 10 seconds.\nIncreases random effect duration by +5 seconds per stack.";
        public override string ItemLore => "As the days became more dull, and bussiness started to dry, a call came in. \"It's your chance... a once in a lifetime chance... to become a <style=cDeath>[Big Shot]</style>.\"";
        public override ItemTier Tier => ItemTier.Lunar;
        public override GameObject ItemModel => MainAssets.LoadAsset<GameObject>("big_shot.prefab");
        public override Sprite ItemIcon => MainAssets.LoadAsset<Sprite>("big_shot_icon.png");
        public override ItemTag[] ItemTags => new ItemTag[] { ItemTag.Utility };

        public override void Init()
        {
            CreateLang();
            CreateItem();
            Hooks();
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
                    childName = "Head",
                    localPos = new Vector3(-0.0181F, 0.18318F, 0.11438F),
                    localAngles = new Vector3(359.9379F, 89.13351F, 344.7787F),
                    localScale = new Vector3(14F, 14F, 14F)

                }
            });
            rules.Add("mdlHuntress", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Head",
                    localPos = new Vector3(-0.05111F, 0.20041F, 0.05749F),
                    localAngles = new Vector3(71.13284F, 0.85515F, 269.7938F),
                    localScale = new Vector3(10.65627F, 10.65627F, 10.65627F)

                }
            });
            rules.Add("mdlToolbot", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Head",
                    localPos = new Vector3(0.4929F, 2.87609F, -0.55534F),
                    localAngles = new Vector3(3.62907F, 268.6543F, 301.4389F),
                    localScale = new Vector3(71.05827F, 71.05827F, 71.05827F)

                }
            });
            rules.Add("mdlEngi", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "HeadCenter",
                    localPos = new Vector3(-0.01001F, -0.04382F, 0.06658F),
                    localAngles = new Vector3(359.9916F, 87.82423F, 5.2221F),
                    localScale = new Vector3(12.66013F, 12.66013F, 12.66013F)

                }
            });
            rules.Add("mdlMage", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Head",
                    localPos = new Vector3(-0.00812F, 0.04364F, 0.07607F),
                    localAngles = new Vector3(1.42508F, 90.78919F, 359.603F),
                    localScale = new Vector3(6.82893F, 6.82893F, 6.82893F)

                }
            });
            rules.Add("mdlMerc", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Head",
                    localPos = new Vector3(-0.00841F, 0.10428F, 0.1014F),
                    localAngles = new Vector3(358.1436F, 86.20536F, 0.08487F),
                    localScale = new Vector3(8.35088F, 8.35088F, 8.35088F)

                }
            });
            rules.Add("mdlTreebot", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "FlowerBase",
                    localPos = new Vector3(-0.01F, -1.06369F, 0.55248F),
                    localAngles = new Vector3(1.072F, 89.76335F, 9.88333F),
                    localScale = new Vector3(13.6413F, 13.6413F, 13.6413F)

                }
            });
            rules.Add("mdlLoader", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Head",
                    localPos = new Vector3(-0.00822F, 0.06086F, 0.09935F),
                    localAngles = new Vector3(357.9385F, 87.69575F, 1.17614F),
                    localScale = new Vector3(8.36439F, 8.36439F, 8.36439F)

                }
            });
            rules.Add("mdlCroco", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Head",
                    localPos = new Vector3(0.09198F, 2.71101F, 0.6164F),
                    localAngles = new Vector3(0.90842F, 268.449F, 232.6822F),
                    localScale = new Vector3(103.8343F, 103.8343F, 103.8343F)
                }
            });
            rules.Add("mdlCaptain", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Head",
                    localPos = new Vector3(-0.00992F, 0.02677F, 0.06682F),
                    localAngles = new Vector3(359.7724F, 91.02103F, 7.08265F),
                    localScale = new Vector3(10.78019F, 10.78019F, 10.78019F)

                }
            });
            rules.Add("mdlBandit2", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Head",
                    localPos = new Vector3(-0.0085F, 0.09478F, 0.10477F),
                    localAngles = new Vector3(2.16612F, 88.15824F, 337.8445F),
                    localScale = new Vector3(8.60173F, 8.60173F, 8.60173F)

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
                    childName = "Head",
                    localPos = new Vector3(-0.00829F, 0.07999F, 0.07684F),
                    localAngles = new Vector3(1.49554F, 93.87408F, 354.721F),
                    localScale = new Vector3(8.26112F, 6.19495F, 6.19495F)

                }
            });
            rules.Add("mdlVoidSurvivor", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Head",
                    localPos = new Vector3(0.05952F, 0.11334F, 0.0741F),
                    localAngles = new Vector3(15.16286F, 99.01782F, 299.4797F),
                    localScale = new Vector3(8.81553F, 7.34627F, 7.34627F)

                }
            });
            rules.Add("mdlChef", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Head",
                    localPos = new Vector3(-0.28516F, 0.06555F, -0.01708F),
                    localAngles = new Vector3(1.14554F, 359.4018F, 259.1054F),
                    localScale = new Vector3(18.30414F, 15.68926F, 15.68926F)


                }
            });
            rules.Add("mdlSeeker", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Head",
                    localPos = new Vector3(-0.01224F, 0.12337F, 0.0967F),
                    localAngles = new Vector3(3.5382F, 92.96323F, 345.6394F),
                    localScale = new Vector3(8.24892F, 7.33238F, 7.33238F)

                }
            });
            rules.Add("mdlFalseSon", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Head",
                    localPos = new Vector3(-0.03355F, 0.13485F, 0.09933F),
                    localAngles = new Vector3(7.05586F, 94.36584F, 356.7482F),
                    localScale = new Vector3(15.23684F, 15.23684F, 15.23684F)

                }
            });
            return rules;
        }

        public override void Hooks()
        {
            //RecalculateStatsAPI.GetStatCoefficients += BigShotEffect;
        }

        // SEEKER ISSUE - When sojourn causes seeker to disappear, maybe make timer per person?
        public void BigShotEffect()
        {
            CharacterBody[] allCharacterBodies = GameObject.FindObjectsOfType<CharacterBody>();
            foreach (CharacterBody sender in allCharacterBodies)
            {
                if (!NetworkServer.active && !sender.isPlayerControlled) return;

                var itemCount = GetCount(sender);
                if (sender.inventory && itemCount > 0)
                {
                    List<BuffDef> allBuffs = new List<BuffDef>();
                    FieldInfo[] fields = typeof(RoR2Content.Buffs).GetFields(BindingFlags.Public | BindingFlags.Static);
                    foreach (var field in fields)
                    {
                        if (field.GetValue(null) is BuffDef buffDef)
                        {
                            allBuffs.Add(buffDef);
                        }
                    }
                    BuffDef randomBuff = allBuffs[Random.Range(0, allBuffs.Count)];
                    sender.AddTimedBuff(randomBuff, 8 + (itemCount - 0) * 5);
                    Debug.Log($"Added random buff: {randomBuff.name} to {sender.name}");
                }
            }
        }    
    }
}
