using R2API;
using RoR2;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using static DeltaruneMod.DeltarunePlugin;

namespace DeltaruneMod.Items.Tier1
{
    public class JackKeyNf : ItemBase<JackKeyNf>
    {
        public override string ItemName => "You're taking too long";

        public override string ItemLangTokenName => "JACK_KEY";

        public override string ItemPickupDesc => "Every 30 seconds on stage, move 5% faster.";

        public override string ItemFullDescription => "";

        public override string ItemLore => "";

        public override ItemTier Tier => ItemTier.Tier1;

        public override GameObject ItemModel => MainAssets.LoadAsset<GameObject>("jackkeynoff.prefab");

        public override Sprite ItemIcon => MainAssets.LoadAsset<Sprite>("jackkeynoff_image.prefab");

        public override bool isChapter1 => false;

        public override bool isChapter2 => false;

        public override bool isChapter3 => false;

        public override bool isChapter4 => true;

        public static BuffDef JackBuff;

        // Numbers for stuff
        private readonly float multi = 0.01f;

        private readonly float baseMulti = 0.05f;

        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            return null;
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

            #region Determine Stack Count
            if (timer && GetCount(self) > 0)
            {
                timer.itemStacks = GetCount(self);
            }
            #endregion
        }

        private void RecalculateStatsAPI_GetStatCoefficients(CharacterBody sender, RecalculateStatsAPI.StatHookEventArgs args)
        {
            if (!NetworkServer.active || !sender.inventory) return;

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
        }

        private class JackNOffTimer : MonoBehaviour
        {
            readonly float timerInterval = 30f;
            float timer = 0f;
            
            public CharacterBody player;
            public int itemStacks = 0;

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
                timer -= Time.deltaTime;
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
