using R2API;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using static DeltaruneMod.DeltarunePlugin;

namespace DeltaruneMod.Items.Tier2
{
    public class TensionBow : ItemBase<TensionBow>
    {
        public override string ItemName => "Tension Bow";

        public override string ItemLangTokenName => "TEN_BOW";

        public override string ItemPickupDesc => "Gain 1% TP every 5 seconds. Gain +0.5 attack speed and armor per TP%.";

        public override string ItemFullDescription => "";

        public override string ItemLore => "";

        public override ItemTier Tier => ItemTier.Tier2;

        public override GameObject ItemModel => MainAssets.LoadAsset<GameObject>("tension_bow.prefab");

        public override Sprite ItemIcon => MainAssets.LoadAsset<Sprite>("tension_bow_icon.png");

        public override bool isChapter1 => false;

        public override bool isChapter2 => true;

        public override bool isChapter3 => false;

        public override bool isChapter4 => false;

        public static BuffDef TPBuff;

        // Numbers for stuff
        private readonly float multi = 0.5f;

        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            return null;
        }

        private void CreateBuff()
        {
            TPBuff = ScriptableObject.CreateInstance<BuffDef>();
            TPBuff.name = "TPBuff";
            TPBuff.buffColor = Color.yellow;
            TPBuff.canStack = true;
            TPBuff.isDebuff = false;
            TPBuff.isHidden = false;

            ContentAddition.AddBuffDef(TPBuff);
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
            var timer = self.GetComponent<TPTimer>();
            if (GetCount(self) > 0 && !timer)
            {
                timer = self.gameObject.AddComponent<TPTimer>();
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
            #region Add Armor and Atk Spd
            if (GetCount(sender) > 0 && sender.HasBuff(TPBuff))
            {
                var buffCount = sender.GetBuffCount(TPBuff);
                var trueBuffCount = Math.Floor(buffCount / 5f);
                var itemCount = GetCount(sender);
                var totalMult = (buffCount * itemCount) * multi;
                args.armorAdd += totalMult;
                args.attackSpeedMultAdd += totalMult;
            }
            #endregion
        }

        public override void Init()
        {
            //CreateItem();
            //CreateLang();
            //CreateBuff();
            //Hooks();
        }

        private class TPTimer : MonoBehaviour
        {
            readonly float timerInterval = 5f;
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
                timer -= Time.deltaTime;
                if (timer <= 0)
                {
                    TPUp();
                    timer = timerInterval;
                }
            }
            // Add buff to increase speed
            private void TPUp()
            {
                Debug.Log("Adding TP!");
                player.AddBuff(TPBuff);
            }
        }
    }
}
