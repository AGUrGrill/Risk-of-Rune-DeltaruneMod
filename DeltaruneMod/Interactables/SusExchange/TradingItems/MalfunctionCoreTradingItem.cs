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
    public class MalfunctionCoreTradingItem : ItemBase<MalfunctionCoreTradingItem>
    {
        public override string ItemName => "Malfunctioning Core";

        public override string ItemLangTokenName => "CORE_TRADE_ITEM";

        public override string ItemPickupDesc => "";

        public override string ItemFullDescription => "";

        public override string ItemLore => "";

        public override ItemTier Tier => ItemTier.AssignedAtRuntime;

        public override GameObject ItemModel => MainAssets.LoadAsset<GameObject>("fake.prefab");

        public override Sprite ItemIcon => MainAssets.LoadAsset<Sprite>("fake.png");

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
            var hostItem = MalfunctionCore.instance.ItemDef;

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
