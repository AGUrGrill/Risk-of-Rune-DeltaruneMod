using R2API;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using static DeltaruneMod.DeltarunePlugin;

namespace DeltaruneMod.Items.Tier1.Gacha
{
    public class GachaBall : ItemBase<GachaBall>
    {
        public override string ItemName => "Grand Prize";

        public override string ItemLangTokenName => "GACHA_BALL";

        public override string ItemPickupDesc => "Gain a temporary gacha pull on stage start.";

        public override string ItemFullDescription => "Gain a random item from the gacha item pool on stage start <style=sStack>(+1 item per stack)</style>.";

        public override string ItemLore => "";

        public override ItemTier Tier => ItemTier.Tier1;

        public override GameObject ItemModel => MainAssets.LoadAsset<GameObject>("gacha_ball.prefab");

        public override Sprite ItemIcon => MainAssets.LoadAsset<Sprite>("gacha_ball_icon.png");

        public override bool isChapter1 => false;

        public override bool isChapter2 => false;

        public override bool isChapter3 => true;

        public override bool isChapter4 => false;

        public override ItemTag[] ItemTags => new ItemTag[] { ItemTag.Utility };

        public static List<ItemDef> gachaItems = new List<ItemDef>();

        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            return null;
        }

        public override void Hooks()
        {
            //On.RoR2.CharacterBody.OnInventoryChanged += CharacterBody_OnInventoryChanged;
            On.RoR2.CharacterMaster.OnBodyStart += CharacterMaster_OnBodyStart;
        }
        /*
        private void CharacterBody_OnInventoryChanged(On.RoR2.CharacterBody.orig_OnInventoryChanged orig, CharacterBody self)
        {
            orig(self);

            #region Add Timer
            var timer = self.GetComponent<GachaBallTimer>();
            if (GetCount(self) > 0 && !timer)
            {
                timer = self.gameObject.AddComponent<GachaBallTimer>();
                timer.player = self;
                timer.stackCount = GetCount(self);
                timer.enabled = true;
            }
            else if (GetCount(self) <= 0 && timer)
            {
                timer.enabled = false;
            }
            else if (timer && timer.stackCount != GetCount(self))
            {
                timer.stackCount = GetCount(self);
            }
            #endregion
        }
        */


        private void CharacterMaster_OnBodyStart(On.RoR2.CharacterMaster.orig_OnBodyStart orig, CharacterMaster self, CharacterBody body)
        {
            orig(self, body);

            if (gachaItems.Count <= 0) GetGachaItems();

            // Global hook to always delete all gacha items on stage start
            try
            {
                foreach (var item in gachaItems)
                {
                    var itemCount = self.inventory.GetItemCountEffective(item);
                    for (int i = 0; i < itemCount; i++)
                    {
                        self.inventory.RemoveItemPermanent(item);
                        Debug.Log("Removed " + item.name + " from " + self.name);
                    }
                }
            }
            catch
            {
                Debug.Log("Error removing gacha items.");
            }

            // On stage start give random items
            if (GetCount(self) > 0)
            {
                for (int i = 0; i < GetCount(self); i++)
                {
                    var ranGachaItem = gachaItems[UnityEngine.Random.Range(0, gachaItems.Count)];
                    self.inventory.GiveItemPermanent(ranGachaItem);
                    Debug.Log("Gave " + self.name + " " + ranGachaItem.name);
                }
            }
        }

        private void GetGachaItems()
        {
            gachaItems.Add(TVDinner.instance.ItemDef);
            gachaItems.Add(ExecBuffet.instance.ItemDef);
            gachaItems.Add(GoldenIdol.instance.ItemDef);
            gachaItems.Add(BlueRibbon.instance.ItemDef);
            gachaItems.Add(GingerGuard.instance.ItemDef);
        }

        public override void Init()
        {
            CreateItem();
            CreateLang();
            Hooks();
        }
        private class GachaBallTimer : MonoBehaviour
        {
            // Temp items last 80sec, 90sec with substandard dup

            readonly float timerInterval = 90f;
            float timer = 0f;

            public int stackCount = 0;
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
            // Timer
            private void FixedUpdate()
            {
                timer -= Time.fixedDeltaTime;
                if (timer <= 0)
                {
                    GAMBLE();
                    timer = timerInterval;
                }
            }
            // Add buff to increase speed
            private void GAMBLE()
            {
                for (int i = 0; i < stackCount; i++)
                {
                    var ranGachaItem = gachaItems[UnityEngine.Random.Range(0, gachaItems.Count)];
                    player.inventory.GiveItemTemp(ranGachaItem.itemIndex);
                    Debug.Log("Gave " + player.name + " " + ranGachaItem.name);
                }
            }
        }

    }
}
