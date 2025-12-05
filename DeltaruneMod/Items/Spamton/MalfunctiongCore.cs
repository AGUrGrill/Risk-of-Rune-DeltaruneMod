using DeltaruneMod.Items;
using R2API;
using RoR2;
using RoR2.Orbs;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;
using static DeltaruneMod.DeltarunePlugin;
using static DeltaruneMod.Items.Lunar.DevilsKnife;

namespace DeltaruneMod.Items.Spamton
{
    public class MalfunctiongCore : ItemBase<MalfunctiongCore>
    {
        public override string ItemName => "Malfunctioning Core";

        public override string ItemLangTokenName => "MALFUNCTION_CORE";

        public override string ItemPickupDesc => "Crit chance randomly increases by 25% for 3 seconds.";

        public override string ItemFullDescription => "Every <style=cIsUtility>5 to 20</style> seconds, gain a <style=cIsUtility>" + critPercent * 100 + "%</style> crit chance increase for <style=cIsUtility>3</style> seconds <style=cStack>(+1 second per stack)</style>.";

        public override string ItemLore => "\"Is it... pulsing?\" \"What even is this?\"" +
            "\n\"I got it from a salesman, he said he'd take some junk off my hand for something much more valuable...\"" +
            "\n\"I have a bad feeling about this, something just feels... off. I think you should get rid of it.\"" +
            "\n\"You're just jealous! The man said I could become " +
            "a... a.. .  .   .     <style=cDeath><style=cMono>[BIG SHOT]</style></style>.\"" +
            "\n\nThe room is filled with laughter, anger, jealousy, then.., finally.., silence.";

        public override ItemTier Tier => ItemTier.Tier2;

        public override GameObject ItemModel => MainAssets.LoadAsset<GameObject>("yoru_orb_plus.prefab");

        public override Sprite ItemIcon => MainAssets.LoadAsset<Sprite>("malfunction_core_icon");

        public override bool isChapter1 => false;

        public override bool isChapter2 => true;

        public override bool isChapter3 => false;

        public override bool isChapter4 => false;

        public override ItemTag[] ItemTags => new ItemTag[] { ItemTag.Utility };

        public static float critPercent = 0.25f;

