using DeltaruneMod.Items.Tier1.Gacha;
using R2API;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Android;

namespace DeltaruneMod.Items.Tier3
{
    public class ScrapBall : ItemBase<ScrapBall>
    {
        public override string ItemName => "Junk Ball";

        public override string ItemLangTokenName => "SCRAP_BALL";

        public override string ItemPickupDesc => "Gain a boost to damage and attack speed for each consumed item you own.";

        public override string ItemFullDescription => "Gain a 5% boost to damage and attack speed for each consumed item you own. <style=cStack>(+5% per stack)</style>";

        public override string ItemLore => "You reach into your pocket and find a small ball full of accumulated things in your pocket.\n" +
            "\nYou look at the ball of junk in admiration of the useless itmes you've accumulated... Nothing happened.";

        public override ItemTier Tier => ItemTier.Tier3;

        public override GameObject ItemModel => DeltarunePlugin.MainAssets.LoadAsset<GameObject>("junk_ball.prefab");

        public override Sprite ItemIcon => DeltarunePlugin.MainAssets.LoadAsset<Sprite>("scrap_ball_icon.png");

        public override bool isChapter1 => false;

        public override bool isChapter2 => false;

        public override bool isChapter3 => false;

        public override bool isChapter4 => false;

        public override ItemTag[] ItemTags => new ItemTag[] { ItemTag.Utility };

        private const float StatMulti = 0.05f;

        public static List<ItemDef> consumedItems = new List<ItemDef>();

