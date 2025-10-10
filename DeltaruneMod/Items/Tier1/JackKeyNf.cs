using R2API;
using RoR2;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using static DeltaruneMod.DeltarunePlugin;
using DeltaruneMod.Items.Tier1;
using IL.RoR2.Artifacts;

namespace DeltaruneMod.Items.Tier1
{
    public class JackKeyNf : ItemBase<JackKeyNf>
    {
        public override string ItemName => "Closet Key";

        public override string ItemLangTokenName => "JACK_KEY";

        public override string ItemPickupDesc => "Every 30 seconds on stage, move 5% faster.";

        public override string ItemFullDescription => "Every <style=cIsUtility>30</style> seconds on stage, " +
            "gain <style=cIsUtility>5%</style> movement speed <style=cStack>(+1% per stack)" +
            " [resets after each stage]</style>.";

        public override string ItemLore => "Stumbling around in the darkness, you enter a room." +
            "\nIn this void there is nothing to see but a faint glow." +
            "\nA drawer is shown, you see the surrounding but no one is here, it looks like a study of sorts..." +
            "\nAs you open the drawer the light shines brightly, " +
            "\nyou feel a strange pulse, your mind racing, a strange voice calls to you." +
            "\nThe laughter is taunting you, the words in your mind, ringing, " +
            "\n<style=cMono><style=cIsDamage>\"YOU'RE TAKING TOO LONG\"</style></style>.";

        public override ItemTier Tier => ItemTier.Tier1;

        public override GameObject ItemModel => MainAssets.LoadAsset<GameObject>("sugma_balls.prefab");

        public override Sprite ItemIcon => MainAssets.LoadAsset<Sprite>("jack_key_icon.png");

        public override bool isChapter1 => false;

        public override bool isChapter2 => false;

        public override bool isChapter3 => false;

        public override bool isChapter4 => true;

        public static BuffDef JackBuff;

        // Numbers for stuff
        private readonly float multi = 0.05f;

