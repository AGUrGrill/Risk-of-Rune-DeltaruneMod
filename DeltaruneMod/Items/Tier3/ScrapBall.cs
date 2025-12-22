using R2API;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace DeltaruneMod.Items.Tier3
{
    public class ScrapBall : ItemBase<ScrapBall>
    {
        public override string ItemName => "Ball of Junk";

        public override string ItemLangTokenName => "SCRAP_BALL";

        public override string ItemPickupDesc => "Gain temporary scrap every 3 minutes.";

        public override string ItemFullDescription => "";

        public override string ItemLore => "";

        public override ItemTier Tier => ItemTier.Tier3;

        public override GameObject ItemModel => DeltarunePlugin.MainAssets.LoadAsset<GameObject>("fake.prefab");

        public override Sprite ItemIcon => DeltarunePlugin.MainAssets.LoadAsset<Sprite>("fake.png");

        public override bool isChapter1 => false;

        public override bool isChapter2 => false;

        public override bool isChapter3 => false;

        public override bool isChapter4 => false;

        public override ItemTag[] ItemTags => new ItemTag[] { ItemTag.Utility, ItemTag.CanBeTemporary };

        public override void Init()
        {
            CreateItem();
            CreateLang();
            CreateItemDisplayRules();
            Hooks();
        }

        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            return null;
        }

        public override void Hooks()
        {
            On.RoR2.CharacterBody.OnInventoryChanged += CharacterBody_OnInventoryChanged;
        }

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

        private class ScrapTimer : MonoBehaviour
        {
            readonly float timerInterval = 180f; // In seconds
            float timer = 0f;

            public CharacterBody player;
            public int stack;

            enum ScrapWeight // Out of 100
            {
                white = 50,
                green = 80,
                red = 90,
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
                if (givenValue <= (int)ScrapWeight.white) player.inventory.GiveItemTemp((ItemIndex)216); // White scrap index
                else if (givenValue <= (int)ScrapWeight.green) player.inventory.GiveItemTemp((ItemIndex)212); // Green scrap index
                else if (givenValue <= (int)ScrapWeight.red) player.inventory.GiveItemTemp((ItemIndex)214); // Red scrap index
                else if (givenValue <= (int)ScrapWeight.yellow) player.inventory.GiveItemTemp((ItemIndex)218); // Yellow scrap index
                else if (givenValue <= (int)ScrapWeight.regen) player.inventory.GiveItemTemp((ItemIndex)208); // Regen scrap index
                Debug.Log("Giving scrap (" + givenValue + ") to " + player.name + "...");
                
            }
        }
    }
}