        public override void Init()
        {
            CreateItem();
            CreateLang();
            CreateItemDisplayRules();
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
                    childName = "FootL",
localPos = new Vector3(0.00155F, 0.18369F, -0.03625F),
localAngles = new Vector3(12.71502F, 118.5311F, 27.65873F),
localScale = new Vector3(3.43066F, 3.43066F, 3.43066F)
                }
            });
            rules.Add("mdlHuntress", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Head",
localPos = new Vector3(-0.05813F, 0.29579F, -0.03698F),
localAngles = new Vector3(344.4833F, 157.2875F, 24.74886F),
localScale = new Vector3(3.00498F, 2.64008F, 3.00498F)
                }
            });
            rules.Add("mdlToolbot", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Head",
localPos = new Vector3(1.48266F, 0.89384F, 0.76922F),
localAngles = new Vector3(346.6342F, 290.7388F, 291.0602F),
localScale = new Vector3(64.77013F, 64.77013F, 64.77013F)
                }
            });
            rules.Add("mdlEngi", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Chest",
localPos = new Vector3(0.35174F, 0.2505F, -0.02213F),
localAngles = new Vector3(314.914F, 17.67926F, 111.525F),
localScale = new Vector3(7.94387F, 7.33221F, 7.7634F)

                }
            });
            rules.Add("mdlMage", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "ThighR",
localPos = new Vector3(-0.08869F, 0.1156F, 0.05396F),
localAngles = new Vector3(335.5005F, 89.55374F, 161.365F),
localScale = new Vector3(4.43965F, 4.43965F, 4.43965F)

                }
            });
            rules.Add("mdlMerc", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "ThighL",
localPos = new Vector3(0.08383F, 0.06065F, 0.02712F),
localAngles = new Vector3(353.3118F, 275.5083F, 186.1312F),
localScale = new Vector3(7.12055F, 7.12055F, 7.12055F)
                }
            });
            rules.Add("mdlTreebot", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Chest",
localPos = new Vector3(-0.53256F, 0.72782F, -0.46537F),
localAngles = new Vector3(347.8391F, 274.0605F, 346.7606F),
localScale = new Vector3(15.32926F, 15.32926F, 15.32926F)
                }
            });
            rules.Add("mdlLoader", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "ThighL",
localPos = new Vector3(0.05372F, 0.07789F, 0.07749F),
localAngles = new Vector3(318.7485F, 78.9231F, 15.94955F),
localScale = new Vector3(6.26722F, 7.77087F, 7.77087F)
                }
            });
            rules.Add("mdlCroco", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Stomach",
localPos = new Vector3(-1.68466F, 0.80861F, 1.56104F),
localAngles = new Vector3(345.425F, 279.6749F, 238.6632F),
localScale = new Vector3(54.54406F, 54.54406F, 54.54406F)

                }
            });
            rules.Add("mdlCaptain", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "FootL",
localPos = new Vector3(0.03731F, 0.10456F, -0.07764F),
localAngles = new Vector3(325.7554F, 162.5365F, 92.34225F),
localScale = new Vector3(4.41354F, 4.41354F, 4.41354F)
                }
            });
            rules.Add("mdlBandit2", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "FootR",
localPos = new Vector3(-0.007F, 0.12154F, -0.07454F),
localAngles = new Vector3(47.78607F, 8.74319F, 143.9257F),
localScale = new Vector3(3.05221F, 3.05221F, 3.05221F)
                }
            });
            rules.Add("mdlVoidSurvivor", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Stomach",
localPos = new Vector3(-0.14299F, 0.03259F, -0.03299F),
localAngles = new Vector3(354.3749F, 132.6399F, 300.215F),
localScale = new Vector3(5.93916F, 6.90403F, 4.9493F)
                }
            });
            rules.Add("mdlChef", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Chest",
localPos = new Vector3(-0.01618F, -0.19109F, -0.28981F),
localAngles = new Vector3(43.93672F, 216.2693F, 305.2357F),
localScale = new Vector3(10.53911F, 10.25567F, 9.03356F)
                }
            });
            rules.Add("mdlSeeker", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "ThighR",
localPos = new Vector3(-0.04061F, 0.10606F, 0.09827F),
localAngles = new Vector3(33.35695F, 358.3484F, 175.5982F),
localScale = new Vector3(6.29222F, 5.59305F, 5.59305F)
                }
            });
            rules.Add("mdlFalseSon", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "LowerArmL",
localPos = new Vector3(0.112F, 0.25971F, -0.09989F),
localAngles = new Vector3(347.2914F, 292.1277F, 127.8913F),
localScale = new Vector3(10.62006F, 10.62006F, 10.62006F)

                }
            });
            return rules;
        }


        public override void Hooks()
        {
            //On.RoR2.CharacterBody.OnInventoryChanged += CharacterBody_OnInventoryChanged;
            RecalculateStatsAPI.GetStatCoefficients += RecalculateStatsAPI_GetStatCoefficients;
        }

        private void RecalculateStatsAPI_GetStatCoefficients(CharacterBody sender, RecalculateStatsAPI.StatHookEventArgs args)
        {
            #region Add Buffs
            if (GetCount(sender) > 0)
            {
                int amountOfBrokenItems = 0;
                if (consumedItems.Count <= 0) GetConsumedItems();
                foreach (ItemDef item in consumedItems)
                {
                    amountOfBrokenItems += sender.inventory.GetItemCountEffective(item);
                }
                args.damageTotalMult += amountOfBrokenItems * StatMulti;
                args.attackSpeedMultAdd += amountOfBrokenItems * StatMulti;
            }
            #endregion
        }

        private void GetConsumedItems()
        {
            List<String> validItems = new List<String>();
            validItems.Add("HealingPotionConsumed");
            validItems.Add("TonicAffliction");
            validItems.Add("TeleportOnLowHealthConsumed");
            validItems.Add("FragileDamageBonusConsumed");
            validItems.Add("ExtraLifeVoidConsumed");
            validItems.Add("ExtraLifeConsumed");
            validItems.Add("LowerPricedChestsConsumed");
            validItems.Add("RegeneratingScrapConsumed");
            foreach (ItemIndex item in RoR2.ItemCatalog.allItems)
            {
                ItemDef itemDef = RoR2.ItemCatalog.GetItemDef(item);
                foreach (String itemName in validItems)
                {
                    if (itemName.Equals(itemDef.name))
                    {
                        consumedItems.Add(itemDef);
                    }
                }
            }
            consumedItems.Add(TVDinnerConsumed.instance.ItemDef);
            consumedItems.Add(ExecBuffetConsumed.instance.ItemDef);

        }

        /*
        private void CharacterBody_OnInventoryChanged(On.RoR2.CharacterBody.orig_OnInventoryChanged orig, CharacterBody self)
        {
            #region Add Timer
            var timer = self.GetComponent<ScrapTimer>();
            if (GetCount(self) > 0 && !timer)
            {
                timer = self.gameObject.AddComponent<ScrapTimer>();
                timer.player = self;
                timer.stack = GetCount(self);
                timer.enabled = true;
                Debug.Log("Scrap timer given.");
            }
            else if (GetCount(self) > 0 && timer && timer.stack < GetCount(self)) timer.stack = GetCount(self); // Refresh stack count when needed
            else if (GetCount(self) <= 0 && timer)
            {
                timer.enabled = false;
            }
            #endregion
        }
        */
        /*
        private class ScrapTimer : MonoBehaviour
        {
            readonly float timerInterval = 180f; // In seconds
            float timer = 0f;

            public CharacterBody player;
            public int stack;

            enum ScrapWeight // Out of 100
            {
                white = 50,
                green = 88,
                red = 93,
                yellow = 98,
                regen = 100
            }

            private void Awake()
            {
                base.enabled = false;
            }

            private void OnEnable()
            {
                if (!player)
                {
                    Debug.Log("Player not found! Destroying scrap timer...");
                    Destroy(this);
                }
                GiveRandomScrap();
                timer = timerInterval;
            }

            private void OnDisable()
            {
                Destroy(this);
            }

            // Timer
            private void FixedUpdate()
            {
                timer -= Time.fixedDeltaTime;
                if (timer <= 0)
                {
                    GiveRandomScrap();
                    timer = timerInterval;
                }
            }
            // Give scrap
            private void GiveRandomScrap()
            {
                foreach (ItemIndex item in RoR2.ItemCatalog.allItems)
                {
                    Debug.Log(item + ": " + RoR2.ItemCatalog.GetItemDef(item));
                }
                int givenValue = UnityEngine.Random.Range(0, 100);
                if (givenValue <= (int)ScrapWeight.white) player.inventory.GiveItemPermanent((ItemIndex)216); // White scrap index
                else if (givenValue <= (int)ScrapWeight.green) player.inventory.GiveItemPermanent((ItemIndex)212); // Green scrap index
                else if (givenValue <= (int)ScrapWeight.red) player.inventory.GiveItemPermanent((ItemIndex)214); // Red scrap index
                else if (givenValue <= (int)ScrapWeight.yellow) player.inventory.GiveItemPermanent((ItemIndex)218); // Yellow scrap index
                else if (givenValue <= (int)ScrapWeight.regen) player.inventory.GiveItemPermanent((ItemIndex)208); // Regen scrap index
                Debug.Log("Giving scrap (" + givenValue + ") to " + player.name + "...");

            }
        }
        */
    }
}
