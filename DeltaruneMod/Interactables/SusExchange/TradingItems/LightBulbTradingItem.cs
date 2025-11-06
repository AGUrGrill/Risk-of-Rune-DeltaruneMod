using DeltaruneMod.Items;
using DeltaruneMod.Items.Spamton;
using R2API;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using static DeltaruneMod.DeltarunePlugin;

namespace DeltaruneMod.Interactables.SusExchange.TradingItems
{
    public class LightBulbTradingItem : ItemBase<LightBulbTradingItem>
    {
        public override string ItemName => "3 Kromer -> Fractured Light";

        public override string ItemLangTokenName => "BULB_TRADE_ITEM";

        public override string ItemPickupDesc => "Increase all lightning damage by 25%.";

        public override string ItemFullDescription => "All forms of lightning damage are increased by <style=cIsUtility>25%</style> <style=cStack>(+25% per stack)</style>.";

        public override string ItemLore => "";

        public override ItemTier Tier => ItemTier.Tier1;

        public override GameObject ItemModel => MainAssets.LoadAsset<GameObject>("light_bulb.prefab");

        public override Sprite ItemIcon => MainAssets.LoadAsset<Sprite>("bulb_icon");

        public override bool isChapter1 => false;

        public override bool isChapter2 => true;

        public override bool isChapter3 => false;

        public override bool isChapter4 => false;

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
            var hostItem = LightBulb.instance.ItemDef;

            var itemCount = GetCount(sender);
            if (itemCount > 0)
            {
                for (int i = 0; i < itemCount; i++)
                {
                    sender.inventory.RemoveItem(ItemDef);
                    sender.inventory.GiveItem(hostItem);
                }
            }
        }

        public override void Init()
        {
            CreateItem();
            CreateLang();
            Hooks();
        }
    }
}
