using DeltaruneMod.Items.Spamton;
using R2API;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using static DeltaruneMod.DeltarunePlugin;

namespace DeltaruneMod.Items.Lunar
{
    public class ThornRing : ItemBase<ThornRing>
    {
        public override string ItemName => "Ring of Regret";

        public override string ItemLangTokenName => "THORN_RING";

        public override string ItemPickupDesc => "Receive <style=cDeath>pain</style> to become <style=cIsUtility><style=cMono>stronger</style></style>.";

        public override string ItemFullDescription => "Apply <style=cIsUtility>1</style> stack of <style=cIsUtility>frostbite on hit </style><style=cStack>(+1 per stack)</style>" +
            "\nStacks of frostbite cause enemies to <style=cIsUtility>freeze</style>." +
            "\nLose <style=cIsHealth>30% hp</style> <style=cStack>(10% hp per stack)</style>.";

        public override string ItemLore => "<style=cShrine>[Angel]</style>, <style=cShrine>[Angel]</style> \nARE YOU LOOKING FOR THE <style=cIsUtility>[Ring]</style>\n OF <style=cDeath>[Thorns]</style> ?";

        public override ItemTier Tier => ItemTier.Lunar;

        public override GameObject ItemModel => MainAssets.LoadAsset<GameObject>("thorn_ring.prefab");

        public override Sprite ItemIcon => MainAssets.LoadAsset<Sprite>("thorn_ring_icon.png");

        public override ItemTag[] ItemTags => new ItemTag[] { ItemTag.Damage };

        public override bool isChapter1 => false;

        public override bool isChapter2 => true;

        public override bool isChapter3 => false;

        public override bool isChapter4 => false;

        public static BuffDef frostbite;

        public static bool healthAmputated = false;

