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

        public override string ItemLore => "";

        public override ItemTier Tier => ItemTier.Tier2;

        public override GameObject ItemModel => MainAssets.LoadAsset<GameObject>("yoru_orb_plus.prefab");

        public override Sprite ItemIcon => MainAssets.LoadAsset<Sprite>("malfunction_core_icon");

        public override bool isChapter1 => false;

        public override bool isChapter2 => true;

        public override bool isChapter3 => false;

        public override bool isChapter4 => false;

        public static float critPercent = 0.25f;

        public static bool critReady = false;

        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            return null;
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
                Debug.Log("Player Crit: " + body.crit);
                if (!appliedCrit)
                {
                    appliedCrit = true;
                    critReady = true;
                }
            }
            
        }
    }
}