        private readonly float baseMulti = 0.01f;
        
        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            ItemDisplayRuleDict rules = new ItemDisplayRuleDict();
            rules.Add("mdlCommandoDualies", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "ThighR",
                    localPos = new Vector3(-0.1798F, 0.49651F, -0.04438F),
                    localAngles = new Vector3(350.9123F, 102.8262F, 174.6391F),
                    localScale = new Vector3(40F, 40F, 40F)

                }
            });
            rules.Add("mdlHuntress", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "ThighR",
                    localPos = new Vector3(0.12523F, 0.57639F, 0.02619F),
                    localAngles = new Vector3(17.53846F, 74.32281F, 173.9909F),
                    localScale = new Vector3(40F, 40F, 40F)
                }
            });
            rules.Add("mdlBandit2", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "MainWeapon",
                    localPos = new Vector3(-0.33806F, -0.16264F, 0.10024F),
                    localAngles = new Vector3(338.4061F, 341.8553F, 342.4209F),
                    localScale = new Vector3(40.17231F, 40.17231F, 40.17231F)
                }
            });
            rules.Add("mdlToolbot", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Stomach",
                    localPos = new Vector3(-0.35322F, -0.65284F, -0.83172F),
                    localAngles = new Vector3(36.83676F, 79.64436F, 347.0035F),
                    localScale = new Vector3(200F, 200F, 200F)
                }
            });
            rules.Add("mdlEngi", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "MuzzleRight",
                    localPos = new Vector3(-0.28417F, 0.53592F, -0.1511F),
                    localAngles = new Vector3(346.3751F, 101.7892F, 180.4147F),
                    localScale = new Vector3(40F, 40F, 40F)
                }
            });
            rules.Add("mdlMage", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "ThighL",
                    localPos = new Vector3(0.22521F, 0.76136F, 0.1713F),
                    localAngles = new Vector3(342.6173F, 269.3727F, 178.4107F),
                    localScale = new Vector3(40F, 40F, 40F)

                }
            });
            rules.Add("mdlMerc", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "ThighL",
                    localPos = new Vector3(0.24248F, 0.70388F, 0.12107F),
                    localAngles = new Vector3(343.777F, 256.632F, 178.1319F),
                    localScale = new Vector3(40F, 40F, 40F)
                }
            });
            rules.Add("mdlTreebot", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "WeaponPlatform",
                    localPos = new Vector3(-0.19147F, 0.16725F, -0.25981F),
                    localAngles = new Vector3(354.1692F, 102.2908F, 71.60953F),
                    localScale = new Vector3(40F, 40F, 40F)
                }
            });
            rules.Add("mdlLoader", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "ThighR",
                    localPos = new Vector3(-0.35432F, -0.24068F, -0.19672F),
                    localAngles = new Vector3(25.01427F, 96.96656F, 30.84815F),
                    localScale = new Vector3(40F, 40F, 40F)
                }
            });
            rules.Add("mdlCroco", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "LowerArmL",
                    localPos = new Vector3(-1.82523F, 3.39563F, 0.22619F),
                    localAngles = new Vector3(29.40898F, 158.3239F, 83.72023F),
                    localScale = new Vector3(60.59527F, 60.59527F, 60.59527F)
                }
            });
            rules.Add("mdlCaptain", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Stomach",
                    localPos = new Vector3(-0.54048F, -0.33216F, -0.12587F),
                    localAngles = new Vector3(24.03601F, 74.05122F, 1.67272F),
                    localScale = new Vector3(40F, 40F, 40F)
                }
            });
            rules.Add("mdlRailGunner", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "ThighR",
                    localPos = new Vector3(-0.22293F, 0.81333F, 0.1847F),
                    localAngles = new Vector3(340.2833F, 97.1996F, 200.2289F),
                    localScale = new Vector3(40F, 40F, 40F)
                }
            });
            rules.Add("mdlVoidSurvivor", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "ShoulderR",
                    localPos = new Vector3(-0.48522F, 0.15462F, -0.26142F),
                    localAngles = new Vector3(293.772F, 200.9876F, 39.2971F),
                    localScale = new Vector3(40F, 40F, 40F)
                }
            });
            rules.Add("mdlSeeker", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Pack",
                    localPos = new Vector3(0.20191F, -0.7463F, -0.1612F),
                    localAngles = new Vector3(332.633F, 92.34814F, 354.5216F),
                    localScale = new Vector3(40F, 40F, 40F)
                }
            });
            rules.Add("mdlFalseSon", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "ClavL",
                    localPos = new Vector3(-0.18452F, 1.06937F, 0.56602F),
                    localAngles = new Vector3(63.28513F, 319.1324F, 162.1586F),
                    localScale = new Vector3(40F, 40F, 40F)
                }
            });
            rules.Add("mdlChef", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Chest",
                    localPos = new Vector3(0.46152F, -0.46659F, -0.54141F),
                    localAngles = new Vector3(28.38098F, 357.0819F, 39.38975F),
                    localScale = new Vector3(40F, 40F, 40F)
                }
            });
            // Ralsei model impossible for this, very bad collision
            return rules;
        }

        private void CreateBuff()
        {
            JackBuff = ScriptableObject.CreateInstance<BuffDef>();
            JackBuff.name = "JackBuff";
            JackBuff.buffColor = Color.green;
            JackBuff.canStack = true;
            JackBuff.isDebuff = false;
            JackBuff.isHidden = true;

            ContentAddition.AddBuffDef(JackBuff);
        }

        public override void Hooks()
        {
            RecalculateStatsAPI.GetStatCoefficients += RecalculateStatsAPI_GetStatCoefficients;
            On.RoR2.CharacterBody.OnInventoryChanged += CharacterBody_OnInventoryChanged;
        }

        private void CharacterBody_OnInventoryChanged(On.RoR2.CharacterBody.orig_OnInventoryChanged orig, CharacterBody self)
        {
            orig(self);

            #region Add Timer
            var timer = self.GetComponent<JackNOffTimer>();
            if (GetCount(self) > 0 && !timer)
            {
                timer = self.gameObject.AddComponent<JackNOffTimer>();
                timer.player = self;
                timer.enabled = true;
            }
            else if (GetCount(self) <= 0 && timer)
            {
                timer.enabled = false;
            }
            #endregion
        }

        private void RecalculateStatsAPI_GetStatCoefficients(CharacterBody sender, RecalculateStatsAPI.StatHookEventArgs args)
        {
            #region Add Speed
            if (GetCount(sender) > 0 && sender.HasBuff(JackBuff))
            {
                var buffCount = sender.GetBuffCount(JackBuff);
                var modifiedItemCount = GetCount(sender) - 1;
                var totalSpeedMult = buffCount * (baseMulti + (modifiedItemCount * multi));
                args.moveSpeedMultAdd += totalSpeedMult;
            }
            #endregion
        }

        public override void Init()
        {
            CreateItem();
            CreateLang();
            CreateBuff();
            Hooks();

            GameObject pickupModel = MainAssets.LoadAsset<GameObject>("jackkeynoff.prefab").InstantiateClone("JackKeyPickup", false);

            ItemDef.pickupModelPrefab = pickupModel;
        }

        private class JackNOffTimer : MonoBehaviour
        {
            readonly float timerInterval = 30f;
            float timer = 0f;
            
            public CharacterBody player;

            private void Awake()
            {
                base.enabled = false;
            }
            private void OnEnable()
            {
                if (!player)
                {
                    Debug.Log("Player not found! Destroying...");
                    Destroy(this);
                }
                
                timer = timerInterval;
            }
            // Jack Key N. Off Timer
            private void FixedUpdate()
            {
                timer -= Time.fixedDeltaTime;
                if (timer <= 0)
                {
                    YourTakingTooLong();
                    timer = timerInterval;
                }
            }
            // Add buff to increase speed
            private void YourTakingTooLong()
            {
                Debug.Log("Adding speed buff!");
                player.AddBuff(JackBuff);
            }
        }
    }
}
