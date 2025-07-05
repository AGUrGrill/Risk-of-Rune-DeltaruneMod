using R2API;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using static DeltaruneMod.DeltarunePlugin;


namespace DeltaruneMod.Items.Tier2
{
    public class ShopSight : ItemBase<ShopSight>
    {
        public override string ItemName => "Shopkeeper's Sight";

        public override string ItemLangTokenName => "SHOP_SIGHT";

        public override string ItemPickupDesc => "Gain an addition use at Shrines of Chance.";

        public override string ItemFullDescription => "Gain <style=cIsUtility>+1</style> uses at Shrines of Chance <style=cStack>(+1 per stack)</style>.";

        public override string ItemLore => "Hee hee... Welcome, travellers." +
            "\nWhat do you like to buy? " +
            "\nMy eye? Ha ha ha ha, I can barter with you.";

        public override ItemTier Tier => ItemTier.Tier2;

        public override GameObject ItemModel => MainAssets.LoadAsset<GameObject>("thorn_ring.prefab");

        public override Sprite ItemIcon => MainAssets.LoadAsset<Sprite>("shop_sight_icon.png");

        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            return null;
        }

        public override void Hooks()
        {
            On.RoR2.ShrineChanceBehavior.AddShrineStack += ShopSightEffect;
        }

        private void ShopSightEffect(On.RoR2.ShrineChanceBehavior.orig_AddShrineStack orig, RoR2.ShrineChanceBehavior self, RoR2.Interactor activator)
        {
            var body = activator.GetComponent<CharacterBody>();
            var itemCount = GetCount(body);
            var existing = self.gameObject.GetComponent<AddShrineCount>();
            if (!existing && body.inventory && itemCount > 0)
            {
                existing = self.gameObject.AddComponent<AddShrineCount>();
                
            } 
            if (existing && body.inventory && itemCount > 0)
            {
                existing.shrine = self;
                existing.stack = itemCount;
            }
            
            orig(self, activator);
        }

        public override void Init()
        {
            //CreateItem();
            //CreateLang();
            //Hooks();
        }

        public class AddShrineCount : CharacterBody.ItemBehavior
        {
            public ShrineChanceBehavior shrine;
            private void Start()
            {
                shrine.maxPurchaseCount += stack;
            }
        }
    }
}
