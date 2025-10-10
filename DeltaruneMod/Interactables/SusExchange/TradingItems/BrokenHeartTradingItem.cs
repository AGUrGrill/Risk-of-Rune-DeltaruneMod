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
    public class BrokenHeartTradingItem : ItemBase<BrokenHeartTradingItem>
    {
        public override string ItemName => "10 Kromer -> Misshapen Heart";

        public override string ItemLangTokenName => "HEART_TRADE_ITEM";

        public override string ItemPickupDesc => "Spawn an orbiting, armor piercing projectile every 2 seconds.";

        public override string ItemFullDescription => "Every <style=cIsUtility>2</style> seconds, spawn an armor piercing projectile that orbits the player in stasis.\" +\r\n            \"\\nSpawn up to 2 maximum projectiles, deals <style=cIsDamage>199.7% base damage</style> <style=cStack>(+199.7% per stack)</style>.";

        public override string ItemLore => "";

        public override ItemTier Tier => ItemTier.AssignedAtRuntime;

        public override GameObject ItemModel => MainAssets.LoadAsset<GameObject>("mis_heart.prefab");

        public override Sprite ItemIcon => MainAssets.LoadAsset<Sprite>("mis_heart_icon");

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
            var hostItem = BrokenHeart.instance.ItemDef;

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
