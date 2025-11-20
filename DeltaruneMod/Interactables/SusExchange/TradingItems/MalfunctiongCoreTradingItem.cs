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
    public class MalfunctiongCoreTradingItem : ItemBase<MalfunctiongCoreTradingItem>
    {
        public override string ItemName => "6 Kromer -> Malfunctioning Core";

        public override string ItemLangTokenName => "CORE_TRADE_ITEM";

        public override string ItemPickupDesc => "Crit chance randomly increases by 25% for 3 seconds.";

        public override string ItemFullDescription => "Every <style=cIsUtility>5 to 20</style> seconds, gain a <style=cIsUtility>25%</style> crit chance increase for <style=cIsUtility>3</style> seconds <style=cStack>(+1 second per stack)</style>.";

        public override string ItemLore => "";

        public override ItemTier Tier => ItemTier.NoTier;

        public override GameObject ItemModel => MainAssets.LoadAsset<GameObject>("yoru_orb_plus.prefab");

        public override Sprite ItemIcon => MainAssets.LoadAsset<Sprite>("trading_malfunction_core_icon");

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
