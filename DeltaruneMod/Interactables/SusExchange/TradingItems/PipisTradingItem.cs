using DeltaruneMod.Items;
using R2API;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using static DeltaruneMod.DeltarunePlugin;

namespace DeltaruneMod.Interactables.SusExchange.TradingItems
{
    public class PipisTradingItem : ItemBase<PipisTradingItem>
    {
        public override string ItemName => "2 Pearls -> Pipis";

        public override string ItemLangTokenName => "PIPIS_TRADE_ITEM";

        public override string ItemPickupDesc => "Increases ALL of your stats.";

        public override string ItemFullDescription => "Increases <style=cIsUtility>ALL stats</style> by <style=cIsUtility>5%</style>.\nPearl be damned my boy ballin'.";

        public override string ItemLore => "You can't get this from an egg!";

        public override ItemTier Tier => ItemTier.AssignedAtRuntime;

        public override GameObject ItemModel => MainAssets.LoadAsset<GameObject>("fake");

        public override Sprite ItemIcon => MainAssets.LoadAsset<Sprite>("trading_pipis_icon");

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

        }

        public override void Init()
        {
            CreateItem();
            CreateLang();
            Hooks();
        }
    }
}
