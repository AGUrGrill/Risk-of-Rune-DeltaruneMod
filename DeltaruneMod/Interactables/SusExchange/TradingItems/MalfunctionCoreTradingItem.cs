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
        public override string ItemName => "6 Kromer -> Malfunctioning Core";

        public override string ItemLangTokenName => "CORE_TRADE_ITEM";

        public override string ItemPickupDesc => "On kill, enemies drop an orb, giving 2 temporary shield.";

        public override string ItemFullDescription => "On kill, enemies will drop an orb that gives <style=cIsUtility>2</style> temporary shield <style=cStack>(+1 per stack)</style>.";

        public override string ItemLore => "";

        public override ItemTier Tier => ItemTier.AssignedAtRuntime;

        public override GameObject ItemModel => MainAssets.LoadAsset<GameObject>("fake.prefab");

        public override Sprite ItemIcon => MainAssets.LoadAsset<Sprite>("yoru_orb_icon.png");

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
