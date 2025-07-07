using BepInEx.Configuration;
using R2API;
using RoR2;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;
using static DeltaruneMod.DeltarunePlugin;

namespace DeltaruneMod.Items.Yellow
{
    public class Pipis : ItemBase<Pipis>
    {
        public override string ItemName => "Pipis";

        public override string ItemLangTokenName => "PIPIS";

        public override string ItemPickupDesc => "Increases ALL of your stats.";

        public override string ItemFullDescription => "Increases <style=cIsUtility>ALL stats</style> by <style=cIsUtility>5%</style>." +
            "\nPearl be damned my boy ballin'.";

        public override string ItemLore => "You can't get this from an egg!";

        public override ItemTier Tier => ItemTier.Boss;

        public override GameObject ItemModel => MainAssets.LoadAsset<GameObject>("pipis.prefab");

        public override Sprite ItemIcon => MainAssets.LoadAsset<Sprite>("pipis_icon.png");

        public override ItemTag[] ItemTags => new ItemTag[] { ItemTag.Utility, ItemTag.Damage };

        public override bool PrinterBlacklisted 
        { 
            get => base.PrinterBlacklisted; 
            set => base.PrinterBlacklisted = true; 
        }

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
                    childName = "LeftJet",
                    localPos = new Vector3(0.00013F, 0.01343F, 0.00686F),
                    localAngles = new Vector3(22.89687F, 268.0432F, 258.2714F),
                    localScale = new Vector3(6.9072F, 6.9072F, 6.9072F)
                }
            });
            rules.Add("mdlHuntress", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Chest",
                    localPos = new Vector3(-0.00725F, 0.12978F, 0.13905F),
                    localAngles = new Vector3(357.6052F, 89.41378F, 342.1603F),
                    localScale = new Vector3(9.75112F, 11.98992F, 9.75112F)
                }
            });
            rules.Add("mdlBandit2", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "MuzzleShotgun",
                    localPos = new Vector3(-0.02101F, -0.00477F, 0.01479F),
                    localAngles = new Vector3(283.734F, 62.9136F, 28.78585F),
                    localScale = new Vector3(5.99235F, 4.61689F, 3.53847F)
                }
            });
            rules.Add("mdlToolbot", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Head",
                    localPos = new Vector3(0.3794F, 2.70397F, -0.97458F),
                    localAngles = new Vector3(307.1695F, 173.5109F, 2.96273F),
                    localScale = new Vector3(43.89983F, 71.05827F, 71.05827F)

                }
            });
            rules.Add("mdlEngi", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "MuzzleRight",
                    localPos = new Vector3(0.00286F, 0.01333F, -0.1631F),
                    localAngles = new Vector3(1.78108F, 89.37039F, 87.38036F),
                    localScale = new Vector3(12.83591F, 15.81911F, 13.84606F)
                }
            });
            rules.Add("mdlMage", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "ClavicleL",
                    localPos = new Vector3(-0.16533F, 0.09445F, -0.10163F),
                    localAngles = new Vector3(349.5243F, 83.51843F, 56.10131F),
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
                    localPos = new Vector3(0.01925F, 0.19819F, -0.09577F),
                    localAngles = new Vector3(286.5604F, 24.27891F, 103.4976F),
                    localScale = new Vector3(6.88069F, 11.03788F, 5.78616F)
                }
            });
            rules.Add("mdlTreebot", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Eye",
                    localPos = new Vector3(-0.00365F, 0.80307F, 0.01732F),
                    localAngles = new Vector3(1.07244F, 104.6857F, 9.88337F),
                    localScale = new Vector3(20.04582F, 23.37344F, 17.21499F)
                }
            });
            rules.Add("mdlLoader", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "MechHandRight",
                    localPos = new Vector3(-0.04124F, 0.32469F, 0.04654F),
                    localAngles = new Vector3(15.33141F, 189.3875F, 78.24608F),
                    localScale = new Vector3(7.36798F, 13.29883F, 7.36798F)
                }
            });
            rules.Add("mdlCroco", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Head",
                    localPos = new Vector3(1.08672F, 1.76753F, 0.084F),
                    localAngles = new Vector3(0.90842F, 268.449F, 232.6822F),
                    localScale = new Vector3(64.47551F, 95.74067F, 64.47551F)
                }
            });
            rules.Add("mdlCaptain", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "HandR",
                    localPos = new Vector3(-0.03565F, 0.07075F, 0.01312F),
                    localAngles = new Vector3(9.92061F, 146.7857F, 17.66426F),
                    localScale = new Vector3(4.8577F, 7.78581F, 4.07227F)
                }
            });
            rules.Add("mdlRailGunner", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "GunBarrel",
                    localPos = new Vector3(-0.00422F, 0.63713F, -0.00013F),
                    localAngles = new Vector3(2.21753F, 266.1243F, 174.4695F),
                    localScale = new Vector3(6.33012F, 9.30456F, 5.06844F)
                }
            });
            rules.Add("mdlVoidSurvivor", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "CannonEnd",
                    localPos = new Vector3(0.10334F, 0.00193F, 0.0155F),
                    localAngles = new Vector3(23.73183F, 114.4923F, 301.8614F),
                    localScale = new Vector3(11.51731F, 13.92004F, 13.6393F)
                }
            });
            rules.Add("mdlChef", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Head",
                    localPos = new Vector3(-0.24121F, 0.03468F, -0.05723F),
                    localAngles = new Vector3(1.51737F, 351.2228F, 259.804F),
                    localScale = new Vector3(13.09572F, 17.80788F, 9.28489F)
                }
            });
            rules.Add("mdlSeeker", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Pack",
                    localPos = new Vector3(0.18237F, -0.31841F, -0.16186F),
                    localAngles = new Vector3(321.2907F, 71.28646F, 1.76035F),
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
                    localPos = new Vector3(0.00591F, 0.21158F, 0.12662F),
                    localAngles = new Vector3(7.60785F, 236.6935F, 268.9621F),
                    localScale = new Vector3(17.39792F, 13.6196F, 9.26764F)
                }
            });
            rules.Add("mdlRalsei", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Head",
                    localPos = new Vector3(-0.01144F, 0.01106F, 0.00398F),
                    localAngles = new Vector3(14.68918F, 284.3936F, 34.94466F),
                    localScale = new Vector3(0.12866F, 0.12866F, 0.12866F)

                }
            });
            return rules;
        }

        public override void Hooks()
        {
            RecalculateStatsAPI.GetStatCoefficients += PipisEffect;
        }

        private void PipisEffect(CharacterBody sender, RecalculateStatsAPI.StatHookEventArgs args)
        {
            if (!NetworkServer.active || !sender) return;

            var existing = sender.GetComponent<PipisTracker>();
            if (sender.inventory && GetCount(sender) > 0)
            {
                if (!existing)
                {
                    existing = sender.gameObject.AddComponent<PipisTracker>();
                    existing.body = sender;
                    existing.currNumOfPipis = GetCount(sender);
                }
                else if (existing && GetCount(sender) <= 0) existing.enabled = false;
                else if (existing && GetCount(sender) > 0 && !existing.enabled) existing.enabled = true;
                if (existing) existing.currNumOfPipis = GetCount(sender);
            }
        }
    }

    public class PipisTracker : CharacterBody.ItemBehavior
    {
        public int prevNumOfPipis = 0;
        public int currNumOfPipis = 0;
        public CharacterBody body;
        private float pipisMult = 1.05F;

        private void FixedUpdate()
        {
            if (currNumOfPipis > prevNumOfPipis)
            {
                body.baseArmor *= pipisMult;
                body.baseMaxHealth *= pipisMult;
                body.baseMoveSpeed *= pipisMult;
                body.baseRegen *= pipisMult;
                body.baseAttackSpeed *= pipisMult;
                body.baseDamage *= pipisMult;
                body.baseCrit *= pipisMult;
                prevNumOfPipis = currNumOfPipis;
            }
        }
    }
}