        public static bool critReady = false;

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
                    localPos = new Vector3(0.01379F, 0.0643F, -0.0393F),
                    localAngles = new Vector3(7.65642F, 337.7309F, 355.4065F),
                    localScale = new Vector3(21.78017F, 20.15846F, 23.01508F)
                }
            });
            rules.Add("mdlHuntress", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Head",
                    localPos = new Vector3(-0.0827F, 0.03344F, -0.01415F),
                    localAngles = new Vector3(22.50764F, 82.01157F, 357.0022F),
                    localScale = new Vector3(19.47546F, 17.23778F, 17.62996F)

                }
            });
            rules.Add("mdlToolbot", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Head",
                    localPos = new Vector3(0.40881F, 2.837F, 1.8306F),
                    localAngles = new Vector3(353.6656F, 279.3937F, 111.3867F),
                    localScale = new Vector3(162.097F, 125.9607F, 133.589F)
                }
            });
            rules.Add("mdlEngi", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "HeadCenter",
                    localPos = new Vector3(-0.15761F, -0.07392F, -0.0629F),
                    localAngles = new Vector3(356.2096F, 344.9958F, 289.955F),
                    localScale = new Vector3(20.48202F, 20.67847F, 24.16052F)
                }
            });
            rules.Add("mdlMage", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Head",
                    localPos = new Vector3(0.04168F, 0.18974F, -0.20211F),
                    localAngles = new Vector3(2.45667F, 262.3736F, 243.0552F),
                    localScale = new Vector3(16.12042F, 21.62177F, 16.12042F)
                }
            });
            rules.Add("mdlMerc", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Chest",
                    localPos = new Vector3(0.02591F, 0.09248F, -0.24747F),
                    localAngles = new Vector3(340.2831F, 185.451F, 340.5156F),
                    localScale = new Vector3(8.82175F, 8.82175F, 8.82175F)

                }
            });
            rules.Add("mdlTreebot", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Chest",
                    localPos = new Vector3(-0.05924F, -0.03185F, 0.03543F),
                    localAngles = new Vector3(354.8709F, 163.4293F, 6.06552F),
                    localScale = new Vector3(44.22409F, 44.22409F, 44.22409F)
                }
            });
            rules.Add("mdlLoader", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Head",
                    localPos = new Vector3(0.01479F, 0.24686F, -0.04453F),
                    localAngles = new Vector3(346.3226F, 12.50503F, 165.8035F),
                    localScale = new Vector3(15.66406F, 20.50996F, 17.59725F)
                }
            });
            rules.Add("mdlCroco", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Head",
                    localPos = new Vector3(-0.94664F, 1.31489F, 0.42623F),
                    localAngles = new Vector3(354.3773F, 254.687F, 358.6025F),
                    localScale = new Vector3(38.86275F, 41.16689F, 62.41214F)

                }
            });
            rules.Add("mdlCaptain", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Head",
                    localPos = new Vector3(0.05099F, 0.14721F, 0.01876F),
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
                    childName = "Head",
                    localPos = new Vector3(-0.16941F, 0.11642F, 0.01702F),
                    localAngles = new Vector3(300.6795F, 48.27805F, 224.9956F),
                    localScale = new Vector3(18.52118F, 19.57209F, 18.73416F)
                }
            });
            rules.Add("mdlVoidSurvivor", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Head",
                    localPos = new Vector3(0.06421F, -0.16104F, 0.02439F),
                    localAngles = new Vector3(4.53002F, 272.7654F, 4.84901F),
                    localScale = new Vector3(25.34135F, 24.49854F, 21.83678F)
                }
            });
            rules.Add("mdlChef", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Head",
                    localPos = new Vector3(0.05366F, 0.0278F, -0.10864F),
                    localAngles = new Vector3(320.1729F, 10.61689F, 101.4086F),
                    localScale = new Vector3(24.11722F, 25.65147F, 25.65147F)
                }
            });
            rules.Add("mdlSeeker", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Head",
                    localPos = new Vector3(0.03639F, -0.06952F, -0.1154F),
                    localAngles = new Vector3(29.66408F, 351.0713F, 1.08804F),
                    localScale = new Vector3(21.67765F, 22.25531F, 18.31414F)

                }
            });
            rules.Add("mdlFalseSon", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemModel,
                    childName = "Head",
                    localPos = new Vector3(-0.01617F, -0.05655F, 0.03941F),
                    localAngles = new Vector3(354.7198F, 153.0287F, 359.5688F),
                    localScale = new Vector3(23.88494F, 31.14603F, 25.87784F)
                }
            });
            return rules;
        }

        public override void Hooks()
        {
            RecalculateStatsAPI.GetStatCoefficients += RecalculateStatsAPI_GetStatCoefficients;

        }
        private void RecalculateStatsAPI_GetStatCoefficients(CharacterBody sender, RecalculateStatsAPI.StatHookEventArgs args)
        {
            #region Effect Controller
            var controller = sender.GetComponent<MalfunctioningCoreEffect>();
            if (GetCount(sender) > 0)
            {
                if (!controller)
                {
                    controller = sender.gameObject.AddComponent<MalfunctioningCoreEffect>();
                    controller.itemStacks = GetCount(sender);
                    controller.body = sender;
                    controller.enabled = true;
                }
                else if (controller) controller.itemStacks = GetCount(sender);
                else if (!controller.enabled) controller.enabled = true;
            }
            else if (controller && GetCount(sender) <= 0) controller.enabled = false;
            #endregion

            if (GetCount(sender) > 0 && critReady)
            {
                args.critAdd += critPercent * 100;
            }

        }

        public override void Init()
        {
            CreateItem();
            CreateLang();
            Hooks();
        }

        

        public class MalfunctioningCoreEffect : MonoBehaviour
        {
            private float timer = 0f;
            private float minTime = 5f;
            private float maxTime = 20f;
            private bool appliedCrit = false;
            public CharacterBody body;
            public int itemStacks = 0;
            

            private void Awake()
            {
                base.enabled = false;
            }
            private void OnEnable()
            {

            }
            private void OnDisable()
            {

            }
            private void FixedUpdate()
            {
                timer -= Time.fixedDeltaTime;
                if (timer <= 0f)
                {
                    CritEffect();
                    if (timer <= (-3 + ((itemStacks-1) * -1))) // Start at 3 seconds, at 1 second per stack
                    {
                        timer = UnityEngine.Random.Range(minTime, maxTime);
                        appliedCrit = false;
                        critReady = false;
                    }
                }
            }
            public void CritEffect()
            {
                //Debug.Log("Player Crit: " + body.crit);
                if (!appliedCrit)
                {
                    appliedCrit = true;
                    critReady = true;
                }
            }
            
        }
    }
}