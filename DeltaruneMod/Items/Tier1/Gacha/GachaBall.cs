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

        public override string ItemPickupDesc => "On stage start, gain a temporary random item.";

        public override string ItemFullDescription => "";

        public override string ItemLore => "";

        public override ItemTier Tier => ItemTier.Tier1;

        public override GameObject ItemModel => MainAssets.LoadAsset<GameObject>("gacha_ball.prefab");

        public override Sprite ItemIcon => MainAssets.LoadAsset<Sprite>("gacha_ball_icon.png");

        public override bool isChapter1 => false;

        public override bool isChapter2 => false;

        public override bool isChapter3 => true;

        public override bool isChapter4 => false;

        public List<ItemDef> gachaItems = new List<ItemDef>();

        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            return null;
        }

        public override void Hooks()
        {
            On.RoR2.CharacterBody.Start += CharacterBody_Start;
        }

        private void CharacterBody_Start(On.RoR2.CharacterBody.orig_Start orig, CharacterBody self)
        {
            orig(self);

            if (gachaItems.Count <= 0) GetGachaItems();

            // Global hook to always delete all gacha items on stage start
            try
            {
                foreach (var item in gachaItems)
                {
                    var itemCount = self.inventory.GetItemCount(item);
                    for (int i = 0; i < itemCount; i++)
                    {
                        self.inventory.RemoveItem(item);
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
                    self.inventory.GiveItem(ranGachaItem);
                    Debug.Log("Gave " + self.name + " " + ranGachaItem.name);
                }
            }
        }

        private void GetGachaItems()
        {
            gachaItems.Add(TVDinner.instance.ItemDef);
            gachaItems.Add(ExecBuffet.instance.ItemDef);
            gachaItems.Add(GoldenIdol.instance.ItemDef);
        }

        public override void Init()
        {
            //CreateItem();
            //CreateLang();
            //Hooks();
        }
    }
}
