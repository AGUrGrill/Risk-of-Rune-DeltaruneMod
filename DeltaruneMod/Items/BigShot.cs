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
                    localPos = new Vector3(-0.04222F, -0.44262F, 0.35685F),
                    localAngles = new Vector3(11.85053F, 148.0504F, 351.0004F),
                    localScale = new Vector3(7F, 7F, 7F)
                }
            });
            rules.Add("mdlHuntress", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Head",
                    localPos = new Vector3(-0.61995F, 0.23677F, 0.12246F),
                    localAngles = new Vector3(50.48947F, 179.8686F, 88.65546F),
                    localScale = new Vector3(6F, 6F, 6F)

                }
            });
            rules.Add("mdlToolbot", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Head",
                    localPos = new Vector3(0.75843F, 0.44873F, -5.97378F),
                    localAngles = new Vector3(48.64816F, 311.6693F, 319.3044F),
                    localScale = new Vector3(60F, 60F, 60F)

                }
            });
            rules.Add("mdlEngi", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "HeadCenter",
                    localPos = new Vector3(-0.07696F, -0.82386F, 0.0362F),
                    localAngles = new Vector3(350.3063F, 149.1217F, 7.03048F),
                    localScale = new Vector3(8F, 8F, 8F)

                }
            });
            rules.Add("mdlMage", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Head",
                    localPos = new Vector3(-0.02684F, -0.25777F, 0.1529F),
                    localAngles = new Vector3(0.99506F, 146.1651F, 359.0777F),
                    localScale = new Vector3(3F, 3F, 3F)

                }
            });
            rules.Add("mdlMerc", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Head",
                    localPos = new Vector3(-0.05516F, -0.36669F, 0.19114F),
                    localAngles = new Vector3(0.78083F, 149.236F, 1.34581F),
                    localScale = new Vector3(5F, 5F, 5F)

                }
            });
            rules.Add("mdlTreebot", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "FlowerBase",
                    localPos = new Vector3(-0.07465F, -1.96628F, 0.74331F),
                    localAngles = new Vector3(3.95929F, 149.5434F, 357.3385F),
                    localScale = new Vector3(10F, 10F, 10F)

                }
            });
            rules.Add("mdlLoader", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Head",
                    localPos = new Vector3(-0.03695F, -0.38561F, 0.19133F),
                    localAngles = new Vector3(358.3326F, 145.6386F, 1.02962F),
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
                    localPos = new Vector3(0.31579F, 6.44934F, -2.27131F),
                    localAngles = new Vector3(39.29791F, 216.9755F, 205.7235F),
                    localScale = new Vector3(50F, 50F, 50F)


                }
            });
            rules.Add("mdlCaptain", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Head",
                    localPos = new Vector3(-0.04942F, -0.43712F, 0.1296F),
                    localAngles = new Vector3(358.6895F, 151.388F, 1.96694F),
                    localScale = new Vector3(5F, 5F, 5F)

                }
            });
            rules.Add("mdlBandit2", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Head",
                    localPos = new Vector3(-0.02233F, -0.43605F, 0.13625F),
                    localAngles = new Vector3(359.8591F, 149.4673F, 358.1331F),
                    localScale = new Vector3(4.6F, 4.6F, 4.6F)

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
                    localPos = new Vector3(-0.04595F, -0.25266F, 0.13105F),
                    localAngles = new Vector3(3.30229F, 153.9347F, 3.33027F),
                    localScale = new Vector3(3F, 3F, 3F)

                }
            });
            rules.Add("mdlVoidSurvivor", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Head",
                    localPos = new Vector3(-0.0316F, -0.32915F, 0.44771F),
                    localAngles = new Vector3(26.42275F, 153.9711F, 348.9298F),
                    localScale = new Vector3(6F, 5F, 5F)

                }
            });
            rules.Add("mdlChef", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Head",
                    localPos = new Vector3(-0.81985F, 0.20914F, -0.05524F),
                    localAngles = new Vector3(55.56215F, 357.1792F, 266.0527F),
                    localScale = new Vector3(7F, 6F, 6F)

                }
            });
            rules.Add("mdlSeeker", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Head",
                    localPos = new Vector3(-0.04614F, -0.32434F, 0.20333F),
                    localAngles = new Vector3(4.86292F, 154.7291F, 0.52935F),
                    localScale = new Vector3(4.5F, 4F, 4F)

                }
            });
            rules.Add("mdlFalseSon", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Head",
                    localPos = new Vector3(-0.01619F, -0.62815F, 0.30422F),
                    localAngles = new Vector3(4.056F, 154.1342F, 354.7828F),
                    localScale = new Vector3(8F, 8F, 8F)

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
