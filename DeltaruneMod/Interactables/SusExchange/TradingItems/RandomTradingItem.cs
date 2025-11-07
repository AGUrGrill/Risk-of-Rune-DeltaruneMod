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
    // This quite litterally only exists to be an icon, nothing needed...
    public class RandomTradingItem : ItemBase<RandomTradingItem>
    {
        public override string ItemName => "TRADE [[Trash]] FOR INSTANT CASH!!! [[No]] REFUNDS!!!";

        public override string ItemLangTokenName => "RANDOM_TRADE_ITEM";

        public override string ItemPickupDesc => "1 Random White or Green Item -> Chance for Upgraded Item...";

        public override string ItemFullDescription => "";

        public override string ItemLore => "";

        public override ItemTier Tier => ItemTier.AssignedAtRuntime;

        public override GameObject ItemModel => MainAssets.LoadAsset<GameObject>("fake.prefab");

        public override Sprite ItemIcon => MainAssets.LoadAsset<Sprite>("random_icon");

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