        public static Sprite FrostbiteEffectIcon = MainAssets.LoadAsset<Sprite>("snowgrave_effect_icon.png");

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
                    localPos = new Vector3(0.04669F, 0.16169F, -0.01152F),
                    localAngles = new Vector3(291.7313F, 35.89888F, 149.2341F),
                    localScale = new Vector3(9.08816F, 9.08816F, 9.08816F)
                }
            });
            rules.Add("mdlHuntress", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "HandR",
                    localPos = new Vector3(-0.06185F, 0.13062F, -0.08525F),
                    localAngles = new Vector3(345.1665F, 257.8601F, 270.5199F),
                    localScale = new Vector3(11.79088F, 12.10101F, 11.79088F)
                }
            });
            rules.Add("mdlEngi", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "HandR",
                    localPos = new Vector3(-0.01161F, 0.19996F, -0.02145F),
                    localAngles = new Vector3(345.7541F, 323.9921F, 201.2191F),
                    localScale = new Vector3(17.70485F, 17.70485F, 17.47323F)
                }
            });
            rules.Add("mdlMage", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "HandR",
                    localPos = new Vector3(0.00205F, 0.18314F, 0.02396F),
                    localAngles = new Vector3(331.9111F, 208.5898F, 186.1979F),
                    localScale = new Vector3(5.52692F, 5.52692F, 5.52692F)

                }
            });
            rules.Add("mdlMerc", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Finger22R",
                    localPos = new Vector3(0.02569F, 0.04368F, -0.02502F),
                    localAngles = new Vector3(344.3791F, 171.3167F, 179.5275F),
                    localScale = new Vector3(13.25547F, 16.42367F, 10.72907F)
                }
            });
            rules.Add("mdlTreebot", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "FootFrontR",
                    localPos = new Vector3(-0.01287F, 1.18832F, -0.0293F),
                    localAngles = new Vector3(4.4797F, 193.5389F, 177.6235F),
                    localScale = new Vector3(100.0454F, 100.0454F, 100.0454F)
                }
            });
            rules.Add("mdlLoader", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "MechFinger23R",
                    localPos = new Vector3(0.01341F, 0.01209F, 0.07881F),
                    localAngles = new Vector3(78.415F, 195.9934F, 13.50731F),
                    localScale = new Vector3(30.7553F, 30.7553F, 30.7553F)
                }
            });
            rules.Add("mdlCroco", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Finger11L",
                    localPos = new Vector3(-0.21772F, 0.50604F, 0.76034F),
                    localAngles = new Vector3(9.3553F, 270.8647F, 167.1162F),
                    localScale = new Vector3(250.6978F, 250.6978F, 250.6978F)
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
                    localScale = new Vector3(14.2909F, 14.2909F, 14.2909F)
                }
            });
            rules.Add("mdlBandit2", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "HandL",
                    localPos = new Vector3(0.05166F, 0.13287F, -0.02415F),
                    localAngles = new Vector3(320.5668F, 121.0486F, 243.9749F),
                    localScale = new Vector3(10.78896F, 10.62206F, 11.96763F)
                }
            });
            rules.Add("mdlVoidSurvivor", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "RingFinger",
                    localPos = new Vector3(-0.00086F, 0.07828F, -0.00452F),
                    localAngles = new Vector3(346.3212F, 77.13858F, 175.9458F),
                    localScale = new Vector3(11.62079F, 13.5087F, 13.63391F)
                }
            });
            rules.Add("mdlChef", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Wheel",
                    localPos = new Vector3(0.50003F, 0.19972F, 0.04703F),
                    localAngles = new Vector3(271.4667F, 53.71669F, 310.0683F),
                    localScale = new Vector3(27.85436F, 27.73527F, 27.16877F)
                }
            });
            rules.Add("mdlSeeker", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "HandR",
                    localPos = new Vector3(-0.0121F, 0.12903F, 0.00147F),
                    localAngles = new Vector3(310.499F, 327.4647F, 205.2438F),
                    localScale = new Vector3(8.85501F, 8.93391F, 8.93391F)
                }
            });
            rules.Add("mdlFalseSon", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "HandR",
                    localPos = new Vector3(0.0936F, 0.26244F, 0.014F),
                    localAngles = new Vector3(353.4551F, 18.60704F, 348.559F),
                    localScale = new Vector3(18.38745F, 18.38745F, 18.38745F)
                }
            });
            return rules;
        }

        // Blacklist from lunar shop
        public override void Init()
        {
            CreateItem();
            CreateLang();
            Hooks();
        }

        public override void Hooks()
        {
            On.RoR2.GlobalEventManager.OnHitEnemy += GlobalEventManager_OnHitEnemy;
            RecalculateStatsAPI.GetStatCoefficients += RecalculateStatsAPI_GetStatCoefficients;
        }

        private void RecalculateStatsAPI_GetStatCoefficients(CharacterBody sender, RecalculateStatsAPI.StatHookEventArgs args)
        {
            if (!NetworkServer.active) return;

            
            int itemCount = GetCount(sender);
            if (sender.inventory && itemCount > 0)
            {
                float curseHPReduction = 0.3f + (0.1f * (itemCount - 1));

                // Cap HP reduction cause, well ya
                if (curseHPReduction >= 0.7f) curseHPReduction = 0.7f;

                // Force HP mult to be curse amount
                args.healthTotalMult *= (1 - curseHPReduction);

                // Convert comm ring to thorn ring if applicable
                ItemDef commRing = CommRing.instance.ItemDef;
                int commRingItemCount = sender.inventory.GetItemCount(commRing);
                if (commRingItemCount > 0)
                {
                    for (int i = 0; i < commRingItemCount; i++)
                    {
                        sender.inventory.RemoveItem(commRing);
                        sender.inventory.GiveItem(ItemDef);
                    }
                }
            }
        }

        public void CreateEffect()
        {
            frostbite = DLC2Content.Buffs.Frost;
            frostbite.name = "FrostbiteDebuff";
            frostbite.iconSprite = FrostbiteEffectIcon;
            frostbite.isDebuff = true;
        }

        private void GlobalEventManager_OnHitEnemy(On.RoR2.GlobalEventManager.orig_OnHitEnemy orig, GlobalEventManager self, DamageInfo damageInfo, GameObject victim)
        {
            orig(self, damageInfo, victim);

            if (!NetworkServer.active) return;

            var attacker = damageInfo.attacker;
            if (!attacker) return;
            var attackerBody = attacker.GetComponent<CharacterBody>();
            if (!attackerBody || !attackerBody.isPlayerControlled) return;
            var victimBody = victim.GetComponent<CharacterBody>();
            if (!victimBody || victimBody.isPlayerControlled) return;

            int itemCount = GetCount(attackerBody);

            // This works like nowhere cause "frost buff isnt created yet or some shi" like bruh ok
            if (!frostbite)
            {
                CreateEffect();
            }

            // Add debuff to enemy
            if (victimBody.name == "BrotherBody(Clone)" || victimBody.name == "ITBrotherBody(Clone)" || victimBody.name == "BrotherHurtBody(Clone)" || victimBody.name == "BrotherGlassBody(Clone)") return;
            if (attackerBody.isPlayerControlled && itemCount > 0)
            {
                for (int i = 0; i < itemCount; i++)
                {
                    victimBody.AddBuff(frostbite);
                }    
            }
        }
    }
}
