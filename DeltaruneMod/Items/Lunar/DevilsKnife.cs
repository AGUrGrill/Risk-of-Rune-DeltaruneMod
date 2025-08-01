﻿using R2API;
using RoR2;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using static DeltaruneMod.DeltarunePlugin;

namespace DeltaruneMod.Items.Lunar
{
    public class DevilsKnife : ItemBase<DevilsKnife>
    {
        public override string ItemName => "Devilsknife";

        public override string ItemLangTokenName => "DEVIL_KNIFE";

        public override string ItemPickupDesc => "Gain a random effect every 10 seconds for 10 seconds...";

        public override string ItemFullDescription => "Grants <style=cIsUtility>WORLD REVOLVING</style> every 10 seconds.\n" +
            "<style=cIsUtility>WORLD REVOLVING</style>: Gain a random effect every 10 seconds.\nRandom effect duration increased by <style=cStack>+5 seconds per stack</style>.";

        public override string ItemLore => "You hear incomprehensible laughter and the world starts to spin.\nYou crave...  <style=cMono><style=cDeath>CHAOS</style></style>.";

        public override ItemTier Tier => ItemTier.Lunar;

        public override GameObject ItemModel => MainAssets.LoadAsset<GameObject>("devils_knife.prefab");

        public override Sprite ItemIcon => MainAssets.LoadAsset<Sprite>("devils_knife_icon.png");
        public override ItemTag[] ItemTags => new ItemTag[] { ItemTag.Utility };

        public override bool isChapter1 => true;

        public override bool isChapter2 => false;

        public override bool isChapter3 => false;

        public override bool isChapter4 => false;

        public static List<BuffDef> buffs = new List<BuffDef>();

        public override void Init()
        {
            CreateItem();
            CreateLang();
            Hooks();
        }

        public override void Hooks()
        {
            RecalculateStatsAPI.GetStatCoefficients += RecalculateStatsAPI_GetStatCoefficients;
        }

