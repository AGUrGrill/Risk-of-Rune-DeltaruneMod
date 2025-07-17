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
        public override string ItemName => "Thorn Ring";

        public override string ItemLangTokenName => "THORN_RING";

        public override string ItemPickupDesc => "Receive <style=cDeath>pain</style> to become <style=cIsUtility><style=cMono>stronger</style></style>.";

        public override string ItemFullDescription => "Apply <style=cIsUtility>1</style> stack of <style=cIsUtility>frostbite on hit </style><style=cStack>(+1 per stack)</style>" +
            "\nStacks of frostbite cause enemies to <style=cIsUtility>freeze</style>." +
            "\nLose <style=cIsHealth>-5% hp</style> on hit <style=cStack>(-3% hp per stack)</style>." +
            "\nYou cannot lose more than <style=cIsHealth>5% hp</style>.";

        public override string ItemLore => "<style=cShrine>[Angel]</style>, <style=cShrine>[Angel]</style> \nARE YOU LOOKING FOR THE <style=cIsUtility>[Ring]</style>\n OF <style=cDeath>[Thorns]</style> ?";

        public override ItemTier Tier => ItemTier.Lunar;

        public override GameObject ItemModel => MainAssets.LoadAsset<GameObject>("thorn_ring.prefab");

        public override Sprite ItemIcon => MainAssets.LoadAsset<Sprite>("thorn_ring_icon.png");

        public override ItemTag[] ItemTags => new ItemTag[] { ItemTag.Damage };

        public static BuffDef frostbite;

        public static bool healthAmputated = false;

        public static float playerNormalHP;

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
                    childName = "Head",
                    localPos = new Vector3(0.34926F, 0.25313F, -0.41051F),
                    localAngles = new Vector3(14.01768F, 50.88708F, 82.11794F),
                    localScale = new Vector3(23.74224F, 23.74224F, 23.74224F)

                }
            });
            rules.Add("mdlHuntress", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Pelvis",
                    localPos = new Vector3(0.09873F, -0.13067F, 0.02605F),
                    localAngles = new Vector3(23.6536F, 93.07247F, 24.55241F),
                    localScale = new Vector3(25.51046F, 16.67866F, 25.51046F)

                }
            });
            rules.Add("mdlToolbot", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "UpperArmL",
                    localPos = new Vector3(0.40818F, 0.30742F, 0.55069F),
                    localAngles = new Vector3(2.01947F, 281.0584F, 67.86629F),
                    localScale = new Vector3(166.2022F, 166.2022F, 166.2022F)

                }
            });
            rules.Add("mdlEngi", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "HandR",
                    localPos = new Vector3(-0.28472F, 0.15643F, -0.12816F),
                    localAngles = new Vector3(354.5077F, 347.7494F, 281.5692F),
                    localScale = new Vector3(50.83732F, 50.83732F, 50.83732F)

                }
            });
            rules.Add("mdlMage", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "ThighR",
                    localPos = new Vector3(-0.12374F, 0.37286F, -0.03677F),
                    localAngles = new Vector3(5.54981F, 275.2047F, 216.0641F),
                    localScale = new Vector3(18.16601F, 18.16601F, 18.16601F)

                }
            });
            rules.Add("mdlMerc", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Chest",
                    localPos = new Vector3(0.18235F, -0.04304F, -0.42368F),
                    localAngles = new Vector3(340.2831F, 185.451F, 340.5156F),
                    localScale = new Vector3(24.97854F, 24.97854F, 24.97854F)

                }
            });
            rules.Add("mdlTreebot", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "FlowerBase",
                    localPos = new Vector3(-1.09289F, 0.13743F, -0.11748F),
                    localAngles = new Vector3(3.25975F, 162.9752F, 33.30826F),
                    localScale = new Vector3(30.4944F, 30.4944F, 30.4944F)
                }
            });
            rules.Add("mdlLoader", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Head",
                    localPos = new Vector3(0.28829F, 0.38631F, 0.02157F),
                    localAngles = new Vector3(6.18454F, 3.13923F, 148.0287F),
                    localScale = new Vector3(13.47214F, 13.47214F, 13.47214F)

                }
            });
            rules.Add("mdlCroco", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Chest",
                    localPos = new Vector3(-2.4045F, -1.67265F, 2.1141F),
                    localAngles = new Vector3(354.3773F, 254.687F, 358.6025F),
                    localScale = new Vector3(121.9903F, 121.9903F, 121.9903F)

                }
            });
            rules.Add("mdlCaptain", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Head",
                    localPos = new Vector3(0.34605F, 0.39082F, -0.10218F),
                    localAngles = new Vector3(5.15182F, 25.06269F, 146.8649F),
                    localScale = new Vector3(14.66418F, 14.66418F, 14.66418F)

                }
            });
            rules.Add("mdlBandit2", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Stomach",
                    localPos = new Vector3(0.09482F, 0.14564F, 0.01243F),
                    localAngles = new Vector3(331.879F, 77.49539F, 210.0147F),
                    localScale = new Vector3(15.98449F, 15.98449F, 15.98449F)

                }
            });
            rules.Add("mdlRailGunner", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "BottomRail",
                    localPos = new Vector3(-0.01703F, 0.23118F, -0.02844F),
                    localAngles = new Vector3(1.06541F, 180.4571F, 13.00509F),
                    localScale = new Vector3(15.71294F, 15.71294F, 15.71294F)

                }
            });
            rules.Add("mdlVoidSurvivor", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "CannonEnd",
                    localPos = new Vector3(0.13748F, -0.11991F, 0.07944F),
                    localAngles = new Vector3(4.53002F, 272.7654F, 4.84901F),
                    localScale = new Vector3(21.07476F, 24.49854F, 17.5623F)

                }
            });
            rules.Add("mdlChef", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Chest",
                    localPos = new Vector3(0.04536F, -0.42105F, 0.09325F),
                    localAngles = new Vector3(320.1729F, 10.61689F, 101.4086F),
                    localScale = new Vector3(21.83015F, 18.71156F, 18.71156F)

                }
            });
            rules.Add("mdlSeeker", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Chest",
                    localPos = new Vector3(0.2744F, -0.30065F, 0.07598F),
                    localAngles = new Vector3(328.4284F, 358.3063F, 35.0899F),
                    localScale = new Vector3(15.67788F, 13.93588F, 13.93588F)

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
            rules.Add("mdlRalsei", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Back",
                    localPos = new Vector3(-0.00243F, -0.00212F, -0.00694F),
                    localAngles = new Vector3(347.736F, 180.3236F, 41.70065F),
                    localScale = new Vector3(0.60172F, 0.60172F, 0.60172F)

                }
            });
            return rules;
        }

        // Blacklist from lunar shop
        public override void Init()
        {
            //CreateItem();
            //CreateLang();
            //Hooks();
        }

        public override void Hooks()
        {
            On.RoR2.GlobalEventManager.OnHitEnemy += ThornRingEffect;
            RecalculateStatsAPI.GetStatCoefficients += CommRingToThornRing;
        }

        private void CommRingToThornRing(CharacterBody sender, RecalculateStatsAPI.StatHookEventArgs args)
        {
            if (!NetworkServer.active) return;

            int itemCount = GetCount(sender);
            if (sender.inventory && itemCount > 0)
            {
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

                sender.healthComponent.health = sender.maxHealth * 0.7f;
                

            }
            else if (healthAmputated && itemCount <= 0 && sender.inventory)
            {
                sender.baseMaxHealth /= 0.7f;
                sender.healthComponent.health = sender.maxHealth;
                healthAmputated = false;
            }
        }

        public void ThornRingEffect(On.RoR2.GlobalEventManager.orig_OnHitEnemy orig, GlobalEventManager self, DamageInfo damageInfo, GameObject victim)
        {
            orig(self, damageInfo, victim);

            if (!NetworkServer.active) return;

            var attacker = damageInfo.attacker;
            CharacterBody sender = attacker.GetComponent<CharacterBody>();
            CharacterBody victimBody = victim.GetComponent<CharacterBody>();

            if (!sender.isPlayerControlled || victimBody.isPlayerControlled) return;

            int itemCount = GetCount(sender);

            frostbite = DLC2Content.Buffs.Frost;
            frostbite.name = "FrostbiteDebuff";
            frostbite.iconSprite = FrostbiteEffectIcon;
            frostbite.isDebuff = true;

            //float thornDmg = sender.baseMaxHealth * (0.05f + (0.03f * (itemCount-1)));
            //int stacksFrostbite = victimBody.GetBuffCount(frostbite);

            if (sender.inventory && itemCount > 0)
            {
                for (int i = 0; i < itemCount; i++)
                {
                    victimBody.AddBuff(frostbite);
                    sender.healthComponent.health -= (sender.maxHealth * 0.01f);
                }
            }
        }
    }
}
