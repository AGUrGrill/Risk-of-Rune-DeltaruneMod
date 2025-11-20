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
    public class MrPipisTradingItem : ItemBase<MrPipisTradingItem>
    {
        public override string ItemName => "3 Irradiant Pearls -> Mr. Pipis";

        public override string ItemLangTokenName => "MRPIPIS_TRADE_ITEM";

        public override string ItemPickupDesc => "Gain [ALL] elite buffs!";

        public override string ItemFullDescription => "Provides <style=cIsUtility>all elite buffs</style> <style=cStack>(excluding Aurelionite's blessing)</style>.";

        public override string ItemLore => "WOWZAS! You can't get THIS from an egg!";

        public override ItemTier Tier => ItemTier.NoTier;

        public override GameObject ItemModel => MainAssets.LoadAsset<GameObject>("fake");

        public override Sprite ItemIcon => MainAssets.LoadAsset<Sprite>("trading_mrpipis_icon");

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