        private void RecalculateStatsAPI_GetStatCoefficients(CharacterBody sender, RecalculateStatsAPI.StatHookEventArgs args)
        {
            // If no buffs in list, populate
            if (buffs.Count <= 0) buffs = Util.Helpers.GetBuffs(99);
            if (!NetworkServer.active || !sender.inventory) return;

            #region Chaos Effect Controller
            var chaosInflicted = sender.GetComponent<WorldRevolvingEffect>();
            if (GetCount(sender) > 0)
            {
                if (!chaosInflicted)
                {
                    chaosInflicted = sender.gameObject.AddComponent<WorldRevolvingEffect>();
                    chaosInflicted.itemStacks = GetCount(sender);
                    chaosInflicted.body = sender;

                } 
                else if (chaosInflicted) chaosInflicted.itemStacks = GetCount(sender);
                else if (!chaosInflicted.enabled) chaosInflicted.enabled = true;
            }
            else if (chaosInflicted && GetCount(sender) <= 0) chaosInflicted.enabled = false;
            #endregion
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
                    childName = "Chest",
                    localPos = new Vector3(-0.0918F, 0.28106F, -0.19443F),
                    localAngles = new Vector3(355.1112F, 194.2511F, 10.04704F),
                    localScale = new Vector3(18.03568F, 18.03568F, 18.03568F)

                }
            });
            rules.Add("mdlHuntress", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Chest",
                    localPos = new Vector3(0.06383F, 0.06582F, -0.13282F),
                    localAngles = new Vector3(21.00448F, 143.5025F, 12.78009F),
                    localScale = new Vector3(15.62801F, 15.62801F, 15.68787F)

                }
            });
            rules.Add("mdlBandit2", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Chest",
                    localPos = new Vector3(-0.02745F, 0.10979F, -0.20252F),
                    localAngles = new Vector3(14.5175F, 172.3048F, 11.61553F),
                    localScale = new Vector3(16.23359F, 16.23359F, 16.23359F)

                }
            });
            rules.Add("mdlToolbot", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Chest",
                    localPos = new Vector3(0.77362F, 1.61468F, -1.71633F),
                    localAngles = new Vector3(354.6938F, 186.0387F, 17.37543F),
                    localScale = new Vector3(173.4925F, 173.4925F, 173.4925F)

                }
            });
            rules.Add("mdlEngi", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "HandL",
                    localPos = new Vector3(-0.06029F, 0.27899F, -0.02909F),
                    localAngles = new Vector3(0.03962F, 357.7523F, 73.14256F),
                    localScale = new Vector3(30.58468F, 30.58468F, 30.58468F)

                }
            });
            rules.Add("mdlMage", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Chest",
                    localPos = new Vector3(0.07952F, 0.14904F, -0.18145F),
                    localAngles = new Vector3(3.58573F, 3.50644F, 338.7134F),
                    localScale = new Vector3(16.85899F, 16.85899F, 16.85899F)

                }
            });
            rules.Add("mdlMerc", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Chest",
                    localPos = new Vector3(0.00552F, 0.1537F, -0.25501F),
                    localAngles = new Vector3(335.9026F, 177.474F, 74.81831F),
                    localScale = new Vector3(16.95792F, 16.95792F, 16.95792F)

                }
            });
            rules.Add("mdlTreebot", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "FlowerBase",
                    localPos = new Vector3(-0.34876F, -0.13709F, 0.27158F),
                    localAngles = new Vector3(1.87692F, 267.5815F, 248.1591F),
                    localScale = new Vector3(40.3113F, 40.3113F, 40.3113F)

                }
            });
            rules.Add("mdlLoader", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Chest",
                    localPos = new Vector3(0.09222F, 0.05228F, -0.36294F),
                    localAngles = new Vector3(342.0858F, 176.2473F, 159.1659F),
                    localScale = new Vector3(19.74982F, 19.74982F, 19.74982F)

                }
            });
            rules.Add("mdlCroco", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Head",
                    localPos = new Vector3(1.85098F, -0.29846F, 0.82102F),
                    localAngles = new Vector3(310.729F, 40.41219F, 76.69464F),
                    localScale = new Vector3(91.30646F, 91.30646F, 91.30646F)

                }
            });
            rules.Add("mdlCaptain", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Chest",
                    localPos = new Vector3(-0.01588F, 0.00531F, -0.25708F),
                    localAngles = new Vector3(0.98438F, 19.50697F, 301.9747F),
                    localScale = new Vector3(23.92296F, 23.92296F, 23.92296F)

                }
            });
            rules.Add("mdlRailGunner", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "GunStock",
                    localPos = new Vector3(0.07663F, -0.00201F, -0.06209F),
                    localAngles = new Vector3(273.7904F, 32.73811F, 110.7573F),
                    localScale = new Vector3(2.15812F, 2.32198F, 2.32198F)
                }
            });
            rules.Add("mdlVoidSurvivor", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Chest",
                    localPos = new Vector3(0.05944F, 0.05716F, -0.23154F),
                    localAngles = new Vector3(344.676F, 6.65559F, 332.8428F),
                    localScale = new Vector3(20.51301F, 17.09416F, 17.09416F)

                }
            });
            rules.Add("mdlChef", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Chest",
                    localPos = new Vector3(-0.08292F, -0.40159F, 0.00578F),
                    localAngles = new Vector3(301.4835F, 177.4607F, 252.1901F),
                    localScale = new Vector3(16.43113F, 14.08382F, 14.08382F)

                }
            });
            rules.Add("mdlSeeker", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Chest",
                    localPos = new Vector3(-0.08968F, 0.16127F, -0.3987F),
                    localAngles = new Vector3(349.7654F, 12.93527F, 20.32264F),
                    localScale = new Vector3(18.98264F, 16.87346F, 16.87346F)

                }
            });
            rules.Add("mdlFalseSon", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Head",
                    localPos = new Vector3(-0.00613F, 0.42402F, 0.07981F),
                    localAngles = new Vector3(351.1979F, 260.1143F, 336.6507F),
                    localScale = new Vector3(18.54679F, 18.54679F, 18.54679F)

                }
            });
            rules.Add("mdlRalsei", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "HandL",
                    localPos = new Vector3(-0.00513F, 0.00236F, -0.00172F),
                    localAngles = new Vector3(15.54062F, 339.6257F, 61.98112F),
                    localScale = new Vector3(0.4012F, 0.4012F, 0.4012F)

                }
            });
            return rules;
        }
    
        // Handles buff applications
        public class WorldRevolvingEffect : MonoBehaviour
        {
            private float timer = 0f;
            private float maxTime = 10f;
            public CharacterBody body;
            public int itemStacks = 0;

            private void Awake()
            {
                base.enabled = false;
            }
            private void OnEnabled()
            {

            }
            // Timer for chaos (buff application)
            private void FixedUpdate()
            {
                timer -= Time.fixedDeltaTime;
                if (timer <= 0f)
                {
                    ChaosChaos();
                    timer = maxTime;
                }
            }
            // Find random buff user doesnt have and give it
            //
            // SEEKER ISSUE - When sojourn causes seeker to disappear, maybe make timer per person?
            public void ChaosChaos()
            {
                BuffDef randomBuff;
                while (true)
                {
                    randomBuff = buffs[UnityEngine.Random.Range(0, buffs.Count)];
                    if (!body.HasBuff(randomBuff)) break;
                    Debug.Log(body.name + " already has buff " + randomBuff.name + "! Cycling to next buff...");
                }
                body.AddTimedBuff(randomBuff, 5 + (itemStacks * 5));
                Debug.Log("Added random buff:" + randomBuff.name + " to " + body.name);
            }
            private void OnDisable()
            {
                Destroy(this);
            }
        }
    }
}
