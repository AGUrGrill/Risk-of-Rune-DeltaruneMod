using R2API;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using static DeltaruneMod.DeltarunePlugin;

namespace DeltaruneMod.Items.Yellow
{
    public class MrPipis : ItemBase<Pipis>
    {
        public override string ItemName => "Mr. Pipis";

        public override string ItemLangTokenName => "MR_PIPIS";

        public override string ItemPickupDesc => "Congrats!! You are a [Big Shot]!!!!";

        public override string ItemFullDescription => "Provides <style=cIsUtility>ALL elite buffs</style>!";

        public override string ItemLore => "The one and only'.";

        public override ItemTier Tier => ItemTier.Boss;

        public override GameObject ItemModel => MainAssets.LoadAsset<GameObject>("mr_pipis.prefab");

        public override Sprite ItemIcon => MainAssets.LoadAsset<Sprite>("mr_pipis_icon.png");

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
                    childName = "Head",
                    localPos = new Vector3(-0.00426F, 0.28094F, -0.00592F),
                    localAngles = new Vector3(353.7496F, 353.853F, 354.2567F),
                    localScale = new Vector3(12.76023F, 16.23372F, 12.76023F)
                }
            });
            rules.Add("mdlHuntress", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Head",
                    localPos = new Vector3(0.00228F, 0.0628F, 0.03952F),
                    localAngles = new Vector3(331.6837F, 17.06083F, 354.8333F),
                    localScale = new Vector3(9.05695F, 12.89827F, 9.89628F)
                }
            });
            rules.Add("mdlBandit2", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Head",
                    localPos = new Vector3(0.00012F, 0.14728F, -0.00827F),
                    localAngles = new Vector3(4.90821F, 83.88029F, 318.1501F),
                    localScale = new Vector3(8.60173F, 10.61426F, 7.68438F)
                }
            });
            rules.Add("mdlToolbot", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Head",
                    localPos = new Vector3(-0.25753F, 2.58962F, 1.20786F),
                    localAngles = new Vector3(303.5001F, 137.2872F, 340.3238F),
                    localScale = new Vector3(71.05827F, 91.78291F, 71.05827F)
                }
            });
            rules.Add("mdlEngi", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "HeadCenter",
                    localPos = new Vector3(-0.00976F, -0.16622F, 0.02857F),
                    localAngles = new Vector3(339.7454F, 0.5669F, 354.8185F),
                    localScale = new Vector3(12.66013F, 12.66013F, 15.70925F)
                }
            });
            rules.Add("mdlMage", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Head",
                    localPos = new Vector3(-0.00013F, 0.14606F, -0.09325F),
                    localAngles = new Vector3(2.27543F, 348.6211F, 345.7619F),
                    localScale = new Vector3(6.82893F, 7.70643F, 6.74123F)
                }
            });
            rules.Add("mdlMerc", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Head",
                    localPos = new Vector3(-0.00885F, 0.20796F, 0.01862F),
                    localAngles = new Vector3(359.5584F, 1.03469F, 353.8554F),
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
                    localPos = new Vector3(-0.3344F, -0.04913F, 0.41831F),
                    localAngles = new Vector3(13.48484F, 348.6151F, 346.5744F),
                    localScale = new Vector3(8.67167F, 7.96958F, 6.45425F)

                }
            });
            rules.Add("mdlLoader", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Head",
                    localPos = new Vector3(-0.00889F, 0.18897F, 0.0452F),
                    localAngles = new Vector3(21.75747F, 6.04681F, 9.19641F),
                    localScale = new Vector3(8.36439F, 8.36439F, 8.15949F)
                }
            });
            rules.Add("mdlCroco", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Head",
                    localPos = new Vector3(-0.30861F, 0.19218F, 0.66591F),
                    localAngles = new Vector3(295.8134F, 350.4054F, 174.2124F),
                    localScale = new Vector3(87.16365F, 99.29857F, 103.8343F)
                }
            });
            rules.Add("mdlCaptain", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Head",
                    localPos = new Vector3(0.03626F, 0.17108F, -0.01637F),
                    localAngles = new Vector3(336.6772F, 351.1885F, 337.5962F),
                    localScale = new Vector3(9.64486F, 9.54612F, 9.64486F)
                }
            }); 
            rules.Add("mdlRailGunner", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "GunStock",
                    localPos = new Vector3(0.01195F, -0.32039F, 0.02558F),
                    localAngles = new Vector3(76.31502F, 226.8212F, 213.0602F),
                    localScale = new Vector3(6.96663F, 6.19495F, 6.19495F)
                }
            });
            rules.Add("mdlVoidSurvivor", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Head",
                    localPos = new Vector3(-0.02832F, 0.07043F, 0.06621F),
                    localAngles = new Vector3(328.4013F, 84.34533F, 35.42325F),
                    localScale = new Vector3(8.81553F, 9.3033F, 7.34627F)
                }
            });
            rules.Add("mdlChef", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Head",
                    localPos = new Vector3(-0.71728F, -0.22781F, -0.04492F),
                    localAngles = new Vector3(288.7068F, 247.8444F, 183.6162F),
                    localScale = new Vector3(13.41672F, 12.80579F, 12.2074F)
                }
            });
            rules.Add("mdlSeeker", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Pack",
                    localPos = new Vector3(-0.20928F, 0.1406F, -0.22238F),
                    localAngles = new Vector3(335.5866F, 33.30312F, 20.72316F),
                    localScale = new Vector3(9.48945F, 9.31357F, 8.43507F)
                }
            });
            rules.Add("mdlFalseSon", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Chest",
                    localPos = new Vector3(0.05703F, 0.27612F, 0.11341F),
                    localAngles = new Vector3(293.5728F, 356.797F, 23.95159F),
                    localScale = new Vector3(12.79138F, 17.81891F, 15.91274F)
                }
            });
            return rules;
        }

        public override void Hooks()
        {
            RecalculateStatsAPI.GetStatCoefficients += MrPipisEffect;
        }

        private void MrPipisEffect(CharacterBody sender, RecalculateStatsAPI.StatHookEventArgs args)
        {
            if (!NetworkServer.active || !sender) return;

            var existing = sender.GetComponent<MrPipisTracker>();
            if (sender.inventory && GetCount(sender) > 0 && !existing)
            {
                existing = sender.gameObject.AddComponent<MrPipisTracker>();
                existing.body = sender;
                existing.enabled = true;
            }
            else if (sender.inventory && GetCount(sender) <= 0 && existing) existing.enabled = false;
            else if (sender.inventory && GetCount(sender) > 0 && !existing.enabled) existing.enabled = true;
        }

        public class MrPipisTracker : CharacterBody.ItemBehavior
        {
            public CharacterBody body;
            List<BuffDef> allAffixes = new List<BuffDef>();

            private void Start()
            {
                allAffixes.Add(RoR2Content.Buffs.AffixBlue);
                allAffixes.Add(RoR2Content.Buffs.AffixEcho);
                allAffixes.Add(RoR2Content.Buffs.AffixHaunted);
                allAffixes.Add(RoR2Content.Buffs.AffixLunar);
                allAffixes.Add(RoR2Content.Buffs.AffixPoison);
                allAffixes.Add(RoR2Content.Buffs.AffixRed);
                allAffixes.Add(RoR2Content.Buffs.AffixWhite);
                allAffixes.Add(DLC1Content.Buffs.EliteVoid);
                allAffixes.Add(DLC1Content.Buffs.EliteEarth);
                //allAffixes.Add(DLC2Content.Buffs.EliteAurelionite);
                allAffixes.Add(DLC2Content.Buffs.EliteBead);
            }

            private void FixedUpdate()
            {
                foreach (var affix in allAffixes)
                {
                    if (!body.HasBuff(affix)) body.AddBuff(affix);
                }
            }

            private void OnDisable()
            {
                foreach (var affix in allAffixes)
                {
                    if (body.HasBuff(affix)) body.RemoveBuff(affix);
                }
            }
        }
    }
}
